using System.IO;
using System.Threading.Tasks;
using Senparc.NeuChar.Entities;
using Senparc.NeuChar.Entities.Request;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;

namespace Comment.Services
{
    public class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {
        public CustomMessageHandler(Stream inputStream, PostModel postModel)
            : base(inputStream, postModel)
        {

        }

        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            int orderLength = 22;//订单长度
            string prefix = "1";//订单前缀
            var requestHandler = requestMessage.StartHandler().Default(() =>
            {
                // 判断是否符合格式
                string content = requestMessage.Content;
                content = content.Trim();
                if (content.Length == orderLength && content.StartsWith(prefix) && content.Contains("-"))
                {
                    // 数据库查询
                    var defaultResponseMessage = base.CreateResponseMessage<ResponseMessageText>();
                    //using (var db = new PetContext()) {
                    //    var order = db.Order.SingleOrDefault(o => o.订单号.Equals(content.Trim()));
                    //    if (order != null) {

                    //        defaultResponseMessage.Content = order.订单号 + " 已绑定成功!";
                    //        #region 模板消息
                    //        //string openId = requestMessage.FromUserName;
                    //        //string template_id = "";
                    //        //string scene = "";
                    //        //var data = new {

                    //        //};

                    //        //TemplateApi.Subscribe(base.AppId, openId, template_id, scene, "这是一条“一次性订阅消息”", data);
                    //        #endregion

                    //        // 判定是否，存储用户信息
                    //        var openId = requestMessage.FromUserName;
                    //        var user = db.User.SingleOrDefault(u => u.WxOpenid == openId);
                    //        if (user == null) {
                    //            var newUser = new User {
                    //                Name = order.收货人,
                    //                WxOpenid = requestMessage.FromUserName
                    //            };
                    //            db.Add(newUser);
                    //            order.User = newUser;
                    //            db.Update(order);
                    //        }
                    //        else {
                    //            order.User = user;
                    //            db.Update(order);
                    //        }
                    //        db.SaveChanges();
                    //    }
                    //    else {
                    //        defaultResponseMessage.Content = "未查到该订单信息";
                    //    }
                    //    return defaultResponseMessage;
                    //}
                    return defaultResponseMessage;
                }
                else
                {
                    var defaultResponseMessage = base.CreateResponseMessage<ResponseMessageText>();
                    defaultResponseMessage.Content = "-_- " + content;
                    return defaultResponseMessage;
                }
            });
            return requestHandler.GetResponseMessage() as IResponseMessageBase;
        }

        /// <summary>
        /// 默认回复
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "";
            return responseMessage;
        }
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override Task<IResponseMessageBase> OnEvent_SubscribeRequestAsync(RequestMessageEvent_Subscribe requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "复制拼多多订单号，发送到对话框即可自动绑定保修卡";
            return base.OnEvent_SubscribeRequestAsync(requestMessage);
        }
    }
}