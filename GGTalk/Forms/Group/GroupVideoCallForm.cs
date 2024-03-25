using CCWin.Win32;
using CCWin.Win32.Const;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TalkBase.Client;
using TalkBase.Client.Bridges;

namespace GGTalk
{
    public partial class GroupVideoCallForm : BaseForm,IGroupVideoForm
    {
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        private string videoGroupID;
        private GGUser requestor;
        private List<string> memberIDList;
        public GroupVideoCallForm(ResourceCenter<GGUser, GGGroup> center ,string groupID, GGUser requestor,List<string> memberIDList)
        {
            InitializeComponent();
            this.videoGroupID = groupID;
            this.requestor = requestor;
            this.memberIDList = memberIDList;
            this.resourceCenter = center;
            this.Text = requestor.DisplayName + "邀请你进行群视频聊天";
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.resourceCenter.Connected)
                {
                    MessageBox.Show("您已经掉线！");
                    return;
                }
                this.resourceCenter.ClientOutter.SendMediaCommunicate(requestor.UserID, TalkBase.CommunicateMediaType.GroupVideo, TalkBase.CommunicateType.Agree, this.videoGroupID);

                Form groupVideoChatForm = this.resourceCenter.ChatFormController.GetNewGroupVideoChatForm(videoGroupID);
                groupVideoChatForm.Show();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally {
                this.Close();
            }

        }

        private void skinButtomReject_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.resourceCenter.Connected)
                {
                    MessageBox.Show("您已经掉线！");
                    return;
                }
                foreach (string memberID in memberIDList)
                {
                    if (memberID == this.resourceCenter.CurrentUserID) { continue; }
                    //拒绝时发送的tag为群组视频ID：videoGroupID
                    this.resourceCenter.ClientOutter.SendMediaCommunicate(memberID, TalkBase.CommunicateMediaType.GroupVideo, TalkBase.CommunicateType.Reject, this.videoGroupID);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            finally {
                this.Close();
            }
        }

        private void GroupVideoCallForm_Load(object sender, EventArgs e)
        {
            this.pnlImgTx.BackgroundImage = this.requestor.GetHeadIcon(GlobalResourceManager.HeadImages).ToBitmap();
            foreach (string memberID in this.memberIDList)
            {
                if (memberID == requestor.ID||memberID==this.resourceCenter.CurrentUserID) { continue; }
                GGUser ggUser= this.resourceCenter.ClientGlobalCache.GetUser(memberID);
                if (ggUser == null) { continue; }
                CCWin.SkinControl.SkinPanel skinPanel = new CCWin.SkinControl.SkinPanel();
                skinPanel.BackColor = System.Drawing.Color.Transparent;
                skinPanel.Size = new System.Drawing.Size(30, 30);
                skinPanel.Radius = 4;
                skinPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
                skinPanel.BackgroundImage=ggUser.GetHeadIcon(GlobalResourceManager.HeadImages).ToBitmap();
                skinPanel.Tag = memberID;
                this.flowLayoutPanel1.Controls.Add(skinPanel);
            }
            //初始化窗口出现位置
            Point p = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
            this.PointToScreen(p);
            this.Location = p;
            NativeMethods.AnimateWindow(this.Handle, 130, AW.AW_SLIDE + AW.AW_VER_NEGATIVE);//开始窗体动画
        }

        #region IGroupVideoForm
        /// <summary>
        /// 收到拒绝加入群聊 的处理方法
        /// </summary>
        /// <param name="rejecterID">拒绝者id</param>
        public void OnRejectJoinReceived(string rejecterID)
        {
            foreach (Control panel in this.flowLayoutPanel1.Controls)
            {
                if (panel.Tag != null && panel.Tag.ToString() == rejecterID)
                {
                    this.flowLayoutPanel1.Controls.Remove(panel);
                    return;
                }
            }
        }

        public void OnGroupVideoAnswerOnOtherDevice()
        {
            this.Close();
        } 
        #endregion
    }
}
