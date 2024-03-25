namespace GGTalk
{
    partial class AtSelectUserFrom
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.skinPictureBox_close = new CCWin.SkinControl.SkinPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.skinPictureBox_close)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 28);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(177, 170);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // skinPictureBox_close
            // 
            this.skinPictureBox_close.BackColor = System.Drawing.Color.Transparent;
            this.skinPictureBox_close.Image = global::GGTalk.Properties.Resources.delete_btn_nor;
            this.skinPictureBox_close.Location = new System.Drawing.Point(147, 4);
            this.skinPictureBox_close.Name = "skinPictureBox_close";
            this.skinPictureBox_close.Size = new System.Drawing.Size(20, 20);
            this.skinPictureBox_close.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.skinPictureBox_close.TabIndex = 1;
            this.skinPictureBox_close.TabStop = false;
            this.skinPictureBox_close.Click += new System.EventHandler(this.skinPictureBox_close_Click);
            // 
            // AtSelectUserFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(177, 198);
            this.ControlBox = false;
            this.Controls.Add(this.skinPictureBox_close);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AtSelectUserFrom";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.skinPictureBox_close)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private CCWin.SkinControl.SkinPictureBox skinPictureBox_close;
    }
}