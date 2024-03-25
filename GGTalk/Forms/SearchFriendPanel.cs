using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using CCWin.SkinControl;
using ESPlus.Rapid;
using CCWin;
using TalkBase.Client;
using TalkBase;
using TalkBase.Client.Application;
using TalkBase.Client.Bridges;
using ESBasic.Helpers;

namespace GGTalk
{
    /// <summary>
    /// 查找好友。
    /// </summary>
    public partial class SearchFriendPanel : UserControl
    {       
        private ResourceCenter<GGUser, GGGroup > resourceCenter;
        private IChatSupporter ggSupporter;
        public SearchFriendPanel()
        {
            InitializeComponent();                    
        }

        public void Initialize( ResourceCenter<GGUser, GGGroup > center,IChatSupporter supporter)
        {
            this.resourceCenter = center;
            this.ggSupporter = supporter;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!this.resourceCenter.Connected) {
                MessageBoxEx.Show("您已掉线！");
            }
            this.chatListBox.Items.Clear();
            string idOrName = this.skinTextBox_id.SkinTxt.Text.Trim();
            List<GGUser> users = new List<GGUser>();

            List<IUser> iUsers = this.resourceCenter.ClientOutter.SearchUserList(idOrName);
            if (iUsers != null)
            {
                foreach (IUser user in iUsers)
                {
                    users.Add(user as GGUser);
                }
            }
            List<IGroup> groups = this.resourceCenter.ClientOutter.SearchGroupList(idOrName);
            this.SetSearchResult(users, groups,true);
    
        }

        private void skinTextBox_id_SkinTxt_TextChanged(object sender, EventArgs e)
        {
            this.skinLabel_noResult.Visible = false;
            this.chatListBox.Items.Clear();
            string id = this.skinTextBox_id.SkinTxt.Text.Trim();
            if (string.IsNullOrEmpty(id))
            {
                return;
            }
            List<GGUser> ggUsers = this.resourceCenter.ClientGlobalCache.SearchUser_Fuzzy(id);
            List<GGGroup> ggGroups = this.resourceCenter.ClientGlobalCache.SearchGroup_Fuzzy(id);
            this.skinLabel_noResult.Text = "按回车获取结果";
            List<IGroup> iGroups = new List<IGroup>();
            if (ggGroups != null)
            {
                iGroups = ESBasic.Collections.CollectionConverter.ConvertListUpper<IGroup, GGGroup>(ggGroups);
            }
            
            this.SetSearchResult(ggUsers, iGroups);
        }

        //设置查询结果
        private void SetSearchResult(List<GGUser> users, List<IGroup> groups,bool isFromServer=false)
        {
            bool hasNoResult = users.Count == 0 && groups.Count == 0;
            if (isFromServer)
            {
                this.skinLabel_noResult.Text = "没有找到符合搜索条件的结果";
            }
            this.skinLabel_noResult.Visible = hasNoResult;
            if (!hasNoResult)
            {
                this.chatListBox.Items.Add(new ChatListItem("查找结果"));
                this.chatListBox.Items[0].IsOpen = true;
                foreach (IUser unit in users)
                {
                    if (unit.ID == this.resourceCenter.CurrentUserID)
                    {
                        continue;
                    }
                    UserStatus status = (!unit.IsUser) ? UserStatus.Online : unit.UserStatus;
                    Image headImage = this.resourceCenter.UnitInfoProvider.GetHeadImage(unit);
                    string catalog = this.resourceCenter.ClientGlobalCache.CurrentUser.GetFriendCatalog(unit.ID);
                    string displayName = unit.DisplayName;
                    if (catalog != null)
                    {
                        displayName += string.Format("  [ {0} ]", catalog);
                    }
                    else
                    {
                        displayName += "  [ 陌生人，双击添加 ]";
                    }
                    ChatListSubItem subItem = new ChatListSubItem(unit.ID, unit.ID, displayName, "", ChatListSubItem.UserStatus.Online, headImage);
                    subItem.Tag = unit;
                    this.chatListBox.Items[0].SubItems.Add(subItem);
                }
                foreach (IGroup group in groups)
                {
                    Image headImage = global::GGTalk.Properties.Resources.Group2;
                    string displayName = group.Name;
                    if (group.MemberList.Contains(this.resourceCenter.CurrentUserID))
                    {
                        displayName += "  [ 群 ]";
                    }
                    else
                    {
                        displayName += "  [ 双击加入 ]";
                    }
                    ChatListSubItem subItem = new ChatListSubItem(group.ID, group.Name, displayName, group.Announce, ChatListSubItem.UserStatus.Online, headImage);
                    subItem.Tag = group;
                    this.chatListBox.Items[0].SubItems.Add(subItem);
                }
            }
        }

        private void chatListBox_DoubleClickSubItem(object sender, ChatListEventArgs e)
        {
            IUnit unit = (IUnit)e.SelectSubItem.Tag;
            if (unit.UnitType == UnitType.User)
            {
                this.StartUserChat((GGUser)unit);
            }
            else if (unit.UnitType == UnitType.Group)
            {
                this.StartGroupChat((GGGroup)unit);
            }            
        }

        private void StartGroupChat(GGGroup group)
        {
            if (group == null)
            {
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            try
            {
                bool isMember = group.MemberList.Contains(this.resourceCenter.CurrentUserID);
                if (isMember)
                {
                    //已是该组成员，弹出组聊天框
                    IChatForm form = this.resourceCenter.ChatFormController.GetForm(group.ID);
                    Program.ChatFormShower.ShowChatForm(form);
                }
                else
                {
                    AddGroupForm addGroupForm = new AddGroupForm(this.resourceCenter, group);
                    DialogResult res = addGroupForm.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        MessageBoxEx.Show("发送成功", "提示");
                        //if (notExist)
                        //{
                        //    this.resourceCenter.ClientGlobalCache.CancelPrepairUnit(friend);
                        //}
                    }
                    
                }

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

        private void StartUserChat(GGUser friend)
        {
            if (this.resourceCenter.CurrentUserID == friend.UserID)
            {
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            try
            {
                bool isFriend = this.resourceCenter.ClientGlobalCache.CurrentUser.GetAllFriendList().Contains(friend.UserID) ;
                if (!isFriend)
                {
                    //todo: 查询是否在黑名单内
                    bool isFriendBlackMe = this.resourceCenter.ClientOutter.IsInHisBlackList(friend.UserID);
                    if (isFriendBlackMe)
                    {
                        MessageBox.Show("对方拒绝添加您为好友！", "提示");
                        return;
                    }

                    bool notExist = this.resourceCenter.ClientGlobalCache.PrepairUnit(friend);
                    AddFriendForm addFriendForm = new AddFriendForm(this.resourceCenter, friend.UserID);
                    DialogResult res = addFriendForm.ShowDialog();
                    //AddFriendResult res = this.resourceCenter.ClientOutter.AddFriend(friend.UserID, FunctionOptions.DefaultFriendCatalog);
                    if (res != DialogResult.OK)
                    {
                        if (notExist)
                        {
                            this.resourceCenter.ClientGlobalCache.CancelPrepairUnit(friend);
                        }
                        return;
                    }
                    MessageBoxEx.Show("发送成功", "提示");
                }
                else
                {
                    //如果是黑名单用户，则显示聊天记录。
                    if (this.resourceCenter.ClientGlobalCache.CurrentUser.IsInBlackList(friend.UserID))
                    {
                        Form form1 = this.resourceCenter.ChatFormController.GetChatRecordForm(friend.UserID);
                        if (form1 != null)
                        {
                            form1.Show();
                        }
                        return;
                    }
                    //双方加为好友后，跳转到好友聊天框
                    IChatForm form = this.resourceCenter.ChatFormController.GetForm(friend.UserID);
                    Program.ChatFormShower.ShowChatForm(form);
                }
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

        private void SearchFriendPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.skinTextBox_id.Focus();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            IUnit unit = (IUnit)this.chatListBox.SelectSubItem.Tag;
            if (unit.UnitType == UnitType.User)
            {
                if (unit.ID == this.resourceCenter.CurrentUserID)
                {
                    return;
                }
                UserInfoForm form = new UserInfoForm(this.resourceCenter, (GGUser)unit);
                form.Show();
            }
            else if (unit.UnitType == UnitType.Group)
            {
                GroupInfoForm form = new GroupInfoForm(this.resourceCenter, (GGGroup)unit);
                form.Show();
            }

        }

        private void 发送消息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GGUser friend = (GGUser)this.chatListBox.SelectSubItem.Tag;
            this.StartUserChat(friend);
        }

        //点击输入框上的关闭
        private void skinTextBox_id_CloseButtonClicked(object sender, EventArgs e)
        {
            this.skinLabel_noResult.Visible = false;
            this.chatListBox.Items.Clear();
            this.Visible = false;
        }        

    }
}
