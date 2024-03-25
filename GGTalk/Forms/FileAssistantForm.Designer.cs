namespace GGTalk
{
    partial class FileAssistantForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileAssistantForm));
            CCWin.SkinControl.Animation animation1 = new CCWin.SkinControl.Animation();
            this.skinToolStrip2 = new CCWin.SkinControl.SkinToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.skinPanel_right = new CCWin.SkinControl.SkinPanel();
            this.skinTabControl2 = new CCWin.SkinControl.SkinTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.fileTransferingViewer1 = new ESFramework.Boost.Controls.FileTransferingViewer();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.chatPanel1 = new GGTalk.ChatPanel();
            this.skinToolStrip2.SuspendLayout();
            this.skinPanel_right.SuspendLayout();
            this.skinTabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // skinToolStrip2
            // 
            this.skinToolStrip2.Arrow = System.Drawing.Color.White;
            this.skinToolStrip2.AutoSize = false;
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
            this.toolStripButton1});
            this.skinToolStrip2.Location = new System.Drawing.Point(4, 28);
            this.skinToolStrip2.Name = "skinToolStrip2";
            this.skinToolStrip2.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinToolStrip2.Size = new System.Drawing.Size(659, 33);
            this.skinToolStrip2.SkinAllColor = true;
            this.skinToolStrip2.TabIndex = 134;
            this.skinToolStrip2.Text = "skinToolStrip2";
            this.skinToolStrip2.TitleAnamorphosis = false;
            this.skinToolStrip2.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(228)))), ((int)(((byte)(236)))));
            this.skinToolStrip2.TitleRadius = 4;
            this.skinToolStrip2.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::GGTalk.Properties.Resources.up_btn_icon;
            this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(36, 30);
            this.toolStripButton1.Text = "传输文件";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // skinPanel_right
            // 
            this.skinPanel_right.BackColor = System.Drawing.Color.White;
            this.skinPanel_right.Controls.Add(this.skinTabControl2);
            this.skinPanel_right.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinPanel_right.Dock = System.Windows.Forms.DockStyle.Right;
            this.skinPanel_right.DownBack = null;
            this.skinPanel_right.Location = new System.Drawing.Point(445, 61);
            this.skinPanel_right.MouseBack = null;
            this.skinPanel_right.Name = "skinPanel_right";
            this.skinPanel_right.NormlBack = null;
            this.skinPanel_right.Size = new System.Drawing.Size(218, 437);
            this.skinPanel_right.TabIndex = 141;
            this.skinPanel_right.Visible = false;
            // 
            // skinTabControl2
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
            this.skinTabControl2.Animation = animation1;
            this.skinTabControl2.AnimatorType = CCWin.SkinControl.AnimationType.HorizSlide;
            this.skinTabControl2.CloseRect = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.skinTabControl2.Controls.Add(this.tabPage1);
            this.skinTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTabControl2.ItemSize = new System.Drawing.Size(70, 36);
            this.skinTabControl2.Location = new System.Drawing.Point(0, 0);
            this.skinTabControl2.Name = "skinTabControl2";
            this.skinTabControl2.PageArrowDown = ((System.Drawing.Image)(resources.GetObject("skinTabControl2.PageArrowDown")));
            this.skinTabControl2.PageArrowHover = ((System.Drawing.Image)(resources.GetObject("skinTabControl2.PageArrowHover")));
            this.skinTabControl2.PageCloseHover = ((System.Drawing.Image)(resources.GetObject("skinTabControl2.PageCloseHover")));
            this.skinTabControl2.PageCloseNormal = ((System.Drawing.Image)(resources.GetObject("skinTabControl2.PageCloseNormal")));
            this.skinTabControl2.PageDown = ((System.Drawing.Image)(resources.GetObject("skinTabControl2.PageDown")));
            this.skinTabControl2.PageHover = ((System.Drawing.Image)(resources.GetObject("skinTabControl2.PageHover")));
            this.skinTabControl2.PageImagePosition = CCWin.SkinControl.SkinTabControl.ePageImagePosition.Left;
            this.skinTabControl2.PageNorml = null;
            this.skinTabControl2.SelectedIndex = 0;
            this.skinTabControl2.Size = new System.Drawing.Size(218, 437);
            this.skinTabControl2.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.skinTabControl2.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.fileTransferingViewer1);
            this.tabPage1.Location = new System.Drawing.Point(0, 36);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(218, 401);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "文件";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // fileTransferingViewer1
            // 
            this.fileTransferingViewer1.AutoScroll = true;
            this.fileTransferingViewer1.BackColor = System.Drawing.Color.Transparent;
            this.fileTransferingViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileTransferingViewer1.Location = new System.Drawing.Point(3, 3);
            this.fileTransferingViewer1.Name = "fileTransferingViewer1";
            this.fileTransferingViewer1.Size = new System.Drawing.Size(212, 395);
            this.fileTransferingViewer1.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(442, 61);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 437);
            this.splitter1.TabIndex = 142;
            this.splitter1.TabStop = false;
            this.splitter1.Visible = false;
            // 
            // chatPanel1
            // 
            this.chatPanel1.BackColor = System.Drawing.Color.Transparent;
            this.chatPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatPanel1.Location = new System.Drawing.Point(4, 61);
            this.chatPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.chatPanel1.Name = "chatPanel1";
            this.chatPanel1.Size = new System.Drawing.Size(438, 437);
            this.chatPanel1.TabIndex = 143;
            // 
            // FileAssistantForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(667, 502);
            this.Controls.Add(this.chatPanel1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.skinPanel_right);
            this.Controls.Add(this.skinToolStrip2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.Name = "FileAssistantForm";
            this.ShowInTaskbar = true;
            this.Text = "文件传输助手";
            this.TopMost = false;
            this.UseCustomIcon = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FileAssistantForm_FormClosing);
            this.Shown += new System.EventHandler(this.FileAssistantForm_Shown);
            this.skinToolStrip2.ResumeLayout(false);
            this.skinToolStrip2.PerformLayout();
            this.skinPanel_right.ResumeLayout(false);
            this.skinTabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinToolStrip skinToolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private CCWin.SkinControl.SkinPanel skinPanel_right;
        private System.Windows.Forms.Splitter splitter1;
        private ChatPanel chatPanel1;
        private CCWin.SkinControl.SkinTabControl skinTabControl2;
        private System.Windows.Forms.TabPage tabPage1;
        private ESFramework.Boost.Controls.FileTransferingViewer fileTransferingViewer1;
    }
}