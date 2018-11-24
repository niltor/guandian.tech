using Microsoft.Extensions.Logging;

namespace Guandian.Services
{
    public class BaseService
    {
        readonly ILogger _logger;

        public BaseService(ILogger logger)
        {
            _logger = logger;
        }
    }
}
