using CCWin;
using CCWin.SkinControl;
using ESBasic;
using ESBasic.Helpers;
using ESBasic.ObjectManagement.Managers;
using ESFramework.Boost.Controls;
using ESFramework.Boost.NetworkDisk.Passive;
using ESPlus.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TalkBase;
using TalkBase.Client;
using TalkBase.Client.Bridges;

namespace GGTalk
{
    /// <summary>
    /// 群组聊天窗口。
    /// </summary>
    public partial class GroupChatForm : BaseForm, IGroupChatForm
    {
        private GGGroup currentGroup;
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        public event CbGeneric<IGroup> DeleteGroupClicked;
        public event CbGeneric<string> ExitGroupClicked;
        public event CbGeneric<string> GroupMemberClicked;
        private ObjectManager<string, GroupBan> groupBanManager = new ObjectManager<string, GroupBan>();
        private bool isExistAllGroupBan, isCreator;
        private ChatListSubItem chatListSubItem_myself;


        #region Ctor
        public GroupChatForm()
        {
            InitializeComponent();
        }

        public GroupChatForm(ResourceCenter<GGUser, GGGroup> center, string groupID)
        {
            this.resourceCenter = center;
            this.currentGroup = this.resourceCenter.ClientGlobalCache.GetGroup(groupID);

            InitializeComponent();
            //this.tabPage_chat.BackgroundImage = GlobalResourceManager.MainBackImage;

            this.chatPanel1.Initialize(this.resourceCenter, this.currentGroup);
            this.linkLabel_softName.Text = GlobalResourceManager.SoftwareName;
            this.toolShow.SetToolTip(this.panelFriendHeadImage, string.Format("组ID：{0}", this.currentGroup.GroupID));

            string dateString = string.Format("{0}-{1}-{2}", this.currentGroup.CreateTime.Year.ToString("00"), this.currentGroup.CreateTime.Month.ToString("00"), this.currentGroup.CreateTime.Day.ToString("00"));
            this.Text = this.currentGroup.Name + "  " + dateString;// string.Format("{0}({1})", this.currentGroup.Name, this.currentGroup.GroupID);
            this.labelGroupName.Text = this.Text;
            this.label_announce.Text = this.currentGroup.Announce;
            if (this.currentGroup.IsPrivate || this.currentGroup.CreatorID != this.resourceCenter.CurrentUserID)//私密群、非群主不能编辑群成员 
            {
                this.skinButtom_editMembers.Visible = false;
            }

            this.isCreator = this.resourceCenter.CurrentUserID == this.currentGroup.CreatorID;
            this.InitGroupBanManager(groupID);
            foreach (string memberID in this.currentGroup.MemberList)
            {
                GGUser friend = this.resourceCenter.ClientGlobalCache.GetUser(memberID);
                this.AddUserItem(friend);
            }
            if (this.isCreator)
            {
                this.禁言_toolStripMenuItem.Visible = true;
                this.解除禁言_toolStripMenuItem.Visible = true;
                this.skinButton_allGroupBan.Visible = true;
            }
            this.chatListBox1.ListSubItemMenu = this.skinContextMenuStrip;
            this.isExistAllGroupBan = this.resourceCenter.ClientOutter.ExistAllGroupBan(groupID);
            this.SetAllGroupBanButtonImg();
            if (this.isExistAllGroupBan)
            {
                //创建者和管理员可以发言
                if (this.isCreator)
                {
                    return;
                }
                this.HandleAllGroupBan();
            }
            else
            {
                this.resourceCenter.ClientOutter.CheckGroupBan4CurrentUser(groupID);
            }

        }

        private void AddUserItem(GGUser friend)
        {
            ChatListSubItem subItem = new ChatListSubItem(friend.UserID, friend.UserID, friend.DisplayName, friend.Signature, GlobalResourceManager.ConvertUserStatus(friend.UserStatus), GlobalResourceManager.GetHeadImage(friend));
            subItem.Tag = friend;
            if (this.groupBanManager.Contains(friend.ID))
            {
                this.SetCustomizedImage(subItem, true);
            }
            if (friend.UserID == this.resourceCenter.CurrentUserID)
            {
                this.chatListSubItem_myself = subItem;
            }
            this.chatListBox1.Items[0].SubItems.AddAccordingToStatus(subItem);
        }
        #endregion


        #region 窗体事件
        public void UnitCommentNameChanged(IUnit unit)
        {
            if (unit.IsUser)
            {
                this.FriendInfoChanged((GGUser)unit);
            }
        }

        public void HandleBePulledIntoGroupNotify(string operatorUserID)
        {
            GGUser user = this.resourceCenter.ClientGlobalCache.GetUser(operatorUserID);
            string msg = string.Format("您被 {0} 邀请加入该讨论组 ...", user.DisplayName);
            this.chatPanel1.AppendSystemMsg(msg);
        }

        public void OnCreateGroupSucceed()
        {
            this.chatPanel1.AppendSystemMsg("您已经成功创建讨论组...");
        }

        public void OnJoinGroupSucceed()
        {
            string msg = "您已经成功加入讨论组，可以开始聊天了...";
            this.chatPanel1.AppendSystemMsg(msg);
        }


        //悬浮至好友Q名时
        private void labelFriendName_MouseEnter(object sender, EventArgs e)
        {
            this.labelGroupName.Font = new Font("微软雅黑", 14F, FontStyle.Underline);
        }

        //离开好友Q名时
        private void labelFriendName_MouseLeave(object sender, EventArgs e)
        {
            this.labelGroupName.Font = new Font("微软雅黑", 14F);
        }

        //渐变层
        private void FrmChat_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SolidBrush sb = new SolidBrush(Color.FromArgb(100, 255, 255, 255));
            g.FillRectangle(sb, new Rectangle(new Point(1, this.chatListBox1.Location.Y), new Size(Width - 2, Height - this.chatListBox1.Location.Y))); //91
        }

        //关闭
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        }

        private void chatListBox1_DoubleClickSubItem(object sender, ChatListEventArgs e)
        {
            if (this.currentGroup.IsPrivate)//私密群不能私聊加好友
            {
                return;
            }
            ChatListSubItem item = e.SelectSubItem;
            item.IsTwinkle = false;

            string friendID = item.ID;
            if (friendID == this.resourceCenter.CurrentUserID)
            {
                return;
            }
            //如果是黑名单用户，则显示聊天记录。
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.IsInBlackList(friendID))
            {
                Form form1 = this.resourceCenter.ChatFormController.GetChatRecordForm(friendID);
                if (form1 != null)
                {
                    form1.Show();
                }
                return;
            }

            if (this.GroupMemberClicked != null)
            {
                this.GroupMemberClicked(friendID);
            }
        }

        private void skinButtom1_Click(object sender, EventArgs e)
        {
            UserSelectedForm form = new UserSelectedForm();
            form.Initialize(this.resourceCenter.ClientGlobalCache, this.currentGroup);
            if (form.ShowDialog() == DialogResult.OK)
            {
                List<string> list = form.UserIDSelected;
                if (!list.Contains(this.currentGroup.CreatorID))
                {
                    list.Add(this.currentGroup.CreatorID);
                }
                this.resourceCenter.ClientOutter.ChangeGroupMembers(this.currentGroup.ID, list);
            }
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (this.currentGroup.CreatorID == this.resourceCenter.CurrentUserID)
            {
                if (this.DeleteGroupClicked != null)
                {
                    this.DeleteGroupClicked(this.currentGroup);
                }
            }
            else if (this.ExitGroupClicked != null)
            {
                this.ExitGroupClicked(this.currentGroup.ID);
            }
        }

        private void labelGroupName_Click(object sender, EventArgs e)
        {
            EditGroupInfoForm form = new EditGroupInfoForm(this.resourceCenter, this.currentGroup);
            form.ShowDialog();
        }

        private void panelFriendHeadImage_MouseClick(object sender, MouseEventArgs e)
        {
            //Clipboard.SetText(this.currentGroup.GroupID);
            //MessageBox.Show(string.Format("已经将组ID（{0}）复制到粘贴板！", this.currentGroup.GroupID));
            GroupInfoForm form = new GroupInfoForm(this.resourceCenter, this.currentGroup);
            form.Show();
        }

        //点击群视频聊天按钮
        private void btn_groupVideo_Click(object sender, EventArgs e)
        {
            //若已经存在 被请求或者 请求他人 群视频聊天的 过程中，则过滤掉
            if (this.resourceCenter.ChatFormController.ExistGroupVideoCallForm() || this.resourceCenter.ChatFormController.ExistGroupVideoChatForm())
            {
                return;
            }
            UserSelectedForm form = new UserSelectedForm();
            form.Text = "请选择视频聊天成员";
            form.Initialize4Group(this.resourceCenter.ClientGlobalCache, this.currentGroup, false);
            if (form.ShowDialog() == DialogResult.OK)
            {
                List<string> userIds = form.UserIDSelected;
                string currentUserID = this.resourceCenter.CurrentUserID;
                userIds.Remove(currentUserID);
                if (userIds.Count > 0)
                {
                    string videoGroupID = this.currentGroup.GroupID + "_" + currentUserID + DateTime.Now.Ticks;
                    userIds.Insert(0, currentUserID);//群成员加上自己
                    userIds = new List<string>(userIds.Distinct());
                    string userIDstr = StringHelper.ContactString(userIds, FunctionOptions.CommaSeparator.ToString());
                    string tag = videoGroupID + FunctionOptions.ColonSeparator + userIDstr;

                    foreach (string userid in userIds)
                    {
                        if (userid == currentUserID) { continue; }
                        this.resourceCenter.ClientOutter.SendMediaCommunicate(userid, CommunicateMediaType.GroupVideo, CommunicateType.Request, tag);
                    }
                    ///跳转到群视频界面
                    Form groupVideoChatForm = this.resourceCenter.ChatFormController.GetNewGroupVideoChatForm(videoGroupID);
                    groupVideoChatForm.Show();
                }
            }
        }

        #endregion

        #region IChatForm 成员      

        public string UnitID
        {
            get { return this.currentGroup.GroupID; }
        }

        public void MyselfOffline()
        {
            foreach (ChatListSubItem item in this.chatListBox1.Items[0].SubItems)
            {
                item.Status = ChatListSubItem.UserStatus.OffLine;
            }
            this.chatListBox1.Invalidate();
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
                ESBasic.Helpers.WindowsHelper.FlashWindow(this);
            }
        }
        #endregion

        public void FriendInfoChanged(IUser user)
        {
            ChatListSubItem[] items = this.chatListBox1.GetSubItemsByNicName(user.ID);
            if (items == null || items.Length == 0)
            {
                return;
            }

            items[0].HeadImage = GlobalResourceManager.GetHeadImage((GGUser)user);
            items[0].DisplayName = user.DisplayName;
            items[0].PersonalMsg = user.Signature;
            items[0].Tag = user;
            this.chatListBox1.Invalidate();
        }

        public void FriendStateChanged(string userID, UserStatus newStatus)
        {
            ChatListSubItem[] items = this.chatListBox1.GetSubItemsByNicName(userID);
            if (items == null || items.Length == 0)
            {
                return;
            }

            items[0].Status = GlobalResourceManager.ConvertUserStatus(newStatus);
            items[0].HeadImage = this.resourceCenter.UnitInfoProvider.GetHeadImage((IUser)items[0].Tag);
            this.chatListBox1.Invalidate();
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
            this.FlashChatWindow();
        }

        public void HandleOfflineGroupChatMessage(string broadcasterID, byte[] info, DateTime msgOccureTime, string tag)
        {
            ChatBoxContent content = CompactPropertySerializer.Default.Deserialize<ChatBoxContent>(info, 0);
            this.chatPanel1.HandleChatMessage(broadcasterID, content, msgOccureTime, tag);
            this.FlashChatWindow();
        }

        public void HandleGroupFileUploadedNotify(string sourceUserID, string groupID, string fileName)
        {
            if (this.nDiskConnected)
            {
                if (sourceUserID != this.resourceCenter.CurrentUserID)
                {
                    this.nDiskBrowser1.RefreshDirectory();
                }
            }

            this.chatPanel1.AppendSystemMsg(string.Format("{0} 上传了群文件‘{1}’！", this.resourceCenter.ClientGlobalCache.GetUserName(sourceUserID), fileName));
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
                this.FriendInfoChanged(user);
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
                string dateString = string.Format("{0}-{1}-{2}", this.currentGroup.CreateTime.Year.ToString("00"), this.currentGroup.CreateTime.Month.ToString("00"), this.currentGroup.CreateTime.Day.ToString("00"));
                this.Text = this.currentGroup.Name + "  " + dateString;// string.Format("{0}({1})", this.currentGroup.Name, this.currentGroup.GroupID);
                this.labelGroupName.Text = this.Text;
                this.label_announce.Text = this.currentGroup.Announce;
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
            this.AddUserItem(user);
            string msg = active ? string.Format("{0}({1})加入了该讨论组！", user.Name, user.UserID) : string.Format("{0}({1})被邀请加入了该讨论组！", user.Name, user.UserID);
            this.chatPanel1.AppendSystemMsg(msg);
        }

        private void DoQuitGroup(string userID, bool active)
        {
            GGUser user = this.resourceCenter.ClientGlobalCache.GetUser(userID);
            ChatListSubItem[] items = this.chatListBox1.GetSubItemsByNicName(userID);
            if (items == null || items.Length == 0)
            {
                return;
            }
            ChatListSubItem item = items[0];
            this.chatListBox1.Items[0].SubItems.Remove(item);
            this.chatListBox1.Invalidate();

            string msg = active ? string.Format("{0}({1})退出了该讨论组！", user.Name, user.UserID) : string.Format("{0}({1})被移除了该讨论组！", user.Name, user.UserID);
            this.chatPanel1.AppendSystemMsg(msg);
        }


        #endregion    

        #region 群共享
        private bool nDiskConnected = false;
        private void skinTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.skinTabControl1.SelectedTab == this.tabPage_file)
            {
                if (!this.nDiskConnected)
                {
                    NDiskOutter nDiskOutter = new NDiskOutter(this.resourceCenter.RapidPassiveEngine.FileOutter, this.resourceCenter.RapidPassiveEngine.CustomizeOutter);
                    this.nDiskBrowser1.NetDiskID = "GroupShare_" + this.currentGroup.GroupID.Substring(1);
                    this.nDiskBrowser1.UploadCompleted += new CbGeneric<string>(nDiskBrowser1_UploadCompleted);
                    this.nDiskBrowser1.Initialize(null, this.resourceCenter.RapidPassiveEngine.FileOutter, nDiskOutter, this.resourceCenter.CurrentUserID);
                    this.nDiskConnected = true;
                }
            }
        }

        void nDiskBrowser1_UploadCompleted(string fileName)
        {
            this.resourceCenter.ClientOutter.GroupFileUploaded(this.currentGroup.ID, fileName);
            this.chatPanel1.AppendSystemMsg(string.Format("您上传了群文件‘{0}’！", fileName));

        }
        #endregion

        #region 群禁言
        public void HandleAllGroupBan()
        {
            if (this.isCreator)
            {
                return;
            }
            this.chatPanel1.SetAllGroupBan();
            this.isExistAllGroupBan = true;
            this.SetAllGroupBanButtonImg();
        }

        public void HandleRemoveAllGroupBan()
        {
            if (this.isCreator)
            {
                return;
            }
            this.chatPanel1.RemoveAllGroupBan();
            //解除全局禁言后，检查当前账号是否存在禁言
            this.resourceCenter.ClientOutter.CheckGroupBan4CurrentUser(this.currentGroup.ID);
            this.isExistAllGroupBan = false;
            this.SetAllGroupBanButtonImg();
        }

        public void HandleGroupBanNotify(string operatorID, double minutes)
        {
            this.SetCustomizedImage(this.chatListSubItem_myself, true);
            if (this.isExistAllGroupBan || this.isCreator)
            {
                return;
            }
            this.chatPanel1.SetGroupBan(operatorID, minutes);

        }

        public void HandleRemoveGroupBanNotify()
        {
            this.SetCustomizedImage(this.chatListSubItem_myself, false);
            if (this.isExistAllGroupBan)
            {
                return;
            }
            this.chatPanel1.RemoveGroupBan();
        }

        private void InitGroupBanManager(string groupID)
        {
            List<GroupBan> groupBans = this.resourceCenter.ClientOutter.GetGroupBans4Group(groupID);
            foreach (GroupBan groupBan in groupBans)
            {
                this.groupBanManager.Add(groupBan.UserID, groupBan);
            }
        }

        /// <summary>
        /// 设置群成员禁言图标
        /// </summary>
        /// <param name="subItem"></param>
        /// <param name="isBan"></param>
        private void SetCustomizedImage(ChatListSubItem subItem, bool isBan)
        {
            subItem.CustomizedImage = isBan ? GGTalk.Properties.Resources.AV_Refuse : null;
        }

        private void toolStripMenuItem_oneDay_Click(object sender, EventArgs e)
        {
            this.SetGroupBan4User(60 * 24);
        }

        private void toolStripMenuItem_tenDays_Click(object sender, EventArgs e)
        {
            this.SetGroupBan4User(10 * 60 * 24);
        }

        private void toolStripMenuItem_forever_Click(object sender, EventArgs e)
        {
            this.SetGroupBan4User(100000 * 60 * 24);
        }

        private void SetGroupBan4User(double minutes)
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }
            GGUser unit = (GGUser)this.chatListBox1.SelectSubItem.Tag;
            GroupBan groupBan = new GroupBan()
            {
                GroupID = this.currentGroup.ID,
                UserID = unit.ID,
                OperatorID = this.resourceCenter.CurrentUserID,
                Comment2 = string.Format("禁言{0}分钟", minutes),
                CreateTime = DateTime.Now,
                EnableTime = DateTime.Now.AddMinutes(minutes)
            };
            this.resourceCenter.ClientOutter.SetGroupBan4User(this.currentGroup.ID, unit.ID, string.Format("禁言{0}分钟", minutes), minutes);
            this.groupBanManager.Add(unit.ID, groupBan);
            this.SetCustomizedImage(this.chatListBox1.SelectSubItem, true);
            string msg = string.Format("{0}({1})被你禁言{2}", unit.DisplayName, unit.ID, TimeHelper.GetDurationStr(new TimeSpan(0, (int)minutes, 0)));
            this.chatPanel1.AppendSystemMsg(msg);

        }

        private void 解除禁言_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }
            GGUser unit = (GGUser)this.chatListBox1.SelectSubItem.Tag;
            this.resourceCenter.ClientOutter.RemoveGroupBan4User(this.currentGroup.ID, unit.ID);
            this.groupBanManager.Remove(unit.ID);
            this.SetCustomizedImage(this.chatListBox1.SelectSubItem, false);
            string msg = string.Format("{0}({1})被你解除禁言", unit.DisplayName, unit.ID);
            this.chatPanel1.AppendSystemMsg(msg);

        }

        private void chatListBox1_BeforeListSubItemMenuShow(object sender, ChatListSubItem e)
        {
            GGUser unit = (GGUser)this.chatListBox1.SelectSubItem.Tag;
            if (unit.ID == this.resourceCenter.CurrentUserID)
            {
                this.禁言_toolStripMenuItem.Visible = false;
                this.解除禁言_toolStripMenuItem.Visible = false;
                this.添加好友ToolStripMenuItem.Visible = false;
                this.TAToolStripMenuItem.Visible = false;
                return;
            }
            else
            {
                this.添加好友ToolStripMenuItem.Visible = true;
                this.TAToolStripMenuItem.Visible = true;
            }
            if (!this.isCreator)
            {
                return;
            }
            if (this.groupBanManager.Contains(unit.ID))
            {
                this.禁言_toolStripMenuItem.Visible = false;
                this.解除禁言_toolStripMenuItem.Visible = true;
            }
            else
            {
                this.禁言_toolStripMenuItem.Visible = true;
                this.解除禁言_toolStripMenuItem.Visible = false;
            }
        }

        private void skinButton_allGroupBan_Click(object sender, EventArgs e)
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }
            if (this.isExistAllGroupBan)
            {
                DialogResult dialogResult = MessageBoxEx.Show("确认要关闭全员禁言吗？", "提示", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    this.resourceCenter.ClientOutter.RemoveAllGroupBan(this.currentGroup.ID);
                    this.isExistAllGroupBan = false;
                    this.skinButton_allGroupBan.Image = GGTalk.Properties.Resources.allGroupBanTurnOn;
                    this.toolShow.SetToolTip(this.skinButton_allGroupBan, "开启全员禁言");
                    this.chatPanel1.AppendSystemMsg("你关闭了全员禁言");
                }
            }
            else
            {
                DialogResult dialogResult = MessageBoxEx.Show("确认要开启全员禁言吗？", "提示", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    this.resourceCenter.ClientOutter.SetAllGroupBan(this.currentGroup.ID);
                    this.isExistAllGroupBan = true;
                    this.skinButton_allGroupBan.Image = GGTalk.Properties.Resources.allGroupBanTurnOff;
                    this.toolShow.SetToolTip(this.skinButton_allGroupBan, "关闭全员禁言");
                    this.chatPanel1.AppendSystemMsg("你开启了全员禁言");
                }
            }

        }

        private void taToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GGUser unit = (GGUser)this.chatListBox1.SelectSubItem.Tag;
            this.chatPanel1.AppendTextToInputBox("@");
            this.chatPanel1.AddAtMember(unit);
        }

        private void 添加好友ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }
            GGUser unit = (GGUser)this.chatListBox1.SelectSubItem.Tag;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //todo: 查询是否在黑名单内
                bool isFriendBlackMe = this.resourceCenter.ClientOutter.IsInHisBlackList(unit.UserID);
                if (isFriendBlackMe)
                {
                    MessageBox.Show("对方拒绝添加您为好友！", "提示");
                    return;
                }
                bool notExist = this.resourceCenter.ClientGlobalCache.PrepairUnit(unit);
                AddFriendForm addFriendForm = new AddFriendForm(this.resourceCenter, unit.UserID);
                DialogResult res = addFriendForm.ShowDialog();
                //AddFriendResult res = this.resourceCenter.ClientOutter.AddFriend(friend.UserID, FunctionOptions.DefaultFriendCatalog);
                if (res != DialogResult.OK)
                {
                    if (notExist)
                    {
                        this.resourceCenter.ClientGlobalCache.CancelPrepairUnit(unit);
                    }
                    return;
                }
                MessageBoxEx.Show("发送成功", "提示");
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(ee.Message, GlobalResourceManager.SoftwareName);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void nDiskBrowser1_Load(object sender, EventArgs e)
        {

        }



        /// <summary>
        /// 设置全员禁言按钮图标
        /// </summary>
        private void SetAllGroupBanButtonImg()
        {
            if (this.isExistAllGroupBan)
            {
                this.skinButton_allGroupBan.Image = GGTalk.Properties.Resources.allGroupBanTurnOff;
            }
            else
            {
                this.skinButton_allGroupBan.Image = GGTalk.Properties.Resources.allGroupBanTurnOn;
            }
        }
        #endregion

    }
}
