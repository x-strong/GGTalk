using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TalkBase;
using TalkBase.Client;

namespace GGTalk
{
    public partial class GroupInfoForm : BaseForm
    {
        private ResourceCenter<GGUser, GGGroup> center;

        public GroupInfoForm(ResourceCenter<GGUser, GGGroup> center, GGGroup group)
        {
            InitializeComponent();
            this.center = center;
            this.InitMemberList(group);
            this.InitGroupDetail(group);
        }

        private void InitMemberList(GGGroup group)
        {
            foreach (string memberID in group.MemberList)
            {
                GGUser ggUser = this.center.ClientGlobalCache.GetUser(memberID);
                UserPanel2 userPanel2 = new UserPanel2(this.center,ggUser);
                if (group.IsPrivate)
                {
                    userPanel2.SetIsShowUserInfo(false);
                }
                userPanel2.Tag = ggUser;
                this.flowLayoutPanel1.Controls.Add(userPanel2);
            } 
        }

        private void InitGroupDetail(GGGroup group)
        {
            this.skinLabel_ID.Text = group.ID.StartsWith(FunctionOptions.PrefixGroupID) ? group.ID.Remove(0, 1) : group.ID;
            this.skinLabel_Name.Text = group.Name;
            this.skinLabel_CreatorName.Text = this.center.ClientGlobalCache.GetUserName(group.CreatorID);
            this.skinLabel_Announce.Text = group.Announce;
            this.skinLabel_groupType.Text = group.IsPrivate ? "密聊群" : "普通群";
        }
    
    }
}
