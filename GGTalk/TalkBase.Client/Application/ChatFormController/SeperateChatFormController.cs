using System;
using System.Collections.Generic;
using System.Text;
using TalkBase.Client;
using System.Windows.Forms;
using ESBasic;
using TalkBase.Client.Application;
using ESBasic.ObjectManagement.Managers;
using CCWin;
using System.Drawing;
using TalkBase;
using TalkBase.Client.Bridges;
using GGTalk;

namespace TalkBase.Client.Application
{
    /// <summary>
    /// 非合并模式的聊天窗口控制器。
    /// </summary>  
    public abstract class SeperateChatFormController<TUser, TGroup> : IChatFormController, IUserNameGetter
        where TUser : class, TalkBase.IUser, new()
        where TGroup : TalkBase.IGroup, new()
    {       
        private ObjectManager<string, IChatForm> chatFormManager = new ObjectManager<string, IChatForm>();
        private Form notifyForm;
        private Form controlForm;
        private IChatForm fileAssistantForm;
        protected ResourceCenter<TUser, TGroup> resourceCenter;
        public event CbGeneric<IChatForm, IUnit> FormCreated;
        private IChatFormShower chatFormShower;

        public SeperateChatFormController(ResourceCenter<TUser, TGroup> center, IChatFormShower formShower)
        {
            this.resourceCenter = center;
            this.chatFormShower = formShower;
        }

        protected abstract IChatForm NewFriendChatForm(string friendID);
        protected abstract IChatForm NewFileAssistantForm();
        protected abstract IChatForm NewGroupChatForm(string groupID);

        protected abstract Form NewNotifyForm();

        protected abstract Form NewControlForm();

        protected abstract Form NewAddFriendForm(string unitID);

        protected abstract Form NewGroupVideoCallForm(string videoGroupID,string requestorID, List<string> memberIDList);

        protected abstract Form NewGroupVideoChatForm(string videoGroupID);
        public ChatFormMode ChatFormMode
        {
            get
            {
                return TalkBase.Client.Bridges.ChatFormMode.Seperated;
            }
        }

        public void FocusOnForm(string unitID, bool createNew)
        {
            IChatForm iChatForm =createNew ? this.GetForm(unitID) : this.GetExistedForm(unitID);
            if (iChatForm != null)
            {
                Form form = iChatForm as Form;
                if (form != null)
                    form.Focus();
                else {
                    this.chatFormShower.ShowChatForm(iChatForm);
                }                
            }            
        }       

        public void CloseForm(string unitID)
        {
            IChatForm iChatForm = this.chatFormManager.Get(unitID);
            if (iChatForm != null)
            {
                Form form = iChatForm as Form;
                if (form != null)
                    form.Close();                
            }
        }

        public void RemoveForm4Cache(string unitID)
        {
            this.chatFormManager.Remove(unitID);
        }

        public void OnNewMessage(string unitID)
        {
           
        }

        public IChatForm GetExistedForm(string unitID)
        {
            IChatForm chatForm =  this.chatFormManager.Get(unitID);

            return chatForm;
        }

        public List<IChatForm> GetAllForms()
        {
            return this.chatFormManager.GetAll();
        }

        public IChatForm GetFileAssistantForm()
        {
            if(this.fileAssistantForm != null)
            {
                return this.fileAssistantForm;
            }
            this.fileAssistantForm = this.NewFileAssistantForm();
            if (this.fileAssistantForm is Form)
            {
                Form form = this.fileAssistantForm as Form;
                form.FormClosing += FileAssistantForm_FormClosing;
            }
            this.chatFormShower.ShowChatForm(this.fileAssistantForm);
            return this.fileAssistantForm;
        }

        private void FileAssistantForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.fileAssistantForm = null;
        }

        public IChatForm GetForm(string unitID)
        {
            if (this.chatFormManager.Contains(unitID))
            {
                return this.chatFormManager.Get(unitID);
            }

            IUnit unit = this.resourceCenter.ClientGlobalCache.GetUnit(unitID);
            IChatForm iChatForm = null;

            if (unit.UnitType == UnitType.User)
            {
                //this.resourceCenter.RapidPassiveEngine.P2PController.P2PConnectAsyn(unitID);//尝试P2P连接。
                iChatForm = this.NewFriendChatForm(unitID);
            }
            else if (unit.UnitType == UnitType.Group)
            {
                iChatForm = this.NewGroupChatForm(unitID);
            }

            if (iChatForm != null)
            {
                iChatForm.Tag = unit;
                this.chatFormManager.Add(unit.ID, iChatForm);
                this.chatFormShower.ShowChatForm(iChatForm);

                if (this.FormCreated != null)
                {
                    this.FormCreated(iChatForm, unit);
                }


                MainForm chatFormShower = (MainForm)Program.ChatFormShower;
                chatFormShower.TwinkleNotifyIcon.PickUnReadMSgForUnitID(unit.ID);
            }
            return iChatForm;
        }

        void groupChatForm_GroupMemberClicked(string unitID)
        {
            this.FocusOnForm(unitID, true);
        }

        void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            IChatForm form = (IChatForm)sender;
            this.chatFormManager.Remove(form.UnitID);         
        }

        public void CloseAllForms()
        {
            foreach (IChatForm iChatForm in this.chatFormManager.GetAll())
            {
                Form form = iChatForm as Form;
                if (form != null)
                {
                    form.Close();
                }                 
            }
        }

        public Form GetChatRecordForm(string unitID)
        {
            IUnit unit = this.resourceCenter.ClientGlobalCache.GetUnit(unitID);
            if (unit == null)
            {
                return null;
            }

            ChatRecordForm form = null;
            if (unit.IsUser)
            {
                TUser friend = this.resourceCenter.ClientGlobalCache.GetUser(unitID);
                form = new ChatRecordForm(this.resourceCenter, this.resourceCenter.ClientGlobalCache.CurrentUser.GetIDName(), friend.GetIDName(), TalkClientInitializer.EmotionDictionary);       
            }
            else
            {
                TGroup group = this.resourceCenter.ClientGlobalCache.GetGroup(unitID);
                form = new ChatRecordForm(this.resourceCenter, unit.GetIDName(), this.resourceCenter.ClientGlobalCache.CurrentUser.GetIDName(), this, TalkClientInitializer.EmotionDictionary);
                form.Show();
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

        public Form GetControlForm()
        {
            if (this.controlForm != null)
            {
                return this.controlForm;
            }
            this.controlForm = this.NewControlForm();
            this.controlForm.FormClosing += ControlForm_FormClosing;
            ESBasic.Helpers.WindowsHelper.SetForegroundWindow(this.controlForm);
            return this.controlForm;
        }

        private void ControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.controlForm = null;
        }

        public Form GetNotifyForm()
        {
            if (this.notifyForm != null)
            {
                return this.notifyForm;
            }

            this.notifyForm = this.NewNotifyForm();
            this.notifyForm.FormClosing += NotifyForm_FormClosing;
            this.notifyForm.Show();
            ESBasic.Helpers.WindowsHelper.SetForegroundWindow(this.notifyForm);
            return this.notifyForm;
        }      

        private void NotifyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.notifyForm = null;
        }

        public bool IsExistNotifyForm()
        {
            return this.notifyForm != null;
        }

        private Form addFriendForm;
        public Form GetAddFriendForm(string unitID)
        {
            if (this.addFriendForm != null)
            {
                return this.addFriendForm;
            }

            this.addFriendForm = this.NewAddFriendForm(unitID);
            this.addFriendForm.FormClosing += AddFriendForm_FormClosing;
            ESBasic.Helpers.WindowsHelper.SetForegroundWindow(this.addFriendForm);
            return this.addFriendForm;
        }

        private void AddFriendForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.addFriendForm = null;
        }

        private Form groupVideoCallForm;
        public Form GetNewGroupVideoCallForm(string videoGroupID, string requestorID, List<string> memberIDList)
        {
            groupVideoCallForm = this.NewGroupVideoCallForm(videoGroupID, requestorID, memberIDList);
            groupVideoCallForm.Tag = videoGroupID;
            this.groupVideoCallForm.FormClosing += GroupVideoCallForm_FormClosing;
            return groupVideoCallForm;
        }

        private void GroupVideoCallForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.groupVideoCallForm = null;
        }

        public bool ExistGroupVideoCallForm()
        {
            return this.groupVideoCallForm != null;
        }

        public Form GetGroupVideoCallForm(string videoGroupID)
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

        private Form groupVideoChatForm;
        public Form GetNewGroupVideoChatForm(string videoGroupID)
        {
            groupVideoChatForm = this.NewGroupVideoChatForm(videoGroupID);
            groupVideoChatForm.Tag = videoGroupID;
            this.groupVideoChatForm.FormClosing += GroupVideoChatForm_FormClosing;
            return groupVideoChatForm;
        }

        private void GroupVideoChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.groupVideoChatForm = null;
        }

        public bool ExistGroupVideoChatForm()
        {
            return this.groupVideoChatForm != null;
        }

        public Form GetGroupVideoChatForm(string videoGroupID)
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
