using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Comment.Data;
using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.HttpUtility;
using Senparc.Weixin.Entities;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Menu;

namespace Comment.Areas.Weixin.Controllers
{
    /// <summary>
    /// 菜单操作参考类
    /// </summary>
    public class MenuController : CommonController
    {
        public MenuController(IOptions<SenparcWeixinSetting> senparcWeixinSetting, ApplicationDbContext context) : base(senparcWeixinSetting, context)
        {
        }
        #region 获取IP
        private static string IP { get; set; }

        /// <summary>
        /// 获得当前服务器外网IP
        /// </summary>
        private string GetIP()
        {
            try {
                if (!string.IsNullOrEmpty(IP)) {
                    return IP;
                }

                var url =
                    "https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=0&rsv_idx=1&tn=baidu&wd=IP&rsv_pq=db4eb7d40002dd86&rsv_t=14d7uOUvNnTdrhnrUx0zdEVTPEN8XDq4aH7KkoHAEpTIXkRQkUD00KJ2p94&rqlang=cn&rsv_enter=1&rsv_sug3=2&rsv_sug1=2&rsv_sug7=100&rsv_sug2=0&inputT=875&rsv_sug4=875";

                var htmlContent = RequestUtility.HttpGet(url, cookieContainer: null);
                var result = Regex.Match(htmlContent, @"(?<=本机IP:[^\d+]*)(\d+\.\d+\.\d+\.\d+)(?=</span>)");
                if (result.Success) {
                    IP = result.Value;
                }
                return IP;
            }
            catch {
                return null;
            }
        }
        #endregion

        public ActionResult Index()
        {
            GetMenuResult result = new GetMenuResult(new ButtonGroup());

            //初始化
            for (int i = 0; i < 3; i++) {
                var subButton = new SubButton();
                for (int j = 0; j < 5; j++) {
                    var singleButton = new SingleClickButton();
                    subButton.sub_button.Add(singleButton);
                }
            }

            //获取服务器外网IP
            ViewData["IP"] = GetIP() ?? "使用CMD命令ping sdk.weixin.senparc.com";

            return View(result);
        }

        public ActionResult InitMenu()
        {
            var token = AccessTokenContainer.TryGetAccessToken(AppId, AppSecret);
            var buttonGroups = new ButtonGroup();
            // 定义按钮
            buttonGroups.button.Add(
                new SingleViewButton {
                    name = "我的订单",
                    type = "click",
                    url = "http://beijichong.guandian.tech/weixin/auth/callbackasync?redirectPath=/weixin/home/OrderList"
                });
            var result = CommonApi.CreateMenu(token, buttonGroups);
            return Json(result.errmsg);
        }

        public ActionResult GetToken(string appId, string appSecret)
        {
            try {
                var result = AccessTokenContainer.TryGetAccessToken(appId, appSecret);
                return Json(result, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
            catch (ErrorJsonResultException ex) {
                return Json(new { error = "API 调用发生错误：{0}".FormatWith(ex.JsonResult.ToJson()) }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex) {
                return Json(new { error = "执行过程发生错误：{0}".FormatWith(ex.Message) }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
        }

        [HttpPost]
        public ActionResult CreateMenu(string token, GetMenuResultFull resultFull, MenuMatchRule menuMatchRule)
        {
            var useAddCondidionalApi = menuMatchRule != null && !menuMatchRule.CheckAllNull();
            var apiName = string.Format("使用接口：{0}。", (useAddCondidionalApi ? "个性化菜单接口" : "普通自定义菜单接口"));
            try {
                if (token.IsNullOrEmpty()) {
                    throw new WeixinException("Token不能为空！");
                }

                //重新整理按钮信息
                WxJsonResult result = null;
                IButtonGroupBase buttonGroup = null;
                if (useAddCondidionalApi) {
                    //个性化接口
                    buttonGroup = CommonApi.GetMenuFromJsonResult(resultFull, new ConditionalButtonGroup()).menu;

                    var addConditionalButtonGroup = buttonGroup as ConditionalButtonGroup;
                    addConditionalButtonGroup.matchrule = menuMatchRule;
                    result = CommonApi.CreateMenuConditional(token, addConditionalButtonGroup);
                    apiName += string.Format("menuid：{0}。", (result as CreateMenuConditionalResult).menuid);
                }
                else {
                    //普通接口
                    buttonGroup = CommonApi.GetMenuFromJsonResult(resultFull, new ButtonGroup()).menu;
                    result = CommonApi.CreateMenu(token, buttonGroup);
                }

                var json = new {
                    Success = result.errmsg == "ok",
                    Message = "菜单更新成功。" + apiName
                };
                return Json(json, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex) {
                var json = new { Success = false, Message = string.Format("更新失败：{0}。{1}", ex.Message, apiName) };
                return Json(json, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
        }

        [HttpPost]
        public ActionResult CreateMenuFromJson(string token, string fullJson)
        {
            //TODO:根据"conditionalmenu"判断自定义菜单

            var apiName = "使用JSON更新";
            try {
                GetMenuResultFull resultFull = Newtonsoft.Json.JsonConvert.DeserializeObject<GetMenuResultFull>(fullJson);

                //重新整理按钮信息
                WxJsonResult result = null;
                IButtonGroupBase buttonGroup = null;

                buttonGroup = CommonApi.GetMenuFromJsonResult(resultFull, new ButtonGroup()).menu;
                result = CommonApi.CreateMenu(token, buttonGroup);

                var json = new {
                    Success = result.errmsg == "ok",
                    Message = "菜单更新成功。" + apiName
                };
                return Json(json, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex) {
                var json = new { Success = false, Message = string.Format("更新失败：{0}。{1}", ex.Message, apiName) };
                return Json(json, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
        }

        public ActionResult GetMenu(string token)
        {
            try {
                var result = CommonApi.GetMenu(token);
                if (result == null) {
                    return Json(new { error = "菜单不存在或验证失败！" }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
                }
                return Json(result, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
            catch (WeixinMenuException ex) {
                return Json(new { error = "菜单不存在或验证失败：" + ex.Message }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex) {
                return Json(new { error = "菜单不存在或验证失败：" + ex.Message }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
        }

        public ActionResult DeleteMenu(string token)
        {
            try {
                var result = CommonApi.DeleteMenu(token);
                var json = new {
                    Success = result.errmsg == "ok",
                    Message = result.errmsg
                };
                return Json(json, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex) {
                var json = new { Success = false, Message = ex.Message };
                return Json(json, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
        }

    }
}