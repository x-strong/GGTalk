using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using GGTalk;
using GGTalk.Linux;
using GGTalk.Linux.Models;
using GGTalk.Linux.Controls;
using GGTalk.Linux.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TalkBase;

namespace GGTalk.Linux.Views
{
    internal class GroupInfoWindow : BaseWindow
    {
        private Collection<GGUserPlus> list = new Collection<GGUserPlus>();
        private GroupDetailUserListModel groupDetailUserListModel = new GroupDetailUserListModel();
        private TextBlock tb_ID, tb_Name, tb_GroupType, tb_CreatorName, tb_Announce;
        private GGGroup currentGroup;
        public GroupInfoWindow(GGGroup _group)
        {
            this.groupDetailUserListModel.UserList = new Collection<GGUserPlus>();
            InitializeComponent();
            this.currentGroup = _group;
            this.InitMemberList();
            this.InitGroupDetail();
        }
        private bool initialized = false;
        protected override void InitializeComponent()
        {
            if (this.initialized) { return; }
            this.Icon = GlobalResourceManager.Png64;
            this.initialized = true;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "群资料";
            Width = 420;
            Height = 600;
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
                                MarginLeft = 24,
                                Text = "群资料",
                            },
                            new Panel
                            {
                                MarginRight = 80f,
                                MarginTop = 0f,
                                ToolTip="最小化",
                                Name="min",
                                Width = 30f,
                                Height = 30f,
                                Children =
                                {
                                    new Line
                                    {
                                        MarginLeft=8,
                                        MarginTop=2,
                                        StartPoint = new Point(1, 13),
                                        EndPoint = new Point(14, 13),
                                        StrokeStyle = "2",
                                        IsAntiAlias=true,
                                        StrokeFill=color
                                    },
                                },
                                Commands =
                                {
                                    {
                                        nameof(Button.MouseUp),
                                        (s,e)=>
                                        {
                                            (e as MouseButtonEventArgs).Handled = true;
                                            this.WindowState = WindowState.Minimized;
                                        }
                                    }
                                },
                                Triggers=
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
                            new Button
                            {
                                MarginRight = 40f,
                                MarginTop = 0f,
                                ToolTip="最大化",
                                Name="min",
                                Width = 30f,
                                Height = 30f,
                                Content= new Picture
                                {
                                    Stretch = Stretch.Fill,
                                    Width = 24,
                                    Height = 24,
                                    Source = CommonOptions.ResourcesCatalog+ "btn_max_normal.png",
                                },
                                Background = Color.Transparent,
                                BorderThickness = new Thickness(0,0,0,0),
                                BorderType = BorderType.BorderThickness,
                                Commands=
                                {
                                    {
                                        nameof(Button.MouseUp),
                                        (s,e)=>
                                        {
                                            this.IsFullScreen = !IsFullScreen;
                                        }
                                    }
                                }
                            },
                            new Panel
                            {
                                MarginRight = 0f,
                                MarginTop = 0f,
                                Name = "close",
                                ToolTip = "关闭",
                                Width = 30f,
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
                            }
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
                        MarginTop = 30,
                        Background = "201, 228, 255",
                        Children =
                        {
                            new TextBlock
                            {
                                MarginLeft = 21,
                                MarginTop = 10,
                                Text = "群账号："
                            },
                            new TextBlock
                            {
                                Name=nameof(this.tb_ID),
                                PresenterFor=this,
                                MarginLeft = 90,
                                MarginTop = 10,
                                Text = "10000_3529"
                            },
                            new TextBlock
                            {
                                MarginBottom = 115,
                                MarginLeft = 21,
                                MarginTop = 36,
                                Text = "群名称："
                            },
                            new TextBlock
                            {
                                Name=nameof(this.tb_Name),
                                PresenterFor=this,
                                MarginBottom = 112,
                                MarginLeft = 90,
                                MarginTop = 36,
                                Text = "人事部"
                            },
                            new TextBlock
                            {
                                MarginLeft = 21,
                                MarginTop = 62,
                                Text = "群类型："
                            },
                            new TextBlock
                            {
                                Name=nameof(this.tb_GroupType),
                                PresenterFor=this,
                                MarginLeft = 90,
                                MarginTop = 62,
                                Text = "普通群"
                            },
                            new TextBlock
                            {
                                MarginBottom = 60,
                                MarginLeft = 21,
                                MarginTop = 88,
                                Text = "群管理："
                            },
                            new TextBlock
                            {
                                Name=nameof(this.tb_CreatorName),
                                PresenterFor=this,
                                MarginLeft = 90,
                                MarginTop = 88,
                                Text = "傲瑞客服"
                            },
                            new TextBlock
                            {
                                MarginBottom = 40,
                                MarginTop = 114,
                                MarginLeft = 21,
                                Text = "群公告："
                            },
                            new TextBlock
                            {
                                Name=nameof(this.tb_Announce),
                                PresenterFor=this,
                                MarginLeft = 90,
                                MarginTop = 114,
                                Text = "努力学习"
                            },
                            new TextBlock
                            {
                                MarginLeft = 21,
                                MarginTop = 137,
                                Text = "群成员："
                            }
                        }
                    },
                    new Panel
                    {
                        MarginBottom = 0,
                        Height = 395,
                        Width = "100%",
                        Children =
                        {
                            new ScrollViewer
                                    {
                                        Width = "100%",
                                        Height = "100%",
                                        Content = new  WrapPanel{
                                            Width ="100%",
                                            MarginTop = 0,
                                            Children =
                                            {

                                                new GroupDetailListBox
                                                {
                                                    Size = SizeField.Fill,
                                                    ItemTemplate = new GroupDetailListBoxItem(){
                                                        Commands={ { nameof(DoubleClick),(s,e)=> {
                                                            if(this.currentGroup.IsPrivate){ return; }
                                                            GGUser ggUser= s.DataContext as GGUser;
                                                            if(ggUser==null){ return; }
                                                            this.UserPanelControl_DoubleClicked(ggUser);

                                                        } } }
                                                    
                                                    },
                                                    Bindings =
                                                    {
                                                        {
                                                            nameof(ListBox.Items),
                                                            nameof(GroupDetailUserListModel.UserList)
                                                        }
                                                    },

                                                }
                                            }
                                        }
                            }
                        }
                    }
                }
            });
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));
            //加载样式文件，文件需要设置为内嵌资源

            if (!DesignMode)//设计模式下不执行，也可以用#if !DesignMode
            {
                this.tb_ID = this.FindPresenterByName<TextBlock>("tb_ID");
                this.tb_Name = this.FindPresenterByName<TextBlock>("tb_Name");
                this.tb_GroupType = this.FindPresenterByName<TextBlock>("tb_GroupType");
                this.tb_CreatorName = this.FindPresenterByName<TextBlock>("tb_CreatorName");
                this.tb_Announce = this.FindPresenterByName<TextBlock>("tb_Announce");

            }
        }

        private void InitMemberList()
        {
            //CommModel commModel = new CommModel();
            foreach (string memberID in currentGroup.MemberList)
            {
                GGUser ggUser = Program.ResourceCenter.ClientGlobalCache.GetUser(memberID);
                this.groupDetailUserListModel.UserList.Add(new GGUserPlus(ggUser));
            }
            this.DataContext = this.groupDetailUserListModel;
        }

        private void UserPanelControl_DoubleClicked(GGUser ggUser)
        {
            if (ggUser == null) { return; }
            UserInfoWindow userInfoWindow = new UserInfoWindow(ggUser);
            userInfoWindow.Show_Topmost();
        }

        private void InitGroupDetail()
        {
            this.tb_ID.Text = currentGroup.ID.StartsWith(FunctionOptions.PrefixGroupID) ? currentGroup.ID.Remove(0, 1) : currentGroup.ID;
            this.tb_Name.Text = currentGroup.Name;
            this.tb_CreatorName.Text = Program.ResourceCenter.ClientGlobalCache.GetUserName(currentGroup.CreatorID);
            this.tb_Announce.Text = currentGroup.Announce;
            this.tb_GroupType.Text = currentGroup.IsPrivate ? "密聊群" : "普通群";
        }
    }
}
