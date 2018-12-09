using Google.Apis.Logging;
using Guandian.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guandian.Services
{
    public class GithubManageService : BaseService
    {
        readonly GitHubClient _client;

        public GithubManageService(ILogger<GithubManageService> logger, IOptions<GithubOption> options) : base(logger)
        {
            _client = new GitHubClient(new ProductHeaderValue("TechViews"))
            {
                Credentials = new Credentials(options.Value.PAT)
            };
        }
        
        /// <summary>
        /// Team中是否有用户
        /// </summary>
        /// <param name="username"></param>
        public async void HasUserAsync(string username)
        {
            // 以组织管理者身份登录 
            var teams = await _client.Organization.Team.GetAll("TechViews");
            var authorTeam = teams.Where(t => t.Name.Equals("AuthorTeam")).SingleOrDefault();
            if (authorTeam != null)
            {
                var response = await _client.Organization.Team.AddOrEditMembership(authorTeam.Id, username, new UpdateTeamMembership(TeamRole.Member));

            }
        }
    }

}
