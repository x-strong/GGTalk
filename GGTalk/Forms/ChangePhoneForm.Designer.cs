namespace GGTalk
{
    partial class ChangePhoneForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangePhoneForm));
            this.skinButton_Cancel = new CCWin.SkinControl.SkinButton();
            this.btnOK = new CCWin.SkinControl.SkinButton();
            this.skinTextBox_phone = new CCWin.SkinControl.SkinTextBox();
            this.skinLabel6 = new CCWin.SkinControl.SkinLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.skinTextBox_phone.SuspendLayout();
            this.SuspendLayout();
            // 
            // skinButton_Cancel
            // 
            this.skinButton_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.skinButton_Cancel.BackColor = System.Drawing.Color.Transparent;
            this.skinButton_Cancel.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.skinButton_Cancel.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.skinButton_Cancel.DownBack = ((System.Drawing.Image)(resources.GetObject("skinButton_Cancel.DownBack")));
            this.skinButton_Cancel.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.skinButton_Cancel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButton_Cancel.Location = new System.Drawing.Point(338, 270);
            this.skinButton_Cancel.Margin = new System.Windows.Forms.Padding(6);
            this.skinButton_Cancel.MouseBack = ((System.Drawing.Image)(resources.GetObject("skinButton_Cancel.MouseBack")));
            this.skinButton_Cancel.Name = "skinButton_Cancel";
            this.skinButton_Cancel.NormlBack = ((System.Drawing.Image)(resources.GetObject("skinButton_Cancel.NormlBack")));
            this.skinButton_Cancel.Palace = true;
            this.skinButton_Cancel.Size = new System.Drawing.Size(124, 54);
            this.skinButton_Cancel.TabIndex = 149;
            this.skinButton_Cancel.Text = "取消";
            this.skinButton_Cancel.UseHandCursor = false;
            this.skinButton_Cancel.UseVisualStyleBackColor = false;
            this.skinButton_Cancel.Click += new System.EventHandler(this.skinButton_Cancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.btnOK.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.DownBack = ((System.Drawing.Image)(resources.GetObject("btnOK.DownBack")));
            this.btnOK.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.btnOK.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(488, 270);
            this.btnOK.Margin = new System.Windows.Forms.Padding(6);
            this.btnOK.MouseBack = ((System.Drawing.Image)(resources.GetObject("btnOK.MouseBack")));
            this.btnOK.Name = "btnOK";
            this.btnOK.NormlBack = ((System.Drawing.Image)(resources.GetObject("btnOK.NormlBack")));
            this.btnOK.Palace = true;
            this.btnOK.Size = new System.Drawing.Size(124, 54);
            this.btnOK.TabIndex = 148;
            this.btnOK.Text = "确定";
            this.btnOK.UseHandCursor = false;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // skinTextBox_phone
            // 
            this.skinTextBox_phone.BackColor = System.Drawing.Color.Transparent;
            this.skinTextBox_phone.CloseButtonVisiable = false;
            this.skinTextBox_phone.Icon = null;
            this.skinTextBox_phone.IconIsButton = false;
            this.skinTextBox_phone.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.skinTextBox_phone.Location = new System.Drawing.Point(172, 108);
            this.skinTextBox_phone.Margin = new System.Windows.Forms.Padding(0);
            this.skinTextBox_phone.MinimumSize = new System.Drawing.Size(56, 56);
            this.skinTextBox_phone.MouseBack = null;
            this.skinTextBox_phone.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.skinTextBox_phone.Name = "skinTextBox_phone";
            this.skinTextBox_phone.NormlBack = null;
            this.skinTextBox_phone.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.skinTextBox_phone.SearchButtonVisiable = false;
            this.skinTextBox_phone.Size = new System.Drawing.Size(304, 56);
            // 
            // skinTextBox_phone.BaseText
            // 
            this.skinTextBox_phone.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skinTextBox_phone.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTextBox_phone.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.skinTextBox_phone.SkinTxt.Location = new System.Drawing.Point(5, 10);
            this.skinTextBox_phone.SkinTxt.Name = "BaseText";
            this.skinTextBox_phone.SkinTxt.Size = new System.Drawing.Size(294, 35);
            this.skinTextBox_phone.SkinTxt.TabIndex = 1;
            this.skinTextBox_phone.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinTextBox_phone.SkinTxt.WaterText = "";
            this.skinTextBox_phone.TabIndex = 145;
            // 
            // skinLabel6
            // 
            this.skinLabel6.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel6.AutoSize = true;
            this.skinLabel6.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel6.BorderColor = System.Drawing.Color.White;
            this.skinLabel6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel6.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.skinLabel6.Location = new System.Drawing.Point(30, 120);
            this.skinLabel6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.skinLabel6.Name = "skinLabel6";
            this.skinLabel6.Size = new System.Drawing.Size(134, 31);
            this.skinLabel6.TabIndex = 144;
            this.skinLabel6.Text = "新手机号：";
            // 
            // ChangePhoneForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(658, 390);
            this.Controls.Add(this.skinButton_Cancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.skinTextBox_phone);
            this.Controls.Add(this.skinLabel6);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "ChangePhoneForm";
            this.Text = "更换手机号";
            this.skinTextBox_phone.ResumeLayout(false);
            this.skinTextBox_phone.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinButton skinButton_Cancel;
        private CCWin.SkinControl.SkinButton btnOK;
        private CCWin.SkinControl.SkinTextBox skinTextBox_phone;
        private CCWin.SkinControl.SkinLabel skinLabel6;
        private System.Windows.Forms.Timer timer1;
    }
}