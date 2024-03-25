
using System.Collections.Generic;
using ESBasic;
using ESBasic.ObjectManagement.Managers;
using TalkBase;
using TalkBase.Client;
using GGTalk.Linux.Views;
using CPF.Controls;
using GGTalk.Linux.Views;
using TalkBase.NetCore.Client.Core;

namespace GGTalk.Linux.Controller
{
    /// <summary>
    /// 非合并模式的聊天窗口控制器。
    /// </summary>  
    internal abstract class SeperateChatFormController<TUser, TGroup> : IChatFormController, IUserNameGetter
        where TUser : class, TalkBase.IUser, new()
        where TGroup : TalkBase.IGroup, new()
    {       
        private ObjectManager<string, Window> chatFormManager = new ObjectManager<string, Window>();
        private Window notifyForm;
        private Window controlForm;
        private Window fileAssistantForm;
        protected ResourceCenter<TUser, TGroup> resourceCenter;
        public event CbGeneric<Window, IUnit> FormCreated;         

        public SeperateChatFormController(ResourceCenter<TUser, TGroup> center)
        {
            this.resourceCenter = center;           
        }

        protected abstract Window NewFriendChatForm(string friendID);
        protected abstract Window NewFileAssistantForm();
        protected abstract Window NewGroupChatForm(string groupID);
         protected abstract Window NewNotifyForm();

        protected abstract Window NewControlForm();

        protected abstract Window NewAddFriendForm(string unitID);

        protected abstract Window NewGroupVideoCallForm(string videoGroupID,string requestorID, List<string> memberIDList);

        protected abstract Window NewGroupVideoChatForm(string videoGroupID);
        public ChatFormMode ChatFormMode
        {
            get
            {
                return ChatFormMode.Seperated;
            }
        }

        public void FocusOnForm(string unitID, bool createNew)
        {
            Window form =createNew ? this.GetForm(unitID) : this.GetExistedForm(unitID);
            if (form != null)
            {
                form.Focus();
            }            
        }       

        public void CloseForm(string unitID)
        {
            Window form = this.chatFormManager.Get(unitID);
            if (form != null)
            {
                form.Close();
            }            
        }

        public void OnNewMessage(string unitID)
        {
           
        }

        public Window GetExistedForm(string unitID)
        {
            return this.chatFormManager.Get(unitID);
        }

        public List<Window> GetAllForms()
        {
            return this.chatFormManager.GetAll();
        }

        public Window GetFileAssistantForm()
        {
            if(this.fileAssistantForm != null)
            {
                return this.fileAssistantForm;
            }

            this.fileAssistantForm = this.NewFileAssistantForm();
            
            this.fileAssistantForm.Closing += FileAssistantForm_FormClosing;
            this.fileAssistantForm.Show_Topmost();
            return this.fileAssistantForm;
        }


        private void FileAssistantForm_FormClosing(object sender, ClosingEventArgs e)
        {
            this.fileAssistantForm = null;
        }

        public Window GetForm(string unitID,bool show = true)
        {
            if (this.chatFormManager.Contains(unitID))
            {
                return this.chatFormManager.Get(unitID);
            }

            IUnit unit = this.resourceCenter.ClientGlobalCache.GetUnit(unitID);  
            Window form = null;

            if (unit.UnitType == UnitType.User)
            {
                //this.resourceCenter.RapidPassiveEngine.P2PController.P2PConnectAsyn(unitID);//尝试P2P连接。
                form = this.NewFriendChatForm(unitID);
            }
            else if (unit.UnitType == UnitType.Group)
            {
                form = this.NewGroupChatForm(unitID);
            }
            form.Tag = unit;
            this.chatFormManager.Add(unit.ID, form);
            
            form.Closing += Form_Closing;
            if (show)
            {
                form.Show_Topmost();
            }
            
            if (this.FormCreated != null)
            {
                this.FormCreated(form ,unit);
            }
            TwinkleNotifyIcon twinkleNotifyIcon = MainWindow.MessageCacheManager as TwinkleNotifyIcon;
            if (twinkleNotifyIcon != null)
            {
                twinkleNotifyIcon.PickUnReadMSgForUnitID(unit.ID);
            }

            return form;
        }

        private void Form_Closing(object sender, ClosingEventArgs e)
        {
            IChatForm<TUser> form = (IChatForm<TUser>)sender;
            this.chatFormManager.Remove(form.UnitID);
        }

        void groupChatForm_GroupMemberClicked(string unitID)
        {
            this.FocusOnForm(unitID, true);
        }    

        public void CloseAllForms()
        {
            foreach (Window form in this.chatFormManager.GetAll())
            {
                form.Close();
            }
        }

        public Window GetChatRecordForm(string unitID)
        {
            IUnit unit = this.resourceCenter.ClientGlobalCache.GetUnit(unitID);
            if (unit == null)
            {
                return null;
            }
        
            ChatRecordWindow form = null;
            if (unit.IsUser)
            {
                TUser friend = this.resourceCenter.ClientGlobalCache.GetUser(unitID);
                form = new ChatRecordWindow(friend, this);
            }
            else
            {
                TGroup group = this.resourceCenter.ClientGlobalCache.GetGroup(unitID);
                form = new ChatRecordWindow(group, this);
            }
            return form;

        }

        public string GetUserName(string userID)
        {
            IUnit unit = this.resourceCenter.ClientGlobalCache.GetUser(userID);
            if (unit == null)
            {
                return "";
            }
            return unit.Name;
        }

        public Window GetControlForm()
        {
            if (this.controlForm != null)
            {
                return this.controlForm;
            }
            this.controlForm = this.NewControlForm();
            this.controlForm.Closing += ControlForm_FormClosing;
          //  ESBasic.Helpers.WindowsHelper.SetForegroundWindow(this.controlForm);
            return this.controlForm;
        }

        private void ControlForm_FormClosing(object sender, ClosingEventArgs e)
        {
            this.controlForm = null;
        }

        public Window GetNotifyForm()
        {
            if (this.notifyForm != null)
            {
                return this.notifyForm;
            }

            this.notifyForm = this.NewNotifyForm();
            this.notifyForm.Closing += NotifyForm_FormClosing;
            this.notifyForm.Show_Topmost();
          //  ESBasic.Helpers.WindowsHelper.SetForegroundWindow(this.notifyForm);
            return this.notifyForm;
        }      

        private void NotifyForm_FormClosing(object sender, ClosingEventArgs e)
        {
            this.notifyForm = null;
        }

        public bool IsExistNotifyForm()
        {
            return this.notifyForm != null;
        }

        private Window addFriendForm;
        public Window GetAddFriendForm(string unitID)
        {
            if (this.addFriendForm != null)
            {
                return this.addFriendForm;
            }

            this.addFriendForm = this.NewAddFriendForm(unitID);
            this.addFriendForm.Closing += AddFriendForm_FormClosing;
         //   ESBasic.Helpers.WindowsHelper.SetForegroundWindow(this.addFriendForm);
            return this.addFriendForm;
        }

        private void AddFriendForm_FormClosing(object sender, ClosingEventArgs e)
        {
            this.addFriendForm = null;
        }

        private Window groupVideoCallForm;
        public Window GetNewGroupVideoCallForm(string videoGroupID, string requestorID, List<string> memberIDList)
        {
            groupVideoCallForm = this.NewGroupVideoCallForm(videoGroupID, requestorID, memberIDList);
            groupVideoCallForm.Tag = videoGroupID;
            this.groupVideoCallForm.Closing += GroupVideoCallForm_FormClosing;
            return groupVideoCallForm;
        }

        private void GroupVideoCallForm_FormClosing(object sender, ClosingEventArgs e)
        {
            this.groupVideoCallForm = null;
        }

        public bool ExistGroupVideoCallForm()
        {
            return this.groupVideoCallForm != null;
        }

        public Window GetGroupVideoCallForm(string videoGroupID)
        {
            if (groupVideoCallForm == null)
            {
                return null;
            }
            if (groupVideoCallForm.Tag.ToString() == videoGroupID)
            {
                return this.groupVideoCallForm;
            }
            return null;
        }

        private Window groupVideoChatForm;
        public Window GetNewGroupVideoChatForm(string videoGroupID)
        {
            groupVideoChatForm = this.NewGroupVideoChatForm(videoGroupID);
            groupVideoChatForm.Tag = videoGroupID;
            this.groupVideoChatForm.Closing += GroupVideoChatForm_FormClosing;
            return groupVideoChatForm;
        }

        private void GroupVideoChatForm_FormClosing(object sender, ClosingEventArgs e)
        {
            this.groupVideoChatForm = null;
        }

        public bool ExistGroupVideoChatForm()
        {
            return this.groupVideoChatForm != null;
        }

        public Window GetGroupVideoChatForm(string videoGroupID)
        {
            if (groupVideoChatForm == null)
            {
                return null;
            }
            if (groupVideoChatForm.Tag.ToString() == videoGroupID)
            {
                return this.groupVideoChatForm;
            }
            return null;
        }
    }
}
