
using System.Collections.Generic;
using TalkBase.Client;
using GGTalk.Linux.Views;
using GGTalk.Linux.Controller;
using CPF.Controls;
using GGTalk.Linux.Views;

namespace GGTalk.Linux
{
    /// <summary>
    /// 非合并模式的聊天窗口控制器。
    /// </summary>
    internal class SeperateController : SeperateChatFormController<GGUser, GGGroup>
    {
        public SeperateController(ResourceCenter<GGUser, GGGroup> center) :base(center) 
        {
                     
        }           

        protected override Window NewFriendChatForm(string friendID)
        {
            return new FriendChatWindow(friendID);
        }

        protected override Window NewGroupChatForm(string groupID)
        {
            GroupChatWindow groupChatForm = new GroupChatWindow(groupID);
            return groupChatForm;
        }

        void groupChatForm_GroupMemberClicked(string unitID)
        {
            this.FocusOnForm(unitID, true);
        }

        protected override Window NewFileAssistantForm()
        {
            FileAssistantWindow form = new FileAssistantWindow();
            return form;
        }

        protected override Window NewNotifyForm()
        {
            NotifyWindow form = new NotifyWindow();
            return form;
        }

        protected override Window NewControlForm()
        {
            //ControlMainWindow.ChatFormController form = new ControlMainWindow.ChatFormController(this.resourceCenter);
            //return form;
            return null;
        }

        protected override Window NewAddFriendForm(string unitID)
        {
            AddFriendWindow addFriendForm = new AddFriendWindow(unitID);
            return addFriendForm;
        }

        protected override Window NewGroupVideoCallForm(string videoGroupID, string requestorID, List<string> memberIDList)
        {
            //GGUser ggUser = this.resourceCenter.ClientGlobalCache.GetUser(requestorID);
            //return new GroupVideoCallWindow(this.resourceCenter, videoGroupID, ggUser, memberIDList);
            return null;
        }

        protected override Window NewGroupVideoChatForm(string videoGroupID)
        {
            //return new GroupVideoChatWindow(this.resourceCenter, videoGroupID);
            return null;
        }
    }
}
