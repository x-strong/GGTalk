﻿namespace OMCS.Boost.MultiChat
{
    partial class FullScreenVideoForm
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
            this.cameraPanel1 = new OMCS.Windows.CameraPanel();
            this.SuspendLayout();
            // 
            // cameraPanel1
            // 
            this.cameraPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cameraPanel1.Dock = System.Windows.Forms.DockStyle.Fill; 
            this.cameraPanel1.Location = new System.Drawing.Point(0, 0);
            this.cameraPanel1.Name = "cameraPanel1";
            this.cameraPanel1.Size = new System.Drawing.Size(335, 272);
            this.cameraPanel1.TabIndex = 0;
            this.cameraPanel1.Text = "cameraPanel1"; 
            // 
            // FullScreenVideoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 272);
            this.Controls.Add(this.cameraPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FullScreenVideoForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FullScreenVideoForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FullScreenVideoForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private Windows.CameraPanel cameraPanel1;
    }
}