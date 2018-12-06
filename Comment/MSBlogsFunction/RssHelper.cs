using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using MSBlogsFunction.Entity;
using MSBlogsFunction.RssFeeds;

namespace MSBlogsFunction
{
    public class RssHelper
    {
        private readonly HttpClient _httpClient;
        public RssHelper()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
            }
        }

        public bool IsContainKey(string[] strArray, string key)
        {
            foreach (string item in strArray)
            {
                if (key.Contains(item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取所有rss内容
        /// </summary>
        /// <returns></returns>
        public List<RssEntity> GetAllBlogs(ILogger log)
        {
            var result = new List<RssEntity>();

            var msFeed = new MicrosoftFeed();
            result.AddRange(msFeed.GetBlogs().Result);

            var dockerFeed = new DockerFeed();
            result.AddRange(dockerFeed.GetBlogs().Result);

            var blogs = new List<RssEntity>();
            foreach (var blog in result)
            {
                if (!blogs.Any(b => b.Title.Contains(blog.Title)))
                {
                    blogs.Add(blog);
                }
            }
            return blogs;
        }

    }
}