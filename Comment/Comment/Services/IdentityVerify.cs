using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Comment.Services
{
    public class IdentityVerify
    {
        /// <summary>
        /// 身份识别
        /// </summary>
        private readonly string recognitionApi = "http://jisusfzsb.market.alicloudapi.com/idcardrecognition/recognize";
        /// <summary>
        /// 身份一致性验证
        /// </summary>
        private readonly string verifyIdentity = "https://checkid.market.alicloudapi.com/IDCard";
        /// <summary>
        /// 验证码
        /// </summary>
        private readonly string appCode;
        public IConfiguration Configuration { get; }

        public IdentityVerify(IConfiguration configuration)
        {
            Configuration = configuration;
            appCode = configuration["appcode"];
        }
        /// <summary>
        /// 身份识别
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public async Task RecognitionIdentityAsync(string filePath)
        {
            using (var hc = new HttpClient())
            {
                // 设置请求头
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "APPCODE" + appCode);
                // 构造请求体
                var file = File.ReadAllBytes(filePath);
                var body = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("pic", Convert.ToBase64String(file))
                };
                var response = await hc.PostAsync(recognitionApi + "?typeid=2", new FormUrlEncodedContent(body));

                Console.WriteLine(response.Content);
            }
        }
        /// <summary>
        /// 身份一致性验证
        /// </summary>
        /// <returns></returns>
        public async Task VerifyIdentityAsync(string name, string identity)
        {
            using (var hc = new HttpClient())
            {
                // 设置请求头
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "APPCODE" + appCode);
                var result = await hc.GetStringAsync(recognitionApi + $"?typeid=2&name={name}&idCard={identity}");
                Console.WriteLine(result);
            }
        }
    }
}
