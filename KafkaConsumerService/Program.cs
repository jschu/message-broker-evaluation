using System;
using Confluent.Kafka;
using Shared;
using Shared.Kafka;

namespace KafkaConsumerService
{
    class Program
    {
        private static readonly string kafkaServer = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
            ? "localhost:19092"
            : "kafka:9092";

        static void Main(string[] args)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = kafkaServer,
                GroupId = Guid.NewGuid().ToString(),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumerBuilder = new ConsumerBuilder<Ignore, Message>(config);
            consumerBuilder.SetValueDeserializer(new MessageDeserializer());
            using (var consumer = consumerBuilder.Build())
            {
                var messageHandler = new MessageHandler();
                consumer.Subscribe(Shared.Kafka.Constants.Topic);
                Console.WriteLine("Listening for messages...");

                while (true)
                {
                    var result = consumer.Consume();
                    messageHandler.Invoke(result.Message.Value);
                }
            }
        }
    }
}
