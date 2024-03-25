using System;
using ESBasic;
using ESFramework;
using ESPlus.Serialization;
using TalkBase.Client;
using GGTalk.Linux.Views;
using TalkBase;
using GGTalk.Linux.Helpers;
using CPF.Controls;

namespace GGTalk.Linux.Controller
{
    /// <summary>
    ///将 （ClientOutter、ClientGlobalCache）的聊天消息事件、状态变化事件 传递给（IChatFormController）。
    /// </summary>    
    internal class Event2ChatFormBridge<TUser, TGroup>
        where TUser : class, TalkBase.IUser, new()
        where TGroup : TalkBase.IGroup, new()
    {      
        private ResourceCenter<TUser, TGroup> resourceCenter;

        public Event2ChatFormBridge(ResourceCenter<TUser, TGroup> center)
        {
            this.resourceCenter = center;         
            this.resourceCenter.RapidPassiveEngine.ContactsOutter.ContactsOffline += new CbGeneric<string>(ContactsOutter_ContactsOffline);           
            this.resourceCenter.ClientGlobalCache.MyBaseInfoChanged += new CbGeneric(ClientGlobalCache_MyInfoChanged);
            this.resourceCenter.ClientGlobalCache.UserStatusChanged += new CbGeneric<TUser>(globalUserCache_FriendStatusChanged);
            this.resourceCenter.ClientGlobalCache.UnitCommentNameChanged += new CbGeneric<IUnit>(ClientGlobalCache_UnitCommentNameChanged);
            this.resourceCenter.ClientGlobalCache.UserBaseInfoChanged += new CbGeneric<TUser>(globalUserCache_FriendInfoChanged);
            this.resourceCenter.ClientGlobalCache.FriendRemoved += new CbGeneric<string>(globalUserCache_FriendRemoved);
            this.resourceCenter.ClientGlobalCache.GroupChanged += new CbGeneric<TGroup, GroupChangedType, string ,string>(globalUserCache_GroupChanged);           
            this.resourceCenter.RapidPassiveEngine.ConnectionInterrupted += new CbGeneric(rapidPassiveEngine_ConnectionInterrupted);
            this.resourceCenter.ClientOutter.FriendChatMessageReceived += new CbGeneric<string, ClientType, byte[] ,DateTime?>(clientOutter_ChatMessageReceived);
            this.resourceCenter.ClientOutter.FriendChatMessageEchoReceived += new CbGeneric<ESFramework.ClientType, string, byte[]>(ClientOutter_FriendChatMessageEchoReceived);
            this.resourceCenter.ClientOutter.SnapchatMessageReceived += ClientOutter_SnapchatMessageReceived;
            this.resourceCenter.ClientOutter.SnapchatReadReceived += ClientOutter_SnapchatReadReceived;
           // this.resourceCenter.ClientOutter.FriendAudioMessageReceived += new CbGeneric<AudioMessage, DateTime?>(ClientOutter_FriendAudioMessageReceived);
           // this.resourceCenter.ClientOutter.FriendAudioMessageEchoReceived += new CbGeneric<ClientType, AudioMessage, string>(ClientOutter_FriendAudioMessageEchoReceived);
            this.resourceCenter.ClientOutter.FriendAdded += new CbGeneric<string>(clientOutter_FriendAdded);
            this.resourceCenter.ClientOutter.AddFriendRequestReceived += ClientOutter_AddFriendRequestReceived;
            this.resourceCenter.ClientOutter.AddFriendResponseReceived += ClientOutter_AddFriendResponseReceived;
            this.resourceCenter.ClientOutter.AddFriendRequestPageReceived += ClientOutter_AddFriendRequestPageReceived;
            this.resourceCenter.ClientOutter.AddGroupRequestReceived += ClientOutter_AddGroupRequestReceived;
            this.resourceCenter.ClientOutter.AddGroupResponseReceived += ClientOutter_AddGroupResponseReceived;
            this.resourceCenter.ClientOutter.AddGroupRequestPageReceived += ClientOutter_AddGroupRequestPageReceived;
            this.resourceCenter.ClientOutter.GroupBan4UserReceived += ClientOutter_GroupBan4UserReceived;
            this.resourceCenter.ClientOutter.RemoveGroupBan4UserReceived += ClientOutter_RemoveGroupBan4UserReceived;
            this.resourceCenter.ClientOutter.GroupBan4GroupReceived += ClientOutter_GroupBan4GroupReceived;
            this.resourceCenter.ClientOutter.RemoveGroupBan4GroupReceived += ClientOutter_RemoveGroupBan4GroupReceived;
            this.resourceCenter.ClientOutter.GroupChatMessageReceived += new CbGeneric<string, string, byte[] ,string,DateTime?>(clientOutter_GroupChatMessageReceived);
            this.resourceCenter.ClientOutter.GroupFileUploadedNotifyReceived += new CbGeneric<string, string, string>(ClientOutter_GroupFileUploadedNotifyReceived);
            this.resourceCenter.ClientOutter.InptingNotifyReceived += new CbGeneric<string>(clientOutter_InptingNotifyReceived);
            this.resourceCenter.ClientOutter.VibrationReceived += new CbGeneric<string>(ClientOutter_VibrationReceived);
            this.resourceCenter.ClientOutter.MediaCommunicateReceived += new CbGeneric<string, ClientType, CommunicateMediaType, CommunicateType, string>(clientOutter_MediaCommunicateReceived);
            this.resourceCenter.ClientOutter.MediaCommunicateAnswerOnOtherDevice += new CbGeneric<string, ClientType, CommunicateMediaType, bool>(ClientOutter_MediaCommunicateAnswerOnOtherDevice);
            this.resourceCenter.ClientOutter.OfflineFileResultReceived += new CbGeneric<string, string, bool>(clientOutter_OfflineFileResultReceived);
        }

        private void ClientOutter_GroupBan4GroupReceived(string operatorID, string groupID)
        {
            UIThreadPoster<string, string> poster = new UIThreadPoster<string, string>(this.do_ClientOutter_GroupBan4GroupReceived, operatorID, groupID);
            poster.Post();            
        }
        private void do_ClientOutter_GroupBan4GroupReceived(string operatorID, string groupID)
        {
            IGroupChatForm<TUser> groupChatForm = (IGroupChatForm<TUser>)MainWindow.ChatFormController.GetExistedForm(groupID);
            if (groupChatForm != null)
            {
                groupChatForm.HandleAllGroupBan();
            }
        }

        private void ClientOutter_RemoveGroupBan4GroupReceived(string groupID)
        {
            UiSafeInvoker.ActionOnUI<string>(this.do_ClientOutter_RemoveGroupBan4GroupReceived, groupID);
        }

        private void do_ClientOutter_RemoveGroupBan4GroupReceived(string groupID)
        {
            IGroupChatForm<TUser> groupChatForm = (IGroupChatForm<TUser>)MainWindow.ChatFormController.GetExistedForm(groupID);
            if (groupChatForm != null)
            {
                groupChatForm.HandleRemoveAllGroupBan();
            }
        }

        private void ClientOutter_RemoveGroupBan4UserReceived(string groupID)
        {
            UiSafeInvoker.ActionOnUI<string>(this.do_ClientOutter_RemoveGroupBan4UserReceived, groupID);
        }

        private void do_ClientOutter_RemoveGroupBan4UserReceived(string groupID)
        {
            IGroupChatForm<TUser> groupChatForm = (IGroupChatForm<TUser>)MainWindow.ChatFormController.GetExistedForm(groupID);
            if (groupChatForm != null)
            {
                groupChatForm.HandleRemoveGroupBanNotify();
            }
        }

        private void ClientOutter_GroupBan4UserReceived(string operatorID, string groupID, double minutes)
        {
            UiSafeInvoker.ActionOnUI<string,string,double>(this.do_ClientOutter_GroupBan4UserReceived, operatorID, groupID, minutes);
        }

        private void do_ClientOutter_GroupBan4UserReceived(string operatorID, string groupID, double minutes)
        {
            IGroupChatForm<TUser> groupChatForm = (IGroupChatForm<TUser>)MainWindow.ChatFormController.GetExistedForm(groupID);
            if (groupChatForm != null)
            {
                groupChatForm.HandleGroupBanNotify(operatorID,minutes);
            }
        }

        private void ClientOutter_AddGroupRequestPageReceived(AddGroupRequestPage page)
        {
            UiSafeInvoker.ActionOnUI<AddGroupRequestPage>(this.do_ClientOutter_AddGroupRequestPageReceived, page);
        }
        private void do_ClientOutter_AddGroupRequestPageReceived(AddGroupRequestPage page)
        {
            INotifyForm notifyForm = (INotifyForm)MainWindow.ChatFormController.GetNotifyForm();
            notifyForm.OnAddGroupRequestPageReceived(page);
        }

        private void ClientOutter_AddGroupResponseReceived(string requesterID, string groupID, bool isAgreed)
        {
            UiSafeInvoker.ActionOnUI<string,string, bool>(this.do_ClientOutter_AddGroupResponseReceived, requesterID, groupID, isAgreed);
        }

        private void do_ClientOutter_AddGroupResponseReceived(string requesterID, string groupID, bool isAgreed)
        {
            bool isExistedFrom = MainWindow.ChatFormController.IsExistNotifyForm();
            INotifyForm notifyForm = (INotifyForm)MainWindow.ChatFormController.GetNotifyForm();
            if (isExistedFrom)
            {
                notifyForm.OnHandleAddGroupRequestReceived(requesterID,groupID, isAgreed);
            }
            notifyForm.ShowTabPage(NotifyType.Group);
        }

        private void ClientOutter_AddGroupRequestReceived(string requesterID, string groupID, string comment)
        {
            UiSafeInvoker.ActionOnUI<string, string, string>(this.do_ClientOutter_AddGroupRequestReceived, requesterID, groupID, comment);
        }

        private void do_ClientOutter_AddGroupRequestReceived(string requesterID, string groupID, string comment)
        {
            bool isExistedFrom = MainWindow.ChatFormController.IsExistNotifyForm();
            INotifyForm notifyForm = (INotifyForm)MainWindow.ChatFormController.GetNotifyForm();
            if (isExistedFrom)
            {
                notifyForm.OnAddGroupRequestReceived(requesterID, groupID, comment);
            }
            notifyForm.ShowTabPage(NotifyType.Group);
        }

        private void ClientOutter_AddFriendRequestPageReceived(AddFriendRequestPage page)
        {
            UiSafeInvoker.ActionOnUI<AddFriendRequestPage>(this.do_ClientOutter_AddFriendRequestPageReceived, page);
        }

        private void do_ClientOutter_AddFriendRequestPageReceived(AddFriendRequestPage page)
        {
            INotifyForm notifyForm = (INotifyForm)MainWindow.ChatFormController.GetNotifyForm();
            notifyForm.OnAddFriendRequestPageReceived(page);
        }

        private void ClientOutter_AddFriendResponseReceived(string friendID,bool isRequester, bool isAgreed)
        {
            UiSafeInvoker.ActionOnUI<string,bool, bool>(this.do_ClientOutter_AddFriendResponseReceived, friendID,isRequester, isAgreed);            
        }

        private void do_ClientOutter_AddFriendResponseReceived(string friendID,bool isRequester, bool isAgreed)
        {
            bool isExistedFrom = MainWindow.ChatFormController.IsExistNotifyForm();
            INotifyForm notifyForm = (INotifyForm)MainWindow.ChatFormController.GetNotifyForm();
            if (isExistedFrom)
            {
                notifyForm.OnHandleAddFriendRequestReceived(friendID, isRequester, isAgreed);
            }
            
            notifyForm.ShowTabPage(NotifyType.User);
        }

        private void ClientOutter_AddFriendRequestReceived(string requesterID, string comment)
        {
            UiSafeInvoker.ActionOnUI<string,string>(this.do_ClientOutter_AddFriendRequestReceived,requesterID,comment);
        }

        private void do_ClientOutter_AddFriendRequestReceived(string requesterID, string comment)
        {
            bool isExistedFrom = MainWindow.ChatFormController.IsExistNotifyForm();
            INotifyForm notifyForm = (INotifyForm)MainWindow.ChatFormController.GetNotifyForm();
            if (isExistedFrom)
            {
               notifyForm.OnAddFriendRequestReceived(requesterID, comment);
            }
            notifyForm.ShowTabPage(NotifyType.User);
        }

        void ClientOutter_VibrationReceived(string userID)
        {
            UiSafeInvoker.ActionOnUI<string>(this.do_ClientOutter_VibrationReceived ,userID);
        }

        void do_ClientOutter_VibrationReceived(string userID)
        {
            IFriendChatForm<TUser> form = (IFriendChatForm<TUser>)MainWindow.ChatFormController.GetForm(userID);
            if (form != null)
            {
                form.HandleVibration();
            }
        }

        #region ClientGlobalCache_UnitCommentNameChanged
        void ClientGlobalCache_UnitCommentNameChanged(IUnit unit)
        {
            UiSafeInvoker.ActionOnUI<IUnit>(this.do_ClientGlobalCache_UnitCommentNameChanged, unit);
        }

        void do_ClientGlobalCache_UnitCommentNameChanged(IUnit unit)
        {
            if(unit.UnitType == UnitType.Group)
            {
                IGroupChatForm<TUser> groupChatForm = (IGroupChatForm<TUser>)MainWindow.ChatFormController.GetExistedForm(unit.ID);
                if (groupChatForm != null)
                {
                    groupChatForm.UnitCommentNameChanged(unit);
                }
                return;
            }

            IFriendChatForm<TUser> chatForm = (IFriendChatForm<TUser>)MainWindow.ChatFormController.GetExistedForm(unit.ID);
            if (chatForm != null)
            {
                chatForm.UnitCommentNameChanged(unit);
            }

            foreach (Window form in MainWindow.ChatFormController.GetAllForms())
            {
                IGroupChatForm<TUser> groupChatForm = form as IGroupChatForm<TUser>;
                if (groupChatForm != null)
                {
                    groupChatForm.UnitCommentNameChanged(unit);
                }
            }

        }
        #endregion 

        void ContactsOutter_ContactsOffline(string userID)
        {
            this.resourceCenter.ClientGlobalCache.ChangeUserStatus(userID, UserStatus.OffLine);    
        }

        void ClientGlobalCache_MyInfoChanged()
        {
            UiSafeInvoker.ActionOnUI(this.do_ClientGlobalCache_MyInfoChanged);
        }

        void do_ClientGlobalCache_MyInfoChanged()
        {
            foreach (IChatForm<TUser> chatForm in MainWindow.ChatFormController.GetAllForms())
            {
                chatForm.MyInfoChanged(this.resourceCenter.ClientGlobalCache.CurrentUser);
            }
        }

        void globalUserCache_GroupChanged(TGroup group, GroupChangedType type,string operatorID, string userID)
        {
            UiSafeInvoker.ActionOnUI<TGroup, GroupChangedType, string, string>(this.do_globalUserCache_GroupChanged, group, type , operatorID, userID);
        }

        void do_globalUserCache_GroupChanged(TGroup group, GroupChangedType type, string operatorID, string userID)
        {
            if (type == GroupChangedType.MyselfBePulledIntoGroup)
            {
                IGroupChatForm<TUser> form = (IGroupChatForm<TUser>)MainWindow.ChatFormController.GetForm(group.ID);
                form.HandleBePulledIntoGroupNotify(operatorID);
                return;
            }

            IGroupChatForm<TUser> groupChatForm = (IGroupChatForm<TUser>)MainWindow.ChatFormController.GetExistedForm(group.ID);
            if (groupChatForm != null)
            {
                groupChatForm.OnGroupChanged(type, operatorID, userID);
            }
        }

        /// <summary>
        /// 删除好友，或被别人从好友中删除
        /// </summary>       
        void globalUserCache_FriendRemoved(string friendID)
        {
            UiSafeInvoker.ActionOnUI<string>(this.do_globalUserCache_FriendRemoved, friendID);
        }

        private void do_globalUserCache_FriendRemoved(string friendID)
        {
            IFriendChatForm<TUser> chatForm = (IFriendChatForm<TUser>)MainWindow.ChatFormController.GetExistedForm(friendID);
            if (chatForm != null)
            {
                chatForm.OnRemovedFromFriend();
            }
        }

        void clientOutter_FriendAdded(string userID)
        {
            UiSafeInvoker.ActionOnUI<string>(this.do_clientOutter_FriendAdded, userID);
        }

        void do_clientOutter_FriendAdded(string userID)
        {
            IFriendChatForm<TUser> form = (IFriendChatForm<TUser>)MainWindow.ChatFormController.GetForm(userID);
            form.HandleFriendAddedNotify();
        }

        void clientOutter_ChatMessageReceived(string userID, ClientType sourceType, byte[] info, DateTime? offlineMsgOccurTime)
        {
            UiSafeInvoker.ActionOnUI<string,ClientType, byte[], DateTime?>(this.do_clientOutter_ChatMessageReceived, userID, sourceType ,info, offlineMsgOccurTime);
        }

        void do_clientOutter_ChatMessageReceived(string userID, ClientType sourceType, byte[] info, DateTime? offlineMsgOccurTime)
        {
            if(userID == this.resourceCenter.CurrentUserID) //文件传送助手-自己其它设备发给自己的消息
            {
                if (sourceType != this.resourceCenter.CurrentClientType)
                {
                    IFileAssistantForm fileAssistantForm = (IFileAssistantForm)MainWindow.ChatFormController.GetFileAssistantForm();
                    fileAssistantForm.HandleChatMessage(info, offlineMsgOccurTime);
                }
                return;
            }

            IFriendChatForm<TUser> form = (IFriendChatForm<TUser>)MainWindow.ChatFormController.GetForm(userID);
            form.HandleChatMessage(info, offlineMsgOccurTime);
            MainWindow.ChatFormController.OnNewMessage(userID);
        }

        void ClientOutter_FriendChatMessageEchoReceived(ClientType clientType, string destUserID, byte[] content)
        {
            UiSafeInvoker.ActionOnUI<ClientType, string, byte[]>(this.do_ClientOutter_FriendChatMessageEchoReceived, clientType, destUserID, content);
        }

        void do_ClientOutter_FriendChatMessageEchoReceived(ClientType clientType, string destUserID, byte[] content)
        {
            IFriendChatForm<TUser> form = MainWindow.ChatFormController.GetExistedForm(destUserID) as IFriendChatForm<TUser>;
            if (form != null)
            {
                form.HandleChatMessageOfMine(content);
            }    
        }

        private void ClientOutter_SnapchatMessageReceived(string sourceUserID, ClientType sourceType, byte[] info, DateTime? offlineMsgOccurTime)
        {
            UiSafeInvoker.ActionOnUI<string, ClientType, byte[], DateTime?>(this.do_ClientOutter_SnapchatMessageReceived, sourceUserID, sourceType, info, offlineMsgOccurTime);
        }

        private void do_ClientOutter_SnapchatMessageReceived(string sourceUserID, ClientType sourceType, byte[] info, DateTime? offlineMsgOccurTime)
        {
            IFriendChatForm<TUser> form = MainWindow.ChatFormController.GetForm(sourceUserID) as IFriendChatForm<TUser>;
            SnapchatMessage message = CompactPropertySerializer.Default.Deserialize<SnapchatMessage>(info, 0);
            form.HandleSnapchatMessage(message,offlineMsgOccurTime);            
        }


        private void ClientOutter_SnapchatReadReceived(string sourceUserID, SnapchatMessageRead snapchatMessageRead)
        {
            UiSafeInvoker.ActionOnUI<string, SnapchatMessageRead>(this.do_ClientOutter_SnapchatReadReceived, sourceUserID, snapchatMessageRead);
        }

        private void do_ClientOutter_SnapchatReadReceived(string sourceUserID, SnapchatMessageRead snapchatMessageRead)
        {
            string destUserID = sourceUserID == this.resourceCenter.CurrentUserID ? snapchatMessageRead.SourceCreatorID : sourceUserID;

            IFriendChatForm<TUser> form = MainWindow.ChatFormController.GetExistedForm(destUserID) as IFriendChatForm<TUser>;
            if (form != null)
            {
                form.HandleSnapchatRead(snapchatMessageRead.SourceCreatorID + "-" + snapchatMessageRead.MessageID);
            }
        }

        //void ClientOutter_FriendAudioMessageReceived(AudioMessage msg, DateTime? offlineMsgOccurTime)
        //{
        //    UiSafeInvoker.ActionOnUI<AudioMessage, DateTime?>(this.do_ClientOutter_FriendAudioMessageReceived, msg, offlineMsgOccurTime);
        //}

        //void ClientOutter_FriendAudioMessageEchoReceived(ClientType clientType, AudioMessage audioMessage, string destUserID)
        //{
        //    UiSafeInvoker.ActionOnUI<ClientType, AudioMessage, string>(this.do_ClientOutter_FriendAudioMessageEchoReceived, clientType, audioMessage ,destUserID);
        //}

        //void do_ClientOutter_FriendAudioMessageEchoReceived(ClientType clientType, AudioMessage audioMessage, string destUserID)
        //{
        //    IFriendChatForm<TUser> form = MainWindow.ChatFormController.GetExistedForm(destUserID) as IFriendChatForm<TUser>;
        //    if (form != null)
        //    {
        //        form.HandleAudioMessageOfMine(audioMessage);
        //    }    
        //}

        //void do_ClientOutter_FriendAudioMessageReceived(AudioMessage msg, DateTime? offlineMsgOccurTime)
        //{
        //    IFriendChatForm<TUser> form = (IFriendChatForm<TUser>)MainWindow.ChatFormController.GetForm(msg.CreatorID);
        //    form.HandleAudioMessage(msg, offlineMsgOccurTime);
        //    MainWindow.ChatFormController.OnNewMessage(msg.CreatorID);
        //}      

        void clientOutter_OfflineFileResultReceived(string userID, string fileName, bool accept)
        {
            UiSafeInvoker.ActionOnUI<string, string, bool>(this.do_clientOutter_OfflineFileResultReceived, userID, fileName, accept);
        }

        void do_clientOutter_OfflineFileResultReceived(string userID, string fileName, bool accept)
        {
            IFriendChatForm<TUser> form = (IFriendChatForm<TUser>)MainWindow.ChatFormController.GetForm(userID);
            form.HandleOfflineFileResultReceived(fileName, accept);
        }

        void clientOutter_MediaCommunicateReceived(string userID, ClientType clientType, CommunicateMediaType type, CommunicateType type2, string tag)
        {
            UiSafeInvoker.ActionOnUI<string, ClientType, CommunicateMediaType, CommunicateType, string>(this.do_clientOutter_MediaCommunicateReceived, userID, clientType, type, type2, tag);
        }

        void do_clientOutter_MediaCommunicateReceived(string userID, ClientType clientType, CommunicateMediaType type, CommunicateType type2, string tag)
        {
            //if (type == CommunicateMediaType.GroupVideo)
            //{
            //    Form groupVideoForm = null;
            //    if (CommunicateType.Reject == type2)//收到拒绝，在界面上处理拒绝结果（移除对应正在加入群聊的头像）
            //    {
            //        groupVideoForm = MainWindow.ChatFormController.GetGroupVideoCallForm(tag);//获取是否存在请求页面
            //        if (groupVideoForm == null)
            //        {
            //            groupVideoForm = MainWindow.ChatFormController.GetGroupVideoChatForm(tag);//获取是否存在通话页面
            //            if (groupVideoForm == null)
            //                return;
            //        }
            //        IGroupVideoForm callForm = (IGroupVideoForm)groupVideoForm;
            //        callForm.OnRejectJoinReceived(userID);
            //    }
            //    else if (CommunicateType.Request == type2)
            //    {
            //        string[] temp = tag.Split(FunctionOptions.ColonSeparator);
            //        string videoGroupID = temp[0];
            //        string[] memberIDs = temp[1].Split(FunctionOptions.CommaSeparator);
            //        if (MainWindow.ChatFormController.ExistGroupVideoCallForm())//若已存在请求中的则自动拒绝
            //        {
            //            this.RejectGroupVideo(memberIDs, videoGroupID);
            //            return;
            //        }
            //        //视频正在用，再次收到请求，直接默认拒绝请求
            //        if (MultimediaManagerFactory.GetSingleton().DeviceIsWorking(OMCS.MultimediaDeviceType.Camera))
            //        {
            //            this.RejectGroupVideo(memberIDs, videoGroupID);
            //            return;
            //        }
            //        groupVideoForm = MainWindow.ChatFormController.GetNewGroupVideoCallForm(videoGroupID, userID, new List<string>(memberIDs));
            //        if (groupVideoForm == null) { return; }
            //        groupVideoForm.Show();
            //    }
            //    return;
            //}
            //IFriendChatForm<TUser> form = (IFriendChatForm<TUser>)MainWindow.ChatFormController.GetForm(userID);
            //form.HandleMediaCommunicate(clientType, type, type2, tag);
            //MainWindow.ChatFormController.OnNewMessage(userID);
        }

        /// <summary>
        /// 拒绝群视频
        /// </summary>
        /// <param name="memberIDs"></param>
        /// <param name="videoGroupID"></param>
        private void RejectGroupVideo(string[] memberIDs,string videoGroupID)
        {
            foreach (string memberID in memberIDs)
            {
                if (memberID == this.resourceCenter.CurrentUserID) { continue; }
                this.resourceCenter.ClientOutter.SendMediaCommunicate(memberID, CommunicateMediaType.GroupVideo, CommunicateType.Reject, videoGroupID, null);
            }
        }

        void ClientOutter_MediaCommunicateAnswerOnOtherDevice(string friendID, ClientType answerType, CommunicateMediaType type, bool agree)
        {
            UiSafeInvoker.ActionOnUI<string, ClientType, CommunicateMediaType, bool>(this.do_ClientOutter_MediaCommunicateAnswerOnOtherDevice, friendID, answerType, type, agree);
        
        }

        void do_ClientOutter_MediaCommunicateAnswerOnOtherDevice(string friendID, ClientType answerType, CommunicateMediaType type, bool agree)
        {
            //friendID 在群视频聊天中为 videoGroupID
            IGroupVideoForm form1 = (IGroupVideoForm)MainWindow.ChatFormController.GetGroupVideoCallForm(friendID);
            if (form1 != null)
            {
                form1.OnGroupVideoAnswerOnOtherDevice();
                return;
            }
            IFriendChatForm<TUser> form = (IFriendChatForm<TUser>)MainWindow.ChatFormController.GetExistedForm(friendID);
            if (form == null)
            {
                return;
            }

            form.OnMediaCommunicateAnswerOnOtherDevice(answerType, type, agree);           
        }   


        void clientOutter_InptingNotifyReceived(string userID)
        {
            UiSafeInvoker.ActionOnUI<string>(this.do_clientOutter_InptingNotifyReceived, userID);
        }

        void do_clientOutter_InptingNotifyReceived(string userID)
        {
            IFriendChatForm<TUser> form = (IFriendChatForm<TUser>)MainWindow.ChatFormController.GetExistedForm(userID);
            if (form != null)
            {
                form.HandleInptingNotify();
            }
        }

        void clientOutter_GroupChatMessageReceived(string broadcasterID, string groupID, byte[] content, string tag, DateTime? offlineMsgOccurTime)
        {
            UiSafeInvoker.ActionOnUI<string, string, byte[],string, DateTime?>(this.do_clientOutter_GroupChatMessageReceived, broadcasterID, groupID, content, tag, offlineMsgOccurTime);
        }

        void do_clientOutter_GroupChatMessageReceived(string broadcasterID, string groupID, byte[] content,string tag, DateTime? offlineMsgOccurTime)
        {
            IGroupChatForm<TUser> form = (IGroupChatForm<TUser>)MainWindow.ChatFormController.GetForm(groupID);
            if (offlineMsgOccurTime == null)
            {                  
                form.HandleGroupChatMessage(broadcasterID, content,tag);
            }
            else
            {
                form.HandleOfflineGroupChatMessage(broadcasterID, content , offlineMsgOccurTime.Value, tag);
            }
            MainWindow.ChatFormController.OnNewMessage(groupID);            
        }

        void ClientOutter_GroupFileUploadedNotifyReceived(string sourceUserID, string groupID, string fileName)
        {
            UiSafeInvoker.ActionOnUI<string, string, string>(this.do_ClientOutter_GroupFileUploadedNotifyReceived, sourceUserID, groupID, fileName);
        }

        void do_ClientOutter_GroupFileUploadedNotifyReceived(string sourceUserID, string groupID, string fileName)
        {
            IGroupChatForm<TUser> form = (IGroupChatForm<TUser>)MainWindow.ChatFormController.GetForm(groupID);
            form.HandleGroupFileUploadedNotify(sourceUserID, groupID, fileName);
            MainWindow.ChatFormController.OnNewMessage(groupID);
        }


        void rapidPassiveEngine_ConnectionInterrupted()
        {
            UiSafeInvoker.ActionOnUI(this.do_rapidPassiveEngine_ConnectionInterrupted);
        }

        void do_rapidPassiveEngine_ConnectionInterrupted()
        {
            foreach (IChatForm<TUser> form in MainWindow.ChatFormController.GetAllForms())
            {
                form.MyselfOffline();
            }
        }

        void globalUserCache_FriendInfoChanged(TUser user)
        {
            UiSafeInvoker.ActionOnUI<TUser>(this.do_globalUserCache_FriendInfoChanged, user);
        }

        void do_globalUserCache_FriendInfoChanged(TUser user)
        {
            IFriendChatForm<TUser> chatForm = (IFriendChatForm<TUser>)MainWindow.ChatFormController.GetExistedForm(user.ID);
            if (chatForm != null)
            {
                chatForm.FriendInfoChanged(user);
            }

            foreach (Window form in MainWindow.ChatFormController.GetAllForms())
            {
                IGroupChatForm<TUser> groupChatForm = form as IGroupChatForm<TUser>;
                if (groupChatForm != null)
                {
                    groupChatForm.FriendInfoChanged(user);
                }
            }
        }

        void globalUserCache_FriendStatusChanged(TUser user)
        {
            UiSafeInvoker.ActionOnUI<TUser>(this.do_globalUserCache_FriendStatusChanged, user);
        }

        private void do_globalUserCache_FriendStatusChanged(TUser user)
        {
            foreach (IChatForm<TUser> chatForm in MainWindow.ChatFormController.GetAllForms())
            {
                chatForm.FriendStateChanged(user.ID, user.UserStatus);
            }
        }      
    }    
}
