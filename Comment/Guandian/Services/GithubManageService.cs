using Guandian.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Octokit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guandian.Services
{
    /// <summary>
    /// Github组织管理
    /// </summary>
    public class GithubManageService : BaseService
    {
        readonly GitHubClient _client;
        static readonly string OrgName = GithubConfig.OrgName;
        static readonly string ReposName = GithubConfig.ReposName;

        public GithubManageService(ILogger<GithubManageService> logger, IOptionsMonitor<GithubOption> options) : base(logger)
        {
            _client = new GitHubClient(new ProductHeaderValue("TechViews"))
            {
                Credentials = new Credentials(options.CurrentValue.PAT)
            };
        }
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <returns></returns>
        public async Task<RepositoryContentInfo> CreateFile(NewFileDataModel filedata)
        {
            try
            {
                var files = await _client.Repository.Content.GetAllContents(filedata.Owner, filedata.Name, filedata.Path);
                return files.FirstOrDefault();
            }
            catch (NotFoundException)
            {
                // 不存在处理
                try
                {
                    var response = await _client.Repository.Content.CreateFile(
                        filedata.Owner,
                        filedata.Name,
                        filedata.Path,
                        new CreateFileRequest(filedata.Message, filedata.Content, true));

                    return response.Content;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    return default;
                }

            }

        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sha"></param>
        /// <returns></returns>
        public async Task DeleteFile(string path, string message, string sha)
        {
            try
            {
                await _client.Repository.Content.DeleteFile(OrgName, ReposName, path, new DeleteFileRequest(message, sha));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                // do nothing 
            }
        }
        /// <summary>
        /// 向用户forked的仓库发起pull request
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="repoName">仓库名称</param>
        /// <returns></returns>
        public async Task<PullRequest> SyncToUserPR(string username, string repoName)
        {
            var newPullRequest = new NewPullRequest("自动同步到TechViews源仓库", OrgName + ":master", "master");
            var result = await _client.PullRequest.Create(username, repoName, newPullRequest);
            return result;
        }
        /// <summary>
        /// 是否有不同，需要同步
        /// </summary>
        /// <param name="username"></param>
        /// <param name="repoName"></param>
        /// <returns></returns>
        public async Task<bool> IsDiff(string username, string repoName)
        {
            var result = await _client.Repository.Commit.Compare(username, repoName, username + ":master", "TechViewsTeam:master");
            if (result.Files?.Count > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 邀请用户到Team
        /// </summary>
        /// <param name="username"></param>
        public async Task<MembershipState> AddUserToTeamAsync(string username)
        {
            var teams = await _client.Organization.Team.GetAll(OrgName);
            var authorTeam = teams.Where(t => t.Name.Equals(ReposName)).SingleOrDefault();
            if (authorTeam != null)
            {
                try
                {
                    var ship = await _client.Organization.Team.GetMembershipDetails(authorTeam.Id, username);
                    return ship.State.Value;
                }
                catch (NotFoundException)
                {
                    var response = await _client.Organization.Team.AddOrEditMembership(authorTeam.Id, username, new UpdateTeamMembership(TeamRole.Member));

                    if (response != null)
                    {
                        return response.State.Value;
                    }
                }
            }
            // 已添加或出错
            return MembershipState.Active;
        }
        /// <summary>
        /// Team中是否有用户
        /// </summary>
        /// <param name="username"></param>
        public async Task<bool> IsInTeam(string username)
        {
            var teams = await _client.Organization.Team.GetAll(OrgName);
            var authorTeam = teams.Where(t => t.Name.Equals(ReposName)).SingleOrDefault();
            if (authorTeam != null)
            {
                try
                {
                    var ship = await _client.Organization.Team.GetMembershipDetails(authorTeam.Id, username);
                    if (ship.State.Value == MembershipState.Active) return true;
                }
                catch (NotFoundException)
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取PR列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PullRequest>> GetPullRequests(int pageIndex, int pageSize)
        {
            try
            {
                var PRs = await _client.PullRequest.GetAllForRepository(OrgName, ReposName,
                    new ApiOptions
                    {
                        PageCount = 1,
                        PageSize = pageSize,
                        StartPage = pageIndex
                    });
                return PRs.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}