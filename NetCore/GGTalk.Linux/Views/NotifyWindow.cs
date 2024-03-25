using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using GGTalk.Linux;
using GGTalk.Linux.Controller;
using GGTalk.Linux.Views;
using GGTalk.Linux.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TalkBase;

namespace GGTalk.Linux.Views
{
    internal class NotifyWindow : Window, INotifyForm
    {
        private NotifyListPanel2 notifyListPanel_User, notifyListPanel_Group;
        private NoticeTabControl tabControl;
        private int totalEntityCount = 0;
        protected override void InitializeComponent()
        {
            this.Icon = GlobalResourceManager.Png64;
            this.notifyListPanel_User = new NotifyListPanel2(TalkBase.NotifyType.User)
            {
                Name = nameof(this.notifyListPanel_User),
                PresenterFor = this,
                Size = SizeField.Fill,
                MarginTop=0,
            };
            this.notifyListPanel_Group = new NotifyListPanel2(TalkBase.NotifyType.Group)
            {
                Name = nameof(this.notifyListPanel_Group),
                PresenterFor = this,
                Size = SizeField.Fill,
                MarginTop = 0,
            };
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "验证消息";
            Width = 600;
            Height = 608;
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
                                Text = "验证消息",
                                Foreground = "#fff"
                            },
                            new Panel
                            {
                                MarginRight = 0,
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
                        }
                    },
                    new NoticeTabControl
                    {
                        Name=nameof(this.tabControl),
                        PresenterFor=this,
                        Width = "100%",
                        Height = 568,
                        MarginTop = 30,
                        Items =
                        {
                            new NoticeTabItem
                            {
                                Foreground = "#fff",
                                FontSize = 14,
                                Size = SizeField.Fill,
                                Header = "好友验证",
                                Content = notifyListPanel_User

                            },
                            new NoticeTabItem
                            {
                                Foreground = "#fff",
                                FontSize = 14,
                                Size = SizeField.Fill,
                                Header = "讨论组验证",
                                Content = notifyListPanel_Group

                            },
                        }
                    }
                }
            }) ;
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));
            //加载样式文件，文件需要设置为内嵌资源

            if (!DesignMode)//设计模式下不执行，也可以用#if !DesignMode
            {
                this.tabControl = this.FindPresenterByName<NoticeTabControl>(nameof(this.tabControl));
                MainWindow.MessageCacheManager.PickoutNotifyMessage("");
                Program.ResourceCenter.ClientOutter.GetAddFriendPage();
                Program.ResourceCenter.ClientOutter.GetAddGroupPage();
            }
        }

        public void ShowTabPage(NotifyType notifyType)
        {
            if (notifyType == NotifyType.User)
            {
                this.tabControl.SelectedIndex = 0;
            }
            else if (notifyType == NotifyType.Group)
            {
                this.tabControl.SelectedIndex = 1;
            }
        }

        #region INotifyForm
        public void OnAddFriendRequestReceived(string requesterID, string comment)
        {
            //若未处理的申请记录已存在过滤掉新的请求。先从容器中删除
            //this.notifyListPanel_User.RemoveNotifyItem4Requesting(requesterID);
            AddFriendRequest addFriendRequest = new AddFriendRequest()
            {
                RequesterID = requesterID,
                RequesterCatalogName = "",
                AccepterID = Program.ResourceCenter.CurrentUserID,
                AccepterCatalogName = "",
                Comment2 = comment,
                State = (byte)RequsetType.Request,
                Notified = true,
                CreateTime = DateTime.Now
            };
            this.notifyListPanel_User.AddNotifyItem(addFriendRequest, true);
        }

        public void OnHandleAddFriendRequestReceived(string friendID,bool isRequester, bool IsAgreed)
        {
            this.notifyListPanel_User.OnHandleAddFriendRequestReceived(friendID, isRequester, IsAgreed);
            //NotifyItemPanel handledRequestPanel = this.notifyListPanel_User.GetNotifyItemPanel4Requesting(friendID, false);
            //if (handledRequestPanel == null)
            //{
            //    return;
            //}
            //handledRequestPanel.OnRequestStateChanged(IsAgreed);
        }

        public void OnAddFriendRequestPageReceived(AddFriendRequestPage page)
        {
            if (page == null)
            {
                return;
            }
            this.totalEntityCount = page.TotalEntityCount;
            foreach (AddFriendRequest addFriendRequest in page.AddFriendRequestList)
            {
                this.notifyListPanel_User.AddNotifyItem(addFriendRequest);
            }
        }


        public void OnAddGroupRequestReceived(string requesterID, string groupID, string comment)
        {
            //若未处理的申请记录已存在过滤掉新的请求。先从容器中删除
            //this.notifyListPanel_Group.RemoveNotifyItem4Requesting(requesterID, groupID);
            AddGroupRequest addGroupRequest = new AddGroupRequest()
            {
                RequesterID = requesterID,
                GroupID = groupID,
                AccepterID = Program.ResourceCenter.CurrentUserID,
                Comment2 = comment,
                State = (byte)RequsetType.Request,
                Notified = true,
                CreateTime = DateTime.Now
            };
            this.notifyListPanel_Group.AddNotifyItem(addGroupRequest, true);
        }

        public void OnHandleAddGroupRequestReceived(string requesterID, string groupID, bool IsAgreed)
        {
            this.notifyListPanel_Group.OnHandleAddGroupRequestReceived(requesterID, groupID, IsAgreed);
            //NotifyItemPanel handledRequestPanel = this.notifyListPanel_Group.GetNotifyItemPanel4Requesting(requesterID, groupID);
            //if (handledRequestPanel == null)
            //{
            //    return;
            //}
            //handledRequestPanel.OnRequestStateChanged(IsAgreed);
        }

        public void OnAddGroupRequestPageReceived(AddGroupRequestPage page)
        {
            if (page == null)
            {
                return;
            }
            this.totalEntityCount = page.TotalEntityCount;
            foreach (AddGroupRequest addFriendRequest in page.AddGroupRequestList)
            {
                this.notifyListPanel_Group.AddNotifyItem(addFriendRequest);
            }
        }
        #endregion

    }
}
