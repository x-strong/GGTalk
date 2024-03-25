using System;
using System.Collections.Generic;
using System.Text;
using ESPlus.Application.CustomizeInfo.Server;
using ESPlus.Application.Basic.Server;
using ESPlus.Application.CustomizeInfo;
using ESFramework.Server.UserManagement;
using ESPlus.Rapid;
using ESPlus.Serialization;
using ESFramework;

using System.IO;
using ESBasic;
using OMCS.Server;
using OMCS.Passive.ShortMessages;
using System.Drawing;
using TalkBase.Core;
using TalkBase.Server.Core;
using ESFramework.Boost.Controls;
using ESPlus.FileTransceiver;

namespace TalkBase.Server
{    
    /// <summary>
    /// 服务端处理所有属于TalkBase框架定义的消息。
    /// （1）离线文件。
    /// （2）离线消息。
    /// （3）将聊天记录持久化存储。
    /// </summary>    
    internal class ServerHandler<TUser, TGroup> : IIntegratedCustomizeHandler, ITalkBaseHandler
        where TUser : TalkBase.IUser ,new()
        where TGroup : TalkBase.IGroup
    {
        private List<ClientType> clientTypeList = new List<ClientType>() { ClientType.DotNET, ClientType.Android, ClientType.IOS };
        private ServerGlobalCache<TUser, TGroup> serverGlobalCache;
        private IRapidServerEngine rapidServerEngine;
        private TalkBaseInfoTypes talkBaseInfoTypes;
        private ITalkBaseHelper<TGroup> talkBaseHelper;
        private CoreHandler<TUser, TGroup> coreHandler ;

        /// <summary>
        /// 当转发系统消息（接受者为某个用户或群）时，触发此事件。
        /// </summary>
        public event CbGeneric<SystemMessageContract> SystemMessageTransfering;

        #region Ctor
        internal ServerHandler(ServerGlobalCache<TUser, TGroup> serverGlobalCache, IRapidServerEngine engine, CoreHandler<TUser, TGroup> sender, TalkBaseInfoTypes infoTypes, ITalkBaseHelper<TGroup> helper)
        {
            this.serverGlobalCache = serverGlobalCache;
            this.rapidServerEngine = engine;
            this.coreHandler = sender;
            this.talkBaseInfoTypes = infoTypes;
            this.talkBaseHelper = helper;
            
        } 
        #endregion       

        public void Initialize()
        {
            this.rapidServerEngine.UserManager.ClientDeviceDisconnected += UserManager_ClientDeviceDisconnected;
            this.rapidServerEngine.UserManager.UserDisconnected += new ESBasic.CbGeneric<string>(UserManager_SomeOneDisconnected);
            this.rapidServerEngine.ContactsController.BroadcastReceived += new ESBasic.CbGeneric<string, string, int, byte[],string>(ContactsOutter_BroadcastReceived);
            this.rapidServerEngine.MessageReceived += new ESBasic.CbGeneric<string,ClientType, int, byte[], string>(rapidServerEngine_MessageReceived);

            this.rapidServerEngine.FileController.FileRequestReceived += new ESPlus.Application.FileTransfering.CbFileRequestReceived(FileController_FileRequestReceived);
            this.rapidServerEngine.FileController.FileReceivingEvents.FileTransCompleted += new ESBasic.CbGeneric<ESPlus.FileTransceiver.ITransferingProject>(FileReceivingEvents_FileTransCompleted);
            this.rapidServerEngine.FileController.FileSendingEvents.FileTransCompleted += new ESBasic.CbGeneric<ESPlus.FileTransceiver.ITransferingProject>(FileSendingEvents_FileTransCompleted);
            this.rapidServerEngine.FileController.FileResponseReceived += new ESBasic.CbGeneric<ESPlus.FileTransceiver.ITransferingProject, bool>(FileController_FileResponseReceived);
        }

        private IMessagePusher messagePusher;
        public IMessagePusher MessagePusher { private get {return this.messagePusher; } set { this.messagePusher = value; } }



        #region 聊天记录、离线消息、更新个人资料
        void rapidServerEngine_MessageReceived(string sourceUserID, ClientType sourceType, int informationType, byte[] info, string tag)
        {
            if (TalkBase.Core.Global.IsExpired())
            {
                return;
            }
            if (informationType == this.talkBaseInfoTypes.Vibration)
            {
                //服务端转发抖动到对方的PC端
                this.rapidServerEngine.SendMessage(tag, informationType, null, sourceUserID, ClientType.DotNET);
                return;
            }

            if (informationType == this.talkBaseInfoTypes.Chat)
            {
                string destID = tag;
                if (this.rapidServerEngine.UserManager.IsUserOnLine(destID))
                {
                    this.rapidServerEngine.SendMessage(sourceType, destID, informationType, info, sourceUserID);
                }
                else
                {                   
                    OfflineMessage msg = new OfflineMessage(sourceUserID, sourceType, destID, null, informationType, info);
                    this.serverGlobalCache.StoreOfflineMessage(msg);
                    this.PushMessage(destID, sourceUserID,UnitType.User, info);
                }
                this.serverGlobalCache.StoreChatRecord(sourceUserID, destID, info);
                return;
            }
            if (informationType == this.talkBaseInfoTypes.MediaCommunicate)
            {
                string destID = tag;
                if (this.rapidServerEngine.UserManager.IsUserOnLine(destID))
                {
                    this.rapidServerEngine.SendMessage(sourceType, destID, informationType, info, sourceUserID);
                }
                else
                {
                    this.PushMediaMessage(destID, sourceUserID, sourceType, info);
                }

                return;
            }
            if (informationType == this.talkBaseInfoTypes.Snapchat)
            {
                string destID = tag;
                if (this.rapidServerEngine.UserManager.IsUserOnLine(destID))
                {
                    this.rapidServerEngine.SendMessage(sourceType, destID, informationType, info, sourceUserID);
                }
                return;
            }

            if (informationType == this.talkBaseInfoTypes.ChatTransfered)
            {
                string[] accepters = tag.Split(',');
                foreach (string destID in accepters)
                {
                    UnitType type = this.talkBaseHelper.Recognize(destID);
                    if (type == UnitType.User)
                    {
                        this.rapidServerEngine_MessageReceived(sourceUserID, sourceType, this.talkBaseInfoTypes.Chat, info, destID);
                    }
                    else//Group
                    {
                        this.rapidServerEngine.ContactsController.Broadcast(destID, this.talkBaseInfoTypes.GroupChat, info, sourceUserID, ActionTypeOnChannelIsBusy.Continue);
                        this.ContactsOutter_BroadcastReceived(sourceUserID, destID, this.talkBaseInfoTypes.GroupChat, info, null);
                    }
                }
                return;
            }

            if (informationType == this.talkBaseInfoTypes.ChangeMyHeadImage)
            {
                ChangeHeadImageContract contract = CompactPropertySerializer.Default.Deserialize<ChangeHeadImageContract>(info, 0);
                //if (contract.HeadImageIndex < 0)
                //{
                //    Image img = ESBasic.Helpers.ImageHelper.Convert(contract.HeadImage);
                //    Image imgEcllips = ESBasic.Helpers.ImageHelper.CutEllipseImage(img, FunctionOptions.GetHeadImageSize());
                //    contract.HeadImage = ESBasic.Helpers.ImageHelper.Convert(imgEcllips);
                //}
                this.serverGlobalCache.UpdateUserHeadImage(sourceUserID, contract.HeadImageIndex, contract.HeadImage);
                TUser user = this.serverGlobalCache.GetUser(sourceUserID);
                contract.UserLatestVersion = user.Version;
                byte[] notify = CompactPropertySerializer.Default.Serialize(contract);
                List<string> contacts = this.serverGlobalCache.GetAllContactsNecessary(sourceUserID);
                foreach (string friendID in contacts)
                {
                    if (friendID != sourceUserID)
                    {
                        //可能要分块发送
                        this.rapidServerEngine.SendMessage(friendID, this.talkBaseInfoTypes.UserHeadImageChanged, notify, null, 2048);
                    }
                }
            }

            if (informationType == this.talkBaseInfoTypes.ChangeMyBusinessInfo)
            {
                ChangeUserBusinessInfoContract contract = CompactPropertySerializer.Default.Deserialize<ChangeUserBusinessInfoContract>(info, 0);
                int ver = this.serverGlobalCache.UpdateUserBusinessInfo(contract.UserID, contract.BusinessInfo, contract.IncreaseVersion);
                if (contract.NotifyContacts)
                {
                    UserBusinessInfoChangedNotifyContract notify = new UserBusinessInfoChangedNotifyContract(contract.UserID, contract.BusinessInfo, ver);
                    byte[] data = CompactPropertySerializer.Default.Serialize(notify);
                    List<string> contacts = this.serverGlobalCache.GetAllContactsNecessary(sourceUserID);
                    foreach (string friendID in contacts)
                    {
                        if (friendID != sourceUserID)
                        {
                            this.rapidServerEngine.SendMessage(friendID, this.talkBaseInfoTypes.UserBusinessInfoChanged, data, null, 2048);
                        }
                    }
                }
                return;
            }
            if (informationType == this.talkBaseInfoTypes.ChangeMyBaseInfo)
            {
                ChangeUserBaseInfoContract contract = CompactPropertySerializer.Default.Deserialize<ChangeUserBaseInfoContract>(info, 0);
                this.serverGlobalCache.UpdateUserInfo(sourceUserID, contract.Name, contract.Signature, contract.OrgID);
                TUser user = this.serverGlobalCache.GetUser(sourceUserID);
                contract.UserLatestVersion = user.Version;
                byte[] notify = CompactPropertySerializer.Default.Serialize(contract);

                List<string> friendIDs = this.serverGlobalCache.GetAllContactsNecessary(sourceUserID);
                foreach (string friendID in friendIDs)
                {
                    if (friendID != sourceUserID)
                    {
                        this.rapidServerEngine.CustomizeController.Send(friendID, this.talkBaseInfoTypes.UserBaseInfoChanged, notify, true, ActionTypeOnChannelIsBusy.Continue);
                    }
                }
                return;
            }
            if (informationType == this.talkBaseInfoTypes.MoveFriendToOtherCatalog)
            {
                this.HandleInformation(sourceUserID, sourceType, informationType, info);
                return;
            }
            if (informationType == this.talkBaseInfoTypes.AddFriendCatalog)
            {
                this.HandleInformation(sourceUserID, sourceType, informationType, info);
                return;
            }

            if (informationType == this.talkBaseInfoTypes.SystemMessage)
            {
                SystemMessageContract contract = CompactPropertySerializer.Default.Deserialize<SystemMessageContract>(info, 0);
                if (this.SystemMessageTransfering != null)
                {
                    this.SystemMessageTransfering(contract);
                }
                this.coreHandler.DoSendSystemMessage(sourceUserID, sourceType, contract, info);
                return;
            }

            if (informationType == this.talkBaseInfoTypes.ChangeUnitCommentName)
            {
                ChangeCommentNameContract contract = CompactPropertySerializer.Default.Deserialize<ChangeCommentNameContract>(info, 0);
                this.serverGlobalCache.ChangeUnitCommentName(sourceUserID, contract.UnitID, contract.CommentName);
                return;
            }
            if (informationType == this.talkBaseInfoTypes.RequestAddFriend)
            {
                RequestAddFriendContract contract = CompactPropertySerializer.Default.Deserialize<RequestAddFriendContract>(info, 0);
                TUser owner = this.serverGlobalCache.GetUser(contract.RequesterID);
                //若双方已是好友，过滤请求
                if (owner.GetAllFriendList().Contains(contract.AccepterID))
                {
                    return;
                }
                if (this.rapidServerEngine.UserManager.IsUserOnLine(contract.AccepterID))
                {
                    this.rapidServerEngine.SendMessage(contract.AccepterID, informationType, info, contract.RequesterID, 2048, null);
                    this.serverGlobalCache.InsertAddFriendRequest(contract.RequesterID, contract.AccepterID, contract.RequesterCatalogName, contract.Comment, true);
                }
                else
                {
                    this.serverGlobalCache.InsertAddFriendRequest(contract.RequesterID, contract.AccepterID, contract.RequesterCatalogName, contract.Comment);
                }

                return;
            }
            if (informationType == this.talkBaseInfoTypes.HandleAddFriendRequest)
            {
                HandleAddFriendRequestContract contract = CompactPropertySerializer.Default.Deserialize<HandleAddFriendRequestContract>(info, 0);
                bool isExist = this.serverGlobalCache.IsUserExist(contract.RequesterID);
                if (!isExist)
                {
                    return;
                }
                string requesterCatalogName = this.serverGlobalCache.GetRequesterCatalogName(contract.RequesterID, contract.AccepterID);
                //更新DB中的处理结果
                this.serverGlobalCache.UpdateAddFriendRequest(contract.RequesterID, contract.AccepterID, requesterCatalogName, contract.AccepterCatalogName, contract.IsAgreed);
                if (this.rapidServerEngine.UserManager.IsUserOnLine(contract.RequesterID))
                {
                    contract.RequesterCatalogName = requesterCatalogName;
                    info = CompactPropertySerializer.Default.Serialize(contract);
                    this.rapidServerEngine.SendMessage(contract.RequesterID, informationType, info, contract.AccepterID, 2048, null);
                    this.serverGlobalCache.SetAddFriendRequestNotified(contract.RequesterID, contract.AccepterID);
                }
                if (contract.IsAgreed)
                {
                    bool isFriend = this.serverGlobalCache.GetUser(contract.RequesterID).IsFriend(contract.AccepterID) || this.serverGlobalCache.GetUser(contract.AccepterID).IsFriend(contract.RequesterID);
                    if (isFriend) { return; }//若双方已是好友直接返回
                    this.serverGlobalCache.AddFriend(contract.RequesterID, contract.AccepterID, requesterCatalogName, contract.AccepterCatalogName);

                    TUser owner = this.serverGlobalCache.GetUser(sourceUserID);
                    owner = (TUser)owner.PartialCopy;
                    byte[] ownerBuff = CompactPropertySerializer.Default.Serialize<TUser>(owner);
                    //通知对方
                    this.rapidServerEngine.SendMessage(contract.RequesterID, this.talkBaseInfoTypes.FriendAddedNotify, ownerBuff, contract.RequesterCatalogName, null);
                }
                return;
            }
            if (informationType == this.talkBaseInfoTypes.GetAddFriendPage)
            {
                GetAddFriendPageContract contract = CompactPropertySerializer.Default.Deserialize<GetAddFriendPageContract>(info, 0);
                AddFriendRequestPage page = this.serverGlobalCache.GetAddFriendRequestPage(contract.UserID, contract.PageIndex, contract.PageSize);
                byte[] bMsg = CompactPropertySerializer.Default.Serialize<AddFriendRequestPage>(page);
                this.rapidServerEngine.SendMessage(sourceUserID, informationType, bMsg, null, sourceType);
                return;
            }
            if (informationType == this.talkBaseInfoTypes.RequestAddGroup)
            {
                RequestAddGroupContract contract = CompactPropertySerializer.Default.Deserialize<RequestAddGroupContract>(info, 0);
                TGroup group = this.serverGlobalCache.GetGroup(contract.GroupID);
                //若已在该讨论组中，过滤请求
                if (group.MemberList.Contains(contract.RequesterID))
                {
                    return;
                }
                if (this.rapidServerEngine.UserManager.IsUserOnLine(group.CreatorID))
                {
                    this.rapidServerEngine.SendMessage(group.CreatorID, informationType, info, contract.RequesterID, 2048, null);
                    this.serverGlobalCache.InsertAddGroupRequest(contract.RequesterID, contract.GroupID, group.CreatorID, contract.Comment, true);
                }
                else
                {
                    this.serverGlobalCache.InsertAddGroupRequest(contract.RequesterID, contract.GroupID, group.CreatorID, contract.Comment, false);
                }
                return;
            }
            if (informationType == this.talkBaseInfoTypes.HandleAddGroupRequest)
            {
                HandleAddGroupRequestContract contract = CompactPropertySerializer.Default.Deserialize<HandleAddGroupRequestContract>(info, 0);
                this.serverGlobalCache.UpdateAddGroupRequest(contract.RequesterID, contract.GroupID, contract.IsAgreed);

                TGroup group = this.serverGlobalCache.GetGroup(contract.GroupID);
                if (group == null)
                {
                    return;
                }
                if (contract.IsAgreed)
                {
                    this.serverGlobalCache.JoinGroup(contract.RequesterID, contract.GroupID);
                    //通知其它组成员  申请者已加入群组
                    List<string> guestList = new List<string>();
                    guestList.Add(contract.RequesterID);
                    ManageGroupMembersNotifyContract notify = new ManageGroupMembersNotifyContract(sourceUserID, new List<string>() { contract.RequesterID }, true);
                    byte[] notifyData = CompactPropertySerializer.Default.Serialize(notify);
                    this.rapidServerEngine.ContactsController.Broadcast(contract.GroupID, this.talkBaseInfoTypes.PulledIntoGroupNotify, notifyData, null, ESFramework.ActionTypeOnChannelIsBusy.Continue);
                }
                if (this.rapidServerEngine.UserManager.IsUserOnLine(group.CreatorID))
                {
                    //通知不同客户端类型的群主 修改 申请状态
                    this.rapidServerEngine.SendMessage(group.CreatorID, informationType, info, contract.RequesterID, 2048, null);
                    this.serverGlobalCache.SetAddGroupRequestNotified(contract.RequesterID, contract.GroupID);
                }
                return;
            }
            if (informationType == this.talkBaseInfoTypes.GetAddGroupPage)
            {
                GetAddGroupPageContract contract = CompactPropertySerializer.Default.Deserialize<GetAddGroupPageContract>(info, 0);
                AddGroupRequestPage page = this.serverGlobalCache.GetAddGroupRequestPage(contract.UserID, contract.PageIndex, contract.PageSize);
                byte[] bMsg = CompactPropertySerializer.Default.Serialize<AddGroupRequestPage>(page);
                this.rapidServerEngine.SendMessage(sourceUserID, informationType, bMsg, null, sourceType);
                return;
            }
            if (informationType == this.talkBaseInfoTypes.GroupBan4User)
            {
                GroupBan4UserContract contract = CompactPropertySerializer.Default.Deserialize<GroupBan4UserContract>(info, 0);
                GroupBan groupBan = new GroupBan()
                {
                    GroupID = contract.GroupID,
                    OperatorID = contract.OperatorID,
                    UserID = contract.UserID,
                    Comment2 = contract.Comment,
                    CreateTime = DateTime.Now,
                    EnableTime = DateTime.Now.AddMinutes(contract.Minutes)
                };
                this.serverGlobalCache.InsertGroupBan4User(groupBan);
                this.rapidServerEngine.SendMessage(contract.UserID, informationType, info, null);
                return;
            }
            if (informationType == this.talkBaseInfoTypes.RemoveGroupBan4User)
            {
                RemoveGroupBan4UserContract contract = CompactPropertySerializer.Default.Deserialize<RemoveGroupBan4UserContract>(info, 0);
                this.serverGlobalCache.DeleteGroupBan4User(contract.GroupID, contract.UserID);
                this.rapidServerEngine.SendMessage(contract.UserID, informationType, info, tag);
                return;
            }
            if (informationType == this.talkBaseInfoTypes.CheckGroupBan4CurrentUser)
            {
                GroupBan groupBan = this.serverGlobalCache.GetGroupBan4User(tag, sourceUserID);
                if (groupBan == null)
                {
                    return;
                }
                double minutes = (groupBan.EnableTime - DateTime.Now).TotalMinutes;
                if (minutes <= 0)
                {
                    return;
                }
                GroupBan4UserContract contract = new GroupBan4UserContract()
                {
                    OperatorID = groupBan.OperatorID,
                    GroupID = groupBan.GroupID,
                    UserID = groupBan.UserID,
                    Comment = groupBan.Comment2,
                    Minutes = minutes
                };
                byte[] bMsg = CompactPropertySerializer.Default.Serialize<GroupBan4UserContract>(contract);
                this.rapidServerEngine.SendMessage(contract.UserID, this.talkBaseInfoTypes.GroupBan4User, bMsg, null);
                return;
            }
            if (informationType == this.talkBaseInfoTypes.GroupBan4Group)
            {
                string groupID = tag;
                GroupBan groupBan = new GroupBan()
                {
                    GroupID = groupID,
                    OperatorID = sourceUserID,
                    UserID = string.Empty,
                    Comment2 = "全员禁言",
                    CreateTime = DateTime.Now,
                    EnableTime = DateTime.MaxValue
                };
                this.serverGlobalCache.InsertGroupBan4User(groupBan);
                this.rapidServerEngine.ContactsController.Broadcast(groupID, informationType, null, sourceUserID, ActionTypeOnChannelIsBusy.Continue);
                return;
            }
            if (informationType == this.talkBaseInfoTypes.RemoveGroupBan4Group)
            {
                string groupID = tag;
                this.serverGlobalCache.DeleteGroupBan4User(tag, "");
                this.rapidServerEngine.ContactsController.Broadcast(groupID, informationType, null, sourceUserID, ActionTypeOnChannelIsBusy.Continue);
                return;
            }
            if (informationType == this.talkBaseInfoTypes.ChangeUserState)
            {
                ChangeUserStateContract contract = CompactPropertySerializer.Default.Deserialize<ChangeUserStateContract>(info, 0);
                if (contract == null || string.IsNullOrEmpty(contract.UserID))
                {
                    return;
                }
                TUser user = this.serverGlobalCache.GetUser(contract.UserID);
                if (user.UserState == contract.UserState)
                {
                    return;
                }
                this.serverGlobalCache.ChangeUserState(contract.UserID, contract.UserState);
                //若被改变状态的用户在线，则通知
                if (this.rapidServerEngine.UserManager.IsUserOnLine(contract.UserID))
                {
                    this.rapidServerEngine.SendMessage(contract.UserID, this.talkBaseInfoTypes.UserStateChanged, null, ((int)contract.UserState).ToString());
                }
                return;
            }

            #region Group
            if (informationType == this.talkBaseInfoTypes.QuitGroup)
            {
                this.HandleInformation(sourceUserID, sourceType, informationType, info);
                return;
            }

            if (informationType == this.talkBaseInfoTypes.DeleteGroup)
            {
                this.HandleInformation(sourceUserID, sourceType, informationType, info);
                return;
            }
            #endregion
            if (informationType == this.talkBaseInfoTypes.RemoveFriend)
            {
                this.HandleInformation(sourceUserID, sourceType, informationType, info);
                return;
            }
        }        

        void ContactsOutter_BroadcastReceived(string broadcasterID, string groupID, int broadcastType, byte[] broadcastContent ,string tag )
        {
            if (broadcastType == this.talkBaseInfoTypes.GroupChat)
            {
                this.serverGlobalCache.StoreGroupChatRecord(groupID, broadcasterID, broadcastContent);

                //离线群聊天消息
                if (FunctionOptions.OfflineGroupOrgChatMessage)
                {
                    TGroup group = this.serverGlobalCache.GetGroup(groupID);
                    if (group != null)
                    {
                        foreach (string destUserID in group.MemberList)
                        {
                            if (!this.rapidServerEngine.UserManager.IsUserOnLine(destUserID))
                            {
                                OfflineMessage msg = new OfflineMessage(broadcasterID, ClientType.ServerSide, destUserID, groupID, broadcastType, broadcastContent,tag);
                                this.serverGlobalCache.StoreOfflineMessage(msg);
                                this.PushMessage(destUserID, groupID, UnitType.Group, broadcastContent);
                            }
                        }
                    }
                }

                return;
            }            
        }

        void UserManager_SomeOneDisconnected(string userID)
        {
            TUser user = this.serverGlobalCache.GetUser(userID);
            if (user != null)
            {
                user.UserStatus = UserStatus.OffLine;
            }
        }
        private void UserManager_ClientDeviceDisconnected(LoginDeviceData userData, ESFramework.Server.DisconnectedType disconnectedType)
        {
            this.serverGlobalCache.UpdateUserOfflineTime(userData.UserID, userData.ClientType);
        }

        //发送离线消息给客户端
        public void SendOfflineMessage(string destUserID)
        {
            List<OfflineMessage> list = this.serverGlobalCache.PickupOfflineMessage(destUserID);
            if (list != null && list.Count > 0)
            {
                foreach (OfflineMessage msg in list)
                {
                    byte[] bMsg = CompactPropertySerializer.Default.Serialize<OfflineMessage>(msg);
                    this.rapidServerEngine.CustomizeController.SendBlob(msg.DestUserID, this.talkBaseInfoTypes.OfflineMessage, bMsg, 2048);
                }
            }

            //加好友申请
            List<AddFriendRequest> addFriendRequests = this.serverGlobalCache.GetAddFriendRequest4NotNotified(destUserID);
            foreach (AddFriendRequest item in addFriendRequests)
            {
                RequsetType addFriendType = (RequsetType)item.State;
                if (addFriendType == RequsetType.Request)
                {
                    RequestAddFriendContract requestContract = new RequestAddFriendContract(item.RequesterID, item.AccepterID, item.Comment2, item.RequesterCatalogName);
                    byte[] info= CompactPropertySerializer.Default.Serialize<RequestAddFriendContract>(requestContract);
                    this.rapidServerEngine.SendMessage(item.AccepterID, this.talkBaseInfoTypes.RequestAddFriend, info, item.RequesterID, 2048, null); 
                }
                else
                {
                    bool isAgreed = addFriendType == RequsetType.Agree;
                    HandleAddFriendRequestContract handleContract = new HandleAddFriendRequestContract(item.RequesterID,item.AccepterID, item.AccepterCatalogName, isAgreed);
                    handleContract.RequesterCatalogName = item.RequesterCatalogName;
                    byte[] info = CompactPropertySerializer.Default.Serialize<HandleAddFriendRequestContract>(handleContract);
                    this.rapidServerEngine.SendMessage(item.RequesterID, this.talkBaseInfoTypes.HandleAddFriendRequest, info, item.AccepterID, 2048, null);
                }
                this.serverGlobalCache.SetAddFriendRequestNotified(item.RequesterID, item.AccepterID);
            }

            //加讨论组申请
            List<AddGroupRequest> addGroupRequests = this.serverGlobalCache.GetAddGroupRequest4NotNotified(destUserID);
            foreach (AddGroupRequest item in addGroupRequests)
            {
                RequsetType requsetType = (RequsetType)item.State;
                if (requsetType == RequsetType.Request)
                {
                    RequestAddGroupContract contract = new RequestAddGroupContract(item.RequesterID, item.GroupID, item.Comment2);
                    TGroup group = this.serverGlobalCache.GetGroup(contract.GroupID);
                    //若已在该讨论组中，过滤请求
                    if (group.MemberList.Contains(contract.RequesterID))
                    {
                        break;
                    }
                    byte[] info = CompactPropertySerializer.Default.Serialize<RequestAddGroupContract>(contract);
                    this.rapidServerEngine.SendMessage(group.CreatorID, this.talkBaseInfoTypes.RequestAddGroup, info, contract.RequesterID, 2048, null);
                }
                else
                {
                    bool isAgreed = requsetType == RequsetType.Agree;
                    HandleAddGroupRequestContract contract = new HandleAddGroupRequestContract(item.RequesterID, item.GroupID, isAgreed);
                    byte[] info = CompactPropertySerializer.Default.Serialize<HandleAddGroupRequestContract>(contract);
                    this.rapidServerEngine.SendMessage(item.RequesterID, this.talkBaseInfoTypes.HandleAddGroupRequest, info, item.AccepterID, null);
                }
                this.serverGlobalCache.SetAddGroupRequestNotified(item.RequesterID, item.GroupID);
            }
        }

        public void SendGroupOfflineMessage(string destUserID, ClientType clientType)
        {
            TUser user = this.serverGlobalCache.GetUser(destUserID);
            List<GroupOfflineMessage> offlineMessages = null;
            if (clientType == ClientType.DotNET || clientType==ClientType.Linux)
            {
                offlineMessages = this.serverGlobalCache.PickupGroupOfflineMessage(destUserID, user.PcOfflineTime);
            }
            else if (clientType == ClientType.Android || clientType == ClientType.IOS)
            {
                offlineMessages = this.serverGlobalCache.PickupGroupOfflineMessage(destUserID, user.MobileOfflineTime);
            }
            if (offlineMessages != null && offlineMessages.Count > 0)
            {
                foreach (GroupOfflineMessage msg in offlineMessages)
                {
                    //byte[] bMsg = CompactPropertySerializer.Default.Serialize<GroupOfflineMessage>(msg);
                    //this.rapidServerEngine.CustomizeController.SendBlob(destUserID, this.talkBaseInfoTypes.GroupOfflineMessage, bMsg, 2048, clientType);
                    this.DistributeGroupOfflineMessage(destUserID, msg, clientType);
                }
            }
        }

        //分发群离线消息 (每次按20条发送)
        private void DistributeGroupOfflineMessage(string destUserID, GroupOfflineMessage groupOfflineMessage,ClientType clientType)
        {
            int step = 20;
            for (int i = 0; i * step < groupOfflineMessage.OfflineMessages.Count; i++)
            {
                int residueCount = groupOfflineMessage.OfflineMessages.Count - i * step;
                int lastCount = residueCount - step > 0 ? step : residueCount;
                List<OfflineMessage> tempMessages = groupOfflineMessage.OfflineMessages.GetRange(i* step, lastCount);
                GroupOfflineMessage offlineMessage = new GroupOfflineMessage() { GroupID = groupOfflineMessage.GroupID, OfflineMessages = tempMessages };
                byte[] bMsg = CompactPropertySerializer.Default.Serialize<GroupOfflineMessage>(offlineMessage);
                this.rapidServerEngine.CustomizeController.SendBlob(destUserID, this.talkBaseInfoTypes.GroupOfflineMessage, bMsg, 2048, clientType);
            }   
        }

        //推送消息给移动端
        private void PushMessage(string destUserID, string sourceUserID, UnitType sourceUnitType, byte[] info)
        {
            if (this.messagePusher != null)
            {
                ChatBoxContent chatBoxContent = ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<ChatBoxContent>(info, 0);
                string content = chatBoxContent.GetTextWithPicPlaceholder("[图]");
                IUnit unit = this.serverGlobalCache.GetUnit(sourceUserID);
                this.messagePusher.PushMessage(destUserID, unit?.DisplayName,content);
            }
        }

        private void PushMediaMessage(string destUserID, string sourceUserID, ClientType sourceClientType, byte[] info)
        {
            if (this.messagePusher != null)
            {
                MediaCommunicateContract mediaCommunicateContract = ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<MediaCommunicateContract>(info, 0);
                if (mediaCommunicateContract == null) { return; }

                string mediaTypeStr = "";
                switch (mediaCommunicateContract.CommunicateMediaType)
                {
                    case CommunicateMediaType.Audio:
                        mediaTypeStr = "语音通话";
                        break;
                    case CommunicateMediaType.Video:
                        mediaTypeStr = "视频通话";
                        break;
                    case CommunicateMediaType.GroupVideo:
                        mediaTypeStr = "群视频通话";
                        break;
                    default:
                        return;
                }
                IUnit unit = this.serverGlobalCache.GetUnit(sourceUserID);

                Tuple<string, string>[] tuples = new Tuple<string, string>[] {
                    new Tuple<string, string>("MediaSpeakerID",sourceUserID),  
                    new Tuple<string, string>("CommunicateType", ((int)mediaCommunicateContract.CommunicateType).ToString()), 
                    new Tuple<string, string>("CommunicateMediaType", ((int)mediaCommunicateContract.CommunicateMediaType).ToString()),
                    new Tuple<string, string>("Tag",mediaCommunicateContract.Tag),
                    new Tuple<string, string>("DestClientType",((int)mediaCommunicateContract.DestClientType).ToString()),
                    new Tuple<string, string>("SourceClientType",((int)sourceClientType).ToString()),
                };
                string CommunicateTypeStr = "";
                switch (mediaCommunicateContract.CommunicateType)
                {
                    case CommunicateType.Request:
                        CommunicateTypeStr = "邀请您进行";
                        break;
                    case CommunicateType.Agree:
                        CommunicateTypeStr = "同意了";
                        break;
                    case CommunicateType.Reject:
                        CommunicateTypeStr = "拒绝了";
                        break;
                    case CommunicateType.Terminate:
                        CommunicateTypeStr = "中断了";
                        break;
                    default:
                        return;
                }

                this.messagePusher.PushMessage(destUserID, unit?.DisplayName, CommunicateTypeStr + mediaTypeStr, tuples);
            }

        }

        #endregion

        #region 离线文件
        void FileController_FileResponseReceived(ESPlus.FileTransceiver.ITransferingProject pro, bool agree)
        {
            TransferingProject project = pro as TransferingProject;
            string senderID = Comment4OfflineFile.ParseUserID(project.Comment);
            if (senderID == null)
            {
                return;
            }

            if (!agree && senderID != project.AccepterID) //客户端拒绝接收离线文件
            {
                File.Delete(project.OriginPath); //删除在服务端保存的离线文件

                //通知发送方
                OfflineFileResultNotifyContract contract = new OfflineFileResultNotifyContract(project.AccepterID, project.ProjectName, false);
                this.rapidServerEngine.CustomizeController.Send(senderID, this.talkBaseInfoTypes.OfflineFileResultNotify, CompactPropertySerializer.Default.Serialize<OfflineFileResultNotifyContract>(contract));

            }
        }

        //离线文件发送给接收者完成
        void FileSendingEvents_FileTransCompleted(ESPlus.FileTransceiver.ITransferingProject pro)
        {
            TransferingProject project = pro as TransferingProject;
            string senderID = Comment4OfflineFile.ParseUserID(project.Comment);
            if (senderID == null)
            {
                return;
            }
            if (project.IsFolder)
            {
                Directory.Delete(project.OriginPath,true);
            }
            else
            {
                File.Delete(project.OriginPath);
            }

            //通知发送方
            OfflineFileResultNotifyContract contract = new OfflineFileResultNotifyContract(project.AccepterID, project.ProjectName, true);
            this.rapidServerEngine.CustomizeController.Send(senderID, this.talkBaseInfoTypes.OfflineFileResultNotify, CompactPropertySerializer.Default.Serialize<OfflineFileResultNotifyContract>(contract));
        }

        //发送者上传离线文件完成
        void FileReceivingEvents_FileTransCompleted(ESPlus.FileTransceiver.ITransferingProject pro)
        {
            ClientType senderType = ClientType.Others;
            TransferingProject project = pro as TransferingProject; 
            string accepterID = Comment4OfflineFile.ParseUserID(project.Comment, out senderType);
            if (accepterID == null)
            {
                return;
            }
            if (project.AccepterID != project.SenderID)
            {
                if (this.rapidServerEngine.UserManager.IsUserOnLine(accepterID)) //如果接收者在线，则直接转发离线文件
                {
                    string newProjectID = null;
                    this.rapidServerEngine.FileController.BeginSendFile(accepterID, project.LocalSavePath, Comment4OfflineFile.BuildComment(project.SenderID, senderType), out newProjectID);
                }
                else
                {
                    OfflineFileItem item = new OfflineFileItem();
                    item.AccepterID = accepterID;
                    item.FileLength = (int)project.TotalSize;
                    item.FileName = project.ProjectName;
                    item.SenderID = project.SenderID;
                    item.RelayFilePath = project.LocalSavePath;
                    item.SenderType = senderType;
                    this.serverGlobalCache.StoreOfflineFileItem(item);
                }
            }
            else //多端助手
            {
                UserData data = this.rapidServerEngine.UserManager.GetUserData(accepterID);
                foreach (ClientType type in this.clientTypeList)
                {
                    if (type == senderType)
                    {
                        continue;
                    }

                    if (data != null && data.GetDevice(type) != null)
                    {
                        string newProjectID = null;
                        this.rapidServerEngine.FileController.BeginSendFile(accepterID, type, project.LocalSavePath, Comment4OfflineFile.BuildComment(project.SenderID, senderType), out newProjectID);
                    }
                    else
                    {
                        OfflineFileItem item = new OfflineFileItem();
                        item.AccepterID = accepterID;
                        item.FileLength = (int)project.TotalSize;
                        item.FileName = project.ProjectName;
                        item.SenderID = project.SenderID;
                        item.RelayFilePath = project.LocalSavePath;
                        item.SenderType = senderType;
                        item.AccepterType = type;
                        this.serverGlobalCache.StoreOfflineFileItem(item);
                    }
                }
            }
        }




        //发送者请求上传离线文件
        void FileController_FileRequestReceived(string projectID, string senderID, ClientType senderType, string fileName, ulong totalSize, ESPlus.FileTransceiver.ResumedProjectItem resumedFileItem, string comment)
        {
            string accepterID = Comment4OfflineFile.ParseUserID(comment);
            if (accepterID == null)
            {
                return;
            }

            string saveFileDir = AppDomain.CurrentDomain.BaseDirectory + "\\OfflineFiles\\";//根据某种策略得到存放文件的路径
            if (!Directory.Exists(saveFileDir))
            {
                Directory.CreateDirectory(saveFileDir);
            }

            saveFileDir = string.Format("{0}\\{1}\\", saveFileDir, accepterID);
            if (!Directory.Exists(saveFileDir))
            {
                Directory.CreateDirectory(saveFileDir);
            }
            string saveFilePath = saveFileDir + fileName;

            //给客户端回复同意，并开始准备接收文件。
            this.rapidServerEngine.FileController.BeginReceiveFile(projectID, saveFilePath);
        }

        /// <summary>
        /// 向目标用户发送离线文件。
        /// </summary>       
        public void SendOfflineFile(string accepterID)
        {
            List<OfflineFileItem> list = this.serverGlobalCache.PickupOfflineFileItem(accepterID);
            if (list != null)
            {
                foreach (OfflineFileItem item in list)
                {
                    string projectID = null;
                    this.rapidServerEngine.FileController.BeginSendFile(item.AccepterID, item.RelayFilePath, Comment4OfflineFile.BuildComment(item.SenderID, item.SenderType), out projectID);
                }
            }

        }

        public void SendAssistantFile(string userID, ClientType type)
        {
            List<OfflineFileItem> list = this.serverGlobalCache.PickupOfflineFileItem4Assistant(userID , type);
            if (list != null)
            {
                foreach (OfflineFileItem item in list)
                {
                    string projectID = null;
                    this.rapidServerEngine.FileController.BeginSendFile(item.AccepterID, item.AccepterType, item.RelayFilePath, Comment4OfflineFile.BuildComment(item.SenderID, item.SenderType), out projectID);
                }
            }

        }
        #endregion

        #region HandleInformation
        /// <summary>
        /// 处理来自客户端的消息。
        /// </summary> 
        public void HandleInformation(string sourceUserID, ClientType clientType, int informationType, byte[] info)
        {
            this.HandleInformation(sourceUserID, clientType, informationType, info, false);
        }

        public void HandleInformation(string sourceUserID, ClientType clientType, int informationType, byte[] info, bool serverCommand)
        {
            if (TalkBase.Core.Global.IsExpired())
            {
                return;
            }

            #region Friends
            if (informationType == this.talkBaseInfoTypes.AddFriendCatalog)
            {
                string catalogName = System.Text.Encoding.UTF8.GetString(info);
                this.serverGlobalCache.AddFriendCatalog(sourceUserID, catalogName);
                return;
            }

            if (informationType == this.talkBaseInfoTypes.RemoveFriend)
            {
                string friendID = System.Text.Encoding.UTF8.GetString(info);
                this.serverGlobalCache.RemoveFriend(sourceUserID, friendID);
                //通知好友
                this.rapidServerEngine.CustomizeController.Send(friendID, this.talkBaseInfoTypes.FriendRemovedNotify, System.Text.Encoding.UTF8.GetBytes(sourceUserID));
                return;
            }

            if (informationType == this.talkBaseInfoTypes.RemoveFriendCatalog)
            {
                string catalogName = System.Text.Encoding.UTF8.GetString(info);
                this.serverGlobalCache.RemoveFriendCatalog(sourceUserID, catalogName);
                return;
            }

            if (informationType == this.talkBaseInfoTypes.ChangeFriendCatalogName)
            {
                ChangeCatalogContract contract = CompactPropertySerializer.Default.Deserialize<ChangeCatalogContract>(info, 0);
                this.serverGlobalCache.ChangeFriendCatalogName(sourceUserID, contract.OldName, contract.NewName);
                return;
            }

            if (informationType == this.talkBaseInfoTypes.MoveFriendToOtherCatalog)
            {
                MoveFriendToOtherCatalogContract contract = CompactPropertySerializer.Default.Deserialize<MoveFriendToOtherCatalogContract>(info, 0);
                this.serverGlobalCache.MoveFriend(sourceUserID, contract.FriendID, contract.OldCatalog, contract.NewCatalog);

                //如果是移入黑名单，则需要将自己从对方的好友中删除
                if (contract.NewCatalog == FunctionOptions.BlackListCatalogName)
                {
                    this.serverGlobalCache.BlackFriend(sourceUserID, contract.FriendID);
                    this.rapidServerEngine.CustomizeController.Send(contract.FriendID, this.talkBaseInfoTypes.FriendRemovedNotify, System.Text.Encoding.UTF8.GetBytes(sourceUserID));
                }
                else if (contract.OldCatalog == FunctionOptions.BlackListCatalogName)
                {
                    this.serverGlobalCache.WhiteFriend(sourceUserID, contract.FriendID);

                    TUser owner = this.serverGlobalCache.GetUser(sourceUserID);
                    owner = (TUser)owner.PartialCopy;
                    byte[] ownerBuff = CompactPropertySerializer.Default.Serialize<TUser>(owner);
                    this.rapidServerEngine.SendMessage(contract.FriendID, this.talkBaseInfoTypes.FriendAddedNotify, ownerBuff, FunctionOptions.DefaultFriendCatalog, null);
                }
                else
                {
                }
                return;
            } 
            #endregion

            #region Group
            if (informationType == this.talkBaseInfoTypes.QuitGroup)
            {
                string groupID = System.Text.Encoding.UTF8.GetString(info);
                this.serverGlobalCache.QuitGroup(sourceUserID, groupID);
                //通知其它组成员
                this.rapidServerEngine.ContactsController.Broadcast(groupID, this.talkBaseInfoTypes.SomeoneQuitGroup, System.Text.Encoding.UTF8.GetBytes(sourceUserID), null, ESFramework.ActionTypeOnChannelIsBusy.Continue);

                return;
            }

            if (informationType == this.talkBaseInfoTypes.DeleteGroup)
            {
                string groupID = System.Text.Encoding.UTF8.GetString(info);
                this.serverGlobalCache.DeleteGroup(groupID);
                //通知其它组成员
                this.rapidServerEngine.ContactsController.Broadcast(groupID, this.talkBaseInfoTypes.GroupDeleted, System.Text.Encoding.UTF8.GetBytes(sourceUserID), null, ESFramework.ActionTypeOnChannelIsBusy.Continue);
                return;
            }

            if (informationType == this.talkBaseInfoTypes.ChangeGroupMembers)
            {
                ChangeGroupMembersContract contract = CompactPropertySerializer.Default.Deserialize<ChangeGroupMembersContract>(info, 0);
                TGroup group = this.serverGlobalCache.GetGroup(contract.GroupID);
                if (group == null)
                {
                    return;
                }

                if (contract.RemovedMembers.Count > 0)
                {
                    ManageGroupMembersNotifyContract notify1 = new ManageGroupMembersNotifyContract(sourceUserID, contract.RemovedMembers ,serverCommand);
                    byte[] notifyData = CompactPropertySerializer.Default.Serialize(notify1);
                    this.rapidServerEngine.ContactsController.Broadcast(group.ID, this.talkBaseInfoTypes.RemovedFromGroupNotify, notifyData, null, ESFramework.ActionTypeOnChannelIsBusy.Continue);

                    foreach (string removedUserID in contract.RemovedMembers)
                    {
                        this.serverGlobalCache.QuitGroup(removedUserID, contract.GroupID);
                    }
                }

                if (contract.AddedMembers.Count > 0)
                {
                    foreach (string addedUserID in contract.AddedMembers)
                    {
                        this.serverGlobalCache.JoinGroup(addedUserID, contract.GroupID);
                    }

                    ManageGroupMembersNotifyContract notify2 = new ManageGroupMembersNotifyContract(sourceUserID, contract.AddedMembers, serverCommand);
                    byte[] notifyData2 = CompactPropertySerializer.Default.Serialize(notify2);
                    this.rapidServerEngine.ContactsController.Broadcast(group.ID, this.talkBaseInfoTypes.PulledIntoGroupNotify, notifyData2, null, ESFramework.ActionTypeOnChannelIsBusy.Continue);
                }
                 
                return;
            }

            if (informationType == this.talkBaseInfoTypes.ChangeGroupInfo)
            {
                ChangeGroupInfoContract contract = CompactPropertySerializer.Default.Deserialize<ChangeGroupInfoContract>(info, 0);
                this.serverGlobalCache.ChangeGroupInfo(contract.GroupID, contract.GroupName, contract.Announce);
                //通知其它组成员
                this.rapidServerEngine.ContactsController.Broadcast(contract.GroupID, this.talkBaseInfoTypes.GroupInfoChanged, info, null, ActionTypeOnChannelIsBusy.Continue);
                return;
            }
            #endregion

            #region GetOfflineMessage/GetOfflineFile/ChangeStatus/ChangeMyInformation


            if (informationType == this.talkBaseInfoTypes.GetOfflineMessage)
            {
                this.SendOfflineMessage(sourceUserID);
                return;
            }
            if (informationType == this.talkBaseInfoTypes.GetGroupOfflineMessage)
            {
                this.SendGroupOfflineMessage(sourceUserID, clientType);
                return;
            }

            if (informationType == this.talkBaseInfoTypes.GetOfflineFile)
            {
                this.SendOfflineFile(sourceUserID);
                this.SendAssistantFile(sourceUserID, clientType);
                return;
            }

            if (informationType == this.talkBaseInfoTypes.ChangeStatus)
            {
                TUser user = this.serverGlobalCache.GetUser(sourceUserID);
                int newStatus = BitConverter.ToInt32(info, 0);
                user.UserStatus = (UserStatus)newStatus;
                List<string> contacts = this.serverGlobalCache.GetAllContactsNecessary(sourceUserID);
                UserStatusChangedContract contract = new UserStatusChangedContract(sourceUserID, newStatus);
                byte[] msg = ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(contract);
                foreach (string friendID in contacts)
                {
                    this.rapidServerEngine.CustomizeController.Send(friendID, this.talkBaseInfoTypes.UserStatusChanged, msg);
                }
                return;
            }           
            #endregion
            
        } 
        #endregion

        #region HandleQuery
        /// <summary>
        /// 处理来自客户端的同步调用请求。
        /// </summary>       
        public byte[] HandleQuery(string sourceUserID, ClientType clientType, int informationType, byte[] info)
        {
            return this.HandleQuery(sourceUserID, clientType ,informationType, info, false);
        }

        public byte[] HandleQuery(string sourceUserID, ClientType clientType, int informationType, byte[] info, bool serverCommand)
        {
            if (informationType == this.talkBaseInfoTypes.GetFriendIDList)
            {
                List<string> friendIDs = this.serverGlobalCache.GetFriends(sourceUserID);
                return CompactPropertySerializer.Default.Serialize<List<string>>(friendIDs);
            }

            if (informationType == this.talkBaseInfoTypes.AddFriend)
            {
                AddFriendContract contract = CompactPropertySerializer.Default.Deserialize<AddFriendContract>(info, 0);
                bool isExist = this.serverGlobalCache.IsUserExist(contract.FriendID);
                if (!isExist)
                {
                    return BitConverter.GetBytes((int)AddFriendResult.FriendNotExist);
                }
                this.serverGlobalCache.AddFriend(sourceUserID, contract.FriendID, contract.CatalogName);

                //0922
                TUser owner = this.serverGlobalCache.GetUser(sourceUserID);
                owner = (TUser)owner.PartialCopy;
                byte[] ownerBuff = CompactPropertySerializer.Default.Serialize<TUser>(owner);

                //通知对方
                this.rapidServerEngine.SendMessage(contract.FriendID, this.talkBaseInfoTypes.FriendAddedNotify, ownerBuff, FunctionOptions.DefaultFriendCatalog, null);
                return BitConverter.GetBytes((int)AddFriendResult.Succeed);
            }

            if (informationType == this.talkBaseInfoTypes.InHisBlackList)
            {
                string friendID = System.Text.Encoding.UTF8.GetString(info);
                TUser friend = this.serverGlobalCache.GetUser(friendID);
                if (friend == null)
                {
                    return BitConverter.GetBytes(false);
                }
                return BitConverter.GetBytes(friend.IsInBlackList(sourceUserID));
            }

            if (informationType == this.talkBaseInfoTypes.GetAllContacts)
            {
                List<string> contacts = this.serverGlobalCache.GetAllContacts(sourceUserID);
                Dictionary<string, TUser> contactDic = new Dictionary<string, TUser>();
                foreach (string friendID in contacts)
                {
                    if (!contactDic.ContainsKey(friendID))
                    {
                        TUser friend = this.serverGlobalCache.GetUser(friendID);
                        if (friend != null)
                        {
                            contactDic.Add(friendID, (TUser)friend.PartialCopy);
                        }
                    }
                }

                return CompactPropertySerializer.Default.Serialize<List<TUser>>(new List<TUser>(contactDic.Values));
            }

            if (informationType == this.talkBaseInfoTypes.GetAllContactIDs)
            {
                List<string> contacts = this.serverGlobalCache.GetAllContacts(sourceUserID);
                return CompactPropertySerializer.Default.Serialize<List<string>>(contacts);
            }

            if (informationType == this.talkBaseInfoTypes.GetSomeUsers)
            {
                List<string> friendIDs = CompactPropertySerializer.Default.Deserialize<List<string>>(info, 0);
                List<TUser> friends = new List<TUser>();
                foreach (string friendID in friendIDs)
                {
                    TUser friend = this.serverGlobalCache.GetUser(friendID);
                    if (friend != null)
                    {
                        friends.Add((TUser)friend.PartialCopy);
                    }
                }

                return CompactPropertySerializer.Default.Serialize<List<TUser>>(friends);
            }

            if (informationType == this.talkBaseInfoTypes.GetContactsRTData)
            {
                List<string> contacts = this.serverGlobalCache.GetAllContactsNecessary(sourceUserID);

                Dictionary<string, UserRTData> dic = new Dictionary<string, UserRTData>();
                foreach (string friendID in contacts)
                {
                    if (!dic.ContainsKey(friendID))
                    {
                        TUser data = this.serverGlobalCache.GetUser(friendID);
                        if (data != null)
                        {
                            UserRTData rtData = new UserRTData(data.UserStatus, data.Version);
                            dic.Add(friendID, rtData);
                        }
                    }
                }
                Dictionary<string, int> groupVerDic = this.serverGlobalCache.GetMyGroupVersions(sourceUserID);
                ContactsRTDataContract contract = new ContactsRTDataContract(dic, groupVerDic );
                return CompactPropertySerializer.Default.Serialize(contract);
            }

            if (informationType == this.talkBaseInfoTypes.GetUserInfo)
            {
                string target = System.Text.Encoding.UTF8.GetString(info);
                TUser user = this.serverGlobalCache.GetUser(target);
                if (user == null)
                {
                    return null;
                }
                if (sourceUserID != target)   
                {
                    user = (TUser)user.PartialCopy;
                }
                return CompactPropertySerializer.Default.Serialize<TUser>(user);
            }

            if (informationType == this.talkBaseInfoTypes.GetUserInfoNewVersion)
            {
                GetUserInfoNewVersionContract contact = CompactPropertySerializer.Default.Deserialize<GetUserInfoNewVersionContract>(info, 0);
                TUser user = this.serverGlobalCache.GetUser(contact.UserID);
                if (user == null || user.Version <= contact.Version)
                {
                    return null;
                }
                if (sourceUserID != contact.UserID)
                {
                    user = (TUser)user.PartialCopy;
                }
                return CompactPropertySerializer.Default.Serialize<TUser>(user);
            }

            if (informationType == this.talkBaseInfoTypes.GetMyGroups)
            {
                List<TGroup> myGroups = this.serverGlobalCache.GetMyGroups(sourceUserID);
                return CompactPropertySerializer.Default.Serialize(myGroups);
            }

            if (informationType == this.talkBaseInfoTypes.GetSomeGroups)
            {
                List<string> groups = ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<List<string>>(info, 0);
                List<TGroup> myGroups = new List<TGroup>();
                foreach (string groupID in groups)
                {
                    TGroup group = this.serverGlobalCache.GetGroup(groupID);
                    if (group != null)
                    {
                        myGroups.Add(group);
                    }
                }

                return CompactPropertySerializer.Default.Serialize(myGroups);
            }

            if (informationType == this.talkBaseInfoTypes.JoinGroup)
            {
                string groupID = System.Text.Encoding.UTF8.GetString(info);
                JoinGroupResult res = this.serverGlobalCache.JoinGroup(sourceUserID, groupID);
                if (res == JoinGroupResult.Succeed)
                {
                    //通知其它组成员
                    this.rapidServerEngine.ContactsController.Broadcast(groupID, this.talkBaseInfoTypes.SomeoneJoinGroup, System.Text.Encoding.UTF8.GetBytes(sourceUserID), null, ESFramework.ActionTypeOnChannelIsBusy.Continue);
                }
                return BitConverter.GetBytes((int)res);
            }

            if (informationType == this.talkBaseInfoTypes.CreateGroup)
            {
                CreateGroupContract contract = CompactPropertySerializer.Default.Deserialize<CreateGroupContract>(info, 0);
                CreateGroupResult res = this.serverGlobalCache.CreateGroup(sourceUserID, contract.ID, contract.Name, contract.Announce, contract.Members,contract.IsPrivate);
                if (res == CreateGroupResult.Succeed && contract.Members != null)
                {
                    //通知其它组成员
                    ManageGroupMembersNotifyContract notify = new ManageGroupMembersNotifyContract(sourceUserID, contract.Members, serverCommand);
                    byte[] notifyData = CompactPropertySerializer.Default.Serialize(notify);
                    this.rapidServerEngine.ContactsController.Broadcast(contract.ID, this.talkBaseInfoTypes.PulledIntoGroupNotify, notifyData, null, ESFramework.ActionTypeOnChannelIsBusy.Continue);
                }
                return BitConverter.GetBytes((int)res); 
            }

            if (informationType == this.talkBaseInfoTypes.GetGroup)
            {
                string groupID = System.Text.Encoding.UTF8.GetString(info);
                TGroup group = this.serverGlobalCache.GetGroup(groupID);
                return CompactPropertySerializer.Default.Serialize(group);
            }

            if (informationType == this.talkBaseInfoTypes.SearchGroup)
            {
                string idOrName= System.Text.Encoding.UTF8.GetString(info);
                List<TGroup> groups = this.serverGlobalCache.SearchGroup(idOrName);
                return CompactPropertySerializer.Default.Serialize(groups);
            }

            if (informationType == this.talkBaseInfoTypes.SearchUser)
            {
                string idOrName = System.Text.Encoding.UTF8.GetString(info);
                List<TUser> users = this.serverGlobalCache.SearchUser(idOrName);
                return CompactPropertySerializer.Default.Serialize(users);
            }

            if (informationType == this.talkBaseInfoTypes.ChangePassword)
            {
                ChangePasswordContract contract = CompactPropertySerializer.Default.Deserialize<ChangePasswordContract>(info, 0);
                ChangePasswordResult res = this.serverGlobalCache.ChangePassword(sourceUserID, contract.OldPasswordMD5, contract.NewPasswordMD5);
                return BitConverter.GetBytes((int)res);
            }

            if (informationType == this.talkBaseInfoTypes.DeleteUser)
            {
                string userID = System.Text.Encoding.UTF8.GetString(info);
                TUser user = this.serverGlobalCache.GetUser(userID);
                if (user == null)
                {
                    return BitConverter.GetBytes(false);
                }

                OperateContract contract = new OperateContract(sourceUserID, userID);
                byte[] notify = CompactPropertySerializer.Default.Serialize(contract);
                List<string> contacts = this.serverGlobalCache.GetAllContacts(userID);
                foreach (string friendID in contacts)
                {
                    this.rapidServerEngine.CustomizeController.Send(friendID, this.talkBaseInfoTypes.SomeoneDeletedNotify, notify);
                }
                this.serverGlobalCache.DeleteUser(userID);
                return BitConverter.GetBytes(true);
            }
            if (informationType == this.talkBaseInfoTypes.GetGroupBans4Group)
            {
                string groupID = System.Text.Encoding.UTF8.GetString(info);
                List<GroupBan> groupBans = this.serverGlobalCache.GetGroupBans4Group(groupID);
                return CompactPropertySerializer.Default.Serialize(groupBans);                 
            }
            if (informationType == this.talkBaseInfoTypes.ExistAllGroupBan)
            {
                string groupID = System.Text.Encoding.UTF8.GetString(info);
                bool isExist = this.serverGlobalCache.ExistAllGroupBan(groupID);
                return BitConverter.GetBytes(isExist);
            }
            if (informationType == this.talkBaseInfoTypes.GetChatMessageRecord)
            {
                int id = BitConverter.ToInt32(info,0);
                ChatMessageRecord record = this.serverGlobalCache.GetChatMessageRecord(id);
                return CompactPropertySerializer.Default.Serialize(record);
            }
            if (informationType == this.talkBaseInfoTypes.GetChatRecordPage)
            {
                GetChatRecordPageContract contract = CompactPropertySerializer.Default.Deserialize<GetChatRecordPageContract>(info, 0);
                ChatRecordPage record = this.serverGlobalCache.GetChatRecordPage(contract.TimeScope,contract.MyID,contract.FriendID,contract.PageSize,contract.PageIndex);
                return CompactPropertySerializer.Default.Serialize(record);
            }
            if (informationType == this.talkBaseInfoTypes.GetGroupChatRecordPage)
            {
                GetGroupChatRecordPageContract contract = CompactPropertySerializer.Default.Deserialize<GetGroupChatRecordPageContract>(info, 0);
                ChatRecordPage record = this.serverGlobalCache.GetGroupChatRecordPage(contract.TimeScope,contract.GroupID,contract.PageSize,contract.PageIndex);
                return CompactPropertySerializer.Default.Serialize(record);
            }

            return null;
        } 
        #endregion

        public bool CanHandle(int informationType)
        {
            return this.talkBaseInfoTypes.Contains(informationType);
        }

        #region 服务器直接进行数据操作
        public void RemoveMemberFromGroup(string operatorID, string groupID, params string[] members)
        {
            if (!this.serverGlobalCache.IsUserExist(operatorID))
            {
                throw new Exception(string.Format("Operator {0} is not exist !", operatorID));
            }

            ChangeGroupMembersContract contract = new ChangeGroupMembersContract(groupID, new List<string>(), new List<string>(members));
            byte[] data = CompactPropertySerializer.Default.Serialize(contract);
            this.HandleInformation(operatorID, ClientType.ServerSide , this.talkBaseInfoTypes.ChangeGroupMembers, data ,true);
        }

        public void AddMemberIntoGroup(string operatorID, string groupID, params string[] members)
        {
            if (!this.serverGlobalCache.IsUserExist(operatorID))
            {
                throw new Exception(string.Format("Operator {0} is not exist !", operatorID));
            }

            ChangeGroupMembersContract contract = new ChangeGroupMembersContract(groupID, new List<string>(members), new List<string>());
            byte[] data = CompactPropertySerializer.Default.Serialize(contract);
            this.HandleInformation(operatorID, ClientType.ServerSide, this.talkBaseInfoTypes.ChangeGroupMembers, data, true);
        }

        public CreateGroupResult CreateGroup(string groupID, string groupName, string creatorID)
        {
            if (!this.serverGlobalCache.IsUserExist(creatorID))
            {
                throw new Exception(string.Format("Creator {0} is not exist !", creatorID));
            }            

            CreateGroupContract contract = new CreateGroupContract(GroupType.CommonGroup, groupID, groupName, "", new List<string>() { creatorID },false);
            byte[] data = CompactPropertySerializer.Default.Serialize(contract);
            byte[] res = this.HandleQuery(creatorID, ClientType.ServerSide, this.talkBaseInfoTypes.CreateGroup, data, true);
            return (CreateGroupResult)BitConverter.ToInt32(res, 0);
        }

        public void DeleteUser(string operatorID, string userID)
        {
            if (!this.serverGlobalCache.IsUserExist(operatorID))
            {
                throw new Exception(string.Format("Operator {0} is not exist !" ,operatorID));
            }
            byte[] data = System.Text.Encoding.UTF8.GetBytes(userID);
            this.HandleQuery(operatorID, ClientType.ServerSide, this.talkBaseInfoTypes.DeleteUser, data, true);
        }

        public void AddFriend(string user1, string user2)
        {
            bool isExist1 = this.serverGlobalCache.IsUserExist(user1);
            bool isExist2 = this.serverGlobalCache.IsUserExist(user2);
            if (!isExist1 || !isExist2)
            {
                throw new Exception("User is not exist !");
            }

            this.serverGlobalCache.AddFriend(user1, user2, FunctionOptions.DefaultFriendCatalog);
            
            TUser owner1 = this.serverGlobalCache.GetUser(user1);
            owner1 = (TUser)owner1.PartialCopy;
            byte[] ownerBuff1 = CompactPropertySerializer.Default.Serialize<TUser>(owner1);            
            this.rapidServerEngine.SendMessage(user2, this.talkBaseInfoTypes.FriendAddedNotify, ownerBuff1, FunctionOptions.DefaultFriendCatalog, null);
            
            TUser owner2 = this.serverGlobalCache.GetUser(user2);
            owner2 = (TUser)owner2.PartialCopy;
            byte[] ownerBuff2 = CompactPropertySerializer.Default.Serialize<TUser>(owner2);
            this.rapidServerEngine.SendMessage(user1, this.talkBaseInfoTypes.FriendAddedNotify, ownerBuff2, FunctionOptions.DefaultFriendCatalog, null);                
        }

        public void RemoveFriend(string user1, string user2)
        {
            bool isExist1 = this.serverGlobalCache.IsUserExist(user1);
            bool isExist2 = this.serverGlobalCache.IsUserExist(user2);
            if (!isExist1 || !isExist2)
            {
                throw new Exception("User is not exist !");
            }

            this.serverGlobalCache.RemoveFriend(user1, user2);

            byte[] notify1 = System.Text.Encoding.UTF8.GetBytes(user1);
            this.rapidServerEngine.CustomizeController.Send(user2, this.talkBaseInfoTypes.FriendRemovedNotify, notify1, true, ESFramework.ActionTypeOnChannelIsBusy.Continue);

            byte[] notify2 = System.Text.Encoding.UTF8.GetBytes(user2);
            this.rapidServerEngine.CustomizeController.Send(user1, this.talkBaseInfoTypes.FriendRemovedNotify, notify2, true, ESFramework.ActionTypeOnChannelIsBusy.Continue);
        }

        public void ChangeUserBaseInfo(string userID, string name, string signature, string orgID)
        {
            ChangeUserBaseInfoContract contract = new ChangeUserBaseInfoContract(userID, name, signature, orgID);
            byte[] data = CompactPropertySerializer.Default.Serialize(contract);
            this.HandleInformation(userID, ClientType.ServerSide, this.talkBaseInfoTypes.ChangeMyBaseInfo, data, true);
        }

        public void RequestAddFirend(string requesterID,string accepterID,string comment,string requesterCatalogName)
        {
            RequestAddFriendContract contract = new RequestAddFriendContract()
            {
                RequesterID = requesterID,
                AccepterID = accepterID,
                Comment = comment,
                RequesterCatalogName = requesterCatalogName
            };
            this.rapidServerEngine_MessageReceived(requesterID, ClientType.ServerSide, this.talkBaseInfoTypes.RequestAddFriend, CompactPropertySerializer.Default.Serialize(contract), null);
        }
        #endregion
    }


    public interface ITalkBaseHandler
    {
        /// <summary>
        /// 当转发系统消息（接受者为某个用户或群）时，触发此事件。
        /// </summary>
        event CbGeneric<SystemMessageContract> SystemMessageTransfering;
    }
}
