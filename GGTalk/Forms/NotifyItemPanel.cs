using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TalkBase;
using TalkBase.Client;
using TalkBase.Client.Bridges;

namespace GGTalk.Forms
{
    public partial class NotifyItemPanel : UserControl
    {
        private NotifyType notifyType = NotifyType.User;
        private ResourceCenter<GGUser, GGGroup> center;
        private bool isRequester = false;

        private string friendID = string.Empty;
        private string groupID = string.Empty;

        /// <summary>
        /// 处理申请请求按钮点击时触发  string: ID  / false：拒绝  true：同意
        /// </summary>
        public event Action<string,bool> OnHandleRequestClicked;

        public string ID { get; set; }


        public NotifyItemPanel()
        {
            InitializeComponent();
            this.OnHandleRequestClicked += delegate { };
        }

        public void Initialize(ResourceCenter<GGUser, GGGroup> center, AddFriendRequest addFriendRequest)
        {
            this.center = center;
            notifyType = NotifyType.User;
            this.isRequester = addFriendRequest.RequesterID == center.CurrentUserID;
            this.friendID = this.isRequester ? addFriendRequest.AccepterID : addFriendRequest.RequesterID;
            GGUser friend = this.center.ClientGlobalCache.GetUser(this.friendID);
            this.pictureBox1.Image = GlobalResourceManager.GetHeadImageOnline(friend);
            this.label_time.Text = addFriendRequest.CreateTime.GetDateTimeFormats('f')[0];
            this.label_UserName.Text = friend.DisplayName;
            this.label_comment.Text = isRequester ? "已发送验证请求" : addFriendRequest.Comment2;
            this.label_result.TextAlign = ContentAlignment.MiddleRight;
            this.InitResultShow(isRequester, (RequsetType)addFriendRequest.State);
        }

        public void Initialize(ResourceCenter<GGUser, GGGroup> center, AddGroupRequest addGroupRequest)
        {
            this.center = center;
            notifyType = NotifyType.Group;
            this.groupID = addGroupRequest.GroupID;
            this.isRequester = addGroupRequest.RequesterID == center.CurrentUserID;
            this.friendID = this.isRequester ? addGroupRequest.AccepterID : addGroupRequest.RequesterID;
            GGGroup group = this.center.ClientGlobalCache.GetGroup(addGroupRequest.GroupID);
            GGUser friend = this.center.ClientGlobalCache.GetUser(this.friendID);
            this.pictureBox1.Image = GlobalResourceManager.GetHeadImageOnline(friend);
            this.label_time.Text = addGroupRequest.CreateTime.GetDateTimeFormats('f')[0];
            this.label_UserName.Text = isRequester ? "已申请加入 " + group.Name : friend.DisplayName + "申请加入 " + group.Name;
            this.label_comment.Text = isRequester ? "已发送验证请求" : addGroupRequest.Comment2;
            this.label_result.TextAlign = ContentAlignment.MiddleRight;
            this.InitResultShow(isRequester, (RequsetType)addGroupRequest.State);
        }

        private void InitResultShow(bool isRequester, RequsetType requsetType)
        {
            if (!isRequester && requsetType == RequsetType.Request)
            {
                label_result.Visible = false;
            }
            else
            {
                btn_refuse.Visible = false;
                btn_agree.Visible = false;
                switch (requsetType)
                {
                    case RequsetType.Request:
                        this.label_result.Text = "等待验证";
                        break;
                    case RequsetType.Agree:
                        this.label_result.Text = "已同意";
                        break;
                    case RequsetType.Reject:
                        this.label_result.Text = "已拒绝";
                        break;
                }
            }
        }

        private void btn_refuse_Click(object sender, EventArgs e)
        {
            if (notifyType == NotifyType.User)
            {
                this.center.ClientOutter.HandleAddFriendRequest(friendID, "", false);                
            }
            else if(notifyType== NotifyType.Group)
            {
                this.center.ClientOutter.HandleAddGroupRequest(friendID, groupID, false);
            }
            this.OnRequestStateChanged(false);
        }

        private void btn_agree_Click(object sender, EventArgs e)
        {
            if (notifyType == NotifyType.User)
            {
                this.center.ClientOutter.HandleAddFriendRequest(friendID, FunctionOptions.DefaultFriendCatalog, true);
                //双方加为好友后，跳转到好友聊天框
                IFriendChatForm form = (IFriendChatForm)this.center.ChatFormController.GetForm(friendID);
                form.AddFriendSucceed();
                Program.ChatFormShower.ShowChatForm(form);
            }
            else if(notifyType==NotifyType.Group)
            {
                this.center.ClientOutter.HandleAddGroupRequest(friendID, groupID, true);
            }
            this.OnRequestStateChanged(true);
        }

        /// <summary>
        /// AddFriendRequest状态发生改变
        /// </summary>
        /// <param name="isAgree"></param>
        public void OnRequestStateChanged(bool isAgree)
        {
            if (isAgree)
            {
               // addFriendRequest.State = (byte)RequsetType.Agree;
                this.label_result.Text = "已同意";
            }
            else {
               // addFriendRequest.State = (byte)RequsetType.Reject;
                this.label_result.Text = "已拒绝";            
            }
            btn_refuse.Visible = false;
            btn_agree.Visible = false;
            label_result.Visible = true;
            this.OnHandleRequestClicked(this.ID,isAgree);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            GGUser friend = this.center.ClientGlobalCache.GetUser(this.friendID);
            if (friend == null)
            {
                return;
            }
            UserInfoForm form = new UserInfoForm(this.center, friend, true);
            form.Show();
        }
    }
}
