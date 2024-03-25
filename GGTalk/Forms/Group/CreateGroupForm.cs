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
using System.Linq;

namespace GGTalk
{
    /// <summary>
    /// 创建群、讨论组。
    /// </summary>
    public partial class CreateGroupForm : BaseForm
    {
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        string groupID;

        public CreateGroupForm(ResourceCenter<GGUser, GGGroup> center)
        {
            InitializeComponent();
            this.resourceCenter = center;
            this.groupID = FunctionOptions.PrefixGroupID + this.resourceCenter.CurrentUserID + "_" + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
       
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
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (this.groupID.Length == 0)
            {
                MessageBoxEx.Show("讨论组帐号不能为空！");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            if (this.skinTextBox_name.SkinTxt.Text.Trim().Length == 0)
            {
                MessageBoxEx.Show("讨论组名称不能为空！");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            try
            {
                List<string> newMembers = new List<string>();

                newMembers.Add(this.resourceCenter.CurrentUserID);
                UserSelectedForm form = new UserSelectedForm();
                GGGroup newGroup = new GGGroup();
                newGroup.CreatorID = this.resourceCenter.CurrentUserID;
                form.Initialize(this.resourceCenter.ClientGlobalCache, newGroup);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    List<string> list = form.UserIDSelected;
                    newMembers.AddRange(list);
                }

                CreateGroupResult res = this.resourceCenter.ClientOutter.CreateGroup(groupID, this.skinTextBox_name.SkinTxt.Text.Trim(), this.skinTextBox_announce.SkinTxt.Text,new List<string>(newMembers.Distinct()), this.skinCheckBox1.Checked);                      
                if (res == CreateGroupResult.GroupExisted)
                {
                    MessageBoxEx.Show("同ID的讨论组已经存在！");
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
                this.group = this.resourceCenter.ClientGlobalCache.GetGroup(groupID);
                this.Close();
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show("创建讨论组失败！" + ee.Message);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }

        private void skinTextBox_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是退格和数字，则屏蔽输入
            if (!(e.KeyChar == '\b' || (e.KeyChar >= '0' && e.KeyChar <= '9')))
            {
                e.Handled = true;
            }
        }
    }
}
