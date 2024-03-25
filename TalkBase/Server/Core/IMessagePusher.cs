using ESFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkBase.Server.Core
{
    public interface IMessagePusher
    {
        /// <summary>
        /// 推送消息到指定用户
        /// </summary>
        /// <param name="destID">接受方ID</param>
        /// <param name="sourceDisPlayName">发送方显示的名称</param>
        /// <param name="msg">消息内容</param>
        void PushMessage(string destID,string sourceDisPlayName, string msg);

        /// <summary>
        /// 推送消息到指定用户 (带参数)
        /// </summary>
        /// <param name="destID">接受方ID</param>
        /// <param name="sourceDisPlayName">发送方显示的名称</param>
        /// <param name="msg">消息内容</param>
        /// <param name="intentParams">键值对参数 item1：key ，item2： value</param>
        void PushMessage(string destID, string sourceDisPlayName, string msg, params Tuple<string, string>[] intentParams);
    }
}
