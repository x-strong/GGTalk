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
using OMCS.Boost;
using OMCS.Boost.Controls;
using OMCS.Boost.Forms;


namespace TalkBase.Client.Application
{
    /// <summary>
    /// 与好友的远程桌面过程管理器。
    /// </summary>    
    public class RemoteDesktopManager<TUser, TGroup>
        where TUser : class, TalkBase.IUser, new()
        where TGroup : TalkBase.IGroup, new()
    {
        private ResourceCenter<TUser, TGroup> resourceCenter;
        private TUser currentFriend;   
        private IOwnerForm ownerForm;

        private RemoteDeskRequestPanel remoteHelpRequestPanel = new RemoteDeskRequestPanel();
        private RemoteDeskHandlePanel remoteHelpHandlePanel = new RemoteDeskHandlePanel();
        private RemoteDeskForm remoteHelpForm = null;
        private string Title_Remote = "远程桌面";

        public RemoteDesktopManager(ResourceCenter<TUser, TGroup> center, IOwnerForm owner , TUser user)
        {
            this.resourceCenter = center;
            this.ownerForm = owner;            
            this.currentFriend = user;

            this.remoteHelpRequestPanel.RemoteRequestAnswerd += new CbGeneric<bool, RemoteHelpStyle, bool>(remoteHelpRequestPanel_RemoteHelpRequestAnswerd);
            this.remoteHelpHandlePanel.RemoteHelpTerminated += new CbGeneric<bool>(remoteHelpHandlePanel_RemoteHelpTerminated);            
        }

        #region HandleRemoteDesktop
        public void HandleRemoteDesktop(CommunicateMediaType mediaType, CommunicateType communicateType, string tag)
        {
            #region RemoteHelp
            if (mediaType == CommunicateMediaType.RemoteHelp)
            {
                if (communicateType == CommunicateType.Request)
                {
                    RemoteHelpStyle style = (RemoteHelpStyle)Enum.Parse(typeof(RemoteHelpStyle), tag);
                    this.OnRemoteHelpRequestReceived(style);
                    this.ownerForm.FlashWindow();
                    return;
                }

                if (communicateType == CommunicateType.Agree)
                {
                    this.OnRemoteHelpAnswerReceived(true);
                    this.ownerForm.FlashWindow();
                    return;
                }

                if (communicateType == CommunicateType.Reject)
                {
                    this.OnRemoteHelpAnswerReceived(false);
                    this.ownerForm.FlashWindow();
                    return;
                }

                if (communicateType == CommunicateType.Terminate)
                {
                    if (tag == "owner")
                    {
                        this.OnOwnerTerminateRemoteHelp();
                    }
                    else
                    {
                        this.OnHelperCloseDesktop(false);
                    }
                    this.ownerForm.FlashWindow();
                    return;
                }
            }
            #endregion

            #region RemoteControl
            if (mediaType == CommunicateMediaType.RemoteControl)
            {
                if (communicateType == CommunicateType.Request)
                {
                    this.OnRemoteControlRequestReceived();
                    this.ownerForm.FlashWindow();
                    return;
                }

                if (communicateType == CommunicateType.Agree)
                {
                    this.OnRemoteControlAnswerReceived(true);
                    this.ownerForm.FlashWindow();
                    return;
                }

                if (communicateType == CommunicateType.Reject)
                {
                    this.OnRemoteControlAnswerReceived(false);
                    this.ownerForm.FlashWindow();
                    return;
                }

                if (communicateType == CommunicateType.Terminate)
                {
                    if (tag == "owner")
                    {
                        this.OnOwnerTerminateRemoteControl();
                    }
                    else
                    {
                        this.OnHelperCloseDesktop(true);
                    }
                    this.ownerForm.FlashWindow();
                    return;
                }
            }
            #endregion
        }
        #endregion

        public RemoteDeskForm RemoteHelpForm
        {
            get { return this.remoteHelpForm; }
        }

        public bool IsWorkding
        {
            get { return this.remoteHelpForm != null || this.ownerForm.ContanisDisplayedPanel(this.Title_Remote); }
        }

        public void BeforeOwnerFormClosed()
        {
            if (this.remoteHelpForm != null)
            {
                this.remoteHelpForm.Close();
            }

            if (this.ownerForm.ContanisDisplayedPanel(this.Title_Remote))
            {
                this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.RemoteHelp, CommunicateType.Reject, null);
                //this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.RemoteHelp, CommunicateType.Terminate, "owner");
            }
        }

        public void OnOffline(bool myself)
        {
            this.ownerForm.RemoveDisplayedPanel(this.Title_Remote);           
        }

        #region RequestRemoteHelp
        public void RequestRemoteHelp()
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }
            if (this.resourceCenter.ClientOutter.IsInHisBlackList(this.currentFriend.ID))
            {
                MessageBox.Show("对方已将您加入黑名单，不能进行远程协助！");
                return;
            }

            if (this.ownerForm.ContanisDisplayedPanel(this.Title_Remote))
            {
                return;
            }


            this.PrepairRemoteHelp(null, RemoteHelpStyle.AllScreen);
        } 
        #endregion

        public void ShareMyDesktop()
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }
            if (this.resourceCenter.ClientOutter.IsInHisBlackList(this.currentFriend.ID))
            {
                MessageBox.Show("对方已将您加入黑名单，不能进行桌面共享！");
                return;
            }            
            ESBasic.Widget.CaptureScreenForm form = new ESBasic.Widget.CaptureScreenForm("按住左键选择桌面共享区域");
            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            this.PrepairRemoteHelp(form.CaptureRegion, RemoteHelpStyle.PartScreen);
        }

        public void RequestControlFriendDesktop()
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }
            if (this.resourceCenter.ClientOutter.IsInHisBlackList(this.currentFriend.ID))
            {
                MessageBox.Show("对方已将您加入黑名单，不能进行远程控制！");
                return;
            }

            IMultimediaManager mgr = MultimediaManagerFactory.GetSingleton();
            if (mgr == null)
            {
                this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.RemoteControl, CommunicateType.Reject, null);
                MessageBox.Show("无法启动多媒体设备！");
                return;
            }

            this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.RemoteControl, CommunicateType.Request, null);
            string loginID4OMCS = ESFramework.MSideHelper.ConstructLoginID(this.currentFriend.ID, ESFramework.ClientType.DotNET);
            this.remoteHelpForm = new RemoteDeskForm(loginID4OMCS, this.currentFriend.DisplayName, RemoteHelpStyle.AllScreen, true);
            this.remoteHelpForm.RemoteControlRequestCancelled += new CbGeneric(remoteHelpForm_RemoteControlRequestCancelled);
            this.remoteHelpForm.RemoteHelpEnded += new CbGeneric<bool, RemoteHelpStyle, bool>(remoteHelpForm_RemoteHelpEnded);
            this.remoteHelpForm.Show();
        }

        void remoteHelpForm_RemoteControlRequestCancelled()
        {
            this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.RemoteControl, CommunicateType.Terminate, null);
        }

        private void OnRemoteControlRequestReceived()
        {
            this.remoteHelpRequestPanel.SetRemoteStyle(true);
            this.remoteHelpRequestPanel.SetRemoteDesktopStyle(RemoteHelpStyle.AllScreen);
            this.ownerForm.AddDisplayedPanel(this.Title_Remote, this.remoteHelpRequestPanel);            
        }

        private void OnRemoteControlAnswerReceived(bool agree)
        {
            if (this.remoteHelpForm == null)
            {
                return;
            }

            string showText = agree ? "对方同意了您控制他的电脑！" : "对方拒绝了您控制他的电脑！";
            this.ownerForm.AppendSysMessage(showText);
            this.remoteHelpForm.OnResponseOfRemoteControl(agree);           
        }

        private void OnOwnerTerminateRemoteControl()
        {
            if (this.remoteHelpForm != null)
            {
                this.remoteHelpForm.OwnerTeminateHelp();
            }            
        }

        /// <summary>
        /// 【站在协助方角度】请求方终止了桌面共享
        /// </summary>
        private void OnOwnerTerminateRemoteHelp()
        {
            if (this.remoteHelpForm != null)
            {
                this.remoteHelpForm.OwnerTeminateHelp();
            }
            else
            {
                this.ownerForm.AppendSysMessage("对方取消了远程协助请求。");
                this.ownerForm.RemoveDisplayedPanel(this.Title_Remote);
            }
        }

        /// <summary>
        /// 【站在请求方角度】协助方关闭了远程桌面
        /// </summary>
        private void OnHelperCloseDesktop(bool isRemoteControl)
        {
            if (isRemoteControl)
            {
                Control ctrl = this.ownerForm.GetDisplayedPanel(this.Title_Remote);
                if ((ctrl as RemoteDeskRequestPanel) != null) //还未接收
                {
                    this.ownerForm.AppendSysMessage("对方取消了远程控制请求。");
                    this.ownerForm.RemoveDisplayedPanel(this.Title_Remote);
                }
                else
                {
                    this.ownerForm.AppendSysMessage("对方结束了对您电脑的控制。");
                    this.remoteHelpHandlePanel.OnTerminate();
                    this.ownerForm.RemoveDisplayedPanel(this.Title_Remote);
                }
                return;  
            }

            this.ownerForm.AppendSysMessage("对方终止了对您远程协助。");
            this.remoteHelpHandlePanel.OnTerminate();
            this.ownerForm.RemoveDisplayedPanel(this.Title_Remote);
        }       

        private void PrepairRemoteHelp(Rectangle? regionSelected, RemoteHelpStyle style)
        {
            string msg = "请求对方远程协助自己，正在等待对方回应...";
            if (regionSelected != null)
            {
                msg = string.Format("已经指定桌面区域{0}，", regionSelected.Value) + msg;
            }
            this.ownerForm.AppendSysMessage(msg);

            IMultimediaManager mgr = MultimediaManagerFactory.GetSingleton();
            if (mgr == null)
            {
                MessageBox.Show("无法启动多媒体设备！");
                return;
            }
            mgr.DesktopRegion = regionSelected; //设为null，表示共享整个屏幕
            mgr.DesktopEncodeQuality = 8;
            this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.RemoteHelp, CommunicateType.Request, style.ToString());

            this.remoteHelpHandlePanel.SetRemoteStyle(false);
            this.ownerForm.AddDisplayedPanel(this.Title_Remote, this.remoteHelpHandlePanel);
        }

        /// <summary>
        /// 【站在请求方角度】对方回复远程协助请求
        /// </summary>        
        private void OnRemoteHelpAnswerReceived(bool agree)
        {
            if (!agree)
            {
                this.ownerForm.AppendSysMessage("对方拒绝了您的远程协助请求。");
                this.ownerForm.RemoveDisplayedPanel(this.Title_Remote);              
                return;
            }

            this.ownerForm.AppendSysMessage("对方同意了您的远程协助请求。");
            this.remoteHelpHandlePanel.OnAgree();
        }

        /// <summary>
        /// 【站在协助方角度】收到远程协助请求
        /// </summary>
        private void OnRemoteHelpRequestReceived(RemoteHelpStyle style)
        {
            this.remoteHelpRequestPanel.SetRemoteStyle(false);
            this.remoteHelpRequestPanel.SetRemoteDesktopStyle(style);
            this.ownerForm.AddDisplayedPanel(this.Title_Remote, this.remoteHelpRequestPanel);            
        }


        /// <summary>
        /// 远程桌面被关闭。可能原因：1.协助方主动叉掉窗口； 2.请求方终止桌面共享
        /// </summary>       
        void remoteHelpForm_RemoteHelpEnded(bool ownerTerminateClose, RemoteHelpStyle style, bool isRemoteControl)
        {
            if (!ownerTerminateClose)
            {
                this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, isRemoteControl ? CommunicateMediaType.RemoteControl : CommunicateMediaType.RemoteHelp, CommunicateType.Terminate, null);
            }

            string showText = ownerTerminateClose ? "对方终止了远程协助。" : "您终止了给对方的远程协助。";
            if (isRemoteControl)
            {
                showText = ownerTerminateClose ? "对方关闭了远程控制。" : "您终止了控制对方的电脑。";
            }
            this.ownerForm.AppendSysMessage(showText);
            this.remoteHelpForm = null;
        }

        /// <summary>
        /// 作为请求方，结束桌面共享。（点击控件的按钮）
        /// </summary>  
        void remoteHelpHandlePanel_RemoteHelpTerminated(bool isRemoteControl)
        {
            if (isRemoteControl)
            {
                this.ownerForm.RemoveDisplayedPanel(this.Title_Remote);
                this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.RemoteControl, CommunicateType.Terminate, "owner");
                string showText = "您终止了对方对自己电脑的控制。";
                this.ownerForm.AppendSysMessage(showText);
            }
            else
            {
                this.ownerForm.RemoveDisplayedPanel(this.Title_Remote);
                this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.RemoteHelp, CommunicateType.Terminate, "owner");
                string showText = this.remoteHelpHandlePanel.IsWorking ? "您终止了远程协助。" : "您取消了远程协助请求。";
                this.ownerForm.AppendSysMessage(showText);
            }
        }

        /// <summary>
        /// 作为协助方，对远程协助/控制请求的应答。（点击控件的按钮）
        /// </summary>       
        void remoteHelpRequestPanel_RemoteHelpRequestAnswerd(bool agree, RemoteHelpStyle style, bool isRemoteControl)
        {
            this.ownerForm.RemoveDisplayedPanel(this.Title_Remote);
            IMultimediaManager mgr = null;
            if (agree)
            {

                mgr = MultimediaManagerFactory.GetSingleton();
                if (mgr == null)
                {
                    this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, isRemoteControl ? CommunicateMediaType.RemoteControl : CommunicateMediaType.RemoteHelp, CommunicateType.Reject, null);
                    MessageBox.Show("无法启动多媒体设备！");
                    return;
                }
                mgr.DesktopRegion = null;
                mgr.DesktopEncodeQuality = 8;
            }

            this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, isRemoteControl ? CommunicateMediaType.RemoteControl : CommunicateMediaType.RemoteHelp, agree ? CommunicateType.Agree : CommunicateType.Reject, null);
            string showText = string.Format("您{0}了对方的远程{1}请求。", agree ? "同意" : "拒绝", isRemoteControl ? "控制" : "协助");
            this.ownerForm.AppendSysMessage(showText);

            if (agree)
            {
                if (isRemoteControl)
                {
                    this.remoteHelpHandlePanel.SetRemoteStyle(true);
                    this.remoteHelpHandlePanel.OnAgree();
                    this.ownerForm.AddDisplayedPanel(this.Title_Remote, this.remoteHelpHandlePanel);                                   
                }
                else
                {
                    string loginID4OMCS = ESFramework.MSideHelper.ConstructLoginID(this.currentFriend.ID, ESFramework.ClientType.DotNET);
                    this.remoteHelpForm = new RemoteDeskForm(loginID4OMCS, this.currentFriend.DisplayName, style, isRemoteControl);
                    this.remoteHelpForm.RemoteHelpEnded += new CbGeneric<bool, RemoteHelpStyle, bool>(remoteHelpForm_RemoteHelpEnded);
                    this.remoteHelpForm.Show();
                }
            }
        }       
    }
}
