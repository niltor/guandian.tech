using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSDev.MetaWeblog;
using Newtonsoft.Json;

namespace Comment.Services
{
    /// <summary>
    /// 博客园同步
    /// </summary>
    public class SyncCnBlogsService : ISyncService
    {
        private readonly string _username;
        private readonly string _password;
        public SyncCnBlogsService(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public async Task SyncTo(List<PostInfo> blogs, string categories = null)
        {
            // 获取blogid
            string blogUrl = "http://www.cnblogs.com";
            string metablogUrl = "https://rpc.cnblogs.com/metaweblog/msdeveloper";
            var blogcon = new BlogConnectionInfo(blogUrl, metablogUrl, "320557", "niltor", "$54NilTor");
            var client = new Client(blogcon);
            var userBlogs = client.GetUsersBlogs();
            blogcon.BlogID = userBlogs.FirstOrDefault().BlogID;
            // 获取分类
            var cnCategories = client.GetCategories();
            var aimCategories = new List<string>();

            if (categories != null)
            {
                aimCategories = cnCategories.Where(c => c.Title.ToLower().Contains(categories.ToLower()))
                    .Select(s => s.Title)
                    .ToList();
            }
            // 添加文章
            try
            {
                foreach (var blog in blogs)
                {
                    blog.Categories = aimCategories;
                    client.NewPost(blog, aimCategories);
                    Console.WriteLine("=====:" + blog.Title + "==:" + JsonConvert.SerializeObject(aimCategories));
                    Thread.Sleep((int)TimeSpan.FromSeconds(62).TotalMilliseconds);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        void ISyncService.SyncFrom(List<PostInfo> blogs, string categories)
        {
            throw new NotImplementedException();
        }
    }
}
