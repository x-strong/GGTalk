using CCWin;
using ESBasic;
using ESFramework.Boost;
using ESFramework.Boost.Controls;
using ESFramework.Boost.NetworkDisk;
using ESPlus.FileTransceiver;
using ESPlus.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TalkBase;
using TalkBase.Client;
using TalkBase.Client.Bridges;

namespace GGTalk
{
    /// <summary>
    /// 文件传送助手窗体。
    /// </summary>
    public partial class FileAssistantForm : BaseForm, IFileAssistantForm, IChatForm
    {
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        private Font messageFont = new Font("微软雅黑", 9);

        public FileAssistantForm(ResourceCenter<GGUser, GGGroup> center)
        {
            this.resourceCenter = center;
            InitializeComponent();
            this.Text = CommonOptions.FileAssistantTitle;
            this.chatPanel1.Initialize(this.resourceCenter, this.resourceCenter.ClientGlobalCache.CurrentUser);
            this.chatPanel1.FileDragDroped += new CbGeneric<string[]>(chatPanel1_FileDragDroped);

            //文件传送
            this.fileTransferingViewer1.Initialize(this.resourceCenter.RapidPassiveEngine.FileOutter, this.FilterTransferingProject);
            this.fileTransferingViewer1.FileTransDisruptted += new CbGeneric<string, bool, FileTransDisrupttedType, string>(fileTransferingViewer1_FileTransDisruptted);
            //this.fileTransferingViewer.FileTransCompleted += new CbGeneric<string, bool ,string,bool>(fileTransferingViewer1_FileTransCompleted);
            this.fileTransferingViewer1.FileTransCompleted2 += new CbGeneric<TransferingProject>(fileTransferingViewer_FileTransCompleted2);
            this.fileTransferingViewer1.FileResumedTransStarted += new CbGeneric<string, bool>(fileTransferingViewer1_FileResumedTransStarted);
            this.fileTransferingViewer1.AllTaskFinished += new CbSimple(fileTransferingViewer1_AllTaskFinished);
            this.fileTransferingViewer1.FileNeedOfflineSend += new CbGeneric<TransferingProject>(fileTransferingViewer_FileNeedOfflineSend);
        }


        private bool FilterTransferingProject(TransferingProject pro)
        {
            if (!ESFramework.NetServer.IsServerUser(pro.DestUserID)) //文件传输助手-以离线文件方式实现
            {
                return false;
            }

            string offlineFileSenderID = Comment4OfflineFile.ParseUserID(pro.Comment);
            return offlineFileSenderID == this.resourceCenter.CurrentUserID;
        }

        void chatPanel1_FileDragDroped(string[] fileOrDirs)
        {
            foreach (string fileOrDirPath in fileOrDirs)
            {
                string projectID;
                SendingFileParas sendingFileParas = new SendingFileParas(20480, 5);
                // BeginSendFile方法
                //（1）accepterID传入null，表示文件的接收者就是服务端
                //（2）巧用comment参数表达离线文件，参见Comment4OfflineFile类
                this.resourceCenter.RapidPassiveEngine.FileOutter.BeginSendFile(null, fileOrDirPath, Comment4OfflineFile.BuildComment(this.resourceCenter.CurrentUserID, this.resourceCenter.CurrentClientType), sendingFileParas, out projectID);
                this.FileRequestReceived(projectID, false);
            }
        }

        #region 收到文件传输请求
        /// <summary>
        /// 当收到文件传输请求的时候 ，展开fileTransferingViewer,如果 本来就是展开 状态，直接添加
        /// 自己发送 文件请求的时候，也调用这里
        /// </summary>        
        internal void FileRequestReceived(string projectID, bool offlineFile)
        {
            TransferingProject pro = this.resourceCenter.RapidPassiveEngine.FileOutter.GetTransferingProject(projectID);
            if (offlineFile)
            {
                this.chatPanel1.AppendSystemMsg(string.Format("收到文件'{0}'，大小：{1}", pro.ProjectName, ESBasic.Helpers.PublicHelper.GetSizeString(pro.TotalSize)));
            }

            this.fileTransferingViewer1.NewFileTransferItem(projectID, offlineFile, false);

            this.skinPanel_right.Visible = true;
            this.splitter1.Visible = true;
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
            ChatMessageRecord record = new ChatMessageRecord(this.resourceCenter.CurrentUserID, this.resourceCenter.CurrentUserID, binaryContent, false);
            this.resourceCenter.LocalChatRecordPersister.InsertChatMessageRecord(record);
        }
        #endregion

        #region fileTransferingViewer1_AllTaskFinished
        private void fileTransferingViewer1_AllTaskFinished()
        {
            this.skinPanel_right.Visible = false;
            this.splitter1.Visible = false;
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
                this.resourceCenter.RapidPassiveEngine.FileOutter.BeginSendFile(null, project.OriginPath,
                    Comment4OfflineFile.BuildComment(this.resourceCenter.CurrentUserID, this.resourceCenter.CurrentClientType), sendingFileParas, out projectID);
                this.FileRequestReceived(projectID);
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(ee.Message, GlobalResourceManager.SoftwareName);
            }
        }

        public void HandleChatMessage(byte[] info, DateTime? msgOccureTime)
        {
            ChatBoxContent content = CompactPropertySerializer.Default.Deserialize<ChatBoxContent>(info, 0);
            this.chatPanel1.HandleChatMessage(null, content, msgOccureTime, null);
            this.FlashChatWindow();
        }

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

        #endregion

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!this.resourceCenter.Connected)
            {
                return;
            }

            try
            {
                string filePath = ESBasic.Helpers.FileHelper.GetFileToOpen("请选择要传输的文件");
                if (filePath == null)
                {
                    return;
                }
                string projectID;
                SendingFileParas sendingFileParas = new SendingFileParas(20480, 5);//文件数据包大小，可以根据网络状况设定，局网内可以设为204800，传输速度可以达到30M/s以上；公网建议设定为2048或4096或8192

                // BeginSendFile方法
                //（1）accepterID传入null，表示文件的接收者就是服务端
                //（2）巧用comment参数，参见Comment4OfflineFile类
                this.resourceCenter.RapidPassiveEngine.FileOutter.BeginSendFile(null, filePath,
                    Comment4OfflineFile.BuildComment(this.resourceCenter.CurrentUserID, this.resourceCenter.CurrentClientType), sendingFileParas, out projectID);
                this.FileRequestReceived(projectID);
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(ee.Message, GlobalResourceManager.SoftwareName);
            }
        }

        private void FileAssistantForm_Shown(object sender, EventArgs e)
        {
            this.chatPanel1.Set4FileAssistant();
        }

        private void FileAssistantForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.fileTransferingViewer1.BeforeDispose();
        }

        #region IChatForm
        public string UnitID => this.resourceCenter.CurrentUserID;

        public void MyselfOffline()
        {

        }

        public void FriendStateChanged(string friendID, UserStatus newStatus)
        {

        }

        public void FriendInfoChanged(IUser user)
        {

        }

        public void UnitCommentNameChanged(IUnit unit)
        {

        }

        public void MyInfoChanged(IUser my)
        {

        }

        public void RefreshUI()
        {

        }
        #endregion
    }
}
