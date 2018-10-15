using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comment.Services;
using Markdig;
using Markdig.SyntaxHighlighting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSDev.DB;
using MSDev.MetaWeblog;

namespace Comment.Areas.Admin.Controllers
{
    public class ToolsController : CommonController
    {
        readonly MSDevContext _context;
        public ToolsController(MSDevContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SyncToCnBlogsAsync()
        {
            var service = new SyncCnBlogsService("niltor", "$54NilTor");
            var currentBlogs = _context.Blog
                .Where(b => b.Catalog.Name.Equals("从零开始学编程"))
                .OrderBy(b => b.CreatedTime)
                .Include(b => b.Catalog)
                .Include(b => b.Video)
                .ToList();

            var cnblogs = new List<PostInfo>();
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseSyntaxHighlighting()
                .Build();

            foreach (var item in currentBlogs)
            {
                cnblogs = currentBlogs.Select(s =>
                {
                    var videoBlock = $"<p>本篇博客对应<a target=\"_blank\" href=\"http://msdevcc.azurewebsites.net/Video/Detail/{s.Video.Id}\" style=\"color:red\"><strong>视频讲解</strong></a></p>";
                    return new PostInfo
                    {
                        DateCreated = s.CreatedTime,
                        Description = videoBlock + Markdown.ToHtml(s.Content, pipeline),
                        Title = s.Title,
                    };
                }).ToList();
            }
            service.SyncTo(cnblogs, currentBlogs.FirstOrDefault()?.Catalog.Name);
            return Content("完成");
        }
    }
}