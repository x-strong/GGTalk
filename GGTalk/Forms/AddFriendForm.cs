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
    /// 添加好友。
    /// </summary>
    internal partial class AddFriendForm : BaseForm
    {
        private ResourceCenter<GGUser, GGGroup> resourceCenter;

        public AddFriendForm(ResourceCenter<GGUser, GGGroup> center)
        {
            InitializeComponent();
            this.initForm(center);
        }

        public AddFriendForm(ResourceCenter<GGUser, GGGroup> center,string friendID)
        {
            InitializeComponent();
            this.initForm(center);
            this.skinTextBox_id.SkinTxt.Text = friendID;
            this.skinTextBox_id.SkinTxt.Enabled = false;
        }

        private void initForm(ResourceCenter<GGUser, GGGroup> center)
        {
            this.Icon = GlobalResourceManager.Icon64;
            this.resourceCenter = center;
            this.skinComboBox1.DataSource = this.resourceCenter.ClientGlobalCache.CurrentUser.GetFriendCatalogList();
            this.skinComboBox1.SelectedIndex = 0;
        }

        public void SetFriendID(string friendID)
        {
            this.skinTextBox_id.SkinTxt.Text = friendID;
            this.skinTextBox_id.SkinTxt.Enabled = false;
        }

        #region FriendID
        private string friendID = "";
        public string FriendID
        {
            get
            {
                return this.friendID;
            }
        } 
        #endregion        

        #region CatalogName
        private string catalogName = "";
        public string CatalogName
        {
            get { return catalogName; }
        }
        #endregion

        #region Comment
        private string comment = "";
        public string Comment
        {
            get { return comment; }
        }
        #endregion

        private void skinButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.friendID = this.skinTextBox_id.SkinTxt.Text.Trim();
            if (this.friendID.Length == 0)
            {
                MessageBoxEx.Show("帐号不能为空！");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }           

            try
            {
                if (this.resourceCenter.ClientGlobalCache.CurrentUser.GetAllFriendList().Contains(this.friendID))
                {
                    MessageBoxEx.Show("该用户已经是好友！");
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }

                this.catalogName = this.skinComboBox1.SelectedItem.ToString();
                //AddFriendResult res = this.resourceCenter.ClientOutter.AddFriend(this.friendID, this.catalogName);
                //if (res == AddFriendResult.FriendNotExist)
                //{
                //    MessageBoxEx.Show("帐号不存在！");
                //    this.DialogResult = System.Windows.Forms.DialogResult.None;
                //    return;
                //}
                this.comment = this.skinTextBox_comment.SkinTxt.Text.Trim();
                this.resourceCenter.ClientOutter.RequestAddFriend(this.friendID, this.comment, this.CatalogName);
                
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show("添加好友失败！" + ee.Message);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }      
         
    }
}
