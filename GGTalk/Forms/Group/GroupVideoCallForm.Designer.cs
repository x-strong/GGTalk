namespace GGTalk
{
    partial class GroupVideoCallForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupVideoCallForm));
            this.skinButtomReject = new CCWin.SkinControl.SkinButton();
            this.btnAccept = new CCWin.SkinControl.SkinButton();
            this.pnlImgTx = new CCWin.SkinControl.SkinPanel();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.SuspendLayout();
            // 
            // skinButtomReject
            // 
            this.skinButtomReject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.skinButtomReject.BackColor = System.Drawing.Color.Transparent;
            this.skinButtomReject.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("skinButtomReject.BackgroundImage")));
            this.skinButtomReject.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.skinButtomReject.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButtomReject.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.skinButtomReject.DownBack = ((System.Drawing.Image)(resources.GetObject("skinButtomReject.DownBack")));
            this.skinButtomReject.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.skinButtomReject.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButtomReject.Location = new System.Drawing.Point(147, 226);
            this.skinButtomReject.MouseBack = null;
            this.skinButtomReject.Name = "skinButtomReject";
            this.skinButtomReject.NormlBack = null;
            this.skinButtomReject.Size = new System.Drawing.Size(90, 28);
            this.skinButtomReject.TabIndex = 129;
            this.skinButtomReject.UseHandCursor = false;
            this.skinButtomReject.UseVisualStyleBackColor = false;
            this.skinButtomReject.Click += new System.EventHandler(this.skinButtomReject_Click);
            // 
            // btnAccept
            // 
            this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAccept.BackColor = System.Drawing.Color.Transparent;
            this.btnAccept.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAccept.BackgroundImage")));
            this.btnAccept.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAccept.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAccept.DownBack = ((System.Drawing.Image)(resources.GetObject("btnAccept.DownBack")));
            this.btnAccept.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.btnAccept.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAccept.Location = new System.Drawing.Point(30, 226);
            this.btnAccept.MouseBack = null;
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.NormlBack = null;
            this.btnAccept.Size = new System.Drawing.Size(90, 28);
            this.btnAccept.TabIndex = 130;
            this.btnAccept.UseHandCursor = false;
            this.btnAccept.UseVisualStyleBackColor = false;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // pnlImgTx
            // 
            this.pnlImgTx.BackColor = System.Drawing.Color.Transparent;
            this.pnlImgTx.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlImgTx.BackgroundImage")));
            this.pnlImgTx.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnlImgTx.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.pnlImgTx.DownBack = null;
            this.pnlImgTx.Location = new System.Drawing.Point(108, 28);
            this.pnlImgTx.Margin = new System.Windows.Forms.Padding(0);
            this.pnlImgTx.MouseBack = null;
            this.pnlImgTx.Name = "pnlImgTx";
            this.pnlImgTx.NormlBack = null;
            this.pnlImgTx.Radius = 4;
            this.pnlImgTx.Size = new System.Drawing.Size(50, 50);
            this.pnlImgTx.TabIndex = 131;
            // 
            // skinLabel1
            // 
            this.skinLabel1.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.skinLabel1.Location = new System.Drawing.Point(69, 85);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(128, 17);
            this.skinLabel1.TabIndex = 132;
            this.skinLabel1.Text = "邀请你进行群视频聊天";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(8, 128);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(256, 92);
            this.flowLayoutPanel1.TabIndex = 133;
            // 
            // skinLabel2
            // 
            this.skinLabel2.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinLabel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.skinLabel2.Location = new System.Drawing.Point(7, 108);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(92, 17);
            this.skinLabel2.TabIndex = 132;
            this.skinLabel2.Text = "参加聊天的还有";
            // 
            // GroupVideoCallForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(271, 264);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.skinLabel2);
            this.Controls.Add(this.skinLabel1);
            this.Controls.Add(this.pnlImgTx);
            this.Controls.Add(this.skinButtomReject);
            this.Controls.Add(this.btnAccept);
            this.Name = "GroupVideoCallForm";
            this.ShowDrawIcon = false;
            this.ShowIcon = false;
            this.Text = "群视频请求";
            this.Load += new System.EventHandler(this.GroupVideoCallForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinButton skinButtomReject;
        private CCWin.SkinControl.SkinButton btnAccept;
        private CCWin.SkinControl.SkinPanel pnlImgTx;
        private CCWin.SkinControl.SkinLabel skinLabel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private CCWin.SkinControl.SkinLabel skinLabel2;
    }
}