using System;
using System.Collections.Generic;
using System.Text;

namespace BingNewsFunction.Models
{
    public class NewsContentFilter
    {
        /// <summary>
        /// 域名
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 筛选地址
        /// </summary>
        public string Path { get; set; }


        /// <summary>
        /// 获取默认筛选列表
        /// </summary>
        /// <returns></returns>
        public static List<NewsContentFilter> GetDefaultFilter()
        {
            var list = new List<NewsContentFilter>();
            return new List<NewsContentFilter>
            {
                new NewsContentFilter{Url="www.cnbeta.com",Path=@"id=""artibody"""},
                new NewsContentFilter{Url="tech.ifeng.com",Path=@"id=""main_content"""},
                new NewsContentFilter{Url="news.zol.com.cn",Path=@"id=""article-content"""},
                new NewsContentFilter{Url="tech.sina.com.cn",Path=@"id=""artibody"""},
                new NewsContentFilter{Url="pchome.net",Path=@"class=""cms-article-text"""},
                new NewsContentFilter{Url="donews.com",Path=@"class=""article-con"""},
                new NewsContentFilter{Url="idcquan.com",Path=@"class=""clear deatil article-content fontSizeSmall BSHARE_POP"""},
                new NewsContentFilter{Url="oschina.net",Path=@"class=""editor-viewer text clear"""}
            };
        }
    }
}
