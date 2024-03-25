using ESBasic;
using ESFramework.Boost.Controls;
using ESPlus.Serialization;
using GGTalk.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TalkBase;
using TalkBase.Client;
using TalkBase.Client.Bridges;

namespace GGTalk.FlatControls
{
    public partial class GroupChatPanel : FlatBasePanel, IGroupChatForm
    {
        private GGGroup currentGroup;
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        public event CbGeneric<IGroup> DeleteGroupClicked;
        public event CbGeneric<string> ExitGroupClicked;
        public event CbGeneric<string> GroupMemberClicked;
        private bool isExistAllGroupBan, isCreator;
        public GroupChatPanel()
        {
            InitializeComponent();
        }

        public GroupChatPanel(ResourceCenter<GGUser, GGGroup> center, string groupID)
        {
            this.resourceCenter = center;
            this.currentGroup = this.resourceCenter.ClientGlobalCache.GetGroup(groupID);

            InitializeComponent();
            this.chatPanel1.SetHistoryBox_SenderShowType(SenderShowType.HeadAndName);
            this.chatPanel1.Initialize(this.resourceCenter, this.currentGroup);
            this.title = this.currentGroup.Name;
            this.isCreator = this.resourceCenter.CurrentUserID == this.currentGroup.CreatorID;
            this.isExistAllGroupBan = this.resourceCenter.ClientOutter.ExistAllGroupBan(groupID);
        }


        #region IFlatControl
        private string title = string.Empty;
        public override string ControlTitle => this.title;

        public override void ClickMore()
        {
            GroupInfoForm form = new GroupInfoForm(this.resourceCenter, this.currentGroup);
            form.ShowDialog();
        }
        #endregion

        #region IChatForm 成员     
        public void UnitCommentNameChanged(IUnit unit)
        {
            if (unit.IsUser)
            {
                this.FriendInfoChanged((GGUser)unit);
            }
        }

        public string UnitID
        {
            get { return this.currentGroup.GroupID; }
        }

        public void MyselfOffline()
        {

        }

        public void RefreshUI()
        {
            this.Refresh();
            this.chatPanel1.FocusOnInputBox();
        }

        #region FlashChatWindow
        private DateTime lastFlashTime = DateTime.Now;
        public void FlashChatWindow()
        {
            if (SystemSettings.Singleton.CombineChatbox)
            {
                return;
            }

            TimeSpan span = DateTime.Now - this.lastFlashTime;
            if (span.TotalSeconds > 1)
            {
                this.lastFlashTime = DateTime.Now;
  
            }
        }
        #endregion

        public void FriendInfoChanged(IUser user)
        {

        }

        public void FriendStateChanged(string userID, UserStatus newStatus)
        {

        }

        public void MyInfoChanged(IUser my)
        {
            this.FriendInfoChanged(my);
        }
        #endregion

        #region IGroupChatForm 成员
        public void HandleGroupChatMessage(string broadcasterID, byte[] info, string tag)
        {
            ChatBoxContent content = CompactPropertySerializer.Default.Deserialize<ChatBoxContent>(info, 0);
            this.chatPanel1.HandleChatMessage(broadcasterID, content, null, tag);
            //this.FlashChatWindow();
        }

        public void HandleOfflineGroupChatMessage(string broadcasterID, byte[] info, DateTime msgOccureTime, string tag)
        {
            ChatBoxContent content = CompactPropertySerializer.Default.Deserialize<ChatBoxContent>(info, 0);
            this.chatPanel1.HandleChatMessage(broadcasterID, content, msgOccureTime, tag);
            //this.FlashChatWindow();
        }

        public void HandleGroupFileUploadedNotify(string sourceUserID, string groupID, string fileName)
        {
            //if (this.nDiskConnected)
            //{
            //    if (sourceUserID != this.resourceCenter.CurrentUserID)
            //    {
            //        this.nDiskBrowser1.RefreshDirectory();
            //    }
            //}

            this.chatPanel1.AppendSysMessage(string.Format("{0} 上传了群文件‘{1}’！", this.resourceCenter.ClientGlobalCache.GetUserName(sourceUserID), fileName));
        }

        public void OnGroupChanged(GroupChangedType type, string operatorID, string userID)
        {
            if (type == GroupChangedType.MemberInfoChanged)
            {
                if (string.IsNullOrEmpty(userID))
                {
                    return;
                }

                GGUser user = this.resourceCenter.ClientGlobalCache.GetUser(userID);
                //this.FriendInfoChanged(user);
                return;
            }

            if (type == GroupChangedType.SomeoneJoin)
            {
                this.DoJoinGroup(userID, true);
                return;
            }

            if (type == GroupChangedType.SomeoneQuit)
            {
                this.DoQuitGroup(userID, true);
                return;
            }

            if (type == GroupChangedType.GroupInfoChanged)
            {
                this.title = this.currentGroup.Name ;// string.Format("{0}({1})", this.currentGroup.Name, this.currentGroup.GroupID);
                return;
            }

            if (type == GroupChangedType.MyselfBeRemovedFromGroup)
            {
                MessageBox.Show("您已被移除该讨论组！");
                this.Close();
                return;
            }

            if (type == GroupChangedType.GroupDeleted)
            {
                MessageBox.Show("该讨论组已经被解散！");
                this.Close();
                return;
            }

            if (type == GroupChangedType.SomeoneDeleted)
            {
                this.DoQuitGroup(userID, false);
                return;
            }

            if (type == GroupChangedType.OtherBeRemovedFromGroup)
            {
                this.DoQuitGroup(userID, false);
                return;
            }

            if (type == GroupChangedType.OtherBePulledIntoGroup)
            {
                this.DoJoinGroup(userID, false);
                return;
            }
        }

        private void DoJoinGroup(string userID, bool active)
        {
            GGUser user = this.resourceCenter.ClientGlobalCache.GetUser(userID);
            string msg = active ? string.Format("{0}({1})加入了该讨论组！", user.Name, user.UserID) : string.Format("{0}({1})被邀请加入了该讨论组！", user.Name, user.UserID);
            this.chatPanel1.AppendSysMessage(msg);
        }

        private void DoQuitGroup(string userID, bool active)
        {
            GGUser user = this.resourceCenter.ClientGlobalCache.GetUser(userID);
            //ChatListSubItem[] items = this.chatListBox1.GetSubItemsByNicName(userID);
            //if (items == null || items.Length == 0)
            //{
            //    return;
            //}
            //ChatListSubItem item = items[0];
            //this.chatListBox1.Items[0].SubItems.Remove(item);
            //this.chatListBox1.Invalidate();

            string msg = active ? string.Format("{0}({1})退出了该讨论组！", user.Name, user.UserID) : string.Format("{0}({1})被移除了该讨论组！", user.Name, user.UserID);
            this.chatPanel1.AppendSysMessage(msg);
        }

        public void HandleBePulledIntoGroupNotify(string operatorUserID)
        {
            GGUser user = this.resourceCenter.ClientGlobalCache.GetUser(operatorUserID);
            string msg = string.Format("您被 {0} 邀请加入该讨论组 ...", user.DisplayName);
            this.chatPanel1.AppendSysMessage(msg);
        }

        public void HandleGroupBanNotify(string operatorID, double minutes)
        {
            //this.SetCustomizedImage(this.chatListSubItem_myself, true);
            //if (this.isExistAllGroupBan || this.isCreator)
            //{
            //    return;
            //}
            //this.chatPanel1.SetGroupBan(operatorID, minutes);
        }

        public void HandleRemoveGroupBanNotify()
        {
            //this.SetCustomizedImage(this.chatListSubItem_myself, false);
            //if (this.isExistAllGroupBan)
            //{
            //    return;
            //}
            //this.chatPanel1.RemoveGroupBan();
        }

        public void HandleAllGroupBan()
        {
            //if (this.isCreator)
            //{
            //    return;
            //}
            //this.chatPanel1.SetAllGroupBan();
            //this.isExistAllGroupBan = true;
            //this.SetAllGroupBanButtonImg();
        }

        public void HandleRemoveAllGroupBan()
        {
            //if (this.isCreator)
            //{
            //    return;
            //}
            //this.chatPanel1.RemoveAllGroupBan();
            ////解除全局禁言后，检查当前账号是否存在禁言
            //this.resourceCenter.ClientOutter.CheckGroupBan4CurrentUser(this.currentGroup.ID);
            //this.isExistAllGroupBan = false;
            //this.SetAllGroupBanButtonImg();
        }
        #endregion

        public void OnCreateGroupSucceed()
        {
            this.chatPanel1.AppendSysMessage("您已经成功创建讨论组...");
        }

        public void OnJoinGroupSucceed()
        {
            string msg = "您已经成功加入讨论组，可以开始聊天了...";
            this.chatPanel1.AppendSysMessage(msg);
        }

    }
}
