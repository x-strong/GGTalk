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
    /// 修改密码。
    /// </summary>
    public partial class ChangePasswordForm : BaseForm
    {
        private ResourceCenter<GGUser, GGGroup> resourceCenter;

        public ChangePasswordForm(ResourceCenter<GGUser, GGGroup> center)
        {
            InitializeComponent();
            this.resourceCenter = center;
        }   

        private void skinButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (this.skinTextBox_new.SkinTxt.Text != this.skinTextBox_new2.SkinTxt.Text)
            {
                MessageBoxEx.Show("两次密码输入不一致！");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }           

            try
            {
                ChangePasswordContract contract = new ChangePasswordContract(ESBasic.Security.SecurityHelper.MD5String2(this.skinTextBox_old.SkinTxt.Text.Trim()), ESBasic.Security.SecurityHelper.MD5String2(this.skinTextBox_new.SkinTxt.Text));
                ChangePasswordResult res = this.resourceCenter.ClientOutter.ChangeMyPassword(ESBasic.Security.SecurityHelper.MD5String2(this.skinTextBox_old.SkinTxt.Text.Trim()), ESBasic.Security.SecurityHelper.MD5String2(this.skinTextBox_new.SkinTxt.Text));
               
                if (res == ChangePasswordResult.OldPasswordWrong)
                {
                    MessageBoxEx.Show("旧密码不正确！");
                    this.skinTextBox_old.SkinTxt.Focus();
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }

                if (res == ChangePasswordResult.UserNotExist)
                {
                    MessageBoxEx.Show("用户不存在！");
                    this.skinTextBox_old.SkinTxt.Focus();
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }

                MessageBoxEx.Show("密码修改成功！");
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show("修改密码失败！" + ee.Message);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }

        private void ChangePasswordForm_Load(object sender, EventArgs e)
        {

        }      
         
    }
}
