using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using MSBlogsFunction.Entity;

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
        /// 微软technet博客
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<List<RssEntity>> GetRss(string url, ILogger log)
        {
            var blogs = new List<RssEntity>();
            string xmlString = await _httpClient.GetStringAsync(url);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDoc = XDocument.Parse(xmlString);

                XNamespace nspc = "http://sxpdata.microsoft.com/metadata";
                IEnumerable<XElement> xmlList = xmlDoc.Root.Element("channel")?.Elements("item");
                //TODO:根据作者进行筛选
                string[] authorfilter = { "[MSFT]", "Team", "Microsoft", "Visual", "Office", "Blog" };

                blogs = xmlList?.Where(x => x.Name == "item")
                    .Where(x => IsContainKey(authorfilter, x.Element("author").Value))
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
                            Description = "",
                            Content = x.Element("description")?.Value,
                            CreateTime = createTime,
                            Author = x.Element("author")?.Value,
                            Link = x.Element("link")?.Value,
                            Categories = x.Element("source")?.Value,
                            LastUpdateTime = createTime,
                            //MobileContent = x.Element("sxp_MobileContent")?.Value
                        };
                    })
                    .ToList();
            }
            return blogs;
        }

        /// <summary>
        /// 获取微软博客RSS
        /// </summary>
        /// <param name="url"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task<List<RssEntity>> GetMSBlogRss(string url, ILogger log)
        {
            var blogs = new List<RssEntity>();
            string xmlString = await _httpClient.GetStringAsync(url);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDoc = XDocument.Parse(xmlString);
                XNamespace nspcDc = "http://purl.org/dc/elements/1.1/";
                XNamespace nspcContent = "http://purl.org/rss/1.0/modules/content/";
                IEnumerable<XElement> xmlList = xmlDoc.Root.Element("channel")?.Elements("item");
                // 根据作者进行筛选
                string[] authorfilter = { "MSFT", "Team", "Microsoft", "Visual", "Office", "Blog" };
                string[] authorBlackList = { "Japan" };
                string[] htmlTagFilter = { "<h1>", "<h2>", "<h3>", "<h4>", "<h5>", "<p></p>" };

                blogs = xmlList?.Where(x => x.Name == "item")
                    .Where(i => IsContainKey(authorfilter, i.Element(nspcDc + "creator").Value)
                        && !IsContainKey(authorBlackList, i.Element(nspcDc + "creator").Value)
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
                            // TODO:格式处理
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

        /// <summary>
        /// 获取TechRePublic RSS具体内容
        /// </summary>
        /// <param name="url"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static (string, string) GetTechRePublicContent(string url, ILogger log)
        {
            using (var wc = new WebClient())
            {
                var html = wc.DownloadString(url);

                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var root = doc.DocumentNode;
                root.Descendants()
                    .Where(n => n.Name == "script")
                    .ToList()
                    .ForEach(n => n.Remove());

                var description = root.SelectSingleNode(".//p[@class='takeaway']")?.InnerText.Trim();

                //var categories = root.SelectSingleNode(".//p[@class='categories']")?.InnerText.Trim();
                //var title = root.SelectSingleNode(".//h1[@class='title']")?.InnerText.Trim();
                //var author = root.SelectSingleNode(".//a[@class='author']")?.InnerText.Trim();
                //var createTime = root.SelectSingleNode(".//span[@class='date']")?.InnerText.Trim();
                //DateTime pubDate = DateTime.Now;
                //if (!string.IsNullOrEmpty(createTime))
                //{
                //    DateTime.TryParse(createTime.Replace("PST", "-08"), out pubDate);
                //}
                var contentNode = root.SelectSingleNode(".//div[@id='content']//article//div[@class='content']");
                #region 去除无用内容
                var allOther = contentNode.SelectNodes("(.//p)[last()]/following-sibling::*");
                var adVideo = contentNode.SelectSingleNode(".//div[@class='shortcode video large']");
                var adArticle = contentNode.SelectSingleNode(".//div[@class='sharethrough-article']");
                var adSub = contentNode.SelectSingleNode(".//div[@class='newsletter-promo']");
                //var adImage = contentNode.SelectSingleNode(".//figure[@class='image pull-none image-large']");

                if (adVideo != null) adVideo.Remove();
                if (adArticle != null) adArticle.Remove();
                if (adSub != null) adSub.Remove();
                if (allOther != null && allOther.Count > 0)
                {
                    foreach (var item in allOther)
                    {
                        item.Remove();
                    }
                }

                #endregion
                var content = contentNode.InnerHtml;
                return (description, content);
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
        public async Task<List<RssEntity>> GetAllBlogs(ILogger log)
        {
            var result = new List<RssEntity>();
            var feeds = new string[] {
                "https://blogs.microsoft.com/on-the-issues/feed/",
                "https://azurecomcdn.azureedge.net/en-us/blog/feed/",
                "https://blogs.windows.com/buildingapps/feed/",
                "https://blogs.microsoft.com/ai/feed/",
                "https://blogs.microsoft.com/feed/",
                "https://blogs.msdn.microsoft.com/dotnet/feed/",
                "http://feeds.feedburner.com/microsoft/devblog"
            };
            foreach (var item in feeds)
            {
                var blogs = await GetMSBlogRss(item, log);
                // 去重添加
                if (blogs.Count > 0)
                {
                    foreach (var blog in blogs)
                    {
                        if (!result.Any(r => r.Title.Equals(blog.Title)))
                        {
                            result.Add(blog);
                        }
                    }
                }
            }

            var technetFeeds = new string[]
            {
                "https://blogs.technet.microsoft.com/cloudplatform/rssfeeds/devblogs?tags=announcement",
                "https://blogs.technet.microsoft.com/cloudplatform/rssfeeds/cloud"
            };
            foreach (var item in technetFeeds)
            {
                var blogs = await GetRss(item, log);
                // 去重添加
                if (blogs.Count > 0)
                {
                    foreach (var blog in blogs)
                    {
                        if (!result.Any(r => r.Title.Equals(blog.Title)))
                        {
                            result.Add(blog);
                        }
                    }
                }
            }
            return result;
        }

    }
}