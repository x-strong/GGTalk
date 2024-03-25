using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CCWin;
using ESBasic;
using ESFramework.Boost.Controls;
using ESPlus.Serialization;
using OMCS.Passive;
using OMCS.Passive.ShortMessages;
using TalkBase;
using TalkBase.Client;

namespace GGTalk
{
    public partial class SnapchatForm : Form
    {
        private EmotionForm emotionForm;
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        private IUnit destUnit;
        /// <summary>
        /// 即焚消息已发送
        /// </summary>
        public event CbGeneric<SnapchatMessage> SnapchatMessageSent;

        public SnapchatForm(ResourceCenter<GGUser, GGGroup> center, IUnit dest)
        {
            InitializeComponent();
            this.resourceCenter = center;
            this.destUnit = dest;
            this.emotionForm = new EmotionForm();
            this.emotionForm.Load += new EventHandler(emotionForm_Load);
            this.emotionForm.Initialize(GlobalResourceManager.EmotionList);
            this.emotionForm.EmotionClicked += new CbGeneric<int, Image>(emotionForm_Clicked);
            this.emotionForm.Visible = false;
            this.emotionForm.LostFocus += new EventHandler(emotionForm_LostFocus);
            this.chatBoxSend.Initialize(GlobalResourceManager.EmotionDictionary);
        }

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


        #region 表情
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

        void emotionForm_Load(object sender, EventArgs e)
        {
            this.SetEmotionFormLocation();
        }

        void emotionForm_Clicked(int imgIndex, Image img)
        {
            this.chatBoxSend.InsertDefaultEmotion((uint)imgIndex);
            this.emotionForm.Visible = false;
        }

        void emotionForm_LostFocus(object sender, EventArgs e)
        {
            this.emotionForm.Visible = false;
        }
        #endregion

        #region 语音消息
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
        }

        //发送语音消息
        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserState == UserState.NoSpeaking)
            {
                MessageBoxEx.Show("您已被管理员禁言！");
                return;
            }
            AudioMessage msg = MultimediaManagerFactory.GetSingleton().AudioMessageController.StopCapture();
            if (msg == null)
            {
                MessageBox.Show("没有采集到声音数据，可能是没有麦克风设备！");
                this.panel_audioMessage.Visible = false;
                return;
            }
            this.panel_audioMessage.Visible = false;

            this.resourceCenter.ClientOutter.SendAudioMessage(this.destUnit.ID, msg);
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #region 发送文本消息

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.resourceCenter.RapidPassiveEngine.Connected)
                {
                    this.toolShow.Show("已经掉线。", this.skinButton_send, new Point(this.skinButton_send.Width / 2, -this.skinButton_send.Height), 3000);
                    return;
                }
                if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserState == UserState.NoSpeaking)
                {
                    MessageBoxEx.Show("您已被管理员禁言！");
                    return;
                }

                ChatBoxContent content = this.chatBoxSend.GetContent();
                if (content.IsEmpty())
                {
                    return;
                }
                if (this.destUnit.UnitType != UnitType.User)
                {
                    return;
                }
                SnapchatMessage snapchatMessage = new SnapchatMessage(DateTime.Now.Ticks.ToString(), this.resourceCenter.CurrentUserID, content);
                this.resourceCenter.ClientOutter.SendSnapchatMessage(this.destUnit.ID, snapchatMessage);

                if (this.SnapchatMessageSent != null)
                {
                    this.SnapchatMessageSent(snapchatMessage);
                }         

                //清空输入框
                this.chatBoxSend.Clear();
                this.Close();
            }
            catch (Exception ee)
            {
                this.resourceCenter.Logger.Log(ee, "SnapchatForm.btnSend_Click", ESBasic.Loggers.ErrorLevel.Standard);
                MessageBoxEx.Show("发送消息失败！" + ee.Message);
            }
        } 
        #endregion

    }
}
