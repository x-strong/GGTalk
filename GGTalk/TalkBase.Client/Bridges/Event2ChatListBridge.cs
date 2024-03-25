using System;
using System.Collections.Generic;
using System.Text;
using TalkBase.Client.UnitViews;
using ESBasic;
using ESBasic.Helpers;

using ESFramework.Boost.Controls;

namespace TalkBase.Client.Bridges
{
    #region ILastWordsComputer
    /// <summary>
    /// 将Unit的LastWordsRecord属性转换成string，给RecentListBox使用。
    /// </summary>
    public interface ILastWordsComputer
    {
        string GetLastWords(IUnit unit);
    }

    /// <summary>
    /// ILastWordsComputer的默认实现。
    /// 其假定LastWordsRecord.ChatContent字段是ChatBoxContent紧凑序列化（使用CompactPropertySerializer）之后的值。
    /// </summary>    
    public class DefaultLastWordsComputer<TUser, TGroup> : ILastWordsComputer
        where TUser : class, TalkBase.IUser, new()
        where TGroup : TalkBase.IGroup, new()
    {
        private ResourceCenter<TUser, TGroup> resourceCenter;
        public DefaultLastWordsComputer(ResourceCenter<TUser, TGroup> center)
        {
            this.resourceCenter = center;
        }

        public string GetLastWords(IUnit unit)
        {
            if (unit == null)
            {
                return null;
            }

            if (unit.LastWordsRecord == null || unit.LastWordsRecord.ChatContent == null)
            {
                return "";
            }

            ChatBoxContent chatBoxContent = ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<ChatBoxContent>(unit.LastWordsRecord.ChatContent, 0);
            string content = chatBoxContent.GetTextWithPicPlaceholder("[图]");
            TalkBase.IUser speaker = this.resourceCenter.ClientGlobalCache.GetUser(unit.LastWordsRecord.SpeakerID);
            string lastWords = null;
            if (unit.IsUser)
            {
                lastWords = string.Format("{0}： {1}", speaker.ID == this.resourceCenter.CurrentUserID ? "我" : "TA", content);                
            }
            else
            {
                lastWords = string.Format("{0}： {1}", speaker.Name, content);
            }
            return lastWords;
        }
    } 
    #endregion

        
    /// <summary>
    /// 将（ClientOutter、ClientGlobalCache、TwinkleNotifyIcon、网络断线事件）传递给unit列表控件（FriendListBox，GroupListBox，RecentListBox）。
    /// </summary>
    internal class Event2ChatListBridge<TUser, TGroup>
        where TUser : class, TalkBase.IUser, new()
        where TGroup : TalkBase.IGroup, new()
    {
        private UiSafeInvoker uiSafeInvoker;
        private ResourceCenter<TUser, TGroup> resourceCenter;        
        private TwinkleNotifyIcon twinkleNotifyIcon;

        private UnitListBox friendListBox;
        private GroupListBox groupListBox;
        private RecentListBox recentListBox;        

        #region LastWordsComputer
        private ILastWordsComputer lastWordsComputer;
        /// <summary>
        /// 将Unit的LastWordsRecord属性转换成string，给RecentListBox使用。初始值为DefaultLastWordsComputer实例。
        /// 如果将其设置为null，则表示不通过Event2ChatListBridge桥接ClientOutter.LastWordsChanged事件与RecentListBox。
        /// </summary>
        public ILastWordsComputer LastWordsComputer
        {
            set { lastWordsComputer = value; }
            get { return lastWordsComputer; }
        } 
        #endregion

        #region Ctor
        /// <summary>
        /// 连接可选的unit列表控件。
        /// </summary>
        /// <param name="notifyIcon">支持闪动的托盘。允许为null</param>
        /// <param name="friend">好友列表控件。允许为null</param>
        /// <param name="group">群组列表控件。允许为null</param>
        /// <param name="recent">最近联系人列表控件。允许为null</param>
        public Event2ChatListBridge(ResourceCenter<TUser, TGroup> center, UiSafeInvoker invoker, TwinkleNotifyIcon notifyIcon, UnitListBox friend, GroupListBox group, RecentListBox recent)
        {
            this.resourceCenter = center;
            this.uiSafeInvoker = invoker;
            this.twinkleNotifyIcon = notifyIcon;
            this.friendListBox = friend;
            this.groupListBox = group;
            this.recentListBox = recent;
            this.lastWordsComputer = new DefaultLastWordsComputer<TUser, TGroup>(this.resourceCenter);

            this.resourceCenter.ClientOutter.LastWordsChanged += new CbGeneric<IUnit>(ClientOutter_LastWordsChanged);
            this.resourceCenter.ClientGlobalCache.UserRegistered += new CbGeneric<TUser>(ClientGlobalCache_SomeoneRegistered);
            this.resourceCenter.ClientGlobalCache.MyBaseInfoChanged += new CbGeneric(ClientGlobalCache_MyInfoChanged);
            this.resourceCenter.ClientGlobalCache.MyStatusChanged += new CbGeneric(ClientGlobalCache_MyStatusChanged);
            this.resourceCenter.ClientGlobalCache.UnitCommentNameChanged += new CbGeneric<IUnit>(ClientGlobalCache_UnitCommentNameChanged);
            this.resourceCenter.ClientGlobalCache.UserBaseInfoChanged += new CbGeneric<TUser>(ClientGlobalCache_UserInfoChanged);
            this.resourceCenter.ClientGlobalCache.UserStatusChanged += new CbGeneric<TUser>(ClientGlobalCache_UserStatusChanged);
            this.resourceCenter.ClientGlobalCache.FriendRemoved += new CbGeneric<string>(ClientGlobalCache_FriendRemoved);
            this.resourceCenter.ClientGlobalCache.UserDeleted += new CbGeneric<TUser>(ClientGlobalCache_UserDeleted);
            this.resourceCenter.ClientGlobalCache.AddFriendSucceed += new CbGeneric<TUser>(ClientGlobalCache_AddFriendSucceed);
            this.resourceCenter.ClientGlobalCache.FriendAdded += new CbGeneric<TUser>(ClientGlobalCache_FriendAdded);
            this.resourceCenter.ClientGlobalCache.FriendCatalogChanged += new CbGeneric<string, string, string>(ClientGlobalCache_FriendCatalogChanged);
            this.resourceCenter.ClientGlobalCache.AddFriendCatalog += ClientGlobalCache_AddFriendCatalog;
            this.resourceCenter.ClientGlobalCache.GroupChanged += new CbGeneric<TGroup, TalkBase.GroupChangedType, string ,string>(ClientGlobalCache_GroupChanged);

            this.resourceCenter.ClientGlobalCache.BatchLoadStarted += new CbGeneric(ClientGlobalCache_BatchLoadStarted);
            this.resourceCenter.ClientGlobalCache.BatchLoadCompleted += new CbGeneric(ClientGlobalCache_BatchLoadCompleted);
            this.resourceCenter.RapidPassiveEngine.ConnectionInterrupted += new CbGeneric(rapidPassiveEngine_ConnectionInterrupted);

            if (this.twinkleNotifyIcon != null)
            {
                this.twinkleNotifyIcon.UnhandleMessageOccured += new CbGeneric<string,UnitType>(twinkleNotifyIcon_UnhandleMessageOccured);
                this.twinkleNotifyIcon.UnhandleMessagePickedOut += new CbGeneric<string, UnitType>(twinkleNotifyIcon_UnhandleMessagePickedOut);
            }
        }

        private void ClientGlobalCache_AddFriendCatalog(string catalogName)
        {
            this.uiSafeInvoker.ActionOnUI<string>(this.do_ClientGlobalCache_AddFriendCatalog, catalogName);
        }

        private void do_ClientGlobalCache_AddFriendCatalog(string catalogName)
        {
            this.friendListBox.AddCatalog(catalogName, true);
        }

        private void ClientGlobalCache_FriendCatalogChanged(string friendID, string oldCatalogName, string newCatalogName)
        {
            this.uiSafeInvoker.ActionOnUI<string,string,string>(this.do_ClientGlobalCache_FriendCatalogChanged, friendID,oldCatalogName,newCatalogName);
        }

        private void do_ClientGlobalCache_FriendCatalogChanged(string friendID, string oldCatalogName, string newCatalogName)
        {
            this.friendListBox.Catalog4Moved(friendID, oldCatalogName, newCatalogName);
        }

        void ClientGlobalCache_BatchLoadCompleted()
        {
            this.uiSafeInvoker.ActionOnUI(this.do_ClientGlobalCache_BatchLoadCompleted);
        }

        void do_ClientGlobalCache_BatchLoadCompleted()
        {

        }

        void ClientGlobalCache_BatchLoadStarted()
        {
        }
        void ClientGlobalCache_UserDeleted(TUser user)
        {
            this.uiSafeInvoker.ActionOnUI<TUser>(this.do_ClientGlobalCache_UserDeleted, user);
        }

        void do_ClientGlobalCache_UserDeleted(TUser user)
        {
            if (this.friendListBox != null)
            {
                this.friendListBox.RemoveUnit(user.ID);
            }

            if (this.recentListBox != null)
            {
                this.recentListBox.RemoveUnit(user.ID);

            }
        }

        void ClientOutter_LastWordsChanged(IUnit unit)
        {
            this.uiSafeInvoker.ActionOnUI<IUnit>(this.do_ClientOutter_LastWordsChanged ,unit);
        }

        void do_ClientOutter_LastWordsChanged(IUnit unit)
        {
            if (this.recentListBox != null)
            {
                if (this.lastWordsComputer != null)
                {
                    string lastWords = this.lastWordsComputer.GetLastWords(unit);
                    this.recentListBox.LastWordChanged(unit, lastWords);
                }
            }
        }

        void ClientGlobalCache_MyStatusChanged()
        {
            this.uiSafeInvoker.ActionOnUI(this.do_ClientGlobalCache_MyStatusChanged);
        }

        void do_ClientGlobalCache_MyStatusChanged()
        {
            if (this.friendListBox != null)
            {
                this.friendListBox.UserStatusChanged(this.resourceCenter.ClientGlobalCache.CurrentUser);
            }           
        }

        void ClientGlobalCache_MyInfoChanged()
        {
            this.uiSafeInvoker.ActionOnUI(this.do_ClientGlobalCache_MyInfoChanged);
        }

        void do_ClientGlobalCache_MyInfoChanged()
        {
            if (this.friendListBox != null)
            {
                this.friendListBox.UnitInfoChanged(this.resourceCenter.ClientGlobalCache.CurrentUser);
            }            
        }

        void ClientGlobalCache_SomeoneRegistered(TUser user)
        {
            this.uiSafeInvoker.ActionOnUI<TUser>(this.do_ClientGlobalCache_SomeoneRegistered, user);
        }

        void do_ClientGlobalCache_SomeoneRegistered(TUser user)
        {
        }

        #region ClientGlobalCache_AddFriendSucceed
        void ClientGlobalCache_AddFriendSucceed(TUser friend)
        {
            this.uiSafeInvoker.ActionOnUI<TUser>(this.do_ClientGlobalCache_AddFriendSucceed, friend);
        }

        void do_ClientGlobalCache_AddFriendSucceed(TUser friend)
        {
            if (this.friendListBox != null)
            {                
                this.friendListBox.AddUnit(friend);
            }            
        } 
        #endregion

        #region ClientGlobalCache_UnitCommentNameChanged
        void ClientGlobalCache_UnitCommentNameChanged(IUnit unit)
        {
            this.uiSafeInvoker.ActionOnUI<IUnit>(this.do_ClientGlobalCache_UnitCommentNameChanged, unit);
        }

        void do_ClientGlobalCache_UnitCommentNameChanged(IUnit unit)
        {
            if (unit.UnitType == UnitType.Group)
            {
                this.groupListBox.GroupCommentNameChanged((TalkBase.IGroup)unit);
                return;
            }

            if (this.friendListBox != null)
            {                
                this.friendListBox.UnitCommentNameChanged(unit);
            }
            if (this.recentListBox != null)
            {
                this.recentListBox.UnitCommentNameChanged(unit);
            }
        } 
        #endregion

        #region rapidPassiveEngine_ConnectionInterrupted
        void rapidPassiveEngine_ConnectionInterrupted()
        {
            this.uiSafeInvoker.ActionOnUI(this.do_rapidPassiveEngine_ConnectionInterrupted);
        }

        void do_rapidPassiveEngine_ConnectionInterrupted()
        {
            if (this.friendListBox != null)
            {
                this.friendListBox.SetAllUnitOffline();
            }
            if (this.recentListBox != null)
            {
                this.recentListBox.SetAllUserOffline();
            }
        } 
        #endregion

        #region ClientGlobalCache_UserInfoChanged
        void ClientGlobalCache_UserInfoChanged(TUser user)
        {
            this.uiSafeInvoker.ActionOnUI<TUser>(this.do_ClientGlobalCache_UserInfoChanged, user);
        }

        void do_ClientGlobalCache_UserInfoChanged(TUser user)
        {
            if (this.friendListBox != null)
            {
                this.friendListBox.UnitInfoChanged(user);
            }
            if (this.recentListBox != null)
            {
                this.recentListBox.UnitInfoChanged(user);
            }
        } 
        #endregion

        #region ClientGlobalCache_GroupChanged
        void ClientGlobalCache_GroupChanged(TGroup group, GroupChangedType groupChangedType, string operatorID, string userID)
        {
            this.uiSafeInvoker.ActionOnUI<TGroup, GroupChangedType, string ,string>(this.do_ClientGlobalCache_GroupChanged, group, groupChangedType, operatorID ,userID);
        }

        void do_ClientGlobalCache_GroupChanged(TGroup group, GroupChangedType type, string operatorID, string userID)
        {
            if (this.groupListBox != null)
            {
                if (type == GroupChangedType.MyselfBePulledIntoGroup)
                {
                    this.groupListBox.AddGroup(group);
                }
                else if (type == GroupChangedType.MyselfBeRemovedFromGroup || type == GroupChangedType.GroupDeleted)
                {
                    this.groupListBox.RemoveGroup(group.ID);
                }
                else if (type == GroupChangedType.GroupInfoChanged || type == GroupChangedType.OtherBePulledIntoGroup || type == GroupChangedType.OtherBeRemovedFromGroup 
                      || type == GroupChangedType.SomeoneDeleted || type == GroupChangedType.SomeoneJoin || type == GroupChangedType.SomeoneQuit)
                {
                    this.groupListBox.OnGroupInfoChanged(group);
                }
                else 
                {                   
                }                
            }
        } 
        #endregion

        #region ClientGlobalCache_FriendAdded
        void ClientGlobalCache_FriendAdded(TUser friend)
        {
            this.uiSafeInvoker.ActionOnUI<TUser>(this.do_ClientGlobalCache_FriendAdded, friend);
        }

        void do_ClientGlobalCache_FriendAdded(TUser friend)
        {
            if (this.friendListBox != null)
            {              
                this.friendListBox.AddUnit(friend);
            }
        } 
        #endregion

        #region ClientGlobalCache_FriendRemoved
        void ClientGlobalCache_FriendRemoved(string friendID)
        {
            this.uiSafeInvoker.ActionOnUI<string>(this.do_ClientGlobalCache_FriendRemoved, friendID);
        }

        private void do_ClientGlobalCache_FriendRemoved(string friendID)
        {
            if (this.friendListBox != null)
            {
                this.friendListBox.RemoveUnit(friendID);
            }
            if (this.recentListBox != null)
            {
                this.recentListBox.RemoveUnit(friendID);
            }
        } 
        #endregion

        #region ClientGlobalCache_UserStatusChanged
        void ClientGlobalCache_UserStatusChanged(TUser user)
        {
            this.uiSafeInvoker.ActionOnUI<TUser>(this.do_ClientGlobalCache_UserStatusChanged, user);
        }

        private void do_ClientGlobalCache_UserStatusChanged(TUser friend)
        {
            if (this.friendListBox != null)
            {
                this.friendListBox.UserStatusChanged(friend);
            }
            if (this.recentListBox != null)
            {
                this.recentListBox.UnitInfoChanged(friend);
            }
        } 
        #endregion

        #region twinkleNotifyIcon_UnhandleMessagePickedOut、twinkleNotifyIcon_UnhandleMessageOccured
        void twinkleNotifyIcon_UnhandleMessagePickedOut(string unitID ,UnitType unitType)
        {
            if (unitType == UnitType.Group)
            {
                if (this.groupListBox != null)
                {
                    this.groupListBox.SetTwinkleState(unitID, false);
                }
            }
            else if (unitType == UnitType.User)
            {
                if (this.friendListBox != null)
                {
                    this.friendListBox.SetTwinkleState(unitID, false);
                }
            }

            if (this.recentListBox != null)
            {
                this.recentListBox.SetTwinkleState(unitID, false);
            }
        }

        void twinkleNotifyIcon_UnhandleMessageOccured(string unitID, UnitType unitType)
        {
            if (unitType == UnitType.Group)
            {
                if (this.groupListBox != null)
                {
                    this.groupListBox.SetTwinkleState(unitID, true);
                }
            }
            else if (unitType == UnitType.User)
            {
                if (this.friendListBox != null)
                {
                    this.friendListBox.SetTwinkleState(unitID, true);
                }
            }

            if (this.recentListBox != null)
            {
                this.recentListBox.SetTwinkleState(unitID, true);
            }
        } 
        #endregion
        #endregion
    }
}
