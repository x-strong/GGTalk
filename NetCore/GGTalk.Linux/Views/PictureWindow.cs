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
    internal class PictureWindow : Window
    {
        private Image image;
        private Panel panel;
        private Picture picture;
        public PictureWindow(Image _image)
        {
            this.image = _image;
        }

        protected override void InitializeComponent()
        {
            Width = 800;
            Height = 600;
            this.Icon = GlobalResourceManager.Png64;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "查看图片";
            Background = "#fff";
            BorderFill = "#619fd7";
            BorderThickness = new Thickness(2, 2, 2, 2);
            BorderType = BorderType.BorderThickness;
            Children.Add(new Grid()
            {
                Children = //内容元素放这里
                {
                    new Panel
                    {
                        MarginTop = 0, 
                        Height = 30,
                        Width = "100%",
                        Background = "#619fd7", 
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
                                Text = "查看图片",
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

                    new Picture
                    {
                        MarginTop = 30,
                        Width = 800,
                        Height = 570,
                        Name=nameof(this.picture),
                        PresenterFor=this,
                        Stretch = Stretch.Uniform,
                    }
                }
            });
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));
            //加载样式文件，文件需要设置为内嵌资源

            if (!DesignMode)//设计模式下不执行，也可以用#if !DesignMode
            {
                this.picture = this.FindPresenterByName<Picture>(nameof(this.picture));
                this.panel = this.FindPresenterByName<Panel>(nameof(this.panel));
                this.picture.Source = this.image;  
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

        } 
    }
}
