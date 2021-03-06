using System;
using Shared;
using Shared.RabbitMQ;

namespace RabbitMQConsumerService
{
    public static class MessageBusExtensions
    {
        public static void TrySubscribe<TMessage, TMessageHandler>(this IMessageBus messageBus, string subscriptionId, TMessageHandler messageHandler, int maxRetryCount, int retryDelayInMs)
           where TMessageHandler : IMessageHandler<TMessage>
        {
            var success = false;
            var currentRetryCount = 0;

            while (!success)
            {
                try
                {
                    messageBus.Subscribe<TMessage, TMessageHandler>(subscriptionId, messageHandler);
                    success = true;
                }
                catch (Exception)
                {
                    if (currentRetryCount >= maxRetryCount)
                    {
                        Console.WriteLine($"The maximum number of retries ({maxRetryCount}) was exceeded while trying to subscribe to the message broker.");
                    }
                    ++currentRetryCount;
                    System.Threading.Thread.Sleep(retryDelayInMs);
                }
            }
        }
    }
}