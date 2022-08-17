using Confluent.Kafka;

namespace MicroServices.Samples.Shared.Common.Utils;


public class KafkaConsumer<TKey, TValue> : IDisposable
{
    private readonly IConsumer<TKey, TValue> _consumer;

    public KafkaConsumer(IConsumer<TKey, TValue> consumer)
    {
        _consumer = consumer;
    }
    public void Dispose()
    {
        _consumer.Dispose();
    }

    public void Consume(Action<ConsumeResult<TKey, TValue>> callback, string topic, int partition = -1, long offset = -1, CancellationToken cancellationToken = default)
    {
        SetSubscribeOrAssign(topic, partition, offset);
        while (!cancellationToken.IsCancellationRequested)
        {
            var result = _consumer.Consume(TimeSpan.FromSeconds(1));
            if (result != null)
            {
                callback(result);
            }
        }
    }

    private void SetSubscribeOrAssign(string topic, int partition = -1, long offset = -1)
    {
        if (partition >= 0)
        {
            if (offset >= 0)
            {
                _consumer.Assign(new TopicPartitionOffset(new TopicPartition(topic, partition), offset));
            }
            else
            {
                _consumer.Assign(new TopicPartition(topic, partition));
            }
        }
        else
        {
            _consumer.Subscribe(topic);
        }
    }
}