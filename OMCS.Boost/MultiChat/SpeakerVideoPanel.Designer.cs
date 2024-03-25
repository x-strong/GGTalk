using CCWin.SkinControl;
using OMCS.Boost.Controls;

namespace OMCS.Boost.MultiChat
{
    partial class SpeakerVideoPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpeakerVideoPanel));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel_displayName = new System.Windows.Forms.ToolStripLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox_Mic = new System.Windows.Forms.PictureBox();
            this.pictureBox_Camera = new System.Windows.Forms.PictureBox();
            this.label_tip = new System.Windows.Forms.Label();
            this.checkBox_allowSpeak = new System.Windows.Forms.CheckBox();
            this.decibelDisplayer1 = new OMCS.Boost.Controls.DecibelDisplayer();
            this.cameraPanel1 = new OMCS.Windows.CameraPanel();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Mic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Camera)).BeginInit();
            this.cameraPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripLabel_displayName});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(234, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "收起";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripLabel_displayName
            // 
            this.toolStripLabel_displayName.Name = "toolStripLabel_displayName";
            this.toolStripLabel_displayName.Size = new System.Drawing.Size(51, 22);
            this.toolStripLabel_displayName.Text = "XT0001";
            // 
            // pictureBox_Mic
            // 
            this.pictureBox_Mic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_Mic.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_Mic.BackgroundImage = global::OMCS.Boost.Properties.Resources.micDis;
            this.pictureBox_Mic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox_Mic.Location = new System.Drawing.Point(203, 10);
            this.pictureBox_Mic.Name = "pictureBox_Mic";
            this.pictureBox_Mic.Size = new System.Drawing.Size(24, 24);
            this.pictureBox_Mic.TabIndex = 1;
            this.pictureBox_Mic.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_Mic, "对方禁用了麦克风");
            this.pictureBox_Mic.Visible = false;
            this.pictureBox_Mic.Click += new System.EventHandler(this.pictureBox_Mic_Click);
            // 
            // pictureBox_Camera
            // 
            this.pictureBox_Camera.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_Camera.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_Camera.BackgroundImage = global::OMCS.Boost.Properties.Resources.cameraDis;
            this.pictureBox_Camera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox_Camera.Location = new System.Drawing.Point(81, 45);
            this.pictureBox_Camera.Name = "pictureBox_Camera";
            this.pictureBox_Camera.Size = new System.Drawing.Size(32, 32);
            this.pictureBox_Camera.TabIndex = 1;
            this.pictureBox_Camera.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_Camera, "对方禁用了摄像头");
            this.pictureBox_Camera.Visible = false;
            // 
            // label_tip
            // 
            this.label_tip.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label_tip.AutoSize = true;
            this.label_tip.BackColor = System.Drawing.Color.Transparent;
            this.label_tip.ForeColor = System.Drawing.Color.White;
            this.label_tip.Location = new System.Drawing.Point(62, 55);
            this.label_tip.Name = "label_tip";
            this.label_tip.Size = new System.Drawing.Size(71, 12);
            this.label_tip.TabIndex = 2;
            this.label_tip.Text = "正在连接...";
            // 
            // checkBox_allowSpeak
            // 
            this.checkBox_allowSpeak.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_allowSpeak.AutoSize = true;
            this.checkBox_allowSpeak.BackColor = System.Drawing.Color.Transparent;
            this.checkBox_allowSpeak.Location = new System.Drawing.Point(157, 3);
            this.checkBox_allowSpeak.Name = "checkBox_allowSpeak";
            this.checkBox_allowSpeak.Size = new System.Drawing.Size(48, 16);
            this.checkBox_allowSpeak.TabIndex = 12;
            this.checkBox_allowSpeak.Text = "发言";
            this.checkBox_allowSpeak.UseVisualStyleBackColor = false;
            this.checkBox_allowSpeak.Visible = false;
            this.checkBox_allowSpeak.CheckedChanged += new System.EventHandler(this.checkBox_allowSpeak_CheckedChanged);
            // 
            // decibelDisplayer1
            // 
            this.decibelDisplayer1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.decibelDisplayer1.BackColor = System.Drawing.Color.White;
            this.decibelDisplayer1.Location = new System.Drawing.Point(211, 7);
            this.decibelDisplayer1.Name = "decibelDisplayer1";
            this.decibelDisplayer1.Size = new System.Drawing.Size(20, 8);
            this.decibelDisplayer1.TabIndex = 10;
            this.decibelDisplayer1.ValueVisible = false;
            this.decibelDisplayer1.Working = true;
            // 
            // cameraPanel1
            // 
            this.cameraPanel1.BackColor = System.Drawing.Color.Black;
            this.cameraPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cameraPanel1.Controls.Add(this.label_tip);
            this.cameraPanel1.Controls.Add(this.pictureBox_Mic);
            this.cameraPanel1.Controls.Add(this.pictureBox_Camera); 
            this.cameraPanel1.Location = new System.Drawing.Point(1, 26);
            this.cameraPanel1.Name = "cameraPanel1";
            this.cameraPanel1.Size = new System.Drawing.Size(231, 171);
            this.cameraPanel1.TabIndex = 13;
            this.cameraPanel1.Text = "cameraPanel1"; 
            // 
            // SpeakerVideoPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_allowSpeak);
            this.Controls.Add(this.decibelDisplayer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.cameraPanel1);
            this.Name = "SpeakerVideoPanel";
            this.Size = new System.Drawing.Size(234, 200);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Mic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Camera)).EndInit();
            this.cameraPanel1.ResumeLayout(false);
            this.cameraPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_displayName;
        private DecibelDisplayer decibelDisplayer1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureBox_Mic;
        private System.Windows.Forms.PictureBox pictureBox_Camera;       
        private System.Windows.Forms.Label label_tip;
        private System.Windows.Forms.CheckBox checkBox_allowSpeak;
        private Windows.CameraPanel cameraPanel1;
    }
}
