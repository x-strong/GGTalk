using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OMCS.Passive;
using OMCS.Passive.MultiChat;

namespace OMCS.Boost.MultiChat
{
    public partial class VideoGroupPanel : UserControl
    {
        public VideoGroupPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化群聊的子组控件。
        /// </summary>
        /// <param name="mgr">OMCS多媒体管理器。</param>
        /// <param name="group">OMCS聊天组。</param>
        /// <param name="list">子组列表</param>     
        public void Initialize(IMultimediaManager mgr, IChatGroup group ,List<SubGroup> list)
        {
            foreach (SubGroup sub in list)
            {
                VideoSubGroupPanel panel = new VideoSubGroupPanel();
                panel.Initialize(mgr, group, sub);
                this.flowLayoutPanel1.Controls.Add(panel);
            }
        }

         /// <summary>
        /// 将列表中的所有用户禁言。
        /// </summary>
        public void DisableAllSpeak()
        {
            foreach (VideoSubGroupPanel panel in this.flowLayoutPanel1.Controls)
            {
                panel.DisableAllSpeak();
            }
        }
    }   
}
