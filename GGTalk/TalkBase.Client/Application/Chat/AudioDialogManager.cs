using System;
using System.Collections.Generic;
using System.Text;
using TalkBase.Client;
using OMCS.Passive;
using TalkBase.Client.Application;
using System.Windows.Forms;
using TalkBase;
using ESBasic;
using OMCS.Boost;
using OMCS.Boost.Controls;
using ESFramework;


namespace TalkBase.Client.Application
{
    /// <summary>
    /// 与好友的语音对话过程管理器。
    /// </summary>    
    public class AudioDialogManager<TUser, TGroup>
        where TUser : class, TalkBase.IUser, new()
        where TGroup : TalkBase.IGroup, new()
    {
        private ResourceCenter<TUser, TGroup> resourceCenter;
        private TUser currentFriend;       
        
        private IOwnerForm ownerForm;

        private string Title_Audio = "语音对话";
        private AudioChatHandlePanel audioHandlePanel = new AudioChatHandlePanel();

        public AudioDialogManager(ResourceCenter<TUser, TGroup> center, IOwnerForm owner, TUser user)
        {
            this.resourceCenter = center;
            this.ownerForm = owner;         
            this.currentFriend = user;
            string loginID4OMCS = ESFramework.MSideHelper.ConstructLoginID(this.currentFriend.ID, this.destClientType4MediaCommunicate);
            this.audioHandlePanel.Initialize(loginID4OMCS);
            this.audioHandlePanel.AudioRequestAnswerd += new CbGeneric<bool>(audioHandlePanel_AudioRequestAnswerd);
            this.audioHandlePanel.AudioTerminated += new CbGeneric(audioHandlePanel_AudioTerminated);
        }

        public void BeforeOwnerFormClosed()
        {
            if (this.ownerForm.ContanisDisplayedPanel(this.Title_Audio))
            {
                CommunicateType type = (this.audioHandlePanel.IsWorking || this.audioHandlePanel.IsSender) ? CommunicateType.Terminate : CommunicateType.Reject;
                this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.Audio, type, null);
            }
        }

        public void OnOffline(bool myself)
        {
            if (this.ownerForm.ContanisDisplayedPanel(this.Title_Audio))
            {
                if (this.audioHandlePanel.IsWorking)
                {
                    this.audioHandlePanel.OnTerminate();
                    string msg = string.Format("{0}已经掉线，与对方的语音对话中止。", myself ? "自己" : "对方");
                    this.ownerForm.AppendSysMessage(msg);
                }
                else
                {
                    string msg = string.Format("{0}已经掉线!", myself ? "自己" : "对方");
                    this.ownerForm.AppendSysMessage(msg);
                }
                this.ownerForm.RemoveDisplayedPanel(this.Title_Audio);                
            }
        }

        private ClientType destClientType4MediaCommunicate = ClientType.DotNET;
        public void HandleAudioDialog(ClientType sourceClientType, CommunicateType communicateType, string tag)
        {
            this.destClientType4MediaCommunicate = sourceClientType;
            if (communicateType == CommunicateType.Request)
            {
                this.OnAudioRequestReceived();
                this.ownerForm.FlashWindow();
                return;
            }

            if (communicateType == CommunicateType.Agree)
            {
                this.OnAudioAnswerReceived(true);
                this.ownerForm.FlashWindow();
                return;
            }

            if (communicateType == CommunicateType.Reject)
            {
                this.OnAudioAnswerReceived(false);
                this.ownerForm.FlashWindow();
                return;
            }

            if (communicateType == CommunicateType.Terminate)
            {
                this.OnAudioHungUpReceived();
                this.ownerForm.FlashWindow();
                return;
            }
            if (communicateType == CommunicateType.Busy)
            {
                this.OnAudioAnswerReceived(false, "对方忙线中");
                this.ownerForm.FlashWindow();
                return;
            }
        }
      
        //发出语音邀请
        public void RequestAudioDialog()
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }         

            if (this.ownerForm.ContanisDisplayedPanel(this.Title_Audio))
            {
                return;
            }
            if (this.resourceCenter.ClientOutter.IsInHisBlackList(this.currentFriend.ID))
            {
                MessageBox.Show("对方已将您加入黑名单，不能进行语音通话！");
                return;
            }

            IMultimediaManager mgr = MultimediaManagerFactory.GetSingleton();
            if (mgr == null)
            {
                MessageBox.Show("无法启动多媒体设备！");
                return;
            }

            mgr.OutputAudio = true;
            string msg = "请求与对方进行语音对话，正在等待对方回应...";
            this.ownerForm.AppendSysMessage(msg);

            this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.Audio, CommunicateType.Request, null);

            this.audioHandlePanel.IsSender = true;
            this.ownerForm.AddDisplayedPanel(this.Title_Audio, this.audioHandlePanel);            
        }

        /// <summary>
        /// 收到语音对话的请求
        /// </summary>  
        private void OnAudioRequestReceived()
        {
            if (!this.ownerForm.ContanisDisplayedPanel(this.Title_Audio))
            {
                this.audioHandlePanel.IsSender = false;
                this.ownerForm.AddDisplayedPanel(this.Title_Audio, this.audioHandlePanel);    
            }
        }

        /// <summary>
        /// 自己回复语音邀请
        /// </summary>  
        void audioHandlePanel_AudioRequestAnswerd(bool agree)
        {
            if (!this.resourceCenter.Connected)
            {
                this.ownerForm.AppendSysMessage("您已掉线！");
                return;
            }
            if (!agree)
            {
                this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.Audio, CommunicateType.Reject, null, this.destClientType4MediaCommunicate);
                this.ownerForm.AppendSysMessage("您拒绝了对方的语音对话请求。");
                this.ownerForm.RemoveDisplayedPanel(this.Title_Audio);
                return;
            }

            this.ownerForm.AppendSysMessage("您同意了对方的语音对话请求，正在启动多媒体设备...");

            IMultimediaManager mgr = MultimediaManagerFactory.GetSingleton();
            if (mgr == null)
            {
                this.ownerForm.AppendSysMessage("无法启动多媒体设备。");
                MessageBox.Show("无法启动多媒体设备！");
                this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.Audio, CommunicateType.Reject, null, this.destClientType4MediaCommunicate);
                return;
            }
            this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.Audio, CommunicateType.Agree, null ,this.destClientType4MediaCommunicate);
            mgr.OutputAudio = true;
            string loginID4OMCS = ESFramework.MSideHelper.ConstructLoginID(this.currentFriend.ID, this.destClientType4MediaCommunicate);
            this.audioHandlePanel.OnAgree(mgr);
        }

        /// <summary>
        /// 对方回复语音邀请
        /// </summary>        
        private void OnAudioAnswerReceived(bool agree,string tips=null)
        {
            if (!agree)
            {
                tips = tips ?? "对方拒绝了您的语音对话请求。";
                this.ownerForm.AppendSysMessage(tips);
                this.ownerForm.RemoveDisplayedPanel(this.Title_Audio);
                return;
            }

            this.ownerForm.AppendSysMessage("对方同意了您的语音对话请求。");

            //MSide
            string loginID4OMCS = ESFramework.MSideHelper.ConstructLoginID(this.currentFriend.ID, this.destClientType4MediaCommunicate);
            this.audioHandlePanel.OnAgree(MultimediaManagerFactory.GetSingleton());
        }

        //自己挂断
        void audioHandlePanel_AudioTerminated()
        {
            string showText = this.audioHandlePanel.IsWorking ? "您挂断了语音对话。" : "您取消了语音对话请求。";

            if (this.audioHandlePanel.IsWorking)
            {
                this.audioHandlePanel.OnTerminate();
            }
            this.ownerForm.RemoveDisplayedPanel(this.Title_Audio);
            this.resourceCenter.ClientOutter.SendMediaCommunicate(this.currentFriend.ID, CommunicateMediaType.Audio, CommunicateType.Terminate, null);
            this.ownerForm.AppendSysMessage(showText);
        }

        /// <summary>
        /// 对方挂断了语音
        /// </summary>       
        private void OnAudioHungUpReceived()
        {            
            if (this.ownerForm.ContanisDisplayedPanel(this.Title_Audio))
            {
                string showText = this.audioHandlePanel.IsWorking ? "对方挂断了语音对话。" : "对方取消了语音对话请求。";
                this.ownerForm.AppendSysMessage(showText);
                this.audioHandlePanel.OnTerminate();
                this.ownerForm.RemoveDisplayedPanel(this.Title_Audio);
            }
        }

        public void OnMediaCommunicateAnswerOnOtherDevice(ClientType answerType, bool agree)
        {
            this.ownerForm.RemoveDisplayedPanel(this.Title_Audio);
            this.ownerForm.AppendSysMessage("已经在其它设备上回应了对方的语音请求。");
        }
        
    }
}
