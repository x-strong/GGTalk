using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CCWin;
using CCWin.Win32;
using CCWin.Win32.Const;
using System.Diagnostics;
using ESBasic;
using ESFramework.Boost.NetworkDisk.Passive;
using ESPlus.Application.FileTransfering.Passive;

namespace ESFramework.Boost.Controls
{
    /// <summary>
    /// 远程磁盘 窗口。
    /// </summary>
    public partial class RemoteDiskForm : CCSkinMain
    {
        private string owner = null;        
        private bool ownerCancel = false;
        private INDiskOutter diskOutter;
        private IFileOutter fileOutter;
        private string currentUserID;

        public event CbGeneric<bool> RemoteDiskEnded; 
        public event CbGeneric RemoteDiskRequestCancelled;


        private string ownerName = "";
        public RemoteDiskForm(string ownerID, string nickName ,INDiskOutter disk, IFileOutter file, string curUserID)
        {
            InitializeComponent();            
            this.RemoteDiskEnded += delegate { };

            this.nDiskBrowser1.Visible = false;

            this.owner = ownerID;
            this.ownerName = nickName;
            this.diskOutter = disk;
            this.fileOutter = file;
            this.currentUserID = curUserID;

            this.Text = string.Format("远程磁盘 - {0}", this.ownerName);
            this.skinLabel_msg.Visible = false;
        }

        public void OnResponseOfRemoteControl(bool agree)
        {
            this.skinLabel_tip.Visible = false;
            if (agree)
            {
                this.Cursor = Cursors.WaitCursor;
                this.skinLabel_tip.Visible = false;
                this.skinLabel_msg.Visible = true;
                this.nDiskBrowser1.Visible = true;
                this.nDiskBrowser1.Initialize(this.owner, this.fileOutter, this.diskOutter ,this.currentUserID);
                this.Cursor = Cursors.Default;
            }
            else
            {
                this.ownerCancel = true;
                this.Close();
            }
        }

        public void OwnerTeminateHelp()
        {
            this.ownerCancel = true;
            this.Close();
        }


        private void RemoteHelpForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.nDiskBrowser1.Initialized)
            {
                if (!this.ownerCancel)
                {
                    if (this.RemoteDiskRequestCancelled != null)
                    {
                        this.RemoteDiskRequestCancelled();
                    }
                }
  
                return;
            }

            if (!this.ownerCancel )
            {
                if (!ESBasic.Helpers.WindowsHelper.ShowQuery("您确定要关闭远程磁盘窗口吗？"))
                {
                    return;
                }
            }
            
            this.RemoteDiskEnded(this.ownerCancel);
        }   
    }    
}
