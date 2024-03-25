using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DataRabbit;
using ESBasic.Helpers;
using ESBasic;
using TalkBase;
using ESFramework.Boost.Controls;
using GGTalk.Core;
using SqlSugar;

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

        #region GGValue
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

}