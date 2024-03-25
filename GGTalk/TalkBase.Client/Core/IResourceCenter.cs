using System;
using ESBasic.Security;
using ESPlus.Rapid;
using ESBasic.Loggers;
using TalkBase.Client.Bridges;
namespace TalkBase.Client
{
    /// <summary>
    /// 客户端全局资源中心。
    /// </summary> 
    public interface IResourceCenter      
    {
        /// <summary>
        /// 聊天窗口控制器。
        /// </summary>
        IChatFormController ChatFormController { get; }        

        /// <summary>
        /// 与服务器/其它在线用户进行交互的通道。
        /// </summary>
        IClientOutter ClientOutter { get; }

        /// <summary>
        /// 与服务器的连接是否正常？
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// 当前登录用户的ID
        /// </summary>
        string CurrentUserID { get; }

        /// <summary>
        /// 聊天记录本地持久化器。
        /// </summary>
        IChatRecordPersister LocalChatRecordPersister { get; }

        /// <summary>
        /// 日志记录器。
        /// </summary>
        IAgileLogger Logger { get; }

        /// <summary>
        /// 网络通信引擎。
        /// </summary>
        IRapidPassiveEngine RapidPassiveEngine { get; }

        /// <summary>
        /// 聊天记录服务端持久化器。
        /// </summary>
        IChatRecordGetter RemoteChatRecordGetter { get; }

        /// <summary>
        /// Unit基础信息提供者。
        /// </summary>
        IUnitInfoProvider UnitInfoProvider { get; }


        /// <summary>
        /// 3DES加密。如果消息不需要加密，则返回null。
        /// </summary>
        DesEncryption DesEncryption { get; }
    }
}
