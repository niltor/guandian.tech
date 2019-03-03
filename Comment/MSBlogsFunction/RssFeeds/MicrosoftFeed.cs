namespace MSBlogsFunction.RssFeeds
{
    public class MicrosoftFeed : BaseFeed
    {
        public MicrosoftFeed()
        {
            Urls = new string[]
            {
                "https://blogs.microsoft.com/on-the-issues/feed/",
                "https://blogs.windows.com/buildingapps/feed/",
                "https://blogs.microsoft.com/ai/feed/",
                "https://blogs.microsoft.com/feed/",
                "https://devblogs.microsoft.com/dotnet/feed/",
                "https://devblogs.microsoft.com/aspnet/feed/",
                "https://devblogs.microsoft.com/powershell/feed/",
                "https://devblogs.microsoft.com/typescript/feed/",
                "https://devblogs.microsoft.com/devops/feed/",
                "https://devblogs.microsoft.com/visualstudio/feed/",
            };
            Authorfilter = new string[] { "MSFT", "Team", "Microsoft", "Visual", "Office", "Blog"
            ,"Jayme Singleton","Nish Anil","Phillip Carter","Olia Gavrysh","Daniel Roth","Cesar De la Torre"};
        }
    }
}
