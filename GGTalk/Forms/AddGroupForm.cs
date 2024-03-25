using CCWin;
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
    public partial class AddGroupForm : BaseForm
    {
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        private GGGroup group;

        public AddGroupForm(ResourceCenter<GGUser, GGGroup> center,GGGroup group)
        {
            InitializeComponent();
            this.resourceCenter = center;
            this.group = group;
            this.linkLabel_id.Text = group.ID.Substring(1);
            this.label_name.Text = group.Name;
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            string groupID = this.group.ID;

            if (groupID.Length == 0)
            {
                MessageBoxEx.Show("组编号不能为空！");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
            try
            {
                IGroup group = this.resourceCenter.ClientOutter.SearchGroup(groupID);
                if (group == null)
                {
                    MessageBoxEx.Show("讨论组不存在！");
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
                if (group.MemberList.Contains(this.resourceCenter.CurrentUserID))
                {
                    MessageBoxEx.Show("您已存在讨论组中！");
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
                string comment = this.skinTextBox_comment.SkinTxt.Text.Trim();
                this.resourceCenter.ClientOutter.RequestAddGroup(groupID, comment);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show("申请加入群组失败！" + ee.Message);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }

        private void linkLabel_id_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GroupInfoForm form = new GroupInfoForm(this.resourceCenter, this.group);
            form.Show();
        }
    }
}
