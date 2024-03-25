using TalkBase.Client;
using ESFramework.Boost.Controls;
using TalkBase.Client.UnitViews;

namespace GGTalk
{
    partial class MainForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripMenuItem23 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem24 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem25 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem26 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem27 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem28 = new System.Windows.Forms.ToolStripMenuItem();
            this.skinContextMenuStrip_State = new CCWin.SkinControl.SkinContextMenuStrip();
            this.toolStripMenuItem20 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem30 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem31 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem32 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem33 = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.skinContextMenuStrip_main = new CCWin.SkinControl.SkinContextMenuStrip();
            this.toolStripMenuItem_control = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.创建讨论组ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.头像显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.大头像ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.小头像ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.个人资料ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.文件传输助手ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.验证消息toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.labelSignature = new CCWin.SkinControl.SkinLabel();
            this.skinButton_State = new CCWin.SkinControl.SkinButton();
            this.skinToolStrip3 = new CCWin.SkinControl.SkinToolStrip();
            this.toolStripSplitButton4 = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripSplitButton3 = new System.Windows.Forms.ToolStripSplitButton();
            this.创建讨论组ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSplitButton2 = new System.Windows.Forms.ToolStripSplitButton();
            this.清空会话列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skinToolStrip1 = new CCWin.SkinControl.SkinToolStrip();
            this.toolstripButton_mainMenu = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_android = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_iOS = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_pc = new System.Windows.Forms.ToolStripButton();
            this.skinLabel_softName = new CCWin.SkinControl.SkinLabel();
            this.labelName = new CCWin.SkinControl.SkinLabel();
            this.pnlTx = new CCWin.SkinControl.SkinPanel();
            this.searchFriendPanel1 = new GGTalk.SearchFriendPanel();
            this.autoDocker1 = new ESFramework.Boost.Controls.AutoDocker(this.components);
            this.friendListBox1 = new TalkBase.Client.UnitViews.UnitListBox();
            this.recentListBox1 = new TalkBase.Client.UnitViews.RecentListBox();
            this.groupListBox1 = new TalkBase.Client.UnitViews.GroupListBox();
            this.twinkleNotifyIcon1 = new TalkBase.Client.TwinkleNotifyIcon(this.components);
            this.skinContextMenuStrip_State.SuspendLayout();
            this.skinContextMenuStrip_main.SuspendLayout();
            this.skinToolStrip3.SuspendLayout();
            this.skinToolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenuItem23
            // 
            this.toolStripMenuItem23.AutoSize = false;
            this.toolStripMenuItem23.Image = global::GGTalk.Properties.Resources.imonline__2_;
            this.toolStripMenuItem23.Name = "toolStripMenuItem23";
            this.toolStripMenuItem23.Size = new System.Drawing.Size(105, 22);
            this.toolStripMenuItem23.Tag = "1";
            this.toolStripMenuItem23.Text = "我在线上";
            this.toolStripMenuItem23.ToolTipText = "表示希望好友看到您在线。\r\n声音：开启\r\n消息提醒框：开启\r\n会话消息：任务栏头像闪动\r\n";
            // 
            // toolStripMenuItem24
            // 
            this.toolStripMenuItem24.AutoSize = false;
            this.toolStripMenuItem24.Image = global::GGTalk.Properties.Resources.Qme__2_;
            this.toolStripMenuItem24.Name = "toolStripMenuItem24";
            this.toolStripMenuItem24.Size = new System.Drawing.Size(105, 22);
            this.toolStripMenuItem24.Tag = "2";
            this.toolStripMenuItem24.Text = "Q我把";
            this.toolStripMenuItem24.ToolTipText = "表示希望好友主动联系您。\r\n声音：开启\r\n消息提醒框：开启\r\n会话消息：自动弹出会话窗口\r\n";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(121, 6);
            // 
            // toolStripMenuItem25
            // 
            this.toolStripMenuItem25.AutoSize = false;
            this.toolStripMenuItem25.Image = global::GGTalk.Properties.Resources.away__2_;
            this.toolStripMenuItem25.Name = "toolStripMenuItem25";
            this.toolStripMenuItem25.Size = new System.Drawing.Size(105, 22);
            this.toolStripMenuItem25.Tag = "3";
            this.toolStripMenuItem25.Text = "离开";
            this.toolStripMenuItem25.ToolTipText = "表示离开，暂无法处理消息。\r\n声音：开启\r\n消息提醒框：开启\r\n会话消息：任务栏头像闪动\r\n";
            // 
            // toolStripMenuItem26
            // 
            this.toolStripMenuItem26.AutoSize = false;
            this.toolStripMenuItem26.Image = global::GGTalk.Properties.Resources.busy__2_;
            this.toolStripMenuItem26.Name = "toolStripMenuItem26";
            this.toolStripMenuItem26.Size = new System.Drawing.Size(105, 22);
            this.toolStripMenuItem26.Tag = "4";
            this.toolStripMenuItem26.Text = "忙碌";
            this.toolStripMenuItem26.ToolTipText = "表示忙碌，不会及时处理消息。\r\n声音：开启\r\n消息提醒框：开启\r\n会话消息：任务栏显示气泡\r\n";
            // 
            // toolStripMenuItem27
            // 
            this.toolStripMenuItem27.AutoSize = false;
            this.toolStripMenuItem27.Image = global::GGTalk.Properties.Resources.mute__2_;
            this.toolStripMenuItem27.Name = "toolStripMenuItem27";
            this.toolStripMenuItem27.Size = new System.Drawing.Size(105, 22);
            this.toolStripMenuItem27.Tag = "5";
            this.toolStripMenuItem27.Text = "请勿打扰";
            this.toolStripMenuItem27.ToolTipText = "表示不想被打扰。\r\n声音：关闭\r\n消息提醒框：关闭\r\n会话消息：任务栏显示气泡\r\n\r\n";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(121, 6);
            // 
            // toolStripMenuItem28
            // 
            this.toolStripMenuItem28.AutoSize = false;
            this.toolStripMenuItem28.Image = global::GGTalk.Properties.Resources.invisible__2_;
            this.toolStripMenuItem28.Name = "toolStripMenuItem28";
            this.toolStripMenuItem28.Size = new System.Drawing.Size(105, 22);
            this.toolStripMenuItem28.Tag = "6";
            this.toolStripMenuItem28.Text = "隐身";
            this.toolStripMenuItem28.ToolTipText = "表示好友看到您是离线的。\r\n声音：开启\r\n消息提醒框：开启\r\n会话消息：任务栏头像闪动\r\n";
            // 
            // skinContextMenuStrip_State
            // 
            this.skinContextMenuStrip_State.Arrow = System.Drawing.Color.Black;
            this.skinContextMenuStrip_State.Back = System.Drawing.Color.White;
            this.skinContextMenuStrip_State.BackRadius = 4;
            this.skinContextMenuStrip_State.Base = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(200)))), ((int)(((byte)(254)))));
            this.skinContextMenuStrip_State.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinContextMenuStrip_State.Fore = System.Drawing.Color.Black;
            this.skinContextMenuStrip_State.HoverFore = System.Drawing.Color.White;
            this.skinContextMenuStrip_State.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.skinContextMenuStrip_State.ItemAnamorphosis = false;
            this.skinContextMenuStrip_State.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_State.ItemBorderShow = false;
            this.skinContextMenuStrip_State.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_State.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_State.ItemRadius = 4;
            this.skinContextMenuStrip_State.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.None;
            this.skinContextMenuStrip_State.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem20,
            this.toolStripMenuItem30,
            this.toolStripMenuItem31,
            this.toolStripMenuItem32,
            this.toolStripMenuItem33});
            this.skinContextMenuStrip_State.ItemSplitter = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinContextMenuStrip_State.Name = "MenuState";
            this.skinContextMenuStrip_State.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinContextMenuStrip_State.Size = new System.Drawing.Size(125, 114);
            this.skinContextMenuStrip_State.SkinAllColor = true;
            this.skinContextMenuStrip_State.TitleAnamorphosis = false;
            this.skinContextMenuStrip_State.TitleColor = System.Drawing.Color.White;
            this.skinContextMenuStrip_State.TitleRadius = 4;
            this.skinContextMenuStrip_State.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // toolStripMenuItem20
            // 
            this.toolStripMenuItem20.Image = global::GGTalk.Properties.Resources.imonline__2_;
            this.toolStripMenuItem20.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItem20.Name = "toolStripMenuItem20";
            this.toolStripMenuItem20.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem20.Tag = "2";
            this.toolStripMenuItem20.Text = "我在线上";
            this.toolStripMenuItem20.ToolTipText = "表示希望好友看到您在线。\r\n声音：开启\r\n消息提醒框：开启\r\n会话消息：任务栏头像闪动\r\n";
            this.toolStripMenuItem20.Click += new System.EventHandler(this.Item_Click);
            // 
            // toolStripMenuItem30
            // 
            this.toolStripMenuItem30.Image = global::GGTalk.Properties.Resources.away__2_;
            this.toolStripMenuItem30.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItem30.Name = "toolStripMenuItem30";
            this.toolStripMenuItem30.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem30.Tag = "3";
            this.toolStripMenuItem30.Text = "离开";
            this.toolStripMenuItem30.ToolTipText = "表示离开，暂无法处理消息。\r\n声音：开启\r\n消息提醒框：开启\r\n会话消息：任务栏头像闪动\r\n";
            this.toolStripMenuItem30.Click += new System.EventHandler(this.Item_Click);
            // 
            // toolStripMenuItem31
            // 
            this.toolStripMenuItem31.Image = global::GGTalk.Properties.Resources.busy__2_;
            this.toolStripMenuItem31.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItem31.Name = "toolStripMenuItem31";
            this.toolStripMenuItem31.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem31.Tag = "4";
            this.toolStripMenuItem31.Text = "忙碌";
            this.toolStripMenuItem31.ToolTipText = "表示忙碌，不会及时处理消息。\r\n声音：开启\r\n消息提醒框：开启\r\n会话消息：任务栏显示气泡\r\n";
            this.toolStripMenuItem31.Click += new System.EventHandler(this.Item_Click);
            // 
            // toolStripMenuItem32
            // 
            this.toolStripMenuItem32.Image = global::GGTalk.Properties.Resources.mute__2_;
            this.toolStripMenuItem32.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItem32.Name = "toolStripMenuItem32";
            this.toolStripMenuItem32.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem32.Tag = "5";
            this.toolStripMenuItem32.Text = "请勿打扰";
            this.toolStripMenuItem32.ToolTipText = "表示不想被打扰。\r\n声音：关闭\r\n消息提醒框：关闭\r\n会话消息：任务栏显示气泡\r\n\r\n";
            this.toolStripMenuItem32.Click += new System.EventHandler(this.Item_Click);
            // 
            // toolStripMenuItem33
            // 
            this.toolStripMenuItem33.Image = global::GGTalk.Properties.Resources.invisible__2_;
            this.toolStripMenuItem33.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItem33.Name = "toolStripMenuItem33";
            this.toolStripMenuItem33.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem33.Tag = "7";
            this.toolStripMenuItem33.Text = "隐身";
            this.toolStripMenuItem33.ToolTipText = "表示好友看到您是离线的。\r\n声音：开启\r\n消息提醒框：开启\r\n会话消息：任务栏头像闪动\r\n";
            this.toolStripMenuItem33.Click += new System.EventHandler(this.Item_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "icon_contacts_selected.png");
            this.imageList.Images.SetKeyName(1, "icon_group_selected.png");
            this.imageList.Images.SetKeyName(2, "icon_last_selected.png");
            // 
            // skinContextMenuStrip_main
            // 
            this.skinContextMenuStrip_main.Arrow = System.Drawing.Color.Black;
            this.skinContextMenuStrip_main.Back = System.Drawing.Color.White;
            this.skinContextMenuStrip_main.BackRadius = 4;
            this.skinContextMenuStrip_main.Base = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(200)))), ((int)(((byte)(254)))));
            this.skinContextMenuStrip_main.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinContextMenuStrip_main.Fore = System.Drawing.Color.Black;
            this.skinContextMenuStrip_main.HoverFore = System.Drawing.Color.White;
            this.skinContextMenuStrip_main.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.skinContextMenuStrip_main.ItemAnamorphosis = false;
            this.skinContextMenuStrip_main.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_main.ItemBorderShow = false;
            this.skinContextMenuStrip_main.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_main.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinContextMenuStrip_main.ItemRadius = 4;
            this.skinContextMenuStrip_main.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.None;
            this.skinContextMenuStrip_main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_control,
            this.toolStripMenuItem4,
            this.toolStripSeparator4,
            this.toolStripMenuItem3,
            this.创建讨论组ToolStripMenuItem1,
            this.toolStripSeparator5,
            this.头像显示ToolStripMenuItem,
            this.个人资料ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripSeparator1,
            this.文件传输助手ToolStripMenuItem,
            this.toolStripMenuItem5,
            this.验证消息toolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.skinContextMenuStrip_main.ItemSplitter = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinContextMenuStrip_main.Name = "MenuState";
            this.skinContextMenuStrip_main.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinContextMenuStrip_main.Size = new System.Drawing.Size(177, 308);
            this.skinContextMenuStrip_main.SkinAllColor = true;
            this.skinContextMenuStrip_main.TitleAnamorphosis = false;
            this.skinContextMenuStrip_main.TitleColor = System.Drawing.Color.White;
            this.skinContextMenuStrip_main.TitleRadius = 4;
            this.skinContextMenuStrip_main.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // toolStripMenuItem_control
            // 
            this.toolStripMenuItem_control.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem_control.Image")));
            this.toolStripMenuItem_control.Name = "toolStripMenuItem_control";
            this.toolStripMenuItem_control.Size = new System.Drawing.Size(176, 26);
            this.toolStripMenuItem_control.Text = "控制台";
            this.toolStripMenuItem_control.Click += new System.EventHandler(this.toolStripMenuItem_control_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(176, 26);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(173, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Image = global::GGTalk.Properties.Resources.finduser_icon;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(176, 26);
            this.toolStripMenuItem3.Text = "查找用户";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // 创建讨论组ToolStripMenuItem1
            // 
            this.创建讨论组ToolStripMenuItem1.Image = global::GGTalk.Properties.Resources.creategroup_icon;
            this.创建讨论组ToolStripMenuItem1.Name = "创建讨论组ToolStripMenuItem1";
            this.创建讨论组ToolStripMenuItem1.Size = new System.Drawing.Size(176, 26);
            this.创建讨论组ToolStripMenuItem1.Text = "创建讨论组（群）";
            this.创建讨论组ToolStripMenuItem1.Click += new System.EventHandler(this.创建讨论组ToolStripMenuItem1_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(173, 6);
            // 
            // 头像显示ToolStripMenuItem
            // 
            this.头像显示ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.大头像ToolStripMenuItem,
            this.小头像ToolStripMenuItem});
            this.头像显示ToolStripMenuItem.Image = global::GGTalk.Properties.Resources.headdisplay_icon;
            this.头像显示ToolStripMenuItem.Name = "头像显示ToolStripMenuItem";
            this.头像显示ToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.头像显示ToolStripMenuItem.Text = "头像显示";
            // 
            // 大头像ToolStripMenuItem
            // 
            this.大头像ToolStripMenuItem.Checked = true;
            this.大头像ToolStripMenuItem.CheckOnClick = true;
            this.大头像ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.大头像ToolStripMenuItem.Name = "大头像ToolStripMenuItem";
            this.大头像ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.大头像ToolStripMenuItem.Text = "大头像";
            this.大头像ToolStripMenuItem.Click += new System.EventHandler(this.大头像ToolStripMenuItem_Click);
            // 
            // 小头像ToolStripMenuItem
            // 
            this.小头像ToolStripMenuItem.CheckOnClick = true;
            this.小头像ToolStripMenuItem.Name = "小头像ToolStripMenuItem";
            this.小头像ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.小头像ToolStripMenuItem.Text = "小头像";
            this.小头像ToolStripMenuItem.Click += new System.EventHandler(this.小头像ToolStripMenuItem_Click);
            // 
            // 个人资料ToolStripMenuItem
            // 
            this.个人资料ToolStripMenuItem.Image = global::GGTalk.Properties.Resources.personaldata_icon;
            this.个人资料ToolStripMenuItem.Name = "个人资料ToolStripMenuItem";
            this.个人资料ToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.个人资料ToolStripMenuItem.Text = "个人资料";
            this.个人资料ToolStripMenuItem.Click += new System.EventHandler(this.个人资料ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Image = global::GGTalk.Properties.Resources.Password_icon;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(176, 26);
            this.toolStripMenuItem1.Text = "修改密码";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(173, 6);
            // 
            // 文件传输助手ToolStripMenuItem
            // 
            this.文件传输助手ToolStripMenuItem.Image = global::GGTalk.Properties.Resources.transmission_icon;
            this.文件传输助手ToolStripMenuItem.Name = "文件传输助手ToolStripMenuItem";
            this.文件传输助手ToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.文件传输助手ToolStripMenuItem.Text = "文件传输助手";
            this.文件传输助手ToolStripMenuItem.Click += new System.EventHandler(this.文件传输助手ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem5.Image")));
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(176, 26);
            this.toolStripMenuItem5.Text = "我的网盘";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // 验证消息toolStripMenuItem
            // 
            this.验证消息toolStripMenuItem.Image = global::GGTalk.Properties.Resources.verification_icon;
            this.验证消息toolStripMenuItem.Name = "验证消息toolStripMenuItem";
            this.验证消息toolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.验证消息toolStripMenuItem.Text = "验证消息";
            this.验证消息toolStripMenuItem.Click += new System.EventHandler(this.验证消息toolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Image = global::GGTalk.Properties.Resources.signout_icon;
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(176, 26);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Group1.png");
            this.imageList1.Images.SetKeyName(1, "normal_group_40.png");
            // 
            // labelSignature
            // 
            this.labelSignature.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.labelSignature.BackColor = System.Drawing.Color.Transparent;
            this.labelSignature.BorderColor = System.Drawing.Color.White;
            this.labelSignature.BorderSize = 4;
            this.labelSignature.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelSignature.ForeColor = System.Drawing.Color.White;
            this.labelSignature.Location = new System.Drawing.Point(100, 68);
            this.labelSignature.Name = "labelSignature";
            this.labelSignature.Size = new System.Drawing.Size(164, 20);
            this.labelSignature.TabIndex = 100;
            this.labelSignature.Text = "退一步海阔天空！";
            // 
            // skinButton_State
            // 
            this.skinButton_State.BackColor = System.Drawing.Color.Transparent;
            this.skinButton_State.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.skinButton_State.BackRectangle = new System.Drawing.Rectangle(4, 4, 4, 4);
            this.skinButton_State.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.skinButton_State.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton_State.DownBack = global::GGTalk.Properties.Resources.allbtn_down;
            this.skinButton_State.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.skinButton_State.Image = global::GGTalk.Properties.Resources.imonline__2_;
            this.skinButton_State.ImageSize = new System.Drawing.Size(11, 11);
            this.skinButton_State.Location = new System.Drawing.Point(75, 66);
            this.skinButton_State.Margin = new System.Windows.Forms.Padding(0);
            this.skinButton_State.MouseBack = global::GGTalk.Properties.Resources.allbtn_highlight;
            this.skinButton_State.Name = "skinButton_State";
            this.skinButton_State.NormlBack = null;
            this.skinButton_State.Palace = true;
            this.skinButton_State.Size = new System.Drawing.Size(23, 23);
            this.skinButton_State.TabIndex = 128;
            this.skinButton_State.Tag = "1";
            this.skinButton_State.UseHandCursor = false;
            this.skinButton_State.UseVisualStyleBackColor = false;
            this.skinButton_State.Click += new System.EventHandler(this.skinButton_State_Click);
            // 
            // skinToolStrip3
            // 
            this.skinToolStrip3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skinToolStrip3.Arrow = System.Drawing.Color.White;
            this.skinToolStrip3.AutoSize = false;
            this.skinToolStrip3.Back = System.Drawing.Color.White;
            this.skinToolStrip3.BackColor = System.Drawing.Color.Transparent;
            this.skinToolStrip3.BackRadius = 4;
            this.skinToolStrip3.BackRectangle = new System.Drawing.Rectangle(2, 2, 2, 2);
            this.skinToolStrip3.Base = System.Drawing.Color.Transparent;
            this.skinToolStrip3.BaseFore = System.Drawing.Color.Black;
            this.skinToolStrip3.BaseForeAnamorphosis = false;
            this.skinToolStrip3.BaseForeAnamorphosisBorder = 4;
            this.skinToolStrip3.BaseForeAnamorphosisColor = System.Drawing.Color.White;
            this.skinToolStrip3.BaseForeOffset = new System.Drawing.Point(0, 0);
            this.skinToolStrip3.BaseHoverFore = System.Drawing.Color.Black;
            this.skinToolStrip3.BaseItemAnamorphosis = true;
            this.skinToolStrip3.BaseItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.skinToolStrip3.BaseItemBorderShow = false;
            this.skinToolStrip3.BaseItemDown = ((System.Drawing.Image)(resources.GetObject("skinToolStrip3.BaseItemDown")));
            this.skinToolStrip3.BaseItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.skinToolStrip3.BaseItemMouse = ((System.Drawing.Image)(resources.GetObject("skinToolStrip3.BaseItemMouse")));
            this.skinToolStrip3.BaseItemPressed = System.Drawing.Color.Transparent;
            this.skinToolStrip3.BaseItemRadius = 2;
            this.skinToolStrip3.BaseItemRadiusStyle = CCWin.SkinClass.RoundStyle.None;
            this.skinToolStrip3.BaseItemSplitter = System.Drawing.Color.Transparent;
            this.skinToolStrip3.Dock = System.Windows.Forms.DockStyle.None;
            this.skinToolStrip3.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinToolStrip3.Fore = System.Drawing.Color.Black;
            this.skinToolStrip3.GripMargin = new System.Windows.Forms.Padding(2, 2, 4, 2);
            this.skinToolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.skinToolStrip3.HoverFore = System.Drawing.Color.White;
            this.skinToolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.skinToolStrip3.ItemAnamorphosis = false;
            this.skinToolStrip3.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinToolStrip3.ItemBorderShow = false;
            this.skinToolStrip3.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinToolStrip3.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinToolStrip3.ItemRadius = 3;
            this.skinToolStrip3.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.None;
            this.skinToolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton4,
            this.toolStripSplitButton3,
            this.toolStripSplitButton2});
            this.skinToolStrip3.Location = new System.Drawing.Point(2, 113);
            this.skinToolStrip3.Name = "skinToolStrip3";
            this.skinToolStrip3.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.skinToolStrip3.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinToolStrip3.Size = new System.Drawing.Size(308, 28);
            this.skinToolStrip3.SkinAllColor = true;
            this.skinToolStrip3.TabIndex = 127;
            this.skinToolStrip3.Text = "skinToolStrip3";
            this.skinToolStrip3.TitleAnamorphosis = false;
            this.skinToolStrip3.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(228)))), ((int)(((byte)(236)))));
            this.skinToolStrip3.TitleRadius = 4;
            this.skinToolStrip3.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // toolStripSplitButton4
            // 
            this.toolStripSplitButton4.AutoSize = false;
            this.toolStripSplitButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton4.DropDownButtonWidth = 20;
            this.toolStripSplitButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton4.Image")));
            this.toolStripSplitButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton4.Name = "toolStripSplitButton4";
            this.toolStripSplitButton4.Size = new System.Drawing.Size(50, 35);
            this.toolStripSplitButton4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripSplitButton4.ToolTipText = "联系人";
            this.toolStripSplitButton4.ButtonClick += new System.EventHandler(this.toolStripSplitButton4_ButtonClick);
            // 
            // toolStripSplitButton3
            // 
            this.toolStripSplitButton3.AutoSize = false;
            this.toolStripSplitButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton3.DropDownButtonWidth = 20;
            this.toolStripSplitButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.创建讨论组ToolStripMenuItem});
            this.toolStripSplitButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton3.Image")));
            this.toolStripSplitButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton3.Name = "toolStripSplitButton3";
            this.toolStripSplitButton3.Size = new System.Drawing.Size(50, 35);
            this.toolStripSplitButton3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripSplitButton3.ToolTipText = "讨论组";
            this.toolStripSplitButton3.ButtonClick += new System.EventHandler(this.toolStripSplitButton3_ButtonClick);
            // 
            // 创建讨论组ToolStripMenuItem
            // 
            this.创建讨论组ToolStripMenuItem.Name = "创建讨论组ToolStripMenuItem";
            this.创建讨论组ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.创建讨论组ToolStripMenuItem.Text = "创建讨论组";
            this.创建讨论组ToolStripMenuItem.Click += new System.EventHandler(this.创建讨论组ToolStripMenuItem_Click);
            // 
            // toolStripSplitButton2
            // 
            this.toolStripSplitButton2.AutoSize = false;
            this.toolStripSplitButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton2.DropDownButtonWidth = 20;
            this.toolStripSplitButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.清空会话列表ToolStripMenuItem});
            this.toolStripSplitButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton2.Image")));
            this.toolStripSplitButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton2.Name = "toolStripSplitButton2";
            this.toolStripSplitButton2.Size = new System.Drawing.Size(50, 35);
            this.toolStripSplitButton2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripSplitButton2.ToolTipText = "会话";
            this.toolStripSplitButton2.ButtonClick += new System.EventHandler(this.toolStripSplitButton2_ButtonClick);
            // 
            // 清空会话列表ToolStripMenuItem
            // 
            this.清空会话列表ToolStripMenuItem.Name = "清空会话列表ToolStripMenuItem";
            this.清空会话列表ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.清空会话列表ToolStripMenuItem.Text = "清空会话列表";
            this.清空会话列表ToolStripMenuItem.Click += new System.EventHandler(this.清空会话列表ToolStripMenuItem_Click);
            // 
            // skinToolStrip1
            // 
            this.skinToolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skinToolStrip1.Arrow = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.skinToolStrip1.AutoSize = false;
            this.skinToolStrip1.Back = System.Drawing.Color.White;
            this.skinToolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.skinToolStrip1.BackRadius = 4;
            this.skinToolStrip1.BackRectangle = new System.Drawing.Rectangle(10, 10, 10, 10);
            this.skinToolStrip1.Base = System.Drawing.Color.Transparent;
            this.skinToolStrip1.BaseFore = System.Drawing.Color.Black;
            this.skinToolStrip1.BaseForeAnamorphosis = true;
            this.skinToolStrip1.BaseForeAnamorphosisBorder = 4;
            this.skinToolStrip1.BaseForeAnamorphosisColor = System.Drawing.Color.White;
            this.skinToolStrip1.BaseForeOffset = new System.Drawing.Point(0, 0);
            this.skinToolStrip1.BaseHoverFore = System.Drawing.Color.Black;
            this.skinToolStrip1.BaseItemAnamorphosis = true;
            this.skinToolStrip1.BaseItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(123)))), ((int)(((byte)(123)))));
            this.skinToolStrip1.BaseItemBorderShow = true;
            this.skinToolStrip1.BaseItemDown = ((System.Drawing.Image)(resources.GetObject("skinToolStrip1.BaseItemDown")));
            this.skinToolStrip1.BaseItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.skinToolStrip1.BaseItemMouse = ((System.Drawing.Image)(resources.GetObject("skinToolStrip1.BaseItemMouse")));
            this.skinToolStrip1.BaseItemPressed = System.Drawing.Color.Transparent;
            this.skinToolStrip1.BaseItemRadius = 2;
            this.skinToolStrip1.BaseItemRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinToolStrip1.BaseItemSplitter = System.Drawing.Color.Transparent;
            this.skinToolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.skinToolStrip1.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinToolStrip1.Fore = System.Drawing.Color.Black;
            this.skinToolStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 4, 2);
            this.skinToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.skinToolStrip1.HoverFore = System.Drawing.Color.White;
            this.skinToolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.skinToolStrip1.ItemAnamorphosis = false;
            this.skinToolStrip1.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinToolStrip1.ItemBorderShow = false;
            this.skinToolStrip1.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinToolStrip1.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinToolStrip1.ItemRadius = 3;
            this.skinToolStrip1.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.None;
            this.skinToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolstripButton_mainMenu,
            this.toolStripButton1,
            this.toolStripButton3,
            this.toolStripButton_android,
            this.toolStripButton_iOS,
            this.toolStripButton_pc});
            this.skinToolStrip1.Location = new System.Drawing.Point(1, 625);
            this.skinToolStrip1.Name = "skinToolStrip1";
            this.skinToolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.skinToolStrip1.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinToolStrip1.Size = new System.Drawing.Size(312, 24);
            this.skinToolStrip1.SkinAllColor = true;
            this.skinToolStrip1.TabIndex = 123;
            this.skinToolStrip1.Text = "skinToolStrip2";
            this.skinToolStrip1.TitleAnamorphosis = false;
            this.skinToolStrip1.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(228)))), ((int)(((byte)(236)))));
            this.skinToolStrip1.TitleRadius = 4;
            this.skinToolStrip1.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // toolstripButton_mainMenu
            // 
            this.toolstripButton_mainMenu.AutoSize = false;
            this.toolstripButton_mainMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolstripButton_mainMenu.Image = global::GGTalk.Properties.Resources.more_icon;
            this.toolstripButton_mainMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripButton_mainMenu.Margin = new System.Windows.Forms.Padding(4, 1, 0, 2);
            this.toolstripButton_mainMenu.Name = "toolstripButton_mainMenu";
            this.toolstripButton_mainMenu.Size = new System.Drawing.Size(24, 24);
            this.toolstripButton_mainMenu.Text = "toolStripButton4";
            this.toolstripButton_mainMenu.ToolTipText = "主菜单";
            this.toolstripButton_mainMenu.Click += new System.EventHandler(this.toolQQMenu_Click);
            this.toolstripButton_mainMenu.MouseEnter += new System.EventHandler(this.FocusCurrent);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.AutoSize = false;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::GGTalk.Properties.Resources.setup_icon;
            this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Margin = new System.Windows.Forms.Padding(4, 1, 0, 2);
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton1.Text = "toolStripButton2";
            this.toolStripButton1.ToolTipText = "打开系统设置";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            this.toolStripButton1.MouseEnter += new System.EventHandler(this.FocusCurrent);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::GGTalk.Properties.Resources.search_icon;
            this.toolStripButton3.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Margin = new System.Windows.Forms.Padding(4, 1, 0, 2);
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(24, 21);
            this.toolStripButton3.Text = "查找联系人或群组";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton_android
            // 
            this.toolStripButton_android.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton_android.AutoSize = false;
            this.toolStripButton_android.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_android.Image = global::GGTalk.Properties.Resources.android;
            this.toolStripButton_android.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_android.Name = "toolStripButton_android";
            this.toolStripButton_android.Size = new System.Drawing.Size(30, 24);
            this.toolStripButton_android.Text = "Android在线";
            this.toolStripButton_android.Visible = false;
            // 
            // toolStripButton_iOS
            // 
            this.toolStripButton_iOS.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton_iOS.AutoSize = false;
            this.toolStripButton_iOS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_iOS.Image = global::GGTalk.Properties.Resources.apple;
            this.toolStripButton_iOS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_iOS.Name = "toolStripButton_iOS";
            this.toolStripButton_iOS.Size = new System.Drawing.Size(30, 24);
            this.toolStripButton_iOS.Text = "iOS在线";
            this.toolStripButton_iOS.Visible = false;
            // 
            // toolStripButton_pc
            // 
            this.toolStripButton_pc.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton_pc.AutoSize = false;
            this.toolStripButton_pc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_pc.Image = global::GGTalk.Properties.Resources.win1;
            this.toolStripButton_pc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_pc.Name = "toolStripButton_pc";
            this.toolStripButton_pc.Size = new System.Drawing.Size(30, 24);
            this.toolStripButton_pc.Text = "PC在线";
            this.toolStripButton_pc.Visible = false;
            // 
            // skinLabel_softName
            // 
            this.skinLabel_softName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skinLabel_softName.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel_softName.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel_softName.BorderColor = System.Drawing.Color.White;
            this.skinLabel_softName.BorderSize = 4;
            this.skinLabel_softName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.skinLabel_softName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.skinLabel_softName.ForeColor = System.Drawing.Color.White;
            this.skinLabel_softName.Location = new System.Drawing.Point(4, 8);
            this.skinLabel_softName.Name = "skinLabel_softName";
            this.skinLabel_softName.Size = new System.Drawing.Size(135, 23);
            this.skinLabel_softName.TabIndex = 99;
            this.skinLabel_softName.Text = "GGTalk 2022";
            this.skinLabel_softName.Click += new System.EventHandler(this.skinLabel_softName_Click);
            // 
            // labelName
            // 
            this.labelName.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.labelName.BackColor = System.Drawing.Color.Transparent;
            this.labelName.BorderColor = System.Drawing.Color.White;
            this.labelName.BorderSize = 4;
            this.labelName.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelName.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.labelName.ForeColor = System.Drawing.Color.White;
            this.labelName.Location = new System.Drawing.Point(83, 39);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(181, 23);
            this.labelName.TabIndex = 99;
            this.labelName.Text = "David";
            // 
            // pnlTx
            // 
            this.pnlTx.BackColor = System.Drawing.Color.Transparent;
            this.pnlTx.BackgroundImage = global::GGTalk.Properties.Resources._64;
            this.pnlTx.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlTx.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.pnlTx.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlTx.DownBack = null;
            this.pnlTx.Location = new System.Drawing.Point(8, 39);
            this.pnlTx.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTx.MouseBack = null;
            this.pnlTx.Name = "pnlTx";
            this.pnlTx.NormlBack = null;
            this.pnlTx.Radius = 4;
            this.pnlTx.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.pnlTx.Size = new System.Drawing.Size(64, 64);
            this.pnlTx.TabIndex = 131;
            this.pnlTx.Click += new System.EventHandler(this.skinButton_headImage_Click);
            // 
            // searchFriendPanel1
            // 
            this.searchFriendPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchFriendPanel1.AutoSize = true;
            this.searchFriendPanel1.BackColor = System.Drawing.Color.White;
            this.searchFriendPanel1.Location = new System.Drawing.Point(1, 113);
            this.searchFriendPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.searchFriendPanel1.Name = "searchFriendPanel1";
            this.searchFriendPanel1.Size = new System.Drawing.Size(312, 510);
            this.searchFriendPanel1.TabIndex = 134;
            this.searchFriendPanel1.Visible = false;
            // 
            // autoDocker1
            // 
            this.autoDocker1.DockedForm = this;
            // 
            // friendListBox1
            // 
            this.friendListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.friendListBox1.AutoSize = true;
            this.friendListBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.friendListBox1.CatalogContextMenuVisiable = true;
            this.friendListBox1.DefaultFriendCatalogName = "我的好友";
            this.friendListBox1.DefaultGroupCatalogName = "我的群";
            this.friendListBox1.DrawContentType = CCWin.SkinControl.DrawContentType.PersonalMsg;
            this.friendListBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.friendListBox1.ForeColor = System.Drawing.Color.Black;
            this.friendListBox1.FriendsMobile = true;
            this.friendListBox1.IconSizeMode = CCWin.SkinControl.ChatListItemIcon.Large;
            this.friendListBox1.Location = new System.Drawing.Point(1, 144);
            this.friendListBox1.Margin = new System.Windows.Forms.Padding(2);
            this.friendListBox1.Name = "friendListBox1";
            this.friendListBox1.PreLoadCatalog = true;
            this.friendListBox1.Size = new System.Drawing.Size(312, 480);
            this.friendListBox1.TabIndex = 2;
            this.friendListBox1.UserContextMenuVisiable = false;
            this.friendListBox1.Visible = false;
            // 
            // recentListBox1
            // 
            this.recentListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.recentListBox1.AutoSize = true;
            this.recentListBox1.BackColor = System.Drawing.Color.White;
            this.recentListBox1.Location = new System.Drawing.Point(1, 144);
            this.recentListBox1.Margin = new System.Windows.Forms.Padding(4);
            this.recentListBox1.Name = "recentListBox1";
            this.recentListBox1.Size = new System.Drawing.Size(312, 480);
            this.recentListBox1.TabIndex = 138;
            this.recentListBox1.Visible = false;
            // 
            // groupListBox1
            // 
            this.groupListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupListBox1.AutoSize = true;
            this.groupListBox1.BackColor = System.Drawing.Color.White;
            this.groupListBox1.Location = new System.Drawing.Point(1, 144);
            this.groupListBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupListBox1.Name = "groupListBox1";
            this.groupListBox1.Size = new System.Drawing.Size(312, 480);
            this.groupListBox1.TabIndex = 139;
            this.groupListBox1.Visible = false;
            // 
            // twinkleNotifyIcon1
            // 
            this.twinkleNotifyIcon1.ContextMenuStrip = this.skinContextMenuStrip_main;
            this.twinkleNotifyIcon1.Visible = true;
            this.twinkleNotifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.BorderPalace = global::GGTalk.Properties.Resources.BackPalace;
            this.CanResize = true;
            this.ClientSize = new System.Drawing.Size(315, 650);
            this.CloseDownBack = global::GGTalk.Properties.Resources.btn_close_down;
            this.CloseMouseBack = global::GGTalk.Properties.Resources.btn_close_highlight;
            this.CloseNormlBack = global::GGTalk.Properties.Resources.btn_close_disable;
            this.Controls.Add(this.searchFriendPanel1);
            this.Controls.Add(this.friendListBox1);
            this.Controls.Add(this.pnlTx);
            this.Controls.Add(this.labelSignature);
            this.Controls.Add(this.skinButton_State);
            this.Controls.Add(this.skinToolStrip1);
            this.Controls.Add(this.skinLabel_softName);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.recentListBox1);
            this.Controls.Add(this.groupListBox1);
            this.Controls.Add(this.skinToolStrip3);
            this.EffectCaption = CCWin.TitleType.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaxDownBack = global::GGTalk.Properties.Resources.btn_max_down;
            this.MaximumSize = new System.Drawing.Size(320, 764);
            this.MaxMouseBack = global::GGTalk.Properties.Resources.btn_max_highlight;
            this.MaxNormlBack = global::GGTalk.Properties.Resources.btn_max_normal;
            this.MiniDownBack = global::GGTalk.Properties.Resources.btn_mini_down;
            this.MinimizeBox = true;
            this.MiniMouseBack = global::GGTalk.Properties.Resources.btn_mini_highlight;
            this.MinimumSize = new System.Drawing.Size(255, 500);
            this.MiniNormlBack = global::GGTalk.Properties.Resources.btn_mini_normal;
            this.Name = "MainForm";
            this.RestoreDownBack = global::GGTalk.Properties.Resources.btn_restore_down;
            this.RestoreMouseBack = global::GGTalk.Properties.Resources.btn_restore_highlight;
            this.RestoreNormlBack = global::GGTalk.Properties.Resources.btn_restore_normal;
            this.Shadow = false;
            this.ShowBorder = false;
            this.ShowDrawIcon = false;
            this.SkinOpacity = 30D;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.skinContextMenuStrip_State.ResumeLayout(false);
            this.skinContextMenuStrip_main.ResumeLayout(false);
            this.skinToolStrip3.ResumeLayout(false);
            this.skinToolStrip3.PerformLayout();
            this.skinToolStrip1.ResumeLayout(false);
            this.skinToolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinLabel labelSignature;
        private System.Windows.Forms.ToolTip toolTip1;
        private CCWin.SkinControl.SkinToolStrip skinToolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private CCWin.SkinControl.SkinLabel labelName;
        private System.Windows.Forms.ToolStripButton toolstripButton_mainMenu;
        private CCWin.SkinControl.SkinButton skinButton_State;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem23;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem24;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem25;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem26;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem27;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem28;
        private CCWin.SkinControl.SkinContextMenuStrip skinContextMenuStrip_State;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem20;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem30;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem31;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem32;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem33;
        private System.Windows.Forms.ImageList imageList;
        private UnitListBox friendListBox1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton4;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton3;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton2;
        private CCWin.SkinControl.SkinToolStrip skinToolStrip3;
        private CCWin.SkinControl.SkinLabel skinLabel_softName;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem 清空会话列表ToolStripMenuItem;
        private CCWin.SkinControl.SkinContextMenuStrip skinContextMenuStrip_main;
        private System.Windows.Forms.ToolStripMenuItem 个人资料ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 创建讨论组ToolStripMenuItem1;
        private CCWin.SkinControl.SkinPanel pnlTx;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem 头像显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 大头像ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 小头像ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 创建讨论组ToolStripMenuItem;
        private AutoDocker autoDocker1;
        private RecentListBox recentListBox1;
        private SearchFriendPanel searchFriendPanel1;
        private TwinkleNotifyIcon twinkleNotifyIcon1;
        private GroupListBox groupListBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem 文件传输助手ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 验证消息toolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton_android;
        private System.Windows.Forms.ToolStripButton toolStripButton_iOS;
        private System.Windows.Forms.ToolStripButton toolStripButton_pc;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_control;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
    }
}

