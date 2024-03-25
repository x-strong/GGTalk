using CCWin;
using ESBasic.ObjectManagement.Managers;
using GGTalk.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TalkBase;
using TalkBase.Client;
using TalkBase.Client.Bridges;

namespace GGTalk
{
    public partial class NotifyForm: BaseForm,INotifyForm
    {
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        private int totalEntityCount = 0;
        private NotifyListPanel notifyListPanel_User, notifyListPanel_Group;

        public NotifyForm(ResourceCenter<GGUser, GGGroup> center)
        {
            InitializeComponent();
            this.resourceCenter = center;
            
            this.notifyListPanel_User = new NotifyListPanel(this.resourceCenter, NotifyType.User);
            this.notifyListPanel_Group = new NotifyListPanel(this.resourceCenter, NotifyType.Group);
            this.tabPage1.Controls.Add(this.notifyListPanel_User);
            this.tabPage2.Controls.Add(this.notifyListPanel_Group);
            this.notifyListPanel_User.Dock = DockStyle.Fill;
            this.notifyListPanel_Group.Dock = DockStyle.Fill;
            this.resourceCenter.ClientOutter.GetAddFriendPage();
            this.resourceCenter.ClientOutter.GetAddGroupPage();
        }

        public void ShowTabPage(NotifyType notifyType)
        {
            if (notifyType == NotifyType.User)
            {
                this.skinTabControl1.SelectedIndex = 0;                
            }
            else if (notifyType == NotifyType.Group)
            {
                this.skinTabControl1.SelectedIndex = 1;                
            }
        }

        #region INotifyForm
        public void OnAddFriendRequestReceived(string requesterID, string comment)
        {
            //若未处理的申请记录已存在过滤掉新的请求。先从容器中删除
            //this.notifyListPanel_User.RemoveNotifyItem4Requesting(requesterID);
            AddFriendRequest addFriendRequest = new AddFriendRequest()
            {
                RequesterID = requesterID,
                RequesterCatalogName = "",
                AccepterID = this.resourceCenter.CurrentUserID,
                AccepterCatalogName = "",
                Comment2 = comment,
                State = (byte)RequsetType.Request,
                Notified = true,
                CreateTime = DateTime.Now
            };
            this.notifyListPanel_User.AddNotifyItem(addFriendRequest, true);
        }

        public void OnHandleAddFriendRequestReceived(string friendID, bool isRequester, bool IsAgreed)
        {
            NotifyItemPanel handledRequestPanel = this.notifyListPanel_User.GetNotifyItemPanel4Requesting(friendID, isRequester);
            if (handledRequestPanel == null)
            {
                return;
            }
            handledRequestPanel.OnRequestStateChanged(IsAgreed);
        }

        public void OnAddFriendRequestPageReceived(AddFriendRequestPage page)
        {
            if (page == null)
            {
                return;
            }
            this.totalEntityCount = page.TotalEntityCount;
            foreach (AddFriendRequest addFriendRequest in page.AddFriendRequestList)
            {
                this.notifyListPanel_User.AddNotifyItem(addFriendRequest);
            }
        }      

        
        public void OnAddGroupRequestReceived(string requesterID, string groupID, string comment)
        {
            //若未处理的申请记录已存在过滤掉新的请求。先从容器中删除
            //this.notifyListPanel_Group.RemoveNotifyItem4Requesting(requesterID,groupID);
            AddGroupRequest addGroupRequest = new AddGroupRequest()
            {
                RequesterID = requesterID,
                GroupID = groupID,
                AccepterID = this.resourceCenter.CurrentUserID,
                Comment2 = comment,
                State = (byte)RequsetType.Request,
                Notified = true,
                CreateTime = DateTime.Now
            };
            this.notifyListPanel_Group.AddNotifyItem(addGroupRequest, true);
        }

        public void OnHandleAddGroupRequestReceived(string requesterID, string groupID, bool IsAgreed)
        {
            NotifyItemPanel handledRequestPanel = this.notifyListPanel_Group.GetNotifyItemPanel4Requesting(requesterID, groupID);
            if (handledRequestPanel == null)
            {
                return;
            }
            handledRequestPanel.OnRequestStateChanged(IsAgreed);
        }

        public void OnAddGroupRequestPageReceived(AddGroupRequestPage page)
        {
            if (page == null)
            {
                return;
            }
            this.totalEntityCount = page.TotalEntityCount;
            foreach (AddGroupRequest addFriendRequest in page.AddGroupRequestList)
            {
                this.notifyListPanel_Group.AddNotifyItem(addFriendRequest);
            }
        }
        #endregion

   



  

    }
}
