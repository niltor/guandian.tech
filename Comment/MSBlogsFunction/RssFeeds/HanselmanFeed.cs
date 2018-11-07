using System.Xml.Linq;

namespace MSBlogsFunction.RssFeeds
{
    public class HanselmanFeed : BaseFeed
    {
        public HanselmanFeed()
        {
            Urls = new string[] { "http://feeds.hanselman.com/scotthanselman" };
            Link = XName.Get("origLink", "http://rssnamespace.org/feedburner/ext/1.0");
            Authorfilter = new string[] { "Scott Hanselman" };
        }
    }
}
