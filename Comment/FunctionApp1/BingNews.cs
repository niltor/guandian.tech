using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;

namespace Functions
{
    public static class BingNews
    {
        [FunctionName("BingNewsAuto")]
        //public static async Task RunAsync([TimerTrigger("*/20 * * * * *")]TimerInfo myTimer, TraceWriter log)
        public static async Task RunAsync([TimerTrigger("0 0 */4 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            // 读取配置
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("local.settings.json", true)
                .Build();
            string connstr = configuration.GetConnectionString("Default");
            string searchKey = Environment.GetEnvironmentVariable("SearchKey");

            if (!string.IsNullOrEmpty(connstr))
            {
                // 获取新闻
                var keywords = new string[] { "微软", "Microsoft", "三星", "谷歌", "科技", "阿里", "英特尔", "编程" };
                var service = new NewsService(log, searchKey);
                foreach (var keyword in keywords)
                {
                    var result = await service.GetNews(keyword);
                    await service.SaveNewsAsync(connstr, result);
                }
                log.Info("finish");
            }
        }
    }
}
