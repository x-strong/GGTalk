using ESBasic;
using ESBasic.Security;
using ESFramework;
using ESFramework.Boost.Controls;
using ESPlus.Application;
using ESPlus.Rapid;
using ESPlus.Serialization;
using GGTalk;
using OMCS.Passive.ShortMessages;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalkBase.Client
{
    /// <summary>
    /// 与服务器/其它在线用户进行交互的通道。
    /// </summary>
    internal class ClientOutter<TUser, TGroup> : IBrige4ClientOutter, IClientOutter
        where TUser : class, TalkBase.IUser, new()
        where TGroup : TalkBase.IGroup, new()
    {
        private BaseGlobalCache<TUser, TGroup> clientGlobalCache;
        private IRapidPassiveEngine rapidPassiveEngine;
        private IChatRecordPersister localChatRecordPersister;
        private TalkBaseInfoTypes talkBaseInfoTypes;
        private ITalkBaseHelper<TGroup> talkBaseHelper;
        private DesEncryption desEncryption = null;

        public ClientOutter(IRapidPassiveEngine engine, BaseGlobalCache<TUser, TGroup> cache, IChatRecordPersister persister, TalkBaseInfoTypes infoTypes, ITalkBaseHelper<TGroup> helper, DesEncryption encryption)
        {
            this.rapidPassiveEngine = engine;
            this.clientGlobalCache = cache;
            this.localChatRecordPersister = persister;
            this.talkBaseInfoTypes = infoTypes;
            this.talkBaseHelper = helper;
            this.desEncryption = encryption;
        }

        #region Event

        public event CbGeneric<SystemMessageContract, DateTime?> SystemMessageReceived;
        public void OnSystemMessageReceived(SystemMessageContract msg, DateTime? offlineMsgOccurTime)
        {
            if (this.SystemMessageReceived != null)
            {
                this.SystemMessageReceived(msg, offlineMsgOccurTime);
            }
        }

        public event CbGeneric<IUnit> LastWordsChanged;

        /// <summary>
        /// 当接收到对方的正在输入的通知时，触发此事件。参数：sourceUserID
        /// </summary>
        public event CbGeneric<string> InptingNotifyReceived;
        public void OnInptingNotifyReceived(string sourceUserID)
        {
            if (this.InptingNotifyReceived != null)
            {
                this.InptingNotifyReceived(sourceUserID);
            }
        }

        /// <summary>
        /// 当接收到对方的抖屏时，触发此事件。参数：sourceUserID
        /// </summary>
        public event CbGeneric<string> VibrationReceived;
        public void OnVibrationReceived(string sourceUserID)
        {
            if (this.VibrationReceived != null)
            {
                this.VibrationReceived(sourceUserID);
            }
        }

        /// <summary>
        /// 当接收到对方关于视频/语音/远程协助的沟通信息时，触发此事件。参数：sourceUserID - sourceClientType - CommunicateMediaType - CommunicateType - tag
        /// </summary>
        public event CbGeneric<string, ClientType, CommunicateMediaType, CommunicateType, string> MediaCommunicateReceived;
        public void OnMediaCommunicateReceived(string sourceUserID, ClientType sourceClientType, CommunicateMediaType mediaType, CommunicateType communicateType, string tag)
        {
            if (this.MediaCommunicateReceived != null)
            {
                this.MediaCommunicateReceived(sourceUserID, sourceClientType, mediaType, communicateType, tag);
            }
        }



        /// <summary>
        /// 被对方添加为好友，参数：sourceUserID。（在触发此事件之前，已经将好友添加到本地缓存。）
        /// </summary>
        public event CbGeneric<string> FriendAdded;
        public void OnFriendAdded(string sourceUserID)
        {
            if (this.FriendAdded != null)
            {
                this.FriendAdded(sourceUserID);
            }
        }

        /// <summary>
        /// 当接收到群聊天消息时，触发此事件。参数：broadcasterID - groupID - content。（在触发此事件之前，已经消息添加到本地聊天记录。）
        /// </summary>
        public event CbGeneric<string, string, byte[], string, DateTime?> GroupChatMessageReceived;
        public void OnGroupChatMessageReceived(string broadcasterID, string groupID, byte[] content, string tag, DateTime? offlineMsgOccurTime)
        {
            if (this.desEncryption != null)
            {
                content = this.desEncryption.Decrypt(content);
            }

            ChatMessageRecord record = new ChatMessageRecord(broadcasterID, groupID, content, true);
            this.localChatRecordPersister.InsertChatMessageRecord(record);

            if (this.GroupChatMessageReceived != null)
            {
                this.GroupChatMessageReceived(broadcasterID, groupID, content, tag, offlineMsgOccurTime);
            }

            LastWordsRecord lastWordsRecord = new LastWordsRecord(broadcasterID, groupID, true, content);
            TGroup group = this.clientGlobalCache.GetGroup(groupID);
            if (group != null)
            {
                group.LastWordsRecord = lastWordsRecord;
                if (this.LastWordsChanged != null)
                {
                    this.LastWordsChanged(group);
                }
            }

        }

        /// <summary>
        /// 当接收到对方的聊天消息时，触发此事件。参数：sourceUserID - sourceType - content - time。（在触发此事件之前，已经消息添加到本地聊天记录。）
        /// </summary>
        public event CbGeneric<string, ClientType, byte[], DateTime?> FriendChatMessageReceived;
        public void OnChatMessageReceived(string sourceUserID, ClientType sourceType, byte[] content, DateTime? offlineMsgOccurTime)
        {
            if (this.desEncryption != null)
            {
                content = this.desEncryption.Decrypt(content);
            }

            ChatMessageRecord record = new ChatMessageRecord(sourceUserID, this.clientGlobalCache.CurrentUser.ID, content, false);
            this.localChatRecordPersister.InsertChatMessageRecord(record);

            if (this.FriendChatMessageReceived != null)
            {
                this.FriendChatMessageReceived(sourceUserID, sourceType, content, offlineMsgOccurTime);
            }

            LastWordsRecord lastWordsRecord = new LastWordsRecord(sourceUserID, this.rapidPassiveEngine.CurrentUserID, false, content);
            TUser user = this.clientGlobalCache.GetUser(sourceUserID);
            if (user != null)
            {
                user.LastWordsRecord = lastWordsRecord;
                if (this.LastWordsChanged != null)
                {
                    this.LastWordsChanged(user);
                }
            }
        }

        /// <summary>
        /// 当接收到自己在其它设备上发送给对方的聊天消息时，触发此事件。参数：ClientType - destUserID - content。（在触发此事件之前，已经消息添加到本地聊天记录。）
        /// </summary>
        public event CbGeneric<ClientType, string, byte[]> FriendChatMessageEchoReceived;
        public void OnChatMessageEchoReceived(ClientType clientType, string destUserID, byte[] content)
        {
            if (this.desEncryption != null)
            {
                content = this.desEncryption.Decrypt(content);
            }

            ChatMessageRecord record = new ChatMessageRecord(this.clientGlobalCache.CurrentUser.ID, destUserID, content, false);
            this.localChatRecordPersister.InsertChatMessageRecord(record);

            if (this.FriendChatMessageEchoReceived != null)
            {
                this.FriendChatMessageEchoReceived(clientType, destUserID, content);
            }

            LastWordsRecord lastWordsRecord = new LastWordsRecord(this.rapidPassiveEngine.CurrentUserID, destUserID, false, content);
            TUser user = this.clientGlobalCache.GetUser(destUserID);
            if (user != null)
            {
                user.LastWordsRecord = lastWordsRecord;
                if (this.LastWordsChanged != null)
                {
                    this.LastWordsChanged(user);
                }
            }
        }

        /// <summary>
        /// 当接收到对方的阅后即焚消息时，触发此事件。参数：sourceUserID - sourceType - content - time。
        /// </summary>
        public event CbGeneric<string, ClientType, byte[], DateTime?> SnapchatMessageReceived;
        public void OnSnapchatMessageReceived(string sourceUserID, ClientType sourceType, byte[] content, DateTime? offlineMsgOccurTime)
        {
            if (this.desEncryption != null)
            {
                content = this.desEncryption.Decrypt(content);
            }
            if (this.SnapchatMessageReceived != null)
            {
                this.SnapchatMessageReceived(sourceUserID, sourceType, content, offlineMsgOccurTime);
            }
        }

        /// <summary>
        /// 当接收到对方（或自己其他类型客户端）的自焚消息被阅读时，触发此事件。参数：sourceUserID -SnapchatMessage
        /// </summary>
        public event CbGeneric<string, SnapchatMessageRead> SnapchatReadReceived;
        public void OnSnapchatReadReceived(string sourceUserID, SnapchatMessageRead snapchatMessageRead)
        {
            if (this.SnapchatReadReceived != null)
            {
                this.SnapchatReadReceived(sourceUserID, snapchatMessageRead);
            }
        }

        public event CbGeneric<AudioMessage, DateTime?> FriendAudioMessageReceived;
        public void OnAudioMessageReceived(AudioMessage msg, DateTime? offlineMsgOccurTime)
        {
            string sourceUserID = msg.CreatorID;
            ChatBoxContent chatBoxContent = new ChatBoxContent("[语音消息]", new System.Drawing.Font("微软雅黑", 9), System.Drawing.Color.Black);
            byte[] content = ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(chatBoxContent);
            ChatMessageRecord record = new ChatMessageRecord(sourceUserID, this.clientGlobalCache.CurrentUser.ID, content, false);
            this.localChatRecordPersister.InsertChatMessageRecord(record);

            if (this.FriendAudioMessageReceived != null)
            {
                this.FriendAudioMessageReceived(msg, offlineMsgOccurTime);
            }

            LastWordsRecord lastWordsRecord = new LastWordsRecord(sourceUserID, this.rapidPassiveEngine.CurrentUserID, false, content);
            TUser user = this.clientGlobalCache.GetUser(sourceUserID);
            if (user != null)
            {
                user.LastWordsRecord = lastWordsRecord;
                if (this.LastWordsChanged != null)
                {
                    this.LastWordsChanged(user);
                }
            }
        }

        public event CbGeneric<ClientType, AudioMessage, string> FriendAudioMessageEchoReceived;
        public void OnAudioMessageEchoReceived(ClientType clientType, AudioMessage msg, string destUserID)
        {
            ChatBoxContent chatBoxContent = new ChatBoxContent("[语音消息]", new System.Drawing.Font("微软雅黑", 9), System.Drawing.Color.Black);
            byte[] content = ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(chatBoxContent);
            ChatMessageRecord record = new ChatMessageRecord(this.clientGlobalCache.CurrentUser.ID, destUserID, content, false);
            this.localChatRecordPersister.InsertChatMessageRecord(record);

            if (this.FriendAudioMessageEchoReceived != null)
            {
                this.FriendAudioMessageEchoReceived(clientType, msg, destUserID);
            }

            LastWordsRecord lastWordsRecord = new LastWordsRecord(this.rapidPassiveEngine.CurrentUserID, destUserID, false, content);
            TUser user = this.clientGlobalCache.GetUser(destUserID);
            if (user != null)
            {
                user.LastWordsRecord = lastWordsRecord;
                if (this.LastWordsChanged != null)
                {
                    this.LastWordsChanged(user);
                }
            }
        }


        /// <summary>
        /// 当接收到对方对我发给他的离线文件的处理结果后，触发此事件，参数：sourceUserID - fileName - accept 。
        /// </summary>
        public event CbGeneric<string, string, bool> OfflineFileResultReceived;
        public void OnOfflineFileResultReceived(string sourceUserID, string fileName, bool accept)
        {
            if (this.OfflineFileResultReceived != null)
            {
                this.OfflineFileResultReceived(sourceUserID, fileName, accept);
            }
        }

        /// <summary>
        /// 当接收到群文件上传完成的通知时，触发此事件。参数：sourceUserID - groupID - fileName
        /// </summary>
        public event CbGeneric<string, string, string> GroupFileUploadedNotifyReceived;
        public void OnGroupFileUploadedNotifyReceived(string sourceUserID, string groupID, string fileName)
        {
            if (this.GroupFileUploadedNotifyReceived != null)
            {
                this.GroupFileUploadedNotifyReceived(sourceUserID, groupID, fileName);
            }
        }

        public event CbGeneric<string, ClientType, CommunicateMediaType, bool> MediaCommunicateAnswerOnOtherDevice;


        public void OnMediaCommunicateAnswerOnOtherDevice(string friendID, ClientType answerType, CommunicateMediaType type, bool agree)
        {
            if (this.MediaCommunicateAnswerOnOtherDevice != null)
            {
                this.MediaCommunicateAnswerOnOtherDevice(friendID, answerType, type, agree);
            }
        }

        #region OnAddFriendRequest
        public event CbGeneric<string, string> AddFriendRequestReceived;
        public event CbGeneric<string, bool, bool> AddFriendResponseReceived;
        public event CbGeneric<AddFriendRequestPage> AddFriendRequestPageReceived;

        /// <summary>
        /// 当收到添加好友申请时触发。
        /// </summary>
        /// <param name="friendID"></param>
        /// <param name="comment"></param>
        public void OnAddFriendRequestReceived(string friendID, string comment)
        {
            if (this.AddFriendRequestReceived != null)
            {
                this.AddFriendRequestReceived(friendID, comment);
            }
        }

        /// <summary>
        /// 收到对方处理好友申请结果的消息时触发。
        /// </summary>
        /// <param name="accepterID"></param>
        /// <param name="isAgreed"></param>
        public void OnHandleAddFriendRequestReceived(string accepterID, bool isRequester, bool isAgreed)
        {
            if (isAgreed)
            {
                TUser friend = this.clientGlobalCache.GetUser(accepterID);
                this.clientGlobalCache.PrepairUnit(friend);
            }
            if (this.AddFriendResponseReceived != null)
            {
                this.AddFriendResponseReceived(accepterID, isRequester, isAgreed);
            }
        }

        /// <summary>
        /// 收到好友申请记录分页
        /// </summary>        
        public void OnAddFriendRequestPageReceived(AddFriendRequestPage page)
        {
            if (AddFriendRequestPageReceived != null)
            {
                this.AddFriendRequestPageReceived(page);
            }
        }

        #endregion

        #region OnAddGroupRequest
        public event CbGeneric<string, string, string> AddGroupRequestReceived;
        public event CbGeneric<string, string, bool> AddGroupResponseReceived;
        public event CbGeneric<AddGroupRequestPage> AddGroupRequestPageReceived;

        /// <summary>
        /// 当收到申请加入讨论组时触发。
        /// </summary>
        /// <param name="requesterID"></param>
        /// <param name="groupID"></param>
        /// <param name="comment"></param>
        public void OnAddGroupRequestReceived(string requesterID, string groupID, string comment)
        {
            if (this.AddGroupRequestReceived != null)
            {
                this.AddGroupRequestReceived(requesterID, groupID, comment);
            }
        }

        /// <summary>
        /// 收到对方处理申请加入讨论组结果的消息时触发。
        /// </summary>
        /// <param name="requesterID"></param>
        /// <param name="groupID"></param>
        /// <param name="isAgreed"></param>
        public void OnHandleAddGroupRequestReceived(string requesterID, string groupID, bool isAgreed)
        {
            if (this.AddGroupResponseReceived != null)
            {
                this.AddGroupResponseReceived(requesterID, groupID, isAgreed);
            }
        }

        /// <summary>
        /// 收到申请加入讨论组记录分页
        /// </summary>  
        public void OnAddGroupRequestPageReceived(AddGroupRequestPage page)
        {
            if (AddGroupRequestPageReceived != null)
            {
                this.AddGroupRequestPageReceived(page);
            }
        }
        #endregion


        public event CbGeneric<string, string, double> GroupBan4UserReceived;
        /// <summary>
        /// 当接收到被禁言通知，触发此事件，参数：operatorID - groupID - minutes 。
        /// </summary>
        public void OnGroupBan4UserReceived(string operatorID, string groupID, double minutes)
        {
            if (this.GroupBan4UserReceived != null)
            {
                this.GroupBan4UserReceived(operatorID, groupID, minutes);
            }
        }


        public event CbGeneric<string> RemoveGroupBan4UserReceived;
        /// <summary>
        /// 当接收到被解除禁言通知，触发此事件，参数：groupID
        /// </summary>
        public void OnRemoveGroupBan4UserReceived(string groupID)
        {
            if (this.RemoveGroupBan4UserReceived != null)
            {
                this.RemoveGroupBan4UserReceived(groupID);
            }
        }

        /// <summary>
        /// 当收到开启全员禁言通知，触发此事件，参数operatorID - groupID
        /// </summary>
        public event CbGeneric<string, string> GroupBan4GroupReceived;
        public void OnGroupBan4GroupReceived(string operatorID, string groupID)
        {
            if (this.clientGlobalCache.CurrentUser.ID == operatorID)
            {
                return;
            }
            if (this.GroupBan4GroupReceived != null)
            {
                this.GroupBan4GroupReceived(operatorID, groupID);
            }
        }

        /// <summary>
        /// 当收到关闭全员禁言通知，触发此事件，参数 groupID
        /// </summary>
        public event CbGeneric<string> RemoveGroupBan4GroupReceived;
        public void OnRemoveGroupBan4GroupReceived(string groupID)
        {
            if (this.RemoveGroupBan4GroupReceived != null)
            {
                this.RemoveGroupBan4GroupReceived(groupID);
            }
        }
        #endregion


        #region Chat
        /// <summary>
        /// 发送聊天消息。
        /// </summary>      
        public ChatMessageRecord SendChatMessage(string destUserID, byte[] msg, ResultHandler handler)
        {
            byte[] encrypted = msg;
            if (this.desEncryption != null)
            {
                encrypted = this.desEncryption.Encrypt(msg);
            }

            //使用Tag携带 接收者的ID
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.Chat, encrypted, destUserID, 2048, handler, null, true);
            ChatMessageRecord record = new ChatMessageRecord(this.clientGlobalCache.CurrentUser.ID, destUserID, msg, false);
            this.localChatRecordPersister.InsertChatMessageRecord(record);

            LastWordsRecord lastWordsRecord = new LastWordsRecord(this.rapidPassiveEngine.CurrentUserID, destUserID, false, msg);
            TUser user = this.clientGlobalCache.GetUser(destUserID);
            if (user != null)
            {
                user.LastWordsRecord = lastWordsRecord;
                if (this.LastWordsChanged != null)
                {
                    this.LastWordsChanged(user);
                }
            }
            return record;
        }

        public void SendChatMessage(List<string> accepters, byte[] msg, ResultHandler handler)
        {
            if (accepters == null || accepters.Count == 0)
            {
                return;
            }

            byte[] encrypted = msg;
            if (this.desEncryption != null)
            {
                encrypted = this.desEncryption.Encrypt(msg);
            }

            //使用Tag携带 接收者的ID
            string ids = ESBasic.Helpers.StringHelper.ContactString(accepters, ",");
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.ChatTransfered, encrypted, ids, 2048, handler, null, true);

            foreach (string unitID in accepters)
            {
                IUnit unit = this.clientGlobalCache.GetUnit(unitID);
                if (unit == null)
                {
                    continue;
                }

                ChatMessageRecord record = new ChatMessageRecord(this.clientGlobalCache.CurrentUser.ID, unitID, msg, unit.UnitType == UnitType.Group);
                this.localChatRecordPersister.InsertChatMessageRecord(record);

                LastWordsRecord lastWordsRecord = new LastWordsRecord(this.rapidPassiveEngine.CurrentUserID, unitID, unit.UnitType == UnitType.Group, msg);
                if (unit != null)
                {
                    unit.LastWordsRecord = lastWordsRecord;
                    if (this.LastWordsChanged != null)
                    {
                        this.LastWordsChanged(unit);
                    }
                }
            }
        }

        /// <summary>
        /// 发送语音消息。
        /// </summary>      
        public ChatMessageRecord SendAudioMessage(string destUserID, AudioMessage msg)
        {
            this.rapidPassiveEngine.SendMessage(destUserID, this.talkBaseInfoTypes.AudioMessage, CompactPropertySerializer.Default.Serialize(msg), null, true);

            //OMCS.Passive.MultimediaManagerFactory.GetSingleton().AudioMessageController.Send(msg, destUserID);

            ChatBoxContent chatBoxContent = new ChatBoxContent("[语音消息]", new System.Drawing.Font("微软雅黑", 9), System.Drawing.Color.Black);
            byte[] content = ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(chatBoxContent);

            ChatMessageRecord record = new ChatMessageRecord(this.clientGlobalCache.CurrentUser.ID, destUserID, content, false);
            this.localChatRecordPersister.InsertChatMessageRecord(record);

            LastWordsRecord lastWordsRecord = new LastWordsRecord(this.rapidPassiveEngine.CurrentUserID, destUserID, false, content);
            TUser user = this.clientGlobalCache.GetUser(destUserID);
            if (user != null)
            {
                user.LastWordsRecord = lastWordsRecord;
                if (this.LastWordsChanged != null)
                {
                    this.LastWordsChanged(user);
                }
            }
            return record;
        }

        /// <summary>
        /// 发送阅后即焚消息
        /// </summary>
        /// <param name="destUserID"></param>
        /// <param name="snapchatMessage"></param>
        public void SendSnapchatMessage(string destUserID, SnapchatMessage snapchatMessage)
        {
            byte[] encrypted = CompactPropertySerializer.Default.Serialize(snapchatMessage);
            if (this.desEncryption != null)
            {
                encrypted = this.desEncryption.Encrypt(encrypted);
            }
            //使用Tag携带 接收者的ID
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.Snapchat, encrypted, destUserID, 2048, false);
        }


        /// <summary>
        /// 发送已阅读了自焚消息
        /// </summary>
        /// <param name="snapchatMessageRead"></param>
        public void SendSnapchatReadMessage(SnapchatMessageRead snapchatMessageRead)
        {
            if (snapchatMessageRead == null)
            {
                return;
            }
            byte[] encrypted = CompactPropertySerializer.Default.Serialize(snapchatMessageRead);
            if (this.desEncryption != null)
            {
                encrypted = this.desEncryption.Encrypt(encrypted);
            }
            this.rapidPassiveEngine.SendMessage(snapchatMessageRead.SourceCreatorID, this.talkBaseInfoTypes.SnapchatRead, encrypted, null, 2048, true);
        }

        /// <summary>
        /// 发送群聊天消息。
        /// </summary>      
        public ChatMessageRecord SendGroupChatMessage(string groupID, byte[] msg, ResultHandler handler, string tag)
        {
            byte[] encrypted = msg;
            if (this.desEncryption != null)
            {
                encrypted = this.desEncryption.Encrypt(msg);
            }

            this.rapidPassiveEngine.ContactsOutter.BroadcastBlob(groupID, this.talkBaseInfoTypes.GroupChat, encrypted, tag, 2048, handler, null);
            ChatMessageRecord record = new ChatMessageRecord(this.clientGlobalCache.CurrentUser.ID, groupID, msg, true);
            this.localChatRecordPersister.InsertChatMessageRecord(record);

            LastWordsRecord lastWordsRecord = new LastWordsRecord(this.rapidPassiveEngine.CurrentUserID, groupID, true, msg);
            TGroup group = this.clientGlobalCache.GetGroup(groupID);
            if (group != null)
            {
                group.LastWordsRecord = lastWordsRecord;
                if (this.LastWordsChanged != null)
                {
                    this.LastWordsChanged(group);
                }
            }
            return record;
        }

        /// <summary>
        /// 发送振动提醒。
        /// </summary>      
        public void SendVibration(string destUserID)
        {
            // this.rapidPassiveEngine.CustomizeOutter.Send(destUserID, this.talkBaseInfoTypes.Vibration, null);
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.Vibration, null, destUserID);
        }

        public void SendInputingNotify(string destUserID)
        {
            this.rapidPassiveEngine.CustomizeOutter.Send(destUserID, this.talkBaseInfoTypes.InputingNotify, null, true, ESFramework.ActionTypeOnChannelIsBusy.Discard);
        }

        #endregion

        #region 视频、语音、远程
        /// <summary>
        /// 发送视频、语音、远程沟通。
        /// </summary>      
        public void SendMediaCommunicate(string destUserID, CommunicateMediaType mediaType, CommunicateType communicateType, string tag, ClientType? destClientType = null)
        {
            int type = -1;
            if (destClientType != null)
            {
                type = (int)destClientType.Value;
            }
            MediaCommunicateContract contract = new MediaCommunicateContract(mediaType, communicateType, tag, type);
            byte[] info = CompactPropertySerializer.Default.Serialize(contract);

            if (communicateType == CommunicateType.Request)
            {
                //this.rapidPassiveEngine.CustomizeOutter.Send(destUserID, this.talkBaseInfoTypes.MediaCommunicate, info);
                this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.MediaCommunicate, info, destUserID, false);
            }
            else
            {
                this.rapidPassiveEngine.SendMessage(destUserID, this.talkBaseInfoTypes.MediaCommunicate, info, tag, true);
            }

            //设置全局通话对象
            if (mediaType == CommunicateMediaType.Audio || mediaType == CommunicateMediaType.Video || mediaType == CommunicateMediaType.GroupVideo)
            {
                if (communicateType == CommunicateType.Agree || communicateType == CommunicateType.Request)
                {
                    destUserID = mediaType == CommunicateMediaType.GroupVideo ? tag : destUserID;
                    CommonOptions.CallingID4VideoOrVoice = destUserID;
                }
                else
                {
                    CommonOptions.CallingID4VideoOrVoice = string.Empty;
                }
            }
        }

        #endregion

        #region Friend

        public List<IUser> SearchUserList(string idOrName)
        {
            byte[] info = this.rapidPassiveEngine.CustomizeOutter.QueryBlob(this.talkBaseInfoTypes.SearchUser, System.Text.Encoding.UTF8.GetBytes(idOrName));
            return ESBasic.Collections.CollectionConverter.ConvertListUpper<IUser, TUser>(CompactPropertySerializer.Default.Deserialize<List<TUser>>(info, 0));
        }

        public IUser SearchUser(string userID)
        {
            byte[] bUser = this.rapidPassiveEngine.CustomizeOutter.QueryBlob(this.talkBaseInfoTypes.GetUserInfo, Encoding.UTF8.GetBytes(userID));
            if (bUser == null)
            {
                return null;
            }
            return ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<TUser>(bUser, 0);
        }

        public void RequestAddFriend(string accepterID, string comment, string requesterCatalogName)
        {
            RequestAddFriendContract contract = new RequestAddFriendContract(this.clientGlobalCache.CurrentUser.ID, accepterID, comment, requesterCatalogName);
            byte[] info = ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(contract);
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.RequestAddFriend, info, accepterID, true);
        }

        public void HandleAddFriendRequest(string requesterID, string accepterCatalogName, bool agree)
        {
            HandleAddFriendRequestContract contract = new HandleAddFriendRequestContract(requesterID, this.clientGlobalCache.CurrentUser.ID, accepterCatalogName, agree);
            byte[] info = ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(contract);
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.HandleAddFriendRequest, info, requesterID, true);
            if (agree)
            {
                TUser friend = this.clientGlobalCache.GetUser(requesterID);
                this.clientGlobalCache.CurrentUser.AddFriend(friend.ID, accepterCatalogName);
                this.clientGlobalCache.AddFriend(friend);
            }
        }

        public void GetAddFriendPage()
        {
            GetAddFriendPageContract contract = new GetAddFriendPageContract(this.rapidPassiveEngine.CurrentUserID, 0, 20);
            byte[] info = ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(contract);
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.GetAddFriendPage, info, null, false);
        }

        public AddFriendResult AddFriend(string friendID, string catalogName)
        {
            AddFriendContract contract = new AddFriendContract(friendID, catalogName);
            byte[] info = ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(contract);
            byte[] bRes = this.rapidPassiveEngine.CustomizeOutter.Query(this.talkBaseInfoTypes.AddFriend, info);
            AddFriendResult res = (AddFriendResult)BitConverter.ToInt32(bRes, 0);
            if (res == AddFriendResult.Succeed)
            {
                TUser friend = this.clientGlobalCache.GetUser(friendID);
                this.clientGlobalCache.AddFriend(friend);
                this.clientGlobalCache.CurrentUser.AddFriend(friend.ID, catalogName);
            }
            return res;
        }

        /// <summary>
        /// 删除好友。（修改本地个人资料，从缓存中移除用户，向服务器发送通知）
        /// </summary>        
        public void RemoveFriend(string friendID)
        {
            //this.rapidPassiveEngine.CustomizeOutter.SendCertainly(null, this.talkBaseInfoTypes.RemoveFriend, System.Text.Encoding.UTF8.GetBytes(friendID));//20210624改为多端
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.RemoveFriend, System.Text.Encoding.UTF8.GetBytes(friendID), null, true);
            this.clientGlobalCache.CurrentUser.RemoveFriend(friendID);
            this.clientGlobalCache.RemovedFriend(friendID);

        }

        public bool IsInHisBlackList(string destUserID)
        {
            byte[] res = this.rapidPassiveEngine.CustomizeOutter.Query(this.talkBaseInfoTypes.InHisBlackList, System.Text.Encoding.UTF8.GetBytes(destUserID));
            return BitConverter.ToBoolean(res, 0);
        }

        public void ChangeUnitCommentName(string unitID, string commentName)
        {
            ChangeCommentNameContract contract = new ChangeCommentNameContract(unitID, commentName);
            byte[] data = CompactPropertySerializer.Default.Serialize(contract);
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.ChangeUnitCommentName, data, null, true);
            this.clientGlobalCache.CurrentUser.ChangeUnitCommentName(unitID, commentName);
            this.clientGlobalCache.ChangeUnitCommentName(unitID, commentName);
        }
        #endregion

        #region FriendCatalog
        /// <summary>
        /// 将好友从一个分组移动到另一个分组。（修改本地个人资料，向服务器发送通知）
        /// </summary>        
        public void MoveFriendCatalog(string friendID, string oldCatalog, string newCatalog)
        {
            this.clientGlobalCache.CurrentUser.MoveFriend(friendID, oldCatalog, newCatalog);
            MoveFriendToOtherCatalogContract contract = new MoveFriendToOtherCatalogContract(friendID, oldCatalog, newCatalog);
            byte[] info = CompactPropertySerializer.Default.Serialize(contract);
            //this.rapidPassiveEngine.CustomizeOutter.Send(this.talkBaseInfoTypes.MoveFriendToOtherCatalog, info);
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.MoveFriendToOtherCatalog, info, "", true);
        }

        /// <summary>
        /// 删除一个分组。（修改本地个人资料，向服务器发送通知）
        /// </summary> 
        public void RemoveFriendCatalog(string catalog)
        {
            this.clientGlobalCache.CurrentUser.RemvoeFriendCatalog(catalog);
            this.rapidPassiveEngine.CustomizeOutter.Send(this.talkBaseInfoTypes.RemoveFriendCatalog, System.Text.Encoding.UTF8.GetBytes(catalog));
        }

        /// <summary>
        /// 修改分组名称。（修改本地个人资料，向服务器发送通知）
        /// </summary> 
        public void ChangeFriendCatalogName(string oldName, string newName, bool isMerge)
        {
            this.clientGlobalCache.CurrentUser.ChangeFriendCatalogName(oldName, newName);
            ChangeCatalogContract contract = new ChangeCatalogContract(oldName, newName);
            byte[] info = CompactPropertySerializer.Default.Serialize(contract);
            this.rapidPassiveEngine.CustomizeOutter.Send(this.talkBaseInfoTypes.ChangeFriendCatalogName, info);
        }

        /// <summary>
        /// 添加分组。（修改本地个人资料，向服务器发送通知）
        /// </summary> 
        public void AddFriendCatalog(string catalog)
        {
            this.clientGlobalCache.CurrentUser.AddFriendCatalog(catalog);
            //this.rapidPassiveEngine.CustomizeOutter.Send(this.talkBaseInfoTypes.AddFriendCatalog, System.Text.Encoding.UTF8.GetBytes(catalog));
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.AddFriendCatalog, System.Text.Encoding.UTF8.GetBytes(catalog), "", true);
        }
        #endregion

        #region Group
        public void RequestAddGroup(string groupID, string comment)
        {
            RequestAddGroupContract contract = new RequestAddGroupContract(this.clientGlobalCache.CurrentUser.ID, groupID, comment);
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.RequestAddGroup, CompactPropertySerializer.Default.Serialize(contract), null, true);
        }

        public void HandleAddGroupRequest(string requesterID, string groupID, bool agree)
        {
            HandleAddGroupRequestContract contract = new HandleAddGroupRequestContract(requesterID, groupID, agree);
            byte[] info = ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(contract);
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.HandleAddGroupRequest, info, requesterID, true);
            //服务端广播通知所有群成员 有新成员被拉入进来
            //if (agree)
            //{
            //    TGroup group = this.clientGlobalCache.GetGroup(groupID);
            //    string[] memberIdArray = new string[group.MemberList.Count];
            //    group.MemberList.CopyTo(memberIdArray);
            //    List<string> memberIDs = new List<string>(memberIdArray);
            //    memberIDs.Add(requesterID);
            //    this.ChangeGroupMembers(groupID, memberIDs);
            //}
        }

        public void GetAddGroupPage()
        {
            GetAddGroupPageContract contract = new GetAddGroupPageContract(this.rapidPassiveEngine.CurrentUserID, 0, 1000);
            byte[] info = ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(contract);
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.GetAddGroupPage, info, null, false);
        }

        /// <summary>
        /// 创建组。如果创建成功，则将组添加到本地缓存中，并更新当前用户的Groups字段。
        /// </summary>        
        public CreateGroupResult CreateGroup(string groupID, string name, string announce, List<string> members, bool isPrivate = false)
        {
            return this.CreateGroup(GroupType.CommonGroup, groupID, name, announce, members, isPrivate);
        }

        /// <summary>
        /// 创建组。如果创建成功，则将组添加到本地缓存中，并更新当前用户的Groups字段。
        /// </summary>        
        public CreateGroupResult CreateGroup(GroupType groupType, string groupID, string name, string announce, List<string> members, bool isPrivate = false)
        {
            TGroup group = default(TGroup);
            CreateGroupContract contract = new CreateGroupContract(groupType, groupID, name, announce, members, isPrivate);
            byte[] bRes = this.rapidPassiveEngine.CustomizeOutter.Query(this.talkBaseInfoTypes.CreateGroup, CompactPropertySerializer.Default.Serialize(contract));
            CreateGroupResult res = (CreateGroupResult)BitConverter.ToInt32(bRes, 0);
            if (res == CreateGroupResult.Succeed)
            {
                this.clientGlobalCache.CurrentUser.JoinGroup(groupID);
                group = this.talkBaseHelper.DoCreateGroup(this.clientGlobalCache.CurrentUser.ID, groupID, name, announce, members, isPrivate);
                this.clientGlobalCache.OnCreateGroup(group);
            }
            return res;
        }

        /// <summary>
        /// 加入组。如果加入成功，则将从服务器加载组资料放入本地缓存，并更新当前用户的Groups字段。
        /// </summary>       
        public JoinGroupResult JoinGroup(string groupID)
        {
            byte[] bRes = this.rapidPassiveEngine.CustomizeOutter.Query(this.talkBaseInfoTypes.JoinGroup, System.Text.Encoding.UTF8.GetBytes(groupID));
            JoinGroupResult res = (JoinGroupResult)BitConverter.ToInt32(bRes, 0);
            if (res == JoinGroupResult.Succeed)
            {
                this.clientGlobalCache.CurrentUser.JoinGroup(groupID);
                this.clientGlobalCache.GetGroup(groupID);
            }
            return res;
        }

        /// <summary>
        /// 根据群名称或群id 精确查找群组
        /// </summary>
        /// <param name="idOrName"></param>
        /// <returns></returns>
        public List<IGroup> SearchGroupList(string idOrName)
        {
            byte[] datas = this.rapidPassiveEngine.CustomizeOutter.Query(this.talkBaseInfoTypes.SearchGroup, System.Text.Encoding.UTF8.GetBytes(idOrName));
            return ESBasic.Collections.CollectionConverter.ConvertListUpper<IGroup, TGroup>(CompactPropertySerializer.Default.Deserialize<List<TGroup>>(datas, 0));
        }

        public IGroup SearchGroup(string groupID)
        {
            byte[] bGroup = this.rapidPassiveEngine.CustomizeOutter.Query(this.talkBaseInfoTypes.GetGroup, System.Text.Encoding.UTF8.GetBytes(groupID));
            return CompactPropertySerializer.Default.Deserialize<TGroup>(bGroup, 0);
        }

        /// <summary>
        /// 退出组。（向服务器发送通知，从当前缓存中移除目标组，从个人资料中删除组）
        /// </summary>        
        public void QuitGroup(string groupID)
        {
            //this.rapidPassiveEngine.CustomizeOutter.SendCertainly(null, this.talkBaseInfoTypes.QuitGroup, System.Text.Encoding.UTF8.GetBytes(groupID));//20210624 改为多端
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.QuitGroup, System.Text.Encoding.UTF8.GetBytes(groupID), null, true);
            this.clientGlobalCache.RemoveGroup(groupID);
            this.clientGlobalCache.CurrentUser.QuitGroup(groupID);
        }

        /// <summary>
        /// 删除组。（向服务器发送通知，从当前缓存中移除目标组，从个人资料中删除组）
        /// </summary>      
        public void DeleteGroup(string groupID)
        {
            //this.rapidPassiveEngine.CustomizeOutter.SendCertainly(null, this.talkBaseInfoTypes.DeleteGroup, System.Text.Encoding.UTF8.GetBytes(groupID));//20210624 改为多端
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.DeleteGroup, System.Text.Encoding.UTF8.GetBytes(groupID), null, true);
            this.clientGlobalCache.RemoveGroup(groupID);
            this.clientGlobalCache.CurrentUser.QuitGroup(groupID);
        }

        public void ChangeGroupInfo(string groupID, string name, string announce)
        {
            TGroup group = this.clientGlobalCache.GetGroup(groupID);
            if (groupID == null)
            {
                return;
            }

            ChangeGroupInfoContract contract = new ChangeGroupInfoContract(this.rapidPassiveEngine.CurrentUserID, groupID, name, announce);
            this.rapidPassiveEngine.CustomizeOutter.Send(this.talkBaseInfoTypes.ChangeGroupInfo, ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(contract));

            group.Name = name;
            group.Announce = announce;
            this.clientGlobalCache.OnGroupInfoChanged(groupID, this.rapidPassiveEngine.CurrentUserID);
        }

        public void ChangeGroupMembers(string groupID, List<string> members)
        {
            TGroup group = this.clientGlobalCache.GetGroup(groupID);
            if (groupID == null)
            {
                return;
            }

            List<string> addedList = new List<string>();
            List<string> removedList = new List<string>();
            foreach (string userID in members)
            {
                if (!group.MemberList.Contains(userID))
                {
                    addedList.Add(userID);
                }
            }
            foreach (string userID in group.MemberList)
            {
                if (!members.Contains(userID))
                {
                    removedList.Add(userID);
                }
            }

            ChangeGroupMembersContract contract = new ChangeGroupMembersContract(groupID, addedList, removedList);
            this.rapidPassiveEngine.CustomizeOutter.Send(this.talkBaseInfoTypes.ChangeGroupMembers, ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(contract));
            this.clientGlobalCache.OnUsersPulledIntoGroup(groupID, this.rapidPassiveEngine.CurrentUserID, addedList);
            this.clientGlobalCache.OnUsersRemovedFromGroup(groupID, this.rapidPassiveEngine.CurrentUserID, removedList);
        }

        public void GroupFileUploaded(string groupID, string fileName)
        {
            this.rapidPassiveEngine.ContactsOutter.Broadcast(groupID, this.talkBaseInfoTypes.GroupFileUploadedNotify, System.Text.Encoding.UTF8.GetBytes(fileName), null, ESFramework.ActionTypeOnChannelIsBusy.Continue);
        }
        #endregion                

        #region SendSystemMessage
        public void SendSystemMessage(string receiverUnitID, int sysMsgType, byte[] content, string tag, bool useOfflineMessageMode)
        {
            SystemMessageContract contract = new SystemMessageContract(this.rapidPassiveEngine.CurrentUserID, receiverUnitID, sysMsgType, content, tag, useOfflineMessageMode);
            byte[] data = CompactPropertySerializer.Default.Serialize(contract);
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.SystemMessage, data, null, 2048);
        }

        #endregion

        #region 修改个人信息、状态
        /// <summary>
        /// 修改当前状态。（修改缓存中自己的状态，向服务器发送通知）
        /// </summary>        
        public void ChangeMyStatus(UserStatus status)
        {
            this.clientGlobalCache.ChangeUserStatus(this.rapidPassiveEngine.CurrentUserID, status);
            this.rapidPassiveEngine.CustomizeOutter.Send(this.talkBaseInfoTypes.ChangeStatus, BitConverter.GetBytes((int)this.clientGlobalCache.CurrentUser.UserStatus));
        }

        /// <summary>
        /// 修改密码。
        /// </summary>       
        public ChangePasswordResult ChangeMyPassword(string oldPasswordMD5, string newPasswordMD5)
        {
            ChangePasswordContract contract = new ChangePasswordContract(oldPasswordMD5, newPasswordMD5);
            byte[] bRes = this.rapidPassiveEngine.CustomizeOutter.Query(this.talkBaseInfoTypes.ChangePassword, CompactPropertySerializer.Default.Serialize(contract));
            ChangePasswordResult res = (ChangePasswordResult)BitConverter.ToInt32(bRes, 0);
            return res;
        }

        /// <summary>
        /// 修改个人资料。
        /// </summary>        
        public void ChangeMyBaseInfo(string name, string signature, string department)
        {
            ChangeUserBaseInfoContract contract = new ChangeUserBaseInfoContract(this.rapidPassiveEngine.CurrentUserID, name, signature, department);
            byte[] data = ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(contract);
            this.rapidPassiveEngine.CustomizeOutter.Send(this.talkBaseInfoTypes.ChangeMyBaseInfo, data);
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.ChangeMyBaseInfo, data, "", true);
            this.clientGlobalCache.UpdateUserBaseInfo(this.rapidPassiveEngine.CurrentUserID, name, signature, department, this.clientGlobalCache.CurrentUser.Version + 1);
        }

        public void ChangeMyHeadImage(int defaultHeadImageIndex, byte[] customizedHeadImage, ResultHandler handler)
        {
            ChangeHeadImageContract contract = new ChangeHeadImageContract(this.rapidPassiveEngine.CurrentUserID, defaultHeadImageIndex, customizedHeadImage);
            byte[] data = ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(contract);
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.ChangeMyHeadImage, data, null, 2048, handler, null, true);
            this.clientGlobalCache.UpdateUserHeadImage(this.rapidPassiveEngine.CurrentUserID, defaultHeadImageIndex, customizedHeadImage, this.clientGlobalCache.CurrentUser.Version + 1);
        }

        public void ChangeMyBusinessInfo(string key, byte[] info, bool notifyContacts, bool increaseVersion)
        {
            Dictionary<string, byte[]> businessInfo = new Dictionary<string, byte[]>();
            businessInfo.Add(key, info);
            this.ChangeMyBusinessInfo(businessInfo, notifyContacts, increaseVersion);
        }

        public void ChangeMyBusinessInfo(Dictionary<string, byte[]> businessInfo, bool notifyContacts, bool increaseVersion)
        {
            ChangeUserBusinessInfoContract contract = new ChangeUserBusinessInfoContract(this.rapidPassiveEngine.CurrentUserID, businessInfo, notifyContacts, increaseVersion);
            byte[] data = ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(contract);
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.ChangeMyBusinessInfo, data, null, 2048);
            int ver = increaseVersion ? this.clientGlobalCache.CurrentUser.Version + 1 : this.clientGlobalCache.CurrentUser.Version;
            this.clientGlobalCache.UpdateUserBusinessInfo(this.rapidPassiveEngine.CurrentUserID, businessInfo, ver);
        }

        /// <summary>
        /// 请求离线消息
        /// </summary>
        public void RequestOfflineMessage()
        {
            this.rapidPassiveEngine.CustomizeOutter.Send(this.talkBaseInfoTypes.GetOfflineMessage, null);
            this.rapidPassiveEngine.CustomizeOutter.Send(this.talkBaseInfoTypes.GetGroupOfflineMessage, null);
        }

        /// <summary>
        /// 请求离线文件
        /// </summary>
        public void RequestOfflineFile()
        {
            this.rapidPassiveEngine.CustomizeOutter.Send(this.talkBaseInfoTypes.GetOfflineFile, null);
        }

        public bool DeleteUser(string userID)
        {
            byte[] res = this.rapidPassiveEngine.CustomizeOutter.Query(this.talkBaseInfoTypes.DeleteUser, System.Text.Encoding.UTF8.GetBytes(userID));
            bool succeed = BitConverter.ToBoolean(res, 0);
            if (succeed)
            {
                this.clientGlobalCache.OnUserDeleted(userID, this.rapidPassiveEngine.CurrentUserID);
            }
            return succeed;
        }

        #endregion

        #region 手动同步用户资料
        /// <summary>
        /// 查看服务器上是否有目标用户资料的最新版本，如果有，则同步到本地。（如果有更新，本地缓存将触发UserBusinessInfoChanged事件）
        /// </summary>
        public void SyncUserBaseInfo(string userID)
        {
            if (!this.clientGlobalCache.IsUserInCache(userID))
            {
                this.clientGlobalCache.GetUser(userID); //自动从服务器加载
                return;
            }

            TUser user = this.clientGlobalCache.GetUser(userID);
            GetUserInfoNewVersionContract contact = new GetUserInfoNewVersionContract(userID, user.Version);
            byte[] bUser = this.rapidPassiveEngine.CustomizeOutter.Query(this.talkBaseInfoTypes.GetUserInfoNewVersion, CompactPropertySerializer.Default.Serialize(contact));
            if (bUser == null)
            {
                return;
            }
            TUser newVersion = CompactPropertySerializer.Default.Deserialize<TUser>(bUser, 0);
            this.clientGlobalCache.ManualSyncUserBaseInfo(newVersion);
        }

        public void SyncUserBaseInfoInBackground(string userID)
        {
            CbGeneric<string> cb = new CbGeneric<string>(this.SyncUserBaseInfo);
            cb.BeginInvoke(userID, null, null);
        }

        #endregion

        #region 群禁言

        public void SetGroupBan4User(string groupID, string destUserID, string comment, double minutes)
        {
            if (minutes <= 0)
            {
                return;
            }
            GroupBan4UserContract contract = new GroupBan4UserContract(groupID, this.clientGlobalCache.CurrentUser.ID, destUserID, comment, minutes);
            byte[] info = ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(contract);
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.GroupBan4User, info, null, true);
        }


        public void RemoveGroupBan4User(string groupID, string destUserID)
        {
            RemoveGroupBan4UserContract contract = new RemoveGroupBan4UserContract(groupID, destUserID);
            byte[] info = ESPlus.Serialization.CompactPropertySerializer.Default.Serialize(contract);
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.RemoveGroupBan4User, info, this.clientGlobalCache.CurrentUser.ID, true);
        }

        public void CheckGroupBan4CurrentUser(string groupID)
        {
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.CheckGroupBan4CurrentUser, null, groupID);
        }

        public List<GroupBan> GetGroupBans4Group(string groupID)
        {
            byte[] info = this.rapidPassiveEngine.CustomizeOutter.Query(this.talkBaseInfoTypes.GetGroupBans4Group, Encoding.UTF8.GetBytes(groupID));
            return CompactPropertySerializer.Default.Deserialize<List<GroupBan>>(info, 0);
        }

        public bool ExistAllGroupBan(string groupID)
        {
            byte[] info = this.rapidPassiveEngine.CustomizeOutter.Query(this.talkBaseInfoTypes.ExistAllGroupBan, Encoding.UTF8.GetBytes(groupID));
            return BitConverter.ToBoolean(info, 0);
        }

        public void SetAllGroupBan(string groupID)
        {
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.GroupBan4Group, null, groupID, true);
        }

        public void RemoveAllGroupBan(string groupID)
        {
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.RemoveGroupBan4Group, null, groupID, true);
        }
        #endregion

        #region 管理员
        public void ChangeUserState4Admin(string userID, UserState userState)
        {
            ChangeUserStateContract contract = new ChangeUserStateContract(userID, userState);
            this.rapidPassiveEngine.SendMessage(null, this.talkBaseInfoTypes.ChangeUserState, CompactPropertySerializer.Default.Serialize<ChangeUserStateContract>(contract), null, false);
        }

        #endregion
    }
}
