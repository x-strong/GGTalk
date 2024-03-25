using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TalkBase.Client;

namespace GGTalk
{
    public partial class UserPanel2 : UserControl
    {
        private GGUser ggUser;
        private ResourceCenter<GGUser, GGGroup> center;
        private bool showUserInfo = true;

        public UserPanel2(ResourceCenter<GGUser, GGGroup> center,GGUser user)
        {
            InitializeComponent();
            this.center = center;
            this.ggUser = user;
            this.skinPictureBox1.Image = GlobalResourceManager.GetHeadImageOnline(ggUser);
            this.skinLabel_name.Text = user.DisplayName;
        }

        /// <summary>
        /// 设置是否显示用户详情
        /// </summary>
        /// <param name="showUserInfo"></param>
        public void SetIsShowUserInfo(bool showUserInfo)
        {
            this.showUserInfo = showUserInfo;
        }

        private void skinPictureBox1_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.skinPictureBox1, this.ggUser.ID);
        }

        private void skinPictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!this.showUserInfo)
            {
                return;
            }
            UserInfoForm form = new UserInfoForm(this.center, ggUser);
            form.Show();
        }
    }
}
