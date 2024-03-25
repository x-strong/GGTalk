using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.ObjectManagement.Managers;
using ESPlus.Rapid;
using ESPlus.Serialization;
using ESBasic;
using System.IO;
using System.Threading;
using ESBasic.Loggers;

namespace TalkBase.Client
{
    /// <summary>
    /// 前提：UserID 与 GroupID 不能重复。
    /// 客户端全局缓存，在内存中缓存用户、群组。且管理所有本地联系人（好友、组友）、组的资料、状态。
    /// （1）当前登录用户的资料不缓存，每次登录从服务器获取最新。
    /// （2）当前登录用户修改个人资料时，服务器会将其Version增加1。如果仅仅是好友列表/所属组发生变化，则Version不会增加。
    /// </summary>
    internal abstract class BaseGlobalCache<TUser, TGroup>
        where TUser : TalkBase.IUser
        where TGroup : TalkBase.IGroup
    {      
        private string persistenceFilePath = "";
        private int pageSize4LoadFriends = 20;
        private ObjectManager<string, TUser> userManager = new ObjectManager<string, TUser>(); //缓存用户资料
        private ObjectManager<string, TGroup> groupManager = new ObjectManager<string, TGroup>();
  
        private UserLocalPersistence<TUser, TGroup> originUserLocalPersistence;
        private IAgileLogger logger;
        private IUnitTypeRecognizer unitTypeRecognizer;

        /// <summary>
        /// 自己的业务资料发生了变化（在触发此事件之前，已经更新了缓存中User的对应业务信息）。参数: key , info
        /// </summary>
        public event CbGeneric<Dictionary<string, byte[]>> MyBusinessInfoChanged;
        public event CbGeneric MyBaseInfoChanged;
        public event CbGeneric MyStatusChanged;
        /// <summary>
        /// 自己已经被删除。事件参数：OperatorID
        /// 处理此事件时，应该提示用户，然后退出客户端程序。
        /// </summary>
        public event CbGeneric<string> MyselfDeleted;

        /// <summary>
        /// 开始从服务器批量加载数据。
        /// </summary>
        public event CbGeneric BatchLoadStarted;
        /// <summary>
        /// 从服务器批量加载数据完成。
        /// </summary>
        public event CbGeneric BatchLoadCompleted;
       
        /// <summary>
        /// 被对方添加为好友，参数：sourceUserID。托盘闪动前触发。
        /// </summary>
        public event CbGeneric<TUser> FriendAdded;
        public event CbGeneric<TUser> AddFriendSucceed;
        public event CbGeneric<TUser> UserRegistered;
        public event CbGeneric<TUser> UserDeleted;
        public event CbGeneric<TGroup, GroupChangedType, string, string> GroupChanged; //group - type - operatorID - target     
        public event CbGeneric<string, string, string> FriendCatalogChanged; // FriendID - OldCatalogName - NewCatalogName

        internal void OnFriendCatalogChanged(string friendID, string oldCatalogName , string newCatalogName)
        {
            if(this.FriendCatalogChanged != null)
            {
                this.FriendCatalogChanged(friendID, oldCatalogName, newCatalogName);
            }
        }
        public event CbGeneric<string> AddFriendCatalog;//catalogName
        internal void OnAddFriendCatalog(string catalogName)
        {
            if (this.AddFriendCatalog != null)
            {
                this.AddFriendCatalog(catalogName);
            }
        }

        /// <summary>
        /// 用户的业务资料发生了变化（在触发此事件之前，已经更新了缓存中User的对应业务信息）。
        /// </summary>
        public event CbGeneric<TUser, Dictionary<string, byte[]>> UserBusinessInfoChanged;
        public event CbGeneric<TUser> UserBaseInfoChanged;
        public event CbGeneric<TUser> UserStatusChanged;       
        public event CbGeneric<string> FriendRemoved;
        
        public event CbGeneric<IUnit> UnitCommentNameChanged;        

        #region CurrentUser
        protected TUser currentUser;
        public TUser CurrentUser
        {
            get { return currentUser; }
        }
        #endregion        

        #region QuickAnswerList
        private List<string> quickAnswerList = new List<string>();
        /// <summary>
        /// 快捷回复列表。
        /// </summary>
        public List<string> QuickAnswerList
        {
            get { return quickAnswerList ?? new List<string>(); }           
        }
        #endregion               
      
        protected abstract TGroup DoGetGroup(string groupID);
        protected abstract TUser DoGetUser(string userID);

        protected abstract List<TGroup> DoGetMyGroups();
        protected abstract List<TGroup> DoGetSomeGroups(List<string> groupIDList);
        protected abstract ContactRTDatas DoGetContactsRTDatas();
        protected abstract List<TUser> DoGetSomeUsers(List<string> userIDList);
        protected abstract List<TUser> DoGetAllContacts(); //好友，包括组友   
        protected abstract List<string> DoGetAllContactIDs(); //好友，包括组友   

        #region Ctor
        public virtual void Initialize(string curUserID, string persistencePath,IUnitTypeRecognizer recognizer, IAgileLogger _logger )
        {
            this.BatchLoadStarted += delegate { };           
            this.GroupChanged += delegate { };
            this.UserBaseInfoChanged += delegate { };
            this.UserStatusChanged += delegate { };
            this.FriendRemoved += delegate { };

            this.BatchLoadCompleted += new CbGeneric(GlobalUserCache_FriendRTDataRefreshCompleted);
            this.unitTypeRecognizer = recognizer;
            this.logger = _logger;

            //自己的信息始终加载最新的           
            this.currentUser = this.DoGetUser(curUserID);
            this.userManager.Add(this.currentUser.ID, this.currentUser);

            this.persistenceFilePath = persistencePath;
            this.originUserLocalPersistence = UserLocalPersistence<TUser, TGroup>.Load(this.persistenceFilePath);//返回null，表示该登录帐号还没有任何缓存
            if (this.originUserLocalPersistence == null)
            {
            }
            else
            {
                this.quickAnswerList = this.originUserLocalPersistence.QuickAnswerList;


                foreach (TUser user in this.originUserLocalPersistence.FriendList)
                {
                    if (user.ID == null)
                    {
                        continue;
                    }
                    if (user.ID != this.currentUser.ID)
                    {                        
                        user.UserStatus = UserStatus.OffLine;
                        user.CommentName = this.currentUser.GetUnitCommentName(user.ID);
                        this.userManager.Add(user.ID, user);
                    }
                }

                foreach (TGroup group in this.originUserLocalPersistence.GroupList)
                {
                    if (this.currentUser.GroupList.Contains(group.ID))
                    {
                        group.CommentName = this.currentUser.GetUnitCommentName(group.ID);
                        this.groupManager.Add(group.ID, group);
                    }
                }
            }
        }

        void GlobalUserCache_FriendRTDataRefreshCompleted()
        {
            this.SaveUserLocalCache(null);
        }
        #endregion

        #region UserLocalCache
        public List<string> GetRecentList()
        {
            if (this.originUserLocalPersistence == null || this.originUserLocalPersistence.RecentList == null)
            {
                return new List<string>();
            }

            return this.originUserLocalPersistence.RecentList;
        }

        public void SaveUserLocalCache(List<string> recentList) //recentID的列表，recentID以“G_”或“U_”开头，以区分用户或组。
        {
            try
            {
                if (recentList == null)
                {
                    if (this.originUserLocalPersistence != null)
                    {
                        recentList = this.originUserLocalPersistence.RecentList;
                    }
                }
                UserLocalPersistence<TUser, TGroup> cache = new UserLocalPersistence<TUser, TGroup>(this.userManager.GetAllReadonly(), this.groupManager.GetAll(), recentList, this.quickAnswerList);
                cache.Save(this.persistenceFilePath);
            }
            catch (Exception ee)
            {
                this.logger.Log(ee, "BaseGlobalUserCache.SaveUserLocalCache", ErrorLevel.Standard);
            }
        }
        #endregion

        #region StartRefreshFriendInfo
        private Thread updateThread;
        /// <summary>
        /// 当登录后窗体显示时，或断线重连成功时，调用此方法。
        /// </summary>
        public void StartRefreshFriendInfo()
        {
            //直接使用线程，可以快速启动。后台线程池初始化需要10秒左右，太慢了。
            this.updateThread = (this.userManager.Count > 1) ? new Thread(new ParameterizedThreadStart(this.RefreshContactRTData)) : new Thread(new ParameterizedThreadStart(this.LoadContactsFromServer));
            this.updateThread.Start();

            //if (this.userManager.Count > 0)
            //{

            //    CbGeneric cb = new CbGeneric(this.RefreshContactRTData);
            //    cb.BeginInvoke(null, null);
            //}
            //else
            //{
            //    CbGeneric cb = new CbGeneric(this.LoadContactsFromServer);
            //    cb.BeginInvoke(null, null);
            //}
        }

        #region LoadContactsFromServer
        private void LoadContactsFromServer(object state)
        {
            try
            {
                this.BatchLoadStarted();
                List<string> allContactList = this.DoGetAllContactIDs();
                int pageCount = allContactList.Count / this.pageSize4LoadFriends;
                int lastPageSize = allContactList.Count % this.pageSize4LoadFriends;
                if (lastPageSize > 0)
                {
                    pageCount += 1;
                }
                else
                {
                    lastPageSize = this.pageSize4LoadFriends;
                }

                if (pageCount == 1)
                {
                    //好友，包括组友                   
                    List<TUser> friends = this.DoGetAllContacts();
                    foreach (TUser friend in friends)
                    {                        
                        if (friend.ID != this.currentUser.ID)
                        {
                            friend.CommentName = this.currentUser.GetUnitCommentName(friend.ID);
                            this.userManager.Add(friend.ID, friend);
                            this.UserBaseInfoChanged(friend);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < pageCount; i++)
                    {
                        string[] ary = (i == pageCount - 1) ? new string[lastPageSize] : new string[this.pageSize4LoadFriends];
                        allContactList.CopyTo(i * this.pageSize4LoadFriends, ary, 0, ary.Length);
                        List<string> tmp = new List<string>(ary);
                        List<TUser> friends = this.DoGetSomeUsers(tmp);
                        foreach (TUser friend in friends)
                        {
                            if (friend.ID != this.currentUser.ID)
                            {
                                friend.CommentName = this.currentUser.GetUnitCommentName(friend.ID);
                                this.userManager.Add(friend.ID, friend);
                                this.UserBaseInfoChanged(friend);
                            }
                        }
                    }
                }

                List<TGroup> myGroups = this.DoGetMyGroups(); 
                foreach (TGroup group in myGroups)
                {
                    this.groupManager.Add(group.ID, group);                   
                }

                foreach (TGroup group in myGroups)
                {
                    this.GroupChanged(group, GroupChangedType.GroupInfoChanged, null ,null);
                }
            }
            catch (Exception ee)
            {
                this.logger.Log(ee, "GlobalUserCache.LoadContactsFromServer", ESBasic.Loggers.ErrorLevel.Standard);
            }

            
            this.BatchLoadCompleted();
        }
        #endregion

        #region RefreshContactRTData
        private void RefreshContactRTData(object state)
        {
            try
            {
                this.BatchLoadStarted();

                ContactRTDatas contract = this.DoGetContactsRTDatas();  //1000用户数据量大小为22k
                foreach (string userID in this.userManager.GetKeyList())
                {
                    if (userID != this.currentUser.ID && !contract.UserStatusDictionary.ContainsKey(userID)) //最新的联系人中不包含缓存用户，则将之从缓存中删除。
                    {
                        this.userManager.Remove(userID);
                        if (this.FriendRemoved != null)
                        {
                            this.FriendRemoved(userID);
                        }
                    }
                }

                foreach (KeyValuePair<string, UserRTData> pair in contract.UserStatusDictionary)
                {
                    if (pair.Key == this.currentUser.ID)
                    {
                        continue;
                    }

                    TUser origin = this.userManager.Get(pair.Key);
                    if (origin == null) //不存在于本地缓存中
                    {
                        TUser user = this.DoGetUser(pair.Key);
                        user.CommentName = this.currentUser.GetUnitCommentName(user.ID);
                        this.userManager.Add(user.ID, user);
                        if (this.UserBaseInfoChanged != null)
                        {
                            this.UserBaseInfoChanged(user);
                        }
                    }
                    else
                    {
                        //资料变化
                        if (pair.Value.Version != origin.Version)
                        {
                            TUser user = this.DoGetUser(pair.Key);                            
                            user.CommentName = this.currentUser.GetUnitCommentName(user.ID);
                            user.LastWordsRecord = origin.LastWordsRecord;
                            user.ReplaceOldUnit(origin);
                            this.userManager.Add(user.ID, user);
                            if (this.UserBaseInfoChanged != null)
                            {
                                this.UserBaseInfoChanged(user);
                            }
                        }
                        else
                        {
                            //状态变化
                            if (origin.UserStatus != pair.Value.UserStatus)
                            {
                                origin.UserStatus = pair.Value.UserStatus;
                                if (this.UserStatusChanged != null)
                                {
                                    this.UserStatusChanged(origin);
                                }
                            }
                        }
                    }
                }

                List<string> updateGroupList = new List<string>();
                foreach (string groupID in this.currentUser.GroupList)
                {
                    TGroup group = this.groupManager.Get(groupID);
                    if (group == null)
                    {
                        updateGroupList.Add(groupID);
                        continue;
                    }

                    if (contract.GroupVersionDictionary.ContainsKey(groupID))
                    {
                        if (contract.GroupVersionDictionary[groupID] != group.Version)
                        {
                            updateGroupList.Add(groupID);
                            continue;
                        }
                    }
                }

                if (updateGroupList.Count > 0)
                {
                    //加载组
                    List<TGroup> newGroups = this.DoGetSomeGroups(updateGroupList);
                    foreach (TGroup group in newGroups)
                    {
                        group.CommentName = this.currentUser.GetUnitCommentName(group.ID);
                        TGroup old = this.groupManager.Get(group.ID);
                        if (old != null)
                        {
                            group.LastWordsRecord = old.LastWordsRecord;
                            group.ReplaceOldUnit(old);
                        }
                        this.groupManager.Add(group.ID, group);
                        if (this.GroupChanged != null)
                        {
                            this.GroupChanged(group, GroupChangedType.GroupInfoChanged, null, null);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                this.logger.Log(ee, "GlobalUserCache.RefreshContactRTData", ESBasic.Loggers.ErrorLevel.Standard);
            }
            this.BatchLoadCompleted();
        }
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// 从缓存中查找目标Unit。
        /// </summary>         
        public IUnit GetUnitExisted(string id)
        {
            UnitType type = this.unitTypeRecognizer.Recognize(id);
            if (type == UnitType.User)
            {
                return this.userManager.Get(id);
            }

            if (type == UnitType.Group)
            {
                return this.groupManager.Get(id);
            }

            return null;
        }

        /// <summary>
        /// 搜索目标Unit。如果缓存中不存在，则从服务器加载。
        /// </summary>      
        public IUnit GetUnit(string id)
        {
            UnitType type = this.unitTypeRecognizer.Recognize(id);
            if (type == UnitType.Group)
            {
                return this.GetGroup(id);
            }

            if (type == UnitType.User)
            {
                return this.GetUser(id);
            }

            return null;
        }

        /// <summary>
        /// 获取缓存中的所有用户。
        /// </summary>   
        public List<TUser> GetAllUser()
        {
            return this.userManager.GetAll();
        }

        /// <summary>
        /// 根据ID或Name从缓存中搜索用户【完全匹配】。
        /// </summary>   
        public List<TUser> SearchUser(string idOrName)
        {
            List<TUser> list = new List<TUser>();
            foreach (TUser user in this.userManager.GetAllReadonly())
            {
                if (user.ID == idOrName || user.Name == idOrName)
                {
                    list.Add(user);
                }
            }
            return list;
        }

        /// <summary>
        /// 根据UserID本地模糊查询 （优先查询是否有完成匹配的，若有则返回该单个用户，若无就返回所有匹配该字符的好友）
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<TUser> SearchUser_Fuzzy(string userID)
        {
            List<TUser> list = new List<TUser>();
            List<TUser> users = this.userManager.GetAllReadonly();

            TUser preciseUser = users.Find((x) => (x.ID == userID || x.DisplayName == userID) && CurrentUser.IsFriend(userID));//若有一个完全账号完全匹配就返回该账号
            if (preciseUser != null)
            {
                list.Add(preciseUser);
                return list;
            }
            foreach (TUser user in users)
            {
                if ((user.ID.Contains(userID) || user.DisplayName.Contains(userID)) && CurrentUser.IsFriend(user.ID))//返回所有包含该字符的所有好友
                {
                    list.Add(user);
                }
            }
            return list;
        }


        /// <summary>
        /// 根据groupID本地模糊查询（优先查询是否有完成匹配的，若有则返回该单个群组，若无就返回所有匹配该字符的自己所在的群组）
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public List<TGroup> SearchGroup_Fuzzy(string groupID)
        {
            List<TGroup> list = new List<TGroup>();
            List<TGroup> groups = this.groupManager.GetAllReadonly();
            TGroup preciseGroup = groups.Find((x) => (x.ID == groupID || x.DisplayName == groupID) && CurrentUser.GroupList.Contains(groupID));//若有一个完全账号完全匹配就返回该群组
            if (preciseGroup != null)
            {
                list.Add(preciseGroup);
                return list;
            }
            foreach (TGroup user in groups)
            {
                if ((user.ID.Contains(groupID) || user.DisplayName.Contains(groupID)) && CurrentUser.GroupList.Contains(groupID))//返回所有包含该字符的且是自己的群组
                {
                    list.Add(user);
                }
            }
            return list;
        }

        /// <summary>
        /// 当自己掉线时调用。不触发UserStatusChanged事件。
        /// </summary>
        public void SetAllUserOffline()
        {
            foreach (TUser friend in this.userManager.GetAll())
            {
                friend.UserStatus = UserStatus.OffLine;
            }
        }

        /// <summary>
        /// 获取缓存中的所有用户的ID。
        /// </summary>  
        public List<string> GetAllUserID()
        {
            return this.userManager.GetKeyList();
        }

        #region GetUserName
        public string GetUserName(string userID)
        {
            TUser user = this.GetUser(userID);
            if (user == null)
            {
                return null;
            }

            return user.Name;
        }
        #endregion

        /// <summary>
        /// 当从缓存外部拿到了用户的新版本资料时，调用此方法更新缓存。将触发UserBaseInfoChanged事件。
        /// </summary>      
        public void ManualSyncUserBaseInfo(TUser userNewVersion)
        {
            TUser origin = this.userManager.Get(userNewVersion.ID);
            userNewVersion.CommentName = this.currentUser.GetUnitCommentName(userNewVersion.ID);
            userNewVersion.LastWordsRecord = origin.LastWordsRecord;
            userNewVersion.ReplaceOldUnit(origin);
            this.userManager.Add(userNewVersion.ID, userNewVersion);
            if (this.UserBaseInfoChanged != null)
            {
                this.UserBaseInfoChanged(userNewVersion);
            }
        }

        /// <summary>
        /// 目标用户在当前缓存中是否存在？
        /// </summary>        
        public bool IsUserInCache(string userID)
        {
            return this.userManager.Contains(userID);
        }

        /// <summary>
        /// 获取用户。如果目标用户在当前缓存中不存在，则从服务器加载后再返回。
        /// </summary>        
        public TUser GetUser(string userID, bool isRefresh = false)
        {
            TUser user = isRefresh ? default(TUser) : this.userManager.Get(userID);
            if (user == null)
            {
                user = this.DoGetUser(userID);
                if (user != null)
                {
                    user.CommentName = this.currentUser.GetUnitCommentName(user.ID);
                    this.userManager.Add(userID, user);
                }
            }
            return user;
        }

        /// <summary>
        /// 如果是自己的状态发生变化，也调用此方法。（将触发 MyStatusChanged 或 UserStatusChanged 事件）
        /// </summary>   
        public void ChangeUserStatus(string userID, UserStatus status)
        {
            if (userID == this.currentUser.ID)
            {
                this.currentUser.UserStatus = status;
                if (this.MyStatusChanged != null)
                {
                    this.MyStatusChanged();
                }
                return;
            }

            TUser user = this.userManager.Get(userID);
            if (user != null)
            {
                user.UserStatus = status;
                if (this.UserStatusChanged != null)
                {
                    this.UserStatusChanged(user);
                }
            }
        }

        public void ChangeUnitCommentName(string unitID, string commentName)
        {
            IUnit unit = this.GetUnit(unitID);
            if (unit != null)
            {
                unit.CommentName = commentName;
                if (this.UnitCommentNameChanged != null)
                {
                    this.UnitCommentNameChanged(unit);
                }
            }
        }


        /// <summary>
        /// 当接收到来自服务端的新用户注册的通知时，调用此方法。（将触发SomeoneRegistered事件）
        /// </summary>       
        public void OnSomeoneRegistered(TUser user)
        {
            this.userManager.Add(user.ID, user);
            if (this.UserRegistered != null)
            {
                this.UserRegistered(user);
            }
        }

        /// <summary>
        /// 当接收到来自服务端的用户被删除的通知时，调用此方法。（将触发UserDeleted、GroupChanged事件）
        /// </summary>       
        public void OnUserDeleted(string userID ,string operatorID)
        {
            if (userID == this.currentUser.ID)
            {
                if (this.MyselfDeleted != null)
                {
                    this.MyselfDeleted(operatorID);                    
                }

                return;
            }

            TUser user = this.userManager.Get(userID);
            if (user == null)
            {
                return;
            }

            foreach (TGroup group in this.groupManager.GetAllReadonly())
            {
                if (group.MemberList.Contains(userID))
                {
                    group.RemoveMember(userID);
                    if (this.GroupChanged != null)
                    {
                        this.GroupChanged(group, GroupChangedType.SomeoneDeleted, operatorID ,userID);
                    }
                }
            }

            this.userManager.Remove(userID);
            if (this.UserDeleted != null)
            {
                this.UserDeleted(user);
            }           
        }

        /// <summary>
        /// 被别人添加为好友。（将触发FriendAdded事件）
        /// </summary>        
        public void OnFriendAdded(TUser user)
        {
            this.userManager.Add(user.ID, user);
            if (this.FriendAdded != null)
            {
                this.FriendAdded(user);
            }
        }

        /// <summary>
        /// 当自己的头像发生变化时，也请调用此方法。（将触发 MyInfoChanged 或 UserInfoChanged 事件）
        /// </summary>   
        public void UpdateUserHeadImage(string userID, int defaultHeadImageIndex, byte[] customizedHeadImage, int userLatestVersion)
        {
            if (this.currentUser.ID == userID)
            {
                this.currentUser.ChangeHeadImage(defaultHeadImageIndex, customizedHeadImage);
                this.currentUser.Version = userLatestVersion;
                if (this.MyBaseInfoChanged != null)
                {
                    this.MyBaseInfoChanged();
                }
            }
            else
            {
                TUser user = this.userManager.Get(userID);
                if (user != null)
                {
                    user.ChangeHeadImage(defaultHeadImageIndex, customizedHeadImage);
                    user.Version = userLatestVersion;
                    if (this.UserBaseInfoChanged != null)
                    {
                        this.UserBaseInfoChanged(user);
                    }
                }
            }
        }

        /// <summary>
        /// 当自己的基础资料发生变化时，也请调用此方法。（将触发 MyBaseInfoChanged 或 UserBaseInfoChanged 事件）
        /// </summary>   
        public void UpdateUserBaseInfo(string userID, string name, string signature, string orgID, int userLatestVersion)
        {
            if (this.currentUser.ID == userID)
            {
                this.currentUser.Name = name ;
                this.currentUser.Signature = signature;
                this.currentUser.OrgID = orgID;
                this.currentUser.Version = userLatestVersion;
                if (this.MyBaseInfoChanged != null)
                {
                    this.MyBaseInfoChanged();
                }                
            }
            else
            {
                TUser user = this.userManager.Get(userID);
                if (user != null)
                {
                    user.Name = name;
                    user.Signature = signature;
                    user.OrgID = orgID;
                    user.Version = userLatestVersion;
                    if (this.UserBaseInfoChanged != null)
                    {
                        this.UserBaseInfoChanged(user);
                    }
                }
            }   
        }

        /// <summary>
        /// 当自己的业务资料发生变化时，也请调用此方法。（将触发 MyBusinessInfoChanged 或 UserBusinessInfoChanged 事件）
        /// </summary>   
        public void UpdateUserBusinessInfo(string userID, Dictionary<string, byte[]> businessInfo, int latestVersion)
        {
            if (this.currentUser.ID == userID)
            {
                this.currentUser.UpdateBusinessInfo(businessInfo);
                this.currentUser.Version = latestVersion;
                if (this.MyBusinessInfoChanged != null)
                {
                    this.MyBusinessInfoChanged(businessInfo);
                }
            }
            else
            {
                TUser user = this.userManager.Get(userID);
                if (user != null)
                {
                    user.UpdateBusinessInfo(businessInfo);
                    user.Version = latestVersion;
                    if (this.UserBusinessInfoChanged != null)
                    {
                        this.UserBusinessInfoChanged(user, businessInfo);
                    }
                }
            }
        }

        /// <summary>
        /// 当User（包括自己）的信息【非框架字段、即属于扩展的业务字段】已经被修改时，调用此方法，可以触发UserInfoChanged事件。
        /// </summary>       
        public void SpringUserInfoChangedEvent(string userID)
        {
            if (userID == this.currentUser.ID)
            {
                if (this.MyBaseInfoChanged != null)
                {
                    this.MyBaseInfoChanged();                    
                }
                return;
            }

            TUser user = this.GetUser(userID);
            if (user != null)
            {
                if (this.UserBaseInfoChanged != null)
                {
                    this.UserBaseInfoChanged(user);
                }
            }
        }

        /// <summary>
        /// 提前将Unit的资料加到缓存中。如果缓存中已经存在，则返回false。
        /// </summary>      
        public bool PrepairUnit(IUnit unit)
        {
            if (unit.UnitType == UnitType.Group)
            {
                if (this.groupManager.Contains(unit.ID))
                {
                    return false;
                }
                this.groupManager.Add(unit.ID, (TGroup)unit);
                return true;
            }

            if (unit.UnitType == UnitType.User)
            {
                if (this.userManager.Contains(unit.ID))
                {
                    return false;
                }
                this.userManager.Add(unit.ID, (TUser)unit);
                return true;
            }

            return true;
        }

        /// <summary>
        /// 将预添加的Unit从缓存中移除。（可能是添加好友或加入组没有通过申请）
        /// </summary>       
        public void CancelPrepairUnit(IUnit unit)
        {
            if (unit.IsUser)
            {
                this.userManager.Remove(unit.ID);
                return;
            }

            if (unit.UnitType == UnitType.Group)
            {
                this.groupManager.Remove(unit.ID);
            }
        }

        /// <summary>
        /// 添加好友通过后，调用此方法将其加入缓存。（将触发AddFriendSucceed事件）
        /// </summary>
        /// <param name="user"></param>
        public void AddFriend(TUser user)
        {
            this.userManager.Add(user.ID, user);
            if (this.AddFriendSucceed != null)
            {
                this.AddFriendSucceed(user);
            }
        }

        public void RemovedFriend(string friendID)
        {

            bool inGroup = false;
            foreach (TGroup group in this.groupManager.GetAll())
            {
                if (group.MemberList.Contains(friendID))
                {
                    inGroup = true;
                    break;
                }
            }
            if (!inGroup)
            {
                this.userManager.Remove(friendID);
            }

            if (this.FriendRemoved != null)
            {
                this.FriendRemoved(friendID);
            }
        }
        #endregion

        #region Group
        /// <summary>
        /// 当自己创建群时，调用此方法添加到缓存中。
        /// </summary>       
        public void OnCreateGroup(TGroup group)
        {
            this.groupManager.Add(group.ID, group);
        }

        public List<TGroup> GetAllGroups()
        {
            return this.groupManager.GetAll();
        }

        public TGroup GetGroup(string groupID,bool isRefresh=false)
        {
            TGroup group = isRefresh ? default(TGroup) : this.groupManager.Get(groupID);
            if (group == null)
            {
                group = this.DoGetGroup(groupID);
                if (group != null)
                {
                    group.CommentName = this.currentUser.GetUnitCommentName(group.ID);
                    this.groupManager.Add(group.ID, group);
                }
            }

            return group;
        }

        /// <summary>
        /// 当退出群时调用。
        /// </summary>        
        public void RemoveGroup(string groupID)
        {
            this.groupManager.Remove(groupID);
        }

        public void OnGroupInfoChanged(string groupID ,string operatorID)
        {
            TGroup group = this.groupManager.Get(groupID);
            if (group == null)
            {
                return;
            }

            this.GroupChanged(group, GroupChangedType.GroupInfoChanged, operatorID ,null);
        }

        public void OnGroupDeleted(string groupID, string operateUserID)
        {
            //if (this.currentUser.ID == operateUserID)
            //{
            //    return;
            //}
            this.CurrentUser.QuitGroup(groupID);
            TGroup group = this.groupManager.Get(groupID);
            if (group == null)
            {
                return;
            }
            this.GroupChanged(group, GroupChangedType.GroupDeleted, operateUserID ,null);
            this.groupManager.Remove(groupID);
        }

        public void OnSomeoneJoinGroup(string groupID, string userID)
        {
            TGroup group = this.groupManager.Get(groupID);
            if (group == null || group.MemberList.Contains(userID))
            {
                return;
            }
            group.AddMember(userID);

            if (this.GroupChanged != null)
            {
                this.GroupChanged(group, GroupChangedType.SomeoneJoin, userID, userID);
            }
        }

        public void OnUsersPulledIntoGroup(string groupID, string operateUserID, List<string> guests)
        {
            TGroup group = this.GetGroup(groupID);
            if (guests.Contains(this.currentUser.ID))
            {
                if (this.GroupChanged != null)
                {
                    this.GroupChanged(group, GroupChangedType.MyselfBePulledIntoGroup, operateUserID, this.currentUser.ID);
                }
                return;
            }

            foreach (string guest in guests)
            {
                if (!group.MemberList.Contains(guest))
                {
                    group.AddMember(guest);

                    if (this.GroupChanged != null)
                    {
                        this.GroupChanged(group, GroupChangedType.OtherBePulledIntoGroup, operateUserID, guest);
                    }
                }
            }
        }

        public void OnUsersRemovedFromGroup(string groupID, string operateUserID, List<string> guests)
        {
            TGroup group = this.groupManager.Get(groupID);
            if (group == null)
            {
                return;
            }

            if (guests.Contains(this.currentUser.ID))
            {
                this.groupManager.Remove(groupID);
                if (this.GroupChanged != null)
                {
                    this.GroupChanged(group, GroupChangedType.MyselfBeRemovedFromGroup, operateUserID, this.currentUser.ID);
                }
                return;
            }

            foreach (string guest in guests)
            {
                group.RemoveMember(guest);
                if (this.GroupChanged != null)
                {
                    this.GroupChanged(group, GroupChangedType.OtherBeRemovedFromGroup, operateUserID, guest);
                }
            }            
        }

        public void OnSomeoneQuitGroup(string groupID, string userID)
        {
            TGroup group = this.groupManager.Get(groupID);
            if (group == null || !group.MemberList.Contains(userID))
            {
                return;
            }
            if (userID == this.currentUser.ID)//20210427 新加
            {
                this.GroupChanged(group, GroupChangedType.GroupDeleted, userID, null);
                this.groupManager.Remove(groupID);
                return;
            }

            group.RemoveMember(userID);

            if (this.GroupChanged != null)
            {
                this.GroupChanged(group, GroupChangedType.SomeoneQuit, userID, userID);
            }
        }
        #endregion

 
    }
}
