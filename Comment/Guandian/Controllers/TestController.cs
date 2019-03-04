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
        readonly GithubService _github;
        public TestController(GithubManageService service, GithubService github, ApplicationDbContext context, ILogger<TestController> logger) : base(context, logger)
        {
            _service = service;
            _github = github;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GithubTestAsync()
        {
            var status = await _service.AddUserToTeamAsync("geethin");
            if (status == Octokit.MembershipState.Pending)
            {
                // TODO:提示跳转到项目页面接受邀请。


            }
            return Content("");
        }
    }
}
