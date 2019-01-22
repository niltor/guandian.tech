using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSBlogsFunction.RssFeeds
{
    public class InfoWorldFeed : BaseFeed
    {
        public InfoWorldFeed()
        {
            Urls = new string[]
            {
                "https://www.infoworld.com/index.rss",
            };
            HasContent = false;
        }


        protected override string GetContent(string url)
        {
            return base.GetContent(url);
        }
    }


}
