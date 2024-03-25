namespace GGTalk
{
    partial class NDiskForm
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
            this.nDiskBrowser1 = new ESFramework.Boost.NetworkDisk.Passive.NDiskBrowser();
            this.SuspendLayout();
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
            this.nDiskBrowser1.Size = new System.Drawing.Size(792, 418);
            this.nDiskBrowser1.TabIndex = 0;
            // 
            // NDiskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.nDiskBrowser1);
            this.Name = "NDiskForm";
            this.Text = "我的网盘";
            this.ResumeLayout(false);

        }

        #endregion

        private ESFramework.Boost.NetworkDisk.Passive.NDiskBrowser nDiskBrowser1;
    }
}