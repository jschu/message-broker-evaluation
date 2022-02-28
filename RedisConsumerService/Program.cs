using System;
using System.Threading;
using Shared;
using Shared.Redis;
using StackExchange.Redis;

namespace KafkaConsumerService
{
    class Program
    {
        private static readonly AutoResetEvent waitHandle = new AutoResetEvent(false);
        private static readonly string redisServer = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
            ? "localhost"
            : "redis";

        static void Main(string[] args)
        {
            var redisConnection = ConnectionMultiplexer.Connect(redisServer);
            var subscriber = redisConnection.GetSubscriber();
            var messageHandler = new MessageHandler();

            subscriber.Subscribe(Constants.Channel, (channel, value) =>
            {
                var message = MessageDeserializer.Deserialize(value.ToString());
                messageHandler.Invoke(message);
            });
            Console.WriteLine("Listening for messages...");
            waitHandle.WaitOne();
        }
    }
}
