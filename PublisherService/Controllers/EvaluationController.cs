using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
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
        public void EvaluateRabbitMQ(int numberOfMessages)
        {
            for (int i = 0; i < numberOfMessages; ++i)
            {
                messageBus.Publish<Message>(new Message(DateTime.Now, i == numberOfMessages - 1));
            }
        }

        [HttpPost("kafka")]
        public void EvaluateKafka(int numberOfMessages)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration.GetSection("Kafka")["Server"]
            };

            var producerBuilder = new ProducerBuilder<Null, Message>(producerConfig);
            producerBuilder.SetValueSerializer(new MessageSerializer());
            using (var producer = producerBuilder.Build())
            {
                for (int i = 0; i < numberOfMessages; ++i)
                {
                    var message = new Message(DateTime.Now, i == numberOfMessages - 1);
                    producer.ProduceAsync(Constants.Topic, new Confluent.Kafka.Message<Null, Message> { Value = message });
                }
            }
        }
    }
}