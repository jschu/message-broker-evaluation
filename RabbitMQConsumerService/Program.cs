using System;
using System.Threading;
using EasyNetQ;
using Shared;
using Shared.RabbitMQ;

namespace RabbitMQConsumerService
{
    class Program
    {
        private static readonly AutoResetEvent waitHandle = new AutoResetEvent(false);
        private static readonly string subscriptionId = Guid.NewGuid().ToString();
        private static readonly int maxRetryCount = 100;
        private static readonly int retryDelayInMs = 1000;
        private static readonly string connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
            ? "username=guest;password=guest;host=localhost"
            : "username=guest;password=guest;host=rabbitmq";

        static void Main(string[] args) 
        {
            using (var bus = RabbitHutch.CreateBus(connectionString)) 
            {
                var messageBus = new EasyNetQMessageBus(bus);
                messageBus.TrySubscribe<Message, MessageHandler>(subscriptionId, new MessageHandler(), maxRetryCount, retryDelayInMs);
                Console.WriteLine("Listening for messages...");
                waitHandle.WaitOne();
            }
        }
    }
}
