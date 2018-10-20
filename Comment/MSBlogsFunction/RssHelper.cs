using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
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

        public static async Task<List<RssEntity>> GetRss(string url, ILogger log)
        {
            var blogs = new List<RssEntity>();
            string xmlString = await httpClient.GetStringAsync(url);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDoc = XDocument.Parse(xmlString);
                XNamespace nspcDc = "http://purl.org/dc/elements/1.1/";
                XNamespace nspcContent = "http://purl.org/rss/1.0/modules/content/";
                IEnumerable<XElement> xmlList = xmlDoc.Root.Element("channel")?.Elements("item");
                // 根据作者进行筛选
                string[] authorfilter = { "MSFT", "Team", "Microsoft", "Visual", "Office", "Blog" };
                string[] htmlTagFilter = { "<h1>", "<h2>", "<h3>", "<h4>", "<h5>", "<p></p>" };

                blogs = xmlList?.Where(x => x.Name == "item")
                    .Where(i => IsContainKey(authorfilter, i.Element(nspcDc + "creator").Value)
                        && IsContainKey(htmlTagFilter, i.Element(nspcContent + "encoded").Value))
                    .Select(x =>
                    {
                        DateTime createTime = DateTime.Now;
                        string createTimeString = x.Element("pubDate")?.Value;
                        if (!string.IsNullOrEmpty(createTimeString))
                        {
                            createTime = DateTime.Parse(createTimeString);
                        }

                        var categories = x.Elements()
                            .Where(e => e.Name.Equals("category"))?
                            .Select(s => s.Value)
                            .ToArray();
                        log.LogInformation(categories.ToString());
                        return new RssEntity
                        {
                            Title = x.Element("title")?.Value,
                            Content = x.Element(nspcContent + "encoded")?.Value?.Replace("<pre", "<pre class=\"notranslate\""),
                            Description = x.Element("description")?.Value,
                            CreateTime = createTime,
                            Author = x.Element(nspcDc + "creator")?.Value,
                            Link = x.Element("link")?.Value,
                            Categories = string.Join(";", categories),
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
        public static async Task<ICollection<RssEntity>> GetAllBlogs(ILogger log)
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
                var blogs = await GetRss(item, log);
                result.AddRange(blogs);
            }
            return result;
        }

    }
}