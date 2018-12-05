using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Octokit;

namespace Guandian.Services
{

    public class GithubService : BaseService
    {
        readonly GitHubClient _client;
        readonly IHttpContextAccessor _httpContext;
        public GithubService(IHttpContextAccessor httpContext, ILogger<GithubService> logger) : base(logger)
        {
            _client = new GitHubClient(new ProductHeaderValue("TechViews"));
            _httpContext = httpContext;

        }


        protected bool SetToken()
        {
            string token = _httpContext.HttpContext.GetTokenAsync("access_token").Result;
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("没有获取到access_token");
                return false;
            }
            if (_client.Credentials == null)
            {
                _client.Credentials = new Credentials(token);
            }
            return true;
        }

        /// <summary>
        /// Fork 仓库
        /// </summary>
        /// <param name="name">仓库名</param>
        /// <returns></returns>
        public async Task<Repository> ForkAsync(string name = "blogs")
        {
            if (SetToken())
            {
                var result = await _client.Repository.Forks.Create("TechViewsTeam", name, new NewRepositoryFork { Organization = null });
                return result;
            }
            return default;
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <returns></returns>
        public async Task<RepositoryContentInfo> CreateFile(NewFileDataModel filedata)
        {
            if (SetToken())
            {
                var response = await _client.Repository.Content.CreateFile(filedata.Owner, filedata.Name, filedata.Path,
                new CreateFileRequest(filedata.Message, filedata.Content, true));
                return response.Content;
            }
            return default;
        }

        /// <summary>
        /// 创建PR请求
        /// </summary>
        /// <returns></returns>
        public async Task<PullRequest> PullRequest(NewPullRequestModel pr)
        {
            var result = await _client.PullRequest.Create(pr.Owner, pr.Name, new NewPullRequest(pr.Title, pr.Head, pr.Base));
            return result;
        }
    }

    public class NewPullRequestModel
    {
        /// <summary>
        /// 组织、用户名
        /// </summary>
        public string Owner { get; set; }
        /// <summary>
        /// 仓库名
        /// </summary>
        public string Name { get; set; }

        public string Title { get; }
        /// <summary>
        /// 目标仓库:分支
        /// </summary>
        public string Base { get; }
        /// <summary>
        /// 自己分支
        /// </summary>
        public string Head { get; }
    }

    public class NewFileDataModel
    {
        /// <summary>
        /// 组织、用户名
        /// </summary>
        public string Owner { get; set; }
        /// <summary>
        /// 仓库名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 分支名
        /// </summary>
        public string Branch { get; set; }
    }
}
