using DataRabbit;
using ESBasic;
using ESFramework.Boost.Controls;
using SqlSugar;
using System;
using System.Collections.Generic;
using TalkBase;

namespace GGTalk
{
    //各个实体定义，对应数据库中的数据表。

    #region User
    [Serializable]
    public class GGUser : UserBase, IEntity<System.String>
    {
        #region Force Static Check
        public const string TableName = "GGUser";
        public const string _PasswordMD5 = "PasswordMD5";
        public const string _Phone = "Phone";
        #endregion

        #region Ctor
        public GGUser() { }
        public GGUser(string id, string pwd, string _name, string _friends, string _orgID, string _signature, int headIndex, string _groups)
        {
            this.SetProperty(id, pwd, _name, _friends, _orgID, _signature, headIndex, _groups);
        }

        public GGUser(string id, string pwd, string _name, string _friends, string _orgID, string _signature, int headIndex, string _groups ,string _phone)
        {
            this.SetProperty(id, pwd, _name, _friends, _orgID, _signature, headIndex, _groups);
            this.phone = _phone;
        }

        private void SetProperty(string id, string pwd, string _name, string _friends, string _orgID, string _signature, int headIndex, string _groups)
        {
            this.UserID = id;
            this.passwordMD5 = pwd;
            this.Name = _name;
            this.Friends = _friends;
            this.Signature = _signature;
            this.OrgID = _orgID;
            this.HeadImageIndex = headIndex;
            this.Groups = _groups;
        }
        #endregion

        #region Phone
        private string phone = "";

        /// <summary>
        /// 电话号码 10.29
        /// </summary>
        [ESPlus.Serialization.NotSerializedCompactly]
        public string Phone
        {
            get { return this.phone; }
            set { this.phone = value; }
        } 
        #endregion

        #region PasswordMD5
        private string passwordMD5 = "";
        /// <summary>
        /// 登录密码(MD5加密)。
        /// </summary>
        public string PasswordMD5
        {
            get { return passwordMD5; }
            set { passwordMD5 = value; }
        }
        #endregion    

        #region OnlineOrHide
        [SugarColumn(IsIgnore = true)]
        public bool OnlineOrHide
        {
            get
            {
                return this.UserStatus != UserStatus.OffLine;
            }
        }
        #endregion

        #region OfflineOrHide
        [SugarColumn(IsIgnore = true)]
        public bool OfflineOrHide
        {
            get
            {
                return this.UserStatus == UserStatus.OffLine || this.UserStatus == UserStatus.Hide;
            }
        }
        #endregion

        #region LastWordsRecord
        [SugarColumn(IsIgnore = true)]
        public override LastWordsRecord LastWordsRecord
        {
            get { return base.LastWordsRecord; }
            set
            {
                base.LastWordsRecord = value;
                this.lastChatBoxContent = null;
            }
        }

        [NonSerialized]
        private ChatBoxContent lastChatBoxContent = null;
        [SugarColumn(IsIgnore = true)]
        public ChatBoxContent LastChatBoxContent
        {
            get
            {
                if (this.lastChatBoxContent == null)
                {
                    if (base.LastWordsRecord != null && base.LastWordsRecord.ChatContent != null)
                    {
                        this.lastChatBoxContent = ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<ChatBoxContent>(base.LastWordsRecord.ChatContent, 0);
                    }
                }
                return this.lastChatBoxContent;
            }
        }
        #endregion

        public System.String GetPKeyValue()
        {
            return this.UserID;
        } 

        public override string ToString()
        {
            return string.Format("{0}({1})-{2}，Ver：{3}" ,this.Name,this.UserID,this.UserStatus,this.Version);
        }                   
    }
    #endregion

    #region GGGroup
    [Serializable]
    public class GGGroup : GroupBase ,IEntity<System.String>
    {
        #region Force Static Check
        public const string TableName = "GGGroup";        
        #endregion        

        #region Ctor
        public GGGroup() { }
        public GGGroup(string id, string _name, string _creator, string _announce, string _members,bool _isPrivate)
        {
            this.GroupID = id;
            this.Name = _name;
            this.CreatorID = _creator;
            this.Announce = _announce;
            this.Members = _members;
            this.IsPrivate = _isPrivate;
        } 
        #endregion

        #region LastWordsRecord
        [SugarColumn(IsIgnore = true)]
        public override LastWordsRecord LastWordsRecord
        {
            get { return base.LastWordsRecord; }
            set
            {
                base.LastWordsRecord = value;
                this.lastChatBoxContent = null;
            }
        }

        [NonSerialized]
        private ChatBoxContent lastChatBoxContent = null;
        [SugarColumn(IsIgnore = true)]
        public ChatBoxContent LastChatBoxContent
        {
            get
            {
                if (this.lastChatBoxContent == null)
                {
                    if (base.LastWordsRecord != null && base.LastWordsRecord.ChatContent != null)
                    {
                        this.lastChatBoxContent = ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<ChatBoxContent>(base.LastWordsRecord.ChatContent, 0);
                    }
                }
                return this.lastChatBoxContent;
            }
        }
        #endregion

        public System.String GetPKeyValue()
        {
            return this.GroupID;
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", this.Name, this.GroupID);
        }        
    } 
    #endregion    

    #region GGConfiguration

    [Serializable]
    public class GGConfiguration : IEntity<System.String>
    {
        #region Force Static Check
        public const string TableName = "GGConfiguration";
        public const string _GGKey = "GGKey";
        public const string _GGValue = "GGValue";
        #endregion

        #region GGKey
        private string ggKey = "";
        public string GGKey
        {
            get { return ggKey; }
            set { ggKey = value; }
        }
        #endregion

        #region ggValue
        private string ggValue = "";
        public string GGValue
        {
            get { return ggValue; }
            set { ggValue = value; }
        }
        #endregion

        public string GetPKeyValue()
        {
            return this.ggKey;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.ggKey, this.ggValue);
        }
    }
    #endregion

    #region SensitiveRecord
    [Serializable]
    public partial class SensitiveRecord : IEntity<System.Int32>
    {
        #region Force Static Check
        public const string TableName = "SensitiveRecord";
        public const string _AutoID = "AutoID";
        public const string _ChatMessageRecordID = "ChatMessageRecordID";
        public const string _SpeakerID = "SpeakerID";
        public const string _AudienceID = "AudienceID";
        public const string _IsGroupChat = "IsGroupChat";
        public const string _ContentImage = "ContentImage";
        public const string _ContentText = "ContentText";
        public const string _ContentType = "ContentType";
        public const string _EvilType = "EvilType";
        public const string _Keywords = "Keywords";
        public const string _Score = "Score";
        public const string _Handled = "Handled";
        public const string _CreateTime = "CreateTime";
        #endregion

        #region Property

        #region AutoID
        private System.Int32 m_AutoID = 0;

        public System.Int32 AutoID
        {
            get
            {
                return this.m_AutoID;
            }
            set
            {
                this.m_AutoID = value;
            }
        }
        #endregion

        #region ChatMessageRecordID
        private System.Int32 m_ChatMessageRecordID = 0;
        public System.Int32 ChatMessageRecordID
        {
            get
            {
                return this.m_ChatMessageRecordID;
            }
            set
            {
                this.m_ChatMessageRecordID = value;
            }
        }
        #endregion

        #region SpeakerID
        private System.String m_SpeakerID = "";
        public System.String SpeakerID
        {
            get
            {
                return this.m_SpeakerID;
            }
            set
            {
                this.m_SpeakerID = value;
            }
        }
        #endregion

        #region EvilType
        private EvilType m_EvilType = EvilType.Normal;
        public EvilType EvilType
        {
            get
            {
                return this.m_EvilType;
            }
            set
            {
                this.m_EvilType = value;
            }
        }


        #endregion

        /// <summary>
        /// 收听者
        /// </summary>
        public string AudienceID { get; set; }

        /// <summary>
        /// 是否为群聊天
        /// </summary>
        public bool IsGroupChat { get; set; }
        #region Keywords
        private System.String m_Keywords = "";
        public System.String Keywords
        {
            get
            {
                return this.m_Keywords;
            }
            set
            {
                this.m_Keywords = value;
            }
        }
        #endregion
        /// <summary>
        /// 分值范围0--100，分数越越明显
        /// </summary>
        public int Score { get; set; }

        public bool Handled { get; set; }

        public byte[] ContentImage { get; set; }

        public string ContentText { get; set; }

        /// <summary>
        /// 聊天内容类型  0：Text  1：Image
        /// </summary>
        public int ContentType { get; set; }

        #region CreateTime
        private System.DateTime m_CreateTime = DateTime.Now;
        public System.DateTime CreateTime
        {
            get
            {
                return this.m_CreateTime;
            }
            set
            {
                this.m_CreateTime = value;
            }
        }
        #endregion
        #endregion

        #region IEntity Members
        public System.Int32 GetPKeyValue()
        {
            return this.AutoID;
        }
        #endregion
    }
    #endregion

    #region SensitiveRecordPage
    [Serializable]
    public class SensitiveRecordPage
    {
        public SensitiveRecordPage() { }
        public SensitiveRecordPage(int total, int index, List<SensitiveRecord> _page)
        {
            this.totalCount = total;
            this.pageIndex = index;
            this.content = _page;
        }

        #region TotalCount
        private int totalCount = 0;
        public int TotalCount
        {
            get { return totalCount; }
            set { totalCount = value; }
        }
        #endregion

        #region PageIndex
        private int pageIndex = 0;
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }
        #endregion

        #region Content
        private List<SensitiveRecord> content = new List<SensitiveRecord>();

        public List<SensitiveRecord> Content
        {
            get { return content; }
            set { content = value; }
        }
        #endregion
    }
    #endregion


}