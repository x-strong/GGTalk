using CCWin;
using System;
using TalkBase;
using TalkBase.Client;

namespace GGTalk
{
    public partial class ChangePhoneForm : BaseForm
    {

        public ChangePhoneForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.None;
            string newPhone = this.skinTextBox_phone.SkinTxt.Text.Trim();
            ChangePhoneResult result = ClientBusinessOutter.Instance.ChangeMyPhone(newPhone);// ClientBusinessOutter.Instance.ChangeMyPhone(this.smsCodeButton1.Phone);
            switch (result)
            {
                case ChangePhoneResult.PhoneExisted:
                    MessageBoxEx.Show("新号码已存在！");
                    break;
                default:
                    MessageBoxEx.Show("内部发生错误，操作失败！");
                    break;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void skinButton_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
