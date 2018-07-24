using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace Functions
{
    public static class BingNews
    {
        static readonly string postUrl = "http://localhost:5000/api/BingNews/multi";

        [FunctionName("GetNews")]
        public static async Task RunAsync([TimerTrigger("*/20 * * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            var keywords = new string[] { "微软", "Microsoft" };
            var service = new NewsService(log);
            var result = await service.GetNews("Microsoft");
            var result1 = await service.GetNews("微软");
            using (var hc = new HttpClient())
            {
                await hc.PostAsync(postUrl, new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json"));
                await hc.PostAsync(postUrl, new StringContent(JsonConvert.SerializeObject(result1), Encoding.UTF8, "application/json"));
            }

            Console.WriteLine("finish");
        }
    }
}
