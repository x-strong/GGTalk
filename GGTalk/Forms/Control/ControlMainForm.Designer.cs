namespace GGTalk
{
    partial class ControlMainForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage__Register = new System.Windows.Forms.TabPage();
            this.updateUserStateBox1 = new GGTalk.Forms.Control.UpdateUserStateBox();
            this.addUserBox1 = new GGTalk.AddUserBox();
            this.autoIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.speakerIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.keywordsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contentImageDataGridViewImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.contentTextDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scoreDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.handledDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tabControl.SuspendLayout();
            this.tabPage__Register.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage__Register);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(4, 28);
            this.tabControl.Name = "tabControl";
            this.tabControl.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabControl.RightToLeftLayout = true;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1016, 618);
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl.TabIndex = 0;
            // 
            // tabPage__Register
            // 
            this.tabPage__Register.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tabPage__Register.Controls.Add(this.updateUserStateBox1);
            this.tabPage__Register.Controls.Add(this.addUserBox1);
            this.tabPage__Register.Location = new System.Drawing.Point(4, 22);
            this.tabPage__Register.Name = "tabPage__Register";
            this.tabPage__Register.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage__Register.Size = new System.Drawing.Size(1008, 592);
            this.tabPage__Register.TabIndex = 0;
            this.tabPage__Register.Text = "用户管理";
            // 
            // updateUserStateBox1
            // 
            this.updateUserStateBox1.BackColor = System.Drawing.Color.White;
            this.updateUserStateBox1.Location = new System.Drawing.Point(348, 6);
            this.updateUserStateBox1.Name = "updateUserStateBox1";
            this.updateUserStateBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.updateUserStateBox1.Size = new System.Drawing.Size(327, 257);
            this.updateUserStateBox1.TabIndex = 1;
            // 
            // addUserBox1
            // 
            this.addUserBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.addUserBox1.BackColor = System.Drawing.Color.White;
            this.addUserBox1.Location = new System.Drawing.Point(6, 6);
            this.addUserBox1.Name = "addUserBox1";
            this.addUserBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.addUserBox1.Size = new System.Drawing.Size(327, 257);
            this.addUserBox1.TabIndex = 0;
            // 
            // autoIDDataGridViewTextBoxColumn
            // 
            this.autoIDDataGridViewTextBoxColumn.DataPropertyName = "AutoID";
            this.autoIDDataGridViewTextBoxColumn.HeaderText = "编号";
            this.autoIDDataGridViewTextBoxColumn.Name = "autoIDDataGridViewTextBoxColumn";
            this.autoIDDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // speakerIDDataGridViewTextBoxColumn
            // 
            this.speakerIDDataGridViewTextBoxColumn.DataPropertyName = "SpeakerID";
            this.speakerIDDataGridViewTextBoxColumn.HeaderText = "发言人";
            this.speakerIDDataGridViewTextBoxColumn.Name = "speakerIDDataGridViewTextBoxColumn";
            this.speakerIDDataGridViewTextBoxColumn.Width = 111;
            // 
            // keywordsDataGridViewTextBoxColumn
            // 
            this.keywordsDataGridViewTextBoxColumn.DataPropertyName = "Keywords";
            this.keywordsDataGridViewTextBoxColumn.HeaderText = "关键词";
            this.keywordsDataGridViewTextBoxColumn.Name = "keywordsDataGridViewTextBoxColumn";
            this.keywordsDataGridViewTextBoxColumn.Width = 125;
            // 
            // contentImageDataGridViewImageColumn
            // 
            this.contentImageDataGridViewImageColumn.DataPropertyName = "ContentImage";
            this.contentImageDataGridViewImageColumn.FillWeight = 300F;
            this.contentImageDataGridViewImageColumn.HeaderText = "可疑图片";
            this.contentImageDataGridViewImageColumn.Name = "contentImageDataGridViewImageColumn";
            this.contentImageDataGridViewImageColumn.Width = 428;
            // 
            // contentTextDataGridViewTextBoxColumn
            // 
            this.contentTextDataGridViewTextBoxColumn.DataPropertyName = "ContentText";
            this.contentTextDataGridViewTextBoxColumn.FillWeight = 300F;
            this.contentTextDataGridViewTextBoxColumn.HeaderText = "可疑文本";
            this.contentTextDataGridViewTextBoxColumn.Name = "contentTextDataGridViewTextBoxColumn";
            this.contentTextDataGridViewTextBoxColumn.Width = 749;
            // 
            // createTimeDataGridViewTextBoxColumn
            // 
            this.createTimeDataGridViewTextBoxColumn.DataPropertyName = "CreateTime";
            this.createTimeDataGridViewTextBoxColumn.HeaderText = "发生时间";
            this.createTimeDataGridViewTextBoxColumn.Name = "createTimeDataGridViewTextBoxColumn";
            this.createTimeDataGridViewTextBoxColumn.Width = 999;
            // 
            // scoreDataGridViewTextBoxColumn
            // 
            this.scoreDataGridViewTextBoxColumn.DataPropertyName = "Score";
            this.scoreDataGridViewTextBoxColumn.HeaderText = "Score";
            this.scoreDataGridViewTextBoxColumn.Name = "scoreDataGridViewTextBoxColumn";
            this.scoreDataGridViewTextBoxColumn.Visible = false;
            // 
            // handledDataGridViewCheckBoxColumn
            // 
            this.handledDataGridViewCheckBoxColumn.DataPropertyName = "Handled";
            this.handledDataGridViewCheckBoxColumn.HeaderText = "已处理";
            this.handledDataGridViewCheckBoxColumn.Name = "handledDataGridViewCheckBoxColumn";
            this.handledDataGridViewCheckBoxColumn.Visible = false;
            // 
            // ControlMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1024, 650);
            this.Controls.Add(this.tabControl);
            this.Name = "ControlMainForm";
            this.Text = "控制台";
            this.TopMost = false;
            this.tabControl.ResumeLayout(false);
            this.tabPage__Register.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage__Register;
        private System.Windows.Forms.DataGridViewTextBoxColumn autoIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn speakerIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn keywordsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn contentImageDataGridViewImageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn contentTextDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scoreDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn handledDataGridViewCheckBoxColumn;
        private AddUserBox addUserBox1;
        private Forms.Control.UpdateUserStateBox updateUserStateBox1;
    }
}