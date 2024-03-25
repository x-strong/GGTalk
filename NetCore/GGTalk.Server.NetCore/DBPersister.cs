using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Security;
using ESBasic.ObjectManagement.Managers;
using System.Configuration;
using ESBasic;
using TalkBase.Server;
using TalkBase;
using TalkBase.Server.Application;
using ESFramework;
using DataRabbit.DBAccessing.Application;
using DataRabbit.DBAccessing.ORM;
using DataRabbit;
using DataRabbit.DBAccessing;
using ChatMessageRecord = TalkBase.ChatMessageRecord;
using SqlSugar;
using System.Linq;
using GGTalk;

namespace GGTalk.Server
{   
    public interface IDBPersisterExtend: IDBPersister<GGUser, GGGroup>
    {
        /// <summary>
        /// 根据用户ID获取其手机号
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        string GetPhone4UserID(string userID);

        /// <summary>
        /// 更新用户手机号
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="phone"></param>
        void UpdateUserPhone(string userID, string phone, int version);

    }

    /// <summary>
    /// 与真实的SqlServer/MySQL数据库交互。
    /// </summary>
    public class DBPersister : DefaultChatRecordPersister, IDBPersisterExtend
    {
        private TransactionScopeFactory transactionScopeFactory;

        public DBPersister()
        {
            DataConfiguration config = null;
            DataBaseType dataBaseType = (DataBaseType)Enum.Parse(typeof(DataBaseType), ConfigurationManager.AppSettings["DBType"]);
            if (dataBaseType == DataBaseType.SqlServer)
            {
                config = new SqlDataConfiguration(ConfigurationManager.AppSettings["DBIP"], ConfigurationManager.AppSettings["DBUserID"], ConfigurationManager.AppSettings["DBPwd"], ConfigurationManager.AppSettings["DBName"], int.Parse(ConfigurationManager.AppSettings["DBPort"]));
            }
            else if(dataBaseType==DataBaseType.MySql)//MySQL
            {
                config = new MysqlDataConfiguration(ConfigurationManager.AppSettings["DBIP"], int.Parse(ConfigurationManager.AppSettings["DBPort"]), ConfigurationManager.AppSettings["DBUserID"], ConfigurationManager.AppSettings["DBPwd"], ConfigurationManager.AppSettings["DBName"]);
            }
            else if (dataBaseType == DataBaseType.GBase)//GBase
            {
                config = new GBaseDataConfiguration(ConfigurationManager.AppSettings["DBIP"], int.Parse(ConfigurationManager.AppSettings["DBPort"]), ConfigurationManager.AppSettings["DBUserID"], ConfigurationManager.AppSettings["DBPwd"], ConfigurationManager.AppSettings["DBName"]);
            }
            this.transactionScopeFactory = new TransactionScopeFactory(config);
            this.transactionScopeFactory.Initialize();
            base.Initialize(this.transactionScopeFactory);
            Console.WriteLine(dataBaseType + "DB初始化成功！");
        }

        public void InsertUser(GGUser t)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                accesser.Insert(t);
                scope.Commit();
            }
        }

        public void DeleteUser(string userID)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                accesser.Delete(userID);
                scope.Commit();
            }
        }

        public void InsertGroup(GGGroup t)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGGroup> accesser = scope.NewOrmAccesser<GGGroup>();
                accesser.Insert(t);
                scope.Commit();
            }
        }

        public void DeleteGroup(string groupID)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGGroup> accesser = scope.NewOrmAccesser<GGGroup>();
                accesser.Delete(groupID);
                scope.Commit();
            }
        }

        public void UpdateUser(GGUser t)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                accesser.Update(t);
                scope.Commit();
            }
        }

        public void UpdateUserFriends(GGUser t)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                List<ColumnUpdating> list = new List<ColumnUpdating>();
                list.Add(new ColumnUpdating(GGUser._Friends, t.Friends));
                list.Add(new ColumnUpdating(GGUser._Version, t.Version));
                accesser.Update(list, new Filter(GGUser._UserID, t.UserID));
                scope.Commit();
            }
        }

        public void UpdateGroup(GGGroup t)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGGroup> accesser = scope.NewOrmAccesser<GGGroup>();
                accesser.Update(t);
                scope.Commit();
            }
        }

        public List<GGUser> GetAllUser()
        {
            List<GGUser> list = new List<GGUser>();
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                list = accesser.GetAll();
                scope.Commit();
            }
            return list;
        }

        public List<GGGroup> SearchGroup(string idOrName)
        {
            List<GGGroup> list = new List<GGGroup>();
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGGroup> accesser = scope.NewOrmAccesser<GGGroup>();
                string id = idOrName.StartsWith(FunctionOptions.PrefixGroupID) ? idOrName : FunctionOptions.PrefixGroupID + idOrName;
                IFilterTree filterTree = new SimpleFilterTree(LogicType.Or, new Filter(GGGroup._GroupID, id), new Filter(GGGroup._Name, idOrName));
                list = accesser.GetMuch(filterTree);
                scope.Commit();
            }
            return list;
        }

        public List<GGGroup> GetAllGroup()
        {
            List<GGGroup> list = new List<GGGroup>();
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGGroup> accesser = scope.NewOrmAccesser<GGGroup>();
                list = accesser.GetAll();
                scope.Commit();
            }
            return list;
        }

        public void UpdateUserPassword(string userID, string newPasswordMD5)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                accesser.Update(new ColumnUpdating(GGUser._PasswordMD5, newPasswordMD5), new Filter(GGUser._UserID, userID));
                scope.Commit();
            }
        }

        public void ChangeUserState(string userID, UserState userState, int version)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                List<ColumnUpdating> columnUpdatings = new List<ColumnUpdating>();
                columnUpdatings.Add(new ColumnUpdating(GGUser._UserState, userState));
                columnUpdatings.Add(new ColumnUpdating(GGUser._Version, version));
                accesser.Update(columnUpdatings, new Filter(GGUser._UserID, userID));
                scope.Commit();
            }
        }

        public void UpdateUserGroups(GGUser user)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                List<ColumnUpdating> list = new List<ColumnUpdating>();
                list.Add(new ColumnUpdating(GGUser._Groups, user.Groups));
                list.Add(new ColumnUpdating(GGUser._Version, user.Version));
                accesser.Update(list, new Filter(GGUser._UserID, user.ID));
                scope.Commit();
            }
        }

        public void UpdateGroupMembers(GGGroup group)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGGroup> accesser = scope.NewOrmAccesser<GGGroup>();
                List<ColumnUpdating> list = new List<ColumnUpdating>();
                list.Add(new ColumnUpdating(GGGroup._Members, group.Members));
                list.Add(new ColumnUpdating(GGGroup._Version, group.Version));
                accesser.Update(list, new Filter(GGGroup._GroupID, group.ID));
                scope.Commit();
            }
        }

        public void UpdateGroupInfo(GGGroup t)
        {
            this.UpdateGroup(t);
        }

        public GGUser GetUser(string userID)
        {
            GGUser user = null;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                user = accesser.GetOne(userID);
                scope.Commit();
            }
            return user;
        }

        public List<GGUser> SearchUser(string idOrName)
        {
            List<GGUser> userList = new List<GGUser>();
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                userList = accesser.GetMuch(new SimpleFilterTree(LogicType.Or, new Filter(GGUser._UserID, idOrName), new Filter(GGUser._Name, idOrName)));
                scope.Commit();
            }
            return userList;
        }

        public GGUser GetUser4Phone(string phone)
        {
            GGUser user = null;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                user = accesser.GetOne(new Filter(GGUser._Phone, phone));
                scope.Commit();
            }
            return user;
        }

        public string GetPhone4UserID(string userID)
        {
            string phone = string.Empty;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                phone = (string)accesser.GetColumnValue(userID, GGUser._Phone);
                scope.Commit();
            }
            return phone;
        }

        public List<OfflineMessage> PickupOfflineMessage(string destUserID)
        {
            List<OfflineMessage> offlineMessages = null;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<OfflineMessage> accesser = scope.NewOrmAccesser<OfflineMessage>();
                Filter filter = new Filter(OfflineMessage._DestUserID, destUserID);
                offlineMessages = accesser.GetMuch(filter);
                accesser.Delete(filter);
                scope.Commit();
            }
            return offlineMessages;
        }

        public Dictionary<string, List<ChatMessageRecord>> PickupGroupOfflineChatMessageRecord(string destUserID, DateTime startTime)
        {
            GGUser GGUser = this.GetUser(destUserID);
            if (GGUser == null)
            {
                return new Dictionary<string, List<ChatMessageRecord>>();
            }
            List<string> groupIDs = GGUser.GroupList;
            if (groupIDs == null || groupIDs.Count == 0)
            {
                return new Dictionary<string, List<ChatMessageRecord>>();
            }
            Dictionary<string, List<ChatMessageRecord>> groupOfflineMessages = new Dictionary<string, List<ChatMessageRecord>>();
            foreach (string groupID in groupIDs)
            {
                List<ChatMessageRecord> messageRecords = this.GetChatRecordList(groupID, startTime);
                if (messageRecords.Count == 0)
                {
                    continue;
                }
                groupOfflineMessages.Add(groupID, messageRecords);
            }
            return groupOfflineMessages;
        }

        private List<ChatMessageRecord> GetChatRecordList(string groupID, DateTime startTime)
        {
            if (startTime > DateTime.Now)
            {
                return new List<ChatMessageRecord>();
            }
            List<ChatMessageRecord> records = new List<ChatMessageRecord>();
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
                IFilterTree filterTree = new SimpleFilterTree(LogicType.And, new Filter(ChatMessageRecord._IsGroupChat, true), new Filter(ChatMessageRecord._AudienceID, groupID), new Filter(ChatMessageRecord._OccureTime, startTime, ComparisonOperators.Greater));
                records = accesser.GetMuch(filterTree, null, new Orderby(ChatMessageRecord._AutoID, true));
                scope.Commit();
            }
            return records;
        }

        public void StoreOfflineMessage(OfflineMessage msg)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<OfflineMessage> accesser = scope.NewOrmAccesser<OfflineMessage>();
                accesser.Insert(msg);
                scope.Commit();
            }
        }

        public void StoreOfflineFileItem(OfflineFileItem item)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<OfflineFileItem> accesser = scope.NewOrmAccesser<OfflineFileItem>();
                accesser.Insert(item);
                scope.Commit();
            }
        }

        public List<OfflineFileItem> PickupOfflineFileItem(string accepterID)
        {
            List<OfflineFileItem> offlineFileItems = new List<OfflineFileItem>();
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<OfflineFileItem> accesser = scope.NewOrmAccesser<OfflineFileItem>();
                Filter filter = new Filter(OfflineFileItem._AccepterID, accepterID);
                offlineFileItems = accesser.GetMuch(filter);
                accesser.Delete(filter);
                scope.Commit();
            }
            return offlineFileItems;
        }

        public List<OfflineFileItem> PickupOfflineFileItem4Assistant(string accepterID, ClientType type)
        {
            List<OfflineFileItem> offlineFileItems = new List<OfflineFileItem>();
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<OfflineFileItem> accesser = scope.NewOrmAccesser<OfflineFileItem>();
                SimpleFilterTree filterTree = new SimpleFilterTree(new Filter(OfflineFileItem._AccepterID, accepterID), new Filter(OfflineFileItem._AccepterType, type));
                offlineFileItems = accesser.GetMuch(filterTree);
                accesser.Delete(filterTree);
                scope.Commit();
            }
            return offlineFileItems;
        }


        public string GetUserPassword(string userID)
        {
            object pwd = null;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                pwd = accesser.GetColumnValue(userID, GGUser._PasswordMD5);
                scope.Commit();
            }
            if (pwd == null)
            {
                return null;
            }
            return pwd.ToString();
        }

        public GGGroup GetGroup(string groupID)
        {
            GGGroup group = null;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGGroup> accesser = scope.NewOrmAccesser<GGGroup>();
                group = accesser.GetOne(groupID);
                scope.Commit();
            }
            return group;
        }

        public void UpdateUserInfo(string userID, string name, string signature, string orgID, int version)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                List<ColumnUpdating> list = new List<ColumnUpdating>();
                list.Add(new ColumnUpdating(GGUser._Signature, signature));
                list.Add(new ColumnUpdating(GGUser._Version, version));
                list.Add(new ColumnUpdating(GGUser._Name, name));
                list.Add(new ColumnUpdating(GGUser._OrgID, orgID));
                accesser.Update(list, new Filter(GGUser._UserID, userID));
                scope.Commit();
            }
        }

        public void UpdateUserHeadImage(string userID, int defaultHeadImageIndex, byte[] customizedHeadImage, int version)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                List<ColumnUpdating> list = new List<ColumnUpdating>();
                list.Add(new ColumnUpdating(GGUser._HeadImageIndex, defaultHeadImageIndex));
                list.Add(new ColumnUpdating(GGUser._HeadImageData, customizedHeadImage));
                list.Add(new ColumnUpdating(GGUser._Version, version));
                accesser.Update(list, new Filter(GGUser._UserID, userID));
                scope.Commit();
            }
        }

        /// <summary>
        /// 更新PC端最后离线时间
        /// </summary>
        /// <param name="userID"></param>
        public void UpdateUserPcOfflineTime(string userID, DateTime dateTime)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                accesser.Update(new ColumnUpdating(GGUser._PcOfflineTime, dateTime), new Filter(GGUser._UserID, userID));
                scope.Commit();
            }
        }

        /// <summary>
        /// 更新手机最后离线时间
        /// </summary>
        /// <param name="userID"></param>
        public void UpdateUserMobileOfflineTime(string userID, DateTime dateTime)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                accesser.Update(new ColumnUpdating(GGUser._MobileOfflineTime, dateTime), new Filter(GGUser._UserID, userID));
                scope.Commit();
            }
        }

        public void UpdateUserPhone(string userID, string phone, int version)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                List<ColumnUpdating> list = new List<ColumnUpdating>();
                list.Add(new ColumnUpdating(GGUser._Phone, phone));
                list.Add(new ColumnUpdating(GGUser._Version, version));
                accesser.Update(list, new Filter(GGUser._UserID, userID));
                scope.Commit();
            }
        }

        public void UpdateUserCommentNames(GGUser t)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                List<ColumnUpdating> list = new List<ColumnUpdating>();
                list.Add(new ColumnUpdating(GGUser._CommentNames, t.CommentNames));
                list.Add(new ColumnUpdating(GGUser._Version, t.Version));
                accesser.Update(list, new Filter(GGUser._UserID, t.UserID));
                scope.Commit();
            }
        }


        public void UpdateUserBusinessInfo(GGUser user, Dictionary<string, byte[]> businessInfo, int version)
        {

        }

        public void InsertAddFriendRequest(string requesterID, string accepterID, string requesterCatalogName, string comment, bool isNotified)
        {
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
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<AddFriendRequest> accesser = scope.NewOrmAccesser<AddFriendRequest>();
                //删除正在申请中的记录
                accesser.Delete(new Filter(AddFriendRequest._RequesterID, requesterID), new Filter(AddFriendRequest._AccepterID, accepterID), new Filter(AddFriendRequest._State, (byte)RequsetType.Request));
                //新增新记录
                accesser.Insert(addFriendRequest);
                scope.Commit();
            }
        }

        public void SetAddFriendRequestNotified(string requesterID, string accepterID)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<AddFriendRequest> accesser = scope.NewOrmAccesser<AddFriendRequest>();
                accesser.Update(new ColumnUpdating(AddFriendRequest._Notified, true), new Filter(AddFriendRequest._RequesterID, requesterID), new Filter(AddFriendRequest._AccepterID, accepterID));

                //    accesser.Delete(new Filter(AddFriendRequest._RequesterID, requesterID), new Filter(AddFriendRequest._AccepterID, accepterID));
                scope.Commit();
            }
        }

        public void UpdateAddFriendRequest(string requesterID, string accepterID, string requesterCatalogName, string accepterCatalogName, bool isAgreed)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<AddFriendRequest> accesser = scope.NewOrmAccesser<AddFriendRequest>();
                List<ColumnUpdating> columnUpdatings = new List<ColumnUpdating>();
                columnUpdatings.Add(new ColumnUpdating(AddFriendRequest._AccepterCatalogName, accepterCatalogName));
                columnUpdatings.Add(new ColumnUpdating(AddFriendRequest._State, isAgreed ? (byte)RequsetType.Agree : (byte)RequsetType.Reject));
                accesser.Update(columnUpdatings, new Filter(AddFriendRequest._RequesterID, requesterID), new Filter(AddFriendRequest._AccepterID, accepterID), new Filter(AddFriendRequest._State, (byte)RequsetType.Request));
                scope.Commit();
            }
        }

        public List<AddFriendRequest> GetAddFriendRequest4NotNotified(string userID)
        {
            List<AddFriendRequest> list = new List<AddFriendRequest>();
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<AddFriendRequest> accesser = scope.NewOrmAccesser<AddFriendRequest>();
                SimpleFilterTree simpleFilter = new SimpleFilterTree(new Filter(AddFriendRequest._AccepterID, userID), new Filter(AddFriendRequest._Notified, false));
                list = accesser.GetMuch(simpleFilter);
                scope.Commit();
            }
            return list;
        }

        public AddFriendRequestPage GetAddFriendRequestPage(string userID, int pageIndex, int pageSize)
        {
            AddFriendRequest[] addFriendRequests;
            int entityCount = 0;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<AddFriendRequest> accesser = scope.NewOrmAccesser<AddFriendRequest>();
                SimpleFilterTree simpleFilter = new SimpleFilterTree(LogicType.Or, new Filter(AddFriendRequest._AccepterID, userID));
                addFriendRequests = accesser.GetPage(simpleFilter, pageSize, pageIndex, AddFriendRequest._AutoID, false, out entityCount);
                scope.Commit();
            }
            List<AddFriendRequest> friendRequestList = new List<AddFriendRequest>();
            friendRequestList.AddRange(addFriendRequests);
            return new AddFriendRequestPage() { AddFriendRequestList = friendRequestList, TotalEntityCount = entityCount };
        }

        public string GetRequesterCatalogName(string requesterID, string accepterID)
        {
            AddFriendRequest entity = null;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<AddFriendRequest> accesser = scope.NewOrmAccesser<AddFriendRequest>();
                entity = accesser.GetOne(new Filter(AddFriendRequest._RequesterID, requesterID), new Filter(AddFriendRequest._AccepterID, accepterID), new Filter(AddFriendRequest._State, (byte)RequsetType.Request));
                scope.Commit();
            }
            if (entity == null)
            {
                return string.Empty;
            }
            return entity.RequesterCatalogName;
        }

        public void InsertAddGroupRequest(string requesterID, string groupID, string accepterID, string comment, bool isNotified)
        {
            AddGroupRequest addGroupRequest = new AddGroupRequest()
            {
                RequesterID = requesterID,
                GroupID = groupID,
                AccepterID = accepterID,
                Comment2 = comment,
                Notified = isNotified,
                State = (byte)RequsetType.Request,
                CreateTime = DateTime.Now
            };
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<AddGroupRequest> accesser = scope.NewOrmAccesser<AddGroupRequest>();
                //删除正在申请中的记录
                accesser.Delete(new Filter(AddGroupRequest._RequesterID, requesterID), new Filter(AddGroupRequest._GroupID, groupID), new Filter(AddGroupRequest._State, (byte)RequsetType.Request));
                //新增新记录
                accesser.Insert(addGroupRequest);
                scope.Commit();
            }
        }

        public void SetAddGroupRequestNotified(string requesterID, string groupID)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<AddGroupRequest> accesser = scope.NewOrmAccesser<AddGroupRequest>();
                accesser.Update(new ColumnUpdating(AddGroupRequest._Notified, true), new Filter(AddGroupRequest._RequesterID, requesterID), new Filter(AddGroupRequest._GroupID, groupID));
                scope.Commit();
            }
        }

        public void UpdateAddGroupRequest(string requesterID, string groupID, bool isAgreed)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<AddGroupRequest> accesser = scope.NewOrmAccesser<AddGroupRequest>();
                List<ColumnUpdating> columnUpdatings = new List<ColumnUpdating>();
                columnUpdatings.Add(new ColumnUpdating(AddGroupRequest._State, isAgreed ? (byte)RequsetType.Agree : (byte)RequsetType.Reject));
                accesser.Update(columnUpdatings, new Filter(AddGroupRequest._RequesterID, requesterID), new Filter(AddGroupRequest._GroupID, groupID), new Filter(AddGroupRequest._State, (byte)RequsetType.Request));
                scope.Commit();
            }
        }

        public void DeleteAddGroupRequest(string groupID)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<AddGroupRequest> accesser = scope.NewOrmAccesser<AddGroupRequest>();
                accesser.Delete(new Filter(AddGroupRequest._GroupID, groupID));
                scope.Commit();
            }
        }

        public List<AddGroupRequest> GetAddGroupRequest4NotNotified(string userID)
        {
            List<AddGroupRequest> list = new List<AddGroupRequest>();
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<AddGroupRequest> accesser = scope.NewOrmAccesser<AddGroupRequest>();
                SimpleFilterTree tree = new SimpleFilterTree(new Filter(AddGroupRequest._AccepterID, userID), new Filter(AddGroupRequest._Notified, false));
                list = accesser.GetMuch(tree);
                scope.Commit();
            }
            return list;
        }

        public AddGroupRequestPage GetAddGroupRequestPage(string userID, int pageIndex, int pageSize)
        {
            AddGroupRequest[] addGroupRequests;
            int entityCount = 0;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<AddGroupRequest> accesser = scope.NewOrmAccesser<AddGroupRequest>();
                addGroupRequests = accesser.GetPage(new SimpleFilterTree(new Filter(AddGroupRequest._AccepterID, userID)), pageSize, pageIndex, AddGroupRequest._AutoID, false, out entityCount);
                scope.Commit();
            }
            List<AddGroupRequest> friendRequestList = new List<AddGroupRequest>();
            friendRequestList.AddRange(addGroupRequests);
            return new AddGroupRequestPage() { AddGroupRequestList = friendRequestList, TotalEntityCount = entityCount };
        }

        public void InsertGroupBan4User(GroupBan groupBan)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GroupBan> accesser = scope.NewOrmAccesser<GroupBan>();
                accesser.Delete(new Filter(GroupBan._GroupID, groupBan.GroupID), new Filter(GroupBan._UserID, groupBan.UserID));
                accesser.Insert(groupBan);
                scope.Commit();
            }
        }

        public void DeleteGroupBan4User(string groupID, string userID)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GroupBan> accesser = scope.NewOrmAccesser<GroupBan>();
                accesser.Delete(new Filter(GroupBan._GroupID, groupID), new Filter(GroupBan._UserID, userID));
                scope.Commit();
            }
        }

        public GroupBan GetGroupBan4User(string groupID, string userID)
        {
            GroupBan groupBan = null;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GroupBan> accesser = scope.NewOrmAccesser<GroupBan>();
                groupBan = accesser.GetOne(new Filter(GroupBan._GroupID, groupID), new Filter(GroupBan._UserID, userID), new Filter(GroupBan._EnableTime, DateTime.Now, ComparisonOperators.Greater));
                scope.Commit();
            }
            return groupBan;
        }

        public List<GroupBan> GetGroupBans4Group(string groupID)
        {
            List<GroupBan> groupBans = new List<GroupBan>();
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GroupBan> accesser = scope.NewOrmAccesser<GroupBan>();
                groupBans = accesser.GetMuch(new Filter(GroupBan._GroupID, groupID), new Filter(GroupBan._EnableTime, DateTime.Now, ComparisonOperators.Greater));
                scope.Commit();
            }
            return groupBans;
        }

        public bool ExistAllGroupBan(string groupID)
        {
            GroupBan groupBan = null;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GroupBan> accesser = scope.NewOrmAccesser<GroupBan>();
                //是否开启全员禁言
                groupBan = accesser.GetOne(new Filter(GroupBan._GroupID, groupID), new Filter(GroupBan._UserID, ""));
                scope.Commit();
            }
            return groupBan != null;
        }
    }



    /// <summary>
    /// 通过SqlSugar与真实的数据库交互。
    /// </summary>
    public class DBPersister_SqlSugar : DefaultChatRecordPersister2, IDBPersisterExtend
    {
        private SqlSugarClient db;

        public DBPersister_SqlSugar(DataBaseType dataBaseType)
        {
            DbType dbType = (DbType)dataBaseType;
            this.ConnectionDb(dbType);
        }

        public DBPersister_SqlSugar(DbType dbType)
        {
            this.ConnectionDb(dbType);
        }

        private void ConnectionDb(DbType dbType)
        {
            string connectionString = string.Format("server={0};database={1};userid={2};pwd={3};port={4}", ConfigurationManager.AppSettings["DBIP"], ConfigurationManager.AppSettings["DBName"], ConfigurationManager.AppSettings["DBUserID"], ConfigurationManager.AppSettings["DBPwd"], int.Parse(ConfigurationManager.AppSettings["DBPort"]));

            if (dbType == DbType.Dm)
            {
                connectionString = string.Format("server={0};userid={1};pwd={2};port={3}", ConfigurationManager.AppSettings["DBIP"], ConfigurationManager.AppSettings["DBUserID"], ConfigurationManager.AppSettings["DBPwd"], int.Parse(ConfigurationManager.AppSettings["DBPort"]));
            }
            else if (dbType == DbType.Oracle)
            {
                connectionString = string.Format("Data Source={0}/{1};User ID={2};Password={3};", ConfigurationManager.AppSettings["DBIP"], ConfigurationManager.AppSettings["DBName"], ConfigurationManager.AppSettings["DBUserID"], ConfigurationManager.AppSettings["DBPwd"]);
            }
            //到时在加工
            db = new SqlSugarClient(new ConnectionConfig()
            {
                DbType = dbType,
                ConnectionString = connectionString,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                AopEvents = new AopEvents
                {
                    OnLogExecuting = (sql, p) =>
                    {
                        Console.WriteLine(sql);
                        Console.WriteLine(string.Join(",", p?.Select(it => it.ParameterName + ":" + it.Value)));
                    }
                }
            });
            base.Initialize(db);
        }

        public void InsertUser(GGUser t)
        {
            db.Insertable(t).ExecuteCommand();
        }

        public void DeleteUser(string userID)
        {
            db.Deleteable<GGUser>().Where(ou => ou.UserID == userID).ExecuteCommand();
        }

        public void InsertGroup(GGGroup t)
        {
            db.Insertable(t).ExecuteCommand();
        }

        public void DeleteGroup(string groupID)
        {
            db.Deleteable<GGGroup>().Where(og => og.GroupID == groupID).ExecuteCommand();
        }

        public void UpdateUser(GGUser t)
        {
            db.Updateable(t).Where(it => it.UserID == t.UserID).ExecuteCommand();
        }

        public void UpdateUserFriends(GGUser t)
        {
            db.Updateable(t).UpdateColumns(it => new { it.Friends, it.Version }).Where(it => it.UserID == t.UserID).ExecuteCommand();
        }

        public void UpdateGroup(GGGroup t)
        {
            db.Updateable(t).Where(it => it.GroupID == t.GroupID).ExecuteCommand();
        }

        public List<GGUser> GetAllUser()
        {
            return db.Queryable<GGUser>().ToList();
        }

        public List<GGGroup> SearchGroup(string idOrName)
        {
            string id = idOrName.StartsWith(FunctionOptions.PrefixGroupID) ? idOrName : FunctionOptions.PrefixGroupID + idOrName;
            return db.Queryable<GGGroup>().Where(it => it.GroupID == id || it.Name.Contains(idOrName)).ToList();
        }

        public List<GGGroup> GetAllGroup()
        {
            return db.Queryable<GGGroup>().ToList();
        }

        public void UpdateUserPassword(string userID, string newPasswordMD5)
        {
            db.Updateable<GGUser>(it => it.PasswordMD5 == newPasswordMD5).Where(it => it.UserID == userID).ExecuteCommand();
        }

        public void ChangeUserState(string userID, UserState userState, int version)
        {
            db.Updateable<GGUser>(it => new GGUser() { UserState = userState, Version = version }).Where(it => it.UserID == userID).ExecuteCommand();
        }

        public void UpdateUserGroups(GGUser user)
        {
            db.Updateable(user).UpdateColumns(it => new { it.Groups, it.Version }).Where(it => it.UserID == user.UserID).ExecuteCommand();
        }

        public void UpdateGroupMembers(GGGroup group)
        {
            db.Updateable(group).UpdateColumns(it => new { it.Members, it.Version }).Where(it => it.GroupID == group.GroupID).ExecuteCommand();
        }

        public void UpdateGroupInfo(GGGroup t)
        {
            db.Updateable(t).Where(it => it.GroupID == t.GroupID).ExecuteCommand();
        }

        public GGUser GetUser(string userID)
        {
            GGUser user = null;
            user = db.Queryable<GGUser>().Where(it => it.UserID == userID).First();
            return user;
        }

        public List<GGUser> SearchUser(string idOrName)
        {
            return db.Queryable<GGUser>().Where(it => it.UserID == idOrName || it.Name.Contains(idOrName)).ToList();
        }

        public GGUser GetUser4Phone(string phone)
        {
            GGUser user = null;
            user = db.Queryable<GGUser>().Where(it => it.Phone == phone).First();
            return user;
        }

        public string GetPhone4UserID(string userID)
        {
            string phone = string.Empty;
            phone = db.Queryable<GGUser>().Where(it => it.UserID == userID).Select(it => it.Phone).First();
            return phone;
        }

        public List<OfflineMessage> PickupOfflineMessage(string destUserID)
        {
            List<OfflineMessage> offlineMessages = null;
            offlineMessages = db.Queryable<OfflineMessage>().Where(it => it.DestUserID == destUserID).ToList();
            db.Deleteable<OfflineMessage>().Where(it => it.DestUserID == destUserID).ExecuteCommand(); ;
            return offlineMessages;
        }

        public Dictionary<string, List<ChatMessageRecord>> PickupGroupOfflineChatMessageRecord(string destUserID, DateTime startTime)
        {
            GGUser GGUser = this.GetUser(destUserID);
            if (GGUser == null)
            {
                return new Dictionary<string, List<ChatMessageRecord>>();
            }
            List<string> groupIDs = GGUser.GroupList;
            if (groupIDs == null || groupIDs.Count == 0)
            {
                return new Dictionary<string, List<ChatMessageRecord>>();
            }
            Dictionary<string, List<ChatMessageRecord>> groupOfflineMessages = new Dictionary<string, List<ChatMessageRecord>>();
            foreach (string groupID in groupIDs)
            {
                List<ChatMessageRecord> messageRecords = this.GetChatRecordList(groupID, startTime);
                if (messageRecords.Count == 0)
                {
                    continue;
                }
                groupOfflineMessages.Add(groupID, messageRecords);
            }
            return groupOfflineMessages;
        }

        private List<ChatMessageRecord> GetChatRecordList(string groupID, DateTime startTime)
        {
            if (startTime > DateTime.Now)
            {
                return new List<ChatMessageRecord>();
            }
            List<ChatMessageRecord> records = new List<ChatMessageRecord>();
            records = db.Queryable<ChatMessageRecord>().Where(it => it.IsGroupChat == true && it.AudienceID == groupID && it.OccureTime > startTime).OrderBy(it => it.AutoID).ToList();

            return records;
        }

        public void StoreOfflineMessage(OfflineMessage msg)
        {
            db.Insertable(msg).ExecuteCommand();
        }

        public void StoreOfflineFileItem(OfflineFileItem item)
        {
            db.Insertable(item).ExecuteCommand();
        }

        public List<OfflineFileItem> PickupOfflineFileItem(string accepterID)
        {
            List<OfflineFileItem> offlineFileItems = new List<OfflineFileItem>();
            offlineFileItems = db.Queryable<OfflineFileItem>().Where(it => it.AccepterID == accepterID).ToList();
            db.Deleteable<OfflineFileItem>().Where(it => it.AccepterID == accepterID).ExecuteCommand();
            return offlineFileItems;
        }

        public List<OfflineFileItem> PickupOfflineFileItem4Assistant(string accepterID, ClientType type)
        {
            List<OfflineFileItem> offlineFileItems = new List<OfflineFileItem>();
            db.Queryable<OfflineFileItem>().Where(it => it.AccepterID == accepterID && it.AccepterType == type).ToList();
            db.Deleteable<OfflineFileItem>().Where(it => it.AccepterID == accepterID && it.AccepterType == type).ExecuteCommand();
            return offlineFileItems;
        }


        public string GetUserPassword(string userID)
        {
            object pwd = null;
            pwd = db.Queryable<GGUser>().Where(it => it.UserID == userID).Select(it => it.PasswordMD5).First();

            if (pwd == null)
            {
                return null;
            }
            return pwd.ToString();
        }

        public GGGroup GetGroup(string groupID)
        {
            GGGroup group = null;
            group = db.Queryable<GGGroup>().Where(it => it.GroupID == groupID).First();
            return group;
        }

        public void UpdateUserInfo(string userID, string name, string signature, string orgID, int version)
        {
            db.Updateable<GGUser>(it => new GGUser() { Signature = signature, Name = name, OrgID = orgID, Version = version }).Where(it => it.UserID == userID).ExecuteCommand();
        }

        public void UpdateUserHeadImage(string userID, int defaultHeadImageIndex, byte[] customizedHeadImage, int version)
        {
            db.Updateable<GGUser>(it => new GGUser() { HeadImageIndex = defaultHeadImageIndex, HeadImageData = customizedHeadImage, Version = version }).Where(it => it.UserID == userID).ExecuteCommand();
        }

        /// <summary>
        /// 更新PC端最后离线时间
        /// </summary>
        /// <param name="userID"></param>
        public void UpdateUserPcOfflineTime(string userID, DateTime dateTime)
        {
            db.Updateable<GGUser>(it => it.PcOfflineTime == dateTime).Where(it => it.UserID == userID).ExecuteCommand();
        }

        /// <summary>
        /// 更新手机最后离线时间
        /// </summary>
        /// <param name="userID"></param>
        public void UpdateUserMobileOfflineTime(string userID, DateTime dateTime)
        {
            db.Updateable<GGUser>(it => it.MobileOfflineTime == dateTime).Where(it => it.UserID == userID).ExecuteCommand();
        }

        public void UpdateUserPhone(string userID, string phone, int version)
        {
            db.Updateable<GGUser>(it => new GGUser { Phone = phone, Version = version }).Where(it => it.UserID == userID).ExecuteCommand();
        }

        public void UpdateUserCommentNames(GGUser t)
        {
            db.Updateable(t).UpdateColumns(it => new { it.CommentNames, it.Version }).Where(it => it.UserID == t.UserID).ExecuteCommand();
        }


        public void UpdateUserBusinessInfo(GGUser user, Dictionary<string, byte[]> businessInfo, int version)
        {

        }

        public void InsertAddFriendRequest(string requesterID, string accepterID, string requesterCatalogName, string comment, bool isNotified)
        {
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

            db.Deleteable<AddFriendRequest>().Where(it => it.RequesterID == requesterID && it.AccepterID == accepterID && it.State == (byte)RequsetType.Request).ExecuteCommand();
            db.Insertable(addFriendRequest).ExecuteCommand();

        }

        public void SetAddFriendRequestNotified(string requesterID, string accepterID)
        {
            db.Updateable<AddFriendRequest>(it => new AddFriendRequest() { Notified = true }).Where(it => it.RequesterID == requesterID && it.AccepterID == accepterID).ExecuteCommand();
        }

        public void UpdateAddFriendRequest(string requesterID, string accepterID, string requesterCatalogName, string accepterCatalogName, bool isAgreed)
        {
            int strState = isAgreed ? (byte)RequsetType.Agree : (byte)RequsetType.Reject;
            db.Updateable<AddFriendRequest>(it => new AddFriendRequest() { AccepterCatalogName = accepterCatalogName, State = strState })
                .Where(it => it.RequesterID == requesterID && it.AccepterID == accepterID && it.State == (byte)RequsetType.Request).ExecuteCommand();
        }

        public List<AddFriendRequest> GetAddFriendRequest4NotNotified(string userID)
        {
            List<AddFriendRequest> list = new List<AddFriendRequest>();
            list = db.Queryable<AddFriendRequest>().Where(it => it.AccepterID == userID && it.Notified == false).ToList();
            return list;
        }

        public AddFriendRequestPage GetAddFriendRequestPage(string userID, int pageIndex, int pageSize)
        {
            int entityCount = 0;

            List<AddFriendRequest> friendRequestList = db.Queryable<AddFriendRequest>().Where(it => it.AccepterID == userID).ToPageList(pageIndex+1, pageSize, ref entityCount);

            return new AddFriendRequestPage() { AddFriendRequestList = friendRequestList, TotalEntityCount = entityCount };
        }

        public string GetRequesterCatalogName(string requesterID, string accepterID)
        {
            AddFriendRequest entity = null;

            entity = db.Queryable<AddFriendRequest>().Where(it => it.RequesterID == requesterID && it.AccepterID == accepterID && it.State == (byte)RequsetType.Request).First();

            if (entity == null)
            {
                return string.Empty;
            }
            return entity.RequesterCatalogName;
        }

        public void InsertAddGroupRequest(string requesterID, string groupID, string accepterID, string comment, bool isNotified)
        {
            AddGroupRequest addGroupRequest = new AddGroupRequest()
            {
                RequesterID = requesterID,
                GroupID = groupID,
                AccepterID = accepterID,
                Comment2 = comment,
                Notified = isNotified,
                State = (byte)RequsetType.Request,
                CreateTime = DateTime.Now
            };

            db.Deleteable<AddGroupRequest>().Where(it => it.RequesterID == requesterID && it.GroupID == groupID && it.State == (byte)RequsetType.Request);
            db.Insertable(addGroupRequest).ExecuteCommand();

        }

        public void SetAddGroupRequestNotified(string requesterID, string groupID)
        {
            db.Updateable<AddGroupRequest>(it => new AddGroupRequest() { Notified = true }).Where(it => it.GroupID == groupID && it.RequesterID == requesterID).ExecuteCommand();
        }

        public void UpdateAddGroupRequest(string requesterID, string groupID, bool isAgreed)
        {
            int state = isAgreed ? (byte)RequsetType.Agree : (byte)RequsetType.Reject;
            int state2 = (byte)RequsetType.Request;
            db.Updateable<AddGroupRequest>(it => new AddGroupRequest() { State = state }).
                Where(it => it.GroupID == groupID && it.RequesterID == requesterID && it.State == state2).ExecuteCommand();
        }

        public void DeleteAddGroupRequest(string groupID)
        {
            db.Deleteable<AddGroupRequest>().Where(it => it.GroupID == groupID).ExecuteCommand();
        }

        public List<AddGroupRequest> GetAddGroupRequest4NotNotified(string userID)
        {
            List<AddGroupRequest> list = new List<AddGroupRequest>();
            list = db.Queryable<AddGroupRequest>().Where(it => it.AccepterID == userID && it.Notified == false).ToList();
            return list;
        }

        public AddGroupRequestPage GetAddGroupRequestPage(string userID, int pageIndex, int pageSize)
        {
            int entityCount = 0;
            List<AddGroupRequest> friendRequestList = db.Queryable<AddGroupRequest>().Where(it => it.AccepterID == userID).OrderBy(it => it.AutoID, OrderByType.Desc).ToPageList(pageIndex+1, pageSize, ref entityCount);
            return new AddGroupRequestPage() { AddGroupRequestList = friendRequestList, TotalEntityCount = entityCount };
        }

        public void InsertGroupBan4User(GroupBan groupBan)
        {
            db.Deleteable<GroupBan>().Where(it => it.GroupID == groupBan.GroupID && it.UserID == groupBan.UserID).ExecuteCommand();
            db.Insertable(groupBan).ExecuteCommand();
        }

        public void DeleteGroupBan4User(string groupID, string userID)
        {
            db.Deleteable<GroupBan>().Where(it => it.GroupID == groupID && it.UserID == userID).ExecuteCommand();
        }

        public GroupBan GetGroupBan4User(string groupID, string userID)
        {
            GroupBan groupBan = null;
            //using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            //{
            //    IOrmAccesser<GroupBan> accesser = scope.NewOrmAccesser<GroupBan>();
            //    groupBan = accesser.GetOne(new Filter(GroupBan._GroupID, groupID), new Filter(GroupBan._UserID, userID),
            //        new Filter(GroupBan._EnableTime, DateTime.Now, ComparisonOperators.Greater));
            //    scope.Commit();
            //}

            groupBan = db.Queryable<GroupBan>().Where(it => it.GroupID == groupID && it.UserID == userID && it.EnableTime > DateTime.Now).First();
            return groupBan;
        }

        public List<GroupBan> GetGroupBans4Group(string groupID)
        {
            //List<GroupBan> groupBans = new List<GroupBan>();
            //using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            //{
            //    IOrmAccesser<GroupBan> accesser = scope.NewOrmAccesser<GroupBan>();
            //    groupBans = accesser.GetMuch(new Filter(GroupBan._GroupID, groupID), new Filter(GroupBan._EnableTime, DateTime.Now, ComparisonOperators.Greater));
            //    scope.Commit();
            //}
            List<GroupBan> groupBans = db.Queryable<GroupBan>().Where(it => it.GroupID == groupID && it.EnableTime > DateTime.Now).ToList();
            return groupBans;
        }

        public bool ExistAllGroupBan(string groupID)
        {
            GroupBan groupBan = null;
            //using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            //{
            //    IOrmAccesser<GroupBan> accesser = scope.NewOrmAccesser<GroupBan>();
            //    //是否开启全员禁言
            //    groupBan = accesser.GetOne(new Filter(GroupBan._GroupID, groupID), new Filter(GroupBan._UserID, ""));
            //    scope.Commit();
            //}
            //是否开启全员禁言
            groupBan = db.Queryable<GroupBan>().Where(it => it.GroupID == groupID && it.UserID == "").First();

            return groupBan != null;
        }

    }


    #region DefaultChatMessageRecordPersister
    /// <summary>
    /// 聊天记录持久化器。
    /// </summary>
    public class DefaultChatRecordPersister2 : IChatRecordPersister
    {
        private SqlSugarClient db;

        public void Initialize(SqlSugarClient dbClient)
        {
            this.db = dbClient;
        }

        /// <summary>
        /// 插入一条聊天记录。
        /// </summary>      
        public int InsertChatMessageRecord(ChatMessageRecord record)
        {
            if (this.db == null)
            {
                return 0;
            }
            int identity = db.Insertable(record).ExecuteReturnIdentity();
            return identity;
        }

        public ChatMessageRecord GetChatMessageRecord(int id)
        {
            ChatMessageRecord chatMessageRecord = null;
            //using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            //{
            //    IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
            //    chatMessageRecord = accesser.GetOne(id);
            //    scope.Commit();
            //}
            chatMessageRecord = db.Queryable<ChatMessageRecord>().Where(it => it.AutoID == id).First();
            return chatMessageRecord;
        }

        /// <summary>
        /// 获取一页群聊天记录。
        /// </summary>
        /// <param name="groupID">群ID</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>     
        /// <returns>聊天记录页</returns>
        public ChatRecordPage GetGroupChatRecordPage(ChatRecordTimeScope chatRecordTimeScope, string groupID, int pageSize, int pageIndex)
        {
            if (this.db == null)
            {
                return new ChatRecordPage(0, 0, new List<ChatMessageRecord>());
            }

            DateTimeScope timeScope = null;
            DateTime now = DateTime.Now;
            if (chatRecordTimeScope == ChatRecordTimeScope.RecentWeek) //一周
            {
                timeScope = new DateTimeScope(now.AddDays(-7), now);
            }
            else if (chatRecordTimeScope == ChatRecordTimeScope.RecentMonth)//一月
            {
                timeScope = new DateTimeScope(now.AddDays(-31), now);
            }
            else if (chatRecordTimeScope == ChatRecordTimeScope.Recent3Month)//三月
            {
                timeScope = new DateTimeScope(now.AddDays(-91), now);
            }
            else //全部
            {
            }

            //List<Filter> filterList = new List<Filter>();
            //filterList.Add(new Filter(ChatMessageRecord._AudienceID, groupID));
            //filterList.Add(new Filter(ChatMessageRecord._IsGroupChat, true));
            var sql = db.Queryable<ChatMessageRecord>().Where(it => it.AudienceID == groupID && it.IsGroupChat == true);

            if (timeScope != null)
            {
                //filterList.Add(new Filter(ChatMessageRecord._OccureTime, new DateTime[] { timeScope.StartDate, timeScope.EndDate }, ComparisonOperators.BetweenAnd));
                sql = sql.Where(it => it.OccureTime > timeScope.StartDate && it.OccureTime < timeScope.EndDate);
            }
            //SimpleFilterTree tree = new SimpleFilterTree(filterList);

            //最后一页
            if (pageIndex == int.MaxValue)
            {
                int total = 0;
                //using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
                //{
                //    IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
                //    total = (int)accesser.GetCount(tree);
                //    scope.Commit();
                //}
                total = sql.Count();
                int pageCount = total / pageSize;
                if (total % pageSize > 0)
                {
                    pageCount += 1;
                }
                pageIndex = pageCount - 1;
            }
            if (pageIndex == -1) //表示没有记录
            {
                return new ChatRecordPage(0, 0, new List<ChatMessageRecord>());
            }
            int totalCount = 0;
            //ChatMessageRecord[] page = null;
            //using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            //{
            //    IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
            //    page = accesser.GetPage(tree, pageSize, pageIndex, ChatMessageRecord._AutoID, true, out totalCount);
            //    scope.Commit();
            //}

            List<ChatMessageRecord> recordList = sql.OrderBy(it => it.AutoID, OrderByType.Desc).ToPageList(pageIndex+1, pageSize, ref totalCount);
            if (recordList != null && recordList.Count > 0)
            {
                recordList.Sort((x, y) => { return x.AutoID.CompareTo(y.AutoID); });
            }
            return new ChatRecordPage(totalCount, pageIndex, recordList);
        }

        /// <summary>
        /// 获取一页与好友的聊天记录。
        /// </summary>
        /// <param name="myID">自己的UserID</param>
        /// <param name="friendID">好友的ID</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>      
        /// <returns>聊天记录页</returns>
        public ChatRecordPage GetChatRecordPage(ChatRecordTimeScope chatRecordTimeScope, string myID, string friendID, int pageSize, int pageIndex)
        {
            if (this.db == null)
            {
                return new ChatRecordPage(0, 0, new List<ChatMessageRecord>());
            }

            DateTimeScope timeScope = null;
            DateTime now = DateTime.Now;
            if (chatRecordTimeScope == ChatRecordTimeScope.RecentWeek) //一周
            {
                timeScope = new DateTimeScope(now.AddDays(-7), now);
            }
            else if (chatRecordTimeScope == ChatRecordTimeScope.RecentMonth)//一月
            {
                timeScope = new DateTimeScope(now.AddDays(-31), now);
            }
            else if (chatRecordTimeScope == ChatRecordTimeScope.Recent3Month)//三月
            {
                timeScope = new DateTimeScope(now.AddDays(-91), now);
            }
            else //全部
            {
            }

            //IFilterTree tree1 = new SimpleFilterTree(new Filter(ChatMessageRecord._SpeakerID, myID), new Filter(ChatMessageRecord._AudienceID, friendID));
            //IFilterTree tree2 = new SimpleFilterTree(new Filter(ChatMessageRecord._SpeakerID, friendID), new Filter(ChatMessageRecord._AudienceID, myID));
            //IFilterTree tmp = new ComplexFilterTree(LogicType.Or, tree1, tree2);
            //ComplexFilterTree tree = new ComplexFilterTree(LogicType.And, tmp, new Filter(ChatMessageRecord._IsGroupChat, false));

            var sql = db.Queryable<ChatMessageRecord>().Where(it => ((it.SpeakerID == myID && it.AudienceID == friendID) || (it.SpeakerID == friendID && it.AudienceID == myID)) && it.IsGroupChat == false);
            if (timeScope != null)
            {
                //tree = new ComplexFilterTree(LogicType.And, tree, new Filter(ChatMessageRecord._OccureTime, new DateTime[] { timeScope.StartDate, timeScope.EndDate }, ComparisonOperators.BetweenAnd));
                sql = sql.Where(it => it.OccureTime > timeScope.StartDate && it.OccureTime < timeScope.EndDate);
            }
            //最后一页
            if (pageIndex == int.MaxValue)
            {
                int total = 0;
                //using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
                //{
                //    IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
                //    total = (int)accesser.GetCount(tree);
                //    scope.Commit();
                //}
                total = sql.Count();
                int pageCount = total / pageSize;
                if (total % pageSize > 0)
                {
                    pageCount += 1;
                }
                pageIndex = pageCount - 1;
            }

            if (pageIndex == -1) //表示没有记录
            {
                return new ChatRecordPage(0, 0, new List<ChatMessageRecord>());
            }

            int totalCount = 0;
            //ChatMessageRecord[] page = null;
            //using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            //{
            //    IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
            //    page = accesser.GetPage(tree, pageSize, pageIndex, ChatMessageRecord._AutoID, true, out totalCount);
            //    scope.Commit();
            //}

            List<ChatMessageRecord> recordList = sql.OrderBy(it => it.AutoID, OrderByType.Desc).ToPageList(pageIndex +1, pageSize, ref totalCount).ToList();
            if (recordList != null && recordList.Count > 0)
            {
                recordList.Sort((x, y) => { return x.AutoID.CompareTo(y.AutoID); });
            }
            return new ChatRecordPage(totalCount, pageIndex, recordList);
        }


        public ChatMessageRecord GetLastFriendRecord(string myID, string friendID)
        {
            if (this.db == null)
            {
                return null;
            }

            ChatMessageRecord record = null;
            //using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            //{
            //    IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
            //    SimpleFilterTree tree1 = new SimpleFilterTree(new Filter(ChatMessageRecord._SpeakerID, myID), new Filter(ChatMessageRecord._AudienceID, friendID));
            //    SimpleFilterTree tree2 = new SimpleFilterTree(new Filter(ChatMessageRecord._SpeakerID, friendID), new Filter(ChatMessageRecord._AudienceID, myID));
            //    ComplexFilterTree tmp = new ComplexFilterTree(LogicType.Or, tree1, tree2);
            //    ComplexFilterTree tree = new ComplexFilterTree(LogicType.And, tmp, new Filter(ChatMessageRecord._IsGroupChat, false));
            //    object id = accesser.GetAggregateValue<object>(AggregateQueryType.Max, ChatMessageRecord._AutoID, tree);
            //    if (id != null)
            //    {
            //        record = accesser.GetOne(id);
            //    }
            //    scope.Commit();
            //}

            record = db.Queryable<ChatMessageRecord>().Where(it => ((it.SpeakerID == myID && it.AudienceID == friendID) || (it.SpeakerID == friendID && it.AudienceID == myID)) && it.IsGroupChat == false).OrderBy(it => it.AutoID, OrderByType.Desc).First();
            return record;
        }

        public ChatMessageRecord GetLastGroupRecord(string groupID)
        {
            if (this.db == null)
            {
                return null;
            }

            ChatMessageRecord record = null;
            //using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            //{
            //    IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
            //    object id = accesser.GetAggregateValue<object>(AggregateQueryType.Max, ChatMessageRecord._AutoID, new Filter(ChatMessageRecord._AudienceID, groupID), new Filter(ChatMessageRecord._IsGroupChat, true));
            //    if (id != null)
            //    {
            //        record = accesser.GetOne(id);
            //    }
            //    scope.Commit();
            //}

            record = db.Queryable<ChatMessageRecord>().Where(it => it.AudienceID == groupID && it.IsGroupChat == true).OrderBy(it => it.AutoID, OrderByType.Asc).First();

            return record;
        }


        public void DeleteChatRecord(string myID, string friendID)
        {
            if (this.db == null)
            {
                return;
            }

            //using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            //{
            //    IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
            //    SimpleFilterTree tree1 = new SimpleFilterTree(new Filter(ChatMessageRecord._SpeakerID, myID), new Filter(ChatMessageRecord._AudienceID, friendID));
            //    SimpleFilterTree tree2 = new SimpleFilterTree(new Filter(ChatMessageRecord._SpeakerID, friendID), new Filter(ChatMessageRecord._AudienceID, myID));
            //    ComplexFilterTree tmp = new ComplexFilterTree(LogicType.Or, tree1, tree2);
            //    ComplexFilterTree tree = new ComplexFilterTree(LogicType.And, tmp, new Filter(ChatMessageRecord._IsGroupChat, false));
            //    accesser.Delete(tree);
            //    scope.Commit();
            //}

            db.Deleteable<ChatMessageRecord>().Where(it => ((it.SpeakerID == myID && it.AudienceID == friendID) || (it.SpeakerID == friendID && it.AudienceID == myID)) && it.IsGroupChat == false).ExecuteCommand();
        }

        public void DeleteGroupChatRecord(string groupID)
        {
            if (this.db == null)
            {
                return;
            }

            //using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            //{
            //    IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
            //    SimpleFilterTree tree = new SimpleFilterTree(new Filter(ChatMessageRecord._AudienceID, groupID), new Filter(ChatMessageRecord._IsGroupChat, true));
            //    accesser.Delete(tree);
            //    scope.Commit();
            //}

            db.Deleteable<ChatMessageRecord>().Where(it => it.AudienceID == groupID && it.IsGroupChat == true).ExecuteCommand();
        }


        public void ClearAllChatRecord()
        {
            if (this.db == null)
            {
                return;
            }

            db.Deleteable<ChatMessageRecord>().ExecuteCommand();
        }


    }
    #endregion

}
