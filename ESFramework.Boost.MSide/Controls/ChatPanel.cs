using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESBasic;
using System.Threading;
using ESPlus.Serialization;
using ESPlus.Application;
using ESPlus.FileTransceiver;
using ESFramework.Boost.Controls;
using ESPlus.Rapid;

namespace ESFramework.Boost
{
    /// <summary>
    /// 聊天面板。适用于单聊和群聊。
    /// </summary>
    public partial class ChatPanel : UserControl 
    {
        private Font messageFont = new Font("微软雅黑", 9);  
        private EmotionForm emotionForm;
        private IRapidPassiveEngine rapidPassiveEngine;
        private IChatPanelHelper chatPanelHelper;
        private string destUnitID; //可以为好友的UserID或群的GroupID
        private bool destUnitIsGroup = false; //是与单个好友聊天、还是群聊？
        private string currentUserName = "";
        public event CbGeneric<string> FileDragDroped; //projectID
        public event CbGeneric VibrationClicked;

        #region Ctor
        public ChatPanel()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.UserPaint, true);//自行绘制            
            this.UpdateStyles();
        } 
        #endregion

        #region Initialize
        public void Initialize(IRapidPassiveEngine engine, IChatPanelHelper helper, string accepter, bool isGroup)
        {
            this.rapidPassiveEngine = engine;
            this.chatPanelHelper = helper;
            this.destUnitID = accepter;
            this.destUnitIsGroup = isGroup;
            this.currentUserName = this.chatPanelHelper.GetName(this.rapidPassiveEngine.CurrentUserID);

            this.Disposed += new EventHandler(ChatPanel_Disposed);
            this.toolVibration.Visible = !this.destUnitIsGroup;

            this.emotionForm = new EmotionForm();
            this.emotionForm.Load += new EventHandler(emotionForm_Load);
            this.emotionForm.Initialize(this.chatPanelHelper.GetEmotionList());
            this.emotionForm.EmotionClicked += new CbGeneric<int, Image>(emotionForm_Clicked);
            this.emotionForm.Visible = false;
            this.emotionForm.LostFocus += new EventHandler(emotionForm_LostFocus);

            Dictionary<uint, Image> dic = this.chatPanelHelper.GetEmotionDictionary();
            this.chatBox_history.Initialize(dic);
            this.chatBoxSend.Initialize(dic);
            this.chatBoxSend.EnableAutoDragDrop = false;
            this.chatBoxSend.AllowDrop = !this.destUnitIsGroup;
            this.chatBoxSend.FileOrFolderDragDrop += new CbGeneric<string[]>(chatBoxSend_FileOrFolderDragDrop);
            this.chatBoxSend.Focus();
        }         
        #endregion

        #region HandleChatMessage
        public void HandleChatMessage(string speakerID, ChatBoxContent content, DateTime? originTime)
        {
            string name = this.chatPanelHelper.GetName(speakerID);
            if (!this.destUnitIsGroup)
            {
                this.AppendChatBoxContent(name, originTime, content, Color.Blue, false);                
            }
            else
            {                
                string talker = string.Format("{0}({1})", name, speakerID);                
                this.AppendChatBoxContent4Group(talker, originTime, content, Color.Blue);
            }            
        } 
        #endregion

        #region ShowVibrationMessage
        public void ShowVibrationMessage()
        {
            string name = this.chatPanelHelper.GetName(this.destUnitID);
            string msg = name + "给您发送了抖动提醒。\n";
            this.AppendMessage(name, Color.Blue, msg);           
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
        #endregion

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

        #region ScrollToCaret4Histoty
        public void ScrollToCaret4Histoty()
        {
            this.chatBox_history.ScrollToCaret();
        }
        #endregion

        #region private
        #region 发送
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.rapidPassiveEngine.Connected)
                {
                    this.toolShow.Show("已经掉线。", this.skinButton_send, new Point(this.skinButton_send.Width / 2, -this.skinButton_send.Height), 3000);
                    return;
                }

                ChatBoxContent content = this.chatBoxSend.GetContent();
                if (content.IsEmpty())
                {
                    return;
                }

                ++this.sendingCount;
                this.gifBox_wait.Visible = true;
                ResultHandler handler = new ResultHandler(this.HandleSentResult);
                if (!this.destUnitIsGroup)
                {
                    this.chatPanelHelper.SendChatMessage(this.destUnitID, content, handler);
                }
                else
                {
                    this.chatPanelHelper.SendGroupChatMessage(this.destUnitID, content, handler);
                }

                this.AppendChatBoxContent(this.currentUserName, null, content, Color.SeaGreen, false);

                //清空输入框
                this.chatBoxSend.Clear();
                this.chatBoxSend.Focus();
            }
            catch (Exception ee)
            {
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
                this.toolShow.Show("因为网络原因，刚才的消息尚未发送成功！", this.skinButton_send, new Point(this.skinButton_send.Width / 2, -this.skinButton_send.Height), 3000);
            }
        }
        #endregion

        #region ProcessCmdKey
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                this.skinButton_send.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion               

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
            }
        }
        #endregion

        #region 手写板
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            IPaintForm paintForm = this.chatPanelHelper.CreatePaintForm();
            Form form = (Form)paintForm;
            form.Location = new Point(this.Left + 20, this.Top + skToolMenu.Top - form.Height);
            if (DialogResult.OK == form.ShowDialog())
            {
                Image bitmap = paintForm.CurrentImage;
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
                MessageBox.Show(ee.Message);
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

            bool hideFormWhenCaptureScreen = true;
            if (hideFormWhenCaptureScreen)
            {
                containerForm.WindowState = FormWindowState.Minimized;
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
                MessageBox.Show(ee.Message);
            }

            if (hideFormWhenCaptureScreen)
            {
                containerForm.WindowState = FormWindowState.Normal;
                SetForegroundWindow(containerForm.Handle);
            }
        }
        #endregion

        #region 聊天记录
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!this.rapidPassiveEngine.Connected)
            {
                return;
            }

            Form form = this.chatPanelHelper.GetChatRecordForm(this.destUnitID); ;
            if (form != null)
            {
                form.Show();
            }
        }
        #endregion

        void ChatPanel_Disposed(object sender, EventArgs e)
        {
            this.emotionForm.Visible = false;
            this.emotionForm.Close();
        }

        void chatBoxSend_FileOrFolderDragDrop(string[] fileOrDirs)
        {
            if (this.FileDragDroped == null)
            {
                return;
            }

            foreach (string fileOrDirPath in fileOrDirs)
            {
                string projectID;
                SendingFileParas sendingFileParas = new SendingFileParas(2048, 0);//文件数据包大小，可以根据网络状况设定，局网内可以设为204800，传输速度可以达到30M/s以上；公网建议设定为2048或4096或8192
                this.rapidPassiveEngine.FileOutter.BeginSendFile(this.destUnitID, fileOrDirPath, null, sendingFileParas, out projectID);
                this.FileDragDroped(projectID);
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
            if (this.destUnitIsGroup)
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
            this.chatPanelHelper.SendInputingNotify(this.destUnitID);
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
            if (!this.rapidPassiveEngine.Connected)
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
            this.chatPanelHelper.SendVibration(this.destUnitID);            
            this.AppendMessage(this.currentUserName, Color.Green, "您发送了一个抖动提醒。\n");

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
            followingWords = false;

            if (!followingWords)
            {
                string showTime = DateTime.Now.ToLongTimeString();
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

            this.chatBox_history.AppendChatBoxContent(content);
            
            this.chatBox_history.Select(this.chatBox_history.Text.Length, 0);
            this.chatBox_history.ScrollToCaret();
        } 
        #endregion        
        #endregion                       
    }

    #region IChatPanelHelper
    /// <summary>
    /// 聊天面板帮助类。
    /// </summary>
    public interface IChatPanelHelper
    {
        void SendChatMessage(string unitID, ChatBoxContent content, ResultHandler handler);

        void SendGroupChatMessage(string unitID, ChatBoxContent content, ResultHandler handler);

        List<Image> GetEmotionList();

        Dictionary<uint, Image> GetEmotionDictionary();

        string GetName(string unitID);

        Form GetChatRecordForm(string unitID);

        IPaintForm CreatePaintForm();

        void SendVibration(string unitID);

        void SendInputingNotify(string unitID);
    }

    /// <summary>
    /// 手写板基础接口。
    /// </summary>
    public interface IPaintForm
    {
        /// <summary>
        /// 完成手写后，得到的图片。
        /// </summary>
        Image CurrentImage { get; }
    }
    #endregion
}
