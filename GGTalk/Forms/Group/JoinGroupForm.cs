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
using TalkBase.Client;
using TalkBase;

namespace GGTalk
{
    /// <summary>
    /// 加入群组。
    /// </summary>
    internal partial class JoinGroupForm : BaseForm
    {           
        private ResourceCenter<GGUser, GGGroup > resourceCenter;

        public JoinGroupForm(ResourceCenter<GGUser, GGGroup > center)
        {
            InitializeComponent();
            this.resourceCenter = center;
        }

        #region GroupID
        private string groupID = null;
        public string GroupID
        {
            get
            {
                return this.groupID;
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
            this.groupID = this.skinTextBox_id.SkinTxt.Text.Trim();
            if (groupID.Length == 0)
            {
                MessageBoxEx.Show("讨论组帐号不能为空！");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }           

            try
            {
                if (this.resourceCenter.ClientGlobalCache.CurrentUser.GroupList.Contains(this.groupID))
                {
                    MessageBoxEx.Show("已经是该讨论组的成员了！");
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }

                JoinGroupResult res = this.resourceCenter.ClientOutter.JoinGroup(groupID);
                if (res == JoinGroupResult.GroupNotExist)
                {
                    MessageBoxEx.Show("讨论组不存在！");
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show("加入讨论组失败！" + ee.Message);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }         
    }
}
