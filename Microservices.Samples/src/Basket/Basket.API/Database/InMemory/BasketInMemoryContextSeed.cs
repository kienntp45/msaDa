namespace MicroServices.Samples.Services.Basket.API.Database.InMemory;

public class BasketInMemoryContextSeed
{
    public async Task SeedAsync(BasketInMemoryContext inMemoryContext, BasketDbConText dbConText)
    {
        var customerBasket = dbConText.CustomerBaskets.ToList();
        foreach (var data in customerBasket)
            {
                if (data != null)
                {
                    await dbConText.Entry(data).Collection(i => i.Items).LoadAsync();
                    inMemoryContext.customerBaskets.Add(data.CusTomerId,data);
                }
            }

    }
}