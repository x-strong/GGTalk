using ESFramework.Boost.Controls;
namespace ESFramework.Boost
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
            this.chatBoxSend = new ESFramework.Boost.Controls.ChatBox();
            this.chatBox_history = new ESFramework.Boost.Controls.ChatBox();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.toolShow = new System.Windows.Forms.ToolTip(this.components);
            this.gifBox_wait = new ESFramework.Boost.Controls.GifBox();
            this.skinButton_send = new System.Windows.Forms.Button();
            this.skToolMenu = new System.Windows.Forms.ToolStrip();
            this.toolFont = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEmotion = new System.Windows.Forms.ToolStripButton();
            this.toolVibration = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.发送屏幕截屏ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.发送本地图片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnClose = new System.Windows.Forms.Button();
            this.skToolMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // chatBoxSend
            // 
            this.chatBoxSend.AllowDrop = true;
            this.chatBoxSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chatBoxSend.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chatBoxSend.ContextMenuMode = ESFramework.Boost.Controls.ChatBoxContextMenuMode.ForInput;
            this.chatBoxSend.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.chatBoxSend.Location = new System.Drawing.Point(0, 247);
            this.chatBoxSend.Name = "chatBoxSend";
            this.chatBoxSend.PopoutImageWhenDoubleClick = false;
            this.chatBoxSend.Size = new System.Drawing.Size(363, 96);
            this.chatBoxSend.TabIndex = 148;
            this.chatBoxSend.Text = "";
            this.chatBoxSend.UseRtf = false;
            // 
            // chatBox_history
            // 
            this.chatBox_history.AllowDrop = true;
            this.chatBox_history.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chatBox_history.BackColor = System.Drawing.Color.White;
            this.chatBox_history.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chatBox_history.ContextMenuMode = ESFramework.Boost.Controls.ChatBoxContextMenuMode.ForOutput;
            this.chatBox_history.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.chatBox_history.Location = new System.Drawing.Point(0, 0);
            this.chatBox_history.Name = "chatBox_history";
            this.chatBox_history.PopoutImageWhenDoubleClick = true;
            this.chatBox_history.ReadOnly = true;
            this.chatBox_history.Size = new System.Drawing.Size(363, 217);
            this.chatBox_history.TabIndex = 147;
            this.chatBox_history.Text = "";
            this.chatBox_history.UseRtf = false;
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
            this.gifBox_wait.Image = global::ESFramework.Boost.Properties.Resources.busy;
            this.gifBox_wait.Location = new System.Drawing.Point(194, 349);
            this.gifBox_wait.Name = "gifBox_wait";
            this.gifBox_wait.Size = new System.Drawing.Size(20, 20);
            this.gifBox_wait.TabIndex = 152;
            this.gifBox_wait.Text = "gifBox1";
            this.toolShow.SetToolTip(this.gifBox_wait, "正在发送 . . .");
            this.gifBox_wait.Visible = false;
            // 
            // skinButton_send
            // 
            this.skinButton_send.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.skinButton_send.BackColor = System.Drawing.Color.Transparent;
            this.skinButton_send.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButton_send.Location = new System.Drawing.Point(292, 349);
            this.skinButton_send.Margin = new System.Windows.Forms.Padding(0);
            this.skinButton_send.Name = "skinButton_send";
            this.skinButton_send.Size = new System.Drawing.Size(61, 24);
            this.skinButton_send.TabIndex = 150;
            this.skinButton_send.Text = "发送(&S)";
            this.skinButton_send.UseVisualStyleBackColor = false;
            this.skinButton_send.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // skToolMenu
            // 
            this.skToolMenu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skToolMenu.AutoSize = false;
            this.skToolMenu.BackColor = System.Drawing.Color.Transparent;
            this.skToolMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.skToolMenu.GripMargin = new System.Windows.Forms.Padding(2, 2, 4, 2);
            this.skToolMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.skToolMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolFont,
            this.toolStripButtonEmotion,
            this.toolVibration,
            this.toolStripButton3,
            this.toolStripButton1,
            this.toolStripSplitButton1});
            this.skToolMenu.Location = new System.Drawing.Point(0, 220);
            this.skToolMenu.Name = "skToolMenu";
            this.skToolMenu.Size = new System.Drawing.Size(363, 27);
            this.skToolMenu.TabIndex = 146;
            this.skToolMenu.Text = "skinToolStrip1";
            // 
            // toolFont
            // 
            this.toolFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolFont.Image = ((System.Drawing.Image)(resources.GetObject("toolFont.Image")));
            this.toolFont.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolFont.Margin = new System.Windows.Forms.Padding(4, 1, 0, 2);
            this.toolFont.Name = "toolFont";
            this.toolFont.Size = new System.Drawing.Size(23, 24);
            this.toolFont.Text = "toolStripButton1";
            this.toolFont.ToolTipText = "字体";
            this.toolFont.Click += new System.EventHandler(this.toolFont_Click);
            // 
            // toolStripButtonEmotion
            // 
            this.toolStripButtonEmotion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEmotion.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEmotion.Image")));
            this.toolStripButtonEmotion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEmotion.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.toolStripButtonEmotion.Name = "toolStripButtonEmotion";
            this.toolStripButtonEmotion.Size = new System.Drawing.Size(23, 24);
            this.toolStripButtonEmotion.Text = "toolStripButton2";
            this.toolStripButtonEmotion.ToolTipText = "表情";
            this.toolStripButtonEmotion.MouseEnter += new System.EventHandler(this.toolStripButtonEmotion_MouseEnter);
            this.toolStripButtonEmotion.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolStripButtonEmotion_MouseUp);
            // 
            // toolVibration
            // 
            this.toolVibration.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolVibration.Image = ((System.Drawing.Image)(resources.GetObject("toolVibration.Image")));
            this.toolVibration.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolVibration.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolVibration.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.toolVibration.Name = "toolVibration";
            this.toolVibration.Size = new System.Drawing.Size(23, 24);
            this.toolVibration.Text = "toolStripButton4";
            this.toolVibration.ToolTipText = "抖动提醒";
            this.toolVibration.Click += new System.EventHandler(this.toolVibration_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 24);
            this.toolStripButton3.Text = "手写板";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton1.Image = global::ESFramework.Boost.Properties.Resources.Conversation1;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(76, 24);
            this.toolStripButton1.Text = "消息记录";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.发送屏幕截屏ToolStripMenuItem,
            this.发送本地图片ToolStripMenuItem});
            this.toolStripSplitButton1.Image = global::ESFramework.Boost.Properties.Resources.cut_red;
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 24);
            this.toolStripSplitButton1.Text = "屏幕截图";
            this.toolStripSplitButton1.ButtonClick += new System.EventHandler(this.toolStripSplitButton1_ButtonClick);
            // 
            // 发送屏幕截屏ToolStripMenuItem
            // 
            this.发送屏幕截屏ToolStripMenuItem.Name = "发送屏幕截屏ToolStripMenuItem";
            this.发送屏幕截屏ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.发送屏幕截屏ToolStripMenuItem.Text = "发送全屏截图";
            this.发送屏幕截屏ToolStripMenuItem.Click += new System.EventHandler(this.发送屏幕截屏ToolStripMenuItem_Click);
            // 
            // 发送本地图片ToolStripMenuItem
            // 
            this.发送本地图片ToolStripMenuItem.Name = "发送本地图片ToolStripMenuItem";
            this.发送本地图片ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.发送本地图片ToolStripMenuItem.Text = "发送本地图片";
            this.发送本地图片ToolStripMenuItem.Click += new System.EventHandler(this.发送本地图片ToolStripMenuItem_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.Location = new System.Drawing.Point(220, 349);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(69, 24);
            this.btnClose.TabIndex = 145;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ChatPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.gifBox_wait);
            this.Controls.Add(this.skinButton_send);
            this.Controls.Add(this.chatBoxSend);
            this.Controls.Add(this.chatBox_history);
            this.Controls.Add(this.skToolMenu);
            this.Controls.Add(this.btnClose);
            this.Name = "ChatPanel";
            this.Size = new System.Drawing.Size(363, 372);
            this.skToolMenu.ResumeLayout(false);
            this.skToolMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ChatBox chatBoxSend;
        private ChatBox chatBox_history;
        private System.Windows.Forms.ToolStrip skToolMenu;
        private System.Windows.Forms.ToolStripButton toolFont;
        private System.Windows.Forms.ToolStripButton toolStripButtonEmotion;
        private System.Windows.Forms.ToolStripButton toolVibration;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem 发送本地图片ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 发送屏幕截屏ToolStripMenuItem;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ToolTip toolShow;
        private System.Windows.Forms.Button skinButton_send;
        private GifBox gifBox_wait;
    }
}
