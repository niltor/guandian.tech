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
        private readonly GithubManageService _githubManage;

        public PractknowsController(ApplicationDbContext context, GithubService github, GithubManageService githubManage, UserManager<User> userManager, ILogger<PractknowsController> logger) : base(userManager, context, logger)
        {
            _github = github;
            _githubManage = githubManage;
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
            // TODO:重复标题内容的处理
            if (ModelState.IsValid)
            {
                // 获取用户信息
                var email = User.FindFirstValue(ClaimTypes.Email);
                var currentUser = _context.Users.Where(a => a.Email.Equals(email)).SingleOrDefault();
                _logger.LogDebug(StringTools.ToJson(currentUser));

                // 先保存用户文章内容，待审批状态，没有结点信息
                var currentFile = _context.Practknow.Where(p => p.User == currentUser && p.Title.Equals(practknow.Title)).SingleOrDefault();
                // 标题重复，则以当前提交的为准
                if (currentFile != null) _context.Remove(currentFile);
                var newFile = new Practknow
                {
                    User = currentUser,
                    AuthorName = currentUser.NickName ?? currentUser.UserName ?? currentUser.Email
                };
                _context.Entry(newFile).CurrentValues.SetValues(practknow);

                // 如果没有fork
                if (!currentUser.IsForkPractknow)
                {
                    var forkResult = await _github.ForkAsync("practknow");
                    Console.WriteLine(StringTools.ToJson(forkResult));

                    if (forkResult.Fork)
                    {
                        // 保存fork状态
                        currentUser.IsForkPractknow = true;
                        _context.Users.Update(currentUser);

                        // 保存仓储信息
                        var repository = new Repository
                        {
                            Name = forkResult?.Name,
                            Tag = "practknow",
                            User = currentUser,
                            Login = forkResult?.Owner?.Login
                        };
                        _context.Repositories.Add(repository);

                        // 提交到个人fork的仓库
                        var createFileResult = await _github.CreateFile(new NewFileDataModel
                        {
                            Branch = "master",
                            Content = practknow.Content,
                            Message = "创建文章:" + practknow.Title,
                            Name = forkResult.Name ?? "practknow",
                            Path = practknow.Path + practknow.Title + ".md",
                            Owner = forkResult.Owner.Login ?? email ?? ""
                        });
                        // 更新文件内容
                        if (createFileResult.Sha != null)
                        {
                            newFile.SHA = createFileResult.Sha;
                        }

                        // 发起pull request ，等待审核 
                        var prResult = await _github.PullRequest(new NewPullRequestModel
                        {
                            Head = forkResult?.Owner?.Login + ":master",
                            Title = "新文章待审核"
                        });
                        Console.WriteLine(StringTools.ToJson(prResult));
                    }
                    else
                    {
                        //　TODO:fork失败处理
                    }
                }
                else
                {
                    // 查询仓库名称
                    var repository = _context.Repositories.Where(r => r.User == currentUser && r.Tag.Equals("practknow")).SingleOrDefault();

                    if (repository != null)
                    {
                        // 由组织发起pull request，同步当前内容
                        var asyncResult = await _githubManage.SyncToUserPR(repository.Login, repository.Name);
                        Console.WriteLine(StringTools.ToJson(asyncResult));
                        // 合并 pull request
                        if (asyncResult != null)
                        {
                            var mergeResult = await _github.MergePR(repository.Login, repository.Name, asyncResult.Number, new Octokit.MergePullRequest
                            {
                                CommitMessage = "自动合并来自组织的Pull Request",
                                CommitTitle = "自动同步合并",
                                MergeMethod = Octokit.PullRequestMergeMethod.Merge,
                                Sha = asyncResult?.Head?.Sha
                            });

                        }
                        // 提交到个人fork的仓库
                        // 判断是否有重复名称的文件，有则取其sha，进行更新？
                        var newFileDataModel = new NewFileDataModel
                        {
                            Branch = "master",
                            Content = practknow.Content,
                            Message = "创建文章:" + practknow.Title,
                            Name = repository.Name ?? "practknow",
                            Path = practknow.Path + practknow.Title + ".md",
                            Owner = repository.Login ?? email ?? ""
                        };
                        var exist = await _github.GetFileInfo(repository.Login, repository.Name, practknow.Path + practknow.Title + ".md");
                        if (exist != null) newFileDataModel.Sha = exist.Sha;

                        var createFileResult = await _github.CreateFile(newFileDataModel);
                        // 更新文件内容
                        if (createFileResult.Sha != null)
                        {
                            newFile.SHA = createFileResult.Sha;
                        }
                        // 发起 新内容pull request ，等待审核 
                        var prResult = await _github.PullRequest(new NewPullRequestModel
                        {
                            Head = repository.Login + ":master",
                            Title = "新践识文章:" + practknow.Title
                        });
                    }
                    else
                    {
                        // TODO:返回提示信息
                    }

                }
                _context.Add(newFile);
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
