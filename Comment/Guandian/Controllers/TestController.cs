using Guandian.Areas.Webhooks.Manager;
using Guandian.Data;
using Guandian.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Guandian.Controllers
{
    public class TestController : BaseController
    {
        readonly GithubManageService _service;
        readonly GithubEventManager _event;
        readonly GithubService _github;
        public TestController(
            GithubManageService service,
            GithubService github,
            GithubEventManager eventManager,
            ApplicationDbContext context,
            ILogger<TestController> logger) : base(context, logger)
        {
            _service = service;
            _github = github;
            _event = eventManager;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult GithubTestAsync()
        {
            return Ok();
        }
    }
}
