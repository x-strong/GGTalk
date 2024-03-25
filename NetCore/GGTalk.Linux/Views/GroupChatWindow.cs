using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using ESFramework.Boost.NetworkDisk.Passive;
using ESPlus.Serialization;
using GGTalk;
using GGTalk.Linux;
using GGTalk.Linux.Controller;
using GGTalk.Linux.Controls;
using GGTalk.Linux.Helpers;
using GGTalk.Linux.Models;
using GGTalk.Linux.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkBase;

namespace GGTalk.Linux.Views
{
    internal class GroupChatWindow : Window, IGroupChatForm<GGUser>
    {
        private ChatPanel chatPanel;
        private GGGroup currentGroup;
        private bool isCreator = false;
        private UserInGroupBox userInGroupBox;
        private ChatTabControl tabControl;
        private bool nDiskConnected = false;
        private TabItem tabItem_file;
        private NDiskBrowser nDiskBrowser;
        private Button btn_allGroupBan, btn_editGroupMember;
        public event Action<IGroup> DeleteGroupClicked;
        public event Action<string> ExitGroupClicked;
        public event Action<string> GroupMemberClicked;
        

        public GroupChatWindow(string groupID)
        {
            this.currentGroup = Program.ResourceCenter.ClientGlobalCache.GetGroup(groupID);
            this.Title = "群聊天 - " +this.currentGroup.Name;
            this.isCreator = Program.ResourceCenter.CurrentUserID == this.currentGroup.CreatorID;
        }


        private void BindDataContext()
        {
            this.DataContext = this.currentGroup;
        }       
        

        protected override void InitializeComponent()
        {
            this.Icon = GlobalResourceManager.Png64;
            CanResize = true;
            CommandContext = null;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Width = 800;
            Height = 600;
            MinWidth = 800;
            MinHeight = 600;
            Background = null;
            Children.Add(new Panel
            {
                BorderFill = "97,159,215",
                BorderThickness = new Thickness(2,2,2,2),
                BorderType = BorderType.BorderThickness,
                ClipToBounds = true,
                Background = "#fff",
                MarginRight = 0f,
                Width = "100%",
                Height = "100%",
                Children = //内容元素放这里
                {
                    new Grid
                    {
                        Size = SizeField.Fill,
                        ColumnDefinitions =
                        {
                            new ColumnDefinition
                            {

                            },
                            new ColumnDefinition
                            {
                                Width = 210
                            },
                        },
                        RowDefinitions =
                        {
                            new RowDefinition
                            {
                                Height = 72
                            },
                            new RowDefinition
                            {

                            }
                        },
                        Children =
                        {
                            new Grid
                            {
                                Size = SizeField.Fill,
                                Background = "#619fd7",
                                Height = 104,
                                Children =
                                {
                                    new Button
                                    {
                                        MarginTop = 12,
                                        Classes = "BlueButton",
                                        MarginLeft = 12,
                                        Cursor = Cursors.Hand,
                                        Content = new Picture
                                        {
                                            Width = 40,
                                            Height = 40,
                                            Stretch = Stretch.Fill,
                                            Source = CommonOptions.ResourcesCatalog+ "Group1.png",
                                        },
                                        Commands={ { nameof(Button.Click),(s,e)=> { this.GroupInfo_Click(); } } }
                                    },
                                    new TextBlock
                                    {
                                        MarginLeft = 81,
                                        MarginTop = 12,
                                        Text = "美术组",
                                        Name = "GroupName",
                                        FontSize = 20,
                                        Bindings={ { nameof(TextBlock.Text),nameof(GGGroup.Name) } },
                                        Commands={ { nameof(MouseDown),(s,e)=> { this.GroupName_Click(); } } }
                                    },
                                    //new TextBlock
                                    //{
                                    //    MarginLeft = 160,
                                    //    MarginTop = 14,
                                    //    Text = "2020-08-18",
                                    //    Name = "CreationDate",
                                    //    FontSize = 18,
                                    //    Bindings={ { nameof(TextBlock.Text),nameof(GGGroup.CreateTime),null,BindingMode.OneWay, x=> ((DateTime)x).ToString("d") } }
                                    //},
                                    new TextBlock
                                    {
                                        MarginLeft = 81,
                                        MarginTop = 39,
                                        Text = "庆祝群成立二十五周年！！",
                                        Name = "notice",
                                        FontSize = 12,
                                        Bindings={ { nameof(TextBlock.Text),nameof(GGGroup.Announce) } }
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
                                    new Panel
                                    {
                                        ToolTip="最大化",
                                        Name="max",
                                        Width = 30f,
                                        Height = 30f,
                                        MarginTop = 0,
                                        MarginRight = 35,
                                        Children=
                                        {
                                            new Rectangle
                                            {
                                                Width=14,
                                                Height=12,
                                                StrokeStyle="2",
                                                StrokeFill = color
                                            }
                                        },
                                        Commands =
                                        {
                                            {
                                                nameof(Button.MouseUp),
                                                (s,e)=>
                                                {
                                                    (e as MouseButtonEventArgs).Handled = true;
                                                    this.WindowState= WindowState.Maximized;
                                                }
                                            }
                                        },
                                        Bindings =
                                        {
                                            {
                                                nameof(Border.Visibility),
                                                nameof(Window.WindowState),
                                                this,
                                                BindingMode.OneWay,
                                                a => (WindowState)a == WindowState.Maximized ? Visibility.Collapsed : Visibility.Visible
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
                                    new Panel
                                    {
                                        ToolTip="向下还原",
                                        Name="nor",
                                        Width = 30f,
                                        Height = 30f,
                                        MarginTop = 0,
                                        MarginRight = 35,
                                        Children=
                                        {
                                            new Rectangle
                                            {
                                                MarginTop=15,
                                                MarginLeft=8,
                                                Width=11,
                                                Height=8,
                                                StrokeStyle="1.5",
                                                StrokeFill = color
                                            },
                                            new Polyline
                                            {
                                                MarginTop=11,
                                                MarginLeft=12,
                                                Points=
                                                {
                                                    new Point(0,3),
                                                    new Point(0,0),
                                                    new Point(9,0),
                                                    new Point(9,7),
                                                    new Point(6,7)
                                                },
                                                StrokeFill = color,
                                                StrokeStyle="2"
                                            }
                                        },
                                        Commands =
                                        {
                                            {
                                                nameof(Button.MouseUp),
                                                (s, e) =>
                                                {
                                                    (e as MouseButtonEventArgs).Handled = true;
                                                    this.WindowState = WindowState.Normal;
                                                }
                                            }
                                        },
                                        Bindings =
                                        {
                                            {
                                                nameof(Border.Visibility),
                                                nameof(Window.WindowState),
                                                this,
                                                BindingMode.OneWay,
                                                a => (WindowState)a == WindowState.Normal ? Visibility.Collapsed : Visibility.Visible
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
                                },//设置窗体拖拽
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
                                        Grid.RowIndex,
                                        0
                                    },
                                    {
                                        Grid.ColumnSpan,
                                        2
                                    }
                                }
                            },
                            new ChatTabControl
                            {
                                Name=nameof(this.tabControl),
                                PresenterFor=this,
                                IsEnabled = true,
                                IsHitTestVisible = true,
                                Width="100%",
                                Height="100%",
                                Background = "#fff",
                                Attacheds =
                                {
                                    {
                                        Grid.RowIndex,
                                        1
                                    },
                                    {
                                        Grid.ColumnIndex,
                                        0
                                    }
                                },
                                Items =
                                {
                                    new GroupChatTabItem(FileHelper.FindAssetsBitmap("message1.png"))
                                    {
                                        Header = "聊天",
                                        Size = SizeField.Fill,
                                        Content= new ChatPanel(){
                                            MarginLeft = -1,
                                            MarginBottom = -1,
                                            Name=nameof(this.chatPanel),
                                            PresenterFor=this,
                                            BorderThickness = new Thickness(0,0,2,0),
                                            BorderType  = BorderType.BorderThickness,
                                            BorderFill = "#eee",
                                            Size=SizeField.Fill,
                                        }
                                    },
                                    new GroupChatTabItem(FileHelper.FindAssetsBitmap("Folder.png"))
                                    {
                                        Name=nameof(this.tabItem_file),
                                        PresenterFor=this,
                                        Header = "文件",
                                        Size = SizeField.Fill,
                                        Content=new NDiskBrowser(){
                                            Name=nameof(this.nDiskBrowser),
                                            PresenterFor=this,

                                        }
                                    }
                            },

                        },
                            new Panel
                            {
                                Size = SizeField.Fill,
                                Background  = "#fff",
                                Children  =
                                {
                                    new Grid
                                    {
                                        Width="100%",
                                        Height="100%",
                                        ColumnDefinitions =
                                        {
                                            new ColumnDefinition
                                            {

                                            }
                                        },
                                        RowDefinitions =
                                        {
                                            new RowDefinition
                                            {
                                                Height = 32
                                            },
                                            new RowDefinition
                                            {

                                            }
                                        },
                                        Children =
                                        {
                                            new Panel
                                            {
                                                Size = SizeField.Fill,
                                                MarginTop = 0,
                                                Background = "#619fd7",
                                                Children =
                                                {
   
                                                },
                                                Attacheds =
                                                {
                                                    {
                                                        Grid.RowIndex,
                                                        0
                                                    },
                                                    {
                                                        Grid.ColumnIndex,
                                                        0
                                                    }
                                                }
                                            },
                                            new Panel
                                            {
                                                Size = SizeField.Fill,
                                                Children =
                                                {
                                                    new UserInGroupBox()
                                                    {
                                                        Name=nameof(this.userInGroupBox),
                                                        PresenterFor=this,

                                                    },
                                                    new Button
                                                    {
                                                        Visibility=Visibility.Collapsed,
                                                        Name=nameof(this.btn_allGroupBan),
                                                        PresenterFor=this,
                                                        BorderThickness = new Thickness(0,0,0,0),
                                                        BorderType = BorderType.BorderThickness,
                                                        Background = Color.Transparent,
                                                        MarginRight = 60,
                                                        MarginTop = 5,
                                                        ToolTip = "开启全员禁言",
                                                        Content =new Picture
                                                        {
                                                            Width = 16,
                                                            Height = 16,
                                                            Stretch = Stretch .Fill,
                                                            Source =CommonOptions.ResourcesCatalog+ "allGroupBanTurnOn.png",
                                                        },
                                                        Commands={ { nameof(Button.Click),(s,e)=> { this.AllGroupBan_Click(); } } }
                                                    },
                                                    new Button
                                                    {
                                                        BorderThickness = new Thickness(0,0,0,0),
                                                        BorderType = BorderType.BorderThickness,
                                                        Background = Color.Transparent,
                                                        MarginRight = 35,
                                                        MarginTop = 5,
                                                        ToolTip = "退出",
                                                        Content =new Picture
                                                        {
                                                            Width = 16,
                                                            Height = 16,
                                                            Stretch = Stretch .Fill,
                                                            Source =CommonOptions.ResourcesCatalog+ "signout_icon.png",
                                                        },
                                                        Commands={ { nameof(Button.Click),(s,e)=> { this.ExitGroup_Click(); } } }
                                                    },
                                                    new Button
                                                    {
                                                        Name=nameof(this.btn_editGroupMember),
                                                        PresenterFor=this,
                                                        BorderThickness = new Thickness(0,0,0,0),
                                                        BorderType = BorderType.BorderThickness,
                                                        Background = Color.Transparent,
                                                        MarginRight = 10,
                                                        MarginTop = 5,
                                                        ToolTip = "编辑成员",
                                                        Content =new Picture
                                                        {
                                                            Width = 16,
                                                            Height = 16,
                                                            Stretch = Stretch .Fill,
                                                            Source =CommonOptions.ResourcesCatalog+ "edit_icon.png",
                                                        },
                                                        Commands={ { nameof(Button.Click),(s,e)=> { this.EditGroupMember_Click(); } } }
                                                    },
                                                },
                                                Attacheds =
                                                {
                                                    {
                                                        Grid.RowIndex,
                                                        1
                                                    },
                                                    {
                                                        Grid.ColumnIndex,
                                                        0
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },
                                Attacheds =
                                {
                                    {
                                        Grid.RowIndex,
                                        1
                                    },
                                    {
                                        Grid.ColumnIndex,
                                        1
                                    }
                                },
                            }
                    }
                }
            }
            }) ;
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));

            //加载样式文件，文件需要设置为内嵌资源
            if (!DesignMode)//设计模式下不执行
            {
                this.userInGroupBox = this.FindPresenterByName<UserInGroupBox>(nameof(this.userInGroupBox));
                this.userInGroupBox.Initialize(this.currentGroup, this);
                this.chatPanel = this.FindPresenterByName<ChatPanel>(nameof(this.chatPanel));
                this.chatPanel.Initialize(this.currentGroup);
                this.chatPanel.CloseChatPanel += ChatPanel_CloseChatPanel;
                this.btn_allGroupBan = this.FindPresenterByName<Button>(nameof(this.btn_allGroupBan));
                this.btn_editGroupMember = this.FindPresenterByName<Button>(nameof(this.btn_editGroupMember));
                this.nDiskBrowser = this.FindPresenterByName<NDiskBrowser>(nameof(this.nDiskBrowser));
                this.tabItem_file = this.FindPresenterByName<TabItem>(nameof(this.tabItem_file));
                this.tabControl = this.FindPresenterByName<ChatTabControl>(nameof(this.tabControl));
                this.tabControl.SelectionChanged += TabControl_SelectionChanged;
                if (this.currentGroup.IsPrivate || !this.isCreator)//私密群、非群主不能编辑群成员 
                {
                    this.btn_editGroupMember.Visibility = Visibility.Collapsed;
                }

                this.BindDataContext();

            }
        }


        #region IGroupChatForm
        public string UnitID => this.currentGroup.ID;

        public void FriendInfoChanged(GGUser user)
        {
            this.userInGroupBox.UpdateUser(user);
        }

        public void FriendStateChanged(string friendID, UserStatus newStatus)
        {
            GGUser ggUser = Program.ResourceCenter.ClientGlobalCache.GetUser(friendID);
            if (ggUser == null) { return; }
            ggUser.UserStatus = newStatus;
            this.userInGroupBox.UpdateUser(ggUser);
        }

        public void HandleAllGroupBan()
        {

        }

        public void HandleBePulledIntoGroupNotify(string operatorUserID)
        {
            if (operatorUserID == Program.ResourceCenter.CurrentUserID) { return; }
            GGUser user = Program.ResourceCenter.ClientGlobalCache.GetUser(operatorUserID);
            string msg = string.Format("您被 {0} 邀请加入该讨论组 ...", user.DisplayName);
            this.chatPanel.AppendSysMessage(msg);
        }

        public void HandleGroupBanNotify(string operatorID, double minutes)
        {

        }

        public void HandleGroupChatMessage(string broadcasterID, byte[] info, string tag)
        {
            ChatBoxContent2 content = CompactPropertySerializer.Default.Deserialize<ChatBoxContent2>(info, 0); 
            this.chatPanel.HandleChatMessage(broadcasterID, content, null, tag);
        }

        public void HandleGroupFileUploadedNotify(string sourceUserID, string groupID, string fileName)
        {
            if (this.nDiskConnected)
            {
                if (sourceUserID != Program.ResourceCenter.CurrentUserID)
                {
                    this.nDiskBrowser.RefreshDirectory();
                }
            }
            this.chatPanel.AppendSysMessage(string.Format("{0} 上传了群文件‘{1}’！", Program.ResourceCenter.ClientGlobalCache.GetUserName(sourceUserID), fileName));
        }

        public void HandleOfflineGroupChatMessage(string broadcasterID, byte[] info, DateTime msgOccureTime, string tag)
        {
            ChatBoxContent2 content = CompactPropertySerializer.Default.Deserialize<ChatBoxContent2>(info, 0);
            this.chatPanel.HandleChatMessage(broadcasterID, content, msgOccureTime, tag);
        }

        public void HandleRemoveAllGroupBan()
        {

        }

        public void HandleRemoveGroupBanNotify()
        {

        }

        public void MyInfoChanged(GGUser my)
        {
            this.userInGroupBox.UpdateUser(my);
        }

        public void MyselfOffline()
        {

        }

        public void OnGroupChanged(GroupChangedType type, string operatorID, string userID)
        {
            if (type == GroupChangedType.MemberInfoChanged)
            {
                if (string.IsNullOrEmpty(userID))
                {
                    return;
                }

                GGUser user = Program.ResourceCenter.ClientGlobalCache.GetUser(userID);
                this.FriendInfoChanged(user);
                return;
            }

            if (type == GroupChangedType.SomeoneJoin)
            {
                this.DoJoinGroup(userID, true);
                return;
            }

            if (type == GroupChangedType.SomeoneQuit)
            {
                this.DoQuitGroup(userID, true);
                return;
            }

            if (type == GroupChangedType.GroupInfoChanged)
            {
                this.currentGroup = Program.ResourceCenter.ClientGlobalCache.GetGroup(this.currentGroup.ID);
                this.BindDataContext();
                return;
            }

            if (type == GroupChangedType.MyselfBeRemovedFromGroup)
            {
                MessageBoxEx.Show("您已被移除该讨论组！");
                this.Close();
                return;
            }

            if (type == GroupChangedType.GroupDeleted)
            {
                MessageBoxEx.Show("该讨论组已经被解散！");
                this.Close();
                return;
            }

            if (type == GroupChangedType.SomeoneDeleted)
            {
                this.DoQuitGroup(userID, false);
                return;
            }

            if (type == GroupChangedType.OtherBeRemovedFromGroup)
            {
                this.DoQuitGroup(userID, false);
                return;
            }

            if (type == GroupChangedType.OtherBePulledIntoGroup)
            {
                this.DoJoinGroup(userID, false);
                return;
            }
        }

        private void DoJoinGroup(string userID, bool active)
        {
            GGUser user = Program.ResourceCenter.ClientGlobalCache.GetUser(userID);
            this.userInGroupBox.AddUser(new GGUserPlus(user));
            //this.AddUserItem(user);
            string msg = active ? string.Format("{0}({1})加入了该讨论组！", user.Name, user.UserID) : string.Format("{0}({1})被邀请加入了该讨论组！", user.Name, user.UserID);
            this.chatPanel.AppendSysMessage(msg);
        }

        private void DoQuitGroup(string userID, bool active)
        {
            GGUser user = Program.ResourceCenter.ClientGlobalCache.GetUser(userID);
            this.userInGroupBox.DeleteUser(userID);
            string msg = active ? string.Format("{0}({1})退出了该讨论组！", user.Name, user.UserID) : string.Format("{0}({1})被移除了该讨论组！", user.Name, user.UserID);
            this.chatPanel.AppendSysMessage(msg);
        }

        public void RefreshUI()
        {

        }

        public void UnitCommentNameChanged(IUnit unit)
        {
            this.userInGroupBox.UpdateUser((GGUser)unit);
        }
        #endregion

        #region 群共享
        private void TabControl_SelectionChanged(TabItem obj)
        {
            if (this.tabControl.SelectedItem == this.tabItem_file)
            {
                if (!this.nDiskConnected)
                {
                    NDiskOutter nDiskOutter = new NDiskOutter(Program.ResourceCenter.RapidPassiveEngine.FileOutter, Program.ResourceCenter.RapidPassiveEngine.CustomizeOutter);
                    this.nDiskBrowser.NetDiskID = "GroupShare_" + this.currentGroup.GroupID.Substring(1);
                    this.nDiskBrowser.ParentWindow = this;
                    this.nDiskBrowser.UploadCompleted += new Action<string>(nDiskBrowser1_UploadCompleted);
                    this.nDiskBrowser.LockRootDirectory = true;
                    this.nDiskBrowser.AllowUploadFolder = false;
                    this.nDiskBrowser.Initialize(null, Program.ResourceCenter.RapidPassiveEngine.FileOutter, nDiskOutter, Program.ResourceCenter.CurrentUserID);
                    this.nDiskConnected = true;
                }
            }
        }

        private void nDiskBrowser1_UploadCompleted(string fileName)
        {
            Program.ResourceCenter.ClientOutter.GroupFileUploaded(this.currentGroup.ID, fileName);
            this.chatPanel.AppendSysMessage(string.Format("您上传了群文件‘{0}’！", fileName));
        }

        #endregion


        #region 三个按钮
        //全员禁言
        private void AllGroupBan_Click()
        {

        }
        //退出群组
        private void ExitGroup_Click()
        {
            if (this.currentGroup.CreatorID == Program.ResourceCenter.CurrentUserID)
            {

                if (this.DeleteGroupClicked != null)
                {
                    this.DeleteGroupClicked(this.currentGroup);
                }
                return;
            }

            if (this.ExitGroupClicked != null)
            {
                this.ExitGroupClicked(this.currentGroup.ID);
            }
        }
        //编辑组成员
        private async void EditGroupMember_Click()
        {            
            UserSelectedWindow window = new UserSelectedWindow();
            window.Initialize4Group(this.currentGroup, false);
            Task<object> task = window.ShowDialog_Topmost(this);
            await task.ConfigureAwait(true);
            if (Convert.ToBoolean(task.Result))
            {
                List<string> list = window.UserIDSelected;
                if (!list.Contains(this.currentGroup.CreatorID))
                {
                    list.Add(this.currentGroup.CreatorID);
                }
                Program.ResourceCenter.ClientOutter.ChangeGroupMembers(this.currentGroup.ID, list);
            }
        }

        #endregion

        private void GroupName_Click()
        {
            UpdateGroupInfoWindow updateGroupInfoWindow = new UpdateGroupInfoWindow(this.currentGroup);
            updateGroupInfoWindow.Show_Topmost();
        }

        private void GroupInfo_Click()
        {
            GroupInfoWindow groupInfoWindow = new GroupInfoWindow(this.currentGroup);
            groupInfoWindow.ShowDialog();
        }

        private void ChatPanel_CloseChatPanel()
        {
            this.Close();
        }
    }
}
