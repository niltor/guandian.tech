using System;
using System.Linq;
using System.Threading.Tasks;
using Guandian.Data;
using Guandian.Data.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
                    Summary = b.Summary,
                    CreatedTime = b.CreatedTime,
                    Title = b.Title,
                    UpdatedTime = b.UpdatedTime,
                    Id = b.Id,
                    Link = b.Link
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

        // GET: News/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: News/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: News/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}