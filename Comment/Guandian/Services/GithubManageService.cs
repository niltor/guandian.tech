using Guandian.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Octokit;
using System.Linq;
using System.Threading.Tasks;

namespace Guandian.Services
{
    /// <summary>
    /// Github组织管理
    /// </summary>
    public class GithubManageService : BaseService
    {
        readonly GitHubClient _client;
        static readonly string OrgName = "TechViewsTeam";
        static readonly string TeamName = "BlogAuthor";

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
            var response = await _client.Repository.Content.CreateFile(filedata.Owner, filedata.Name, filedata.Path,
            new CreateFileRequest(filedata.Message, filedata.Content, true));
            return response.Content;
        }
        /// <summary>
        /// 邀请用户到Team
        /// </summary>
        /// <param name="username"></param>
        public async Task<MembershipState> AddUserToTeamAsync(string username)
        {
            var teams = await _client.Organization.Team.GetAll(OrgName);
            var authorTeam = teams.Where(t => t.Name.Equals(TeamName)).SingleOrDefault();
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
            var authorTeam = teams.Where(t => t.Name.Equals(TeamName)).SingleOrDefault();
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
    }

}