using System;
using System.Collections.Generic;
using System.Text;

using ESBasic.ObjectManagement.Managers;
using ESBasic;
using ESBasic.Helpers;
using System.Windows.Forms;
using TalkBase.Client.UnitViews;
using OMCS.Passive.ShortMessages;
using ESFramework;
using ESFramework.Boost.Controls;
using ESPlus.Serialization;
using OMCS.Passive;

namespace TalkBase.Client.Bridges
{
    /// <summary>
    ///将 （ClientOutter、ClientGlobalCache）的聊天消息事件、状态变化事件 传递给（IChatFormController）。
    /// </summary>    
    public class Event2ChatFormBridge<TUser, TGroup>
        where TUser : class, TalkBase.IUser, new()
        where TGroup : TalkBase.IGroup, new()
    {      
        private ResourceCenter<TUser, TGroup> resourceCenter;
        private UiSafeInvoker uiSafeInvoker;
        private IChatFormShower formShower;
        public Event2ChatFormBridge(ResourceCenter<TUser, TGroup> center, UiSafeInvoker invoker, IChatFormShower _formShower)
        {
            this.resourceCenter = center;          
            this.uiSafeInvoker = invoker;
            this.formShower = _formShower;
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
            this.resourceCenter.ClientOutter.FriendAudioMessageReceived += new CbGeneric<AudioMessage, DateTime?>(ClientOutter_FriendAudioMessageReceived);
            this.resourceCenter.ClientOutter.FriendAudioMessageEchoReceived += new CbGeneric<ClientType, AudioMessage, string>(ClientOutter_FriendAudioMessageEchoReceived);
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
            this.uiSafeInvoker.ActionOnUI<string,string>(this.do_ClientOutter_GroupBan4GroupReceived,operatorID, groupID);
        }
        private void do_ClientOutter_GroupBan4GroupReceived(string operatorID, string groupID)
        {
            IGroupChatForm groupChatForm = (IGroupChatForm)this.resourceCenter.ChatFormController.GetExistedForm(groupID);
            if (groupChatForm != null)
            {
                groupChatForm.HandleAllGroupBan();
            }
        }

        private void ClientOutter_RemoveGroupBan4GroupReceived(string groupID)
        {
            this.uiSafeInvoker.ActionOnUI<string>(this.do_ClientOutter_RemoveGroupBan4GroupReceived, groupID);
        }

        private void do_ClientOutter_RemoveGroupBan4GroupReceived(string groupID)
        {
            IGroupChatForm groupChatForm = (IGroupChatForm)this.resourceCenter.ChatFormController.GetExistedForm(groupID);
            if (groupChatForm != null)
            {
                groupChatForm.HandleRemoveAllGroupBan();
            }
        }

        private void ClientOutter_RemoveGroupBan4UserReceived(string groupID)
        {
            this.uiSafeInvoker.ActionOnUI<string>(this.do_ClientOutter_RemoveGroupBan4UserReceived, groupID);
        }

        private void do_ClientOutter_RemoveGroupBan4UserReceived(string groupID)
        {
            IGroupChatForm groupChatForm = (IGroupChatForm)this.resourceCenter.ChatFormController.GetExistedForm(groupID);
            if (groupChatForm != null)
            {
                groupChatForm.HandleRemoveGroupBanNotify();
            }
        }

        private void ClientOutter_GroupBan4UserReceived(string operatorID, string groupID, double minutes)
        {
            this.uiSafeInvoker.ActionOnUI<string,string,double>(this.do_ClientOutter_GroupBan4UserReceived, operatorID, groupID, minutes);
        }

        private void do_ClientOutter_GroupBan4UserReceived(string operatorID, string groupID, double minutes)
        {
            IGroupChatForm groupChatForm = (IGroupChatForm)this.resourceCenter.ChatFormController.GetExistedForm(groupID);
            if (groupChatForm != null)
            {
                groupChatForm.HandleGroupBanNotify(operatorID,minutes);
            }
        }

        private void ClientOutter_AddGroupRequestPageReceived(AddGroupRequestPage page)
        {
            this.uiSafeInvoker.ActionOnUI<AddGroupRequestPage>(this.do_ClientOutter_AddGroupRequestPageReceived, page);
        }
        private void do_ClientOutter_AddGroupRequestPageReceived(AddGroupRequestPage page)
        {
            INotifyForm notifyForm = (INotifyForm)this.resourceCenter.ChatFormController.GetNotifyForm();
            notifyForm.OnAddGroupRequestPageReceived(page);
        }

        private void ClientOutter_AddGroupResponseReceived(string requesterID, string groupID, bool isAgreed)
        {
            this.uiSafeInvoker.ActionOnUI<string,string, bool>(this.do_ClientOutter_AddGroupResponseReceived, requesterID, groupID, isAgreed);
        }

        private void do_ClientOutter_AddGroupResponseReceived(string requesterID, string groupID, bool isAgreed)
        {
            bool isExistedFrom = this.resourceCenter.ChatFormController.IsExistNotifyForm();
            INotifyForm notifyForm = (INotifyForm)this.resourceCenter.ChatFormController.GetNotifyForm();
            if (isExistedFrom)
            {
                notifyForm.OnHandleAddGroupRequestReceived(requesterID,groupID, isAgreed);
            }
            notifyForm.ShowTabPage(NotifyType.Group);
        }

        private void ClientOutter_AddGroupRequestReceived(string requesterID, string groupID, string comment)
        {
            this.uiSafeInvoker.ActionOnUI<string, string, string>(this.do_ClientOutter_AddGroupRequestReceived, requesterID, groupID, comment);
        }

        private void do_ClientOutter_AddGroupRequestReceived(string requesterID, string groupID, string comment)
        {
            bool isExistedFrom = this.resourceCenter.ChatFormController.IsExistNotifyForm();
            INotifyForm notifyForm = (INotifyForm)this.resourceCenter.ChatFormController.GetNotifyForm();
            if (isExistedFrom)
            {
                notifyForm.OnAddGroupRequestReceived(requesterID, groupID, comment);
            }
            notifyForm.ShowTabPage(NotifyType.Group);
        }

        private void ClientOutter_AddFriendRequestPageReceived(AddFriendRequestPage page)
        {
            this.uiSafeInvoker.ActionOnUI<AddFriendRequestPage>(this.do_ClientOutter_AddFriendRequestPageReceived, page);
        }

        private void do_ClientOutter_AddFriendRequestPageReceived(AddFriendRequestPage page)
        {
            INotifyForm notifyForm = (INotifyForm)this.resourceCenter.ChatFormController.GetNotifyForm();
            notifyForm.OnAddFriendRequestPageReceived(page);
        }

        private void ClientOutter_AddFriendResponseReceived(string accepterID, bool isRequester, bool isAgreed)
        {
            this.uiSafeInvoker.ActionOnUI<string,bool, bool>(this.do_ClientOutter_AddFriendResponseReceived, accepterID,isRequester, isAgreed);            
        }

        private void do_ClientOutter_AddFriendResponseReceived(string accepterID, bool isRequester, bool isAgreed)
        {
            bool isExistedFrom = this.resourceCenter.ChatFormController.IsExistNotifyForm();
            INotifyForm notifyForm = (INotifyForm)this.resourceCenter.ChatFormController.GetNotifyForm();
            if (isExistedFrom)
            {
                notifyForm.OnHandleAddFriendRequestReceived(accepterID, isRequester, isAgreed);
            }
            notifyForm.ShowTabPage(NotifyType.User);
        }

        private void ClientOutter_AddFriendRequestReceived(string requesterID, string comment)
        {
            this.uiSafeInvoker.ActionOnUI<string,string>(this.do_ClientOutter_AddFriendRequestReceived,requesterID,comment);
        }

        private void do_ClientOutter_AddFriendRequestReceived(string requesterID, string comment)
        {
            bool isExistedFrom = this.resourceCenter.ChatFormController.IsExistNotifyForm();
            INotifyForm notifyForm = (INotifyForm)this.resourceCenter.ChatFormController.GetNotifyForm();
            if (isExistedFrom)
            {
               notifyForm.OnAddFriendRequestReceived(requesterID, comment);
            }
            notifyForm.ShowTabPage(NotifyType.User);
        }

        void ClientOutter_VibrationReceived(string userID)
        {
            this.uiSafeInvoker.ActionOnUI<string>(this.do_ClientOutter_VibrationReceived ,userID);
        }

        void do_ClientOutter_VibrationReceived(string userID)
        {
            IFriendChatForm form = (IFriendChatForm)this.resourceCenter.ChatFormController.GetForm(userID);
            if (form != null)
            {
                form.HandleVibration();
            }
        }

        #region ClientGlobalCache_UnitCommentNameChanged
        void ClientGlobalCache_UnitCommentNameChanged(IUnit unit)
        {
            this.uiSafeInvoker.ActionOnUI<IUnit>(this.do_ClientGlobalCache_UnitCommentNameChanged, unit);
        }

        void do_ClientGlobalCache_UnitCommentNameChanged(IUnit unit)
        {
            if(unit.UnitType == UnitType.Group)
            {
                IGroupChatForm groupChatForm = (IGroupChatForm)this.resourceCenter.ChatFormController.GetExistedForm(unit.ID);
                if (groupChatForm != null)
                {
                    groupChatForm.UnitCommentNameChanged(unit);
                }
                return;
            }

            IFriendChatForm chatForm = (IFriendChatForm)this.resourceCenter.ChatFormController.GetExistedForm(unit.ID);
            if (chatForm != null)
            {
                chatForm.UnitCommentNameChanged(unit);
            }

            foreach (IChatForm form in this.resourceCenter.ChatFormController.GetAllForms())
            {
                IGroupChatForm groupChatForm = form as IGroupChatForm;
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
            this.uiSafeInvoker.ActionOnUI(this.do_ClientGlobalCache_MyInfoChanged);
        }

        void do_ClientGlobalCache_MyInfoChanged()
        {
            foreach (IChatForm chatForm in this.resourceCenter.ChatFormController.GetAllForms())
            {
                chatForm.MyInfoChanged(this.resourceCenter.ClientGlobalCache.CurrentUser);
            }
        }

        void globalUserCache_GroupChanged(TGroup group, GroupChangedType type,string operatorID, string userID)
        {
            this.uiSafeInvoker.ActionOnUI<TGroup, GroupChangedType, string, string>(this.do_globalUserCache_GroupChanged, group, type , operatorID, userID);
        }

        void do_globalUserCache_GroupChanged(TGroup group, GroupChangedType type, string operatorID, string userID)
        {
            if (type == GroupChangedType.MyselfBePulledIntoGroup)
            {
                IGroupChatForm form = (IGroupChatForm)this.resourceCenter.ChatFormController.GetForm(group.ID);
                form.HandleBePulledIntoGroupNotify(operatorID);
                return;
            }

            IGroupChatForm groupChatForm = (IGroupChatForm)this.resourceCenter.ChatFormController.GetExistedForm(group.ID);
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
            this.uiSafeInvoker.ActionOnUI<string>(this.do_globalUserCache_FriendRemoved, friendID);
        }

        private void do_globalUserCache_FriendRemoved(string friendID)
        {
            IFriendChatForm chatForm = (IFriendChatForm)this.resourceCenter.ChatFormController.GetExistedForm(friendID);
            if (chatForm != null)
            {
                chatForm.OnRemovedFromFriend();
            }
        }

        void clientOutter_FriendAdded(string userID)
        {
            this.uiSafeInvoker.ActionOnUI<string>(this.do_clientOutter_FriendAdded, userID);
        }

        void do_clientOutter_FriendAdded(string userID)
        {
            IFriendChatForm form = (IFriendChatForm)this.resourceCenter.ChatFormController.GetForm(userID);
            form.HandleFriendAddedNotify();
        }

        void clientOutter_ChatMessageReceived(string userID, ClientType sourceType, byte[] info, DateTime? offlineMsgOccurTime)
        {
            this.uiSafeInvoker.ActionOnUI<string,ClientType, byte[], DateTime?>(this.do_clientOutter_ChatMessageReceived, userID, sourceType ,info, offlineMsgOccurTime);
        }

        void do_clientOutter_ChatMessageReceived(string userID, ClientType sourceType, byte[] info, DateTime? offlineMsgOccurTime)
        {
            if(userID == this.resourceCenter.CurrentUserID) //文件传送助手-自己其它设备发给自己的消息
            {
                if (sourceType != this.resourceCenter.CurrentClientType)
                {
                    IFileAssistantForm fileAssistantForm = (IFileAssistantForm)this.resourceCenter.ChatFormController.GetFileAssistantForm();
                    fileAssistantForm.HandleChatMessage(info, offlineMsgOccurTime);
                }
                return;
            }

            IFriendChatForm form = (IFriendChatForm)this.resourceCenter.ChatFormController.GetForm(userID);
            form.HandleChatMessage(info, offlineMsgOccurTime);
            this.resourceCenter.ChatFormController.OnNewMessage(userID);
        }

        void ClientOutter_FriendChatMessageEchoReceived(ClientType clientType, string destUserID, byte[] content)
        {
            this.uiSafeInvoker.ActionOnUI<ClientType, string, byte[]>(this.do_ClientOutter_FriendChatMessageEchoReceived, clientType, destUserID, content);
        }

        void do_ClientOutter_FriendChatMessageEchoReceived(ClientType clientType, string destUserID, byte[] content)
        {
            IFriendChatForm form = this.resourceCenter.ChatFormController.GetExistedForm(destUserID) as IFriendChatForm;
            if (form != null)
            {
                form.HandleChatMessageOfMine(content);
            }    
        }

        private void ClientOutter_SnapchatMessageReceived(string sourceUserID, ClientType sourceType, byte[] info, DateTime? offlineMsgOccurTime)
        {
            this.uiSafeInvoker.ActionOnUI<string, ClientType, byte[], DateTime?>(this.do_ClientOutter_SnapchatMessageReceived, sourceUserID, sourceType, info, offlineMsgOccurTime);
        }

        private void do_ClientOutter_SnapchatMessageReceived(string sourceUserID, ClientType sourceType, byte[] info, DateTime? offlineMsgOccurTime)
        {
            IFriendChatForm form = this.resourceCenter.ChatFormController.GetForm(sourceUserID) as IFriendChatForm;
            SnapchatMessage message = CompactPropertySerializer.Default.Deserialize<SnapchatMessage>(info, 0);
            form.HandleSnapchatMessage(message,offlineMsgOccurTime);            
        }


        private void ClientOutter_SnapchatReadReceived(string sourceUserID, SnapchatMessageRead snapchatMessageRead)
        {
            this.uiSafeInvoker.ActionOnUI<string, SnapchatMessageRead>(this.do_ClientOutter_SnapchatReadReceived, sourceUserID, snapchatMessageRead);
        }

        private void do_ClientOutter_SnapchatReadReceived(string sourceUserID, SnapchatMessageRead snapchatMessageRead)
        {
            string destUserID = sourceUserID == this.resourceCenter.CurrentUserID ? snapchatMessageRead.SourceCreatorID : sourceUserID;

            IFriendChatForm form = this.resourceCenter.ChatFormController.GetExistedForm(destUserID) as IFriendChatForm;
            if (form != null)
            {
                form.HandleSnapchatRead(snapchatMessageRead.SourceCreatorID + "-" + snapchatMessageRead.MessageID);
            }
        }

        void ClientOutter_FriendAudioMessageReceived(AudioMessage msg, DateTime? offlineMsgOccurTime)
        {
            this.uiSafeInvoker.ActionOnUI<AudioMessage, DateTime?>(this.do_ClientOutter_FriendAudioMessageReceived, msg, offlineMsgOccurTime);
        }

        void ClientOutter_FriendAudioMessageEchoReceived(ClientType clientType, AudioMessage audioMessage, string destUserID)
        {
            this.uiSafeInvoker.ActionOnUI<ClientType, AudioMessage, string>(this.do_ClientOutter_FriendAudioMessageEchoReceived, clientType, audioMessage ,destUserID);
        }

        void do_ClientOutter_FriendAudioMessageEchoReceived(ClientType clientType, AudioMessage audioMessage, string destUserID)
        {
            IFriendChatForm form = this.resourceCenter.ChatFormController.GetExistedForm(destUserID) as IFriendChatForm;
            if (form != null)
            {
                form.HandleAudioMessageOfMine(audioMessage);
            }    
        }

        void do_ClientOutter_FriendAudioMessageReceived(AudioMessage msg, DateTime? offlineMsgOccurTime)
        {
            IFriendChatForm form = (IFriendChatForm)this.resourceCenter.ChatFormController.GetForm(msg.CreatorID);
            form.HandleAudioMessage(msg, offlineMsgOccurTime);
            this.resourceCenter.ChatFormController.OnNewMessage(msg.CreatorID);
        }      

        void clientOutter_OfflineFileResultReceived(string userID, string fileName, bool accept)
        {
            this.uiSafeInvoker.ActionOnUI<string, string, bool>(this.do_clientOutter_OfflineFileResultReceived, userID, fileName, accept);
        }

        void do_clientOutter_OfflineFileResultReceived(string userID, string fileName, bool accept)
        {
            IFriendChatForm form = (IFriendChatForm)this.resourceCenter.ChatFormController.GetForm(userID);
            form.HandleOfflineFileResultReceived(fileName, accept);
        }

        void clientOutter_MediaCommunicateReceived(string userID, ClientType clientType, CommunicateMediaType type, CommunicateType type2, string tag)
        {
            this.uiSafeInvoker.ActionOnUI<string, ClientType, CommunicateMediaType, CommunicateType, string>(this.do_clientOutter_MediaCommunicateReceived, userID, clientType, type, type2, tag);
        }

        void do_clientOutter_MediaCommunicateReceived(string userID, ClientType clientType, CommunicateMediaType type, CommunicateType type2, string tag)
        {
            #region 收到同意回复，但请求该请求页面已不存在，直接回复中断
            if ((type == CommunicateMediaType.Video || type == CommunicateMediaType.Audio) && type2 == CommunicateType.Agree)
            {
                if (string.IsNullOrEmpty(GGTalk.CommonOptions.CallingID4VideoOrVoice) || 
                    (!string.IsNullOrEmpty(GGTalk.CommonOptions.CallingID4VideoOrVoice) && GGTalk.CommonOptions.CallingID4VideoOrVoice != userID))
                {
                    this.resourceCenter.ClientOutter.SendMediaCommunicate(userID, type, CommunicateType.Terminate, null, clientType);
                    return;
                }
            } 
            #endregion
            if (type == CommunicateMediaType.GroupVideo)
            {
                Form groupVideoForm = null;
                if (CommunicateType.Reject == type2)//收到拒绝，在界面上处理拒绝结果（移除对应正在加入群聊的头像）
                {
                    groupVideoForm = this.resourceCenter.ChatFormController.GetGroupVideoCallForm(tag);//获取是否存在请求页面
                    if (groupVideoForm == null)
                    {
                        groupVideoForm = this.resourceCenter.ChatFormController.GetGroupVideoChatForm(tag);//获取是否存在通话页面
                        if (groupVideoForm == null)
                            return;
                    }
                    IGroupVideoForm callForm = (IGroupVideoForm)groupVideoForm;
                    callForm.OnRejectJoinReceived(userID);
                }
                else if (CommunicateType.Request == type2)
                {
                    string[] temp = tag.Split(FunctionOptions.ColonSeparator);
                    string videoGroupID = temp[0];
                    string[] memberIDs = temp[1].Split(FunctionOptions.CommaSeparator);
                    if (this.resourceCenter.ChatFormController.ExistGroupVideoCallForm())//若已存在请求中的则自动拒绝
                    {
                        this.RejectGroupVideo(memberIDs, videoGroupID);
                        return;
                    }
                    //视频正在用，再次收到请求，直接默认拒绝请求
                    if (MultimediaManagerFactory.GetSingleton().DeviceIsWorking(OMCS.MultimediaDeviceType.Camera))
                    {
                        this.RejectGroupVideo(memberIDs, videoGroupID);
                        return;
                    }
                    groupVideoForm = this.resourceCenter.ChatFormController.GetNewGroupVideoCallForm(videoGroupID, userID, new List<string>(memberIDs));
                    if (groupVideoForm == null) { return; }
                    groupVideoForm.Show();
                }
                return;
            }
            IFriendChatForm form = (IFriendChatForm)this.resourceCenter.ChatFormController.GetForm(userID);
            form.HandleMediaCommunicate(clientType, type, type2, tag);
            this.resourceCenter.ChatFormController.OnNewMessage(userID);
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
            this.uiSafeInvoker.ActionOnUI<string, ClientType, CommunicateMediaType, bool>(this.do_ClientOutter_MediaCommunicateAnswerOnOtherDevice, friendID, answerType, type, agree);
        
        }

        void do_ClientOutter_MediaCommunicateAnswerOnOtherDevice(string friendID, ClientType answerType, CommunicateMediaType type, bool agree)
        {
            //friendID 在群视频聊天中为 videoGroupID
            IGroupVideoForm form1 = (IGroupVideoForm)this.resourceCenter.ChatFormController.GetGroupVideoCallForm(friendID);
            if (form1 != null)
            {
                form1.OnGroupVideoAnswerOnOtherDevice();
                return;
            }
            IFriendChatForm form = (IFriendChatForm)this.resourceCenter.ChatFormController.GetExistedForm(friendID);
            if (form == null)
            {
                return;
            }

            form.OnMediaCommunicateAnswerOnOtherDevice(answerType, type, agree);           
        }   


        void clientOutter_InptingNotifyReceived(string userID)
        {
            this.uiSafeInvoker.ActionOnUI<string>(this.do_clientOutter_InptingNotifyReceived, userID);
        }

        void do_clientOutter_InptingNotifyReceived(string userID)
        {
            IFriendChatForm form = (IFriendChatForm)this.resourceCenter.ChatFormController.GetExistedForm(userID);
            if (form != null)
            {
                form.HandleInptingNotify();
            }
        }

        void clientOutter_GroupChatMessageReceived(string broadcasterID, string groupID, byte[] content, string tag, DateTime? offlineMsgOccurTime)
        {
            this.uiSafeInvoker.ActionOnUI<string, string, byte[],string, DateTime?>(this.do_clientOutter_GroupChatMessageReceived, broadcasterID, groupID, content, tag, offlineMsgOccurTime);
        }

        void do_clientOutter_GroupChatMessageReceived(string broadcasterID, string groupID, byte[] content,string tag, DateTime? offlineMsgOccurTime)
        {
            IGroupChatForm form = (IGroupChatForm)this.resourceCenter.ChatFormController.GetForm(groupID);
            if (offlineMsgOccurTime == null)
            {
                form.HandleGroupChatMessage(broadcasterID, content,tag);
            }
            else
            {
                form.HandleOfflineGroupChatMessage(broadcasterID, content , offlineMsgOccurTime.Value, tag);
            }
            this.resourceCenter.ChatFormController.OnNewMessage(groupID);            
        }

        void ClientOutter_GroupFileUploadedNotifyReceived(string sourceUserID, string groupID, string fileName)
        {
            this.uiSafeInvoker.ActionOnUI<string, string, string>(this.do_ClientOutter_GroupFileUploadedNotifyReceived, sourceUserID, groupID, fileName);
        }

        void do_ClientOutter_GroupFileUploadedNotifyReceived(string sourceUserID, string groupID, string fileName)
        {
            IGroupChatForm form = (IGroupChatForm)this.resourceCenter.ChatFormController.GetForm(groupID);
            form.HandleGroupFileUploadedNotify(sourceUserID, groupID, fileName);
            this.resourceCenter.ChatFormController.OnNewMessage(groupID);
        }


        void rapidPassiveEngine_ConnectionInterrupted()
        {
            this.uiSafeInvoker.ActionOnUI(this.do_rapidPassiveEngine_ConnectionInterrupted);
        }

        void do_rapidPassiveEngine_ConnectionInterrupted()
        {
            GGTalk.CommonOptions.CallingID4VideoOrVoice = string.Empty;
            foreach (IChatForm form in this.resourceCenter.ChatFormController.GetAllForms())
            {
                form.MyselfOffline();
            }
        }

        void globalUserCache_FriendInfoChanged(TUser user)
        {
            this.uiSafeInvoker.ActionOnUI<TUser>(this.do_globalUserCache_FriendInfoChanged, user);
        }

        void do_globalUserCache_FriendInfoChanged(TUser user)
        {
            IFriendChatForm chatForm = (IFriendChatForm)this.resourceCenter.ChatFormController.GetExistedForm(user.ID);
            if (chatForm != null)
            {
                chatForm.FriendInfoChanged(user);
            }

            foreach (IChatForm form in this.resourceCenter.ChatFormController.GetAllForms())
            {
                IGroupChatForm groupChatForm = form as IGroupChatForm;
                if (groupChatForm != null)
                {
                    groupChatForm.FriendInfoChanged(user);
                }
            }
        }

        void globalUserCache_FriendStatusChanged(TUser user)
        {
            this.uiSafeInvoker.ActionOnUI<TUser>(this.do_globalUserCache_FriendStatusChanged, user);
        }

        private void do_globalUserCache_FriendStatusChanged(TUser user)
        {
            //this.userListBox1.UserStatusChanged(user);
            foreach (IChatForm chatForm in this.resourceCenter.ChatFormController.GetAllForms())
            {
                chatForm.FriendStateChanged(user.ID, user.UserStatus);
            }
        }      
    }    
}
