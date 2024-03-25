using System;
using System.Collections.Generic;
using System.Text;
using ESBasic;
using ESFramework;

namespace ESFramework.Boost.DynamicGroup.Server
{
    /// <summary>
    /// 组控制器。
    /// </summary>
    public interface IDynamicGroupController
    {
        /// <summary>
        /// 当服务端接收到要转发的广播消息（包括大数据块信息）时，触发此事件。参数：broadcasterID - groupID - broadcastType - broadcastContent
        /// </summary>
        event CbGeneric<string, string, int, byte[]> BroadcastReceived;

        /// <summary>
        /// 加入/退出组、上下线等事件是否通知组友。默认值为true。
        /// 注意，如果关闭该选项，客户端则不能保持实时的组的状态，将导致广播信息发送出现错误（IGroupOutter.Broadcast方法）。
        /// </summary>
        bool GroupNotifyEnabled { get; set; }

         /// <summary>
        /// 组友上下线通知是否使用单独的线程。默认值为false。
        /// 如果要开启组友通知线程，必须先开启组友通知（即必须先将GroupNotifyEnabled设置为true）。
        /// </summary>
        bool UseGroupNotifyThread { get; set; }       
      

        /// <summary>
        /// 在组内广播信息。
        /// </summary>        
        void Broadcast(string groupID, int broadcastType, byte[] broadcastContent, ActionTypeOnChannelIsBusy action);
    }
}
