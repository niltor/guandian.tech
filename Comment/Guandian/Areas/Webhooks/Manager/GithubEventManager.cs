﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guandian.Areas.Webhooks.Models;
using Guandian.Data;
using Guandian.Data.Entity;
using Z.EntityFramework.Plus;

namespace Guandian.Areas.Webhooks.Manager
{
    /// <summary>
    /// github webhook事件管理
    /// </summary>
    public class GithubEventManager : BaseManager
    {
        public GithubEventManager(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> PullRequestAsync(PullRequestEventModel pr)
        {
            int count = 0;
            switch (pr.Action)
            {
                case "closed":
                    // 通过合并 
                    if (pr.PullRequest.Merged && pr.PullRequest.MergedBy != null)
                    {
                        var sha = pr.PullRequest.MergeCommitSha;
                        count = await _context.Practknow
                           .Where(p => p.PRNumber == pr.PullRequest.Number)
                           .Where(p => p.MergeStatus == MergeStatus.NeedMerge)
                           .UpdateAsync(p => new Practknow { MergeStatus = MergeStatus.NeedArchive, PRSHA = sha });
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
            return count;
        }
    }
}
