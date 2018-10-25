using System.IO;
using System.Threading.Tasks;
using Senparc.NeuChar.Entities;
using Senparc.NeuChar.Entities.Request;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;

namespace Guandian.Services
{
    public class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {
        public CustomMessageHandler(Stream inputStream, PostModel postModel)
            : base(inputStream, postModel)
        {

        }

        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var requestHandler = requestMessage.StartHandler().Default(() =>
            {
                string content = requestMessage.Content;
                content = content.Trim();
                var defaultResponseMessage = base.CreateResponseMessage<ResponseMessageText>();
                defaultResponseMessage.Content = "访问 https://guandian.tech， 获取更多更详情的内容！";
                return defaultResponseMessage;
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
            responseMessage.Content = "欢迎订阅TechViews，我们会与您分享最新科技资讯和科技观点!";
            return base.OnEvent_SubscribeRequestAsync(requestMessage);
        }
    }
}