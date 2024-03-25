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
    /// 远程磁盘 请求面板。
    /// </summary>
    public partial class RemoteDiskRequestPanel : UserControl
    {       
        /// <summary>
        /// 回复远程磁盘请求
        /// </summary>
        public event CbGeneric<bool> RemoteRequestAnswerd;

        public RemoteDiskRequestPanel()
        {
            InitializeComponent();
        }      

        private void skinButtomReject_Click(object sender, EventArgs e)
        {
            if (this.RemoteRequestAnswerd != null)
            {
                this.RemoteRequestAnswerd(false);
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (this.RemoteRequestAnswerd != null)
            {
                this.RemoteRequestAnswerd(true);
            }
        }        
    }
}
