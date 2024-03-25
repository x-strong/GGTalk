using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using ESFramework.Boost.NetworkDisk.Passive;
using ESPlus.Application.FileTransfering.Passive;
using GGTalk.Linux;
using GGTalk.Linux.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGTalk.Linux.Views
{
    internal class NDiskWindow : Window
    {

        private NDiskBrowser nDiskBrowser;
        public NDiskWindow(IFileOutter _fileOutter, INDiskOutter _diskOutter, string _curUserID)
        {
            InitializeComponent();
            this.nDiskBrowser.Initialize(null, _fileOutter, _diskOutter, _curUserID);       
        }


        private bool initialized = false;
        protected override void InitializeComponent()
        {
            if (this.initialized) { return; }
            this.initialized = true;
            this.Icon = GlobalResourceManager.Png64;
            CanResize = true;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "我的网盘";
            Width = 800;
            Height = 450;
            MinWidth = 800;
            MinHeight = 450;
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
                Children = {
                    new Grid
                    {
                        Size = SizeField.Fill,
                        ColumnDefinitions =
                        {
                            new ColumnDefinition
                            {

                            }
                        },
                        RowDefinitions = {
                            new RowDefinition
                            {
                                Height = 30,
                            },
                            new RowDefinition
                            {
                            }
                        },
                        Children =
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
                                        MarginLeft = 24,
                                        Text = "我的网盘",
                                    },
                                    new Panel
                                    {
                                        MarginRight = 1f,
                                        MarginLeft = "Auto",
                                        MarginTop = -3f,
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
                                },
                                Attacheds =
                                {
                                    {
                                        Grid.ColumnIndex,
                                        0
                                    },
                                    {
                                        Grid.RowIndex,
                                        0
                                    }
                                }
                            },
                            new NDiskBrowser(){
                                Name="nDiskBrowser",
                                PresenterFor=this,
                                Size=SizeField.Fill,
                                Attacheds =
                                {
                                    {
                                        Grid.ColumnIndex,
                                        0
                                    },
                                    {
                                        Grid.RowIndex,
                                        1
                                    }
                                }
                            }

                        }
                }

            }
            });
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));
            if (!DesignMode)//设计模式下不执行，也可以用#if !DesignMode
            {
                this.nDiskBrowser = this.FindPresenterByName<NDiskBrowser>("nDiskBrowser");
                this.nDiskBrowser.ParentWindow = this;
                this.nDiskBrowser.AllowUploadFolder = false;
                this.nDiskBrowser.LockRootDirectory = false;
            }
        }
    }
}
