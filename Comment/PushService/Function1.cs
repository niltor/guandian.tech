using System;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace PushService
{
    public static class Function1
    {
        [FunctionName("PushWeixinService")]
        public static async void RunAsync([TimerTrigger("*/20 * * * * *")]TimerInfo myTimer, ILogger log)
        //public static async void RunAsync([TimerTrigger("0 20 23 * * *")]TimerInfo myTimer, ILogger log)
        {
            int retryNum = 3;
            using (var hc = new HttpClient())
            {
                var response = await hc.GetAsync("https://guandian.tech/weixin/Push/PushToWeixinAsync");
                while (retryNum > 0 && !response.IsSuccessStatusCode)
                {
                    retryNum--;
                    response = await hc.GetAsync("https://guandian.tech/weixin/Push/PushToWeixinAsync");
                }
            }
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
