
namespace GGTalk
{
    partial class FriendChatForm
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
            CCWin.SkinControl.Animation animation1 = new CCWin.SkinControl.Animation();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FriendChatForm));
            CCWin.SkinControl.Animation animation2 = new CCWin.SkinControl.Animation();
            this.toolShow = new System.Windows.Forms.ToolTip(this.components);
            this.skinLabel_FriendID = new CCWin.SkinControl.SkinLabel();
            this.skinLabel_FriendName = new CCWin.SkinControl.SkinLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.skinAnimator1 = new CCWin.SkinControl.SkinAnimator(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.chatPanel1 = new GGTalk.ChatPanel();
            this.panelFriendHeadImage = new CCWin.SkinControl.SkinPanel();
            this.skinPanel_status = new CCWin.SkinControl.SkinPanel();
            this.skinLabel_inputing = new CCWin.SkinControl.SkinLabel();
            this.skinTabControl1 = new CCWin.SkinControl.SkinTabControl();
            this.skinPanel_right = new CCWin.SkinControl.SkinPanel();
            this.skinPanel_friend = new CCWin.SkinControl.SkinPanel();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.skinToolStrip2 = new CCWin.SkinControl.SkinToolStrip();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.ToolFile = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripMenuItem32 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem33 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem34 = new System.Windows.Forms.ToolStripMenuItem();
            this.发送离线文件夹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSplitButton2 = new System.Windows.Forms.ToolStripSplitButton();
            this.请求控制对方电脑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.请求远程协助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.桌面共享指定区域ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelFriendName = new CCWin.SkinControl.SkinLabel();
            this.labelFriendSignature = new CCWin.SkinControl.SkinLabel();
            this.skinPanel1 = new CCWin.SkinControl.SkinPanel();
            this.panel1.SuspendLayout();
            this.panelFriendHeadImage.SuspendLayout();
            this.skinPanel_right.SuspendLayout();
            this.skinToolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // skinLabel_FriendID
            // 
            this.skinLabel_FriendID.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel_FriendID.AutoSize = true;
            this.skinLabel_FriendID.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel_FriendID.BorderColor = System.Drawing.Color.White;
            this.skinAnimator1.SetDecoration(this.skinLabel_FriendID, CCWin.SkinControl.DecorationType.None);
            this.skinLabel_FriendID.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel_FriendID.Location = new System.Drawing.Point(47, 147);
            this.skinLabel_FriendID.Name = "skinLabel_FriendID";
            this.skinLabel_FriendID.Size = new System.Drawing.Size(43, 17);
            this.skinLabel_FriendID.TabIndex = 130;
            this.skinLabel_FriendID.Text = "10003";
            this.toolShow.SetToolTip(this.skinLabel_FriendID, "点击复制");
            this.skinLabel_FriendID.Click += new System.EventHandler(this.skinLabel_FriendID_Click);
            // 
            // skinLabel_FriendName
            // 
            this.skinLabel_FriendName.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel_FriendName.AutoSize = true;
            this.skinLabel_FriendName.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel_FriendName.BorderColor = System.Drawing.Color.White;
            this.skinAnimator1.SetDecoration(this.skinLabel_FriendName, CCWin.SkinControl.DecorationType.None);
            this.skinLabel_FriendName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel_FriendName.Location = new System.Drawing.Point(47, 171);
            this.skinLabel_FriendName.Name = "skinLabel_FriendName";
            this.skinLabel_FriendName.Size = new System.Drawing.Size(32, 17);
            this.skinLabel_FriendName.TabIndex = 130;
            this.skinLabel_FriendName.Text = "刘海";
            this.toolShow.SetToolTip(this.skinLabel_FriendName, "点击复制");
            this.skinLabel_FriendName.Click += new System.EventHandler(this.skinLabel_FriendName_Click);
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
            // skinAnimator1
            // 
            this.skinAnimator1.AnimationType = CCWin.SkinControl.AnimationType.Custom;
            this.skinAnimator1.Cursor = null;
            animation1.AnimateOnlyDifferences = true;
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
            animation1.TimeCoeff = 0F;
            animation1.TransparencyCoeff = 0F;
            this.skinAnimator1.DefaultAnimation = animation1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.chatPanel1);
            this.skinAnimator1.SetDecoration(this.panel1, CCWin.SkinControl.DecorationType.None);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 91);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(609, 505);
            this.panel1.TabIndex = 136;
            // 
            // chatPanel1
            // 
            this.chatPanel1.BackColor = System.Drawing.Color.Transparent;
            this.skinAnimator1.SetDecoration(this.chatPanel1, CCWin.SkinControl.DecorationType.None);
            this.chatPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatPanel1.Location = new System.Drawing.Point(0, 0);
            this.chatPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.chatPanel1.Name = "chatPanel1";
            this.chatPanel1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.chatPanel1.Size = new System.Drawing.Size(609, 505);
            this.chatPanel1.TabIndex = 139;
            // 
            // panelFriendHeadImage
            // 
            this.panelFriendHeadImage.BackColor = System.Drawing.Color.Transparent;
            this.panelFriendHeadImage.BackgroundImage = global::GGTalk.Properties.Resources._64;
            this.panelFriendHeadImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelFriendHeadImage.Controls.Add(this.skinPanel_status);
            this.panelFriendHeadImage.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.panelFriendHeadImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.skinAnimator1.SetDecoration(this.panelFriendHeadImage, CCWin.SkinControl.DecorationType.None);
            this.panelFriendHeadImage.DownBack = null;
            this.panelFriendHeadImage.Location = new System.Drawing.Point(13, 11);
            this.panelFriendHeadImage.Margin = new System.Windows.Forms.Padding(0);
            this.panelFriendHeadImage.MouseBack = null;
            this.panelFriendHeadImage.Name = "panelFriendHeadImage";
            this.panelFriendHeadImage.NormlBack = null;
            this.panelFriendHeadImage.Radius = 5;
            this.panelFriendHeadImage.Size = new System.Drawing.Size(42, 40);
            this.panelFriendHeadImage.TabIndex = 6;
            this.panelFriendHeadImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelFriendHeadImage_MouseClick);
            // 
            // skinPanel_status
            // 
            this.skinPanel_status.BackColor = System.Drawing.Color.Transparent;
            this.skinPanel_status.BackgroundImage = global::GGTalk.Properties.Resources.imonline__2_;
            this.skinPanel_status.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.skinPanel_status.BackRectangle = new System.Drawing.Rectangle(4, 4, 4, 4);
            this.skinPanel_status.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinAnimator1.SetDecoration(this.skinPanel_status, CCWin.SkinControl.DecorationType.None);
            this.skinPanel_status.DownBack = null;
            this.skinPanel_status.Location = new System.Drawing.Point(25, 23);
            this.skinPanel_status.MouseBack = null;
            this.skinPanel_status.Name = "skinPanel_status";
            this.skinPanel_status.NormlBack = null;
            this.skinPanel_status.Size = new System.Drawing.Size(25, 23);
            this.skinPanel_status.TabIndex = 135;
            this.skinPanel_status.Visible = false;
            // 
            // skinLabel_inputing
            // 
            this.skinLabel_inputing.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.Anamorphosis;
            this.skinLabel_inputing.AutoSize = true;
            this.skinLabel_inputing.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel_inputing.BorderColor = System.Drawing.Color.White;
            this.skinAnimator1.SetDecoration(this.skinLabel_inputing, CCWin.SkinControl.DecorationType.None);
            this.skinLabel_inputing.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel_inputing.ForeColor = System.Drawing.Color.Black;
            this.skinLabel_inputing.Location = new System.Drawing.Point(169, 15);
            this.skinLabel_inputing.Name = "skinLabel_inputing";
            this.skinLabel_inputing.Size = new System.Drawing.Size(53, 12);
            this.skinLabel_inputing.TabIndex = 135;
            this.skinLabel_inputing.Text = "正在输入";
            this.skinLabel_inputing.Visible = false;
            // 
            // skinTabControl1
            // 
            animation2.AnimateOnlyDifferences = true;
            animation2.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation2.BlindCoeff")));
            animation2.LeafCoeff = 0F;
            animation2.MaxTime = 1F;
            animation2.MinTime = 0F;
            animation2.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation2.MosaicCoeff")));
            animation2.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation2.MosaicShift")));
            animation2.MosaicSize = 0;
            animation2.Padding = new System.Windows.Forms.Padding(0);
            animation2.RotateCoeff = 0F;
            animation2.RotateLimit = 0F;
            animation2.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation2.ScaleCoeff")));
            animation2.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation2.SlideCoeff")));
            animation2.TimeCoeff = 0F;
            animation2.TransparencyCoeff = 0F;
            this.skinTabControl1.Animation = animation2;
            this.skinTabControl1.AnimationStart = false;
            this.skinTabControl1.AnimatorType = CCWin.SkinControl.AnimationType.HorizSlide;
            this.skinTabControl1.CloseRect = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.skinAnimator1.SetDecoration(this.skinTabControl1, CCWin.SkinControl.DecorationType.None);
            this.skinTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTabControl1.ItemSize = new System.Drawing.Size(70, 36);
            this.skinTabControl1.Location = new System.Drawing.Point(0, 0);
            this.skinTabControl1.Name = "skinTabControl1";
            this.skinTabControl1.PageArrowDown = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageArrowDown")));
            this.skinTabControl1.PageArrowHover = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageArrowHover")));
            this.skinTabControl1.PageCloseHover = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageCloseHover")));
            this.skinTabControl1.PageCloseNormal = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageCloseNormal")));
            this.skinTabControl1.PageDown = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageDown")));
            this.skinTabControl1.PageHover = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageHover")));
            this.skinTabControl1.PageImagePosition = CCWin.SkinControl.SkinTabControl.ePageImagePosition.Left;
            this.skinTabControl1.PageNorml = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageNorml")));
            this.skinTabControl1.SelectedIndex = 0;
            this.skinTabControl1.Size = new System.Drawing.Size(218, 505);
            this.skinTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.skinTabControl1.TabIndex = 0;
            this.skinTabControl1.Visible = false;
            // 
            // skinPanel_right
            // 
            this.skinPanel_right.BackColor = System.Drawing.Color.White;
            this.skinPanel_right.Controls.Add(this.skinTabControl1);
            this.skinPanel_right.Controls.Add(this.skinPanel_friend);
            this.skinPanel_right.Controls.Add(this.skinLabel_FriendID);
            this.skinPanel_right.Controls.Add(this.skinLabel_FriendName);
            this.skinPanel_right.Controls.Add(this.skinLabel2);
            this.skinPanel_right.Controls.Add(this.skinLabel1);
            this.skinPanel_right.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinAnimator1.SetDecoration(this.skinPanel_right, CCWin.SkinControl.DecorationType.None);
            this.skinPanel_right.Dock = System.Windows.Forms.DockStyle.Right;
            this.skinPanel_right.DownBack = null;
            this.skinPanel_right.Location = new System.Drawing.Point(613, 91);
            this.skinPanel_right.MouseBack = null;
            this.skinPanel_right.Name = "skinPanel_right";
            this.skinPanel_right.NormlBack = null;
            this.skinPanel_right.Size = new System.Drawing.Size(218, 505);
            this.skinPanel_right.TabIndex = 134;
            // 
            // skinPanel_friend
            // 
            this.skinPanel_friend.BackColor = System.Drawing.Color.Transparent;
            this.skinPanel_friend.BackgroundImage = global::GGTalk.Properties.Resources._7;
            this.skinPanel_friend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.skinPanel_friend.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinAnimator1.SetDecoration(this.skinPanel_friend, CCWin.SkinControl.DecorationType.None);
            this.skinPanel_friend.DownBack = null;
            this.skinPanel_friend.Location = new System.Drawing.Point(50, 13);
            this.skinPanel_friend.Margin = new System.Windows.Forms.Padding(0);
            this.skinPanel_friend.MouseBack = null;
            this.skinPanel_friend.Name = "skinPanel_friend";
            this.skinPanel_friend.NormlBack = null;
            this.skinPanel_friend.Radius = 4;
            this.skinPanel_friend.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinPanel_friend.Size = new System.Drawing.Size(120, 120);
            this.skinPanel_friend.TabIndex = 6;

            // 
            // skinLabel2
            // 
            this.skinLabel2.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinAnimator1.SetDecoration(this.skinLabel2, CCWin.SkinControl.DecorationType.None);
            this.skinLabel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel2.Location = new System.Drawing.Point(5, 171);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(44, 17);
            this.skinLabel2.TabIndex = 130;
            this.skinLabel2.Text = "名称：";
            // 
            // skinLabel1
            // 
            this.skinLabel1.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinAnimator1.SetDecoration(this.skinLabel1, CCWin.SkinControl.DecorationType.None);
            this.skinLabel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel1.Location = new System.Drawing.Point(5, 147);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(44, 17);
            this.skinLabel1.TabIndex = 130;
            this.skinLabel1.Text = "帐号：";
            // 
            // skinToolStrip2
            // 
            this.skinToolStrip2.Arrow = System.Drawing.Color.White;
            this.skinToolStrip2.Back = System.Drawing.Color.White;
            this.skinToolStrip2.BackColor = System.Drawing.Color.Transparent;
            this.skinToolStrip2.BackRadius = 4;
            this.skinToolStrip2.BackRectangle = new System.Drawing.Rectangle(10, 10, 10, 10);
            this.skinToolStrip2.Base = System.Drawing.Color.Transparent;
            this.skinToolStrip2.BaseFore = System.Drawing.Color.Black;
            this.skinToolStrip2.BaseForeAnamorphosis = false;
            this.skinToolStrip2.BaseForeAnamorphosisBorder = 4;
            this.skinToolStrip2.BaseForeAnamorphosisColor = System.Drawing.Color.White;
            this.skinToolStrip2.BaseForeOffset = new System.Drawing.Point(0, 0);
            this.skinToolStrip2.BaseHoverFore = System.Drawing.Color.White;
            this.skinToolStrip2.BaseItemAnamorphosis = true;
            this.skinToolStrip2.BaseItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(93)))), ((int)(((byte)(93)))));
            this.skinToolStrip2.BaseItemBorderShow = true;
            this.skinToolStrip2.BaseItemDown = ((System.Drawing.Image)(resources.GetObject("skinToolStrip2.BaseItemDown")));
            this.skinToolStrip2.BaseItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.skinToolStrip2.BaseItemMouse = ((System.Drawing.Image)(resources.GetObject("skinToolStrip2.BaseItemMouse")));
            this.skinToolStrip2.BaseItemPressed = System.Drawing.Color.Transparent;
            this.skinToolStrip2.BaseItemRadius = 2;
            this.skinToolStrip2.BaseItemRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinToolStrip2.BaseItemSplitter = System.Drawing.Color.Transparent;
            this.skinAnimator1.SetDecoration(this.skinToolStrip2, CCWin.SkinControl.DecorationType.None);
            this.skinToolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.skinToolStrip2.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinToolStrip2.Fore = System.Drawing.Color.Black;
            this.skinToolStrip2.GripMargin = new System.Windows.Forms.Padding(2, 2, 4, 2);
            this.skinToolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.skinToolStrip2.HoverFore = System.Drawing.Color.White;
            this.skinToolStrip2.ItemAnamorphosis = false;
            this.skinToolStrip2.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinToolStrip2.ItemBorderShow = false;
            this.skinToolStrip2.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinToolStrip2.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinToolStrip2.ItemRadius = 1;
            this.skinToolStrip2.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.None;
            this.skinToolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton5,
            this.toolStripButton6,
            this.ToolFile,
            this.toolStripSplitButton2});
            this.skinToolStrip2.Location = new System.Drawing.Point(4, 60);
            this.skinToolStrip2.Name = "skinToolStrip2";
            this.skinToolStrip2.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinToolStrip2.Size = new System.Drawing.Size(181, 39);
            this.skinToolStrip2.SkinAllColor = true;
            this.skinToolStrip2.TabIndex = 133;
            this.skinToolStrip2.Text = "skinToolStrip2";
            this.skinToolStrip2.TitleAnamorphosis = false;
            this.skinToolStrip2.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(228)))), ((int)(((byte)(236)))));
            this.skinToolStrip2.TitleRadius = 4;
            this.skinToolStrip2.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = global::GGTalk.Properties.Resources.video_btn_icon;
            this.toolStripButton5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton5.Text = "toolStripButton5";
            this.toolStripButton5.ToolTipText = "视频会话";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripDropDownButton1_ButtonClick);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = global::GGTalk.Properties.Resources.voice_btn_icon;
            this.toolStripButton6.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton6.Text = "语音对话";
            this.toolStripButton6.ToolTipText = "语音对话";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // ToolFile
            // 
            this.ToolFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem32,
            this.toolStripMenuItem33,
            this.toolStripMenuItem34,
            this.发送离线文件夹ToolStripMenuItem});
            this.ToolFile.Image = global::GGTalk.Properties.Resources.up_btn_icon;
            this.ToolFile.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ToolFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolFile.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.ToolFile.Name = "ToolFile";
            this.ToolFile.Size = new System.Drawing.Size(48, 36);
            this.ToolFile.MouseEnter += new System.EventHandler(this.FocusCurrent);
            // 
            // toolStripMenuItem32
            // 
            this.toolStripMenuItem32.Name = "toolStripMenuItem32";
            this.toolStripMenuItem32.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem32.Text = "发送文件";
            this.toolStripMenuItem32.Click += new System.EventHandler(this.toolStripButton_fileTransfer_Click);
            // 
            // toolStripMenuItem33
            // 
            this.toolStripMenuItem33.Name = "toolStripMenuItem33";
            this.toolStripMenuItem33.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem33.Text = "发送文件夹";
            this.toolStripMenuItem33.Click += new System.EventHandler(this.toolStripMenuItem33_Click);
            // 
            // toolStripMenuItem34
            // 
            this.toolStripMenuItem34.Name = "toolStripMenuItem34";
            this.toolStripMenuItem34.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem34.Text = "发送离线文件";
            this.toolStripMenuItem34.Click += new System.EventHandler(this.toolStripMenuItem34_Click);
            // 
            // 发送离线文件夹ToolStripMenuItem
            // 
            this.发送离线文件夹ToolStripMenuItem.Name = "发送离线文件夹ToolStripMenuItem";
            this.发送离线文件夹ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.发送离线文件夹ToolStripMenuItem.Text = "发送离线文件夹";
            this.发送离线文件夹ToolStripMenuItem.Click += new System.EventHandler(this.发送离线文件夹ToolStripMenuItem_Click);
            // 
            // toolStripSplitButton2
            // 
            this.toolStripSplitButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.请求控制对方电脑ToolStripMenuItem,
            this.请求远程协助ToolStripMenuItem,
            this.桌面共享指定区域ToolStripMenuItem});
            this.toolStripSplitButton2.Image = global::GGTalk.Properties.Resources.control_btn_icon;
            this.toolStripSplitButton2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripSplitButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton2.Name = "toolStripSplitButton2";
            this.toolStripSplitButton2.Size = new System.Drawing.Size(48, 36);
            this.toolStripSplitButton2.MouseEnter += new System.EventHandler(this.FocusCurrent2);
            // 
            // 请求控制对方电脑ToolStripMenuItem
            // 
            this.请求控制对方电脑ToolStripMenuItem.Name = "请求控制对方电脑ToolStripMenuItem";
            this.请求控制对方电脑ToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.请求控制对方电脑ToolStripMenuItem.Text = "请求控制对方电脑";
            this.请求控制对方电脑ToolStripMenuItem.Click += new System.EventHandler(this.请求控制对方电脑ToolStripMenuItem_Click);
            // 
            // 请求远程协助ToolStripMenuItem
            // 
            this.请求远程协助ToolStripMenuItem.Name = "请求远程协助ToolStripMenuItem";
            this.请求远程协助ToolStripMenuItem.ShowShortcutKeys = false;
            this.请求远程协助ToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.请求远程协助ToolStripMenuItem.Text = "请求远程协助";
            this.请求远程协助ToolStripMenuItem.Click += new System.EventHandler(this.请求远程协助ToolStripMenuItem_Click);
            // 
            // 桌面共享指定区域ToolStripMenuItem
            // 
            this.桌面共享指定区域ToolStripMenuItem.Name = "桌面共享指定区域ToolStripMenuItem";
            this.桌面共享指定区域ToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.桌面共享指定区域ToolStripMenuItem.Text = "请求远程协助（指定桌面区域）";
            this.桌面共享指定区域ToolStripMenuItem.Click += new System.EventHandler(this.桌面共享指定区域ToolStripMenuItem_Click);
            // 
            // labelFriendName
            // 
            this.labelFriendName.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.labelFriendName.AutoSize = true;
            this.labelFriendName.BackColor = System.Drawing.Color.Transparent;
            this.labelFriendName.BorderColor = System.Drawing.Color.White;
            this.labelFriendName.BorderSize = 4;
            this.labelFriendName.Cursor = System.Windows.Forms.Cursors.Default;
            this.skinAnimator1.SetDecoration(this.labelFriendName, CCWin.SkinControl.DecorationType.None);
            this.labelFriendName.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.labelFriendName.ForeColor = System.Drawing.Color.Black;
            this.labelFriendName.Location = new System.Drawing.Point(58, 7);
            this.labelFriendName.Name = "labelFriendName";
            this.labelFriendName.Size = new System.Drawing.Size(82, 24);
            this.labelFriendName.TabIndex = 100;
            this.labelFriendName.Text = "我的好友";
            // 
            // labelFriendSignature
            // 
            this.labelFriendSignature.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.labelFriendSignature.AutoSize = true;
            this.labelFriendSignature.BackColor = System.Drawing.Color.Transparent;
            this.labelFriendSignature.BorderColor = System.Drawing.Color.White;
            this.labelFriendSignature.BorderSize = 4;
            this.skinAnimator1.SetDecoration(this.labelFriendSignature, CCWin.SkinControl.DecorationType.None);
            this.labelFriendSignature.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelFriendSignature.ForeColor = System.Drawing.Color.Black;
            this.labelFriendSignature.Location = new System.Drawing.Point(61, 36);
            this.labelFriendSignature.Name = "labelFriendSignature";
            this.labelFriendSignature.Size = new System.Drawing.Size(80, 17);
            this.labelFriendSignature.TabIndex = 103;
            this.labelFriendSignature.Text = "我的个性签名";
            // 
            // skinPanel1
            // 
            this.skinPanel1.BackColor = System.Drawing.Color.Transparent;
            this.skinPanel1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinAnimator1.SetDecoration(this.skinPanel1, CCWin.SkinControl.DecorationType.None);
            this.skinPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.skinPanel1.DownBack = null;
            this.skinPanel1.Location = new System.Drawing.Point(4, 28);
            this.skinPanel1.MouseBack = null;
            this.skinPanel1.Name = "skinPanel1";
            this.skinPanel1.NormlBack = null;
            this.skinPanel1.Size = new System.Drawing.Size(827, 63);
            this.skinPanel1.TabIndex = 137;
            // 
            // FriendChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Back = ((System.Drawing.Image)(resources.GetObject("$this.Back")));
            this.BorderPalace = global::GGTalk.Properties.Resources.BackPalace;
            this.CanResize = true;
            this.ClientSize = new System.Drawing.Size(835, 600);
            this.CloseDownBack = global::GGTalk.Properties.Resources.btn_close_down;
            this.CloseMouseBack = global::GGTalk.Properties.Resources.btn_close_highlight;
            this.CloseNormlBack = global::GGTalk.Properties.Resources.btn_close_disable;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelFriendHeadImage);
            this.Controls.Add(this.skinLabel_inputing);
            this.Controls.Add(this.skinPanel_right);
            this.Controls.Add(this.skinToolStrip2);
            this.Controls.Add(this.labelFriendName);
            this.Controls.Add(this.labelFriendSignature);
            this.Controls.Add(this.skinPanel1);
            this.skinAnimator1.SetDecoration(this, CCWin.SkinControl.DecorationType.None);
            this.EffectCaption = CCWin.TitleType.None;
            this.EffectWidth = 4;
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.Name = "FriendChatForm";
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
            this.Load += new System.EventHandler(this.FriendChatForm_Load);
            this.Shown += new System.EventHandler(this.ChatForm_Shown);
            this.SizeChanged += new System.EventHandler(this.ChatForm_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmChat_Paint);
            this.panel1.ResumeLayout(false);
            this.panelFriendHeadImage.ResumeLayout(false);
            this.skinPanel_right.ResumeLayout(false);
            this.skinPanel_right.PerformLayout();
            this.skinToolStrip2.ResumeLayout(false);
            this.skinToolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private CCWin.SkinControl.SkinLabel labelFriendName;
        private CCWin.SkinControl.SkinLabel labelFriendSignature;
        private CCWin.SkinControl.SkinPanel panelFriendHeadImage;
        private System.Windows.Forms.ToolTip toolShow;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.FontDialog fontDialog1;
        private CCWin.SkinControl.SkinTabControl skinTabControl1;
        private CCWin.SkinControl.SkinPanel skinPanel_friend;
        private CCWin.SkinControl.SkinLabel skinLabel_FriendName;
        private CCWin.SkinControl.SkinLabel skinLabel_FriendID;
        private CCWin.SkinControl.SkinPanel skinPanel_right;
        private CCWin.SkinControl.SkinPanel skinPanel_status;
        private CCWin.SkinControl.SkinLabel skinLabel_inputing;
        private CCWin.SkinControl.SkinAnimator skinAnimator1;
        private CCWin.SkinControl.SkinLabel skinLabel2;
        private CCWin.SkinControl.SkinLabel skinLabel1;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripSplitButton ToolFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem32;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem33;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem34;
        private System.Windows.Forms.ToolStripMenuItem 发送离线文件夹ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton2;
        private System.Windows.Forms.ToolStripMenuItem 请求控制对方电脑ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 请求远程协助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 桌面共享指定区域ToolStripMenuItem;
        private CCWin.SkinControl.SkinToolStrip skinToolStrip2;
        private System.Windows.Forms.Panel panel1;
        private ChatPanel chatPanel1;
        private CCWin.SkinControl.SkinPanel skinPanel1;
    }
}