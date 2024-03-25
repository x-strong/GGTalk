using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.ObjectManagement.Managers;
using ESBasic.Security;
using System.Configuration;
using ESBasic;
using ESFramework;
using ESFramework.Boost.Controls;
using ESPlus.Serialization;

namespace TalkBase.Server
{
    /// <summary>
    /// 服务端全局缓存。
    /// </summary>    
    public class ServerGlobalCache<TUser, TGroup>
        where TUser : TalkBase.IUser
        where TGroup : TalkBase.IGroup
    {
        private IDBPersister<TUser, TGroup> dbPersister;
        private ITalkBaseHelper<TGroup> talkBaseHelper;
        private ObjectManager<string, TUser> userCache = new ObjectManager<string, TUser>(); // key:用户ID 。 Value：用户信息
        private ObjectManager<string, TGroup> groupCache = new ObjectManager<string, TGroup>();  // key:组ID 。 Value：Group信息     


        #region Ctor
        public ServerGlobalCache(IDBPersister<TUser, TGroup> persister, ITalkBaseHelper<TGroup> helper)
        {
            this.dbPersister = persister;
            this.talkBaseHelper = helper;
        }
        #endregion

        public void PreLoadUsers()
        {
            foreach (TUser user in this.dbPersister.GetAllUser())
            {
                this.userCache.Add(user.ID, user);
            }
        }

        public void PreLoadGroups()
        {
            foreach (TGroup group in this.dbPersister.GetAllGroup())
            {
                this.groupCache.Add(group.ID, group);
            }
        }

        public IDBPersister<TUser, TGroup> DBPersister
        {
            get { return this.dbPersister; }
        }

        public IUnit GetUnit(string unitID)
        {
            if (unitID.StartsWith(FunctionOptions.PrefixGroupID))
            {
                return this.GetGroup(unitID);
            }
            return this.GetUser(unitID);
        }

        #region UserTable       

        /// <summary>
        /// 根据ID或Name搜索用户【完全匹配】。
        /// </summary>   
        public List<TUser> SearchUser(string idOrName)
        {
            List<TUser> list = new List<TUser>();
            foreach (TUser user in this.userCache.GetAllReadonly())
            {
                if (user.ID == idOrName || user.Name == idOrName)
                {
                    list.Add(user);
                }
            }
            return list;
        }

        /// <summary>
        /// 插入一个新用户。
        /// </summary>      
        public void InsertUser(TUser user)
        {
            this.userCache.Add(user.ID, user);
            this.dbPersister.InsertUser(user);
        }

        /// <summary>
        /// 删除一个用户。从其好友的好友列表中删除，从群中删除，从缓存中删除，从数据库中删除。
        /// </summary>
        /// <param name="userID"></param>
        public void DeleteUser(string userID)
        {
            TUser user = this.GetUser(userID);
            if (user == null)
            {
                return;
            }

            foreach (string friendID in user.GetAllFriendList())
            {
                TUser friend = this.GetUser(friendID);
                if (friend != null)
                {
                    friend.RemoveFriend(userID);
                    this.dbPersister.UpdateUserFriends(friend);
                }
            }

            foreach (string groupID in user.GroupList)
            {
                TGroup group = this.GetGroup(groupID);
                if (group != null)
                {
                    group.RemoveMember(userID);
                    group.Version += 1;
                    this.dbPersister.UpdateGroupMembers(group);
                }
            }

            this.userCache.Remove(userID);
            this.dbPersister.DeleteUser(userID);
        }

        public void UpdateUserHeadImage(string userID, int defaultHeadImageIndex, byte[] customizedHeadImage)
        {
            TUser user = this.GetUser(userID);
            if (user == null)
            {
                return;
            }

            user.ChangeHeadImage(defaultHeadImageIndex, customizedHeadImage);
            user.Version += 1;
            user.DeletePartialCopy();//2019.07.19
            this.dbPersister.UpdateUserHeadImage(userID, defaultHeadImageIndex, customizedHeadImage, user.Version);
        }



        public int UpdateUserBusinessInfo(string userID, Dictionary<string, byte[]> businessInfo, bool increaseVersion)
        {
            TUser user = this.GetUser(userID);
            if (user == null)
            {
                return -1;
            }

            user.UpdateBusinessInfo(businessInfo);
            if (increaseVersion)
            {
                user.Version += 1;
            }
            this.dbPersister.UpdateUserBusinessInfo(user, businessInfo, user.Version);
            return user.Version;
        }

        public void UpdateUserInfo(string userID, string name, string signature, string orgID)
        {
            TUser user = this.GetUser(userID);
            if (user == null)
            {
                return;
            }

            user.Name = name;
            user.Signature = signature;
            user.OrgID = orgID;
            user.Version += 1;
            user.DeletePartialCopy();
            this.dbPersister.UpdateUserInfo(userID, name, signature, orgID, user.Version);
        }

        public List<string> GetAllUserID()
        {
            return this.userCache.GetKeyList();
        }

        public List<TUser> GetAllUser()
        {
            return this.userCache.GetAll();
        }

        /// <summary>
        /// 目标帐号是否已经存在？
        /// </summary>    
        public bool IsUserExist(string userID)
        {
            return this.GetUser(userID) != null;
        }

        /// <summary>
        /// 获取目标用户，如果缓存中不存在，则从DB加载。
        /// </summary>        
        public TUser GetUser(string userID)
        {
            TUser user = this.userCache.Get(userID);
            if (user == null)
            {
                user = this.dbPersister.GetUser(userID);
                if (user != null)
                {
                    this.userCache.Add(userID, user);
                }
            }

            return user;
        }

        /// <summary>
        /// 从DB中查询用户 （id或名称 精确查询）
        /// </summary>
        /// <param name="idOrName"></param>
        /// <returns></returns>
        public List<TUser> DoSearchUser(string idOrName)
        {
            return this.dbPersister.SearchUser(idOrName);
        }

        /// <summary>
        /// 根据电话号码获取用户信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public TUser GetUser4Phone(string phone)
        {
            return this.dbPersister.GetUser4Phone(phone);
        }

        /// <summary>
        /// 根据电话号码获取已绑定的用户列表
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public List<TUser> GetUserList4Phone(string phone)
        {
            return this.dbPersister.GetUserList4Phone(phone);
        }

        /// <summary>
        /// 重新从数据库中加载资料。
        /// </summary>        
        public TUser ReloadUser(string userID)
        {
            this.userCache.Remove(userID);
            return this.GetUser(userID);
        }

        /// <summary>
        /// 在缓存中查找目标用户。
        /// </summary>        
        public TUser GetUserInCache(string userID)
        {
            return this.userCache.Get(userID);
        }

        public void ChangeUnitCommentName(string ownerID, string unitID, string commentName)
        {
            TUser user = this.GetUser(ownerID);
            if (user == null)
            {
                return;
            }
            user.ChangeUnitCommentName(unitID, commentName);
            this.dbPersister.UpdateUserCommentNames(user);
        }

        public ChangePasswordResult ChangePassword(string userID, string oldPasswordMD5, string newPasswordMD5)
        {
            TUser user = this.GetUser(userID);
            if (user == null)
            {
                return ChangePasswordResult.UserNotExist;
            }

            string old = this.dbPersister.GetUserPassword(userID);
            if (old != oldPasswordMD5)
            {
                return ChangePasswordResult.OldPasswordWrong;
            }

            this.dbPersister.UpdateUserPassword(userID, newPasswordMD5);
            return ChangePasswordResult.Succeed;
        }

        public ChangePasswordResult ChangePassword(string userID, string newPasswordMD5)
        {
            TUser user = this.GetUser(userID);
            if (user == null)
            {
                return ChangePasswordResult.UserNotExist;
            }

            this.dbPersister.UpdateUserPassword(userID, newPasswordMD5);
            return ChangePasswordResult.Succeed;
        }

        /// <summary>
        /// 获取某个用户的好友列表。
        /// </summary>      
        public List<string> GetFriends(string userID)
        {
            TUser user = this.GetUser(userID);
            if (user == null)
            {
                return new List<string>();
            }

            return user.GetAllFriendList();
        }

        /// <summary>
        /// 添加好友，建立双向关系
        /// </summary>  
        public void AddFriend(string ownerID, string friendID, string catalogName, string destCatalogName = FunctionOptions.DefaultFriendCatalog)
        {
            TUser user1 = this.GetUser(ownerID);
            TUser user2 = this.GetUser(friendID);
            if (user1 == null || user2 == null)
            {
                return;
            }

            user1.AddFriend(friendID, catalogName);
            user2.AddFriend(ownerID, destCatalogName);
            this.dbPersister.UpdateUserFriends(user1);
            this.dbPersister.UpdateUserFriends(user2);
        }

        /// <summary>
        /// 删除好友，并删除双向关系
        /// </summary>  
        public void RemoveFriend(string ownerID, string friendID)
        {
            TUser user1 = this.GetUser(ownerID);
            if (user1 != null)
            {
                user1.RemoveFriend(friendID);
                this.dbPersister.UpdateUserFriends(user1);
            }

            TUser user2 = this.GetUser(friendID);
            if (user2 != null)
            {
                user2.RemoveFriend(ownerID);
                this.dbPersister.UpdateUserFriends(user2);
            }
        }

        /// <summary>
        /// 拉黑好友，好友会删除自己
        /// </summary>
        public void BlackFriend(string ownerID, string friendID)
        {
            TUser user = this.GetUser(friendID);
            if (user != null)
            {
                user.RemoveFriend(ownerID);
                this.dbPersister.UpdateUserFriends(user);
            }
        }

        /// <summary>
        /// 拉白好友，好友会添加自己到他默认的好友分组
        /// </summary>
        public void WhiteFriend(string ownerID, string friendID)
        {
            TUser user = this.GetUser(friendID);
            if (user != null)
            {
                user.AddFriend(ownerID, FunctionOptions.DefaultFriendCatalog);
                this.dbPersister.UpdateUserFriends(user);
            }
        }

        public void ChangeFriendCatalogName(string ownerID, string oldName, string newName)
        {
            TUser user = this.GetUser(ownerID);
            if (user == null)
            {
                return;
            }

            user.ChangeFriendCatalogName(oldName, newName);
            this.dbPersister.UpdateUserFriends(user);
        }

        public void AddFriendCatalog(string ownerID, string catalogName)
        {
            TUser user = this.GetUser(ownerID);
            if (user == null)
            {
                return;
            }

            user.AddFriendCatalog(catalogName);
            this.dbPersister.UpdateUserFriends(user);
        }

        public void RemoveFriendCatalog(string ownerID, string catalogName)
        {
            TUser user = this.GetUser(ownerID);
            if (user == null)
            {
                return;
            }
            user.RemvoeFriendCatalog(catalogName);
            this.dbPersister.UpdateUserFriends(user);
        }

        public void MoveFriend(string ownerID, string friendID, string oldCatalog, string newCatalog)
        {
            TUser user = this.GetUser(ownerID);
            if (user == null)
            {
                return;
            }
            user.MoveFriend(friendID, oldCatalog, newCatalog);
            this.dbPersister.UpdateUserFriends(user);
        }
        #endregion

        #region GroupTable       

        /// <summary>
        /// 获取某用户所在的所有组列表。
        /// 建议：可将某个用户所在的组ID列表挂接在用户资料的某个字段上，以避免遍历计算。
        /// </summary>       
        public List<TGroup> GetMyGroups(string userID)
        {
            List<TGroup> groups = new List<TGroup>();
            TUser user = this.GetUser(userID);
            if (user == null)
            {
                return groups;
            }

            foreach (string groupID in user.GroupList)
            {
                TGroup g = this.GetGroup(groupID);
                if (g != null)
                {
                    groups.Add(g);
                }
            }
            return groups;
        }



        public Dictionary<string, int> GetMyGroupVersions(string userID)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            TUser user = this.GetUser(userID);
            if (user == null)
            {
                return dic;
            }

            foreach (string groupID in user.GroupList)
            {
                TGroup g = this.GetGroup(groupID);
                if (g != null)
                {
                    dic.Add(groupID, g.Version);
                }
            }
            return dic;
        }

        /// <summary>
        /// 获取某个组
        /// </summary>       
        public TGroup GetGroup(string groupID)
        {
            TGroup group = this.groupCache.Get(groupID);
            if (group == null)
            {
                group = this.dbPersister.GetGroup(groupID);
                if (group != null)
                {
                    this.groupCache.Add(groupID, group);
                }
            }

            return group;
        }

        public List<TGroup> SearchGroup(string idOrName)
        {
            return this.dbPersister.SearchGroup(idOrName);
        }

        public TGroup GetExistedGroup(string groupID)
        {
            return this.groupCache.Get(groupID);
        }

        /// <summary>
        /// 重新从数据库中加载资料。
        /// </summary>        
        public TGroup ReloadGroup(string groupID)
        {
            this.groupCache.Remove(groupID);
            return this.GetGroup(groupID);
        }

        /// <summary>
        /// 创建组
        /// </summary>       
        public CreateGroupResult CreateGroup(string creatorID, string groupID, string groupName, string announce, List<string> members,bool isPrivate)
        {
            if (this.groupCache.Contains(groupID))
            {
                return CreateGroupResult.GroupExisted;
            }

            if (!members.Contains(creatorID))
            {
                members.Add(creatorID);
            }

            TGroup group = this.talkBaseHelper.DoCreateGroup(creatorID, groupID, groupName, announce, members,isPrivate);
            this.groupCache.Add(groupID, group);
            this.dbPersister.InsertGroup(group);

            TUser user = this.GetUser(creatorID);
            user.JoinGroup(groupID);
            this.dbPersister.UpdateUserGroups(user);

            if (members != null)
            {
                foreach (string id in members)
                {
                    if (id == creatorID)
                    {
                        continue;
                    }
                    TUser member = this.GetUser(id);
                    member.JoinGroup(groupID);
                    this.dbPersister.UpdateUserGroups(member);
                }
            }

            return CreateGroupResult.Succeed;
        }

        /// <summary>
        /// 退出组
        /// </summary>       
        public void QuitGroup(string userID, string groupID)
        {
            TGroup group = this.GetGroup(groupID);
            if (group == null)
            {
                return;
            }
            group.RemoveMember(userID);
            group.Version += 1;
            this.dbPersister.UpdateGroupMembers(group);

            TUser user = this.GetUser(userID);
            user.QuitGroup(groupID);
            this.dbPersister.UpdateUserGroups(user);
        }

        public void DeleteGroup(string groupID)
        {
            TGroup group = this.GetGroup(groupID);
            if (group == null)
            {
                return;
            }
            foreach (string userID in group.MemberList)
            {
                TUser user = this.GetUser(userID);
                if (user != null)
                {
                    user.QuitGroup(groupID);
                    this.dbPersister.UpdateUserGroups(user);
                }
            }
            this.groupCache.Remove(groupID);
            this.dbPersister.DeleteGroup(groupID);
            this.dbPersister.DeleteAddGroupRequest(groupID);
        }

        /// <summary>
        /// 更新群的名称、公告
        /// </summary>        
        public void ChangeGroupInfo(string groupID, string name, string announce)
        {
            TGroup group = this.GetGroup(groupID);
            if (group == null)
            {
                return;
            }

            group.Name = name;
            group.Announce = announce;
            group.Version += 1;
            this.dbPersister.UpdateGroupInfo(group);
        }

        /// <summary>
        /// 加入某个组。
        /// </summary>        
        public JoinGroupResult JoinGroup(string userID, string groupID)
        {
            TGroup group = this.GetGroup(groupID);
            if (group == null)
            {
                return JoinGroupResult.GroupNotExist;
            }

            TUser user = this.GetUser(userID);
            if (!user.GroupList.Contains(groupID))
            {
                user.JoinGroup(groupID);
                this.dbPersister.UpdateUserGroups(user);
            }

            if (!group.MemberList.Contains(userID))
            {
                group.AddMember(userID);
                group.Version += 1;
                this.dbPersister.UpdateGroupMembers(group);
            }

            return JoinGroupResult.Succeed;
        }

        public List<string> GetAllContactsNecessary(string sourceUserID)
        {
            List<string> contacts = null;
            if (FunctionOptions.ContactsCareType == ContactsCareType.All)
            {
                contacts = this.GetAllContacts(sourceUserID);
            }
            else if (FunctionOptions.ContactsCareType == ContactsCareType.FriendsOnly)
            {
                TUser sourceUser = this.GetUser(sourceUserID);
                contacts = sourceUser.GetAllFriendList();
            }
            else
            {
                contacts = new List<string>();
            }

            return contacts;
        }

        /// <summary>
        /// 获取某个用户的所有联系人（组友，好友）。
        /// 建议：由于该方法经常被调用，可将组友关系缓存在内存中，而非每次都遍历计算一遍。
        /// </summary>        
        public List<string> GetAllContacts(string userID)
        {
            List<string> contacts = new List<string>();
            TUser user = this.GetUser(userID);
            if (user == null)
            {
                return contacts;
            }
            contacts = user.GetAllFriendList();
            foreach (string groupID in user.GroupList)
            {
                TGroup g = this.GetGroup(groupID);
                if (g != null)
                {
                    foreach (string memberID in g.MemberList)
                    {
                        if (memberID != userID && !contacts.Contains(memberID))
                        {
                            contacts.Add(memberID);
                        }
                    }
                }
            }

            return contacts;
        }
        #endregion     

        #region ChatRecord
        public void StoreChatRecord(string senderID, string accepterID, byte[] content)
        {
            this.dbPersister.InsertChatMessageRecord(new ChatMessageRecord(senderID, accepterID, content, false));
        }

        public ChatMessageRecord GetChatMessageRecord(int id)
        {
            return this.dbPersister.GetChatMessageRecord(id);
        }

        public ChatRecordPage GetChatRecordPage(ChatRecordTimeScope timeScope, string senderID, string accepterID, int pageSize, int pageIndex)
        {
            return this.dbPersister.GetChatRecordPage(timeScope, senderID, accepterID, pageSize, pageIndex);
        }

        public ChatRecordPage GetChatRecordPage(DateTimeScope timeScope, string senderID, string accepterID, int pageSize, int pageIndex)
        {
            return this.dbPersister.GetChatRecordPage(timeScope, senderID, accepterID, pageSize, pageIndex);
        }

        public ChatRecordPage GetGroupChatRecordPage(ChatRecordTimeScope timeScope, string groupID, int pageSize, int pageIndex)
        {
            return this.dbPersister.GetGroupChatRecordPage(timeScope, groupID, pageSize, pageIndex);
        }

        public ChatRecordPage GetGroupChatRecordPage(DateTimeScope timeScope, string groupID, int pageSize, int pageIndex)
        {
            return this.dbPersister.GetGroupChatRecordPage(timeScope, groupID, pageSize, pageIndex);
        }


        public void StoreGroupChatRecord(string groupID, string senderID, byte[] content)
        {
            this.dbPersister.InsertChatMessageRecord(new ChatMessageRecord(senderID, groupID, content, true));
        }



        public void ClearAllChatRecord()
        {
            this.dbPersister.ClearAllChatRecord();
        }
        #endregion               

        #region OfflineMessage
        /// <summary>
        /// 存储离线消息。
        /// </summary>       
        /// <param name="msg">要存储的离线消息</param>
        public void StoreOfflineMessage(OfflineMessage msg)
        {
            //大规模的情况下，可考虑使用队列缓冲。
            this.dbPersister.StoreOfflineMessage(msg);
        }

        /// <summary>
        /// 提取目标用户的所有离线消息。
        /// </summary>       
        /// <param name="destUserID">接收离线消息用户的ID</param>
        /// <returns>属于目标用户的离线消息列表，按时间升序排列</returns>
        public List<OfflineMessage> PickupOfflineMessage(string destUserID)
        {
            return this.dbPersister.PickupOfflineMessage(destUserID);
        }
        #endregion

        #region GroupOfflineMessage
        public List<GroupOfflineMessage> PickupGroupOfflineMessage(string destUserID, DateTime startTime)
        {
            List<GroupOfflineMessage> groupOfflineMessages = new List<GroupOfflineMessage>();
            Dictionary<string, List<ChatMessageRecord>> dic = this.dbPersister.PickupGroupOfflineChatMessageRecord(destUserID, startTime);
            foreach (KeyValuePair<string, List<ChatMessageRecord>> kvp in dic)
            {
                List<OfflineMessage> offlineMessages = new List<OfflineMessage>();
                foreach (ChatMessageRecord item in kvp.Value)
                {
                    offlineMessages.Add(this.ChatRecord2OfflineMessage_Group(item));
                }
                groupOfflineMessages.Add(new GroupOfflineMessage() { GroupID = kvp.Key, OfflineMessages = offlineMessages });
            }
            return groupOfflineMessages;
        }

        private OfflineMessage ChatRecord2OfflineMessage_Group(ChatMessageRecord messageRecord)
        {
            //   ChatMessageRecord chatMessageRecord= CompactPropertySerializer.Default.Deserialize<ChatMessageRecord>(messageRecord.Content, 0);

            // byte[] info = CompactPropertySerializer.Default.Serialize<RequestAddGroupContract>(contract);
            OfflineMessage offlineMessage = new OfflineMessage(messageRecord.SpeakerID, ESFramework.ClientType.DotNET, messageRecord.AudienceID, messageRecord.AudienceID, 1080, messageRecord.Content);
            offlineMessage.TimeTransfer = messageRecord.OccureTime;
            return offlineMessage;
        } 
        #endregion

        #region OfflineFile
        /// <summary>
        /// 将一个离线文件条目保存到数据库中。
        /// </summary>     
        public void StoreOfflineFileItem(OfflineFileItem item)
        {
            this.dbPersister.StoreOfflineFileItem(item);
        }

        /// <summary>
        /// 从数据库中提取接收者为指定用户的所有离线文件条目。
        /// </summary>       
        public List<OfflineFileItem> PickupOfflineFileItem(string accepterID)
        {
            return this.dbPersister.PickupOfflineFileItem(accepterID);
        }

        /// <summary>
        /// 从数据库中提取接收者为指定用户的所有多端助手文件条目。
        /// </summary>       
        public List<OfflineFileItem> PickupOfflineFileItem4Assistant(string accepterID, ClientType type)
        {
            return this.dbPersister.PickupOfflineFileItem4Assistant(accepterID, type);
        }

        #endregion        

        #region AddFriendRequest
        /// <summary>
        /// 插入一条好友申请记录。
        /// </summary>      
        public void InsertAddFriendRequest(string requesterID, string accepterID, string requesterCatalogName, string comment, bool isNotified = false)
        {
            this.dbPersister.InsertAddFriendRequest(requesterID, accepterID, requesterCatalogName, comment, isNotified);
        }

        /// <summary>
        /// DB中的好友申请记录为已通知
        /// </summary>
        /// <param name="requesterID">申请者ID</param>
        /// <param name="accepterID">接收者ID</param>
        public void SetAddFriendRequestNotified(string requesterID, string accepterID)
        {
            this.dbPersister.SetAddFriendRequestNotified(requesterID, accepterID);
        }

        public void UpdateAddFriendRequest(string requesterID, string accepterID, string requesterCatalogName, string accepterCatalogName, bool IsAgreed)
        {
            this.dbPersister.UpdateAddFriendRequest(requesterID, accepterID, requesterCatalogName, accepterCatalogName, IsAgreed);
        }

        public List<AddFriendRequest> GetAddFriendRequest4NotNotified(string userID)
        {
            return this.dbPersister.GetAddFriendRequest4NotNotified(userID);
        }


        public string GetRequesterCatalogName(string requesterID, string accepterID)
        {
            string catalogName = this.dbPersister.GetRequesterCatalogName(requesterID, accepterID);
            if (string.IsNullOrEmpty(catalogName))
            {
                return FunctionOptions.DefaultFriendCatalog;
            }
            if (!this.userCache.Get(requesterID).GetFriendCatalogList().Contains(catalogName))//若申请者该分组已删除，就直接添加到我的好友中
            {
                return FunctionOptions.DefaultFriendCatalog;
            }
            return catalogName;
        }

        public AddFriendRequestPage GetAddFriendRequestPage(string userID, int pageIndex, int pageSize)
        {
            return this.dbPersister.GetAddFriendRequestPage(userID, pageIndex, pageSize);
        }
        #endregion

        #region AddGroupRequest
        /// <summary>
        /// 插入一条好友申请记录。
        /// </summary>      
        public void InsertAddGroupRequest(string requesterID, string groupID, string accepterID, string comment, bool isNotified = false)
        {
            this.dbPersister.InsertAddGroupRequest(requesterID, groupID, accepterID, comment, isNotified);
        }

        /// <summary>
        /// DB中的好友申请记录为已通知
        /// </summary>
        /// <param name="requesterID">申请者ID</param>
        /// <param name="groupID"></param>
        public void SetAddGroupRequestNotified(string requesterID, string groupID)
        {
            this.dbPersister.SetAddGroupRequestNotified(requesterID, groupID);
        }

        public void UpdateAddGroupRequest(string requesterID, string groupID, bool IsAgreed)
        {
            this.dbPersister.UpdateAddGroupRequest(requesterID, groupID, IsAgreed);
        }

        public List<AddGroupRequest> GetAddGroupRequest4NotNotified(string userID)
        {
            return this.dbPersister.GetAddGroupRequest4NotNotified(userID);
        }


        public AddGroupRequestPage GetAddGroupRequestPage(string userID, int pageIndex, int pageSize)
        {
            return this.dbPersister.GetAddGroupRequestPage(userID, pageIndex, pageSize);
        }
        #endregion


        #region GroupBan4User
        public void InsertGroupBan4User(GroupBan groupBan)
        {
            this.dbPersister.InsertGroupBan4User(groupBan);
        }

        public void DeleteGroupBan4User(string groupID, string userID)
        {
            this.dbPersister.DeleteGroupBan4User(groupID, userID);
        }

        public GroupBan GetGroupBan4User(string groupID, string userID)
        {
            return this.dbPersister.GetGroupBan4User(groupID, userID);
        }

        public List<GroupBan> GetGroupBans4Group(string groupID)
        {
            return this.dbPersister.GetGroupBans4Group(groupID);
        }

        public bool ExistAllGroupBan(string groupID)
        {
            return this.dbPersister.ExistAllGroupBan(groupID);
        }
        #endregion

        public void ChangeUserState(string userID, UserState userState)
        {
            TUser user = this.GetUser(userID);
            if (user == null)
            {
                return;
            }
            user.UserState = userState;
            user.Version++;
            this.dbPersister.ChangeUserState(userID, userState, user.Version);
        }

        /// <summary>
        /// 更新用户离线时间
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="clientType">客户端设备类型</param>
        public void UpdateUserOfflineTime(string userID,ClientType clientType)
        {
            TUser user = this.userCache.Get(userID);
            DateTime now = DateTime.Now;
            if (clientType == ClientType.DotNET || clientType == ClientType.Linux)
            {
                this.dbPersister.UpdateUserPcOfflineTime(userID, now);
                if (user != null)
                {
                    user.PcOfflineTime = now;
                }
            }
            else if (clientType == ClientType.Android || clientType == ClientType.IOS)
            {
                this.dbPersister.UpdateUserMobileOfflineTime(userID, now);
                if (user != null)
                {
                    user.MobileOfflineTime = now;
                }
            }
        }
    }
}
