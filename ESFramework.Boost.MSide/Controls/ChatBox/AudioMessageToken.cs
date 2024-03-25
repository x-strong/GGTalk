using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ESFramework.Boost.Controls
{
    public partial class AudioMessageToken : Control
    {
        private int spanInsecs = 0 ;
        private Image curImage = null;
        private Rectangle currentImageRectangle = new Rectangle(0, 0, 26, 26);
        private int imageIndex = 0;
        private string audioMessageID;
        public string AudioMessageID
        {
            get { return audioMessageID; }
        }

        private object audioMessage;
        public object AudioMessage
        {
            get { return audioMessage; }
        }

        #region 构造函数

        public AudioMessageToken()
            : base()
        {
            this.InitializeComponent();

            //SetStyle(
            //    ControlStyles.UserPaint |
            //    ControlStyles.AllPaintingInWmPaint |
            //    ControlStyles.OptimizedDoubleBuffer |
            //    ControlStyles.SupportsTransparentBackColor |
            //    ControlStyles.CacheText |
            //    ControlStyles.ResizeRedraw, true);

            //SetStyle(ControlStyles.Opaque, false);

            this.Cursor = Cursors.Arrow;

            this.curImage = this.imageList1.Images[0];
        }

        #endregion

        public void Initialize(string msgID, object audioMsg, int secs)
        {
            this.audioMessageID = msgID;
            this.audioMessage = audioMsg;
            this.spanInsecs = secs;
        }

        private bool isPlaying = false;
        public bool IsPlaying
        {
            get { return isPlaying; }
        }

        public void Start()
        {
            if (this.isPlaying)
            {
                return;
            }

            this.isPlaying = true;
            this.timer1.Start();            
        }

        public void Stop()
        {
            if (!this.isPlaying)
            {
                return;
            }

            this.isPlaying = false;
            this.timer1.Stop();
            this.curImage = this.imageList1.Images[0];
            this.imageIndex = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ++imageIndex;
            this.curImage = this.imageList1.Images[imageIndex % this.imageList1.Images.Count];           
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            if (this.curImage != null)
            {
                e.Graphics.DrawImage(this.curImage, this.currentImageRectangle, 0, 0, this.currentImageRectangle.Width, this.currentImageRectangle.Height, GraphicsUnit.Pixel);
                e.Graphics.DrawString(string.Format("{0}'" ,this.spanInsecs) ,new Font("微软雅黑",9) ,new SolidBrush(Color.Black),30,0) ;
            }

            ControlPaint.DrawBorder(
                   e.Graphics,
                   ClientRectangle,
                   Color.Red,
                   ButtonBorderStyle.Solid);
        }
    }
}
