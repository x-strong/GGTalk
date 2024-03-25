using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OMCS.Boost.Forms;

namespace TalkBase.Client.Application
{
    /// <summary>
    /// 聊天主窗体接口，（语音AudioDialogManager、视频VideoDialogManager、远程桌面RemoteDesktopManager、远程磁盘RemoteDiskManager）管理器通过该接口与对应的聊天窗口互动。
    /// </summary>
    public interface IOwnerForm
    {
        void FlashWindow();
        void AppendSysMessage(string msg);

        void AddDisplayedPanel(string title, Control displayedPanel);
        void RemoveDisplayedPanel(string title);
        Control GetDisplayedPanel(string title);
        bool ContanisDisplayedPanel(string title);

        IVideoChatForm DoCreateVideoChatForm(bool waitingAnswer);
    }
}
