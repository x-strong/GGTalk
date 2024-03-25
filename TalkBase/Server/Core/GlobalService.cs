using System;
using System.Collections.Generic;
using System.Text;
using ESPlus.Rapid;
using ESPlus.Serialization;
using ESFramework;

namespace TalkBase.Server
{
    /// <summary>
    /// 提供给第三方系统的Remoting服务。第三方系统在操作完DB后，再使用该接口通知服务端。
    /// </summary>  
    internal class GlobalService<TUser, TGroup> : MarshalByRefObject, IGlobalService
        where TUser : TalkBase.IUser, new()
        where TGroup : TalkBase.IGroup
    {
        private ServerGlobalCache<TUser, TGroup> globalCache;
        private IRapidServerEngine rapidServerEngine;
        private TalkBaseInfoTypes talkBaseInfoTypes;
        private CoreHandler<TUser, TGroup> coreHandler;
        private ServerHandler<TUser, TGroup> serverHandler;

        public GlobalService(ServerGlobalCache<TUser, TGroup> cache, IRapidServerEngine engine, CoreHandler<TUser, TGroup> cHandler,ServerHandler<TUser, TGroup> handler , TalkBaseInfoTypes types)
        {
            this.globalCache = cache;
            this.rapidServerEngine = engine;
            this.talkBaseInfoTypes = types;
            this.coreHandler = cHandler;
            this.serverHandler = handler;
        }

        public void OnUserRegistered(params string[] users)
        {
            foreach (string newUserID in users)
            {
                TUser newUser = this.globalCache.GetUser(newUserID); //加载到内存
                if (newUser != null)
                {
                    byte[] info = ESPlus.Serialization.CompactPropertySerializer.Default.Serialize<TUser>((TUser)newUser.PartialCopy);
                    List<string> contacts = this.globalCache.GetAllContacts(newUserID); 
                    foreach (string userID in contacts)
                    {
                        this.rapidServerEngine.CustomizeController.Send(userID, this.talkBaseInfoTypes.SomeoneRegisteredNotify, info);
                    }
                }          
            }            
        }

        public void OnUserDeleted(params string[] users)
        {
            foreach (string userID in users)
            {
                TUser user = this.globalCache.GetUser(userID);
                if (user == null)
                {
                    return;
                }

                OperateContract contract = new OperateContract(null, userID);
                byte[] info = CompactPropertySerializer.Default.Serialize(contract);
                List<string> contacts = this.globalCache.GetAllContacts(userID);// GetAllContacts 会根据是否启用了组织结构，而返回正确的结果
                foreach (string friendID in contacts)
                {
                    this.rapidServerEngine.CustomizeController.Send(friendID, this.talkBaseInfoTypes.SomeoneDeletedNotify, info);
                }
                this.globalCache.DeleteUser(userID);
            }
        }

        public void OnUserInfoChanged(params string[] users)
        {
            foreach (string userID in users)
            {
                TUser user = this.globalCache.ReloadUser(userID);
                if (user != null)
                {
                    ChangeUserBaseInfoContract contract = new ChangeUserBaseInfoContract(userID, user.Name, user.Signature, user.OrgID);
                    contract.UserLatestVersion = user.Version;
                    byte[] notify = CompactPropertySerializer.Default.Serialize(contract);
                    this.rapidServerEngine.CustomizeController.Send(userID, this.talkBaseInfoTypes.UserBaseInfoChanged, notify, true, ActionTypeOnChannelIsBusy.Continue);
                    List<string> contacts = this.globalCache.GetAllContactsNecessary(userID);
                    foreach (string friendID in contacts)
                    {
                        if (friendID != userID)
                        {
                            this.rapidServerEngine.CustomizeController.Send(friendID, this.talkBaseInfoTypes.UserBaseInfoChanged, notify, true, ActionTypeOnChannelIsBusy.Continue);
                        }
                    }
                }
            }
        }

        public void OnUserHeadImageChanged(params string[] users)
        {
            foreach (string userID in users)
            {
                TUser user = this.globalCache.ReloadUser(userID);
                if (user != null)
                {
                    ChangeHeadImageContract contract = new ChangeHeadImageContract(userID, user.HeadImageIndex, user.HeadImageData);                                   
                    contract.UserLatestVersion = user.Version;
                    byte[] notify = CompactPropertySerializer.Default.Serialize(contract);
                    this.rapidServerEngine.CustomizeController.Send(userID, this.talkBaseInfoTypes.UserHeadImageChanged, notify, true, ActionTypeOnChannelIsBusy.Continue);
                    List<string> contacts = this.globalCache.GetAllContactsNecessary(userID);
                    foreach (string friendID in contacts)
                    {
                        if (friendID != userID)
                        {
                            //可能要分块发送
                            this.rapidServerEngine.CustomizeController.Send(friendID, this.talkBaseInfoTypes.UserHeadImageChanged, notify, true, ActionTypeOnChannelIsBusy.Continue);
                        }
                    }
                }
            }
        }

        public void OnGroupAdded(params string[] groups)
        {
            this.OnGroupMembersChanged(groups);
        }

        public void OnGroupRemoved(params string[] groups)
        {
            foreach (string groupID in groups)
            {
                this.rapidServerEngine.ContactsController.Broadcast(groupID, this.talkBaseInfoTypes.GroupDeleted, null, null, ESFramework.ActionTypeOnChannelIsBusy.Continue);
                this.globalCache.DeleteGroup(groupID);
            }
        }

        public void OnGroupInfoChanged(params string[] groups)
        {
            foreach (string groupID in groups)
            {
                TGroup group = this.globalCache.ReloadGroup(groupID);
                if (group != null)
                {
                    ChangeGroupInfoContract contract = new ChangeGroupInfoContract(null, group.ID, group.Name, group.Announce);
                    byte[] notify = CompactPropertySerializer.Default.Serialize(contract);
                    this.rapidServerEngine.ContactsController.Broadcast(contract.GroupID, this.talkBaseInfoTypes.GroupInfoChanged, notify, null, ActionTypeOnChannelIsBusy.Continue);
                }
            }
        }

        public void OnGroupMembersChanged(params string[] groups)
        {
            foreach (string groupID in groups)
            {
                TGroup oldGroup = this.globalCache.GetExistedGroup(groupID);
                TGroup newGroup = this.globalCache.ReloadGroup(groupID);
                if (oldGroup == null && newGroup == null)
                {
                    continue;
                }

                if (oldGroup == null)
                {
                    ManageGroupMembersNotifyContract notify2 = new ManageGroupMembersNotifyContract(null, oldGroup.MemberList ,true);
                    byte[] notifyData2 = CompactPropertySerializer.Default.Serialize(notify2);
                    this.rapidServerEngine.ContactsController.Broadcast(groupID, this.talkBaseInfoTypes.PulledIntoGroupNotify, notifyData2, null, ESFramework.ActionTypeOnChannelIsBusy.Continue);
                }
                else if (newGroup == null)
                {
                    this.rapidServerEngine.ContactsController.Broadcast(groupID, this.talkBaseInfoTypes.GroupDeleted, null, null, ESFramework.ActionTypeOnChannelIsBusy.Continue);                    
                }
                else
                {
                    List<string> addedList = new List<string>();
                    List<string> removedList = new List<string>();
                    foreach (string userID in newGroup.MemberList)
                    {
                        if (!oldGroup.MemberList.Contains(userID))
                        {
                            addedList.Add(userID);
                        }
                    }
                    foreach (string userID in oldGroup.MemberList)
                    {
                        if (!newGroup.MemberList.Contains(userID))
                        {
                            removedList.Add(userID);
                        }
                    }

                    if (removedList.Count > 0)
                    {
                        ManageGroupMembersNotifyContract notify1 = new ManageGroupMembersNotifyContract(null, removedList ,true);
                        byte[] notifyData = CompactPropertySerializer.Default.Serialize(notify1);
                        this.rapidServerEngine.ContactsController.Broadcast(groupID, this.talkBaseInfoTypes.RemovedFromGroupNotify, notifyData, null, ESFramework.ActionTypeOnChannelIsBusy.Continue);
                    }

                    if (addedList.Count > 0)
                    {
                        ManageGroupMembersNotifyContract notify2 = new ManageGroupMembersNotifyContract(null, addedList, true);
                        byte[] notifyData2 = CompactPropertySerializer.Default.Serialize(notify2);
                        this.rapidServerEngine.ContactsController.Broadcast(groupID, this.talkBaseInfoTypes.PulledIntoGroupNotify, notifyData2, null, ESFramework.ActionTypeOnChannelIsBusy.Continue);
                    }
                }
            }
        }

        public void SendSystemMessage(string unitID, int sysMsgType, byte[] content,string tag , bool useOfflineMessageMode)
        {
            this.coreHandler.SendSystemMessage(null, unitID, sysMsgType, content,tag , useOfflineMessageMode);
        }

        public void RemoveMemberFromGroup(string operatorID, string groupID, params string[] members)
        {
            this.serverHandler.RemoveMemberFromGroup(operatorID ,groupID, members);
        }

        public void DeleteUser(string operatorID, string userID)
        {
            this.serverHandler.DeleteUser(operatorID, userID);
        }

        public void AddMemberIntoGroup(string operatorID, string groupID, params string[] members)
        {
            this.serverHandler.AddMemberIntoGroup(operatorID, groupID, members);
        }

        public void AddFriend(string user1, string user2)
        {
            this.serverHandler.AddFriend(user1, user2);
        }

        public void RemoveFriend(string user1, string user2)
        {
            this.serverHandler.RemoveFriend(user1, user2);
        }

        public CreateGroupResult CreateGroup(string groupID, string groupName ,string creatorID)
        {
            return this.serverHandler.CreateGroup(groupID, groupName, creatorID);
        }

        public void ChangeUserBaseInfo(string userID, string name, string signature, string orgID)
        {
            this.serverHandler.ChangeUserBaseInfo(userID, name, signature, orgID);
        }

        public void RequestAddFirend(string requesterID, string accepterID, string comment, string requesterCatalogName)
        {
            this.serverHandler.RequestAddFirend(requesterID, accepterID, comment, requesterCatalogName);
        }
    }
}
