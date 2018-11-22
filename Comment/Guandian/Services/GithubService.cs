using Octokit;

namespace Guandian.Services
{

    public class GithubService
    {
        readonly string _token;
        readonly GitHubClient _client;
        public GithubService(string accessToken)
        {
            _token = accessToken;
            _client = new GitHubClient(new ProductHeaderValue("TechViews"))
            {
                Credentials = new Credentials(_token)
            };
        }


    }
}
