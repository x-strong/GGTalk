using System;
using System.Collections.Generic;
using System.Text;
using TalkBase.Server.Application;
using ESBasic.ObjectManagement.Managers;
using TalkBase.Server;
using TalkBase;
using ESBasic.Security;
using ESBasic;

namespace GGTalk.Server
{
    /// <summary>
    /// 内存虚拟数据库。
    /// </summary>
    public class MemoryPersister : OfflineMemoryCache, IDBPersisterExtend
    {
        private string pwdMD5 = SecurityHelper.MD5String2("1");
        private MessageRecordMemoryCache messageRecordMemoryCache = new MessageRecordMemoryCache();
        private ObjectManager<string, GGUser> userManager = new ObjectManager<string, GGUser>();
        private ObjectManager<string, GGGroup> groupManager = new ObjectManager<string, GGGroup>();
        //string :  requesterID + "-" + accepterID
        private ObjectManager<string, AddFriendRequest> addFriendRequestManager = new ObjectManager<string, AddFriendRequest>();
        //string :  requesterID + "-" + groupID
        private ObjectManager<string, AddGroupRequest> addGroupRequestManager = new ObjectManager<string, AddGroupRequest>();
        //string :  groupID + "-" + userID
        private ObjectManager<string, GroupBan> groupBanManager = new ObjectManager<string, GroupBan>();

        public MemoryPersister()
        {
            //10000 - 123123
            this.userManager.Add("10000", new GGUser("10000", pwdMD5, "张超", "我的好友:10001,10002,10003,10004", "#006", "每一天都是崭新的！", 0, "*G001,*G002"));
            this.userManager.Add("10001", new GGUser("10001", pwdMD5, "刘海", "我的好友:10002,10000,10003,10004", "#006", "加油，努力。", 1, "*G001,*G002"));
            this.userManager.Add("10002", new GGUser("10002", pwdMD5, "马小华", "我的好友:10001,10000,10003,10004", "#003", "随风而逝...", 2, "*G001,*G002"));
            this.userManager.Add("10003", new GGUser("10003", pwdMD5, "李建平", "我的好友:10000,10001,10002", "#003", "有事请call我", 3, "*G001,*G002"));
            this.userManager.Add("10004", new GGUser("10004", pwdMD5, "刘珊琪", "", "#004", "岁月是把杀猪刀", 4, "*G001,*G002"));
            this.userManager.Add("10005", new GGUser("10005", pwdMD5, "周晓新", "", "#004", "我想静静......", 0, "*G001"));
            this.userManager.Add("10006", new GGUser("10006", pwdMD5, "李文畅", "", "#005", "加油，努力。", 5, "*G001"));
            this.userManager.Add("10007", new GGUser("10007", pwdMD5, "王云", "", "#005", "每一天都是崭新的！", 6, "*G001"));
            this.userManager.Add("10008", new GGUser("10008", pwdMD5, "陈思思", "", "#007", "我命由我不由天", 1, "*G001"));
            this.userManager.Add("10009", new GGUser("10009", pwdMD5, "周嘉怡", "", "#007", "这只是你的错觉...", 2, "*G001"));
            this.userManager.Add("10010", new GGUser("10010", pwdMD5, "王承轩", "", "#007", "自强不息，厚德载物。", 3, "*G001"));
            List<GGUser> ggUsers = this.userManager.GetAll();
            foreach (GGUser user in ggUsers)
            {
                user.Phone = "135123" + user.ID;
            }

            this.groupManager.Add("*G001", new GGGroup("*G001", "测试群1", "10000", "本周周末安排加班！", "10000,10001,10002,10003,10004,10005,10006,10007,10008,10009,10010", false));
            this.groupManager.Add("*G002", new GGGroup("*G002", "测试群2", "10000", "春节长假快到了，请大家做好收尾工作！", "10000,10001,10002,10003,10004",false));
        }

        #region GetAllUser
        public List<GGUser> GetAllUser()
        {
            return this.userManager.GetAll();
        }
        #endregion

        public void DeleteGroup(string groupID)
        {
            this.groupManager.Remove(groupID);
        }

        public List<GGGroup> SearchGroup(string idOrName)
        {
            List<GGGroup> ggGroups = new List<GGGroup>();
            foreach (GGGroup group in this.groupManager.GetAll())
            {
                string id = idOrName.StartsWith(FunctionOptions.PrefixGroupID) ? idOrName : FunctionOptions.PrefixGroupID + idOrName;
                if (group.ID == id || group.Name == idOrName)
                {
                    ggGroups.Add(group);
                }
            }
            return ggGroups;
        }

        #region GetAllGroup
        public List<GGGroup> GetAllGroup()
        {
            return this.groupManager.GetAll();
        }
        #endregion

        public void UpdateUserFriends(GGUser t)
        {
            GGUser user = this.userManager.Get(t.UserID);
            if (user != null)
            {
                t.Version = user.Version + 1;
            }
            this.userManager.Add(t.UserID, t);
        }

        public void InsertUser(GGUser t)
        {
            this.userManager.Add(t.UserID, t);
        }

        public void InsertGroup(GGGroup t)
        {
            this.groupManager.Add(t.GroupID, t);
        }

        public void UpdateUser(GGUser t)
        {
            GGUser user = this.userManager.Get(t.UserID);
            if (user != null)
            {
                t.Version = user.Version + 1;
            }
            this.userManager.Add(t.UserID, t);
        }

        /// <summary>
        /// 更新PC端最后离线时间
        /// </summary>
        /// <param name="userID"></param>
        public void UpdateUserPcOfflineTime(string userID, DateTime dateTime) 
        {
            GGUser user = this.userManager.Get(userID);
            if (user != null)
            {
                user.PcOfflineTime = dateTime;                    
            }      
        }

        /// <summary>
        /// 更新手机最后离线时间
        /// </summary>
        /// <param name="userID"></param>
        public void UpdateUserMobileOfflineTime(string userID, DateTime dateTime)
        {
            GGUser user = this.userManager.Get(userID);
            if (user != null)
            {
                user.MobileOfflineTime = dateTime;
            }
        }

        public void UpdateGroup(GGGroup t)
        {
            GGGroup group = this.groupManager.Get(t.GroupID);
            if (group != null)
            {
                t.Version = group.Version+1;
            }
            this.groupManager.Add(t.GroupID, t);
        }


        public void UpdateUserGroups(GGUser t)
        {
            GGUser user = this.userManager.Get(t.UserID);
            if (user != null)
            {
                t.Version = user.Version + 1;
            }
            this.userManager.Add(t.UserID, t);
        }

        public void UpdateGroupMembers(GGGroup t)
        {
            GGGroup group = this.groupManager.Get(t.GroupID);
            group.Members = t.Members;
            group.Version += 1;
        }

        public void UpdateGroupInfo(GGGroup t)
        {
            GGGroup group = this.groupManager.Get(t.GroupID);
            if (group != null)
            {
                t.Version = group.Version + 1;
            }
            this.groupManager.Add(t.GroupID, t);
        }

        public GGUser GetUser(string userID)
        {
            return this.userManager.Get(userID);
        }

        public List<GGUser> SearchUser(string idOrName)
        {
            List<GGUser> ggUsers = new List<GGUser>();
            foreach (GGUser ggUser in this.userManager.GetAll())
            {
                if (ggUser.ID == idOrName || ggUser.Name == idOrName)
                {
                    ggUsers.Add(ggUser);
                }
            }
            return ggUsers;
        }

        public GGUser GetUser4Phone(string phone)
        {
           List<GGUser> ggUsers= this.userManager.GetAll();
           return ggUsers.Find((x) => { return x.Phone == phone; });
        }

        public List<GGUser> GetUserList4Phone(string phone)
        {
            List<GGUser> ggUsers = this.userManager.GetAll();
            return ggUsers.FindAll((x) => { return x.Phone == phone; });
        }

        public string GetPhone4UserID(string userID)
        {
            GGUser ggUser = this.userManager.Get(userID);
            if (ggUser == null)
            {
                return string.Empty;
            }
            return ggUser.Phone;
        }

        public int GetUserCount4Phone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return 0;
            }
            try
            {
                return this.userManager.GetAll().FindAll(x => x.Phone == phone).Count;
            }
            catch
            {
                return 0;
            }            
        }

        public string GetUserPassword(string userID)
        {
            GGUser user = this.userManager.Get(userID);
            if (user != null)
            {
                return user.PasswordMD5;
            }

            return null;
        }


        public GGGroup GetGroup(string groupID)
        {
            return this.groupManager.Get(groupID);
        }
        #region ChatMessageRecord

        public int InsertChatMessageRecord(ChatMessageRecord record)
        {
            return this.messageRecordMemoryCache.InsertChatMessageRecord(record);
        }
        public ChatMessageRecord GetChatMessageRecord(int id)
        {
            return this.messageRecordMemoryCache.GetChatMessageRecord(id);
        }

        public ChatRecordPage GetChatRecordPage(ChatRecordTimeScope timeScope, string myID, string friendID, int pageSize, int pageIndex)
        {
            return this.messageRecordMemoryCache.GetChatRecordPage(timeScope, myID, friendID, pageSize, pageIndex);
        }

        public ChatRecordPage GetGroupChatRecordPage(ChatRecordTimeScope timeScope, string groupID, int pageSize, int pageIndex)
        {
            return this.messageRecordMemoryCache.GetGroupChatRecordPage(timeScope, groupID, pageSize, pageIndex);
        }


        public ChatRecordPage GetChatRecordPage(DateTimeScope timeScope, string myID, string friendID, int pageSize, int pageIndex)
        {
            return this.messageRecordMemoryCache.GetChatRecordPage(ChatRecordTimeScope.All, myID, friendID, pageSize, pageIndex);
        }

        public ChatRecordPage GetGroupChatRecordPage(DateTimeScope timeScope, string groupID, int pageSize, int pageIndex)
        {
            return this.messageRecordMemoryCache.GetGroupChatRecordPage(ChatRecordTimeScope.All, groupID, pageSize, pageIndex);
        } 
        #endregion


        #region GroupOfflineMessage
        public Dictionary<string, List<ChatMessageRecord>> PickupGroupOfflineChatMessageRecord(string destUserID, DateTime startTime)
        {

            GGUser ggUser = this.GetUser(destUserID);
            if (ggUser == null)
            {
                return new Dictionary<string, List<ChatMessageRecord>>();
            }
            List<string> groupIDs = ggUser.GroupList;
            if (groupIDs == null || groupIDs.Count == 0)
            {
                return new Dictionary<string, List<ChatMessageRecord>>();
            }            
            Dictionary<string, List<ChatMessageRecord>> groupOfflineMessages = new Dictionary<string, List<ChatMessageRecord>>();
            foreach (string groupID in groupIDs)
            {
                List<ChatMessageRecord> messageRecords = this.messageRecordMemoryCache.GetChatRecordList(groupID, startTime);
                if (messageRecords.Count == 0)
                {
                    continue;
                }
                groupOfflineMessages.Add(groupID, messageRecords);             
            }
            return groupOfflineMessages;
        }


        #endregion


        public void UpdateUserCommentNames(GGUser t)
        {
            GGUser user = this.userManager.Get(t.UserID);
            if (user != null)
            {
                t.Version = user.Version + 1;
            }
            this.userManager.Add(t.UserID, t);
        }

        public void UpdateUserInfo(string userID, string name, string signature, string department, int version)
        {
           GGUser ggUser=  this.userManager.Get(userID);
            if (ggUser == null)
            {
                return;
            }
            ggUser.Name = name;
            ggUser.Signature = signature;
            ggUser.OrgID = department;
            ggUser.Version = version;
        }

        public void UpdateUserHeadImage(string userID, int defaultHeadImageIndex, byte[] customizedHeadImage, int version)
        {
            GGUser ggUser = this.userManager.Get(userID);
            if (ggUser == null)
            {
                return;
            }
            ggUser.HeadImageIndex = defaultHeadImageIndex;
            ggUser.HeadImageData = customizedHeadImage;
            ggUser.Version = version;
        }

        public void UpdateUserPhone(string userID, string phone, int version)
        {
            GGUser ggUser = this.userManager.Get(userID);
            if (ggUser == null)
            {
                return;
            }
            ggUser.Phone = phone;
            ggUser.Version = version;
        }

        public void UpdateUserPassword(string userID, string newPasswordMD5)
        {
            GGUser user = this.userManager.Get(userID);
            if (user != null)
            {
                user.PasswordMD5 = newPasswordMD5;
            }
        }

        public void ChangeUserState(string userID, UserState userState, int version)
        {
            GGUser user = this.userManager.Get(userID);
            if (user != null)
            {
                user.UserState = userState;
                user.Version = version;
            }
        }


        public void DeleteChatRecord(string myID, string friendID)
        {

        }

        public void DeleteGroupChatRecord(string groupID)
        {

        }


        public void DeleteUser(string userID)
        {
            this.userManager.Remove(userID);
        }


        public void UpdateUserBusinessInfo(GGUser user, Dictionary<string, byte[]> businessInfo, int version)
        {

        }

        public void ClearAllChatRecord()
        {
            this.messageRecordMemoryCache.Clear();
        }
        private const char H_Line_Char = '-';

        public void InsertAddFriendRequest(string requesterID, string accepterID, string requesterCatalogName, string comment,bool isNotified)
        {
            string key = requesterID + H_Line_Char + accepterID;
            if (this.addFriendRequestManager.Contains(key)) {
                this.addFriendRequestManager.Remove(key);
            }

            AddFriendRequest addFriendRequest = new AddFriendRequest()
            {
                RequesterID = requesterID,
                AccepterID = accepterID,
                RequesterCatalogName = requesterCatalogName,
                AccepterCatalogName = string.Empty,
                Comment2 = comment,
                State = (byte)RequsetType.Request,
                Notified = isNotified,
                CreateTime = DateTime.Now
            };
            this.addFriendRequestManager.Add(key, addFriendRequest);
        }

        public void SetAddFriendRequestNotified(string requesterID, string accepterID)
        {
            string key = requesterID + H_Line_Char + accepterID;
            if (this.addFriendRequestManager.Contains(key))
            {
                AddFriendRequest addFriendRequest = this.addFriendRequestManager.Get(key);
                addFriendRequest.Notified = true;
            }
        }

        public void UpdateAddFriendRequest(string requesterID, string accepterID, string requesterCatalogName, string accepterCatalogName, bool isAgreed)
        {
            string key = requesterID + H_Line_Char + accepterID;
            if (!this.addFriendRequestManager.Contains(key))
            {
                return;
            }
            AddFriendRequest addFriendRequest = this.addFriendRequestManager.Get(key);
            addFriendRequest.RequesterCatalogName = requesterCatalogName;
            addFriendRequest.AccepterCatalogName = accepterCatalogName;
            addFriendRequest.State= isAgreed ? (byte)RequsetType.Agree : (byte)RequsetType.Reject;
        }

        public List<AddFriendRequest> GetAddFriendRequest4NotNotified(string userID)
        {
            List<AddFriendRequest> list = new List<AddFriendRequest>();
            List <AddFriendRequest> allRequestList = this.addFriendRequestManager.GetAll();
            foreach (AddFriendRequest item in allRequestList)
            {
                if (item.Notified)
                {
                    continue;
                }
                if (item.AccepterID == userID)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public AddFriendRequestPage GetAddFriendRequestPage(string userID, int pageIndex, int pageSize)
        {
            List<AddFriendRequest> list = new List<AddFriendRequest>();
            List<AddFriendRequest> allRequestList = this.addFriendRequestManager.GetAll();
            foreach (AddFriendRequest item in allRequestList)
            {
                if (item.AccepterID == userID)
                {
                    list.Add(item);
                }
            }
            int entityCount = list.Count;
            list.Sort((x, y) => { return -x.CreateTime.CompareTo(y.CreateTime); });
            int startIndex = pageIndex * pageSize;
            if (entityCount <= startIndex)
            {
                return new AddFriendRequestPage() { AddFriendRequestList = new List<AddFriendRequest>(), TotalEntityCount = entityCount };
            }
            if (startIndex + pageSize > entityCount)
            {
                return new AddFriendRequestPage() { AddFriendRequestList = list.GetRange(startIndex, entityCount - startIndex), TotalEntityCount = entityCount };
            }
            return new AddFriendRequestPage() { AddFriendRequestList = list.GetRange(startIndex, pageSize), TotalEntityCount = entityCount };
        }

        public string GetRequesterCatalogName(string requesterID, string accepterID)
        {
            string key = requesterID + H_Line_Char + accepterID;
            if (!this.addFriendRequestManager.Contains(key))
            {
                return string.Empty;
            }
            return this.addFriendRequestManager.Get(key).RequesterCatalogName;
        }

        public void InsertAddGroupRequest(string requesterID, string groupID, string accepterID, string comment, bool isNotified)
        {
            AddGroupRequest addGroupRequest = new AddGroupRequest()
            {
                RequesterID = requesterID,
                AccepterID = accepterID,
                GroupID = groupID,
                Comment2 = comment,
                State = (byte)RequsetType.Request,
                Notified = isNotified,
                CreateTime = DateTime.Now
            };
            this.addGroupRequestManager.Add(requesterID + H_Line_Char + groupID, addGroupRequest);
        }

        public void SetAddGroupRequestNotified(string requesterID, string groupID)
        {
            string key = requesterID + H_Line_Char + groupID;
            if (this.addGroupRequestManager.Contains(key))
            {
                AddGroupRequest addGroupRequest = this.addGroupRequestManager.Get(key);
                addGroupRequest.Notified = true;
            }
        }

        public void UpdateAddGroupRequest(string requesterID, string groupID, bool isAgreed)
        {
            string key = requesterID + H_Line_Char + groupID;
            if (!this.addGroupRequestManager.Contains(key))
            {
                return;
            }
            AddGroupRequest addGroupRequest = this.addGroupRequestManager.Get(key);

            addGroupRequest.State = isAgreed ? (byte)RequsetType.Agree : (byte)RequsetType.Reject;
        }

        public void DeleteAddGroupRequest(string groupID)
        {
            foreach (string key in this.addGroupRequestManager.GetKeyList())
            {
                string[] ids = key.Split(H_Line_Char);
                if (ids[1] == groupID)
                {
                    this.addGroupRequestManager.Remove(key);
                }
            }
        }

        public List<AddGroupRequest> GetAddGroupRequest4NotNotified(string userID)
        {
            List<AddGroupRequest> list = new List<AddGroupRequest>();
            List<AddGroupRequest> allRequestList = this.addGroupRequestManager.GetAll();
            foreach (AddGroupRequest item in allRequestList)
            {
                if (item.Notified)
                {
                    continue;
                }
                if (item.AccepterID == userID)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public AddGroupRequestPage GetAddGroupRequestPage(string userID, int pageIndex, int pageSize)
        {
            List<AddGroupRequest> list = new List<AddGroupRequest>();
            List<AddGroupRequest> allRequestList = this.addGroupRequestManager.GetAll();
            foreach (AddGroupRequest item in allRequestList)
            {
                if (item.AccepterID == userID)
                {
                    list.Add(item);
                }
            }
            int entityCount = list.Count;
            list.Sort((x, y) => { return -x.CreateTime.CompareTo(y.CreateTime); });
            int startIndex = pageIndex * pageSize;
            if (entityCount <= startIndex)
            {
                return new AddGroupRequestPage() { AddGroupRequestList = new List<AddGroupRequest>(), TotalEntityCount = entityCount };
            }
            if (startIndex + pageSize > entityCount)
            {
                return new AddGroupRequestPage() { AddGroupRequestList = list.GetRange(startIndex, entityCount - startIndex), TotalEntityCount = entityCount };
            }
            return new AddGroupRequestPage() { AddGroupRequestList = list.GetRange(startIndex, pageSize), TotalEntityCount = entityCount };
        }


        /// <summary>
        /// DB中插入群禁言记录
        /// </summary>
        /// <param name="groupBan"></param>
        /// <returns>返回 DB中插入的autoID</returns>
        public void InsertGroupBan4User(GroupBan groupBan)
        {
            this.groupBanManager.Add(groupBan.GroupID + H_Line_Char + groupBan.UserID, groupBan);
        }

        /// <summary>
        /// 删除群中对应的人员的禁言记录
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="userID"></param>
        public void DeleteGroupBan4User(string groupID, string userID)
        {
            this.groupBanManager.Remove(groupID + H_Line_Char + userID);
        }

        public GroupBan GetGroupBan4User(string groupID, string userID)
        {           
            GroupBan groupBan = this.groupBanManager.Get(groupID + H_Line_Char + userID);
            if (groupBan != null && groupBan.EnableTime <= DateTime.Now)
            {
                this.groupBanManager.Remove(groupID + H_Line_Char + userID);
                return null;
            }
            return groupBan;
        }

        public List<GroupBan> GetGroupBans4Group(string groupID)
        {
            List<GroupBan> groupBans = new List<GroupBan>();
            Dictionary<string, GroupBan> groupBanDic = this.groupBanManager.ToDictionary();
            foreach (KeyValuePair<string, GroupBan> item in groupBanDic)
            {
                string[] key = item.Key.Split(H_Line_Char);
                if (key[0] == groupID && item.Value.EnableTime > DateTime.Now)
                {
                    groupBans.Add(item.Value);
                }
            }
            return groupBans;
        }

        public bool ExistAllGroupBan(string groupID)
        {
            foreach (GroupBan item in this.groupBanManager.GetAllReadonly())
            {
                if (item.GroupID == groupID && item.UserID == "")
                {
                    return true;
                }
            }
            return false;
        }



    }
}
