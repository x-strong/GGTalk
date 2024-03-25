using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TalkBase;
using TalkBase.Client;
using ESBasic.ObjectManagement.Managers;

namespace GGTalk.Forms
{
    public partial class NotifyListPanel : UserControl
    {
        private NotifyType notifyType ;
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        //状态为请求中的 控件集合  key：requesterID+"-"+accepterID 或者 requesterID+"-"+groupID
        private ObjectManager<string, NotifyItemPanel> requestPanel4RequestingDic = new ObjectManager<string, NotifyItemPanel>();
        private const char H_Line_Char = '-';

        public NotifyListPanel(ResourceCenter<GGUser, GGGroup> center, NotifyType notifyType)
        {
            InitializeComponent();
            this.resourceCenter = center;
            this.notifyType = notifyType;
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.HorizontalScroll.Visible = false;
            this.flowLayoutPanel1.HorizontalScroll.Enabled = false;

            this.flowLayoutPanel1.ControlAdded += FlowLayoutPanel1_ControlAdded;
            this.flowLayoutPanel1.ControlRemoved += FlowLayoutPanel1_ControlRemoved;

        }

        private bool currentHasVerticalScroll = false;
        private void FlowLayoutPanel1_ControlRemoved(object sender, ControlEventArgs e)
        {
            this.CheckVerticalScroll_SetChildPanelWidth();
        }

        private void FlowLayoutPanel1_ControlAdded(object sender, ControlEventArgs e)
        {
            this.CheckVerticalScroll_SetChildPanelWidth();
        }

        /// <summary>
        /// 检查垂直滚动条并设置子控件width
        /// </summary>
        private void CheckVerticalScroll_SetChildPanelWidth()
        {
            bool hasVerticalScroll= IsVerticalScrollBarVisible();
            if (this.currentHasVerticalScroll != hasVerticalScroll)
            {
                this.currentHasVerticalScroll = hasVerticalScroll;
                foreach (System.Windows.Forms.Control item in this.flowLayoutPanel1.Controls)
                {
                    item.Width = hasVerticalScroll ? item.Width - 25 : item.Width +25;
                }
            }
        }

        /// <summary>
        /// 是否出现垂直滚动条
        /// </summary>
        /// <returns></returns>
        private bool IsVerticalScrollBarVisible()
        {
            if (this.flowLayoutPanel1.Controls.Count == 0) return false;
            if (this.flowLayoutPanel1.Controls[this.flowLayoutPanel1.Controls.Count-1].Bottom >= this.flowLayoutPanel1.Height)
                return true;
            return false;
        }

        private int itemPanelWidth = 0;
        private volatile bool firstAddItem = true;

        public void AddNotifyItem(AddFriendRequest addFriendRequest, bool isNewAdd = false)
        {
            if (this.notifyType == NotifyType.Group)
            {
                return;
            }
            NotifyItemPanel panel = new NotifyItemPanel();
            if ((RequsetType)addFriendRequest.State == RequsetType.Request)
            {
                this.RemoveNotifyItem4Requesting(addFriendRequest.RequesterID);
                this.requestPanel4RequestingDic.Add(addFriendRequest.RequesterID + H_Line_Char + addFriendRequest.AccepterID, panel);
            }
            panel.Initialize(this.resourceCenter, addFriendRequest);            
            this.AddItemPanel(panel, isNewAdd);

        }

        public void AddNotifyItem(AddGroupRequest addGroupRequest, bool isNewAdd = false)
        {
            if (this.notifyType == NotifyType.User)
            {
                return;
            }
            
            NotifyItemPanel panel = new NotifyItemPanel();
            if ((RequsetType)addGroupRequest.State == RequsetType.Request)
            {
                this.RemoveNotifyItem4Requesting(addGroupRequest.RequesterID, addGroupRequest.GroupID);
                string key = addGroupRequest.RequesterID + H_Line_Char + addGroupRequest.GroupID;
                panel.ID = key;
                this.requestPanel4RequestingDic.Add(key, panel);
            }
            panel.Initialize(this.resourceCenter, addGroupRequest);

            this.AddItemPanel(panel, isNewAdd);

        }

        private void AddItemPanel(NotifyItemPanel panel, bool isNewAdd)
        {
            panel.Dock = DockStyle.Top;
            if (this.firstAddItem)
            {
                this.itemPanelWidth = panel.Width;
                this.firstAddItem = false;
            }
            panel.OnHandleRequestClicked += Panel_OnHandleRequestClicked;
            if (this.currentHasVerticalScroll)
            {
                panel.Width -= 25;
            }
            //panel.Width = 415;
            this.flowLayoutPanel1.Controls.Add(panel);
            if (isNewAdd)
            {
                this.flowLayoutPanel1.Controls.SetChildIndex(panel, 0);
            }
        }

        private void Panel_OnHandleRequestClicked(string key,bool isAgree)
        {
            foreach (string item in this.requestPanel4RequestingDic.GetKeyList())
            {
                if (item == key)
                {
                    this.requestPanel4RequestingDic.Remove(key);
                    return;
                }
            }
        }


        /// <summary>
        /// 获取指定用户 在请求状态中的 控件
        /// </summary>
        /// <param name="userID">指定用户ID</param>
        /// <param name="isRequester">是否为请求者</param>
        /// <returns></returns>
        public NotifyItemPanel GetNotifyItemPanel4Requesting(string userID,bool isRequester)
        {
            string currentUserID = this.resourceCenter.CurrentUserID;
            List<string> keys = this.requestPanel4RequestingDic.GetKeyList();
            foreach (string key in keys)
            {
                string[] strs = key.Split(H_Line_Char);
                if (isRequester && userID == strs[0] && currentUserID == strs[1])
                {
                    return this.requestPanel4RequestingDic.Get(key);
                }
                if (isRequester)
                {
                    continue;
                }
                if(currentUserID == strs[0] && userID == strs[1])
                {
                    return this.requestPanel4RequestingDic.Get(key);
                }
            }
            return null;
        }

        public NotifyItemPanel GetNotifyItemPanel4Requesting(string requesterID,string groupID)
        {
            List<string> keys = this.requestPanel4RequestingDic.GetKeyList();
            foreach (string key in keys)
            {
                string[] strs = key.Split(H_Line_Char);
                if (requesterID == strs[0] && groupID == strs[1])
                {
                    return this.requestPanel4RequestingDic.Get(key);
                }
            }
            return null;
        }

        private void RemoveNotifyItem4Requesting(string requesterID)
        {
            NotifyItemPanel panel = this.GetNotifyItemPanel4Requesting(requesterID, true);
            if (panel != null)
            {
                this.flowLayoutPanel1.Controls.Remove(panel);
            }
        }

        private void RemoveNotifyItem4Requesting(string requesterID, string groupID)
        {
            NotifyItemPanel panel = this.GetNotifyItemPanel4Requesting(requesterID, groupID);
            if (panel != null)
            {
                this.flowLayoutPanel1.Controls.Remove(panel);
            }
        }


    }
}
