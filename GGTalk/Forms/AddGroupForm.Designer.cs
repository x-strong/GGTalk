namespace GGTalk
{
    partial class AddGroupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddGroupForm));
            this.label_name = new System.Windows.Forms.Label();
            this.linkLabel_id = new System.Windows.Forms.LinkLabel();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.skinButton1 = new CCWin.SkinControl.SkinButton();
            this.btnClose = new CCWin.SkinControl.SkinButton();
            this.skinTextBox_comment = new CCWin.SkinControl.SkinTextBox();
            this.skinLabel3 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.skinTextBox_comment.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.BackColor = System.Drawing.Color.White;
            this.label_name.Location = new System.Drawing.Point(166, 180);
            this.label_name.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(82, 24);
            this.label_name.TabIndex = 144;
            this.label_name.Text = "石斑鱼";
            // 
            // linkLabel_id
            // 
            this.linkLabel_id.AutoSize = true;
            this.linkLabel_id.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel_id.Location = new System.Drawing.Point(166, 118);
            this.linkLabel_id.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.linkLabel_id.Name = "linkLabel_id";
            this.linkLabel_id.Size = new System.Drawing.Size(130, 24);
            this.linkLabel_id.TabIndex = 143;
            this.linkLabel_id.TabStop = true;
            this.linkLabel_id.Text = "linkLabel1";
            this.linkLabel_id.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_id_LinkClicked);
            // 
            // skinLabel2
            // 
            this.skinLabel2.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinLabel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel2.Location = new System.Drawing.Point(60, 174);
            this.skinLabel2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(110, 31);
            this.skinLabel2.TabIndex = 142;
            this.skinLabel2.Text = "群名称：";
            // 
            // skinButton1
            // 
            this.skinButton1.BackColor = System.Drawing.Color.Transparent;
            this.skinButton1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.skinButton1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.skinButton1.DownBack = ((System.Drawing.Image)(resources.GetObject("skinButton1.DownBack")));
            this.skinButton1.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.skinButton1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButton1.Location = new System.Drawing.Point(298, 318);
            this.skinButton1.Margin = new System.Windows.Forms.Padding(6);
            this.skinButton1.MouseBack = ((System.Drawing.Image)(resources.GetObject("skinButton1.MouseBack")));
            this.skinButton1.Name = "skinButton1";
            this.skinButton1.NormlBack = ((System.Drawing.Image)(resources.GetObject("skinButton1.NormlBack")));
            this.skinButton1.Palace = true;
            this.skinButton1.Size = new System.Drawing.Size(124, 54);
            this.skinButton1.TabIndex = 140;
            this.skinButton1.Text = "取消";
            this.skinButton1.UseHandCursor = false;
            this.skinButton1.UseVisualStyleBackColor = false;
            this.skinButton1.Click += new System.EventHandler(this.skinButton1_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.btnClose.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.DownBack = ((System.Drawing.Image)(resources.GetObject("btnClose.DownBack")));
            this.btnClose.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.btnClose.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.Location = new System.Drawing.Point(434, 318);
            this.btnClose.Margin = new System.Windows.Forms.Padding(6);
            this.btnClose.MouseBack = ((System.Drawing.Image)(resources.GetObject("btnClose.MouseBack")));
            this.btnClose.Name = "btnClose";
            this.btnClose.NormlBack = ((System.Drawing.Image)(resources.GetObject("btnClose.NormlBack")));
            this.btnClose.Palace = true;
            this.btnClose.Size = new System.Drawing.Size(124, 54);
            this.btnClose.TabIndex = 141;
            this.btnClose.Text = "确定";
            this.btnClose.UseHandCursor = false;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // skinTextBox_comment
            // 
            this.skinTextBox_comment.BackColor = System.Drawing.Color.Transparent;
            this.skinTextBox_comment.CloseButtonVisiable = false;
            this.skinTextBox_comment.Icon = null;
            this.skinTextBox_comment.IconIsButton = false;
            this.skinTextBox_comment.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.skinTextBox_comment.Location = new System.Drawing.Point(170, 230);
            this.skinTextBox_comment.Margin = new System.Windows.Forms.Padding(0);
            this.skinTextBox_comment.MinimumSize = new System.Drawing.Size(60, 56);
            this.skinTextBox_comment.MouseBack = null;
            this.skinTextBox_comment.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.skinTextBox_comment.Name = "skinTextBox_comment";
            this.skinTextBox_comment.NormlBack = null;
            this.skinTextBox_comment.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.skinTextBox_comment.SearchButtonVisiable = false;
            this.skinTextBox_comment.Size = new System.Drawing.Size(412, 56);
            // 
            // skinTextBox_comment.BaseText
            // 
            this.skinTextBox_comment.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skinTextBox_comment.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTextBox_comment.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.skinTextBox_comment.SkinTxt.Location = new System.Drawing.Point(5, 10);
            this.skinTextBox_comment.SkinTxt.Name = "BaseText";
            this.skinTextBox_comment.SkinTxt.Size = new System.Drawing.Size(402, 35);
            this.skinTextBox_comment.SkinTxt.TabIndex = 0;
            this.skinTextBox_comment.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinTextBox_comment.SkinTxt.WaterText = "";
            this.skinTextBox_comment.TabIndex = 138;
            // 
            // skinLabel3
            // 
            this.skinLabel3.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel3.AutoSize = true;
            this.skinLabel3.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel3.BorderColor = System.Drawing.Color.White;
            this.skinLabel3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel3.Location = new System.Drawing.Point(84, 238);
            this.skinLabel3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.skinLabel3.Name = "skinLabel3";
            this.skinLabel3.Size = new System.Drawing.Size(86, 31);
            this.skinLabel3.TabIndex = 136;
            this.skinLabel3.Text = "备注：";
            // 
            // skinLabel1
            // 
            this.skinLabel1.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel1.Location = new System.Drawing.Point(60, 110);
            this.skinLabel1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(110, 31);
            this.skinLabel1.TabIndex = 137;
            this.skinLabel1.Text = "群帐号：";
            // 
            // AddGroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(626, 412);
            this.Controls.Add(this.label_name);
            this.Controls.Add(this.linkLabel_id);
            this.Controls.Add(this.skinLabel2);
            this.Controls.Add(this.skinButton1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.skinTextBox_comment);
            this.Controls.Add(this.skinLabel3);
            this.Controls.Add(this.skinLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "AddGroupForm";
            this.Text = "加入群组";
            this.TopMost = false;
            this.skinTextBox_comment.ResumeLayout(false);
            this.skinTextBox_comment.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinButton skinButton1;
        private CCWin.SkinControl.SkinButton btnClose;
        private CCWin.SkinControl.SkinTextBox skinTextBox_comment;
        private CCWin.SkinControl.SkinLabel skinLabel3;
        private CCWin.SkinControl.SkinLabel skinLabel1;
        private CCWin.SkinControl.SkinLabel skinLabel2;
        private System.Windows.Forms.LinkLabel linkLabel_id;
        private System.Windows.Forms.Label label_name;
    }
}