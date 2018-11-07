using System;
using System.Linq;
using System.Threading.Tasks;
using Guandian.Data;
using Guandian.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guandian.Areas.Admin.Controllers
{

    public class ArticlesController : CommonController
    {
        private readonly ApplicationDbContext _context;

        public ArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1, int pageSize = 12)
        {
            page = page < 1 ? 1 : page;
            ViewBag.Page = page;
            var result = _context.Blogs
                .Where(b => !string.IsNullOrEmpty(b.Content))
                .Select(s => new Blog
                {
                    Title = s.Title,
                    AuthorName = s.AuthorName,
                    CreatedTime = s.CreatedTime,
                    Status = s.Status,
                    UpdatedTime = s.UpdatedTime,
                    Id = s.Id,
                    IsPublishMP = s.IsPublishMP
                })
                .OrderByDescending(n => n.UpdatedTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return View(result);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // POST: Admin/Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,AuthorName,ViewNunmber,Content,Keywords,Summary,Id,CreatedTime,UpdatedTime,Status")] Article article)
        {
            if (id != article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null) return NotFound();
            var article = await _context.Articles.FindAsync(id);
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Obsolete(Guid id)
        {
            if (id == null) return NotFound();
            var article = await _context.Articles.FindAsync(id);
            if (article == null) return NotFound();
            article.Status = Status.Obsolete;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ArticleExists(Guid id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
