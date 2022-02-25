using System;
using Confluent.Kafka;

namespace KafkaConsumerService
{
    public static class ConsumerExtensions
    {
        public static void TrySubscribe<TKey, TValue>(this IConsumer<TKey, TValue> consumer, string topic, int maxRetryCount, int retryDelayInMs)
        {
            var success = false;
            var currentRetryCount = 0;

            while (!success)
            {
                try
                {
                    consumer.Subscribe(topic);
                    success = true;
                }
                catch (Exception)
                {
                    if (currentRetryCount >= maxRetryCount)
                    {
                        Console.WriteLine($"The maximum number of retries ({maxRetryCount}) was exceeded while trying to subscribe to the message broker.");
                    }
                    ++currentRetryCount;
                    Console.WriteLine("s");
                    System.Threading.Thread.Sleep(retryDelayInMs);
                }
            }
        }
    }
}