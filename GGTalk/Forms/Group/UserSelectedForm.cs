using CCWin;
using CCWin.SkinControl;
using GGTalk.Forms.Group;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TalkBase;
using TalkBase.Client;

namespace GGTalk
{
    public partial class UserSelectedForm : BaseForm, IUnitInfoProvider
    {
        private BaseGlobalCache<GGUser, GGGroup> globalCache;
        private GGGroup currentGroup;
        private List<string> currentMemberList = new List<string>();
        private bool restrictDelete = false;//限制删除操作（若为true，只有管理员和创建者才有权）

        public UserSelectedForm()
        {
            InitializeComponent();
        }

        public void Initialize(BaseGlobalCache<GGUser, GGGroup> cache,GGGroup ggGroup)
        {
            this.globalCache = cache;
            this.unitListBox1.Initialize(globalCache.CurrentUser, this);
            foreach (string friendID in globalCache.CurrentUser.GetAllFriendList())
            {
                if (friendID == globalCache.CurrentUser.UserID)
                {
                    continue;
                }
                GGUser friend = globalCache.GetUser(friendID);
                if (friend != null)
                {
                    this.unitListBox1.AddUnit(friend);
                }
            }
            this.unitListBox1.ExpandAllCatalog();
            this.currentGroup = ggGroup;
            if (ggGroup != null)
            {
                foreach (String userID in ggGroup.MemberList)
                {
                    GGUser ggUser = globalCache.GetUser(userID);
                    if (ggUser != null)
                    {
                        this.unitListBox1_UnitClicked(ggUser);
                    }                    
                }
            }
            this.restrictDelete = true;
        }

        public void Initialize4Group(BaseGlobalCache<GGUser, GGGroup> cache, GGGroup ggGroup,bool showMyself)
        {
            this.globalCache = cache;
            this.skinTextBox1.Visible = false;
            this.unitListBox1.Visible = false;
            this.chatListBox_search.Visible = true;
            this.currentGroup = ggGroup;
            if (ggGroup != null)
            {
                this.chatListBox_search.Items.Add(new ChatListItem("群组成员"));
                this.chatListBox_search.Items[0].IsOpen = true;
                foreach (String userID in ggGroup.MemberList)
                {
                    if (!showMyself && userID == cache.CurrentUser.UserID)
                    {
                        continue;
                    }
                    GGUser unit = globalCache.GetUser(userID);
                    if (unit != null)
                    {
                        Image headImage = this.GetHeadImage(unit);
                        string catalog = this.globalCache.CurrentUser.GetFriendCatalog(unit.ID);
                        string displayName = unit.DisplayName;
                        if (catalog != null)
                        {
                            displayName += string.Format("  [ {0} ]", catalog);
                        }
                        ChatListSubItem subItem = new ChatListSubItem(unit.ID, unit.ID, displayName, unit.Signature, ChatListSubItem.UserStatus.Online, headImage);
                        subItem.Tag = unit;
                        this.chatListBox_search.Items[0].SubItems.Add(subItem);
                    }
                }
            }
        }

        private void unitListBox1_UnitClicked(TalkBase.IUnit unit)
        {
            if (this.currentMemberList.Contains(unit.ID))
            {
                return;
            }
            GGUser user = unit as GGUser;
            if (user != null)
            {
                UserPanel panel = new UserPanel();
                panel.GGUser = user;
                panel.Closed += new ESBasic.CbGeneric<UserPanel>(panel_Closed);
                panel.Size = new Size(this.flowLayoutPanel1.Width - 30, panel.Height);
                this.flowLayoutPanel1.Controls.Add(panel);
                this.currentMemberList.Add(user.ID);
                this.SetMemberCount();
            }
        }

        void panel_Closed(UserPanel panel)
        {
            if (this.restrictDelete)
            {
                if (globalCache.CurrentUser.ID != this.currentGroup.CreatorID)
                {
                    MessageBoxEx.Show("您无权操作！");
                    return;
                }
                if (panel.GGUser.ID == this.currentGroup.CreatorID)
                {
                    MessageBoxEx.Show("不能删除管理员！");
                    return;
                }
            }
            this.flowLayoutPanel1.Controls.Remove(panel);
            this.currentMemberList.Remove(panel.GGUser.ID);
            this.SetMemberCount();
        }

        private void SetMemberCount()
        {
            this.skinLabel1.Text = string.Format("已选择：（{0}）人", this.currentMemberList.Count);
        }

        public Image GetHeadImage(IUnit unit)
        {
            return GlobalResourceManager.GetHeadImageOnline((GGUser)unit);
        }

        public string GetCatalog(IUnit unit)
        {
            return null;
        }

        private void skinTextBox1_CloseButtonClicked(object sender, EventArgs e)
        {
            this.skinTextBox1.SkinTxt.Clear();
            this.chatListBox_search.Items.Clear();
            this.chatListBox_search.Visible = false;
            this.skinLabel_noResult.Visible = false;

            this.unitListBox1.Visible = true;        
        }

        private void skinTextBox1_EnterKeyInput(object sender, EventArgs e)
        {
            this.DoSearch();
            this.chatListBox_search.Visible = true;
            this.unitListBox1.Visible = false;
        }

        private void DoSearch()
        {
            this.chatListBox_search.Items.Clear();
            string idOrName = this.skinTextBox1.SkinTxt.Text.Trim();
            List<GGUser> users = this.globalCache.SearchUser(idOrName);       
            bool hasNoResult = users.Count == 0;

            this.skinLabel_noResult.Visible = hasNoResult;
            if (!hasNoResult)
            {
                this.chatListBox_search.Items.Add(new ChatListItem("查找结果"));
                this.chatListBox_search.Items[0].IsOpen = true;
                foreach (GGUser unit in users)
                {
                    UserStatus status = (!unit.IsUser) ? UserStatus.Online : ((IUser)unit).UserStatus;
                    Image headImage = this.GetHeadImage(unit);                  
                    string catalog = this.globalCache.CurrentUser.GetFriendCatalog(unit.ID);
                    string displayName = unit.DisplayName;
                    if (catalog != null)
                    {
                        displayName += string.Format("  [ {0} ]", catalog);
                    }                    
                    ChatListSubItem subItem = new ChatListSubItem(unit.ID, unit.ID, displayName, unit.Signature, ChatListSubItem.UserStatus.Online, headImage);
                    subItem.Tag = unit;
                    this.chatListBox_search.Items[0].SubItems.Add(subItem);
                }               
            }
        }        
       
        private void chatListBox_search_DoubleClickSubItem(object sender, CCWin.SkinControl.ChatListEventArgs e)
        {
            IUnit unit = (IUnit)e.SelectSubItem.Tag;
            if (unit.UnitType == UnitType.User)
            {
                this.unitListBox1_UnitClicked(unit);
             }            
        }

        private void skinButton_ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        private void skinButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private List<string> userIDSelected = new List<string>();
        public List<string> UserIDSelected
        {
            get
            {
                return this.currentMemberList;
            }
        }


    }
}
