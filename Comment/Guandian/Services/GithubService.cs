using System.Linq;
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

            _client.Credentials = new Credentials(token);
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
            return null;
        }
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <returns></returns>
        public async Task<RepositoryContentInfo> CreateFile(NewFileDataModel filedata)
        {
            if (SetToken())
            {
                try
                {
                    if (!string.IsNullOrEmpty(filedata.Sha))
                    {
                        var response = await _client.Repository.Content.UpdateFile(
                            filedata.Owner,
                            filedata.Name,
                            filedata.Path,
                            new UpdateFileRequest(filedata.Message, filedata.Content, filedata.Sha));
                        return response.Content;
                    }
                    else
                    {
                        var response = await _client.Repository.Content.CreateFile(
                            filedata.Owner,
                            filedata.Name,
                            filedata.Path,
                            new CreateFileRequest(filedata.Message, filedata.Content, true));
                        return response.Content;
                    }
                }
                catch (System.Exception e)
                {
                    _logger.LogError("创建github文件时出错:" + e.Message + e.Source);
                    return null;
                }
            }
            return null;
        }
        /// <summary>
        /// 创建PR请求
        /// </summary>
        /// <returns></returns>
        public async Task<PullRequest> PullRequest(NewPullRequestModel pr)
        {
            if (SetToken())
            {
                var result = await _client.PullRequest.Create(pr.Owner, pr.Name, new NewPullRequest(pr.Title, pr.Head, pr.Base));
                return result;
            }
            return null;
        }

        /// <summary>
        /// merge PR
        /// </summary>
        /// <param name="owner">用户</param>
        /// <param name="name">仓储名称</param>
        /// <param name="number">pull request number</param>
        /// <param name="mergeModel"></param>
        /// <returns></returns>
        public async Task<PullRequestMerge> MergePR(string owner, string name, int number, MergePullRequest mergeModel)
        {
            if (SetToken())
            {
                var response = await _client.PullRequest.Merge(owner, name, number, mergeModel);
                _logger.LogDebug("同步合并=>" + owner + ":" + name);
                return response;
            }
            return null;
        }

        /// <summary>
        /// 获取某文件信息
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<RepositoryContent> GetFileInfo(string owner, string name, string path)
        {
            if (SetToken())
            {
                try
                {
                    var result = await _client.Repository.Content.GetAllContents(owner, name, path);
                    return result.FirstOrDefault();
                }
                catch (NotFoundException e)
                {
                    _logger.LogInformation("没有该文件:" + e.Message);
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// 是否有待处理的PR
        /// </summary>
        /// <param name="pr"></param>
        /// <returns></returns>
        public bool HasPR(NewPullRequestModel pr, out PullRequest pullRequet)
        {
            if (SetToken())
            {
                var response = _client.PullRequest.GetAllForRepository(pr.Owner, pr.Name, new PullRequestRequest
                {
                    Base = pr.Base,
                    Head = pr.Head
                }).Result;
                if (response.Count > 0)
                {
                    pullRequet = response.FirstOrDefault();
                    return true;
                }
            }
            pullRequet = null;
            return false;
        }
    }


    public class NewPullRequestModel
    {
        /// <summary>
        /// 组织、用户名
        /// </summary>
        public string Owner { get; set; } = GithubConfig.OrgName;
        /// <summary>
        /// 仓库名
        /// </summary>
        public string Name { get; set; } = GithubConfig.ReposName;
        public string Title { get; set; }
        /// <summary>
        /// 分支
        /// </summary>
        public string Base { get; set; } = GithubConfig.DefaultBranch;
        /// <summary>
        /// 示例：niltor:master
        /// </summary>
        public string Head { get; set; }
    }

    public class NewFileDataModel
    {
        /// <summary>
        /// 组织、用户名
        /// </summary>
        public string Owner { get; set; } = GithubConfig.OrgName;
        /// <summary>
        /// 仓库名
        /// </summary>
        public string Name { get; set; } = GithubConfig.ReposName;
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
        public string Branch { get; set; } = GithubConfig.DefaultBranch;
        public string Sha { get; set; }

    }
}
