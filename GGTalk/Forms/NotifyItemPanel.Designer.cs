namespace GGTalk.Forms
{
    partial class NotifyItemPanel
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label_time = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label_result = new System.Windows.Forms.Label();
            this.label_comment = new System.Windows.Forms.Label();
            this.label_UserName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_refuse = new CCWin.SkinControl.SkinButton();
            this.btn_agree = new CCWin.SkinControl.SkinButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label_time
            // 
            this.label_time.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_time.AutoSize = true;
            this.label_time.Location = new System.Drawing.Point(3, 8);
            this.label_time.Name = "label_time";
            this.label_time.Size = new System.Drawing.Size(47, 12);
            this.label_time.TabIndex = 0;
            this.label_time.Text = "8月15日";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label_comment);
            this.panel1.Controls.Add(this.label_UserName);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(435, 54);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_refuse);
            this.panel2.Controls.Add(this.btn_agree);
            this.panel2.Controls.Add(this.label_result);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(277, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(158, 54);
            this.panel2.TabIndex = 3;
            // 
            // label_result
            // 
            this.label_result.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label_result.AutoSize = true;
            this.label_result.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_result.Location = new System.Drawing.Point(100, 21);
            this.label_result.Name = "label_result";
            this.label_result.Size = new System.Drawing.Size(41, 12);
            this.label_result.TabIndex = 0;
            this.label_result.Text = "已同意";
            this.label_result.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_comment
            // 
            this.label_comment.AutoEllipsis = true;
            this.label_comment.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label_comment.Location = new System.Drawing.Point(54, 32);
            this.label_comment.Name = "label_comment";
            this.label_comment.Size = new System.Drawing.Size(221, 12);
            this.label_comment.TabIndex = 2;
            this.label_comment.Text = "备注特别长备注特别长备注特别长备注特别长备注特别长备注特别长";
            // 
            // label_UserName
            // 
            this.label_UserName.AutoSize = true;
            this.label_UserName.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_UserName.Location = new System.Drawing.Point(53, 10);
            this.label_UserName.Name = "label_UserName";
            this.label_UserName.Size = new System.Drawing.Size(35, 14);
            this.label_UserName.TabIndex = 1;
            this.label_UserName.Text = "姓名";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::GGTalk.Properties.Resources.app_icon_401;
            this.pictureBox1.Location = new System.Drawing.Point(5, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 40);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // btn_refuse
            // 
            this.btn_refuse.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btn_refuse.BackColor = System.Drawing.Color.Transparent;
            this.btn_refuse.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btn_refuse.DownBack = global::GGTalk.Properties.Resources.refuse_btn_pre;
            this.btn_refuse.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.btn_refuse.Location = new System.Drawing.Point(4, 16);
            this.btn_refuse.MouseBack = global::GGTalk.Properties.Resources.refuse_btn_pre;
            this.btn_refuse.Name = "btn_refuse";
            this.btn_refuse.NormlBack = global::GGTalk.Properties.Resources.refuse_btn_nor;
            this.btn_refuse.Size = new System.Drawing.Size(70, 24);
            this.btn_refuse.TabIndex = 2;
            this.btn_refuse.UseHandCursor = false;
            this.btn_refuse.UseVisualStyleBackColor = false;
            this.btn_refuse.Click += new System.EventHandler(this.btn_refuse_Click);
            // 
            // btn_agree
            // 
            this.btn_agree.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btn_agree.BackColor = System.Drawing.Color.Transparent;
            this.btn_agree.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btn_agree.DownBack = global::GGTalk.Properties.Resources.agree_btn_pre;
            this.btn_agree.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.btn_agree.Location = new System.Drawing.Point(80, 16);
            this.btn_agree.MouseBack = global::GGTalk.Properties.Resources.agree_btn_pre;
            this.btn_agree.Name = "btn_agree";
            this.btn_agree.NormlBack = global::GGTalk.Properties.Resources.agree_btn_nor;
            this.btn_agree.Size = new System.Drawing.Size(70, 24);
            this.btn_agree.TabIndex = 2;
            this.btn_agree.UseHandCursor = false;
            this.btn_agree.UseVisualStyleBackColor = false;
            this.btn_agree.Click += new System.EventHandler(this.btn_agree_Click);
            // 
            // NotifyItemPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label_time);
            this.Name = "NotifyItemPanel";
            this.Size = new System.Drawing.Size(435, 81);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_time;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label_result;
        private System.Windows.Forms.Label label_comment;
        private System.Windows.Forms.Label label_UserName;
        private CCWin.SkinControl.SkinButton btn_agree;
        private CCWin.SkinControl.SkinButton btn_refuse;
    }
}
