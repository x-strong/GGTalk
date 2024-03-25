namespace TalkBase.Client.UnitViews
{
    partial class UnitListBox
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnitListBox));
            this.chatListBox = new CCWin.SkinControl.ChatListBox();
            this.skinContextMenuStrip_user = new CCWin.SkinControl.SkinContextMenuStrip();
            this.toolStripMenuItem51 = new System.Windows.Forms.ToolStripMenuItem();
            this.消息记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.修改备注姓名ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_moveFriend = new System.Windows.Forms.ToolStripMenuItem();
            this.删除好友ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skinContextMenuStrip1 = new CCWin.SkinControl.SkinContextMenuStrip();
            this.修改名称ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加分组ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除分组ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.skinContextMenuStrip_user.SuspendLayout();
            this.skinContextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chatListBox
            // 
            this.chatListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chatListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatListBox.DrawContentType = CCWin.SkinControl.DrawContentType.PersonalMsg;
            this.chatListBox.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(134)));
            this.chatListBox.ForeColor = System.Drawing.Color.Black;
            this.chatListBox.FriendsMobile = true;
            this.chatListBox.ListHadOpenGroup = null;
            this.chatListBox.ListSubItemMenu = this.skinContextMenuStrip_user;
            this.chatListBox.Location = new System.Drawing.Point(0, 0);
            this.chatListBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.chatListBox.Name = "chatListBox";
            this.chatListBox.SelectSubItem = null;
            this.chatListBox.ShowNicName = false;
            this.chatListBox.Size = new System.Drawing.Size(272, 738);
            this.chatListBox.SubItemMenu = this.skinContextMenuStrip1;
            this.chatListBox.TabIndex = 0;
            this.chatListBox.Text = "chatListBox1";
            this.chatListBox.ClickSubItem += new CCWin.SkinControl.ChatListBox.ChatListSubItemEventHandler(this.chatListBox_ClickSubItem);
            this.chatListBox.DoubleClickSubItem += new CCWin.SkinControl.ChatListBox.ChatListEventHandler(this.chatListBox_DoubleClickSubItem);
            this.chatListBox.MouseEnterHead += new CCWin.SkinControl.ChatListBox.ChatListEventHandler(this.chatShow_MouseEnterHead);
            this.chatListBox.MouseLeaveHead += new CCWin.SkinControl.ChatListBox.ChatListEventHandler(this.chatShow_MouseLeaveHead);
            this.chatListBox.DragSubItemDrop += new CCWin.SkinControl.ChatListBox.DragListEventHandler(this.chatListBox_DragSubItemDrop);
            this.chatListBox.BeforeListSubItemMenuShow += new CCWin.SkinControl.ChatListBox.ChatListSubItemEventHandler(this.chatListBox_BeforeListSubItemMenuShow);
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
            this.skinContextMenuStrip_user.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.skinContextMenuStrip_user.ItemAnamorphosis = false;
            this.skinContextMenuStrip_user.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_user.ItemBorderShow = false;
            this.skinContextMenuStrip_user.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_user.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_user.ItemRadius = 4;
            this.skinContextMenuStrip_user.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.None;
            this.skinContextMenuStrip_user.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem51,
            this.消息记录ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripSeparator1,
            this.修改备注姓名ToolStripMenuItem,
            this.toolStripMenuItem_moveFriend,
            this.删除好友ToolStripMenuItem});
            this.skinContextMenuStrip_user.ItemSplitter = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinContextMenuStrip_user.Name = "MenuState";
            this.skinContextMenuStrip_user.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinContextMenuStrip_user.Size = new System.Drawing.Size(237, 238);
            this.skinContextMenuStrip_user.SkinAllColor = true;
            this.skinContextMenuStrip_user.TitleAnamorphosis = false;
            this.skinContextMenuStrip_user.TitleColor = System.Drawing.Color.White;
            this.skinContextMenuStrip_user.TitleRadius = 4;
            this.skinContextMenuStrip_user.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // toolStripMenuItem51
            // 
            this.toolStripMenuItem51.Image = global::GGTalk.Properties.Resources.sendout_icon;
            this.toolStripMenuItem51.Name = "toolStripMenuItem51";
            this.toolStripMenuItem51.Size = new System.Drawing.Size(236, 38);
            this.toolStripMenuItem51.Tag = "1";
            this.toolStripMenuItem51.Text = "发送消息";
            this.toolStripMenuItem51.ToolTipText = "表示希望好友看到您在线。\r\n声音：开启\r\n消息提醒框：开启\r\n会话消息：任务栏头像闪动\r\n";
            this.toolStripMenuItem51.Click += new System.EventHandler(this.toolStripMenuItem51_Click);
            // 
            // 消息记录ToolStripMenuItem
            // 
            this.消息记录ToolStripMenuItem.Image = global::GGTalk.Properties.Resources.record_icon;
            this.消息记录ToolStripMenuItem.Name = "消息记录ToolStripMenuItem";
            this.消息记录ToolStripMenuItem.Size = new System.Drawing.Size(236, 38);
            this.消息记录ToolStripMenuItem.Text = "消息记录";
            this.消息记录ToolStripMenuItem.Click += new System.EventHandler(this.消息记录ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Image = global::GGTalk.Properties.Resources.data_icon;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(236, 38);
            this.toolStripMenuItem1.Text = "查看资料";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(233, 6);
            // 
            // 修改备注姓名ToolStripMenuItem
            // 
            this.修改备注姓名ToolStripMenuItem.Image = global::GGTalk.Properties.Resources.remarks_icon;
            this.修改备注姓名ToolStripMenuItem.Name = "修改备注姓名ToolStripMenuItem";
            this.修改备注姓名ToolStripMenuItem.Size = new System.Drawing.Size(236, 38);
            this.修改备注姓名ToolStripMenuItem.Text = "修改备注名称";
            this.修改备注姓名ToolStripMenuItem.Click += new System.EventHandler(this.修改备注姓名ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem_moveFriend
            // 
            this.toolStripMenuItem_moveFriend.Image = global::GGTalk.Properties.Resources.move_icon;
            this.toolStripMenuItem_moveFriend.Name = "toolStripMenuItem_moveFriend";
            this.toolStripMenuItem_moveFriend.Size = new System.Drawing.Size(236, 38);
            this.toolStripMenuItem_moveFriend.Text = "移动联系人至";
            this.toolStripMenuItem_moveFriend.DropDownOpening += new System.EventHandler(this.toolStripMenuItem_moveFriend_DropDownOpening);
            this.toolStripMenuItem_moveFriend.Click += new System.EventHandler(this.toolStripMenuItem_moveFriend_Click);
            // 
            // 删除好友ToolStripMenuItem
            // 
            this.删除好友ToolStripMenuItem.Image = global::GGTalk.Properties.Resources.data_icon;
            this.删除好友ToolStripMenuItem.Name = "删除好友ToolStripMenuItem";
            this.删除好友ToolStripMenuItem.Size = new System.Drawing.Size(236, 38);
            this.删除好友ToolStripMenuItem.Text = "删除好友";
            this.删除好友ToolStripMenuItem.Click += new System.EventHandler(this.删除好友ToolStripMenuItem_Click);
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
            this.修改名称ToolStripMenuItem,
            this.添加分组ToolStripMenuItem,
            this.删除分组ToolStripMenuItem});
            this.skinContextMenuStrip1.ItemSplitter = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip1.Name = "skinContextMenuStrip1";
            this.skinContextMenuStrip1.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinContextMenuStrip1.Size = new System.Drawing.Size(185, 118);
            this.skinContextMenuStrip1.SkinAllColor = true;
            this.skinContextMenuStrip1.TitleAnamorphosis = true;
            this.skinContextMenuStrip1.TitleColor = System.Drawing.Color.White;
            this.skinContextMenuStrip1.TitleRadius = 4;
            this.skinContextMenuStrip1.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // 修改名称ToolStripMenuItem
            // 
            this.修改名称ToolStripMenuItem.Name = "修改名称ToolStripMenuItem";
            this.修改名称ToolStripMenuItem.Size = new System.Drawing.Size(184, 38);
            this.修改名称ToolStripMenuItem.Text = "修改组名";
            this.修改名称ToolStripMenuItem.Click += new System.EventHandler(this.修改名称ToolStripMenuItem_Click);
            // 
            // 添加分组ToolStripMenuItem
            // 
            this.添加分组ToolStripMenuItem.Name = "添加分组ToolStripMenuItem";
            this.添加分组ToolStripMenuItem.Size = new System.Drawing.Size(184, 38);
            this.添加分组ToolStripMenuItem.Text = "添加分组";
            this.添加分组ToolStripMenuItem.Click += new System.EventHandler(this.添加分组ToolStripMenuItem_Click);
            // 
            // 删除分组ToolStripMenuItem
            // 
            this.删除分组ToolStripMenuItem.Name = "删除分组ToolStripMenuItem";
            this.删除分组ToolStripMenuItem.Size = new System.Drawing.Size(184, 38);
            this.删除分组ToolStripMenuItem.Text = "删除分组";
            this.删除分组ToolStripMenuItem.Click += new System.EventHandler(this.删除分组ToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Group1.png");
            this.imageList1.Images.SetKeyName(1, "user4.png");
            // 
            // UnitListBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.chatListBox);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "UnitListBox";
            this.Size = new System.Drawing.Size(272, 738);
            this.skinContextMenuStrip_user.ResumeLayout(false);
            this.skinContextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.ChatListBox chatListBox;
        private CCWin.SkinControl.SkinContextMenuStrip skinContextMenuStrip_user;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem51;
        private System.Windows.Forms.ToolStripMenuItem 消息记录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除好友ToolStripMenuItem;
        private CCWin.SkinControl.SkinContextMenuStrip skinContextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 修改名称ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加分组ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除分组ToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem 修改备注姓名ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_moveFriend;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}
