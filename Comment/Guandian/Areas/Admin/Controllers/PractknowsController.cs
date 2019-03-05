using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Guandian.Areas.Admin.Models;
using Guandian.Data;
using Guandian.Data.Entity;
using Guandian.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace Guandian.Areas.Admin.Controllers
{
    public class PractknowsController : CommonController
    {
        readonly GithubManageService _github;
        public PractknowsController(ApplicationDbContext context, GithubManageService github) : base(context)
        {
            _github = github;
        }
        public ActionResult Archive(int page = 1, int pageSize = 12)
        {
            // 查询分类
            var nodes = _context.FileNodes
                .Where(f => f.IsFile == false && !string.IsNullOrEmpty(f.Path))
                .OrderBy(f => f.Path)
                .Select(s => new FileNode
                {
                    Id = s.Id,
                    Path = s.Path
                })
                .ToList();
            ViewBag.Nodes = nodes;
            page = page < 1 ? 1 : page;
            ViewBag.Page = page;
            var result = _context.Practknow
                .OrderByDescending(n => n.CreatedTime)
                .Where(p => p.MergeStatus == MergeStatus.NeedArchive)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return View(result);
        }
        /// <summary>
        /// 归档
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Archive(Guid nodeId, List<Guid> ids)
        {
            // TODO:更改目录
            var fileNode = _context.FileNodes.SingleOrDefault(f => f.Id == nodeId);
            if (fileNode == null)
            {
                return NotFound();
            }
            // 查询对应的节点信息
            var currentPractknows = _context.Practknow.Where(p => ids.Contains(p.Id))
                .Include(p => p.FileNode)
                .ToList();
            var currentFileNodeIds = currentPractknows.Select(s => s.FileNode.Id).Distinct().ToList();

            try
            {
                // 同步到github
                foreach (var p in currentPractknows)
                {
                    var currentFileNode = p.FileNode;
                    // 删除内容
                    await _github.DeleteFile(currentFileNode.Path, "合并删除", currentFileNode.SHA);
                    // 添加内容
                    await _github.CreateFile(new NewFileDataModel
                    {
                        Path = fileNode.Path + "/" + currentFileNode.FileName,
                        Content = p.Content,
                        Message = "合并归档新建"
                    });
                    // 更新审核状态
                    p.MergeStatus = MergeStatus.Merged;
                }
                // 更新父节点信息。TODO:回调保持一致，避免同步失败后的不一致
                var updateFileNodes = _context.FileNodes
                    .Where(f => currentFileNodeIds.Contains(f.Id))
                    .ToList();
                updateFileNodes.ForEach(f => f.ParentNode = fileNode);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.Source);
                return Json("失败");

            }
            return Json("成功");
        }
        /// <summary>
        /// PR管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Review>>> PullRequest(int pageIndex = 1, int pageSize = 12)
        {
            // 获取Pull Request列表
            var data = await _context.Reviews
                .Include(r => r.User)
                .Take(20)
                .ToListAsync();

            if (pageIndex < 1) pageIndex = 1;
            ViewBag.Page = pageIndex;

            return View(data);
        }

        [HttpGet]
        public ActionResult<Practknow> PullRequestDetail(Guid id)
        {
            if (ModelState.IsValid)
            {
                var pracknow = _context.Practknow.SingleOrDefault(p => p.Id == id);
                return View(pracknow);
            }
            else
            {
                return NotFound("不存在该践识");
            }
        }

        /// <summary>
        /// 审核内容
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MergePR()
        {
            return View();
        }

        /// <summary>
        /// FileNodes管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FileNodes(Guid? id)
        {
            TempData["id"] = id;
            var path = new List<FileNode>();
            var fileNodes = new List<FileNode>();
            // 根目录
            if (id == null)
            {
                fileNodes = _context.FileNodes.Where(f => f.ParentNode == null).ToList();
            }
            else
            {
                var currentNode = _context.FileNodes
                    .Include(f => f.ParentNode)
                    .Where(f => f.Id == id)
                    .SingleOrDefault();

                if (currentNode != null)
                {
                    // 查询路径
                    if (!currentNode.IsFile)
                    {
                        // 查询当前内容
                        path = GetFilePath(currentNode.Id);
                        fileNodes = _context.FileNodes.Where(f => f.ParentNode.Id == id).ToList();
                    }
                    else
                    {
                        path = GetFilePath(currentNode.ParentNode.Id);
                        fileNodes = _context.FileNodes.Where(f => f.ParentNode.Id == currentNode.ParentNode.Id).ToList();
                    }
                }
            }
            var data = new FileNodesView
            {
                FileNodes = fileNodes,
                Path = path
            };
            return View(data);
        }
        /// <summary>
        /// 添加FileNode
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="id">父结点id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddFileNode(string name, string content, Guid? id)
        {
            if (string.IsNullOrEmpty(name)) return BadRequest();
            name = name.Trim();
            // 判断是否存在
            var exist = _context.FileNodes.SingleOrDefault(f => f.FileName.Equals(name));
            if (exist == null)
            {
                var parentNode = new FileNode();
                if (id != null)
                {
                    parentNode = _context.FileNodes.SingleOrDefault(f => f.Id == id);
                }
                // 构造github 新文件内容
                var newFileDataModel = new NewFileDataModel
                {
                    Content = string.IsNullOrEmpty(content) ? $"目录:{name}" : content,
                    Message = $"新建目录：{name}",
                    Path = $"{name}/README.md"
                };

                var newFileNode = new FileNode
                {
                    FileName = name,
                    IsFile = false,
                    ReadmeContent = content
                };
                // 有父节点时
                if (parentNode.FileName != null)
                {
                    newFileNode.ParentNode = parentNode;
                    var paths = GetFilePath(parentNode.Id)?.Select(p => p.FileName)?.ToArray();
                    // 设置路径
                    newFileNode.Path = string.Join("/", paths) + "/" + name;
                    newFileDataModel.Path = string.Join("/", paths) + "/" + name + "/readme.md";
                }
                else
                {
                    newFileNode.Path = name;
                }
                newFileDataModel.Path = WebUtility.UrlEncode(newFileDataModel.Path);
                var createFileResult = await _github.CreateFile(newFileDataModel);
                if (createFileResult != null)
                {
                    newFileNode.SHA = createFileResult.Sha;
                    newFileNode.GithubLink = createFileResult.Url;
                }
                _context.Add(newFileNode);
                _context.SaveChanges();
                return RedirectToAction(nameof(PractknowsController.FileNodes), new { id = newFileNode.Id });
            }
            else
            {
                return RedirectToAction(nameof(PractknowsController.FileNodes), new { id = exist.Id });
            }
        }
        /// <summary>
        /// 修改更新 readme
        /// </summary>
        /// <param name="content"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UpdateFileNode(string content, Guid id)
        {
            if (id == null) return BadRequest();
            // 判断是否存在
            var currentFileNode = _context.FileNodes.SingleOrDefault(f => f.Id == id);
            if (currentFileNode == null) return NotFound();

            // 更新github 文件内容
            var updateFileDataModel = new NewFileDataModel
            {
                Content = string.IsNullOrEmpty(content) ? $"目录:{currentFileNode.FileName}" : content,
                Message = $"README更新",
                Path = currentFileNode.Path + "/README.md",
                Sha = currentFileNode.SHA
            };
            // 更新内容
            currentFileNode.ReadmeContent = content;

            updateFileDataModel.Path = WebUtility.UrlEncode(updateFileDataModel.Path);
            var createFileResult = await _github.CreateFile(updateFileDataModel);
            if (createFileResult != null)
            {
                currentFileNode.SHA = createFileResult.Sha;
                currentFileNode.GithubLink = createFileResult.Url;
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(PractknowsController.FileNodes), new { id = currentFileNode.Id });
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

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult ClearFileNodes()
        {
            var all = _context.FileNodes.ToList();
            _context.FileNodes.RemoveRange(all);
            var result = _context.SaveChanges();
            return Content(result.ToString());
        }

        [AllowAnonymous]
        public ActionResult Test()
        {
            //var count = _context.Practknow.Where(p => p.MergeStatus == MergeStatus.NeedMerge)
            //          .Update(p => new Practknow { MergeStatus = MergeStatus.NeedArchive });
            return Content("");
        }
    }
}