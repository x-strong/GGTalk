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
    internal class ImageSelectorWindow : Window
    {
        private Picture pic;

        public Image ContentBitmap => (Image)this.pic.Source;
        private string buttonStr = "确认";//确认按钮上的字符

        /// <summary>
        /// 图片选择器
        /// </summary>
        /// <param name="bitmap">默认图片</param>
        /// <param name="_buttonStr">确认按钮上的字符</param>
        public ImageSelectorWindow(Bitmap bitmap,string _buttonStr = "确认")
        {
            this.buttonStr = _buttonStr;
            this.InitializeComponent();
            this.pic.Source = bitmap;
        }

        /// <summary>
        /// 图片选择器
        /// </summary>
        /// <param name="imgPath">默认图片路径</param>
        /// <param name="_buttonStr">确认按钮上的字符</param>
        public ImageSelectorWindow(string imgPath, string _buttonStr= "确认")
        {
            this.buttonStr = _buttonStr;
            this.InitializeComponent();
            this.pic.Source = new Bitmap(imgPath);
        }
        private bool initialized = false;
        protected override void InitializeComponent()
        {
            if (this.initialized)
            {
                return;
            }
            this.Icon = GlobalResourceManager.Png64;
            this.initialized = true;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "选择图片";
            Width = 510;
            Height = 510;
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
                        Background = "#adcdeb",
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
                                Text = "选择图片",
                            },
                            new Panel
                            {
                                MarginRight = 2,
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
                        Height = 480,
                        MarginBottom = 0,
                        Background = "#fff",
                        Children =
                        {
                            //new Border
                            //{
                            //    Width = 210,
                            //    Height = 210,
                            //    BorderFill = "#eee",
                            //    BorderThickness = new Thickness(1,1,1,1),
                            //    BorderType = BorderType.BorderThickness,
                            //    MarginTop = 28,
                            //},
                            new Picture
                            {
                                Name=nameof(this.pic),
                                PresenterFor=this,
                                MarginTop = 12,
                                Width = 468,
                                Height = 397,
                                Stretch = Stretch.Uniform,
                                Source= null,
                            },
                            new Button
                            {
                                Classes = "commonButton",
                                MarginLeft = 153,
                                MarginBottom = 25,
                                Width = 64,
                                Height = 26,
                                BorderFill = "207,207,207",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Content = "取消",
                                Background = "#fff",
                                Commands=
                                {
                                    {
                                        nameof(Button.Click),
                                        (s,e)=>  this.Btn_cancel_Click()
                                    }
                                }
                            },
                            new Button
                            {
                                Classes = "commonButton",
                                MarginRight = 153,
                                MarginBottom = 25,
                                Width = 64,
                                Height = 26,
                                BorderFill = "207,207,207",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Content = this.buttonStr,
                                Background = "#fff",
                                Commands=
                                {
                                    {
                                        nameof(Button.Click),
                                        (s,e)=>  this.Btn_ok_Click()
                                    }
                                }
                            }
                        }
                    }
                }
            });
            //加载样式文件，文件需要设置为内嵌资源
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));
            if (!DesignMode)//设计模式下不执行，也可以用#if !DesignMode
            {
                this.pic = this.FindPresenterByName<Picture>(nameof(this.pic));
            }
        }

        private void Btn_cancel_Click()
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Btn_ok_Click()
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
