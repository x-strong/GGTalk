using CPF;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using ESBasic;
using ESFramework;
using ESFramework.Boost.DynamicGroup.Passive;
using ESFramework.Boost.NetworkDisk.Passive;
using ESPlus.Application.Basic;
using ESPlus.Application.CustomizeInfo;
using ESPlus.Application.FileTransfering;
using ESPlus.FileTransceiver;
using ESPlus.Serialization;
using GGTalk.Linux.Controller;
using GGTalk.Linux.Controls;
using GGTalk.Linux.Helpers;
using GGTalk.Linux.Models;
using System;
using System.Threading.Tasks;
using TalkBase;
using TalkBase.Client;
using TalkBase.NetCore.Client.Core;
using UnitType = TalkBase.UnitType;

namespace GGTalk.Linux.Views
{
    internal class MainWindow : Window, ITwinkleNotifySupporter
    {

        private SearchUserOrGroupPanel searchPanel;
        private ICustomizeHandler customizeHandler;
        private NDiskPassiveHandler nDiskPassiveHandler;
        private DynamicGroupOutter dynamicGroupOutter;
        private INDiskOutter nDiskOutter;
        private Event2ChatFormBridge<GGUser, GGGroup> event2ChatFormBridge;
        private Event2ChatListBridge<GGUser, GGGroup> event2ChatListBridge;
        internal Event2ChatListBridge<GGUser, GGGroup> Event2ChatListBridge
        {
            get { return event2ChatListBridge; }
        }
        private UserStatus loginUserStatus = UserStatus.Online;
        private bool initialized = false;
        public MainWindow(ICustomizeHandler handler, NDiskPassiveHandler _nDiskPassiveHandler ,DynamicGroupOutter _dynamicGroupOutter, UserStatus _loginUserStatus)
        {
            GlobalResourceManager.SetPrimaryScreen(Screen);
            this.customizeHandler = handler;
            this.nDiskPassiveHandler = _nDiskPassiveHandler;
            this.dynamicGroupOutter = _dynamicGroupOutter;
            this.loginUserStatus = _loginUserStatus;
            this.Closing += MainWindow_Closing;
            this.Closed += MainWindow_Closed;
            this.Initialized += MainWindow_Initialized;

        }

        private void MainWindow_Initialized(object sender, EventArgs e)
        {
            this.InitializeContextMenu();
        }


        private bool gotoExit = false;
        private void MainWindow_Closing(object sender, ClosingEventArgs e)
        {
            if (!SystemSettings.Singleton.ExitWhenCloseMainForm && !this.gotoExit)
            {
                this.Visibility = Visibility.Hidden;
                e.Cancel = true;
                return;
            }

            Program.ResourceCenter.ClientGlobalCache.SaveUserLocalCache(this.recentListBox.GetRecentUserList(20));
            ChatFormController?.CloseAllForms();
            Program.PassiveEngine.Close();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (this.notifyIcon != null)
            {
                this.notifyIcon.Dispose();
            }
            System.Environment.Exit(0);
        }


        private ContextMenu mainContextMenu, pop;
        protected override void InitializeComponent()
        {
            try
            {
                MarginRight = 312;
                MarginTop = 200;
                this.Icon = GlobalResourceManager.Png64;
                //LoginWindow loginWindow = new LoginWindow(this.customizeHandler);
                //Task<object> task = loginWindow.ShowDialog(this);
                //await task.ConfigureAwait(true);
                //if (!(bool)task.Result)
                //{
                //    this.Close();
                //    return;
                //}
                this.CanResize = true;
                CommandContext = this;
                ViewFill color = "#fff";
                ViewFill hoverColor = "255,255,255,40";
                Title = GlobalResourceManager.SoftwareName;
                Width = 274;
                Height = 698;
                MinWidth = 274;
                MinHeight = 698;
                Background = null;
                Children.Add(new Grid
                {
                    BorderFill = "97,159,215",
                    BorderThickness = new Thickness(2, 2, 2, 2),
                    BorderType = BorderType.BorderThickness,
                    ClipToBounds = true,
                    Background = "#fff",
                    Size = SizeField.Fill,
                    RowDefinitions =
                    {
                        new RowDefinition{Height = 130},
                        new RowDefinition{},
                        new RowDefinition{Height = 30},
                    },
                    Children = //内容元素放这里
                    {

                        new StackPanel
                        { 
                            Background = "#619fd7",
                            Size = SizeField.Fill,
                            Attacheds =
                            {
                                {
                                    Grid.RowIndex,
                                    0
                                }, 
                            },
                            Orientation = Orientation.Vertical,
                            Children =
                            {
                                new Panel()
                                { 
                                    Width= "100%",
                                    Height =30,
                                    Children={
                                        new TextBlock
                                        {
                                            Foreground = "#fff",
                                            FontSize = 12,
                                            MarginLeft = 10,
                                            MarginTop = 10,
                                            Text = GlobalResourceManager.SoftwareName,
                                            Cursor = Cursors.Hand,
                                            Commands={ { nameof(MouseDown),(s,e)=> {
                                                try
                                                {
                                                    CommonHelper.OpenUrl4Browser(GlobalResourceManager.CompanyUrl);
                                                }
                                                catch (Exception ee)
                                                {
                                                    GlobalResourceManager.WriteErrorLog(ee);
                                                }
                                            } } }
                                        },
                                        new Panel
                                        {
                                            MarginRight = 28f,
                                            MarginTop = 0f,
                                            ToolTip="最小化",
                                            Name="min",
                                            Width = 30f,
                                            Height = 30f,
                                            Children =
                                            {
                                                new Line
                                                {
                                                    MarginLeft=8,
                                                    MarginTop=2,
                                                    StartPoint = new Point(1, 13),
                                                    EndPoint = new Point(14, 13),
                                                    StrokeStyle = "2",
                                                    IsAntiAlias=true,
                                                    StrokeFill=color
                                                },
                                            },
                                            Commands =
                                            {
                                                {
                                                    nameof(Button.MouseUp),
                                                    (s,e)=>
                                                    {
                                                        (e as MouseButtonEventArgs).Handled = true;
                                                        this.WindowState = WindowState.Minimized;
                                                    }
                                                }
                                            },
                                            Triggers=
                                            {
                                                new Trigger(nameof(Panel.IsMouseOver), Relation.Me)
                                                {
                                                    Setters =
                                                    {
                                                        {
                                                            nameof(Panel.Background),
                                                            hoverColor
                                                        }
                                                    }
                                                }
                                            },
                                        },
                                        new Panel
                                        {
                                            MarginRight = 0f,
                                            MarginTop = 0f,
                                            Name = "close",
                                            ToolTip = "关闭",
                                            Width = 30f,
                                            Height = 30f,
                                            Children =
                                            {
                                                new Line
                                                {
                                                    MarginTop=8,
                                                    MarginLeft=8,
                                                    StartPoint = new Point(1, 1),
                                                    EndPoint = new Point(14, 13),
                                                    StrokeStyle = "2",
                                                    IsAntiAlias=true,
                                                    StrokeFill=color
                                                },
                                                new Line
                                                {
                                                    MarginTop=8,
                                                    MarginLeft=8,
                                                    StartPoint = new Point(14, 1),
                                                    EndPoint = new Point(1, 13),
                                                    StrokeStyle = "2",
                                                    IsAntiAlias=true,
                                                    StrokeFill=color
                                                }
                                            },
                                            Commands =
                                            {
                                                {
                                                    nameof(Button.MouseUp),
                                                    async (s,e)=>
                                                    {
                                                        (e as MouseButtonEventArgs).Handled=true;
                                                        if(!GlobalResourceManager.IsWindowsOS)
                                                        {
                                                           Task<ButtonResult>  res= MessageBoxEx.ShowDialogQuery(this,"你确定要退出程序吗？");
                                                            await res.ConfigureAwait(true);
                                                            if(res.Result==ButtonResult.Yes)
                                                            {
                                                                this.gotoExit=true;
                                                                this.Close();
                                                            }
                                                            return;
                                                        }
                                                        this.Close();
                                                    }
                                                }
                                            },
                                            Triggers =
                                            {
                                                new Trigger(nameof(Panel.IsMouseOver), Relation.Me)
                                                {
                                                    Setters =
                                                    {
                                                        {
                                                            nameof(Panel.Background),
                                                            hoverColor
                                                        }
                                                    }
                                                }
                                            },
                                        },
                                    },
                                    //设置窗体拖拽
                                    Commands =
                                    {
                                        {
                                            nameof(MouseDown),
                                            (s,e)=>this.DragMove()
                                        }
                                    }
                                }, 

                                new StackPanel
                                { 
                                    Width= "100%",
                                    Orientation = Orientation.Horizontal,
                                    Children =
                                    {
                                        new Ellipse
                                        { 
                                            Name = nameof(headPic),
                                            ToolTip="账号 ：10001", 
                                            Height =78,
                                            Width = 78,
                                            PresenterFor=this,
                                            MarginTop = 8,
                                            MarginLeft = 6,
                                            StrokeFill = null,
                                            Fill = new TextureFill() {Image=CommonOptions.ResourcesCatalog+ "8.png",  Stretch = Stretch.Fill   }, 
                                            Commands={ { nameof(MouseDown), (s, e) => UpdateUserInfo() } },
                                            Bindings={ { nameof(Picture.ToolTip),nameof(GGUserPlus.ID), null, BindingMode.OneWay, x => "账号 ：" + x  } }
                                        },
                                        new StackPanel
                                        {
                                            Size = SizeField.Fill,
                                            Orientation = Orientation.Vertical,
                                            Children =
                                            { 
                                                new TextBlock
                                                {
                                                    Name = nameof(tb_NickName),
                                                    PresenterFor=this,
                                                    Foreground = "#fff",
                                                    FontSize = 14,
                                                    FontStyle = FontStyles.Bold,
                                                    MaxWidth = 150,
                                                    TextTrimming = TextTrimming.CharacterEllipsis,
                                                    MarginLeft = 10,
                                                    MarginTop = 20,
                                                    Text = "傲瑞客服",
                                                    Bindings={ { nameof(TextBlock.Text),nameof(GGUserPlus.Name)} }
                                                },
                                                new StackPanel
                                                {
                                                    Width= "100%", 
                                                    MarginTop = 10,
                                                    Orientation = Orientation.Horizontal,
                                                    Children =
                                                    { 
                                                        new Panel
                                                        {
                                                            Name = "State",
                                                            PresenterFor=this,
                                                            Height = 22,
                                                            Width = 22,
                                                            Background = Color.Transparent, 
                                                            Children =
                                                            {
                                                                new Picture
                                                                {
                                                                    Name ="statusIco",
                                                                    PresenterFor=this,
                                                                    Stretch = Stretch.Fill,
                                                                    Source=CommonOptions.ResourcesCatalog+ "0.png",
                                                                    Bindings={ { nameof(Picture.Source),nameof(GGUserPlus.UserStatus),null,BindingMode.OneWay, a => { return CommonHelper.GetUserStatusIco((UserStatus)a); } }},
                                                                    Commands={ { nameof(MouseDown),(s,e)=> {
                                                                        this.pop.PlacementTarget = (UIElement)s;
                                                                        this.pop.IsOpen = true; } } }
                                                                }
                                                            }
                                                        },
                                                        new TextBlock
                                                        {
                                                            Name=nameof(tb_Signature),
                                                            PresenterFor=this,
                                                            MarginLeft = 20,
                                                            Foreground = "#fff",
                                                            FontSize = 12,
                                                            Text = "欢迎质询 !",
                                                            MaxWidth = 130,
                                                            TextTrimming = TextTrimming.CharacterEllipsis,
                                                            Bindings={ { nameof(TextBlock.Text),nameof(GGUserPlus.Signature)} }
                                                        },
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        },

                        new Grid
                        {  
                            Size = SizeField.Fill,
                            Attacheds =
                            {
                                {
                                    Grid.RowIndex,
                                    1
                                },
                            },
                            Children  =
                            {
                                new TabControl
                                {  
                                    Size = SizeField.Fill,
                                    Items =
                                    {
                                        new MainTabItem
                                        {
                                            Header="好友",
                                            ToolTip="好友",  
                                            Bindings =
                                            {
                                                {
                                                    nameof(Width),
                                                    nameof(ActualSize),
                                                    1,
                                                    BindingMode.OneWay,
                                                    (Size a)=>a.Width/3
                                                }
                                            }, 
                                            Height = 30,
                                            PictureSource =CommonOptions.ResourcesCatalog+"person.png",
                                            Content= new FriendListBox()
                                            {
                                                Name=nameof(friendListBox),
                                                PresenterFor=this,
                                                Size = SizeField.Fill 
                                            }
                                        },
                                        new MainTabItem
                                        {
                                            Header="讨论组",
                                            ToolTip="讨论组",
                                            Bindings =
                                            {
                                                {
                                                    nameof(Width),
                                                    nameof(ActualSize),
                                                    1,
                                                    BindingMode.OneWay,
                                                    (Size a)=>a.Width/3
                                                }
                                            },
                                            Height = 30,
                                            PictureSource =CommonOptions.ResourcesCatalog+"Group2.png",
                                            Content=new GroupListBox(){
                                                Name=nameof(this.groupListBox),
                                                PresenterFor=this,
                                                Size = SizeField.Fill
                                            }
                                        },
                                        new MainTabItem
                                        {
                                            Name=nameof(this.recentTabItem),
                                            PresenterFor=this,
                                            Header="会话",
                                            ToolTip="会话",
                                            Bindings =
                                            {
                                                {
                                                    nameof(Width),
                                                    nameof(ActualSize),
                                                    1,
                                                    BindingMode.OneWay,
                                                    (Size a)=>a.Width/3
                                                }
                                            },
                                            Height = 30,
                                            PictureSource =CommonOptions.ResourcesCatalog+"comment.png",
                                            Content= new RecentListBox(){
                                                Name=nameof(this.recentListBox),
                                                PresenterFor=this,
                                                Size = SizeField.Fill
                                            }
                                        }
                                    }
                                },
                                //搜索联系人
                                new SearchUserOrGroupPanel
                                {
                                    PresenterFor = this,
                                    Name = nameof(searchPanel),
                                    Visibility= Visibility.Collapsed,
                                    Size = SizeField.Fill
                                },
                                new Border
                                {
                                    BorderFill = "#619fd7",
                                    Background = "#619fd7",
                                    MarginTop = 0,
                                    Height = 1,
                                    Width = "100%",
                                },
                                new Border
                                {
                                    BorderFill = "#619fd7",
                                    Background = "#619fd7",
                                    MarginTop = 0,
                                    MarginLeft = 0,
                                    Height = 30,
                                    Width = "1",
                                },
                                new Border
                                {
                                    BorderFill = "#619fd7",
                                    Background = "#619fd7",
                                    MarginTop = 0,
                                    MarginRight = 0,
                                    Height = 30,
                                    Width = "1",
                                }
                            }
                        },

                        new Panel
                        { 
                            Background = "#619fd7",
                            Size = SizeField.Fill,
                            Attacheds =
                            {
                                {
                                    Grid.RowIndex,
                                    2
                                },
                            },
                            Children  =
                            {
                                new Button
                                {
                                    ToolTip="主菜单",
                                    Classes  = "BlueButton",
                                    MarginLeft = 0f,
                                    MarginTop = 1f,
                                    Width = 26,
                                    Height = 26,
                                    Background = Color.Transparent,
                                    Content = new Picture
                                    {
                                        Stretch = Stretch.Fill,
                                        Source = CommonOptions.ResourcesCatalog+ "three.png",
                                    },
                                    Commands={ { nameof(Button.Click),(s,e)=> {
                                                    this.mainContextMenu.PlacementTarget = (UIElement)s;
                                                    this.mainContextMenu.IsOpen = true;
                                                }
                                        }
                                    }
                                },
                                new Button
                                {
                                    ToolTip="打开系统设置",
                                    MarginLeft = 30f,
                                    MarginTop = 1f,
                                    Width = 26,
                                    Classes = "BlueButton",
                                    Height = 26,
                                    Commands =
                                    {
                                        {
                                            nameof(Button.Click),
                                            (s,e)=>
                                            {
                                                SystemSettingWindow systemSettingWindow = new SystemSettingWindow();
                                                systemSettingWindow.Show_Topmost();
                                            }
                                        }
                                    },
                                    Background = Color.Transparent,
                                    Content = new Picture
                                    {
                                        Stretch = Stretch.Fill,
                                        Source = CommonOptions.ResourcesCatalog+ "setup.png",
                                    },
                                },
                                new Button
                                {
                                    ToolTip="查找联系人或群组",
                                    MarginLeft = 60f,
                                    MarginTop = 1f,
                                    Width = 26,
                                    Classes = "BlueButton",
                                    Height = 26,
                                    Background = Color.Transparent,
                                    Content = new Picture
                                    {
                                        MarginLeft = 1,
                                        Stretch = Stretch.Fill,
                                        Source = CommonOptions.ResourcesCatalog+ "search.png",
                                    },
                                    Commands =
                                    {
                                        {
                                            nameof(Button.Click),
                                            nameof(this.ClickSearchUserOrGroup)
                                        }
                                    }
                                },
                                new Button
                                {
                                    Visibility=Visibility.Visible,
                                    ToolTip="PC在线",
                                    MarginRight = 0,
                                    MarginTop = 2f,
                                    Width = 26,
                                    Height = 26,
                                    Classes = "BlueButton",
                                    Content = new Picture
                                    {
                                        Stretch = Stretch.Fill,
                                        Width = 20,
                                        Height = 20,
                                        Source = CommonOptions.ResourcesCatalog+ "win1.png",
                                    },
                                    Commands =
                                    {
                                        {
                                            nameof(Button.Click),
                                            nameof(this.ClickJump)
                                        }
                                    }
                                },
                            }
                        },

                    }
                });
                Cursor = Cursors.Wait;
                LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));

                //加载样式文件，文件需要设置为内嵌资源

                if (!DesignMode)//设计模式下不执行
                {
                    this.SetComponent();
                    
                    this.Initialize(this.loginUserStatus);
                }
            }
            catch (Exception ee)
            {
                GlobalResourceManager.WriteErrorLog(ee);
            }
  
        }

        private async void InitializeContextMenu()
        {

            //切换登录状态的ContextMenu
            this.pop = new ContextMenu
            {
                Placement = PlacementMode.Mouse,
                Items = new UIElement[]
                {
                    new MenuItem
                    {
                        Icon = GlobalResourceManager.GetStatusImage(UserStatus.Online),
                        Header = "我在线上",
                            Commands={
                            {
                                nameof(MouseDown),
                                (s,e)=> MainWindow_UserStatusChanged(UserStatus.Online)
                            }
                        }
                    },
                    new MenuItem
                    {
                        Icon = GlobalResourceManager.GetStatusImage(UserStatus.Away),
                        Header = "离开",
                            Commands={
                            {
                                nameof(MouseDown),
                                (s,e)=> MainWindow_UserStatusChanged(UserStatus.Away)
                            }
                        }
                    },
                    new MenuItem
                    {
                        Icon = GlobalResourceManager.GetStatusImage(UserStatus.Busy),
                        Header = "忙碌",
                            Commands={
                            {
                                nameof(MouseDown),
                                (s,e)=> MainWindow_UserStatusChanged(UserStatus.Busy)
                            }
                        }
                    },
                    new MenuItem
                    {
                        Icon = GlobalResourceManager.GetStatusImage(UserStatus.DontDisturb),
                        Header = "请勿打扰",
                            Commands={
                            {
                                nameof(MouseDown),
                                (s,e)=> MainWindow_UserStatusChanged(UserStatus.DontDisturb)
                            }
                        }
                    },
                    new MenuItem
                    {
                        Icon = GlobalResourceManager.GetStatusImage(UserStatus.Hide),
                        Header = "隐身",
                            Commands={
                            {
                                nameof(MouseDown),
                                (s,e)=> MainWindow_UserStatusChanged(UserStatus.Hide)
                            }
                        }
                    },
                },
            };
            this.mainContextMenu = new ContextMenu
            {
                Placement = PlacementMode.Margin,
                PopupMarginLeft = 20,
                //PopupMarginTop = -100,
                PopupMarginBottm = 20,
                Items = new UIElement[]
                {
                    new MenuItem
                    {
                        Icon = await ResourceManager.GetImage(CommonOptions.ResourcesCatalog+ "finduser_icon.png"),
                        Header = "查找用户",
                            Commands={
                            {
                                nameof(MouseDown),
                                (s,e)=> SearchUser_Click()
                            }
                        }
                    },
                    new MenuItem
                    {
                        Icon = await ResourceManager.GetImage(CommonOptions.ResourcesCatalog+ "creategroup_icon.png"),
                        Header = "创建讨论组（群）",
                            Commands={
                            {
                                nameof(MouseDown),
                                (s,e)=> CreateGroup_Click()
                            }
                        }
                    },
                    new MenuItem
                    {
                        Icon = await ResourceManager.GetImage(CommonOptions.ResourcesCatalog+ "personaldata_icon.png"),
                        Header = "个人资料",
                            Commands={
                            {
                                nameof(MouseDown),
                                (s,e)=> UpdateUserInfo()
                            }
                        }
                    },
                    new MenuItem
                    {
                        Icon = await ResourceManager.GetImage(CommonOptions.ResourcesCatalog+ "Password_icon.png"),
                        Header = "修改密码",
                            Commands={
                            {
                                nameof(MouseDown),
                                (s,e)=> UpdatePassword_Click()
                            }
                        }
                    },
                    new MenuItem
                    {
                        Icon = await ResourceManager.GetImage(CommonOptions.ResourcesCatalog+ "transmission_icon.png"),
                        Header = "文件传输助手",
                            Commands={
                            {
                                nameof(MouseDown),
                                (s,e)=> FileAssistant_Click()
                            }
                        }
                    },
                    new MenuItem
                    {
                        Icon = await ResourceManager.GetImage(CommonOptions.ResourcesCatalog+ "networkdisk.png"),
                        Header = "我的网盘",
                            Commands={
                            {
                                nameof(MouseDown),
                                (s,e)=> NetworkDisk_Click()
                            }
                        }
                    },
                    new MenuItem
                    {
                        Icon = await ResourceManager.GetImage(CommonOptions.ResourcesCatalog+ "verification_icon.png"),
                        Header = "验证消息",
                            Commands={
                            {
                                nameof(MouseDown),
                                (s,e)=> VerifyMessage_Click()
                            }
                        }
                    },
                    new MenuItem
                    {
                        Icon = await ResourceManager.GetImage(CommonOptions.ResourcesCatalog+ "signout_icon.png"),
                        Header = "退出",
                            Commands={
                            {
                                nameof(MouseDown),
                                (s,e)=> Quit_Click()
                            }
                        }
                    },
                },
            };
            if (this.notifyIcon != null)
            {
                this.notifyIcon.ContextMenu = this.mainContextMenu;
            }
        }



        private Picture statusIco;
        private Ellipse headPic;
        private TextBlock tb_NickName, tb_Signature;
        private void SetComponent()
        {            
            this.statusIco = this.FindPresenterByName<Picture>(nameof(statusIco));
            this.headPic = this.FindPresenterByName<Ellipse>(nameof(headPic));
            this.tb_NickName = this.FindPresenterByName<TextBlock>(nameof(tb_NickName));
            this.tb_Signature = this.FindPresenterByName<TextBlock>(nameof(tb_Signature));
            this.searchPanel = FindPresenterByName<SearchUserOrGroupPanel>(nameof(searchPanel));
            this.recentTabItem = this.FindPresenterByName<MainTabItem>(nameof(recentTabItem));
        }

        private static IChatFormController chatFormController;
        public static IChatFormController ChatFormController { get { return chatFormController; } }

        private static IMessageCacheManager messageCacheManager;
        public static IMessageCacheManager MessageCacheManager { get { return messageCacheManager; } }

        private CPF.Controls.NotifyIcon notifyIcon;
        private void Initialize(UserStatus userStatus)
        {
            string sqliteFilePath = SystemSettings.SystemSettingsDir + Program.PassiveEngine.CurrentUserID + ".db";
            string globalCacheFilePath = SystemSettings.SystemSettingsDir + Program.PassiveEngine.CurrentUserID + ".dat";
            if (GlobalResourceManager.IsWindowsOS)
            {
                TwinkleNotifyIcon twinkleNotifyIcon = new TwinkleNotifyIcon();
                twinkleNotifyIcon.NotifyIcon.Click += NotifyIcon_MouseClick;
                this.notifyIcon = twinkleNotifyIcon.NotifyIcon;
                messageCacheManager = twinkleNotifyIcon;
            }
            else
            {
                messageCacheManager = new MessageCacheManager();
            }
            Program.ResourceCenter.Initialize(Program.PassiveEngine, new TalkBaseHelper(), new TalkBaseInfoTypes(1000), globalCacheFilePath, sqliteFilePath, GlobalResourceManager.Logger, GlobalResourceManager.RemotingService, messageCacheManager);
            Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus = userStatus;
            Program.ResourceCenter.ClientGlobalCache.UserDeleted += ClientGlobalCache_UserDeleted;
            Program.ResourceCenter.ClientGlobalCache.FriendRemoved += ClientGlobalCache_FriendRemoved;
            Program.ResourceCenter.ClientGlobalCache.GroupChanged += ClientGlobalCache_GroupChanged;
            messageCacheManager.Initialize(this, (IBrige4ClientOutter)Program.ResourceCenter.ClientOutter, Program.ResourceCenter.TalkBaseInfoTypes);
            messageCacheManager.UnhandleMessageOccured += MessageCacheManager_UnhandleMessageOccured;
            messageCacheManager.UnhandleMessagePickedOut += MessageCacheManager_UnhandleMessagePickedOut;
            messageCacheManager.UnhandleNotifyMessageOccured += MessageCacheManager_UnhandleNotifyMessageOccured;
            messageCacheManager.UnhandleNotifyMessagePickedOut += MessageCacheManager_UnhandleNotifyMessagePickedOut;
            Program.ResourceCenter.ClientGlobalCache.BatchLoadCompleted += ClientGlobalCache_BatchLoadCompleted;
            Program.ResourceCenter.ClientGlobalCache.MyBaseInfoChanged += new CbGeneric(ClientGlobalCache_MyInfoChanged);
            Program.ResourceCenter.ClientGlobalCache.MyStatusChanged += ClientGlobalCache_MyStatusChanged;
            Program.ResourceCenter.ClientGlobalCache.StartRefreshFriendInfo();
            chatFormController = new SeperateController(Program.ResourceCenter);
            chatFormController.FormCreated += ChatFormController_FormCreated;
            this.event2ChatFormBridge = new Event2ChatFormBridge<GGUser, GGGroup>(Program.ResourceCenter);

            ////文件传送
            Program.PassiveEngine.FileOutter.FileRequestReceived += new CbFileRequestReceived(fileOutter_FileRequestReceived);
            Program.PassiveEngine.FileOutter.FileResponseReceived += new CbGeneric<ITransferingProject, bool>(fileOutter_FileResponseReceived);

            Program.PassiveEngine.ConnectionInterrupted += new CbGeneric(rapidPassiveEngine_ConnectionInterrupted);//预订断线事件
            Program.PassiveEngine.BasicOutter.BeingPushedOut += new CbGeneric(BasicOutter_BeingPushedOut);
            Program.PassiveEngine.BasicOutter.BeingKickedOut += new CbGeneric(BasicOutter_BeingKickedOut);
            Program.PassiveEngine.BasicOutter.MyDeviceOffline += new CbGeneric<ClientType>(BasicOutter_MyDeviceOffline);
            Program.PassiveEngine.BasicOutter.MyDeviceOnline += new CbGeneric<ClientType>(BasicOutter_MyDeviceOnline);
            Program.PassiveEngine.RelogonCompleted += new CbGeneric<LogonResponse>(rapidPassiveEngine_RelogonCompleted);//预订重连成功事件       

            ////网盘访问器
            this.nDiskOutter = new NDiskOutter(Program.ResourceCenter.RapidPassiveEngine.FileOutter, Program.ResourceCenter.RapidPassiveEngine.CustomizeOutter);

            this.nDiskPassiveHandler.Initialize(Program.PassiveEngine.FileOutter, null);
            this.dynamicGroupOutter.Initialize(Program.PassiveEngine.CurrentUserID);
            this.BindSource();            
        }

        private void NotifyIcon_MouseClick(object sender, EventArgs e)
        {
            if (!this.initialized)
            {
                return;
            }
            this.Visibility = Visibility.Visible;
           // Show();
        }

        private void BindSource()
        {
            GGUserPlus ggUserPlus = new GGUserPlus(Program.ResourceCenter.ClientGlobalCache.CurrentUser);
            this.DataContext = ggUserPlus;
            this.headPic.Fill = new TextureFill() { Image = ggUserPlus.HeadImage, Stretch = Stretch.Fill };
        }

        private void ClientGlobalCache_MyInfoChanged()
        {
           this.BindSource();
           messageCacheManager.ChangeText(String.Format("{0}：{1}（{2}）\n状态：{3}", GlobalResourceManager.SoftwareName, Program.ResourceCenter.ClientGlobalCache.CurrentUser.Name, Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserID, GlobalResourceManager.GetUserStatusName(Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus)));
        }

        private void ClientGlobalCache_MyStatusChanged()
        {
            this.loginUserStatus = Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus;
            this.statusIco.Source = CommonHelper.GetUserStatusIco(Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus);
            messageCacheManager.ChangeText(String.Format("{0}：{1}（{2}）\n状态：{3}", GlobalResourceManager.SoftwareName, Program.ResourceCenter.ClientGlobalCache.CurrentUser.Name, Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserID, GlobalResourceManager.GetUserStatusName(Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus)));

            // messageCacheManager.ChangeMyStatus(Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus);
        }

        private void ClientGlobalCache_BatchLoadCompleted()
        {
            UiSafeInvoker.ActionOnUI(this.InitView);
            GlobalResourceManager.Logger.LogWithTime("开始请求离线消息");
            //请求离线消息 
            Program.ResourceCenter.ClientOutter.RequestOfflineMessage();
            //请求离线文件
            Program.ResourceCenter.ClientOutter.RequestOfflineFile();
            //正式通知好友，自己上线
            Program.ResourceCenter.ClientOutter.ChangeMyStatus(Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus);
        }

        #region InitView
        private void InitView()
        {
            if (this.initialized) { return; }
            this.InitFriendListView();
            this.InitGroupListView();
            this.InitRecentListView();
            this.event2ChatListBridge = new Event2ChatListBridge<GGUser, GGGroup>(Program.ResourceCenter, this.friendListBox, this.groupListBox, this.recentListBox);
            this.searchPanel.Initialize(this);
            this.initialized = true;
            Cursor = Cursors.Arrow;
        }

        FriendListBox friendListBox;
        GroupListBox groupListBox;
        internal RecentListBox recentListBox;
        MainTabItem recentTabItem;

        private void InitFriendListView()
        {
            this.friendListBox = this.FindPresenterByName<FriendListBox>(nameof(this.friendListBox));
            this.friendListBox.Initialize();
        }


        private void InitGroupListView()
        {
            this.groupListBox = this.FindPresenterByName<GroupListBox>(nameof(this.groupListBox));
            this.groupListBox.Initialize();

        }

        private void InitRecentListView()
        {
            this.recentListBox = this.FindPresenterByName<RecentListBox>("recentListBox");
            recentListBox.Initialize();
            recentListBox.HasNewMsgEvent += RecentListBox_HasNewMsgEvent;
        }

        private void RecentListBox_HasNewMsgEvent(bool hasNewMsg)
        {
            string sourceImg= hasNewMsg ?  "comment2.png" : "comment.png";
            this.recentTabItem.SetPictureSource(CommonOptions.ResourcesCatalog+ sourceImg);
        }

        #endregion

        private void rapidPassiveEngine_RelogonCompleted(LogonResponse logonResponse)
        {
            if (logonResponse.LogonResult != LogonResult.Succeed)
            {
                MessageBoxEx.Show(GlobalResourceManager.SoftwareName, "自动重登录失败，可能是密码已经被修改。请重启程序，手动登录！");
                return;
            }
            Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus = this.loginUserStatus;
            Program.ResourceCenter.ClientGlobalCache.StartRefreshFriendInfo();
            MainWindow_UserStatusChanged(this.loginUserStatus);
        }

        private void ClientGlobalCache_FriendRemoved(string friendID)
        {
            messageCacheManager.RemoveUnhanleMessage(Program.ResourceCenter.ClientGlobalCache.GetUser(friendID));
        }

        private void ClientGlobalCache_GroupChanged(GGGroup ggGroup, GroupChangedType type, string operatorID, string target)
        {
            if (type == GroupChangedType.GroupDeleted)
            {
                messageCacheManager.RemoveUnhanleMessage(ggGroup);
            }
        }

        private void ClientGlobalCache_UserDeleted(GGUser ggUser)
        {
            messageCacheManager.RemoveUnhanleMessage(ggUser);
        }

        #region ChatFormController相关
        private void ChatFormController_FormCreated(Window form, IUnit unit)
        {
            if (GlobalResourceManager.IsWindowsOS)//winows 双击托盘会提取新消息，这里只需移除缓存中的新消息
            {
                //messageCacheManager.RemoveUnhanleMessage(unit);
                this.MessageCacheManager_UnhandleMessagePickedOut(unit.ID, unit.UnitType);
                if (unit.UnitType == UnitType.User)
                {

                }
                else if (unit.UnitType == UnitType.Group)
                {
                    GroupChatWindow groupChatForm = form as GroupChatWindow;
                    if (groupChatForm != null)
                    {
                        groupChatForm.ExitGroupClicked += new Action<string>(groupChatForm_ExitGroupClicked);
                        groupChatForm.DeleteGroupClicked += GroupChatForm_DeleteGroupClicked;
                    }
                }
            }
            else
            {
                if (unit.UnitType == UnitType.User)
                {
                    messageCacheManager.PickoutFriendMessage(unit.ID);
                }
                else if (unit.UnitType == UnitType.Group)
                {
                    messageCacheManager.PickoutGroupMessage(unit.ID);
                    GroupChatWindow groupChatForm = form as GroupChatWindow;
                    if (groupChatForm != null)
                    {
                        groupChatForm.ExitGroupClicked += new Action<string>(groupChatForm_ExitGroupClicked);
                        groupChatForm.DeleteGroupClicked += GroupChatForm_DeleteGroupClicked;
                    }
                }
            }
        }

        private async void GroupChatForm_DeleteGroupClicked(IGroup group)
        {
            try
            {
                if (Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
                {
                    return;
                }

                if (group.CreatorID != Program.ResourceCenter.CurrentUserID)
                {
                    MessageBoxEx.Show("只有创始人才能解散。");
                    return;
                }
                Task<ButtonResult> task = MessageBoxEx.ShowDialogQuery(this, string.Format("您确定要解散{0}({1})吗？", group.ID, group.Name));
                await task.ConfigureAwait(true);
                if (task.Result != ButtonResult.Yes)
                {
                    return;
                }
                Program.ResourceCenter.ClientOutter.DeleteGroup(group.ID);
                this.groupListBox.RemoveGroup(group.ID);
                this.recentListBox.RemoveUnit(group.ID);
                ChatFormController.CloseForm(group.ID);
                MessageBoxEx.Show(string.Format("您已经解散{0}({1})。", group.ID, group.Name));
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show("请求超时！" + ee.Message);
            }
        }

        private async void groupChatForm_ExitGroupClicked(string groupID)
        {
            try
            {
                if (Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
                {
                    return;
                }
                GGGroup group = Program.ResourceCenter.ClientGlobalCache.GetGroup(groupID,true);
                if (group == null || group.MemberList.Count == 0 || !group.MemberList.Contains(Program.ResourceCenter.CurrentUserID))
                {
                    return;
                }

                if (group.CreatorID == Program.ResourceCenter.CurrentUserID)
                {
                    MessageBoxEx.Show("创始人不能退出。");
                    return;
                }
                Task<ButtonResult> task = MessageBoxEx.ShowDialogQuery(this, string.Format("您确定要退出{0}({1})吗？", group.ID, group.Name));
                await task.ConfigureAwait(true);
                if (task.Result != ButtonResult.Yes)
                {
                    return;
                }
                Program.ResourceCenter.ClientOutter.QuitGroup(group.ID);
                this.groupListBox.RemoveGroup(group.ID);
                this.recentListBox.RemoveUnit(group.ID);
                MainWindow.ChatFormController.CloseForm(group.ID);
                MessageBoxEx.Show(string.Format("您已经退出{0}({1})。", group.ID, group.Name));
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show("请求超时！" + ee.Message);
            }
        } 
        #endregion

        #region MessageCacheManager 相关
        private void MessageCacheManager_UnhandleNotifyMessagePickedOut()
        {
            UiSafeInvoker.ActionOnUI<bool, string, TalkBase.UnitType?>(this.SetNewNotifyMessageIco, false, string.Empty,null);
        }

        private void MessageCacheManager_UnhandleNotifyMessageOccured(string userID, int notifyType, byte[] info)
        {
            UnitType unitType;
            string msgDescribe = this.GetNotifyMsgDescribe(userID, notifyType, info,out unitType);
            UiSafeInvoker.ActionOnUI<bool, string, TalkBase.UnitType?>(this.SetNewNotifyMessageIco, true, msgDescribe,unitType);
        }

        private void MessageCacheManager_UnhandleMessagePickedOut(string unitID, UnitType unitType)
        {
            if (unitType == UnitType.Group)
            {
                GlobalResourceManager.Logger.LogWithTime(string.Format("消息被提出，unitID:{0},unitType:{1}", unitID, unitType));
            }

            UiSafeInvoker.ActionOnUI<string, bool>(this.SetNewMessageIco, unitID, false);
        }

        private void MessageCacheManager_UnhandleMessageOccured(string unitID, TalkBase.UnitType unitType)
        {
            if (unitType == UnitType.Group) {
            
            GlobalResourceManager.Logger.LogWithTime(string.Format("有新的消息收到，unitID:{0},unitType:{1}", unitID, unitType));
            }
            UiSafeInvoker.ActionOnUI<string, bool>(this.SetNewMessageIco, unitID, true);
        }

        private void SetNewMessageIco(string unitID, bool isNewMsg)
        {
            IUnit unit = Program.ResourceCenter.ClientGlobalCache.GetUnit(unitID);
            this.recentListBox.SetNewMessageIco(unit, isNewMsg);
        }

        private void SetNewNotifyMessageIco(bool isNewMsg, string msgDescribe,TalkBase.UnitType? unitType)        {
            this.recentListBox.SetNewNotifyMessage(isNewMsg, msgDescribe, unitType);
        }

        /// <summary>
        /// 获取通知消息的描述
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="notifyType"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private string GetNotifyMsgDescribe(string userID, int notifyType, byte[] info,out TalkBase.UnitType unitType)
        {
            unitType = UnitType.User;
            string msgDescribe = "";
            string userName = Program.ResourceCenter.ClientGlobalCache.GetUserName(userID);
            if (notifyType == Program.ResourceCenter.TalkBaseInfoTypes.RequestAddFriend)
            {
                msgDescribe = string.Format("{0}申请加你为好友", userName);
            }
            else if (notifyType == Program.ResourceCenter.TalkBaseInfoTypes.HandleAddFriendRequest)
            {
                HandleAddFriendRequestContract contract = CompactPropertySerializer.Default.Deserialize<HandleAddFriendRequestContract>(info, 0);
                msgDescribe = string.Format("{0}{1}你的好友申请", userName, contract.IsAgreed ? "同意了" : "拒绝了");
            }
            else if (notifyType == Program.ResourceCenter.TalkBaseInfoTypes.RequestAddGroup)
            {
                RequestAddGroupContract contract = CompactPropertySerializer.Default.Deserialize<RequestAddGroupContract>(info, 0);
                GGGroup ggGroup = Program.ResourceCenter.ClientGlobalCache.GetGroup(contract.GroupID);
                msgDescribe = string.Format("{0}申请加入群{1}", userName, ggGroup.DisplayName);
                unitType = UnitType.Group;
            }
            else if (notifyType == Program.ResourceCenter.TalkBaseInfoTypes.HandleAddGroupRequest)
            {
                HandleAddGroupRequestContract contract = CompactPropertySerializer.Default.Deserialize<HandleAddGroupRequestContract>(info, 0);
                GGGroup ggGroup = Program.ResourceCenter.ClientGlobalCache.GetGroup(contract.GroupID);
                msgDescribe = string.Format("{0}{1}您加入群{2}", userName, contract.IsAgreed ? "同意了" : "拒绝了", ggGroup.DisplayName);
                unitType = UnitType.Group;
            }

            return msgDescribe;
        }

        #endregion

        //搜索联系人事件
        private void SearchUser_Clicked()
        {
            this.searchPanel.Visibility = Visibility.Visible;
            this.searchPanel.SetFocus();
        }

        private void ClickSearchUserOrGroup()
        {
            if (this.searchPanel.Visibility == Visibility.Visible)
            {
                this.searchPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                this.searchPanel.Visibility = Visibility.Visible;
                this.searchPanel.SetFocus();
            }
        }

        private void ClickJump()
        {
            ///TODO 你要跳转的动作
        }

        #region 基础通知
        private void BasicOutter_MyDeviceOnline(ClientType obj)
        {

        }

        private void BasicOutter_MyDeviceOffline(ClientType obj)
        {

        }

        private void BasicOutter_BeingKickedOut()
        {
            UiSafeInvoker.ActionOnUI<string>(this.ForceClose, "已经被强制下线！");
        }

        private void BasicOutter_BeingPushedOut()
        {
            UiSafeInvoker.ActionOnUI<string>(this.ForceClose, "已经在其它地方登录！");
        }

        private async void ForceClose(string tips)
        {
            this.reconnecting = false;
            Task<ButtonResult> result = MessageBoxEx.ShowQuery(GlobalResourceManager.SoftwareName, tips, ButtonEnum.Ok);
            await result.ConfigureAwait(true);
            this.gotoExit = true;
            this.Close();
        }

        private void rapidPassiveEngine_ConnectionInterrupted()
        {
            UiSafeInvoker.ActionOnUI(this.do_rapidPassiveEngine_ConnectionInterrupted);
        }

        private bool reconnecting = true;
        void do_rapidPassiveEngine_ConnectionInterrupted()
        {
            Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus = UserStatus.OffLine;
            string reconnectingStr = this.reconnecting ? "，正在重连 . . ." : "";
            messageCacheManager.ChangeText(String.Format("{0}：{1}（{2}）\n状态：离线{3}", GlobalResourceManager.SoftwareName, Program.ResourceCenter.ClientGlobalCache.CurrentUser.Name, Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserID, reconnectingStr));

            foreach (GGUser friend in Program.ResourceCenter.ClientGlobalCache.GetAllUser())
            {
                friend.UserStatus = UserStatus.OffLine;
            }
            this.friendListBox.SetAllUnitOffline();
        }
        #endregion

        #region ITwinkleNotifySupporter
        public string GetFriendName(string friendID)
        {
            GGUser user = Program.ResourceCenter.ClientGlobalCache.GetUser(friendID);
            return user.Name;
        }

        public string GetGroupName(string groupID)
        {
            GGGroup group = Program.ResourceCenter.ClientGlobalCache.GetGroup(groupID);
            return group.Name;
        }

        public bool NeedTwinkle4User(string userID)
        { 
            return ChatFormController.GetExistedForm(userID) == null;
        }

        public bool NeedTwinkle4Group(string groupID)
        {
            return ChatFormController.GetExistedForm(groupID) == null;
        }

        public bool NeedTwinkle4AddFriendNotify()
        {
            return !ChatFormController.IsExistNotifyForm();
        }

        public void PlayAudioAsyn()
        {
            GlobalResourceManager.PlayAudioAsyn(); // Linux 播放声音出错！
        }

        public Image GetHeadIcon(string userID)
        {
            GGUser user = Program.ResourceCenter.ClientGlobalCache.GetUser(userID);
            return GlobalResourceManager.GetHeadImage(user);
        }

        public Image Icon64
        {
            get { return GlobalResourceManager.Icon64; }
        }

        public Image NoneIcon64
        {
            get { return GlobalResourceManager.NoneIcon64; }
        }

        public Image GroupIcon
        {
            get { return GlobalResourceManager.GroupIcon; }
        }

        public Image NotifyIcon
        {
            get { return GlobalResourceManager.NotifyIcon; }
        }
        public Image GetStatusIcon(UserStatus status)
        {
            return GlobalResourceManager.GetStatusIcon(status);
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

            UiSafeInvoker.ActionOnUI<string, string, ClientType, string, ulong, ResumedProjectItem, string>(this.do_fileOutter_FileRequestReceived, projectID, senderID, senderType, projectName, totalSize, resumedFileItem, comment);
        }

        void do_fileOutter_FileRequestReceived(string projectID, string senderID, ClientType senderType, string projectName, ulong totalSize, ResumedProjectItem resumedFileItem, string comment)
        {
            ClientType senderClientType = ClientType.Others;
            string offlineFileSenderID = Comment4OfflineFile.ParseUserID(comment, out senderClientType);
            bool offlineFile = (offlineFileSenderID != null);
            if (offlineFile)
            {
                senderID = offlineFileSenderID;
                if (senderID == Program.ResourceCenter.CurrentUserID)
                {
                    if (senderClientType == Program.ResourceCenter.CurrentClientType)
                    {
                        return;
                    }
                    FileAssistantWindow fileAssistantForm = (FileAssistantWindow)MainWindow.ChatFormController.GetFileAssistantForm();
                    fileAssistantForm.FileRequestReceived(projectID, offlineFile);
                    return;
                }
            }
            FriendChatWindow form = (FriendChatWindow)ChatFormController.GetForm(senderID);
            form.FileRequestReceived(projectID, offlineFile);

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

            UiSafeInvoker.ActionOnUI<TransferingProject, bool>(this.do_fileOutter_FileResponseReceived, project, agreeReceive);
        }
        void do_fileOutter_FileResponseReceived(TransferingProject project, bool agreeReceive)
        {
            FriendChatWindow form = (FriendChatWindow)ChatFormController.GetForm(project.DestUserID);
            form.Show_Topmost();
        }
        #endregion
        #endregion

        #region 按钮事件

        private void MainWindow_UserStatusChanged(UserStatus userStatus)
        {
            this.loginUserStatus = userStatus;
            Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus = userStatus;
            Program.ResourceCenter.ClientOutter.ChangeMyStatus(userStatus);
            this.statusIco.Source = CommonHelper.GetUserStatusIco(userStatus);
            messageCacheManager.ChangeText(String.Format("{0}：{1}（{2}）\n状态：{3}", GlobalResourceManager.SoftwareName, Program.ResourceCenter.ClientGlobalCache.CurrentUser.Name, Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserID, GlobalResourceManager.GetUserStatusName(Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus)));

            //messageCacheManager.ChangeMyStatus(Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus);
        }

        #endregion

        #region 右键菜单

        //查询用户
        private void SearchUser_Click()
        {
            this.SearchUser_Clicked();
            //this.searchUserOrGroupPanel.IsVisible = true;
        }

        //创建群组
        private void CreateGroup_Click()
        {
            CreateGroupWindow window = new CreateGroupWindow();
            window.Show_Topmost();
        }

        //修改个人信息
        private async void UpdateUserInfo()
        {
            UpdateUserInfoWindow updateUserInfoWindow = new UpdateUserInfoWindow();
            await updateUserInfoWindow.ShowDialog_Topmost(this);
        }

        //修改密码
        private async void UpdatePassword_Click()
        {
            UpdatePasswordWindow updatePasswordWindow = new UpdatePasswordWindow();

            updatePasswordWindow.Initialize(Program.PassiveEngine, Program.ResourceCenter.ClientGlobalCache.CurrentUser);
            System.Threading.Tasks.Task<object> task = updatePasswordWindow.ShowDialog_Topmost(this);
            await task.ConfigureAwait(true);
            if (Convert.ToBoolean(task.Result))
            {
                MessageBoxEx.Show("修改成功！");
            }
        }

        //文件传输助手
        private void FileAssistant_Click()
        {
            Window window = MainWindow.ChatFormController.GetFileAssistantForm();
            window.Show_Topmost();
        }

        //网盘
        private void NetworkDisk_Click()
        {            
            NDiskWindow nDiskWindow = new NDiskWindow(Program.ResourceCenter.RapidPassiveEngine.FileOutter, this.nDiskOutter, Program.ResourceCenter.CurrentUserID);
            nDiskWindow.Show_Topmost();
        }

        //消息验证
        private void VerifyMessage_Click()
        {
            CommonHelper.MoveToNotifyWindow(NotifyType.User);
        }

        //退出
        private void Quit_Click()
        {
            this.gotoExit = true;
            this.Close();
        }

        #endregion
    }
}
