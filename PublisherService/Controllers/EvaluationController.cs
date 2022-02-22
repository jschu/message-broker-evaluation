using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PublisherService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluationController : ControllerBase
    {
        private readonly ILogger<EvaluationController> logger;

        public EvaluationController(ILogger<EvaluationController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        public async Task EvaluateMessageBroker(int numberOfMessages)
        {
            logger.LogInformation("Received Request");
        }
    }
}
