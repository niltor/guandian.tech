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
        //static readonly string baseApi = "http://localhost:9843/";
        static readonly string baseApi = "https://guandian.tech/";
        [FunctionName("MSBlogs")]
        //public static async Task RunAsync([TimerTrigger("*/20 * * * * *")]TimerInfo myTimer, ILogger log)
        public static async Task RunAsync([TimerTrigger("0 0 */6 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var helper = new RssHelper();
            var MSBlogs = await helper.GetAllBlogs(log);
            await SaveMSBlogs(MSBlogs, log);

            //var techRePublicBlogs = helper.GetTechRePublicRss(log);
            //await SaveTechRePublicBlogs(techRePublicBlogs, log);

            log.LogInformation("finish");
        }



        static async Task SaveTechRePublicBlogs(List<RssEntity> blogs, ILogger log)
        {
            string subKey = Environment.GetEnvironmentVariable("GoogleTranslateKey");
            var translateHelper = new TranslateTextHelper(subKey);

            var tobeAddBlogs = new List<BlogForm>();
            log.LogInformation("采集TechRePublic RSS：" + blogs.Count + "条;\r\n" + string.Join(";\r\n", blogs.Select(b => b.Title).ToArray()));
            // blogs去重
            using (var hc = new HttpClient())
            {
                var response = await hc.PostAsJsonAsync(baseApi + "admin/tools/UniqueBlogs", blogs.Select(b => b.Title).ToList());
                if (!response.IsSuccessStatusCode)
                {
                    log.LogError("请求去重接口失败" + response.StatusCode);
                    return;
                }
                var json = await response.Content.ReadAsStringAsync();
                var uniqueBlogs = JsonConvert.DeserializeObject<List<string>>(json);
                if (uniqueBlogs == null)
                {
                    log.LogInformation("没有新增内容");
                    return;
                }
                log.LogInformation("新增条数:" + uniqueBlogs?.Count);
                //blogs = blogs.Where(b => uniqueBlogs.Any(u => b.Title.Equals(u))).ToList();
                if (blogs.Count > 0)
                {
                    foreach (var item in blogs)
                    {
                        // 获取具体内容
                        var (description, content) = RssHelper.GetTechRePublicContent(item.Link, log);
                        var blogForm = new BlogForm
                        {
                            ContentEn = content,
                            AuthorName = item.Author,
                            Categories = item.Categories,
                            Content = await translateHelper.GetTranslateByGoogle(content),
                            Summary = await translateHelper.GetTranslateByGoogle(description),
                            Title = await translateHelper.GetTranslateByGoogle(item.Title),
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
                    response = await hc.PostAsJsonAsync(baseApi + "admin/tools/AddRssBlogs", tobeAddBlogs);
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
        }

        /// <summary>
        /// 采集微软RSS博客
        /// </summary>
        /// <param name="blogs"></param>
        /// <param name="log"></param>
        static async Task SaveMSBlogs(List<RssEntity> blogs, ILogger log)
        {
            string subKey = Environment.GetEnvironmentVariable("SubKey");
            var translateHelper = new TranslateTextHelper(subKey);
            var tobeAddBlogs = new List<BlogForm>();
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
                log.LogInformation("新增条数:" + uniqueBlogs?.Count);
                blogs = blogs.Where(b => uniqueBlogs.Any(u => b.Title.Equals(u))).ToList();

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
                    response = await hc.PostAsJsonAsync(baseApi + "admin/tools/AddRssBlogs", tobeAddBlogs);
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
        }
    }
}
