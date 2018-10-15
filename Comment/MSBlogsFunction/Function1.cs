using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using MSBlogsFunction.Models;

namespace MSBlogsFunction
{
    public static class Function1
    {
        [FunctionName("MSBlogs")]
        public static async Task RunAsync([TimerTrigger("0 0 */12 * * *")]TimerInfo myTimer, TraceWriter log)

        //public static async Task RunAsync([TimerTrigger("0 * */6 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            var blogs = await RssHelper.GetDevBlogs();
            var tobeAddBlogs = new List<BlogForm>();
            string subKey = Environment.GetEnvironmentVariable("SubKey");
            var translateHelper = new TranslateTextHelper(subKey);

            if (blogs.Count > 0)
            {
                foreach (var item in blogs)
                {
                    var blogForm = new BlogForm
                    {
                        ContentEn = translateHelper.TranslateText(item.Description),
                        AuthorName = item.Author,
                        Categories = item.Categories,
                        Content = item.Description,
                        Title = item.Title,
                        TitleEn = translateHelper.TranslateText(item.Title),
                        Link = item.Link,
                        CreatedTime = item.CreateTime
                    };
                    tobeAddBlogs.Add(blogForm);
                }
            }

            var cloudBlogs = await RssHelper.GetCloudNews();
            if (cloudBlogs.Count > 0)
            {
                foreach (var item in cloudBlogs)
                {
                    var blogForm = new BlogForm
                    {
                        ContentEn = translateHelper.TranslateText(item.Description),
                        AuthorName = item.Author,
                        Categories = item.Categories,
                        Content = item.Description,
                        Title = item.Title,
                        TitleEn = translateHelper.TranslateText(item.Title),
                        Link = item.Link,
                        CreatedTime = item.CreateTime
                    };
                    tobeAddBlogs.Add(blogForm);
                }
            }
            // 发送请求入库
            using (var hc = new HttpClient())
            {
                var response = await hc.PostAsJsonAsync("http://guandian.tech/admin/tools/AddRssBlogs", tobeAddBlogs);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    log.Info("success:" + result);
                }
                else
                {
                    log.Error(response.StatusCode + response.ReasonPhrase);
                }
            }
            log.Info("finish");
        }
    }
}
