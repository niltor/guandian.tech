using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;
using MSBlogsFunction.Entity;

namespace MSBlogsFunction.RssFeeds
{
    /// <summary>
    /// 基础feed
    /// </summary>
    public class BaseFeed
    {
        /// <summary>
        /// 链接
        /// </summary>
        public string[] Urls { get; set; }
        /// <summary>
        /// 作者过滤
        /// </summary>
        protected string[] Authorfilter { get; set; }
        /// <summary>
        /// 内容html标签过滤
        /// </summary>
        protected string[] HtmlTagFilter { get; set; } = { "<h1>", "<h2>", "<h3>", "<h4>", "<h5>", "<p></p>" };
        /// <summary>
        /// xml root名称
        /// </summary>
        protected string RootName { get; set; } = "channel";
        /// <summary>
        /// xml item 名称
        /// </summary>
        protected string ItemName { get; set; } = "item";
        public XName Creator { get; set; } = XName.Get("creator", "http://purl.org/dc/elements/1.1/");
        public XName Content { get; set; } = XName.Get("encoded", "http://purl.org/rss/1.0/modules/content/");
        public XName PubDate { get; set; } = "pubDate";
        public XName Category { get; set; } = "category";
        public XName Title { get; set; } = "title";
        public XName Description { get; set; } = "description";
        public XName Link { get; set; } = "link";

        /// <summary>
        /// 是否包含内容
        /// </summary>
        public bool HasContent { get; set; } = true;
        protected XDocument xmlDoc;

        protected HttpClient _httpClient = new HttpClient();

        public BaseFeed()
        {
        }

        /// <summary>
        /// 解析返回
        /// </summary>
        /// <param name="number">数量</param>
        /// <returns></returns>
        public virtual async Task<List<RssEntity>> GetBlogs(int number = 3)
        {
            var result = new List<RssEntity>();
            try
            {
                foreach (var url in Urls)
                {
                    string xmlString = await _httpClient.GetStringAsync(url);
                    if (!string.IsNullOrEmpty(xmlString))
                    {
                        xmlDoc = XDocument.Parse(xmlString);
                        IEnumerable<XElement> xmlList = xmlDoc.Root.Element(RootName)?.Elements(ItemName);

                        var blogs = xmlList.Where(i => IsContainKey(Authorfilter, i.Element(Creator)?.Value)
                                && IsContainKey(HtmlTagFilter, i.Element(Content)?.Value))
                            .Select(x =>
                            {
                                DateTime createTime = DateTime.Now;
                                string createTimeString = x.Element(PubDate)?.Value;
                                if (!string.IsNullOrEmpty(createTimeString))
                                {
                                    DateTime.TryParse(createTimeString, out createTime);
                                }

                                var description = x.Element(Description)?.Value;
                                // 去除html标签
                                //description = Regex.Replace(description, "<.*?>", String.Empty);

                                if (!string.IsNullOrEmpty(description))
                                {
                                    if (description.Length > 999)
                                    {
                                        description = description.Substring(0, 999);
                                    }
                                }
                                string content = x.Element(Content)?.Value;
                                if (!string.IsNullOrEmpty(content))
                                {
                                    content = content.Replace("<pre", "<pre class=\"notranslate\"");
                                }

                                return new RssEntity
                                {
                                    Title = x.Element(Title)?.Value,
                                    Content = content,
                                    Description = description ?? "",
                                    CreateTime = createTime,
                                    Author = x.Element(Creator)?.Value,
                                    Link = x.Element(Link)?.Value,
                                    Categories = GetCategories(x),
                                    LastUpdateTime = createTime,
                                    ThumbUrl = GetThumb(x.Element("guid"))
                                };
                            })
                            .Take(number)
                            .ToList();
                        result.AddRange(blogs);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.InnerException); ;
                throw;
            }
            // 处理没有内容的博客
            result.Where(r => string.IsNullOrEmpty(r.Content)).ToList()
                .ForEach(item =>
                {
                    item.Content = GetContent(item.Link);
                });

            return result.Where(r => !string.IsNullOrEmpty(r.Content))
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected virtual string GetContent(string url) => "";
        /// <summary>
        /// 获取标签目录
        /// </summary>
        /// <returns></returns>
        protected virtual string GetCategories(XElement element)
        {
            var categories = element.Elements()
                .Where(e => e.Name.Equals(Category))?
                .Select(s => s.Value)
                .ToArray();
            return string.Join(";", categories);
        }
        /// <summary>
        /// 获取缩略图
        /// </summary>
        /// <param name="url"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        protected virtual string GetThumb(XElement x)
        {

            return "";
        }
        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="strArray"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected bool IsContainKey(string[] strArray, string key)
        {
            if (strArray == null || strArray.Length < 1 || string.IsNullOrEmpty(key))
            {
                return true;
            }
            foreach (string item in strArray)
            {
                if (key.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
