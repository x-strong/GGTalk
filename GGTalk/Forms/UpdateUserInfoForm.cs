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
using TalkBase;

namespace GGTalk
{
    /// <summary>
    /// 修改个人资料。
    /// </summary>
    public partial class UpdateUserInfoForm : BaseForm
    {
        private int headImageIndex = 0;
        private ResourceCenter<GGUser, GGGroup > resourceCenter;   
       
        public event CbGeneric<GGUser> UserInfoChanged; 
       

        public UpdateUserInfoForm(ResourceCenter<GGUser, GGGroup > center)
        {
            InitializeComponent();
#if !QQLike
            this.radioButton2.Visible = false;
#endif

            this.resourceCenter = center;   

            this.skinLabel_ID.Text = this.resourceCenter.ClientGlobalCache.CurrentUser.UserID;
            this.skinTextBox_nickName.SkinTxt.Text = this.resourceCenter.ClientGlobalCache.CurrentUser.Name;
            this.skinTextBox_signature.SkinTxt.Text = this.resourceCenter.ClientGlobalCache.CurrentUser.Signature;
            this.orgID = this.resourceCenter.ClientGlobalCache.CurrentUser.OrgID;            

            if (this.resourceCenter.ClientGlobalCache.CurrentUser.HeadImageIndex >= 0)
            {
                this.headImageIndex = this.resourceCenter.ClientGlobalCache.CurrentUser.HeadImageIndex;
                this.pnlImgTx.BackgroundImage = GlobalResourceManager.GetHeadImage(this.resourceCenter.ClientGlobalCache.CurrentUser); //根据ID获取头像   
            }
            else
            {
                this.pnlImgTx.BackgroundImage = this.resourceCenter.ClientGlobalCache.CurrentUser.HeadImage;
                this.selfPhoto = true;
            }
        }        

        private void skinButton1_Click(object sender, EventArgs e)
        {
            this.Close();       
        }
        
        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                //0923
                if (!this.resourceCenter.Connected)
                {
                    this.toolTip1.Show("离线状态，无法修改资料。", this.btnRegister, new Point(this.btnRegister.Width / 2, -this.btnRegister.Height), 3000);
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                GGUser mine = this.resourceCenter.ClientGlobalCache.CurrentUser;
                string newSignature = this.skinTextBox_signature.SkinTxt.Text;
                string newName = this.skinTextBox_nickName.SkinTxt.Text ;
                if (newSignature != mine.Signature || this.orgID != mine.OrgID || newName != mine.Name)
                {
                    this.resourceCenter.ClientOutter.ChangeMyBaseInfo(newName, newSignature, this.orgID);
                }

                if (this.selfPhoto)
                {
                    this.headImageIndex = -1;
                }

                ResultHandler handler = new ResultHandler(this.UpdateCallback);
                bool headChanged = false;
                byte[] newHead = null;

                if (this.headImageIndex != mine.HeadImageIndex)
                {
                    headChanged = true;
                    if (this.headImageIndex < 0)
                    {
                        newHead = ESBasic.Helpers.ImageHelper.Convert(this.pnlImgTx.BackgroundImage);
                    }
                }
                else
                {
                    if (this.headImageIndex < 0)
                    {
                        newHead = ESBasic.Helpers.ImageHelper.Convert(this.pnlImgTx.BackgroundImage);
                        headChanged = newHead.Length != mine.HeadImageData.Length;
                        if (!headChanged)
                        {
                            for (int i = 0; i < newHead.Length; i++)
                            {
                                if (newHead[i] != mine.HeadImageData[i])
                                {
                                    headChanged = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (headChanged)
                {

                    Image imgEcllips = ESBasic.Helpers.ImageHelper.CutEllipseImage(this.pnlImgTx.BackgroundImage, FunctionOptions.GetHeadImageSize());
                    newHead = ESBasic.Helpers.ImageHelper.Convert(imgEcllips);
                    this.resourceCenter.ClientOutter.ChangeMyHeadImage(this.headImageIndex, newHead, handler);
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("修改成功！");
                    this.Close();
                }
            }
            catch (Exception ee)
            {
                this.Cursor = Cursors.Default;
                this.toolTip1.Show("修改失败！" + ee.Message, this.btnRegister, new Point(this.btnRegister.Width / 2, -this.btnRegister.Height), 3000);
            }
        }

        //0923
        private void UpdateCallback(bool acked, object tag)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<bool, object>(this.UpdateCallback), acked, tag);
            }
            else {
                this.Cursor = Cursors.Default;
                if (acked)
                {
                    MessageBox.Show("修改成功！");
                    this.Close();
                }
                else
                {
                    this.toolTip1.Show("提交超时，修改失败！", this.btnRegister, new Point(this.btnRegister.Width / 2, -this.btnRegister.Height), 3000);
                }
            }
        }



        private string orgID = null;
        private void skinButton2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.None;
        }

        private void skinPictureBox_updateImage_Click(object sender, EventArgs e)
        {
            this.headImageIndex = (++this.headImageIndex) % GlobalResourceManager.HeadImages.Length;
            this.pnlImgTx.BackgroundImage = GlobalResourceManager.HeadImages[this.headImageIndex];
            this.selfPhoto = false;
        }
        private bool selfPhoto = false;

        private void skinPictureBox_uploadImage_Click(object sender, EventArgs e)
        {
            //HeadImageForm form = new HeadImageForm();
            //if (form.ShowDialog() == DialogResult.OK)
            //{
            //    this.pnlImgTx.BackgroundImage = form.CurrentImage;
            //    this.selfPhoto = true;
            //}
            string file = ESBasic.Helpers.FileHelper.GetFileToOpen("请选择要使用的图片");
            if (file == null)
            {
                return;
            }

            Image img = Image.FromFile(file);
            this.pnlImgTx.BackgroundImage = img;
            this.selfPhoto = true;
        }
    }
}
