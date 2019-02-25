using System.Linq;
using Guandian.Data;
using Guandian.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using Z.EntityFramework.Plus;

namespace Guandian.Areas.Webhooks.Controllers
{
    public class GithubController : BaseController
    {
        public GithubController(ApplicationDbContext context) : base(context)
        {
        }

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
                    // TODO：合并pr
                    var sha = pr.PullRequest.MergeCommitSha;
                    var count = _context.Practknow.Where(p => p.MergeStatus == MergeStatus.NeedMerge)
                        .Update(p => new Practknow { MergeStatus = MergeStatus.NeedArchive });
                    break;
                case "opened":
                    // TODO:消息提醒
                    break;
                case "synchronize":
                    // do nothing
                    break;
                default:
                    break;
            }
            return Ok();
        }
    }
}
