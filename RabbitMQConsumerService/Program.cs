using System;
using EasyNetQ;
using Shared;

namespace RabbitMQConsumerService
{
    class Program
    {
        static void Main(string[] args) 
        {
            var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
                ? "username=guest;password=guest;host=localhost"
                : "username=guest;password=guest;host=rabbitmq";

            using (var bus = RabbitHutch.CreateBus(connectionString)) 
            {
                var messageBus = new EasyNetQMessageBus(bus);
                messageBus.Subscribe<Message, MessageHandler>(new MessageHandler());
                Console.WriteLine("Listening for messages...");
                Console.ReadLine();
            }
        }
    }
}
