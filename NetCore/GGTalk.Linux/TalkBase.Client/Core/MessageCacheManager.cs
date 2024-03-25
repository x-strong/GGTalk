using CPF;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using ESBasic;
using ESFramework;
using ESPlus.Serialization;
using GGTalk.Linux;
using GGTalk.Linux.Helpers;
using GGTalk.Linux.Views;
using System;
using System.Collections.Generic;
using System.Text;
using TalkBase.Client;

namespace TalkBase.NetCore.Client.Core
{
    internal interface ITwinkleNotifySupporter
    {
        string GetFriendName(string friendID);
        string GetGroupName(string groupID);
        bool NeedTwinkle4User(string userID);
        bool NeedTwinkle4Group(string groupID);
        bool NeedTwinkle4AddFriendNotify();
        void PlayAudioAsyn();

        Image GetHeadIcon(string userID);
        Image Icon64 { get; }
        Image NoneIcon64 { get; }
        Image GroupIcon { get; }

        Image NotifyIcon { get; }
        Image GetStatusIcon(UserStatus status);

    }

    internal interface IMessageCacheManager {
        void Initialize(ITwinkleNotifySupporter getter, IBrige4ClientOutter outter, TalkBaseInfoTypes infoTypes);
        /// <summary>
        /// 当出现未处理的聊天消息时，在后台线程中触发此事件。参数：unitID - isGroup
        /// </summary>
        event CbGeneric<string, UnitType> UnhandleMessageOccured;
        /// <summary>
        /// 当用户点击托盘图标提取聊天消息时，或调用Pickout方法提取消息时，将触发此事件。（在触发clientOutter相关事件之后）。参数：unitID - UnitType
        /// </summary>
        event CbGeneric<string, UnitType> UnhandleMessagePickedOut;

        /// <summary>
        /// 当出现未处理的通知消息时，在后台线程中触发此事件。参数：string -  发送者ID ； int -协议号
        /// </summary>
        event CbGeneric<string, int, byte[]> UnhandleNotifyMessageOccured;
        /// <summary>
        /// 当用户点击托盘图标提取通知消息时，或调用Pickout方法提取消息时，将触发此事件。（在触发clientOutter相关事件之后）。
        /// </summary>
        event CbGeneric UnhandleNotifyMessagePickedOut;


        /// <summary>
        /// 移除指定对象 未提取的消息数
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        void RemoveUnhanleMessage(IUnit unit);



        void PushFriendMessage(string userID, ClientType sourceClientType, int informationType, byte[] info);

        void PushGroupMessage(string broadcasterID, string groupID, int broadcastType, byte[] content, string tag);

        void PushNotifyMessage(string userID, int notifyType, byte[] info);

        void PickoutNotifyMessage(string userID);

        /// <summary>
        /// 获取指定对象 未提取的消息数
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        int GetUnhanleMessageCount(IUnit unit);
        /// <summary>
        /// 提取指定用户的未处理消息。将触发UnhandleMessagePickedOut事件。
        /// </summary>       
        void PickoutFriendMessage(string userID);

        /// <summary>
        /// 提取指定组的未处理消息。将触发UnhandleMessagePickedOut事件。
        /// </summary>    
        void PickoutGroupMessage(string groupID);

        /// <summary>
        /// 改变托盘的图标状态
        /// </summary>
        /// <param name="status"></param>
        //void ChangeMyStatus(UserStatus status);

        /// <summary>
        /// 改变托盘的tips
        /// </summary>
        /// <param name="text"></param>
        void ChangeText(string text);

    }

    /// <summary>
    /// 会闪动的托盘图标。
    /// （1）缓存未处理的好友或群消息。
    /// （2）当收到未提取的聊天消息时，触发UnhandleMessageOccured事件。
    /// （3）当消息已经被提取时，触发UnhandleMessagePickedOut事件。
    /// （4）提取消息时，将其统一转接为触发clientOutter的相关事件。
    /// </summary>
    internal partial class MessageCacheManager : IMessageCacheManager
    {
        private IBrige4ClientOutter clientOutter;

        private object locker = new object();
        private List<UnhandleFriendMessageBox> friendQueue = new List<UnhandleFriendMessageBox>();
        private List<UnhandleGroupMessageBox> groupQueue = new List<UnhandleGroupMessageBox>();
        private List<UnhandleNotifyMessageBox> notifyQueue = new List<UnhandleNotifyMessageBox>();
        private ITwinkleNotifySupporter twinkleNotifySupporter;
        private TalkBaseInfoTypes talkBaseInfoTypes;

        /// <summary>
        /// 当出现未处理的聊天消息时，在后台线程中触发此事件。参数：unitID - isGroup
        /// </summary>
        public event CbGeneric<string, UnitType> UnhandleMessageOccured;
        /// <summary>
        /// 当用户点击托盘图标提取聊天消息时，或调用Pickout方法提取消息时，将触发此事件。（在触发clientOutter相关事件之后）。参数：unitID - UnitType
        /// </summary>
        public event CbGeneric<string, UnitType> UnhandleMessagePickedOut;

        /// <summary>
        /// 当出现未处理的通知消息时，在后台线程中触发此事件。参数：string -  发送者ID ； int -协议号
        /// </summary>
        public event CbGeneric<string,int,byte[]> UnhandleNotifyMessageOccured;
        /// <summary>
        /// 当用户点击托盘图标提取通知消息时，或调用Pickout方法提取消息时，将触发此事件。（在触发clientOutter相关事件之后）。
        /// </summary>
        public event CbGeneric UnhandleNotifyMessagePickedOut;
        


        public void Initialize(ITwinkleNotifySupporter getter, IBrige4ClientOutter outter, TalkBaseInfoTypes infoTypes)
        {
            this.twinkleNotifySupporter = getter;
            this.clientOutter = outter;
            this.talkBaseInfoTypes = infoTypes;
        }

        private void Message4Group(string broadcasterID, string groupID, int broadcastType, byte[] content, string tag)
        {
            if (broadcastType == this.talkBaseInfoTypes.GroupChat)
            {
                this.clientOutter.OnGroupChatMessageReceived(broadcasterID, groupID, content, tag, null);
                return;
            }

            if (broadcastType == this.talkBaseInfoTypes.GroupFileUploadedNotify)
            {
                string fileName = System.Text.Encoding.UTF8.GetString(content);
                this.clientOutter.OnGroupFileUploadedNotifyReceived(broadcasterID, groupID, fileName);
                return;
            }

            if (broadcastType == this.talkBaseInfoTypes.OfflineMessage)
            {
                OfflineMessage msg = CompactPropertySerializer.Default.Deserialize<OfflineMessage>(content, 0);
                if (msg.InformationType == this.talkBaseInfoTypes.GroupChat)
                {
                    this.clientOutter.OnGroupChatMessageReceived(msg.SourceUserID, msg.GroupID, msg.Information, msg.Tag, msg.TimeTransfer);
                }
                return;
            }
            /*     if (broadcastType == this.talkBaseInfoTypes.GroupOfflineMessage)
                 {
                     this.clientOutter.OnGroupChatMessageReceived(broadcasterID, groupID, content, "", Convert.ToDateTime(tag));
                     return;
                 }*/
        }

        private void Message4User(string sourceUserID, ClientType srcClientType, int informationType, byte[] info)
        {
            if (informationType == this.talkBaseInfoTypes.Chat)
            {
                this.clientOutter.OnChatMessageReceived(sourceUserID, srcClientType, info, null);
                return;
            }
            if (informationType == this.talkBaseInfoTypes.Snapchat)
            {
                this.clientOutter.OnSnapchatMessageReceived(sourceUserID, srcClientType, info, null);
                return;
            }

            if (informationType == this.talkBaseInfoTypes.AudioMessage)
            {
                //AudioMessage msg = CompactPropertySerializer.Default.Deserialize<AudioMessage>(info, 0);
                //this.clientOutter.OnAudioMessageReceived(msg, null);
                //return;
            }

            if (informationType == this.talkBaseInfoTypes.MediaCommunicate)
            {
                MediaCommunicateContract contract = CompactPropertySerializer.Default.Deserialize<MediaCommunicateContract>(info, 0);
                this.clientOutter.OnMediaCommunicateReceived(sourceUserID, srcClientType, contract.CommunicateMediaType, contract.CommunicateType, contract.Tag);
                return;
            }

            if (informationType == this.talkBaseInfoTypes.FriendAddedNotify)
            {
                this.clientOutter.OnFriendAdded(sourceUserID);
                return;
            }

            if (informationType == this.talkBaseInfoTypes.OfflineMessage)
            {
                OfflineMessage msg = CompactPropertySerializer.Default.Deserialize<OfflineMessage>(info, 0);
                if (msg.InformationType == this.talkBaseInfoTypes.Chat)
                {
                    this.clientOutter.OnChatMessageReceived(msg.SourceUserID, msg.SourceType, msg.Information, msg.TimeTransfer);
                }
                if (msg.InformationType == this.talkBaseInfoTypes.Snapchat)
                {
                    this.clientOutter.OnSnapchatMessageReceived(msg.SourceUserID, msg.SourceType, msg.Information, msg.TimeTransfer);
                    return;
                }
                if (msg.InformationType == this.talkBaseInfoTypes.AudioMessage)
                {
                    //AudioMessage audioMessage = CompactPropertySerializer.Default.Deserialize<AudioMessage>(msg.Information, 0);
                    //this.clientOutter.OnAudioMessageReceived(audioMessage, msg.Time);
                }

                return;
            }

            if (informationType == this.talkBaseInfoTypes.OfflineFileResultNotify)
            {
                OfflineFileResultNotifyContract contract = CompactPropertySerializer.Default.Deserialize<OfflineFileResultNotifyContract>(info, 0);
                this.clientOutter.OnOfflineFileResultReceived(contract.AccepterID, contract.FileName, contract.Accept);
                return;
            }
        }

        private void Message4Notify(int notifyType, byte[] info, string tag)
        {
            if (notifyType == this.talkBaseInfoTypes.RequestAddFriend)
            {
                RequestAddFriendContract contract = CompactPropertySerializer.Default.Deserialize<RequestAddFriendContract>(info, 0);
                this.clientOutter.OnAddFriendRequestReceived(contract.RequesterID, contract.Comment);
                return;
            }
            if (notifyType == this.talkBaseInfoTypes.HandleAddFriendRequest)
            {
                HandleAddFriendRequestContract contract = CompactPropertySerializer.Default.Deserialize<HandleAddFriendRequestContract>(info, 0);
                this.clientOutter.OnHandleAddFriendRequestReceived(contract.AccepterID,false, contract.IsAgreed);
                return;
            }
            if (notifyType == this.talkBaseInfoTypes.RequestAddGroup)
            {
                RequestAddGroupContract contract = CompactPropertySerializer.Default.Deserialize<RequestAddGroupContract>(info, 0);
                this.clientOutter.OnAddGroupRequestReceived(contract.RequesterID, contract.GroupID, contract.Comment);
                return;
            }
            if (notifyType == this.talkBaseInfoTypes.HandleAddGroupRequest)
            {
                HandleAddGroupRequestContract contract = CompactPropertySerializer.Default.Deserialize<HandleAddGroupRequestContract>(info, 0);
                this.clientOutter.OnHandleAddGroupRequestReceived(contract.RequesterID, contract.GroupID, contract.IsAgreed);
                return;
            }
        }



        private string normalText = null;


        void notifyIcon1_MouseClick()
        {
            //0920
            try
            {
                lock (this.locker)
                {
                    if (this.friendQueue.Count > 0)
                    {
                        UnhandleFriendMessageBox cache = this.friendQueue[0];
                        this.friendQueue.RemoveAt(0);
                        foreach (Parameter<ClientType, int, byte[]> para in cache.MessageList)
                        {
                            this.Message4User(cache.User, para.Arg1, para.Arg2, para.Arg3);
                        }

                        this.DetectUnhandleMessage();

                        if (this.UnhandleMessagePickedOut != null)
                        {
                            this.UnhandleMessagePickedOut(cache.User, UnitType.User);
                        }
                        return;
                    }

                    if (this.groupQueue.Count > 0)
                    {
                        UnhandleGroupMessageBox cache = this.groupQueue[0];
                        this.groupQueue.RemoveAt(0);
                        foreach (Parameter<string, int, byte[], string> para in cache.MessageList)
                        {
                            this.Message4Group(para.Arg1, cache.Group, para.Arg2, para.Arg3, para.Arg4);
                        }

                        this.DetectUnhandleMessage();

                        if (this.UnhandleMessagePickedOut != null)
                        {
                            this.UnhandleMessagePickedOut(cache.Group, UnitType.Group);
                        }
                        return;
                    }

                    if (this.notifyQueue.Count > 0)
                    {
                        UnhandleNotifyMessageBox cache = this.notifyQueue[0];
                        this.notifyQueue.RemoveAt(0);
                        foreach (Parameter<string, int, byte[]> para in cache.MessageList)
                        {
                            this.Message4Notify(para.Arg2, para.Arg3, para.Arg1);
                        }

                        this.DetectUnhandleMessage();
                        return;
                    }
                }
            }
            catch (Exception ee)
            {
                //MessageBox.Show(ee.Message + " - " + ee.StackTrace);
            }
        }

        public void ChangeMyStatus(UserStatus status)
        {
            
        }

        public void ChangeText(string text)
        {
        }

        #region PushFriendMessage
        public void PushFriendMessage(string userID, ClientType sourceClientType, int informationType, byte[] info)
        { 
            //0920
            lock (this.locker)
            {
                try
                { 
                    this.twinkleNotifySupporter.PlayAudioAsyn();
                    if (!this.twinkleNotifySupporter.NeedTwinkle4User(userID))
                    {
                        this.Message4User(userID, sourceClientType, informationType, info);
                        return;
                    }

                    UnhandleFriendMessageBox cache = null;
                    lock (this.locker)
                    {
                        for (int i = 0; i < this.friendQueue.Count; i++)
                        {
                            if (this.friendQueue[i].User == userID)
                            {
                                cache = this.friendQueue[i];
                                break;
                            }
                        }

                        if (cache == null)
                        {
                            cache = new UnhandleFriendMessageBox(userID);
                            this.friendQueue.Add(cache);
                            if (this.UnhandleMessageOccured != null)
                            {
                                this.UnhandleMessageOccured(userID, UnitType.User);
                            }
                        }

                        cache.MessageList.Add(new Parameter<ClientType, int, byte[]>(sourceClientType, informationType, info));
                    }

                    string userName = this.twinkleNotifySupporter.GetFriendName(userID);
 
                }
                catch (Exception ee)
                {
                    //MessageBox.Show(ee.Message);
                }
            }
                 
        }
        #endregion

        #region PushGroupMessage
        public void PushGroupMessage(string broadcasterID, string groupID, int broadcastType, byte[] content, string tag)
        {
            //0920lock (this.locker)
            { 
                this.twinkleNotifySupporter.PlayAudioAsyn();
                if (!this.twinkleNotifySupporter.NeedTwinkle4Group(groupID))
                {
                    this.Message4Group(broadcasterID, groupID, broadcastType, content, tag);
                    return;
                }

                UnhandleGroupMessageBox cache = null;
                lock (this.locker)
                {
                    for (int i = 0; i < this.groupQueue.Count; i++)
                    {
                        if (this.groupQueue[i].Group == groupID)
                        {
                            cache = this.groupQueue[i];
                            break;
                        }
                    }

                    if (cache == null)
                    {
                        cache = new UnhandleGroupMessageBox(groupID);
                        this.groupQueue.Add(cache);
                        if (this.UnhandleMessageOccured != null)
                        {
                            this.UnhandleMessageOccured(groupID, UnitType.Group);
                        }
                    }

                    cache.MessageList.Add(new Parameter<string, int, byte[], string>(broadcasterID, broadcastType, content, tag));
                }
                string groupName = this.twinkleNotifySupporter.GetGroupName(groupID);
            }
        }
        #endregion
        
        #region PushNotifyMessage
        public void PushNotifyMessage(string userID, int notifyType, byte[] info)
        {
            //2019.08.21
            lock (this.locker)
            {
                try
                {
                    this.twinkleNotifySupporter.PlayAudioAsyn();
                    if (!this.twinkleNotifySupporter.NeedTwinkle4AddFriendNotify())
                    {
                        this.Message4Notify(notifyType, info, userID);
                        return;
                    }

                    UnhandleNotifyMessageBox cache = null;
                    lock (this.locker)
                    {
                        for (int i = 0; i < this.notifyQueue.Count; i++)
                        {
                            if (this.notifyQueue[i].UserID == userID)
                            {
                                cache = this.notifyQueue[i];
                                break;
                            }
                        }

                        if (cache == null)
                        {
                            cache = new UnhandleNotifyMessageBox(userID);
                            this.notifyQueue.Add(cache);
                            if (this.UnhandleNotifyMessageOccured != null)
                            {

                                this.UnhandleNotifyMessageOccured(userID,notifyType,info);
                            }
                        }

                        cache.MessageList.Add(new Parameter<string, int, byte[]>(userID, notifyType, info));
                    }
                }
                catch (Exception ee)
                {
                    //MessageBox.Show(ee.Message);
                }
            }
        }



        #endregion

        #region PickoutFriendMessage
        /// <summary>
        /// 提取指定用户的未处理消息。将触发UnhandleMessagePickedOut事件。
        /// </summary>       
        public void PickoutFriendMessage(string userID)
        {
            lock (this.locker)
            {
                for (int i = 0; i < this.friendQueue.Count; i++)
                {
                    UnhandleFriendMessageBox tmp = this.friendQueue[i];
                    if (tmp.User == userID)
                    {
                        this.friendQueue.RemoveAt(i);
                        //foreach (Parameter<ClientType, int, byte[]> para in tmp.MessageList)
                        //{
                        //    this.Message4User(tmp.User, para.Arg1, para.Arg2, para.Arg3);
                        //}
                        this.DetectUnhandleMessage();
                        if (this.UnhandleMessagePickedOut != null)
                        {
                            this.UnhandleMessagePickedOut(userID, UnitType.User);
                        }
                        return;
                    }
                }
            }
        }

        public int GetUnhanleMessageCount(IUnit unit)
        {
            int count = 0;

            if (unit.UnitType == UnitType.User)
            {
                for (int i = 0; i < this.friendQueue.Count; i++)
                {
                    UnhandleFriendMessageBox tmp = this.friendQueue[i];
                    if (tmp.User == unit.ID)
                    {
                        return tmp.MessageList.Count;                        
                    }
                }
            }
            else if (unit.UnitType == UnitType.Group)
            {
                for (int i = 0; i < this.groupQueue.Count; i++)
                {
                    UnhandleGroupMessageBox tmp = this.groupQueue[i];
                    if (tmp.Group == unit.ID)
                    {
                        return tmp.MessageList.Count;
                    }
                }
            }  

            return count;
        }

        /// <summary>
        /// 移除未读消息（删除好友、群组时直接移除该未读消息）
        /// </summary>
        /// <param name="unit"></param>
        public void RemoveUnhanleMessage(IUnit unit)
        {
            if (unit == null) { return; }
            if (unit.UnitType == UnitType.User)
            {
                for (int i = 0; i < this.friendQueue.Count; i++)
                {
                    UnhandleFriendMessageBox tmp = this.friendQueue[i];
                    if (tmp.User == unit.ID)
                    {
                        this.friendQueue.RemoveAt(i);
                        return;
                    }
                }
            }
            else if (unit.UnitType == UnitType.Group)
            {
                for (int i = 0; i < this.groupQueue.Count; i++)
                {
                    UnhandleGroupMessageBox tmp = this.groupQueue[i];
                    if (tmp.Group == unit.ID)
                    {
                        this.groupQueue.RemoveAt(i);
                        return;
                    }
                }
            }
        }
        #endregion

        #region PickoutGroupMessage
        /// <summary>
        /// 提取指定组的未处理消息。将触发UnhandleMessagePickedOut事件。
        /// </summary>    
        public void PickoutGroupMessage(string groupID)
        {
            lock (this.locker)
            {
                for (int i = 0; i < this.groupQueue.Count; i++)
                {
                    UnhandleGroupMessageBox tmp = this.groupQueue[i];
                    if (tmp.Group == groupID)
                    {
                        this.groupQueue.RemoveAt(i);
                        //foreach (Parameter<string, int, byte[], string> para in tmp.MessageList)
                        //{
                        //    this.Message4Group(para.Arg1, groupID, para.Arg2, para.Arg3, para.Arg4);
                        //}
                        this.DetectUnhandleMessage();
                        if (this.UnhandleMessagePickedOut != null)
                        {
                            this.UnhandleMessagePickedOut(groupID, UnitType.Group);
                        }
                        return;
                    }
                }
            }


        }
        #endregion        

        #region PickoutNotifyMessage
        public void PickoutNotifyMessage(string userID)
        {
            lock (this.locker)
            {
                this.notifyQueue.Clear();
                //for (int i = 0; i < this.notifyQueue.Count; i++)
                //{
                //    UnhandleNotifyMessageBox tmp = this.notifyQueue[i];
                //if (tmp.UserID == userID)
                //{
                //this.notifyQueue.RemoveAt(i);
                //foreach (Parameter<string, int, byte[]> para in tmp.MessageList)
                //{
                //    this.Message4Notify(para.Arg2, para.Arg3, para.Arg1);
                //}
                this.DetectUnhandleMessage();
                        if (this.UnhandleNotifyMessagePickedOut != null)
                        {
                            this.UnhandleNotifyMessagePickedOut();
                        }
                        //return;
                    //}
                //}
            }
        }
        #endregion

        #region DetectUnhandleMessage
        private void DetectUnhandleMessage()
        {
            if (this.friendQueue.Count == 0 && this.groupQueue.Count == 0 && this.notifyQueue.Count == 0)
            {

            }
            else if (this.friendQueue.Count > 0)
            {
                UnhandleFriendMessageBox cache = this.friendQueue[0];
                string userName = this.twinkleNotifySupporter.GetFriendName(cache.User);

            }
            else if (this.groupQueue.Count > 0)
            {
                UnhandleGroupMessageBox cache = this.groupQueue[0];
                string groupName = this.twinkleNotifySupporter.GetGroupName(cache.Group);

            }
            else if (this.notifyQueue.Count > 0)
            {
                UnhandleNotifyMessageBox cache = this.notifyQueue[0];

            }
        }
        #endregion
    }


    /// <summary>
    /// 会闪动的托盘图标。
    /// （1）缓存未处理的好友或群消息。
    /// （2）当收到未提取的聊天消息时，触发UnhandleMessageOccured事件。
    /// （3）当消息已经被提取时，触发UnhandleMessagePickedOut事件。
    /// （4）提取消息时，将其统一转接为触发clientOutter的相关事件。
    /// </summary>
    internal partial class TwinkleNotifyIcon : Control, IMessageCacheManager
    {
        private IBrige4ClientOutter clientOutter;
        private object locker = new object();
        private List<UnhandleFriendMessageBox> friendQueue = new List<UnhandleFriendMessageBox>();
        private List<UnhandleGroupMessageBox> groupQueue = new List<UnhandleGroupMessageBox>();
        private List<UnhandleNotifyMessageBox> notifyQueue = new List<UnhandleNotifyMessageBox>();
        private ITwinkleNotifySupporter twinkleNotifySupporter;
        private System.Threading.Timer timer;
        private TalkBaseInfoTypes talkBaseInfoTypes;

        /// <summary>
        /// 当出现未处理的聊天消息时，在后台线程中触发此事件。参数：unitID - isGroup
        /// </summary>
        public event CbGeneric<string, UnitType> UnhandleMessageOccured;
        /// <summary>
        /// 当用户点击托盘图标提取聊天消息时，或调用Pickout方法提取消息时，将触发此事件。（在触发clientOutter相关事件之后）。参数：unitID - UnitType
        /// </summary>
        public event CbGeneric<string, UnitType> UnhandleMessagePickedOut;

        /// <summary>
        /// 当出现未处理的通知消息时，在后台线程中触发此事件。参数：string -  发送者ID ； int -协议号
        /// </summary>
        public event CbGeneric<string, int, byte[]> UnhandleNotifyMessageOccured;
        /// <summary>
        /// 当用户点击托盘图标提取通知消息时，或调用Pickout方法提取消息时，将触发此事件。（在触发clientOutter相关事件之后）。
        /// </summary>
        public event CbGeneric UnhandleNotifyMessagePickedOut;

        public event EventHandler MouseClick;
        NotifyIcon notifyIcon1 = new CPF.Controls.NotifyIcon();

        public NotifyIcon NotifyIcon { get { return this.notifyIcon1; } }
        public TwinkleNotifyIcon()
        {
            InitializeComponent();

        }

        public void Initialize(ITwinkleNotifySupporter getter, IBrige4ClientOutter outter, TalkBaseInfoTypes infoTypes)
        {
            this.twinkleNotifySupporter = getter;
            this.clientOutter = outter;
            this.talkBaseInfoTypes = infoTypes;
            this.timer = new System.Threading.Timer(new System.Threading.TimerCallback(timer_Tick), null, -1, 500);


            this.normalIcon = this.twinkleNotifySupporter.Icon64;
            this.notifyIcon1.Icon = this.normalIcon;
            this.notifyIcon1.MouseDown += new EventHandler(notifyIcon1_MouseClick);
            this.notifyIcon1.Visible = true;
        }

        private void Message4Group(string broadcasterID, string groupID, int broadcastType, byte[] content, string tag)
        {
            if (broadcastType == this.talkBaseInfoTypes.GroupChat)
            {
                this.clientOutter.OnGroupChatMessageReceived(broadcasterID, groupID, content, tag, null);
                return;
            }

            if (broadcastType == this.talkBaseInfoTypes.GroupFileUploadedNotify)
            {
                string fileName = System.Text.Encoding.UTF8.GetString(content);
                this.clientOutter.OnGroupFileUploadedNotifyReceived(broadcasterID, groupID, fileName);
                return;
            }

            if (broadcastType == this.talkBaseInfoTypes.OfflineMessage)
            {
                OfflineMessage msg = CompactPropertySerializer.Default.Deserialize<OfflineMessage>(content, 0);
                if (msg.InformationType == this.talkBaseInfoTypes.GroupChat)
                {
                    this.clientOutter.OnGroupChatMessageReceived(msg.SourceUserID, msg.GroupID, msg.Information, msg.Tag, msg.TimeTransfer);
                }
                return;
            }
            /*     if (broadcastType == this.talkBaseInfoTypes.GroupOfflineMessage)
                 {
                     this.clientOutter.OnGroupChatMessageReceived(broadcasterID, groupID, content, "", Convert.ToDateTime(tag));
                     return;
                 }*/
        }

        private void Message4User(string sourceUserID, ClientType srcClientType, int informationType, byte[] info)
        {
            if (informationType == this.talkBaseInfoTypes.Chat)
            {
                this.clientOutter.OnChatMessageReceived(sourceUserID, srcClientType, info, null);
                return;
            }
            if (informationType == this.talkBaseInfoTypes.Snapchat)
            {
                this.clientOutter.OnSnapchatMessageReceived(sourceUserID, srcClientType, info, null);
                return;
            }

            //if (informationType == this.talkBaseInfoTypes.AudioMessage)
            //{
            //    AudioMessage msg = CompactPropertySerializer.Default.Deserialize<AudioMessage>(info, 0);
            //    this.clientOutter.OnAudioMessageReceived(msg, null);
            //    return;
            //}

            if (informationType == this.talkBaseInfoTypes.MediaCommunicate)
            {
                MediaCommunicateContract contract = CompactPropertySerializer.Default.Deserialize<MediaCommunicateContract>(info, 0);
                this.clientOutter.OnMediaCommunicateReceived(sourceUserID, srcClientType, contract.CommunicateMediaType, contract.CommunicateType, contract.Tag);
                return;
            }

            if (informationType == this.talkBaseInfoTypes.FriendAddedNotify)
            {
                this.clientOutter.OnFriendAdded(sourceUserID);
                return;
            }

            if (informationType == this.talkBaseInfoTypes.OfflineMessage)
            {
                OfflineMessage msg = CompactPropertySerializer.Default.Deserialize<OfflineMessage>(info, 0);
                if (msg.InformationType == this.talkBaseInfoTypes.Chat)
                {
                    this.clientOutter.OnChatMessageReceived(msg.SourceUserID, msg.SourceType, msg.Information, msg.TimeTransfer);
                }
                if (msg.InformationType == this.talkBaseInfoTypes.Snapchat)
                {
                    this.clientOutter.OnSnapchatMessageReceived(msg.SourceUserID, msg.SourceType, msg.Information, msg.TimeTransfer);
                    return;
                }
                //if (msg.InformationType == this.talkBaseInfoTypes.AudioMessage)
                //{
                //    AudioMessage audioMessage = CompactPropertySerializer.Default.Deserialize<AudioMessage>(msg.Information, 0);
                //    this.clientOutter.OnAudioMessageReceived(audioMessage, msg.TimeTransfer);
                //}

                return;
            }

            if (informationType == this.talkBaseInfoTypes.OfflineFileResultNotify)
            {
                OfflineFileResultNotifyContract contract = CompactPropertySerializer.Default.Deserialize<OfflineFileResultNotifyContract>(info, 0);
                this.clientOutter.OnOfflineFileResultReceived(contract.AccepterID, contract.FileName, contract.Accept);
                return;
            }
        }

        private void Message4Notify(int notifyType, byte[] info, string tag)
        {
            if (notifyType == this.talkBaseInfoTypes.RequestAddFriend)
            {
                RequestAddFriendContract contract = CompactPropertySerializer.Default.Deserialize<RequestAddFriendContract>(info, 0);
                this.clientOutter.OnAddFriendRequestReceived(contract.RequesterID, contract.Comment);
                return;
            }
            if (notifyType == this.talkBaseInfoTypes.HandleAddFriendRequest)
            {
                HandleAddFriendRequestContract contract = CompactPropertySerializer.Default.Deserialize<HandleAddFriendRequestContract>(info, 0);
                this.clientOutter.OnHandleAddFriendRequestReceived(contract.AccepterID, false, contract.IsAgreed);
                return;
            }
            if (notifyType == this.talkBaseInfoTypes.RequestAddGroup)
            {
                RequestAddGroupContract contract = CompactPropertySerializer.Default.Deserialize<RequestAddGroupContract>(info, 0);
                this.clientOutter.OnAddGroupRequestReceived(contract.RequesterID, contract.GroupID, contract.Comment);
                return;
            }
            if (notifyType == this.talkBaseInfoTypes.HandleAddGroupRequest)
            {
                HandleAddGroupRequestContract contract = CompactPropertySerializer.Default.Deserialize<HandleAddGroupRequestContract>(info, 0);
                this.clientOutter.OnHandleAddGroupRequestReceived(contract.RequesterID, contract.GroupID, contract.IsAgreed);
                return;
            }
        }

        private void ControlTimer(bool start)
        {
            GGTalk.Linux.Helpers.UiSafeInvoker.ActionOnUI<bool>(this.doControlTimer, start);
        }

        private void doControlTimer(bool start)
        {
            if (start)
            {
                this.timer.Change(0, 500);
                this.timerEnabled = true;
            }
            else
            {
                this.timer.Change(-1, 1000000);
                this.timerEnabled = false;
                //2014.11.05
                this.notifyIcon1.Icon = this.normalIcon;
                this.notifyIcon1.Text = this.normalText;
            }
        }

        private bool timerEnabled = false;

        private Image twinkleIcon = null;
        private Image normalIcon;
        public void ChangeMyStatus(UserStatus status)
        {
            this.normalIcon = this.twinkleNotifySupporter.GetStatusIcon(status);
            if (!this.timerEnabled)
            {
                this.notifyIcon1.Icon = this.normalIcon;
            }
        }

        private string normalText = null;
        public void ChangeText(string text)
        {
            this.normalText = text;
            if (!this.timerEnabled)
            {
                this.notifyIcon1.Text = this.normalText;
            }
        }

        void notifyIcon1_MouseClick(object sender, EventArgs e)
        {
            //0920
            try
            {

                GGTalk.Linux.Helpers.UiSafeInvoker.ActionOnUI(() => {
                    LoginWindow.MainWindow.Show_Topmost();
                    if(LoginWindow.MainWindow.WindowState == WindowState.Minimized)
                    {
                        LoginWindow.MainWindow.WindowState = WindowState.Normal;
                    }
                });
                CPF.Input.NotifyIconMouseEventArgs eventArgs = e as CPF.Input.NotifyIconMouseEventArgs;
                if (eventArgs == null || eventArgs.Button != MouseButton.Left)
                {
                    return;
                }
                this.PickUnReadMSgForTray();

                if (this.MouseClick != null)
                {
                    this.MouseClick(sender, e);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message + " - " + ee.StackTrace);
            }
        }

        internal void PickUnReadMSgForTray()
        {
            lock (this.locker)
            {
                if (this.friendQueue.Count > 0)
                { 
                    //UiSafeInvoker.ActionOnUI(() =>
                    //{ 
                        UnhandleFriendMessageBox cache = this.friendQueue[0];
                        MainWindow.ChatFormController.GetForm(cache.User);
                    //});
                    return;
                }

                if (this.groupQueue.Count > 0)
                {
                    //UiSafeInvoker.ActionOnUI(() =>
                    //{
                        UnhandleGroupMessageBox cache = this.groupQueue[0];
                        MainWindow.ChatFormController.GetForm(cache.Group);
                    //});
                    return;
                }

                if (this.notifyQueue.Count > 0)
                {
                    UnhandleNotifyMessageBox cache = this.notifyQueue[0];
                    this.notifyQueue.RemoveAt(0);
                    foreach (Parameter<string, int, byte[]> para in cache.MessageList)
                    {
                        this.Message4Notify(para.Arg2, para.Arg3, para.Arg1);
                    }

                    this.DetectUnhandleMessage();
                    return;
                }
            }
        }


        internal void PickUnReadMSgForUnitID(string unitID)
        {
            lock (this.locker)
            {
                if (this.friendQueue.Count > 0)
                { 
                    UnhandleFriendMessageBox cache = this.friendQueue.Find(x => x.User == unitID);
                    if (cache != null)
                    {
                        //UiSafeInvoker.ActionOnUI(() =>
                        //{
                            foreach (Parameter<ClientType, int, byte[]> para in cache.MessageList)
                            {
                                this.Message4User(cache.User, para.Arg1, para.Arg2, para.Arg3);
                            }
                            if (this.UnhandleMessagePickedOut != null)
                            {
                                this.UnhandleMessagePickedOut(cache.User, UnitType.User);
                            }
                            this.DetectUnhandleMessage();
                            IUnit unit = Program.ResourceCenter.ClientGlobalCache.GetUnit(unitID);
                            this.RemoveUnhanleMessage(unit);
                        //});
                        return;
                    }
                }

                if (this.groupQueue.Count > 0)
                {
                    UnhandleGroupMessageBox cache = this.groupQueue.Find(x => x.Group == unitID);
                    if (cache != null)
                    {
                        //UiSafeInvoker.ActionOnUI(() =>
                        //{
                            foreach (Parameter<string, int, byte[], string> para in cache.MessageList)
                            {
                                this.Message4Group(para.Arg1, cache.Group, para.Arg2, para.Arg3, para.Arg4);
                            } 
                            if (this.UnhandleMessagePickedOut != null)
                            {
                                this.UnhandleMessagePickedOut(cache.Group, UnitType.Group);
                            }
                            this.DetectUnhandleMessage();
                            IUnit unit = Program.ResourceCenter.ClientGlobalCache.GetUnit(unitID);
                            this.RemoveUnhanleMessage(unit);
                        //});
                        return;
                    }
                } 
            }
        }

        void timer_Tick(object sender)
        {
            if (this.friendQueue.Count == 0 && this.groupQueue.Count == 0 && this.notifyQueue.Count == 0)
            {
                this.ControlTimer(false);
                return;
            }

            if (this.notifyIcon1.Icon == this.twinkleNotifySupporter.NoneIcon64)
            {
                this.notifyIcon1.Icon = this.twinkleIcon;
            }
            else
            {
                this.notifyIcon1.Icon = this.twinkleNotifySupporter.NoneIcon64;
            }
        }

        #region Property
        public ContextMenu ContextMenuStrip
        {
            get
            {
                return this.notifyIcon1.ContextMenu;
            }
            set
            {
                this.notifyIcon1.ContextMenu = value;
            }
        }

        public bool Visible
        {
            get
            {
                return this.notifyIcon1.Visible;
            }
            set
            {
                this.notifyIcon1.Visible = value;
            }
        }
        #endregion

        #region PushFriendMessage
        public void PushFriendMessage(string userID, ClientType sourceClientType, int informationType, byte[] info)
        {
            //0920
            lock (this.locker)
            {
                try
                { 
                    this.twinkleNotifySupporter.PlayAudioAsyn();
                    if (!this.twinkleNotifySupporter.NeedTwinkle4User(userID))
                    {
                        this.Message4User(userID, sourceClientType, informationType, info);
                        return;
                    }
                    //if (informationType == this.talkBaseInfoTypes.Chat)
                    //{
                    //    CommonHelper.UnReadHandle(userID, userID, info);
                    //    //return;
                    //}

                    UnhandleFriendMessageBox cache = null;
                    lock (this.locker)
                    {
                        for (int i = 0; i < this.friendQueue.Count; i++)
                        {
                            if (this.friendQueue[i].User == userID)
                            {
                                cache = this.friendQueue[i];
                                break;
                            }
                        }

                        if (cache == null)
                        {
                            cache = new UnhandleFriendMessageBox(userID);
                            this.friendQueue.Add(cache);
                            if (this.UnhandleMessageOccured != null)
                            {
                                this.UnhandleMessageOccured(userID, UnitType.User);
                            }
                        }

                        cache.MessageList.Add(new Parameter<ClientType, int, byte[]>(sourceClientType, informationType, info));
                    }

                    string userName = this.twinkleNotifySupporter.GetFriendName(userID);
                    this.notifyIcon1.Text = string.Format("{0}({1})  {2}条消息", userName, userID, cache.MessageList.Count);
                    this.twinkleIcon = this.twinkleNotifySupporter.GetHeadIcon(userID);
                    this.ControlTimer(true);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        } 
         
        #endregion

        #region PushGroupMessage
        public void PushGroupMessage(string broadcasterID, string groupID, int broadcastType, byte[] content, string tag)
        { 
            lock (this.locker)
            { 
                this.twinkleNotifySupporter.PlayAudioAsyn();
                if (!this.twinkleNotifySupporter.NeedTwinkle4Group(groupID))
                {
                    this.Message4Group(broadcasterID, groupID, broadcastType, content, tag);
                    return;
                }

                //if (broadcastType == this.talkBaseInfoTypes.GroupChat)
                //{
                //    CommonHelper.UnReadHandle(groupID, broadcasterID, content);
                //    return;
                //}
                UnhandleGroupMessageBox cache = null;
                lock (this.locker)
                {
                    for (int i = 0; i < this.groupQueue.Count; i++)
                    {
                        if (this.groupQueue[i].Group == groupID)
                        {
                            cache = this.groupQueue[i];
                            break;
                        }
                    }

                    if (cache == null)
                    {
                        cache = new UnhandleGroupMessageBox(groupID);
                        this.groupQueue.Add(cache);
                        if (this.UnhandleMessageOccured != null)
                        {
                            this.UnhandleMessageOccured(groupID, UnitType.Group);
                        }
                    }

                    cache.MessageList.Add(new Parameter<string, int, byte[], string>(broadcasterID, broadcastType, content, tag));
                }
                string groupName = this.twinkleNotifySupporter.GetGroupName(groupID);
                this.notifyIcon1.Text = string.Format("{0}({1})  {2}条消息", groupName, groupID, cache.MessageList.Count);
                this.twinkleIcon = this.twinkleNotifySupporter.GroupIcon;
                this.ControlTimer(true);
            } 
        }
        #endregion      

        #region PushNotifyMessage
        public void PushNotifyMessage(string userID, int notifyType, byte[] info)
        {
            //2019.08.21
            lock (this.locker)
            {
                try
                {
                    this.twinkleNotifySupporter.PlayAudioAsyn();
                    if (!this.twinkleNotifySupporter.NeedTwinkle4AddFriendNotify())
                    {
                        this.Message4Notify(notifyType, info, userID);
                        return;
                    }

                    UnhandleNotifyMessageBox cache = null;
                    lock (this.locker)
                    {
                        for (int i = 0; i < this.notifyQueue.Count; i++)
                        {
                            if (this.notifyQueue[i].UserID == userID)
                            {
                                cache = this.notifyQueue[i];
                                break;
                            }
                        }

                        if (cache == null)
                        {
                            cache = new UnhandleNotifyMessageBox(userID);
                            this.notifyQueue.Add(cache);
                            if (this.UnhandleNotifyMessageOccured != null)
                            {

                                this.UnhandleNotifyMessageOccured(userID, notifyType, info);
                            }
                        }

                        cache.MessageList.Add(new Parameter<string, int, byte[]>(userID, notifyType, info));
                        this.twinkleIcon = this.twinkleNotifySupporter.NotifyIcon;
                        this.ControlTimer(true);
                    }
                }
                catch (Exception ee)
                {
                    //MessageBox.Show(ee.Message);
                }
            }
        }

        #endregion

        #region PickoutFriendMessage
        /// <summary>
        /// 提取指定用户的未处理消息。将触发UnhandleMessagePickedOut事件。
        /// </summary>       
        public void PickoutFriendMessage(string userID)
        {
            lock (this.locker)
            {
                for (int i = 0; i < this.friendQueue.Count; i++)
                {
                    UnhandleFriendMessageBox tmp = this.friendQueue[i];
                    if (tmp.User == userID)
                    {
                        this.friendQueue.RemoveAt(i);
                        //foreach (Parameter<ClientType, int, byte[]> para in tmp.MessageList)
                        //{
                        //    this.Message4User(tmp.User, para.Arg1, para.Arg2, para.Arg3);
                        //}
                        this.DetectUnhandleMessage();
                        if (this.UnhandleMessagePickedOut != null)
                        {
                            this.UnhandleMessagePickedOut(userID, UnitType.User);
                        }
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定对象 未提取的消息数
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public int GetUnhanleMessageCount(IUnit unit)
        {
            int count = 0;

            if (unit.UnitType == UnitType.User)
            {
                for (int i = 0; i < this.friendQueue.Count; i++)
                {
                    UnhandleFriendMessageBox tmp = this.friendQueue[i];
                    if (tmp.User == unit.ID)
                    {
                        return tmp.MessageList.Count;
                    }
                }
            }
            else if (unit.UnitType == UnitType.Group)
            {
                for (int i = 0; i < this.groupQueue.Count; i++)
                {
                    UnhandleGroupMessageBox tmp = this.groupQueue[i];
                    if (tmp.Group == unit.ID)
                    {
                        return tmp.MessageList.Count;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// 移除未读消息（删除好友、群组时直接移除该未读消息）
        /// </summary>
        /// <param name="unit"></param>
        public void RemoveUnhanleMessage(IUnit unit)
        { 
            if (unit == null) { return; }
            if (unit.UnitType == UnitType.User)
            {
                for (int i = 0; i < this.friendQueue.Count; i++)
                {
                    UnhandleFriendMessageBox tmp = this.friendQueue[i];
                    if (tmp.User == unit.ID)
                    {
                        this.friendQueue.RemoveAt(i);
                        this.DetectUnhandleMessage();
                        return;
                    }
                }
            }
            else if (unit.UnitType == UnitType.Group)
            {
                for (int i = 0; i < this.groupQueue.Count; i++)
                {
                    UnhandleGroupMessageBox tmp = this.groupQueue[i];
                    if (tmp.Group == unit.ID)
                    {
                        this.groupQueue.RemoveAt(i);
                        this.DetectUnhandleMessage();
                        return;
                    }
                }
            }
            
        }
        #endregion

        #region PickoutGroupMessage
        /// <summary>
        /// 提取指定组的未处理消息。将触发UnhandleMessagePickedOut事件。
        /// </summary>    
        public void PickoutGroupMessage(string groupID)
        {
            lock (this.locker)
            {
                for (int i = 0; i < this.groupQueue.Count; i++)
                {
                    UnhandleGroupMessageBox tmp = this.groupQueue[i];
                    if (tmp.Group == groupID)
                    {
                        this.groupQueue.RemoveAt(i);
                        //foreach (Parameter<string, int, byte[], string> para in tmp.MessageList)
                        //{
                        //    this.Message4Group(para.Arg1, groupID, para.Arg2, para.Arg3, para.Arg4);
                        //}
                        this.DetectUnhandleMessage();
                        if (this.UnhandleMessagePickedOut != null)
                        {
                            this.UnhandleMessagePickedOut(groupID, UnitType.Group);
                        }
                        return;
                    }
                }
            }
        }
        #endregion        

        #region PickoutNotifyMessage
        public void PickoutNotifyMessage(string userID)
        {
            //lock (this.locker)
            //{
            //    for (int i = 0; i < this.notifyQueue.Count; i++)
            //    {
            //        UnhandleNotifyMessageBox tmp = this.notifyQueue[i];
            //        if (tmp.UserID == userID)
            //        {
            //            this.notifyQueue.RemoveAt(i);
            //            foreach (Parameter<string, int, byte[]> para in tmp.MessageList)
            //            {
            //                this.Message4Notify(para.Arg2, para.Arg3, para.Arg1);
            //            }
            //            this.DetectUnhandleMessage();
            //            return;
            //        }
            //    }
            //}
            lock (this.locker)
            {
                this.notifyQueue.Clear();
                //for (int i = 0; i < this.notifyQueue.Count; i++)
                //{
                //    UnhandleNotifyMessageBox tmp = this.notifyQueue[i];
                //if (tmp.UserID == userID)
                //{
                //this.notifyQueue.RemoveAt(i);
                //foreach (Parameter<string, int, byte[]> para in tmp.MessageList)
                //{
                //    this.Message4Notify(para.Arg2, para.Arg3, para.Arg1);
                //}
                this.DetectUnhandleMessage();
                if (this.UnhandleNotifyMessagePickedOut != null)
                {
                    this.UnhandleNotifyMessagePickedOut();
                }
                //return;
                //}
                //}
            }
        }
        #endregion

        #region DetectUnhandleMessage
        private void DetectUnhandleMessage()
        {
            if (this.friendQueue.Count == 0 && this.groupQueue.Count == 0 && this.notifyQueue.Count == 0)
            {
                this.ControlTimer(false);
            }
            else if (this.friendQueue.Count > 0)
            {
                UnhandleFriendMessageBox cache = this.friendQueue[0];
                string userName = this.twinkleNotifySupporter.GetFriendName(cache.User);
                this.notifyIcon1.Text = string.Format("{0}({1})  {2}条消息", cache.User, userName, cache.MessageList.Count);
                this.twinkleIcon = this.twinkleNotifySupporter.GetHeadIcon(cache.User);
            }
            else if (this.groupQueue.Count > 0)
            {
                UnhandleGroupMessageBox cache = this.groupQueue[0];
                string groupName = this.twinkleNotifySupporter.GetGroupName(cache.Group);
                this.notifyIcon1.Text = string.Format("{0}({1})  {2}条消息", groupName, cache.Group, cache.MessageList.Count);
                this.twinkleIcon = this.twinkleNotifySupporter.GroupIcon;
            }
            else if (this.notifyQueue.Count > 0)
            {
                UnhandleNotifyMessageBox cache = this.notifyQueue[0];
                this.notifyIcon1.Text = string.Format("{0}条消息", cache.MessageList.Count);
                this.twinkleIcon = this.twinkleNotifySupporter.NotifyIcon;
            }
        }
        #endregion



    }


    #region UnhandleFriendMessageBox
    internal class UnhandleFriendMessageBox
    {
        public UnhandleFriendMessageBox(string user)
        {
            this.User = user;
        }

        public string User { get; set; }
        //object 用于存放解析后的数据
        public List<Parameter<ClientType, int, byte[]>> MessageList = new List<Parameter<ClientType, int, byte[]>>();
    }
    #endregion

    #region UnhandleGroupMessageBox
    internal class UnhandleGroupMessageBox
    {
        public UnhandleGroupMessageBox(string group)
        {
            this.Group = group;
        }

        public string Group { get; set; }
        public List<Parameter<string, int, byte[], string>> MessageList = new List<Parameter<string, int, byte[], string>>();
    }
    #endregion

    #region UnhandleNotifyMessageBox
    internal class UnhandleNotifyMessageBox
    {
        public UnhandleNotifyMessageBox(string userID)
        {
            this.UserID = userID;
        }

        public string UserID { get; set; }
        //object 用于存放解析后的数据
        public List<Parameter<string, int, byte[]>> MessageList = new List<Parameter<string, int, byte[]>>();
    }
    #endregion
}
