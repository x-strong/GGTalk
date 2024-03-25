using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESBasic;
using TalkBase;
using TalkBase.Client;

namespace GGTalk
{
    public partial class AtSelectUserFrom : Form
    {
        private ResourceCenter<GGUser, GGGroup> center;
        private GGGroup group;
        public event Action<IUnit> MemberSelected;
        public event CbGeneric PanleClosed;
        
        public AtSelectUserFrom(ResourceCenter<GGUser, GGGroup> center,GGGroup group)
        {
            InitializeComponent();
            this.center = center;
            this.group = group;
            this.InitPanelList(group.MemberList);
            this.InsetAtAllItemPanel();
        }

        private void InitPanelList(List<string> memberIDs)
        {
            this.flowLayoutPanel1.Controls.Clear();
            foreach (string memberID in memberIDs)
            {
                GGUser ggUser = this.center.ClientGlobalCache.GetUser(memberID);
                if (ggUser != null)
                {
                    this.AddItemPanel(ggUser);
                }
            }

        }

        /// <summary>
        /// 将@all Panel插入到第一个位置
        /// </summary>
        private void InsetAtAllItemPanel()
        {
            if (this.center.CurrentUserID != this.group.CreatorID)
            {
                return;
            }
            GGUser allMember = new GGUser();
            allMember.UserID = "@all";
            allMember.Name = "全体成员";
            allMember.HeadImageIndex = -1;
            GroupMemberPanel memberPanel = new GroupMemberPanel(allMember);
            memberPanel.OnClicked += MemberPanel_OnClicked;
            this.flowLayoutPanel1.Controls.Add(memberPanel);
            this.flowLayoutPanel1.Controls.SetChildIndex(memberPanel, 0);
        }

        private void AddItemPanel(GGUser ggUser)
        {
            GroupMemberPanel memberPanel = new GroupMemberPanel(ggUser);
            memberPanel.OnClicked += MemberPanel_OnClicked;
            this.flowLayoutPanel1.Controls.Add(memberPanel);
        }

        private void MemberPanel_OnClicked(IUnit unit)
        {
            if (this.MemberSelected != null)
            {
                this.MemberSelected(unit);
            }
        }

        private void skinPictureBox_close_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            if (this.PanleClosed != null)
            {
                this.PanleClosed();
            }
        }
    }
}
