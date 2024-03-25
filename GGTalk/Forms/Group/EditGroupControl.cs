using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CCWin;
using TalkBase.Client;
using GGTalk.Forms.Group;

namespace GGTalk.LikeQQ.Yes
{
    /// <summary>
    /// 编辑群组的空间。
    /// </summary>
    public partial class EditGroupControl : UserControl
    {
        private BaseGlobalCache<GGUser, GGGroup> globalUserCache;
        private GGGroup currentGroup;
        private ESBasic.Collections.SortedArray<string> currentMemberList = new ESBasic.Collections.SortedArray<string>();
        public EditGroupControl()
        {
            InitializeComponent();
        }

        public void Initialize(BaseGlobalCache<GGUser, GGGroup> cache, GGGroup group)
        {
            this.globalUserCache = cache;
            this.currentGroup = group;
            if (this.currentGroup != null)
            {
                this.AddPanelItems(this.currentGroup.MemberList);
            }
        }

        private void AddPanelItems(List<string> memberIDs)
        {
            foreach (string userID in memberIDs)
            {
                GGUser user = this.globalUserCache.GetUser(userID);
                if (user != null && !currentMemberList.Contains(user.ID))
                {
                    currentMemberList.Add(user.UserID);
                    UserPanel userPanel = new UserPanel();
                    userPanel.GGUser = user;
                    userPanel.Closed += UserPanel_UserRemoved;
                    userPanel.Size = new Size(this.flowLayoutPanel1.Width - 30, userPanel.Height);
                    this.flowLayoutPanel1.Controls.Add(userPanel);
                }
            }
        }

        private void UserPanel_UserRemoved(UserPanel userPanel)
        {
            GGUser ggUser = userPanel.GGUser;
            if (ggUser.ID == this.globalUserCache.CurrentUser.UserID)
            {
                MessageBox.Show("不能将自己移出讨论组！");
                return;
            }

            if (this.currentGroup != null && ggUser.ID == this.currentGroup.CreatorID)
            {
                MessageBox.Show("不能将群/组的创始人移除组！");
                return;
            }
            this.currentMemberList.Remove(ggUser.ID);
            this.flowLayoutPanel1.Controls.Remove(userPanel);
        }

        private GGUser resultUser = null;

        private void skinButton3_Click(object sender, EventArgs e)
        {            
            if (this.resultUser != null)
            {             
                if (this.currentMemberList.Contains(this.resultUser.UserID))
                {
                    MessageBox.Show("用户已经在讨论组中！");
                    return;
                }
                this.currentMemberList.Add(this.resultUser.UserID);
            }
        }

        public List<string> GetGroupMembers()
        {           
            return this.currentMemberList.GetAll();
        }    


        private void skinButton_select_Click(object sender, EventArgs e)
        {
            UserSelectedForm form = new UserSelectedForm();
            form.Initialize(this.globalUserCache,this.currentGroup);
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.AddPanelItems(form.UserIDSelected);  
            }
        }


        private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {

        }
    }
}
