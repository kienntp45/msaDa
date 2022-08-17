using Confluent.Kafka;

namespace MicroServices.Samples.Shared.Common.Utils;

public class KafkaProducer<TKey, TValue> : IDisposable
{
    private readonly IProducer<TKey, TValue> _producer;

    public KafkaProducer(IProducer<TKey, TValue> producer)
    {
        _producer = producer;
    }

    public void Dispose()
    {
        _producer.Dispose();
    }

    public void Produce(Message<TKey, TValue> message, string topic)
    {
        try
        {
            _producer.Produce(topic, message);
        }
        catch (ProduceException<TKey, TValue> ex)
        {
            if (ex.Error.Code == ErrorCode.Local_QueueFull)
            {
                _producer.Poll(TimeSpan.FromSeconds(1));
            }
            else
            {
                throw;
            }
        }
    }
}