using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CCWin;
using CCWin.Win32;
using CCWin.Win32.Const;
using System.Diagnostics;
using System.Configuration;
using ESBasic.Security;
using ESPlus.Rapid;
using ESBasic;
using ESPlus.Application.CustomizeInfo;
using ESPlus.Application;
using TalkBase.Client;

namespace GGTalk
{
    /// <summary>
    /// 用户详细资料。
    /// </summary>
    public partial class UserInfoForm : BaseForm
    {
        private ResourceCenter<GGUser, GGGroup > resourceCenter;
        private GGUser currentFriend;
        private bool isShowDialog;

        public UserInfoForm()
        {
            InitializeComponent();
        }

        public UserInfoForm(ResourceCenter<GGUser, GGGroup> center, GGUser friend, bool showDialog = false)
        {
            InitializeComponent();
            this.resourceCenter = center;
            this.currentFriend = friend;
            this.isShowDialog = showDialog;
            this.pnlImgTx.BackgroundImage = GlobalResourceManager.GetHeadImageOnline(friend); //根据ID获取头像   
            this.linkLabel_ID.Text = friend.ID;
            this.skinLabel_Name.Text = friend.Name;
            this.linkLabel_commentName.Text = friend.CommentName;
            this.linkLabel_addComment.Visible = string.IsNullOrEmpty(friend.CommentName);
            this.skinLabel_signature.Text = friend.Signature;        
            this.skinLabel_register.Text = friend.CreateTime.ToString("yyyy-MM-dd");
        }

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

        private void skinLabel_ID_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.linkLabel_ID.Text);
            MessageBox.Show("帐号 已复制。");
        }

        private void linkLabel_ID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (currentFriend.ID == this.resourceCenter.CurrentUserID || this.isShowDialog)
            {
                return;
            }
            FriendChatForm form = (FriendChatForm)this.resourceCenter.ChatFormController.GetForm(currentFriend.UserID);
            form.Show();
            ESBasic.Helpers.WindowsHelper.SetForegroundWindow(form);
        }
    }
}
