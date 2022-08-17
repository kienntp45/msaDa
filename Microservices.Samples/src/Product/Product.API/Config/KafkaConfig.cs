namespace MicroServices.Samples.Services.Product.API.Config;

public class KafkaConfig
{
    public string BootstrapServers { get; set; }
    public int QueueBufferingMaxMessages { get; set; }
    public int MessageSendMaxRetries { get; set; }
    public int RetryBackOffMs { get; set; }
    public int LingerMs { get; set; }
    public string DeliveryReportFields { get; set; }
}