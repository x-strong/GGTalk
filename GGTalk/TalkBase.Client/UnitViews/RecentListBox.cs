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
    /// 最近联系人列表，
    /// </summary>
    public partial class RecentListBox : UserControl
    {
        public event CbGeneric<IUnit> UnitClicked;
        public event CbGeneric<IUnit> UnitDoubleClicked;
        public event CbGeneric<IUnit> ChatRecordClicked;

        private ObjectManager<string, ChatListSubItem> objectManager = new ObjectManager<string, ChatListSubItem>();


        #region Property
        public ContextMenuStrip RecentContextMenuStrip
        {
            get
            {
                return this.skinContextMenuStrip_recent;
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
                if (this.chatListBox.SelectSubItem == null)
                {
                    return null;
                }

                return (IUnit)this.chatListBox.SelectSubItem.Tag;
            }
        }
        #endregion
        #endregion    

        public RecentListBox()
        {
            InitializeComponent();
        }
        
        private IUnitInfoProvider unitInfoProvider;
        public void Initialize(IUnitInfoProvider getter)
        {
            this.unitInfoProvider = getter;
        }

        public void Clear()
        {
            this.chatListBox.Items[0].SubItems.Clear();
            this.objectManager.Clear();            
        }

        public void AddRecentUnit(IUnit unit, int insertIndex, string lastWords)
        {
            UserStatus status = (!unit.IsUser) ? UserStatus.Online : ((IUser)unit).UserStatus;
            Image img = this.unitInfoProvider.GetHeadImage(unit);
            ChatListSubItem subItem = new ChatListSubItem(unit.ID, "", unit.DisplayName, "", this.ConvertUserStatus(status), img);
            subItem.Tag = unit;
            subItem.LastWords = lastWords;
            this.chatListBox.Items[0].SubItems.Insert(insertIndex, subItem);
            this.chatListBox.Invalidate();
            this.objectManager.Add(unit.ID, subItem);
        }

        public void LastWordChanged(IUnit unit ,string lastWords)
        {
            ChatListSubItem[] items = this.chatListBox.GetSubItemsById(unit.ID);
            if (items != null && items.Length > 0)
            {
                ChatListSubItem item = items[0];
                item.LastWords = lastWords;
                item.OwnerListItem.SubItems.Remove(item);
                item.OwnerListItem.SubItems.Insert(0, item);
                
            }
            else
            {
                this.AddRecentUnit(unit, 0, lastWords);
            }
            this.chatListBox.Invalidate();
        }

        public void RemoveUnit(string unitID)
        {
            this.chatListBox.RemoveSubItemsById(unitID);
            this.chatListBox.Invalidate();
            this.objectManager.Remove(unitID);
        }
        
        public void UnitInfoChanged(IUser user)
        {
            ChatListSubItem[] items = this.chatListBox.GetSubItemsById(user.ID);
            if (items == null || items.Length == 0)
            {
                return;
            }
            
            items[0].DisplayName = user.DisplayName;
            items[0].HeadImage = this.unitInfoProvider.GetHeadImage(user);
            items[0].Status = this.ConvertUserStatus(user.UserStatus);           
            this.chatListBox.Invalidate();
        }

        public bool ContainsUnit(string unitID)
        {
            return this.objectManager.Contains(unitID);
        }

        public int GetUnitCount()
        {
            return this.objectManager.Count;
        }

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

        public List<string> GetRecentUserList(int maxCount)
        {
            List<string> recentList = new List<string>();
            int count = 0;
            foreach (ChatListSubItem item in this.chatListBox.Items[0].SubItems)
            {
                recentList.Add(item.ID);               
                ++count;
                if (count >= maxCount)
                {
                    break;
                }
            }
            return recentList;
        }

        public void SetTwinkleState(string id,bool twinkle)
        {
            ChatListSubItem[] items = this.chatListBox.GetSubItemsById(id);
            if (items == null || items.Length == 0)
            {
                return;
            }
            items[0].IsTwinkle = twinkle;
        }

        public void SetAllUserOffline()
        {
            foreach (ChatListItem item in this.chatListBox.Items)
            {
                foreach (ChatListSubItem sub in item.SubItems)
                {
                    IUnit unit = (IUnit)sub.Tag;
                    if (unit.IsUser)
                    {
                        sub.Status = ChatListSubItem.UserStatus.OffLine;
                    }
                }
            }

            this.chatListBox.Invalidate();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ChatListSubItem item = this.chatListBox.SelectSubItem;          
            item.IsTwinkle = false;
            IUnit unit = (IUnit)item.Tag;
            if (this.UnitDoubleClicked != null)
            {
                this.UnitDoubleClicked(unit);
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ChatListSubItem item = this.chatListBox.SelectSubItem;
            IUnit unit = (IUnit)item.Tag;
            if (this.ChatRecordClicked != null)
            {
                this.ChatRecordClicked(unit);
            }
        }

        private void 从列表中移除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.chatListBox.Items[0].SubItems.Remove(this.chatListBox.SelectSubItem);
        }

        private void chatListBox_DoubleClickSubItem(object sender, ChatListEventArgs e)
        {
            IUnit unit = (IUnit)e.SelectSubItem.Tag;

            if (this.UnitDoubleClicked != null)
            {
                this.UnitDoubleClicked(unit);
            }
        }        

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

        private void chatListBox_ClickSubItem(object sender, ChatListSubItem e)
        {
            if (this.UnitClicked != null)
            {
                this.UnitClicked((IUnit)e.Tag);
            }
        }

        /// <summary>
        /// 选中子节点
        /// </summary>
        /// <param name="unitID"></param>
        public void SelectSubItem(string unitID)
        {
            ChatListSubItem chatListSubItem = this.objectManager.Get(unitID);
            this.chatListBox.SelectSubItem = chatListSubItem;
        }
    }    
}
