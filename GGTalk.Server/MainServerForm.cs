﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESFramework.Server.UserManagement;
using ESBasic;
using ESFramework;
using ESPlus.Core;
using ESPlus.Rapid;
using System.Diagnostics;
using ESBasic.ObjectManagement.Managers;
using OMCS.Server;

namespace GGTalk.Server
{
    /// <summary>
    /// 内置的TCP服务器主窗体。可显示在线用户相关数据、连接数、线程数等信息。
    /// 仅仅用于开发调试阶段或Demo演示。MainServerForm会实时刷新每个在线用户的实时状态，其对CPU资源的消耗是不可忽视的，所以在正式上线的系统中，请不要使用MainServerForm。
    /// </summary>
    public partial class MainServerForm : Form, ILoginDeviceDisplayer
    {       
        private IRapidServerEngine rapidServerEngine;
        private IMultimediaServer multimediaServer;

        private bool toExit = false;
        private System.Threading.Timer timerMonitor;        
        public event CbGeneric CustomFunctionActivated;        

        public void SetIcon(Icon icon)
        {
            this.Icon = icon;
            this.notifyIcon1.Icon = icon;
        }

        public ToolStripMenuItem CustomizeMenu
        {
            get
            {
                return this.自定义功能ToolStripMenuItem;
            }
        }

        #region Ctor     

        public MainServerForm(IRapidServerEngine engine, IMultimediaServer server, bool showUserList)
        {
            InitializeComponent();
            this.CustomFunctionActivated += delegate { };
            this.TextChanged += new EventHandler(MainServerForm_TextChanged);
            
            this.rapidServerEngine = engine;     
            this.multimediaServer = server;
            this.ShowInformation(null);                     
            this.toolStripStatusLabel_state.Text = string.Format("启动时间：{0}",DateTime.Now);         ;
            this.toolStripStatusLabel_procotol.Text = string.Format("监听TCP端口：{0}", this.rapidServerEngine.Port);

            this.listView1.Visible = showUserList;
            if (showUserList)
            {
                this.rapidServerEngine.UserManager.LoginDeviceDisplayer = this;

                //在界面显示之前，可能已有客户端重连上来
                foreach (UserData data in this.rapidServerEngine.UserManager.GetAllUserData())
                {
                    foreach (LoginDeviceData device in data.GetDevices())
                    {
                        this.AddLoginDevice(device.LoginID, device.ClientType, device.Address.ToString());
                    }
                }
            }

            System.Threading.ThreadPool.SetMaxThreads(100, 10); //IOCP 线程数推荐为：（总核数 * 2 + 2） 
            this.timerMonitor = new System.Threading.Timer(new System.Threading.TimerCallback(this.ShowInformation), null, 1000, 1000);
        }

        void MainServerForm_TextChanged(object sender, EventArgs e)
        {
            this.notifyIcon1.Text = this.Text;
        }      

        #region ShowInformation
        private void ShowInformation(object state)
        {
            try
            {
                if (this.Disposing) { return; }
                if (this.InvokeRequired)
                {
                    this.Invoke(new CbGeneric<object>(this.ShowInformation), state);
                }
                else
                {
                    int count1, count2;
                    System.Threading.ThreadPool.GetAvailableThreads(out count1, out count2);
                    this.toolStripStatusLabel_threads.Text = string.Format("线程池:{0} ,IOCP线程:{1}", count1, count2);
                    int connCount = this.rapidServerEngine.ConnectionCount;
                    int deviceCount = this.rapidServerEngine.UserManager.GetAllLoginDevice().Count;
                    if (connCount < this.rapidServerEngine.MaxConnectionCount)
                    {
                        if (this.rapidServerEngine.MaxConnectionCount < 1000000)
                        {
                            this.toolStripStatusLabel_userCount.Text = string.Format("连接数：{0}-{1}/{2}", connCount, deviceCount, this.rapidServerEngine.MaxConnectionCount);
                        }
                        else
                        {
                            this.toolStripStatusLabel_userCount.Text = string.Format("连接数：{0}-{1}/无限制", connCount, deviceCount);
                        }
                        this.toolStripStatusLabel_userCount.ForeColor = Color.Black;
                    }
                    else
                    {
                        this.toolStripStatusLabel_userCount.Text = string.Format("连接数：{0}-{1}/{2}（已满）", connCount, deviceCount, this.rapidServerEngine.MaxConnectionCount);
                        this.toolStripStatusLabel_userCount.ForeColor = Color.Red;
                    }
                }
            }
            catch { }
        }       
        #endregion             
        #endregion         

        #region EventHandler     
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = !this.Visible;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.toExit)
            {
                this.Visible = false;
                e.Cancel = true;
            }
        }

        private void 自定义功能ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CustomFunctionActivated();
        }                  


        private void tuiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.GotoExit();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.GotoExit();
        }

        private void GotoExit()
        {
            if (ESBasic.Helpers.WindowsHelper.ShowQuery("您确定要退出服务器吗？"))
            {
                this.timerMonitor.Dispose();
                this.rapidServerEngine.Close();     
                this.multimediaServer.Close();
                this.toExit = true;
                System.Threading.Thread.Sleep(500);
                this.Close();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();
            List<UserInfo> list = this.onlineManager.GetAll() ;
            list.Sort(new Comparison<UserInfo>(delegate(UserInfo a, UserInfo b) {return (int)((a.LogonTime - b.LogonTime).TotalSeconds) ;}));
            foreach (UserInfo info in list)
            {
                string[] subItems = { info.LoginID, info.ClientType.ToString(), info.Address, info.LogonTime.ToString() };
                ListViewItem item = new ListViewItem(subItems);
                info.ListViewItem = item;
                item.Tag = info;
                this.listView1.Items.Add(item);
            }            
        }     
        #endregion

        private void 版本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ESFramework \n\n武汉傲瑞科技有限公司 www.oraycn.com\nCopyright © 2011 Oraycn. All Rights Reserved ");
        }

        private void 官网ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.oraycn.com");
        }

        #region ILoginDeviceDisplayer 成员
        private ObjectManager<string, UserInfo> onlineManager = new ObjectManager<string, UserInfo>();
        public void ClearAll()
        {
            if(this.InvokeRequired)
			{
				this.Invoke(new CbSimple(this.ClearAll),null);
			}
			else
			{
				this.listView1.Items.Clear();
			}
        }

        public void AddLoginDevice(string loginID, ClientType clientType, string userAddress)
        {
            if (this.onlineManager.Contains(loginID))
            {
                return;
            }
            string userID = MSideHelper.GetUserID(loginID);
            UserInfo info = new UserInfo(userID, loginID, clientType, userAddress);
            this.onlineManager.Add(loginID, info);
            this.AddItem(info);
        }

        public void RemoveLoginDevice(string loginID, string cause)
        {
            UserInfo info = this.onlineManager.Get(loginID);
            if (info != null)
            {
                this.RemoveItem(info.ListViewItem);
                this.onlineManager.Remove(loginID);
            }
        }

        public void OnMessageSent(string userID, int messageType)
        {           
            UserInfo info = this.onlineManager.Get(userID);
            if (info != null)
            {
                info.TotalDownloadCount += 1;
                info.LastDownloadTime = DateTime.Now;
            }
        }

        public void OnMessageReceived(string userID, int messageType)
        {
            UserInfo info = this.onlineManager.Get(userID);
            if (info != null)
            {
                info.TotalUploadCount += 1;
                info.LastUploadTime = DateTime.Now;
            }
        }

        private void RemoveItem(ListViewItem item)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CbGeneric<ListViewItem>(this.RemoveItem), item);
            }
            else
            {
                this.listView1.Items.Remove(item);
            }
        }

        private void AddItem(UserInfo info)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CbGeneric<UserInfo>(this.AddItem), info);
            }
            else
            {
                string[] subItems = { info.UserID, info.LoginID, info.ClientType.ToString(), info.Address, info.LogonTime.ToString() };
                ListViewItem item = new ListViewItem(subItems);
                info.ListViewItem = item;
                item.Tag = info;
                this.listView1.Items.Add(item);
            }
        }
        #endregion

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                ListViewHitTestInfo info = this.listView1.HitTest(e.Location);
                if (info == null || info.Item == null)
                {
                    return;
                }

                UserInfo user = (UserInfo)info.Item.Tag;

                MessageBox.Show(user.ToString());
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }


        
    }

    class UserInfo
    {
        public UserInfo(string userID, string loginID, ClientType type, string addr)
        {
            this.UserID = userID;
            this.LoginID = loginID;
            this.ClientType = type;
            this.Address = addr;
            this.LogonTime = DateTime.Now;
            this.TotalDownloadCount = 0;
            this.TotalUploadCount = 0;
        }

        public string UserID { get; set; }
        public string LoginID { get; set; }
        public string Address { get; set; }
        public DateTime LogonTime { get; set; }
        public ClientType ClientType { get; set; }
        public DateTime LastUploadTime { get; set; }
        public DateTime LastDownloadTime { get; set; }
        public int TotalUploadCount { get; set; }
        public int TotalDownloadCount { get; set; }
        public ListViewItem ListViewItem { get; set; }

        public override string ToString()
        {
            return string.Format("{0} \n上传次数:{1,-8} 最后一次上传:{2}\n下载次数:{3,-8} 最后一次下载:{4}\n", this.LoginID, this.TotalUploadCount.ToString("00000000"), this.LastUploadTime, this.TotalDownloadCount.ToString("00000000"), this.LastDownloadTime);
        }
    }
}