using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Confluent.Kafka;
using Shared;
using Shared.Kafka;
using Shared.RabbitMQ;

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
            return PublishMessages(numberOfMessages, (messageNumber) =>
            {
                var message = CreateMessage(messageNumber, numberOfMessages, messageData);
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
            producerBuilder.SetValueSerializer(new MessageSerializer());
            using (var producer = producerBuilder.Build())
            {
                var messageData = CreateMessageData(sizeOfMessageInKB);
                return PublishMessages(numberOfMessages, (messageNumber) =>
                {
                    var message = CreateMessage(messageNumber, numberOfMessages, messageData);
                    producer.ProduceAsync(Constants.Topic, new Confluent.Kafka.Message<Null, Message> { Value = message });
                });
            }
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

        private Message CreateMessage(int messageNumber, int numberOfMessages, List<byte[]> messageData)
        {
            return new Message(DateTime.Now, messageNumber == numberOfMessages - 1, messageData);
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