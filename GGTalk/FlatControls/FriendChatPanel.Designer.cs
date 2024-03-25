
namespace GGTalk.Controls
{
    partial class FriendChatPanel
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
            this.chatPanel1 = new GGTalk.ChatPanelPlus();
            this.SuspendLayout();
            // 
            // chatPanel1
            // 
            this.chatPanel1.BackColor = System.Drawing.Color.White;
            this.chatPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatPanel1.Location = new System.Drawing.Point(0, 0);
            this.chatPanel1.Name = "chatPanel1";
            this.chatPanel1.Size = new System.Drawing.Size(633, 613);
            this.chatPanel1.TabIndex = 0;
            // 
            // FriendChatControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chatPanel1);
            this.Name = "FriendChatControl";
            this.Size = new System.Drawing.Size(633, 613);
            this.Load += new System.EventHandler(this.FriendChatControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ChatPanelPlus chatPanel1;
    }
}
