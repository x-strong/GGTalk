using CCWin;
using ESBasic;
using OMCS.Passive;
using OMCS.Passive.RemoteDesktop;
using System;
using System.Windows.Forms;

namespace OMCS.Boost.Forms
{
    /// <summary>
    /// 远程桌面/远程协助 窗口。
    /// </summary>
    public partial class RemoteDeskForm : CCSkinMain
    {
        private DynamicDesktopConnector dynamicDesktopConnector1 = new DynamicDesktopConnector();
        private string owner = null;
        private bool isRemoteControl = false;
        private RemoteHelpStyle remoteDesktopStyle = RemoteHelpStyle.PartScreen;
        public event CbGeneric<bool ,RemoteHelpStyle ,bool> RemoteHelpEnded; //参数：true - 协助方终止；false - 请求方终止
        public event CbGeneric RemoteControlRequestCancelled;

        private string ownerName = "";
        public RemoteDeskForm(string ownerID, string nickName ,RemoteHelpStyle style ,bool remoteControl)
        {
            InitializeComponent();
            this.RemoteHelpEnded += delegate { };

            this.isRemoteControl = remoteControl;
            this.owner = ownerID;
            this.ownerName = nickName;
            this.remoteDesktopStyle = style;

            this.dynamicDesktopConnector1.WatchingOnly = false; //可以操控桌面
            this.dynamicDesktopConnector1.ConnectEnded += new CbGeneric<string ,ConnectResult>(desktopConnector1_ConnectEnded);
            this.dynamicDesktopConnector1.Disconnected += new CbGeneric<string, ConnectorDisconnectedType>(desktopConnector1_Disconnected);

            if (!remoteControl)
            {
                this.Text = string.Format("远程协助{0} - {1}", this.remoteDesktopStyle == RemoteHelpStyle.PartScreen ? "（指定屏幕区域）" : "", this.ownerName);    
                this.Cursor = Cursors.WaitCursor;
                this.skinLabel_tip.Visible = false;
                this.dynamicDesktopConnector1.BeginConnect(ownerID);
            }
            else
            {
                this.Text = string.Format("远程控制 - {0}", this.ownerName);    
                this.skinLabel_msg.Visible = false;
            }
        }

        public void OnResponseOfRemoteControl(bool agree)
        {
            this.skinLabel_tip.Visible = false;
            if (agree)
            {
                this.Cursor = Cursors.WaitCursor;
                this.skinLabel_tip.Visible = false;
                this.skinLabel_msg.Visible = true;
                this.dynamicDesktopConnector1.BeginConnect(this.owner);
            }
            else
            {
                this.ownerCancel = true;
                this.Close();
            }
        }

        void desktopConnector1_Disconnected(string ownerID ,ConnectorDisconnectedType disconnectedType)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<string ,ConnectorDisconnectedType>(this.desktopConnector1_Disconnected), ownerID, disconnectedType);
            }
            else
            {
                if (disconnectedType == ConnectorDisconnectedType.GuestActiveDisconnect)
                {
                    return;
                }

                MessageBoxEx.Show(string.Format("到{0}的桌面连接断开。原因：{1}", this.ownerName, disconnectedType));
                this.Close();
            }
        }

        void desktopConnector1_ConnectEnded(string ownerID, ConnectResult res)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<string, ConnectResult>(this.desktopConnector1_ConnectEnded), ownerID, res);
            }
            else
            {
                this.Cursor = Cursors.Default;
                if (res == ConnectResult.Succeed)
                {
                    this.dynamicDesktopConnector1.SetViewer(this.desktopPanel1);
                    this.skinPanel_pic.Visible = false;
                    this.skinLabel_msg.Visible = false;
                    this.skinComboBox_quality.Visible = true;
                    this.skinLabel_quality.Visible = true;

                    int quality = this.dynamicDesktopConnector1.GetVideoQuality();
                    int index = (quality - 1) / 7;
                    if (index < 0)
                    {
                        index = 0;
                    }
                    if (index > 3)
                    {
                        index = 3;
                    }
                    this.skinComboBox_quality.SelectedIndex = index;

                    return;
                }

                MessageBoxEx.Show(string.Format("连接{0}的桌面失败。原因：{1}", this.ownerName, res));
                this.Close();
            }
        }

        private bool ownerCancel = false;
        public void OwnerTeminateHelp()
        {
            this.ownerCancel = true;
            this.Close();
        }

        private void RemoteHelpForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.dynamicDesktopConnector1.Connected)
            {
                if (this.isRemoteControl && !this.ownerCancel)
                {
                    if (this.RemoteControlRequestCancelled != null)
                    {
                        this.RemoteControlRequestCancelled();
                    }
                }
  
                return;
            }

            if (!this.ownerCancel )
            {
                if (!ESBasic.Helpers.WindowsHelper.ShowQuery("您确定要关闭远程桌面窗口吗？"))
                {
                    return;
                }
            }

            this.dynamicDesktopConnector1.Disconnect();
            this.RemoteHelpEnded(this.ownerCancel ,this.remoteDesktopStyle ,this.isRemoteControl);
        }

        private void skinComboBox_quality_SelectedIndexChanged(object sender, EventArgs e)
        {
            int quality = this.skinComboBox_quality.SelectedIndex * 7 + 1;
            this.dynamicDesktopConnector1.ChangeOwnerDesktopEncodeQuality(quality);
        }       
    }    
}
