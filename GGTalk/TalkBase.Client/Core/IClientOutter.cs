using ESBasic;
using ESFramework;
using ESPlus.Application;
using OMCS.Passive.ShortMessages;
using System;
using System.Collections.Generic;

namespace TalkBase.Client
{
    /// <summary>
    /// 与服务器/其它在线用户进行交互的通道。
    /// </summary>
    public interface IClientOutter
    {
        #region Event

        /// <summary>
        /// 当接收到对方申请添加我为好友时，触发该事件。 参数：sourceUserID - comment
        /// </summary>
        event CbGeneric<string, string> AddFriendRequestReceived;

        /// <summary>
        /// 当收到对方处理我的好友申请时，触发该事件。 参数：destUserID - isAgreed
        /// </summary>
        event CbGeneric<string, bool, bool> AddFriendResponseReceived;

        /// <summary>
        /// 当收到服务端返回的好友申请分页信息时，触发该事件 参数： AddFriendRequestPage 好友申请分页信息
        /// </summary>
        event CbGeneric<AddFriendRequestPage> AddFriendRequestPageReceived;

        /// <summary>
        /// 当接收到对方申请加入讨论组时，触发该事件。 参数：requesterID -groupID- comment
        /// </summary>
        event CbGeneric<string, string, string> AddGroupRequestReceived;

        /// <summary>
        /// 当收到对方处理我的加入讨论组申请时，触发。 参数requesterID- groupID - isAgreed
        /// </summary>
        event CbGeneric<string, string, bool> AddGroupResponseReceived;

        /// <summary>
        /// 当收到服务端返回的加入讨论组申请分页信息时，触发该事件 参数： AddGroupRequestPage 好友申请分页信息
        /// </summary>
        event CbGeneric<AddGroupRequestPage> AddGroupRequestPageReceived;

        /// <summary>
        /// 当与某个Unit最后交谈记录发生变化时，触发此事件。（在触发此事件之前，已经为目标Unit的LastWordsRecord属性赋值）
        /// 1.当发送消息给好友或群后；2.在GroupChatMessageReceived或FriendChatMessageReceived之后触发。
        /// </summary>
        event CbGeneric<IUnit> LastWordsChanged;

        /// <summary>
        /// 当接收到对方的正在输入的通知时，触发此事件。参数：sourceUserID
        /// </summary>
        event CbGeneric<string> InptingNotifyReceived;

        /// <summary>
        /// 当接收到对方的抖屏时，触发此事件。参数：sourceUserID
        /// </summary>
        event CbGeneric<string> VibrationReceived;

        /// <summary>
        /// 当接收到对方关于视频/语音/远程协助的沟通信息时，触发此事件。参数：sourceUserID - sourceClientType - CommunicateMediaType - CommunicateType - tag
        /// </summary>
        event CbGeneric<string, ClientType, CommunicateMediaType, CommunicateType, string> MediaCommunicateReceived;

        /// <summary>
        /// 当在其它设备上对CommunicateType.Request进行了回答时，触发此事件。参数：friendID - ClientType（应答的设备） - CommunicateMediaType - agree
        /// </summary>
        event CbGeneric<string, ClientType, CommunicateMediaType, bool> MediaCommunicateAnswerOnOtherDevice;

        /// <summary>
        /// 被对方添加为好友，参数：sourceUserID。（托盘闪动点击后触发。在触发此事件之前，已经将好友添加到本地缓存。）
        /// </summary>
        event CbGeneric<string> FriendAdded;

        /// <summary>
        /// 当接收到群聊天消息时，触发此事件。参数：broadcasterID - groupID - content - offlineMsgOccurTime。（在触发此事件之前，已经消息添加到本地聊天记录。）
        /// </summary>
        event CbGeneric<string, string, byte[], string, DateTime?> GroupChatMessageReceived;


        /// <summary>
        /// 当接收到对方的聊天消息时，触发此事件。参数：sourceUserID - sourceType - content - offlineMsgOccurTime。（在触发此事件之前，已经消息添加到本地聊天记录。）
        /// </summary>
        event CbGeneric<string, ClientType, byte[], DateTime?> FriendChatMessageReceived;

        /// <summary>
        /// 当接收到自己在其它设备上发送给对方的聊天消息时，触发此事件。参数：ClientType - destUserID - content。（在触发此事件之前，已经消息添加到本地聊天记录。）
        /// </summary>
        event CbGeneric<ClientType, string, byte[]> FriendChatMessageEchoReceived;

        /// <summary>
        /// 当接收到对方的阅后即焚消息时，触发此事件。参数：sourceUserID - sourceType - content - time。
        /// </summary>
        event CbGeneric<string, ClientType, byte[], DateTime?> SnapchatMessageReceived;

        /// <summary>
        /// 当接收到对方（或自己其他类型客户端）的自焚消息被阅读时，触发此事件。参数：sourceUserID -SnapchatMessageRead
        /// </summary>
        event CbGeneric<string, SnapchatMessageRead> SnapchatReadReceived;

        /// <summary>
        /// 当接收到对方的语音消息时，触发此事件。参数：AudioMessage - offlineMsgOccurTime。（在触发此事件之前，已经消息添加到本地聊天记录。）
        /// </summary>
        event CbGeneric<AudioMessage, DateTime?> FriendAudioMessageReceived;

        /// <summary>
        /// 当接收到自己在其它设备上发送给对方的语音消息时，触发此事件。参数：ClientType - AudioMessage - destUserID。（在触发此事件之前，已经消息添加到本地聊天记录。）
        /// </summary>
        event CbGeneric<ClientType, AudioMessage, string> FriendAudioMessageEchoReceived;

        /// <summary>
        /// 当接收到对方对我发给他的离线文件的处理结果后，触发此事件，参数：sourceUserID - fileName - accept 。
        /// </summary>
        event CbGeneric<string, string, bool> OfflineFileResultReceived;

        /// <summary>
        /// 当收到系统消息时，触发此事件。参数：SystemMessageContract - offlineMsgOccurTime。
        /// </summary>
        event CbGeneric<SystemMessageContract, DateTime?> SystemMessageReceived;


        /// <summary>
        /// 当接收到群文件上传完成的通知时，触发此事件。参数：sourceUserID - groupID - fileName
        /// </summary>
        event CbGeneric<string, string, string> GroupFileUploadedNotifyReceived;

        /// <summary>
        /// 当接收到被禁言通知，触发此事件，参数：operatorID - groupID - minutes 。
        /// </summary>
        event CbGeneric<string, string, double> GroupBan4UserReceived;

        /// <summary>
        /// 当接收到被解除禁言通知，触发此事件，参数：groupID
        /// </summary>
        event CbGeneric<string> RemoveGroupBan4UserReceived;

        /// <summary>
        /// 当收到开启全员禁言通知，触发此事件，参数operatorID - groupID
        /// </summary>
        event CbGeneric<string, string> GroupBan4GroupReceived;

        /// <summary>
        /// 当收到关闭全员禁言通知，触发此事件，参数 groupID
        /// </summary>
        event CbGeneric<string> RemoveGroupBan4GroupReceived;


        #endregion

        #region Chat
        /// <summary>
        /// 发送聊天消息。（并插入本地聊天记录）
        /// </summary>      
        ChatMessageRecord SendChatMessage(string destUserID, byte[] msg, ResultHandler handler);

        /// <summary>
        /// 发送聊天消息（接收者可以为多个User和Group，用于转发消息）。（并插入本地聊天记录）
        /// </summary>      
        void SendChatMessage(List<string> accepters, byte[] msg, ResultHandler handler);

        /// <summary>
        /// 发送语音消息。（并插入本地聊天记录）
        /// </summary> 
        ChatMessageRecord SendAudioMessage(string destUserID, AudioMessage msg);

        /// <summary>
        /// 发送阅后自焚消息
        /// </summary>
        /// <param name="destUserID"></param>
        /// <param name="msg"></param>
        void SendSnapchatMessage(string destUserID, SnapchatMessage msg);

        /// <summary>
        /// 发送已阅读了自焚消息
        /// </summary>
        /// <param name="snapchatMessageRead"></param>
        void SendSnapchatReadMessage(SnapchatMessageRead snapchatMessageRead);

        /// <summary>
        /// 发送群聊天消息。（并插入本地聊天记录）
        /// </summary>      
        ChatMessageRecord SendGroupChatMessage(string groupID, byte[] msg, ResultHandler handler, string tag = null);

        /// <summary>
        /// 发送振动提醒。
        /// </summary>      
        void SendVibration(string destUserID);


        void SendInputingNotify(string destUserID);

        #endregion

        #region 视频、语音、远程
        /// <summary>
        /// 发送视频、语音、远程沟通。
        /// </summary>      
        void SendMediaCommunicate(string destUserID, CommunicateMediaType mediaType, CommunicateType communicateType, string tag, ClientType? destClientType = null);
        #endregion

        #region Friend

        /// <summary>
        /// 查询群组（精确查询）。 如果不存在则返回  null. 
        /// </summary>
        /// <param name="idOrName"></param>
        /// <returns></returns>
        List<IUser> SearchUserList(string idOrName);

        /// <summary>
        /// 根据userID精确查询群组
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        IUser SearchUser(string userID);

        /// <summary>
        /// 申请添加好友
        /// </summary>
        /// <param name="friendID">接收者ID</param>
        /// <param name="comment">备注</param>
        /// <param name="catalogName">申请者的好友分组</param>
        void RequestAddFriend(string friendID, string comment, string catalogName);

        /// <summary>
        /// 处理添加好友请求
        /// </summary>
        /// <param name="friendID"></param>
        /// <param name="accepterCatalogName">接收者的好友分组</param>
        /// <param name="agree"></param>
        void HandleAddFriendRequest(string friendID, string accepterCatalogName, bool agree);

        /// <summary>
        /// 发送请求添加好友分页消息， 会收到服务端发来的消息类型为 GetAddFriendPage 的消息
        /// </summary>
        void GetAddFriendPage();

        /// <summary>
        /// 添加好友。如果服务器返回成功，则将其资料添加到本地缓存。
        /// </summary>      
        AddFriendResult AddFriend(string friendID, string catalogName);

        /// <summary>
        /// 删除好友。（修改本地个人资料，从缓存中移除用户，向服务器发送通知）
        /// </summary>        
        void RemoveFriend(string friendID);

        /// <summary>
        /// 查询服务器自己是否位于对方的黑名单中？
        /// </summary>       
        bool IsInHisBlackList(string destUserID);
        #endregion

        #region FriendCatalog
        /// <summary>
        /// 将好友从一个分组移动到另一个分组。（修改本地个人资料，向服务器发送通知）
        /// </summary>        
        void MoveFriendCatalog(string friendID, string oldCatalog, string newCatalog);

        /// <summary>
        /// 删除一个分组。（修改本地个人资料，向服务器发送通知）
        /// </summary> 
        void RemoveFriendCatalog(string catalog);

        /// <summary>
        /// 修改分组名称。（修改本地个人资料，向服务器发送通知）
        /// </summary> 
        void ChangeFriendCatalogName(string oldName, string newName, bool isMerge);

        /// <summary>
        /// 添加分组。（修改本地个人资料，向服务器发送通知）
        /// </summary> 
        void AddFriendCatalog(string catalog);
        #endregion

        #region Group
        /// <summary>
        /// 申请加入群组
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="comment"></param>
        void RequestAddGroup(string groupID, string comment);

        /// <summary>
        /// 处理加入群组申请 （创建者或管理员 操作）
        /// </summary>
        /// <param name="requesterID"></param>
        /// <param name="groupID"></param>
        /// <param name="agree"></param>
        void HandleAddGroupRequest(string requesterID, string groupID, bool agree);

        /// <summary>
        /// 发送请求添加好友分页消息， 会收到服务端发来的消息类型为 GetAddFriendPage 的消息
        /// </summary>
        void GetAddGroupPage();

        /// <summary>
        /// 创建组。如果创建成功，则将组添加到本地缓存中。
        /// </summary>        
        CreateGroupResult CreateGroup(string groupID, string name, string announce, List<string> members, bool isPrivate = false);

        /// <summary>
        /// 创建组（如：群、讨论组、语音聊天室、视频会议 等）。如果创建成功，则将组添加到本地缓存中。
        /// </summary>
        CreateGroupResult CreateGroup(GroupType groupType, string groupID, string name, string announce, List<string> members, bool isPrivate = false);

        /// <summary>
        /// 加入组。如果加入成功，则将从服务器加载组资料，本地缓存。
        /// </summary>       
        JoinGroupResult JoinGroup(string groupID);

        /// <summary>
        /// 查询群组。 如果不存在则返回  空List
        /// </summary>
        /// <param name="idOrName"></param>
        /// <returns></returns>
        List<IGroup> SearchGroupList(string idOrName);

        /// <summary>
        /// 根据groupID精确查询群组
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        IGroup SearchGroup(string groupID);

        /// <summary>
        /// 退出组。（向服务器发送通知，从当前缓存中移除目标组，从个人资料中删除组）
        /// </summary>        
        void QuitGroup(string groupID);

        /// <summary>
        /// 删除组。（向服务器发送通知，从当前缓存中移除目标组，从个人资料中删除组）
        /// </summary>      
        void DeleteGroup(string groupID);

        /// <summary>
        /// 修改组资料。（向服务器发送通知，修改当前缓存中的目标组资料）
        /// </summary>        
        void ChangeGroupInfo(string groupID, string name, string announce);

        /// <summary>
        /// 修改组成员。（向服务器发送通知，修改当前缓存中的目标组资料）
        /// </summary>        
        void ChangeGroupMembers(string groupID, List<string> members);

        /// <summary>
        /// 群共享文件上传完成时，调用此接口通知其它群成员。
        /// </summary>        
        void GroupFileUploaded(string groupID, string fileName);
        #endregion        

        #region SystemMessage
        /// <summary>
        /// 发送系统消息给某个用户或群组。（接收方将触发SystemMessageReceived事件）
        /// </summary>
        /// <param name="receiverUnitID">接收者的ID，可以为UserID或GroupID。如果为null，表示发送给所有用户。</param>
        /// <param name="sysMsgType">系统消息类型</param>
        /// <param name="content">系统消息的内容</param>
        /// <param name="useOfflineMessageMode">如果某接收者不在线，是否将其转化为离线消息。</param>
        void SendSystemMessage(string receiverUnitID, int sysMsgType, byte[] content, string tag, bool useOfflineMessageMode);
        #endregion        

        #region 修改个人信息、状态
        /// <summary>
        /// 修改当前状态。（修改缓存中自己的状态，向服务器发送通知）
        /// </summary>        
        void ChangeMyStatus(UserStatus status);

        /// <summary>
        /// 修改密码。
        /// </summary>       
        ChangePasswordResult ChangeMyPassword(string oldPasswordMD5, string newPasswordMD5);

        /// <summary>
        /// 修改个人基础资料（将修改个人信息的版本号）。（将信息提交到服务器，然后修改缓存中自己的资料）
        /// </summary>        
        void ChangeMyBaseInfo(string name, string signature, string department);

        /// <summary>
        /// 修改自己的头像（将修改个人信息的版本号）。（将信息提交到服务器，然后修改缓存中自己的资料）
        /// </summary>        
        void ChangeMyHeadImage(int defaultHeadImageIndex, byte[] customizedHeadImage, ResultHandler handler);

        /// <summary>
        /// 修改自己的业务信息（可能会影响到个人信息的版本号）。（将信息提交到服务器，然后修改缓存中自己的资料，缓存将触发MyBusinessInfoChanged事件。）
        /// </summary>
        /// <param name="key">业务信息项Key</param>
        /// <param name="info">业务信息数据</param>
        /// <param name="notifyContacts">是否将变化告知我的联系人。（联系人一方的缓存将触发UserBusinessInfoChanged事件）</param>
        /// <param name="increaseVersion">是否增加版本号</param>
        void ChangeMyBusinessInfo(string key, byte[] info, bool notifyContacts, bool increaseVersion);

        /// <summary>
        /// 修改自己的业务信息（可能会影响到个人信息的版本号）。（将信息提交到服务器，然后修改缓存中自己的资料，缓存将触发MyBusinessInfoChanged事件。）
        /// </summary>
        /// <param name="businessInfo">Key:业务信息项 ,Value:业务信息数据</param>        
        /// <param name="notifyContacts">是否将变化告知我的联系人。（联系人一方的缓存将触发UserBusinessInfoChanged事件）</param>
        /// <param name="increaseVersion">是否增加版本号</param>
        void ChangeMyBusinessInfo(Dictionary<string, byte[]> businessInfo, bool notifyContacts, bool increaseVersion);

        /// <summary>
        /// 请求离线消息（包括好友聊天消息、系统消息、加好友请求）。如果有离线消息，随后将触发OfflineChatMessageReceived事件。
        /// </summary>
        void RequestOfflineMessage();

        /// <summary>
        /// 请求离线文件。如果有离线文件，随后将触发OfflineFileResultReceived事件。
        /// </summary>
        void RequestOfflineFile();

        /// <summary>
        /// 增加或更新好友/群备注。如果commentName为null或String.Empty，则表示取消备注。（向服务器发送通知、修改本地自己的个人资料，修改缓存中对应Unit的CommentName字段）
        /// </summary>   
        void ChangeUnitCommentName(string unitID, string commentName);

        /// <summary>
        /// 彻底删除用户（将信息提交到服务器，然后从好友中移除、从群中移除、从缓存中移除、从DB中移除）。
        /// </summary>        
        bool DeleteUser(string userID);
        #endregion

        #region 手动同步用户资料
        /// <summary>
        /// 查看服务器上是否有目标用户资料的最新版本，如果有，则同步到本地。（如果有更新，本地缓存将触发UserBusinessInfoChanged事件）
        /// </summary>
        void SyncUserBaseInfo(string userID);

        /// <summary>
        /// [后台运行]查看服务器上是否有目标用户资料的最新版本，如果有，则同步到本地。（如果有更新，本地缓存将触发UserBusinessInfoChanged事件）
        /// </summary>
        void SyncUserBaseInfoInBackground(string userID);
        #endregion

        #region 群禁言
        /// <summary>
        /// 将群中具体成员设置禁言
        /// </summary>
        /// <param name="groupID">群ID</param>
        /// <param name="destUserID">对方账号</param>
        /// <param name="comment">备注</param>
        /// <param name="minutes">禁言时长（单位：分钟）</param>
        void SetGroupBan4User(string groupID, string destUserID, string comment, double minutes);

        /// <summary>
        /// 解除群中具体成员禁言
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="destUserID"></param>
        void RemoveGroupBan4User(string groupID, string destUserID);

        /// <summary>
        /// 检查当前用户在该groupID组中是否存在群禁言,若存在，会收到GroupBan4User的消息
        /// </summary>
        /// <param name="groupID"></param>
        void CheckGroupBan4CurrentUser(string groupID);

        /// <summary>
        /// 获取群组中所有正在禁言的记录
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        List<GroupBan> GetGroupBans4Group(string groupID);

        /// <summary>
        /// 获取该组是否存在全局禁言
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        bool ExistAllGroupBan(string groupID);

        /// <summary>
        /// 开启群组全员全员禁言（管理员、创始人除外）
        /// </summary>
        /// <param name="groupID"></param>
        void SetAllGroupBan(string groupID);

        /// <summary>
        /// 关闭群组全员禁言
        /// </summary>
        /// <param name="groupID"></param>
        void RemoveAllGroupBan(string groupID);

        #endregion

        #region 管理员
        /// <summary>
        /// 修改用户的状态
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userState"></param>
        void ChangeUserState4Admin(string userID, UserState userState);
        #endregion

    }
}
