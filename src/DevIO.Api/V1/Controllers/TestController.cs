using DevIO.Api.Controllers;
using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DevIO.Api.V1.Controllers
{
    [ApiVersion("1.0", Deprecated = true),
     Route("api/v{version:apiVersion}/[controller]")]
    public class TestController : MainController
    {
        private readonly ILogger _logger;

        public TestController(INotifier notifier, IUser user, ILogger<TestController> logger) :
            base(notifier, user)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Value()
        {
            _logger.LogTrace("Trace");
            _logger.LogDebug("Debug");
            _logger.LogInformation("Information");
            _logger.LogWarning("Warning");
            _logger.LogError("Error");
            _logger.LogCritical("Critical");

            return "I am version v1.";
        }
    }
}
