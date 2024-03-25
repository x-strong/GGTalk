using ESFramework.Boost.NetworkDisk.Passive;
using ESPlus.Application.FileTransfering.Passive;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GGTalk
{
    public partial class NDiskForm : BaseForm
    {
        public NDiskForm(IFileOutter fileOutter, INDiskOutter diskOutter ,string curUserID)
        {
            InitializeComponent();
            this.TopMost = false;
            this.ShowInTaskbar = true;
            this.nDiskBrowser1.Initialize(null, fileOutter, diskOutter, curUserID);            
        }
    }
}
