using OMCS.Boost.MultiChat;
using OMCS.Passive;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TalkBase.Client;
using TalkBase.Client.Bridges;

namespace GGTalk
{
    public partial class GroupVideoChatForm : BaseForm, IGroupVideoForm, IMemberNameGetter
    {
        private IMultimediaManager multimediaManager;
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        public GroupVideoChatForm(ResourceCenter<GGUser, GGGroup> center, string chatRoomID)
        {
            InitializeComponent();
            this.resourceCenter = center;
            this.multimediaManager = MultimediaManagerFactory.GetSingleton();
            this.Text = this.Text + " - " + chatRoomID;
            //this.skinLabel_userID.Text = this.multimediaManager.CurrentUserID;
            this.multimediaManager.OutputAudio = true;
            this.multimediaManager.OutputVideo = true;
            this.multiVideoChatContainer1.Initialize(this.multimediaManager, chatRoomID,this);
            this.multiVideoChatContainer1.ChatGroup.SomeoneExit += ChatGroup_SomeoneExit;
            this.multiVideoChatContainer1.ChatGroup.SomeoneJoin += ChatGroup_SomeoneJoin;
        }

        private void ChatGroup_SomeoneJoin(OMCS.Passive.MultiChat.IChatUnit unit)
        {
            this.toolStripLabel_Msg.Text = this.resourceCenter.ClientGlobalCache.GetUserName(CommonHelper.GetUserID4OMCS(unit.MemberID) ) + "加入了群视频邀请";
        }

        private void ChatGroup_SomeoneExit(string userID)
        {
            this.toolStripLabel_Msg.Text = this.resourceCenter.ClientGlobalCache.GetUserName(CommonHelper.GetUserID4OMCS(userID)) + "退出了群视频邀请";
        }

        #region IGroupVideoForm
        public void OnGroupVideoAnswerOnOtherDevice()
        {
            this.Close();
        }

        public void OnRejectJoinReceived(string rejecterID)
        {
            this.toolStripLabel_Msg.Text = this.resourceCenter.ClientGlobalCache.GetUserName(rejecterID) + "拒绝了群视频邀请";
        } 
        #endregion

        private void GroupVideoChatForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.multiVideoChatContainer1.Close();
        }

        public string GetMemberName(string memberID)
        {
            return this.resourceCenter.ClientGlobalCache.GetUserName(CommonHelper.GetUserID4OMCS(memberID) );
        }


    }
}
