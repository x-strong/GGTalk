using System;
using System.Collections.Generic;
using System.Text;
using TalkBase.Client;
using OMCS.Passive;
using System.Windows.Forms;
using TalkBase.Client.Application;
using TalkBase;
using ESBasic;
using System.Drawing;
using ESFramework.Boost.Controls;
using ESFramework.Boost.NetworkDisk.Passive;

namespace TalkBase.Client.Application
{
    /// <summary>
    /// 与好友的远程磁盘过程管理器。
    /// </summary>    
    public class RemoteDiskManager<TUser, TGroup>
        where TUser : class, TalkBase.IUser, new()
        where TGroup : TalkBase.IGroup, new()
    {
        private ResourceCenter<TUser, TGroup> resourceCenter;
        private TUser currentFriend;   
        private IOwnerForm ownerForm;

        private RemoteDiskRequestPanel remoteDiskRequestPanel = new RemoteDiskRequestPanel();
        private RemoteDiskHandlePanel remoteDiskHandlePanel = new RemoteDiskHandlePanel();
        private RemoteDiskForm remoteDiskForm = null;
        private string Title_Disk = "远程磁盘";

        public RemoteDiskManager(ResourceCenter<TUser, TGroup> center, IOwnerForm owner, TUser user)
        {
            this.resourceCenter = center;
            this.ownerForm = owner;            
            this.currentFriend = user;

            this.remoteDiskRequestPanel.RemoteRequestAnswerd += new CbGeneric<bool>(remoteDiskRequestPanel_RemoteRequestAnswerd);
            this.remoteDiskHandlePanel.RemoteDiskTerminated += new CbGeneric(remoteDiskHandlePanel_RemoteDiskTerminated);            
        }

        void remoteDiskHandlePanel_RemoteDiskTerminated()
        {
            this.ownerForm.RemoveDisplayedPanel(this.Title_Disk);
            this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.RemoteDisk, CommunicateType.Terminate, "owner");
            string showText = "您关闭了磁盘共享。";
            this.ownerForm.AppendSysMessage(showText);
        }

        void remoteDiskRequestPanel_RemoteRequestAnswerd(bool agree)
        {
            this.ownerForm.RemoveDisplayedPanel(this.Title_Disk);    
            this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.RemoteDisk, agree ? CommunicateType.Agree : CommunicateType.Reject, null);
            string showText = string.Format("您{0}了对方的磁盘访问请求。", agree ? "同意" : "拒绝");
            this.ownerForm.AppendSysMessage(showText);

            if (agree)
            {
                this.remoteDiskHandlePanel.OnAgree();
                this.ownerForm.AddDisplayedPanel(this.Title_Disk, this.remoteDiskHandlePanel);
            }
        }

        #region RequestControlFriendDisk
        public void RequestControlFriendDisk()
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }           

            this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.RemoteDisk, CommunicateType.Request, null);
            NDiskOutter diskOutter = new NDiskOutter(this.resourceCenter.RapidPassiveEngine.FileOutter, this.resourceCenter.RapidPassiveEngine.CustomizeOutter);            
            this.remoteDiskForm = new RemoteDiskForm(this.currentFriend.ID, this.currentFriend.DisplayName, diskOutter, this.resourceCenter.RapidPassiveEngine.FileOutter, this.resourceCenter.CurrentUserID);
            this.remoteDiskForm.RemoteDiskRequestCancelled += new CbGeneric(remoteDiskForm_RemoteDiskRequestCancelled);
            this.remoteDiskForm.RemoteDiskEnded += new CbGeneric<bool>(remoteDiskForm_RemoteDiskEnded);
            this.remoteDiskForm.Show();
        }

        void remoteDiskForm_RemoteDiskEnded(bool ownerTerminateClose)
        {
            if (!ownerTerminateClose)
            {
                this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.RemoteDisk, CommunicateType.Terminate, null);
            }

            string showText = ownerTerminateClose ? "对方关闭了磁盘共享。" : "您关闭了远程磁盘访问。";           
            this.ownerForm.AppendSysMessage(showText);
            this.remoteDiskForm = null;
        }

        void remoteDiskForm_RemoteDiskRequestCancelled()
        {
            this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.RemoteDisk, CommunicateType.Terminate, null);
        } 
        #endregion

        #region HandleRemoteDisk
        public void HandleRemoteDisk(CommunicateType communicateType, string tag)
        {
            if (communicateType == CommunicateType.Request)
            {
                this.OnRemoteDiskRequestReceived();
                this.ownerForm.FlashWindow();
                return;
            }

            if (communicateType == CommunicateType.Agree)
            {
                this.OnRemoteDiskAnswerReceived(true);
                this.ownerForm.FlashWindow();
                return;
            }

            if (communicateType == CommunicateType.Reject)
            {
                this.OnRemoteDiskAnswerReceived(false);
                this.ownerForm.FlashWindow();
                return;
            }

            if (communicateType == CommunicateType.Terminate)
            {
                if (tag == "owner")
                {
                    this.OnOwnerTerminateRemoteDisk();
                }
                else
                {
                    this.OnGuestCloseRemoteDisk();
                }
                this.ownerForm.FlashWindow();
                return;
            }
            
        }
        

        private void OnGuestCloseRemoteDisk()
        {
            Control ctrl = this.ownerForm.GetDisplayedPanel(this.Title_Disk);
            if ((ctrl as RemoteDiskRequestPanel) != null) //还未接收
            {
                this.ownerForm.AppendSysMessage("对方取消了磁盘访问请求。");
                this.ownerForm.RemoveDisplayedPanel(this.Title_Disk);
            }
            else
            {
                this.ownerForm.AppendSysMessage("对方关闭远程磁盘访问。");
                this.remoteDiskHandlePanel.OnTerminate();
                this.ownerForm.RemoveDisplayedPanel(this.Title_Disk);
            }
        }    

        private void OnOwnerTerminateRemoteDisk()
        {
            if (this.remoteDiskForm != null)
            {
                this.remoteDiskForm.OwnerTeminateHelp();
            }
        }

        private void OnRemoteDiskRequestReceived()
        {
            this.ownerForm.AddDisplayedPanel(this.Title_Disk, this.remoteDiskRequestPanel);
        }

        private void OnRemoteDiskAnswerReceived(bool agree)
        {
            if (this.remoteDiskForm == null)
            {
                return;
            }

            string showText = agree ? "对方同意了您访问他的磁盘！" : "对方拒绝了您访问他的磁盘！";
            this.ownerForm.AppendSysMessage(showText);
            this.remoteDiskForm.OnResponseOfRemoteControl(agree);
        }

        #endregion


        public void OnOffline(bool myself)
        {
            this.ownerForm.RemoveDisplayedPanel(this.Title_Disk);
        }

        public void BeforeOwnerFormClosed()
        {
            if (this.remoteDiskForm != null)
            {
                this.remoteDiskForm.Close();
            }

            if (this.ownerForm.ContanisDisplayedPanel(this.Title_Disk))
            {
                this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.RemoteDisk, CommunicateType.Reject, null);               
            }
        }
    }
}
