using CCWin;
using CCWin.SkinControl;
using ESBasic;
using ESFramework;
using ESFramework.Boost.DynamicGroup.Passive;
using ESFramework.Boost.NetworkDisk.Passive;
using ESPlus.Application.Basic;
using ESPlus.Application.FileTransfering;
using ESPlus.FileTransceiver;
using OMCS.Passive;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TalkBase;
using TalkBase.Client;
using TalkBase.Client.Bridges;

namespace GGTalk
{
    /// <summary>
    /// 主窗体。
    /// </summary>
    public partial class MainForm : BaseForm, IChatSupporter, ITwinkleNotifySupporter, IUnitInfoProvider, IChatFormShower
    {
        private bool initialized = false;       
        private UserStatus myStatus4Relogon = UserStatus.OffLine;
        private INDiskOutter nDiskOutter; 
        private ResourceCenter<GGUser, GGGroup > resourceCenter;
        private Event2ChatFormBridge<GGUser, GGGroup > event2ChatFormBridge;
        private Event2ChatListBridge<GGUser, GGGroup > event2ChatListBridge;
        private ChatList2ChatFormBridge<GGUser, GGGroup > chatList2ChatFormBridge;
        private IDynamicGroupOutter dynamicGroupOutter;
        

        public TwinkleNotifyIcon TwinkleNotifyIcon
        {
            get
            {
                return this.twinkleNotifyIcon1;
            }
        }     

        /// <summary>
        /// 是否为管理员
        /// </summary>
        public static bool IsAdmin
        {
            get;set;
        }

        #region Ctor ,Initialize
        public MainForm()
        {
            InitializeComponent();


            GlobalResourceManager.SetUiSafeInvoker(new ESBasic.Helpers.UiSafeInvoker(this, true,true, GlobalResourceManager.Logger));

            this.friendListBox1.AddCatalogClicked += new CbGeneric(friendListBox1_AddCatalogClicked);
            this.friendListBox1.ChangeCatalogNameClicked += new CbGeneric<string>(friendListBox1_ChangeCatalogNameClicked);
            this.friendListBox1.ChangeUnitCommentNameClicked += new CbGeneric<IUnit>(friendListBox1_ChangeUnitCommentNameClicked);
            this.friendListBox1.ViewInfoClicked += new CbGeneric<IUnit>(friendListBox1_ViewInfoClicked);
        }

        void friendListBox1_ViewInfoClicked(IUnit unit)
        {
            if (unit.ID == this.resourceCenter.CurrentUserID)
            {
                return;
            }

            UserInfoForm form = new UserInfoForm(this.resourceCenter, (GGUser)unit);
            form.Show();
        }

        void friendListBox1_ChangeUnitCommentNameClicked(IUnit unit)
        {
            EditCommentNameForm form = new EditCommentNameForm(unit.CommentName);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.resourceCenter.ClientOutter.ChangeUnitCommentName(unit.ID, form.NewName);                
            }
        }        

        void friendListBox1_AddCatalogClicked()
        {
            EditCatelogNameForm form = new EditCatelogNameForm();
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.friendListBox1.AddCatalog(form.NewName);
            }
        }

        void friendListBox1_ChangeCatalogNameClicked(string catalogName)
        {
            EditCatelogNameForm form = new EditCatelogNameForm(catalogName);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.friendListBox1.ChangeCatelogName(catalogName, form.NewName);
            }
        }        

        /// <summary>
        /// 初始化流程：
        /// （1）Initialize时，从服务器加载自己的全部信息，从本地缓存文件加载好友列表。
        /// （2）MainForm_Load，填充ChatListBox
        /// （3）MainForm_Shown，调用resourceCenter.ClientGlobalCache在后台刷新：若是第一次登录，则分批从服务器加载好友资料。否则，从服务器获取好友最新状态和版本，并刷新本地缓存。
        /// （4）resourceCenter.ClientGlobalCache.FriendRTDataRefreshCompleted 事件，请求离线消息、离线文件、正式通知好友自己上线
        /// </summary>
        public void Initialize(ResourceCenter<GGUser, GGGroup> center, UserStatus iniStatus, IDynamicGroupOutter groupOutter)
        {           
            this.Cursor = Cursors.WaitCursor;
            this.dynamicGroupOutter = groupOutter;
            this.resourceCenter = center;
            this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus = (UserStatus)((int)iniStatus);
            this.resourceCenter.ClientGlobalCache.BatchLoadCompleted += new CbGeneric(globalUserCache_FriendRTDataRefreshCompleted);
            this.resourceCenter.ClientGlobalCache.MyBaseInfoChanged += new CbGeneric(ClientGlobalCache_MyInfoChanged);
            this.resourceCenter.ClientGlobalCache.MyStatusChanged += ClientGlobalCache_MyStatusChanged;         
            this.resourceCenter.ChatFormController.FormCreated += new CbGeneric<IChatForm ,IUnit>(chatFormController_ChatFormCreated);
            this.twinkleNotifyIcon1.Initialize(this, this, (IBrige4ClientOutter)this.resourceCenter.ClientOutter, this.resourceCenter.TalkBaseInfoTypes);
            this.resourceCenter.SetExtendCache(CommonOptions.TwinkleNotifyIcon, this.twinkleNotifyIcon1);
            this.event2ChatFormBridge = new Event2ChatFormBridge<GGUser, GGGroup >(this.resourceCenter, GlobalResourceManager.UiSafeInvoker,this);

            this.toolStripMenuItem4.Visible = false;            
            this.toolStripSplitButton4_ButtonClick(this.toolStripSplitButton4, new EventArgs());

            this.event2ChatListBridge = new Event2ChatListBridge<GGUser, GGGroup >(this.resourceCenter, GlobalResourceManager.UiSafeInvoker, this.twinkleNotifyIcon1, this.friendListBox1, this.groupListBox1, this.recentListBox1);
            this.chatList2ChatFormBridge = new ChatList2ChatFormBridge<GGUser, GGGroup >(this.resourceCenter, this.friendListBox1, this.groupListBox1, this.recentListBox1,this);

            this.toolTip1.SetToolTip(this.pnlTx, "帐号：" + this.resourceCenter.CurrentUserID);              
            this.myStatus4Relogon = this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus;
            this.labelSignature.Text = this.resourceCenter.ClientGlobalCache.CurrentUser.Signature;
            this.labelName.Text = this.resourceCenter.ClientGlobalCache.CurrentUser.Name;
            this.skinLabel_softName.Text = GlobalResourceManager.SoftwareName;
            this.ShowLatestStatus();
            

            this.MaximumSize = new Size(543, Screen.GetWorkingArea(this).Height);
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - this.Width - 20, 40);

            this.groupListBox1.Initialize(this.resourceCenter.ClientGlobalCache.CurrentUser ,this);
            this.recentListBox1.Initialize(this);
            this.friendListBox1.Initialize(this.resourceCenter.ClientGlobalCache.CurrentUser, this);
            this.friendListBox1.Tag = this.resourceCenter.RapidPassiveEngine.CurrentUserID;
            if (!SystemSettings.Singleton.ShowLargeIcon)
            {
                this.friendListBox1.IconSizeMode = ChatListItemIcon.Small;
                this.大头像ToolStripMenuItem.Checked = false;
                this.小头像ToolStripMenuItem.Checked = true;
            }
            if (!MainForm.IsAdmin)
            {
                this.toolStripMenuItem_control.Visible = false;
                this.toolStripSeparator4.Visible = false;
            }

            //文件传送
            this.resourceCenter.RapidPassiveEngine.FileOutter.FileRequestReceived += new CbFileRequestReceived(fileOutter_FileRequestReceived);
            this.resourceCenter.RapidPassiveEngine.FileOutter.FileResponseReceived += new CbGeneric<ITransferingProject, bool>(fileOutter_FileResponseReceived);
          
            this.resourceCenter.RapidPassiveEngine.ConnectionInterrupted += new CbGeneric(rapidPassiveEngine_ConnectionInterrupted);//预订断线事件
            this.resourceCenter.RapidPassiveEngine.BasicOutter.BeingPushedOut += new CbGeneric(BasicOutter_BeingPushedOut);
            this.resourceCenter.RapidPassiveEngine.BasicOutter.BeingKickedOut += new CbGeneric(BasicOutter_BeingKickedOut);
            this.resourceCenter.RapidPassiveEngine.BasicOutter.MyDeviceOffline += new CbGeneric<ClientType>(BasicOutter_MyDeviceOffline);
            this.resourceCenter.RapidPassiveEngine.BasicOutter.MyDeviceOnline += new CbGeneric<ClientType>(BasicOutter_MyDeviceOnline);
            this.resourceCenter.RapidPassiveEngine.RelogonCompleted += new CbGeneric<LogonResponse>(rapidPassiveEngine_RelogonCompleted);//预订重连成功事件       

            MultimediaManagerFactory.GetSingleton().ConnectionInterrupted += MainForm_ConnectionInterrupted;
            MultimediaManagerFactory.GetSingleton().ConnectionRebuildSucceed += MainForm_ConnectionRebuildSucceed;

            //ShortConnectionHelper.Singleton.InnerMultimediaManager.DesktopEncodeQuality = 20;
            this.searchFriendPanel1.Initialize(this.resourceCenter,this);

            //网盘访问器
            this.nDiskOutter = new NDiskOutter(this.resourceCenter.RapidPassiveEngine.FileOutter, this.resourceCenter.RapidPassiveEngine.CustomizeOutter);           
        }

        private void MainForm_ConnectionRebuildSucceed(System.Net.IPEndPoint obj)
        {
            
        }

        private void MainForm_ConnectionInterrupted(System.Net.IPEndPoint obj)
        {
            
        }

        private void ShowLatestStatus()
        {
            this.pnlTx.BackgroundImage = GlobalResourceManager.GetHeadImage(this.resourceCenter.ClientGlobalCache.CurrentUser);
            this.skinButton_State.Enabled = true;
            this.skinButton_State.Image = GlobalResourceManager.GetStatusImage(this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus);
            this.skinButton_State.Tag = (int)this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus;             
            this.twinkleNotifyIcon1.ChangeText(String.Format("{0}：{1}（{2}）\n状态：{3}", GlobalResourceManager.SoftwareName, this.resourceCenter.ClientGlobalCache.CurrentUser.Name, this.resourceCenter.ClientGlobalCache.CurrentUser.UserID, GlobalResourceManager.GetUserStatusName(this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus)));
            this.twinkleNotifyIcon1.ChangeMyStatus(this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus);

            this.toolStripButton_pc.Visible = true;

            List<ClientType> deviceList = this.resourceCenter.RapidPassiveEngine.BasicOutter.GetMyOnlineDevice();
            this.toolStripButton_iOS.Visible = deviceList.Contains(ClientType.IOS);
            this.toolStripButton_android.Visible = deviceList.Contains(ClientType.Android);           
        }

        void BasicOutter_MyDeviceOnline(ClientType clientType)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<ClientType>(this.BasicOutter_MyDeviceOnline), clientType);
            }
            else
            {
                if (clientType == ClientType.IOS)
                {
                    this.toolStripButton_iOS.Visible = true;                    
                }

                if (clientType == ClientType.Android)
                {
                    this.toolStripButton_android.Visible = true;
                }
            }
        }

        void BasicOutter_MyDeviceOffline(ClientType clientType)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<ClientType>(this.BasicOutter_MyDeviceOffline), clientType);
            }
            else
            {
                if (clientType == ClientType.IOS)
                {
                    this.toolStripButton_iOS.Visible = false;
                }

                if (clientType == ClientType.Android)
                {
                    this.toolStripButton_android.Visible = false;
                }
            }
        }                     

        void ClientGlobalCache_MyInfoChanged()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CbGeneric(this.ClientGlobalCache_MyInfoChanged));
            }else
            {
                this.labelSignature.Text = this.resourceCenter.ClientGlobalCache.CurrentUser.Signature;
                this.pnlTx.BackgroundImage = GlobalResourceManager.GetHeadImage(this.resourceCenter.ClientGlobalCache.CurrentUser);
                this.labelName.Text = this.resourceCenter.ClientGlobalCache.CurrentUser.Name;
                this.twinkleNotifyIcon1.ChangeText(String.Format("{0}：{1}（{2}）\n状态：{3}", GlobalResourceManager.SoftwareName, this.resourceCenter.ClientGlobalCache.CurrentUser.Name, this.resourceCenter.ClientGlobalCache.CurrentUser.UserID, GlobalResourceManager.GetUserStatusName(this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus)));
            }
        }

        private void ClientGlobalCache_MyStatusChanged()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CbGeneric(this.ClientGlobalCache_MyStatusChanged));
            }
            else
            {
                this.skinButton_State.Enabled = true;
                this.skinButton_State.Image = GlobalResourceManager.GetStatusImage(this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus);
                this.skinButton_State.Tag = (int)this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus;
                this.twinkleNotifyIcon1.ChangeText(String.Format("{0}：{1}（{2}）\n状态：{3}", GlobalResourceManager.SoftwareName, this.resourceCenter.ClientGlobalCache.CurrentUser.Name, this.resourceCenter.ClientGlobalCache.CurrentUser.UserID, GlobalResourceManager.GetUserStatusName(this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus)));
            }
        }

        void chatFormController_ChatFormCreated(IChatForm form ,IUnit unit)
        {
            if (unit.UnitType == UnitType.User)
            {
                //this.twinkleNotifyIcon1.RemoveUnhanleMessage(unit);
            }
            else if (unit.UnitType == UnitType.Group)
            {
                //this.twinkleNotifyIcon1.RemoveUnhanleMessage(unit);
                GroupChatForm groupChatForm = form as GroupChatForm;
                if (groupChatForm != null)
                {
                    groupChatForm.ExitGroupClicked += new CbGeneric<string>(groupChatForm_ExitGroupClicked);
                    groupChatForm.DeleteGroupClicked += GroupChatForm_DeleteGroupClicked;
                }
            }
        
        }

        private void GroupChatForm_DeleteGroupClicked(IGroup group)
        {
            this.groupListBox1.DismissGroup(group);
        }

        void groupChatForm_ExitGroupClicked(string groupID)
        {
            this.groupListBox1.RequestQuitGroup(groupID);
        }

        void BasicOutter_BeingPushedOut()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CbGeneric(this.BasicOutter_BeingPushedOut));
            }
            else
            {
                this.ForceClose("已经在其它地方登录！");
            }
        }

        void BasicOutter_BeingKickedOut()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CbGeneric(this.BasicOutter_BeingKickedOut));
            }
            else
            {
                this.ForceClose("已经被强制下线！");
            }
        }

        private void ForceClose(string tips)
        {
            this.reconnecting = false;
            MessageBox.Show(tips,GlobalResourceManager.SoftwareName);
            this.gotoExit = true;
            this.Close();
        }

        #region globalUserCache_FriendRTDataRefreshCompleted
        void globalUserCache_FriendRTDataRefreshCompleted()
        {            
            GlobalResourceManager.UiSafeInvoker.ActionOnUI(this.do_globalUserCache_FriendRTDataRefreshCompleted);
        }

        void do_globalUserCache_FriendRTDataRefreshCompleted()
        {
            //请求离线消息 
            this.resourceCenter.ClientOutter.RequestOfflineMessage();
            //请求离线文件
            this.resourceCenter.ClientOutter.RequestOfflineFile();
            //正式通知好友，自己上线
            this.resourceCenter.ClientOutter.ChangeMyStatus(this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus);            
            this.InitializeFinished();
        }

        private void InitializeFinished()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new CbGeneric(this.InitializeFinished));
            }
            else
            {
                this.friendListBox1.SortAllUnit();
                this.Cursor = Cursors.Default;
            }
        }
        #endregion     

        #endregion

        #region 网络状态变化事件
        #region rapidPassiveEngine_RelogonCompleted
        void rapidPassiveEngine_RelogonCompleted(LogonResponse logonResponse)
        {
            GlobalResourceManager.UiSafeInvoker.ActionOnUI<LogonResponse>(this.do_rapidPassiveEngine_RelogonCompleted, logonResponse);
        }

        void do_rapidPassiveEngine_RelogonCompleted(LogonResponse logonResponse)
        {
            if (logonResponse.LogonResult != LogonResult.Succeed)
            {
                this.twinkleNotifyIcon1.ChangeText(String.Format("{0}：{1}（{2}）\n状态：离线，请重新登录。", GlobalResourceManager.SoftwareName, this.resourceCenter.ClientGlobalCache.CurrentUser.Name, this.resourceCenter.ClientGlobalCache.CurrentUser.UserID));
                MessageBoxEx.Show("自动重登录失败，可能是密码已经被修改。请重启程序，手动登录！", GlobalResourceManager.SoftwareName);
                return;
            }

            this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus = this.myStatus4Relogon;
            this.ShowLatestStatus();
            this.resourceCenter.ClientGlobalCache.StartRefreshFriendInfo();
        }
        #endregion

        #region rapidPassiveEngine_ConnectionInterrupted
        private bool reconnecting = true;
        void rapidPassiveEngine_ConnectionInterrupted()
        {
            GlobalResourceManager.UiSafeInvoker.ActionOnUI(this.do_rapidPassiveEngine_ConnectionInterrupted);
        }

        void do_rapidPassiveEngine_ConnectionInterrupted()
        {
            this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus = UserStatus.OffLine;
            this.pnlTx.BackgroundImage = GlobalResourceManager.GetHeadImage(this.resourceCenter.ClientGlobalCache.CurrentUser);
            this.skinButton_State.Image = Properties.Resources.imoffline__2_;
            this.skinButton_State.Tag = ChatListSubItem.UserStatus.OffLine;
            this.skinButton_State.Enabled = false;

            this.toolStripButton_pc.Visible = false;
            this.toolStripButton_iOS.Visible = false;
            this.toolStripButton_android.Visible = false;

            //this.myselfChatListSubItem.Status = ChatListSubItem.UserStatus.OffLine;
            string reconnectingStr = this.reconnecting ? "，正在重连 . . ." : "";
            this.twinkleNotifyIcon1.ChangeText(String.Format("{0}：{1}（{2}）\n状态：离线{3}", GlobalResourceManager.SoftwareName, this.resourceCenter.ClientGlobalCache.CurrentUser.Name, this.resourceCenter.ClientGlobalCache.CurrentUser.UserID, reconnectingStr));
            this.twinkleNotifyIcon1.ChangeMyStatus(UserStatus.OffLine);

            foreach (GGUser friend in this.resourceCenter.ClientGlobalCache.GetAllUser())
            {
                friend.UserStatus = UserStatus.OffLine;

                //ChatListSubItem[] items = this.chatListBox.GetSubItemsById(friend.UserID);
                //if (items != null && items.Length > 0)
                //{
                //    items[0].Status = ChatListSubItem.UserStatus.OffLine;
                //}
            }
            this.friendListBox1.SetAllUnitOffline();
        }
        #endregion
        #endregion      

        #region MainForm_Load       
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.DesignMode)
                {                   
                    this.twinkleNotifyIcon1.Visible = true;
                }

                InformationForm frm = new InformationForm(this.resourceCenter.RapidPassiveEngine.CurrentUserID, this.resourceCenter.ClientGlobalCache.CurrentUser.Name, GlobalResourceManager.GetHeadImage(this.resourceCenter.ClientGlobalCache.CurrentUser));
                frm.Show();

                this.friendListBox1.AddUnit(this.resourceCenter.ClientGlobalCache.CurrentUser);


                foreach (string friendID in this.resourceCenter.ClientGlobalCache.CurrentUser.GetAllFriendList()) 
                {
                    if (friendID == this.resourceCenter.ClientGlobalCache.CurrentUser.UserID)
                    {
                        continue;
                    }

                    GGUser friend = this.resourceCenter.ClientGlobalCache.GetUser(friendID);
                    if (friend != null)
                    {
                        this.friendListBox1.AddUnit(friend);
                    }
                }

                this.friendListBox1.SortAllUnit();
                this.friendListBox1.ExpandCatalog(this.friendListBox1.DefaultFriendCatalogName);

               foreach (GGGroup group in this.resourceCenter.ClientGlobalCache.GetAllGroups()) 
                {
                    this.groupListBox1.AddGroup(group);                  
                }

                //加载最近联系人
                DefaultLastWordsComputer<GGUser, GGGroup > computer = new DefaultLastWordsComputer<GGUser, GGGroup >(this.resourceCenter);
                int insertIndex = 0;
                foreach (string unitID in this.resourceCenter.ClientGlobalCache.GetRecentList())
                {
                    IUnit unit = this.resourceCenter.ClientGlobalCache.GetUnit(unitID);
                    if (unit == null)
                    {
                        continue;
                    }

                    string lastWords = computer.GetLastWords(unit);
                    this.recentListBox1.AddRecentUnit(unit, insertIndex, lastWords);  
                    ++insertIndex;
                }

                this.initialized = true;
            }
            catch (Exception ee)
            {
                GlobalResourceManager.Logger.Log(ee, "", ESBasic.Loggers.ErrorLevel.Standard);
                MessageBox.Show(ee.Message);
            }
        }
        #endregion

        #region MainForm_Shown
        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.resourceCenter.ClientGlobalCache.StartRefreshFriendInfo();
        } 
        #endregion                
             
        #region 文件传输
        #region //接收方收到文件发送 请求时 的 处理
        void fileOutter_FileRequestReceived(string projectID, string senderID, ClientType senderType, string projectName, ulong totalSize, ResumedProjectItem resumedFileItem, string comment)
        {
            string dir = null;// Comment4NDisk.ParseDirectoryPath(comment);
            if (dir != null) //表明为网盘或远程磁盘
            {
                return;
            }

            if (ESFramework.NetServer.IsServerUser(senderID) && Comment4OfflineFile.ParseUserID(comment) == null)
            {
                return;
            }

            GlobalResourceManager.UiSafeInvoker.ActionOnUI<string, string, ClientType, string, ulong, ResumedProjectItem, string>(this.do_fileOutter_FileRequestReceived, projectID, senderID, senderType, projectName, totalSize, resumedFileItem, comment);
        }

        void do_fileOutter_FileRequestReceived(string projectID, string senderID, ClientType senderType, string projectName, ulong totalSize, ResumedProjectItem resumedFileItem, string comment)
        {
            ClientType senderClientType = ClientType.Others;
            string offlineFileSenderID = Comment4OfflineFile.ParseUserID(comment ,out senderClientType);
            bool offlineFile = (offlineFileSenderID != null);
            if (offlineFile)
            {
                senderID = offlineFileSenderID;
                if(senderID == this.resourceCenter.CurrentUserID)
                {
                    if(senderClientType == this.resourceCenter.CurrentClientType)
                    {
                        return;
                    }

                    FileAssistantForm fileAssistantForm = (FileAssistantForm)this.resourceCenter.ChatFormController.GetFileAssistantForm();
                    fileAssistantForm.FileRequestReceived(projectID, offlineFile);
                    fileAssistantForm.FlashChatWindow();
                    return;
                }
            }
            FriendChatForm form = (FriendChatForm)this.resourceCenter.ChatFormController.GetForm(senderID);
            form.FileRequestReceived(projectID, offlineFile);
            form.FlashChatWindow();
        }
        #endregion

        #region 发送方收到 接收方（同意或者拒绝 接收文件）的回应时的
        void fileOutter_FileResponseReceived(ITransferingProject pro, bool agreeReceive)
        {
            TransferingProject project = pro as TransferingProject;
            if (project.Comment != null) //表示为网盘或远程磁盘
            {
                return;
            }

            GlobalResourceManager.UiSafeInvoker.ActionOnUI<TransferingProject, bool>(this.do_fileOutter_FileResponseReceived, project, agreeReceive);
        }
        void do_fileOutter_FileResponseReceived(TransferingProject project, bool agreeReceive)
        {
          

            FriendChatForm form = (FriendChatForm)this.resourceCenter.ChatFormController.GetForm(project.DestUserID);
            form.FlashChatWindow();
        }
        #endregion
        #endregion

        //跳转到官网网站
        private void skinLabel_softName_Click(object sender, EventArgs e)
        {
            CommonHelper.JumpToWebsite();
        }

        //打开QQ主菜单
        private void toolQQMenu_Click(object sender, EventArgs e)
        {
            this.skinContextMenuStrip_main.Show(skinToolStrip1, new Point(3, -2), ToolStripDropDownDirection.AboveRight);    
        }

        #region 显示用户资料
        private void chatShow_MouseEnterHead(object sender, ChatListEventArgs e)
        {
            //return;

            //ChatListSubItem item = e.MouseOnSubItem ;
            //if (item == null)
            //{
            //    item = e.SelectSubItem;
            //}

            //int top = this.Top + this.chatListBox.Top + (item.HeadRect.Y - this.chatListBox.chatVScroll.Value);
            //int left = this.Left - 279 - 5;
            //int ph = Screen.GetWorkingArea(this).Height;

            //if (top + 181 > ph)
            //{
            //    top = ph - 181 - 5;
            //}

            //if (left < 0)
            //{
            //    left = this.Right + 5;
            //}
            
            //if (this.userInformationForm == null)
            //{
            //    this.userInformationForm = new UserInformationForm(new Point(left, top));
            //}

            
            //GGUser user = (GGUser)item.Tag;          
            //this.userInformationForm.SetUser(user, item);
            //this.userInformationForm.Location = new Point(left, top);            
            //this.userInformationForm.Show();
        }

        private void chatShow_MouseLeaveHead(object sender, ChatListEventArgs e)
        {
            //return;

            //Thread.Sleep(100);
            //if (!this.userInformationForm.Bounds.Contains(Cursor.Position))
            //{
            //    this.userInformationForm.Hide();
            //}
        } 
        #endregion

        //选择状态
        private void skinButton_State_Click(object sender, EventArgs e)
        {
            this.skinContextMenuStrip_State.Show(skinButton_State, new Point(0, skinButton_State.Height), ToolStripDropDownDirection.Right);
        }

        
        //状态选择项
        private void Item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem Item = (ToolStripMenuItem)sender;
            skinButton_State.Image = Item.Image;
            skinButton_State.Tag = Item.Tag;
            this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus = (UserStatus)int.Parse(Item.Tag.ToString());   
            this.myStatus4Relogon = this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus;
            this.pnlTx.BackgroundImage = GlobalResourceManager.GetHeadImage(this.resourceCenter.ClientGlobalCache.CurrentUser ,true);
            this.twinkleNotifyIcon1.ChangeMyStatus(this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus);                    
            this.twinkleNotifyIcon1.ChangeText( String.Format("{0}：{1}（{2}）\n状态：{3}", GlobalResourceManager.SoftwareName, this.resourceCenter.ClientGlobalCache.CurrentUser.Name, this.resourceCenter.ClientGlobalCache.CurrentUser.UserID, GlobalResourceManager.GetUserStatusName(this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus)));

            this.resourceCenter.ClientOutter.ChangeMyStatus(this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus);   
        } 

        private void toolQQShow_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.Focus();
        }

        private bool gotoExit = false;
        private void toolExit_Click(object sender, EventArgs e)
        {
            this.gotoExit = true;
            this.Close();
        }

        #region MainForm_FormClosing
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!SystemSettings.Singleton.ExitWhenCloseMainForm && !this.gotoExit)
            {
                this.Visible = false;
                e.Cancel = true;
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            this.resourceCenter.ClientGlobalCache.SaveUserLocalCache(this.recentListBox1.GetRecentUserList(20));

            this.resourceCenter.ChatFormController.CloseAllForms();
            this.Visible = false;
            this.twinkleNotifyIcon1.Visible = false;
            this.resourceCenter.RapidPassiveEngine.Close();
            MultimediaManagerFactory.GetSingleton().Dispose();
            this.Cursor = Cursors.Default;
        } 
        #endregion           

        private void toolStripSplitButton3_ButtonClick(object sender, EventArgs e)
        {
            this.recentListBox1.Visible = false;
            this.friendListBox1.Visible = false;
            this.groupListBox1.Visible = true;
        }

        private void toolStripSplitButton4_ButtonClick(object sender, EventArgs e)
        {
            if (this.friendListBox1.Visible)
            {
                this.friendListBox1.CloseAllCatalog();
                return;
            }

            this.recentListBox1.Visible = false;
            this.friendListBox1.Visible = true;
            this.groupListBox1.Visible = false;
        }

        private void toolStripSplitButton2_ButtonClick(object sender, EventArgs e)
        {
            this.recentListBox1.Visible = true;
            this.friendListBox1.Visible = false;
            this.groupListBox1.Visible = false;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.recentListBox1.Visible = false;
            this.friendListBox1.Visible = false;
            this.groupListBox1.Visible = false;
        }  

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            Image img = this.pnlTx.BackgroundImage;
            byte[] buff = ESBasic.Helpers.ImageHelper.Convert(img);

            Bitmap bmp2 = new Bitmap(img.Width, img.Height);
            Graphics g = Graphics.FromImage(bmp2);
            g.DrawImage(img, new Point(0, 0));
            g.Dispose();

            byte[] buf2 = ESBasic.Helpers.SerializeHelper.SerializeObject(bmp2);

            string s1 = buff.Length.ToString();
            string s2 = buf2.Length.ToString();

            this.recentListBox1.Visible = false;
            this.friendListBox1.Visible = false;
            this.groupListBox1.Visible = false;           
            this.searchFriendPanel1.Visible = true;
        }

        private void toolStripButton19_Click(object sender, EventArgs e)
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            JoinGroupForm form = new JoinGroupForm(this.resourceCenter);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.ToJoinGroup(form.GroupID, true);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SystemSettingForm form = new SystemSettingForm();
            form.Show();
        }

        //1115
        private void ToJoinGroup(string groupID, bool active)
        {
            this.resourceCenter.ClientGlobalCache.CurrentUser.JoinGroup(groupID);
            GGGroup group = this.resourceCenter.ClientGlobalCache.GetGroup(groupID);
            this.groupListBox1.AddGroup(group);
            //string dateString = string.Format("{0}-{1}-{2}", group.CreateTime.Year.ToString("00"), group.CreateTime.Month.ToString("00"), group.CreateTime.Day.ToString("00"));
            //ChatListSubItem subItem = new ChatListSubItem(group.GroupID, "", group.Name, string.Format("{0}人 {1}", group.MemberList.Count, dateString), ChatListSubItem.UserStatus.Online, this.imageList1.Images[0]);
            //subItem.Tag = group;
            //this.chatListBox_group.Items[0].SubItems.Add(subItem);

            GroupChatForm groupChatForm = (GroupChatForm)this.resourceCenter.ChatFormController.GetForm(group.GroupID);            
            groupChatForm.OnJoinGroupSucceed();
            groupChatForm.Show();
            groupChatForm.Focus();
        }

        

        private void ToQuitGroup(GGGroup group, bool active)
        {
            this.groupListBox1.RemoveGroup(group.ID);
            GroupChatForm form = (GroupChatForm)this.resourceCenter.ChatFormController.GetExistedForm(group.GroupID);
            if (form != null)
            {
                form.Close();
            }
            this.resourceCenter.ClientGlobalCache.RemoveGroup(group.GroupID);
            this.resourceCenter.ClientGlobalCache.CurrentUser.QuitGroup(group.GroupID);
            string msg = active ? string.Format("您已经退出讨论组{0}({1})。", group.GroupID, group.Name) : string.Format("您已经被从讨论组{0}({1})中移除。", group.GroupID, group.Name);
            MessageBoxEx.Show(msg);        
        }      

        private void 清空会话列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.recentListBox1.Clear();
        }       

        private void UpdateMyInfo()
        {
            UpdateUserInfoForm form = UniqueFormManager.GetInstance.OnNewUpdateUserInfoForm(this.resourceCenter);
            form.Show();          
        }     

        private void skinPanel_HeadImage_MouseEnter(object sender, EventArgs e)
        {
            this.pnlTx.BorderStyle = BorderStyle.Fixed3D;
        }

        private void skinPanel_HeadImage_MouseLeave(object sender, EventArgs e)
        {
            this.pnlTx.BorderStyle = BorderStyle.None;
        }

       

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            SystemSettingForm form = new SystemSettingForm();
            form.Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.gotoExit = true;
            this.Close();
        }

        private void 个人资料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            this.UpdateMyInfo();
        }     

        private void 创建讨论组ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            CreateGroupForm form = new CreateGroupForm(this.resourceCenter);
            form.FormClosing += new FormClosingEventHandler(form_FormClosing);           
            form.Show();
        }

        void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            CreateGroupForm form = (CreateGroupForm)sender;
            GGGroup group = form.Group;
            if (group == null)
            {
                return;
            }
            this.resourceCenter.ClientGlobalCache.CurrentUser.JoinGroup(group.GroupID);
            this.resourceCenter.ClientGlobalCache.OnCreateGroup(group);
            this.groupListBox1.AddGroup(group);

            GroupChatForm groupChatForm = (GroupChatForm)this.resourceCenter.ChatFormController.GetForm(group.GroupID);
            groupChatForm.OnCreateGroupSucceed();
            groupChatForm.Show();
            groupChatForm.Focus();       
        }      

        private void skinButton_headImage_Click(object sender, EventArgs e)
        {
            if (this.resourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            this.UpdateMyInfo();
        }        

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);//设置此窗体为活动窗体
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (!this.initialized)
            {
                return;
            }
            this.autoDocker1.ShowOnce();
            Show();
            SetForegroundWindow(this.Handle);
        }       

        private void 小头像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.friendListBox1.IconSizeMode = ChatListItemIcon.Small;
            this.大头像ToolStripMenuItem.Checked = false;
            this.friendListBox1.Invalidate();
            SystemSettings.Singleton.ShowLargeIcon = false;
            SystemSettings.Singleton.Save();
        }

        private void 大头像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.friendListBox1.IconSizeMode = ChatListItemIcon.Large;
            this.小头像ToolStripMenuItem.Checked = false;
            this.friendListBox1.Invalidate();
            SystemSettings.Singleton.ShowLargeIcon = true;
            SystemSettings.Singleton.Save();
        }    
       
        private void FocusCurrent(object sender, EventArgs e)
        {
            this.Focus();
        }       

        #region ITwinkleNotifySupporter
        public string GetFriendName(string friendID)
        {
            GGUser user = this.resourceCenter.ClientGlobalCache.GetUser(friendID);
            return user.Name;
        }

        public string GetGroupName(string groupID)
        {
            GGGroup group = this.resourceCenter.ClientGlobalCache.GetGroup(groupID);
            return group.Name;
        }

        public void PlayAudioAsyn()
        {
            GlobalResourceManager.PlayAudioAsyn();
        }

        public Icon GetHeadIcon(string userID)
        {
            GGUser user = this.resourceCenter.ClientGlobalCache.GetUser(userID);
            return user.GetHeadIcon(GlobalResourceManager.HeadImages);
        }

        public Icon Icon64
        {
            get { return GlobalResourceManager.Icon64; }
        }

        public Icon NoneIcon64
        {
            get { return GlobalResourceManager.NoneIcon64; }
        }

        public Icon GroupIcon
        {
            get { return GlobalResourceManager.GroupIcon; }
        }

        public Icon GetStatusIcon(UserStatus status)
        {
            return GlobalResourceManager.GetStatusIcon(status);
        }

        public bool NeedTwinkle4User(string userID)
        {
            //this.ChatFormManager.OnNewMessage(userID);
            return this.resourceCenter.ChatFormController.GetExistedForm(userID) == null;
        }

        public bool NeedTwinkle4Group(string groupID)
        {
            //this.ChatFormManager.OnNewMessage(groupID);
            return this.resourceCenter.ChatFormController.GetExistedForm(groupID) == null;
        }

        public bool NeedTwinkle4AddFriendNotify()
        {
            return !this.resourceCenter.ChatFormController.IsExistNotifyForm();
        }

        public Icon NotifyIcon
        {
            get { return GlobalResourceManager.NotifyIcon; }
        }
        #endregion

        private void 创建讨论组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.创建讨论组ToolStripMenuItem1_Click(sender, e);
        }


        #region IUnitInfoProvider
        public Image GetHeadImage(IUnit unit)
        {
            if (unit.UnitType == UnitType.Group)
            {
                return this.imageList1.Images[0];
            }
            return GlobalResourceManager.GetHeadImage((GGUser)unit);
        }

        public string GetCatalog(IUnit unit)
        {
            if (unit.UnitType == UnitType.Group)
            {
                IGroup group = (IGroup)unit;
                if (group.GroupType == GroupType.CommonGroup)
                {
                    return "我的群";
                }
                if (group.GroupType == GroupType.DiscussGroup)
                {
                    return "讨论组";
                }
                return "";
            }

            return null;
        } 
        #endregion

        #region IChatSupporter
        public bool IsFriend(string friendID)
        {
            return this.friendListBox1.ContainsUser(friendID);
        }

        public bool IsInGroup(string groupID)
        {
            foreach (string gid in this.resourceCenter.ClientGlobalCache.CurrentUser.GroupList)
            {
                if (groupID == gid)
                {
                    return true;
                }
            }

            return false;
        }

        public List<ChatListSubItem> SearchChatListSubItem(string idOrName)
        {
            return this.friendListBox1.SearchChatListSubItem(idOrName);
        }
        #endregion
 

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.searchFriendPanel1.Visible = !this.searchFriendPanel1.Visible;  
        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ChangePasswordForm form = new ChangePasswordForm(this.resourceCenter);
            DialogResult result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                MessageBoxEx.Show("修改成功！");
            }
        }


        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.searchFriendPanel1.Visible = true;  
        }

        private void 文件传输助手ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileAssistantForm fileAssistantForm = (FileAssistantForm)this.resourceCenter.ChatFormController.GetFileAssistantForm();
            fileAssistantForm.Show();
        }

        private void 验证消息toolStripMenuItem_Click(object sender, EventArgs e)
        {
           NotifyForm addFriendNotifyForm=(NotifyForm) this.resourceCenter.ChatFormController.GetNotifyForm();
           addFriendNotifyForm.Show();
        }

        //点击控制台
        private void toolStripMenuItem_control_Click(object sender, EventArgs e)
        {
            ControlMainForm controlMainForm = (ControlMainForm)this.resourceCenter.ChatFormController.GetControlForm();
            controlMainForm.Show();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            NDiskForm form = new NDiskForm(this.resourceCenter.RapidPassiveEngine.FileOutter, this.nDiskOutter, this.resourceCenter.CurrentUserID);
            form.Show();
        }

        #region IChatFormShower
        public void ShowChatForm(IChatForm chatForm)
        {
            Form form = chatForm as Form;
            if (form == null) return;
            form.FormClosed += new FormClosedEventHandler(chatForm_FormClosing);
            form.Show();
            ESBasic.Helpers.WindowsHelper.SetForegroundWindow(form);
        }

        private void chatForm_FormClosing(object sender, FormClosedEventArgs e)
        {
            IChatForm chatForm = sender as IChatForm;
            if (chatForm == null) return;
            this.resourceCenter.ChatFormController.RemoveForm4Cache(chatForm.UnitID);
        }

        public void ShowControl(Control control)
        {
            if (control is Form)
            {
                control.Show();
            }
        }
        #endregion


    }

    public interface IChatSupporter
    {
        bool IsFriend(string friendID);
        bool IsInGroup(string groupID);
        List<ChatListSubItem> SearchChatListSubItem(string idOrName);
    }
}