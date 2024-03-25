using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TalkBase;
using TalkBase.Client;

namespace GGTalk
{
    public partial class Snapchat4ShowForm : BaseForm
    {
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        private SnapchatMessage snapchatMessage;
        private int showSec;

        /// <summary>
        /// 显示自焚内容
        /// </summary>
        /// <param name="snapchatMessage">自焚消息</param>
        /// <param name="showSec">显示时长（单位：秒）</param>
        public Snapchat4ShowForm(ResourceCenter<GGUser, GGGroup> resourceCenter, SnapchatMessage snapchatMessage,int showSec)
        {
            InitializeComponent();
            this.resourceCenter = resourceCenter;
            this.snapchatMessage = snapchatMessage;
            this.chatBox1.Initialize(GlobalResourceManager.EmotionDictionary);
            if (this.snapchatMessage.CreatorID == resourceCenter.CurrentUserID)
            {
                return;
            }
            this.showSec = showSec;
            this.label1.Visible = true;
            this.label1.Text = showSec.ToString();
            this.timer1.Interval = 1000;
            this.timer1.Tick += Timer1_Tick;
            this.timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            --this.showSec;
            if (this.showSec <= 0)
            {
                this.timer1.Stop();
                this.Close();
                return;
            }
            this.label1.Text = this.showSec.ToString();
        }

        private void Snapchat4ShowForm_Shown(object sender, EventArgs e)
        {
            this.chatBox1.AppendChatBoxContent(snapchatMessage.ChatBoxContent);
            this.chatBox1.Select(0, 0);
            this.label1.Focus();            
        }
    }
}
