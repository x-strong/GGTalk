using ESFramework.Boost.Controls;
namespace GGTalk
{
    partial class ChatPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatPanel));
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.toolShow = new System.Windows.Forms.ToolTip(this.components);
            this.gifBox_wait = new ESFramework.Boost.Controls.GifBox();
            this.panel_audioMessage = new System.Windows.Forms.Panel();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.skinButton2 = new CCWin.SkinControl.SkinButton();
            this.skinButton1 = new CCWin.SkinControl.SkinButton();
            this.skinLabel_Ban = new CCWin.SkinControl.SkinLabel();
            this.chatBoxSend = new ESFramework.Boost.Controls.ChatBox();
            this.skinContextMenuStrip4Send = new CCWin.SkinControl.SkinContextMenuStrip();
            this.toolStripMenuItem_enter = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_ctrl = new System.Windows.Forms.ToolStripMenuItem();
            this.skinContextMenuStrip_quickAnswer = new CCWin.SkinControl.SkinContextMenuStrip();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.另存为ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新窗口显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清屏ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSendMenu = new CCWin.SkinControl.SkinButton();
            this.skinButton_send = new CCWin.SkinControl.SkinButton();
            this.skToolMenu = new CCWin.SkinControl.SkinToolStrip();
            this.toolStripButtonEmotion = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.发送屏幕截屏ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuIte_hideForm = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton_audioMsg = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_snapchat = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new CCWin.SkinControl.SkinButton();
            this.atMessagePanle1 = new GGTalk.AtMessagePanle();
            this.chatBox_history = new GGTalk.Controls.ChatRender4Dll.ChatBox();
            this.panel_audioMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.skinContextMenuStrip4Send.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.skToolMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // fontDialog1
            // 
            this.fontDialog1.Color = System.Drawing.SystemColors.ControlText;
            this.fontDialog1.ShowColor = true;
            // 
            // gifBox_wait
            // 
            this.gifBox_wait.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gifBox_wait.BackColor = System.Drawing.Color.Transparent;
            this.gifBox_wait.BorderColor = System.Drawing.Color.Transparent;
            this.gifBox_wait.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.gifBox_wait.Image = global::GGTalk.Properties.Resources.busy;
            this.gifBox_wait.Location = new System.Drawing.Point(220, 351);
            this.gifBox_wait.Name = "gifBox_wait";
            this.gifBox_wait.Size = new System.Drawing.Size(22, 20);
            this.gifBox_wait.TabIndex = 152;
            this.gifBox_wait.Text = "gifBox1";
            this.toolShow.SetToolTip(this.gifBox_wait, "正在发送 . . .");
            this.gifBox_wait.Visible = false;
            // 
            // panel_audioMessage
            // 
            this.panel_audioMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_audioMessage.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel_audioMessage.Controls.Add(this.skinLabel1);
            this.panel_audioMessage.Controls.Add(this.pictureBox1);
            this.panel_audioMessage.Controls.Add(this.skinButton2);
            this.panel_audioMessage.Controls.Add(this.skinButton1);
            this.panel_audioMessage.Location = new System.Drawing.Point(0, 177);
            this.panel_audioMessage.Margin = new System.Windows.Forms.Padding(2);
            this.panel_audioMessage.Name = "panel_audioMessage";
            this.panel_audioMessage.Size = new System.Drawing.Size(399, 38);
            this.panel_audioMessage.TabIndex = 153;
            this.panel_audioMessage.Visible = false;
            // 
            // skinLabel1
            // 
            this.skinLabel1.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel1.Location = new System.Drawing.Point(23, 12);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(149, 31);
            this.skinLabel1.TabIndex = 136;
            this.skinLabel1.Text = "正在录音 . . .";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::GGTalk.Properties.Resources.chatfrom_voice_playing;
            this.pictureBox1.Location = new System.Drawing.Point(4, 11);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(17, 18);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 135;
            this.pictureBox1.TabStop = false;
            // 
            // skinButton2
            // 
            this.skinButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.skinButton2.BackColor = System.Drawing.Color.Transparent;
            this.skinButton2.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.skinButton2.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.skinButton2.DownBack = ((System.Drawing.Image)(resources.GetObject("skinButton2.DownBack")));
            this.skinButton2.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.skinButton2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButton2.Location = new System.Drawing.Point(325, 6);
            this.skinButton2.MouseBack = ((System.Drawing.Image)(resources.GetObject("skinButton2.MouseBack")));
            this.skinButton2.Name = "skinButton2";
            this.skinButton2.NormlBack = ((System.Drawing.Image)(resources.GetObject("skinButton2.NormlBack")));
            this.skinButton2.Palace = true;
            this.skinButton2.Size = new System.Drawing.Size(60, 28);
            this.skinButton2.TabIndex = 134;
            this.skinButton2.Text = "取消";
            this.skinButton2.UseHandCursor = false;
            this.skinButton2.UseVisualStyleBackColor = false;
            this.skinButton2.Click += new System.EventHandler(this.skinButton2_Click);
            // 
            // skinButton1
            // 
            this.skinButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.skinButton1.BackColor = System.Drawing.Color.Transparent;
            this.skinButton1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.skinButton1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.skinButton1.DownBack = ((System.Drawing.Image)(resources.GetObject("skinButton1.DownBack")));
            this.skinButton1.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.skinButton1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButton1.Location = new System.Drawing.Point(254, 6);
            this.skinButton1.MouseBack = ((System.Drawing.Image)(resources.GetObject("skinButton1.MouseBack")));
            this.skinButton1.Name = "skinButton1";
            this.skinButton1.NormlBack = ((System.Drawing.Image)(resources.GetObject("skinButton1.NormlBack")));
            this.skinButton1.Palace = true;
            this.skinButton1.Size = new System.Drawing.Size(60, 28);
            this.skinButton1.TabIndex = 134;
            this.skinButton1.Text = "发送";
            this.skinButton1.UseHandCursor = false;
            this.skinButton1.UseVisualStyleBackColor = false;
            this.skinButton1.Click += new System.EventHandler(this.skinButton1_Click);
            // 
            // skinLabel_Ban
            // 
            this.skinLabel_Ban.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.skinLabel_Ban.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel_Ban.AutoSize = true;
            this.skinLabel_Ban.BackColor = System.Drawing.Color.White;
            this.skinLabel_Ban.BorderColor = System.Drawing.Color.White;
            this.skinLabel_Ban.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel_Ban.Location = new System.Drawing.Point(176, 288);
            this.skinLabel_Ban.Name = "skinLabel_Ban";
            this.skinLabel_Ban.Size = new System.Drawing.Size(86, 31);
            this.skinLabel_Ban.TabIndex = 158;
            this.skinLabel_Ban.Text = "禁言中";
            this.skinLabel_Ban.Visible = false;
            // 
            // chatBoxSend
            // 
            this.chatBoxSend.AllowDrop = true;
            this.chatBoxSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chatBoxSend.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chatBoxSend.ContextMenuMode = ESFramework.Boost.Controls.ChatBoxContextMenuMode.ForInput;
            this.chatBoxSend.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.chatBoxSend.Location = new System.Drawing.Point(0, 253);
            this.chatBoxSend.Name = "chatBoxSend";
            this.chatBoxSend.PopoutImageWhenDoubleClick = false;
            this.chatBoxSend.Size = new System.Drawing.Size(399, 89);
            this.chatBoxSend.TabIndex = 148;
            this.chatBoxSend.Text = "";
            this.chatBoxSend.UseRtf = false;
            this.chatBoxSend.TextChanged += new System.EventHandler(this.chatBoxSend_TextChanged);
            // 
            // skinContextMenuStrip4Send
            // 
            this.skinContextMenuStrip4Send.Arrow = System.Drawing.Color.Black;
            this.skinContextMenuStrip4Send.Back = System.Drawing.Color.White;
            this.skinContextMenuStrip4Send.BackRadius = 4;
            this.skinContextMenuStrip4Send.Base = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(200)))), ((int)(((byte)(254)))));
            this.skinContextMenuStrip4Send.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinContextMenuStrip4Send.Fore = System.Drawing.Color.Black;
            this.skinContextMenuStrip4Send.HoverFore = System.Drawing.Color.White;
            this.skinContextMenuStrip4Send.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.skinContextMenuStrip4Send.ItemAnamorphosis = false;
            this.skinContextMenuStrip4Send.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip4Send.ItemBorderShow = false;
            this.skinContextMenuStrip4Send.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip4Send.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip4Send.ItemRadius = 4;
            this.skinContextMenuStrip4Send.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.None;
            this.skinContextMenuStrip4Send.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_enter,
            this.toolStripMenuItem_ctrl});
            this.skinContextMenuStrip4Send.ItemSplitter = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinContextMenuStrip4Send.Name = "MenuState";
            this.skinContextMenuStrip4Send.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinContextMenuStrip4Send.Size = new System.Drawing.Size(351, 84);
            this.skinContextMenuStrip4Send.SkinAllColor = true;
            this.skinContextMenuStrip4Send.TitleAnamorphosis = false;
            this.skinContextMenuStrip4Send.TitleColor = System.Drawing.Color.White;
            this.skinContextMenuStrip4Send.TitleRadius = 4;
            this.skinContextMenuStrip4Send.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // toolStripMenuItem_enter
            // 
            this.toolStripMenuItem_enter.Checked = true;
            this.toolStripMenuItem_enter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem_enter.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItem_enter.Name = "toolStripMenuItem_enter";
            this.toolStripMenuItem_enter.Size = new System.Drawing.Size(350, 40);
            this.toolStripMenuItem_enter.Text = "按Enter键发送消息";
            this.toolStripMenuItem_enter.Click += new System.EventHandler(this.toolStripMenuItem_enter_Click);
            // 
            // toolStripMenuItem_ctrl
            // 
            this.toolStripMenuItem_ctrl.Name = "toolStripMenuItem_ctrl";
            this.toolStripMenuItem_ctrl.Size = new System.Drawing.Size(350, 40);
            this.toolStripMenuItem_ctrl.Text = "按Ctrl+Enter键发送消息";
            this.toolStripMenuItem_ctrl.Click += new System.EventHandler(this.toolStripMenuItem_ctrl_Click);
            // 
            // skinContextMenuStrip_quickAnswer
            // 
            this.skinContextMenuStrip_quickAnswer.Arrow = System.Drawing.Color.Black;
            this.skinContextMenuStrip_quickAnswer.Back = System.Drawing.Color.White;
            this.skinContextMenuStrip_quickAnswer.BackRadius = 4;
            this.skinContextMenuStrip_quickAnswer.Base = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(200)))), ((int)(((byte)(254)))));
            this.skinContextMenuStrip_quickAnswer.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinContextMenuStrip_quickAnswer.Fore = System.Drawing.Color.Black;
            this.skinContextMenuStrip_quickAnswer.HoverFore = System.Drawing.Color.White;
            this.skinContextMenuStrip_quickAnswer.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.skinContextMenuStrip_quickAnswer.ItemAnamorphosis = true;
            this.skinContextMenuStrip_quickAnswer.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_quickAnswer.ItemBorderShow = true;
            this.skinContextMenuStrip_quickAnswer.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_quickAnswer.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_quickAnswer.ItemRadius = 4;
            this.skinContextMenuStrip_quickAnswer.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinContextMenuStrip_quickAnswer.ItemSplitter = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_quickAnswer.Name = "skinContextMenuStrip_quickAnswer";
            this.skinContextMenuStrip_quickAnswer.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinContextMenuStrip_quickAnswer.Size = new System.Drawing.Size(61, 4);
            this.skinContextMenuStrip_quickAnswer.SkinAllColor = true;
            this.skinContextMenuStrip_quickAnswer.TitleAnamorphosis = true;
            this.skinContextMenuStrip_quickAnswer.TitleColor = System.Drawing.Color.White;
            this.skinContextMenuStrip_quickAnswer.TitleRadius = 4;
            this.skinContextMenuStrip_quickAnswer.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制ToolStripMenuItem,
            this.另存为ToolStripMenuItem,
            this.新窗口显示ToolStripMenuItem,
            this.清屏ToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(209, 156);
            // 
            // 复制ToolStripMenuItem
            // 
            this.复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
            this.复制ToolStripMenuItem.Size = new System.Drawing.Size(208, 38);
            this.复制ToolStripMenuItem.Text = "复制";
            this.复制ToolStripMenuItem.Click += new System.EventHandler(this.复制ToolStripMenuItem_Click);
            // 
            // 另存为ToolStripMenuItem
            // 
            this.另存为ToolStripMenuItem.Name = "另存为ToolStripMenuItem";
            this.另存为ToolStripMenuItem.Size = new System.Drawing.Size(208, 38);
            this.另存为ToolStripMenuItem.Text = "另存为";
            this.另存为ToolStripMenuItem.Click += new System.EventHandler(this.另存为ToolStripMenuItem_Click);
            // 
            // 新窗口显示ToolStripMenuItem
            // 
            this.新窗口显示ToolStripMenuItem.Name = "新窗口显示ToolStripMenuItem";
            this.新窗口显示ToolStripMenuItem.Size = new System.Drawing.Size(208, 38);
            this.新窗口显示ToolStripMenuItem.Text = "新窗口显示";
            this.新窗口显示ToolStripMenuItem.Click += new System.EventHandler(this.新窗口显示ToolStripMenuItem_Click);
            // 
            // 清屏ToolStripMenuItem1
            // 
            this.清屏ToolStripMenuItem1.Name = "清屏ToolStripMenuItem1";
            this.清屏ToolStripMenuItem1.Size = new System.Drawing.Size(208, 38);
            this.清屏ToolStripMenuItem1.Text = "清屏";
            this.清屏ToolStripMenuItem1.Click += new System.EventHandler(this.清屏ToolStripMenuItem_Click);
            // 
            // btnSendMenu
            // 
            this.btnSendMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendMenu.BackColor = System.Drawing.Color.Transparent;
            this.btnSendMenu.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.btnSendMenu.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnSendMenu.DownBack = ((System.Drawing.Image)(resources.GetObject("btnSendMenu.DownBack")));
            this.btnSendMenu.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.btnSendMenu.Location = new System.Drawing.Point(4, 350);
            this.btnSendMenu.Margin = new System.Windows.Forms.Padding(0);
            this.btnSendMenu.MouseBack = ((System.Drawing.Image)(resources.GetObject("btnSendMenu.MouseBack")));
            this.btnSendMenu.Name = "btnSendMenu";
            this.btnSendMenu.NormlBack = ((System.Drawing.Image)(resources.GetObject("btnSendMenu.NormlBack")));
            this.btnSendMenu.Size = new System.Drawing.Size(22, 24);
            this.btnSendMenu.TabIndex = 151;
            this.btnSendMenu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSendMenu.UseHandCursor = false;
            this.btnSendMenu.UseVisualStyleBackColor = false;
            this.btnSendMenu.Visible = false;
            this.btnSendMenu.Click += new System.EventHandler(this.btnSendMenu_Click);
            // 
            // skinButton_send
            // 
            this.skinButton_send.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.skinButton_send.BackColor = System.Drawing.Color.WhiteSmoke;
            this.skinButton_send.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.skinButton_send.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton_send.DownBack = global::GGTalk.Properties.Resources.button_frame;
            this.skinButton_send.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.skinButton_send.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButton_send.Location = new System.Drawing.Point(324, 348);
            this.skinButton_send.Margin = new System.Windows.Forms.Padding(0);
            this.skinButton_send.MouseBack = global::GGTalk.Properties.Resources.button_frame_pre;
            this.skinButton_send.Name = "skinButton_send";
            this.skinButton_send.NormlBack = global::GGTalk.Properties.Resources.button_frame;
            this.skinButton_send.Palace = true;
            this.skinButton_send.Radius = 4;
            this.skinButton_send.Size = new System.Drawing.Size(65, 28);
            this.skinButton_send.TabIndex = 150;
            this.skinButton_send.Text = "发送";
            this.skinButton_send.UseHandCursor = false;
            this.skinButton_send.UseVisualStyleBackColor = false;
            this.skinButton_send.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // skToolMenu
            // 
            this.skToolMenu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skToolMenu.Arrow = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.skToolMenu.AutoSize = false;
            this.skToolMenu.Back = System.Drawing.Color.White;
            this.skToolMenu.BackColor = System.Drawing.Color.Transparent;
            this.skToolMenu.BackRadius = 4;
            this.skToolMenu.BackRectangle = new System.Drawing.Rectangle(10, 10, 10, 10);
            this.skToolMenu.Base = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.skToolMenu.BaseFore = System.Drawing.Color.Black;
            this.skToolMenu.BaseForeAnamorphosis = false;
            this.skToolMenu.BaseForeAnamorphosisBorder = 4;
            this.skToolMenu.BaseForeAnamorphosisColor = System.Drawing.Color.White;
            this.skToolMenu.BaseForeOffset = new System.Drawing.Point(0, 0);
            this.skToolMenu.BaseHoverFore = System.Drawing.Color.Black;
            this.skToolMenu.BaseItemAnamorphosis = true;
            this.skToolMenu.BaseItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.skToolMenu.BaseItemBorderShow = true;
            this.skToolMenu.BaseItemDown = ((System.Drawing.Image)(resources.GetObject("skToolMenu.BaseItemDown")));
            this.skToolMenu.BaseItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.skToolMenu.BaseItemMouse = ((System.Drawing.Image)(resources.GetObject("skToolMenu.BaseItemMouse")));
            this.skToolMenu.BaseItemPressed = System.Drawing.Color.Transparent;
            this.skToolMenu.BaseItemRadius = 2;
            this.skToolMenu.BaseItemRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skToolMenu.BaseItemSplitter = System.Drawing.Color.Transparent;
            this.skToolMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.skToolMenu.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skToolMenu.Fore = System.Drawing.Color.Black;
            this.skToolMenu.GripMargin = new System.Windows.Forms.Padding(2, 2, 4, 2);
            this.skToolMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.skToolMenu.HoverFore = System.Drawing.Color.White;
            this.skToolMenu.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.skToolMenu.ItemAnamorphosis = false;
            this.skToolMenu.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skToolMenu.ItemBorderShow = false;
            this.skToolMenu.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skToolMenu.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skToolMenu.ItemRadius = 3;
            this.skToolMenu.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.None;
            this.skToolMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonEmotion,
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSplitButton1,
            this.toolStripButton_audioMsg,
            this.toolStripButton_snapchat});
            this.skToolMenu.Location = new System.Drawing.Point(0, 218);
            this.skToolMenu.Name = "skToolMenu";
            this.skToolMenu.Padding = new System.Windows.Forms.Padding(0);
            this.skToolMenu.RadiusStyle = CCWin.SkinClass.RoundStyle.None;
            this.skToolMenu.Size = new System.Drawing.Size(399, 32);
            this.skToolMenu.SkinAllColor = true;
            this.skToolMenu.TabIndex = 146;
            this.skToolMenu.Text = "skinToolStrip1";
            this.skToolMenu.TitleAnamorphosis = false;
            this.skToolMenu.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(228)))), ((int)(((byte)(236)))));
            this.skToolMenu.TitleRadius = 4;
            this.skToolMenu.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // toolStripButtonEmotion
            // 
            this.toolStripButtonEmotion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEmotion.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEmotion.Image")));
            this.toolStripButtonEmotion.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonEmotion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEmotion.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.toolStripButtonEmotion.Name = "toolStripButtonEmotion";
            this.toolStripButtonEmotion.Size = new System.Drawing.Size(46, 29);
            this.toolStripButtonEmotion.Text = "toolStripButton2";
            this.toolStripButtonEmotion.ToolTipText = "表情";
            this.toolStripButtonEmotion.MouseEnter += new System.EventHandler(this.toolStripButtonEmotion_MouseEnter);
            this.toolStripButtonEmotion.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolStripButtonEmotion_MouseUp);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(134, 26);
            this.toolStripButton1.Text = "消息记录";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Margin = new System.Windows.Forms.Padding(1, 2, 2, 4);
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(46, 26);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.ToolTipText = "图片";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click_1);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.发送屏幕截屏ToolStripMenuItem,
            this.ToolStripMenuIte_hideForm});
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(47, 26);
            this.toolStripSplitButton1.Text = "屏幕截图";
            this.toolStripSplitButton1.ButtonClick += new System.EventHandler(this.toolStripSplitButton1_ButtonClick);
            // 
            // 发送屏幕截屏ToolStripMenuItem
            // 
            this.发送屏幕截屏ToolStripMenuItem.Name = "发送屏幕截屏ToolStripMenuItem";
            this.发送屏幕截屏ToolStripMenuItem.Size = new System.Drawing.Size(315, 44);
            this.发送屏幕截屏ToolStripMenuItem.Text = "发送全屏截图";
            this.发送屏幕截屏ToolStripMenuItem.Click += new System.EventHandler(this.发送屏幕截屏ToolStripMenuItem_Click);
            // 
            // ToolStripMenuIte_hideForm
            // 
            this.ToolStripMenuIte_hideForm.Name = "ToolStripMenuIte_hideForm";
            this.ToolStripMenuIte_hideForm.Size = new System.Drawing.Size(315, 44);
            this.ToolStripMenuIte_hideForm.Text = "截图时隐藏窗体";
            this.ToolStripMenuIte_hideForm.Click += new System.EventHandler(this.截图时隐藏窗体ToolStripMenuItem_Click);
            // 
            // toolStripButton_audioMsg
            // 
            this.toolStripButton_audioMsg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_audioMsg.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_audioMsg.Image")));
            this.toolStripButton_audioMsg.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton_audioMsg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_audioMsg.Name = "toolStripButton_audioMsg";
            this.toolStripButton_audioMsg.Size = new System.Drawing.Size(46, 26);
            this.toolStripButton_audioMsg.Text = "语音消息";
            this.toolStripButton_audioMsg.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton_snapchat
            // 
            this.toolStripButton_snapchat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_snapchat.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_snapchat.Image")));
            this.toolStripButton_snapchat.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton_snapchat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_snapchat.Name = "toolStripButton_snapchat";
            this.toolStripButton_snapchat.Size = new System.Drawing.Size(46, 26);
            this.toolStripButton_snapchat.Text = "toolStripButton2";
            this.toolStripButton_snapchat.ToolTipText = "悄悄话";
            this.toolStripButton_snapchat.Visible = false;
            this.toolStripButton_snapchat.Click += new System.EventHandler(this.toolStripButton_snapchat_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnClose.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.btnClose.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.DownBack = global::GGTalk.Properties.Resources.button_frame;
            this.btnClose.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.btnClose.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.Location = new System.Drawing.Point(247, 348);
            this.btnClose.MouseBack = global::GGTalk.Properties.Resources.button_frame_pre;
            this.btnClose.Name = "btnClose";
            this.btnClose.NormlBack = global::GGTalk.Properties.Resources.button_frame;
            this.btnClose.Palace = true;
            this.btnClose.Size = new System.Drawing.Size(65, 28);
            this.btnClose.TabIndex = 145;
            this.btnClose.Text = "关闭";
            this.btnClose.UseHandCursor = false;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // atMessagePanle1
            // 
            this.atMessagePanle1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.atMessagePanle1.AutoSize = true;
            this.atMessagePanle1.Location = new System.Drawing.Point(0, 0);
            this.atMessagePanle1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.atMessagePanle1.Name = "atMessagePanle1";
            this.atMessagePanle1.Size = new System.Drawing.Size(399, 31);
            this.atMessagePanle1.TabIndex = 155;
            this.atMessagePanle1.Visible = false;
            // 
            // chatBox_history
            // 
            this.chatBox_history.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chatBox_history.BackColor = System.Drawing.Color.White;
            this.chatBox_history.ForeColor = System.Drawing.Color.Transparent;
            this.chatBox_history.Location = new System.Drawing.Point(0, 0);
            this.chatBox_history.Name = "chatBox_history";
            this.chatBox_history.Size = new System.Drawing.Size(399, 215);
            this.chatBox_history.TabIndex = 147;
            // 
            // ChatPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.skinLabel_Ban);
            this.Controls.Add(this.atMessagePanle1);
            this.Controls.Add(this.panel_audioMessage);
            this.Controls.Add(this.gifBox_wait);
            this.Controls.Add(this.btnSendMenu);
            this.Controls.Add(this.skinButton_send);
            this.Controls.Add(this.chatBoxSend);
            this.Controls.Add(this.skToolMenu);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.chatBox_history);
            this.Name = "ChatPanel";
            this.Size = new System.Drawing.Size(399, 380);
            this.panel_audioMessage.ResumeLayout(false);
            this.panel_audioMessage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.skinContextMenuStrip4Send.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.skToolMenu.ResumeLayout(false);
            this.skToolMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinButton btnSendMenu;
        private System.Windows.Forms.ToolStripButton toolStripButtonEmotion;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem 发送屏幕截屏ToolStripMenuItem;
        private CCWin.SkinControl.SkinButton btnClose;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ToolTip toolShow;
        private CCWin.SkinControl.SkinContextMenuStrip skinContextMenuStrip4Send;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_enter;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_ctrl;
        private CCWin.SkinControl.SkinContextMenuStrip skinContextMenuStrip_quickAnswer;
        private CCWin.SkinControl.SkinButton skinButton_send;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuIte_hideForm;
        private GifBox gifBox_wait;
        private System.Windows.Forms.ToolStripButton toolStripButton_audioMsg;
        private System.Windows.Forms.Panel panel_audioMessage;
        private System.Windows.Forms.PictureBox pictureBox1;
        private CCWin.SkinControl.SkinButton skinButton2;
        private CCWin.SkinControl.SkinButton skinButton1;
        private CCWin.SkinControl.SkinLabel skinLabel1;
        private AtMessagePanle atMessagePanle1;
        private CCWin.SkinControl.SkinToolStrip skToolMenu;
        private System.Windows.Forms.ToolStripButton toolStripButton_snapchat;
        private ChatBox chatBoxSend;
        private CCWin.SkinControl.SkinLabel skinLabel_Ban;
        private Controls.ChatRender4Dll.ChatBox chatBox_history;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 复制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 另存为ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新窗口显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清屏ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
    }
}
