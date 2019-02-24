using Guandian.Data;
using Guandian.Data.Entity;
using Guandian.Models.Forms;
using Guandian.Models.PractknowView;
using Guandian.Services;
using Guandian.Utilities;
using Microsoft.AspNetCore.Authorization;
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
    //[Authorize("Github")]
    public class PractknowsController : BaseController
    {
        private readonly GithubService _github;
        private readonly GithubManageService _githubManage;

        public PractknowsController(ApplicationDbContext context, GithubService github, GithubManageService githubManage, UserManager<User> userManager, ILogger<PractknowsController> logger) : base(userManager, context, logger)
        {
            _github = github;
            _githubManage = githubManage;
        }

        /// <summary>
        /// 践识主页
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
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
                nodeTree = _context.FileNodes.Where(f => f.ParentNode.Id == nodeId).ToList();

                if (currentNode == null) return View();
                // 如果是文件夹
                if (!currentNode.IsFile)
                {
                    currentNodes = GetFilePath(currentNode.Id);
                }
                else
                {
                    practknow = await _context.Practknow.SingleOrDefaultAsync(p => p.FileNode == currentNode);
                }
            }
            else
            {
                // 默认内容
                nodeTree = _context.FileNodes.Where(f => f.ParentNode == null).ToList();
            }

            return View(new PractknowIndexView
            {
                CurrentNodes = currentNodes,
                NodeTree = nodeTree,
                Practknow = practknow
            });
        }
        /// <summary>
        /// 获取当前结点路径 TODO:待抽象复用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected List<FileNode> GetFilePath(Guid id)
        {
            var result = new List<FileNode>();
            var node = _context.FileNodes.Where(f => f.Id == id)
                .Include(f => f.ParentNode)
                .SingleOrDefault();

            while (node != null)
            {
                result.Add(node);
                node = GetParentNode(node.Id);
            }

            FileNode GetParentNode(Guid currentId)
            {
                var currentNode = _context.FileNodes.Where(f => f.Id == currentId)
                    .Include(f => f.ParentNode)
                    .SingleOrDefault();
                return currentNode?.ParentNode ?? null;
            }
            result.Reverse();
            return result;
        }

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

        /// <summary>
        /// 创建践识
        /// </summary>
        /// <param name="practknow"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddPractknowForm practknow)
        {
            if (ModelState.IsValid)
            {
                // 获取用户信息
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var currentUser = _context.Users.Where(a => a.Id == userId).SingleOrDefault();
                // 查询是否已发布同标题文章
                var currentFile = _context.Practknow.Where(p => p.User == currentUser && p.Title.Equals(practknow.Title)).SingleOrDefault();
                // 标题重复，则以当前提交的为准
                if (currentFile != null) _context.Remove(currentFile);
                var newFile = new Practknow
                {
                    User = currentUser,
                    AuthorName = currentUser.NickName ?? currentUser.UserName ?? currentUser.Email
                };
                _context.Entry(newFile).CurrentValues.SetValues(practknow);
                string owner = "";// github登录名
                string reposName = "";// 仓储名
                string sha = "";// 文件sha，有则是更新，无则为新增

                // 如果没有fork
                if (!currentUser.IsForkPractknow)
                {
                    var forkResult = await _github.ForkAsync("practknow");
                    if (forkResult.Fork)
                    {
                        owner = forkResult?.Owner?.Login;
                        reposName = forkResult?.Name ?? "practknow";
                        // 保存fork状态
                        currentUser.IsForkPractknow = true;

                        // 保存仓储信息
                        var repository = new Repository
                        {
                            Name = reposName,
                            Tag = "practknow",
                            User = currentUser,
                            Login = owner
                        };
                        _context.Repositories.Add(repository);
                    }
                    else
                    {
                        //　TODO:fork失败处理
                    }
                }
                else // 同步仓储
                {
                    // 查询仓库名称
                    var repository = _context.Repositories.Where(r => r.User == currentUser && r.Tag.Equals("practknow")).SingleOrDefault();
                    owner = repository.Login;
                    reposName = repository.Name;
                    if (repository != null)
                    {
                        var isDiff = await _githubManage.IsDiff(owner, reposName);
                        if (isDiff)
                        {
                            // 由组织发起pull request，同步当前内容
                            var asyncResult = await _githubManage.SyncToUserPR(owner, reposName);
                            // 合并 pull request
                            if (asyncResult != null)
                            {
                                var mergeResult = await _github.MergePR(owner, reposName, asyncResult.Number, new Octokit.MergePullRequest
                                {
                                    CommitMessage = "自动合并来自组织的Pull Request",
                                    CommitTitle = "自动同步合并",
                                    MergeMethod = Octokit.PullRequestMergeMethod.Merge,
                                    Sha = asyncResult?.Head?.Sha
                                });
                            }
                        }
                        // 判断是否有重复名称的文件，有则取其sha，进行更新
                        var exist = await _github.GetFileInfo(owner, reposName, practknow.Path + practknow.Title + ".md");
                        if (exist != null) sha = exist.Sha;
                    }
                    else
                    {
                        // TODO:返回提示信息
                    }
                }
                // 提交到个人fork的仓库
                var createFileResult = await _github.CreateFile(new NewFileDataModel
                {
                    Branch = "master",
                    Content = practknow.Content,
                    Message = "创建文章:" + practknow.Title,
                    Name = reposName ?? "practknow",
                    Path = practknow.Path + practknow.Title + ".md",
                    Owner = owner ?? userId ?? "",
                    Sha = sha
                });
                // 提交成功后，添加FileNode更新sha
                if (createFileResult != null)
                {
                    // 添加FileNode到未分类下,TODO
                    var parentNode = _context.FileNodes.SingleOrDefault(f => f.FileName == "未分类");
                    var fileNode = new FileNode
                    {
                        IsFile = true,
                        FileName = practknow.Title,
                        GithubLink = createFileResult.Url,
                        SHA = createFileResult.Sha,
                        Path = "未分类/" + practknow.Title,
                        ParentNode = parentNode
                    };
                    _context.Add(fileNode);
                    // 更新文件sha
                    if (createFileResult.Sha != null) newFile.SHA = createFileResult.Sha;
                }
                // 先查询是否已经存在pull request
                var hasPR = _github.HasPR(new NewPullRequestModel { Head = owner + ":master" }, out var pullRequest);
                if (!hasPR)
                {
                    // 发起 新内容pull request ，等待审核 
                    var prResult = await _github.PullRequest(new NewPullRequestModel
                    {
                        Head = owner + ":master",
                        Title = "新践识文章:" + practknow.Title
                    });
                    newFile.PRNumber = prResult.Number;
                    newFile.PRSHA = prResult.MergeCommitSha;
                }
                else
                {
                    newFile.PRSHA = pullRequest.MergeCommitSha;
                }
                // 更新PR信息
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
