using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CCWin;
using CCWin.SkinControl;
using TalkBase;
using TalkBase.Client;


namespace GGTalk
{
    /// <summary>
    /// 用户资料。
    /// </summary>
    public partial class UserInformationForm : BaseForm ,IUserInformationForm
    {
        private ResourceCenter<GGUser, GGGroup> resourceCenter;

        public UserInformationForm()
        {
            InitializeComponent();     
        }
        public UserInformationForm(Point startPosition, ResourceCenter<GGUser, GGGroup> center)
        {            
            InitializeComponent();         
            this.resourceCenter = center;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = startPosition;
        }

        public void SetUser(IUser user, Point location)
        {
            this.lblQm.Text = user.Signature;
            this.skinLabel_id.Text = user.ID;
            this.skinLabel_name.Text = user.Name;
            this.pnlImgTx.BackgroundImage = GlobalResourceManager.GetHeadImageOnline((GGUser)user);
            this.Location = location;
        }

        private void UserInformationForm_Load(object sender, EventArgs e)
        {
            
        }         

        //窗体重绘时
        private void FrmUserInformation_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SolidBrush sb = new SolidBrush(Color.FromArgb(100, 255, 255, 255));
            g.FillRectangle(sb, new Rectangle(new Point(1, Height - 103), new Size(Width - 2, 80)));
        }

        //计时器
        private bool flag = false;
        private void timShow_Tick(object sender, EventArgs e)
        {
            //鼠标不在窗体内时
            if (!this.Bounds.Contains(Cursor.Position) && flag)
            {
                this.Hide();
                flag = false;
            }
            else if (this.Bounds.Contains(Cursor.Position))
            {
                flag = true;
            }
        }

       
    }
}
