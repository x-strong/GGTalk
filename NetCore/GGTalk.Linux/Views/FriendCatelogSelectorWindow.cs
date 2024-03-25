using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using GGTalk.Linux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGTalk.Linux.Views
{
    internal class FriendCatelogSelectorWindow : BaseWindow
    {
        private ComboBox comboBox;
        private string oldCatelogName, newCatelogName;
        public string NewName => this.newCatelogName;

        public FriendCatelogSelectorWindow(string _oldCatelogName, List<string> catelogNameList)
        {
            this.InitializeComponent();
            this.oldCatelogName = _oldCatelogName;
            this.comboBox.Items = catelogNameList;
            this.comboBox.SelectedValue = this.oldCatelogName;
        }

        private volatile bool initialized = false;
        protected override void InitializeComponent()
        {
            if (this.initialized) { return; }
            this.Icon = GlobalResourceManager.Png64;
            this.initialized = true;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "修改备注姓名";
            Width = 320;
            Height = 136;
            Background = null;
            BorderFill = "#eee";
            BorderThickness = new Thickness(2, 2, 2, 2);
            BorderType = BorderType.BorderThickness;
            Children.Add(new Panel()
            {
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
                        Background = "#adcdeb",
                        MarginTop = 0,
                        Children =
                        {
                            new Picture
                            {
                                Margin = "4,4,0,0",
                                Width = 16, Height = 16,
                                Stretch = Stretch.UniformToFill,
                                Source=CommonOptions.ResourcesCatalog+"32.ico" ,
                            },
                            new TextBlock
                            {
                                MarginLeft = 35,
                                Text = "修改备注姓名",
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
                                            this.DialogResult=false;
                                        }
                                    },
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
                        Height = 106,
                        MarginBottom = 0,
                        Background = "247,250,253",
                        Children =
                        {
                            new TextBlock
                            {
                                MarginLeft = 20,
                                MarginTop = 11,
                                Text = "分组："
                            },
                            new ComboBox
                            {
                                Name=nameof(this.comboBox),
                                PresenterFor=this,
                                MarginLeft = 20,
                                MarginTop = 33,
                                Width = 280,
                                Height = 28,
                                BorderFill = "150,180,198",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness
                            },
                            new Button
                            {
                                MarginLeft = 161,
                                MarginTop = 72,
                                Width = 64,
                                Height = 26,
                                BorderFill = "207,207,207",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Content = "取消",
                                Background = "#fff",
                                Commands={ {nameof(MouseDown),(s,e)=> this.DialogResult=false} }
                            },
                            new Button
                            {
                                MarginLeft = 232,
                                MarginTop = 72,
                                Width = 64,
                                Height = 26,
                                BorderFill = "207,207,207",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Content = "确定",
                                Background = "#fff",
                                Commands={ {nameof(MouseDown),(s,e)=> { this.newCatelogName = this.comboBox.SelectedValue.ToString(); this.DialogResult = true; } } }
                            }
                        }
                    }
                }
            }) ;
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));
            //加载样式文件，文件需要设置为内嵌资源

            if (!DesignMode)//设计模式下不执行，也可以用#if !DesignMode
            {
                this.comboBox = this.FindPresenterByName<ComboBox>(nameof(this.comboBox));
            }
        }
    }
}
