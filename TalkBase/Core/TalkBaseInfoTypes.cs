using System;
using System.Collections.Generic;
using System.Text;

namespace TalkBase
{    
    /// <summary>
    /// TalkBase框架的所有消息类型的定义。
    /// </summary>
    public class TalkBaseInfoTypes : ESPlus.Application.CustomizeInfo.BaseInformationTypes
    {
        public TalkBaseInfoTypes(int startKey)
        {
            base.StartKey = startKey;
            base.Initialize();
        }

        #region 0-99
        #region Function

        #region Chat
        private int chat = 0;
        /// <summary>
        /// 聊天信息 0
        /// </summary>
        public int Chat
        {
            get { return chat; }
            set { chat = value; }
        }
        #endregion

        #region Vibration
        private int vibration = 1;
        /// <summary>
        /// 窗口抖动 1
        /// </summary>
        public int Vibration
        {
            get { return vibration; }
            set { vibration = value; }
        }
        #endregion

        #region MediaCommunicate
        private int mediaCommunicate = 2;
        /// <summary>
        /// 关于视频/语音/远程协助的沟通。
        /// </summary>
        public int MediaCommunicate
        {
            get { return mediaCommunicate; }
            set { mediaCommunicate = value; }
        }
        #endregion

        #region InputingNotify
        private int inputingNotify = 3;
        /// <summary>
        /// 通知对方自己正在输入 （C->C）
        /// </summary>
        public int InputingNotify
        {
            get { return inputingNotify; }
            set { inputingNotify = value; }
        }
        #endregion

        #region GetOfflineMessage
        private int getOfflineMessage = 4;
        /// <summary>
        /// 请求离线消息 （C->S）
        /// </summary>
        public int GetOfflineMessage
        {
            get { return getOfflineMessage; }
            set { getOfflineMessage = value; }
        }
        #endregion

        #region OfflineMessage
        private int offlineMessage = 5;
        /// <summary>
        /// 服务端转发离线消息给客户端 （S->C）
        /// </summary>
        public int OfflineMessage
        {
            get { return offlineMessage; }
            set { offlineMessage = value; }
        }
        #endregion

        #region GetOfflineFile
        private int getOfflineFile = 6;
        /// <summary>
        /// 请求离线文件 （C->S）
        /// </summary>
        public int GetOfflineFile
        {
            get { return getOfflineFile; }
            set { getOfflineFile = value; }
        }
        #endregion

        #region OfflineFileResultNotify
        private int offlineFileResultNotify = 7;
        /// <summary>
        /// 通知发送方，对方是接收了离线文件，还是拒绝了离线文件。（S->C）
        /// </summary>
        public int OfflineFileResultNotify
        {
            get { return offlineFileResultNotify; }
            set { offlineFileResultNotify = value; }
        }
        #endregion        

        #region SystemMessage
        private int systemMessage = 8;
        /// <summary>
        /// 推送的系统消息。C->S、S->C
        /// </summary>
        public int SystemMessage
        {
            get { return systemMessage; }
            set { systemMessage = value; }
        }
        #endregion

        #region AudioMessage
        private int audioMessage = 10;
        /// <summary>
        ///  语音聊天消息。
        /// </summary>
        public int AudioMessage
        {
            get { return audioMessage; }
            set { audioMessage = value; }
        }
        #endregion
        #endregion        

        #region 个人资料、状态

        #region GetUserInfo
        private int getUserInfo = 21;
        /// <summary>
        /// 获取用户资料（C->S）
        /// </summary>
        public int GetUserInfo
        {
            get { return getUserInfo; }
            set { getUserInfo = value; }
        }
        #endregion

        #region ChangeMyBaseInfo
        private int changeMyBaseInfo = 22;
        /// <summary>
        /// 修改自己的基础个人资料（C->S）
        /// </summary>
        public int ChangeMyBaseInfo
        {
            get { return changeMyBaseInfo; }
            set { changeMyBaseInfo = value; }
        }
        #endregion

        #region UserBaseInfoChanged
        private int userBaseInfoChanged = 23;
        /// <summary>
        /// 通知某人的基础资料发生了变化（S->C）
        /// </summary>
        public int UserBaseInfoChanged
        {
            get { return userBaseInfoChanged; }
            set { userBaseInfoChanged = value; }
        }
        #endregion

        #region ChangeMyHeadImage
        private int changeMyHeadImage = 24;
        /// <summary>
        /// 修改自己的头像（C->S）
        /// </summary>
        public int ChangeMyHeadImage
        {
            get { return changeMyHeadImage; }
            set { changeMyHeadImage = value; }
        }
        #endregion

        #region UserHeadImageChanged
        private int userHeadImageChanged = 25;
        /// <summary>
        /// 用户头像发生变化。
        /// </summary>
        public int UserHeadImageChanged
        {
            get { return userHeadImageChanged; }
            set { userHeadImageChanged = value; }
        }
        #endregion

        #region GetSomeUsers
        private int getSomeUsers = 26;
        /// <summary>
        /// 获取指定某些用户的资料（C->S）
        /// </summary>
        public int GetSomeUsers
        {
            get { return getSomeUsers; }
            set { getSomeUsers = value; }
        }
        #endregion

        #region ChangeStatus
        private int changeStatus = 27;
        /// <summary>
        /// 修改状态（C->S）
        /// </summary>
        public int ChangeStatus
        {
            get { return changeStatus; }
            set { changeStatus = value; }
        }
        #endregion

        #region UserStatusChanged
        private int userStatusChanged = 28;
        /// <summary>
        /// 通知某人状态发生了变化（S->C）
        /// </summary>
        public int UserStatusChanged
        {
            get { return userStatusChanged; }
            set { userStatusChanged = value; }
        }
        #endregion

        #region ChangePassword
        private int changePassword = 29;
        /// <summary>
        /// 修改密码（C->S）
        /// </summary>
        public int ChangePassword
        {
            get { return changePassword; }
            set { changePassword = value; }
        }
        #endregion

        #region ChangeUnitCommentName
        private int changeUnitCommentName = 30;
        /// <summary>
        /// 修改好友/群的备注名称（C->S）。
        /// </summary>
        public int ChangeUnitCommentName
        {
            get { return changeUnitCommentName; }
            set { changeUnitCommentName = value; }
        }
        #endregion

        #region ChangeMyBusinessInfo
        private int changeMyBusinessInfo = 31;
        /// <summary>
        /// 修改自己的业务资料（C->S）
        /// </summary>
        public int ChangeMyBusinessInfo
        {
            get { return changeMyBusinessInfo; }
            set { changeMyBusinessInfo = value; }
        }
        #endregion

        #region UserBusinessInfoChanged
        private int userBusinessInfoChanged = 32;
        /// <summary>
        /// 通知某人的业务资料发生了变化（S->C）
        /// </summary>
        public int UserBusinessInfoChanged
        {
            get { return userBusinessInfoChanged; }
            set { userBusinessInfoChanged = value; }
        }
        #endregion

        #region GetUserInfoNewVersion
        private int getUserInfoNewVersion = 33;
        /// <summary>
        /// 获取用户新版本资料（C->S）。如果已经是最新，则服务器返回null。
        /// </summary>
        public int GetUserInfoNewVersion
        {
            get { return getUserInfoNewVersion; }
            set { getUserInfoNewVersion = value; }
        }
        #endregion

        #endregion

        #region contacts
        #region SomeoneRegisteredNotify
        private int someoneRegisteredNotify = 40;
        /// <summary>
        /// 当有新用户注册时，通知其它用户，适用GGTalkMode.Enterprise模式。（S->C）
        /// </summary>
        public int SomeoneRegisteredNotify
        {
            get { return someoneRegisteredNotify; }
            set { someoneRegisteredNotify = value; }
        }
        #endregion        

        #region GetFriendIDList
        private int getFriendIDList = 41;
        /// <summary>
        /// 获取我的所有好友ID（C->S）
        /// </summary>  
        public int GetFriendIDList
        {
            get { return getFriendIDList; }
            set { getFriendIDList = value; }
        }
        #endregion

        #region GetAllContacts
        private int getAllContacts = 42;
        /// <summary>
        /// 获取我的所有联系人资料（C->S）
        /// </summary>
        public int GetAllContacts
        {
            get { return getAllContacts; }
            set { getAllContacts = value; }
        }
        #endregion

        #region AddFriend
        private int addFriend = 43;
        /// <summary>
        /// 添加好友（C->S）
        /// </summary>
        public int AddFriend
        {
            get { return addFriend; }
            set { addFriend = value; }
        }
        #endregion

        #region RemoveFriend
        private int removeFriend = 44;
        /// <summary>
        /// 删除好友（C->S）
        /// </summary>
        public int RemoveFriend
        {
            get { return removeFriend; }
            set { removeFriend = value; }
        }
        #endregion

        #region FriendRemovedNotify
        private int friendRemovedNotify = 45;
        /// <summary>
        /// 通知客户端其被对方从好友中删除（S->C）
        /// </summary>
        public int FriendRemovedNotify
        {
            get { return friendRemovedNotify; }
            set { friendRemovedNotify = value; }
        }
        #endregion

        #region FriendAddedNotify
        private int friendAddedNotify = 46;
        /// <summary>
        /// 通知客户端其被对方添加为好友（S->C）
        /// </summary>
        public int FriendAddedNotify
        {
            get { return friendAddedNotify; }
            set { friendAddedNotify = value; }
        }
        #endregion

        #region InHisBlackList
        private int inHisBlackList = 47;
        /// <summary>
        /// 查询服务器自己是否位于对方的黑名单中（C->S）
        /// </summary>
        public int InHisBlackList
        {
            get { return inHisBlackList; }
            set { inHisBlackList = value; }
        }
        #endregion

        #region AddFriendCatalog
        private int addFriendCatalog = 48;
        /// <summary>
        /// 添加好友分组（C->S）
        /// </summary>
        public int AddFriendCatalog
        {
            get { return addFriendCatalog; }
            set { addFriendCatalog = value; }
        }
        #endregion

        #region ChangeFriendCatalogName
        private int changeFriendCatalogName = 49;
        /// <summary>
        /// 修改好友分组名称（C->S）
        /// </summary>
        public int ChangeFriendCatalogName
        {
            get { return changeFriendCatalogName; }
            set { changeFriendCatalogName = value; }
        }
        #endregion

        #region RemoveFriendCatalog
        private int removeFriendCatalog = 50;
        /// <summary>
        /// 删除好友分组（C->S）
        /// </summary>
        public int RemoveFriendCatalog
        {
            get { return removeFriendCatalog; }
            set { removeFriendCatalog = value; }
        }
        #endregion

        #region MoveFriendToOtherCatalog
        private int moveFriendToOtherCatalog = 51;
        /// <summary>
        /// 移动好友到别的分组（C->S）
        /// </summary>
        public int MoveFriendToOtherCatalog
        {
            get { return moveFriendToOtherCatalog; }
            set { moveFriendToOtherCatalog = value; }
        }
        #endregion

        #region GetContactsRTData
        private int getContactsRTData = 52;
        /// <summary>
        /// 获取我的所有联系人的在线状态、版本号，以及我的所有组的版本号（C->S）
        /// </summary>
        public int GetContactsRTData
        {
            get { return getContactsRTData; }
            set { getContactsRTData = value; }
        }
        #endregion

        #region GetAllContactIDs
        private int getAllContactIDs = 54;
        /// <summary>
        /// 获取我的所有联系人ID（C->S）
        /// </summary>
        public int GetAllContactIDs
        {
            get { return getAllContactIDs; }
            set { getAllContactIDs = value; }
        }
        #endregion

        #region DeleteUser
        private int deleteUser = 55;
        /// <summary>
        /// 删除用户。
        /// </summary>
        public int DeleteUser
        {
            get { return deleteUser; }
            set { deleteUser = value; }
        }
        #endregion

        #region SomeoneDeletedNotify
        private int someoneDeletedNotify = 56;
        /// <summary>
        /// 用户被删除时，通知其相关联系人。
        /// </summary>
        public int SomeoneDeletedNotify
        {
            get { return someoneDeletedNotify; }
            set { someoneDeletedNotify = value; }
        }
        #endregion
        #endregion

        #region Group

        #region GetMyGroups
        private int getMyGroups = 60;
        /// <summary>
        /// 获取我的所有组资料（C->S）
        /// </summary>
        public int GetMyGroups
        {
            get { return getMyGroups; }
            set { getMyGroups = value; }
        }
        #endregion

        #region GetSomeGroups
        private int getSomeGroups = 61;
        /// <summary>
        /// 获取指定的某些组的资料（C->S）
        /// </summary>
        public int GetSomeGroups
        {
            get { return getSomeGroups; }
            set { getSomeGroups = value; }
        }
        #endregion

        #region JoinGroup
        private int joinGroup = 62;
        /// <summary>
        /// 加入组（C->S）
        /// </summary>
        public int JoinGroup
        {
            get { return joinGroup; }
            set { joinGroup = value; }
        }
        #endregion

        #region GetGroup
        private int getGroup = 63;
        /// <summary>
        /// 获取组资料（C->S）
        /// </summary>
        public int GetGroup
        {
            get { return getGroup; }
            set { getGroup = value; }
        }
        #endregion

        #region CreateGroup
        private int createGroup = 64;
        /// <summary>
        /// 创建组（C->S）
        /// </summary>
        public int CreateGroup
        {
            get { return createGroup; }
            set { createGroup = value; }
        }
        #endregion

        #region QuitGroup
        private int quitGroup = 65;
        /// <summary>
        /// 退出组（C->S）
        /// </summary>
        public int QuitGroup
        {
            get { return quitGroup; }
            set { quitGroup = value; }
        }
        #endregion

        #region DeleteGroup
        private int deleteGroup = 66;
        /// <summary>
        /// 解散组（C->S）
        /// </summary>
        public int DeleteGroup
        {
            get { return deleteGroup; }
            set { deleteGroup = value; }
        }
        #endregion

        #region ChangeGroupMembers
        private int changeGroupMembers = 67;
        /// <summary>
        /// 编辑组成员
        /// </summary>
        public int ChangeGroupMembers
        {
            get { return changeGroupMembers; }
            set { changeGroupMembers = value; }
        }
        #endregion

        #region ChangeGroupInfo
        private int changeGroupInfo = 68;
        /// <summary>
        /// 修改组资料（名称、公告）
        /// </summary>
        public int ChangeGroupInfo
        {
            get { return changeGroupInfo; }
            set { changeGroupInfo = value; }
        }
        #endregion

        #region GroupFileUploadedNotify
        private int groupFileUploadedNotify = 69;
        /// <summary>
        /// 上传群文件完成通知
        /// </summary>
        public int GroupFileUploadedNotify
        {
            get { return groupFileUploadedNotify; }
            set { groupFileUploadedNotify = value; }
        }
        #endregion
        #endregion        

        #region Broadcast
        #region GroupChat
        private int groupChat = 80;
        /// <summary>
        /// 群聊天
        /// </summary>
        public int GroupChat
        {
            get { return groupChat; }
            set { groupChat = value; }
        }
        #endregion

        #region SomeoneJoinGroup
        private int someoneJoinGroup = 81;
        /// <summary>
        /// 某用户加入组
        /// </summary>
        public int SomeoneJoinGroup
        {
            get { return someoneJoinGroup; }
            set { someoneJoinGroup = value; }
        }
        #endregion

        #region SomeoneQuitGroup
        private int someoneQuitGroup = 82;
        /// <summary>
        /// 某用户退出组
        /// </summary>
        public int SomeoneQuitGroup
        {
            get { return someoneQuitGroup; }
            set { someoneQuitGroup = value; }
        }
        #endregion

        #region GroupDeleted
        private int groupDeleted = 83;
        /// <summary>
        /// 组被解散
        /// </summary>
        public int GroupDeleted
        {
            get { return groupDeleted; }
            set { groupDeleted = value; }
        }
        #endregion

        #region GroupInfoChanged
        private int groupInfoChanged = 84;
        /// <summary>
        /// 组资料被修改
        /// </summary>
        public int GroupInfoChanged
        {
            get { return groupInfoChanged; }
            set { groupInfoChanged = value; }
        }
        #endregion

        #region PulledIntoGroupNotify
        private int pulledIntoGroupNotify = 85;
        /// <summary>
        /// 某些人被拉入群。
        /// </summary>
        public int PulledIntoGroupNotify
        {
            get { return pulledIntoGroupNotify; }
            set { pulledIntoGroupNotify = value; }
        }
        #endregion

        #region RemovedFromGroupNotify
        private int removedFromGroupNotify = 86;
        /// <summary>
        /// 某些人被移除群。
        /// </summary>
        public int RemovedFromGroupNotify
        {
            get { return removedFromGroupNotify; }
            set { removedFromGroupNotify = value; }
        }
        #endregion
        #endregion

        #region 群禁言

        #region GroupBan4User
        private int groupBan4User = 87;

        /// <summary>
        /// 群禁言（针对具体群成员）
        /// </summary>
        public int GroupBan4User
        {
            get { return groupBan4User; }
            set { groupBan4User = value; }
        }
        #endregion  

        #region RemoveGroupBan4User
        private int removeGroupBan4User = 88;

        /// <summary>
        /// 解除群禁言（针对具体群成员）
        /// </summary>
        public int RemoveGroupBan4User
        {
            get { return removeGroupBan4User; }
            set { removeGroupBan4User = value; }
        }
        #endregion

        #region CheckGroupBan4CurrentUser
        private int checkGroupBan4CurrentUser = 89;

        /// <summary>
        /// 检查当前用户是否存在群禁言,若存在，会收到GroupBan4User的消息
        /// </summary>
        public int CheckGroupBan4CurrentUser
        {
            get { return checkGroupBan4CurrentUser; }
            set { checkGroupBan4CurrentUser = value; }
        }
        #endregion

        #region GetGroupBans4Group
        private int getGroupBans4Group = 90;

        /// <summary>
        /// 获取群组中所有正在禁言的记录
        /// </summary>
        public int GetGroupBans4Group
        {
            get { return getGroupBans4Group; }
            set { getGroupBans4Group = value; }
        }
        #endregion

        #region ExistAllGroupBan
        private int existAllGroupBan = 91;

        /// <summary>
        /// 该组是否存在全局禁言
        /// </summary>
        public int ExistAllGroupBan
        {
            get { return existAllGroupBan; }
            set { existAllGroupBan = value; }
        }

        #endregion


        #region GroupBan4Group
        private int groupBan4Group = 92;
        /// <summary>
        /// 开启全群禁言（管理员除外）
        /// </summary>
        public int GroupBan4Group
        {
            get { return groupBan4Group; }
            set { groupBan4Group = value; }
        }
        #endregion

        #region RemoveGroupBan4Group
        private int removeGroupBan4Group = 93;
        /// <summary>
        /// 关闭全群禁言
        /// </summary>
        public int RemoveGroupBan4Group
        {
            get { return removeGroupBan4Group; }
            set { removeGroupBan4Group = value; }
        }
        #endregion

        #endregion

        #region ChatTransfered
        private int chatTransfered = 71;
        /// <summary>
        /// 聊天信息转发。2019.07.23
        /// </summary>
        public int ChatTransfered
        {
            get { return chatTransfered; }
            set { chatTransfered = value; }
        }
        #endregion

        #region AddFriend,AddGroup
        #region RequestAddFriend
        private int requestAddFriend = 72;

        /// <summary>
        /// 申请添加好友。 2019.08.06
        /// </summary>
        public int RequestAddFriend
        {
            get { return requestAddFriend; }
            set { requestAddFriend = value; }
        }

        #endregion

        #region HandleAddFriend
        private int handleAddFriend = 73;

        /// <summary>
        /// 处理添加好友申请。 2019.08.06
        /// </summary>  
        public int HandleAddFriendRequest
        {
            get { return handleAddFriend; }
            set { handleAddFriend = value; }
        }
        #endregion

        #region GetAddFriendPage        
        private int getAddFriendPage = 74;
        /// <summary>
        /// 获取申请添加好友记录分页
        /// </summary>
        public int GetAddFriendPage
        {
            get { return getAddFriendPage; }
            set { getAddFriendPage = value; }
        }
        #endregion

        #region RequestAddGroup
        private int requestAddGroup = 75;
        /// <summary>
        /// 申请加入群组
        /// </summary>
        public int RequestAddGroup
        {
            get { return requestAddGroup; }
            set { requestAddGroup = value; }
        }
        #endregion

        #region HandleAddGroup
        private int handleAddGroup = 76;

        /// <summary>
        /// 处理加入群组的请求
        /// </summary>
        public int HandleAddGroupRequest
        {
            get { return handleAddGroup; }
            set { handleAddGroup = value; }
        }
        #endregion

        #region GetAddGroupPage
        private int getAddGroupPage = 77;
        /// <summary>
        /// 获取申请加入讨论组记录分页
        /// </summary>
        public int GetAddGroupPage
        {
            get { return getAddGroupPage; }
            set { getAddGroupPage = value; }
        }

        #endregion

        #region SearchGroup
        private int searchGroup = 78;
        /// <summary>
        /// 根据群ID或群名称精确查找群组
        /// </summary>
        public int SearchGroup
        {
            get { return searchGroup; }
            set { searchGroup = value; }
        }
        #endregion

        #region SearchUser
        private int searchUser = 79;

        /// <summary>
        /// 从DB中根据群ID或群名称精确查找用户
        /// </summary>
        public int SearchUser
        {
            get { return searchUser; }
            set { searchUser = value; }
        }
        #endregion
        #endregion

        #endregion

        #region 阅后即焚
        #region Snapchat
        private int snapchat = 100;
        /// <summary>
        /// 阅后即焚 消息
        /// </summary>
        public int Snapchat
        {
            get { return snapchat; }
            set { snapchat = value; }
        }
        #endregion

        #region SnapchatRead
        private int snapchatRead = 101;
        /// <summary>
        /// 阅后即焚 消息被阅读
        /// </summary>
        public int SnapchatRead
        {
            get { return snapchatRead; }
            set { snapchatRead = value; }
        }
        #endregion
        #endregion

        #region 管理员相关
        #region ChangeUserState
        private int changeUserState = 110;
        /// <summary>
        /// 更改用户状态（禁言、冻结、正常 等等） C->S
        /// </summary>
        public int ChangeUserState
        {
            get { return changeUserState; }
            set { changeUserState = value; }
        }
        #endregion

        #region UserStateChanged
        private int userStateChanged = 111;
        /// <summary>
        /// 用户状态已改变 S-> C
        /// </summary>
        public int UserStateChanged
        {
            get { return this.userStateChanged; }
            set { this.userStateChanged = value; }
        }
        #endregion
        #endregion

        #region 群离线消息
        #region GetGroupOfflineMessage
        private int getGroupOfflineMessage = 112;
        /// <summary>
        /// 请求群离线消息 （C->S）
        /// </summary>
        public int GetGroupOfflineMessage
        {
            get { return this.getGroupOfflineMessage; }
            set { this.getGroupOfflineMessage = value; }
        }
        #endregion

        #region GroupOfflineMessage
        private int groupOfflineMessage = 113;
        /// <summary>
        /// 服务端转发群离线消息给客户端 （S->C）
        /// </summary>
        public int GroupOfflineMessage
        {
            get { return this.groupOfflineMessage; }
            set { this.groupOfflineMessage = value; }
        }
        #endregion
        #endregion

        #region 聊天记录

        #region GetChatMessageRecord
        private int getChatMessageRecord = 120;
        /// <summary>
        /// 获取聊天记录 （C->S）
        /// </summary>
        public int GetChatMessageRecord
        {
            get { return this.getChatMessageRecord; }
            set { this.getChatMessageRecord = value; }
        }
        #endregion

        #region GetChatRecordPage
        private int getChatRecordPage = 121;
        /// <summary>
        /// 获取一页与好友的聊天记录 （C->S）
        /// </summary>
        public int GetChatRecordPage
        {
            get { return this.getChatRecordPage; }
            set { this.getChatRecordPage = value; }
        }
        #endregion

        #region GetGroupChatRecordPage
        private int getGroupChatRecordPage = 122;
        /// <summary>
        /// 获取一页群聊天记录 （C->S）
        /// </summary>
        public int GetGroupChatRecordPage
        {
            get { return this.getGroupChatRecordPage; }
            set { this.getGroupChatRecordPage = value; }
        }
        #endregion

        #endregion
    }
}
