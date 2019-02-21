using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Octokit;

namespace Guandian.Areas.Webhooks.Controllers
{
    public class GithubController : BaseController
    {
        [HttpGet("test")]
        public string Test()
        {
            return "test";
        }

        [HttpPost("pullrequest")]
        public ActionResult PullRequestHook(PullRequestEventPayload pr)
        {
            switch (pr.Action)
            {
                case "closed":
                    // TODO：用户端消息提醒
                    break;
                case "opened":
                    // TODO:消息提醒
                    break;
                case "synchronize":
                    // TODO: 处理pr 合并

                    break;
                default:
                    break;
            }
            return Ok();
        }
    }
}
