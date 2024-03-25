using CCWin;
using ESBasic;
using ESFramework;
using ESFramework.Boost;
using ESFramework.Boost.Controls;
using ESFramework.Boost.NetworkDisk;
using ESPlus.FileTransceiver;
using ESPlus.Serialization;
using OMCS.Boost.Forms;
using OMCS.Passive.ShortMessages;
using GGTalk.Core;
using GGTalk.FlatControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TalkBase;
using TalkBase.Client;
using TalkBase.Client.Application;
using TalkBase.Client.Bridges;

namespace GGTalk.Controls
{
    public partial class FriendChatPanel : FlatBasePanel, IFriendChatForm, IOwnerForm
    {
        public const string Token4ConvertToOfflineFile = "转为离线发送";

        private RemoteDiskManager<GGUser, GGGroup> remoteDiskManager;
        private RemoteDesktopManager<GGUser, GGGroup> remoteDesktopManager;
        private AudioDialogManager<GGUser, GGGroup> audioDialogManager;
        private VideoDialogManager<GGUser, GGGroup> videoDialogManager;

        private Font messageFont = new Font("微软雅黑", 9);
        private string Title_FileTransfer = "文件";
        private ChatBoxCore fileTransferingViewer ;
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        private GGUser currentFriend;
        public event CbGeneric<string> VibrationNeeded;
        private bool isInHisBlackList = false;//是否在对方的黑名单中

        #region Ctor
        public FriendChatPanel(ResourceCenter<GGUser, GGGroup> center, string friendID)
        {
            this.resourceCenter = center;
            this.currentFriend = this.resourceCenter.ClientGlobalCache.GetUser(friendID);
            this.remoteDiskManager = new RemoteDiskManager<GGUser, GGGroup>(this.resourceCenter, this, this.currentFriend);
            this.remoteDesktopManager = new RemoteDesktopManager<GGUser, GGGroup>(this.resourceCenter, this, this.currentFriend);
            this.audioDialogManager = new AudioDialogManager<GGUser, GGGroup>(this.resourceCenter, this, this.currentFriend);
            this.videoDialogManager = new VideoDialogManager<GGUser, GGGroup>(this.resourceCenter, this, this.currentFriend);

            InitializeComponent();
            this.chatPanel1.SetHistoryBox_SenderShowType(SenderShowType.HeadAndName);
            this.chatPanel1.Initialize(this.resourceCenter,this.currentFriend);
            this.chatPanel1.FileDragDroped += new CbGeneric<string[]>(chatPanel1_FileDragDroped);
            this.chatPanel1.AudioDialogManager = this.audioDialogManager;
            this.chatPanel1.VideoDialogManager = this.videoDialogManager;
            this.chatPanel1.SendFileClicked += ChatPanel1_SendFileClicked;
            this.Size = SystemSettings.Singleton.ChatFormSize;
            this.FriendInfoChanged(this.currentFriend);
            this.MyInfoChanged(this.resourceCenter.ClientGlobalCache.CurrentUser);

            //this.resourceCenter.RapidPassiveEngine.P2PController.P2PChannelOpened += new CbGeneric<P2PChannelState>(P2PController_P2PChannelOpened);
            //this.resourceCenter.RapidPassiveEngine.P2PController.P2PChannelClosed += new CbGeneric<P2PChannelState>(P2PController_P2PChannelClosed);

            this.fileTransferingViewer = this.chatPanel1.ChatContentBox.ChatBoxCore;
            //文件传送
            this.fileTransferingViewer.Initialize(this.resourceCenter, this.resourceCenter.RapidPassiveEngine.FileOutter, new ESBasic.Func<TransferingProject, bool>(this.FilterTransferingProject));
            this.fileTransferingViewer.FileTransDisruptted += new CbGeneric<string, bool, FileTransDisrupttedType, string>(fileTransferingViewer1_FileTransDisruptted);
            //this.fileTransferingViewer.FileTransCompleted += new CbGeneric<string, bool ,string,bool>(fileTransferingViewer1_FileTransCompleted);
            this.fileTransferingViewer.FileTransCompleted2 += new CbGeneric<TransferingProject>(fileTransferingViewer_FileTransCompleted2);
            this.fileTransferingViewer.FileResumedTransStarted += new CbGeneric<string, bool>(fileTransferingViewer1_FileResumedTransStarted);
            this.fileTransferingViewer.AllTaskFinished += new CbSimple(fileTransferingViewer1_AllTaskFinished);
            this.fileTransferingViewer.FileNeedOfflineSend += new CbGeneric<TransferingProject>(fileTransferingViewer_FileNeedOfflineSend);

            //this.ShowP2PState();
        }



        private void FriendChatControl_Load(object sender, EventArgs e)
        {
            if (this.resourceCenter.Connected)
            {
                this.isInHisBlackList = this.resourceCenter.ClientOutter.IsInHisBlackList(this.currentFriend.ID);
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
        #endregion



        #region IFlatControl
        private string title = string.Empty;
        public override string ControlTitle => this.title;

        public override void ClickMore()
        {
            UserInfoForm form = new UserInfoForm(this.resourceCenter, this.currentFriend, true);
            form.ShowDialog();
        }
        #endregion





        #region IOwnerForm Members
        public void FlashWindow()
        {
            this.FlashChatWindow();
        }

        public void AppendSysMessage(string msg)
        {
            this.chatPanel1.AppendSysMessage(msg);
        }

        public void AddDisplayedPanel(string title, Control displayedPanel)
        {
            ///TODO: 显示其他多媒体（语音通话）
            BaseForm form = new BaseForm();
            form.Text = title;
            form.Width = displayedPanel.Width;
            form.Height = displayedPanel.Height;
            //displayedPanel.Dock = DockStyle.Fill;
            form.Controls.Add(displayedPanel);

            form.ShowDialog();
            //if (!this.TabControlContains(title))
            //{
            //    TabPage page = new TabPage(title);
            //    page.BackColor = System.Drawing.Color.White;
            //    Panel pannel = new Panel();
            //    page.Controls.Add(pannel);
            //    pannel.BackColor = Color.Transparent;
            //    pannel.Dock = DockStyle.Fill;
            //    pannel.Controls.Add(displayedPanel);
            //    this.fileTransferingViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            //    this.skinTabControl1.TabPages.Add(page);
            //    this.skinTabControl1.SelectedIndex = this.GetSelectIndex(title);
            //    this.ResetTabControVisible();
            //}
        }

        public void RemoveDisplayedPanel(string title)
        {
            ///TODO: 移除其他多媒体（语音通话）

            //int index = this.GetSelectIndex(title);
            //if (index < 0)
            //{
            //    return;
            //}

            //this.skinTabControl1.TabPages.RemoveAt(index);
            //this.skinTabControl1.SelectedIndex = this.skinTabControl1.TabPages.Count - 1;
            //this.ResetTabControVisible();
        }

        public Control GetDisplayedPanel(string title)
        {
            ///TODO: 移获取其他多媒体（语音通话）
            return null;
            //int index = this.GetSelectIndex(title);
            //if (index < 0)
            //{
            //    return null;
            //}

            //return this.skinTabControl1.TabPages[index].Controls[0].Controls[0];
        }

        public bool ContanisDisplayedPanel(string title)
        {
            ///TODO: 返回是否存在
            return false;
            //return this.GetSelectIndex(title) >= 0;
        }

        public IVideoChatForm DoCreateVideoChatForm(bool waitingAnswer)
        {
            //VideoChatForm.CommonQualityVideo = 400 + 300; //普通质量分辨率
            //VideoChatForm.HighQualityVideo = 800 + 600;   //高质量分辨率
            //
            string loginID4OMCS = ESFramework.MSideHelper.ConstructLoginID(this.resourceCenter.CurrentUserID, ClientType.DotNET);
            VideoChatForm form = new VideoChatForm(loginID4OMCS,  this.currentFriend.DisplayName, waitingAnswer, this.resourceCenter.Logger);
            //form.DisplayVideoParas = true;
            return form;
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
            this.title = "与 " + this.currentFriend.DisplayName + " 对话中";
        }

        #endregion

        #region OnRemovedFromFriend
        public void OnRemovedFromFriend()
        {
            MessageBoxEx.Show("好友已被删除！");
            ///TODO: this.Close();
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
                //ESBasic.Helpers.WindowsHelper.FlashWindow(this.ParentForm);
            }
        }
        #endregion

        #region RefreshUI
        public void RefreshUI()
        {
            this.chatPanel1.FocusOnInputBox();
        }
        #endregion

        #region HandleVibration
        public void HandleVibration()
        {

        }


        #endregion

        #region HandleInptingNotify
        public void HandleInptingNotify()
        {
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
            this.chatPanel1.AppendSysMessage("对方添加您为好友，可以开始对话了...");
            return;
        }

        public void AddFriendSucceed()
        {
            this.chatPanel1.AppendSysMessage("已经添加对方为好友，可以开始对话了...");
            return;
        }
        #endregion

        #region HandleOfflineFileResultReceived
        public void HandleOfflineFileResultReceived(string fileName, bool accept)
        {
            string msg = string.Format("对方{0}了您发送的离线文件'{1}'", accept ? "已成功接收" : "拒绝", fileName);
            this.chatPanel1.AppendSysMessage(msg);
            this.FlashChatWindow();
        }
        #endregion

        #endregion

        #region 文件传输
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


        #region 发送 传输文件的请求

        private void ChatPanel1_SendFileClicked(string fileOrFolderPath)
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

        #endregion

        #region 收到文件传输请求
        /// <summary>
        /// 当收到文件传输请求的时候 ，展开fileTransferingViewer,如果 本来就是展开 状态，直接添加
        /// 自己发送 文件请求的时候，也调用这里
        /// </summary>        
        internal void FileRequestReceived(string projectID, bool offlineFile)
        {
            ///TODO: 接收文件传输
            //if (!this.TabControlContains(this.Title_FileTransfer))
            //{
            //    TabPage page = new TabPage(this.Title_FileTransfer);
            //    page.BackColor = System.Drawing.Color.White;
            //    Panel pannel = new Panel();
            //    page.Controls.Add(pannel);
            //    pannel.BackColor = Color.Transparent;
            //    pannel.Dock = DockStyle.Fill;
            //    pannel.Controls.Add(this.fileTransferingViewer);
            //    this.fileTransferingViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            //    this.skinTabControl1.TabPages.Add(page);
            //    this.skinTabControl1.SelectedIndex = this.GetSelectIndex(this.Title_FileTransfer);
            //    this.ResetTabControVisible();
            //}

            
            TransferingProject pro = this.resourceCenter.RapidPassiveEngine.FileOutter.GetTransferingProject(projectID);
            //if (offlineFile)
            //{
            //    string strFile = pro.IsFolder ? "离线文件夹" : "离线文件";
            //    this.chatPanel1.AppendSysMessage(string.Format("对方给您发送了{0}'{1}'，大小：{2}", strFile, pro.ProjectName, ESBasic.Helpers.PublicHelper.GetSizeString(pro.TotalSize)));
            //}

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
            //string showText = FileTransHelper.GetTipMessage4FileTransDisruptted(projectName, isSender, type, cause == FriendChatForm.Token4ConvertToOfflineFile);
            //this.chatPanel1.AppendSysMessage(showText);
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
            this.chatPanel1.AppendSysMessage(showText);
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
            this.chatPanel1.AppendSysMessage(showText);
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
            ///TODO: 所有文件传输完成
            //this.skinTabControl1.TabPages.RemoveAt(this.GetSelectIndex(this.Title_FileTransfer));
            //this.skinTabControl1.SelectedIndex = this.skinTabControl1.TabPages.Count - 1;
            //this.ResetTabControVisible();
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

 
    }
}
