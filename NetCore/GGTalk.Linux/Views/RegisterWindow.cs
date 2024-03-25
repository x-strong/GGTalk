using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using ESPlus.Application.Basic;
using GGTalk;
using GGTalk.Linux;
using GGTalk.Linux.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using TalkBase;

namespace GGTalk.Linux.Views
{
    internal class RegisterWindow : BaseWindow
    {
        private Button bt_SmsCode;
        private TextBox tv_id, tv_name, tv_pwd1, tv_pwd2, tv_signature, tv_phone;
        private Timer timer;
        private int headImageIndex = 0;
        private Picture pnlImgTx;
        private string depID = "#0";
        private bool selfPhoto = false;

        protected override void InitializeComponent()
        {
            this.Icon = GlobalResourceManager.Png64;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "注册";
            Width = 413;
            Height = 530;
            Background = null;
            Children.Add(new Panel
            {
                BorderFill = "#619fd7",
                BorderThickness = new Thickness(1, 1, 1, 1),
                BorderType = BorderType.BorderThickness,
                ClipToBounds = true,
                Background = "rgb(249,253,254)",
                Width = "100%",
                Height = "100%",
                Children = //内容元素放这里
                {
                    new Panel
                    {
                        Height = 30,
                        Width = "100%",
                        Background = "#619fd7",
                        MarginTop = 0,
                        Children =
                        {
                            new Picture
                            {
                                Margin = "4,4,0,0",
                                Width = 16, Height = 16,
                                Stretch = Stretch.UniformToFill,
                                Source=CommonOptions.ResourcesCatalog+ "32.ico",
                            },
                            new TextBlock
                            {
                                MarginTop = 7,
                                MarginLeft = 24,
                                Text = "注册",
                            },
                            new Panel
                            {
                                MarginRight = 3,
                                MarginTop = -3,
                                Name = "close",
                                ToolTip = "关闭",
                                Width = 30,
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
                                        (s,e)=>
                                        {
                                            (e as MouseButtonEventArgs).Handled=true;
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
                        Commands =
                        {
                            {
                                nameof(MouseDown),
                                (s,e)=>this.DragMove()
                            }
                        }
                    },
                    new Panel
                    {
                        Width = "100%",
                        Height = 500,
                        MarginBottom = 0,
                        Children =
                        {
                            new Picture
                            {
                                Width="100%",
                                Height = 160,
                                MarginTop = 0,
                                Stretch = Stretch.Fill,
                                Source=CommonOptions.ResourcesCatalog+ "userInfoBackground.png",
                                Commands =
                                {
                                    {
                                        nameof(MouseDown),
                                        (s,e)=>this.DragMove()
                                    }
                                }
                            },
                            new Border
                            {
                                MarginLeft = 153.5f,
                                MarginTop = 25f,
                                Width = 106,
                                Height = 106,
                                BorderFill = "#eee",
                                Visibility=Visibility.Collapsed
                            },
                            new Picture
                            {
                                Name = nameof(this.pnlImgTx),
                                PresenterFor=this,
                                MarginLeft = 156,
                                MarginTop = 28,
                                Stretch = Stretch.Fill,
                                Source=CommonOptions.ResourcesCatalog+ "8.png",
                                Cursor=Cursors.Hand,
                                ToolTip="更换系统头像",
                                Commands={ { nameof(Picture.MouseDown),(s,e)=> { this.UpdateHeadImageIndex();  } } }
                            },
                            new TextBlock
                            {
                                MarginLeft = 45,
                                MarginTop = 173,
                                Text = "账号 :"
                            },
                            new TextBox
                            {
                                Classes = "commonTextbox",
                                Name=nameof(this.tv_id),
                                PresenterFor=this,
                                MarginLeft =87,
                                MarginTop = 168,
                                MaxLength=18,
                                Width = 200,
                                Height = 26,
                            },
                            new TextBlock
                            { 
                                MarginLeft = 45,
                                MarginTop = 216,
                                Text = "姓名 :"
                            },
                            new TextBox
                            {
                                Classes = "commonTextbox",
                                Name=nameof(this.tv_name),
                                PresenterFor=this,
                                MarginLeft = 87,
                                MarginTop = 211,
                                MaxLength=20,
                                Width = 200,
                                Height = 26,
                            },
                            new TextBlock
                            {
                                MarginLeft = 45,
                                MarginTop = 260,
                                Text = "密码 :"
                            },
                            new TextBox
                            {
                                Classes = "commonTextbox",
                                Name=nameof(this.tv_pwd1),
                                PresenterFor=this,
                                IsInputMethodEnabled=false,
                                PasswordChar='*',
                                MaxLength=12,                                
                                MarginLeft = 87,
                                MarginTop = 255,
                                Width = 200,
                                Height = 26,
                            },
                            new TextBlock
                            {
                                MarginLeft = 21,
                                MarginTop = 302,
                                Text = "确认密码 :"
                            },
                            new TextBox
                            {
                                Name=nameof(this.tv_pwd2),
                                PresenterFor=this,
                                IsInputMethodEnabled=false,
                                PasswordChar='*',
                                MaxLength=12,
                                MarginLeft = 87,
                                MarginTop = 297,
                                Classes = "commonTextbox",
                                Width = 200,
                                Height = 26,
                            },
                            new TextBlock
                            {
                                MarginLeft = 45,
                                MarginTop = 346,
                                Text = "签名 :"
                            },
                            new TextBox
                            {
                                Name=nameof(this.tv_signature),
                                PresenterFor=this,
                                MarginLeft = 87,
                                MarginTop = 341,
                                Classes = "commonTextbox",
                                MaxLength=30,
                                Width = 300,
                                Height = 26,
                            },
                            new TextBlock
                            {
                                MarginLeft = 22,
                                MarginTop = 388,
                                Text = "手机号码 :"
                            },
                            new TextBox
                            {
                                Name=nameof(this.tv_phone),
                                PresenterFor=this,
                                MarginLeft = 87,
                                MarginTop = 383,
                                MaxLength=15,
                                Classes = "commonTextbox",
                                Width = 200,
                                Height = 26,
                            },
                            new Button
                            {
                                Classes = "commonButton",
                                Foreground = "#000000",
                                MarginLeft = 264,
                                MarginBottom = 11,
                                Width = 60,
                                Height = 26,
                                BorderThickness = "3,3,3,3",
                                BorderFill = "#eee",
                                Content = "取消",
                                Commands=
                                {
                                    {
                                        nameof(Button.MouseDown),
                                        (s,e)=>this.Cancel_Click()
                                    }
                                }
                            },
                            new Button
                            {
                                Classes = "commonButton",
                                Foreground = "#000000",
                                MarginLeft = 338,
                                MarginBottom = 11,
                                Width = 60,
                                Height = 26,
                                BorderThickness = "3,3,3,3",
                                BorderFill = "#eee",
                                Content = "确定",
                                Commands=
                                {
                                    {
                                        nameof(Button.MouseDown),
                                        (s,e)=>this.OK_Click()
                                    }
                                }
                            }
                        }
                    }
                }
            });
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));
            //加载样式文件，文件需要设置为内嵌资源

            if (!DesignMode)//设计模式下不执行
            {
                this.tv_id = this.FindPresenterByName<TextBox>(nameof(this.tv_id));
                this.tv_id.TextInput += Tv_id_TextInput;
                this.tv_name = this.FindPresenterByName<TextBox>(nameof(this.tv_name));
                this.tv_pwd1 = this.FindPresenterByName<TextBox>(nameof(this.tv_pwd1));
                this.tv_pwd2 = this.FindPresenterByName<TextBox>(nameof(this.tv_pwd2));
                this.tv_pwd1.TextInput += Tv_pwd1_TextInput;
                this.tv_pwd2.TextInput += Tv_pwd1_TextInput;
                this.tv_signature = this.FindPresenterByName<TextBox>(nameof(this.tv_signature));
                this.tv_phone = this.FindPresenterByName<TextBox>(nameof(this.tv_phone));
                this.tv_phone.TextInput += Tv_phone_TextInput;
                this.pnlImgTx = this.FindPresenterByName<Picture>(nameof(this.pnlImgTx));
                this.Initialize();
            }
        }

        private void Tv_id_TextInput(object sender, TextInputEventArgs e)
        {
            //如果输入的不是退格和数字，则屏蔽输入
            Regex regex = new Regex(@"^[A-Za-z0-9]+$");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        private void Tv_pwd1_TextInput(object sender, TextInputEventArgs e)
        {
            //如果是汉字，则屏蔽输入
            string reg = @"^[\u4e00-\u9fa5]*$";
            if (Regex.Match(e.Text, reg).Success)
            {
                e.Handled = true;
            }
        }

        private void Tv_phone_TextInput(object sender, TextInputEventArgs e)
        {
            //如果输入的不是退格和数字，则屏蔽输入
            Regex regex = new Regex("^[0-9]*$");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }


        private new void Initialize()
        {
            Random ran = new Random();
            this.headImageIndex = ran.Next(0, GlobalResourceManager.HeadImages.Length);
            this.pnlImgTx.Source = GlobalResourceManager.HeadImages[this.headImageIndex];//根据ID获取头像  
            this.timer = new Timer(1000);
            this.timer.Elapsed += this.Timer1_Tick;
        }

        //更换系统自带头像索引
        private void UpdateHeadImageIndex()
        {
            this.headImageIndex = (++this.headImageIndex) % GlobalResourceManager.HeadImages.Length;
            this.pnlImgTx.Source= GlobalResourceManager.HeadImages[this.headImageIndex];
            this.selfPhoto = false;
        }

        private void Bt_SmsCode_Click()
        {
            try
            {
                string phone = this.tv_phone.Text;
                if (string.IsNullOrEmpty(phone))
                {
                    MessageBoxEx.Show("手机号码不能为空！");
                    return;
                }
                Regex regex = new Regex("[0-9]{11,11}");
                if (!regex.IsMatch(phone))
                {
                    MessageBoxEx.Show("手机号码不正确！");
                    return;
                }
                string token = FunctionOptions.GetRegistCodeToken + phone;
                LogonResponse response = Program.PassiveEngine.Initialize(phone, token, ConfigurationManager.AppSettings["ServerIP"], int.Parse(ConfigurationManager.AppSettings["ServerPort"]), null);
                if (response.FailureCause == RegisterResult.Succeed.ToString())
                {
                    this.SendSmsCodeSucceed();
                }
                else
                {
                    MessageBoxEx.Show("验证码发送失败，请稍后在试！");
                    return;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

        }

        private void SendSmsCodeSucceed()
        {
            this.tv_phone.IsEnabled = false;
            this.bt_SmsCode.IsEnabled = false;
            this.timer.Start();
        }

        private int countdown = 90;
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (countdown <= 0)
            {
                this.timer.Stop();
                countdown = 90;
                this.bt_SmsCode.Content = "发送验证码";
                this.bt_SmsCode.IsEnabled = true;
                this.tv_phone.IsEnabled = true;
                return;
            }
            this.bt_SmsCode.Content = string.Format("（{0}秒）", countdown);
            countdown--;
        }

        private void OK_Click()
        {
            string userID = this.tv_id.Text.Trim();
            if (string.IsNullOrEmpty(userID))
            {
                this.tv_id.FocusLastIndex();
                MessageBoxEx.Show("帐号不能为空！");
                return;
            }

            string userName = this.tv_name.Text;
            if (string.IsNullOrEmpty(userName))
            {
                MessageBoxEx.Show("姓名不能为空！");
                this.tv_name.ScrollToEnd();
                this.tv_name.FocusLastIndex();
                return;
            }
            string pwd = this.tv_pwd1.Text;
            if (string.IsNullOrEmpty(pwd))
            {
                MessageBoxEx.Show("密码不能为空！");
                this.tv_pwd1.ScrollToEnd();
                this.tv_pwd1.FocusLastIndex();
                return;
            }
            if (pwd != this.tv_pwd2.Text)
            {
                MessageBoxEx.Show("两次输入的密码不一致！");
                this.tv_pwd1.ScrollToEnd();
                this.tv_pwd1.FocusLastIndex();
                return;
            }
            if (pwd.Length < 6 || pwd.Length > 12)
            {
                MessageBoxEx.Show("密码长度必须在6-12位");
                this.tv_pwd1.ScrollToEnd();
                this.tv_pwd1.FocusLastIndex();
                return;
            }
            string phone = this.tv_phone.Text;
            if (string.IsNullOrEmpty(phone))
            {
                MessageBoxEx.Show("手机号码不能为空！");
                this.tv_phone.ScrollToEnd();
                this.tv_phone.FocusLastIndex();
                return;
            }
            if (!CommonHelper.IsPhoneNumber(phone))
            {
                MessageBoxEx.Show("手机号码格式有误！");
                this.tv_phone.ScrollToEnd();
                this.tv_phone.FocusLastIndex();
                return;
            }
            string smsCode = "0000";
            if (this.depID == null)
            {
                this.depID = string.Empty;
            }

            try
            {
                GGUser user = new GGUser(userID, pwd, userName, "", this.depID, this.tv_signature.Text, this.headImageIndex, "");
                user.Phone = phone;
                if (this.selfPhoto)
                {
                    user.HeadImageData = BitmapHelper.Bitmap2Byte((Bitmap)this.pnlImgTx.Source);
                    user.HeadImageIndex = -1;
                }
                //# Reg:name;pwd;phone;smsCode;#12(orgID);3(headIndex);加油！(signature)
                string token = FunctionOptions.RegistActionToken2 + String.Format("{0};{1};{2};{3};{4};{5};{6};{7}", user.Name, user.PasswordMD5, user.Phone, smsCode, user.OrgID, user.HeadImageIndex, user.Signature, user.UserID);
                LogonResponse response = Program.PassiveEngine.Initialize("regist", token, ConfigurationManager.AppSettings["ServerIP"], int.Parse(ConfigurationManager.AppSettings["ServerPort"]), null);

                switch (response.FailureCause)
                {
                    case "Existed":
                        MessageBoxEx.Show("用户帐号已经存在！");
                        return;
                    case "PhoneExisted":
                        MessageBoxEx.Show("电话号码已绑定过！");
                        return;
                    case "Error":
                        MessageBoxEx.Show("注册出现错误！");
                        return;
                    default:
                        if (response.FailureCause.StartsWith(RegisterResult.Succeed.ToString()))
                        {
                            user.UserID = response.FailureCause.Substring(RegisterResult.Succeed.ToString().Length);
                            break;
                        }
                        MessageBoxEx.Show(response.FailureCause);
                        return;
                }
                this.DialogResult = user;
                this.Close();
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show("注册失败！" + ee.Message);
            }
        }

        private void Cancel_Click()
        {
            this.DialogResult = null;
            this.Close();
        }
    }
}
