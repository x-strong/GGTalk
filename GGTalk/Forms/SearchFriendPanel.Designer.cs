namespace GGTalk
{
    partial class SearchFriendPanel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.skinLabel_noResult = new CCWin.SkinControl.SkinLabel();
            this.chatListBox = new CCWin.SkinControl.ChatListBox();
            this.skinContextMenuStrip_user = new CCWin.SkinControl.SkinContextMenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.发送消息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BaseText = new CCWin.SkinControl.SkinWaterTextBox();
            this.skinTextBox_id = new CCWin.SkinControl.SkinTextBox();
            this.skinContextMenuStrip_user.SuspendLayout();
            this.skinTextBox_id.SuspendLayout();
            this.SuspendLayout();
            // 
            // skinLabel_noResult
            // 
            this.skinLabel_noResult.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.skinLabel_noResult.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel_noResult.AutoSize = true;
            this.skinLabel_noResult.BackColor = System.Drawing.Color.White;
            this.skinLabel_noResult.BorderColor = System.Drawing.Color.White;
            this.skinLabel_noResult.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel_noResult.Location = new System.Drawing.Point(63, 45);
            this.skinLabel_noResult.Name = "skinLabel_noResult";
            this.skinLabel_noResult.Size = new System.Drawing.Size(77, 17);
            this.skinLabel_noResult.TabIndex = 138;
            this.skinLabel_noResult.Text = "无搜索结果...";
            this.skinLabel_noResult.Visible = false;
            // 
            // chatListBox
            // 
            this.chatListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chatListBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.chatListBox.DrawContentType = CCWin.SkinControl.DrawContentType.PersonalMsg;
            this.chatListBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chatListBox.ForeColor = System.Drawing.Color.Black;
            this.chatListBox.FriendsMobile = false;
            this.chatListBox.ListHadOpenGroup = null;
            this.chatListBox.ListSubItemMenu = this.skinContextMenuStrip_user;
            this.chatListBox.Location = new System.Drawing.Point(-1, 31);
            this.chatListBox.Margin = new System.Windows.Forms.Padding(0);
            this.chatListBox.Name = "chatListBox";
            this.chatListBox.SelectSubItem = null;
            this.chatListBox.ShowNicName = false;
            this.chatListBox.Size = new System.Drawing.Size(250, 396);
            this.chatListBox.SubItemMenu = null;
            this.chatListBox.TabIndex = 137;
            this.chatListBox.DoubleClickSubItem += new CCWin.SkinControl.ChatListBox.ChatListEventHandler(this.chatListBox_DoubleClickSubItem);
            // 
            // skinContextMenuStrip_user
            // 
            this.skinContextMenuStrip_user.Arrow = System.Drawing.Color.Gray;
            this.skinContextMenuStrip_user.Back = System.Drawing.Color.White;
            this.skinContextMenuStrip_user.BackRadius = 4;
            this.skinContextMenuStrip_user.Base = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(200)))), ((int)(((byte)(254)))));
            this.skinContextMenuStrip_user.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinContextMenuStrip_user.Fore = System.Drawing.Color.Black;
            this.skinContextMenuStrip_user.HoverFore = System.Drawing.Color.White;
            this.skinContextMenuStrip_user.ImageScalingSize = new System.Drawing.Size(11, 11);
            this.skinContextMenuStrip_user.ItemAnamorphosis = false;
            this.skinContextMenuStrip_user.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_user.ItemBorderShow = false;
            this.skinContextMenuStrip_user.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_user.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_user.ItemRadius = 4;
            this.skinContextMenuStrip_user.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.None;
            this.skinContextMenuStrip_user.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.发送消息ToolStripMenuItem});
            this.skinContextMenuStrip_user.ItemSplitter = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinContextMenuStrip_user.Name = "MenuState";
            this.skinContextMenuStrip_user.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinContextMenuStrip_user.Size = new System.Drawing.Size(125, 48);
            this.skinContextMenuStrip_user.SkinAllColor = true;
            this.skinContextMenuStrip_user.TitleAnamorphosis = false;
            this.skinContextMenuStrip_user.TitleColor = System.Drawing.Color.White;
            this.skinContextMenuStrip_user.TitleRadius = 4;
            this.skinContextMenuStrip_user.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem1.Text = "查看资料";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // 发送消息ToolStripMenuItem
            // 
            this.发送消息ToolStripMenuItem.Name = "发送消息ToolStripMenuItem";
            this.发送消息ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.发送消息ToolStripMenuItem.Text = "发送消息";
            this.发送消息ToolStripMenuItem.Click += new System.EventHandler(this.发送消息ToolStripMenuItem_Click);
            // 
            // BaseText
            // 
            this.BaseText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BaseText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BaseText.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.BaseText.Location = new System.Drawing.Point(5, 5);
            this.BaseText.Name = "BaseText";
            this.BaseText.Size = new System.Drawing.Size(240, 18);
            this.BaseText.TabIndex = 0;
            this.BaseText.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.BaseText.WaterText = "请输入账号";
            this.BaseText.TextChanged += new System.EventHandler(this.skinTextBox_id_SkinTxt_TextChanged);
            // 
            // skinTextBox_id
            // 
            this.skinTextBox_id.BackColor = System.Drawing.Color.Transparent;
            this.skinTextBox_id.CloseButtonVisiable = true;
            this.skinTextBox_id.Dock = System.Windows.Forms.DockStyle.Top;
            this.skinTextBox_id.Icon = null;
            this.skinTextBox_id.IconIsButton = false;
            this.skinTextBox_id.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.skinTextBox_id.Location = new System.Drawing.Point(0, 0);
            this.skinTextBox_id.Margin = new System.Windows.Forms.Padding(0);
            this.skinTextBox_id.MinimumSize = new System.Drawing.Size(28, 28);
            this.skinTextBox_id.MouseBack = null;
            this.skinTextBox_id.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.skinTextBox_id.Name = "skinTextBox_id";
            this.skinTextBox_id.NormlBack = null;
            this.skinTextBox_id.Padding = new System.Windows.Forms.Padding(25, 5, 25, 5);
            this.skinTextBox_id.SearchButtonVisiable = true;
            this.skinTextBox_id.Size = new System.Drawing.Size(250, 30);
            // 
            // skinTextBox_id.BaseText
            // 
            this.skinTextBox_id.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skinTextBox_id.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTextBox_id.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.skinTextBox_id.SkinTxt.Location = new System.Drawing.Point(25, 5);
            this.skinTextBox_id.SkinTxt.Name = "BaseText";
            this.skinTextBox_id.SkinTxt.Size = new System.Drawing.Size(200, 18);
            this.skinTextBox_id.SkinTxt.TabIndex = 0;
            this.skinTextBox_id.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinTextBox_id.SkinTxt.WaterText = "请输入账号，按回车查询";
            this.skinTextBox_id.SkinTxt.TextChanged += new System.EventHandler(this.skinTextBox_id_SkinTxt_TextChanged);
            this.skinTextBox_id.TabIndex = 140;
            this.skinTextBox_id.CloseButtonClicked += new System.EventHandler(this.skinTextBox_id_CloseButtonClicked);
            this.skinTextBox_id.EnterKeyInput += new System.EventHandler(this.btnFind_Click);
            // 
            // SearchFriendPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.skinTextBox_id);
            this.Controls.Add(this.skinLabel_noResult);
            this.Controls.Add(this.chatListBox);
            this.Name = "SearchFriendPanel";
            this.Size = new System.Drawing.Size(250, 428);
            this.VisibleChanged += new System.EventHandler(this.SearchFriendPanel_VisibleChanged);
            this.skinContextMenuStrip_user.ResumeLayout(false);
            this.skinTextBox_id.ResumeLayout(false);
            this.skinTextBox_id.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CCWin.SkinControl.SkinLabel skinLabel_noResult;
        private CCWin.SkinControl.ChatListBox chatListBox;
        private CCWin.SkinControl.SkinContextMenuStrip skinContextMenuStrip_user;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 发送消息ToolStripMenuItem;
        private CCWin.SkinControl.SkinWaterTextBox BaseText;
        private CCWin.SkinControl.SkinTextBox skinTextBox_id;
    }
}
