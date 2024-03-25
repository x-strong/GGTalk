using CPF;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using ESPlus.Application.Basic;
using ESPlus.Rapid;
using ESPlus.Serialization;
using GGTalk.Linux.Controls;
using System;
using System.Configuration;
using TalkBase;

namespace GGTalk.Linux.Views
{
    internal class UpdatePasswordWindow : BaseWindow
    {
        private IRapidPassiveEngine rapidPassiveEngine;
        private TextBlock tb_title;
        private TextBox tb_oldPwd, txb_NewPassword, txb_NewPassword2;
        private Button btn_ok;
        private GGUser currentUser;

        public UpdatePasswordWindow()
        {
            this.InitializeComponent();
        }


        public void Initialize(IRapidPassiveEngine engine, GGUser user)
        {
            this.rapidPassiveEngine = engine;
            this.currentUser = user;
        }

        private bool initialized = false;
        protected override void InitializeComponent()
        {
            if (this.initialized) { return; }
            this.Icon = GlobalResourceManager.Png64;
            this.initialized = true;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "修改密码";
            Width = 360;
            Height = 200;
            Background = null;
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
                                Name=nameof(this.tb_title),
                                PresenterFor=this,
                                MarginTop = 4,
                                MarginLeft = 24,
                                Text = "修改密码",
                            },
                            new Panel
                            {
                                MarginRight = 0,
                                MarginLeft = "Auto",
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
                        Height = 170,
                        MarginBottom = 0,
                        Background = "247,250,253",
                        Children =
                        {
                            new TextBlock
                            {
                                MarginLeft = 40,
                                MarginTop = 25,
                                Text = "旧密码："
                            },
                            new ElTextBox
                            {
                                Classes = "TextBoxModeA",
                                Name=nameof(this.tb_oldPwd),
                                PresenterFor=this,
                                WordWarp = false,
                                BorderThickness = new Thickness(1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                MarginLeft = 99,
                                MarginTop = 22,
                                Width = 176,
                                Height = 24,
                                MaxLength=12,
                                PasswordChar = '*',
                                BorderFill = "150,180,198",
                                Placeholder = "请输入旧密码"
                            },
                            new TextBlock
                            {
                                MarginLeft = 40,
                                MarginTop = 65,
                                Text = "新密码："
                            },
                            new ElTextBox
                            {
                                Classes = "TextBoxModeA",
                                Name=nameof(this.txb_NewPassword),
                                PresenterFor=this,
                                MarginLeft = 98,
                                MarginTop = 60,
                                Width = 178,
                                Height = 26,
                                MaxLength=12,
                                PasswordChar = '*',
                                BorderFill = "150,180,198",
                                BorderThickness = new Thickness(1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Placeholder = "请输入新密码"
                            },
                            new TextBlock
                            {
                                MarginLeft = 16,
                                MarginTop = 103,
                                Text = "确认新密码："
                            },
                            new ElTextBox
                            {
                                Classes = "TextBoxModeA",
                                Name=nameof(this.txb_NewPassword2),
                                PresenterFor=this,
                                MarginLeft = 98,
                                MarginTop = 98,
                                Width = 178,
                                Height = 26,
                                MaxLength=12,
                                PasswordChar = '*',
                                BorderFill = "150,180,198",
                                BorderThickness = new Thickness(1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Placeholder = "请再次输入新密码"
                            },
                             new Button
                            {
                                Classes = "commonButton",
                                MarginLeft = 192,
                                MarginTop = 135,
                                Width = 64,
                                Height = 24,
                                BorderFill = "207,207,207",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Content = "取消",
                                Background = "#fff",
                                Commands={ { nameof(Button.Click),(s,e)=> { this.Btn_cancel_Click(); } } }
                            },
                            new Button
                            {
                                Classes = "commonButton",
                                Name=nameof(this.btn_ok),
                                PresenterFor=this,
                                MarginLeft = 271,
                                MarginTop = 135,
                                Width = 64,
                                Height = 24,
                                BorderFill = "207,207,207",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Content = "确定",
                                Background = "#fff",
                                Commands={ { nameof(Button.Click),(s,e)=> { this.Btn_ok_Click(); } } }
                            }
                        }
                    }
                }
            }) ;
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));
            //加载样式文件，文件需要设置为内嵌资源

            if (!DesignMode)//设计模式下不执行，也可以用#if !DesignMode
            {
                this.tb_title = this.FindPresenterByName<TextBlock>(nameof(this.tb_title));
                this.tb_oldPwd = this.FindPresenterByName<TextBox>("tb_oldPwd");
                this.txb_NewPassword = this.FindPresenterByName<TextBox>("txb_NewPassword");
                this.txb_NewPassword.TextInput += Tv_pwd1_TextInput;
                this.txb_NewPassword2 = this.FindPresenterByName<TextBox>(nameof(this.txb_NewPassword2));
                this.btn_ok = this.FindPresenterByName<Button>(nameof(this.btn_ok));
            }
        }


        private void Tv_pwd1_TextInput(object sender, TextInputEventArgs e)
        {
            //如果是汉字，则屏蔽输入
            string reg = @"^[\u4e00-\u9fa5]*$";
            if (System.Text.RegularExpressions.Regex.Match(e.Text, reg).Success)
            {
                e.Handled = true;
            }
        }

        private void Btn_ok_Click()
        {
            try
            {
                this.btn_ok.IsEnabled = false;
                string oldPwd = this.tb_oldPwd.Text.Trim();
                if (string.IsNullOrEmpty(oldPwd))
                {
                    MessageBoxEx.Show("旧密码不能为空！");
                    return;
                }
                string newPsw = this.txb_NewPassword.Text.Trim();
                if (string.IsNullOrEmpty(newPsw))
                {
                    MessageBoxEx.Show("新密码不能为空！");
                    this.txb_NewPassword.FocusLastIndex();
                    return;
                }
                if (newPsw.Length < 6 || newPsw.Length > 12)
                {
                    MessageBoxEx.Show("新密码长度必须在6-12位");
                    this.txb_NewPassword.ScrollToEnd();
                    this.txb_NewPassword.FocusLastIndex();
                    return;
                }

                try
                {
                    ChangePasswordResult res = Program.ResourceCenter.ClientOutter.ChangeMyPassword(ESBasic.Security.SecurityHelper.MD5String2(oldPwd), ESBasic.Security.SecurityHelper.MD5String2(newPsw));

                    if (res == ChangePasswordResult.OldPasswordWrong)
                    {
                        MessageBoxEx.Show("旧密码不正确！");
                        this.tb_oldPwd.Focus();
                        this.DialogResult = false;
                        return;
                    }
                    this.DialogResult = true;
                    this.Close();
                }
                catch (Exception ee)
                {
                    MessageBoxEx.Show("修改密码失败！" + ee.Message);
                    this.DialogResult = false;
                }                
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(ee.Message);
            }
            finally
            {
                this.btn_ok.IsEnabled = true;
            }

        }

        private void Btn_cancel_Click()
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
