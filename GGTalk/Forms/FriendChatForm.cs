using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CCWin;
using CCWin.SkinControl;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using ESPlus.Rapid;
using ESBasic.ObjectManagement.Forms;
using ESPlus.FileTransceiver;
using ESBasic;
using OMCS.Passive;
using GGTalk;
using ESBasic.Helpers;
using ESPlus.Serialization;
using ESPlus.Application;
using TalkBase;
using TalkBase.Client;
using TalkBase.Client.Application;
using System.Diagnostics;
using ESFramework.Boost.Controls;
using TalkBase.Client.Bridges;
using ESFramework.Boost.NetworkDisk;
using OMCS.Boost.Forms;
using OMCS.Passive.ShortMessages;
using ESFramework;
using ESFramework.Boost;

namespace GGTalk
{
    /// <summary>
    /// 好友聊天窗口。
    /// </summary>
    public partial class FriendChatForm : BaseForm, IFriendChatForm, IOwnerForm
    {
        public const string Token4ConvertToOfflineFile = "转为离线发送";

        private RemoteDiskManager<GGUser, GGGroup> remoteDiskManager;
        private RemoteDesktopManager<GGUser, GGGroup> remoteDesktopManager;
        private AudioDialogManager<GGUser, GGGroup> audioDialogManager;
        private VideoDialogManager<GGUser, GGGroup> videoDialogManager;

        private Font messageFont = new Font("微软雅黑", 9);
        private string Title_FileTransfer = "文件";
        private FileTransferingViewer fileTransferingViewer = new FileTransferingViewer();
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        private GGUser currentFriend;
        private System.Windows.Forms.Timer inputingTimer = new System.Windows.Forms.Timer();
        public event CbGeneric<string> VibrationNeeded;
        private bool isInHisBlackList = false;//是否在对方的黑名单中

        #region Ctor

        private bool FilterTransferingProject(TransferingProject pro)
        {
            #region 2016.05.06
            NDiskParameters para = Comment4NDisk.Parse(pro.Comment);
            if (para != null)
            {
                return false;
            }
            #endregion

            if (ESFramework.NetServer.IsServerUser(pro.DestUserID))
            {
                string offlineFileSenderID = Comment4OfflineFile.ParseUserID(pro.Comment);
                return offlineFileSenderID == this.currentFriend.ID;
            }

            return pro.DestUserID == this.currentFriend.ID;
        }

        public FriendChatForm(ResourceCenter<GGUser, GGGroup> center, string friendID)
        {
            this.resourceCenter = center;
            this.currentFriend = this.resourceCenter.ClientGlobalCache.GetUser(friendID);
            this.remoteDiskManager = new RemoteDiskManager<GGUser, GGGroup>(this.resourceCenter, this, this.currentFriend);
            this.remoteDesktopManager = new RemoteDesktopManager<GGUser, GGGroup>(this.resourceCenter, this, this.currentFriend);
            this.audioDialogManager = new AudioDialogManager<GGUser, GGGroup>(this.resourceCenter, this, this.currentFriend);
            this.videoDialogManager = new VideoDialogManager<GGUser, GGGroup>(this.resourceCenter, this, this.currentFriend);

            InitializeComponent();
            this.chatPanel1.Initialize(this.resourceCenter, this.currentFriend);
            this.chatPanel1.FileDragDroped += new CbGeneric<string[]>(chatPanel1_FileDragDroped);
            this.chatPanel1.VibrationClicked += new CbGeneric(chatPanel1_VibrationClicked);


            this.Size = SystemSettings.Singleton.ChatFormSize;
            this.FriendInfoChanged(this.currentFriend);
            this.MyInfoChanged(this.resourceCenter.ClientGlobalCache.CurrentUser);

            //this.resourceCenter.RapidPassiveEngine.P2PController.P2PChannelOpened += new CbGeneric<P2PChannelState>(P2PController_P2PChannelOpened);
            //this.resourceCenter.RapidPassiveEngine.P2PController.P2PChannelClosed += new CbGeneric<P2PChannelState>(P2PController_P2PChannelClosed);

            //文件传送
            this.fileTransferingViewer.Initialize(this.resourceCenter.RapidPassiveEngine.FileOutter, new ESBasic.Func<TransferingProject, bool>(this.FilterTransferingProject));
            this.fileTransferingViewer.FileTransDisruptted += new CbGeneric<string, bool, FileTransDisrupttedType, string>(fileTransferingViewer1_FileTransDisruptted);
            //this.fileTransferingViewer.FileTransCompleted += new CbGeneric<string, bool ,string,bool>(fileTransferingViewer1_FileTransCompleted);
            this.fileTransferingViewer.FileTransCompleted2 += new CbGeneric<TransferingProject>(fileTransferingViewer_FileTransCompleted2);
            this.fileTransferingViewer.FileResumedTransStarted += new CbGeneric<string, bool>(fileTransferingViewer1_FileResumedTransStarted);
            this.fileTransferingViewer.AllTaskFinished += new CbSimple(fileTransferingViewer1_AllTaskFinished);
            this.fileTransferingViewer.FileNeedOfflineSend += new CbGeneric<TransferingProject>(fileTransferingViewer_FileNeedOfflineSend);

            //this.ShowP2PState();

            this.inputingTimer.Interval = 1000;
            this.inputingTimer.Tick += new EventHandler(timer_Tick);
            this.inputingTimer.Start();

            Icon icon = this.currentFriend.GetHeadIcon(GlobalResourceManager.HeadImages);
            if (icon != null)
            {
                this.UseCustomIcon = true;
                this.Icon = icon;
            }
        }

        private void FriendChatForm_Load(object sender, EventArgs e)
        {
            if (this.resourceCenter.Connected)
            {
                this.isInHisBlackList = this.resourceCenter.ClientOutter.IsInHisBlackList(this.currentFriend.ID);
            }
        }


        void chatPanel1_VibrationClicked()
        {
            if (this.resourceCenter.ChatFormController.ChatFormMode == ChatFormMode.Seperated)
            {
                this.Vibration();
            }
            else
            {
                if (this.VibrationNeeded != null)
                {
                    this.VibrationNeeded(this.currentFriend.ID);
                }
            }
        }

        void chatPanel1_FileDragDroped(string[] fileOrDirs)
        {
            SendingFileParas sendingFileParas = new SendingFileParas(20480, 5);
            if (this.currentFriend.UserStatus == UserStatus.OffLine)
            {
                foreach (string fileOrDirPath in fileOrDirs)
                {
                    string projectID;
                    // BeginSendFile方法
                    //（1）accepterID传入null，表示文件的接收者就是服务端
                    //（2）巧用comment参数表达离线文件，参见Comment4OfflineFile类
                    this.resourceCenter.RapidPassiveEngine.FileOutter.BeginSendFile(null, fileOrDirPath, Comment4OfflineFile.BuildComment(this.currentFriend.UserID, this.resourceCenter.CurrentClientType), sendingFileParas, out projectID);
                    this.FileRequestReceived(projectID, false);
                    this.AppendSysMessage(string.Format("正在发送离线文件'{0}'", ESBasic.Helpers.FileHelper.GetFileNameNoPath(fileOrDirPath)));
                }
            }
            else
            {
                foreach (string fileOrDirPath in fileOrDirs)
                {
                    string projectID;
                    this.resourceCenter.RapidPassiveEngine.FileOutter.BeginSendFile(this.currentFriend.ID, fileOrDirPath, null, sendingFileParas, out projectID);
                    this.FileRequestReceived(projectID, false);
                }
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.CheckInptingVisiable();
        }
        #endregion

        #region IOwnerForm Members
        public void FlashWindow()
        {
            this.FlashChatWindow();
        }

        public void AppendSysMessage(string msg)
        {
            this.chatPanel1.AppendSystemMsg(msg);
        }

        public void AddDisplayedPanel(string title, Control displayedPanel)
        {
            if (!this.TabControlContains(title))
            {
                TabPage page = new TabPage(title);
                page.BackColor = System.Drawing.Color.White;
                Panel pannel = new Panel();
                page.Controls.Add(pannel);
                pannel.BackColor = Color.Transparent;
                pannel.Dock = DockStyle.Fill;
                pannel.Controls.Add(displayedPanel);
                this.fileTransferingViewer.Dock = System.Windows.Forms.DockStyle.Fill;
                this.skinTabControl1.TabPages.Add(page);
                this.skinTabControl1.SelectedIndex = this.GetSelectIndex(title);
                this.ResetTabControVisible();
            }
        }

        public void RemoveDisplayedPanel(string title)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<string>(this.RemoveDisplayedPanel), title);
            }
            else
            {
                int index = this.GetSelectIndex(title);
                if (index < 0)
                {
                    return;
                }

                this.skinTabControl1.TabPages.RemoveAt(index);
                this.skinTabControl1.SelectedIndex = this.skinTabControl1.TabPages.Count - 1;
                this.ResetTabControVisible();
            }
        }

        public Control GetDisplayedPanel(string title)
        {
            int index = this.GetSelectIndex(title);
            if (index < 0)
            {
                return null;
            }

            return this.skinTabControl1.TabPages[index].Controls[0].Controls[0];
        }

        public bool ContanisDisplayedPanel(string title)
        {
            return this.GetSelectIndex(title) >= 0;
        }
        #endregion

        #region IFriendChatForm Members

        public string UnitID
        {
            get { return this.currentFriend.UserID; }
        }

        #region FriendInfoChanged
        public void FriendInfoChanged(IUser _friend)
        {
            GGUser friend = _friend as GGUser;
            if (friend == null) return;
            this.currentFriend = friend;
            this.Text = "与 " + this.currentFriend.DisplayName + " 对话中";
            this.labelFriendName.Text = this.currentFriend.DisplayName;
            this.skinLabel_inputing.Location = new Point(this.labelFriendName.Location.X + this.labelFriendName.Width - 5, this.skinLabel_inputing.Location.Y);
            this.labelFriendSignature.Text = this.currentFriend.Signature;
            this.panelFriendHeadImage.BackgroundImage = GlobalResourceManager.GetHeadImage(this.currentFriend);
            this.skinLabel_FriendID.Text = this.currentFriend.UserID;
            this.skinLabel_FriendName.Text = this.currentFriend.Name;
            this.skinPanel_friend.BackgroundImage = GlobalResourceManager.GetHeadImageOnline(this.currentFriend);

            this.skinPanel_status.BackgroundImage = GlobalResourceManager.GetStatusImage(friend.UserStatus);
            this.skinPanel_status.Visible = !(friend.OfflineOrHide || friend.UserStatus == UserStatus.Online);
            this.toolShow.SetToolTip(this.panelFriendHeadImage, "状态：" + GlobalResourceManager.GetUserStatusName(friend.UserStatus));
        }

        #endregion

        #region OnRemovedFromFriend
        public void OnRemovedFromFriend()
        {
            MessageBoxEx.Show("好友已被删除！");
            this.Close();
        }
        #endregion

        #region MyInfoChanged
        public void MyInfoChanged(IUser my)
        {

        }
        #endregion

        #region UnitCommentNameChanged
        public void UnitCommentNameChanged(IUnit unit)
        {
            this.FriendInfoChanged((GGUser)unit);
        }
        #endregion

        #region FriendStateChanged
        public void FriendStateChanged(string friendID, UserStatus newStatus)
        {
            if (friendID != this.currentFriend.ID)
            {
                return;
            }

            this.FriendInfoChanged(this.currentFriend);
            if (newStatus == UserStatus.OffLine)
            {
                GlobalResourceManager.UiSafeInvoker.ActionOnUI(this.FriendOffline);
            }
        }
        #endregion

        #region MyselfOffline ,FriendOffline
        /// <summary>
        /// 自己掉线
        /// </summary>
        public void MyselfOffline()
        {
            this.OnOffline(true);
        }

        /// <summary>
        /// 好友掉线
        /// </summary>
        private void FriendOffline()
        {
            this.FriendInfoChanged(this.currentFriend);
            this.OnOffline(false);
        }

        private void OnOffline(bool myself)
        {
            this.remoteDiskManager.OnOffline(myself);
            this.videoDialogManager.OnOffline(myself);
            this.remoteDesktopManager.OnOffline(myself);
            this.audioDialogManager.OnOffline(myself);

            this.ResetTabControVisible();
        }
        #endregion

        #region FlashChatWindow
        private DateTime lastFlashTime = DateTime.Now;
        public void FlashChatWindow()
        {
            if (SystemSettings.Singleton.CombineChatbox)
            {
                return;
            }

            TimeSpan span = DateTime.Now - this.lastFlashTime;
            if (span.TotalSeconds > 1)
            {
                this.lastFlashTime = DateTime.Now;
                ESBasic.Helpers.WindowsHelper.FlashWindow(this);
            }
        }
        #endregion

        #region RefreshUI
        public void RefreshUI()
        {
            this.skinPanel_right.Refresh();
            this.chatPanel1.FocusOnInputBox();
        }
        #endregion

        #region HandleVibration
        public void HandleVibration()
        {
            if (this.resourceCenter.ChatFormController.ChatFormMode == ChatFormMode.Seperated)
            {
                this.Vibration();
            }
            else
            {
                if (this.VibrationNeeded != null)
                {
                    this.VibrationNeeded(this.currentFriend.ID);
                }
            }
            this.chatPanel1.ShowVibrationMessage();



            //if (this.TopMost)
            //{
            //    this.Focus();
            //}
            //else
            //{
            //    this.TopMost = true;
            //    Vibration();
            //    this.TopMost = false;
            //}
        }

        #region Vibration 震动
        //震动方法
        private void Vibration()
        {
            Point pOld = this.Location;//原来的位置
            int radius = 3;//半径
            for (int n = 0; n < 3; n++) //旋转圈数
            {
                //右半圆逆时针
                for (int i = -radius; i <= radius; i++)
                {
                    int x = Convert.ToInt32(Math.Sqrt(radius * radius - i * i));
                    int y = -i;

                    this.Location = new Point(pOld.X + x, pOld.Y + y);
                    Thread.Sleep(10);
                }
                //左半圆逆时针
                for (int j = radius; j >= -radius; j--)
                {
                    int x = -Convert.ToInt32(Math.Sqrt(radius * radius - j * j));
                    int y = -j;

                    this.Location = new Point(pOld.X + x, pOld.Y + y);
                    Thread.Sleep(10);
                }
            }
            //抖动完成，恢复原来位置
            this.Location = pOld;
        }

        #endregion
        #endregion

        #region HandleInptingNotify
        public void HandleInptingNotify()
        {
            this.skinLabel_inputing.Visible = true;
            this.dtInptingVisiableShowTime = DateTime.Now;
        }

        private DateTime dtInptingVisiableShowTime = DateTime.Now;
        private void CheckInptingVisiable()
        {
            if (!this.skinLabel_inputing.Visible)
            {
                return;
            }

            if ((DateTime.Now - this.dtInptingVisiableShowTime).TotalSeconds >= 10)
            {
                this.skinLabel_inputing.Visible = false;
            }
        }
        #endregion

        #region HandleMediaCommunicate

        public void HandleMediaCommunicate(ClientType sourceClientType, CommunicateMediaType mediaType, CommunicateType communicateType, string tag)
        {
            if (mediaType == CommunicateMediaType.Video)
            {
                this.videoDialogManager.HandleVideoDialog(sourceClientType, communicateType, tag);
                return;
            }

            if (mediaType == CommunicateMediaType.Audio)
            {
                this.audioDialogManager.HandleAudioDialog(sourceClientType, communicateType, tag);
                return;
            }

            if (mediaType == CommunicateMediaType.RemoteHelp || mediaType == CommunicateMediaType.RemoteControl)
            {
                this.remoteDesktopManager.HandleRemoteDesktop(mediaType, communicateType, tag);
                return;
            }

            if (mediaType == CommunicateMediaType.RemoteDisk)
            {
                this.remoteDiskManager.HandleRemoteDisk(communicateType, tag);
                return;
            }
        }

        public void OnMediaCommunicateAnswerOnOtherDevice(ClientType answerType, CommunicateMediaType mediaType, bool agree)
        {
            if (mediaType == CommunicateMediaType.Video)
            {
                this.videoDialogManager.OnMediaCommunicateAnswerOnOtherDevice(answerType, agree);
                return;
            }

            if (mediaType == CommunicateMediaType.Audio)
            {
                this.audioDialogManager.OnMediaCommunicateAnswerOnOtherDevice(answerType, agree);
                return;
            }
        }
        #endregion

        #region HandleChatMessage / HandleOfflineChatMessage
        public void HandleChatMessage(byte[] info, DateTime? msgOccureTime)
        {
            ChatBoxContent content = CompactPropertySerializer.Default.Deserialize<ChatBoxContent>(info, 0);
            this.OnReceivedMsg(content, msgOccureTime);
            this.FlashChatWindow();
        }

        public void HandleChatMessageOfMine(byte[] info)
        {
            ChatBoxContent content = CompactPropertySerializer.Default.Deserialize<ChatBoxContent>(info, 0);
            this.chatPanel1.HandleChatMessageOfMine(content);
        }

        private void OnReceivedMsg(ChatBoxContent content, DateTime? originTime)
        {
            this.skinLabel_inputing.Visible = false;
            this.chatPanel1.HandleChatMessage(null, content, originTime, null);
        }
        #endregion

        public void HandleSnapchatMessage(SnapchatMessage snapchatMessage, DateTime? msgOccureTime)
        {
            this.chatPanel1.OnSnapchatMessage(snapchatMessage, msgOccureTime);
        }

        public void HandleSnapchatRead(string messageID)
        {
            this.chatPanel1.HandleSnapchatRead(messageID);
        }

        public void HandleAudioMessage(AudioMessage msg, DateTime? msgOccureTime)
        {
            this.chatPanel1.OnAudioMessage(msg, false);
        }

        public void HandleAudioMessageOfMine(AudioMessage msg)
        {
            this.chatPanel1.OnAudioMessage(msg, true);
        }

        #region HandleFriendAddedNotify
        public void HandleFriendAddedNotify()
        {
            this.chatPanel1.AppendSystemMsg("对方添加您为好友，可以开始对话了...");
            return;
        }

        public void AddFriendSucceed()
        {
            this.chatPanel1.AppendSystemMsg("已经添加对方为好友，可以开始对话了...");
            return;
        }
        #endregion

        #region HandleOfflineFileResultReceived
        public void HandleOfflineFileResultReceived(string fileName, bool accept)
        {
            string msg = string.Format("对方{0}了您发送的离线文件'{1}'", accept ? "已成功接收" : "拒绝", fileName);
            this.chatPanel1.AppendSystemMsg(msg);
            this.FlashChatWindow();
        }
        #endregion

        #endregion

        #region P2P 通道状态
        //void P2PController_P2PChannelClosed(P2PChannelState state)
        //{
        //    this.ShowP2PState();
        //}

        //void P2PController_P2PChannelOpened(P2PChannelState state)
        //{
        //    this.ShowP2PState();
        //}

        //public void ShowP2PState()
        //{
        //    GlobalResourceManager.UiSafeInvoker.ActionOnUI(this.do_ShowP2PState);
        //}

        //private void do_ShowP2PState()
        //{
        //    P2PChannelState state = this.resourceCenter.RapidPassiveEngine.P2PController.GetP2PChannelState(this.currentFriend.UserID);
        //    if (state != null)
        //    {
        //        this.Text = string.Format("与 {0} 对话中...【P2P直连/{1}】", this.currentFriend.Name, state.ProtocolType);
        //        this.skinLabel_p2PState.Text = string.Format("P2P直连/{0}", state.ProtocolType);
        //        this.pictureBox_state.Visible = true;
        //    }
        //    else
        //    {
        //        this.Text = string.Format("与 {0} 对话中...", this.currentFriend.Name);
        //        this.skinLabel_p2PState.Text = "";
        //        this.pictureBox_state.Visible = false;
        //    }
        //}
        #endregion

        #region TabControlContains
        private bool TabControlContains(string text)
        {
            for (int i = 0; i < this.skinTabControl1.TabPages.Count; i++)
            {
                if (this.skinTabControl1.TabPages[i].Text == text)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region GetSelectIndex
        private int GetSelectIndex(string text)
        {
            for (int i = 0; i < this.skinTabControl1.TabPages.Count; i++)
            {
                if (this.skinTabControl1.TabPages[i].Text == text)
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion

        #region 文件传输
        #region 发送 传输文件的请求
        private void SendFileOrFolder(bool isFolder)
        {
            if (!this.resourceCenter.Connected)
            {
                return;
            }

            try
            {
                if (this.isInHisBlackList)
                {
                    MessageBox.Show("对方已将您加入黑名单，不能进行文件发送！");
                    return;
                }
                string fileOrFolderPath = null;
                if (isFolder)
                {
                    fileOrFolderPath = ESBasic.Helpers.FileHelper.GetFolderToOpen(false);
                }
                else
                {
                    fileOrFolderPath = ESBasic.Helpers.FileHelper.GetFileToOpen("请选择要发送的文件");
                }
                if (fileOrFolderPath == null)
                {
                    return;
                }

                SendingFileParas sendingFileParas = new SendingFileParas(20480, 5);
                string projectID;
                this.resourceCenter.RapidPassiveEngine.FileOutter.BeginSendFile(this.currentFriend.UserID, fileOrFolderPath, null, sendingFileParas, out projectID);
                this.FileRequestReceived(projectID);
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(ee.Message, GlobalResourceManager.SoftwareName);
            }
        }

        private void toolStripButton_fileTransfer_Click(object sender, EventArgs e)
        {
            this.SendFileOrFolder(false);
        }

        private void toolStripMenuItem33_Click(object sender, EventArgs e)
        {
            this.SendFileOrFolder(true);
        }
        #endregion

        #region 收到文件传输请求
        /// <summary>
        /// 当收到文件传输请求的时候 ，展开fileTransferingViewer,如果 本来就是展开 状态，直接添加
        /// 自己发送 文件请求的时候，也调用这里
        /// </summary>        
        internal void FileRequestReceived(string projectID, bool offlineFile)
        {
            if (!this.TabControlContains(this.Title_FileTransfer))
            {
                TabPage page = new TabPage(this.Title_FileTransfer);
                page.BackColor = System.Drawing.Color.White;
                Panel pannel = new Panel();
                page.Controls.Add(pannel);
                pannel.BackColor = Color.Transparent;
                pannel.Dock = DockStyle.Fill;
                pannel.Controls.Add(this.fileTransferingViewer);
                this.fileTransferingViewer.Dock = System.Windows.Forms.DockStyle.Fill;
                this.skinTabControl1.TabPages.Add(page);
                this.skinTabControl1.SelectedIndex = this.GetSelectIndex(this.Title_FileTransfer);
                this.ResetTabControVisible();
            }
            TransferingProject pro = this.resourceCenter.RapidPassiveEngine.FileOutter.GetTransferingProject(projectID);
            if (offlineFile)
            {
                string strFile = pro.IsFolder ? "离线文件夹" : "离线文件";
                this.chatPanel1.AppendSystemMsg(string.Format("对方给您发送了{0}'{1}'，大小：{2}", strFile, pro.ProjectName, ESBasic.Helpers.PublicHelper.GetSizeString(pro.TotalSize)));
            }

            this.fileTransferingViewer.NewFileTransferItem(projectID, offlineFile, false);
        }

        internal void FileRequestReceived(string projectID)
        {
            this.FileRequestReceived(projectID, false);
        }
        #endregion

        #region fileTransferingViewer1_FileTransDisruptted
        /// <summary>
        /// 文件传输失败
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="isSender">是接收者，还是发送者</param>
        /// <param name="fileTransDisrupttedType">失败原因</param>
        private void fileTransferingViewer1_FileTransDisruptted(string projectName, bool isSender, FileTransDisrupttedType type, string cause)
        {
            string showText = FileTransHelper.GetTipMessage4FileTransDisruptted(projectName, isSender, type, cause == FriendChatForm.Token4ConvertToOfflineFile);
            this.chatPanel1.AppendSystemMsg(showText);
            //this.AppendMessage("系统", Color.Gray, showText, false);      
        }
        #endregion

        #region fileTransferingViewer1_FileResumedTransStarted
        /// <summary>
        /// 文件续传
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="isSender">接收者，还是发送者</param>
        private void fileTransferingViewer1_FileResumedTransStarted(string projectName, bool isSender)
        {
            string showText = string.Format("正在续传文件 '{0}'...", projectName);
            this.chatPanel1.AppendSystemMsg(showText);
        }
        #endregion

        #region fileTransferingViewer1_FileTransCompleted
        /// <summary>
        /// 文件传输成功
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="isSender">接收者，还是发送者</param>
        private void fileTransferingViewer1_FileTransCompleted(string projectName, bool isSender, string comment, bool isFolder)
        {
            string offlineFile = (Comment4OfflineFile.ParseUserID(comment) == null) ? "" : "离线文件";
            if (isFolder && !string.IsNullOrEmpty(offlineFile))
            {
                offlineFile += "夹";
            }
            string showText = offlineFile + string.Format("'{0}' {1}完成！", projectName, isSender ? "发送" : "接收");
            this.chatPanel1.AppendSystemMsg(showText);
        }

        void fileTransferingViewer_FileTransCompleted2(TransferingProject pro)
        {
            string offlineFile = (Comment4OfflineFile.ParseUserID(pro.Comment) == null) ? "" : "离线文件";
            if (pro.IsFolder && !string.IsNullOrEmpty(offlineFile))
            {
                offlineFile += "夹";
            }
            string showText = offlineFile + string.Format("{0} {1}完成！", pro.ProjectName, pro.IsSender ? "发送" : "接收");

            if (pro.IsSender)
            {
                this.chatPanel1.OnFileEvent(showText, pro.OriginPath);
            }
            else
            {
                showText += string.Format("保存路径为：{0}", pro.LocalSavePath);
                this.chatPanel1.OnFileEvent(showText, pro.LocalSavePath);
            }

            ChatBoxContent content = new ChatBoxContent(showText + "  ", this.messageFont, Color.Gray);
            content.AddLinkFile((uint)showText.Length, pro.IsSender ? pro.OriginPath : pro.LocalSavePath);
            byte[] binaryContent = CompactPropertySerializer.Default.Serialize(content);
            ChatMessageRecord record = new ChatMessageRecord(this.currentFriend.ID, this.resourceCenter.CurrentUserID, binaryContent, false);
            this.resourceCenter.LocalChatRecordPersister.InsertChatMessageRecord(record);
        }
        #endregion

        #region fileTransferingViewer1_AllTaskFinished
        private void fileTransferingViewer1_AllTaskFinished()
        {
            this.skinTabControl1.TabPages.RemoveAt(this.GetSelectIndex(this.Title_FileTransfer));
            this.skinTabControl1.SelectedIndex = this.skinTabControl1.TabPages.Count - 1;
            this.ResetTabControVisible();
        }
        #endregion

        #region fileTransferingViewer_FileNeedOfflineSend
        void fileTransferingViewer_FileNeedOfflineSend(TransferingProject project)
        {
            try
            {
                this.resourceCenter.RapidPassiveEngine.FileOutter.CancelTransfering(project.ProjectID, FriendChatForm.Token4ConvertToOfflineFile);

                string projectID;
                SendingFileParas sendingFileParas = new SendingFileParas(20480, 5);//文件数据包大小，可以根据网络状况设定，局网内可以设为204800，传输速度可以达到30M/s以上；公网建议设定为2048或4096或8192
                this.resourceCenter.RapidPassiveEngine.FileOutter.BeginSendFile(null, project.OriginPath, Comment4OfflineFile.BuildComment(this.currentFriend.UserID, this.resourceCenter.CurrentClientType), sendingFileParas, out projectID);
                this.FileRequestReceived(projectID);
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(ee.Message, GlobalResourceManager.SoftwareName);
            }
        }
        #endregion

        #endregion

        #region 窗体事件

        //渐变层
        private void FrmChat_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SolidBrush sb = new SolidBrush(Color.FromArgb(100, 255, 255, 255));
            g.FillRectangle(sb, new Rectangle(new Point(1, 91), new Size(Width - 2, Height - 91)));
        }
        #endregion

        #region 关闭窗体
        //关闭
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.remoteDiskManager.BeforeOwnerFormClosed();
            this.remoteDesktopManager.BeforeOwnerFormClosed();
            this.audioDialogManager.BeforeOwnerFormClosed();
            this.videoDialogManager.BeforeOwnerFormClosed();

            this.inputingTimer.Stop();
            this.inputingTimer.Dispose();
            //this.resourceCenter.RapidPassiveEngine.P2PController.P2PChannelOpened -= new CbGeneric<P2PChannelState>(P2PController_P2PChannelOpened);
            //this.resourceCenter.RapidPassiveEngine.P2PController.P2PChannelClosed -= new CbGeneric<P2PChannelState>(P2PController_P2PChannelClosed);

            this.fileTransferingViewer.BeforeDispose();
            e.Cancel = false;
        }
        #endregion

        #region 离线文件 V3.2
        private void toolStripMenuItem34_Click(object sender, EventArgs e)
        {
            if (!this.resourceCenter.Connected)
            {
                return;
            }

            try
            {
                if (this.isInHisBlackList)
                {
                    MessageBox.Show("对方已将您加入黑名单，不能进行文件发送！");
                    return;
                }

                string filePath = ESBasic.Helpers.FileHelper.GetFileToOpen("请选择要发送的离线文件");
                if (filePath == null)
                {
                    return;
                }
                string projectID;
                SendingFileParas sendingFileParas = new SendingFileParas(20480, 5);//文件数据包大小，可以根据网络状况设定，局网内可以设为204800，传输速度可以达到30M/s以上；公网建议设定为2048或4096或8192

                // BeginSendFile方法
                //（1）accepterID传入null，表示文件的接收者就是服务端
                //（2）巧用comment参数，参见Comment4OfflineFile类
                this.resourceCenter.RapidPassiveEngine.FileOutter.BeginSendFile(null, filePath, Comment4OfflineFile.BuildComment(this.currentFriend.UserID, this.resourceCenter.CurrentClientType), sendingFileParas, out projectID);
                this.FileRequestReceived(projectID);
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(ee.Message, GlobalResourceManager.SoftwareName);
            }
        }

        private void 发送离线文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.resourceCenter.Connected)
            {
                return;
            }
            if (this.isInHisBlackList)
            {
                MessageBox.Show("对方已将您加入黑名单，不能进行文件夹发送！");
                return;
            }
            try
            {
                string folderPath = ESBasic.Helpers.FileHelper.GetFolderToOpen(false);
                if (folderPath == null)
                {
                    return;
                }
                string projectID;
                SendingFileParas sendingFileParas = new SendingFileParas(20480, 5);//文件数据包大小，可以根据网络状况设定，局网内可以设为204800，传输速度可以达到30M/s以上；公网建议设定为2048或4096或8192

                // BeginSendFile方法
                //（1）accepterID传入null，表示文件的接收者就是服务端
                //（2）巧用comment参数，参见Comment4OfflineFile类
                this.resourceCenter.RapidPassiveEngine.FileOutter.BeginSendFile(null, folderPath, Comment4OfflineFile.BuildComment(this.currentFriend.UserID, this.resourceCenter.CurrentClientType), sendingFileParas, out projectID);
                this.FileRequestReceived(projectID);
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(ee.Message, GlobalResourceManager.SoftwareName);
            }
        }
        #endregion

        private void ChatForm_Shown(object sender, EventArgs e)
        {
            this.chatPanel1.FocusOnInputBox();
        }

        private void FocusCurrent(object sender, EventArgs e)
        {
            this.Focus();
            this.ToolFile.ShowDropDown();
        }

        private void FocusCurrent2(object sender, EventArgs e)
        {
            this.Focus();
            this.toolStripSplitButton2.ShowDropDown();
        }

        private void ResetTabControVisible()
        {
            if (this.skinTabControl1.TabPages.Count == 0)
            {
                this.skinTabControl1.Visible = false;
                this.skinPanel_right.Invalidate();
            }
            else
            {
                this.skinTabControl1.Visible = true;
            }
        }

        private void ChatForm_SizeChanged(object sender, EventArgs e)
        {
            this.chatPanel1.ScrollToCaret4Histoty();

            //0920
            if (this.WindowState == FormWindowState.Maximized || this.WindowState == FormWindowState.Minimized)
            {
                return;
            }

            SystemSettings.Singleton.ChatFormSize = this.Size;
            SystemSettings.Singleton.Save();

            //this.Width = 400;

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc.exe");
        }



        private void labelFriendSignature_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.labelFriendSignature.Text);
            MessageBox.Show("已经将签名复制到粘贴板！");
        }

        private void panelFriendHeadImage_Click(object sender, EventArgs e)
        {
            //UserInfoForm form = new UserInfoForm(this.resourceCenter, this.currentFriend);
            //form.Show();
        }

        private void 请求控制对方电脑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.remoteDesktopManager.RequestControlFriendDesktop();
        }

        private void 请求远程协助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.remoteDesktopManager.RequestRemoteHelp();
        }

        private void 桌面共享指定区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.remoteDesktopManager.ShareMyDesktop();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            this.audioDialogManager.RequestAudioDialog();
        }

        private void toolStripDropDownButton1_ButtonClick(object sender, EventArgs e)
        {
            this.videoDialogManager.RequestVideoDialog();
        }

        private void skinLabel_FriendID_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.currentFriend.ID);
            MessageBox.Show("已经将帐号复制到粘贴板！");
        }

        private void skinLabel_FriendName_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.currentFriend.DisplayName);
            MessageBox.Show("已经将名称复制到粘贴板！");
        }

        private void panelFriendHeadImage_MouseClick(object sender, MouseEventArgs e)
        {
            UserInfoForm form = new UserInfoForm(this.resourceCenter, this.currentFriend, true);
            form.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.remoteDiskManager.RequestControlFriendDisk();
        }


        public IVideoChatForm DoCreateVideoChatForm(bool waitingAnswer)
        {
            string loginID4OMCS = ESFramework.MSideHelper.ConstructLoginID(this.resourceCenter.CurrentUserID, ClientType.DotNET);
            VideoChatForm form = new VideoChatForm(loginID4OMCS, this.currentFriend.DisplayName, waitingAnswer, this.resourceCenter.Logger);
            return form;
        }


    }
}
