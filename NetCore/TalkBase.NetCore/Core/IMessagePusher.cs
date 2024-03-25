using System;
using System.Collections.Generic;
using System.Text;

namespace TalkBase.NetCore.Core
{
    public interface IMessagePusher
    {
        /// <summary>
        /// 推送消息到指定用户
        /// </summary>
        /// <param name="destID">接受方ID</param>
        /// <param name="sourceDisPlayName">发送方显示的名称</param>
        /// <param name="msg">消息内容</param>
        void PushMessage(string destID, string sourceDisPlayName, string msg);
    }
}
