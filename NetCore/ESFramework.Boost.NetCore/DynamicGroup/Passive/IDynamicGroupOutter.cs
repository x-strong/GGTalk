using System;
using System.Collections.Generic;
using ESFramework;
using ESFramework.Core ;
using ESFramework.Passive ;
using ESBasic;

namespace ESFramework.Boost.DynamicGroup.Passive
{
	/// <summary>
	/// 用于客户端发送与组操作相关的信息和广播信息。    
	/// </summary>
    public interface IDynamicGroupOutter
	{
        /// <summary>
        /// 当新成员加入组时，触发该事件。参数：GroupID - MemberID
        /// </summary>
        event CbGeneric<string, string> SomeoneJoinGroup;

        /// <summary>
        /// 当成员退出组时，触发该事件。参数：GroupID - MemberID
        /// </summary>
        event CbGeneric<string, string> SomeoneQuitGroup;

        /// <summary>
        /// 当用户掉线，从所有动态组中退出时，触发该事件。参数：MemberID
        /// </summary>
        event CbGeneric<string> GroupmateOffline;

        /// <summary>
        /// 当接收到某个组内的广播消息（包括大数据块信息）时，触发此事件。参数：broadcasterID - groupID - broadcastType - broadcastContent。
        /// 如果broadcasterID为null，表示是服务端发送的广播。
        /// </summary>
        event CbGeneric<string, string, int, byte[]> BroadcastReceived;

        /// <summary>
        /// 发送广播消息。
        /// </summary>        
        void Broadcast(string groupID, int broadcastType, byte[] broadcastContent, ActionTypeOnChannelIsBusy action);
        
        /// <summary>
        /// 当组友上线时，是否自动进行P2P连接。默认值为true。
        /// </summary>
        bool TryP2PWhenGroupmateConnected { get; set; }
      
        /// <summary>
        /// 加入组。
        /// </summary>  
        void JoinGroup(string groupID);

        bool Recruit(string groupID, string memberID);

        void Fire(string groupID, string memberID);

        /// <summary>
        /// 退出组。
        /// </summary>  
        void QuitGroup(string groupID);

        void DestroyGroup(string groupID);

        /// <summary>
        /// 获取组成员列表。如果本地有缓存，有直接从本地缓存提取。
        /// </summary>        
        List<string> GetGroupMembers(string groupID);

        /// <summary>
        /// 从服务器获取组成员列表。即使本地有缓存，也查询服务器。
        /// </summary>
        List<string> GetGroupMembersFromServer(string groupID);

        /// <summary>
        /// 在组内广播信息。
        /// </summary>
        /// <param name="groupID">接收广播信息的组ID</param>
        /// <param name="broadcastType">广播信息的类型</param>
        /// <param name="broadcastContent">信息的内容</param>
        /// <param name="broadcastChannelMode">选择通道模型。</param>
        /// <param name="action">当通道繁忙时采取的操作。</param>        
        void Broadcast(string groupID, int broadcastType, byte[] broadcastContent, ActionTypeOnChannelIsBusy action, BroadcastChannelMode broadcastChannelMode);

        /// <summary>
        /// 在组内广播大数据块信息。直到数据发送完毕，该方法才会返回。如果担心长时间阻塞调用线程，可考虑异步调用本方法。
        /// </summary>
        /// <param name="groupID">接收广播信息的组ID</param>
        /// <param name="broadcastType">广播信息的类型</param>
        /// <param name="blobContent">大数据块的内容</param>
        /// <param name="broadcastChannelMode">选择通道模型。</param>
        /// <param name="fragmentSize">分片传递时，片段的大小</param>  
        void BroadcastBlob(string groupID, int broadcastType, byte[] blobContent, int fragmentSize, BroadcastChannelMode broadcastChannelMode);
	}

    /// <summary>
    /// 广播通道选择模型。
    /// </summary>
    public enum BroadcastChannelMode
    {
        /// <summary>
        /// 如果和某个用户之间的P2P通道存在，则到该用户的广播经过P2P通道传送；否则，经过服务器中转。
        /// </summary>
        P2PChannelFirst = 0,
        /// <summary>
        /// 全部经过服务器中转，不使用任何P2P通道。
        /// </summary>
        AllTransferByServer,
        /// <summary>
        /// 仅仅使用P2P通道。如果与某个用户之间的P2P通道不存在，则不发送广播到该用户。
        /// </summary>
        AllByP2PChannel
    }
}
