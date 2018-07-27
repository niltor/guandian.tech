using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Functions.Data;
using Functions.Data.Entity;
using Functions.Models;
using HtmlAgilityPack;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Functions
{
    public class NewsService
    {
        private readonly string BingSearchKey = "4a959632315b40559f810a1edffed02b";
        private const string ImageSearchEndPoint = "https://api.cognitive.microsoft.com/bing/v7.0/images/search";
        private const string AutoSuggestionEndPoint = "https://api.cognitive.microsoft.com/bing/v7.0/suggestions";
        private const string NewsSearchEndPoint = "https://api.cognitive.microsoft.com/bing/v7.0/news/search";
        private const string TopNewsSearchEndPoint = "https://api.cognitive.microsoft.com/bing/v7.0/news";
        private const double Similarity = 0.5;//定义相似度
        private static HttpClient SearchClient { get; set; }
        TraceWriter _log;

        public NewsService(TraceWriter log)
        {
            _log = log;
            SearchClient = new HttpClient();
            SearchClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", BingSearchKey);
        }
        public async Task<List<BingNewsEntity>> GetNews(string keyword, string freshness = "Day")
        {
            //获取新闻
            if (string.IsNullOrEmpty(BingSearchKey))
            {
                return default;
            }
            List<BingNewsEntity> newNews = await GetNewsSearchResults(keyword);
            if (newNews == null)
            {
                return default;
            }

            //TODO:获取过滤来源白名单  改成域名过滤更好
            string[] providerFilter = { "新浪科技", "DoNews", "中关村在线", "中国IDC圈", "oschina", "cnBeta", "腾讯网", "凤凰网 科技" };

            //数据预处理
            for (int i = 0; i < newNews.Count; i++)
            {
                //来源过滤
                if (!providerFilter.Any(p => p.ToLower().Equals(newNews[i].Provider.ToLower())))
                {
                    _log.Info("filter:" + newNews[i].Provider + newNews[i].Title);
                    newNews[i].Title = string.Empty;
                    continue;
                }
                //无缩略图过滤
                if (string.IsNullOrEmpty(newNews[i].ThumbnailUrl))
                {
                    _log.Info("noPic:" + newNews[i].Title);
                    newNews[i].Title = string.Empty;
                    continue;
                }

                //TODO: 语义分词重复过滤
                for (int j = i + 1; j < newNews.Count; j++)
                {
                    //重复过滤
                    if ((StringTools.Similarity(newNews[i].Title, newNews[j].Title) > Similarity))
                    {
                        _log.Info("repeat" + newNews[i].Title);
                        newNews[i].Title = string.Empty;
                        continue;
                    }

                }
            }

            return newNews.Where(n => n.Title != string.Empty).ToList();
        }



        /// <summary>
        /// 获取必应新闻列表
        /// </summary>
        /// <param name="query">搜索关键词</param>
        /// <param name="count">数量</param>
        /// <param name="offset">偏移量</param>
        /// <param name="market">地区</param>
        /// <param name="freshness">时间频率</param>
        /// <returns></returns>
        public async Task<List<BingNewsEntity>> GetNewsSearchResults(string query, int count = 20, int offset = 0, string market = "zh-CN", string freshness = "Day")
        {
            var articles = new List<BingNewsEntity>();
            try
            {
                HttpResponseMessage result = await SearchClient.GetAsync(
              $"{NewsSearchEndPoint}/?q={WebUtility.UrlEncode(query)}&count={count}&offset={offset}&mkt={market}&freshness={freshness}");

                result.EnsureSuccessStatusCode();
                string json = await result.Content.ReadAsStringAsync();

                dynamic data = JObject.Parse(json);

                if (data.value == null || data.value.Count <= 0)
                {
                    return articles;
                }

                for (int i = 0; i < data.value.Count; i++)
                {
                    var news = new BingNewsEntity
                    {
                        Title = data.value[i].name,
                        Url = data.value[i].url,
                        Description = data.value[i].description,
                        ThumbnailUrl = data.value[i].image?.thumbnail?.contentUrl,
                        Provider = data.value[i].provider?[0].name,
                        DatePublished = data.value[i].datePublished,
                        Category = data.value[i].category
                    };
                    if (!string.IsNullOrEmpty(news.ThumbnailUrl))
                    {
                        articles.Add(news);
                    }
                }

                articles = articles.Where(m => m.Category != null && m.Category.Equals("ScienceAndTechnology"))
                    .ToList();
            }
            catch (Exception e)
            {
                _log.Info(e.Source + e.Message);
            }

            return articles;
        }

        /// <summary>
        /// 保存新闻到数据库
        /// </summary>
        public void SaveNews(string connectionString, List<BingNewsEntity> bingNews)
        {
            using (var context = new NewsDbContext(connectionString))
            {
                var news = new List<News>();
                news = bingNews.Select(s => new News
                {
                    Description = s.Description,
                    Provider = s.Provider,
                    ThumbnailUrl = s.ThumbnailUrl,
                    Title = s.Title,
                    Url = s.Url
                }).ToList();

                // 与过去50条对比，去除相似内容
                var oldNews = context.News.Take(50).ToList();

                foreach (var item in news)
                {
                    if (oldNews.Any(o => StringTools.Similarity(o.Title, item.Title) >= 0.5))
                    {
                        // 标记为无效
                        item.Title = null;
                    }
                }
                news = news.Where(n => n.Title != null).ToList();

                // 获取并解析新闻具体内容
                
                context.News.AddRange(news);
                context.SaveChanges();
            }
        }

        public async Task<string> GetNewsContentAsync(string url)
        {
            // 判断url来源，根据不同来源使用不同规则获取内容

            var hw = new HtmlWeb();
            var originContent = await hw.LoadFromWebAsync(url);


            return default;
        }
    }
}
