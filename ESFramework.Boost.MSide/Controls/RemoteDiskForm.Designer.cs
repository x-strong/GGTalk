namespace ESFramework.Boost.Controls
{
    partial class RemoteDiskForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoteDiskForm));
            this.skinPanel_pic = new CCWin.SkinControl.SkinPanel();
            this.skinLabel_msg = new CCWin.SkinControl.SkinLabel();
            this.skinLabel_tip = new CCWin.SkinControl.SkinLabel();
            this.nDiskBrowser1 = new ESFramework.Boost.NetworkDisk.Passive.NDiskBrowser();
            this.SuspendLayout();
            // 
            // skinPanel_pic
            // 
            this.skinPanel_pic.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.skinPanel_pic.BackColor = System.Drawing.Color.Transparent;
            this.skinPanel_pic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.skinPanel_pic.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinPanel_pic.DownBack = null;
            this.skinPanel_pic.Location = new System.Drawing.Point(328, 208);
            this.skinPanel_pic.MouseBack = null;
            this.skinPanel_pic.Name = "skinPanel_pic";
            this.skinPanel_pic.NormlBack = null;
            this.skinPanel_pic.Size = new System.Drawing.Size(96, 96);
            this.skinPanel_pic.TabIndex = 131;
            // 
            // skinLabel_msg
            // 
            this.skinLabel_msg.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.skinLabel_msg.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel_msg.AutoSize = true;
            this.skinLabel_msg.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel_msg.BorderColor = System.Drawing.Color.White;
            this.skinLabel_msg.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel_msg.Location = new System.Drawing.Point(309, 317);
            this.skinLabel_msg.Name = "skinLabel_msg";
            this.skinLabel_msg.Size = new System.Drawing.Size(125, 17);
            this.skinLabel_msg.TabIndex = 130;
            this.skinLabel_msg.Text = "正在连接对方桌面 . . .";
            // 
            // skinLabel_tip
            // 
            this.skinLabel_tip.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel_tip.AutoSize = true;
            this.skinLabel_tip.BackColor = System.Drawing.Color.White;
            this.skinLabel_tip.BorderColor = System.Drawing.Color.White;
            this.skinLabel_tip.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel_tip.Location = new System.Drawing.Point(309, 307);
            this.skinLabel_tip.Name = "skinLabel_tip";
            this.skinLabel_tip.Size = new System.Drawing.Size(125, 17);
            this.skinLabel_tip.TabIndex = 132;
            this.skinLabel_tip.Text = "正在等待对方回应 . . .";
            // 
            // nDiskBrowser1
            // 
            this.nDiskBrowser1.AllowDrop = true;
            this.nDiskBrowser1.AllowUploadFolder = false;
            this.nDiskBrowser1.BackColor = System.Drawing.Color.Transparent;
            this.nDiskBrowser1.Connected = true;
            this.nDiskBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nDiskBrowser1.Location = new System.Drawing.Point(4, 28);
            this.nDiskBrowser1.LockRootDirectory = false;
            this.nDiskBrowser1.Name = "nDiskBrowser1";
            this.nDiskBrowser1.NetDiskID = "";
            this.nDiskBrowser1.Size = new System.Drawing.Size(747, 537);
            this.nDiskBrowser1.TabIndex = 133;
            // 
            // RemoteDiskForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Back = ((System.Drawing.Image)(resources.GetObject("$this.Back")));
            this.BorderPalace = ((System.Drawing.Image)(resources.GetObject("$this.BorderPalace")));
            this.ClientSize = new System.Drawing.Size(755, 569);
            this.Controls.Add(this.nDiskBrowser1);
            this.Controls.Add(this.skinLabel_tip);
            this.Controls.Add(this.skinPanel_pic);
            this.Controls.Add(this.skinLabel_msg);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RemoteDiskForm";
            this.Shadow = false;
            this.Text = "远程磁盘";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RemoteHelpForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

       
        private CCWin.SkinControl.SkinPanel skinPanel_pic;
        private CCWin.SkinControl.SkinLabel skinLabel_msg;
        private CCWin.SkinControl.SkinLabel skinLabel_tip;
        private NetworkDisk.Passive.NDiskBrowser nDiskBrowser1;

    }
}