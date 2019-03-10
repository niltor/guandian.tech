using GithubWebhook.Events;
using Guandian.Data;
using Guandian.Data.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;
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


        public async Task<string> HandleEventAsync(string eventName, string requestBody)
        {
            string re = "";
            switch (eventName)
            {
                case PingEvent.EventString:
                    var pingModel = PingEvent.FromJson(requestBody);
                    await PingAsync(pingModel);
                    break;
                case CommitCommentEvent.EventString:
                    var commitCommentModel = CommitCommentEvent.FromJson(requestBody);
                    await CommitCommentAsync(commitCommentModel);
                    break;
                case CreateEvent.EventString:
                    var createModel = CreateEvent.FromJson(requestBody);
                    await CreateAsync(createModel);
                    break;
                case DeleteEvent.EventString:
                    var deleteModel = DeleteEvent.FromJson(requestBody);
                    await DeleteAsync(deleteModel);
                    break;
                case DeploymentEvent.EventString:
                    var deploymentModel = DeploymentEvent.FromJson(requestBody);
                    await DeploymentAsync(deploymentModel);
                    break;
                case DeploymentStatusEvent.EventString:
                    var deploymentStatusModel = DeploymentStatusEvent.FromJson(requestBody);
                    await DeploymentStatusAsync(deploymentStatusModel);
                    break;
                case ForkEvent.EventString:
                    var forkModel = ForkEvent.FromJson(requestBody);
                    await ForkAsync(forkModel);
                    break;
                case GollumEvent.EventString:
                    var gollumModel = GollumEvent.FromJson(requestBody);
                    await GollumAsync(gollumModel);
                    break;
                case InstallationEvent.EventString:
                    var installationModel = InstallationEvent.FromJson(requestBody);
                    await InstallationAsync(installationModel);
                    break;
                case InstallationRepositoriesEvent.EventString:
                    var installationRepositoriesModel = InstallationRepositoriesEvent.FromJson(requestBody);
                    await InstallationRepositoriesAsync(installationRepositoriesModel);
                    break;
                case IssueCommentEvent.EventString:
                    var issueCommentModel = IssueCommentEvent.FromJson(requestBody);
                    await IssueCommentAsync(issueCommentModel);
                    break;
                case IssuesEvent.EventString:
                    var issuesModel = IssuesEvent.FromJson(requestBody);
                    await IssuesAsync(issuesModel);
                    break;
                case LabelEvent.EventString:
                    var labelModel = LabelEvent.FromJson(requestBody);
                    await LabelAsync(labelModel);
                    break;
                case MemberEvent.EventString:
                    var memberModel = MemberEvent.FromJson(requestBody);
                    await MemberAsync(memberModel);
                    break;
                case MembershipEvent.EventString:
                    var membershipModel = MembershipEvent.FromJson(requestBody);
                    await MembershipAsync(membershipModel);
                    break;
                case MilestoneEvent.EventString:
                    var milestoneModel = MilestoneEvent.FromJson(requestBody);
                    await MilestoneAsync(milestoneModel);
                    break;
                case OrganizationEvent.EventString:
                    var organizationModel = OrganizationEvent.FromJson(requestBody);
                    await OrganizationAsync(organizationModel);
                    break;
                case OrgBlockEvent.EventString:
                    var orgBlockModel = OrgBlockEvent.FromJson(requestBody);
                    await OrgBlockAsync(orgBlockModel);
                    break;
                case PageBuildEvent.EventString:
                    var pageBuildModel = PageBuildEvent.FromJson(requestBody);
                    await PageBuildAsync(pageBuildModel);
                    break;
                case ProjectCardEvent.EventString:
                    var projectCardModel = ProjectCardEvent.FromJson(requestBody);
                    await ProjectCardAsync(projectCardModel);
                    break;
                case ProjectColumnEvent.EventString:
                    var projectColumnModel = ProjectColumnEvent.FromJson(requestBody);
                    await ProjectColumnAsync(projectColumnModel);
                    break;
                case ProjectEvent.EventString:
                    var projectModel = ProjectEvent.FromJson(requestBody);
                    await ProjectAsync(projectModel);
                    break;
                case PublicEvent.EventString:
                    var publicModel = PublicEvent.FromJson(requestBody);
                    await PublicAsync(publicModel);
                    break;
                case PullRequestEvent.EventString:
                    var pullRequestModel = PullRequestEvent.FromJson(requestBody);
                    await PullRequestAsync(pullRequestModel);
                    break;
                case PullRequestReviewEvent.EventString:
                    var pullRequestReviewModel = PullRequestReviewEvent.FromJson(requestBody);
                    await PullRequestReviewAsync(pullRequestReviewModel);
                    break;
                case PullRequestReviewCommentEvent.EventString:
                    var pullRequestReviewCommentModel = PullRequestReviewCommentEvent.FromJson(requestBody);
                    await PullRequestReviewCommentAsync(pullRequestReviewCommentModel);
                    break;
                case PushEvent.EventString:
                    var pushModel = PushEvent.FromJson(requestBody);
                    await PushAsync(pushModel);
                    break;
                case ReleaseEvent.EventString:
                    var releaseModel = ReleaseEvent.FromJson(requestBody);
                    await ReleaseAsync(releaseModel);
                    break;
                case RepositoryEvent.EventString:
                    var repositoryModel = RepositoryEvent.FromJson(requestBody);
                    await RepositoryAsync(repositoryModel);
                    break;
                case StatusEvent.EventString:
                    var statusModel = StatusEvent.FromJson(requestBody);
                    await StatusAsync(statusModel);
                    break;
                case WatchEvent.EventString:
                    var watchModel = WatchEvent.FromJson(requestBody);
                    await WatchAsync(watchModel);
                    break;
                default:
                    throw new NotImplementedException("");
            }

            return re;
        }

        private Task StatusAsync(StatusEvent statusModel)
        {
            throw new NotImplementedException();
        }

        private Task WatchAsync(WatchEvent watchModel)
        {
            throw new NotImplementedException();
        }

        private Task RepositoryAsync(RepositoryEvent repositoryModel)
        {
            throw new NotImplementedException();
        }

        private Task ReleaseAsync(ReleaseEvent releaseModel)
        {
            throw new NotImplementedException();
        }

        private Task PushAsync(PushEvent pushModel)
        {
            throw new NotImplementedException();
        }

        private Task PullRequestReviewCommentAsync(PullRequestReviewCommentEvent pullRequestReviewCommentModel)
        {
            throw new NotImplementedException();
        }

        private Task PullRequestReviewAsync(PullRequestReviewEvent pullRequestReviewModel)
        {
            throw new NotImplementedException();
        }

        private Task PublicAsync(PublicEvent publicModel)
        {
            throw new NotImplementedException();
        }

        private Task ProjectAsync(ProjectEvent projectModel)
        {
            throw new NotImplementedException();
        }

        private Task ProjectColumnAsync(ProjectColumnEvent projectColumnModel)
        {
            throw new NotImplementedException();
        }

        private Task ProjectCardAsync(ProjectCardEvent projectCardModel)
        {
            throw new NotImplementedException();
        }

        private Task PageBuildAsync(PageBuildEvent pageBuildModel)
        {
            throw new NotImplementedException();
        }

        private Task OrgBlockAsync(OrgBlockEvent orgBlockModel)
        {
            throw new NotImplementedException();
        }

        private Task OrganizationAsync(OrganizationEvent organizationModel)
        {
            throw new NotImplementedException();
        }

        private Task MilestoneAsync(MilestoneEvent milestoneModel)
        {
            throw new NotImplementedException();
        }

        private Task MembershipAsync(MembershipEvent membershipModel)
        {
            throw new NotImplementedException();
        }

        private Task MemberAsync(MemberEvent memberModel)
        {
            throw new NotImplementedException();
        }

        private Task LabelAsync(LabelEvent labelModel)
        {
            throw new NotImplementedException();
        }

        private Task IssuesAsync(IssuesEvent issuesModel)
        {
            throw new NotImplementedException();
        }

        private Task IssueCommentAsync(IssueCommentEvent issueCommentModel)
        {
            throw new NotImplementedException();
        }

        private Task InstallationRepositoriesAsync(InstallationRepositoriesEvent installationRepositoriesModel)
        {
            throw new NotImplementedException();
        }

        private Task InstallationAsync(InstallationEvent installationModel)
        {
            throw new NotImplementedException();
        }

        private Task GollumAsync(GollumEvent gollumModel)
        {
            throw new NotImplementedException();
        }

        private Task ForkAsync(ForkEvent forkModel)
        {
            throw new NotImplementedException();
        }

        private Task DeploymentStatusAsync(DeploymentStatusEvent deploymentStatusModel)
        {
            throw new NotImplementedException();
        }

        private Task DeploymentAsync(DeploymentEvent deploymentModel)
        {
            throw new NotImplementedException();
        }

        private Task DeleteAsync(DeleteEvent deleteModel)
        {
            throw new NotImplementedException();
        }

        private Task CreateAsync(CreateEvent createModel)
        {
            throw new NotImplementedException();
        }

        private Task CommitCommentAsync(CommitCommentEvent commitCommentModel)
        {
            throw new NotImplementedException();
        }

        private Task PingAsync(PingEvent pingModel)
        {
            throw new NotImplementedException();
        }

        public async Task<int> PullRequestAsync(PullRequestEvent pr)
        {
            int count = 0;
            switch (pr.Action)
            {
                case "closed":
                    // 通过合并 
                    if (pr.PullRequest.Merged.GetValueOrDefault() && pr.PullRequest.MergedBy != null)
                    {
                        var sha = pr.PullRequest.MergeCommitSha;
                        count = await _context.Practknow
                           .Where(p => p.PRNumber == pr.PullRequest.Number)
                           .Where(p => p.MergeStatus == MergeStatus.NeedMerge)
                           .UpdateAsync(p => new Practknow { MergeStatus = MergeStatus.NeedArchive, PRSHA = sha });
                    }
                    // 关闭未通过合并 
                    else if (!pr.PullRequest.Merged.GetValueOrDefault() && pr.PullRequest.MergedBy == null)
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
