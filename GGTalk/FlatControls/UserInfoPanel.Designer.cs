
namespace GGTalk.FlatControls
{
    partial class UserInfoPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserInfoPanel));
            this.skinLabel_signature = new CCWin.SkinControl.RtfRichTextBox();
            this.linkLabel_addComment = new System.Windows.Forms.LinkLabel();
            this.linkLabel_commentName = new System.Windows.Forms.LinkLabel();
            this.skinLabel_register = new CCWin.SkinControl.SkinLabel();
            this.linkLabel_ID = new CCWin.SkinControl.SkinLabel();
            this.skinLabel_Name = new CCWin.SkinControl.SkinLabel();
            this.skinLabel3 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel4 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel8 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel5 = new CCWin.SkinControl.SkinLabel();
            this.pnlImgTx = new CCWin.SkinControl.SkinPanel();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.skinButton_send = new CCWin.SkinControl.SkinButton();
            this.SuspendLayout();
            // 
            // skinLabel_signature
            // 
            this.skinLabel_signature.BackColor = System.Drawing.SystemColors.Control;
            this.skinLabel_signature.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skinLabel_signature.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel_signature.ForeColor = System.Drawing.SystemColors.GrayText;
            this.skinLabel_signature.HiglightColor = CCWin.SkinControl.RtfRichTextBox.RtfColor.White;
            this.skinLabel_signature.Location = new System.Drawing.Point(142, 118);
            this.skinLabel_signature.Name = "skinLabel_signature";
            this.skinLabel_signature.ReadOnly = true;
            this.skinLabel_signature.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.skinLabel_signature.Size = new System.Drawing.Size(269, 72);
            this.skinLabel_signature.TabIndex = 161;
            this.skinLabel_signature.Text = "你好啊utyu发单款数据个的分管领导开发工具的价格的发给打款了估计都";
            this.skinLabel_signature.TextColor = CCWin.SkinControl.RtfRichTextBox.RtfColor.Black;
            // 
            // linkLabel_addComment
            // 
            this.linkLabel_addComment.AutoSize = true;
            this.linkLabel_addComment.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel_addComment.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel_addComment.Location = new System.Drawing.Point(202, 258);
            this.linkLabel_addComment.Name = "linkLabel_addComment";
            this.linkLabel_addComment.Size = new System.Drawing.Size(93, 20);
            this.linkLabel_addComment.TabIndex = 159;
            this.linkLabel_addComment.TabStop = true;
            this.linkLabel_addComment.Text = "添加备注姓名";
            this.linkLabel_addComment.Visible = false;
            this.linkLabel_addComment.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_commentName_LinkClicked);
            // 
            // linkLabel_commentName
            // 
            this.linkLabel_commentName.AutoSize = true;
            this.linkLabel_commentName.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel_commentName.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel_commentName.Location = new System.Drawing.Point(202, 259);
            this.linkLabel_commentName.Name = "linkLabel_commentName";
            this.linkLabel_commentName.Size = new System.Drawing.Size(33, 20);
            this.linkLabel_commentName.TabIndex = 160;
            this.linkLabel_commentName.TabStop = true;
            this.linkLabel_commentName.Text = "123";
            this.linkLabel_commentName.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_commentName_LinkClicked);
            // 
            // skinLabel_register
            // 
            this.skinLabel_register.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel_register.AutoSize = true;
            this.skinLabel_register.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel_register.BorderColor = System.Drawing.Color.White;
            this.skinLabel_register.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel_register.ForeColor = System.Drawing.SystemColors.InfoText;
            this.skinLabel_register.Location = new System.Drawing.Point(202, 285);
            this.skinLabel_register.Name = "skinLabel_register";
            this.skinLabel_register.Size = new System.Drawing.Size(79, 20);
            this.skinLabel_register.TabIndex = 156;
            this.skinLabel_register.Text = "2016.06.15";
            // 
            // linkLabel_ID
            // 
            this.linkLabel_ID.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.linkLabel_ID.AutoSize = true;
            this.linkLabel_ID.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel_ID.BorderColor = System.Drawing.Color.White;
            this.linkLabel_ID.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel_ID.ForeColor = System.Drawing.SystemColors.InfoText;
            this.linkLabel_ID.Location = new System.Drawing.Point(201, 229);
            this.linkLabel_ID.Name = "linkLabel_ID";
            this.linkLabel_ID.Size = new System.Drawing.Size(49, 20);
            this.linkLabel_ID.TabIndex = 157;
            this.linkLabel_ID.Text = "10000";
            // 
            // skinLabel_Name
            // 
            this.skinLabel_Name.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel_Name.AutoSize = true;
            this.skinLabel_Name.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel_Name.BorderColor = System.Drawing.Color.White;
            this.skinLabel_Name.Font = new System.Drawing.Font("微软雅黑", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel_Name.ForeColor = System.Drawing.SystemColors.InfoText;
            this.skinLabel_Name.Location = new System.Drawing.Point(142, 87);
            this.skinLabel_Name.Name = "skinLabel_Name";
            this.skinLabel_Name.Size = new System.Drawing.Size(67, 25);
            this.skinLabel_Name.TabIndex = 158;
            this.skinLabel_Name.Text = "10000";
            // 
            // skinLabel3
            // 
            this.skinLabel3.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel3.AutoSize = true;
            this.skinLabel3.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel3.BorderColor = System.Drawing.Color.White;
            this.skinLabel3.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel3.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.skinLabel3.Location = new System.Drawing.Point(138, 285);
            this.skinLabel3.Name = "skinLabel3";
            this.skinLabel3.Size = new System.Drawing.Size(51, 20);
            this.skinLabel3.TabIndex = 151;
            this.skinLabel3.Text = "注册：";
            // 
            // skinLabel4
            // 
            this.skinLabel4.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel4.AutoSize = true;
            this.skinLabel4.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel4.BorderColor = System.Drawing.Color.White;
            this.skinLabel4.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel4.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.skinLabel4.Location = new System.Drawing.Point(138, 229);
            this.skinLabel4.Name = "skinLabel4";
            this.skinLabel4.Size = new System.Drawing.Size(51, 20);
            this.skinLabel4.TabIndex = 153;
            this.skinLabel4.Text = "帐号：";
            // 
            // skinLabel8
            // 
            this.skinLabel8.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel8.AutoSize = true;
            this.skinLabel8.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel8.BorderColor = System.Drawing.Color.White;
            this.skinLabel8.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel8.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.skinLabel8.Location = new System.Drawing.Point(138, 257);
            this.skinLabel8.Name = "skinLabel8";
            this.skinLabel8.Size = new System.Drawing.Size(51, 20);
            this.skinLabel8.TabIndex = 154;
            this.skinLabel8.Text = "备注：";
            // 
            // skinLabel5
            // 
            this.skinLabel5.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel5.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel5.BorderColor = System.Drawing.Color.White;
            this.skinLabel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.skinLabel5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel5.Location = new System.Drawing.Point(142, 198);
            this.skinLabel5.Name = "skinLabel5";
            this.skinLabel5.Size = new System.Drawing.Size(330, 2);
            this.skinLabel5.TabIndex = 162;
            // 
            // pnlImgTx
            // 
            this.pnlImgTx.BackColor = System.Drawing.Color.Transparent;
            this.pnlImgTx.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlImgTx.BackgroundImage")));
            this.pnlImgTx.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlImgTx.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.pnlImgTx.DownBack = null;
            this.pnlImgTx.Location = new System.Drawing.Point(420, 87);
            this.pnlImgTx.Margin = new System.Windows.Forms.Padding(0);
            this.pnlImgTx.MouseBack = null;
            this.pnlImgTx.Name = "pnlImgTx";
            this.pnlImgTx.NormlBack = null;
            this.pnlImgTx.Radius = 4;
            this.pnlImgTx.Size = new System.Drawing.Size(60, 60);
            this.pnlImgTx.TabIndex = 155;
            // 
            // skinLabel2
            // 
            this.skinLabel2.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinLabel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.skinLabel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel2.Location = new System.Drawing.Point(142, 357);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(330, 2);
            this.skinLabel2.TabIndex = 163;
            // 
            // skinButton_send
            // 
            this.skinButton_send.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.skinButton_send.BackColor = System.Drawing.Color.WhiteSmoke;
            this.skinButton_send.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.skinButton_send.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton_send.DownBack = global::GGTalk.Properties.Resources.button_frame;
            this.skinButton_send.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.skinButton_send.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButton_send.Location = new System.Drawing.Point(264, 395);
            this.skinButton_send.Margin = new System.Windows.Forms.Padding(0);
            this.skinButton_send.MouseBack = global::GGTalk.Properties.Resources.button_frame_pre;
            this.skinButton_send.Name = "skinButton_send";
            this.skinButton_send.NormlBack = global::GGTalk.Properties.Resources.button_frame;
            this.skinButton_send.Palace = true;
            this.skinButton_send.Radius = 4;
            this.skinButton_send.Size = new System.Drawing.Size(101, 35);
            this.skinButton_send.TabIndex = 164;
            this.skinButton_send.Text = "发消息";
            this.skinButton_send.UseHandCursor = false;
            this.skinButton_send.UseVisualStyleBackColor = false;
            this.skinButton_send.Click += new System.EventHandler(this.skinButton_send_Click);
            // 
            // UserInfoPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.skinButton_send);
            this.Controls.Add(this.skinLabel2);
            this.Controls.Add(this.skinLabel5);
            this.Controls.Add(this.pnlImgTx);
            this.Controls.Add(this.skinLabel_signature);
            this.Controls.Add(this.linkLabel_addComment);
            this.Controls.Add(this.linkLabel_commentName);
            this.Controls.Add(this.skinLabel_register);
            this.Controls.Add(this.linkLabel_ID);
            this.Controls.Add(this.skinLabel_Name);
            this.Controls.Add(this.skinLabel3);
            this.Controls.Add(this.skinLabel4);
            this.Controls.Add(this.skinLabel8);
            this.Name = "UserInfoPanel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinPanel pnlImgTx;
        private CCWin.SkinControl.RtfRichTextBox skinLabel_signature;
        private System.Windows.Forms.LinkLabel linkLabel_addComment;
        private System.Windows.Forms.LinkLabel linkLabel_commentName;
        private CCWin.SkinControl.SkinLabel skinLabel_register;
        private CCWin.SkinControl.SkinLabel linkLabel_ID;
        private CCWin.SkinControl.SkinLabel skinLabel_Name;
        private CCWin.SkinControl.SkinLabel skinLabel3;
        private CCWin.SkinControl.SkinLabel skinLabel4;
        private CCWin.SkinControl.SkinLabel skinLabel8;
        private CCWin.SkinControl.SkinLabel skinLabel5;
        private CCWin.SkinControl.SkinLabel skinLabel2;
        private CCWin.SkinControl.SkinButton skinButton_send;
    }
}
