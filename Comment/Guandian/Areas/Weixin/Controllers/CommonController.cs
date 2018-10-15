using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Comment.Data;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
namespace Comment.Areas.Weixin.Controllers
{
    [Area("Weixin")]
    public class CommonController : Controller
    {
        public static readonly string Token = Config.SenparcWeixinSetting.Token;//与微信公众账号后台的Token设置保持一致，区分大小写。
        public static readonly string EncodingAESKey = Config.SenparcWeixinSetting.EncodingAESKey;//与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
        public static readonly string AppId = Config.SenparcWeixinSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
        public static readonly string AppSecret = Config.SenparcWeixinSetting.WeixinAppSecret;//与微信公众账号后台的AppId设置保持一致，区分大小写。
        protected readonly Func<string> _getRandomFileName = () => DateTime.Now.ToString("yyyyMMdd-HHmmss") + Guid.NewGuid().ToString("n").Substring(0, 6);
        protected readonly SenparcWeixinSetting _senparcWeixinSetting;
        protected readonly ApplicationDbContext _context;


        public CommonController(IOptions<SenparcWeixinSetting> senparcWeixinSetting, ApplicationDbContext context)
        {
            _senparcWeixinSetting = senparcWeixinSetting.Value;
            _context = context;
        }
    }
}