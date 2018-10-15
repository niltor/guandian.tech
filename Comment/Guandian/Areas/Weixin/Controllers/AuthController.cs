using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Comment.Data;
using Comment.Data.Entity;
using Comment.Services;
using Senparc.CO2NET.HttpUtility;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MvcExtension;

namespace Comment.Areas.Weixin.Controllers
{
    public class AuthController : CommonController
    {
        public AuthController(IOptions<SenparcWeixinSetting> senparcWeixinSetting, ApplicationDbContext context) : base(senparcWeixinSetting, context)
        {
        }

        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://sdk.weixin.senparc.com/weixin
        /// </summary>
        [HttpGet]
        [ActionName("Index")]
        public IActionResult Get(PostModel postModel, string echostr)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token)) {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else {
                return Content("failed:" + postModel.Signature + "," + Senparc.Weixin.MP.CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, Token));
            }
        }

        /// <summary>
        /// 回调地址
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CallbackAsync(string code, string redirectPath)
        {
            string openId = string.Empty;
            if (code == null) {
                return Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx1777893e5533714e&redirect_uri=http://beijichong.guandian.tech/weixin/auth/callbackasync?redirectPath="
                    + redirectPath
                    + "&response_type=code&scope=snsapi_base&state=123#wechat_redirect");
            }
            else {
                var OAuthResult = await OAuthApi.GetAccessTokenAsync(AppId, AppSecret, code);
                openId = OAuthResult.openid;
                // 身份或Session?
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,openId ),
                    new Claim(ClaimTypes.Role, "Weixin"),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                ViewData["openId"] = openId;

                return Redirect(redirectPath);
            }
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult<string> GetAccessToken()
        {
            string accesstoken = "";
            // TODO: 时间间隔

            //var accessToken = _context.SimpleConfigs.Where(c => c.Name.Equals("WeixinAccessToken"))
            //    .FirstOrDefault();
            //var expiredTime = _context.SimpleConfigs.Where(c => c.Name.Equals("WeixinExpiredTime"))
            //    .FirstOrDefault();
            //// 初始状态
            //if (accessToken == null || expiredTime == null) {
            //    //根据appId、appSecret获取
            //    string access_token = AccessTokenContainer.TryGetAccessToken(AppId, AppSecret);
            //    _context.SimpleConfigs.AddRange(
            //        new SimpleConfig {
            //            Name = "WeixinExpiredTime",
            //            Value = DateTime.Now.ToString(),
            //            Type = "Weixin"
            //        },
            //        new SimpleConfig {
            //            Name = "WeixinAccessToken",
            //            Value = access_token,
            //            Type = "Weixin"
            //        }
            //    );
            //    _context.SaveChanges();
            //    accesstoken = access_token;
            //}
            //else {
            //    // 需要更新
            //    var _expiredTime = Convert.ToDateTime(expiredTime.Value);
            //    if ((DateTime.Now - _expiredTime).TotalMinutes >= 100) {
            //        string access_token = AccessTokenContainer.TryGetAccessToken(AppId, AppSecret);
            //        accessToken.Value = access_token;
            //        expiredTime.Value = (DateTime.Now + TimeSpan.FromMinutes(100)).ToString();
            //        _context.SimpleConfigs.Update(accessToken);
            //        _context.SimpleConfigs.Update(expiredTime);
            //        accesstoken = access_token;
            //    }
            //    else {
            //        accesstoken = accessToken.Value;
            //    }
            //}

            return accesstoken;
        }

        /// <summary>
        /// 最简化的处理流程（不加密）
        /// </summary>
        [HttpPost]
        [ActionName("Index")]
        public IActionResult MiniPost(PostModel postModel)
        {
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token)) {
                //return Content("参数错误！");//v0.7-
                return new WeixinResult("参数错误！");//v0.8+
            }
            postModel.Token = Token;
            postModel.EncodingAESKey = EncodingAESKey;//根据自己后台的设置保持一致
            postModel.AppId = AppId;//根据自己后台的设置保持一致
            var messageHandler = new CustomMessageHandler(Request.GetRequestMemoryStream(), postModel);
            messageHandler.Execute();//执行微信处理过程
                                     //return Content(messageHandler.ResponseDocument.ToString());//v0.7-
                                     //return new WeixinResult(messageHandler);//v0.8+
            return new FixWeixinBugWeixinResult(messageHandler);//v0.8+
        }

    }
}