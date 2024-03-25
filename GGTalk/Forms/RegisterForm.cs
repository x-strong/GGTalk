using CCWin;
using ESPlus.Application.Basic;
using ESPlus.Rapid;
using System;
using System.Configuration;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TalkBase;

namespace GGTalk
{
    /// <summary>
    /// 注册。
    /// </summary>
    public partial class RegisterForm : BaseForm
    {
        private int headImageIndex = 0;
        private IGGService remotingService;
        private IRapidPassiveEngine rapidPassiveEngine;
        private bool selfPhoto = false;
        private int countdown = 90;

        public RegisterForm(IRapidPassiveEngine engine)
        {
            InitializeComponent();
            this.rapidPassiveEngine = engine;
            int registerPort = int.Parse(ConfigurationManager.AppSettings["RemotingPort"]);
            this.remotingService = (IGGService)Activator.GetObject(typeof(IGGService), string.Format("tcp://{0}:{1}/GGService", ConfigurationManager.AppSettings["ServerIP"], registerPort));
              
            Random ran = new Random();
            this.headImageIndex = ran.Next(0,GlobalResourceManager.HeadImages.Length);
            this.pnlImgTx.BackgroundImage = GlobalResourceManager.HeadImages[this.headImageIndex];//根据ID获取头像            
        }

        #region RegisteredUser
        private GGUser registeredUser = null;
        public GGUser RegisteredUser
        {
            get
            {
                return this.registeredUser;
            }
        } 
        #endregion       

        private void skinButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;            
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.None;
            string userID = this.skinTextBox_id.SkinTxt.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(userID))
            {
                this.skinTextBox_id.SkinTxt.Focus();
                MessageBoxEx.Show("帐号不能为空！");
                return;
            }

            string userName = this.skinTextBox_nickName.SkinTxt.Text.Trim();
            if (string.IsNullOrEmpty(userName))
            {
                MessageBoxEx.Show("姓名不能为空！");
                this.skinTextBox_nickName.SkinTxt.SelectAll();
                this.skinTextBox_nickName.SkinTxt.Focus();
                return;
            }
            string pwd = this.skinTextBox_pwd.SkinTxt.Text;
            if (string.IsNullOrEmpty(pwd))
            {
                MessageBoxEx.Show("密码不能为空！");
                this.skinTextBox_pwd.SkinTxt.SelectAll();
                this.skinTextBox_pwd.SkinTxt.Focus();
                return;
            }
            if (pwd != this.skinTextBox_pwd2.SkinTxt.Text)
            {
                MessageBoxEx.Show("两次输入的密码不一致！");
                this.skinTextBox_pwd.SkinTxt.SelectAll();
                this.skinTextBox_pwd.SkinTxt.Focus();
                return;
            }
            string phone = this.skinTextBox_phone.SkinTxt.Text.Trim();
            if (string.IsNullOrEmpty(phone))
            {
                MessageBoxEx.Show("手机号码不能为空！");
                this.skinTextBox_phone.SkinTxt.SelectAll();
                this.skinTextBox_phone.SkinTxt.Focus();
                return;
            }
            if (!CommonHelper.IsPhoneNumber(phone))
            {
                MessageBoxEx.Show("手机号码格式有误！");
                this.skinTextBox_phone.SkinTxt.SelectAll();
                this.skinTextBox_phone.SkinTxt.Focus();
                return;
            }
            if(this.depID == null)
            {
                this.depID = "";
            }

            try
            {
                GGUser user = new GGUser(userID, pwd, userName, "", this.depID, this.skinTextBox_signature.SkinTxt.Text, this.headImageIndex, "");
                user.Phone = phone;
                if (this.selfPhoto)
                {
                    user.HeadImageData = ESBasic.Helpers.ImageHelper.Convert(this.pnlImgTx.BackgroundImage);
                    user.HeadImageIndex = -1;
                }
                string smsCode = "0000";//验证码功能未开启默认0000
                //# Reg:name;pwd;phone;smsCode;#12(orgID);3(headIndex);加油！(signature)
                string token = FunctionOptions.RegistActionToken2 + String.Format("{0};{1};{2};{3};{4};{5};{6};{7}", user.Name, user.PasswordMD5, user.Phone, smsCode, user.OrgID, user.HeadImageIndex, user.Signature,user.UserID);
                LogonResponse response = this.rapidPassiveEngine.Initialize("regist", token, ConfigurationManager.AppSettings["ServerIP"], int.Parse(ConfigurationManager.AppSettings["ServerPort"]), null);

                switch (response.FailureCause)
                {
                    case "Existed":
                        MessageBoxEx.Show("用户帐号已经存在！");
                        return;
                    case "PhoneExisted":
                        MessageBoxEx.Show("电话号码已绑定过！");
                        return;
                    case "Error":
                        MessageBoxEx.Show("注册出现错误！");
                        return;
                    default:
                        if (response.FailureCause.StartsWith(RegisterResult.Succeed.ToString()))
                        {
                            user.UserID = response.FailureCause.Substring(RegisterResult.Succeed.ToString().Length);
                            break;
                        }
                        MessageBoxEx.Show(response.FailureCause);
                        return;
                }
                this.registeredUser = user;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ee)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                MessageBoxEx.Show("注册失败！" + ee.Message);
            }

        }


        private string depID = "#0";

        private void skinPictureBox_updateImage_Click(object sender, EventArgs e)
        {
            this.headImageIndex = (++this.headImageIndex) % GlobalResourceManager.HeadImages.Length;
            this.pnlImgTx.BackgroundImage = GlobalResourceManager.HeadImages[this.headImageIndex];
            this.selfPhoto = false;
        }


        private void skinPictureBox_uploadImage_Click(object sender, EventArgs e)
        {

            string file = ESBasic.Helpers.FileHelper.GetFileToOpen("请选择要使用的图片");
            if (file == null)
            {
                return;
            }

            Image img = Image.FromFile(file);
            this.pnlImgTx.BackgroundImage = img;
            this.selfPhoto = true;
        }

        private void skinTextBox_id_SkinTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是退格和数字，则屏蔽输入
            if (!(e.KeyChar == '\b' || (e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= 'A' && e.KeyChar <= 'Z')))
            {
                e.Handled = true;
            }
        }
    }
}
