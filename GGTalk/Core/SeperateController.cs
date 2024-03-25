using System;
using System.Collections.Generic;
using System.Text;
using TalkBase.Client;
using System.Windows.Forms;
using ESBasic;
using ESBasic.ObjectManagement.Managers;
using CCWin;
using System.Drawing;
using TalkBase.Client.Application;
using TalkBase.Client.Bridges;
using TalkBase;

namespace GGTalk
{
    /// <summary>
    /// 非合并模式的聊天窗口控制器。
    /// </summary>
    public class SeperateController : SeperateChatFormController<GGUser, GGGroup>
    {
        public SeperateController(ResourceCenter<GGUser, GGGroup> center, IChatFormShower formShower) :base(center, formShower) 
        {
                     
        }           

        protected override IChatForm NewFriendChatForm(string friendID)
        {
            return new FriendChatForm(base.resourceCenter, friendID);
        }

        protected override IChatForm NewGroupChatForm(string groupID)
        {
            GroupChatForm groupChatForm = new GroupChatForm(this.resourceCenter, groupID);
            groupChatForm.GroupMemberClicked += new CbGeneric<string>(groupChatForm_GroupMemberClicked);
            return groupChatForm;
        }

        void groupChatForm_GroupMemberClicked(string unitID)
        {
            this.FocusOnForm(unitID, true);
        }

        protected override IChatForm NewFileAssistantForm()
        {
            FileAssistantForm form = new FileAssistantForm(this.resourceCenter);
            return form;
        }

        protected override Form NewNotifyForm()
        {
            NotifyForm form = new NotifyForm(this.resourceCenter);
            return form;
        }

        protected override Form NewControlForm()
        {
            ControlMainForm form = new ControlMainForm(this.resourceCenter);
            return form;
        }

        protected override Form NewAddFriendForm(string unitID)
        {
            AddFriendForm addFriendForm = new AddFriendForm(this.resourceCenter,unitID);
            return addFriendForm;
        }

        protected override Form NewGroupVideoCallForm(string videoGroupID, string requestorID, List<string> memberIDList)
        {
            GGUser ggUser = this.resourceCenter.ClientGlobalCache.GetUser(requestorID);
            return new GroupVideoCallForm(this.resourceCenter, videoGroupID, ggUser, memberIDList);
        }

        protected override Form NewGroupVideoChatForm(string videoGroupID)
        {
            return new GroupVideoChatForm(this.resourceCenter, videoGroupID);
        }
    }
}
