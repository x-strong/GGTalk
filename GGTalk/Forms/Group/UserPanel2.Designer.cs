namespace GGTalk
{
    partial class UserPanel2
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
            this.components = new System.ComponentModel.Container();
            this.skinLabel_name = new CCWin.SkinControl.SkinLabel();
            this.skinPictureBox1 = new CCWin.SkinControl.SkinPictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.skinPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // skinLabel_name
            // 
            this.skinLabel_name.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel_name.AutoEllipsis = true;
            this.skinLabel_name.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel_name.BorderColor = System.Drawing.Color.White;
            this.skinLabel_name.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel_name.Location = new System.Drawing.Point(3, 50);
            this.skinLabel_name.Name = "skinLabel_name";
            this.skinLabel_name.Size = new System.Drawing.Size(57, 17);
            this.skinLabel_name.TabIndex = 1;
            this.skinLabel_name.Text = "skinLwereretrytryr";
            this.skinLabel_name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // skinPictureBox1
            // 
            this.skinPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.skinPictureBox1.Location = new System.Drawing.Point(10, 3);
            this.skinPictureBox1.Name = "skinPictureBox1";
            this.skinPictureBox1.Size = new System.Drawing.Size(40, 40);
            this.skinPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.skinPictureBox1.TabIndex = 0;
            this.skinPictureBox1.TabStop = false;
            this.skinPictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.skinPictureBox1_MouseClick);
            this.skinPictureBox1.MouseEnter += new System.EventHandler(this.skinPictureBox1_MouseEnter);
            // 
            // UserPanel2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.skinLabel_name);
            this.Controls.Add(this.skinPictureBox1);
            this.Name = "UserPanel2";
            this.Size = new System.Drawing.Size(63, 70);
            ((System.ComponentModel.ISupportInitialize)(this.skinPictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinPictureBox skinPictureBox1;
        private CCWin.SkinControl.SkinLabel skinLabel_name;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
