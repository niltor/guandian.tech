using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Comment.Models;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Comment.Data;

namespace Comment.Controllers
{
    public class HomeController : Controller
    {
        readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 主页，资讯列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IActionResult Index(int page = 1, int pageSize = 20)
        {

            var news = _context.News
                .OrderByDescending(n => n.UpdatedTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View(new NewsListViewModel { NewsList = news });
        }


        /// <summary>
        /// 微信推送用内容
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IActionResult WeixinMP(int page = 1, int pageSize = 10)
        {
            var news = _context.News
                .OrderByDescending(n => n.UpdatedTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return View(new NewsListViewModel { NewsList = news });
        }


        public IActionResult Viewpoint()
        {
            return View();
        }

        public IActionResult Subject()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
