using ESBasic;
using ESFramework;
using ESPlus.Serialization;
using GGTalk;
using OMCS.Passive.ShortMessages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TalkBase.Client
{
    /// <summary>
    /// 闪动托盘的支持接口。
    /// </summary>
    public interface ITwinkleNotifySupporter
    {
        string GetFriendName(string friendID);
        string GetGroupName(string groupID);
        bool NeedTwinkle4User(string userID);
        bool NeedTwinkle4Group(string groupID);
        bool NeedTwinkle4AddFriendNotify();
        void PlayAudioAsyn();
        Icon GetHeadIcon(string userID);
        Icon Icon64 { get; }
        Icon NoneIcon64 { get; }
        Icon GroupIcon { get; }
        Icon NotifyIcon { get; }
        Icon GetStatusIcon(UserStatus status);
    }


    /// <summary>
    /// 会闪动的托盘图标。
    /// （1）缓存未处理的好友或群消息。
    /// （2）当收到未提取的聊天消息时，触发UnhandleMessageOccured事件。
    /// （3）当消息已经被提取时，触发UnhandleMessagePickedOut事件。
    /// （4）提取消息时，将其统一转接为触发clientOutter的相关事件。
    /// </summary>
    public partial class TwinkleNotifyIcon : Component
    {
        private IBrige4ClientOutter clientOutter;
        private Control control;
        private object locker = new object();
        private List<UnhandleFriendMessageBox> friendQueue = new List<UnhandleFriendMessageBox>();
        private List<UnhandleGroupMessageBox> groupQueue = new List<UnhandleGroupMessageBox>();
        private List<UnhandleNotifyMessageBox> notifyQueue = new List<UnhandleNotifyMessageBox>();
        private ITwinkleNotifySupporter twinkleNotifySupporter;
        private System.Windows.Forms.Timer timer = new Timer();
        private TalkBaseInfoTypes talkBaseInfoTypes;

        /// <summary>
        /// 当出现未处理的聊天消息时，在后台线程中触发此事件。参数：unitID - isGroup
        /// </summary>
        public event CbGeneric<string, UnitType> UnhandleMessageOccured;
        /// <summary>
        /// 当用户点击托盘图标提取聊天消息时，或调用Pickout方法提取消息时，将触发此事件。（在触发clientOutter相关事件之后）。参数：unitID - UnitType
        /// </summary>
        public event CbGeneric<string, UnitType> UnhandleMessagePickedOut;
        public event MouseEventHandler MouseClick;

        public TwinkleNotifyIcon(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        public void Initialize(ITwinkleNotifySupporter getter, Control ctrl, IBrige4ClientOutter outter, TalkBaseInfoTypes infoTypes)
        {
            this.control = ctrl;
            this.twinkleNotifySupporter = getter;
            this.clientOutter = outter;
            this.talkBaseInfoTypes = infoTypes;
            this.timer.Tick += new EventHandler(timer_Tick);
            this.timer.Interval = 500;

            this.normalIcon = this.twinkleNotifySupporter.Icon64;
            this.notifyIcon1.Icon = this.normalIcon;
            this.notifyIcon1.MouseClick += new MouseEventHandler(notifyIcon1_MouseClick);
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
                AudioMessage msg = CompactPropertySerializer.Default.Deserialize<AudioMessage>(info, 0);
                this.clientOutter.OnAudioMessageReceived(msg, null);
                return;
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
                    AudioMessage audioMessage = CompactPropertySerializer.Default.Deserialize<AudioMessage>(msg.Information, 0);
                    this.clientOutter.OnAudioMessageReceived(audioMessage, msg.TimeTransfer);
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
            if (this.control.InvokeRequired)
            {
                this.control.BeginInvoke(new CbGeneric<bool>(this.ControlTimer), start);
            }
            else
            {
                if (start)
                {
                    this.timer.Start(); ;
                }
                else
                {
                    this.timer.Stop();
                    //2014.11.05
                    this.notifyIcon1.Icon = this.normalIcon;
                    this.notifyIcon1.Text = this.normalText;
                }
            }
        }




        private Icon twinkleIcon = null;
        private Icon normalIcon;
        public void ChangeMyStatus(UserStatus status)
        {
            this.normalIcon = this.twinkleNotifySupporter.GetStatusIcon(status);
            if (!this.timer.Enabled)
            {
                this.notifyIcon1.Icon = this.normalIcon;
            }
        }

        private string normalText = null;
        public void ChangeText(string text)
        {
            this.normalText = text;
            if (!this.timer.Enabled)
            {
                this.notifyIcon1.Text = this.normalText;
            }
        }

        void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            //0920
            try
            {
                if (e.Button != MouseButtons.Left)
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
                    GlobalResourceManager.UiSafeInvoker.ActionOnUI(() =>
                    {
                        UnhandleFriendMessageBox cache = this.friendQueue[0];
                        Program.ResourceCenter.ChatFormController.GetForm(cache.User);
                    });
                    return;
                }

                if (this.groupQueue.Count > 0)
                {
                    GlobalResourceManager.UiSafeInvoker.ActionOnUI(() =>
                    {
                        UnhandleGroupMessageBox cache = this.groupQueue[0];
                        Program.ResourceCenter.ChatFormController.GetForm(cache.Group);
                    });
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
                        GlobalResourceManager.UiSafeInvoker.ActionOnUI(() =>
                        {
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
                        });
                        return;
                    }
                }

                if (this.groupQueue.Count > 0)
                {
                    UnhandleGroupMessageBox cache = this.groupQueue.Find(x => x.Group == unitID);
                    if (cache != null)
                    {
                        GlobalResourceManager.UiSafeInvoker.ActionOnUI(() =>
                        {
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
                        });
                        return;
                    }
                }
            }
        }

        void timer_Tick(object sender, EventArgs e)
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
        public ContextMenuStrip ContextMenuStrip
        {
            get
            {
                return this.notifyIcon1.ContextMenuStrip;
            }
            set
            {
                this.notifyIcon1.ContextMenuStrip = value;
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
            //0920
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
                //    //return;
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
                        }

                        cache.MessageList.Add(new Parameter<string, int, byte[]>(userID, notifyType, info));
                    }
                    this.notifyIcon1.Text = string.Format("{0}条通知消息", cache.MessageList.Count);
                    this.twinkleIcon = this.twinkleNotifySupporter.NotifyIcon;
                    this.ControlTimer(true);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
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
                        foreach (Parameter<ClientType, int, byte[]> para in tmp.MessageList)
                        {
                            this.Message4User(tmp.User, para.Arg1, para.Arg2, para.Arg3);
                        }
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
                        foreach (Parameter<string, int, byte[], string> para in tmp.MessageList)
                        {
                            this.Message4Group(para.Arg1, groupID, para.Arg2, para.Arg3, para.Arg4);
                        }
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
                for (int i = 0; i < this.notifyQueue.Count; i++)
                {
                    UnhandleNotifyMessageBox tmp = this.notifyQueue[i];
                    if (tmp.UserID == userID)
                    {
                        this.notifyQueue.RemoveAt(i);
                        foreach (Parameter<string, int, byte[]> para in tmp.MessageList)
                        {
                            this.Message4Notify(para.Arg2, para.Arg3, para.Arg1);
                        }
                        this.DetectUnhandleMessage();
                        return;
                    }
                }
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
    public class UnhandleFriendMessageBox
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
    public class UnhandleGroupMessageBox
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
    public class UnhandleNotifyMessageBox
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
