using GGTalk.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TalkBase.Client;
using TalkBase.Client.Bridges;

namespace GGTalk.FlatControls
{
    public partial class UserInfoPanel : FlatBasePanel
    {
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        private GGUser currentFriend;
        public UserInfoPanel(ResourceCenter<GGUser, GGGroup> center, GGUser friend)
        {
            InitializeComponent();
            this.resourceCenter = center;
            this.currentFriend = friend;
            this.pnlImgTx.BackgroundImage = GlobalResourceManager.GetHeadImageOnline(friend); //根据ID获取头像   
            this.linkLabel_ID.Text = friend.ID;
            this.skinLabel_Name.Text = friend.Name;
            this.linkLabel_commentName.Text = friend.CommentName;
            this.linkLabel_addComment.Visible = string.IsNullOrEmpty(friend.CommentName);
            this.skinLabel_signature.Text = friend.Signature;
            this.skinLabel_register.Text = friend.CreateTime.ToString("yyyy-MM-dd");
        }

        public override string ControlTitle => "";

        private void linkLabel_commentName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EditCommentNameForm form = new EditCommentNameForm(this.linkLabel_commentName.Text);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.resourceCenter.ClientOutter.ChangeUnitCommentName(this.currentFriend.ID, form.NewName);
                this.linkLabel_commentName.Text = form.NewName;
                this.linkLabel_addComment.Visible = string.IsNullOrEmpty(form.NewName);
            }
        }

        private void skinButton_send_Click(object sender, EventArgs e)
        {
            if (currentFriend.ID == this.resourceCenter.CurrentUserID)
            {
                return;
            }
            Program.ChatFormShower.ShowChatForm(this.resourceCenter.ChatFormController.GetForm(currentFriend.UserID));
        }
    }
}
