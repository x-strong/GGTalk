using System;
using System.Collections.Generic;
using System.Text;
using TalkBase.Client.UnitViews;
using ESBasic;
using System.Windows.Forms;

namespace TalkBase.Client.Bridges
{
    /// <summary>
    /// 将unit列表控件（FriendListBox，GroupListBox，RecentListBox）的事件传递给 （ResourceCenter ,IChatFormController）。
    /// 注：除了FriendListBox的AddCatalogClicked、ChangeCatalogNameClicked事件外，其它的事件都已传递。
    /// </summary>
    internal class ChatList2ChatFormBridge<TUser, TGroup>
        where TUser : class, TalkBase.IUser, new()
        where TGroup : TalkBase.IGroup, new()       
    {
        private ResourceCenter<TUser, TGroup> resourceCenter;       
        private UnitListBox friendListBox;
        private GroupListBox groupListBox;
        private RecentListBox recentListBox;
        private IChatFormShower formShower;

        /// <summary>
        /// 连接可选的unit列表控件。
        /// </summary>        
        /// <param name="friend">好友列表控件。允许为null</param>
        /// <param name="group">群组列表控件。允许为null</param>
        /// <param name="recent">最近联系人列表控件。允许为null</param>
         public ChatList2ChatFormBridge(ResourceCenter<TUser, TGroup> center, UnitListBox friend, GroupListBox group, RecentListBox recent, IChatFormShower _formShower)
        {
            this.resourceCenter = center;                 
            this.friendListBox = friend;
            this.groupListBox = group;
            this.recentListBox = recent;
            this.formShower = _formShower;
            if (this.friendListBox != null)
            {              
                this.friendListBox.UnitDoubleClicked += new CbGeneric<IUnit>(friendListBox1_UserDoubleClicked);
                this.friendListBox.RemoveUnitClicked += new CbGeneric<IUnit>(friendListBox1_RemoveUserClicked);
                this.friendListBox.ChatRecordClicked += new CbGeneric<IUnit>(friendListBox1_ChatRecordClicked);
                this.friendListBox.CatalogAdded += new CbGeneric<string>(friendListBox1_CatalogAdded);
                this.friendListBox.CatalogNameChanged += new CbGeneric<string, string, bool>(friendListBox1_CatalogNameChanged);
                this.friendListBox.CatalogRemoved += new CbGeneric<string>(friendListBox1_CatalogRemoved);
                this.friendListBox.UnitCatalogMoved += new CbGeneric<string, string, string>(friendListBox1_FriendCatalogMoved);               
            }
            if (this.recentListBox != null)
            {
                this.recentListBox.UnitDoubleClicked += new CbGeneric<IUnit>(recentListBox1_UserDoubleClicked);
                this.recentListBox.ChatRecordClicked += new CbGeneric<IUnit>(recentListBox1_ChatRecordClicked);
            }
            if (this.groupListBox != null)
            {
                this.groupListBox.GroupDoubleClicked += new CbGeneric<TalkBase.IGroup>(groupListBox_GroupDoubleClicked);
                this.groupListBox.DismissGroupClicked += new CbGeneric<TalkBase.IGroup>(groupListBox_DismissGroupClicked);
                this.groupListBox.QuitGroupClicked += new CbGeneric<TalkBase.IGroup>(groupListBox_QuitGroupClicked);
                this.groupListBox.ChatRecordClicked += new CbGeneric<TalkBase.IGroup>(groupListBox_ChatRecordClicked);
            }
        }

        void groupListBox_ChatRecordClicked(TalkBase.IGroup group)
        {
            Form form = this.resourceCenter.ChatFormController.GetChatRecordForm(group.ID);
            if (form != null)
            {
                form.Show();
            }
        }

        void groupListBox_QuitGroupClicked(TalkBase.IGroup group)
        {
            try
            {
                if (group.CreatorID == this.resourceCenter.CurrentUserID)
                {
                    MessageBox.Show("创始人不能退出。");
                    return;
                }

                if (!ESBasic.Helpers.WindowsHelper.ShowQuery(string.Format("您确定要退出{0}({1})吗？", group.ID, group.Name)))
                {
                    return;
                }
                
                this.resourceCenter.ClientOutter.QuitGroup(group.ID);
                this.groupListBox.RemoveGroup(group.ID);
                this.recentListBox.RemoveUnit(group.ID);
                this.resourceCenter.ChatFormController.CloseForm(group.ID);
                MessageBox.Show(string.Format("您已经退出{0}({1})。", group.ID, group.Name));
            }
            catch (Exception ee)
            {
                MessageBox.Show("请求超时！" + ee.Message);
            }
        }

        void groupListBox_DismissGroupClicked(TalkBase.IGroup group)
        {
            try
            {
                if (group.CreatorID != this.resourceCenter.CurrentUserID)
                {
                    MessageBox.Show("只有创始人才能解散。");                   
                    return;
                }

                if (!ESBasic.Helpers.WindowsHelper.ShowQuery(string.Format("您确定要解散{0}({1})吗？", group.ID, group.Name)))
                {
                    return;
                }

                this.resourceCenter.ClientOutter.DeleteGroup(group.ID);
                this.groupListBox.RemoveGroup(group.ID);
                this.recentListBox.RemoveUnit(group.ID);
                this.resourceCenter.ChatFormController.CloseForm(group.ID);
                MessageBox.Show(string.Format("您已经解散{0}({1})。", group.ID, group.Name));
            }
            catch (Exception ee)
            {
                MessageBox.Show("请求超时！" + ee.Message);
            }
        }

        void groupListBox_GroupDoubleClicked(TalkBase.IGroup group)
        {
            try
            {
                this.resourceCenter.ChatFormController.FocusOnForm(group.ID, true);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        void recentListBox1_ChatRecordClicked(IUnit unit)
        {
            Form form = this.resourceCenter.ChatFormController.GetChatRecordForm(unit.ID);
            if (form != null)
            {
                form.Show();
            }
        }

        void recentListBox1_UserDoubleClicked(IUnit unit)
        {
            if (unit.ID == this.resourceCenter.CurrentUserID)
            {
                IChatForm form = this.resourceCenter.ChatFormController.GetFileAssistantForm();
                this.formShower.ShowChatForm(form);
                return;
            }
            this.resourceCenter.ChatFormController.FocusOnForm(unit.ID,true);
        }

        void friendListBox1_FriendCatalogMoved(string friendID, string oldCatalog, string newCatalog)
        {
            this.resourceCenter.ClientOutter.MoveFriendCatalog(friendID, oldCatalog, newCatalog);
            if (newCatalog == FunctionOptions.BlackListCatalogName)
            {
                if (this.recentListBox != null)
                {
                    this.recentListBox.RemoveUnit(friendID);
                }
            }
        }

        void friendListBox1_CatalogRemoved(string catalog)
        {
            this.resourceCenter.ClientOutter.RemoveFriendCatalog(catalog);
        }

        void friendListBox1_CatalogNameChanged(string oldName, string newName, bool isMerge)
        {
            this.resourceCenter.ClientOutter.ChangeFriendCatalogName(oldName, newName, isMerge);
        }

        void friendListBox1_CatalogAdded(string catalog)
        {
            this.resourceCenter.ClientOutter.AddFriendCatalog(catalog);
        }

        void friendListBox1_ChatRecordClicked(IUnit unit)
        {
            Form form = this.resourceCenter.ChatFormController.GetChatRecordForm(unit.ID);
            if (form != null)
            {
                form.Show();
            }
        }        

        void friendListBox1_UserDoubleClicked(IUnit unit)
        {
            if (unit.ID == this.resourceCenter.CurrentUserID)
            {
                IChatForm chatForm = this.resourceCenter.ChatFormController.GetFileAssistantForm();
                this.formShower.ShowChatForm(chatForm);
                return;
            }

            //如果是黑名单用户，则显示聊天记录。
            if (unit.IsUser && this.resourceCenter.ClientGlobalCache.CurrentUser.IsInBlackList(unit.ID))
            {
                Form form = this.resourceCenter.ChatFormController.GetChatRecordForm(unit.ID);
                if (form != null)
                {
                    form.Show();
                }
                return;
            }            

            this.resourceCenter.ChatFormController.FocusOnForm(unit.ID, true);            
        }

        void friendListBox1_RemoveUserClicked(IUnit friend)
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            try
            {
                if (friend.ID == this.resourceCenter.RapidPassiveEngine.CurrentUserID)
                {
                    return;
                }

                if (!ESBasic.Helpers.WindowsHelper.ShowQuery(string.Format("您确定要删除好友 {0}({1}) 吗？", friend.Name, friend.ID)))
                {
                    return;
                }

                //SendCertainly 发送请求，并等待Ack回复
                this.resourceCenter.ClientOutter.RemoveFriend(friend.ID);

                if (this.friendListBox != null)
                {
                    this.friendListBox.RemoveUnit(friend.ID);
                }

                this.resourceCenter.ChatFormController.CloseForm(friend.ID);
            }
            catch (Exception ee)
            {
                MessageBox.Show("请求超时！" + ee.Message);
            }
        }
    }
}
