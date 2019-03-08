using System;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace PushService
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async void RunAsync([TimerTrigger("0 20 23 * * *")]TimerInfo myTimer, ILogger log)
        {
            int retryNum = 3;
            using (var hc = new HttpClient())
            {
                var response = await hc.GetAsync("https://guandian.tech/weixin/Push/PushToWeixin");
                while (retryNum > 0 && !response.IsSuccessStatusCode)
                {
                    retryNum--;
                    response = await hc.GetAsync("https://guandian.tech/weixin/Push/PushToWeixin");
                }
            }
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
