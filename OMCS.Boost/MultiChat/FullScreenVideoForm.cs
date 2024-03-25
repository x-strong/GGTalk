using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OMCS.Passive.MultiChat;

namespace OMCS.Boost.MultiChat
{
    /// <summary>
    /// 某个成员的全屏视频窗口。
    /// 双击窗口，可以恢复到正常尺寸。
    /// </summary>
    public partial class FullScreenVideoForm : Form
    {
        private SpeakerVideoPanel speakerVideoPanel;

        public FullScreenVideoForm(SpeakerVideoPanel panel ,IChatUnit unit)
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.Text = unit.MemberID;
            this.speakerVideoPanel = panel;
            unit.DynamicCameraConnector.SetViewer(this.cameraPanel1);

            this.speakerVideoPanel.Visible = false;
        }

        private void FullScreenVideoForm_FormClosing(object sender, FormClosingEventArgs e)
        {            
            this.speakerVideoPanel.ResetViewer();

            this.speakerVideoPanel.Visible = true;
        }        

        private void videoPanel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }
    }
}
