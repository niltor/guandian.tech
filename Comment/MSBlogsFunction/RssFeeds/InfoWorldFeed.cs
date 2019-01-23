using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml.Linq;
using HtmlAgilityPack;

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

        protected override string GetCategories(XElement element)
        {
            XName categoriesTag = "categories";
            var categories = element.Element(categoriesTag).Elements()
                .Where(e => e.Name.Equals(Category))
                .ToList();

            return string.Join(";", categories);
        }

        protected override string GetContent(string url)
        {

            if (HasContent != false) return "";
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler))
            {
                cookieContainer.Add(new Cookie("nsdr", @"""em=geethin%2540outlook.com|tkn=1|fnm=niltor|industry=Aerospace%252FDefense%2520Contractor|jobFunction=Applications|jobPosition=Manager""", "/", "infoworld.com"));
                cookieContainer.Add(new Cookie("idg_uuid", "745fb237-4296-42e1-95ed-e06ebfa7c6ac", "/", "infoworld.com"));

                var content = client.GetStringAsync(url).Result;
                if (!string.IsNullOrEmpty(content))
                {
                    var doc = new HtmlDocument();
                    doc.LoadHtml(content);
                    var contentNode = doc.DocumentNode.SelectSingleNode("//id[@drr-container]");
                    var removeXPaths = new List<string>();
                    var asideNodes = contentNode.SelectNodes("//aside").Select(n => n.XPath).ToList();
                    var scriptNodes = contentNode.SelectNodes("//script").Select(n => n.XPath).ToList();
                    removeXPaths.AddRange(asideNodes);
                    removeXPaths.AddRange(scriptNodes);
                    removeXPaths.ForEach(path =>
                    {
                        contentNode.SelectSingleNode(path).Remove();
                    });
                    return contentNode.InnerHtml;
                }
            }
            return "";
        }
    }


}
