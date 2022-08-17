using System.Text.Json;
using Disruptor;
using MicroServices.Samples.Services.Product.ProductPersistent.Application.Model;
using MicroServices.Samples.Services.Product.ProductPersistent.Database;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.Samples.Services.Product.ProductPersistent.Application.Events;


public class ProductKafkaMessageHandler : IEventHandler<ProductRingMessage>
{
    private readonly IDbContextFactory<ProductDbContext> _dbContextFactory;
    private readonly List<ProductItem> addItems;
    private readonly List<ProductItem> updateItems;
    private readonly List<ProductItem> deleteItems;
    private int count;
    private readonly int _idHandler;

    public ProductKafkaMessageHandler(IDbContextFactory<ProductDbContext> dbContextFactory, int idHandler)
    {
        addItems = new List<ProductItem>();
        updateItems = new List<ProductItem>();
        deleteItems = new List<ProductItem>();
        _dbContextFactory = dbContextFactory;
        count = 1;
        _idHandler = idHandler;
    }
    public void OnEvent(ProductRingMessage productRingMessage, long sequence, bool endOfBatch)
    {
        if (_idHandler == productRingMessage.IdHandler)
        {
            ProductKafkaMessage productKafkaMessage = JsonSerializer.Deserialize<ProductKafkaMessage>(productRingMessage.Message);

            ProductItem data = addItems.Where(e => e.Id == productKafkaMessage.Data.Id).FirstOrDefault();
            if (productKafkaMessage.ActionType == 0)
            {
                if (data == null)
                {
                    addItems.Add(productKafkaMessage.Data);
                }
            }
            else if (productKafkaMessage.ActionType == 1)
            {
                if (data != null)
                {
                    data.Name = productKafkaMessage.Data.Name;
                    data.Price = productKafkaMessage.Data.Price;
                    data.AvailableQuantity = productKafkaMessage.Data.AvailableQuantity;
                }
                else
                {
                    updateItems.Add(productKafkaMessage.Data);
                }
            }
            else
            {
                if (data != null)
                {
                    addItems.Remove(data);
                }
                else
                {
                    ProductItem updateData = updateItems.Where(e => e.Id == productKafkaMessage.Data.Id).FirstOrDefault();
                    if (updateData != null)
                        updateItems.Remove(updateData);
                    deleteItems.Add(productKafkaMessage.Data);
                }
            }
            count++;

            if (productKafkaMessage.Data.Id == 10000)
            {
                Console.WriteLine("EndTime : " + DateTime.Now.ToString("HH:mm:ss.fff"));
            }
        }
        if (count > 100 || endOfBatch)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            using var transaction = dbContext.Database.BeginTransaction();
            try
            {
                if (addItems.Count() > 0)
                {
                    dbContext.AddRangeAsync(addItems);
                    dbContext.SaveChangesAsync();
                    addItems.Clear();

                }
                if (updateItems.Count() > 0)
                {
                    dbContext.UpdateRange(updateItems);
                    dbContext.SaveChangesAsync();
                    updateItems.Clear();
                }
                if (deleteItems.Count() > 0)
                {
                    dbContext.RemoveRange(deleteItems);
                    dbContext.SaveChangesAsync();
                    deleteItems.Clear();
                }
                transaction.CommitAsync();
                count = 1;
            }
            catch (Exception)
            {
                transaction.RollbackAsync();
            }
        }
    }
}
