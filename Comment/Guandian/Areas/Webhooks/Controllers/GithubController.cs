using System.Linq;
using Guandian.Areas.Webhooks.Models;
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
        public ActionResult PullRequestHook([FromBody]PullRequestEventModel pr)
        {
            // Notice: octokit.net 提供的EventPayload无法正常使用
            switch (pr.Action)
            {
                case "closed":
                    // 通过合并 
                    if (pr.PullRequest.Merged && pr.PullRequest.MergedBy != null)
                    {
                        var sha = pr.PullRequest.MergeCommitSha;
                        var count = _context.Practknow
                            .Where(p => p.PRNumber == pr.PullRequest.Number)
                            .Where(p => p.MergeStatus == MergeStatus.NeedMerge)
                            .Update(p => new Practknow { MergeStatus = MergeStatus.NeedArchive });
                    }
                    // 关闭未通过合并 
                    else if (!pr.PullRequest.Merged && pr.PullRequest.MergedBy == null)
                    {
                        // TODO:消息提醒
                    }
                    break;
                case "opened":
                    // TODO: 管理员邮件消息提醒
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
