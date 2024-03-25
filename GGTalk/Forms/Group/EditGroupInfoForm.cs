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
using ESPlus.Rapid;
using ESPlus.Serialization;
using TalkBase.Client;
using TalkBase;

namespace GGTalk
{
    /// <summary>
    /// 编辑群组的基本信息（公告，名称等）。
    /// </summary>
    public partial class EditGroupInfoForm : BaseForm
    {
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        private GGGroup currentGroup;

        public EditGroupInfoForm(ResourceCenter<GGUser, GGGroup> center, GGGroup group)
        {
            InitializeComponent();
            this.resourceCenter = center;
            this.currentGroup = group;
            this.linkLabel_groupID.Text = this.currentGroup.GroupID.StartsWith(FunctionOptions.PrefixGroupID)?this.currentGroup.GroupID.Remove(0,1):this.currentGroup.GroupID;
            this.skinTextBox_name.SkinTxt.Text = group.Name;
            this.skinTextBox_announce.SkinTxt.Text = group.Announce;
        }

        #region Group
        private GGGroup group = null;
        public GGGroup Group
        {
            get
            {
                return this.group;
            }
        } 
        #endregion        

        private void skinButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (this.skinTextBox_name.SkinTxt.Text.Trim().Length == 0)
            {
                MessageBoxEx.Show("讨论组名称不能为空！");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            try
            {
                this.resourceCenter.ClientOutter.ChangeGroupInfo(this.currentGroup.GroupID, this.skinTextBox_name.SkinTxt.Text.Trim(), this.skinTextBox_announce.SkinTxt.Text.Trim());             

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show("创建讨论组失败！" + ee.Message);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }

        private void linkLabel_groupID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(this.linkLabel_groupID.Text);
            MessageBoxEx.Show("已复制组编号！");
        }      
         
    }
}
