using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Xml.Linq;

namespace MSBlogsFunction.RssFeeds
{
    public class MicrosoftFeed : BaseFeed
    {
        public MicrosoftFeed()
        {
            Urls = new string[]
            {
                "https://blogs.microsoft.com/on-the-issues/feed/",
                "https://blogs.windows.com/buildingapps/feed/",
                "https://blogs.microsoft.com/ai/feed/",
                "https://blogs.microsoft.com/feed/",
                "https://devblogs.microsoft.com/dotnet/feed/",
                "https://devblogs.microsoft.com/aspnet/feed/",
                "https://devblogs.microsoft.com/powershell/feed/",
                "https://devblogs.microsoft.com/typescript/feed/",
                "https://devblogs.microsoft.com/devops/feed/",
                "https://devblogs.microsoft.com/visualstudio/feed/",
            };
            Authorfilter = new string[] { "MSFT", "Team", "Microsoft", "Visual", "Office", "Blog"
            ,"Jayme Singleton","Nish Anil","Phillip Carter","Olia Gavrysh","Daniel Roth","Cesar De la Torre"};
        }

        /// <summary>
        /// 获取缩略图
        /// </summary>
        /// <param name="url"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        protected override string GetThumb(XElement x)
        {
            // TODO:获取guid链接，然后获取图片
            var guidLink = new Uri(x.Value);
            var guid = guidLink.ParseQueryString().Get("p");
            var link = guidLink.AbsoluteUri.Replace(guidLink.Query, "");

            var hw = new HtmlWeb();
            var doc = hw.LoadFromWebAsync(link).Result;
            var content = doc.DocumentNode.SelectSingleNode($"//article[@id='post-{guid}']//img")
                .GetAttributeValue("src", null);
            return content;
        }
    }
}
