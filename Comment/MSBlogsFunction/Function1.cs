using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace MSBlogsFunction
{
    public static class Function1
    {
        [FunctionName("MSBlogs")]
        public static async Task RunAsync([TimerTrigger("*/20 * * * * *")]TimerInfo myTimer, TraceWriter log)

        //public static async Task RunAsync([TimerTrigger("0 * */6 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            var blogs = await RssHelper.GetDevBlogs();

            // 读取配置

            //string connstr = configuration.GetConnectionString("Default");
            string subKey = Environment.GetEnvironmentVariable("SubKey");

            var translateHelper = new TranslateTextHelper(subKey);
            var result = translateHelper.TranslateText(blogs.First().Description);

            //log.Info(result);

            log.Info("finish");
        }
    }
}
