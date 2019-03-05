using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using GithubWebhook.Events;
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
        public GithubController(ApplicationDbContext context, IOptionsMonitor<GithubOption> options) : base(context)
        {
            _options = options.CurrentValue;
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
        public ActionResult EventHook()
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
                bodyString = reader.ReadToEnd();
            }

            if (!string.IsNullOrEmpty(signature))
            {
                if (!SignatureValid(bodyString, signature))
                {
                    return BadRequest($"签名验证错误!");
                }

                // 解析内容

            }
            else
            {
                return BadRequest("无效的签名信息！");
            }
            return Ok();
        }

        private object ConvertPayload(string eventName, string PayloadText)
        {
            switch (eventName)
            {
                case PingEvent.EventString:
                    return PingEvent.FromJson(PayloadText);
                case CommitCommentEvent.EventString:
                    return CommitCommentEvent.FromJson(PayloadText);
                case CreateEvent.EventString:
                    return CreateEvent.FromJson(PayloadText);
                case DeleteEvent.EventString:
                    return DeleteEvent.FromJson(PayloadText);
                case DeploymentEvent.EventString:
                    return DeploymentEvent.FromJson(PayloadText);
                case DeploymentStatusEvent.EventString:
                    return DeploymentStatusEvent.FromJson(PayloadText);
                case ForkEvent.EventString:
                    return ForkEvent.FromJson(PayloadText);
                case GollumEvent.EventString:
                    return GollumEvent.FromJson(PayloadText);
                case InstallationEvent.EventString:
                    return InstallationEvent.FromJson(PayloadText);
                case InstallationRepositoriesEvent.EventString:
                    return InstallationRepositoriesEvent.FromJson(PayloadText);
                case IssueCommentEvent.EventString:
                    return IssueCommentEvent.FromJson(PayloadText);
                case IssuesEvent.EventString:
                    return IssuesEvent.FromJson(PayloadText);
                case LabelEvent.EventString:
                    return LabelEvent.FromJson(PayloadText);
                case MemberEvent.EventString:
                    return MemberEvent.FromJson(PayloadText);
                case MembershipEvent.EventString:
                    return MembershipEvent.FromJson(PayloadText);
                case MilestoneEvent.EventString:
                    return MilestoneEvent.FromJson(PayloadText);
                case OrganizationEvent.EventString:
                    return OrganizationEvent.FromJson(PayloadText);
                case OrgBlockEvent.EventString:
                    return OrgBlockEvent.FromJson(PayloadText);
                case PageBuildEvent.EventString:
                    return PageBuildEvent.FromJson(PayloadText);
                case ProjectCardEvent.EventString:
                    return ProjectCardEvent.FromJson(PayloadText);
                case ProjectColumnEvent.EventString:
                    return ProjectColumnEvent.FromJson(PayloadText);
                case ProjectEvent.EventString:
                    return ProjectEvent.FromJson(PayloadText);
                case PublicEvent.EventString:
                    return PublicEvent.FromJson(PayloadText);
                case PullRequestEvent.EventString:
                    return PullRequestEvent.FromJson(PayloadText);
                case PullRequestReviewEvent.EventString:
                    return PullRequestReviewEvent.FromJson(PayloadText);
                case PullRequestReviewCommentEvent.EventString:
                    return PullRequestReviewCommentEvent.FromJson(PayloadText);
                case PushEvent.EventString:
                    return PushEvent.FromJson(PayloadText);
                case ReleaseEvent.EventString:
                    return ReleaseEvent.FromJson(PayloadText);
                case RepositoryEvent.EventString:
                    return RepositoryEvent.FromJson(PayloadText);
                case StatusEvent.EventString:
                    return StatusEvent.FromJson(PayloadText);
                case WatchEvent.EventString:
                    return WatchEvent.FromJson(PayloadText);
                default:
                    throw new NotImplementedException("");
            }
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
