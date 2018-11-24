namespace MSBlogsFunction.RssFeeds
{
    public class TechnetFeed : BaseFeed
    {
        public TechnetFeed()
        {
            Urls = new string[]
            {
                "https://blogs.technet.microsoft.com/cloudplatform/rssfeeds/devblogs?tags=announcement",
                "https://blogs.technet.microsoft.com/cloudplatform/rssfeeds/cloud"
            };
            Category = "source";
            Creator = "author";
            Content = "description";
            Authorfilter = new string[] { "[MSFT]", "Team", "Microsoft", "Visual", "Office", "Blog" };
        }
    }
}
