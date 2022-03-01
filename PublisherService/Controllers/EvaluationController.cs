using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Confluent.Kafka;
using Shared;
using Shared.RabbitMQ;
using StackExchange.Redis;

namespace PublisherService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluationController : ControllerBase
    {
        private readonly ILogger<EvaluationController> logger;
        private readonly IConfiguration configuration;
        private readonly IMessageBus messageBus;

        public EvaluationController(ILogger<EvaluationController> logger, IConfiguration configuration, IMessageBus messageBus)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.messageBus = messageBus;
        }

        [HttpPost("rabbitmq")]
        public string EvaluateRabbitMQ(int numberOfMessages, int sizeOfMessageInKB)
        {
            var messageData = CreateMessageData(sizeOfMessageInKB);
            return PublishMessages(numberOfMessages, (messageIndex) =>
            {
                var message = CreateMessage(messageIndex, numberOfMessages, messageData);
                messageBus.Publish<Message>(message);
            });
        }

        [HttpPost("kafka")]
        public string EvaluateKafka(int numberOfMessages, int sizeOfMessageInKB)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration.GetSection("Kafka")["Server"]
            };

            var producerBuilder = new ProducerBuilder<Null, Message>(producerConfig);
            producerBuilder.SetValueSerializer(new Shared.Kafka.MessageSerializer());
            using (var producer = producerBuilder.Build())
            {
                var messageData = CreateMessageData(sizeOfMessageInKB);
                return PublishMessages(numberOfMessages, (messageIndex) =>
                {
                    var message = CreateMessage(messageIndex, numberOfMessages, messageData);
                    producer.ProduceAsync(Shared.Kafka.Constants.Topic, new Confluent.Kafka.Message<Null, Message> { Value = message });
                });
            }
        }

        [HttpPost("redis")]
        public string EvaluateRedis(int numberOfMessages, int sizeOfMessageInKB)
        {
            var redisConnection = ConnectionMultiplexer.Connect(configuration.GetSection("Redis")["Server"]);
            var subscriber = redisConnection.GetSubscriber();
            var messageData = CreateMessageData(sizeOfMessageInKB);
            return PublishMessages(numberOfMessages, (messageIndex) =>
            {
                var message = CreateMessage(messageIndex, numberOfMessages, messageData);
                var serializedMessage = Shared.Redis.MessageSerializer.Serialize(message);
                subscriber.PublishAsync(Shared.Redis.Constants.Channel, serializedMessage);
            });
        }

        private string PublishMessages(int numberOfMessages, Action<int> PublishAction)
        {
            for (int i = 0; i < numberOfMessages; ++i)
            {
                try
                {
                    PublishAction(i);
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
            return $"Successfully published {numberOfMessages} messages";
        }

        private Message CreateMessage(int messageIndex, int numberOfMessages, List<byte[]> messageData)
        {
            return new Message(DateTime.Now, messageIndex + 1, numberOfMessages, messageData);
        }

        private List<byte[]> CreateMessageData(int sizeOfMessageInKB)
        {
            var data = new List<byte[]>();

            for (int i = 0; i < sizeOfMessageInKB; ++i)
            {
                byte[] dataBlock = new byte[1024];
                data.Add(dataBlock);
            }
            return data;
        }
    }
}