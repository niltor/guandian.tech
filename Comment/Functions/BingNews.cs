using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Functions
{
    public static class BingNews
    {

        [FunctionName("GetNews")]
        public static void Run([TimerTrigger("0 0 */4 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
