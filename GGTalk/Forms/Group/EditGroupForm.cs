using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CCWin;
using CCWin.Win32;
using CCWin.Win32.Const;
using System.Diagnostics;
using System.Configuration;
using ESPlus.Rapid;
using TalkBase.Client;

namespace GGTalk
{
    /// <summary>
    /// 编辑群组的窗体。
    /// </summary>
    internal partial class EditGroupForm : BaseForm
    {
        private ResourceCenter<GGUser, GGGroup > resourceCenter;
        private GGGroup currentGroup;

        public EditGroupForm(ResourceCenter<GGUser, GGGroup > center, GGGroup group)
        {
            InitializeComponent();
            this.resourceCenter = center;
            this.currentGroup = group;
            this.editGroupControl1.Initialize(this.resourceCenter.ClientGlobalCache, group);
        }        

        private void skinButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> list = this.editGroupControl1.GetGroupMembers();
                if (!list.Contains(this.currentGroup.CreatorID))
                {
                    list.Add(this.currentGroup.CreatorID);
                }

                this.resourceCenter.ClientOutter.ChangeGroupMembers(this.currentGroup.ID, list);               
                MessageBoxEx.Show("操作成功！");
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show("编辑组成员失败！" + ee.Message);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {            
            this.btnClose_Click(sender, e);
        }         
    }

    public class GroupMemberItem
    {
        public GroupMemberItem() { }
        public GroupMemberItem(string id ,string name, Image head)
        {
            this.ID = id;
            this.Name = name;
            this.HeadImage = head;           
        }
        public string ID { get; set; }
        public Image HeadImage { get; set; }   
        public string Name { get; set; }
          
    }
}
