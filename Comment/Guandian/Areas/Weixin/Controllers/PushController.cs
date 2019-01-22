using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            int maxTitleLength = 60;
            // 获取新闻原内容
            var news = _context.News
                .Where(n => n.IsPublishToMP == false && n.CreatedTime.Date >= DateTime.Now.Date.AddDays(-5))
                .OrderByDescending(n => n.CreatedTime)
                .Take(10)
                .ToList();

            var random = new Random();
            int number = random.Next(3, 6);
            var blogs = _context.Blogs
                .Where(b => b.IsPublishMP == false
                && b.Status != Data.Entity.Status.Obsolete
                && !string.IsNullOrEmpty(b.Content))
                .OrderByDescending(b => b.UpdatedTime)
                .Take(number)
                .ToList();

            var newsList = new List<NewsModel>();
            var token = AccessTokenContainer.TryGetAccessToken(AppId, AppSecret);

            DumpConsole("开始处理新闻");
            try
            {
                // 新闻内容
                if (news != null && news.Count > 0)
                {
                    string content = "";
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
                            content += $@"<div class='row'>
                        <div class='news-title mt-1'>
                            <strong style='font-size:18px;color:#015cda'>{item.Title}</strong>
                        </div>
                        <div>
                            <img src='{uploadImgResult.url}' width='100%' />
                        </div>
                        <div class='news-description'>{item.Description}</div> 
                        <br /><br />
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
                        mainNews.title = mainNews.title.Length > maxTitleLength ? mainNews.title.Substring(0, maxTitleLength) : mainNews.title;
                        newsList.Add(mainNews);
                    }
                }
                DumpConsole("新闻处理完成");
                DumpConsole("开始处理博客");
                // 博客内容
                if (blogs != null && blogs.Count > 0)
                {
                    using (var hc = new HttpClient())
                    {
                        hc.Timeout = TimeSpan.FromSeconds(5);
                        var htmlDoc = new HtmlDocument();
                        try
                        {
                            foreach (var item in blogs)
                            {
                                // 处理文章内图片
                                htmlDoc.LoadHtml(item.Content);
                                var root = htmlDoc.DocumentNode;
                                var imageNodes = root.SelectNodes(".//img");
                                // 移除视频元素
                                var videoNodes = root.SelectNodes(".//video");
                                if (videoNodes != null && videoNodes.Count > 0)
                                {
                                    foreach (var video in videoNodes)
                                    {
                                        root.SelectSingleNode(video.XPath).Remove();
                                    }
                                }
                                string mediaId = "";
                                if (imageNodes != null)
                                {
                                    for (int i = 0; i < imageNodes.Count; i++)
                                    {
                                        var url = imageNodes[i].GetAttributeValue("src", null);
                                        // 只保留jpg图片
                                        if (!url.EndsWith(".jpg"))
                                        {
                                            root.SelectSingleNode(imageNodes[i].XPath).Remove();
                                        }
                                        else
                                        {
                                            DumpConsole(item.Title + " 图片处理");
                                            var image = imageNodes[i].Attributes["src"].Value;
                                            var tempFileName = StringTools.GetTempFileName("jpg");
                                            await DownloadFile(hc, image, tempFileName);
                                            // 判断大小. TODO:处理图片大小
                                            var file = new FileInfo(tempFileName);
                                            if (file.Length > 1 * 1024 * 1024) continue;
                                            var uploadImgResult = await MediaApi.UploadImgAsync(token, tempFileName);
                                            DumpConsole("上传图片" + image + " " + file.Length / 1024);
                                            System.IO.File.Delete(tempFileName);
                                            // 替换文本
                                            imageNodes[i].SetAttributeValue("src", uploadImgResult.url);

                                            // TODO:后面可删除该封面
                                        }
                                    }
                                }
                                // 缩略图
                                var mediaResult = await MediaApi.GetOthersMediaListAsync(token, Senparc.Weixin.MP.UploadMediaFileType.image, 0, 2);
                                var defaultMediaId = mediaResult.item?.FirstOrDefault()?.media_id;

                                // 下载缩略图，下载失败则使用默认图
                                var tempFileName1 = StringTools.GetTempFileName("jpg");
                                await DownloadFile(hc, item.Thumbnail, tempFileName1);
                                if (System.IO.File.Exists(tempFileName1))
                                {
                                    var thumbImg = await MediaApi.UploadForeverMediaAsync(token, tempFileName1);
                                    mediaId = thumbImg.media_id;
                                }
                                else
                                {
                                    mediaId = defaultMediaId;
                                }

                                // 处理内容，微信消息最大长度为2W字符，小于1M
                                Console.WriteLine("长度:" + root.InnerHtml.Length);
                                if (root.InnerHtml.Length >= 20000)
                                {
                                    root.InnerHtml = root.InnerHtml.Substring(0, 19500);
                                }

                                // 构造图文消息体
                                var currentNews = new NewsModel
                                {
                                    author = item.AuthorName,
                                    thumb_media_id = mediaId,
                                    // html如果有#，会报错
                                    content = root.InnerHtml.Replace("#", ""),
                                    // 长度处理
                                    title = item.Title.Length > maxTitleLength ? item.Title.Substring(0, maxTitleLength) : item.Title,
                                    show_cover_pic = "0",
                                    content_source_url = "https://guandian.tech/blogs/detail/" + item.Id,
                                    digest = "",
                                };
                                DumpConsole("博客标题:" + currentNews.title + "; 长度:" + currentNews.title.Length);
                                item.IsPublishMP = true;
                                newsList.Add(currentNews);
                            }
                        }
                        catch (Exception e)
                        {
                            DumpConsole(e.Message);
                        }
                    }
                }
                if (newsList.Count < 1)
                {
                    DumpConsole("无可推送内容");
                    return Ok();
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
            }
            return Ok();
        }


        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task DownloadFile(HttpClient hc, string url, string filename)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(filename)) return;
            try
            {
                using (var result = await hc.GetAsync(url))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        var stream = await result.Content.ReadAsStreamAsync();
                        using (var fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
                        {
                            await stream.CopyToAsync(fileStream);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                DumpConsole(e.Message);
            }

        }
    }
}