using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Platform;
using CPF.Shapes;
using CPF.Styling;
using ESFramework.Boost.DynamicGroup.Passive;
using ESFramework.Boost.NetworkDisk.Passive;
using ESPlus.Application.Basic;
using ESPlus.Application.CustomizeInfo;
using GGTalk.Linux.Controls;
using GGTalk.Linux.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkBase;

namespace GGTalk.Linux.Views
{
    internal class LoginWindow : BaseWindow
    {
        internal static MainWindow MainWindow;
        private ICustomizeHandler customizeHandler;
        private TextBox textBox_ID, textBox_Pwd;
        private CheckBox checkBoxRememberPwd, checkBoxAutoLogin;
        private Button buttonLogin;
        private Picture statusIco;
        ContextMenu pop;
        private bool pwdIsMD5 = false;
        private string pwdMD5;
        private NDiskPassiveHandler nDiskPassiveHandler;
        DynamicGroupOutter dynamicGroupOutter;
        private string title;
        public LoginWindow(ICustomizeHandler handler, NDiskPassiveHandler _nDiskPassiveHandler, DynamicGroupOutter _dynamicGroupOutter)
        {
            this.customizeHandler = handler;
            this.title = GlobalResourceManager.SoftwareName;
            this.nDiskPassiveHandler = _nDiskPassiveHandler;
            this.dynamicGroupOutter = _dynamicGroupOutter;
            this.InitializeComponent();
            this.Initialized += LoginWindow_Initialized;

        }

        private void LoginWindow_Initialized(object sender, EventArgs e)
        {
            if (SystemSettings.Singleton.AutoLogin)
            {
                Helpers.UiSafeInvoker.ActionOnUI(this.Login);
            }
        }

        public LoginWindow(ICustomizeHandler handler)
        {
            this.customizeHandler = handler;
            this.title = GlobalResourceManager.SoftwareName;
            this.InitializeComponent();
            if (SystemSettings.Singleton.AutoLogin)
            {
                this.Login();
            }
        }
        private volatile bool initialized = false;
        protected override void InitializeComponent()
        {
            if (this.initialized) { return; }
            this.Icon = GlobalResourceManager.Png64;
            this.initialized = true;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "登录";
            Width = 424;
            Height = 340;
            Background = null;
            ShowInTaskbar = true;
            //切换登录状态的ContextMenu
            pop = new ContextMenu
            {
                //PlacementTarget = btn,
                //Placement = PlacementMode.Mouse,
                Items = new UIElement[]
                {
                    new MenuItem
                    {
                        Icon =  GlobalResourceManager.GetStatusImage(UserStatus.Online),
                        Header = "我在线上",
                        Commands={
                            {
                                nameof(MouseDown),
                                (s,e)=> StatusItem_Click(UserStatus.Online)
                            }
                        }
                    },
                    new MenuItem
                    {
                        Icon =GlobalResourceManager.GetStatusImage(UserStatus.Away),
                        Header = "离开",
                        Commands={
                            {
                                nameof(MouseDown),
                                (s,e)=> StatusItem_Click(UserStatus.Away)
                            }
                        }
                    },
                    new MenuItem
                    {
                        Icon =GlobalResourceManager.GetStatusImage(UserStatus.Busy),
                        Header = "忙碌",
                        Commands={
                            {
                                nameof(MouseDown),
                                (s,e)=> StatusItem_Click(UserStatus.Busy)
                            }
                        }
                    },
                    new MenuItem
                    {
                        Icon =GlobalResourceManager.GetStatusImage(UserStatus.DontDisturb),
                        Header = "请勿打扰",
                        Commands={
                            {
                                nameof(MouseDown),
                                (s,e)=> StatusItem_Click(UserStatus.DontDisturb)
                            }
                        }
                    },
                    new MenuItem
                    {
                        Icon =GlobalResourceManager.GetStatusImage(UserStatus.Hide),
                        Header = "隐身",
                        Commands={
                            {
                                nameof(MouseDown),
                                (s,e)=> StatusItem_Click(UserStatus.Hide)
                            }
                        }
                    },
                },
            };
            Children.Add(new Panel()
            {
                BorderFill = "#619fd7",
                BorderThickness = new Thickness(2, 2, 2, 2),
                BorderType = BorderType.BorderThickness,
                ClipToBounds = true,
                Background = "#fff",
                MarginRight = 0f,
                Width = "100%",
                Height = "100%",
                Children = //内容元素放这里
                {
                    new Picture
                    {
                        MarginLeft = -1f,
                        MarginTop = -4f,
                        Height = 96.1f,
                        Width = 424,
                        Source=CommonOptions.ResourcesCatalog+ "image_sign.png",
                        //设置窗体拖拽
                        Commands =
                        {
                            {
                                nameof(MouseDown),
                                (s,e)=>this.DragMove()
                            }
                        }
                    },
                    new TextBlock
                    {
                        MarginLeft = 8.4f,
                        MarginTop = 8.9f,
                        Foreground = "#FFFFFF",
                        Text = title,
                    },
                    new Picture
                    {
                        MarginLeft = 357f,
                        MarginTop = 2f,
                        Height = 27f,
                        Width = 27f,
                        Cursor = Cursors.Hand,
                        Source=CommonOptions.ResourcesCatalog+ "help_btn_icon.png",
                        ToolTip="关于",
                        Commands={ { nameof(MouseUp),(s,e)=> {
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
                        MarginRight = 6f,
                        MarginLeft = "Auto",
                        MarginTop = 0f,
                        Name = "close",
                        ToolTip = "关闭",
                        Width = 30,
                        Height = 30f,
                        Cursor = Cursors.Hand,
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
                                (s,e)=>
                                {
                                    (e as MouseButtonEventArgs).Handled=true;
                                    this.DialogResult=false;
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
                    new Picture
                    {
                        MarginLeft = 178.5f,
                        MarginTop = 45f,
                        Stretch = Stretch.Fill,
                        Height = 66,
                        Width = 66,
                        Source=CommonOptions.ResourcesCatalog+ "8.png",
                    },
                    new Panel
                    {

                        Width = 22,
                        Height = 22,
                        MarginLeft = 230.5f,
                        MarginTop = 91f,
                        Background = Color.Transparent,
                        Children =
                        {
                            new Picture
                            {
                                Name=nameof(statusIco),
                                PresenterFor=this,
                                
                                Stretch = Stretch.Fill,
                                Source=CommonOptions.ResourcesCatalog+ "0.png",
                                Commands={
                                    {
                                        nameof(Button.MouseDown),
                                        (s,e)=>
                                        {
                                            this.pop.PlacementTarget = (UIElement)s;
                                            pop.IsOpen=true;
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new ElTextBox
                    {
                        Classes = "TextBoxModeA",
                        Name="txb_id",
                        PresenterFor = this,
                        MarginLeft = 87f,
                        MarginTop = 139f,
                        Placeholder = "账号",
                        Width = 250,
                        Height = 26,
                        MaxLength=20,
                        WordWarp=false,
                        AcceptsReturn=false,
                        IsAllowPasteImage=false,
                        BorderFill = "150,180,198",
                        BorderThickness = new Thickness(1,1,1,1),
                        BorderType = BorderType.BorderThickness,
                        BorderStroke = "1"
                    },
                    new ElTextBox
                    {
                        Classes = "TextBoxModeA",
                        Name="txb_password",
                        PresenterFor = this,
                        MarginLeft = 87f,
                        MarginTop = 179f,
                        Placeholder = "密码",
                        PasswordChar= '*',
                        Width = 250,
                        Height = 26,
                        MaxLength=12,
                        WordWarp=false,
                        AcceptsReturn=false,
                        IsAllowPasteImage=false,
                        BorderFill = "150,180,198",
                        BorderThickness = new Thickness(1,1,1,1),
                        BorderType = BorderType.BorderThickness,
                        BorderStroke = "1",
                        Commands =
                        {
                            {
                                nameof(KeyUp),
                                (s,e)=>
                                {
                                    this.pwdIsMD5 = false;
                                }
                            }
                        }
                    },
                    new CheckBox
                    {
                        Name="checkBoxRememberPwd",
                        PresenterFor = this,
                        Foreground = new SolidColorFill
                        {
                            Color = "#939292",
                        },
                        MarginLeft = 87,
                        MarginTop = 220.5f,
                        Content = "记住密码",
                    },
                    new CheckBox
                    {
                        Name="checkBoxAutoLogin",
                        PresenterFor = this,
                        Height = 18.2f,
                        Foreground = new SolidColorFill
                        {
                            Color = "#939292",
                        },
                        MarginLeft = 189f,
                        MarginTop = 221.5f,
                        Content = "自动登录",
                    },
                    new TextBlock
                    {
                        Name="bt_Register",
                        PresenterFor = this,
                        MarginLeft = 289f,
                        MarginTop = 222.5f,
                        Cursor = Cursors.Hand,
                        Text = "注册账号",
                        Foreground = "#0C7AB9",
                        Commands={ { nameof(MouseDown),(s,e)=> { this.SkipRegister(); } } }
                    },
                    
                    //new TextBlock                    
                    //{
                    //    MarginLeft = 289f,
                    //    MarginTop = 222.5f,
                    //    Cursor = Cursors.Hand,
                    //    Text = "忘记密码",
                    //    Foreground = "147,146,146",
                    //    Commands={ { nameof(MouseDown),(s,e)=> { this.ForgetPassword_Click(); } } }
                    //},
                    new Button
                    {
                        BorderThickness = new Thickness(0,0,0,0),
                        BorderType = BorderType.BorderThickness,
                        Classes = "loginBt",
                        Name="bt_Login",
                        PresenterFor = this,
                        CornerRadius = "4",
                        FontSize = 16f,
                        Height = 40f,
                        Width = 250,
                        MarginLeft = 87,
                        MarginTop = 253.4f,
                        Content = "登录",
                        Commands =
                        {
                            {
                                nameof(Button.Click),
                                (s,e)=> this.Login()
                            }
                        }
                    },
                    new TextBlock
                    {
                        Foreground="#0C7AB9",
                        PresenterFor = this,
                        MarginTop = 310.9f,
                        MarginLeft = 30f,
                        Cursor = Cursors.Hand,
                        Text = "作者QQ：2027224508",
                        Commands={ { nameof(MouseDown),(s,e)=> {
                            try
                            {
                                Clipboard.SetData((DataFormat.Text, "2027224508"));
                                MessageBoxEx.Show("号码已复制");
                            }
                            catch (Exception ee)
                            {
                                GlobalResourceManager.WriteErrorLog(ee);
                            }
                        } } }
                    },

                    new TextBlock
                    {
                        Foreground="#0C7AB9",
                        MarginRight = 30f,
                        MarginTop = 310.9f,
                        Text = "GGTalk博客",
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
                },
            });

            //加载样式文件，文件需要设置为内嵌资源
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));

            if (!DesignMode)
            {
                Icon = GlobalResourceManager.Png64;
                this.SetComponent();
                
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Keys.Enter && this.buttonLogin.IsEnabled)
            {
                this.Login();
            }
        }

        private void SetComponent()
        {
            this.Title = title;
            this.textBox_ID = this.FindPresenterByName<TextBox>("txb_id");
            this.textBox_Pwd = this.FindPresenterByName<TextBox>("txb_password");
            this.checkBoxAutoLogin = this.FindPresenterByName<CheckBox>("checkBoxAutoLogin");
            this.checkBoxRememberPwd = this.FindPresenterByName<CheckBox>("checkBoxRememberPwd");
            this.buttonLogin = this.FindPresenterByName<Button>("bt_Login");
            this.textBox_ID.Text = SystemSettings.Singleton.LastLoginUserID;
            this.statusIco = this.FindPresenterByName<Picture>(nameof(statusIco));

            this.checkBoxAutoLogin.Checked += CheckBoxAutoLogin_Checked;
            if (SystemSettings.Singleton.RememberPwd)
            {
                this.textBox_Pwd.Text = "11111111";
                this.pwdMD5 = SystemSettings.Singleton.LastLoginPwdMD5;
                this.pwdIsMD5 = true;
            }
            this.checkBoxAutoLogin.IsChecked = SystemSettings.Singleton.AutoLogin;
            this.checkBoxRememberPwd.IsChecked = SystemSettings.Singleton.RememberPwd;
        }

        private void CheckBoxAutoLogin_Checked(object sender, EventArgs e)
        {
            if (this.checkBoxAutoLogin.IsChecked.Value) {
                this.checkBoxRememberPwd.IsChecked = true;
            }
        }

        private volatile bool logining = false;
        private async void Login()
        {
            if (this.logining || this.Visibility == Visibility.Collapsed || this.Visibility == Visibility.Hidden) { return; }           
            this.logining = true;
            string id = this.textBox_ID.Text;
            string pwd = this.textBox_Pwd.Text;
            if (string.IsNullOrEmpty(id)) { return; }

            this.buttonLogin.IsEnabled = false;
            try
            {
                Program.PassiveEngine.SecurityLogEnabled = false;

                
                if (!this.pwdIsMD5)
                {
                    //if (string.IsNullOrEmpty(pwd)) { return; }
                    pwdMD5 = ESBasic.Security.SecurityHelper.MD5String2(pwd);
                }
                LogonResponse response = Program.PassiveEngine.Initialize(id, pwdMD5, System.Configuration.ConfigurationManager.AppSettings["ServerIP"], int.Parse(System.Configuration.ConfigurationManager.AppSettings["ServerPort"]), this.customizeHandler);

                if (response.LogonResult == LogonResult.Failed)
                {
                    MessageBoxEx.Show(response.FailureCause);
                    return;
                }

                //0923
                if (response.LogonResult == LogonResult.VersionMismatched)
                {
                    MessageBoxEx.Show("客户端与服务器的ESFramework版本不一致！");
                    return;
                }

                if (response.LogonResult == LogonResult.HadLoggedOn)
                {
                    MessageBoxEx.Show("该帐号已经在其它地方登录！");
                    return;
                }
                SystemSettings.Singleton.LastLoginUserID = id;
                SystemSettings.Singleton.RememberPwd = this.checkBoxRememberPwd.IsChecked.Value;
                SystemSettings.Singleton.LastLoginPwdMD5 = pwdMD5;
                SystemSettings.Singleton.AutoLogin = this.checkBoxAutoLogin.IsChecked.Value;
                SystemSettings.Singleton.Save();
                MainWindow = new MainWindow(customizeHandler, nDiskPassiveHandler, dynamicGroupOutter, this.loginStatus);
                this.Hide();
                MainWindow.Show();
                this.DialogResult = true;
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show("登录失败：" + ee.Message);
            }
            finally
            {
                this.buttonLogin.IsEnabled = true;
                this.logining = false;
            }
        }

        private UserStatus loginStatus = UserStatus.Online;
        public UserStatus LoginStatus => loginStatus;

        private void StatusItem_Click(UserStatus userStatus)
        {
            this.loginStatus = userStatus;
            this.statusIco.Source = CommonHelper.GetUserStatusIco(userStatus);
        }

        private async void SkipRegister()
        {
            RegisterWindow registerWindow = new RegisterWindow();
            //registerWindow.Show();
            System.Threading.Tasks.Task<object> task = registerWindow.ShowDialog_Topmost(this);
            await task.ConfigureAwait(true);
            GGUser user = task.Result as GGUser;
            if (user != null)
            {
                MessageBoxEx.Show("快速登录", string.Format("注册成功！您的帐号为{0}！", user.UserID));
                this.textBox_ID.Text = user.UserID;
            }
        }
    }
}
