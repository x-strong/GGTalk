using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OMCS.Passive;
using OMCS.Passive.MultiChat;
using ESBasic;

namespace OMCS.Boost.MultiChat
{
    /// <summary>
    /// 用于OMCS聊天组中的一个子集（子组）的音/视频显示控件。
    /// </summary>
    public partial class VideoSubGroupPanel : UserControl
    {
        #region Members
        private IMultimediaManager multimediaManager;
        private IChatGroup chatGroup;
        private SubGroup subGroup;       
        #endregion

        #region Ctor
        public VideoSubGroupPanel()
        {
            InitializeComponent();
        } 
        #endregion

        /// <summary>
        /// 初始化群聊的子组控件。
        /// </summary>
        /// <param name="mgr">OMCS多媒体管理器。</param>
        /// <param name="group">OMCS聊天组。</param>
        /// <param name="subGroupName">子组名称</param>
        /// <param name="subGroupUserDic">子组中的所有成员列表</param>
        /// <param name="_justAudio">仅仅使用音频？</param>
        public void Initialize(IMultimediaManager mgr, IChatGroup group ,SubGroup sub)
        {
            this.multimediaManager = mgr;            
            this.chatGroup = group ;
            this.subGroup = sub;           
            this.chatGroup.SomeoneJoin += new CbGeneric<IChatUnit>(chatGroup_SomeoneJoin);
            this.chatGroup.SomeoneExit += new CbGeneric<string>(chatGroup_SomeoneExit);

            this.toolStripLabel_groupName.Text = this.subGroup.Name ;
            foreach (VideoMember member in this.subGroup.MemberList)
            {
                IChatUnit unit = this.chatGroup.GetMember(member.UserID);
                if (unit == null)
                {
                    continue;
                }
                this.AddMember(unit ,member.JustAudio);
            }
        }

        /// <summary>
        /// 将列表中的所有用户禁言。
        /// </summary>
        public void DisableAllSpeak()
        {
            foreach (VideoMember member in this.subGroup.MemberList)
            {
                IChatUnit unit = this.chatGroup.GetMember(member.UserID);
                if (unit == null)
                {
                    continue;
                }
                unit.MicrophoneConnector.ChangeOwnerOutput(false);
            }
        }

        private void AddMember(IChatUnit unit, bool justAudio)
        {          
            if (justAudio)
            {
                SpeakerAudioPanel panel = new SpeakerAudioPanel();
                panel.Initialize(unit, this.multimediaManager.CurrentUserID == unit.MemberID);
                this.flowLayoutPanel1.Controls.Add(panel);
            }
            else
            {
                SpeakerVideoPanel panel = new SpeakerVideoPanel();
                panel.SpeakControlEnabled = true;
                panel.Initialize(unit, this.multimediaManager.CurrentUserID == unit.MemberID ,unit.MemberID);
                this.flowLayoutPanel1.Controls.Add(panel);
            }
        }

        void chatGroup_SomeoneExit(string memberID)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<string>(this.chatGroup_SomeoneExit), memberID);
            }
            else
            {
                ISpeakerPanel target = null;
                foreach (ISpeakerPanel panel in this.flowLayoutPanel1.Controls)
                {
                    if (panel.MemberID == memberID)
                    {
                        target = panel;
                        break;
                    }
                }

                if (target == null)
                {
                    return;
                }

                this.flowLayoutPanel1.Controls.Remove((Control)target);
                this.toolStripLabel_count.Text = string.Format("在线:{0}人", this.flowLayoutPanel1.Controls.Count);
            }
        }

        void chatGroup_SomeoneJoin(IChatUnit unit)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<IChatUnit>(this.chatGroup_SomeoneJoin), unit);
            }
            else
            {
                VideoMember member = null;
                foreach (VideoMember tmp in this.subGroup.MemberList)
                {
                    if (tmp.UserID == unit.MemberID)
                    {
                        member = tmp;
                        break;
                    }
                }

                if (member == null)
                {
                    return;
                }

                this.AddMember(unit ,member.JustAudio);
                this.toolStripLabel_count.Text = string.Format("在线:{0}人", this.flowLayoutPanel1.Controls.Count);
            }
        }              
    }

    /// <summary>
    /// OMCS聊天组中的一个子集（子组）。
    /// </summary>
    public class SubGroup
    {
        public SubGroup() { }
        public SubGroup(string _name, List<VideoMember> members)
        {
            this.name = _name;
            this.memberList = members;
        }


        #region Name
        private string name = "";
        /// <summary>
        /// 子组的名称。
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        } 
        #endregion

        #region MemberList
        private List<VideoMember> memberList = new List<VideoMember>();
        /// <summary>
        /// 子组的成员列表。
        /// </summary>
        public List<VideoMember> MemberList
        {
            get { return memberList; }
            set { memberList = value; }
        }     
        #endregion    
    }

    public class VideoMember
    {
        public VideoMember() { }
        public VideoMember(string id, string _name, bool audio)
        {
            this.userID = id;
            this.name = _name;
            this.justAudio = audio;
        }

        private string userID = "";
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        private string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private bool justAudio = false;
        public bool JustAudio
        {
            get { return justAudio; }
            set { justAudio = value; }
        }
    }
}
