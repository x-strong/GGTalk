using System;
using System.Collections.Generic;
using System.Text;
using ESBasic;
using System.Xml.Serialization;
using System.ComponentModel;
using SqlSugar;

namespace TalkBase
{
    /// <summary>
    /// 用户实体基础类。
    /// </summary>
    [Serializable]
    public class UserBase :IUser, INotifyPropertyChanged
    {
        #region Force Static Check
        public const string _UserID = "UserID";        
        public const string _Name = "Name";
        public const string _OrgID = "OrgID";
        public const string _Signature = "Signature";
        public const string _HeadImageIndex = "HeadImageIndex";
        public const string _HeadImageData = "HeadImageData";
        public const string _Groups = "Groups";
        public const string _Friends = "Friends";
        public const string _CommentNames = "CommentNames";
        public const string _UserState = "UserState";
        public const string _PcOfflineTime = "PcOfflineTime";
        public const string _MobileOfflineTime = "MobileOfflineTime";
        public const string _CreateTime = "CreateTime";
        public const string _Version = "Version";
        #endregion

        #region UserID
        private string userID = "";
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        [SugarColumn(IsIgnore = true)]
        public string ID
        {
            get
            {
                return this.userID;
            }
        }
        #endregion

        #region Name
        private string name = "";
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(CommentName)));
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
                }
            }
        }
        #endregion

        #region Signature
        private string signature = "";
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature
        {
            get { return signature; }
            set { signature = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Signature)));
                }
            }
        }
        #endregion

        #region HeadImageIndex
        private int headImageIndex = 0;
        /// <summary>
        /// 头像图片的索引。如果为-1，表示自定义头像。
        /// </summary>
        public int HeadImageIndex
        {
            get { return headImageIndex; }
            set
            {
                headImageIndex = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(HeadImageIndex)));
                }
            }
        }
        #endregion

        #region HeadImageData
        private byte[] headImageData = null;
        public byte[] HeadImageData
        {
            get { return headImageData; }
            set
            {
                headImageData = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(HeadImageData)));
                }
            }
        }
        #endregion              

        #region Friends
        private string friends = "";
        /// <summary>
        /// 好友。如 我的好友：10000,10001,1234;家人:1200,1201 。
        /// </summary>
        public string Friends
        {
            get { return friends; }
            set
            {
                friends = value;
                this.friendDicationary = null;
                this.allFriendList = null;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Friends)));
                }
            }
        }

        #region 非DB字段
        private Dictionary<string, List<string>> friendDicationary = null;
        /// <summary>
        /// 好友ID的分组。非DB字段。
        /// </summary>
        [XmlIgnore]
        [SugarColumn(IsIgnore = true)]
        internal Dictionary<string, List<string>> FriendDicationary
        {
            get
            {
                if (this.friendDicationary == null)
                {
                    if (string.IsNullOrEmpty(this.friends))
                    {
                        this.friends = "我的好友:";
                        if (FunctionOptions.BlackList)
                        {
                            this.friends += string.Format(";{0}:", FunctionOptions.BlackListCatalogName);
                        }
                    }
                    this.friendDicationary = new Dictionary<string, List<string>>();
                    string[] catalogs = this.friends.Split(';');
                    foreach (string catalog in catalogs)
                    {
                        if (string.IsNullOrEmpty(catalog))
                        {
                            break;
                        }

                        string[] ary = catalog.Split(':');
                        string catalogName = ary[0];
                        List<string> friendList = new List<string>(ary[1].Split(','));
                        if (friendList.Count == 1)
                        {
                            friendList.Remove("");
                        }
                        this.friendDicationary.Add(catalogName, friendList);
                    }

                    if (FunctionOptions.BlackList)
                    {
                        if (!this.friendDicationary.ContainsKey(FunctionOptions.BlackListCatalogName))
                        {
                            this.friendDicationary.Add(FunctionOptions.BlackListCatalogName, new List<string>());
                            this.friends += string.Format(";{0}:", FunctionOptions.BlackListCatalogName);
                        }
                    }
                }
                return friendDicationary;
            }
        }

        private List<string> allFriendList = null;
        public List<string> GetAllFriendList()
        {
            if (this.allFriendList == null)
            {
                List<string> list = new List<string>();
                foreach (List<string> tmp in this.FriendDicationary.Values)
                {
                    list.AddRange(tmp);
                }
                this.allFriendList = list;
            }
            return new List<string>(this.allFriendList);
        }

        public bool IsFriend(string userID)
        {
            foreach (List<string> tmp in this.FriendDicationary.Values)
            {
                if (tmp.Contains(userID))
                {
                    return true;
                }
            }

            return false;
        }

        private string GetFriendsVal(Dictionary<string, List<string>> friendDic)
        {
            StringBuilder sb = new StringBuilder("");
            int count = 0;
            foreach (KeyValuePair<string, List<string>> pair in friendDic)
            {
                if (count > 0)
                {
                    sb.Append(";");
                }
                string ff = ESBasic.Helpers.StringHelper.ContactString(pair.Value, ",");
                sb.Append(string.Format("{0}:{1}", pair.Key, ff));
                ++count;
            }
            return sb.ToString();
        }
        #endregion

        public string GetFriendCatalog(string friendID)
        {
            foreach (KeyValuePair<string, List<string>> pair in this.FriendDicationary)
            {
                if (pair.Value.Contains(friendID))
                {
                    return pair.Key;
                }
            }

            return null;
        }

        public void AddFriend(string friendID, string catalog)
        {
            if (!this.FriendDicationary.ContainsKey(catalog))
            {
                return;
            }
            if (this.FriendDicationary[catalog].Contains(friendID))
            {
                return;
            }

            this.FriendDicationary[catalog].Add(friendID);
            this.friends = this.GetFriendsVal(this.friendDicationary);
            this.allFriendList = null;
        }

        public void RemoveFriend(string friendID)
        {
            foreach (KeyValuePair<string, List<string>> pair in this.FriendDicationary)
            {
                pair.Value.Remove(friendID);
            }

            this.friends = this.GetFriendsVal(this.friendDicationary);
            this.allFriendList = null;
        }

        public void ChangeFriendCatalogName(string oldName, string newName)
        {
            if (!this.FriendDicationary.ContainsKey(oldName))
            {
                return;
            }

            List<string> merged = new List<string>();
            if (this.FriendDicationary.ContainsKey(newName))
            {
                merged = this.FriendDicationary[newName];
                this.FriendDicationary.Remove(newName);
            }
            List<string> friends = this.friendDicationary[oldName];
            friends.AddRange(merged);
            this.FriendDicationary.Remove(oldName);
            this.FriendDicationary.Add(newName, friends);
            this.friends = this.GetFriendsVal(this.friendDicationary);           
        }

        public void AddFriendCatalog(string name)
        {
            if (this.FriendDicationary.ContainsKey(name))
            {
                return;
            }

            this.FriendDicationary.Add(name, new List<string>());
            this.friends = this.GetFriendsVal(this.friendDicationary);
        }

        public void RemvoeFriendCatalog(string name)
        {
            if (!this.FriendDicationary.ContainsKey(name) || name == FunctionOptions.BlackListCatalogName || name == FunctionOptions.DefaultFriendCatalog)
            {
                return;
            }

            this.FriendDicationary.Remove(name);
            this.friends = this.GetFriendsVal(this.friendDicationary);
        }

        public void MoveFriend(string friendID, string oldCatalog, string newCatalog)
        {
            if (!this.FriendDicationary.ContainsKey(oldCatalog) || !this.FriendDicationary.ContainsKey(newCatalog))
            {
                return;
            }
            this.friendDicationary[oldCatalog].Remove(friendID);
            if (!this.friendDicationary[newCatalog].Contains(friendID))
            {
                this.friendDicationary[newCatalog].Add(friendID);
            }
            this.friends = this.GetFriendsVal(this.friendDicationary);
        }

        public List<string> GetFriendCatalogList()
        {
            return new List<string>(this.FriendDicationary.Keys);
        }

        public bool IsInBlackList(string userID)
        {
            if (!this.FriendDicationary.ContainsKey(FunctionOptions.BlackListCatalogName))
            {
                return false;
            }
            return this.FriendDicationary[FunctionOptions.BlackListCatalogName].Contains(userID);
        }
        #endregion        

        #region Groups
        private string groups = "";
        /// <summary>
        /// 该用户所属的组。组ID用英文逗号隔开
        /// </summary>
        public string Groups
        {
            get { return groups; }
            set
            {
                groups = value;
                this.groupList = null;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Groups)));
                }
            }
        }

        #region 非DB字段
        private List<string> groupList = null;
        /// <summary>
        /// 所属组ID的数组。非DB字段。
        /// </summary>
        [XmlIgnore]
        [SugarColumn(IsIgnore = true)]
        public List<string> GroupList
        {
            get
            {
                if (this.groupList == null)
                {
                    this.groupList = new List<string>(this.groups.Split(','));
                    if (this.groupList.Count == 1)
                    {
                        this.groupList.Remove("");
                    }
                }
                return groupList;
            }
        }
        #endregion
        private object locker = new object();
        public void JoinGroup(string groupID)
        {
            lock (this.locker)
            {
                if (this.GroupList.Contains(groupID))
                {
                    return;
                }
                this.GroupList.Add(groupID);
                this.groups = ESBasic.Helpers.StringHelper.ContactString(this.GroupList, ",");
            }
        }

        public void QuitGroup(string groupID)
        {
            lock (this.locker)
            {
                this.GroupList.Remove(groupID);
                this.groups = ESBasic.Helpers.StringHelper.ContactString(this.GroupList, ",");
            }
        }
        #endregion

        #region OrgID
        private string orgID = "";
        /// <summary>
        /// 所属部门
        /// </summary>
        public string OrgID
        {
            get { return orgID; }
            set { orgID = value; }
        }        
        #endregion

        #region CommentNames
        private string commentNames = "";
        /// <summary>
        /// 为好友设定的备注名。格式：
        /// 10001:宝宝;10002:贝贝
        /// </summary>
        public string CommentNames
        {
            get { return commentNames; }
            set
            {
                commentNames = value;
                this.commandNameDictionary = null;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(CommentNames)));
                }
            }
        }

        #region CommandNameDictionary        
        private Dictionary<string, string> commandNameDictionary = null;
        /// <summary>
        /// 好友或群的备注名称。key：unitID，val：commentName
        /// </summary>      
        [XmlIgnore]
        [SugarColumn(IsIgnore = true)]
        public Dictionary<string, string> CommandNameDictionary
        {
            get
            {
                if (this.commandNameDictionary != null)
                {
                    return this.commandNameDictionary;
                }

                if (string.IsNullOrEmpty(this.commentNames))
                {
                    this.commandNameDictionary = new Dictionary<string, string>();
                }
                else
                {
                    this.commandNameDictionary = ESBasic.Helpers.StringHelper.SplitStringToDictionary<string, string>(this.commentNames, ';', ':');
                }

                return this.commandNameDictionary;
            }
        }
        #endregion

        /// <summary>
        /// 增加或更新好友备注。如果commentName为null或String.Empty，则表示取消备注。
        /// </summary>      
        public void ChangeUnitCommentName(string unitID, string commentName)
        {
            if (this.CommandNameDictionary.ContainsKey(unitID))
            {
                this.CommandNameDictionary.Remove(unitID);
            }
            if (!string.IsNullOrEmpty(commentName))
            {
                this.CommandNameDictionary.Add(unitID, commentName);
            }
            this.commentNames = ESBasic.Helpers.StringHelper.CombinDictionaryIntoString<string, string>(this.CommandNameDictionary, ';', ':');
        }

        public string GetUnitCommentName(string unitID)
        {
            if (this.CommandNameDictionary.ContainsKey(unitID))
            {
                return this.commandNameDictionary[unitID];
            }

            return null;
        }
        #endregion

        #region UserState
        private UserState userState = UserState.Normal;
        /// <summary>
        /// 用户状态 （正常，冻结，禁言，停用）
        /// </summary>
        public UserState UserState
        {
            get { return this.userState; }
            set { this.userState = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(UserState)));
                }
            }
        }
        #endregion

        #region Version
        private int version = 0;
        public int Version
        {
            get { return version; }
            set { version = value; }
        }
        #endregion

        #region CreateTime
        private DateTime createTime = DateTime.Now;
        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }
        #endregion

        #region PcOfflineTime

        private DateTime pcOfflineTime = DateTime.Now;
        /// <summary>
        /// PC端最后离线时间
        /// </summary>

        [XmlIgnore]
        [ESPlus.Serialization.NotSerializedCompactly]
        public DateTime PcOfflineTime
        {
            get { return pcOfflineTime; }
            set { pcOfflineTime = value; }
        }

        #endregion

        #region MobileOfflineTime
        /// <summary>
        /// 移动端最后离线时间
        /// </summary>

        private DateTime mobileOfflineTime = DateTime.Now;

        [XmlIgnore]
        [ESPlus.Serialization.NotSerializedCompactly]
        public DateTime MobileOfflineTime { get { return mobileOfflineTime; } set { mobileOfflineTime = value; } }
        #endregion


        #region Tag
        private object tag = null;
        [XmlIgnore]
        [SugarColumn(IsIgnore = true)]
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        #endregion

        #region 非DB字段
        #region PartialCopy
        [NonSerialized]
        private UserBase partialCopy = null;
        [XmlIgnore]
        [SugarColumn(IsIgnore = true)]
        public IUser PartialCopy
        {
            get
            {
                if (this.partialCopy == null)
                {
                    this.partialCopy = (UserBase)this.MemberwiseClone();
                    this.partialCopy.CommentNames = "";
                    this.partialCopy.Groups = "";
                    this.partialCopy.Friends = "";
                }
                this.partialCopy.UserStatus = this.userStatus; //2015.04.15
                this.partialCopy.UserState = this.userState;//2021.07.20
                return partialCopy;
            }
        }
        #endregion

        #region UserStatus       
        private UserStatus userStatus = UserStatus.OffLine;
        /// <summary>
        /// 在线状态。非DB字段。
        /// </summary>  
        [XmlIgnore]
        [SugarColumn(IsIgnore = true)]
        public UserStatus UserStatus
        {
            get { return userStatus; }
            set { userStatus = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(UserStatus)));
                }
            }
        }
        #endregion 

        #region CommentName        
        private string commentName = null;
        /// <summary>
        /// 用于客户端，为好友设定的备注名称。
        /// </summary>
        [XmlIgnore]
        [SugarColumn(IsIgnore = true)]
        public string CommentName
        {
            get { return commentName; }
            set { commentName = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(CommentName)));
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
                }
            }
        }
        #endregion

        #region DisplayName
        [XmlIgnore]
        [SugarColumn(IsIgnore = true)]
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.CommentName))
                {
                    return this.CommentName;
                }

                return this.Name;
            }
        }
        #endregion

        #region IsGroup
        [XmlIgnore]
        [SugarColumn(IsIgnore = true)]
        public bool IsGroup
        {
            get { return false; }
        } 
        #endregion       


        #region LastWordsRecord 不应该加入Compact序列化。
        private LastWordsRecord lastWordsRecord;

        public event PropertyChangedEventHandler PropertyChanged;

        [XmlIgnore]
        [SugarColumn(IsIgnore = true)]
        public virtual LastWordsRecord LastWordsRecord
        {
            get { return lastWordsRecord; }
            set { lastWordsRecord = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(LastWordsRecord)));
                }
            }
        } 
        #endregion     

        public Parameter<string, string> GetIDName()
        {
            return new Parameter<string, string>(this.UserID, this.Name);
        }
        #endregion           

        public void ChangeHeadImage(int defaultHeadImageIndex, byte[] customizedHeadImage)
        {
            this.HeadImageIndex = defaultHeadImageIndex;
            this.HeadImageData = customizedHeadImage;
        }

        public void DeletePartialCopy()
        {
            this.partialCopy = null;
        }

        public virtual void ReplaceOldUnit(IUnit old)
        {
        }

        public virtual void UpdateBusinessInfo(Dictionary<string, byte[]> businessInfo)
        {
        }


        [XmlIgnore]
        [SugarColumn(IsIgnore = true)]
        public UnitType UnitType
        {
            get { return TalkBase.UnitType.User; }
        }

        [XmlIgnore]
        [SugarColumn(IsIgnore = true)]
        public bool IsUser
        {
            get { return true; }
        }
    }
}
