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
    internal class EditCatelogNameWindow : BaseWindow
    {
        private TextBox txb_newName;

        private string oldName = "", newName = "";
        public string NewName => this.newName;

        public EditCatelogNameWindow()
        {
            this.InitializeComponent();
            this.Title = "新增分组";
            this.txb_newName.FocusLastIndex();
        }

        private void EditCatelogNameWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Keys.Escape)
            {
                this.DialogResult = false;
                this.Close();
            }
        }

        public EditCatelogNameWindow(string _oldName)
        {
            this.InitializeComponent();
            this.Title = "修改分组";
            this.oldName = _oldName;
            this.txb_newName.Text = this.oldName;
            this.txb_newName.FocusLastIndex();
        }

        private volatile bool initialized = false;
        protected override void InitializeComponent()
        {
            if (this.initialized) { return; }
            this.Icon = GlobalResourceManager.Png64;
            this.initialized = true;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "编辑组名";
            Width = 320;
            Height = 126;
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
                                Source=CommonOptions.ResourcesCatalog+"32.ico" ,
                            },
                            new TextBlock
                            {
                                MarginTop = 4,
                                MarginLeft = 22,
                                Text = "编辑组名",
                            },
                            new Panel
                            {
                                MarginRight = 0,
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
                        Height = 96,
                        MarginBottom = 0,
                        Background = "247,250,253",
                        Children =
                        {
                            new TextBlock
                            {
                                MarginLeft = 10,
                                MarginTop = 23,
                                Text = "分组名称："
                            },
                            new TextBox
                            {
                                Classes ="commonTextBoxB",
                                Name=nameof(this.txb_newName),
                                PresenterFor=this,
                                MarginLeft = 72,
                                MarginTop = 17,
                                Width = 226,
                                Height = 28,
                                BorderFill = "150,180,198",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness
                            },
                            new Button
                            {
                                Classes = "commonButton",
                                MarginLeft = 153,
                                MarginTop = 59,
                                Width = 64,
                                Height = 26,
                                BorderFill = "207,207,207",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Content = "取消",
                                Background = "#fff",
                                Commands={ {nameof(MouseDown),(s,e)=>this.Btn_cancel_Click()} }
                            },
                            new Button
                            {
                                Classes = "commonButton",
                                MarginLeft = 232,
                                MarginTop = 59,
                                Width = 64,
                                Height = 26,
                                BorderFill = "207,207,207",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Content = "确定",
                                Background = "#fff",
                                Commands={ {nameof(MouseDown),(s,e)=>this.Btn_ok_Click()} }
                            }
                        }
                    }
                }
            });
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));
            //加载样式文件，文件需要设置为内嵌资源

            if (!DesignMode)//设计模式下不执行，也可以用#if !DesignMode
            {
                this.txb_newName = this.FindPresenterByName<TextBox>(nameof(this.txb_newName));
                this.txb_newName.ScrollToEnd();
                this.txb_newName.ScrollToCaret();
                //this.KeyDown += EditCatelogNameWindow_KeyDown;
            }
        }


        private void Btn_ok_Click()
        {
            this.newName = this.txb_newName.Text.Trim();

            if (this.newName == this.oldName)
            {
                this.DialogResult = false;
                return;
            }

            if (this.newName.Contains(":") || this.newName.Contains(";") || this.newName.Contains(","))
            {
                MessageBoxEx.Show("名称中不能包含特殊字符！");
                return;
            }

            this.DialogResult = true;
        }

        private void Btn_cancel_Click()
        {
            this.DialogResult = false;
        }
    }
}
