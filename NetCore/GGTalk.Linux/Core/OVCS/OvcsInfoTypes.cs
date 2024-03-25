using System;
using System.Collections.Generic;
using System.Text;

namespace OVCS.Core
{
    /// <summary>
    /// 自定义信息的类型
    /// </summary>
    internal static class OvcsInfoTypes
    {
        #region 广播消息
        /// <summary>
        /// 广播聊天信息
        /// </summary>
        public const int BroadcastChat = 200;
        /// <summary>
        /// 广播共享桌面
        /// </summary>
        public const int BroadcastShareDesk = 201;       
        #endregion

        #region p2p消息

        /// <summary>
        /// 授权或者取消远程控制
        /// </summary>
        public const int ControlDesktop = 252;
        /// <summary>
        /// 共享桌面
        /// </summary>
        public const int ShareDesk = 253;    
        #endregion      
       
    }
}
