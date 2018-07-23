using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Functions
{
    public static class BingNews
    {

        [FunctionName("GetNews")]
        public static async Task RunAsync([TimerTrigger("*/20 * * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            var keywords = new string[] { "微软", "Microsoft" };
            var service = new NewsService(log);
            var result = await service.GetNews("Microsoft");

            Console.WriteLine("finish");
        }
    }
}
