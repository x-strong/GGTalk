using CPF.Controls;
using GGTalk.Linux.Views;
using System;
using System.Collections.Generic;
using System.Text;
using TalkBase.Client;

namespace GGTalk.Linux
{
    internal class CommonBusinessMethod
    {
        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="parentWindow"></param>
        /// <param name="resourceCenter"></param>
        /// <param name="userID"></param>
        internal async static void AddFriend(Window parentWindow, ResourceCenter<GGUser, GGGroup> resourceCenter,string userID)
        {
            GGUser friend = resourceCenter.ClientGlobalCache.GetUser(userID);
            if (friend.ID == resourceCenter.CurrentUserID)
            {
                MessageBoxEx.Show("提示", "不能添加自己为好友！");
                return;
            }
            bool isFriend = resourceCenter.ClientGlobalCache.CurrentUser.GetAllFriendList().Contains(friend.ID);
            if (!isFriend)
            {
                //todo: 查询是否在黑名单内
                bool isFriendBlackMe = resourceCenter.ClientOutter.IsInHisBlackList(friend.ID);
                if (isFriendBlackMe)
                {
                    MessageBoxEx.Show("提示", "对方拒绝添加您为好友！");
                    return;
                }
                bool notExist = resourceCenter.ClientGlobalCache.PrepairUnit(friend);
            
                AddFriendWindow addFriendForm = new AddFriendWindow(friend.ID);
                System.Threading.Tasks.Task<object> task = addFriendForm.ShowDialog_Topmost(parentWindow);
                await task.ConfigureAwait(true);
                if (!Convert.ToBoolean(task.Result))
                {
                    if (notExist)
                    {
                        resourceCenter.ClientGlobalCache.CancelPrepairUnit(friend);
                    }
                    return;
                }
                MessageBoxEx.Show("提示", "发送成功");
            }
            else
            {
                if (resourceCenter.ClientGlobalCache.CurrentUser.IsInBlackList(friend.UserID))
                {
                    //TODO  如果是黑名单用户，则显示聊天记录。
                    Window form = MainWindow.ChatFormController.GetChatRecordForm(friend.ID);
                    if (form != null)
                    {
                        form.Show_Topmost();
                    }
                    return;
                }
                ///跳转到聊天界面
                CommonHelper.MoveToChat(friend);
            }
        }

    }
}
