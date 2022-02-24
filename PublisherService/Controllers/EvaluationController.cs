using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;

namespace PublisherService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluationController : ControllerBase
    {
        private readonly ILogger<EvaluationController> logger;
        private readonly IMessageBus messageBus;

        public EvaluationController(ILogger<EvaluationController> logger, IMessageBus messageBus)
        {
            this.logger = logger;
            this.messageBus = messageBus;
        }

        [HttpPost]
        public void EvaluateRabbitMQ(int numberOfMessages)
        {
            for (int i = 0; i < numberOfMessages; ++i)
            {
                messageBus.Publish<Message>(new Message(DateTime.Now, i == numberOfMessages - 1));
            }
        }
    }
}
