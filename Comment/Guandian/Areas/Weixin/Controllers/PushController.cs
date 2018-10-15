using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Guandian.Areas.Weixin.Controllers;
using Guandian.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using Senparc.Weixin.MP.Containers;

namespace Guandian.Areas.Weixin.Controllers
{
    /// <summary>
    /// 微信图文消息推送
    /// </summary>
    public class PushController : CommonController
    {
        public PushController(IOptions<SenparcWeixinSetting> senparcWeixinSetting, ApplicationDbContext context) : base(senparcWeixinSetting, context)
        {

        }

        public async System.Threading.Tasks.Task<ActionResult> PushToWeixinAsync()
        {

            // 获取原内容,当天
            var news = _context.News
                .Where(n => n.IsPublishToMP == false && n.CreatedTime.Date >= DateTime.Now.Date.AddDays(-3))
                .OrderByDescending(n => n.CreatedTime)
                .Take(6)
                .ToList();

            if (news != null && news.Count > 0)
            {
                var token = AccessTokenContainer.TryGetAccessToken(AppId, AppSecret);
                string content = "<h6>TechViews今日资讯一览!</h6>";

                // 上传文章内图片
                using (var wc = new WebClient())
                {

                    var firstNews = news.FirstOrDefault();
                    foreach (var item in news)
                    {
                        wc.DownloadFile(item.ThumbnailUrl + "&w=600", "temp.jpg");
                        var uploadImgResult = await MediaApi.UploadImgAsync(token, "temp.jpg");
                        System.IO.File.Delete("temp.jpg");

                        // 替换图片链接
                        content += $@"<div class='row news-list bg-white m-0 mb-2 p-2'>
        <div class='content col-md-8'>
            <div class='news-title my-1'>
                <h5>
                    <strong>{item.Title}</strong>
                </h5>
            </div>
            <div>
                <img src='{uploadImgResult.url}' width='100%' />
            </div>
            <div class='news-description'>{item.Description}</div>
        </div>
        <br />
    </div>";
                        item.IsPublishToMP = true;
                    }
                    _context.UpdateRange(news);
                    _context.SaveChanges();
                    // 上传封面图片
                    wc.DownloadFile(firstNews?.ThumbnailUrl + "&w=600", "temp.jpg");
                    var thumbImg = await MediaApi.UploadForeverMediaAsync(token, "temp.jpg");
                    // 上传图文
                    var newsList = new List<NewsModel>
                    {
                        new NewsModel
                        {
                            author = "MSDev_NilTor",
                            thumb_media_id = thumbImg.media_id,
                            content = content,
                            title = "资讯一览:"+firstNews?.Title,
                            show_cover_pic = "0",
                            content_source_url = "https://guandian.tech",
                            digest = firstNews.Description,
                        }
                    };
                    var uploadNewsResult = await MediaApi.UploadNewsAsync(token, news: newsList.ToArray());
                    if (uploadNewsResult.media_id != null)
                    {
                        var userList = await UserApi.GetAsync(token, null);
                        var firstUserOpenId = userList.data.openid.FirstOrDefault();
                        // 群发消息
                        var sendNewsResult = await GroupMessageApi.SendGroupMessageByTagIdAsync(token, null, uploadNewsResult.media_id, Senparc.Weixin.MP.GroupMessageType.mpnews, true);
                    }
                }
            }
            return Ok();
        }
    }
}