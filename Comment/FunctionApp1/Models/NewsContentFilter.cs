using System.Collections.Generic;

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
        /// 需要移除的内容
        /// </summary>
        public string RemovePath { get; set; } = string.Empty;

        /// <summary>
        /// 获取默认筛选列表
        /// </summary>
        /// <returns></returns>
        public static List<NewsContentFilter> GetDefaultFilter()
        {
            return new List<NewsContentFilter>
            {
                new NewsContentFilter{Url="www.cnbeta.com",Path=@"id=""artibody"""},
                new NewsContentFilter{Url="tech.ifeng.com",Path=@"id=""main_content"""},
                new NewsContentFilter{Url="news.zol.com.cn",Path=@"id=""article-content"""},
                new NewsContentFilter{Url="tech.sina.com.cn",Path=@"id=""artibody"""},
                new NewsContentFilter{Url="m.pchome.net",Path=@"class=""article-content"""},
                new NewsContentFilter{Url="donews.com",Path=@"class=""article-con"""},
                new NewsContentFilter{Url="idcquan.com",Path=@"class=""clear deatil article-content fontSizeSmall BSHARE_POP"""},
                new NewsContentFilter{Url="oschina.net",Path=@"class=""editor-viewer text clear"""},
                new NewsContentFilter{Url="homea.people.com.cn",Path=@"id=""rwb_zw"""},
                new NewsContentFilter{Url="tech.qq.com",Path=@"id=""Cnt-Main-Article-QQ"""},
                new NewsContentFilter{Url="www.ebrun.com",Path=@"class=""post-text"""},
                new NewsContentFilter{Url="cn.engadget.com",Path=@"id=""page_body""",RemovePath=@"//footer[@class=""o-article_block""]"},
                new NewsContentFilter{Url="tech.huanqiu.com",Path=@"class=""la_con"""}
            };
        }
    }
}
