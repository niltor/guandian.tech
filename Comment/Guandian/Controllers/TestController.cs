using Guandian.Areas.Webhooks.Manager;
using Guandian.Data;
using Guandian.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Guandian.Controllers
{
    public class TestController : BaseController
    {
        readonly GithubManageService _service;
        readonly GithubEventManager _event;
        readonly GithubService _github;
        readonly IDistributedCache _cache;
        public TestController(
            GithubManageService service,
            GithubService github,
            GithubEventManager eventManager,
            ApplicationDbContext context,
            IDistributedCache cache,
            ILogger<TestController> logger) : base(context, logger)
        {
            _service = service;
            _github = github;
            _event = eventManager;
            _cache = cache;
        }
        public ActionResult GithubTestAsync()
        {
            return Ok();
        }

        //public ActionResult TestSetCache()
        //{
        //    _cache.SetString("key", "0916");
        //    return Ok();
        //}

        //public ActionResult TestGetCache()
        //{
        //    var value = _cache.GetString("key");
        //    return Ok(value);
        //}


    }
}
