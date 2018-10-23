using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MSBlogsFunction.Models;
using Newtonsoft.Json;

namespace MSBlogsFunction
{
    public static class Function1
    {
        static readonly string baseApi = "http://localhost:3719/";
        //static readonly string baseApi = "http://guandian.tech/";
        [FunctionName("MSBlogs")]
        public static async Task RunAsync([TimerTrigger("*/20 * * * * *")]TimerInfo myTimer, ILogger log)
        //public static async Task RunAsync([TimerTrigger("0 0 */6 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var blogs = await RssHelper.GetAllBlogs(log);
            var tobeAddBlogs = new List<BlogForm>();
            string subKey = Environment.GetEnvironmentVariable("SubKey");
            var translateHelper = new TranslateTextHelper(subKey);
            log.LogInformation("采集：" + blogs.Count + "条;\r\n" + string.Join(";\r\n", blogs.Select(b => b.Title).ToArray()));
            // blogs去重
            using (var hc = new HttpClient())
            {
                var response = await hc.PostAsJsonAsync(baseApi + "admin/tools/UniqueBlogs", blogs.Select(b => b.Title).ToList());
                var json = await response.Content.ReadAsStringAsync();
                var uniqueBlogs = JsonConvert.DeserializeObject<List<string>>(json);
                if (uniqueBlogs == null)
                {
                    log.LogInformation("没有新增内容");
                    return;
                }
                log.LogInformation("新增条数:" + uniqueBlogs?.Count);
                blogs = blogs.Where(b => uniqueBlogs.Any(u => b.Title.Equals(u))).ToList();
            }

            if (blogs.Count > 0)
            {
                foreach (var item in blogs)
                {
                    var blogForm = new BlogForm
                    {
                        ContentEn = item.Content,
                        AuthorName = item.Author,
                        Categories = item.Categories,
                        Content = translateHelper.TranslateText(item.Content),
                        Summary = translateHelper.TranslateText(item.Description),
                        Title = translateHelper.TranslateText(item.Title),
                        TitleEn = item.Title,
                        Link = item.Link,
                        CreatedTime = item.CreateTime
                    };
                    tobeAddBlogs.Add(blogForm);
                }
            }

            // 发送请求入库
            if (tobeAddBlogs.Count > 0)
            {
                using (var hc = new HttpClient())
                {
                    var response = await hc.PostAsJsonAsync(baseApi + "admin/tools/AddRssBlogs", tobeAddBlogs);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        log.LogInformation("success:" + result);
                    }
                    else
                    {
                        log.LogError(response.StatusCode + response.ReasonPhrase);
                    }
                }
            }
            log.LogInformation("finish");
        }
    }
}
