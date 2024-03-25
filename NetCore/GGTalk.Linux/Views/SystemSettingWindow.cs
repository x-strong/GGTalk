using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGTalk.Linux.Views
{
    internal class SystemSettingWindow : BaseWindow
    {
        private CheckBox skinCheckBox_autoLogin;
        private RadioButton radiobutton_hide;

        protected override void InitializeComponent()
        {
            this.Icon = GlobalResourceManager.Png64;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "系统设置";
            Width = 440;
            Height = 230;
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
                                MarginTop = 4,
                                MarginLeft = 25,
                                Text = "系统设置",
                            },
                            new Panel
                            {
                                MarginRight = 1,
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
                        MarginLeft = 0,
                        Background = "#fff",
                        Width = "100%",
                        Height = 200,
                        MarginBottom = 0,
                        Children =
                        {
                            new TextBlock
                            {
                                MarginLeft = 22,
                                MarginTop = 13,
                                Text = "关闭主面板时："
                            },
                            new RadioButton
                            {
                                Name=nameof(this.radiobutton_hide),
                                PresenterFor=this,
                                GroupName="Menu",
                                MarginLeft = 117,
                                MarginTop = 12,
                                Content = "隐藏任务栏通知区域"
                            },
                            new RadioButton
                            {
                                GroupName="Menu",
                                IsChecked=true,
                                MarginBottom = 170,
                                MarginRight = 91,
                                MarginTop = 12,
                                Content = "退出程序"
                            },
                            new TextBlock
                            {
                                MarginBottom = 140,
                                MarginLeft = 27,
                                MarginTop = 44,
                                Text = "摄像头选择："
                            },
                            new ComboBox
                            {
                                Classes = "SystemComboBox",
                                MarginLeft = 117,
                                MarginTop = 41,
                                Width = 300,
                                Height = 23,
                                Items =
                                {
                                    "0",
                                    "1",
                                    "2"
                                }
                            },
                            new TextBlock
                            {
                                MarginLeft = 27,
                                MarginTop = 77,
                                Text = "麦克风选择："
                            },
                            new ComboBox
                            {
                                Classes = "SystemComboBox",
                                MarginLeft = 117,
                                MarginTop = 74,
                                Width = 300,
                                Height = 23,
                                Items =
                                {
                                    "0",
                                    "1",
                                    "2"
                                }
                            },
                            new TextBlock
                            {
                                MarginLeft = 27,
                                MarginTop = 110,
                                Text = "扬声器选择："
                            },
                            new ComboBox
                            {
                                Classes = "SystemComboBox",
                                MarginLeft = 117,
                                MarginTop = 107,
                                Width = 300,
                                Height = 23,
                                Items =
                                {
                                    "0",
                                    "1",
                                    "2"
                                }
                            },
                            //new TextBlock
                            //{
                            //    MarginLeft = 27,
                            //    MarginTop = 146,
                            //    Text = "打开聊天窗："
                            //},
                            //new CheckBox
                            //{
                            //    MarginRight = 146,
                            //    MarginLeft = 117,
                            //    MarginTop = 146,
                            //    Content = "显示上一次交谈的最后一句话"
                            //},
                            //new TextBlock
                            //{
                            //    MarginLeft = 27,
                            //    MarginTop = 177,
                            //    Text = "信息提示音："
                            //},
                            //new CheckBox
                            //{
                            //    MarginLeft = 117,
                            //    MarginTop = 175,
                            //    Content = "当收到信息时，播放信息提示音"
                            //},
                            new TextBlock
                            {
                                MarginLeft = 39,
                                MarginTop = 139,
                                Text = "自动启动："
                            },
                            //new CheckBox
                            //{
                            //    MarginLeft = 117,
                            //    MarginTop = 146,
                            //    Content = "开机时自动启动"
                            //},
                            new CheckBox
                            {
                                Name=nameof(this.skinCheckBox_autoLogin),
                                PresenterFor=this,
                                MarginLeft =117,// 237,
                                MarginTop = 140,
                                Content = "自动登录"
                            },
                            new Button
                            {
                                Classes = "commonButton",
                                MarginBottom = 11,
                                MarginRight = 28,
                                Width = 64,
                                Height = 26,
                                BorderFill = "207,207,207",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Content = "确定",
                                Background = "#fff",
                                Commands={ { nameof(Button.Click),(s,e)=> { this.Confirm_Click(); } } }
                            }
                        }
                    }
                }
            });
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));
            //加载样式文件，文件需要设置为内嵌资源

            if (!DesignMode)//设计模式下不执行，也可以用#if !DesignMode
            {
                 this.skinCheckBox_autoLogin = this.FindPresenterByName<CheckBox>(nameof(this.skinCheckBox_autoLogin));
                this.radiobutton_hide = this.FindPresenterByName<RadioButton>(nameof(this.radiobutton_hide));
                this.skinCheckBox_autoLogin.IsChecked = SystemSettings.Singleton.AutoLogin;
                this.radiobutton_hide.IsChecked = !SystemSettings.Singleton.ExitWhenCloseMainForm;
            }
        }


        private void Confirm_Click()
        {
            SystemSettings.Singleton.ExitWhenCloseMainForm = !this.radiobutton_hide.IsChecked.Value;
            SystemSettings.Singleton.AutoLogin = this.skinCheckBox_autoLogin.IsChecked.GetValueOrDefault();
            SystemSettings.Singleton.Save();
            this.Close();
        }
    }
}
