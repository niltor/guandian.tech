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

            var service = new NewsService();
            var result = await service.GetNews("微软");

            Console.WriteLine("finish");
        }
    }
}
