using System;
using Shared;

namespace RabbitMQConsumerService
{
    public static class ExtensionMethods
    {
        public static void TrySubscribe<TMessage, TMessageHandler>(this IMessageBus messageBus, TMessageHandler messageHandler, int maxRetryCount, int retryDelayInMs)
           where TMessageHandler : IMessageHandler<TMessage>
        {
            var success = false;
            var currentRetryCount = 0;

            while (!success)
            {
                try
                {
                    messageBus.Subscribe<TMessage, TMessageHandler>(messageHandler);
                    success = true;
                }
                catch (Exception)
                {
                    if (currentRetryCount >= maxRetryCount)
                    {
                        Console.WriteLine($"The maximum number of retries ({maxRetryCount}) was exceeded while trying to subscribe to the message broker.");
                    }
                    ++currentRetryCount;
                    Console.WriteLine("Sleep");
                    //System.Threading.Thread.Sleep(retryDelayInMs);
                }
            }
        }
    }
}