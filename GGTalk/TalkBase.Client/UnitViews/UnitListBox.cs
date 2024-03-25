using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CCWin.SkinControl;
using ESBasic.ObjectManagement.Managers;
using ESBasic;
using TalkBase.Client.UnitViews;
using System.Threading;

namespace TalkBase.Client.UnitViews
{
    /// <summary>
    /// 好友列表。（如果User和Group混合使用，则必须保证User的ID与Group的ID不会一样）
    /// </summary>
    public partial class UnitListBox : UserControl, IComparer<ChatListSubItem>
    {
        private IUserInformationForm userInformationForm;//悬浮至头像时   
        private IGroupInformationForm groupInformationForm;        
        private IUser currentUser;
        private IUnitInfoProvider unitInfoProvider;
        private Comparison<ChatListItem> comparison4ListItem;        

        public UnitListBox()
        {
            InitializeComponent();
            this.修改备注姓名ToolStripMenuItem.Visible = FunctionOptions.FriendCommentName;
            this.comparison4ListItem = new Comparison<ChatListItem>(this.CompareChatListItem);            
        }

        #region event
        /// <summary>
        /// 添加分组的菜单被点击。参数:组名称
        /// </summary>
        public event CbGeneric AddCatalogClicked;
        /// <summary>
        /// 修改组名的菜单被点击。参数:组名称
        /// </summary>
        public event CbGeneric<string> ChangeCatalogNameClicked;
        public event CbGeneric<IUnit> UnitClicked;
        public event CbGeneric<IUnit> UnitDoubleClicked;
        public event CbGeneric<IUnit> ChatRecordClicked;
        public event CbGeneric<IUnit> RemoveUnitClicked;
        public event CbGeneric<IUnit> ChangeUnitCommentNameClicked;
        public event CbGeneric<IUnit> ViewInfoClicked;
        /// <summary>
        /// 当修改分组名称完成时，触发此事件。参数：oldName - newName - isMerge
        /// </summary>
        public event CbGeneric<string, string, bool> CatalogNameChanged;
        /// <summary>
        /// 当增加一个分组完成时，触发此事件。参数：CatelogName
        /// </summary>
        public event CbGeneric<string> CatalogAdded;
        /// <summary>
        /// 当删除一个分组完成时，触发此事件。参数：CatelogName
        /// </summary>
        public event CbGeneric<string> CatalogRemoved;
        /// <summary>
        /// 当将好友转移到另一组完成时，触发此事件。参数：  UnitID - oldCatalog - newCatalogName
        /// </summary>
        public event CbGeneric<string, string, string> UnitCatalogMoved;

        public event CbGeneric<IUnit> BeforeUnitContextMenuShow;
        #endregion

        #region Property
        #region IconSizeMode
        public ChatListItemIcon IconSizeMode
        {
            get
            {
                return this.chatListBox.IconSizeMode;
            }
            set
            {
                this.chatListBox.IconSizeMode = value;
                this.chatListBox.Invalidate();
            }
        }
        #endregion

        #region PreLoadCatalog
        private bool preLoadCatalog = true;
        public bool PreLoadCatalog
        {
            get { return preLoadCatalog; }
            set { preLoadCatalog = value; }
        }
        #endregion

        #region CatalogContextMenuVisiable
        public bool CatalogContextMenuVisiable
        {
            get { return this.chatListBox.SubItemMenu != null; }
            set { this.chatListBox.SubItemMenu = value ? this.skinContextMenuStrip1 : null; }
        }
        #endregion

        #region UserContextMenuVisiable
        public bool UserContextMenuVisiable
        {
            get { return this.chatListBox.ListSubItemMenu.Visible; }
            set { this.chatListBox.ListSubItemMenu.Visible = value; }
        }
        #endregion

        #region SetUserContextMenuStrip
        public void SetUserContextMenuStrip(ContextMenuStrip menu)
        {
            this.chatListBox.ListSubItemMenu = menu ?? this.skinContextMenuStrip_user;
        }

        /// <summary>
        /// 将子节点右键菜单清空 （用于右键自己时不弹出菜单）
        /// </summary>
        public void CancelSubItemContextMenuStrip()
        {
            this.chatListBox.ListSubItemMenu = null;
        }
        #endregion

        #region DrawContentType
        public DrawContentType DrawContentType
        {
            get
            {
                return this.chatListBox.DrawContentType;
            }
            set
            {
                this.chatListBox.DrawContentType = value;
            }
        }
        #endregion

        #region FriendsMobile
        public bool FriendsMobile
        {
            get
            {
                return this.chatListBox.FriendsMobile;
            }
            set
            {
                this.chatListBox.FriendsMobile = value;
            }
        }
        #endregion

        #region DefaultGroupCatalogName
        private string defaultGroupCatalogName = "我的群";
        public string DefaultGroupCatalogName
        {
            get { return defaultGroupCatalogName; }
            set { defaultGroupCatalogName = value; }
        }
        #endregion

        #region SelectedUnit
        /// <summary>
        /// 当前选中的Unit。
        /// </summary>
        public IUnit SelectedUnit
        {
            get
            {
                if (this.chatListBox.SelectSubItem == null)
                {
                    return null;
                }

                return (IUnit)this.chatListBox.SelectSubItem.Tag;
            }
        }
        #endregion    

        #region DefaultFriendCatalogName
        private string defaultFriendCatalogName = "我的好友";
        /// <summary>
        /// 当从我的FriendDictionary中搜索不到好友的所属组时，则使用DefaultFriendCatalogName。
        /// </summary>
        public string DefaultFriendCatalogName
        {
            get { return defaultFriendCatalogName; }
            set { defaultFriendCatalogName = value; }
        }
        #endregion         

        #region UnitContextMenuStrip
        public ContextMenuStrip UnitContextMenuStrip
        {
            get
            {
                return this.skinContextMenuStrip_user;
            }
        } 
        #endregion
        #endregion

        #region Initialize
        public void Initialize(IUser current, IUnitInfoProvider provider)
        {
            this.Initialize(current, provider, null, null);            
        }

        public void Initialize(IUser current, IUnitInfoProvider provider, IUserInformationForm form, IGroupInformationForm groupForm)
        {
            this.unitInfoProvider = provider;
            this.currentUser = current;
            this.userInformationForm = form;
            this.groupInformationForm = groupForm;
            if (this.userInformationForm != null)
            {
                ((Form)this.userInformationForm).Visible = false;
            }
            
            if (this.preLoadCatalog)
            {
                this.AssureCatalog(FunctionOptions.DefaultFriendCatalog);
                foreach (string catalog in this.currentUser.GetFriendCatalogList())
                {
                    this.AssureCatalog(catalog);
                }

                if (FunctionOptions.BlackList)
                {
                    this.AssureCatalog(FunctionOptions.BlackListCatalogName);
                }
            }
        } 
        #endregion

        #region SelectUnit
        public void SelectUnit(string unitID, bool springClickEvent)
        {
            ChatListSubItem[] items = this.chatListBox.GetSubItemsById(unitID);
            if (items == null || items.Length == 0)
            {
                return;
            }

            this.chatListBox.SelectSubItem = items[0];
            if (springClickEvent)
            {
                this.chatListBox_ClickSubItem(this, items[0]);
            }
        } 
        #endregion

        #region AssureCatalog/GetCatelogChatListItem
        /// <summary>
        /// 确保目标分组存在。
        /// </summary>
        /// <param name="catalog">分组名称</param>
        private void AssureCatalog(string catalog)
        {
            if (!this.catelogManager.Contains(catalog))
            {
                ChatListItem item = new ChatListItem(catalog);
                this.catelogManager.Add(catalog, item);
                this.chatListBox.Items.Add(item);
                this.chatListBox.Items.Sort(this.comparison4ListItem);
            }
        }

        private ObjectManager<string, ChatListItem> catelogManager = new ObjectManager<string, ChatListItem>();
        private ChatListItem GetCatelogChatListItem(IUnit unit)
        {
            if (unit.ID == this.currentUser.ID)
            {
                this.AssureCatalog(this.defaultFriendCatalogName);
                return this.catelogManager.Get(this.defaultFriendCatalogName);
            }

            string theCatalog = this.unitInfoProvider.GetCatalog(unit);
            if (theCatalog != null)
            {
                this.AssureCatalog(theCatalog);
                return this.catelogManager.Get(theCatalog);
            }

            if (!unit.IsUser)
            {                           
                this.AssureCatalog(this.defaultGroupCatalogName);
                return this.catelogManager.Get(this.defaultGroupCatalogName);
            }

            string catalog = this.currentUser.GetFriendCatalog(unit.ID) ?? this.defaultFriendCatalogName;           
            this.AssureCatalog(catalog);
            return this.catelogManager.Get(catalog);
        } 
        #endregion      

        #region AddUnit
        public void AddUnit(IUnit unit)
        {
            ChatListSubItem[] items = this.chatListBox.GetSubItemsById(unit.ID);
            if (items != null && items.Length > 0)
            {
                return;
            }

            string signature = (!unit.IsUser) ? "" : ((IUser)unit).Signature;
            UserStatus status = (!unit.IsUser) ? UserStatus.Online : ((IUser)unit).UserStatus;
            Image headImage = this.unitInfoProvider.GetHeadImage(unit) ?? ((!unit.IsUser) ? this.imageList1.Images[0] : this.imageList1.Images[1]);

            ChatListSubItem subItem = new ChatListSubItem(unit.ID, "", unit.DisplayName, signature, this.ConvertUserStatus(status), headImage);
            subItem.Tag = unit;
            this.GetCatelogChatListItem(unit).SubItems.AddAccordingToStatus(subItem);            
            //subItem.OwnerListItem.SubItems.Sort(this);
        } 
        #endregion

        #region GetUnitCount
        public int GetUnitCount(string catalog)
        {
            if (catalog == null)
            {
                int totalCount = 0;
                foreach (ChatListItem item in this.chatListBox.Items)
                {
                    totalCount += item.SubItems.Count;
                }
                return totalCount;
            }

            ChatListItem target = this.catelogManager.Get(catalog);
            if (target == null)
            {
                return 0;
            }

            return target.SubItems.Count;
        } 
        #endregion

        #region ExpandAllCatalog/ExpandCatalog/CloseCatalog/CloseAllCatalog
        public void ExpandAllCatalog()
        {
            foreach (ChatListItem item in this.chatListBox.Items)
            {
                item.IsOpen = true;
            }
        }         

        public void ExpandCatalog(string catalog)
        {
            ChatListItem item = this.catelogManager.Get(catalog);
            if (item != null)
            {
                item.IsOpen = true;
            }
        }

        public void CloseCatalog(string catalog)
        {
            ChatListItem item = this.catelogManager.Get(catalog);
            if (item != null)
            {
                item.IsOpen = false;
            }
        }

        public void CloseAllCatalog()
        {
            foreach (ChatListItem item in this.chatListBox.Items)
            {
                item.IsOpen = false;
            }
        }
        #endregion

        #region SetAllUnitOffline/SortAllUnit/RemoveUnit
        public void SetAllUnitOffline()
        {
            foreach (ChatListItem item in this.chatListBox.Items)
            {
                foreach (ChatListSubItem sub in item.SubItems)
                {
                    sub.Status = ChatListSubItem.UserStatus.OffLine;
                }
            }

            this.chatListBox.Invalidate();
        }         

        public void SortAllUnit()
        {
            foreach (ChatListItem item in this.catelogManager.GetAll())
            {
                if (item.SubItems.Count > 0)
                {
                    item.SubItems.Sort(this);
                }
            }
        }

        public void RemoveUnit(string unitID)
        {
            this.chatListBox.RemoveSubItemsById(unitID);
            this.chatListBox.Invalidate();           
        }
        #endregion

        #region ContainsUser
        public bool ContainsUser(string userID)
        {
            ChatListSubItem[] items = this.chatListBox.GetSubItemsById(userID);
            return (items != null && items.Length > 0);
        } 
        #endregion

        #region SetTwinkleState
        public void SetTwinkleState(string unitID, bool twinkle)
        {
            ChatListSubItem[] items = this.chatListBox.GetSubItemsById(unitID);
            if (items == null || items.Length == 0)
            {
                return;
            }
            items[0].IsTwinkle = twinkle;
        } 
        #endregion

        #region UserStatusChanged
        public void UserStatusChanged(IUnit unit)
        {
            if (!unit.IsUser)
            {
                return;
            }

            IUser user = unit as IUser;
            ChatListSubItem[] items = this.chatListBox.GetSubItemsById(user.ID);
            if (items == null || items.Length == 0)
            {
                return;
            }

            items[0].HeadImage = this.unitInfoProvider.GetHeadImage(user);
            items[0].Status = this.ConvertUserStatus(user.UserStatus);
            ChatListItem item = items[0].OwnerListItem;
            if (item != null)
            {
                item.SubItems.Sort(this);
            }
            this.chatListBox.Invalidate();
        } 
        #endregion

        #region SearchChatListSubItem
        public List<ChatListSubItem> SearchChatListSubItem(string idOrName)
        {
            ChatListSubItem[] items = this.chatListBox.GetSubItemsByText(idOrName);
            List<ChatListSubItem> list = new List<ChatListSubItem>();
            if (items != null)
            {
                foreach (ChatListSubItem item in items)
                {
                    if (item.ID != this.currentUser.ID)
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        } 
        #endregion

        #region UnitInfoChanged
        public void UnitInfoChanged(IUnit unit)
        {
            ChatListSubItem[] items = this.chatListBox.GetSubItemsById(unit.ID);
            if (items != null && items.Length > 0) 
            {
                IUnit origin = (IUnit)items[0].Tag;
                ChatListItem ownerItem = items[0].OwnerListItem;
                ownerItem.SubItems.Remove(items[0]);
                this.AddUnit(unit); //有可能是新添加的好友
            }
        } 
        #endregion

        #region UnitCommentNameChanged
        public void UnitCommentNameChanged(IUnit unit)
        {
            ChatListSubItem[] items = this.chatListBox.GetSubItemsById(unit.ID);
            if (items != null && items.Length > 0) 
            {
                items[0].DisplayName = unit.DisplayName;
            }
        }
        #endregion

        #region Menu
        private void toolStripMenuItem51_Click(object sender, EventArgs e)
        {
            ChatListSubItem item = this.chatListBox.SelectSubItem;
            IUnit friend = (IUnit)item.Tag;
            item.IsTwinkle = false;

            if (friend.ID == this.currentUser.ID)
            {
                return;
            }

            if (this.UnitDoubleClicked != null)
            {
                this.UnitDoubleClicked(friend);
            }
        }

        private void 消息记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.currentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            IUnit friend = (IUnit)this.chatListBox.SelectSubItem.Tag;
            if (friend.ID == this.currentUser.ID)
            {
                return;
            }

            if (this.ChatRecordClicked != null)
            {
                this.ChatRecordClicked(friend);
            }
        }

        private void 删除好友ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.currentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            IUnit friend = (IUnit)this.chatListBox.SelectSubItem.Tag;
            if (friend.ID == this.currentUser.ID)
            {
                return;
            }

            if (this.RemoveUnitClicked != null)
            {
                this.RemoveUnitClicked(friend);
            }
        }

        private void chatListBox_DoubleClickSubItem(object sender, ChatListEventArgs e)
        {
            ChatListSubItem item = e.SelectSubItem;
            IUnit friend = (IUnit)item.Tag;
            item.IsTwinkle = false;

            if (this.UnitDoubleClicked != null)
            {
                this.UnitDoubleClicked(friend);
            }
        }

        private void chatListBox_ClickSubItem(object sender, ChatListSubItem e)
        {
            if (this.UnitClicked != null)
            {
                this.UnitClicked((IUnit)e.Tag);
            }
        }

        private void chatListBox_DragSubItemDrop(object sender, DragListEventArgs e)
        {            
            if (this.UnitCatalogMoved != null)
            {
                IUnit user = (IUnit)e.QSubItem.Tag;
                this.UnitCatalogMoved(user.ID, e.QSubItem.OwnerListItem.Text, e.HSubItem.OwnerListItem.Text);
            }

            ChatListItem newOwner = this.catelogManager.Get(e.HSubItem.OwnerListItem.Text);
            newOwner.SubItems.Sort(this);

        }

        private void 修改名称ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.currentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            string oldName = this.chatListBox.SelectItem.Text;
            if (oldName == FunctionOptions.BlackListCatalogName || oldName == FunctionOptions.DefaultFriendCatalog)
            {
                MessageBox.Show("当前分组名称不能被修改。");
                return;
            }

            if (this.ChangeCatalogNameClicked != null)
            {
                this.ChangeCatalogNameClicked(oldName);
            }
        }

        private void 添加分组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.currentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            if (this.AddCatalogClicked != null)
            {
                this.AddCatalogClicked();
            }
        }

        private void 删除分组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.currentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            string name = this.chatListBox.SelectItem.Text;
            if (FunctionOptions.DefaultFriendCatalog == name)
            {
                MessageBox.Show(string.Format("分组 [{0}] 是默认分组，不能删除！", name));
                return;
            }

            if (name == FunctionOptions.BlackListCatalogName)
            {
                MessageBox.Show(string.Format("分组 [{0}] 是默认分组，不能删除！", FunctionOptions.BlackListCatalogName));
                return;
            }

            if (this.chatListBox.SelectItem.SubItems.Count > 0)
            {
                MessageBox.Show(string.Format("分组 [{0}] 不为空，不能删除！", name));
                return;
            }

            if (!ESBasic.Helpers.WindowsHelper.ShowQuery(string.Format("您确定要删除分组 [{0}] 吗？", name)))
            {
                return;
            }

            this.chatListBox.Items.Remove(this.chatListBox.SelectItem);
            this.catelogManager.Remove(name);
            if (this.CatalogRemoved != null)
            {
                this.CatalogRemoved(name);
            }
        }
        #endregion

        #region 显示用户资料       
        private void chatShow_MouseEnterHead(object sender, ChatListEventArgs e)
        {
            if (this.userInformationForm == null && this.groupInformationForm == null)
            {
                return;
            }

            ChatListSubItem item = e.MouseOnSubItem;
            if (item == null)
            {
                item = e.SelectSubItem;
            }

            Point loc = this.PointToScreen(this.Location);

            //int top = this.Top + this.chatListBox.Top + (item.HeadRect.Y - this.chatListBox.chatVScroll.Value);
            //int left = this.Left - 279 - 5;
            int top = loc.Y + (item.HeadRect.Y - this.chatListBox.chatVScroll.Value) - this.Location.Y;
            int left = loc.X - 279 - 5;
            //int ph = Screen.GetWorkingArea(this).Height;

            //if (top + 181 > ph)
            //{
            //    top = ph - 181 - 5;
            //}

            if (left < 0)
            {
                left = this.Right + 5;
            }

            Point location = new Point(left, top);
            Form form = null;
            IUnit unit = (IUnit)item.Tag;            
            if (unit.IsUser)
            {
                form = (Form)this.userInformationForm;
                this.userInformationForm.SetUser((IUser)unit, location);
                
            }
            else
            {
                form = (Form)this.groupInformationForm;
                this.groupInformationForm.SetGroup((IGroup)unit, location);               
            }           
            form.Show();            
        }

        private void chatShow_MouseLeaveHead(object sender, ChatListEventArgs e)
        {
            if (this.userInformationForm == null)
            {
                return;
            }

            Thread.Sleep(100);
            Form form = (Form)this.userInformationForm;
            if (!form.Bounds.Contains(Cursor.Position))
            {
                form.Hide();
            }
        }
        #endregion        

        #region ChangeCatelogName/AddCatalog
        public void ChangeCatelogName(string oldName, string newName)
        {
            if (this.currentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            if (oldName == FunctionOptions.BlackListCatalogName)
            {
                MessageBox.Show("当前分组名称不能被修改。");
                return;
            }

            this.catelogManager.Remove(oldName);
            ChatListItem existedItem = null;
            foreach (ChatListItem item in this.chatListBox.Items)
            {
                if (item.Text == newName)
                {
                    existedItem = item;
                    break;
                }
            }
            if (existedItem != null)
            {
                foreach (ChatListSubItem sub in this.chatListBox.SelectItem.SubItems)
                {
                    sub.OwnerListItem = existedItem;
                    existedItem.SubItems.Add(sub);
                }
                existedItem.SubItems.Sort(this);
                this.chatListBox.Items.Remove(this.chatListBox.SelectItem);
                existedItem.IsOpen = true;
                if (this.CatalogNameChanged != null)
                {
                    this.CatalogNameChanged(oldName, newName, true);
                }
                return;
            }

            this.catelogManager.Add(newName, this.chatListBox.SelectItem);
            this.chatListBox.SelectItem.Text = newName;

            this.chatListBox.Items.Sort(this.comparison4ListItem);
            if (this.CatalogNameChanged != null)
            {
                this.CatalogNameChanged(oldName, newName, false);
            }
        }

        public void AddCatalog(string catelogName, bool isEcho = false)
        {
            if (this.currentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }           

            foreach (ChatListItem item in this.chatListBox.Items)
            {
                if (item.Text == catelogName)
                {
                    MessageBox.Show("已经存在同名的分组！");
                    return;
                }
            }

            ChatListItem newItem = new ChatListItem(catelogName);
            this.catelogManager.Add(catelogName, newItem);
            this.chatListBox.Items.Add(newItem);
            this.chatListBox.Items.Sort(this.comparison4ListItem);

            if (!isEcho&&this.CatalogAdded != null)
            {
                this.CatalogAdded(catelogName);
            }
        } 
        #endregion        

        #region ConvertUserStatus
        private ChatListSubItem.UserStatus ConvertUserStatus(UserStatus status)
        {
            if (status == UserStatus.Hide)
            {
                return ChatListSubItem.UserStatus.OffLine;
            }

            return (ChatListSubItem.UserStatus)((int)status);
        }
        #endregion        

        #region CompareChatListItem
        private int CompareChatListItem(ChatListItem a, ChatListItem b)
        {
            if (a == b)
            {
                return 0;
            }

            if (a.Text == FunctionOptions.DefaultFriendCatalog)
            {
                return -1;
            }

            if (b.Text == FunctionOptions.DefaultFriendCatalog)
            {
                return 1;
            }

            if (a.Text == FunctionOptions.BlackListCatalogName)
            {
                return 1;
            }

            if (b.Text == FunctionOptions.BlackListCatalogName)
            {
                return -1;
            }

            return a.Text.CompareTo(b.Text);
        } 
        #endregion

        #region CompareSubItem
        public int Compare(ChatListSubItem x, ChatListSubItem y)
        {
            if (x == y)
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            IUnit ux = (IUnit)x.Tag;
            IUnit uy = (IUnit)y.Tag;

            if (ux.ID == this.currentUser.ID)
            {
                return -1;
            }

            if (uy.ID == this.currentUser.ID)
            {
                return 1;
            }

            if (x.Status == ChatListSubItem.UserStatus.OffLine && y.Status == ChatListSubItem.UserStatus.OffLine)
            {
                return x.DisplayName.CompareTo(y.DisplayName);
            }

            if (x.Status != ChatListSubItem.UserStatus.OffLine && y.Status != ChatListSubItem.UserStatus.OffLine)
            {
                return x.DisplayName.CompareTo(y.DisplayName);
            }

            if (x.Status != ChatListSubItem.UserStatus.OffLine)
            {
                return -1;
            }

            return 1;
        }
        #endregion

        private void chatListBox_BeforeListSubItemMenuShow(object sender, ChatListSubItem e)
        {
            ChatListSubItem item = this.chatListBox.SelectSubItem;
            IUnit friend = (IUnit)item.Tag;
            if(this.BeforeUnitContextMenuShow != null)
            {
                this.BeforeUnitContextMenuShow(friend);
            }
        }

        private void 修改备注姓名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChatListSubItem item = this.chatListBox.SelectSubItem;
            IUnit friend = (IUnit)item.Tag;
            item.IsTwinkle = false;

            if (friend.ID == this.currentUser.ID)
            {
                return;
            }

            if (this.ChangeUnitCommentNameClicked != null)
            {
                this.ChangeUnitCommentNameClicked(friend);
            }
        }

        private void toolStripMenuItem_moveFriend_DropDownOpening(object sender, EventArgs e)
        {
            this.toolStripMenuItem_moveFriend.DropDownItems.Clear();
            IUnit friend = (IUnit)this.chatListBox.SelectSubItem.Tag;
            if (friend == this.currentUser)
            {
                return;
            }

            foreach (ChatListItem item in this.chatListBox.Items)
            {
                if (item.Text == this.chatListBox.SelectSubItem.OwnerListItem.Text)
                {
                    continue;
                }

                this.toolStripMenuItem_moveFriend.DropDownItems.Add(item.Text, null, new EventHandler(this.Catalog4MovedClicked));
            }
        }

        private void Catalog4MovedClicked(object sender, EventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            if (item != null)
            {
                string newCatalog = item.Text;
                //ChatListItem newOwner = null;
                //foreach (ChatListItem tmp in this.chatListBox.Items)
                //{
                //    if (tmp.Text == newCatalog)
                //    {
                //        newOwner = tmp;
                //        break;
                //    }
                //}

                string unitID = this.SelectedUnit.ID ;
                if (unitID == this.currentUser.ID)
                {
                    return;
                }

                ChatListSubItem target = this.chatListBox.SelectSubItem;
                ChatListItem oldOwner = target.OwnerListItem;

                this.Catalog4Moved(unitID, oldOwner.Text, newCatalog);
                //oldOwner.SubItems.Remove(target);
                //newOwner.SubItems.Add(target);
                //target.OwnerListItem = newOwner;
                //newOwner.SubItems.Sort(this);
                //this.chatListBox.Refresh();

                if (this.UnitCatalogMoved != null)
                {
                    this.UnitCatalogMoved(unitID, oldOwner.Text, newCatalog);
                }
            }
        }

        public void Catalog4Moved(string friendID, string oldCatalogName, string newCatalogName)
        {
            ChatListItem newOwner =null, oldOwner = null;
            ChatListSubItem target =null;
            foreach (ChatListItem tmp in this.chatListBox.Items)
            {
                if (tmp.Text == newCatalogName)
                {
                    newOwner = tmp;             
                }
                if (tmp.Text == oldCatalogName)
                {
                    oldOwner = tmp;
                }
                foreach (ChatListSubItem item in tmp.SubItems)
                {
                    if (item.ID == friendID)
                    {
                        target = item;
                    }
                }
            }
            if (target != null && newOwner != null && oldOwner != null)
            {
                oldOwner.SubItems.Remove(target);
                newOwner.SubItems.Add(target);
                target.OwnerListItem = newOwner;
                newOwner.SubItems.Sort(this);
                this.chatListBox.Refresh();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.ViewInfoClicked != null)
            {
                this.ViewInfoClicked(this.SelectedUnit);
            }
        }

        private void toolStripMenuItem_moveFriend_Click(object sender, EventArgs e)
        {

        }
    }    
}
