using System;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace PushService
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async void RunAsync([TimerTrigger("0 30 0 * * *")]TimerInfo myTimer, TraceWriter log)
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
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
