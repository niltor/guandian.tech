using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guandian.Services
{
    public static class GithubConfig
    {
        /// <summary>
        /// 组织名
        /// </summary>
        public static string OrgName = "TechViewsTeam";
        /// <summary>
        /// 仓库名
        /// </summary>
        public static string ReposName = "Practknow";
        /// <summary>
        /// 默认分支
        /// </summary>
        public static string DefaultBranch = "master";
        /// <summary>
        /// 默认提交待审核目录名称
        /// </summary>
        public static string DefaultDicName = "待审核";
    }
}
