using Guandian.Data;
using Guandian.Data.Entity;
using Guandian.Models.Forms;
using Guandian.Models.PractknowView;
using Guandian.Services;
using Guandian.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Guandian.Controllers
{
    public class PractknowsController : BaseController
    {
        private readonly GithubService _github;

        public PractknowsController(ApplicationDbContext context, GithubService github, UserManager<IdentityUser> userManager, ILogger<PractknowsController> logger) : base(userManager, context, logger)
        {
            _github = github;
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
                if (currentNode == null) return View();
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
            var navNodes = _context.FileNodes.Where(f => f.IsFile == false)
                .Where(f => f.ParentNode == null)
                .Include(f => f.ChildrenNodes)
                .ToList();
            string navHtml = "";
            foreach (var node in navNodes)
            {
                navHtml += BuildNavHtml(node);
            }
            ViewBag.NavHtml = navHtml;
            return View();
        }

        /// <summary>
        /// 构建导航菜单
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public string BuildNavHtml(FileNode node)
        {
            string result = "";
            if (node.ChildrenNodes != null && node.ChildrenNodes.Count > 0)
            {
                result = $"<ul id='{node.Id}'>{node.FileName}";
                foreach (var childnode in node.ChildrenNodes)
                {
                    result += BuildNavHtml(childnode);
                }
                result += "</ul>";
            }
            else
            {
                result = $"<li id='{node.Id}'>{node.FileName}</li>";
            }
            return result;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddPractknowForm practknow)
        {
            if (ModelState.IsValid)
            {
                // 获取用户信息
                var email = User.FindFirst(ClaimTypes.Email);
                var currentUser = _context.Authors.Where(a => a.Email.Equals(email.Value)).SingleOrDefault();
                _logger.LogDebug(StringTools.ToJson(currentUser));

                // TODO:处理路径
                // 先fork仓库
                var forkResult = await _github.ForkAsync();

                // 提交到github，获取fileNode信息
                var createFileResult = await _github.CreateFile(new NewFileDataModel
                {
                    Branch = "master",
                    Content = practknow.Content,
                    Message = "创建文章:" + practknow.Title,
                    Name = forkResult.Name ?? "blogs",
                    Path = practknow.Path,
                    Owner = forkResult.Owner.Name ?? email.Value
                });

                Console.WriteLine(StringTools.ToJson(createFileResult));

                // 添加新内容
                var newPractknow = new Practknow
                {
                    Title = practknow.Title,
                    Keywords = practknow.Keywords,
                    Summary = practknow.Summary,
                    Content = practknow.Content,
                    FileNode = new FileNode
                    {
                        IsFile = true,
                        FileName = practknow.Title + ".md",
                        GithubLink = createFileResult.GitUrl,
                        Path = practknow.Path + "/" + practknow.Title,
                    }
                };



                newPractknow.Author = currentUser;
                newPractknow.AuthorName = currentUser.UserName;
                _context.Add(newPractknow);

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
