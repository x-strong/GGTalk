using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CCWin.SkinControl;
using ESPlus.Rapid;
using CCWin;
using ESBasic.ObjectManagement.Managers;
using ESBasic;
using ESBasic.ObjectManagement.Forms;
using System.Diagnostics;
using TalkBase.Client;
using TalkBase.Client.Application;
using System.Runtime.InteropServices;
using System.Threading;
using TalkBase;
using TalkBase.Client.Bridges;

namespace GGTalk
{
    /// <summary>
    /// 合并模式的聊天窗口。
    /// </summary>
    public partial class CombinedChatForm : BaseForm, IUnitInfoProvider, IChatContainerForm
    {
        private IUnit dialogUnit = null;
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        private ObjectManager<string, Form> chatFormManager = new ObjectManager<string, Form>();  

        public event CbGeneric<string> UnitClicked;
        public event CbGeneric<Form ,IUnit> FormCreated;        

        #region Vibration 震动
        public void HandleVibration()
        {
            if (this.TopMost)
            {
                this.Focus();
            }
            else
            {
                this.TopMost = true;
                Vibration();
                this.TopMost = false;
            }
        }

        //震动方法
        private void Vibration()
        {
            Point pOld = this.Location;//原来的位置
            int radius = 3;//半径
            for (int n = 0; n < 3; n++) //旋转圈数
            {
                //右半圆逆时针
                for (int i = -radius; i <= radius; i++)
                {
                    int x = Convert.ToInt32(Math.Sqrt(radius * radius - i * i));
                    int y = -i;

                    this.Location = new Point(pOld.X + x, pOld.Y + y);
                    Thread.Sleep(10);
                }
                //左半圆逆时针
                for (int j = radius; j >= -radius; j--)
                {
                    int x = -Convert.ToInt32(Math.Sqrt(radius * radius - j * j));
                    int y = -j;

                    this.Location = new Point(pOld.X + x, pOld.Y + y);
                    Thread.Sleep(10);
                }
            }
            //抖动完成，恢复原来位置
            this.Location = pOld;
        }      
        #endregion

        private void CombinedChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.chatFormManager.Count > 0)
            {
                if (!ESBasic.Helpers.WindowsHelper.ShowQuery("还有聊天窗口尚未关闭，您确定要关闭所有聊天窗口吗？"))
                {
                    e.Cancel = true;
                    return;
                }
            }

            this.resourceCenter.ClientGlobalCache.UserBaseInfoChanged -= new CbGeneric<GGUser>(globalUserCache_FriendInfoChanged);
            this.resourceCenter.ClientGlobalCache.UserStatusChanged -= new CbGeneric<GGUser>(globalUserCache_FriendStatusChanged);
        }

        public CombinedChatForm(ResourceCenter<GGUser, GGGroup> center)
        {
            InitializeComponent();
            this.Resize += new EventHandler(CombinedChatForm_Resize);

            this.toolStripMenuItem_topmost.Checked = SystemSettings.Singleton.ChatFormTopMost;
            this.userListBox1.SetUserContextMenuStrip(this.skinContextMenuStrip1);
            this.TopMost = SystemSettings.Singleton.ChatFormTopMost;

            this.resourceCenter = center;          
            this.Text = GlobalResourceManager.SoftwareName + " - 聊天窗口";
            this.BackgroundImage = GlobalResourceManager.MainBackImage;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.AutoScroll = false;
            this.HorizontalScroll.Visible = false;
            this.VerticalScroll.Visible = false;
            //this.AutoScrollMinSize = new Size(Screen.PrimaryScreen.Bounds.Width + 1, Screen.PrimaryScreen.Bounds.Height + 1);
            //this.IsMdiContainer = true;

            this.resourceCenter.ClientGlobalCache.UserBaseInfoChanged += new CbGeneric<GGUser>(globalUserCache_FriendInfoChanged);
            this.resourceCenter.ClientGlobalCache.UserStatusChanged += new CbGeneric<GGUser>(globalUserCache_FriendStatusChanged);

            this.userListBox1.Initialize(this.resourceCenter.ClientGlobalCache.CurrentUser, this);
            this.userListBox1.CatalogContextMenuVisiable = false;
            this.userListBox1.UserContextMenuVisiable = false;
            this.userListBox1.UnitDoubleClicked += new ESBasic.CbGeneric<IUnit>(userListBox1_UnitDoubleClicked);
            this.userListBox1.UnitClicked += new ESBasic.CbGeneric<IUnit>(userListBox1_UnitClicked);
        }       

        void CombinedChatForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                return;
            }

            if (this.dialogUnit != null)
            {
                IChatForm form = (IChatForm)this.chatFormManager.Get(this.dialogUnit.ID);
                form.RefreshUI();
            }
        }       

        void globalUserCache_FriendStatusChanged(GGUser user)
        {
            GlobalResourceManager.UiSafeInvoker.ActionOnUI<GGUser>(this.do_globalUserCache_FriendStatusChanged, user);
        }

        private void do_globalUserCache_FriendStatusChanged(GGUser user)
        {
            this.userListBox1.UserStatusChanged(user);
        }

        void globalUserCache_FriendInfoChanged(GGUser user)
        {
            GlobalResourceManager.UiSafeInvoker.ActionOnUI<GGUser>(this.do_globalUserCache_FriendInfoChanged, user);
        }

        void do_globalUserCache_FriendInfoChanged(GGUser user)
        {
            this.userListBox1.UnitInfoChanged(user);
        }        

        void userListBox1_UnitClicked(IUnit unit)
        {
            this.userListBox1_UnitDoubleClicked(unit);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);//设置此窗体为活动窗体       

        void userListBox1_UnitDoubleClicked(IUnit unit)
        {
            Form oldForm = this.chatFormManager.Get(this.dialogUnit.ID);
            oldForm.Size = new Size(1,1);
            oldForm.Invalidate();           

            Form newForm = this.chatFormManager.Get(unit.ID);
            this.SetFormSize(newForm);
            newForm.DesktopLocation = new Point(0, 0);      
            newForm.Focus();
            ((IChatForm)newForm).RefreshUI();    

            this.dialogUnit = unit;
            this.userListBox1.SetTwinkleState(unit.ID, false);

            if (this.UnitClicked != null)
            {
                this.UnitClicked(unit.ID);
            }
        }      

         public void FocusOnForm(string unitID, bool createNew)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Focus();  

            Form form =createNew ? this.GetForm(unitID) : this.GetExistedForm(unitID);
            if (form != null)
            {
                form.Focus();
            }

            this.userListBox1_UnitClicked((IUnit)form.Tag);
        }

        public void CloseForm(string unitID)
        {            
            Form form = this.chatFormManager.Get(unitID);
            if (form != null)
            {
                form.Close();
            }
            this.AfterChatFormClose(unitID);
        }

        public void OnNewMessage(string unitID)
        {
            if (this.dialogUnit != null && this.dialogUnit.ID != unitID)
            {
                this.userListBox1.SetTwinkleState(unitID, true);
            }
        }

        public void FlashChatWindow()
        {
            ESBasic.Helpers.WindowsHelper.FlashWindow(this);
        }

        public Form GetExistedForm(string unitID)
        {
            return this.chatFormManager.Get(unitID);
        }       

        public List<Form> GetAllForms()
        {
            return this.chatFormManager.GetAll();
        }

        public Form GetForm(string unitID)
        {
            if (this.chatFormManager.Contains(unitID))
            {
                return this.chatFormManager.Get(unitID);
            }

            IUnit unit = this.resourceCenter.ClientGlobalCache.GetUnitExisted(unitID);           

            string catalog = this.GetCatalog(unit);
            this.userListBox1.AddUnit(unit);
            if (this.userListBox1.GetUnitCount(catalog) == 1)
            {
                this.userListBox1.ExpandCatalog(catalog);
            }

            BaseForm form = null;

            if (unit.UnitType == UnitType.Group)
            {
                GroupChatForm groupChatForm = new GroupChatForm(this.resourceCenter, unit.ID);
                groupChatForm.GroupMemberClicked += new CbGeneric<string>(groupChatForm_GroupMemberClicked);
                form = groupChatForm;
            }
            else if (unit.UnitType == UnitType.User)
            {
                GGUser friend = (GGUser)unit;
                //this.resourceCenter.RapidPassiveEngine.P2PController.P2PConnectAsyn(unitID);//尝试P2P连接。
                FriendChatForm friendChatForm = new FriendChatForm(this.resourceCenter, unit.ID);
                friendChatForm.VibrationNeeded += new CbGeneric<string>(friendChatForm_VibrationClicked);
                form = friendChatForm;
            }
            form.Tag = unit;
            this.chatFormManager.Add(unit.ID, form);
            form.FormClosing += new FormClosingEventHandler(form_FormClosing);
            form.Shown += new EventHandler(form_Shown);
            //form.MouseMove += new MouseEventHandler(form_MouseMove);
            //form.MouseDown += new MouseEventHandler(form_MouseDown);
            form.MaximizeBox = false;
            form.MinimizeBox = false;           
            //form.ControlBox = false;
            form.CanResize = false;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.DesktopLocation = new Point(0, 0);      
            
            form.Mobile = MobileStyle.None;
            form.MdiParent = this;   
            this.SetFormSize(form);
            form.Show();

            this.Visible = true;

            if (this.dialogUnit != null)
            {
                Form oldForm = this.chatFormManager.Get(this.dialogUnit.ID);
                oldForm.Size = new Size(1, 1);
                oldForm.Invalidate();        
            }
            this.dialogUnit = unit;
            this.userListBox1.SelectUnit(unitID, false);

            if (this.FormCreated != null)
            {
                this.FormCreated(form, unit);
            }

            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            SetForegroundWindow(this.Handle);
            return form;
        }

        /// <summary>
        /// 必须要设置一下，当CominedChatForm在最小化时，创建新的ChatForm，其DesktopLocation将不是严格的（0，0），从而导致出现滚动条。
        /// </summary>       
        void form_Shown(object sender, EventArgs e)
        {
            Form form = (Form)sender;
            form.DesktopLocation = new Point(0, 0);    
        }

        void friendChatForm_VibrationClicked(string userID)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            SetForegroundWindow(this.Handle);

            this.Vibration();
        }

        void groupChatForm_GroupMemberClicked(string unitID)
        {
            this.FocusOnForm(unitID, true);
        }

        void form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.lastMovePoint = this.PointToScreen( e.Location);                
            }
        }

        private Point lastMovePoint;       
        void form_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
            {
                return;
            }

            Point newPoint = this.PointToScreen(e.Location);
            int delt = Math.Abs(newPoint.X - this.lastMovePoint.X) + Math.Abs(newPoint.Y - this.lastMovePoint.Y);
            if (delt < 6)
            {
                return;
            }

            this.Location = new Point(this.Location.X + newPoint.X - this.lastMovePoint.X, this.Location.Y + newPoint.Y - this.lastMovePoint.Y);
            this.lastMovePoint = newPoint;
        }

        void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.MdiFormClosing)
            {
                return;
            }

            Form form = (Form)sender;
            form.MdiParent = null;
            IUnit unit = (IUnit)form.Tag;
            this.AfterChatFormClose(unit.ID);            
        }

        private void AfterChatFormClose(string unitID)
        {
            this.userListBox1.RemoveUnit(unitID);
            this.chatFormManager.Remove(unitID);

            List<Form> formList = this.chatFormManager.GetAll();
            if (formList.Count > 0)
            {
                this.SetFormSize(formList[0]);
                this.dialogUnit = (IUnit)formList[0].Tag;
                this.userListBox1.SelectUnit(this.dialogUnit.ID, false);
            }
            else
            {
                this.dialogUnit = null;
                this.Close();
            }
        }

        private void CombinedChatForm_SizeChanged(object sender, EventArgs e)
        {          
            foreach (Form form in this.MdiChildren)
            {
                this.SetFormSize(form );
                //form.Invalidate(true);
            }                        
        }

        private void SetFormSize(Form form )
        {           
            form.Size = new System.Drawing.Size(this.Width - this.userListBox1.Width - 12, this.Height - 36 );
            //form.Size = new System.Drawing.Size(this.Width - this.panel_left.Width - 5, this.Height - 5);
        }

        public void CloseAllForms()
        {
            this.Close();
        }



        public Image GetHeadImage(IUnit unit)
        {
            if (unit.IsUser)
            {
                return GlobalResourceManager.GetHeadImage((GGUser)unit);
            }

            //使用内置的
            return null;
        }

        public string GetCatalog(IUnit unit)
        {
            return "交谈中";
        }
        

        private void 移除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.userListBox1.SelectedUnit == null)
            {
                return;
            }

            this.CloseForm(this.userListBox1.SelectedUnit.ID);
        }

        private void CombinedChatForm_SysBottomClick(object sender, SysButtonEventArgs e)
        {
            skinContextMenuStrip_sys.Show(this, new Point(this.Width - this.CloseBoxSize.Width * 3 + 10, this.CloseBoxSize.Height + 5)); 
        }

        private void toolStripMenuItem_topmost_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItem_topmost.Checked = !this.toolStripMenuItem_topmost.Checked;
            this.TopMost = this.toolStripMenuItem_topmost.Checked;
            SystemSettings.Singleton.ChatFormTopMost = this.toolStripMenuItem_topmost.Checked;
            SystemSettings.Singleton.Save();
        }

        private void skinLabel14_Click(object sender, EventArgs e)
        {
            Process.Start(GlobalResourceManager.CompanyUrl);
        }

        public string BlackListCatalogName
        {
            get { return ""; }
        }
    }  
}
