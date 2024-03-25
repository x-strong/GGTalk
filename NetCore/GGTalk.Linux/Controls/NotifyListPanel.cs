using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Shapes;
using CPF.Styling;
using ESBasic.ObjectManagement.Managers;
using GGTalk.Linux;
using System;
using System.Collections.Generic;
using System.Text;
using TalkBase;

namespace GGTalk.Linux.Controls
{
    [CPF.Design.DesignerLoadStyle("res://GGTalk.Linux.Controls/Stylesheet1.css")]//用于设计的时候加载样式
    internal class NotifyListPanel : NetWorkListBoxTemplate
    {
        private NotifyType notifyType = NotifyType.User;
        //状态为请求中的 控件集合  key：requesterID+"-"+accepterID 或者 requesterID+"-"+groupID
        private ObjectManager<string, NotifyItemPanel> requestPanel4RequestingDic = new ObjectManager<string, NotifyItemPanel>();
        private const char H_Line_Char = '-';

        public NotifyListPanel(NotifyType _notifyType)
        {
            this.notifyType = _notifyType;
            Background = "#fff";
            this.Initialized += NotifyListPanel_Initialized;
        }

        private ScrollViewer scrollViewer;
        private void NotifyListPanel_Initialized(object sender, EventArgs e)
        {
            if (this.GetChildren().Count > 0)
            {
                this.Children.RemoveAt(0);
                //if (this.Children[0] is ScrollViewer)
                //{
                //    scrollViewer = this.Children[0] as ScrollViewer;
                //}
            }
        }

        public void AddNotifyItem(AddFriendRequest addFriendRequest, bool isNewAdd = false)
        {
            if (this.notifyType == NotifyType.Group)
            {
                return;
            }
            NotifyItemPanel panel = new NotifyItemPanel();
            panel.Initialize(addFriendRequest);
            this.AddItemPanel(panel, isNewAdd);

            if ((RequsetType)addFriendRequest.State == RequsetType.Request)
            {
                this.requestPanel4RequestingDic.Add(addFriendRequest.RequesterID + H_Line_Char + addFriendRequest.AccepterID, panel);
            }
        }

        public void AddNotifyItem(AddGroupRequest addGroupRequest, bool isNewAdd = false)
        {
            if (this.notifyType == NotifyType.User)
            {
                return;
            }
            NotifyItemPanel panel = new NotifyItemPanel();
            panel.Initialize(addGroupRequest);
            this.AddItemPanel(panel, isNewAdd);
            if ((RequsetType)addGroupRequest.State == RequsetType.Request)
            {
                this.requestPanel4RequestingDic.Add(addGroupRequest.RequesterID + H_Line_Char + addGroupRequest.GroupID, panel);
            }
        }
        private List<NotifyItemPanel> controls = new List<NotifyItemPanel>();
        private void AddItemPanel(NotifyItemPanel panel, bool isNewAdd)
        {
            if (isNewAdd)
            {
                this.controls.Insert(0, panel);
                //this.scrollViewer.Content = this.controls;
                this.Children.Insert(0, panel);
                //this.Items.Insert(0, panel);
            }
            else
            {
                this.controls.Add(panel);
                //this.scrollViewer.Content = this.controls;
                this.Children.Add(panel);
                //this.Items.Add(panel);
            }
            this.SetMarginTop();
        }




        /// <summary>
        /// 获取指定用户 在请求状态中的 控件
        /// </summary>
        /// <param name="userID">指定用户ID</param>
        /// <param name="isRequester">是否为请求者</param>
        /// <returns></returns>
        public NotifyItemPanel GetNotifyItemPanel4Requesting(string userID, bool isRequester)
        {
            string currentUserID = Program.ResourceCenter.CurrentUserID;
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
                if (currentUserID == strs[0] && userID == strs[1])
                {
                    return this.requestPanel4RequestingDic.Get(key);
                }
            }
            return null;
        }

        public NotifyItemPanel GetNotifyItemPanel4Requesting(string requesterID, string groupID)
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

        public void RemoveNotifyItem4Requesting(string requesterID)
        {
            NotifyItemPanel panel = this.GetNotifyItemPanel4Requesting(requesterID, true);
            if (panel != null)
            {
                //this.Items.Remove(panel);
                this.controls.Remove(panel);
                //this.scrollViewer.Content = this.controls;
                this.Children.Remove(panel);
                //this.Items = controls;
            }
            this.SetMarginTop();
        }

        private void SetMarginTop()
        {
            for (int i = 0; i < this.Children.Count; i++)
            {
                this.Children[i].MarginTop = (i - 1) * 134;
            }
        

        }

        public void RemoveNotifyItem4Requesting(string requesterID, string groupID)
        {
            NotifyItemPanel panel = this.GetNotifyItemPanel4Requesting(requesterID, groupID);
            if (panel != null)
            {
                //this.Items.Remove(panel);
                this.controls.Remove(panel);
                //this.scrollViewer.Content = this.controls;
                this.Children.Remove(panel);
                //this.Items = controls;
            }
        }
    }
}
