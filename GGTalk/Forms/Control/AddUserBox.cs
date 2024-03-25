using System;
using System.Windows.Forms;
using CCWin;
using ESPlus.Rapid;
using ESBasic.Security;
using ESPlus.Serialization;
using TalkBase;


namespace GGTalk
{
    public partial class AddUserBox : UserControl
    {
        IRapidPassiveEngine engine;
        public AddUserBox()
        {
            InitializeComponent();            
        }

        public void Initialize(IRapidPassiveEngine engine)
        {
            this.engine = engine;
        }

        private void skinButton_submit_Click(object sender, EventArgs e)
        {
            string id = this.skinTextBox_id.SkinTxt.Text.Trim();
            if (string.IsNullOrEmpty(id))
            {
                MessageBoxEx.Show("账号不能为空！");
                return;
            }

            string name = this.skinTextBox_name.SkinTxt.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBoxEx.Show("名称不能为空！");
                return;
            }
            string phone = this.skinTextBox_phone.SkinTxt.Text.Trim();
            RegisterUserContract registerUserContract = new RegisterUserContract(id, name, SecurityHelper.MD5String2("123456"), phone);
            byte[] info = this.engine.CustomizeOutter.Query(BusinessInfoTypes.Register4Admin, CompactPropertySerializer.Default.Serialize<RegisterUserContract>(registerUserContract));
            RegisterResult result = (RegisterResult)BitConverter.ToInt32(info, 0);
            switch (result)
            {
                case RegisterResult.PhoneExisted:
                    MessageBoxEx.Show("手机号码已存在！");
                    return;
                case RegisterResult.Existed:
                    MessageBoxEx.Show("账号已存在！");
                    return;
                case RegisterResult.Error:
                    MessageBoxEx.Show("内部错误！");
                    return;
                default:
                    break;
            }
            MessageBoxEx.Show("新增成功！");
            this.skinTextBox_id.SkinTxt.Text = "";
            this.skinTextBox_name.SkinTxt.Text = "";
            this.skinTextBox_phone.SkinTxt.Text = "";
        }
    }
}
