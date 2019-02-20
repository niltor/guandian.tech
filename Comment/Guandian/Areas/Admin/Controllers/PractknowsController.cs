using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Guandian.Areas.Admin.Models;
using Guandian.Data;
using Guandian.Data.Entity;
using Guandian.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guandian.Areas.Admin.Controllers
{
    public class PractknowsController : CommonController
    {
        readonly GithubManageService _github;
        public PractknowsController(ApplicationDbContext context, GithubManageService github) : base(context)
        {
            _github = github;
        }
        public ActionResult Index(int page = 1, int pageSize = 12)
        {
            page = page < 1 ? 1 : page;
            ViewBag.Page = page;
            var result = _context.Practknow
                .OrderByDescending(n => n.CreatedTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return View(result);
        }

        /// <summary>
        /// PR管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<Practknow>> PullRequest(int pageIndex = 1, int pageSize = 20)
        {
            if (pageIndex < 1) pageIndex = 1;
            ViewBag.Page = pageIndex;

            var data = _context.Practknow
                .Where(p => p.Status == Status.Default)
                .OrderBy(p => p.CreatedTime)
                .Select(s => new Practknow
                {
                    Id = s.Id,
                    AuthorName = s.AuthorName,
                    CreatedTime = s.CreatedTime,
                    PRNumber = s.PRNumber,
                    Status = s.Status,
                    Title = s.Title
                })
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

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
                .Where(f => f.Id == id)
                .SingleOrDefault();

                if (currentNode != null)
                {
                    // 查询路径
                    path = GetFilePath(currentNode.Id);
                    if (!currentNode.IsFile)
                    {
                        // 查询当前内容
                        fileNodes = _context.FileNodes.Where(f => f.ParentNode.Id == id).ToList();
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
        public async Task<ActionResult> AddFileNode(string name, Guid? id)
        {
            var parentNode = new FileNode();
            if (id != null)
            {
                parentNode = _context.FileNodes.SingleOrDefault(f => f.Id == id);
            }
            // 构造github 新文件内容
            var newFileDataModel = new NewFileDataModel
            {
                Content = $"目录:{name}",
                Message = $"初始化目录：{name}",
                Path = $"{name}/README.md"
            };

            var newFileNode = new FileNode
            {
                FileName = name,
                IsFile = false,
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
    }
}