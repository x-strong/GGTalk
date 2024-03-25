namespace GGTalk
{
    partial class GroupMemberPanel
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
            this.skinPictureBox1 = new CCWin.SkinControl.SkinPictureBox();
            this.skinLabel_displayName = new CCWin.SkinControl.SkinLabel();
            ((System.ComponentModel.ISupportInitialize)(this.skinPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // skinPictureBox1
            // 
            this.skinPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.skinPictureBox1.Location = new System.Drawing.Point(3, 2);
            this.skinPictureBox1.Name = "skinPictureBox1";
            this.skinPictureBox1.Size = new System.Drawing.Size(25, 25);
            this.skinPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.skinPictureBox1.TabIndex = 0;
            this.skinPictureBox1.TabStop = false;
            this.skinPictureBox1.Click += new System.EventHandler(this.GroupMemberPanel_Click);
            // 
            // skinLabel_displayName
            // 
            this.skinLabel_displayName.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel_displayName.AutoSize = true;
            this.skinLabel_displayName.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel_displayName.BorderColor = System.Drawing.Color.White;
            this.skinLabel_displayName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel_displayName.Location = new System.Drawing.Point(35, 6);
            this.skinLabel_displayName.Name = "skinLabel_displayName";
            this.skinLabel_displayName.Size = new System.Drawing.Size(32, 17);
            this.skinLabel_displayName.TabIndex = 1;
            this.skinLabel_displayName.Text = "名称";
            this.skinLabel_displayName.Click += new System.EventHandler(this.GroupMemberPanel_Click);
            // 
            // GroupMemberPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.skinLabel_displayName);
            this.Controls.Add(this.skinPictureBox1);
            this.Name = "GroupMemberPanel";
            this.Size = new System.Drawing.Size(150, 30);
            this.Click += new System.EventHandler(this.GroupMemberPanel_Click);
            ((System.ComponentModel.ISupportInitialize)(this.skinPictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinPictureBox skinPictureBox1;
        private CCWin.SkinControl.SkinLabel skinLabel_displayName;
    }
}
