using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TalkBase.Client.UnitViews;
using CCWin.SkinControl;
using ESBasic;
using ESBasic.ObjectManagement.Managers;

namespace TalkBase.Client.UnitViews
{
    /// <summary>
    /// 群组列表。
    /// </summary>
    public partial class GroupListBox : UserControl
    {
        private IUser currentUser;
        private IUnitInfoProvider unitInfoProvider;
        private ESBasic.Func<IGroup, bool> groupFilter;

        public event CbGeneric<IGroup> GroupDoubleClicked;
        public event CbGeneric<IGroup> ChatRecordClicked;
        public event CbGeneric<IGroup> DismissGroupClicked;
        public event CbGeneric<IGroup> QuitGroupClicked;

        #region Property      
        public ContextMenuStrip GroupContextMenuStrip
        {
            get
            {
                return this.skinContextMenuStrip_Group;
            }
        }

        #region SelectedUnit
        /// <summary>
        /// 当前选中的Unit。
        /// </summary>
        public IUnit SelectedUnit
        {
            get
            {
                if (this.chatListBox_group.SelectSubItem == null)
                {
                    return null;
                }

                return (IUnit)this.chatListBox_group.SelectSubItem.Tag;
            }
        }
        #endregion    
        #endregion    

        public GroupListBox()
        {
            InitializeComponent();
        }

        public void Initialize(IUser current, IUnitInfoProvider provider)
        {
            this.Initialize(current, provider ,null);
        }

        /// <summary>
        /// 初始化。
        /// </summary>       
        /// <param name="filter">哪些Group能出现在当前列表中。</param>
        public void Initialize(IUser current, IUnitInfoProvider provider, ESBasic.Func<IGroup, bool> filter)
        {
            this.currentUser = current;
            this.unitInfoProvider = provider;
            this.groupFilter = filter;

            
            this.AssureCatalog(FunctionOptions.DefaultGroupCatalog);
        } 

        public void AddGroup(IGroup group)
        {
            if (this.groupFilter != null)
            {
                if (!this.groupFilter(group))
                {
                    return;
                }
            }

            Image headImage = this.unitInfoProvider.GetHeadImage(group) ?? this.imageList1.Images[0];
            string dateString = string.Format("{0}-{1}-{2}", group.CreateTime.Year.ToString("00"), group.CreateTime.Month.ToString("00"), group.CreateTime.Day.ToString("00"));
            string signature = string.Format("{0}人 {1}", group.MemberList.Count, dateString);
            ChatListSubItem subItem = new ChatListSubItem(group.ID, "", group.DisplayName, signature, ChatListSubItem.UserStatus.Online, headImage);
            subItem.Tag = group;

            ChatListItem ownerItem = this.GetCatelogChatListItem(group);
            ownerItem.IsOpen = true;
            ownerItem.SubItems.Add(subItem);
        }

        #region AssureCatalog/GetCatelogChatListItem
        private void AssureCatalog(string catalog)
        {
            if (!this.catelogManager.Contains(catalog))
            {
                ChatListItem item = new ChatListItem(catalog);
                this.catelogManager.Add(catalog, item);
                this.chatListBox_group.Items.Add(item);                
            }
        }

        private ObjectManager<string, ChatListItem> catelogManager = new ObjectManager<string, ChatListItem>();
        private ChatListItem GetCatelogChatListItem(IGroup group)
        {
            string theCatalog = this.unitInfoProvider.GetCatalog(group);
            if (theCatalog == null)
            {
                theCatalog = "未命名";
            }
            this.AssureCatalog(theCatalog);
            return this.catelogManager.Get(theCatalog);                  
        }
        #endregion      

        public void RequestQuitGroup(string groupID)
        {
            if (this.currentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            ChatListSubItem[] items = this.chatListBox_group.GetSubItemsById(groupID);
            if (items == null || items.Length == 0)
            {
                return;
            }

            IGroup group = (IGroup)items[0].Tag;
            if (this.QuitGroupClicked != null)
            {
                this.QuitGroupClicked(group);
            }
        }

        public void RemoveGroup(string groupID)
        {
            this.chatListBox_group.RemoveSubItemsById(groupID);
            this.chatListBox_group.Invalidate();
        }

        public void SetTwinkleState(string groupID, bool twinkle)
        {
            ChatListSubItem[] items = this.chatListBox_group.GetSubItemsById(groupID);
            if (items == null || items.Length == 0)
            {
                return;
            }
            items[0].IsTwinkle = twinkle;
        }

        public void OnGroupInfoChanged(IGroup group)
        {            
            ChatListSubItem[] subItems = this.chatListBox_group.GetSubItemsById(group.ID);     
            if (subItems == null || subItems.Length == 0)            
            {
                if (this.currentUser.GroupList.Contains(group.ID))
                {
                    this.AddGroup(group);
                }
                return;
            }

            subItems[0].Tag = group;
            subItems[0].DisplayName = group.DisplayName;
            string dateString = string.Format("{0}-{1}-{2}", group.CreateTime.Year.ToString("00"), group.CreateTime.Month.ToString("00"), group.CreateTime.Day.ToString("00"));
            string signature = string.Format("{0}人 {1}", group.MemberList.Count, dateString);
            subItems[0].PersonalMsg = signature;             
        }

        #region GroupCommentNameChanged
        public void GroupCommentNameChanged(IGroup group)
        {
            ChatListSubItem[] items = this.chatListBox_group.GetSubItemsById(group.ID);
            if (items != null && items.Length > 0)
            {
                items[0].DisplayName = group.DisplayName;
            }
        }
        #endregion

        private void 消息记录ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.currentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            IGroup group = (IGroup)this.chatListBox_group.SelectSubItem.Tag;
            if (this.ChatRecordClicked != null)
            {
                this.ChatRecordClicked(group);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.currentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            IGroup group = (IGroup)this.chatListBox_group.SelectSubItem.Tag;
            if (this.QuitGroupClicked != null)
            {
                this.QuitGroupClicked(group);
            }
        }

        public void DismissGroup(IGroup group)
        {
            if (this.currentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }
            if (this.DismissGroupClicked != null)
            {
                this.DismissGroupClicked(group);
            }
        }

        private void 解散该群ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IGroup group = (IGroup)this.chatListBox_group.SelectSubItem.Tag;
            this.DismissGroup(group);
        }

        private void chatListBox_group_DoubleClickSubItem(object sender, ChatListEventArgs e)
        {
            ChatListSubItem item = e.SelectSubItem;
            IGroup group = (IGroup)item.Tag;
            item.IsTwinkle = false;

            if (this.GroupDoubleClicked != null)
            {
                this.GroupDoubleClicked(group);
            }
        }
    }
}
