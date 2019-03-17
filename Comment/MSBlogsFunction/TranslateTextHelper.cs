using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Translation.V2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MSBlogsFunction
{
    public class TranslateTextHelper
    {
        public string MSKey { get; set; }
        public string SubScriptKey { get; set; }
        public TranslateTextHelper(string subscriptKey)
        {
            SubScriptKey = subscriptKey;
        }

        /// <summary>
        /// 文本翻译
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string TranslateText(string content, Provider provider = Provider.Default)
        {
            if (string.IsNullOrEmpty(content))
            {
                return "";
            }
            string seperator = "<!--divider-->";
            // TODO:拆分算法待优化通用。根据p标签的数量和长度去拆分。
            int maxNumber = 4800;
            if (content.Length > maxNumber)
            {
                // 插入分隔符
                content = addSeperator(content, 0);
            }
            // 内部方法，添加分隔符
            string addSeperator(string str, int tagLevel)
            {
                if (tagLevel > 5) return str;
                var tags = new string[] { "</h1>", "</h2>", "</h3>", "</h4>", "</h5>", "<p></p>" };
                var result = "";
                // 以标题标签分隔
                var tag = tags[tagLevel];
                if (str.Contains(tag))
                {
                    str = str.Replace(tag, tag + seperator);
                    var contentParts = str.Split(new string[] { tag }, StringSplitOptions.None);
                    foreach (var item in contentParts)
                    {
                        var row = item + tag;
                        if (row.Length >= maxNumber)
                        {
                            row = addSeperator(row, tagLevel + 1);
                        }
                        result += row;
                    }
                    return result;
                }
                else
                {
                    result = addSeperator(str, tagLevel + 1);
                }
                return result;
            }

            var contentArray = content.Split(new string[] { seperator }, StringSplitOptions.None);
            string translation = "";//最后翻译结果
            foreach (var item in contentArray)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;

                if (provider == Provider.Microsoft)
                {
                    translation += GetTranslateAsync(item).Result;
                }
                else if (provider == Provider.Google)
                {
                    translation += GetTranslateByGoogle(item).Result;
                }
                else
                {
                    translation += GetTranslateByWeb(item).Result;
                }
            }
            return translation;
        }

        /// <summary>
        /// 使用智能感知翻译API
        /// </summary>
        /// <param name="content"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public async Task<string> GetTranslateAsync(string content, string from = "en", string to = "zh-Hans")
        {
            string result = "";
            string uri = "https://api.cognitive.microsofttranslator.com/translate?api-version=3.0" + "&from=" + from + "&to=" + to;
            uri += "&textType=html";

            object[] body = new object[] { new { Text = content } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(uri);
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    request.Headers.Add("Ocp-Apim-Subscription-Key", MSKey);
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        JArray translations = JsonConvert.DeserializeObject<JArray>(responseBody);
                        result = translations.First["translations"]?.First["text"]?.ToString();
                    }
                    return result;

                }
            }
        }
        /// <summary>
        /// 谷歌翻译api，太贵，暂停使用
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<string> GetTranslateByGoogle(string content)
        {
            if (content == null)
            {
                return default;
            }
            try
            {
                var client = TranslationClient.CreateFromApiKey(SubScriptKey);
                var response = await client.TranslateHtmlAsync(content, LanguageCodes.ChineseSimplified);
                return response.TranslatedText;
            }
            catch (Exception)
            {
                Console.WriteLine("Error，长度：" + content.Length + "，内容:" + content);
                return "";
            }

        }
        /// <summary>
        /// 谷歌翻译web，有不稳定风险
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<string> GetTranslateByWeb(string content)
        {
            if (content == null) return default;
            string baseUrl = "https://translate.google.com/translate_a/single?client=gtx&sl=en&tl=zh-CN&dt=t";
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(baseUrl);
                    var requestParams = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("q", content)
                        };
                    request.Content = new FormUrlEncodedContent(requestParams);
                    string responseBody = "";
                    try
                    {
                        var response = await client.SendAsync(request);
                        if (response.IsSuccessStatusCode)
                        {
                            responseBody = await response.Content.ReadAsStringAsync();
                            responseBody = responseBody.Replace("</ ", "</");
                            JArray jsonArray = JsonConvert.DeserializeObject<JArray>(responseBody);
                            if (jsonArray[0].HasValues)
                            {
                                string result = "";
                                foreach (var item in jsonArray[0])
                                {
                                    result += item[0];
                                }
                                return result;
                            }
                        }
                        return default;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error ! 原：" + responseBody);
                    }
                }
            }
            return default;
        }
        public enum Provider
        {
            Microsoft,
            Google,
            Default
        }
    }

}
