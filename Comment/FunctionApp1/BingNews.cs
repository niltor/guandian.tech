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
        public static async Task RunAsync([TimerTrigger("0 0 */4 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            // 读取配置
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("local.settings.json", true)
                .Build();
            string connstr = configuration.GetConnectionString("Default");

            if (!string.IsNullOrEmpty(connstr))
            {
                // 获取新闻
                var keywords = new string[] { "微软", "Microsoft" };
                var service = new NewsService(log);
                var result = await service.GetNews("Microsoft");
                var result1 = await service.GetNews("微软");

                // 处理并入库新闻
                service.SaveNews(connstr, result);
                service.SaveNews(connstr, result1);
                log.Info("finish");
            }
        }
    }
}
