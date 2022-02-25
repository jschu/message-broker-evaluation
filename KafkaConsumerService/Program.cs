using System;
using Confluent.Kafka;
using Shared;
using Shared.Kafka;

namespace KafkaConsumerService
{
    class Program
    {
        private static readonly int maxRetryCount = 100;
        private static readonly int retryDelayInMs = 1000;
        private static readonly string kafkaServer = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
            ? "localhost:9092"
            : "kafka:9092";

        static void Main(string[] args)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = kafkaServer,
                GroupId = Shared.Kafka.Constants.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumerBuilder = new ConsumerBuilder<Ignore, Message>(config);
            consumerBuilder.SetValueDeserializer(new MessageDeserializer());
            using (var consumer = consumerBuilder.Build())
            {
                var messageHandler = new MessageHandler();
                consumer.TrySubscribe<Ignore, Message>(Shared.Kafka.Constants.Topic, maxRetryCount, retryDelayInMs);
                Console.WriteLine("Listening for messages...");

                while (true)
                {
                    var result = consumer.Consume();
                    if (result != null)
                    {
                        messageHandler.Invoke(result.Message.Value);
                    }
                }
            }
        }
    }
}
