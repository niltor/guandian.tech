using System;
using System.Net.Http;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Functions
{
    public static class BingNews
    {
        [FunctionName("BingNewsAuto")]
        public static async void RunAsync([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("local.settings.json", true)
                .Build();

            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            string postUrl = Environment.GetEnvironmentVariable("API_NEWS_POST", EnvironmentVariableTarget.Process);

            if (!string.IsNullOrEmpty(postUrl))
            {
                var keywords = new string[] { "微软", "Microsoft" };
                var service = new NewsService(log);
                var result = await service.GetNews("Microsoft");
                var result1 = await service.GetNews("微软");
                using (var hc = new HttpClient())
                {
                    await hc.PostAsync(postUrl, new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json"));
                    await hc.PostAsync(postUrl, new StringContent(JsonConvert.SerializeObject(result1), Encoding.UTF8, "application/json"));
                }

                log.Info("finish");
            }
        }
    }
}
