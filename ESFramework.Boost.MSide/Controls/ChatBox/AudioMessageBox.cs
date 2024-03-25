using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESBasic;

namespace ESFramework.Boost.Controls
{
    public partial class AudioMessageBox : UserControl
    {
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

        public AudioMessageBox()
        {
            InitializeComponent();

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.CacheText |
                ControlStyles.ResizeRedraw, true);

            SetStyle(ControlStyles.Opaque, false);            
        }

        public void Initialize(string msgID, object audioMsg ,int spanInsecs)
        {
            this.audioMessageID = msgID;
            this.audioMessage = audioMsg;
            this.label1.Text = string.Format("{0}'",spanInsecs);
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
            this.pictureBox1.Image = this.imageList1.Images[0];
            this.imageIndex = 0;
            this.Invalidate(this.pictureBox1.Bounds);
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            ++imageIndex;
            this.pictureBox1.Image = this.imageList1.Images[imageIndex%this.imageList1.Images.Count];           
            this.Invalidate(this.pictureBox1.Bounds);
        }   
    }
}
