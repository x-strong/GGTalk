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
    /// <summary>
    /// 远程磁盘 主人控制面板
    /// </summary>
    public partial class RemoteDiskHandlePanel : UserControl
    {
        /// <summary>
        /// 中止远程磁盘
        /// </summary>
        public event CbGeneric RemoteDiskTerminated;

        public RemoteDiskHandlePanel()
        {
            InitializeComponent();

            this.timerLabel1.Visible = false;           
        }      

        public bool IsWorking
        {
            get
            {
                return this.timerLabel1.IsWorking;
            }
        }

        private void skinButtomReject_Click(object sender, EventArgs e)
        {
            if (this.RemoteDiskTerminated != null)
            {
                this.RemoteDiskTerminated();
            }

            this.OnTerminate();
        }

        public void OnAgree()
        {            
            this.timerLabel1.Visible = true;                   
            this.timerLabel1.Start();
            this.timerLabel1.Location = new Point(this.Width / 2 - this.timerLabel1.Width / 2, this.timerLabel1.Location.Y);
        }       
        
        public void OnTerminate()
        {             
            this.timerLabel1.Visible = false;
            this.timerLabel1.Stop();
            this.timerLabel1.Reset();                    
        }
    }
}
