using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Guandian.Data;
using Guandian.Data.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Guandian.Controllers
{
    //[Authorize(Policy = "GitHub")]
    public class BlogsController : Controller
    {
        readonly ApplicationDbContext _context;
        public BlogsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult Index(int page = 1, int pageSize = 20)
        {
            var blogs = _context.Blogs
                .OrderByDescending(n => n.UpdatedTime)
                .Select(b => new Blog
                {
                    AuthorName = b.AuthorName,
                    Summary = Regex.Replace(b.Summary, @"<(.|\n)*?>", string.Empty),
                    CreatedTime = b.CreatedTime,
                    Title = b.Title,
                    UpdatedTime = b.UpdatedTime,
                    Id = b.Id,
                    Link = b.Link,
                    Thumbnail = b.Thumbnail
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return View(blogs);
        }
        /// <summary>
        /// 博客详情页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult<Blog> Detail(Guid id)
        {
            var blog = _context.Blogs.Where(n => n.Id == id).FirstOrDefault();
            return View(blog);
        }
        public IActionResult Create()
        {
            var _token = HttpContext.GetTokenAsync("access_token").Result;
            Console.WriteLine(_token);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,AuthorName,ViewNunmber,Content,Keywords,Summary,Id,CreatedTime,UpdatedTime,Status")] Article article)
        {
            if (ModelState.IsValid)
            {
                article.Id = Guid.NewGuid();
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }
    }
}