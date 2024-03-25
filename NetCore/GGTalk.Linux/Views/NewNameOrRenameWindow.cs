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
    internal class NewNameOrRenameWindow : BaseWindow
    {
        private TextBox txb_newName;

        private string oldName = "", newName = "";
        public string NewName => this.newName;
        private string titleName = "";
        private bool isAddName = false;//是否为新建名称

        public NewNameOrRenameWindow(string _titleName = "名称", string _oldName=null)
        {
            this.titleName = _titleName;
            if (string.IsNullOrEmpty(_oldName))
            {
                this.isAddName = true;
                return;
            }
            this.isAddName = false;
            this.oldName = _oldName;
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
            Title = this.isAddName ? "新增" + this.titleName : "修改" + this.titleName;
            Width = 320;
            Height = 136;
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
                                MarginLeft = 24,
                                Text = this.Title,
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
                        Background = "#fff",
                        Children =
                        {
                            new TextBlock
                            {
                                MarginLeft = 20,
                                MarginTop = 11,
                                Text = string.Format("请输入{0}：",this.titleName) 
                            },
                            new TextBox
                            {
                                Classes = "commonTextBoxB",
                                Name=nameof(this.txb_newName),
                                PresenterFor=this,
                                Text=this.oldName,
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
                                Classes = "commonButton",
                                MarginLeft = 161,
                                MarginTop = 72,
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
                                MarginTop = 72,
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
                //this.KeyDown += EditCatelogNameWindow_KeyDown;
            }
        }
        private void EditCatelogNameWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Keys.Escape)
            {
                this.DialogResult = false;
                this.Close();
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

            if (this.newName.Contains(":") || this.newName.Contains(";"))
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
