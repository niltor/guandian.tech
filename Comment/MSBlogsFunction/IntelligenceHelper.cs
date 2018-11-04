using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Rest;

namespace MSBlogsFunction
{
    /// <summary>
    /// 智能服务帮助
    /// </summary>
    public class IntelligenceHelper
    {
        readonly string TextKey;
        readonly string ImageKey;
        public IntelligenceHelper()
        {
            TextKey = Environment.GetEnvironmentVariable("TextKey");
            ImageKey = Environment.GetEnvironmentVariable("SearchKey");
        }
        /// <summary>
        /// 根据文本获取对就关联的图片
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<string> GetImageFromTextAsync(string content)
        {
            // 调用文本分析提取关键词
            var textClient = new TextAnalyticsClient(new ApiKeyServiceClientCredentials(TextKey))
            {
                Endpoint = "https://eastasia.api.cognitive.microsoft.com"
            };
            // 根据关键词查询图片
            var result = await textClient.KeyPhrasesAsync(new MultiLanguageBatchInput(new List<MultiLanguageInput>()
            {
                new MultiLanguageInput()
                {
                    Id ="id",
                    Text = content,
                    Language ="en"
                }
            }));

            foreach (var keyword in result.Documents[0].KeyPhrases)
            {
                var imageClient = new ImageSearchAPI(new ApiKeyServiceClientCredentials(ImageKey));
                var imageResults = await imageClient.Images.SearchAsync(query: keyword, acceptLanguage: "EN", color: "ColorOnly", freshness: "Month", count: 5, size: "Medium");
                if (imageResults.Value?.Count > 0)
                {
                    var firstImageResult = imageResults.Value?.First();
                    return firstImageResult.ContentUrl;
                }
                else
                {
                    continue;
                }
            }
            return default;
        }
    }
    /// <summary>
    /// ApiKeyServiceClientCredentials
    /// </summary>
    public class ApiKeyServiceClientCredentials : ServiceClientCredentials
    {
        readonly string _subscriptionKey;
        public ApiKeyServiceClientCredentials(string key)
        {
            _subscriptionKey = key;
        }
        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }
}
