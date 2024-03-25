namespace GGTalk
{
    partial class Snapchat4ShowForm
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.chatBox1 = new ESFramework.Boost.Controls.ChatBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("幼圆", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(333, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "10";
            this.label1.Visible = false;
            // 
            // chatBox1
            // 
            this.chatBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chatBox1.BackColor = System.Drawing.Color.White;
            this.chatBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chatBox1.CausesValidation = false;
            this.chatBox1.ContextMenuMode = ESFramework.Boost.Controls.ChatBoxContextMenuMode.ForOutput;
            this.chatBox1.Cursor = System.Windows.Forms.Cursors.No;
            this.chatBox1.DetectUrls = false;
            this.chatBox1.Location = new System.Drawing.Point(2, 31);
            this.chatBox1.Name = "chatBox1";
            this.chatBox1.PopoutImageWhenDoubleClick = true;
            this.chatBox1.ReadOnly = true;
            this.chatBox1.Size = new System.Drawing.Size(365, 133);
            this.chatBox1.TabIndex = 2;
            this.chatBox1.Text = "";
            this.chatBox1.UseRtf = false;
            // 
            // Snapchat4ShowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Red;
            this.BackShade = false;
            this.BackToColor = false;
            this.CanResize = true;
            this.CaptionFont = new System.Drawing.Font("微软雅黑", 9F);
            this.ClientSize = new System.Drawing.Size(368, 166);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chatBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Snapchat4ShowForm";
            this.ShadowColor = System.Drawing.Color.Transparent;
            this.ShowDrawIcon = false;
            this.ShowIcon = false;
            this.Special = false;
            this.Text = "消息将自动销毁";
            this.TitleColor = System.Drawing.Color.White;
            this.Shown += new System.EventHandler(this.Snapchat4ShowForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ESFramework.Boost.Controls.ChatBox chatBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
    }
}