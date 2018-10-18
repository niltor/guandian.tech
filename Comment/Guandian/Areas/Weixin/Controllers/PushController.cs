using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Guandian.Data;
using Guandian.Utilities;
using HtmlAgilityPack;
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

        /// <summary>
        /// 推送到微信公共号
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> PushToWeixinAsync()
        {
            // 获取新闻原内容
            var news = _context.News
                .Where(n => n.IsPublishToMP == false && n.CreatedTime.Date >= DateTime.Now.Date.AddDays(-3))
                .OrderByDescending(n => n.CreatedTime)
                .Take(8)
                .ToList();

            var blogs = _context.Blogs
                .Where(b => b.IsPublishMP == false && b.UpdatedTime.Date >= DateTime.Now.Date.AddDays(-3))
                .OrderByDescending(b => b.UpdatedTime)
                .Take(2)
                .ToList();

            var newsList = new List<NewsModel>();
            var token = AccessTokenContainer.TryGetAccessToken(AppId, AppSecret);

            DumpConsole("开始处理新闻");
            try
            {
                // 新闻内容
                if (news != null && news.Count > 0)
                {
                    string content = "<h6>TechViews今日资讯一览!</h6>";
                    // 上传文章内图片
                    using (var wc = new WebClient())
                    {
                        var firstNews = news.FirstOrDefault();
                        foreach (var item in news)
                        {
                            var tempFileName = StringTools.GetTempFileName("jpg");
                            wc.DownloadFile(item.ThumbnailUrl + "&w=600", tempFileName);
                            var uploadImgResult = await MediaApi.UploadImgAsync(token, tempFileName);
                            System.IO.File.Delete(tempFileName);

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

                        // 上传封面图片
                        wc.DownloadFile(firstNews?.ThumbnailUrl + "&w=600", "temp.jpg");
                        var thumbImg = await MediaApi.UploadForeverMediaAsync(token, "temp.jpg");
                        // TODO:上传后可删除
                        var mainNews = new NewsModel
                        {
                            author = "MSDev_NilTor",
                            thumb_media_id = thumbImg.media_id,
                            content = content,
                            title = "资讯一览:" + firstNews?.Title,
                            show_cover_pic = "0",
                            content_source_url = "https://guandian.tech",
                            digest = "",
                        };
                        newsList.Add(mainNews);
                    }
                }
                DumpConsole("新闻处理完成");
                DumpConsole("开始处理博客");
                // 博客内容
                if (blogs != null && blogs.Count > 0)
                {
                    using (var wc = new WebClient())
                    {
                        var htmlDoc = new HtmlDocument();
                        try
                        {
                            foreach (var item in blogs)
                            {
                                // 处理文章内图片
                                htmlDoc.LoadHtml(item.Content);
                                var imageNodes = htmlDoc.DocumentNode.SelectNodes(".//img");

                                string mediaId = "";
                                if (imageNodes != null)
                                {
                                    var images = imageNodes.Select(img => img.GetAttributeValue("src", null))
                                        .ToList();
                                    images = images.Where(i => i != null && i.ToLower().EndsWith(".jpg"))
                                        .ToList();

                                    // 获取默认封面
                                    if (images == null || images?.Count < 1)
                                    {
                                        var mediaResult = await MediaApi.GetOthersMediaListAsync(token, Senparc.Weixin.MP.UploadMediaFileType.image, 0, 2);
                                        mediaId = mediaResult.item?.FirstOrDefault()?.media_id;
                                    }
                                    else
                                    {
                                        DumpConsole(item.Title + " 图片处理");
                                        var coverImage = "";
                                        foreach (var image in images)
                                        {
                                            var tempFileName = StringTools.GetTempFileName("jpg");
                                            wc.DownloadFile(image, tempFileName);
                                            // 判断大小. TODO:处理图片大小
                                            var file = new FileInfo(tempFileName);
                                            if (file.Length > 1 * 1024 * 1024) continue;
                                            var uploadImgResult = await MediaApi.UploadImgAsync(token, tempFileName);
                                            DumpConsole("上传图片" + image + " " + file.Length / 1024);
                                            System.IO.File.Delete(tempFileName);
                                            // 替换文本
                                            item.Content.Replace(image, uploadImgResult.url);
                                            coverImage = image;
                                        }
                                        // 设置封面
                                        if (!string.IsNullOrEmpty(coverImage))
                                        {
                                            wc.DownloadFile(coverImage, "temp.jpg");
                                            var uploadCoverResult = await MediaApi.UploadForeverMediaAsync(token, "temp.jpg");
                                            mediaId = uploadCoverResult.media_id;
                                        }
                                        else
                                        {
                                            var mediaResult = await MediaApi.GetOthersMediaListAsync(token, Senparc.Weixin.MP.UploadMediaFileType.image, 0, 2);
                                            mediaId = mediaResult.item?.FirstOrDefault()?.media_id;
                                        }
                                        DumpConsole("设置封面，mediaId" + mediaId);
                                        // TODO:后面可删除该封面
                                    }

                                }
                                else
                                {
                                    var mediaResult = await MediaApi.GetOthersMediaListAsync(token, Senparc.Weixin.MP.UploadMediaFileType.image, 0, 2);
                                    mediaId = mediaResult.item?.FirstOrDefault()?.media_id;
                                }

                                // 处理内容，微信消息最大长度为2W字符，小于1M
                                Console.WriteLine("长度:" + item.Content.Length);
                                if (item.Content.Length >= 20000)
                                {
                                    item.Content = item.Content.Substring(0, 19500);
                                }

                                // 构造图文消息体
                                var currentNews = new NewsModel
                                {
                                    author = item.AuthorName,
                                    thumb_media_id = mediaId,
                                    content = item.Content,
                                    title = item.Title,
                                    show_cover_pic = "0",
                                    content_source_url = "https://guandian.tech/blogs/detail/" + item.Id,
                                    digest = "",
                                };
                                item.IsPublishMP = true;
                                newsList.Add(currentNews);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
                // 上传图文
                var uploadNewsResult = await MediaApi.UploadNewsAsync(token, news: newsList.ToArray());
                if (uploadNewsResult.media_id != null)
                {
                    var userList = await UserApi.GetAsync(token, null);
                    var firstUserOpenId = userList.data.openid.FirstOrDefault();
                    // 预览
                    var sendPreviewResult = await GroupMessageApi.SendGroupMessagePreviewAsync(token, Senparc.Weixin.MP.GroupMessageType.mpnews, uploadNewsResult.media_id, null, "EstNil");
                    // 群发消息
                    var sendNewsResult = await GroupMessageApi.SendGroupMessageByTagIdAsync(token, null, uploadNewsResult.media_id, Senparc.Weixin.MP.GroupMessageType.mpnews, true);
                    // 未成功则删除上传的素材
                    if (sendNewsResult.errcode != 0)
                    {
                        var deleteResult = await MediaApi.DeleteForeverMediaAsync(token, uploadNewsResult.media_id);
                    }
                    else
                    {
                        // 成功后更新数据库标识 
                        _context.UpdateRange(news);
                        _context.UpdateRange(blogs);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                DumpConsole(e.Message + e.InnerException);
                throw;
            }
            return Ok();
        }
    }
}