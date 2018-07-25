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
        public IActionResult Index(int page = 1, int pageSize = 12)
        {
            var news = _context.News
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
