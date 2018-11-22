using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guandian.Data;
using Guandian.Data.Entity;
using Guandian.Models.PractknowView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guandian.Controllers
{
    public class PractknowsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PractknowsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PractknowIndexView>> Index(Guid? nodeId = null)
        {
            var nodeTree = new List<FileNode>();
            var currentNodes = new List<FileNode>();
            var practknow = new Practknow();
            if (nodeId != null)
            {
                var currentNode = await _context.FileNodes
                    .Include(f => f.ParentNode)
                    .SingleOrDefaultAsync(f => f.Id == nodeId);
                if (currentNode == null) return View(default);
                // 如果是文件夹
                if (!currentNode.IsFile)
                {
                    currentNodes = await _context.FileNodes.Where(f => f.ParentNode == currentNode).ToListAsync();
                }
                else
                {
                    practknow = await _context.Practknow.SingleOrDefaultAsync(p => p.FileNode == currentNode);
                }
            }
            else
            {
                currentNodes = await _context.FileNodes.Where(f => f.Practknows == null).ToListAsync();
            }

            return View(new PractknowIndexView
            {
                CurrentNodes = currentNodes,
                NodeTree = nodeTree,
                Practknow = practknow
            });
        }

        // GET: Practknows/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practknow = await _context.Practknow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (practknow == null)
            {
                return NotFound();
            }

            return View(practknow);
        }

        public IActionResult Create()
        {
            // 查询目录
            var navNodes = _context.FileNodes.Where(f => f.IsFile == false).ToList();
            ViewBag.NavNodes = navNodes;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,AuthorName,ViewNunmber,Content,Keywords,Summary,Id,CreatedTime,UpdatedTime,Status")] Practknow practknow)
        {
            if (ModelState.IsValid)
            {
                practknow.Id = Guid.NewGuid();
                _context.Add(practknow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(practknow);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practknow = await _context.Practknow.FindAsync(id);
            if (practknow == null)
            {
                return NotFound();
            }
            return View(practknow);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,AuthorName,ViewNunmber,Content,Keywords,Summary,Id,CreatedTime,UpdatedTime,Status")] Practknow practknow)
        {
            if (id != practknow.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(practknow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PractknowExists(practknow.Id))
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
            return View(practknow);
        }

        // GET: Practknows/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practknow = await _context.Practknow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (practknow == null)
            {
                return NotFound();
            }

            return View(practknow);
        }

        // POST: Practknows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var practknow = await _context.Practknow.FindAsync(id);
            _context.Practknow.Remove(practknow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PractknowExists(Guid id)
        {
            return _context.Practknow.Any(e => e.Id == id);
        }
    }
}
