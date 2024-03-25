using CPF;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using GGTalk.Linux.Controller;
using GGTalk.Linux.Controls;
using System;
using System.Text.RegularExpressions;
using TalkBase;

namespace GGTalk.Linux.Views
{
    internal class ChatRecordWindow : BaseWindow
    {
        private ChatBox chatBox;
        private ComboBox combox;
        private Button btn_FirstPage, btn_PreviousPage, btn_NextPage, btn_LastPage;
        private int totalPageCount = 1;
        private int currentPageIndex = -1;
        private int pageSize = 25;
        private GGUser friend;
        private GGGroup group;
        private bool isGroupChat = false;
        private IUserNameGetter userNameGetter;
        private IChatRecordGetter CurrentChatRecordGetter
        {
            get
            {
                return Program.ResourceCenter.RemoteChatRecordGetter;
            }
        }
        public ChatRecordWindow(IUser _friend, IUserNameGetter getter)
        {
            this.isGroupChat = false;
            this.friend = (GGUser)_friend;
            this.userNameGetter = getter;
            this.Title = "聊天记录 - " + this.friend.DisplayName;
        }

        public ChatRecordWindow(IGroup gr, IUserNameGetter getter)
        {
            this.isGroupChat = true;
            this.group = (GGGroup)gr;
            this.userNameGetter = getter;
            this.Title = "群消息记录 - " + gr.DisplayName;
        }


        private void ShowLastPage()
        {
            this.ShowRecord(0, false);
        }
        private void ShowRecord(int pageIndex, bool allowCache)
        {
            if (pageIndex != int.MaxValue)
            {
                if (pageIndex + 1 > this.totalPageCount)
                {
                    pageIndex = this.totalPageCount - 1;
                }

                if (pageIndex < 0)
                {
                    pageIndex = 0;
                }
                if (this.currentPageIndex == pageIndex && allowCache)
                {
                    return;
                }
            }

            this.Cursor = Cursors.Wait;
            try
            {
                ChatRecordTimeScope timeScope = ChatRecordTimeScope.All;
                DateTime now = DateTime.Now;
                if (this.combox.SelectedIndex == 0) //一周
                {
                    timeScope = ChatRecordTimeScope.RecentWeek;
                }
                else if (this.combox.SelectedIndex == 1)//一月
                {
                    timeScope = ChatRecordTimeScope.RecentMonth;
                }
                else if (this.combox.SelectedIndex == 2)//三月
                {
                    timeScope = ChatRecordTimeScope.Recent3Month;
                }
                else //全部
                {
                }
                this.chatBox.InitLastMessageTime();

                ChatRecordPage page = null;
                if (this.isGroupChat)
                {
                    page = this.CurrentChatRecordGetter.GetGroupChatRecordPage(timeScope, this.group.ID, this.pageSize, pageIndex);
                }
                else
                {
                    page = this.CurrentChatRecordGetter.GetChatRecordPage(timeScope, Program.ResourceCenter.CurrentUserID, friend.ID, this.pageSize, pageIndex);
                }
                this.chatBox.Clear();

                if (page == null || page.Content.Count == 0)
                {
                    MessageBoxEx.Show("没有消息记录！");
                    return;
                }

                this.currentPageIndex = page.PageIndex;
                for (int i = 0; i < page.Content.Count; i++)
                {
                    ChatMessageRecord record = page.Content[i];
                    byte[] msg = record.Content;

                    if (Program.ResourceCenter.DesEncryption != null)
                    {
                        msg = Program.ResourceCenter.DesEncryption.Decrypt(msg);
                    }

                    ChatBoxContent2 content = ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<ChatBoxContent2>(msg, 0);
                    bool isMe = record.SpeakerID == Program.ResourceCenter.CurrentUserID;
                    string name = this.userNameGetter.GetUserName(record.SpeakerID) ?? record.SpeakerID;
                    if (this.isGroupChat)
                    {


                        this.AppendChatBoxContent4Group(isMe, string.Format("{0}({1})", name, record.SpeakerID), record.OccureTime, content);
                    }
                    else
                    {
                        this.AppendChatBoxContent(isMe, string.Format("{0}({1})", name, record.SpeakerID), record.OccureTime, content);
                    }
                }

                int pageCount = page.TotalCount / this.pageSize;
                if (page.TotalCount % this.pageSize > 0)
                {
                    ++pageCount;
                }
                this.totalPageCount = pageCount;
                this.ScrollToEnd();
                this.SetPageIndexButtonEnabled();
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(ee.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void SetPageIndexButtonEnabled()
        {
            this.btn_FirstPage.IsEnabled = true;
            this.btn_PreviousPage.IsEnabled = true;
            this.btn_NextPage.IsEnabled = true;
            this.btn_LastPage.IsEnabled = true;
            if (this.currentPageIndex == 0)
            {
                this.btn_NextPage.IsEnabled = false;
                this.btn_LastPage.IsEnabled = false;
            }
            if (this.currentPageIndex + 1 == this.totalPageCount)
            {
                this.btn_FirstPage.IsEnabled = false;
                this.btn_PreviousPage.IsEnabled = false;
            }
        }

        private void AppendChatBoxContent(bool isMe, string displayName, DateTime originTime, ChatBoxContent2 content)
        {

            this.chatBox.AppendChatMessage(isMe, content, originTime,null, displayName);
        }

        private void AppendChatBoxContent4Group(bool isMe, string displayName, DateTime speakTime, ChatBoxContent2 content)
        {
            this.chatBox.AppendChatMessage(isMe, content, speakTime,null, displayName);
        }

        #region 内容移动到最底部
        private void ScrollToEnd()
        {
            this.chatBox.ScrollToEnd();   
        }

        #endregion

        protected override void InitializeComponent()
        {
            this.Icon = GlobalResourceManager.Png64;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Width = 800;
            Height = 640;
            Background = null;
            Children.Add(new Panel()
            {
                BorderFill = "203,205,194",
                BorderThickness = new Thickness(2, 2, 2, 2),
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
                      Children =
                        {
                            new Panel
                            {
                                
                                Size = SizeField.Fill,
                                Background = "203,205,194",
                                Children =
                                {
                                    new Picture
                                    {
                                        Margin = "4,4,0,0",
                                        Width = 16, Height = 16,
                                        Stretch = Stretch.UniformToFill,
                                        Source=CommonOptions.ResourcesCatalog +"32.ico",
                                    },
                                    new TextBlock
                                    {
                                        MarginLeft = 35,
                                        MarginTop = 5f,
                                        Text = this.Title,
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
                                            Source =CommonOptions.ResourcesCatalog + "btn_max_normal.png",
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
                                    },
                                    new ComboBox
                                    {
                                        Name=nameof(this.combox),
                                        PresenterFor=this,
                                        MarginLeft = 25,
                                        MarginTop = 40,
                                        Background = "#fff",
                                        Width = 92,
                                        Height = 26,
                                        SelectedIndex = 0,
                                        Items =
                                        {
                                            "最近一周",
                                            "最近一月",
                                            "最近三月",
                                            "全部"
                                        }
                                    }
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
                                        Grid.RowIndex,
                                        0
                                    }
                                }
                            },
                            new Panel
                            {
                                Size = SizeField.Fill,
                                
                                Children =
                                {
                                    new ChatBox
                                    {
                                        PresenterFor = this,
                                        Name = nameof(this.chatBox),
                                        MarginTop = 0,
                                        HScrollBarVisibility= ScrollBarVisibility.Hidden,
                                        IsChatRecord=true,
                                        WordWarp=true,
                                        IsReadOnly = true,
                                        Size = SizeField.Fill,
                                        Background = "#eee",
                                    },
                                },
                                Attacheds =
                                {
                                    {
                                        Grid.RowIndex,
                                        1
                                    }
                                }
                            },
                            new Panel
                            {
                                
                                Background = "173,176,159",
                                Size = SizeField.Fill,
                                Children =
                                {
                                    new Button
                                    {
                                        Name=nameof(this.btn_FirstPage),
                                        PresenterFor=this,
                                        Classes = "chathistoryButton",
                                        BorderThickness = new Thickness(0,0,0,0),
                                        BorderType = BorderType.BorderThickness,
                                        Cursor = Cursors.Hand,
                                        Width = 22,
                                        Height = 22,
                                        MarginRight = 90,
                                        ToolTip = "首页",
                                        Content =
                                        new Picture
                                        {
                                            Stretch = Stretch.Fill,
                                            Source =CommonOptions.ResourcesCatalog+ "home_btn.png",
                                        },
                                        Commands=
                                        {
                                            {
                                                nameof(Button.Click),
                                                (s,e)=>
                                                {
                                                    this.FirstPage_Click();
                                                }
                                            }
                                        }
                                    },
                                    new Button
                                    {
                                        Name=nameof(this.btn_PreviousPage),
                                        PresenterFor=this,
                                        BorderThickness = new Thickness(0,0,0,0),
                                        BorderType = BorderType.BorderThickness,
                                        Classes = "chathistoryButton",
                                        Cursor = Cursors.Hand,
                                        Width = 22,
                                        Height = 22,
                                        MarginRight = 60,
                                        ToolTip = "上一页",
                                        Content =
                                        new Picture
                                        {
                                            Stretch = Stretch.Fill,
                                            Source =CommonOptions.ResourcesCatalog+ "previouspage_btn.png",
                                        },
                                        Commands=
                                        {
                                            {
                                                nameof(Button.Click),
                                                (s,e)=>
                                                {
                                                    this.PreviousPage_Click();
                                                }
                                            }
                                        }
                                    },
                                    new Button
                                    {
                                        Name=nameof(this.btn_NextPage),
                                        PresenterFor=this,
                                        BorderThickness = new Thickness(0,0,0,0),
                                        BorderType = BorderType.BorderThickness,
                                        Classes = "chathistoryButton",
                                        Cursor = Cursors.Hand,
                                        Width = 22,
                                        Height = 22,
                                        MarginRight = 30,
                                        ToolTip = "下一页",
                                        Content =
                                        new Picture
                                        {
                                            Stretch = Stretch.Fill,
                                            Source =CommonOptions.ResourcesCatalog+ "nextpage_btn.png",
                                        },
                                        Commands=
                                        {
                                            {
                                                nameof(Button.Click),
                                                (s,e)=>
                                                {
                                                    this.NextPage_Click();
                                                }
                                            }
                                        }
                                    },
                                    new Button
                                    {
                                        Name=nameof(this.btn_LastPage),
                                        PresenterFor=this,
                                        BorderThickness = new Thickness(0,0,0,0),
                                        BorderType = BorderType.BorderThickness,
                                        Classes = "chathistoryButton",
                                        Cursor = Cursors.Hand,
                                        Width = 22,
                                        Height = 22,
                                        MarginRight = 2,
                                        ToolTip = "末页",
                                        Content =
                                        new Picture
                                        {
                                            Stretch = Stretch.Fill,
                                            Source =CommonOptions.ResourcesCatalog+ "last_btn.png",
                                        },
                                        Commands=
                                        {
                                            {
                                                nameof(Button.Click),
                                                (s,e)=>
                                                {
                                                    this.LastPage_Click();
                                                }
                                            }
                                        }
                                    }
                                },
                                Attacheds =
                                {
                                    {
                                        Grid.RowIndex,
                                        2
                                    }
                                }
                            }
                        },
                      RowDefinitions =
                        {
                            new RowDefinition
                            {
                                Height = 76,
                            },
                            new RowDefinition
                            {
                                
                            },
                            new RowDefinition
                            {
                                Height = 30,
                            }
                        }
                    },
                    

                }
            });
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));
            //加载样式文件，文件需要设置为内嵌资源


            if (!DesignMode)//设计模式下不执行，也可以用#if !DesignMode
            {
                this.btn_FirstPage = this.FindPresenterByName<Button>(nameof(this.btn_FirstPage));
                this.btn_PreviousPage = this.FindPresenterByName<Button>(nameof(this.btn_PreviousPage));
                this.btn_NextPage = this.FindPresenterByName<Button>(nameof(this.btn_NextPage));
                this.btn_LastPage = this.FindPresenterByName<Button>(nameof(this.btn_LastPage));
                this.chatBox = this.FindPresenterByName<ChatBox>(nameof(this.chatBox));
                this.combox = this.FindPresenterByName<ComboBox>(nameof(this.combox));
                this.combox.SelectedIndex = 0;
                this.combox.SelectionChanged += Combox_SelectionChanged;
                //this.ShowLastPage();
            }
        }


        private void Tv_id_TextInput(object sender, TextInputEventArgs e)
        {
            //如果输入的不是退格和数字，则屏蔽输入
            Regex regex = new Regex("^[0-9]*$");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }


        private void Combox_SelectionChanged(object sender, EventArgs e)
        {
            this.ShowLastPage();
        }


        #region 分页按钮
        private void NextPage_Click()
        {
            this.ShowRecord(this.currentPageIndex - 1);
        }

        private void PreviousPage_Click()
        {
            this.ShowRecord(this.currentPageIndex + 1);
        }

        private void LastPage_Click()
        {
            this.ShowRecord(0);
        }

        private void FirstPage_Click()
        {
            this.ShowRecord(this.totalPageCount - 1);            
        }

        private void ShowRecord(int pageIndex)
        {
            this.ShowRecord(pageIndex, true);
        }
        #endregion
    }
}
