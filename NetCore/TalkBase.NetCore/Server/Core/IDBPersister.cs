using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Security;
using ESBasic.ObjectManagement.Managers;
using System.Configuration;
using ESBasic;
using ESFramework;

namespace TalkBase.Server
{
    /// <summary>
    /// 与数据库交互的持久化器接口。
    /// </summary>
    public interface IDBPersister<TUser, TGroup> : IChatRecordPersister
    {
        void InsertUser(TUser t);
        void DeleteUser(string userID);

        /// <summary>
        /// 更新用户的好友字段，内存中的user的版本号已经加1。
        /// </summary>       
        void UpdateUserFriends(TUser t);
        /// <summary>
        /// 更新用户的组字段，内存中的user的版本号已经加1。
        /// </summary>    
        void UpdateUserGroups(TUser t);
        /// <summary>
        /// 更新用户的备注名称字段，内存中的user的版本号已经加1。
        /// </summary>    
        void UpdateUserCommentNames(TUser t);
        /// <summary>
        /// 更新用户的基础信息，内存中的user的版本号已经加1。
        /// </summary>    
        void UpdateUserInfo(string userID, string name, string signature, string orgID, int version);
        /// <summary>
        /// 更新用户的头像，内存中的user的版本号已经加1。
        /// </summary>    
        void UpdateUserHeadImage(string userID, int defaultHeadImageIndex, byte[] customizedHeadImage, int version);
        /// <summary>
        /// 更新PC端最后离线时间
        /// </summary>
        /// <param name="userID"></param>
        void UpdateUserPcOfflineTime(string userID, DateTime dateTime);

        /// <summary>
        /// 更新手机最后离线时间
        /// </summary>
        /// <param name="userID"></param>
        void UpdateUserMobileOfflineTime(string userID, DateTime dateTime);

        /// <summary>
        /// 更改用户的业务资料。
        /// </summary>      
        void UpdateUserBusinessInfo(TUser user, Dictionary<string, byte[]> businessInfo, int version);

        void UpdateUserPassword(string userID, string newPasswordMD5);
        string GetUserPassword(string userID);

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userState"></param>
        /// <param name="version"></param>
        void ChangeUserState(string userID, UserState userState, int version);
        TUser GetUser(string userID);

        List<TUser> SearchUser(string idOrName);

        List<TUser> GetAllUser();

        /// <summary>
        /// 根据电话号码获取用户信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        TUser GetUser4Phone(string phone);

        void InsertGroup(TGroup t);
        /// <summary>
        /// 更新组的成员字段，内存中的group的版本号已经加1。
        /// </summary>    
        void UpdateGroupMembers(TGroup t);
        /// <summary>
        /// 更新组的基础信息，内存中的group的版本号已经加1。
        /// </summary> 
        void UpdateGroupInfo(TGroup t);

        void DeleteGroup(string groupID);
        TGroup GetGroup(string groupID);

        List<TGroup> SearchGroup(string idOrName);
        List<TGroup> GetAllGroup();

        #region OfflineMessage相关

        /// <summary>
        /// 提取目标用户的所有离线消息,并从DB中删除。
        /// </summary>       
        /// <param name="destUserID">接收离线消息用户的ID</param>
        /// <returns>属于目标用户的离线消息列表，按时间升序排列</returns>
        List<OfflineMessage> PickupOfflineMessage(string destUserID);


        /// <summary>
        /// 从ChatMessageRecord表中提取离线记录
        /// </summary>
        /// <param name="destUserID">目标用户</param>
        /// <param name="startTime">提取的开始时间</param>
        /// <returns>属于目标用户的离线消息列表，按时间升序排列</returns>
        Dictionary<string, List<ChatMessageRecord>> PickupGroupOfflineChatMessageRecord(string destUserID, DateTime startTime);
        void StoreOfflineMessage(OfflineMessage msg);
        void StoreOfflineFileItem(OfflineFileItem item);



        /// <summary>
        /// 从数据库中提取接收者为指定用户的所有离线文件条目 ,并从DB中删除。
        /// </summary>
        /// <param name="accepterID"></param>
        /// <returns></returns>
        List<OfflineFileItem> PickupOfflineFileItem(string accepterID);

        /// <summary>
        /// 提取文件管理组手中的离线文件条目，并从DB中删除
        /// </summary>
        /// <param name="accepterID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        List<OfflineFileItem> PickupOfflineFileItem4Assistant(string accepterID, ClientType type);

        #endregion

        /// <summary>
        /// DB中插入 好友申请记录
        /// </summary>
        /// <param name="requesterID">申请者ID</param>
        /// <param name="accepterID">接收者ID</param>
        /// <param name="requesterCatalogName">申请者的好友分组</param>
        /// <param name="comment">备注</param>
        /// <param name="isNotified">是否已通知对方</param>
        void InsertAddFriendRequest(string requesterID, string accepterID, string requesterCatalogName, string comment,bool isNotified);

        /// <summary>
        /// 根据编号设置 DB中的好友申请记录为已通知
        /// </summary>
        /// <param name="requesterID">申请者ID</param>
        /// <param name="accepterID">接收者ID</param>
        void SetAddFriendRequestNotified(string requesterID,string accepterID);

        /// <summary>
        /// 更新DB中好友申请记录
        /// </summary>
        /// <param name="requesterID">申请者ID</param>
        /// <param name="accepterID">接收者ID</param>
        /// <param name="requesterCatalogName">申请者的好友分组</param>
        /// <param name="accepterCatalogName">接收者的好友分组</param>
        /// <param name="isAgreed">是否同意</param>
        void UpdateAddFriendRequest(string requesterID, string accepterID, string requesterCatalogName, string accepterCatalogName, bool isAgreed);

        /// <summary>
        /// 获取 DB中 有关该用户的还未通知完成的 好友申请记录
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<AddFriendRequest> GetAddFriendRequest4NotNotified(string userID);


        /// <summary>
        /// 获取有关该用户的好友申请分页信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        AddFriendRequestPage GetAddFriendRequestPage(string userID, int pageIndex, int pageSize);

        /// <summary>
        /// 获取申请者选择的好友分组
        /// </summary>
        /// <param name="requesterID"></param>
        /// <param name="accepterID"></param>
        /// <returns></returns>
        string GetRequesterCatalogName(string requesterID, string accepterID);

        /// <summary>
        /// DB中插入 加入讨论组申请记录
        /// </summary>
        /// <param name="requesterID"></param>
        /// <param name="groupID"></param>
        /// <param name="accepterID"></param>
        /// <param name="comment"></param>
        /// <param name="isNotified">是否已通知接收者</param>
        void InsertAddGroupRequest(string requesterID, string groupID, string accepterID, string comment, bool isNotified);

        /// <summary>
        /// 根据编号设置 DB中的加入讨论组申请记录为已通知
        /// </summary>
        /// <param name="requesterID">申请者ID</param>
        /// <param name="groupID"></param>
        void SetAddGroupRequestNotified(string requesterID, string groupID);

        /// <summary>
        /// 更新申请加入讨论组记录
        /// </summary>
        /// <param name="requesterID"></param>
        /// <param name="groupID"></param>
        /// <param name="isAgreed"></param>
        void UpdateAddGroupRequest(string requesterID, string groupID, bool isAgreed);

        /// <summary>
        /// 删除申请加入讨论组的记录
        /// </summary>
        /// <param name="groupID"></param>
        void DeleteAddGroupRequest(string groupID);

        /// <summary>
        /// 获取 DB中 有关该用户的还未通知完成的 申请加入讨论组记录
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<AddGroupRequest> GetAddGroupRequest4NotNotified(string userID);

        /// <summary>
        /// 获取有关该用户的申请加入讨论组记录分页信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        AddGroupRequestPage GetAddGroupRequestPage(string userID, int pageIndex, int pageSize);

        /// <summary>
        /// DB中插入群禁言记录
        /// </summary>
        /// <param name="groupBan"></param>
        /// <returns>返回 DB中插入的autoID</returns>
        void InsertGroupBan4User(GroupBan groupBan);

        /// <summary>
        /// 删除群中对应的人员的禁言记录
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="userID"></param>
        void DeleteGroupBan4User(string groupID,string userID);

        /// <summary>
        /// 获取群禁言记录
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        GroupBan GetGroupBan4User(string groupID, string userID);

        /// <summary>
        /// 获取某群组中所有禁言记录
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        List<GroupBan> GetGroupBans4Group(string groupID);

        /// <summary>
        /// 该群组是否存在全局禁言
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        bool ExistAllGroupBan(string groupID);
    }

}
