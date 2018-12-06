using System;
using System.Collections.Generic;
using System.Text;

namespace MSBlogsFunction.RssFeeds
{
    public class DockerFeed : BaseFeed
    {
        public DockerFeed()
        {
            Urls = new string[]
            {
                "https://blog.docker.com/feed/",
            };
        }
    }
}
