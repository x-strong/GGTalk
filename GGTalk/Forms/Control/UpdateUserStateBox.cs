using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GGTalk.Models;
using ESBasic;
using TalkBase;
using TalkBase.Client;
using CCWin;

namespace GGTalk.Forms.Control
{
    public partial class UpdateUserStateBox : UserControl
    {
        private IUser currentUser;
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        public UpdateUserStateBox()
        {
            InitializeComponent();
            this.SetComboBox();
        }

        public void Initialize(ResourceCenter<GGUser, GGGroup> resourceCenter)
        {
            this.resourceCenter = resourceCenter;
        }

        private void SetComboBox()
        {
            List<ListItem<int>> listItems = new List<ListItem<int>>();
            listItems.Add(new ListItem<int>(EnumDescription.GetFieldText(UserState.Normal), (int)UserState.Normal));
            listItems.Add(new ListItem<int>(EnumDescription.GetFieldText(UserState.Frozen), (int)UserState.Frozen));
            listItems.Add(new ListItem<int>(EnumDescription.GetFieldText(UserState.NoSpeaking), (int)UserState.NoSpeaking));
            listItems.Add(new ListItem<int>(EnumDescription.GetFieldText(UserState.StopUsing), (int)UserState.StopUsing));
            this.skinComboBox1.DisplayMember = "Key";
            this.skinComboBox1.ValueMember = "Value";
            this.skinComboBox1.DataSource = listItems;
        }

        private void skinButton_query_Click(object sender, EventArgs e)
        {
            string userID= this.skinTextBox_id.SkinTxt.Text.Trim();
            this.currentUser= this.resourceCenter.ClientGlobalCache.GetUser(userID);
            if (this.currentUser == null)
            {
                this.SetResult(false);
                this.skinLabel_result.Text = "查询无此用户！";
                return;
            }
            this.SetResult(true);
            this.skinLabel_result.Text = string.Format("用户编号：{0}  昵称：{1}  状态：{2}", this.currentUser.ID, this.currentUser.Name, EnumDescription.GetFieldText(this.currentUser.UserState));
            this.skinComboBox1.SelectedIndex = (int)this.currentUser.UserState;
        }

        private void SetResult(bool haveResult)
        {
            this.skinLabel_state.Visible = haveResult;
            this.skinComboBox1.Visible = haveResult;
        }


        private void skinButton_submit_Click(object sender, EventArgs e)
        {
            if (!this.skinComboBox1.Visible)
            {
                return;
            }
            if (this.currentUser == null)
            {
                return;
            }
            UserState userState = (UserState)this.skinComboBox1.SelectedIndex;
            //if (userState == this.currentUser.UserState)
            //{
            //    return;
            //}
            this.resourceCenter.ClientOutter.ChangeUserState4Admin(this.currentUser.ID, userState);
            this.currentUser.UserState = userState;
            MessageBoxEx.Show("修改成功！");
        }
    }
}
