using System;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace PushService
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async void RunAsync([TimerTrigger("0 0 9 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            using (var hc = new HttpClient())
            {
                var response = await hc.GetStringAsync("https://guandian.tech/weixin/Push/PushToWeixinAsync");
            }

            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
