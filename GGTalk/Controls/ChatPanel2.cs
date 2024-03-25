using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESBasic;
using TalkBase.Client;
using System.Threading;
using ESPlus.Serialization;
using ESPlus.Application;
using ESPlus.FileTransceiver;
using ESFramework.Boost.Controls;
using TalkBase;
using TalkBase.Client.Application;
using OMCS.Passive;
using OMCS.Passive.ShortMessages;
using ESBasic.Helpers;
using ESBasic.ObjectManagement.Managers;
using CCWin;

namespace OrayTalk
{
    /// <summary>
    /// 聊天面板。（可在好友聊天和群组聊天中复用）
    /// </summary>
    public partial class ChatPanel2 : UserControl, IUserNameGetter
    {
        private Font messageFont = new Font("微软雅黑", 9);  
        private EmotionForm emotionForm;
        private ResourceCenter<OrayUser, OrayGroup ,OrayOrganization> resourceCenter;
        private IUnit destUnit;
        private AudioDialogManager<OrayUser, OrayGroup, OrayOrganization> audioDialogManager;
        private VideoDialogManager<OrayUser, OrayGroup, OrayOrganization> videoDialogManager;
        public event CbGeneric<string[]> FileDragDroped; //projectID
        public event CbGeneric VibrationClicked;
        private AtSelectUserFrom atSelectUserForm;
        //key: userID,value:userName
        private ObjectManager<string, string> atSelectedUserManager;
        private bool IsInHisBlackList = false;
        private TwinkleNotifyIcon twinkleNotifyIcon;
        private IChatRecordGetter CurrentChatRecordGetter
        {
            get
            {
                return this.resourceCenter.LocalChatRecordPersister;
            }
        }

        public AudioDialogManager<OrayUser, OrayGroup, OrayOrganization> AudioDialogManager { set { this.audioDialogManager = value; this.toolStripButton_Audio.Visible = true; } }
        public VideoDialogManager<OrayUser, OrayGroup, OrayOrganization> VideoDialogManager { set { this.videoDialogManager = value; this.toolStripButton_Video.Visible = true; } }
        private int pageSize = 30;
        private int pageIndex = int.MaxValue;
        private int currentPageIndex = -1;

        public ChatPanel2()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.UserPaint, true);//自行绘制            
            this.UpdateStyles();

            //this.skinLabel_Ban.Location = this.PointToClient(this.chatBoxSend.Location);
        }

        public void Initialize(ResourceCenter<OrayUser, OrayGroup, OrayOrganization> center, IOwnerForm owner, IUnit dest)
        {
            this.resourceCenter = center;
            this.destUnit = dest;
            this.twinkleNotifyIcon = (TwinkleNotifyIcon)this.resourceCenter.GetExtendCache(CommonOptions.TwinkleNotifyIcon);
            this.toolStripButton_audioMsg.Visible = this.destUnit.IsUser;

            this.Disposed += new EventHandler(ChatPanel_Disposed);
            //this.toolVibration.Visible = this.destUnit.IsUser;
            this.toolStripMenuItem_ctrl.Checked = SystemSettings.Singleton.SendNeedCtrlKey;
            this.toolStripMenuItem_enter.Checked = !SystemSettings.Singleton.SendNeedCtrlKey;
            this.ToolStripMenuIte_hideForm.Checked = SystemSettings.Singleton.HideFormWhenCaptureScreen;
            if (dest.UnitType == UnitType.User)
            {
                this.toolStripButton_snapchat.Visible = true;
                if (this.resourceCenter.Connected)
                {
                    this.IsInHisBlackList = this.resourceCenter.ClientOutter.IsInHisBlackList(dest.ID);
                }
            }
            else if (dest.UnitType == UnitType.Group)
            {
                this.atSelectedUserManager = new ObjectManager<string, string>();
                this.atSelectUserForm = new AtSelectUserFrom(center, (OrayGroup)dest);
                this.atSelectUserForm.Visible = false;
                this.atSelectUserForm.MemberSelected += AtSelectUserForm_MemberSelected;
                this.atSelectUserForm.PanleClosed += AtSelectUserForm_PanleClosed;
                //this.atSelectUserForm.LostFocus += AtSelectUserForm_LostFocus;
            }

            this.emotionForm = new EmotionForm();
            this.emotionForm.Load += new EventHandler(emotionForm_Load);
            this.emotionForm.Initialize(GlobalResourceManager.EmotionList);
            this.emotionForm.EmotionClicked += new CbGeneric<int, Image>(emotionForm_Clicked);
            this.emotionForm.Visible = false;
            this.emotionForm.LostFocus += new EventHandler(emotionForm_LostFocus);

            this.chatBox_history.Initialize(GlobalResourceManager.EmotionDictionary);
            this.chatBox_history.EnableAutoDragDrop = false;
            this.chatBox_history.AllowDrop = this.destUnit.IsUser;
            this.chatBox_history.FileOrFolderDragDrop += new CbGeneric<string[]>(chatBoxSend_FileOrFolderDragDrop);
            this.chatBox_history.AudioMessageClicked += new CbGeneric<string, object>(chatBox_history_AudioMessageClicked);
            this.chatBox_history.SnapchatMessageClicked += ChatBox_history_SnapchatMessageClicked;

            this.chatBoxSend.Initialize(GlobalResourceManager.EmotionDictionary);
            this.chatBoxSend.Font = SystemSettings.Singleton.Font;
            this.chatBoxSend.ForeColor = SystemSettings.Singleton.FontColor;
            this.chatBoxSend.EnableAutoDragDrop = false;
            this.chatBoxSend.AllowDrop = this.destUnit.IsUser;
            this.chatBoxSend.FileOrFolderDragDrop += new CbGeneric<string[]>(chatBoxSend_FileOrFolderDragDrop);
            this.chatBoxSend.Focus();
            this.unhanleMessageCount = this.twinkleNotifyIcon.GetUnhanleMessageCount(this.destUnit);
            if (SystemSettings.Singleton.LoadLastWordsWhenChatFormOpened)
            {

                //if (this.destUnit.LastWordsRecord != null)
                //{


                #region 获取最后2页的聊天记录并显示到历史记录中
                ChatRecordPage page = this.GetChatRecordPage(this.pageIndex);
                if (page == null || page.Content.Count == 0)
                {
                    return;
                }
                this.currentPageIndex = page.PageIndex;
                if (this.currentPageIndex > 0)
                {
                    this.currentPageIndex--;
                    ChatRecordPage page2 = this.GetChatRecordPage(this.currentPageIndex);
                    this.SetHistoryChatMessageRecord(page2, false);
                }
                this.SetHistoryChatMessageRecord(page, true);

                #endregion
                //    ChatBoxContent chatBoxContent = CompactPropertySerializer.Default.Deserialize<ChatBoxContent>(this.destUnit.LastWordsRecord.ChatContent, 0);
                //    IUser speaker = this.resourceCenter.ClientGlobalCache.GetUser(this.destUnit.LastWordsRecord.SpeakerID);
                //    if (this.destUnit.IsUser)
                //    {
                //        bool isMe = speaker.ID == this.resourceCenter.CurrentUserID;
                //        this.AppendChatBoxContent(isMe ? this.resourceCenter.ClientGlobalCache.CurrentUser.Name : this.destUnit.Name, this.destUnit.LastWordsRecord.SpeakTime, chatBoxContent, isMe ? Color.SeaGreen : Color.Blue, false, false);
                //    }
                //    else
                //    {
                //        string talker = string.Format("{0}({1})", speaker.DisplayName, this.destUnit.LastWordsRecord.SpeakerID);
                //        this.AppendChatBoxContent4Group(talker, this.destUnit.LastWordsRecord.SpeakTime, chatBoxContent, Color.Blue);
                //    }
                this.chatBox_history.AppendRichText("                   —————— 以上是历史消息 ——————", new Font(this.messageFont, FontStyle.Regular), Color.Gray);
                this.chatBox_history.Select(this.chatBox_history.Text.Length, 0);
                this.chatBox_history.ScrollToCaret();
                //}
            }
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserState == UserState.NoSpeaking)
            {
                this.SetControl_GroupBan();
            }
        }
        //未处理消息数
        private int unhanleMessageCount = 0;
        private ChatRecordPage GetChatRecordPage(int pageIndex)
        {
            ChatRecordPage page = null;
            if (this.destUnit.UnitType == UnitType.Group)
            {
                page = this.CurrentChatRecordGetter.GetGroupChatRecordPage(ChatRecordTimeScope.All, this.destUnit.ID, this.pageSize, pageIndex);
            }
            else
            {
                page = this.CurrentChatRecordGetter.GetChatRecordPage(ChatRecordTimeScope.All, this.resourceCenter.CurrentUserID, this.destUnit.ID, this.pageSize, pageIndex);
            }
            return page;
        }

        private void SetHistoryChatMessageRecord(ChatRecordPage page, bool lastPage)
        {
            if (page == null || page.Content.Count == 0)
            {
                return;
            }
            int leftCount = page.Content.Count;
            if (lastPage)
            {                
                leftCount = page.Content.Count - this.unhanleMessageCount;// 删除最后几条未提取的消息记录
                if (leftCount <= 0)
                {
                    return;
                }
            }

            for (int i = 0; i < leftCount; i++)
            {
                ChatMessageRecord record = page.Content[i];
                byte[] msg = record.Content;
                ChatBoxContent content = ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<ChatBoxContent>(msg, 0);

                IUser speaker = this.resourceCenter.ClientGlobalCache.GetUser(record.SpeakerID);
                Parameter<string, string> my = this.resourceCenter.ClientGlobalCache.CurrentUser.GetIDName();
                //Parameter<string,string> dest=
                if (this.destUnit.IsUser)
                {
                    bool isMe = speaker.ID == this.resourceCenter.CurrentUserID;
                    this.AppendChatBoxContent(isMe ? this.resourceCenter.ClientGlobalCache.CurrentUser.Name : this.destUnit.Name, record.OccureTime, content, isMe ? Color.SeaGreen : Color.Blue, false, false);
                }
                else
                {
                    string talker = speaker.DisplayName + "（" + record.SpeakerID + ")";  //string.Format("{0}({1})", speaker.DisplayName, record.SpeakerID);
                    this.AppendChatBoxContent4Group(talker, record.OccureTime, content, Color.Blue);
                }

            }
        }


        public void ScrollToCaret4Histoty()
        {
            this.chatBox_history.ScrollToCaret();
        }

        public void Set4FileAssistant()
        {
            this.toolStripButton_audioMsg.Visible = false;
        }

        #region AddStripButton
        public void AddStripButton(string name, Image img, EventHandler onClick)
        {
            ToolStripItem item = new ToolStripButton(name, img, onClick);
            item.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.skToolMenu.Items.Add(item);
        } 
        #endregion

        #region FocusOnInputBox
        public void FocusOnInputBox()
        {
            this.chatBoxSend.Focus();
        }
        #endregion

        #region HandleChatMessage
        public void HandleChatMessage(string broadcasterID, ChatBoxContent content, DateTime? originTime,string tag)
        {
            if (this.destUnit.IsUser)
            {
                bool followingWords = false;
                if (this.destUnit.LastWordsRecord != null && this.destUnit.LastWordsRecord.SpeakerID != this.resourceCenter.CurrentUserID)
                {
                    followingWords = (DateTime.Now - this.destUnit.LastWordsRecord.SpeakTime).TotalSeconds <= 30;
                }
                this.AppendChatBoxContent(this.destUnit.Name, originTime, content, Color.Blue, followingWords);  
            }
            else
            {
                OrayUser user = this.resourceCenter.ClientGlobalCache.GetUser(broadcasterID);
                string talker = string.Format("{0}({1})", broadcasterID, broadcasterID);
                if (user != null)
                {
                    talker = string.Format("{0}({1})", user.Name, user.UserID);
                }
                Color color = broadcasterID == this.resourceCenter.CurrentUserID ? Color.SeaGreen : Color.Blue;
                this.AppendChatBoxContent4Group(talker, originTime, content, color);
                //检查是否自己被@
                if (string.IsNullOrEmpty(tag))
                {
                    return;
                }
                List<string> memberIDs = new List<string>(tag.Split(','));

                if (memberIDs.Contains(this.resourceCenter.CurrentUserID)|| memberIDs.Contains("@all"))
                {
                    this.atMessagePanle1.Initialize(user.DisplayName, GlobalResourceManager.GetHeadImageOnline(user), content);
                    this.atMessagePanle1.Visible = true;
                }
            }

        }

        public void HandleChatMessageOfMine(ChatBoxContent content)
        {
            this.AppendChatBoxContent(this.resourceCenter.ClientGlobalCache.CurrentUser.Name, null, content, Color.SeaGreen, false);           
        } 
        #endregion

        #region OnFileEvent
        public void OnFileEvent(string msg, string filePath)
        {
            this.AppendMessage("系统", Color.Gray, msg);
            this.chatBox_history.AppendText("\n");
            if (filePath != null)
            {                
                this.chatBox_history.AppendFileLink("    " ,filePath);
                this.chatBox_history.AppendText("\n");
            }
        }
        #endregion

        #region ShowVibrationMessage
        public void ShowVibrationMessage()
        {
            string msg = this.destUnit.Name + "给您发送了抖动提醒。\n";
            this.AppendMessage(this.destUnit.Name, Color.Blue, msg);           
        }
        #endregion

        #region AppendMessage ,AppendSysMessage
        public void AppendMessage(string userName, Color color, string msg)
        {
            DateTime showTime = DateTime.Now;
            this.chatBox_history.AppendRichText(string.Format("{0}  {1}", userName, showTime.ToLongTimeString()), new Font(this.messageFont, FontStyle.Regular), color);
            this.chatBox_history.AppendText("    ");

            this.chatBox_history.AppendText(msg);
            this.chatBox_history.Select(this.chatBox_history.Text.Length, 0);
            this.chatBox_history.ScrollToCaret();
        }

        public void AppendSysMessage(string msg)
        {
            this.AppendMessage("系统", Color.Gray, msg);
            this.chatBox_history.AppendText("\n");
        }

        private void AppendMessage4InputAtMember(string msg)
        {
            this.chatBoxSend.AppendText(msg);
            this.SetChatBoxCursorToLast();
        }

        /// <summary>
        /// 在输入框中加入字符
        /// </summary>
        /// <param name="msg"></param>
        public void AppendText(string msg)
        {
            this.chatBoxSend.AppendText(msg);
            this.SetChatBoxCursorToLast();
        }

        //将光标定位最最后一个字符后
        private void SetChatBoxCursorToLast()
        {            
            this.chatBoxSend.Focus();
            this.chatBoxSend.Select(this.chatBoxSend.Text.Length, 0);
            this.chatBoxSend.ScrollToCaret();
        }
        #endregion

        #region private
        #region 字体
        //显示字体对话框
        private void toolFont_Click(object sender, EventArgs e)
        {
            this.fontDialog1.Font = this.chatBoxSend.Font;
            this.fontDialog1.Color = this.chatBoxSend.ForeColor;
            if (DialogResult.OK == this.fontDialog1.ShowDialog())
            {
                this.chatBoxSend.Font = this.fontDialog1.Font;
                this.chatBoxSend.ForeColor = this.fontDialog1.Color;

                SystemSettings.Singleton.FontColor = this.fontDialog1.Color;
                SystemSettings.Singleton.Font = this.fontDialog1.Font;
                SystemSettings.Singleton.Save();
            }
        }
        #endregion

        #region 手写板
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            PaintForm form = new PaintForm();
            form.Location = new Point(this.Left + 20, this.Top + skToolMenu.Top - form.Height);
            if (DialogResult.OK == form.ShowDialog())
            {
                Bitmap bitmap = form.CurrentImage;
                if (bitmap != null)
                {
                    this.chatBoxSend.InsertImage(bitmap);
                    this.chatBoxSend.Focus();
                    this.chatBoxSend.ScrollToCaret();
                }
            }
        }
        #endregion

        #region 图片，截屏
        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            this.CaptureScreen(false);
        }

        private void 发送本地图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string file = ESBasic.Helpers.FileHelper.GetFileToOpen2("请选择图片", null, ".jpg", ".bmp", ".png", ".gif");
                if (file == null)
                {
                    return;
                }

                Image img = Image.FromFile(file);
                this.chatBoxSend.InsertImage(img);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, GlobalResourceManager.SoftwareName);
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);//设置此窗体为活动窗体

        private void 发送屏幕截屏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CaptureScreen(true);
        }

        private void buttonCapture_Click(object sender, EventArgs e)
        {
            
        }

        private void CaptureScreen(bool allScreen)
        {
            Form containerForm = this.ParentForm;
            if (this.ParentForm.MdiParent != null)
            {
                containerForm = this.ParentForm.MdiParent;
            }

            if (SystemSettings.Singleton.HideFormWhenCaptureScreen)
            {
                //containerForm.WindowState = FormWindowState.Minimized;
                containerForm.Visible = false;
                System.Threading.Thread.Sleep(100);
            }
            try
            {
                if (allScreen)
                {
                    Bitmap capturedBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                    Graphics graphics4CapturedBitmap = Graphics.FromImage(capturedBitmap);
                    graphics4CapturedBitmap.CopyFromScreen(new Point(0, 0), new Point(0, 0), Screen.PrimaryScreen.Bounds.Size);
                    graphics4CapturedBitmap.Dispose();
                    this.chatBoxSend.InsertImage(capturedBitmap);
                }
                else
                {
                    ScreenCapturer imageCapturer = new ScreenCapturer();
                    if (imageCapturer.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        this.chatBoxSend.InsertImage(imageCapturer.Image);
                        this.chatBoxSend.Focus();
                        this.chatBoxSend.ScrollToCaret();
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, GlobalResourceManager.SoftwareName);
            }

            if (SystemSettings.Singleton.HideFormWhenCaptureScreen)
            {
                //containerForm.WindowState = FormWindowState.Normal;
                containerForm.Visible = true;
                SetForegroundWindow(containerForm.Handle);
            }
        }
        #endregion

        #region 聊天记录
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            ChatRecordForm form = null;
            if (this.destUnit.IsUser)
            {
                form = new ChatRecordForm(this.resourceCenter, this.resourceCenter.ClientGlobalCache.CurrentUser.GetIDName(), this.destUnit.GetIDName(), GlobalResourceManager.EmotionDictionary);
            }
            else
            {
                form = new ChatRecordForm(this.resourceCenter, this.destUnit.GetIDName(), this.resourceCenter.ClientGlobalCache.CurrentUser.GetIDName(), this, GlobalResourceManager.EmotionDictionary);
            }
            form.Show();
        }
        #endregion

        void ChatPanel_Disposed(object sender, EventArgs e)
        {
            this.emotionForm.Visible = false;
            this.emotionForm.Close();
            if (this.atSelectedUserManager != null)
            {
                this.atSelectUserForm.Visible = false;
                this.atSelectUserForm.Close();
            }
        }

        void chatBoxSend_FileOrFolderDragDrop(string[] fileOrDirs)
        {
            if (this.FileDragDroped != null)
            {
                this.FileDragDroped(fileOrDirs);                
            }            
        }

        void emotionForm_LostFocus(object sender, EventArgs e)
        {
            this.emotionForm.Visible = false;
        }

        void emotionForm_Load(object sender, EventArgs e)
        {
            this.SetEmotionFormLocation();
        }

        private DateTime dtLastSendInptingNotify = DateTime.Now;
        private void textBoxSend_TextChanged(object sender, EventArgs e)
        {
            if (!this.destUnit.IsUser)
            {
                return;
            }

            if (this.chatBoxSend.TextLength == 0)
            {
                return;
            }

            if ((DateTime.Now - this.dtLastSendInptingNotify).TotalSeconds <= 5)
            {
                return;
            }

            this.dtLastSendInptingNotify = DateTime.Now; //20150316
            this.resourceCenter.ClientOutter.SendInputingNotify(this.destUnit.ID);
        }

        private void toolStripButtonEmotion_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void toolStripButtonEmotion_MouseUp(object sender, MouseEventArgs e)
        {
            this.SetEmotionFormLocation();
            this.emotionForm.Visible = !this.emotionForm.Visible;
        }

        private void SetEmotionFormLocation()
        {
            Point pt = this.PointToScreen(this.skToolMenu.Location);
            Point pos = new Point(pt.X + 30 - (this.emotionForm.Width / 2), pt.Y - this.emotionForm.Height);
            
            if (pos.X < 10)
            {
                pos = new Point(10, pos.Y);
            }
            this.emotionForm.Location = pos;
        }

        void emotionForm_Clicked(int imgIndex, Image img)
        {
            this.chatBoxSend.InsertDefaultEmotion((uint)imgIndex);
            this.emotionForm.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.ParentForm.Close();
        }

        private DateTime lastVibrationTime = DateTime.Now.AddSeconds(-30);
        //震动
        private void toolVibration_Click(object sender, EventArgs e)
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            //if ((DateTime.Now - this.lastVibrationTime).TotalSeconds < 15)
            //{
            //    return;
            //}

            if (this.VibrationClicked != null)
            {
                this.VibrationClicked();
            }

            this.lastVibrationTime = DateTime.Now;
            this.resourceCenter.ClientOutter.SendVibration(this.destUnit.ID);
            this.AppendMessage(this.resourceCenter.ClientGlobalCache.CurrentUser.Name, Color.Green, "您发送了一个抖动提醒。\n");

            this.chatBoxSend.Text = string.Empty;
            this.chatBoxSend.Focus();            
        }

        #region AppendMessage
        private void AppendChatBoxContent4Group(string userName, DateTime? speakTime, ChatBoxContent content, Color color)
        {
            string showTime = speakTime == null ? DateTime.Now.ToLongTimeString() : speakTime.ToString();
            this.chatBox_history.AppendRichText(string.Format("{0}  {1}", userName, showTime), new Font(this.messageFont, FontStyle.Regular), color);
            this.chatBox_history.AppendText("    ");
            this.chatBox_history.AppendChatBoxContent(content);
            this.chatBox_history.Select(this.chatBox_history.Text.Length, 0);
            this.chatBox_history.ScrollToCaret();
        }

        private void AppendChatBoxContent(string userName, DateTime? originTime, ChatBoxContent content, Color color, bool followingWords)
        {
            this.AppendChatBoxContent(userName, originTime, content, color, followingWords, originTime != null);
        }

        private void AppendChatBoxContent(string userName, DateTime? originTime, ChatBoxContent content, Color color, bool followingWords, bool offlineMessage)
        {
            int startIndex = this.chatBox_history.Text.Length;
            followingWords = false;

            if (!followingWords)
            {
                string showTime = DateTime.Now.ToString();
                if (!offlineMessage && originTime != null)
                {
                    showTime = originTime.Value.ToString();
                }
                this.chatBox_history.AppendRichText(string.Format("{0}  {1}", userName, showTime), new Font(this.messageFont, FontStyle.Regular), color);
                if (originTime != null && offlineMessage)
                {
                    this.chatBox_history.AppendText(string.Format("    [{0} 离线消息] ", originTime.Value.ToString()));
                }
                else
                {
                    this.chatBox_history.AppendText("    ");
                }
            }
            else
            {
                this.chatBox_history.AppendText("   .");
            }

            try
            {
                this.chatBox_history.AppendChatBoxContent(content);
                this.chatBox_history.Select(this.chatBox_history.Text.Length, 0);
                this.chatBox_history.ScrollToCaret();
            }
            catch (Exception ee)
            {

            }

            int count = this.chatBox_history.Text.Length - startIndex;
        } 
        #endregion        
        #endregion        

        public string GetUserName(string userID)
        {
            return this.resourceCenter.ClientGlobalCache.GetUserName(userID);
        }

        #region 发送
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.resourceCenter.RapidPassiveEngine.Connected)
                {
                    MessageBox.Show("已经掉线。");
                    //this.toolShow.Show("已经掉线。", this.skinButton_send, new Point(this.skinButton_send.Width / 2, -this.skinButton_send.Height), 3000);
                    return;
                }
                if (this.IsInHisBlackList)
                {
                    MessageBox.Show("对方已将您加入黑名单，不能发送消息！");
                    return;
                }

                ChatBoxContent content = this.chatBoxSend.GetContent();
                if (content.IsEmpty())
                {
                    if (FunctionOptions.ShortcutResponse)
                    {
                        this.LoadQuickAnswer(this.resourceCenter.ClientGlobalCache.QuickAnswerList);
                    }
                    return;
                }

                byte[] buff = CompactPropertySerializer.Default.Serialize(content);

                ++this.sendingCount;
                this.gifBox_wait.Visible = true;
                UIResultHandler handler = new UIResultHandler(this, this.HandleSentResult);
                if (this.destUnit.UnitType == UnitType.User)
                {
                    this.resourceCenter.ClientOutter.SendChatMessage(this.destUnit.ID, buff, handler.Create());                    
                }
                else if (this.destUnit.UnitType == UnitType.Group)
                {
                    string atMemberIDs = this.GetAtMemberIDs();
                    this.resourceCenter.ClientOutter.SendGroupChatMessage(this.destUnit.ID, buff, handler.Create(), atMemberIDs);
                }
                else
                {
                    this.resourceCenter.ClientOutter.SendOrgChatMessage(this.destUnit.ID, buff, handler.Create());
                }

                bool followingWords = false;

                if (this.destUnit.LastWordsRecord != null && this.destUnit.LastWordsRecord.SpeakerID == this.resourceCenter.CurrentUserID)
                {
                    followingWords = (DateTime.Now - this.destUnit.LastWordsRecord.SpeakTime).TotalSeconds <= 30;
                }             

                this.AppendChatBoxContent(this.resourceCenter.ClientGlobalCache.CurrentUser.Name, null, content, Color.SeaGreen, followingWords);

                //清空输入框
                this.chatBoxSend.Clear();
                this.chatBoxSend.Focus();
            }
            catch (Exception ee)
            {
                this.resourceCenter.Logger.Log(ee, "ChatPanel.btnSend_Click", ESBasic.Loggers.ErrorLevel.Standard);
                this.AppendSysMessage("发送消息失败！" + ee.Message);
            }
        }

        //0923
        private int sendingCount = 0;
        private void HandleSentResult(bool succeed, object tag)
        {
            --this.sendingCount;
            if (this.sendingCount <= 0)
            {
                this.sendingCount = 0;
                this.gifBox_wait.Visible = false;
            }

            if (!succeed)
            {
                MessageBox.Show("因为网络原因，刚才的消息尚未发送成功！");
               // this.toolShow.Show("因为网络原因，刚才的消息尚未发送成功！", this.skinButton_send, new Point(this.skinButton_send.Width / 2, -this.skinButton_send.Height), 3000);
            }
        }

        private void LoadQuickAnswer(List<string> answers)
        {
            this.skinContextMenuStrip_quickAnswer.Items.Clear();
            foreach (string answer in answers)
            {
                ToolStripItem item = this.skinContextMenuStrip_quickAnswer.Items.Add(answer, null, new EventHandler(this.OnQucikAnswerClicked));
                item.Tag = 0;
            }
            this.skinContextMenuStrip_quickAnswer.Items.Add(new ToolStripSeparator());
            ToolStripItem sysItem = new ToolStripMenuItem("进行快捷回复菜单设置", null, new EventHandler(this.OnQucikAnswerClicked));
            sysItem.Tag = 1;
            this.skinContextMenuStrip_quickAnswer.Items.Add(sysItem);
            this.skinContextMenuStrip_quickAnswer.Show(skinButton_send, new Point(-this.skinContextMenuStrip_quickAnswer.Width/2, -this.skinContextMenuStrip_quickAnswer.Height - 5));
        }

        private void OnQucikAnswerClicked(object sender, EventArgs args)
        {
            ToolStripItem item = sender as ToolStripItem;
            if (item == null || item.Tag == null)
            {
                return;
            }

            if (item.Tag.ToString() == "0")
            {
                this.chatBoxSend.Text = item.Text;
                this.skinButton_send.PerformClick();
                return;
            }

            QuickAnswerForm form = new QuickAnswerForm(this.resourceCenter.ClientGlobalCache.QuickAnswerList);
            form.Show();
        }
        #endregion        

        #region 发送方式
        private void toolStripMenuItem_ctrl_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItem_ctrl.Checked = true;
            this.toolStripMenuItem_enter.Checked = false;
            SystemSettings.Singleton.SendNeedCtrlKey = this.toolStripMenuItem_ctrl.Checked;
            SystemSettings.Singleton.Save();
        }

        private void toolStripMenuItem_enter_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItem_ctrl.Checked = false;
            this.toolStripMenuItem_enter.Checked = true;
            SystemSettings.Singleton.SendNeedCtrlKey = this.toolStripMenuItem_ctrl.Checked;
            SystemSettings.Singleton.Save();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!this.skinButton_send.Enabled)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
            if ((keyData == (Keys.Control | Keys.Enter)) && SystemSettings.Singleton.SendNeedCtrlKey)
            {
                this.skinButton_send.PerformClick();
                return true;
            }

            if (keyData == Keys.Enter && !SystemSettings.Singleton.SendNeedCtrlKey)
            {
                this.skinButton_send.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnSendMenu_Click(object sender, EventArgs e)
        {
            this.skinContextMenuStrip4Send.Show(btnSendMenu, new Point(0, btnSendMenu.Height + 5));
        } 
        #endregion        

        private void 截图时隐藏窗体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ToolStripMenuIte_hideForm.Checked = !this.ToolStripMenuIte_hideForm.Checked;
            SystemSettings.Singleton.HideFormWhenCaptureScreen = this.ToolStripMenuIte_hideForm.Checked;
            SystemSettings.Singleton.Save();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            LinkText linkText = new LinkText("详细情况请点击\n123\n1223", "www.oraycn.com");
            this.chatBoxSend.InsertLinkText(linkText);
            //this.skinButton_send.PerformClick();
        }

        #region 语言消息
        //2019.02.27 收到语音消息
        public void OnAudioMessage(OMCS.Passive.ShortMessages.AudioMessage msg, bool myslef)
        {
            msg.PlayFinished += new CbGeneric<AudioMessage>(msg_PlayFinished);
            msg.PlayInterrupted += new CbGeneric<AudioMessage>(msg_PlayInterrupted);

            if (myslef)
            {
                this.chatBox_history.AppendRichText(string.Format("{0}  {1}", this.resourceCenter.ClientGlobalCache.CurrentUser.Name, DateTime.Now), new Font(this.messageFont, FontStyle.Regular), Color.SeaGreen);
            }
            else
            {
                this.chatBox_history.AppendRichText(string.Format("{0}  {1}", this.destUnit.DisplayName, DateTime.Now), new Font(this.messageFont, FontStyle.Regular), Color.Blue);
            }
            this.chatBox_history.InsertAudioMessageBox(msg.GetUniqueID(), msg.SpanInMSecs / 1000 + 1, msg);
        }

        private bool audioMessageControllerIniDone = false; //是否调用过语音消息控制器的初始化
        //检查语音消息控制器的状态
        private bool CheckAudioMessageController()
        {
            if (MultimediaManagerFactory.GetSingleton().AudioMessageController.Initialized)
            {
                return true;
            }

            if (this.audioMessageControllerIniDone)
            {
                MessageBox.Show("语音消息控制器初始化失败！");
                return false;
            }

            try
            {
                this.audioMessageControllerIniDone = true;
                MultimediaManagerFactory.GetSingleton().AudioMessageController.Initialize();
            }
            catch (Exception ee)
            {
                MessageBox.Show("语音消息控制器初始化失败！" + ee.Message);
                return false;
            }

            return true;
        }

        //开始录制语音消息
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (!this.CheckAudioMessageController())
            {
                return;
            }
            if (this.IsInHisBlackList)
            {
                MessageBox.Show("对方已将您加入黑名单，不能发送语音消息！");
                return;
            }
            try
            {
                this.panel_audioMessage.Visible = true;                
                MultimediaManagerFactory.GetSingleton().AudioMessageController.StartCapture();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        //取消语音消息
        private void skinButton2_Click(object sender, EventArgs e)
        {
            this.panel_audioMessage.Visible = false;
            MultimediaManagerFactory.GetSingleton().AudioMessageController.StopCapture();
        }

        //发送语音消息
        private void skinButton1_Click(object sender, EventArgs e)
        {
            AudioMessage msg = MultimediaManagerFactory.GetSingleton().AudioMessageController.StopCapture();
            if (msg == null)
            {
                MessageBox.Show("没有采集到声音数据，可能是没有麦克风设备！");
                this.panel_audioMessage.Visible = false;
                return;
            }

            msg.PlayFinished += new CbGeneric<AudioMessage>(msg_PlayFinished);
            msg.PlayInterrupted += new CbGeneric<AudioMessage>(msg_PlayInterrupted);
            this.panel_audioMessage.Visible = false;
            this.chatBox_history.AppendRichText(string.Format("{0}  {1}", this.resourceCenter.ClientGlobalCache.CurrentUser.Name, DateTime.Now), new Font(this.messageFont, FontStyle.Regular), Color.SeaGreen);
            this.chatBox_history.InsertAudioMessageBox(msg.GetUniqueID(), msg.SpanInMSecs / 1000 + 1, msg);
            this.resourceCenter.ClientOutter.SendAudioMessage(this.destUnit.ID, msg);
        }

        void msg_PlayInterrupted(AudioMessage msg)
        {
            this.chatBox_history.StopPlayAudioMessageAnimation(msg.GetUniqueID());
        }

        void msg_PlayFinished(AudioMessage msg)
        {
            this.chatBox_history.StopPlayAudioMessageAnimation(msg.GetUniqueID());
        }

        void chatBox_history_AudioMessageClicked(string msgID, object msg)
        {
            if (!this.CheckAudioMessageController())
            {
                return;
            }

            if (this.chatBox_history.IsPlayingAudioMessageAnimation(msgID))
            {
                this.chatBox_history.StopPlayAudioMessageAnimation(msgID);
                MultimediaManagerFactory.GetSingleton().AudioMessageController.AudioMessagePlayer.Stop();
            }
            else
            {
                this.chatBox_history.StartPlayAudioMessageAnimation(msgID);
                MultimediaManagerFactory.GetSingleton().AudioMessageController.AudioMessagePlayer.Play((AudioMessage)msg);
            }
        } 
        #endregion

        #region 禁言
        /// <summary>
        /// 移除禁止发言
        /// </summary>
        public void RemoveGroupBan()
        {
            this.SetControl_RemoveGroupBan();
            this.AppendSysMessage("您已被管理员解除禁言");
        }

        /// <summary>
        /// 移除全员禁言
        /// </summary>
        public void RemoveAllGroupBan()
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserState == UserState.NoSpeaking)
            {
                return;
            }
            this.SetControl_RemoveGroupBan();
            this.AppendSysMessage("管理员关闭了全员禁言");
        }

        /// <summary>
        /// 禁止发言
        /// </summary>
        /// <param name="minutes">禁言时长（单位分钟）</param>
        public void SetGroupBan(string operatorID, double minutes)
        {
            this.SetControl_GroupBan();
            string banDurationStr = TimeHelper.GetDurationStr(new TimeSpan(0, (int)minutes, 0));
            this.skinLabel_Ban.Text = string.Format("禁言中，{0}后解禁", banDurationStr);
            string msg = "您已被管理员禁言";
            this.AppendSysMessage(msg);
        }

        /// <summary>
        /// 全员禁言
        /// </summary>
        public void SetAllGroupBan()
        {
            this.SetControl_GroupBan();
            this.skinLabel_Ban.Text = string.Format("全员禁言中，只允许管理员发言");
            string msg = "管理员开启了全员禁言";
            this.AppendSysMessage(msg);
        }


        /// <summary>
        /// 移除禁言相关的控件
        /// </summary>
        private void SetControl_RemoveGroupBan()
        {
            this.toolStripButtonEmotion.Enabled = true;            
            this.toolStripButton3.Enabled = true;
            this.toolStripSplitButton1.Enabled = true;
            this.toolStripButton_audioMsg.Enabled = true;
            this.toolStripButton_snapchat.Enabled = true;
            this.skinButton1.Enabled = true;
            this.skinButton_send.Enabled = true;
            this.chatBoxSend.Enabled = true;
            this.skinLabel_Ban.Visible = false;            
        }

        /// <summary>
        /// 设置禁言相关的控件
        /// </summary>
        private void SetControl_GroupBan()
        {
            this.toolStripButtonEmotion.Enabled = false;
            this.toolStripButton3.Enabled = false;
            this.toolStripSplitButton1.Enabled = false;
            this.toolStripButton_audioMsg.Enabled = false;
            this.toolStripButton_snapchat.Enabled = false;
            this.skinButton1.Enabled = false;
            this.skinButton_send.Enabled = false;
            this.chatBoxSend.Enabled = false;
            this.skinLabel_Ban.Visible = true;
        } 
        #endregion

        #region @功能

        private bool lastChatIsAite = false;//输入的最后一个字符为@
        private void chatBoxSend_TextChanged(object sender, EventArgs e)
        {
            if (this.destUnit.UnitType != UnitType.Group)
            {
                return;
            }
            string text = this.chatBoxSend.Text;
            if (text.Length > 0 && text.LastIndexOf('@') == text.Length - 1)
            {
                this.lastChatIsAite = true;
                Point point = this.chatBoxSend.GetPositionFromCharIndex(text.Length - 1);
                Point pt = this.PointToScreen(point);
                this.ChatPanel1_InputAiteCharEvent(new Point(pt.X + 10, pt.Y + this.chatBox_history.Height + this.skToolMenu.Height));
            }
            else if (this.lastChatIsAite)
            {
                this.lastChatIsAite = false;
                this.ChatPanel1_RemoveAiteCharEvent();
            }
        }

        private void AtSelectUserForm_PanleClosed()
        {
            this.chatBoxSend.Text = this.chatBoxSend.Text.Remove(this.chatBoxSend.Text.Length - 1, 1);
            this.SetChatBoxCursorToLast();
        }

        private void AtSelectUserForm_LostFocus(object sender, EventArgs e)
        {
            this.atSelectUserForm.Visible = false;
        }

        private void AtSelectUserForm_MemberSelected(IUnit unit)
        {
            if (unit.UnitType != UnitType.User)
            {
                return;
            }
            this.AppendMessage4InputAtMember(unit.Name + " ");
            this.atSelectedUserManager.Add(unit.ID, unit.Name);
        }

        private void ChatPanel1_RemoveAiteCharEvent()
        {
            this.atSelectUserForm.Visible = false;
        }

        private void ChatPanel1_InputAiteCharEvent(Point point)
        {
            this.atSelectUserForm.Location = point;
            this.atSelectUserForm.Visible = true;
            this.atSelectUserForm.Location = point;
            this.chatBoxSend.Focus();
        }

        //获取at用户的ID 集合
        private string GetAtMemberIDs()
        {
            string memberIDs = "";
            string text = this.chatBoxSend.Text;
            Dictionary<string, string> dic = this.atSelectedUserManager.ToDictionary();
            foreach (KeyValuePair<string,string> kvp in dic)
            {
                if (text.Contains('@' + kvp.Value))
                {
                    memberIDs += kvp.Key + ",";
                }
            }
            if (memberIDs.Length > 0)
            {
                memberIDs = memberIDs.Substring(0, memberIDs.Length - 1);
            }
            this.atSelectedUserManager.Clear();
            return memberIDs;
        }

        /// <summary>
        /// 添加@对象到输入框
        /// </summary>
        /// <param name="unit"></param>
        public void AddAtMember(IUnit unit)
        {
            if (unit.UnitType != UnitType.User)
            {
                return;
            }
            int startIndex = this.chatBoxSend.Text.LastIndexOf('@');
            if (startIndex + 1 < this.chatBoxSend.Text.Length)
            {
                this.chatBoxSend.Text = this.chatBoxSend.Text.Remove(startIndex + 1);
            }

            this.AppendText(unit.Name + " ");
            this.atSelectedUserManager.Add(unit.ID, unit.Name);
        }

        #endregion

        #region 阅后即焚
        private void toolStripButton_snapchat_Click(object sender, EventArgs e)
        {
            if (this.IsInHisBlackList)
            {
                MessageBox.Show("对方已将您加入黑名单，不能发送悄悄话！");
                return;
            }
            SnapchatForm snapchatForm = new SnapchatForm(this.resourceCenter, this.destUnit);
          //  snapchatForm.Location = new Point(this.ParentForm.Location.X + this.skToolMenu.Location.X , this.ParentForm.Location.Y + this.skToolMenu.Location.Y);
            snapchatForm.SnapchatMessageSent += SnapchatForm_SnapchatMessageSent;
            snapchatForm.ShowDialog();
        }

        private void SnapchatForm_SnapchatMessageSent(SnapchatMessage message)
        {
            this.InsertSnapchatMessageBox(message,null);
        }

        public void OnSnapchatMessage(SnapchatMessage snapchatMessage, DateTime? msgOccureTime)
        {
            this.InsertSnapchatMessageBox(snapchatMessage, msgOccureTime);
        }

        private void InsertSnapchatMessageBox(SnapchatMessage message, DateTime? msgOccureTime)
        {
            int startIndex = this.chatBox_history.AppendRichText(string.Format("{0}  {1}", this.resourceCenter.ClientGlobalCache.GetUser(message.CreatorID).DisplayName, msgOccureTime==null? DateTime.Now: msgOccureTime), new Font(this.messageFont, FontStyle.Regular), Color.SeaGreen);
            SnapchatMessageBox snapchatMessageBox = new SnapchatMessageBox();
            snapchatMessageBox.Initialze(message, message.GetUniqueID());
            int endIndex = this.chatBox_history.InsertSnapchatMessageBox(snapchatMessageBox);
            snapchatMessageBox.SetPostion4InChatBox(startIndex, endIndex);
        }

        private void ChatBox_history_SnapchatMessageClicked(SnapchatMessageBox snapchatMessageBox)
        {            
            //DialogResult dialogResult= MessageBoxEx.Show("该消息阅读后将自动销毁，确认现在阅读吗？","提示",MessageBoxButtons.OKCancel);
            //if (dialogResult == DialogResult.OK)
            //{
                SnapchatMessage snapchatMessage = (SnapchatMessage)snapchatMessageBox.Message;
                Snapchat4ShowForm snapchat4ShowForm = new Snapchat4ShowForm(this.resourceCenter, snapchatMessage, 10);
                snapchat4ShowForm.Show();                
                if (snapchatMessage.CreatorID == this.resourceCenter.CurrentUserID)
                {
                    return;
                }
                this.resourceCenter.ClientOutter.SendSnapchatReadMessage(new SnapchatMessageRead(snapchatMessage.ID,snapchatMessage.CreatorID));
                this.chatBox_history.RemoveSnapchatMessageBox(snapchatMessageBox.MessageID);
            //}
        }

        public void HandleSnapchatRead(string messageID)
        {
            this.chatBox_history.RemoveSnapchatMessageBox(messageID);
        }
        #endregion

        private void toolStripButton_voice_Click(object sender, EventArgs e)
        {
            this.audioDialogManager.RequestAudioDialog();
        }

        private void toolStripButton_Video_Click(object sender, EventArgs e)
        {
            this.videoDialogManager.RequestVideoDialog();
        }
    }
}
