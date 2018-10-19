using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using MSBlogsFunction.Entity;

namespace MSBlogsFunction
{
    public static class RssHelper
    {
        private static readonly HttpClient httpClient;
        static RssHelper()
        {
            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }
        }

        public static async Task<List<RssEntity>> GetRss(string url)
        {
            var blogs = new List<RssEntity>();
            string xmlString = await httpClient.GetStringAsync(url);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDoc = XDocument.Parse(xmlString);

                XNamespace nspc = "http://sxpdata.microsoft.com/metadata";
                IEnumerable<XElement> xmlList = xmlDoc.Root.Element("channel")?.Elements("item");
                // 根据作者进行筛选
                string[] authorfilter = { "MSFT", "Team", "Microsoft", "Visual", "Office", "Blog" };
                string[] htmlTagFilter = { "<h1>", "<h2>", "<h3>", "<h4>", "<h5>", "<p></p>" };
                blogs = xmlList?.Where(x => x.Name == "item")
                    .Where(x => IsContainKey(authorfilter, x.Element("author").Value)
                        && IsContainKey(htmlTagFilter, x.Element("content:encoded").Value))
                    .Select(x =>
                    {
                        DateTime createTime = DateTime.Now;

                        string createTimeString = x.Element("pubDate")?.Value;
                        if (!string.IsNullOrEmpty(createTimeString))
                        {
                            createTime = DateTime.Parse(createTimeString);
                        }

                        return new RssEntity
                        {
                            Title = x.Element("title")?.Value,
                            Content = x.Element("content:encoded")?.Value?.Replace("<pre", "<pre class=\"notranslate\""),
                            Description = x.Element("description")?.Value,
                            CreateTime = createTime,
                            Author = x.Element("author")?.Value,
                            Link = x.Element("link")?.Value,
                            Categories = x.Element("source")?.Value,
                            LastUpdateTime = createTime,
                        };
                    })
                    .ToList();
            }
            return blogs;
        }

        public static bool IsContainKey(string[] strArray, string key)
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
        public static async Task<ICollection<RssEntity>> GetAllBlogs()
        {
            var result = new List<RssEntity>();
            var feeds = new string[] {
                "https://blogs.microsoft.com/on-the-issues/feed/",
                "https://azurecomcdn.azureedge.net/en-us/blog/feed/",
                "https://blogs.windows.com/buildingapps/feed/",
                "https://blogs.microsoft.com/ai/feed/",
                "https://blogs.microsoft.com/feed/",
                "https://blogs.technet.microsoft.com/feed/"
            };
            foreach (var item in feeds)
            {
                var blogs = await GetRss(item);
                result.AddRange(blogs);
            }
            return result;
        }

    }
}