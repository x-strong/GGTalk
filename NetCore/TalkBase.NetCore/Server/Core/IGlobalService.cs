using System;
using System.Collections.Generic;
using System.Text;

namespace TalkBase.Server
{
    /// <summary>
    /// 提供给第三方系统在操作完DB后，再使用该接口通知服务端。
    /// </summary>
    public interface IGlobalService
    {
        /// <summary>
        /// 通过外部系统注册用户后，调用此方法通知TalkBase服务端。
        /// </summary>       
        void OnUserRegistered(params string[] users);

        /// <summary>
        /// 通过外部系统删除用户后，调用此方法通知TalkBase服务端。
        /// </summary>       
        void OnUserDeleted(params string[] users);    

        /// <summary>
        /// 通过外部系统修改用户资料(用户的Version要增加)后，调用此方法通知TalkBase服务端。
        /// </summary>       
        void OnUserInfoChanged(params string[] users);
                
        /// <summary>
        /// 通过外部系统修改用户头像时，调用此方法通知TalkBase服务端。
        /// </summary>       
        void OnUserHeadImageChanged(params string[] users);

        /// <summary>
        /// 通过外部系统添加组/群（支持已完成组员列表的编辑）后，调用此方法通知TalkBase服务端。
        /// </summary>       
        void OnGroupAdded(params string[] groups);

        /// <summary>
        /// 通过外部系统删除组/群后，调用此方法通知TalkBase服务端。
        /// </summary>       
        void OnGroupRemoved(params string[] groups);

        /// <summary>
        /// 通过外部系统修改组/群资料后，调用此方法通知TalkBase服务端。
        /// </summary>       
        void OnGroupInfoChanged(params string[] groups);

        /// <summary>
        /// 通过外部系统修改组/群成员后，调用此方法通知TalkBase服务端。
        /// </summary>       
        void OnGroupMembersChanged(params string[] groups);

        /// <summary>
        /// 发送系统消息。（接收方将触发IClientOutter的SystemMessageReceived事件）
        /// </summary>
        /// <param name="unitID">接收者的ID，可以为UserID或GroupID。如果为null，表示发送给所有用户。</param>
        /// <param name="sysMsgType">系统消息类型</param>
        /// <param name="content">系统消息的内容</param>
        /// <param name="useOfflineMessageMode">如果某接收者不在线，是否将其转化为离线消息。</param>
        void SendSystemMessage(string unitID, int sysMsgType, byte[] content, string tag ,bool useOfflineMessageMode);

        #region 命令服务端进行数据操作（之前的API是外部系统修改数据库后通知服务端将新数据从DB加载到内存的缓存中，此处API是命令服务端直接操作数据库并修改缓存）
        /// <summary>
        /// 命令服务端从群组中移除成员。（将从DB和服务端缓存中移除，并通知被移除的人和组成员）
        /// </summary>      
        void RemoveMemberFromGroup(string operatorID, string groupID, params string[] members);

        /// <summary>
        /// 命令服务端将成员加入群组。（将添加到DB和服务端缓存中，并通知被添加的人和组成员）
        /// </summary>      
        void AddMemberIntoGroup(string operatorID, string groupID, params string[] members);

        /// <summary>
        /// 命令服务端创建群组。（将添加到DB和服务端缓存中）
        /// </summary>      
        CreateGroupResult CreateGroup(string groupID, string groupName, string creatorID);

        /// <summary>
        /// 命令服务端删除用户。（将从DB和服务端缓存中删除，并通知其相关联系人）
        /// </summary>     
        void DeleteUser(string operatorID, string userID);

        /// <summary>
        /// 命令服务端将两个用户互加为好友。（将添加到DB和服务端缓存中，并通知用户）
        /// </summary>     
        void AddFriend(string user1, string user2);

        /// <summary>
        /// 命令服务端解除两个用户的好友关系。（将修改同步到DB和服务端缓存中，并通知用户）
        /// </summary>    
        void RemoveFriend(string user1, string user2);

        /// <summary>
        /// 命令服务端修改用户基础资料。（将修改DB和服务端缓存中的数据，并通知其相关联系人）
        /// </summary>     
        void ChangeUserBaseInfo(string userID, string name, string signature, string orgID);

        /// <summary>
        /// 申请添加好友
        /// </summary>
        /// <param name="requesterID">申请者ID</param>
        /// <param name="accepterID">接收者ID</param>
        /// <param name="comment">备注</param>
        /// <param name="requesterCatalogName">申请者的好友分组</param>
        void RequestAddFirend(string requesterID, string accepterID, string comment, string requesterCatalogName);
        #endregion
    }
}
