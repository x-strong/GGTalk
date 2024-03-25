using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Shapes;
using CPF.Styling;
using ESBasic.ObjectManagement.Managers;
using GGTalk.Linux;
using GGTalk.Linux.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using TalkBase;

namespace GGTalk.Linux.Controls
{
    [CPF.Design.DesignerLoadStyle("res://GGTalk.Linux.Controls/Stylesheet1.css")]//用于设计的时候加载样式
    public class NotifyListPanel2 : ListBox
    {
        private NotifyType notifyType = NotifyType.User;
        //状态为请求中的 控件集合  key：requesterID+"-"+accepterID 或者 requesterID+"-"+groupID
        private ObjectManager<string, AddFriendOrGroupRequestModel> requestPanel4RequestingDic = new ObjectManager<string, AddFriendOrGroupRequestModel>();
        private const char H_Line_Char = '-';

        private Collection<AddFriendOrGroupRequestModel> models = new Collection<AddFriendOrGroupRequestModel>();

        public NotifyListPanel2(NotifyType _notifyType)
        {
            this.notifyType = _notifyType;
            Background = "#fff";
            //this.DataContext = models;
        }

        private volatile bool isInitialized = false;
        protected override void InitializeComponent()
        {
            if (this.isInitialized) { return; }
            this.isInitialized = true;

            WrapPanel panel = new WrapPanel();

            panel.Name = "itemsPanel";
            panel.PresenterFor = this;
            
            panel.Width = "100%";
            if (IsVirtualizing)
            {
                Children.Add(new ScrollViewer { Width = "100%", Height = "100%", Content =  new VirtualizationPresenter<ListBoxItem> { Child = panel, PresenterFor = this } , });
            }
            else
            {
                Children.Add(new ScrollViewer { Width = "100%", Height = "100%", Content =  panel });
            }

            Size = SizeField.Fill;
            ItemTemplate = typeof(NotifyItemPanel2);
            this.Bindings.Add(nameof(ListBox.Items), (nameof(NotifyListViewModel.Models)));
            notifyListViewModel.Models = this.models;
            this.DataContext = this.notifyListViewModel;
        }

        private NotifyListViewModel notifyListViewModel = new NotifyListViewModel();
        public void AddNotifyItem(AddFriendRequest addFriendRequest, bool isNewAdd = false)
        {
            if (this.notifyType == NotifyType.Group)
            {
                return;
            }
            AddFriendOrGroupRequestModel model = new AddFriendOrGroupRequestModel(addFriendRequest);
            if ((RequsetType)addFriendRequest.State == RequsetType.Request)
            {
                this.RemoveNotifyItem4Requesting(addFriendRequest.RequesterID);
                this.requestPanel4RequestingDic.Add(addFriendRequest.RequesterID + H_Line_Char + addFriendRequest.AccepterID, model);
            }
            this.AddItemPanel(model, isNewAdd);


        }

        public void AddNotifyItem(AddGroupRequest addGroupRequest, bool isNewAdd = false)
        {
            if (this.notifyType == NotifyType.User)
            {
                return;
            }
            AddFriendOrGroupRequestModel model = new AddFriendOrGroupRequestModel(addGroupRequest);
            if ((RequsetType)addGroupRequest.State == RequsetType.Request)
            {
                this.RemoveNotifyItem4Requesting(addGroupRequest.RequesterID, addGroupRequest.GroupID);
                this.requestPanel4RequestingDic.Add(addGroupRequest.RequesterID + H_Line_Char + addGroupRequest.GroupID, model);
            }
            this.AddItemPanel(model, isNewAdd);

        }

        private void AddItemPanel(AddFriendOrGroupRequestModel model, bool isNewAdd)
        {
            if (isNewAdd)
            {
                this.models.Insert(0,model);
            }
            else
            {
                this.models.Add(model);
            }
            this.models.Sort((x, y) => { return -x.CreateTime.CompareTo(y.CreateTime); });
        }


        /// <summary>
        /// 获取指定用户 在请求状态中的 控件
        /// </summary>
        /// <param name="userID">指定用户ID</param>
        /// <param name="isRequester">是否为请求者</param>
        /// <returns></returns>
        internal AddFriendOrGroupRequestModel GetNotifyItemPanel4Requesting(string userID, bool isRequester)
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

        internal AddFriendOrGroupRequestModel GetNotifyItemPanel4Requesting(string requesterID, string groupID)
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
            AddFriendOrGroupRequestModel model = this.GetNotifyItemPanel4Requesting(requesterID, true);
            if (model != null && model.State == (int)RequsetType.Request)
            {
                this.models.Remove(model);
            }
        }

        private void RemoveNotifyItem4Requesting(string requesterID, string groupID)
        {
            AddFriendOrGroupRequestModel model = this.GetNotifyItemPanel4Requesting(requesterID, groupID);
            if (model != null && model.State == (int)RequsetType.Request)
            {
                this.models.Remove(model);
            }
        }

        public void OnHandleAddFriendRequestReceived(string friendID, bool isRequester, bool IsAgreed)
        {
            AddFriendOrGroupRequestModel model = this.GetNotifyItemPanel4Requesting(friendID, isRequester);
            if (model == null && model.State == (int)RequsetType.Request)
            {
                return;
            }
            NotifyItemPanel2 handledRequestPanel = this.GetNotifyItemPanel2(model);
            handledRequestPanel.OnRequestStateChanged(IsAgreed);
        }

        private NotifyItemPanel2 GetNotifyItemPanel2(AddFriendOrGroupRequestModel model)
        {
            if (model == null) { return null; }
            foreach (UIElement item in this.GetItemsPanel().Children)
            {
                if (item.DataContext == model)
                {
                    return item as NotifyItemPanel2;
                }
            }
            return null;
        }

        public void OnHandleAddGroupRequestReceived(string requesterID, string groupID, bool IsAgreed)
        {
            AddFriendOrGroupRequestModel model = this.GetNotifyItemPanel4Requesting(requesterID, groupID);
            if (model == null)
            {
                return;
            }
            NotifyItemPanel2 handledRequestPanel = this.GetNotifyItemPanel2(model);
            handledRequestPanel.OnRequestStateChanged(IsAgreed);
        }
    }
}
