﻿namespace GGTalk
{
    partial class EditGroupInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditGroupInfoForm));
            this.btnClose = new CCWin.SkinControl.SkinButton();
            this.skinButton1 = new CCWin.SkinControl.SkinButton();
            this.skinTextBox_name = new CCWin.SkinControl.SkinTextBox();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel3 = new CCWin.SkinControl.SkinLabel();
            this.skinTextBox_announce = new CCWin.SkinControl.SkinTextBox();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.linkLabel_groupID = new System.Windows.Forms.LinkLabel();
            this.skinTextBox_name.SuspendLayout();
            this.skinTextBox_announce.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.btnClose.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.DownBack = ((System.Drawing.Image)(resources.GetObject("btnClose.DownBack")));
            this.btnClose.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.btnClose.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.Location = new System.Drawing.Point(439, 165);
            this.btnClose.MouseBack = ((System.Drawing.Image)(resources.GetObject("btnClose.MouseBack")));
            this.btnClose.Name = "btnClose";
            this.btnClose.NormlBack = ((System.Drawing.Image)(resources.GetObject("btnClose.NormlBack")));
            this.btnClose.Palace = true;
            this.btnClose.Size = new System.Drawing.Size(62, 27);
            this.btnClose.TabIndex = 133;
            this.btnClose.Text = "确定";
            this.btnClose.UseHandCursor = false;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // skinButton1
            // 
            this.skinButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.skinButton1.BackColor = System.Drawing.Color.Transparent;
            this.skinButton1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.skinButton1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.skinButton1.DownBack = ((System.Drawing.Image)(resources.GetObject("skinButton1.DownBack")));
            this.skinButton1.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.skinButton1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButton1.Location = new System.Drawing.Point(364, 165);
            this.skinButton1.MouseBack = ((System.Drawing.Image)(resources.GetObject("skinButton1.MouseBack")));
            this.skinButton1.Name = "skinButton1";
            this.skinButton1.NormlBack = ((System.Drawing.Image)(resources.GetObject("skinButton1.NormlBack")));
            this.skinButton1.Palace = true;
            this.skinButton1.Size = new System.Drawing.Size(62, 27);
            this.skinButton1.TabIndex = 133;
            this.skinButton1.Text = "取消";
            this.skinButton1.UseHandCursor = false;
            this.skinButton1.UseVisualStyleBackColor = false;
            this.skinButton1.Click += new System.EventHandler(this.skinButton1_Click);
            // 
            // skinTextBox_name
            // 
            this.skinTextBox_name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skinTextBox_name.BackColor = System.Drawing.Color.Transparent;
            this.skinTextBox_name.CloseButtonVisiable = false;
            this.skinTextBox_name.Icon = null;
            this.skinTextBox_name.IconIsButton = false;
            this.skinTextBox_name.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.skinTextBox_name.Location = new System.Drawing.Point(97, 75);
            this.skinTextBox_name.Margin = new System.Windows.Forms.Padding(0);
            this.skinTextBox_name.MinimumSize = new System.Drawing.Size(28, 28);
            this.skinTextBox_name.MouseBack = null;
            this.skinTextBox_name.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.skinTextBox_name.Name = "skinTextBox_name";
            this.skinTextBox_name.NormlBack = null;
            this.skinTextBox_name.Padding = new System.Windows.Forms.Padding(5);
            this.skinTextBox_name.SearchButtonVisiable = false;
            this.skinTextBox_name.Size = new System.Drawing.Size(414, 28);
            // 
            // skinTextBox_name.BaseText
            // 
            this.skinTextBox_name.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skinTextBox_name.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTextBox_name.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.skinTextBox_name.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.skinTextBox_name.SkinTxt.Name = "BaseText";
            this.skinTextBox_name.SkinTxt.Size = new System.Drawing.Size(404, 18);
            this.skinTextBox_name.SkinTxt.TabIndex = 0;
            this.skinTextBox_name.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinTextBox_name.SkinTxt.WaterText = "";
            this.skinTextBox_name.TabIndex = 135;
            // 
            // skinLabel2
            // 
            this.skinLabel2.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinLabel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel2.Location = new System.Drawing.Point(14, 80);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(80, 17);
            this.skinLabel2.TabIndex = 134;
            this.skinLabel2.Text = "讨论组名称：";
            // 
            // skinLabel3
            // 
            this.skinLabel3.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel3.AutoSize = true;
            this.skinLabel3.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel3.BorderColor = System.Drawing.Color.White;
            this.skinLabel3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel3.Location = new System.Drawing.Point(14, 119);
            this.skinLabel3.Name = "skinLabel3";
            this.skinLabel3.Size = new System.Drawing.Size(80, 17);
            this.skinLabel3.TabIndex = 134;
            this.skinLabel3.Text = "讨论组公告：";
            // 
            // skinTextBox_announce
            // 
            this.skinTextBox_announce.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skinTextBox_announce.BackColor = System.Drawing.Color.Transparent;
            this.skinTextBox_announce.CloseButtonVisiable = false;
            this.skinTextBox_announce.Icon = null;
            this.skinTextBox_announce.IconIsButton = false;
            this.skinTextBox_announce.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.skinTextBox_announce.Location = new System.Drawing.Point(97, 114);
            this.skinTextBox_announce.Margin = new System.Windows.Forms.Padding(0);
            this.skinTextBox_announce.MinimumSize = new System.Drawing.Size(28, 28);
            this.skinTextBox_announce.MouseBack = null;
            this.skinTextBox_announce.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.skinTextBox_announce.Name = "skinTextBox_announce";
            this.skinTextBox_announce.NormlBack = null;
            this.skinTextBox_announce.Padding = new System.Windows.Forms.Padding(5);
            this.skinTextBox_announce.SearchButtonVisiable = false;
            this.skinTextBox_announce.Size = new System.Drawing.Size(414, 28);
            // 
            // skinTextBox_announce.BaseText
            // 
            this.skinTextBox_announce.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skinTextBox_announce.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTextBox_announce.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.skinTextBox_announce.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.skinTextBox_announce.SkinTxt.Name = "BaseText";
            this.skinTextBox_announce.SkinTxt.Size = new System.Drawing.Size(404, 18);
            this.skinTextBox_announce.SkinTxt.TabIndex = 0;
            this.skinTextBox_announce.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinTextBox_announce.SkinTxt.WaterText = "";
            this.skinTextBox_announce.TabIndex = 135;
            // 
            // skinLabel1
            // 
            this.skinLabel1.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel1.Location = new System.Drawing.Point(14, 44);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(80, 17);
            this.skinLabel1.TabIndex = 134;
            this.skinLabel1.Text = "讨论组编号：";
            // 
            // linkLabel_groupID
            // 
            this.linkLabel_groupID.AutoSize = true;
            this.linkLabel_groupID.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel_groupID.Location = new System.Drawing.Point(95, 49);
            this.linkLabel_groupID.Name = "linkLabel_groupID";
            this.linkLabel_groupID.Size = new System.Drawing.Size(29, 12);
            this.linkLabel_groupID.TabIndex = 136;
            this.linkLabel_groupID.TabStop = true;
            this.linkLabel_groupID.Text = "*101";
            this.linkLabel_groupID.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_groupID_LinkClicked);
            // 
            // EditGroupInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Back = ((System.Drawing.Image)(resources.GetObject("$this.Back")));
            this.BorderPalace = ((System.Drawing.Image)(resources.GetObject("$this.BorderPalace")));
            this.ClientSize = new System.Drawing.Size(523, 206);
            this.CloseDownBack = global::GGTalk.Properties.Resources.btn_close_down;
            this.CloseMouseBack = global::GGTalk.Properties.Resources.btn_close_highlight;
            this.CloseNormlBack = global::GGTalk.Properties.Resources.btn_close_disable;
            this.Controls.Add(this.linkLabel_groupID);
            this.Controls.Add(this.skinTextBox_announce);
            this.Controls.Add(this.skinTextBox_name);
            this.Controls.Add(this.skinLabel3);
            this.Controls.Add(this.skinLabel1);
            this.Controls.Add(this.skinLabel2);
            this.Controls.Add(this.skinButton1);
            this.Controls.Add(this.btnClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaxDownBack = global::GGTalk.Properties.Resources.btn_max_down;
            this.MaxMouseBack = global::GGTalk.Properties.Resources.btn_max_highlight;
            this.MaxNormlBack = global::GGTalk.Properties.Resources.btn_max_normal;
            this.MiniDownBack = global::GGTalk.Properties.Resources.btn_mini_down;
            this.MiniMouseBack = global::GGTalk.Properties.Resources.btn_mini_highlight;
            this.MiniNormlBack = global::GGTalk.Properties.Resources.btn_mini_normal;
            this.Name = "EditGroupInfoForm";
            this.RestoreDownBack = global::GGTalk.Properties.Resources.btn_restore_down;
            this.RestoreMouseBack = global::GGTalk.Properties.Resources.btn_restore_highlight;
            this.RestoreNormlBack = global::GGTalk.Properties.Resources.btn_restore_normal;
            this.Text = "修改讨论组信息";
            this.UseCustomIcon = true;
            this.skinTextBox_name.ResumeLayout(false);
            this.skinTextBox_name.PerformLayout();
            this.skinTextBox_announce.ResumeLayout(false);
            this.skinTextBox_announce.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinButton btnClose;
        private CCWin.SkinControl.SkinButton skinButton1;
        private CCWin.SkinControl.SkinTextBox skinTextBox_name;
        private CCWin.SkinControl.SkinLabel skinLabel2;
        private CCWin.SkinControl.SkinLabel skinLabel3;
        private CCWin.SkinControl.SkinTextBox skinTextBox_announce;
        private CCWin.SkinControl.SkinLabel skinLabel1;
        private System.Windows.Forms.LinkLabel linkLabel_groupID;
    }
}