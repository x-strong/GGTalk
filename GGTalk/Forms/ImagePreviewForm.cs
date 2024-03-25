using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace GGTalk.Forms
{
    public partial class ImagePreviewForm : Form
    {

        public ImagePreviewForm(Image img)
        {
            InitializeComponent();
            this.pictureBox1.BackgroundImage = img;
        }
 
        internal Image image;

        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);
            // 获取屏幕的宽度和高度
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            // 计算窗口的位置坐标
            int formWidth = this.Width;
            int formHeight = this.Height;
            int left = (screenWidth - formWidth) / 2;
            int top = (screenHeight - formHeight) / 2;

            // 设置窗口的位置
            this.Left = left;
            this.Top = top;
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            this.DialogResult = DialogResult.Cancel;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.image = (Image)this.pictureBox1.BackgroundImage.Clone();
            this.pictureBox1.BackgroundImage.Dispose();
            this.pictureBox1.Dispose();
            this.DialogResult = DialogResult.OK;
        }
    }
}
