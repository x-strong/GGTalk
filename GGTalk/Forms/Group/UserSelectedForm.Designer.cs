namespace GGTalk
{
    partial class UserSelectedForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserSelectedForm));
            this.unitListBox1 = new TalkBase.Client.UnitViews.UnitListBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.skinTextBox1 = new CCWin.SkinControl.SkinTextBox();
            this.skinLabel_noResult = new CCWin.SkinControl.SkinLabel();
            this.skinButton_ok = new CCWin.SkinControl.SkinButton();
            this.skinButton1 = new CCWin.SkinControl.SkinButton();
            this.chatListBox_search = new CCWin.SkinControl.ChatListBox();
            this.skinTextBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // unitListBox1
            // 
            this.unitListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unitListBox1.AutoSize = true;
            this.unitListBox1.BackColor = System.Drawing.Color.White;
            this.unitListBox1.CatalogContextMenuVisiable = true;
            this.unitListBox1.DefaultFriendCatalogName = "我的好友";
            this.unitListBox1.DefaultGroupCatalogName = "我的群";
            this.unitListBox1.DrawContentType = CCWin.SkinControl.DrawContentType.PersonalMsg;
            this.unitListBox1.FriendsMobile = true;
            this.unitListBox1.IconSizeMode = CCWin.SkinControl.ChatListItemIcon.Small;
            this.unitListBox1.Location = new System.Drawing.Point(12, 65);
            this.unitListBox1.Name = "unitListBox1";
            this.unitListBox1.PreLoadCatalog = true;
            this.unitListBox1.Size = new System.Drawing.Size(288, 498);
            this.unitListBox1.TabIndex = 0;
            this.unitListBox1.UserContextMenuVisiable = false;
            this.unitListBox1.UnitClicked += new ESBasic.CbGeneric<TalkBase.IUnit>(this.unitListBox1_UnitClicked);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(306, 65);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(274, 498);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // skinLabel1
            // 
            this.skinLabel1.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel1.Location = new System.Drawing.Point(322, 41);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(51, 20);
            this.skinLabel1.TabIndex = 4;
            this.skinLabel1.Text = "已选择";
            // 
            // skinTextBox1
            // 
            this.skinTextBox1.BackColor = System.Drawing.Color.Transparent;
            this.skinTextBox1.CloseButtonVisiable = true;
            this.skinTextBox1.Icon = null;
            this.skinTextBox1.IconIsButton = false;
            this.skinTextBox1.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.skinTextBox1.Location = new System.Drawing.Point(12, 32);
            this.skinTextBox1.Margin = new System.Windows.Forms.Padding(0);
            this.skinTextBox1.MinimumSize = new System.Drawing.Size(28, 28);
            this.skinTextBox1.MouseBack = null;
            this.skinTextBox1.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.skinTextBox1.Name = "skinTextBox1";
            this.skinTextBox1.NormlBack = null;
            this.skinTextBox1.Padding = new System.Windows.Forms.Padding(25, 5, 25, 5);
            this.skinTextBox1.SearchButtonVisiable = true;
            this.skinTextBox1.Size = new System.Drawing.Size(291, 30);
            // 
            // skinTextBox1.BaseText
            // 
            this.skinTextBox1.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skinTextBox1.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTextBox1.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.skinTextBox1.SkinTxt.Location = new System.Drawing.Point(25, 5);
            this.skinTextBox1.SkinTxt.Name = "BaseText";
            this.skinTextBox1.SkinTxt.Size = new System.Drawing.Size(241, 18);
            this.skinTextBox1.SkinTxt.TabIndex = 0;
            this.skinTextBox1.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinTextBox1.SkinTxt.WaterText = "";
            this.skinTextBox1.TabIndex = 5;
            this.skinTextBox1.CloseButtonClicked += new System.EventHandler(this.skinTextBox1_CloseButtonClicked);
            this.skinTextBox1.EnterKeyInput += new System.EventHandler(this.skinTextBox1_EnterKeyInput);
            // 
            // skinLabel_noResult
            // 
            this.skinLabel_noResult.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.skinLabel_noResult.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel_noResult.AutoSize = true;
            this.skinLabel_noResult.BackColor = System.Drawing.Color.White;
            this.skinLabel_noResult.BorderColor = System.Drawing.Color.White;
            this.skinLabel_noResult.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel_noResult.Location = new System.Drawing.Point(110, 116);
            this.skinLabel_noResult.Name = "skinLabel_noResult";
            this.skinLabel_noResult.Size = new System.Drawing.Size(77, 17);
            this.skinLabel_noResult.TabIndex = 139;
            this.skinLabel_noResult.Text = "无搜索结果...";
            this.skinLabel_noResult.Visible = false;
            // 
            // skinButton_ok
            // 
            this.skinButton_ok.BackColor = System.Drawing.Color.Transparent;
            this.skinButton_ok.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.skinButton_ok.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton_ok.DownBack = ((System.Drawing.Image)(resources.GetObject("skinButton_ok.DownBack")));
            this.skinButton_ok.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.skinButton_ok.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButton_ok.Location = new System.Drawing.Point(515, 569);
            this.skinButton_ok.MouseBack = ((System.Drawing.Image)(resources.GetObject("skinButton_ok.MouseBack")));
            this.skinButton_ok.Name = "skinButton_ok";
            this.skinButton_ok.NormlBack = ((System.Drawing.Image)(resources.GetObject("skinButton_ok.NormlBack")));
            this.skinButton_ok.Palace = true;
            this.skinButton_ok.Size = new System.Drawing.Size(62, 27);
            this.skinButton_ok.TabIndex = 145;
            this.skinButton_ok.Text = "确定";
            this.skinButton_ok.UseHandCursor = false;
            this.skinButton_ok.UseVisualStyleBackColor = false;
            this.skinButton_ok.Click += new System.EventHandler(this.skinButton_ok_Click);
            // 
            // skinButton1
            // 
            this.skinButton1.BackColor = System.Drawing.Color.Transparent;
            this.skinButton1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.skinButton1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton1.DownBack = ((System.Drawing.Image)(resources.GetObject("skinButton1.DownBack")));
            this.skinButton1.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.skinButton1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButton1.Location = new System.Drawing.Point(447, 569);
            this.skinButton1.MouseBack = ((System.Drawing.Image)(resources.GetObject("skinButton1.MouseBack")));
            this.skinButton1.Name = "skinButton1";
            this.skinButton1.NormlBack = ((System.Drawing.Image)(resources.GetObject("skinButton1.NormlBack")));
            this.skinButton1.Palace = true;
            this.skinButton1.Size = new System.Drawing.Size(62, 27);
            this.skinButton1.TabIndex = 145;
            this.skinButton1.Text = "取消";
            this.skinButton1.UseHandCursor = false;
            this.skinButton1.UseVisualStyleBackColor = false;
            this.skinButton1.Click += new System.EventHandler(this.skinButton1_Click);
            // 
            // chatListBox_search
            // 
            this.chatListBox_search.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chatListBox_search.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.chatListBox_search.DrawContentType = CCWin.SkinControl.DrawContentType.PersonalMsg;
            this.chatListBox_search.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chatListBox_search.ForeColor = System.Drawing.Color.Black;
            this.chatListBox_search.FriendsMobile = false;
            this.chatListBox_search.ListHadOpenGroup = null;
            this.chatListBox_search.ListSubItemMenu = null;
            this.chatListBox_search.Location = new System.Drawing.Point(12, 65);
            this.chatListBox_search.Margin = new System.Windows.Forms.Padding(0);
            this.chatListBox_search.Name = "chatListBox_search";
            this.chatListBox_search.SelectSubItem = null;
            this.chatListBox_search.ShowNicName = false;
            this.chatListBox_search.Size = new System.Drawing.Size(288, 498);
            this.chatListBox_search.SubItemMenu = null;
            this.chatListBox_search.TabIndex = 138;
            this.chatListBox_search.Visible = false;
            this.chatListBox_search.DoubleClickSubItem += new CCWin.SkinControl.ChatListBox.ChatListEventHandler(this.chatListBox_search_DoubleClickSubItem);
            // 
            // UserSelectedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(594, 606);
            this.Controls.Add(this.skinButton1);
            this.Controls.Add(this.skinButton_ok);
            this.Controls.Add(this.skinLabel_noResult);
            this.Controls.Add(this.chatListBox_search);
            this.Controls.Add(this.skinTextBox1);
            this.Controls.Add(this.skinLabel1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.unitListBox1);
            this.Name = "UserSelectedForm";
            this.Text = "添加用户";
            this.skinTextBox1.ResumeLayout(false);
            this.skinTextBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TalkBase.Client.UnitViews.UnitListBox unitListBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private CCWin.SkinControl.SkinLabel skinLabel1;
        private CCWin.SkinControl.SkinTextBox skinTextBox1;
        private CCWin.SkinControl.SkinLabel skinLabel_noResult;
        private CCWin.SkinControl.SkinButton skinButton_ok;
        private CCWin.SkinControl.SkinButton skinButton1;
        private CCWin.SkinControl.ChatListBox chatListBox_search;
    }
}