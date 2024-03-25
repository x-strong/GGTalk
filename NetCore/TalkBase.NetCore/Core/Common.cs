using System;
using System.Collections.Generic;
using System.Text;
using ESBasic;
using ESBasic.ObjectManagement.Managers;
using System.Drawing;
using ESFramework;
using ESFramework.Boost.Controls;
using SqlSugar;

namespace TalkBase
{
    #region IUnitTypeRecognizer
    /// <summary>
    /// Unit类型识别器。
    /// </summary>
    public interface IUnitTypeRecognizer
    {
        /// <summary>
        /// 根据UnitID识别Unit的类型。
        /// </summary>       
        UnitType Recognize(string unitID);
    } 
    #endregion

    #region IUnit
    /// <summary>
    /// Unit基础接口。在TalkBase中，有三种类型的Unit：用户、群组、组织结构节点。
    /// </summary>
    public interface IUnit
    {
        string ID { get; }
        string Name { get; set; }
        int Version { get; set; }
        UnitType UnitType { get; }
        
        bool IsUser { get; }

        /// <summary>
        /// 【用于客户端】为好友/群设定的备注名称。
        /// </summary>
        string CommentName { get; set; }

        /// <summary>
        /// 【用于客户端】用于在客户端的好友列表中显示的名称。
        /// 如果CommentName不为null，则显示CommentName，否则显示Name。
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// 【用于客户端】用于记录与每个Unit的最后一句交谈记录。
        /// </summary>
        LastWordsRecord LastWordsRecord { get; set; }

        /// <summary>
        /// 【用于客户端】当BaseGlobalUserCache从服务器加载了最新版本的Unit以取代缓存中的旧对象时，会调用此方法。
        /// （比如，此时可以将本地缓存的一些扩展字段的值从old拷贝到新的Unit）
        /// </summary>        
        void ReplaceOldUnit(IUnit old);

        /// <summary>
        /// 修改业务资料。Key:业务信息项 ,Value:业务信息数据。
        /// </summary>       
        void UpdateBusinessInfo(Dictionary<string, byte[]> businessInfo);


        Parameter<string, string> GetIDName();
    }
    #endregion

    #region IUser
    /// <summary>
    /// 用户基础接口。
    /// </summary>
    public interface IUser : IUnit
    {
        List<string> GroupList { get; }
        UserStatus UserStatus { get; set; }

        string GetFriendCatalog(string friendID);
        string GetUnitCommentName(string unitID);

        string Signature { get; set; }       

        string OrgID { get; set; }

        /// <summary>
        /// 用户使用状态
        /// </summary>
        UserState UserState { get; set; }

        bool IsFriend(string userID);
        List<string> GetAllFriendList();
        List<string> GetFriendCatalogList();
        void ChangeHeadImage(int defaultHeadImageIndex, byte[] customizedHeadImage);
        DateTime PcOfflineTime { get; set; }
        DateTime MobileOfflineTime { get; set; }

        #region IUserBase
        /// <summary>
        /// 当前User的部分复制，Friends/Groups/CommentNames字段都为空、某些不需要同步的业务字段最好也设置为null。
        /// </summary>
        IUser PartialCopy { get; }

        byte[] HeadImageData { get; }
        int HeadImageIndex { get; }

        void AddFriend(string friendID, string catalog);
        void RemoveFriend(string friendID);
        void ChangeFriendCatalogName(string oldName, string newName);
        void AddFriendCatalog(string name);
        void RemvoeFriendCatalog(string name);
        void MoveFriend(string friendID, string oldCatalog, string newCatalog);
        void JoinGroup(string groupID);
        void QuitGroup(string groupID);
        bool IsInBlackList(string userID);

        /// <summary>
        /// 由于PartialCopy有缓存，所以，每次修改资料后，应调用此方法将缓存的PartialCopy删除。2015.04.15
        /// </summary>
        void DeletePartialCopy();

        /// <summary>
        /// 增加或更新好友备注。如果commentName为null或String.Empty，则表示取消备注。
        /// </summary>      
        void ChangeUnitCommentName(string unitID, string commentName);
        #endregion
    }
    #endregion

    #region IGroup
    /// <summary>
    /// 群、讨论组 基础接口。
    /// </summary>
    public interface IGroup : IUnit
    {
        GroupType GroupType { get; }
        string CreatorID { get; }
        DateTime CreateTime { get; }

        List<string> MemberList { get; }

        void AddMember(string userID);
        void RemoveMember(string userID);

        string Announce { get; set; }
        void ChangeMembers(List<string> members);

    }

    /// <summary>
    /// 组的类型。
    /// </summary>
    public enum GroupType
    {
        /// <summary>
        /// 相当于QQ的群。
        /// </summary>
        CommonGroup = 0,
        /// <summary>
        /// 相当于QQ的讨论组。
        /// </summary>
        DiscussGroup,
        /// <summary>
        /// 语音聊天室。
        /// </summary>
        AudioGroup,
        /// <summary>
        /// 视频会议
        /// </summary>
        VideoMeeting
    }
    #endregion        

    #region IUnitInfoProvider
    /// <summary>
    /// Unit基础信息提供者接口。
    /// </summary>
    public interface IUnitInfoProvider
    {
        /// <summary>
        /// 获取Unit的头像。（Unit可能是用户，也可能是群）
        /// </summary>  
        Image GetHeadImage(IUnit unit);

        /// <summary>
        /// 获取Unit所在分组的组名。（Unit可能是用户，也可能是群）
        /// </summary>        
        string GetCatalog(IUnit unit);
    } 
    #endregion

    #region IUserInformationForm
    /// <summary>
    /// 用于显示用户基础信息的窗口。
    /// </summary>
    public interface IUserInformationForm
    {
        void SetUser(IUser user, Point location);
    } 
    #endregion

    #region IGroupInformationForm
    /// <summary>
    /// 用于显示群组基础信息的窗口。
    /// </summary>
    public interface IGroupInformationForm
    {
        void SetGroup(IGroup group, Point location);
    } 
    #endregion

    #region ContactRTDatas
    public class UserRTData
    {
        public UserRTData() { }
        public UserRTData(UserStatus status, int ver)
        {
            this.UserStatus = status;
            this.Version = ver;
        }

        public UserStatus UserStatus { get; set; }
        public int Version { get; set; }
    }

    public class ContactRTDatas
    {
        public Dictionary<string, UserRTData> UserStatusDictionary { get; set; }
        public Dictionary<string, int> GroupVersionDictionary { get; set; }
    }
    #endregion

    #region LastWordsRecord
    /// <summary>
    /// 交谈最后一句话的记录。
    /// </summary>
    [Serializable]
    public class LastWordsRecord
    {
        public LastWordsRecord() { }
        public LastWordsRecord(string _speakerID, string _audienceID, bool group, byte[] content)
        {
            this.speakerID = _speakerID;
            this.audienceID = _audienceID;
            this.isGroup = group;
            this.chatContent = content;
        }

        #region SpeakerID
        private string speakerID;
        public string SpeakerID
        {
            get { return speakerID; }
            set { speakerID = value; }
        }
        #endregion

        #region AudienceID
        private string audienceID;
        public string AudienceID
        {
            get { return audienceID; }
            set { audienceID = value; }
        }
        #endregion

        #region IsGroup
        private bool isGroup = false;
        public bool IsGroup
        {
            get { return isGroup; }
            set { isGroup = value; }
        }
        #endregion

        #region ChatContent
        private byte[] chatContent;
        public byte[] ChatContent
        {
            get { return chatContent; }
            set { chatContent = value; }
        }
        #endregion

        #region SpeakTime
        private DateTime speakTime = DateTime.Now;
        public DateTime SpeakTime
        {
            get { return speakTime; }
            set { speakTime = value; }
        }
        #endregion

        //public string ContentText
        //{
        //    get
        //    {
        //        if (this.ChatContent == null)
        //        {
        //            return "";
        //        }
        //        ChatBoxContent chatBoxContent = ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<ChatBoxContent>(this.ChatContent, 0);
        //        return chatBoxContent.GetTextWithPicPlaceholder("[图]");
        //    }
        //}
    }
    #endregion

    #region OfflineMessage
    /// <summary>
    /// 离线消息。
    /// </summary>
    [Serializable]
    public class OfflineMessage
    {
        #region Force Static Check
        public const string TableName = "OfflineMessage";
        public const string _SourceUserID = "SourceUserID";
        public const string _SourceType = "SourceType";
        public const string _DestUserID = "DestUserID";
        public const string _GroupID = "GroupID";
        public const string _InformationType = "InformationType";
        public const string _Information = "Information";
        public const string _Tag = "Tag";
        public const string _TimeTransfer = "TimeTransfer";
        #endregion


        #region Ctor
        public OfflineMessage() { }
        public OfflineMessage(string _sourceUserID, ClientType _sourceType, string _destUserID, string _groupID, int _informationType, byte[] info)
        {
            this.sourceUserID = _sourceUserID;
            this.sourceType = _sourceType;
            this.destUserID = _destUserID;
            this.groupID = _groupID ?? "";
            this.informationType = _informationType;
            this.information = info;
        }

        public OfflineMessage(string _sourceUserID, ClientType _sourceType, string _destUserID, string _groupID, int _informationType, byte[] info ,string _tag)
        {
            this.sourceUserID = _sourceUserID;
            this.sourceType = _sourceType;
            this.destUserID = _destUserID;
            this.groupID = _groupID ?? "";
            this.informationType = _informationType;
            this.information = info;
            this.tag = _tag;
        }
        #endregion

        #region SourceUserID
        private string sourceUserID = "";
        /// <summary>
        /// 发送离线消息的用户ID。
        /// </summary>
        public string SourceUserID
        {
            get { return sourceUserID; }
            set { sourceUserID = value; }
        }
        #endregion

        #region SourceType
        private ClientType sourceType = ClientType.DotNET;
        public ClientType SourceType
        {
            get { return sourceType; }
            set { sourceType = value; }
        } 
        #endregion

        #region DestUserID
        private string destUserID = "";
        /// <summary>
        /// 接收离线消息的用户ID。
        /// </summary>
        public string DestUserID
        {
            get { return destUserID; }
            set { destUserID = value; }
        }
        #endregion

        #region GroupID
        private string groupID = "";
        /// <summary>
        /// 该字段用于群离线消息
        /// </summary>
        public string GroupID
        {
            get { return groupID; }
            set { groupID = value ?? ""; }
        }
        #endregion

        #region InformationType
        private int informationType = 0;
        /// <summary>
        /// 信息的类型。
        /// </summary>
        public int InformationType
        {
            get { return informationType; }
            set { informationType = value; }
        }
        #endregion

        #region Information
        private byte[] information;
        /// <summary>
        /// 信息内容
        /// </summary>
        public byte[] Information
        {
            get { return information; }
            set { information = value; }
        }
        #endregion

        #region Tag
        private string tag = string.Empty;
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        } 
        #endregion

        #region Time
        private DateTime time = DateTime.Now;
        /// <summary>
        /// 服务器接收到要转发离线消息的时间。
        /// </summary>
        public DateTime TimeTransfer
        {
            get { return time; }
            set { time = value; }
        }
        #endregion
    }
    #endregion

    #region GroupOfflineMessage
    [Serializable]
    public class GroupOfflineMessage
    {
        public string GroupID { get; set; }

        public List<OfflineMessage> OfflineMessages { get; set; }
    }
    #endregion

    #region OfflineFileItem
    /// <summary>
    /// 离线文件条目
    /// </summary>
    public class OfflineFileItem
    {
        #region Force Static Check
        public const string TableName = "OfflineFileItem";
        public const string _AutoID = "AutoID";
        public const string _FileName = "FileName";
        public const string _FileLength = "FileLength";
        public const string _SenderID = "SenderID";
        public const string _SenderType = "SenderType";
        public const string _AccepterType = "AccepterType";
        public const string _AccepterID = "AccepterID";
        public const string _RelayFilePath = "RelayFilePath";
        #endregion

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        /// <summary>
        /// 条目的唯一编号，数据库自增序列，主键。
        /// </summary>
        public string AutoID { get; set; }

        /// <summary>
        /// 离线文件的名称。
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件的大小。
        /// </summary>
        public int FileLength { get; set; }

        /// <summary>
        /// 发送者ID。
        /// </summary>
        public string SenderID { get; set; }

        /// <summary>
        /// 发送者设备。
        /// </summary>
        public ClientType SenderType { get; set; }

        /// <summary>
        /// 接收者设备。（用于多端助手的文件传送，如果是他人发给自己的离线文件，则忽略该字段）
        /// </summary>
        public ClientType AccepterType { get; set; }

        /// <summary>
        /// 接收者ID。
        /// </summary>
        public string AccepterID { get; set; }

        /// <summary>
        /// 在服务器上存储离线文件的临时路径。
        /// </summary>
        public string RelayFilePath { get; set; }
    }
    #endregion


    #region SnapchatMessage
    /// <summary>
    /// 阅后自焚消息
    /// </summary>
    [Serializable]
    public class SnapchatMessage
    {
        public SnapchatMessage() { }
        public SnapchatMessage(string id, string creator, ChatBoxContent content)
        {
            this.ID = id;
            this.CreatorID = creator;
            this.ChatBoxContent = content;
        }

        //
        // 摘要:
        //     消息编号。
        public string ID { get; set; }

        //
        // 摘要:
        //     短信创建者的ID。
        public string CreatorID { get; set; }

        //
        // 摘要:
        //     内容数据列表。
        public ChatBoxContent ChatBoxContent { get; set; }
        //
        // 摘要:
        //     携带的数据。（比如可用于在群内语音消息）
        public string Tag { get; set; }

        //
        // 摘要:
        //     获取唯一编号。CreatorID - ID
        public string GetUniqueID()
        {
            return this.CreatorID + "-" + this.ID;
        }
    }

    /// <summary>
    /// 阅后即焚消息已被阅读
    /// </summary>
    [Serializable]
    public class SnapchatMessageRead
    {

        public SnapchatMessageRead() { }
        public SnapchatMessageRead(string messageID, string creatorID)
        {
            this.MessageID = messageID;
            this.SourceCreatorID = creatorID;
        }

        public string MessageID { get; set; }
        public string SourceCreatorID { get; set; }
    }

    #endregion





}
