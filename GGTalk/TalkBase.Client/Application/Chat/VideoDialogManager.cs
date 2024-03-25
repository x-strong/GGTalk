using System;
using System.Collections.Generic;
using System.Text;
using TalkBase.Client;
using ESBasic;
using TalkBase;
using TalkBase.Client.Application;
using System.Windows.Forms;
using OMCS.Passive;
using OMCS.Boost;
using OMCS.Boost.Controls;
using OMCS.Boost.Forms;
using ESFramework;


namespace TalkBase.Client.Application
{
    /// <summary>
    /// 与好友的视频对话过程管理器。
    /// </summary>   
    public class VideoDialogManager<TUser, TGroup>
        where TUser : class, TalkBase.IUser, new()
        where TGroup : TalkBase.IGroup, new()
    {
        private ResourceCenter<TUser, TGroup> resourceCenter;
        private TUser currentFriend;      
        private IOwnerForm ownerForm;
        string loginID4OMCS = string.Empty;
        private string Title_Video = "视频";
        private VideoChatRequestPanel videoRequestPanel = new VideoChatRequestPanel();
        private IVideoChatForm videoForm = null;
        
        public VideoDialogManager(ResourceCenter<TUser, TGroup> center, IOwnerForm owner,  TUser user)
        {
            this.resourceCenter = center;
            this.ownerForm = owner;           
            this.currentFriend = user;

            this.videoRequestPanel.VideoRequestAnswerd += new CbGeneric<bool>(videoRequestPanel_VideoRequestAnswerd);       
        }

        public void BeforeOwnerFormClosed()
        {
            if (this.videoForm != null)
            {
                ((Form)this.videoForm).Close();
            }

            if (this.ownerForm.ContanisDisplayedPanel(this.Title_Video))
            {
                this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.Video, CommunicateType.Reject, null, this.destClientType4MediaCommunicate);
            }
        }

        public void OnOffline(bool myself)
        {
            bool showMsg = false;
            if (this.videoForm != null)
            {
                this.videoForm.OnHungUpVideo();
                showMsg = true;
            }
            if (this.ownerForm.ContanisDisplayedPanel(this.Title_Video))
            {
                this.ownerForm.RemoveDisplayedPanel(this.Title_Video);
                showMsg = true;
            }
            if (showMsg) {
                string msg = string.Format("{0}已经掉线，与对方的视频会话中止。", myself ? "自己" : "对方");
                this.ownerForm.AppendSysMessage(msg);
            }
        }

        private ClientType destClientType4MediaCommunicate = ClientType.DotNET;
        public void HandleVideoDialog(ClientType sourceClientType, CommunicateType communicateType, string tag)
        {
            this.destClientType4MediaCommunicate = sourceClientType;
            this.loginID4OMCS = ESFramework.MSideHelper.ConstructLoginID(this.currentFriend?.ID, this.destClientType4MediaCommunicate);

            if (communicateType == CommunicateType.Request)
            {
                this.OnVideoRequestReceived();
                this.ownerForm.FlashWindow();
                return;
            }

            if (communicateType == CommunicateType.Agree)
            {
                this.OnVideoAnswerReceived(true);
                this.ownerForm.FlashWindow();
                return;
            }

            if (communicateType == CommunicateType.Reject)
            {
                this.OnVideoAnswerReceived(false);
                this.ownerForm.FlashWindow();
                return;
            }

            if (communicateType == CommunicateType.Terminate)
            {
                this.OnVideoHungUpReceived();
                this.ownerForm.FlashWindow();
                return;
            }
            if (communicateType == CommunicateType.Busy)
            {
                this.OnVideoAnswerReceived(false, "对方忙线中");
                this.ownerForm.FlashWindow();
                return;
            }
        }

        public void OnMediaCommunicateAnswerOnOtherDevice(ClientType answerType, bool agree)
        {
            this.ownerForm.RemoveDisplayedPanel(this.Title_Video);
            this.ownerForm.AppendSysMessage("已经在其它设备上回应了对方的视频请求。");
        }
       
        //发出视频邀请
        public void RequestVideoDialog()
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            if (this.videoForm != null)
            {
                return;
            }
            if (this.resourceCenter.ClientOutter.IsInHisBlackList(this.currentFriend.ID)) {
                MessageBox.Show("对方已将您加入黑名单，不能进行视频通话！");
                return;
            }

            this.videoForm = this.ownerForm.DoCreateVideoChatForm(true);
            ((Form)this.videoForm).FormClosed += new FormClosedEventHandler(videoForm_FormClosed);
            this.videoForm.HungUpVideo += new CbGeneric<HungUpCauseType>(videoForm_ActiveHungUpVideo);
            ((Form)this.videoForm).Show();


            IMultimediaManager mgr = MultimediaManagerFactory.GetSingleton();
            if (mgr == null)
            {
                MessageBox.Show("无法启动多媒体设备！");
                if (this.videoForm != null)
                {
                    ((Form)this.videoForm).Close();
                }
                return;
            }

            this.videoForm.Initialize(mgr,this.loginID4OMCS);
            this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.Video, CommunicateType.Request, null);
        }

        /// <summary>
        /// 收到视频聊天的请求
        /// </summary>  
        private void OnVideoRequestReceived()
        {
            this.ownerForm.AddDisplayedPanel(this.Title_Video, this.videoRequestPanel);            
        }

        /// <summary>
        /// 对方回复视频邀请
        /// </summary>        
        private void OnVideoAnswerReceived(bool agree,string tips=null)
        {
            if (this.videoForm == null)
            {
                return;
            }

            if (agree)
            {
                //MSide
                this.videoForm.OnAgree(this.loginID4OMCS);
            }
            else
            {
                this.videoForm.OnReject();
                ((Form)this.videoForm).Close();
                this.videoForm = null;
                tips = tips ?? "对方拒绝了您的视频邀请。";
                this.ownerForm.AppendSysMessage(tips);
            }
        }

        /// <summary>
        /// 对方挂断了视频
        /// </summary>       
        private void OnVideoHungUpReceived()
        {
            if (this.videoForm != null)
            {
                this.ownerForm.AppendSysMessage("对方中止了视频通话。");
                this.videoForm.OnHungUpVideo();
                return;
            }
          
            if (this.ownerForm.ContanisDisplayedPanel(this.Title_Video))
            {
                this.ownerForm.RemoveDisplayedPanel(this.Title_Video);
                this.ownerForm.AppendSysMessage("对方结束了视频会话邀请。");
            }
        }

        /// <summary>
        /// 作为视频接收方，对视频会话请求的应答。（点击控件的按钮）
        /// </summary>
        private void videoRequestPanel_VideoRequestAnswerd(bool agree)
        {
            this.ownerForm.RemoveDisplayedPanel(this.Title_Video);
            if (!this.resourceCenter.Connected)
            {
                this.ownerForm.AppendSysMessage("您已掉线！");
                return;
            }
            IMultimediaManager mgr = null;
            if (agree)
            {
                this.videoForm = this.ownerForm.DoCreateVideoChatForm(false);
                ((Form)this.videoForm).FormClosed += new FormClosedEventHandler(videoForm_FormClosed);
                this.videoForm.HungUpVideo += new CbGeneric<HungUpCauseType>(videoForm_ActiveHungUpVideo);
                this.videoForm.ConnectorDisconnected += new CbGeneric(videoForm_ConnectorDisconnected);
                ((Form)this.videoForm).Show();

                this.ownerForm.AppendSysMessage("您同意了对方的视频会话请求，正在启动多媒体设备...");
                mgr = MultimediaManagerFactory.GetSingleton();
                if (mgr == null)
                {
                    this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.Video, CommunicateType.Reject, null, this.destClientType4MediaCommunicate);
                    this.ownerForm.AppendSysMessage("无法启动多媒体设备！");
                    MessageBox.Show("无法启动多媒体设备！");
                    if (this.videoForm != null)
                    {
                        ((Form)this.videoForm).Close();
                    }
                    return;
                }

                this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.Video, CommunicateType.Agree, null, this.destClientType4MediaCommunicate);
                //MSide
                this.videoForm.Initialize(mgr, loginID4OMCS);
            }
            else
            {
                this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.Video, CommunicateType.Reject, null, this.destClientType4MediaCommunicate);
                this.ownerForm.AppendSysMessage("您拒绝了对方的视频会话请求。");
            }
        }

        void videoForm_ConnectorDisconnected()
        {
            this.ownerForm.AppendSysMessage("与对方的网络连接中断，视频通话结束。");
        }

        private void videoForm_ActiveHungUpVideo(HungUpCauseType hungUpCauseType)
        {
            this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.Video, CommunicateType.Terminate, null);
            this.ownerForm.AppendSysMessage("您中止了视频通话。");            
        }

        private void videoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.videoForm = null;
        }                           
    }
}
