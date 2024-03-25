using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESBasic;
using System.Net;
using ESFramework.Engine.Udp.Reliable;

namespace ESFramework.Boost.Controls
{
    /// <summary>
    /// 用于监控当前客户端建立的所有UDP Session的状态。
    /// </summary>
    public partial class ReliableUdpSessionStateViewer : UserControl
    {
        private volatile bool closed = false;
        private IUdpSessionStateViewer udpSessionStateViewer;        
        private int refreshSpanInSecs = 1;

        public ReliableUdpSessionStateViewer()
        {
            InitializeComponent();
        }

        public void Initialize(IUdpSessionStateViewer viewer, int _refreshSpanInSecs)
        {
            if (viewer == null)
            {
                return;
            }

            this.udpSessionStateViewer = viewer;
            this.RefreshState();

            CbGeneric cb = new CbGeneric(this.ShowState);
            cb.BeginInvoke(null, null);
        }

       
        private void ShowState()
        {
            while (!this.closed)
            {
                this.RefreshState();
                System.Threading.Thread.Sleep(this.refreshSpanInSecs * 1000);
            }
        }

        private void RefreshState()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CbGeneric(this.RefreshState), null);
            }
            else
            {
                if (this.checkBox_In.Checked)
                {
                    this.dataGridView1.DataSource = this.udpSessionStateViewer.GetInUdpSessionStates();
                }
                else
                {
                    this.dataGridView1.DataSource = this.udpSessionStateViewer.GetOutUdpSessionStates();
                }
                this.dataGridView1.Refresh();
            }
        }

        public void Stop()
        {
            this.closed = true;
        }

        private void checkBox_In_CheckedChanged(object sender, EventArgs e)
        {

        }
        
    }
}
