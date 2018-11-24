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

        /// <summary>
        /// 获取TechRePublic Rss概要内容
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public List<RssEntity> GetTechRePublicRss(ILogger log)
        {
            var result = new List<RssEntity>();
            var urls = new string[]
            {
                "https://www.techrepublic.com/rssfeeds/topic/open-source/",
                "https://www.techrepublic.com/rssfeeds/topic/artificial-intelligence/",
                "https://www.techrepublic.com/rssfeeds/topic/cloud/",
                "https://www.techrepublic.com/rssfeeds/topic/developer/",
            };
            using (var wc = new WebClient())
            {
                foreach (var url in urls)
                {
                    var xml = wc.DownloadString(url);
                    var xdoc = XDocument.Parse(xml);
                    XNamespace nspcMedia = "http://search.yahoo.com/mrss/";
                    XNamespace nspcS = "https://www.techrepublic.com/search";
                    var channel = xdoc.Root.Element("channel");

                    var items = channel.Elements("item")
                        .Where(e => !e.Element("link").Value.Contains("videos"))
                        .Select(s =>
                        {
                            // 格式处理
                            DateTime createTime = DateTime.Now;
                            var createTimeString = s.Element("pubDate").Value;
                            if (!string.IsNullOrEmpty(createTimeString))
                            {
                                DateTime.TryParse(createTimeString, out createTime);
                            }
                            return new RssEntity
                            {
                                Title = s.Element("title").Value,
                                Link = s.Element("link").Value,
                                Description = s.Element("description").Value,
                                Author = s.Element(nspcMedia + "credit").Value,
                                CreateTime = createTime,
                            };
                        })
                        .ToList();
                    // 当前去重
                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            if (result.Any(r => StringTools.Similarity(item.Title, r.Title) > 0.5))
                            {
                                log.LogInformation("重复内容:" + item.Title);
                                continue;
                            }
                            else
                            {
                                result.Add(item);
                            }
                        }
                    }
                }
            }
            return result;
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

            var technetFeed = new TechnetFeed();
            result.AddRange(technetFeed.GetBlogs().Result);

            var msFeed = new MicrosoftFeed();
            result.AddRange(msFeed.GetBlogs().Result);

            var rss = new HanselmanFeed();
            result.AddRange(rss.GetBlogs().Result);

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