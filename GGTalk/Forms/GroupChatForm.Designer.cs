
namespace GGTalk
{
    partial class GroupChatForm
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
            this.components = new System.ComponentModel.Container();
            CCWin.SkinControl.ChatListItem chatListItem1 = new CCWin.SkinControl.ChatListItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupChatForm));
            CCWin.SkinControl.Animation animation1 = new CCWin.SkinControl.Animation();
            this.chatListBox1 = new CCWin.SkinControl.ChatListBox();
            this.skinContextMenuStrip = new CCWin.SkinControl.SkinContextMenuStrip();
            this.禁言_toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_oneDay = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_tenDays = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_forever = new System.Windows.Forms.ToolStripMenuItem();
            this.解除禁言_toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加好友ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolShow = new System.Windows.Forms.ToolTip(this.components);
            this.skinButton_allGroupBan = new CCWin.SkinControl.SkinButton();
            this.skinButton1 = new CCWin.SkinControl.SkinButton();
            this.skinButtom_editMembers = new CCWin.SkinControl.SkinButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.skinTabControl1 = new CCWin.SkinControl.SkinTabControl();
            this.tabPage_chat = new System.Windows.Forms.TabPage();
            this.chatPanel1 = new GGTalk.ChatPanel();
            this.tabPage_file = new System.Windows.Forms.TabPage();
            this.nDiskBrowser1 = new ESFramework.Boost.NetworkDisk.Passive.NDiskBrowser();
            this.pnlTx = new CCWin.SkinControl.SkinPanel();
            this.panelFriendHeadImage = new CCWin.SkinControl.SkinPanel();
            this.labelGroupName = new CCWin.SkinControl.SkinLabel();
            this.label_announce = new CCWin.SkinControl.SkinLabel();
            this.skinPanel3 = new CCWin.SkinControl.SkinPanel();
            this.btn_groupVideo = new CCWin.SkinControl.SkinButton();
            this.skinPanel1 = new CCWin.SkinControl.SkinPanel();
            this.skinPanel2 = new CCWin.SkinControl.SkinPanel();
            this.linkLabel_softName = new System.Windows.Forms.LinkLabel();
            this.skinContextMenuStrip.SuspendLayout();
            this.skinTabControl1.SuspendLayout();
            this.tabPage_chat.SuspendLayout();
            this.tabPage_file.SuspendLayout();
            this.pnlTx.SuspendLayout();
            this.skinPanel3.SuspendLayout();
            this.skinPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chatListBox1
            // 
            this.chatListBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.chatListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatListBox1.DrawContentType = CCWin.SkinControl.DrawContentType.None;
            this.chatListBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chatListBox1.ForeColor = System.Drawing.Color.Black;
            this.chatListBox1.FriendsMobile = true;
            this.chatListBox1.IconSizeMode = CCWin.SkinControl.ChatListItemIcon.Small;
            chatListItem1.Bounds = new System.Drawing.Rectangle(0, 1, 243, 25);
            chatListItem1.IsOpen = true;
            chatListItem1.IsTwinkleHide = false;
            chatListItem1.OwnerChatListBox = this.chatListBox1;
            chatListItem1.Text = "讨论组成员";
            chatListItem1.TwinkleSubItemNumber = 0;
            this.chatListBox1.Items.AddRange(new CCWin.SkinControl.ChatListItem[] {
            chatListItem1});
            this.chatListBox1.ListHadOpenGroup = null;
            this.chatListBox1.ListSubItemMenu = null;
            this.chatListBox1.Location = new System.Drawing.Point(0, 36);
            this.chatListBox1.Name = "chatListBox1";
            this.chatListBox1.SelectSubItem = null;
            this.chatListBox1.ShowNicName = false;
            this.chatListBox1.Size = new System.Drawing.Size(243, 471);
            this.chatListBox1.SubItemMenu = null;
            this.chatListBox1.TabIndex = 159;
            this.chatListBox1.Text = "chatListBox1";
            // 
            // skinContextMenuStrip
            // 
            this.skinContextMenuStrip.Arrow = System.Drawing.Color.Black;
            this.skinContextMenuStrip.Back = System.Drawing.Color.White;
            this.skinContextMenuStrip.BackRadius = 4;
            this.skinContextMenuStrip.Base = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(200)))), ((int)(((byte)(254)))));
            this.skinContextMenuStrip.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinContextMenuStrip.Fore = System.Drawing.Color.Black;
            this.skinContextMenuStrip.HoverFore = System.Drawing.Color.White;
            this.skinContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.skinContextMenuStrip.ItemAnamorphosis = true;
            this.skinContextMenuStrip.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip.ItemBorderShow = true;
            this.skinContextMenuStrip.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip.ItemRadius = 4;
            this.skinContextMenuStrip.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.禁言_toolStripMenuItem,
            this.解除禁言_toolStripMenuItem,
            this.添加好友ToolStripMenuItem,
            this.TAToolStripMenuItem});
            this.skinContextMenuStrip.ItemSplitter = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip.Name = "skinContextMenuStrip_groupBan";
            this.skinContextMenuStrip.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinContextMenuStrip.Size = new System.Drawing.Size(125, 92);
            this.skinContextMenuStrip.SkinAllColor = true;
            this.skinContextMenuStrip.TitleAnamorphosis = true;
            this.skinContextMenuStrip.TitleColor = System.Drawing.Color.White;
            this.skinContextMenuStrip.TitleRadius = 4;
            this.skinContextMenuStrip.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // 禁言_toolStripMenuItem
            // 
            this.禁言_toolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_oneDay,
            this.toolStripMenuItem_tenDays,
            this.toolStripMenuItem_forever});
            this.禁言_toolStripMenuItem.Name = "禁言_toolStripMenuItem";
            this.禁言_toolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.禁言_toolStripMenuItem.Text = "禁言";
            this.禁言_toolStripMenuItem.Visible = false;
            // 
            // toolStripMenuItem_oneDay
            // 
            this.toolStripMenuItem_oneDay.Name = "toolStripMenuItem_oneDay";
            this.toolStripMenuItem_oneDay.Size = new System.Drawing.Size(102, 22);
            this.toolStripMenuItem_oneDay.Text = "1天";
            this.toolStripMenuItem_oneDay.Click += new System.EventHandler(this.toolStripMenuItem_oneDay_Click);
            // 
            // toolStripMenuItem_tenDays
            // 
            this.toolStripMenuItem_tenDays.Name = "toolStripMenuItem_tenDays";
            this.toolStripMenuItem_tenDays.Size = new System.Drawing.Size(102, 22);
            this.toolStripMenuItem_tenDays.Text = "10天";
            this.toolStripMenuItem_tenDays.Click += new System.EventHandler(this.toolStripMenuItem_tenDays_Click);
            // 
            // toolStripMenuItem_forever
            // 
            this.toolStripMenuItem_forever.Name = "toolStripMenuItem_forever";
            this.toolStripMenuItem_forever.Size = new System.Drawing.Size(102, 22);
            this.toolStripMenuItem_forever.Text = "永久";
            this.toolStripMenuItem_forever.Click += new System.EventHandler(this.toolStripMenuItem_forever_Click);
            // 
            // 解除禁言_toolStripMenuItem
            // 
            this.解除禁言_toolStripMenuItem.Name = "解除禁言_toolStripMenuItem";
            this.解除禁言_toolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.解除禁言_toolStripMenuItem.Text = "解除禁言";
            this.解除禁言_toolStripMenuItem.Visible = false;
            this.解除禁言_toolStripMenuItem.Click += new System.EventHandler(this.解除禁言_toolStripMenuItem_Click);
            // 
            // 添加好友ToolStripMenuItem
            // 
            this.添加好友ToolStripMenuItem.Name = "添加好友ToolStripMenuItem";
            this.添加好友ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.添加好友ToolStripMenuItem.Text = "添加好友";
            this.添加好友ToolStripMenuItem.Click += new System.EventHandler(this.添加好友ToolStripMenuItem_Click);
            // 
            // TAToolStripMenuItem
            // 
            this.TAToolStripMenuItem.Name = "TAToolStripMenuItem";
            this.TAToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.TAToolStripMenuItem.Text = "@TA";
            this.TAToolStripMenuItem.Click += new System.EventHandler(this.taToolStripMenuItem_Click);
            // 
            // skinButton_allGroupBan
            // 
            this.skinButton_allGroupBan.BackColor = System.Drawing.Color.White;
            this.skinButton_allGroupBan.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.skinButton_allGroupBan.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton_allGroupBan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.skinButton_allGroupBan.DownBack = null;
            this.skinButton_allGroupBan.DrawType = CCWin.SkinControl.DrawStyle.None;
            this.skinButton_allGroupBan.Image = global::GGTalk.Properties.Resources.allGroupBanTurnOn;
            this.skinButton_allGroupBan.Location = new System.Drawing.Point(161, 39);
            this.skinButton_allGroupBan.Margin = new System.Windows.Forms.Padding(0);
            this.skinButton_allGroupBan.MouseBack = null;
            this.skinButton_allGroupBan.Name = "skinButton_allGroupBan";
            this.skinButton_allGroupBan.NormlBack = null;
            this.skinButton_allGroupBan.Size = new System.Drawing.Size(17, 19);
            this.skinButton_allGroupBan.TabIndex = 160;
            this.toolShow.SetToolTip(this.skinButton_allGroupBan, "开启全员禁言");
            this.skinButton_allGroupBan.UseHandCursor = false;
            this.skinButton_allGroupBan.UseVisualStyleBackColor = false;
            this.skinButton_allGroupBan.Visible = false;
            this.skinButton_allGroupBan.Click += new System.EventHandler(this.skinButton_allGroupBan_Click);
            // 
            // skinButton1
            // 
            this.skinButton1.BackColor = System.Drawing.Color.White;
            this.skinButton1.BaseColor = System.Drawing.Color.White;
            this.skinButton1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.skinButton1.DownBack = null;
            this.skinButton1.DrawType = CCWin.SkinControl.DrawStyle.None;
            this.skinButton1.Image = global::GGTalk.Properties.Resources.signout_icon;
            this.skinButton1.Location = new System.Drawing.Point(186, 40);
            this.skinButton1.Margin = new System.Windows.Forms.Padding(0);
            this.skinButton1.MouseBack = null;
            this.skinButton1.Name = "skinButton1";
            this.skinButton1.NormlBack = null;
            this.skinButton1.Size = new System.Drawing.Size(17, 19);
            this.skinButton1.TabIndex = 161;
            this.toolShow.SetToolTip(this.skinButton1, "退出该讨论组");
            this.skinButton1.UseHandCursor = false;
            this.skinButton1.UseVisualStyleBackColor = false;
            this.skinButton1.Click += new System.EventHandler(this.skinButton1_Click);
            // 
            // skinButtom_editMembers
            // 
            this.skinButtom_editMembers.BackColor = System.Drawing.Color.White;
            this.skinButtom_editMembers.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.skinButtom_editMembers.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButtom_editMembers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.skinButtom_editMembers.DownBack = null;
            this.skinButtom_editMembers.DrawType = CCWin.SkinControl.DrawStyle.None;
            this.skinButtom_editMembers.Image = global::GGTalk.Properties.Resources.edit_icon;
            this.skinButtom_editMembers.Location = new System.Drawing.Point(207, 41);
            this.skinButtom_editMembers.Margin = new System.Windows.Forms.Padding(0);
            this.skinButtom_editMembers.MouseBack = null;
            this.skinButtom_editMembers.Name = "skinButtom_editMembers";
            this.skinButtom_editMembers.NormlBack = null;
            this.skinButtom_editMembers.Size = new System.Drawing.Size(17, 17);
            this.skinButtom_editMembers.TabIndex = 162;
            this.toolShow.SetToolTip(this.skinButtom_editMembers, "编辑成员");
            this.skinButtom_editMembers.UseHandCursor = false;
            this.skinButtom_editMembers.UseVisualStyleBackColor = false;
            this.skinButtom_editMembers.Click += new System.EventHandler(this.skinButtom1_Click);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.AutoSize = false;
            this.toolStripLabel2.BackColor = System.Drawing.Color.Transparent;
            this.toolStripLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripLabel2.Image = global::GGTalk.Properties.Resources.pictureBox1_Image;
            this.toolStripLabel2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripLabel2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(10, 24);
            this.toolStripLabel2.Text = "toolStripButton6";
            // 
            // fontDialog1
            // 
            this.fontDialog1.Color = System.Drawing.SystemColors.ControlText;
            this.fontDialog1.ShowColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "chat_icon.png");
            this.imageList1.Images.SetKeyName(1, "Folder.png");
            this.imageList1.Images.SetKeyName(2, "VideoWaitToAnswer.png");
            // 
            // skinTabControl1
            // 
            animation1.AnimateOnlyDifferences = false;
            animation1.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.BlindCoeff")));
            animation1.LeafCoeff = 0F;
            animation1.MaxTime = 1F;
            animation1.MinTime = 0F;
            animation1.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicCoeff")));
            animation1.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicShift")));
            animation1.MosaicSize = 0;
            animation1.Padding = new System.Windows.Forms.Padding(0);
            animation1.RotateCoeff = 0F;
            animation1.RotateLimit = 0F;
            animation1.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.ScaleCoeff")));
            animation1.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.SlideCoeff")));
            animation1.TimeCoeff = 2F;
            animation1.TransparencyCoeff = 0F;
            this.skinTabControl1.Animation = animation1;
            this.skinTabControl1.AnimationStart = false;
            this.skinTabControl1.AnimatorType = CCWin.SkinControl.AnimationType.HorizSlide;
            this.skinTabControl1.CloseRect = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.skinTabControl1.Controls.Add(this.tabPage_chat);
            this.skinTabControl1.Controls.Add(this.tabPage_file);
            this.skinTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTabControl1.ImageList = this.imageList1;
            this.skinTabControl1.ItemSize = new System.Drawing.Size(70, 36);
            this.skinTabControl1.Location = new System.Drawing.Point(4, 60);
            this.skinTabControl1.Name = "skinTabControl1";
            this.skinTabControl1.PageArrowDown = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageArrowDown")));
            this.skinTabControl1.PageArrowHover = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageArrowHover")));
            this.skinTabControl1.PageCloseHover = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageCloseHover")));
            this.skinTabControl1.PageCloseNormal = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageCloseNormal")));
            this.skinTabControl1.PageDown = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageDown")));
            this.skinTabControl1.PageHover = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageHover")));
            this.skinTabControl1.PageImagePosition = CCWin.SkinControl.SkinTabControl.ePageImagePosition.Left;
            this.skinTabControl1.PageNorml = null;
            this.skinTabControl1.SelectedIndex = 0;
            this.skinTabControl1.Size = new System.Drawing.Size(524, 507);
            this.skinTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.skinTabControl1.TabIndex = 155;
            this.skinTabControl1.SelectedIndexChanged += new System.EventHandler(this.skinTabControl1_SelectedIndexChanged);
            // 
            // tabPage_chat
            // 
            this.tabPage_chat.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage_chat.Controls.Add(this.chatPanel1);
            this.tabPage_chat.ImageIndex = 0;
            this.tabPage_chat.Location = new System.Drawing.Point(0, 36);
            this.tabPage_chat.Name = "tabPage_chat";
            this.tabPage_chat.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_chat.Size = new System.Drawing.Size(524, 471);
            this.tabPage_chat.TabIndex = 0;
            this.tabPage_chat.Text = "聊天";
            // 
            // chatPanel1
            // 
            this.chatPanel1.BackColor = System.Drawing.Color.Transparent;
            this.chatPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatPanel1.Location = new System.Drawing.Point(3, 3);
            this.chatPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.chatPanel1.Name = "chatPanel1";
            this.chatPanel1.Size = new System.Drawing.Size(518, 465);
            this.chatPanel1.TabIndex = 158;
            // 
            // tabPage_file
            // 
            this.tabPage_file.Controls.Add(this.nDiskBrowser1);
            this.tabPage_file.ImageIndex = 1;
            this.tabPage_file.Location = new System.Drawing.Point(0, 36);
            this.tabPage_file.Name = "tabPage_file";
            this.tabPage_file.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_file.Size = new System.Drawing.Size(524, 471);
            this.tabPage_file.TabIndex = 1;
            this.tabPage_file.Text = "文件";
            this.tabPage_file.UseVisualStyleBackColor = true;
            // 
            // nDiskBrowser1
            // 
            this.nDiskBrowser1.AllowDrop = true;
            this.nDiskBrowser1.AllowUploadFolder = false;
            this.nDiskBrowser1.BackColor = System.Drawing.Color.Transparent;
            this.nDiskBrowser1.Connected = true;
            this.nDiskBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nDiskBrowser1.Location = new System.Drawing.Point(3, 3);
            this.nDiskBrowser1.LockRootDirectory = true;
            this.nDiskBrowser1.Margin = new System.Windows.Forms.Padding(4);
            this.nDiskBrowser1.Name = "nDiskBrowser1";
            this.nDiskBrowser1.NetDiskID = "";
            this.nDiskBrowser1.Size = new System.Drawing.Size(194, 58);
            this.nDiskBrowser1.TabIndex = 0;
            this.nDiskBrowser1.Load += new System.EventHandler(this.nDiskBrowser1_Load);
            // 
            // pnlTx
            // 
            this.pnlTx.BackColor = System.Drawing.Color.Transparent;
            this.pnlTx.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlTx.BackRectangle = new System.Drawing.Rectangle(5, 5, 5, 5);
            this.pnlTx.Controls.Add(this.panelFriendHeadImage);
            this.pnlTx.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.pnlTx.DownBack = global::GGTalk.Properties.Resources.aio_head_normal;
            this.pnlTx.Location = new System.Drawing.Point(6, 8);
            this.pnlTx.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTx.MouseBack = global::GGTalk.Properties.Resources.aio_head_normal;
            this.pnlTx.Name = "pnlTx";
            this.pnlTx.NormlBack = global::GGTalk.Properties.Resources.aio_head_normal;
            this.pnlTx.Palace = true;
            this.pnlTx.Size = new System.Drawing.Size(49, 50);
            this.pnlTx.TabIndex = 104;
            // 
            // panelFriendHeadImage
            // 
            this.panelFriendHeadImage.BackColor = System.Drawing.Color.Transparent;
            this.panelFriendHeadImage.BackgroundImage = global::GGTalk.Properties.Resources.Group2;
            this.panelFriendHeadImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelFriendHeadImage.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.panelFriendHeadImage.DownBack = null;
            this.panelFriendHeadImage.Location = new System.Drawing.Point(3, 3);
            this.panelFriendHeadImage.Margin = new System.Windows.Forms.Padding(0);
            this.panelFriendHeadImage.MouseBack = null;
            this.panelFriendHeadImage.Name = "panelFriendHeadImage";
            this.panelFriendHeadImage.NormlBack = null;
            this.panelFriendHeadImage.Radius = 5;
            this.panelFriendHeadImage.Size = new System.Drawing.Size(42, 44);
            this.panelFriendHeadImage.TabIndex = 6;
            this.panelFriendHeadImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelFriendHeadImage_MouseClick);
            // 
            // labelGroupName
            // 
            this.labelGroupName.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.labelGroupName.AutoSize = true;
            this.labelGroupName.BackColor = System.Drawing.Color.Transparent;
            this.labelGroupName.BorderColor = System.Drawing.Color.White;
            this.labelGroupName.BorderSize = 4;
            this.labelGroupName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelGroupName.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.labelGroupName.ForeColor = System.Drawing.Color.Black;
            this.labelGroupName.Location = new System.Drawing.Point(58, 8);
            this.labelGroupName.Name = "labelGroupName";
            this.labelGroupName.Size = new System.Drawing.Size(111, 24);
            this.labelGroupName.TabIndex = 100;
            this.labelGroupName.Text = "测试讨论组1";
            this.labelGroupName.Click += new System.EventHandler(this.labelGroupName_Click);
            // 
            // label_announce
            // 
            this.label_announce.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.label_announce.AutoSize = true;
            this.label_announce.BackColor = System.Drawing.Color.Transparent;
            this.label_announce.BorderColor = System.Drawing.Color.White;
            this.label_announce.BorderSize = 4;
            this.label_announce.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label_announce.ForeColor = System.Drawing.Color.Black;
            this.label_announce.Location = new System.Drawing.Point(61, 35);
            this.label_announce.Name = "label_announce";
            this.label_announce.Size = new System.Drawing.Size(226, 17);
            this.label_announce.TabIndex = 103;
            this.label_announce.Text = "2.10上午10点一号会议室全体员工大会！";
            // 
            // skinPanel3
            // 
            this.skinPanel3.BackColor = System.Drawing.Color.Transparent;
            this.skinPanel3.Controls.Add(this.skinButton_allGroupBan);
            this.skinPanel3.Controls.Add(this.skinButton1);
            this.skinPanel3.Controls.Add(this.skinButtom_editMembers);
            this.skinPanel3.Controls.Add(this.chatListBox1);
            this.skinPanel3.Controls.Add(this.btn_groupVideo);
            this.skinPanel3.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinPanel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.skinPanel3.DownBack = null;
            this.skinPanel3.Location = new System.Drawing.Point(528, 60);
            this.skinPanel3.MouseBack = null;
            this.skinPanel3.Name = "skinPanel3";
            this.skinPanel3.NormlBack = null;
            this.skinPanel3.Size = new System.Drawing.Size(243, 507);
            this.skinPanel3.TabIndex = 160;
            // 
            // btn_groupVideo
            // 
            this.btn_groupVideo.BackColor = System.Drawing.Color.Transparent;
            this.btn_groupVideo.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.btn_groupVideo.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btn_groupVideo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_groupVideo.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_groupVideo.DownBack = global::GGTalk.Properties.Resources.button_frame;
            this.btn_groupVideo.DrawType = CCWin.SkinControl.DrawStyle.None;
            this.btn_groupVideo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_groupVideo.Image = global::GGTalk.Properties.Resources.VideoWaitToAnswer;
            this.btn_groupVideo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_groupVideo.Location = new System.Drawing.Point(0, 0);
            this.btn_groupVideo.MouseBack = global::GGTalk.Properties.Resources.button_frame_pre;
            this.btn_groupVideo.Name = "btn_groupVideo";
            this.btn_groupVideo.NormlBack = global::GGTalk.Properties.Resources.button_frame;
            this.btn_groupVideo.Palace = true;
            this.btn_groupVideo.Size = new System.Drawing.Size(243, 36);
            this.btn_groupVideo.TabIndex = 158;
            this.btn_groupVideo.Text = " 视频通话";
            this.btn_groupVideo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_groupVideo.UseHandCursor = false;
            this.btn_groupVideo.UseVisualStyleBackColor = false;
            this.btn_groupVideo.Click += new System.EventHandler(this.btn_groupVideo_Click);
            // 
            // skinPanel1
            // 
            this.skinPanel1.BackColor = System.Drawing.Color.Transparent;
            this.skinPanel1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.skinPanel1.DownBack = null;
            this.skinPanel1.Location = new System.Drawing.Point(4, 28);
            this.skinPanel1.MouseBack = null;
            this.skinPanel1.Name = "skinPanel1";
            this.skinPanel1.NormlBack = null;
            this.skinPanel1.Size = new System.Drawing.Size(767, 32);
            this.skinPanel1.TabIndex = 158;
            // 
            // skinPanel2
            // 
            this.skinPanel2.BackColor = System.Drawing.Color.Transparent;
            this.skinPanel2.Controls.Add(this.linkLabel_softName);
            this.skinPanel2.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.skinPanel2.DownBack = null;
            this.skinPanel2.Location = new System.Drawing.Point(4, 567);
            this.skinPanel2.MouseBack = null;
            this.skinPanel2.Name = "skinPanel2";
            this.skinPanel2.NormlBack = null;
            this.skinPanel2.Size = new System.Drawing.Size(767, 29);
            this.skinPanel2.TabIndex = 159;
            // 
            // linkLabel_softName
            // 
            this.linkLabel_softName.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkLabel_softName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel_softName.AutoSize = true;
            this.linkLabel_softName.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel_softName.DisabledLinkColor = System.Drawing.Color.Black;
            this.linkLabel_softName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel_softName.ForeColor = System.Drawing.Color.Black;
            this.linkLabel_softName.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel_softName.LinkColor = System.Drawing.Color.Black;
            this.linkLabel_softName.Location = new System.Drawing.Point(7, 6);
            this.linkLabel_softName.Name = "linkLabel_softName";
            this.linkLabel_softName.Size = new System.Drawing.Size(76, 17);
            this.linkLabel_softName.TabIndex = 131;
            this.linkLabel_softName.TabStop = true;
            this.linkLabel_softName.Text = "GGTalk ";
            this.linkLabel_softName.VisitedLinkColor = System.Drawing.SystemColors.HotTrack;
            // 
            // GroupChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Back = ((System.Drawing.Image)(resources.GetObject("$this.Back")));
            this.BorderPalace = global::GGTalk.Properties.Resources.BackPalace;
            this.CanResize = true;
            this.ClientSize = new System.Drawing.Size(775, 600);
            this.CloseDownBack = global::GGTalk.Properties.Resources.btn_close_down;
            this.CloseMouseBack = global::GGTalk.Properties.Resources.btn_close_highlight;
            this.CloseNormlBack = global::GGTalk.Properties.Resources.btn_close_disable;
            this.Controls.Add(this.skinTabControl1);
            this.Controls.Add(this.pnlTx);
            this.Controls.Add(this.labelGroupName);
            this.Controls.Add(this.label_announce);
            this.Controls.Add(this.skinPanel3);
            this.Controls.Add(this.skinPanel1);
            this.Controls.Add(this.skinPanel2);
            this.EffectCaption = CCWin.TitleType.None;
            this.EffectWidth = 4;
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaxDownBack = global::GGTalk.Properties.Resources.btn_max_down;
            this.MaximizeBox = true;
            this.MaxMouseBack = global::GGTalk.Properties.Resources.btn_max_highlight;
            this.MaxNormlBack = global::GGTalk.Properties.Resources.btn_max_normal;
            this.MiniDownBack = global::GGTalk.Properties.Resources.btn_mini_down;
            this.MinimizeBox = true;
            this.MiniMouseBack = global::GGTalk.Properties.Resources.btn_mini_highlight;
            this.MinimumSize = new System.Drawing.Size(586, 524);
            this.MiniNormlBack = global::GGTalk.Properties.Resources.btn_mini_normal;
            this.Name = "GroupChatForm";
            this.RestoreDownBack = global::GGTalk.Properties.Resources.btn_restore_down;
            this.RestoreMouseBack = global::GGTalk.Properties.Resources.btn_restore_highlight;
            this.RestoreNormlBack = global::GGTalk.Properties.Resources.btn_restore_normal;
            this.Shadow = false;
            this.ShowBorder = false;
            this.ShowDrawIcon = false;
            this.ShowInTaskbar = true;
            this.Special = false;
            this.TopMost = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmChat_Paint);
            this.skinContextMenuStrip.ResumeLayout(false);
            this.skinTabControl1.ResumeLayout(false);
            this.tabPage_chat.ResumeLayout(false);
            this.tabPage_file.ResumeLayout(false);
            this.pnlTx.ResumeLayout(false);
            this.skinPanel3.ResumeLayout(false);
            this.skinPanel2.ResumeLayout(false);
            this.skinPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private CCWin.SkinControl.SkinLabel labelGroupName;
        private CCWin.SkinControl.SkinLabel label_announce;
        private CCWin.SkinControl.SkinPanel pnlTx;
        private CCWin.SkinControl.SkinPanel panelFriendHeadImage;
        private System.Windows.Forms.ToolTip toolShow;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.FontDialog fontDialog1;
        private CCWin.SkinControl.SkinTabControl skinTabControl1;
        private System.Windows.Forms.TabPage tabPage_chat;
        private System.Windows.Forms.TabPage tabPage_file;
        private System.Windows.Forms.ImageList imageList1;
        private ESFramework.Boost.NetworkDisk.Passive.NDiskBrowser nDiskBrowser1;
        private CCWin.SkinControl.SkinContextMenuStrip skinContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 禁言_toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 解除禁言_toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_oneDay;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_tenDays;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_forever;
        private System.Windows.Forms.ToolStripMenuItem 添加好友ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TAToolStripMenuItem;
        private ChatPanel chatPanel1;
        private CCWin.SkinControl.SkinPanel skinPanel1;
        private CCWin.SkinControl.SkinPanel skinPanel2;
        private System.Windows.Forms.LinkLabel linkLabel_softName;
        private CCWin.SkinControl.SkinPanel skinPanel3;
        private CCWin.SkinControl.ChatListBox chatListBox1;
        private CCWin.SkinControl.SkinButton btn_groupVideo;
        private CCWin.SkinControl.SkinButton skinButton_allGroupBan;
        private CCWin.SkinControl.SkinButton skinButton1;
        private CCWin.SkinControl.SkinButton skinButtom_editMembers;
    }
}