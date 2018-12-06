using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MSBlogsFunction.Entity;
using MSBlogsFunction.Models;
using Newtonsoft.Json;

namespace MSBlogsFunction
{
    public static class Function1
    {
        //static readonly string baseApi = "http://localhost:3719/";
        static readonly string baseApi = "https://guandian.tech/";
        [FunctionName("MSBlogs")]
        public static async Task RunAsync([TimerTrigger("*/20 * * * * *")]TimerInfo myTimer, ILogger log)
        //public static async Task RunAsync([TimerTrigger("0 0 */6 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var helper = new RssHelper();
            var MSBlogs = helper.GetAllBlogs(log);
            await SaveMSBlogs(MSBlogs, log);

            log.LogInformation("finish");
        }

        /// <summary>
        /// 采集微软RSS博客
        /// </summary>
        /// <param name="blogs"></param>
        /// <param name="log"></param>
        static async Task SaveMSBlogs(List<RssEntity> blogs, ILogger log)
        {
            string subKey = Environment.GetEnvironmentVariable("GoogleTranslateKey");
            var translateHelper = new TranslateTextHelper(subKey);
            log.LogInformation("采集微软RSS：" + blogs.Count + "条;\r\n" + string.Join(";\r\n", blogs.Select(b => b.Title).ToArray()));
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
                blogs = blogs.Where(b => uniqueBlogs.Any(u => b.Title.Equals(u)) && b.Title != null)
                    .ToList();

                log.LogInformation("新增条数:" + uniqueBlogs?.Count + "\r\n" + string.Join(";\r\n", blogs.Select(b => b.Title).ToArray()));

                if (blogs.Count > 0)
                {
                    var intelligence = new IntelligenceHelper();
                    foreach (var item in blogs)
                    {
                        try
                        {
                            var thumbnail = await intelligence.GetImageFromTextAsync(item.Title);
                            //var thumbnail = "";

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
                                CreatedTime = item.CreateTime,
                                Thumbnail = thumbnail
                            };
                            var tobeAddBlogs = new List<BlogForm> { blogForm };
                            response = await hc.PostAsJsonAsync(baseApi + "admin/tools/AddRssBlogs", tobeAddBlogs);
                            if (response.IsSuccessStatusCode)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                log.LogInformation("成功:" + item.Title);
                            }
                            else
                            {
                                log.LogError(response.StatusCode + response.ReasonPhrase);
                            }
                        }
                        catch (Exception e)
                        {
                            log.LogError(e.Message + e.StackTrace + e.InnerException?.Message);
                        }
                    }
                }
            }
        }
    }
}
