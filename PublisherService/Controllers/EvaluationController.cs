using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Confluent.Kafka;
using Shared;
using Shared.Kafka;

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
        public string EvaluateRabbitMQ(int numberOfMessages)
        {
            return PublishMessages(numberOfMessages, (messageNumber) =>
            {
                messageBus.Publish<Message>(CreateMessage(messageNumber, numberOfMessages));
            });
        }

        [HttpPost("kafka")]
        public string EvaluateKafka(int numberOfMessages)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration.GetSection("Kafka")["Server"]
            };

            var producerBuilder = new ProducerBuilder<Null, Message>(producerConfig);
            producerBuilder.SetValueSerializer(new MessageSerializer());
            using (var producer = producerBuilder.Build())
            {
                return PublishMessages(numberOfMessages, (messageNumber) =>
                {
                    producer.ProduceAsync(Constants.Topic, new Confluent.Kafka.Message<Null, Message> { Value = CreateMessage(messageNumber, numberOfMessages) });
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

        private Message CreateMessage(int messageNumber, int numberOfMessages)
        {
            return new Message(DateTime.Now, messageNumber == numberOfMessages - 1);
        }
    }
}