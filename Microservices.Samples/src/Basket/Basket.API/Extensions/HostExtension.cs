using Microsoft.EntityFrameworkCore;

namespace MicroServices.Samples.Services.Basket.API.Extensions;

public static class IHostExtensions
{
    public static IHost LoadDataInMemory<TInMemoryContext,TDbContext>( this IHost host , Action<TInMemoryContext,TDbContext> seeder)
    where TDbContext:DbContext
    {
         using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetService<TInMemoryContext>();
            var dbContext = services.GetRequiredService<TDbContext>();
            seeder(context, dbContext);
        }
        return host;
    }
    public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
    where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();
            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);
                InvokeSeeder(seeder, context, services);
                logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
            }
        }
        return host;
    }
    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
    where TContext : DbContext
    {
        if (context != null)
        {
            try
            {
                context.Database.EnsureCreated();
            }
            finally
            {
                seeder(context, services);
            }
        }
    }
}
