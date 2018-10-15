using System.Collections.Generic;
using System.Threading.Tasks;
using MSDev.MetaWeblog;

namespace Guandian.Services
{
    /// <summary>
    /// 同步接口
    /// </summary>
    interface ISyncService
    {
        Task SyncTo(List<PostInfo> blogs, string categories = null);
        void SyncFrom(List<PostInfo> blogs, string categories = null);
    }
}
