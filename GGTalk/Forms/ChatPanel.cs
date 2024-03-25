using ESBasic;
using ESBasic.Helpers;
using ESBasic.ObjectManagement.Managers;
using ESFramework.Boost.Controls;
using ESFramework.Extensions.ChatRendering;
using ESPlus.Application;
using ESPlus.Serialization;
using GGTalk.Controls.ChatRender4Dll;
using GGTalk.Forms;
using GGTalk.Models;
using OMCS.Passive;
using OMCS.Passive.ShortMessages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TalkBase;
using TalkBase.Client;
using TalkBase.Client.Application;

namespace GGTalk
{
    /// <summary>
    /// 聊天面板。（可在好友聊天和群组聊天中复用）
    /// </summary>
    public partial class ChatPanel : UserControl, IUserNameGetter
    {
        private Font messageFont = new Font("微软雅黑", 9);  
        private EmotionForm emotionForm;
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        private IUnit destUnit;       
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
                return this.resourceCenter.RemoteChatRecordGetter;
            }
        }
        private int pageSize = 30;
        private int pageIndex = 0;
        private int currentPageIndex = -1;
        private IChatRender chatRender => this.chatBox_history?.ChatRender;

        public ChatPanel()
        {
            InitializeComponent();
            this.chatBox_history.chatPanel = this;
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.UserPaint, true);//自行绘制            
            this.UpdateStyles();

            //this.skinLabel_Ban.Location = this.PointToClient(this.chatBoxSend.Location);
        }

        public void Initialize(ResourceCenter<GGUser, GGGroup> center, IUnit dest)
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
                this.atSelectUserForm = new AtSelectUserFrom(center, (GGGroup)dest);
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
              
            this.chatBox_history.AllowDrop = this.destUnit.IsUser;
            this.chatBox_history.FileOrFolderDragDrop += new CbGeneric<string[]>(chatBoxSend_FileOrFolderDragDrop);
            this.chatBox_history.AudioMessageClicked += new CbGeneric<string, object>(AudioMessageClicked);
            this.chatBox_history.SnapchatMessageClicked += ChatBox_history_SnapchatMessageClicked;
            this.chatBox_history.ChatRender_ShowContextMenu += ChatBox_history_ChatRender_ShowContextMenu;
            this.InitChatBox_History();
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
                if (this.destUnit.LastWordsRecord != null)
                {
                    this.SetHistory(Program.ResourceCenter.ClientGlobalCache.CurrentUser, this.destUnit.LastWordsRecord.ChatContent);
                }
                this.chatBox_history.ScrollToBottom();

            }


            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserState == UserState.NoSpeaking)
            {
                this.SetControl_GroupBan();
            }
        }




        private void SetHistory(IUnit speaker, byte[] data)
        {
            ChatBoxContent content = CompactPropertySerializer.Default.Deserialize<ChatBoxContent>(data, 0);
            bool isMe = speaker.ID == Program.ResourceCenter.CurrentUserID;
            if (this.destUnit.IsUser)
            { 
                string name = isMe ? this.resourceCenter.ClientGlobalCache.CurrentUser.Name : this.destUnit.Name;
                this.AppendChatBoxContent(name, this.destUnit.LastWordsRecord.SpeakerID, this.destUnit.LastWordsRecord.SpeakTime, content, isMe ? Color.SeaGreen : Color.Blue, false, false);
            }
            else
            {
                this.AppendChatBoxContent4Group(this.destUnit.LastWordsRecord.SpeakerID, this.destUnit.LastWordsRecord.SpeakTime, content, Color.Blue);
            }
        }




        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if(this.chatBox_history != null)
                this.chatBox_history.ScrollToBottom();
        }

        private void SetHistoryRecord()
        {
            this.Cursor = Cursors.WaitCursor;
            CbGeneric cb = new CbGeneric(this.DoSetHistoryRecord);
            cb.BeginInvoke(null, null);

            //Task task = Task.Run(() =>
            //{
            //    try
            //    {
            //        //System.Threading.Thread.Sleep(250);
            //        ChatRecordPage page = this.GetChatRecordPage(this.pageIndex);
            //        if (page == null || page.Content.Count == 0)
            //        {
            //            return;
            //        }
            //        this.SetHistoryChatMessageRecord(page, true);
            //    }
            //    catch (Exception ee)
            //    {
            //        GlobalResourceManager.Logger.Log(ee, "SetHistoryRecord", ESBasic.Loggers.ErrorLevel.Standard);
            //    }

            //});
        }

        private void DoSetHistoryRecord()
        {
            try
            {
                //System.Threading.Thread.Sleep(250);
                ChatRecordPage page = this.GetChatRecordPage(this.pageIndex);
                if (page == null || page.Content.Count == 0)
                {
                    return;
                }
                this.SetHistoryChatMessageRecord(page, true);
            }
            catch (Exception ee)
            {
                GlobalResourceManager.Logger.Log(ee, "SetHistoryRecord", ESBasic.Loggers.ErrorLevel.Standard);
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
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new System.Action<ChatRecordPage, bool>(this.SetHistoryChatMessageRecord), page, lastPage);
            }
            else
            {
                try
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
                            this.AppendChatBoxContent(isMe ? this.resourceCenter.ClientGlobalCache.CurrentUser.Name : this.destUnit.Name, record.SpeakerID, record.OccureTime, content, isMe ? Color.SeaGreen : Color.Blue, false, false);
                        }
                        else
                        { 
                            this.AppendChatBoxContent4Group(record.SpeakerID, record.OccureTime, content, Color.Blue);
                        }
                    }
                }
                catch (Exception ee)
                {
                    GlobalResourceManager.Logger.Log(ee, "SetHistoryChatMessageRecord", ESBasic.Loggers.ErrorLevel.Standard);
                }
                finally
                {
                    this.Cursor = Cursors.Arrow;
                }               
            }


        }


        public void ScrollToCaret4Histoty()
        {
            this.chatBox_history.ScrollToBottom();
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
                this.AppendChatBoxContent(this.destUnit.Name, this.destUnit.ID, originTime, content, Color.Blue, followingWords);  
            }
            else
            {
                GGUser user = this.resourceCenter.ClientGlobalCache.GetUser(broadcasterID); 
                Color color = broadcasterID == this.resourceCenter.CurrentUserID ? Color.SeaGreen : Color.Blue;
                this.AppendChatBoxContent4Group(broadcasterID, originTime, content, color);
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
            this.AppendChatBoxContent(this.resourceCenter.ClientGlobalCache.CurrentUser.Name,this.resourceCenter.CurrentUserID, null, content, Color.SeaGreen, false);           
        } 
        #endregion

        #region OnFileEvent
        public void OnFileEvent(string msg, string filePath)
        {
            this.AppendSystemMsg(filePath);
        }
        #endregion

        #region ShowVibrationMessage
        public void ShowVibrationMessage()
        {
            string msg = this.destUnit.Name + "给您发送了抖动提醒。";
            this.AppendSystemMsg(msg);           
        }
        #endregion

        #region
        private void AppendMessage4InputAtMember(string msg)
        {
            this.chatBoxSend.AppendText(msg);
            this.SetChatBoxCursorToLast();
        }

        /// <summary>
        /// 在输入框中加入字符
        /// </summary>
        /// <param name="msg"></param>
        public void AppendTextToInputBox(string msg)
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
                    if (imageCapturer.ShowDialog() == DialogResult.OK)
                    {
                        ImagePreviewForm previewForm = new ImagePreviewForm(imageCapturer.Image);
                        if (previewForm.ShowDialog() == DialogResult.OK)
                        {
                            ChatBoxContent content = GetContentForImage(previewForm.image); 
                            this.SendChatBoxContent(content);
                        }
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
            this.AppendSystemMsg("您发送了一个抖动提醒。");

            this.chatBoxSend.Text = string.Empty;
            this.chatBoxSend.Focus();            
        }

        #region AppendMessage
        private void AppendChatBoxContent4Group(string userID, DateTime? speakTime, ChatBoxContent content, Color color)
        { 
            string str = CommonHelper.ParseChatBoxContent(content);
            Dictionary<uint, NonTextItem> dictionary_Img = content.NonTextItemDictionary;
            if (string.IsNullOrEmpty(str) && dictionary_Img?.Count > 0)
            {
                List<uint> list = dictionary_Img.Keys.ToList();
                foreach (var obj in list)
                {
                    NonTextItem nonTextItem = content.NonTextItemDictionary[obj];
                    if (nonTextItem.ChatBoxElementType == ChatBoxElementType.ForeignImage)
                    {
                        this.AppendImage(userID, nonTextItem.ForeignImage, speakTime);
                        continue;
                    }
                }
                return;
            }
            this.AppendText(userID, str, speakTime);
        }

        private void AppendChatBoxContent(string userName, string speakerID, DateTime? originTime, ChatBoxContent content, Color color, bool followingWords)
        {
            this.AppendChatBoxContent(userName, speakerID, originTime, content, color, followingWords, originTime != null);
        }

        private void AppendChatBoxContent(string userName, string speakerID, DateTime? originTime, ChatBoxContent content, Color color, bool followingWords, bool offlineMessage)
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
                if (originTime != null && offlineMessage)
                {
                    this.AppendSystemMsg(string.Format("    [{0} 离线消息] ", originTime.Value.ToString()));
                }
            } 
            string str = CommonHelper.ParseChatBoxContent(content);
            Dictionary<uint, NonTextItem> dictionary_Img = content.NonTextItemDictionary;
            if (string.IsNullOrEmpty(str) && dictionary_Img ?.Count > 0)
            {
                List<uint> list = dictionary_Img.Keys.ToList();
                foreach(var obj in list)
                {
                    NonTextItem nonTextItem = content.NonTextItemDictionary[obj];
                    if(nonTextItem.ChatBoxElementType == ChatBoxElementType.ForeignImage)
                    {
                        this.AppendImage(speakerID, nonTextItem.ForeignImage, originTime);
                        continue;
                    }
                } 
                return;
            }
            this.AppendText(speakerID, str, originTime);
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
                this.SendChatBoxContent(content);
                

                //清空输入框
                this.chatBoxSend.Clear();
                this.chatBoxSend.Focus();
            }
            catch (Exception ee)
            {
                this.resourceCenter.Logger.Log(ee, "ChatPanel.btnSend_Click", ESBasic.Loggers.ErrorLevel.Standard);
                this.AppendSystemMsg("发送消息失败！" + ee.Message);
            }
        }

        private void SendChatBoxContent(ChatBoxContent content)
        {
            byte[] buff = CompactPropertySerializer.Default.Serialize(content);

            ++this.sendingCount;
            this.gifBox_wait.Visible = true;
            ResultHandler handler = new ResultHandler(this.HandleSentResult);
            ChatMessageRecord record = null;
            if (this.destUnit.UnitType == UnitType.User)
            {
                record = this.resourceCenter.ClientOutter.SendChatMessage(this.destUnit.ID, buff, handler);
            }
            else if (this.destUnit.UnitType == UnitType.Group)
            {
                string atMemberIDs = this.GetAtMemberIDs();
                record = this.resourceCenter.ClientOutter.SendGroupChatMessage(this.destUnit.ID, buff, handler, atMemberIDs);
            }
            bool followingWords = false;

            if (this.destUnit.LastWordsRecord != null && this.destUnit.LastWordsRecord.SpeakerID == this.resourceCenter.CurrentUserID)
            {
                followingWords = (DateTime.Now - this.destUnit.LastWordsRecord.SpeakTime).TotalSeconds <= 30;
            }
            content.RecordID = (int)record.AutoID;

            this.AppendChatBoxContent(this.resourceCenter.ClientGlobalCache.CurrentUser.Name, this.resourceCenter.CurrentUserID, null, content, Color.SeaGreen, followingWords);
        }

        //0923
        private int sendingCount = 0;
        private void HandleSentResult(bool succeed, object tag)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<bool, object>(this.HandleSentResult), succeed, tag);
            }
            else {
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
            LinkText linkText = new LinkText("详细情况请点击\n123\n1223", "www.ggtalk.com");
            this.chatBoxSend.InsertLinkText(linkText);
            //this.skinButton_send.PerformClick();
        }

        #region 语言消息
        //2019.02.27 收到语音消息
        public void OnAudioMessage(OMCS.Passive.ShortMessages.AudioMessage msg, bool myslef)
        {  
            this.AppendAudio(msg.ID.ToString(), msg.CreatorID, msg,null);
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
             
            this.panel_audioMessage.Visible = false; 
            this.AppendAudio(msg.ID.ToString(), this.resourceCenter.CurrentUserID, msg, DateTime.Now);
            this.resourceCenter.ClientOutter.SendAudioMessage(this.destUnit.ID, msg);
        }
     

        private void AudioMessageClicked(string msgID, Object audioMessage)
        {

            if (!this.CheckAudioMessageController())
            {
                return;
            }
            if (this.chatRender.IsPlayingAudioMessageAnimation(msgID))
            {
                this.chatRender.StopPlayAudioMessageAnimation();
                MultimediaManagerFactory.GetSingleton().AudioMessageController.AudioMessagePlayer.Stop();
            }
            else
            {
                this.chatRender.StopPlayAudioMessageAnimation();
                this.chatRender.StartPlayAudioMessageAnimation(msgID);
                MultimediaManagerFactory.GetSingleton().AudioMessageController.AudioMessagePlayer.PlayInterrupted += AudioMessagePlayer_PlayInterrupted;
                MultimediaManagerFactory.GetSingleton().AudioMessageController.AudioMessagePlayer.PlayFinished += AudioMessagePlayer_PlayFinished;
                MultimediaManagerFactory.GetSingleton().AudioMessageController.AudioMessagePlayer.Play((AudioMessage)audioMessage);
            }
        }

       

        private void AudioMessagePlayer_PlayFinished(AudioMessage obj)
        {
            this.chatRender.StopPlayAudioMessageAnimation(obj.ID.ToString());
        }

        private void AudioMessagePlayer_PlayInterrupted(AudioMessage obj)
        {
            this.chatRender.StopPlayAudioMessageAnimation(obj.ID.ToString());
        }

        #endregion

        #region 禁言
        /// <summary>
        /// 移除禁止发言
        /// </summary>
        public void RemoveGroupBan()
        {
            this.SetControl_RemoveGroupBan();
            this.AppendSystemMsg("您已被管理员解除禁言");
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
            this.AppendSystemMsg("管理员关闭了全员禁言");
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
            this.AppendSystemMsg(msg);
        }

        /// <summary>
        /// 全员禁言
        /// </summary>
        public void SetAllGroupBan()
        {
            this.SetControl_GroupBan();
            this.skinLabel_Ban.Text = string.Format("全员禁言中，只允许管理员发言");
            string msg = "管理员开启了全员禁言";
            this.AppendSystemMsg(msg);
        }


        /// <summary>
        /// 移除禁言相关的控件
        /// </summary>
        private void SetControl_RemoveGroupBan()
        {
            this.toolStripButtonEmotion.Enabled = true;            
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

            this.AppendTextToInputBox(unit.Name + " ");
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
            string guid = message.ID; 
            this.AppendSnap(message, message.CreatorID, msgOccureTime);
        }

        private void ChatBox_history_SnapchatMessageClicked(string id)
        {
            //DialogResult dialogResult= MessageBoxEx.Show("该消息阅读后将自动销毁，确认现在阅读吗？","提示",MessageBoxButtons.OKCancel);
            //if (dialogResult == DialogResult.OK)
            //{ 
            SnapchatMessage snapchatMessage = this.GetSnapForId(id);
            Snapchat4ShowForm snapchat4ShowForm = new Snapchat4ShowForm(this.resourceCenter, snapchatMessage, 10);
            snapchat4ShowForm.Show();                
            if (snapchatMessage.CreatorID == this.resourceCenter.CurrentUserID)
            {
                return;
            }
            this.resourceCenter.ClientOutter.SendSnapchatReadMessage(new SnapchatMessageRead(snapchatMessage.ID,snapchatMessage.CreatorID)); 
            this.RemoveMsg(snapchatMessage.CreatorID +"-" + snapchatMessage.ID); 
            //}
        }

        public void HandleSnapchatRead(string messageID)
        {
            this.RemoveMsg(messageID);
        }
        #endregion

        #region InitChatBox_History

        /// <summary>
        /// 初始化ChatRender
        /// </summary>
        private void InitChatBox_History()
        { 
            bool multiSpeaker = this.destUnit.UnitType == UnitType.Group;
            RenderAttr attr = new RenderAttr(this.resourceCenter.CurrentUserID, this.destUnit.ID, multiSpeaker, WinRenderType.Winform);
            this.chatBox_history.InitChatRender(attr);
        }

        #endregion

        #region RenderChatMsg

        private List<string> guidMangers;
        private object locker = new object();
        private ObjectManager<string, SnapchatMessage> snapMangers;
        private DateTime lastMessageTime = DateTime.MinValue;

        /// <summary>
        /// 绘制系统时间
        /// </summary>
        /// <param name="time"></param>
        private void AppendSendTime(DateTime? sendTime = null)
        {
            lock (this.locker)
            {
                DateTime time = sendTime == null ? DateTime.Now : (DateTime)sendTime;
                if (time.Subtract(this.lastMessageTime) > TimeSpan.FromMinutes(5))
                {
                    this.lastMessageTime = time;
                    this.chatRender.AddChatItemTime(time);
                }
            }
        }


        /// <summary>
        /// 绘制系统消息
        /// </summary>
        /// <param name="strings"></param>
        public void AppendSystemMsg(string str)
        {
            if (string.IsNullOrEmpty(str)) return;
            this.AppendSendTime();
            this.chatRender.AddChatItemSystemMessage(str);
            this.chatBox_history.ScrollToBottom();
        }

        private void AppendText(string speakerID,string text,DateTime? time)
        {
            if (string.IsNullOrEmpty(text)) return;
            this.AppendSendTime(time);
            string guid = Guid.NewGuid().ToString();
            this.chatRender.AddChatItemText(guid, speakerID, text);
            this.AddGuidToManager(guid);
            this.chatBox_history.ScrollToBottom();
        }

        /// <summary>
        /// 在调用AppendImage时，需要生成一个唯一的GUID作为key Image是Value值存到字典中
        /// </summary>
        private ObjectManager<string, ImgModel> chatRenderImgList;
        private void SaveImageToManager(string guid, ImgModel img)
        {
            if (chatRenderImgList == null) chatRenderImgList = new ObjectManager<string, ImgModel>();
            chatRenderImgList.Add(guid, img);
        }

        private void AppendImage(string speakerID, Image Image, DateTime? time)
        {
            if (Image == null) return;
            Image img = ESFramework.Boost.CommonHelper.CropPicture(Image);
            this.AppendSendTime(time);
            string guid = Guid.NewGuid().ToString(); 
            ImgModel model = new ImgModel(guid, Image, img);  
            this.AppendSendTime(time);
            Image image = model.GetShowImage();
            this.chatRender.AddChatItemImage(guid, speakerID, image, image.Size);
            this.SaveImageToManager(guid, model);
            this.AddGuidToManager(guid);
            this.chatBox_history.ScrollToBottom();
        }

        private void AppendAudio(string audioID, string speakerID , AudioMessage msg, DateTime? time)
        {
            this.AppendSendTime(time);
            int secs = CommonHelper.GetAudioMessageSecs(msg);
            this.chatRender.AddChatItemAudio(audioID, speakerID, secs, msg);
            this.AddGuidToManager(audioID);
            this.chatBox_history.ScrollToBottom();
        }

        private void AppendSnap(SnapchatMessage snapchat, string speakerID, DateTime? time)
        {
            this.AppendSendTime(time);
            string guid = speakerID + "-" + snapchat.ID;
            this.AddSnapToManager(guid,snapchat);
            this.chatRender.AddChatItemSnap(guid, speakerID);
            this.AddGuidToManager(guid);
            this.chatBox_history.ScrollToBottom();
        }

        private void AddGuidToManager(string guid)
        {
            if (this.guidMangers == null)
                this.guidMangers = new List<string>();
            this.guidMangers.Add(guid);
        } 
         
        private void AddSnapToManager(string guid ,SnapchatMessage snapchat)
        {
            if (snapMangers == null) snapMangers = new ObjectManager<string, SnapchatMessage>();
            snapMangers.Add(guid,snapchat);
        }

        internal SnapchatMessage GetSnapForId(string id)
        {
            return snapMangers.Get(id);
        } 

        private void RemoveMsg(string guid)
        { 
            this.chatRender.RemoveChatMessage(guid);
        }

        #endregion

        #region ChatRender ContextMenu 

        private string selectGuid;

        private ChatMessageType selectType;
        private void ChatBox_history_ChatRender_ShowContextMenu(Point point, ChatMessageType type, string guid)
        {
            if (guidMangers == null || guidMangers.Count <=0) return;
            this.selectGuid = string.Empty;
            this.selectType = type;
            if (type == ChatMessageType.Image)
            {
                this.selectGuid = guid;
            }
            bool isShowMenuItem = !(string.IsNullOrEmpty(selectGuid));
            foreach (ToolStripItem obj in this.contextMenuStrip1.Items)
            {
                if (obj.Text == "复制" && this.selectType == ChatMessageType.TextEmotion || this.selectType == ChatMessageType.Image)
                {
                    obj.Visible = true;
                }
                else if (obj.Text == "另存为") obj.Visible = isShowMenuItem;
                else if (obj.Text == "新窗口显示") obj.Visible = isShowMenuItem; 
            } 
            Point loc = this.PointToScreen(point);
            loc.Y -= this.chatBox_history.offsetY;
            this.contextMenuStrip1.Show(loc); 
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectType == ChatMessageType.TextEmotion)
                {
                    string str = this.chatRender.GetSelectedText();
                    if (!string.IsNullOrEmpty(str))
                    {
                        Clipboard.SetText(str);
                    }
                }
                else if (selectType == ChatMessageType.Image)
                {
                    Image image = this.GetImageForGuid(selectGuid);
                    if (image == null) return;
                    Clipboard.SetImage(image);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            } 
        }

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Image image = this.GetImageForGuid(selectGuid);
                if (image == null) return;
                string path = CommonHelper.GetPathToSave("请选择保存路径", "image.jpg", null);
                if (path == null)
                {
                    return;
                }
                ImageHelper.Save(image, path, ImageFormat.Jpeg);
                MessageBox.Show("成功保存图片。");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void 新窗口显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image image = this.GetImageForGuid(selectGuid);
            if (image == null) return;
            ImageForm form = new ImageForm(image);
            form.Show();
        }

        private void 清屏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.guidMangers != null)
            {
                foreach (var guid in this.guidMangers)
                {
                    this.RemoveMsg(guid);
                }
            }
            this.guidMangers.Clear();
            this.snapMangers?.Clear();
            this.chatRenderImgList?.Clear();
            this.lastMessageTime = DateTime.MinValue;
            this.chatBox_history.ScrollToBottom(); 
            this.Focus();
        }


        internal Image GetImageForGuid(string guid)
        {
            ImgModel imgModel = chatRenderImgList.Get(guid);
            Image image = imgModel.sourceImg != null ? imgModel.sourceImg : imgModel.thumbnailImg;
            return image;
        }



        #endregion

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            string path = FileHelper.GetFileToOpen2("请选择图片", null, ".jpg", ".bmp", ".png");
            if (path == null)
            {
                return;
            } 
            if (string.IsNullOrEmpty(path)) return;
            if(!File.Exists(path))
            {
                MessageBox.Show("图片路径不存在！");
                return;
            } 
            using(Image img = Image.FromFile(path))
            {
                ImagePreviewForm previewForm = new ImagePreviewForm(img);
                if (previewForm.ShowDialog() == DialogResult.OK)
                {
                    ChatBoxContent content = GetContentForImage(previewForm.image);
                    this.SendChatBoxContent(content);
                }
            }
        }


        #region 通过Image构造ChatBoxContent

        public ChatBoxContent GetContentForImage(Image img)
        { 
            ChatBoxContent content = new ChatBoxContent(" ", this.Font, this.ForeColor);
            content.AddForeignImage(0, img);
            return content;
        }

        #endregion
    }
}
