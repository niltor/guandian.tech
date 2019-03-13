using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using GithubWebhook.Events;
using Guandian.Areas.Webhooks.Manager;
using Guandian.Data;
using Guandian.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Guandian.Areas.Webhooks.Controllers
{
    public class GithubController : BaseController
    {

        private readonly GithubOption _options;
        readonly GithubEventManager _eventManager;
        public GithubController(ApplicationDbContext context, IOptionsMonitor<GithubOption> options, GithubEventManager eventManager) : base(context)
        {
            _options = options.CurrentValue;
            _eventManager = eventManager;
        }
        [HttpGet("test")]
        public string Test()
        {
            return "test";
        }

        /// <summary>
        /// 解析处理event
        /// </summary>
        /// <returns></returns>
        [HttpPost("event_hook")]
        public async System.Threading.Tasks.Task<ActionResult> EventHookAsync()
        {
            var request = HttpContext.Request;
            request.Headers.TryGetValue("X-GitHub-Event", out var eventName);
            request.Headers.TryGetValue("X-Hub-Signature", out var signature);
            request.Headers.TryGetValue("X-GitHub-Delivery", out var delivery);
            request.Headers.TryGetValue("content-type", out var contentType);

            // 验证
            if (contentType != "application/json")
            {
                return BadRequest("错误的content-type.仅接受application/json");
            }
            string bodyString = "";
            using (var reader = new StreamReader(request.Body, Encoding.UTF8))
            {
                bodyString = await reader.ReadToEndAsync();
            }

            if (!string.IsNullOrEmpty(signature))
            {
                //if (!SignatureValid(bodyString, signature))
                //{
                //    return BadRequest($"签名验证错误!");
                //}

                // 解析内容
                await _eventManager.HandleEventAsync(eventName, bodyString);
            }
            else
            {
                return BadRequest("无效的签名信息！");
            }
            return Ok();
        }
        private bool SignatureValid(string payloadText, string signature)
        {
            return ValidateSignature(payloadText, signature, _options.WebHookSecret) == signature;
        }

        private static string ValidateSignature(string payload, string signatureWithPrefix, string secret)
        {
            if (!signatureWithPrefix.StartsWith("sha1=", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Invalid shaPrefix");

            var secretBytes = Encoding.UTF8.GetBytes(secret);
            var payloadBytes = Encoding.UTF8.GetBytes(payload);

            using (var hmSha1 = new HMACSHA1(secretBytes))
            {
                var hash = hmSha1.ComputeHash(payloadBytes);

                return $"sha1={ToHexString(hash)}";
            }
        }
        private static string ToHexString(IReadOnlyCollection<byte> bytes)
        {
            var builder = new StringBuilder(bytes.Count * 2);
            foreach (var b in bytes) builder.AppendFormat("{0:x2}", b);

            return builder.ToString();
        }
    }
}
