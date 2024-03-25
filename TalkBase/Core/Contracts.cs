using System;
using System.Collections.Generic;
using System.Text;


namespace TalkBase
{
    /*
     * TalkBase中用到的通信协议类。
     */

    public class OperateContract
    {
        public OperateContract() { }
        public OperateContract(string operatorID, string userID)
        {
            this.OperatorUserID = operatorID;
            this.TargetUserID = userID;
        }

        public string OperatorUserID { get; set; }
        public string TargetUserID { get; set; }
    }

    public class SystemMessageContract
    {
        public SystemMessageContract() { }
        public SystemMessageContract(string sourceUserID, string unitID, int sysMsgType, byte[] content, string tag ,bool useOfflineMessageMode)
        {
            this.SourceUserID = sourceUserID;
            this.ReceiverUnitID = unitID;
            this.SystemMessageType = sysMsgType;
            this.Content = content;
            this.Tag = tag;
            this.UseOfflineMessageMode = useOfflineMessageMode;
        }

        public string SourceUserID { get; set; }
        public string ReceiverUnitID { get; set; }
        public int SystemMessageType { get; set; }
        public byte[] Content { get; set; }
        public string Tag { get; set; }
        public bool UseOfflineMessageMode { get; set; }
    }
    public class ChangeHeadImageContract
    {
        public ChangeHeadImageContract() { }
        public ChangeHeadImageContract(string userID, int headImageIndex, byte[] headImage)
        {
            this.UserID = userID;
            this.HeadImageIndex = headImageIndex;
            this.HeadImage = headImage;
        }
        public string UserID { get; set; }
        public int HeadImageIndex { get; set; }
        public byte[] HeadImage { get; set; }

        /// <summary>
        /// 当服务端向好友发送通知前，设置该字段为最新的版本号。
        /// </summary>
        public int UserLatestVersion { get; set; }
    }

    public class ChangeUserBaseInfoContract
    {
        public ChangeUserBaseInfoContract() { }
        public ChangeUserBaseInfoContract(string userID, string name, string signature, string orgID)
        {
            this.UserID = userID;
            this.Name = name;
            this.Signature = signature;
            this.OrgID = orgID;           
        }

        public string UserID { get; set; }
        public string Name { get; set; }
        public string Signature { get; set; }
        public string OrgID { get; set; }
        /// <summary>
        /// 当服务端向好友发送通知前，设置该字段为最新的版本号。
        /// </summary>
        public int UserLatestVersion { get; set; }
    }

    public class ChangeUserBusinessInfoContract
    {
        public ChangeUserBusinessInfoContract() { }
        public ChangeUserBusinessInfoContract(string userID, Dictionary<string, byte[]> businessInfo, bool notifyContacts, bool increaseVersion)
        {
            this.UserID = userID;
            this.BusinessInfo = businessInfo;           
            this.NotifyContacts = notifyContacts;
            this.IncreaseVersion = increaseVersion;
        }

        public string UserID { get; set; }
        public Dictionary<string, byte[]> BusinessInfo { get; set; }       
        public bool NotifyContacts { get; set; }
        public bool IncreaseVersion { get; set; }
    }

    public class UserBusinessInfoChangedNotifyContract
    {
        public UserBusinessInfoChangedNotifyContract() { }
        public UserBusinessInfoChangedNotifyContract(string userID, Dictionary<string, byte[]> businessInfo, int latestVersion)
        {
            this.UserID = userID;
            this.BusinessInfo = businessInfo;           
            this.LatestVersion = latestVersion;            
        }

        public string UserID { get; set; }
        public Dictionary<string, byte[]> BusinessInfo { get; set; }
        public int LatestVersion { get; set; }       
    }

    public class MediaCommunicateContract
    {     
        public CommunicateMediaType CommunicateMediaType { get; set; }
        public CommunicateType CommunicateType { get; set; }
        public string Tag { get; set; }
        /// <summary>
        /// -1表示所有设备
        /// </summary>
        public int DestClientType { get; set; }

        public MediaCommunicateContract() { }
        public MediaCommunicateContract(CommunicateMediaType media, CommunicateType type, string tag ,int clientType)
        {
            this.CommunicateMediaType = media;
            this.CommunicateType = type;
            this.Tag = tag;
            this.DestClientType = clientType;
        }
    }

    public class OfflineFileResultNotifyContract
    {
        public OfflineFileResultNotifyContract() { }
        public OfflineFileResultNotifyContract(string accepterID, string file, bool accept)
        {
            this.AccepterID = accepterID;
            this.FileName = file;
            this.Accept = accept;
        }
        public string AccepterID { get; set; }

        public string FileName { get; set; }

        public bool Accept { get; set; }
    }

    public class RequestAddGroupContract
    {
        public RequestAddGroupContract() { }
        public RequestAddGroupContract(string requesterID, string groupID, string comment)
        {
            this.RequesterID = requesterID;
            this.GroupID = groupID;
            this.Comment = comment;
        }

        public string RequesterID { get; set; }
        public string GroupID { get; set; }
        public string Comment { get; set; }
    }

    public class HandleAddGroupRequestContract
    {
        public HandleAddGroupRequestContract() { }
        public HandleAddGroupRequestContract(string requesterID, string groupID, bool agree)
        {
            this.RequesterID = requesterID;
            this.GroupID = groupID;
            this.IsAgreed = agree;
        }
        /// <summary>
        /// 申请人
        /// </summary>
        public string RequesterID { get; set; }
        public string GroupID { get; set; }
        public bool IsAgreed { get; set; }
    }

    public class GetAddGroupPageContract
    {
        public GetAddGroupPageContract() { }

        public GetAddGroupPageContract(string userID, int pageIndex, int pageSize)
        {
            this.UserID = userID;
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }
        public string UserID { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }


    public class CreateGroupContract
    {
        public CreateGroupContract() { }
        public CreateGroupContract(GroupType type, string id, string name, string announce, List<string> members, bool isPrivate)
        {
            this.GroupType = type;
            this.ID = id;
            this.Name = name;
            this.Announce = announce;
            this.Members = members;
            this.IsPrivate = isPrivate;
        }

        public GroupType GroupType { get; set; }

        public string ID { get; set; }

        public string Name { get; set; }
       
        public string Announce { get; set; }

        public List<string> Members { get; set; }
        public bool IsPrivate { get; set; }
    }

    public class ChangeGroupMembersContract
    {
        public ChangeGroupMembersContract() { }
        public ChangeGroupMembersContract(string groupID, List<string> added, List<string> removed)
        {            
            this.GroupID = groupID;
            this.AddedMembers = added;
            this.RemovedMembers = removed;
        }
       
        public string GroupID { get; set; }
        public List<string> AddedMembers { get; set; }
        public List<string> RemovedMembers { get; set; }
    }

    public class ChangeGroupInfoContract
    {
        public ChangeGroupInfoContract() { }
        public ChangeGroupInfoContract(string operatorID ,string groupID, string name, string announce)
        {
            this.OperatorID = operatorID;
            this.GroupID = groupID;
            this.GroupName = name;
            this.Announce = announce;
        }

        public string OperatorID { get; set; }

        public string GroupID { get; set; }

        public string GroupName { get; set; }

        public string Announce { get; set; }
    }

    public class GroupFileUploadedNotifyContract
    {
        public GroupFileUploadedNotifyContract() { }
        public GroupFileUploadedNotifyContract(string userID, string groupID, string file)
        {
            this.UserID = userID;
            this.GroupID = groupID;
            this.FileName = file;          
        }

        public string UserID { get; set; }

        public string GroupID { get; set; }

        public string FileName { get; set; }
    }

    public class ChangePasswordContract
    {
        public ChangePasswordContract() { }
        public ChangePasswordContract(string oldPasswordMD5, string newPasswordMD5)
        {
            this.OldPasswordMD5 = oldPasswordMD5;
            this.NewPasswordMD5 = newPasswordMD5;
        }

        public string OldPasswordMD5 { get; set; }

        public string NewPasswordMD5 { get; set; }
    }

    public class UserStatusChangedContract
    {
        public UserStatusChangedContract() { }
        public UserStatusChangedContract(string userID, int newStatus)
        {
            this.UserID = userID;
            this.NewStatus = newStatus;
        }

        public string UserID { get; set; }

        public int NewStatus { get; set; }
    }

    public class ContactsRTDataContract : ContactRTDatas
    {
        public ContactsRTDataContract() { }
        public ContactsRTDataContract(Dictionary<string, UserRTData> dic ,Dictionary<string, int> gVersion)
        {
            this.UserStatusDictionary = dic;
            this.GroupVersionDictionary = gVersion;            
        }
    }

    public class RequestAddFriendContract
    {
        public RequestAddFriendContract() { }
        public RequestAddFriendContract(string requesterID,  string accepterID, string comment,string requesterCatalogName)
        {
            this.RequesterID = requesterID;
            this.AccepterID = accepterID;
            this.Comment = comment;
            this.RequesterCatalogName = requesterCatalogName;
        }

        public string RequesterID { get; set; }
        public string AccepterID { get; set; }
        public string RequesterCatalogName { get; set; }
        public string Comment { get; set; }
    }

    public class HandleAddFriendRequestContract
    {
        public HandleAddFriendRequestContract() { }
        public HandleAddFriendRequestContract(string requesterID, string accepterID, string accepterCatalogName, bool agree)
        {
            this.RequesterID = requesterID;
            this.AccepterID = accepterID;
            this.AccepterCatalogName = accepterCatalogName;
            this.IsAgreed = agree;
        }

        public string RequesterID { get; set; }

        public string AccepterID { get; set; }

        public string AccepterCatalogName { get; set; }

        public String RequesterCatalogName { get; set; }

        public bool IsAgreed { get; set; }
    }
         

    public class GetAddFriendPageContract
    {
        public GetAddFriendPageContract() { }

        public GetAddFriendPageContract(string userID, int pageIndex, int pageSize)
        {
            this.UserID = userID;
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }
        public string UserID { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class AddFriendContract
    {
        public AddFriendContract() { }
        public AddFriendContract(string friendID, string catalog)
        {
            this.FriendID = friendID;
            this.CatalogName = catalog;
        }

        public string FriendID { get; set; }
        public string CatalogName { get; set; }
    }

    public class ChangeCatalogContract
    {
        public ChangeCatalogContract() { }
        public ChangeCatalogContract(string oldName, string newName)
        {
            this.OldName = oldName;
            this.NewName = newName;
        }

        public string OldName { get; set; }
        public string NewName { get; set; }
    }

    public class MoveFriendToOtherCatalogContract
    {
        public MoveFriendToOtherCatalogContract() { }
        public MoveFriendToOtherCatalogContract(string friendID, string oldCatalog, string newCatalog)
        {
            this.FriendID = friendID;
            this.OldCatalog = oldCatalog;
            this.NewCatalog = newCatalog;
        }

        public string FriendID{ get; set; }
        public string OldCatalog { get; set; }
        public string NewCatalog { get; set; }
    }

    public class ManageGroupMembersNotifyContract
    {
        public ManageGroupMembersNotifyContract() { }
        public ManageGroupMembersNotifyContract(string operatorID, List<string> guests, bool serverCommand)
        {
            this.OperatorID = operatorID;
            this.GuestList = guests;
            this.ServerCommand = serverCommand;
        }

        public bool ServerCommand { get; set; }
        public string OperatorID { get; set; }
        public List<string> GuestList { get; set; }
    }

    public class ChangeCommentNameContract
    {
        public ChangeCommentNameContract() { }
        public ChangeCommentNameContract(string unitID, string name)
        {
            this.UnitID = unitID;
            this.CommentName = name;
        }
        public string UnitID { get; set; }
        public string CommentName { get; set; }
    }

    public class GetUserInfoNewVersionContract
    {
        public GetUserInfoNewVersionContract() { }
        public GetUserInfoNewVersionContract(string userID, int version)
        {
            this.UserID = userID;
            this.Version = version;
        }
        public string UserID { get; set; }
        public int  Version { get; set; }
    }

    public class GroupBan4UserContract
    {
        public GroupBan4UserContract() { }
        /// <summary>
        /// 群禁言协议体
        /// </summary>
        /// <param name="groupID">群ID</param>
        /// <param name="operatorID">操作人ID</param>
        /// <param name="userID">被禁言人ID</param>
        /// <param name="comment">备注</param>
        /// <param name="minutes">禁言时长（单位：分钟）</param>
        public GroupBan4UserContract(string groupID, string operatorID, string userID, string comment, double minutes)
        {
            this.GroupID = groupID;
            this.OperatorID = operatorID;
            this.UserID = userID;
            this.Comment = comment;
            this.Minutes = minutes;
        }

        public string GroupID { get; set; }
        public string OperatorID { get; set; }
        public string UserID { get; set; }
        public string Comment { get; set; }
        /// <summary>
        /// 禁言时长（单位：分钟）
        /// </summary>
        public double Minutes { get; set; }
    }

    public class RemoveGroupBan4UserContract
    {
        public RemoveGroupBan4UserContract() { }
        public RemoveGroupBan4UserContract(string groupID,string userID)
        {
            this.GroupID = groupID;
            this.UserID = userID;
        }

        public string GroupID { get; set; }
        public string UserID { get; set; }
    }

    public class ChangeUserStateContract
    {
        public ChangeUserStateContract() { }
        public ChangeUserStateContract(string userID, UserState userState)
        {
            this.UserID = userID;
            this.UserState = userState;
        }
        public string UserID { get; set; }
        public UserState UserState { get; set; }
    }


    public class GetChatRecordPageContract
    {
        public ChatRecordTimeScope TimeScope { get; set; }
        public string MyID { get; set; }
        public string FriendID { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public GetChatRecordPageContract() { }
        public GetChatRecordPageContract(ChatRecordTimeScope timeScope, string myID, string friendID, int pageSize, int pageIndex)
        {
            this.TimeScope = timeScope;
            this.MyID = myID;
            this.FriendID = friendID;
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
        }
    }

    public class GetGroupChatRecordPageContract
    {
        public ChatRecordTimeScope TimeScope { get; set; }
        public string GroupID { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public GetGroupChatRecordPageContract() { }
        public GetGroupChatRecordPageContract(ChatRecordTimeScope timeScope, string groupID, int pageSize, int pageIndex)
        {
            this.TimeScope = timeScope;
            this.GroupID = groupID;
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
        }
    }
}
