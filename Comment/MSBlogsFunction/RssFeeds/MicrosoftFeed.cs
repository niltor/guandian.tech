namespace MSBlogsFunction.RssFeeds
{
    public class MicrosoftFeed : BaseFeed
    {
        public MicrosoftFeed()
        {
            Urls = new string[]
            {
                "https://blogs.microsoft.com/on-the-issues/feed/",
                "https://azurecomcdn.azureedge.net/en-us/blog/feed/",
                "https://blogs.windows.com/buildingapps/feed/",
                "https://blogs.microsoft.com/ai/feed/",
                "https://blogs.microsoft.com/feed/",
                "https://blogs.msdn.microsoft.com/dotnet/feed/",
                "http://feeds.feedburner.com/microsoft/devblog"
            };
            Authorfilter = new string[] { "MSFT", "Team", "Microsoft", "Visual", "Office", "Blog"
            ,"Jayme Singleton","Nish Anil","Phillip Carter","Olia Gavrysh","Daniel Roth","Cesar De la Torre"};
        }
    }
}
