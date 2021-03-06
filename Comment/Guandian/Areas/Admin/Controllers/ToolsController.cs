using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guandian.Areas.Admin.Models;
using Guandian.Data;
using Guandian.Data.Entity;
using Guandian.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using MSDev.DB;
//using MSDev.MetaWeblog;

namespace Guandian.Areas.Admin.Controllers
{
    public class ToolsController : CommonController
    {
        //readonly MSDevContext _msdevContext;
        //public ToolsController(MSDevContext context, ApplicationDbContext applicationDbContext)
        //{
        //    _msdevContext = context;
        //    _context = applicationDbContext;
        //}

        public ToolsController(ApplicationDbContext context) : base(context)
        {
        }
        public ActionResult Index()
        {
            return View();
        }

        //public async Task<ActionResult> SyncToCnBlogsAsync()
        //{
        //    var service = new SyncCnBlogsService("niltor", "$54NilTor");
        //    var currentBlogs = _msdevContext.Blog
        //        .Where(b => b.Catalog.Name.Equals("从零开始学编程"))
        //        .OrderBy(b => b.CreatedTime)
        //        .Include(b => b.Catalog)
        //        .Include(b => b.Video)
        //        .ToList();

        //    var cnblogs = new List<PostInfo>();
        //    var pipeline = new MarkdownPipelineBuilder()
        //        .UseAdvancedExtensions()
        //        .UseSyntaxHighlighting()
        //        .Build();

        //    foreach (var item in currentBlogs)
        //    {
        //        cnblogs = currentBlogs.Select(s =>
        //        {
        //            var videoBlock = $"<p>本篇博客对应<a target=\"_blank\" href=\"http://msdevcc.azurewebsites.net/Video/Detail/{s.Video.Id}\" style=\"color:red\"><strong>视频讲解</strong></a></p>";
        //            return new PostInfo
        //            {
        //                DateCreated = s.CreatedTime,
        //                Description = videoBlock + Markdown.ToHtml(s.Content, pipeline),
        //                Title = s.Title,
        //            };
        //        }).ToList();
        //    }
        //    service.SyncTo(cnblogs, currentBlogs.FirstOrDefault()?.Catalog.Name);
        //    return Content("完成");
        //}

        /// <summary>
        /// 添加博客
        /// </summary>
        /// <param name="blogForms"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        // TODO: 验证
        public async Task<ActionResult> AddRssBlogs([FromBody]List<BlogForm> blogForms)
        {
            var blogs = new List<Blog>();
            blogs = blogForms
                .Where(b => !string.IsNullOrEmpty(b.Title))
                .Select(b => new Blog
                {
                    AuthorName = b.AuthorName,
                    Categories = "BlogRss",
                    Content = b.Content,
                    ContentEn = b.ContentEn,
                    CreatedTime = b.CreatedTime,
                    Keywords = b.Categories,
                    Summary = b.Summary,
                    Link = b.Link,
                    Title = b.Title,
                    TitleEn = b.TitleEn,
                    Thumbnail = b.Thumbnail
                }).ToList();
            // 插入前再去重
            var oldBlogs = _context.Blogs.OrderByDescending(b => b.CreatedTime)
                .Take(50)
                .ToList();

            foreach (var blog in blogs)
            {
                if (oldBlogs.Any(b => b.TitleEn == blog.TitleEn)) continue;
                _context.Add(blog);
            }
            var num = await _context.SaveChangesAsync();
            return Ok(num);
        }
        /// <summary>
        /// 去重博客
        /// </summary>
        /// <param name="blogs"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<List<string>> UniqueBlogs([FromBody]List<string> blogs)
        {

            var result = new List<string>(blogs);
            // 取库中数据 
            var currentBlogs = _context.Blogs
                .OrderByDescending(b => b.UpdatedTime)
                .Take(40)
                .ToList();
            // 筛选
            foreach (var item in blogs)
            {
                if (currentBlogs.Any(c => StringTools.Similarity(c.TitleEn, item) >= 0.85 || c.TitleEn == item))
                {
                    result.Remove(item);
                }
            }
            return Json(result);
        }
    }
}