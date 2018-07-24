using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comment.Areas.Admin.Models;
using Comment.Data;
using Comment.Data.Entity;
using Comment.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Comment.Areas.Admin.Controllers
{
    public class BingNewsController : CommonController
    {
        private readonly ApplicationDbContext _context;

        public BingNewsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost("multi")]
        public ActionResult AddNews([FromBody] List<BingNewsEntity> bingNews)
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

            Console.WriteLine(JsonConvert.SerializeObject(news));
            // 与过去100条对比，去除相似内容
            var oldNews = _context.News.Take(50).ToList();

            foreach (var item in news)
            {
                if (oldNews.Any(o => StringTools.Similarity(o.Title, item.Title) >= 0.5)){
                    // 标记为无效
                    item.Title = null;
                }
            }

            news = news.Where(n => n.Title != null).ToList();
            Console.WriteLine(JsonConvert.SerializeObject(news));
            _context.News.AddRange(news);
            _context.SaveChanges();
            return Ok();
        }
    }
}