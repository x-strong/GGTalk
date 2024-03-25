using TalkBase.Client.UnitViews;

namespace GGTalk
{
    partial class CombinedChatForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CombinedChatForm));
            CCWin.CmSysButton cmSysButton1 = new CCWin.CmSysButton();
            this.panel_left = new System.Windows.Forms.Panel();
            this.userListBox1 = new TalkBase.Client.UnitViews.UnitListBox();
            this.skinContextMenuStrip1 = new CCWin.SkinControl.SkinContextMenuStrip();
            this.移除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skinLabel14 = new CCWin.SkinControl.SkinLabel();
            this.skinContextMenuStrip_sys = new CCWin.SkinControl.SkinContextMenuStrip();
            this.toolStripMenuItem_topmost = new System.Windows.Forms.ToolStripMenuItem();
            this.panel_left.SuspendLayout();
            this.skinContextMenuStrip1.SuspendLayout();
            this.skinContextMenuStrip_sys.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_left
            // 
            this.panel_left.Controls.Add(this.userListBox1);
            this.panel_left.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_left.Location = new System.Drawing.Point(4, 28);
            this.panel_left.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel_left.Name = "panel_left";
            this.panel_left.Size = new System.Drawing.Size(276, 1094);
            this.panel_left.TabIndex = 3;
            // 
            // userListBox1
            // 
            this.userListBox1.AutoSize = true;
            this.userListBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(236)))), ((int)(((byte)(240)))));
            this.userListBox1.CatalogContextMenuVisiable = false;
            this.userListBox1.DefaultFriendCatalogName = "我的好友";
            this.userListBox1.DefaultGroupCatalogName = "我的群";
            this.userListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userListBox1.DrawContentType = CCWin.SkinControl.DrawContentType.None;
            this.userListBox1.FriendsMobile = false;
            this.userListBox1.IconSizeMode = CCWin.SkinControl.ChatListItemIcon.Small;
            this.userListBox1.Location = new System.Drawing.Point(0, 0);
            this.userListBox1.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.userListBox1.Name = "userListBox1";
            this.userListBox1.PreLoadCatalog = false;
            this.userListBox1.Size = new System.Drawing.Size(276, 1094);
            this.userListBox1.TabIndex = 1;
            this.userListBox1.UserContextMenuVisiable = false;
            // 
            // skinContextMenuStrip1
            // 
            this.skinContextMenuStrip1.Arrow = System.Drawing.Color.Black;
            this.skinContextMenuStrip1.Back = System.Drawing.Color.White;
            this.skinContextMenuStrip1.BackRadius = 4;
            this.skinContextMenuStrip1.Base = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(200)))), ((int)(((byte)(254)))));
            this.skinContextMenuStrip1.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinContextMenuStrip1.Fore = System.Drawing.Color.Black;
            this.skinContextMenuStrip1.HoverFore = System.Drawing.Color.White;
            this.skinContextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.skinContextMenuStrip1.ItemAnamorphosis = true;
            this.skinContextMenuStrip1.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip1.ItemBorderShow = true;
            this.skinContextMenuStrip1.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip1.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip1.ItemRadius = 4;
            this.skinContextMenuStrip1.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.移除ToolStripMenuItem});
            this.skinContextMenuStrip1.ItemSplitter = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip1.Name = "skinContextMenuStrip1";
            this.skinContextMenuStrip1.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinContextMenuStrip1.Size = new System.Drawing.Size(137, 42);
            this.skinContextMenuStrip1.SkinAllColor = true;
            this.skinContextMenuStrip1.TitleAnamorphosis = true;
            this.skinContextMenuStrip1.TitleColor = System.Drawing.Color.White;
            this.skinContextMenuStrip1.TitleRadius = 4;
            this.skinContextMenuStrip1.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // 移除ToolStripMenuItem
            // 
            this.移除ToolStripMenuItem.Name = "移除ToolStripMenuItem";
            this.移除ToolStripMenuItem.Size = new System.Drawing.Size(136, 38);
            this.移除ToolStripMenuItem.Text = "移除";
            this.移除ToolStripMenuItem.Click += new System.EventHandler(this.移除ToolStripMenuItem_Click);
            // 
            // skinLabel14
            // 
            this.skinLabel14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.skinLabel14.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel14.AutoSize = true;
            this.skinLabel14.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel14.BorderColor = System.Drawing.Color.White;
            this.skinLabel14.Cursor = System.Windows.Forms.Cursors.Hand;
            this.skinLabel14.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel14.ForeColor = System.Drawing.Color.DimGray;
            this.skinLabel14.Location = new System.Drawing.Point(1168, 10);
            this.skinLabel14.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.skinLabel14.Name = "skinLabel14";
            this.skinLabel14.Size = new System.Drawing.Size(58, 24);
            this.skinLabel14.TabIndex = 141;
            this.skinLabel14.Text = "举报";
            this.skinLabel14.Click += new System.EventHandler(this.skinLabel14_Click);
            // 
            // skinContextMenuStrip_sys
            // 
            this.skinContextMenuStrip_sys.Arrow = System.Drawing.Color.Black;
            this.skinContextMenuStrip_sys.Back = System.Drawing.Color.White;
            this.skinContextMenuStrip_sys.BackRadius = 4;
            this.skinContextMenuStrip_sys.Base = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(200)))), ((int)(((byte)(254)))));
            this.skinContextMenuStrip_sys.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinContextMenuStrip_sys.Fore = System.Drawing.Color.Black;
            this.skinContextMenuStrip_sys.HoverFore = System.Drawing.Color.White;
            this.skinContextMenuStrip_sys.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.skinContextMenuStrip_sys.ItemAnamorphosis = false;
            this.skinContextMenuStrip_sys.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_sys.ItemBorderShow = false;
            this.skinContextMenuStrip_sys.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_sys.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_sys.ItemRadius = 4;
            this.skinContextMenuStrip_sys.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.None;
            this.skinContextMenuStrip_sys.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_topmost});
            this.skinContextMenuStrip_sys.ItemSplitter = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinContextMenuStrip_sys.Name = "MenuState";
            this.skinContextMenuStrip_sys.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinContextMenuStrip_sys.Size = new System.Drawing.Size(233, 42);
            this.skinContextMenuStrip_sys.SkinAllColor = true;
            this.skinContextMenuStrip_sys.TitleAnamorphosis = false;
            this.skinContextMenuStrip_sys.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(228)))), ((int)(((byte)(236)))));
            this.skinContextMenuStrip_sys.TitleRadius = 4;
            this.skinContextMenuStrip_sys.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // toolStripMenuItem_topmost
            // 
            this.toolStripMenuItem_topmost.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItem_topmost.Name = "toolStripMenuItem_topmost";
            this.toolStripMenuItem_topmost.Size = new System.Drawing.Size(232, 38);
            this.toolStripMenuItem_topmost.Text = "保持窗口最前";
            this.toolStripMenuItem_topmost.Click += new System.EventHandler(this.toolStripMenuItem_topmost_Click);
            // 
            // CombinedChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CanResize = true;
            this.ClientSize = new System.Drawing.Size(1514, 1126);
            this.Controls.Add(this.skinLabel14);
            this.Controls.Add(this.panel_left);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.MinimumSize = new System.Drawing.Size(1484, 1110);
            this.Name = "CombinedChatForm";
            this.ShowInTaskbar = true;
            cmSysButton1.Bounds = new System.Drawing.Rectangle(1391, -1, 28, 20);
            cmSysButton1.BoxState = CCWin.ControlBoxState.Normal;
            cmSysButton1.Location = new System.Drawing.Point(1391, -1);
            cmSysButton1.Name = null;
            cmSysButton1.OwnerForm = this;
            cmSysButton1.SysButtonDown = global::GGTalk.Properties.Resources.AIO_SetBtn_down;
            cmSysButton1.SysButtonMouse = global::GGTalk.Properties.Resources.AIO_SetBtn_highlight;
            cmSysButton1.SysButtonNorml = global::GGTalk.Properties.Resources.AIO_SetBtn_normal;
            cmSysButton1.ToolTip = null;
            this.SysButtonItems.AddRange(new CCWin.CmSysButton[] {
            cmSysButton1});
            this.TitleSuitColor = true;
            this.SysBottomClick += new CCWin.CCSkinMain.SysBottomEventHandler(this.CombinedChatForm_SysBottomClick);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CombinedChatForm_FormClosing);
            this.SizeChanged += new System.EventHandler(this.CombinedChatForm_SizeChanged);
            this.panel_left.ResumeLayout(false);
            this.panel_left.PerformLayout();
            this.skinContextMenuStrip1.ResumeLayout(false);
            this.skinContextMenuStrip_sys.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UnitListBox userListBox1;
        private System.Windows.Forms.Panel panel_left;
        private CCWin.SkinControl.SkinContextMenuStrip skinContextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 移除ToolStripMenuItem;
        private CCWin.SkinControl.SkinLabel skinLabel14;
        private CCWin.SkinControl.SkinContextMenuStrip skinContextMenuStrip_sys;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_topmost;

    }
}