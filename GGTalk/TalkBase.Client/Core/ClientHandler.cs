using System;
using System.Collections.Generic;
using System.Text;
using ESBasic;
using ESPlus.Serialization;
using System.Runtime.InteropServices;

using ESPlus.Rapid;
using ESPlus.Application.CustomizeInfo;
using OMCS.Passive.ShortMessages;
using ESFramework;
using OMCS.Passive;

namespace TalkBase.Client
{
    /// <summary>
    /// TalkBase客户端消息处理器，集中处理来自服务端或其它客户端的属于TalkBase框架定义的消息。
    /// </summary>   
    public class ClientHandler<TUser, TGroup> : IIntegratedCustomizeHandler
        where TUser : class, TalkBase.IUser, new()
        where TGroup : TalkBase.IGroup, new()
    {
        private ResourceCenter<TUser, TGroup> resourceCenter;
        private TwinkleNotifyIcon twinkleNotifyIcon;
        private IBrige4ClientOutter brige4ClientOutter;

        #region SilenceOnFriendAdded
        private bool silenceOnFriendAdded = false;
        /// <summary>
        /// 当被别人添加为好友时，是否不做任何提醒？（如果为true，ClientGlobalCache的FriendAdded事件仍然被触发，但是不会传递给TwinkleNotifyIcon和ClientOutter）
        /// </summary>
        public bool SilenceOnFriendAdded
        {
            get { return silenceOnFriendAdded; }
            set { silenceOnFriendAdded = value; }
        } 
        #endregion

        /// <summary>
        /// 初始化客户端消息处理器。
        /// </summary>
        /// <param name="center">资源中心</param>
        /// <param name="icon">支持闪动的托盘。允许为null</param>
        public void Initialize(ResourceCenter<TUser, TGroup> center, TwinkleNotifyIcon icon)
        {
            this.resourceCenter = center;           
            this.twinkleNotifyIcon = icon;
            this.brige4ClientOutter = (IBrige4ClientOutter)this.resourceCenter.ClientOutter;

            this.resourceCenter.RapidPassiveEngine.MessageReceived += new CbGeneric<string, ClientType,int, byte[], string>(rapidPassiveEngine_MessageReceived);
            this.resourceCenter.RapidPassiveEngine.EchoMessageReceived += new CbGeneric<ClientType, string, int, byte[], string>(RapidPassiveEngine_EchoMessageReceived);
            this.resourceCenter.RapidPassiveEngine.ContactsOutter.BroadcastReceived += new CbGeneric<string,ClientType, string, int, byte[] ,string>(ContactsOutter_BroadcastReceived);            
        }

        //clientType - destUserID - informationType - message - tag 。
        void RapidPassiveEngine_EchoMessageReceived(ClientType clientType, string destUserID, int informationType, byte[] info, string tag)
        {
            //if (TalkBase.Core.Global.IsExpired())
            //{
            //    return;
            //}

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.ChangeUnitCommentName)
            {
                ChangeCommentNameContract contract = CompactPropertySerializer.Default.Deserialize<ChangeCommentNameContract>(info, 0);
                this.resourceCenter.ClientGlobalCache.CurrentUser.ChangeUnitCommentName(contract.UnitID, contract.CommentName);
                this.resourceCenter.ClientGlobalCache.ChangeUnitCommentName(contract.UnitID, contract.CommentName);
                return;
            }

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.Chat)
            {
                destUserID = tag;
                this.brige4ClientOutter.OnChatMessageEchoReceived(clientType ,destUserID, info);
                return;
            }

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.ChatTransfered)
            {
                string[] accepters = tag.Split(',');
                foreach (string destID in accepters)
                {
                    IUnit unit = this.resourceCenter.ClientGlobalCache.GetUnit(destID);
                    if (unit.UnitType == UnitType.User)
                    {
                        this.brige4ClientOutter.OnChatMessageEchoReceived(clientType, destID, info);
                    }
                }
                return;
            }

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.MediaCommunicate)
            {
                MediaCommunicateContract contract = CompactPropertySerializer.Default.Deserialize<MediaCommunicateContract>(info, 0);
                if (contract.CommunicateType == CommunicateType.Agree || contract.CommunicateType == CommunicateType.Reject)
                {                    
                    bool agree = contract.CommunicateType == CommunicateType.Agree;
                    if (contract.CommunicateMediaType == CommunicateMediaType.GroupVideo)
                    {
                        destUserID = tag;
                    }
                    this.brige4ClientOutter.OnMediaCommunicateAnswerOnOtherDevice(destUserID, clientType, contract.CommunicateMediaType, agree);
                }
                return;
            }
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.ChangeMyBaseInfo)
            {
                ChangeUserBaseInfoContract contract = CompactPropertySerializer.Default.Deserialize<ChangeUserBaseInfoContract>(info, 0);
                this.resourceCenter.ClientGlobalCache.UpdateUserBaseInfo(contract.UserID, contract.Name, contract.Signature, contract.OrgID, contract.UserLatestVersion);
                return;
            }
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.ChangeMyHeadImage)
            {
                ChangeHeadImageContract contract = CompactPropertySerializer.Default.Deserialize<ChangeHeadImageContract>(info, 0);
                this.resourceCenter.ClientGlobalCache.UpdateUserHeadImage(contract.UserID, contract.HeadImageIndex, contract.HeadImage, contract.UserLatestVersion);
                return;
            }
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.MoveFriendToOtherCatalog)
            {
                MoveFriendToOtherCatalogContract contract = CompactPropertySerializer.Default.Deserialize<MoveFriendToOtherCatalogContract>(info, 0);
                this.resourceCenter.ClientGlobalCache.CurrentUser.MoveFriend(contract.FriendID, contract.OldCatalog, contract.NewCatalog);
                this.resourceCenter.ClientGlobalCache.OnFriendCatalogChanged(contract.FriendID, contract.OldCatalog, contract.NewCatalog);
                return;
            }
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.AddFriendCatalog)
            {
                string catalog = System.Text.Encoding.UTF8.GetString(info);
                this.resourceCenter.ClientGlobalCache.CurrentUser.AddFriendCatalog(catalog);
                this.resourceCenter.ClientGlobalCache.OnAddFriendCatalog(catalog);
                return;
            }

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.AudioMessage)
            {
                AudioMessage audioMessage = CompactPropertySerializer.Default.Deserialize<AudioMessage>(info, 0);
                this.brige4ClientOutter.OnAudioMessageEchoReceived(clientType, audioMessage, destUserID);
                return;
            }
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.HandleAddFriendRequest)
            {
                HandleAddFriendRequestContract contract = CompactPropertySerializer.Default.Deserialize<HandleAddFriendRequestContract>(info, 0);
                this.brige4ClientOutter.OnHandleAddFriendRequestReceived(contract.RequesterID,true, contract.IsAgreed);
                return;
            }
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.HandleAddGroupRequest)
            {
                HandleAddGroupRequestContract contract = CompactPropertySerializer.Default.Deserialize<HandleAddGroupRequestContract>(info, 0);
                this.brige4ClientOutter.OnHandleAddGroupRequestReceived(contract.RequesterID,contract.GroupID, contract.IsAgreed);
                return;
            }
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.SnapchatRead)
            {
                SnapchatMessageRead snapchatMessageRead = CompactPropertySerializer.Default.Deserialize<SnapchatMessageRead>(info, 0);
                this.brige4ClientOutter.OnSnapchatReadReceived(snapchatMessageRead.SourceCreatorID, snapchatMessageRead);
                return;
            }
            #region Group
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.QuitGroup)
            {
                string groupID = Encoding.UTF8.GetString(info);

                this.resourceCenter.ClientGlobalCache.OnGroupDeleted(groupID, destUserID);
                return;
            }

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.DeleteGroup)
            {
                string groupID = Encoding.UTF8.GetString(info);
                this.resourceCenter.ClientGlobalCache.OnGroupDeleted(groupID, destUserID);
                return;
            }
            #endregion

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.RemoveFriend)
            {
                string friendID = System.Text.Encoding.UTF8.GetString(info);
                this.resourceCenter.ClientGlobalCache.RemovedFriend(friendID);
                this.resourceCenter.ClientGlobalCache.CurrentUser.RemoveFriend(friendID);
                return;
            }
        }

        #region rapidPassiveEngine_MessageReceived
        void rapidPassiveEngine_MessageReceived(string sourceUserID, ClientType clientType, int informationType, byte[] info, string tag)
        {
            //if (TalkBase.Core.Global.IsExpired())
            //{
            //    return;
            //}
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.Vibration)
            {
                sourceUserID = tag;
                this.brige4ClientOutter.OnVibrationReceived(sourceUserID);
                return;
            }

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.Chat)
            {
                sourceUserID = tag;
                if (this.twinkleNotifyIcon != null && sourceUserID != this.resourceCenter.CurrentUserID) //并且不是文件传输助手消息
                {
                    this.resourceCenter.ClientGlobalCache.ChangeUserStatus(sourceUserID, UserStatus.Online);
                    this.twinkleNotifyIcon.PushFriendMessage(sourceUserID,clientType, informationType, info);
                }
                else
                {
                    this.brige4ClientOutter.OnChatMessageReceived(sourceUserID, clientType, info, null);
                }
                return;
            }

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.AudioMessage)
            {
                AudioMessage msg = CompactPropertySerializer.Default.Deserialize<AudioMessage>(info, 0);
                msg.CreatorID = MSideHelper.GetUserID(msg.CreatorID);
                if (this.twinkleNotifyIcon != null)
                {
                    info = CompactPropertySerializer.Default.Serialize(msg);
                    this.twinkleNotifyIcon.PushFriendMessage(sourceUserID, ClientType.DotNET, this.resourceCenter.TalkBaseInfoTypes.AudioMessage, info);
                }
                else
                {                    
                    this.brige4ClientOutter.OnAudioMessageReceived(msg, null);
                }
                return;
            }

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.UserHeadImageChanged)
            {
                ChangeHeadImageContract contract = CompactPropertySerializer.Default.Deserialize<ChangeHeadImageContract>(info, 0);
                this.resourceCenter.ClientGlobalCache.UpdateUserHeadImage(contract.UserID, contract.HeadImageIndex, contract.HeadImage, contract.UserLatestVersion);
                return;
            }

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.UserBusinessInfoChanged)
            {
                UserBusinessInfoChangedNotifyContract contract = CompactPropertySerializer.Default.Deserialize<UserBusinessInfoChangedNotifyContract>(info, 0);
                this.resourceCenter.ClientGlobalCache.UpdateUserBusinessInfo(contract.UserID, contract.BusinessInfo, contract.LatestVersion);
                return;
            }


            if (informationType == this.resourceCenter.TalkBaseInfoTypes.SystemMessage) //目前 系统消息不闪图标
            {
                SystemMessageContract contract = CompactPropertySerializer.Default.Deserialize<SystemMessageContract>(info, 0);
                this.brige4ClientOutter.OnSystemMessageReceived(contract, null);
                return;

            }

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.MediaCommunicate)
            {
                sourceUserID = string.IsNullOrEmpty(sourceUserID) ? tag : sourceUserID;
                this.HandleInformation(sourceUserID, clientType, informationType, info);
                return;
            }

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.RequestAddFriend)
            {
                sourceUserID = tag;
                if (this.twinkleNotifyIcon != null) 
                {                    
                    this.twinkleNotifyIcon.PushNotifyMessage(sourceUserID, informationType, info);
                }
                else
                {
                    RequestAddFriendContract contract = CompactPropertySerializer.Default.Deserialize<RequestAddFriendContract>(info, 0);
                    this.brige4ClientOutter.OnAddFriendRequestReceived(contract.RequesterID, contract.Comment);
                }
                return;
            }
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.HandleAddFriendRequest)
            {
                HandleAddFriendRequestContract contract = CompactPropertySerializer.Default.Deserialize<HandleAddFriendRequestContract>(info, 0);
                //若是已同意，在之前已发送了FriendAddedNotify 的消息，此时不发送通知消息
                if (contract.IsAgreed)
                {
                    return;
                }
                sourceUserID = tag;
                if (this.twinkleNotifyIcon != null)
                {
                    this.twinkleNotifyIcon.PushNotifyMessage(sourceUserID, informationType, info);
                }
                else
                {                   
                    this.brige4ClientOutter.OnHandleAddFriendRequestReceived(contract.AccepterID,false, contract.IsAgreed);
                } 
                return;
            }
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.GetAddFriendPage)
            {
                AddFriendRequestPage page = CompactPropertySerializer.Default.Deserialize<AddFriendRequestPage>(info, 0);
                this.brige4ClientOutter.OnAddFriendRequestPageReceived(page);
                return;
            }
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.FriendAddedNotify)
            {
                TUser owner = CompactPropertySerializer.Default.Deserialize<TUser>(info, 0); // 0922
                string catalog = string.IsNullOrEmpty(tag)? FunctionOptions.DefaultFriendCatalog: tag;
                this.resourceCenter.ClientGlobalCache.CurrentUser.AddFriend(owner.ID, catalog);
                this.resourceCenter.ClientGlobalCache.OnFriendAdded(owner);

                if (!this.silenceOnFriendAdded)
                {
                    if (this.twinkleNotifyIcon != null)
                    {
                        this.twinkleNotifyIcon.PushFriendMessage(owner.ID, clientType, informationType, info);
                    }
                    else
                    {
                        this.brige4ClientOutter.OnFriendAdded(owner.ID);
                    }
                }
                return;
            }
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.RequestAddGroup)
            {
                sourceUserID = tag;
                if (this.twinkleNotifyIcon != null)
                {
                    this.twinkleNotifyIcon.PushNotifyMessage(sourceUserID, informationType, info);
                }
                else
                {
                    RequestAddGroupContract contract = CompactPropertySerializer.Default.Deserialize<RequestAddGroupContract>(info, 0);
                    this.brige4ClientOutter.OnAddGroupRequestReceived(contract.RequesterID,contract.GroupID, contract.Comment);
                }
                return;
            }
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.HandleAddGroupRequest)
            {
                HandleAddGroupRequestContract contract = CompactPropertySerializer.Default.Deserialize<HandleAddGroupRequestContract>(info, 0);
                //若是已同意，在之前已发送了ChangeGroupMembers 的消息，此时不发送通知消息
                if (contract.IsAgreed)
                {
                    return;
                }
                sourceUserID = tag;
                if (this.twinkleNotifyIcon != null)
                {
                    this.twinkleNotifyIcon.PushNotifyMessage(sourceUserID, informationType, info);
                }
                else
                {
                    this.brige4ClientOutter.OnHandleAddGroupRequestReceived(contract.RequesterID, contract.GroupID, contract.IsAgreed);
                }

                return;
            }
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.GetAddGroupPage)
            {
                AddGroupRequestPage page = CompactPropertySerializer.Default.Deserialize<AddGroupRequestPage>(info, 0);
                this.brige4ClientOutter.OnAddGroupRequestPageReceived(page);
                return;
            }
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.GroupBan4User)
            {
                GroupBan4UserContract contract = CompactPropertySerializer.Default.Deserialize<GroupBan4UserContract>(info, 0);
                this.brige4ClientOutter.OnGroupBan4UserReceived(contract.OperatorID, contract.GroupID, contract.Minutes);
                return;
            }
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.RemoveGroupBan4User)
            {
                RemoveGroupBan4UserContract contract = CompactPropertySerializer.Default.Deserialize<RemoveGroupBan4UserContract>(info, 0);
                this.brige4ClientOutter.OnRemoveGroupBan4UserReceived(contract.GroupID);
                return;
            }
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.Snapchat)
            {
                sourceUserID = tag;
                if (this.twinkleNotifyIcon != null)
                {
                    this.twinkleNotifyIcon.PushFriendMessage(sourceUserID, clientType, informationType, info);
                }
                else
                {
                    this.brige4ClientOutter.OnSnapchatMessageReceived(sourceUserID, clientType, info, null);
                }
                return;
            }
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.SnapchatRead)
            {
                SnapchatMessageRead snapchatMessageRead = CompactPropertySerializer.Default.Deserialize<SnapchatMessageRead>(info, 0);
                this.brige4ClientOutter.OnSnapchatReadReceived(sourceUserID, snapchatMessageRead);
                return;
            }
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.UserStateChanged)
            {
                UserState userState = (UserState)int.Parse(tag);
                this.resourceCenter.ClientGlobalCache.CurrentUser.UserState= userState;
                return;
            }
        }      
        #endregion

        #region HandleInformation
        public void HandleInformation(string sourceUserID, ClientType clientType, int informationType, byte[] info)
        {
            //if (TalkBase.Core.Global.IsExpired())
            //{
            //    return;
            //}

            #region 需要Push的消息
            if (informationType == this.resourceCenter.TalkBaseInfoTypes.MediaCommunicate)
            {
                MediaCommunicateContract contract = CompactPropertySerializer.Default.Deserialize<MediaCommunicateContract>(info, 0);
                if (contract.DestClientType != -1 && (ClientType)contract.DestClientType != this.resourceCenter.CurrentClientType)
                {
                    return;
                }
                if (contract.CommunicateMediaType == CommunicateMediaType.GroupVideo)
                {
                    if (contract.CommunicateType != CommunicateType.Agree)
                    {
                        this.brige4ClientOutter.OnMediaCommunicateReceived(sourceUserID, clientType ,contract.CommunicateMediaType, contract.CommunicateType, contract.Tag);
                    }

                    return;
                }
                //麦克风正在用，直接回复拒绝
                if (MultimediaManagerFactory.GetSingleton().DeviceIsWorking(OMCS.MultimediaDeviceType.Microphone)&& contract.CommunicateType==CommunicateType.Request)
                {
                    this.resourceCenter.ClientOutter.SendMediaCommunicate(sourceUserID, contract.CommunicateMediaType, CommunicateType.Busy, "", clientType);
                    return;
                }
                if (this.twinkleNotifyIcon != null)
                {
                    this.twinkleNotifyIcon.PushFriendMessage(sourceUserID, clientType, informationType, info);
                }
                else
                {                   
                    this.brige4ClientOutter.OnMediaCommunicateReceived(sourceUserID, clientType ,contract.CommunicateMediaType, contract.CommunicateType, contract.Tag);
                }
                return;
            }



            if (informationType == this.resourceCenter.TalkBaseInfoTypes.OfflineMessage)//只处理离线的 好友聊天消息、群聊天消息、Extra消息、系统消息
            {
                OfflineMessage msg = CompactPropertySerializer.Default.Deserialize<OfflineMessage>(info, 0);

                if (msg.InformationType == this.resourceCenter.TalkBaseInfoTypes.Chat || msg.InformationType == this.resourceCenter.TalkBaseInfoTypes.AudioMessage) 
                {                   
                    if (this.twinkleNotifyIcon != null)
                    {
                        this.twinkleNotifyIcon.PushFriendMessage(msg.SourceUserID, clientType,  informationType, info);
                    }
                    else
                    {
                        this.brige4ClientOutter.OnChatMessageReceived(msg.SourceUserID, ClientType.DotNET, msg.Information, msg.TimeTransfer);
                    }
                    return;
                }

                if (msg.InformationType == this.resourceCenter.TalkBaseInfoTypes.Snapchat)
                {
                    if (this.twinkleNotifyIcon != null)
                    {
                        this.twinkleNotifyIcon.PushFriendMessage(msg.SourceUserID, clientType, informationType, info);
                    }
                    else
                    {
                        this.brige4ClientOutter.OnSnapchatMessageReceived(msg.SourceUserID, clientType, msg.Information, msg.TimeTransfer);
                    }
                    return;
                }

                if (msg.InformationType == this.resourceCenter.TalkBaseInfoTypes.GroupChat) //离线群消息
                {                   
                    if (this.twinkleNotifyIcon != null)
                    {
                        this.twinkleNotifyIcon.PushGroupMessage(msg.SourceUserID, msg.GroupID, informationType, info, msg.Tag);
                    }
                    else
                    {
                        this.brige4ClientOutter.OnGroupChatMessageReceived(msg.SourceUserID, msg.GroupID, msg.Information,msg.Tag, msg.TimeTransfer);
                    }
                    return;
                }


                if (msg.InformationType == this.resourceCenter.TalkBaseInfoTypes.SystemMessage)//系统消息不闪图标
                {
                    SystemMessageContract contract = CompactPropertySerializer.Default.Deserialize<SystemMessageContract>(msg.Information, 0);
                    this.brige4ClientOutter.OnSystemMessageReceived(contract ,msg.TimeTransfer);
                    return;
                }

               
            }

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.GroupOfflineMessage)
            {
                GroupOfflineMessage offlineMessage = CompactPropertySerializer.Default.Deserialize<GroupOfflineMessage>(info, 0);
                foreach (OfflineMessage msg in offlineMessage.OfflineMessages)
                {
                    if (this.twinkleNotifyIcon != null)
                    {
                        byte[] data= CompactPropertySerializer.Default.Serialize<OfflineMessage>(msg);
                        this.twinkleNotifyIcon.PushGroupMessage(msg.SourceUserID, msg.GroupID, this.resourceCenter.TalkBaseInfoTypes.OfflineMessage, data, msg.TimeTransfer.ToString());
                    }
                    else
                    {
                        this.brige4ClientOutter.OnGroupChatMessageReceived(msg.SourceUserID, msg.GroupID, msg.Information, msg.Tag, msg.TimeTransfer);
                    }
                }
                return;                
            }

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.OfflineFileResultNotify)
            {
                OfflineFileResultNotifyContract contract = CompactPropertySerializer.Default.Deserialize<OfflineFileResultNotifyContract>(info, 0);
                if (contract.AccepterID == this.resourceCenter.CurrentUserID)
                {
                    return;
                }

                if (this.twinkleNotifyIcon != null)
                {
                    this.twinkleNotifyIcon.PushFriendMessage(contract.AccepterID, clientType, informationType, info);
                }
                else
                {                    
                    this.brige4ClientOutter.OnOfflineFileResultReceived(contract.AccepterID, contract.FileName, contract.Accept);
                }
                return;
            }            
            #endregion

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.SomeoneRegisteredNotify)
            {
                TUser user = ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<TUser>(info, 0);
                this.resourceCenter.ClientGlobalCache.OnSomeoneRegistered(user);
                return;
            }

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.SomeoneDeletedNotify)
            {
                OperateContract contract = CompactPropertySerializer.Default.Deserialize<OperateContract>(info, 0);
                this.resourceCenter.ClientGlobalCache.OnUserDeleted(contract.TargetUserID, contract.OperatorUserID);
                return;
            }

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.InputingNotify)
            {
                this.brige4ClientOutter.OnInptingNotifyReceived(sourceUserID);
                return;
            } 

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.FriendRemovedNotify)
            {
                string friendID = System.Text.Encoding.UTF8.GetString(info);
                this.resourceCenter.ClientGlobalCache.RemovedFriend(friendID);
                this.resourceCenter.ClientGlobalCache.CurrentUser.RemoveFriend(friendID);
                return;
            }

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.UserBaseInfoChanged)
            {
                ChangeUserBaseInfoContract contract = CompactPropertySerializer.Default.Deserialize<ChangeUserBaseInfoContract>(info, 0);
                this.resourceCenter.ClientGlobalCache.UpdateUserBaseInfo(contract.UserID,contract.Name,contract.Signature,contract.OrgID ,contract.UserLatestVersion);
                return;
            }           

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.UserStatusChanged)
            {
                UserStatusChangedContract contract = ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<UserStatusChangedContract>(info, 0);
                this.resourceCenter.ClientGlobalCache.ChangeUserStatus(contract.UserID, (UserStatus)contract.NewStatus);
                return;
            }

            if (informationType == this.resourceCenter.TalkBaseInfoTypes.Vibration)
            {
                this.brige4ClientOutter.OnVibrationReceived(sourceUserID);
                return;
            }
        } 
        #endregion        

        public byte[] HandleQuery(string sourceUserID, ClientType clientType, int informationType, byte[] info)
        {
            return null;
        }

        public bool CanHandle(int informationType)
        {
            return this.resourceCenter.TalkBaseInfoTypes.Contains(informationType);
        }

        #region ContactsOutter_BroadcastReceived
        void ContactsOutter_BroadcastReceived(string broadcasterID, ClientType broadcasterClientType, string groupID, int broadcastType, byte[] content, string tag)
        {
            if (broadcasterID == this.resourceCenter.CurrentUserID && broadcasterClientType == this.resourceCenter.CurrentClientType)
            {
                return;
            }

            if (broadcastType == this.resourceCenter.TalkBaseInfoTypes.GroupChat)
            {
                if(broadcasterID == null)
                {
                    broadcasterID = tag;
                }

                if (this.twinkleNotifyIcon != null)
                {
                    this.twinkleNotifyIcon.PushGroupMessage(broadcasterID, groupID, broadcastType, content, tag);
                }
                else
                {
                    this.brige4ClientOutter.OnGroupChatMessageReceived(broadcasterID, groupID, content, tag, null);
                }

                return;
            }
            if (broadcastType == this.resourceCenter.TalkBaseInfoTypes.GroupFileUploadedNotify)
            {
                if (this.twinkleNotifyIcon != null)
                {
                    this.twinkleNotifyIcon.PushGroupMessage(broadcasterID, groupID, broadcastType, content, null);
                }
                else
                {
                    string fileName = System.Text.Encoding.UTF8.GetString(content);
                    this.brige4ClientOutter.OnGroupFileUploadedNotifyReceived(broadcasterID, groupID, fileName);
                }

                return;
            }
            if (broadcastType == this.resourceCenter.TalkBaseInfoTypes.SomeoneJoinGroup)
            {
                string userID = System.Text.Encoding.UTF8.GetString(content);
                this.resourceCenter.ClientGlobalCache.OnSomeoneJoinGroup(groupID, userID);
                return;
            }

            if (broadcastType == this.resourceCenter.TalkBaseInfoTypes.SomeoneQuitGroup)
            {
                string userID = System.Text.Encoding.UTF8.GetString(content);
                this.resourceCenter.ClientGlobalCache.OnSomeoneQuitGroup(groupID, userID);
                return;
            }

            if (broadcastType == this.resourceCenter.TalkBaseInfoTypes.GroupDeleted)
            {
                string operatorID = null;
                if (content != null)
                {
                    operatorID = System.Text.Encoding.UTF8.GetString(content);
                }
                this.resourceCenter.ClientGlobalCache.OnGroupDeleted(groupID, operatorID);
                return;
            }

            if (broadcastType == this.resourceCenter.TalkBaseInfoTypes.GroupInfoChanged)
            {
                ChangeGroupInfoContract contract = CompactPropertySerializer.Default.Deserialize<ChangeGroupInfoContract>(content, 0);
                TGroup group = this.resourceCenter.ClientGlobalCache.GetGroup(groupID);
                if (group != null)
                {
                    group.Announce = contract.Announce;
                    group.Name = contract.GroupName;
                    this.resourceCenter.ClientGlobalCache.OnGroupInfoChanged(groupID, contract.OperatorID);
                }
                return;
            }

            if (broadcastType == this.resourceCenter.TalkBaseInfoTypes.PulledIntoGroupNotify)
            {
                ManageGroupMembersNotifyContract contract = CompactPropertySerializer.Default.Deserialize<ManageGroupMembersNotifyContract>(content, 0);
                if (contract.OperatorID == this.resourceCenter.CurrentUserID && !contract.ServerCommand)
                {
                    return;
                }
                this.resourceCenter.ClientGlobalCache.OnUsersPulledIntoGroup(groupID, contract.OperatorID, contract.GuestList);
                return;
            }

            if (broadcastType == this.resourceCenter.TalkBaseInfoTypes.RemovedFromGroupNotify)
            {
                ManageGroupMembersNotifyContract contract = CompactPropertySerializer.Default.Deserialize<ManageGroupMembersNotifyContract>(content, 0);
                if (contract.OperatorID == this.resourceCenter.CurrentUserID && !contract.ServerCommand)
                {
                    return;
                }
                this.resourceCenter.ClientGlobalCache.OnUsersRemovedFromGroup(groupID, contract.OperatorID, contract.GuestList);
                return;
            }
            if (broadcastType == this.resourceCenter.TalkBaseInfoTypes.GroupBan4Group)
            {
                this.brige4ClientOutter.OnGroupBan4GroupReceived(tag, groupID);
                return;
            }
            if (broadcastType == this.resourceCenter.TalkBaseInfoTypes.RemoveGroupBan4Group)
            {
                this.brige4ClientOutter.OnRemoveGroupBan4GroupReceived(groupID);
                return;
            }
        } 
        #endregion
    }
}
