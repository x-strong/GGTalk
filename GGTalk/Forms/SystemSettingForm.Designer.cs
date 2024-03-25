﻿namespace GGTalk
{
    partial class SystemSettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemSettingForm));
            this.btnClose = new CCWin.SkinControl.SkinButton();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.skinRadioButton_hide = new CCWin.SkinControl.SkinRadioButton();
            this.skinRadioButton2 = new CCWin.SkinControl.SkinRadioButton();
            this.skinLabel3 = new CCWin.SkinControl.SkinLabel();
            this.skinComboBox_camera = new CCWin.SkinControl.SkinComboBox();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.skinComboBox_mic = new CCWin.SkinControl.SkinComboBox();
            this.skinCheckBox_autoRun = new CCWin.SkinControl.SkinCheckBox();
            this.skinCheckBox_autoLogin = new CCWin.SkinControl.SkinCheckBox();
            this.skinCheckBox_ring = new CCWin.SkinControl.SkinCheckBox();
            this.skinLabel4 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel5 = new CCWin.SkinControl.SkinLabel();
            this.skinCheckBox_lastWords = new CCWin.SkinControl.SkinCheckBox();
            this.skinLabel6 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel7 = new CCWin.SkinControl.SkinLabel();
            this.skinComboBox_speaker = new CCWin.SkinControl.SkinComboBox();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.btnClose.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.DownBack = global::GGTalk.Properties.Resources.button_frame;
            this.btnClose.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.btnClose.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.Location = new System.Drawing.Point(353, 252);
            this.btnClose.MouseBack = global::GGTalk.Properties.Resources.button_frame_pre;
            this.btnClose.Name = "btnClose";
            this.btnClose.NormlBack = global::GGTalk.Properties.Resources.button_frame;
            this.btnClose.Palace = true;
            this.btnClose.Size = new System.Drawing.Size(62, 28);
            this.btnClose.TabIndex = 133;
            this.btnClose.Text = "确定";
            this.btnClose.UseHandCursor = false;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // skinLabel2
            // 
            this.skinLabel2.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinLabel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel2.Location = new System.Drawing.Point(12, 51);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(92, 17);
            this.skinLabel2.TabIndex = 135;
            this.skinLabel2.Text = "关闭主面板时：";
            // 
            // skinRadioButton_hide
            // 
            this.skinRadioButton_hide.AutoSize = true;
            this.skinRadioButton_hide.BackColor = System.Drawing.Color.Transparent;
            this.skinRadioButton_hide.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinRadioButton_hide.DownBack = null;
            this.skinRadioButton_hide.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinRadioButton_hide.Location = new System.Drawing.Point(116, 49);
            this.skinRadioButton_hide.MouseBack = null;
            this.skinRadioButton_hide.Name = "skinRadioButton_hide";
            this.skinRadioButton_hide.NormlBack = null;
            this.skinRadioButton_hide.SelectedDownBack = null;
            this.skinRadioButton_hide.SelectedMouseBack = null;
            this.skinRadioButton_hide.SelectedNormlBack = null;
            this.skinRadioButton_hide.Size = new System.Drawing.Size(146, 21);
            this.skinRadioButton_hide.TabIndex = 136;
            this.skinRadioButton_hide.Text = "隐藏到任务栏通知区域";
            this.skinRadioButton_hide.UseVisualStyleBackColor = false;
            // 
            // skinRadioButton2
            // 
            this.skinRadioButton2.AutoSize = true;
            this.skinRadioButton2.BackColor = System.Drawing.Color.Transparent;
            this.skinRadioButton2.Checked = true;
            this.skinRadioButton2.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinRadioButton2.DownBack = null;
            this.skinRadioButton2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinRadioButton2.Location = new System.Drawing.Point(303, 49);
            this.skinRadioButton2.MouseBack = null;
            this.skinRadioButton2.Name = "skinRadioButton2";
            this.skinRadioButton2.NormlBack = null;
            this.skinRadioButton2.SelectedDownBack = null;
            this.skinRadioButton2.SelectedMouseBack = null;
            this.skinRadioButton2.SelectedNormlBack = null;
            this.skinRadioButton2.Size = new System.Drawing.Size(74, 21);
            this.skinRadioButton2.TabIndex = 136;
            this.skinRadioButton2.TabStop = true;
            this.skinRadioButton2.Text = "退出程序";
            this.skinRadioButton2.UseVisualStyleBackColor = false;
            // 
            // skinLabel3
            // 
            this.skinLabel3.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel3.AutoSize = true;
            this.skinLabel3.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel3.BorderColor = System.Drawing.Color.White;
            this.skinLabel3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel3.Location = new System.Drawing.Point(25, 113);
            this.skinLabel3.Name = "skinLabel3";
            this.skinLabel3.Size = new System.Drawing.Size(80, 17);
            this.skinLabel3.TabIndex = 135;
            this.skinLabel3.Text = "麦克风选择：";
            // 
            // skinComboBox_camera
            // 
            this.skinComboBox_camera.ArrowColor = System.Drawing.Color.Black;
            this.skinComboBox_camera.BaseColor = System.Drawing.Color.White;
            this.skinComboBox_camera.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.skinComboBox_camera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.skinComboBox_camera.FormattingEnabled = true;
            this.skinComboBox_camera.Location = new System.Drawing.Point(116, 82);
            this.skinComboBox_camera.Name = "skinComboBox_camera";
            this.skinComboBox_camera.Size = new System.Drawing.Size(299, 22);
            this.skinComboBox_camera.TabIndex = 137;
            this.skinComboBox_camera.WaterText = "";
            // 
            // skinLabel1
            // 
            this.skinLabel1.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel1.Location = new System.Drawing.Point(25, 82);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(80, 17);
            this.skinLabel1.TabIndex = 135;
            this.skinLabel1.Text = "摄像头选择：";
            // 
            // skinComboBox_mic
            // 
            this.skinComboBox_mic.ArrowColor = System.Drawing.Color.Black;
            this.skinComboBox_mic.BaseColor = System.Drawing.Color.White;
            this.skinComboBox_mic.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.skinComboBox_mic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.skinComboBox_mic.FormattingEnabled = true;
            this.skinComboBox_mic.Location = new System.Drawing.Point(116, 113);
            this.skinComboBox_mic.Name = "skinComboBox_mic";
            this.skinComboBox_mic.Size = new System.Drawing.Size(299, 22);
            this.skinComboBox_mic.TabIndex = 137;
            this.skinComboBox_mic.WaterText = "";
            // 
            // skinCheckBox_autoRun
            // 
            this.skinCheckBox_autoRun.AutoSize = true;
            this.skinCheckBox_autoRun.BackColor = System.Drawing.Color.Transparent;
            this.skinCheckBox_autoRun.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinCheckBox_autoRun.DownBack = null;
            this.skinCheckBox_autoRun.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinCheckBox_autoRun.Location = new System.Drawing.Point(116, 224);
            this.skinCheckBox_autoRun.MouseBack = null;
            this.skinCheckBox_autoRun.Name = "skinCheckBox_autoRun";
            this.skinCheckBox_autoRun.NormlBack = null;
            this.skinCheckBox_autoRun.SelectedDownBack = null;
            this.skinCheckBox_autoRun.SelectedMouseBack = null;
            this.skinCheckBox_autoRun.SelectedNormlBack = null;
            this.skinCheckBox_autoRun.Size = new System.Drawing.Size(111, 21);
            this.skinCheckBox_autoRun.TabIndex = 138;
            this.skinCheckBox_autoRun.Text = "开机时自动启动";
            this.skinCheckBox_autoRun.UseVisualStyleBackColor = false;
            // 
            // skinCheckBox_autoLogin
            // 
            this.skinCheckBox_autoLogin.AutoSize = true;
            this.skinCheckBox_autoLogin.BackColor = System.Drawing.Color.Transparent;
            this.skinCheckBox_autoLogin.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinCheckBox_autoLogin.DownBack = null;
            this.skinCheckBox_autoLogin.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinCheckBox_autoLogin.Location = new System.Drawing.Point(240, 224);
            this.skinCheckBox_autoLogin.MouseBack = null;
            this.skinCheckBox_autoLogin.Name = "skinCheckBox_autoLogin";
            this.skinCheckBox_autoLogin.NormlBack = null;
            this.skinCheckBox_autoLogin.SelectedDownBack = null;
            this.skinCheckBox_autoLogin.SelectedMouseBack = null;
            this.skinCheckBox_autoLogin.SelectedNormlBack = null;
            this.skinCheckBox_autoLogin.Size = new System.Drawing.Size(75, 21);
            this.skinCheckBox_autoLogin.TabIndex = 139;
            this.skinCheckBox_autoLogin.Text = "自动登录";
            this.skinCheckBox_autoLogin.UseVisualStyleBackColor = false;
            // 
            // skinCheckBox_ring
            // 
            this.skinCheckBox_ring.AutoSize = true;
            this.skinCheckBox_ring.BackColor = System.Drawing.Color.Transparent;
            this.skinCheckBox_ring.Checked = true;
            this.skinCheckBox_ring.CheckState = System.Windows.Forms.CheckState.Checked;
            this.skinCheckBox_ring.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinCheckBox_ring.DownBack = null;
            this.skinCheckBox_ring.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinCheckBox_ring.Location = new System.Drawing.Point(116, 197);
            this.skinCheckBox_ring.MouseBack = null;
            this.skinCheckBox_ring.Name = "skinCheckBox_ring";
            this.skinCheckBox_ring.NormlBack = null;
            this.skinCheckBox_ring.SelectedDownBack = null;
            this.skinCheckBox_ring.SelectedMouseBack = null;
            this.skinCheckBox_ring.SelectedNormlBack = null;
            this.skinCheckBox_ring.Size = new System.Drawing.Size(195, 21);
            this.skinCheckBox_ring.TabIndex = 138;
            this.skinCheckBox_ring.Text = "当收到信息时，播放信息提示音";
            this.skinCheckBox_ring.UseVisualStyleBackColor = false;
            // 
            // skinLabel4
            // 
            this.skinLabel4.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel4.AutoSize = true;
            this.skinLabel4.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel4.BorderColor = System.Drawing.Color.White;
            this.skinLabel4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel4.Location = new System.Drawing.Point(25, 198);
            this.skinLabel4.Name = "skinLabel4";
            this.skinLabel4.Size = new System.Drawing.Size(80, 17);
            this.skinLabel4.TabIndex = 135;
            this.skinLabel4.Text = "信息提示音：";
            // 
            // skinLabel5
            // 
            this.skinLabel5.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel5.AutoSize = true;
            this.skinLabel5.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel5.BorderColor = System.Drawing.Color.White;
            this.skinLabel5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel5.Location = new System.Drawing.Point(38, 225);
            this.skinLabel5.Name = "skinLabel5";
            this.skinLabel5.Size = new System.Drawing.Size(68, 17);
            this.skinLabel5.TabIndex = 135;
            this.skinLabel5.Text = "自动启动：";
            // 
            // skinCheckBox_lastWords
            // 
            this.skinCheckBox_lastWords.AutoSize = true;
            this.skinCheckBox_lastWords.BackColor = System.Drawing.Color.Transparent;
            this.skinCheckBox_lastWords.Checked = true;
            this.skinCheckBox_lastWords.CheckState = System.Windows.Forms.CheckState.Checked;
            this.skinCheckBox_lastWords.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinCheckBox_lastWords.DownBack = null;
            this.skinCheckBox_lastWords.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinCheckBox_lastWords.Location = new System.Drawing.Point(116, 170);
            this.skinCheckBox_lastWords.MouseBack = null;
            this.skinCheckBox_lastWords.Name = "skinCheckBox_lastWords";
            this.skinCheckBox_lastWords.NormlBack = null;
            this.skinCheckBox_lastWords.SelectedDownBack = null;
            this.skinCheckBox_lastWords.SelectedMouseBack = null;
            this.skinCheckBox_lastWords.SelectedNormlBack = null;
            this.skinCheckBox_lastWords.Size = new System.Drawing.Size(171, 21);
            this.skinCheckBox_lastWords.TabIndex = 138;
            this.skinCheckBox_lastWords.Text = "显示上次交谈的最后一句话";
            this.skinCheckBox_lastWords.UseVisualStyleBackColor = false;
            // 
            // skinLabel6
            // 
            this.skinLabel6.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel6.AutoSize = true;
            this.skinLabel6.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel6.BorderColor = System.Drawing.Color.White;
            this.skinLabel6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel6.Location = new System.Drawing.Point(25, 172);
            this.skinLabel6.Name = "skinLabel6";
            this.skinLabel6.Size = new System.Drawing.Size(80, 17);
            this.skinLabel6.TabIndex = 135;
            this.skinLabel6.Text = "打开聊天窗：";
            // 
            // skinLabel7
            // 
            this.skinLabel7.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel7.AutoSize = true;
            this.skinLabel7.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel7.BorderColor = System.Drawing.Color.White;
            this.skinLabel7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel7.Location = new System.Drawing.Point(25, 141);
            this.skinLabel7.Name = "skinLabel7";
            this.skinLabel7.Size = new System.Drawing.Size(80, 17);
            this.skinLabel7.TabIndex = 135;
            this.skinLabel7.Text = "扬声器选择：";
            // 
            // skinComboBox_speaker
            // 
            this.skinComboBox_speaker.ArrowColor = System.Drawing.Color.Black;
            this.skinComboBox_speaker.BaseColor = System.Drawing.Color.White;
            this.skinComboBox_speaker.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.skinComboBox_speaker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.skinComboBox_speaker.FormattingEnabled = true;
            this.skinComboBox_speaker.Location = new System.Drawing.Point(116, 141);
            this.skinComboBox_speaker.Name = "skinComboBox_speaker";
            this.skinComboBox_speaker.Size = new System.Drawing.Size(299, 22);
            this.skinComboBox_speaker.TabIndex = 137;
            this.skinComboBox_speaker.WaterText = "";
            // 
            // SystemSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Back = ((System.Drawing.Image)(resources.GetObject("$this.Back")));
            this.BorderPalace = ((System.Drawing.Image)(resources.GetObject("$this.BorderPalace")));
            this.ClientSize = new System.Drawing.Size(435, 297);
            this.CloseDownBack = global::GGTalk.Properties.Resources.btn_close_down;
            this.CloseMouseBack = global::GGTalk.Properties.Resources.btn_close_highlight;
            this.CloseNormlBack = global::GGTalk.Properties.Resources.btn_close_disable;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.skinCheckBox_autoLogin);
            this.Controls.Add(this.skinCheckBox_lastWords);
            this.Controls.Add(this.skinCheckBox_ring);
            this.Controls.Add(this.skinCheckBox_autoRun);
            this.Controls.Add(this.skinComboBox_speaker);
            this.Controls.Add(this.skinComboBox_mic);
            this.Controls.Add(this.skinComboBox_camera);
            this.Controls.Add(this.skinRadioButton2);
            this.Controls.Add(this.skinRadioButton_hide);
            this.Controls.Add(this.skinLabel1);
            this.Controls.Add(this.skinLabel5);
            this.Controls.Add(this.skinLabel6);
            this.Controls.Add(this.skinLabel7);
            this.Controls.Add(this.skinLabel4);
            this.Controls.Add(this.skinLabel3);
            this.Controls.Add(this.skinLabel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaxDownBack = global::GGTalk.Properties.Resources.btn_max_down;
            this.MaxMouseBack = global::GGTalk.Properties.Resources.btn_max_highlight;
            this.MaxNormlBack = global::GGTalk.Properties.Resources.btn_max_normal;
            this.MiniDownBack = global::GGTalk.Properties.Resources.btn_mini_down;
            this.MiniMouseBack = global::GGTalk.Properties.Resources.btn_mini_highlight;
            this.MiniNormlBack = global::GGTalk.Properties.Resources.btn_mini_normal;
            this.Name = "SystemSettingForm";
            this.RestoreDownBack = global::GGTalk.Properties.Resources.btn_restore_down;
            this.RestoreMouseBack = global::GGTalk.Properties.Resources.btn_restore_highlight;
            this.RestoreNormlBack = global::GGTalk.Properties.Resources.btn_restore_normal;
            this.Text = "系统设置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinButton btnClose;
        private CCWin.SkinControl.SkinLabel skinLabel2;
        private CCWin.SkinControl.SkinRadioButton skinRadioButton_hide;
        private CCWin.SkinControl.SkinRadioButton skinRadioButton2;
        private CCWin.SkinControl.SkinLabel skinLabel3;
        private CCWin.SkinControl.SkinComboBox skinComboBox_camera;
        private CCWin.SkinControl.SkinLabel skinLabel1;
        private CCWin.SkinControl.SkinComboBox skinComboBox_mic;
        private CCWin.SkinControl.SkinCheckBox skinCheckBox_autoRun;
        private CCWin.SkinControl.SkinCheckBox skinCheckBox_autoLogin;
        private CCWin.SkinControl.SkinCheckBox skinCheckBox_ring;
        private CCWin.SkinControl.SkinLabel skinLabel4;
        private CCWin.SkinControl.SkinLabel skinLabel5;
        private CCWin.SkinControl.SkinCheckBox skinCheckBox_lastWords;
        private CCWin.SkinControl.SkinLabel skinLabel6;
        private CCWin.SkinControl.SkinLabel skinLabel7;
        private CCWin.SkinControl.SkinComboBox skinComboBox_speaker;
    }
}