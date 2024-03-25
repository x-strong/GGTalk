using CPF;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using ESBasic;
using ESFramework;
using ESFramework.Boost;
using ESFramework.Boost.NetworkDisk;
using ESPlus.FileTransceiver;
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
using System.Text;
using TalkBase;

namespace GGTalk.Linux.Views
{
    internal class FriendChatWindow:Window, IFriendChatForm<GGUser>
    {
        private bool init;
        private GGUser currentFriend;
        private ChatPanel chatPanel;
        private Panel userInfoPanel;
        private TabControl skinTabControl;
        public const string Token4ConvertToOfflineFile = "转为离线发送";
        private FileTransferingViewer fileTransferingViewer;
        private bool isInHisBlackList = false;//是否在对方的黑名单中

        public FriendChatWindow(string friendID)
        {
            this.currentFriend = Program.ResourceCenter.ClientGlobalCache.GetUser(friendID);
            this.Initialized += FriendChatWindow_Initialized;
            this.Closing += FriendChatWindow_Closing; 
        }

        private void FriendChatWindow_Closing(object sender, ClosingEventArgs e)
        {
            this.fileTransferingViewer.BeforeDispose();
        }

        private void FriendChatWindow_Initialized(object sender, EventArgs e)
        {
            if (Program.ResourceCenter.Connected)
            {
                this.isInHisBlackList = Program.ResourceCenter.ClientOutter.IsInHisBlackList(this.currentFriend.ID);
            }
        }

        protected override void InitializeComponent()
        { 
            this.Icon = GlobalResourceManager.GetHeadImage(this.currentFriend);
            CanResize = true;
            Size = SizeField.Fill;
            DataContext = null;
            CommandContext = this;
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
                                Height = 90
                            },
                            new RowDefinition
                            {

                            },
                        },
                        Children =
                        {
                            new Panel
                            {
                                Size = SizeField.Fill,
                                Background = "#619fd7",
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
                                            Source = CommonOptions.ResourcesCatalog +"8.png",
                                            Bindings={ { nameof(Picture.Source),nameof(GGUserPlus.HeadImage)} }                                           
                                        },
                                        Commands={ { nameof(Button.Click),(s,e)=> {
                                            UserInfoWindow userInfoWindow = new UserInfoWindow(this.currentFriend);
                                            userInfoWindow.Show_Topmost(); } 
                                            } }
                                    },
                                    new TextBlock
                                    {
                                        MarginLeft = 81,
                                        MarginTop = 12,
                                        Text = "李建平",
                                        Name = "NickName",
                                        FontSize = 20,
                                        Bindings={ { nameof(TextBlock.Text),nameof(GGUserPlus.DisplayName)} }
                                    },
                                    new TextBlock
                                    {
                                        MarginLeft = 81,
                                        MarginTop = 39,
                                        Text = "个性签名",
                                        Name = "Signature",
                                        FontSize = 12,
                                        Bindings={ { nameof(TextBlock.Text),nameof(GGUserPlus.Signature)} }
                                    },
                                    new Button
                                    {
                                        Classes = "BlueButton",
                                        MarginLeft = 6,
                                        MarginTop = 60,
                                        Cursor = Cursors.Hand,
                                        ToolTip = "发送文件",
                                        Content =
                                        new Picture
                                        {
                                            Width = 29,
                                            Height = 26,
                                            Stretch = Stretch.UniformToFill,
                                            Source = CommonOptions.ResourcesCatalog +"up_btn_icon.png",
                                            Commands={ { nameof(MouseDown),(s,e)=>this.ClickSendFile() } }
                                        }
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
                                },
                                //设置窗体拖拽
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
                            new TabControl
                            {
                                Name=nameof(this.skinTabControl),
                                PresenterFor=this,
                                Size = SizeField.Fill,
                                Background = "#fff",
                                Visibility =Visibility.Visible,
                                Items =
                                {
                                    new TabItem{
                                        Header="文件传输",
                                        Content= new FileTransferingViewer
                                        {
                                            Name = nameof(this.fileTransferingViewer),
                                            PresenterFor = this,
                                            Size = SizeField.Fill,
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
                                }
                            },
                            new Panel
                            {
                                Name=nameof(this.userInfoPanel),
                                PresenterFor=this,
                                Visibility = Visibility.Visible,
                                Size = SizeField.Fill,
                                Background = "#fff",
                                Children =
                                {
                                    new Picture
                                    {
                                        MarginLeft = 43,
                                        MarginTop = 55,
                                        Width = 120,
                                        Height = 120,
                                        Source = CommonOptions.ResourcesCatalog + "8.png",
                                        Bindings={ { nameof(Picture.Source),nameof(GGUserPlus.HeadImage) } }
                                    },
                                    new TextBlock
                                    {
                                        MarginLeft = 31,
                                        MarginTop = 202,
                                        Text = "账号：10003",
                                        FontSize = 13,
                                        Bindings={ { nameof(TextBlock.Text),nameof(GGUserPlus.ID), null, BindingMode.OneWay, x => "账号："+x } }
                                    },
                                    new TextBlock
                                    {
                                        MarginLeft = 31,
                                        MarginTop = 237,
                                        Text = "名称：李建平",
                                        FontSize = 13,
                                        Bindings={ { nameof(TextBlock.Text),nameof(GGUserPlus.Name), null, BindingMode.OneWay, x => "名称：" + x } }
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
                                        1
                                    }
                                }
                            },
                            new ChatPanel(){
                                Name=nameof(this.chatPanel),
                                PresenterFor=this,
                                Size=SizeField.Fill,
                                BorderThickness = new Thickness(0,0,2,0),
                                BorderType  = BorderType.BorderThickness,
                                BorderFill = "#eee",
                                Attacheds = {
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
                }
            });
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));

            //加载样式文件，文件需要设置为内嵌资源
            if (!DesignMode)//设计模式下不执行
            {
                this.skinTabControl = this.FindPresenterByName<TabControl>(nameof(this.skinTabControl));
                this.userInfoPanel = this.FindPresenterByName<Panel>(nameof(this.userInfoPanel));
                this.fileTransferingViewer = this.FindPresenterByName<FileTransferingViewer>(nameof(this.fileTransferingViewer));
                //文件传送
                this.fileTransferingViewer.Initialize(Program.ResourceCenter.RapidPassiveEngine.FileOutter, new ESBasic.Func<TransferingProject, bool>(this.FilterTransferingProject), this);
                this.fileTransferingViewer.FileTransDisruptted += new CbGeneric<string, bool, FileTransDisrupttedType, string>(fileTransferingViewer1_FileTransDisruptted);
                //this.fileTransferingViewer.FileTransCompleted += new CbGeneric<string, bool ,string,bool>(fileTransferingViewer1_FileTransCompleted);
                this.fileTransferingViewer.FileTransCompleted2 += new CbGeneric<TransferingProject>(fileTransferingViewer_FileTransCompleted2);
                this.fileTransferingViewer.FileResumedTransStarted += new CbGeneric<string, bool>(fileTransferingViewer1_FileResumedTransStarted);
                this.fileTransferingViewer.AllTaskFinished += new CbSimple(fileTransferingViewer1_AllTaskFinished);
                this.fileTransferingViewer.FileNeedOfflineSend += new CbGeneric<TransferingProject>(fileTransferingViewer_FileNeedOfflineSend);

                this.chatPanel = this.FindPresenterByName<ChatPanel>(nameof(this.chatPanel)); 
                this.chatPanel.Initialize(this.currentFriend);
                this.chatPanel.CloseChatPanel += ChatPanel_CloseChatPanel;
                this.FriendInfoChanged(this.currentFriend);//绑定DataContext
            }
        }

        private void ChatPanel_CloseChatPanel()
        {
            this.Close();
        }

        #region 按钮处理事件

        //点击视频聊天
        private void ClickSendVideoChat()
        {
        }

        //点击语音聊天
        private void ClickSendAudioChat()
        {
        }

        //点击远程控制
        private void ClickSendRemoteControl()
        {
        }

        //点击发送文件按钮
        private void ClickSendFile()
        {
            this.SendFileOrFolder(false);
        }
        #endregion

        #region 文件传送相关事件

        private void fileTransferingViewer_FileNeedOfflineSend(TransferingProject project)
        {
            try
            {
                Program.ResourceCenter.RapidPassiveEngine.FileOutter.CancelTransfering(project.ProjectID, FriendChatWindow.Token4ConvertToOfflineFile);

                string projectID;
                SendingFileParas sendingFileParas = new SendingFileParas(20480, 5);//文件数据包大小，可以根据网络状况设定，局网内可以设为204800，传输速度可以达到30M/s以上；公网建议设定为2048或4096或8192
                Program.ResourceCenter.RapidPassiveEngine.FileOutter.BeginSendFile(null, project.OriginPath, Comment4OfflineFile.BuildComment(this.currentFriend.UserID, Program.ResourceCenter.CurrentClientType), sendingFileParas, out projectID);
                this.FileRequestReceived(projectID);
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(GlobalResourceManager.SoftwareName, ee.Message);
            }
        }

        private void SetRightPanel()
        {

        }

        private void fileTransferingViewer1_AllTaskFinished()
        {

            this.ResetTabControlVisible(false);
        }

        private void ResetTabControlVisible(bool showTabControl)
        {
            if (!showTabControl)
            {
                this.skinTabControl.Visibility = Visibility.Hidden;
                //this.scrollViewer.IsVisible = false;
                this.userInfoPanel.Visibility = Visibility.Visible;
            }
            else
            {
                this.userInfoPanel.Visibility = Visibility.Hidden; ;
                //this.scrollViewer.IsVisible = true;
                this.skinTabControl.Visibility = Visibility.Visible;
            }
        }

        private void fileTransferingViewer1_FileResumedTransStarted(string projectName, bool isSender)
        {
            string showText = string.Format("正在续传文件 '{0}'...", projectName);
            this.chatPanel.AppendSysMessage(showText);
        }

        /// <summary>
        /// 文件传输成功
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="isSender">接收者，还是发送者</param>
        private void fileTransferingViewer1_FileTransCompleted(string projectName, bool isSender, string comment, bool isFolder)
        {
            string offlineFile = (Comment4OfflineFile.ParseUserID(comment) == null) ? "" : "离线文件";
            if (isFolder && !string.IsNullOrEmpty(offlineFile))
            {
                offlineFile += "夹";
            }
            string showText = offlineFile + string.Format("'{0}' {1}完成！", projectName, isSender ? "发送" : "接收");
            this.chatPanel.AppendSysMessage(showText);
        }

        private void fileTransferingViewer_FileTransCompleted2(TransferingProject pro)
        {
            string offlineFile = (Comment4OfflineFile.ParseUserID(pro.Comment) == null) ? "" : "离线文件";
            if (pro.IsFolder && !string.IsNullOrEmpty(offlineFile))
            {
                offlineFile += "夹";
            }
            string showText = offlineFile + string.Format("{0} {1}完成！", pro.ProjectName, pro.IsSender ? "发送" : "接收");

            if (pro.IsSender)
            {
                this.chatPanel.OnFileEvent(showText, pro.OriginPath);
            }
            else
            {
                //showText += string.Format("保存路径为：{0}", pro.LocalSavePath);
                this.chatPanel.OnFileEvent(showText, pro.LocalSavePath);
            }
            ///TO DO 插入本地数据库
            //ChatBoxContent2 content = new ChatBoxContent2(showText + "  ", new System.Drawing.Font("微软雅黑", 9), Color.Gray);
            //content.AddLinkFile((uint)showText.Length, pro.IsSender ? pro.OriginPath : pro.LocalSavePath);
            //byte[] binaryContent = CompactPropertySerializer.Default.Serialize(content);
            //ChatMessageRecord record = new ChatMessageRecord(this.currentFriend.ID, this.resourceCenter.CurrentUserID, binaryContent, false);
            //this.resourceCenter.LocalChatRecordPersister.InsertChatMessageRecord(record);

        }

        /// <summary>
        /// 文件传输失败
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="isSender">是接收者，还是发送者</param>
        /// <param name="fileTransDisrupttedType">失败原因</param>
        private void fileTransferingViewer1_FileTransDisruptted(string projectName, bool isSender, FileTransDisrupttedType type, string cause)
        {
            string showText = FileTransHelper.GetTipMessage4FileTransDisruptted(projectName, isSender, type, cause == FriendChatWindow.Token4ConvertToOfflineFile);
            this.chatPanel.AppendSysMessage(showText);
            //this.AppendMessage("系统", Color.Gray, showText, false);      
        }

        private bool FilterTransferingProject(TransferingProject pro)
        {

            NDiskParameters para = Comment4NDisk.Parse(pro.Comment);
            if (para != null)
            {
                return false;
            }


            if (ESFramework.NetServer.IsServerUser(pro.DestUserID))
            {
                string offlineFileSenderID = Comment4OfflineFile.ParseUserID(pro.Comment);
                return offlineFileSenderID == this.currentFriend.ID;
            }

            return pro.DestUserID == this.currentFriend.ID;
        }

        #endregion

        #region 收到文件传输请求
        /// <summary>
        /// 当收到文件传输请求的时候 ，展开fileTransferingViewer,如果 本来就是展开 状态，直接添加
        /// 自己发送 文件请求的时候，也调用这里
        /// </summary>        
        internal void FileRequestReceived(string projectID, bool offlineFile)
        {  
            this.ResetTabControlVisible(true);
            CommonHelper.Log_Tmp($"projectID-{projectID}：FileRequestReceived 1");
            TransferingProject pro = Program.ResourceCenter.RapidPassiveEngine.FileOutter.GetTransferingProject(projectID);
            CommonHelper.Log_Tmp($"projectID-{projectID}：FileRequestReceived 2");
            if (offlineFile)
            {
                string strFile = pro.IsFolder ? "离线文件夹" : "离线文件";
                this.chatPanel.AppendSysMessage(string.Format("对方给您发送了{0}'{1}'，大小：{2}", strFile, pro.ProjectName, ESBasic.Helpers.PublicHelper.GetSizeString(pro.TotalSize)));
            }
            CommonHelper.Log_Tmp($"projectID-{projectID}：FileRequestReceived 3");

            this.fileTransferingViewer.NewFileTransferItem(projectID, offlineFile, false);
            CommonHelper.Log_Tmp($"projectID-{projectID}：FileRequestReceived 4");
        }

        internal void FileRequestReceived(string projectID)
        {
            this.FileRequestReceived(projectID, false);
        }
        #endregion

        private int sendToken;

        #region 发送 传输文件的请求
        /// <summary>
        /// 发送文件（文件夹）
        /// </summary>
        /// <param name="isFolder"></param>
        private async void SendFileOrFolder(bool isFolder)
        {
            if (!Program.ResourceCenter.Connected)
            {
                return;
            }

            try
            {
                if (this.isInHisBlackList)
                {
                    MessageBoxEx.Show("对方已将您加入黑名单，不能进行文件发送！");
                    return;
                }
                string fileOrFolderPath = null;
                if (isFolder)
                {
                    System.Threading.Tasks.Task<string> task = FileHelper.OpenFolderDialog(this, "请选择要发送的文件夹");
                    await task.ConfigureAwait(true);
                    if (task.Result == null || task.Result.Length == 0)
                    {
                        return;
                    }
                    fileOrFolderPath = task.Result;
                }
                else
                {
                    sendToken++;
                    System.Threading.Tasks.Task<string[]> task = FileHelper.OpenFileDialog(this, "请选择要发送的文件");
                    await task.ConfigureAwait(true);
                    if (task.Result == null || task.Result.Length == 0)
                    {
                        return;
                    }
                    fileOrFolderPath = task.Result[0];
                }
                if (fileOrFolderPath == null)
                {
                    CommonHelper.Log_Tmp($"sendToken-【{sendToken}】文件路径不存在：{fileOrFolderPath}");
                    return;
                }
                CommonHelper.Log_Tmp($"sendToken-【{sendToken}】准备发送的文件路径：{fileOrFolderPath}");

                SendingFileParas sendingFileParas = new SendingFileParas(20480, 5);//文件数据包大小，可以根据网络状况设定，局网内可以设为204800，传输速度可以达到30M/s以上；公网建议设定为2048或4096或8192

                string projectID;
                Program.ResourceCenter.RapidPassiveEngine.FileOutter.BeginSendFile(this.currentFriend.UserID, fileOrFolderPath, null, sendingFileParas, out projectID);

                CommonHelper.Log_Tmp($"sendToken-【{sendToken}】准备开始渲染到界面：{fileOrFolderPath} projectID- 【{projectID}】");
                this.FileRequestReceived(projectID);
                CommonHelper.Log_Tmp($"sendToken-【{sendToken}】界面渲染已完成：{fileOrFolderPath} projectID-【{projectID}】");
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(GlobalResourceManager.SoftwareName, ee.Message);
            }
        }


        private void toolStripMenuItem33_Click(object sender, EventArgs e)
        {
            this.SendFileOrFolder(true);
        }
        #endregion

        #region IFriendChatForm<ggUser>

        public string UnitID => this.currentFriend.ID;

        public void FriendInfoChanged(GGUser user)
        {
            if (user.ID != this.currentFriend.ID) { return; }
            this.currentFriend = user;
            GGUserPlus ggUserPlus = new GGUserPlus(this.currentFriend);
            this.DataContext = ggUserPlus;
            this.Title = "与 " + this.currentFriend.DisplayName + " 对话中";
        }

        public void FriendStateChanged(string friendID, UserStatus newStatus)
        {
            if (friendID != this.currentFriend.ID)
            {
                return;
            }
            this.currentFriend.UserStatus = newStatus;

        }

        public void HandleChatMessage(byte[] info, DateTime? msgOccureTime)
        {
            try
            {
                ChatBoxContent2 content = CompactPropertySerializer.Default.Deserialize<ChatBoxContent2>(info, 0); 
                this.chatPanel.HandleChatMessage(null, content, msgOccureTime, null);
            }
            catch (Exception ee)
            {
                GlobalResourceManager.Logger.LogWithTime("程序出错，" + ee.Message);
            }
        }

        public void HandleChatMessageOfMine(byte[] info)
        {
            ChatBoxContent2 content = CompactPropertySerializer.Default.Deserialize<ChatBoxContent2>(info, 0);
            this.chatPanel.HandleChatMessageOfMine(content);
        }

        public void HandleFriendAddedNotify()
        {
            this.chatPanel.AppendSysMessage("对方添加您为好友，可以开始对话了...");
        }

        public void AddFriendSucceed()
        {
            this.chatPanel.AppendSysMessage("已经添加对方为好友，可以开始对话了...");
            return;
        }

        public void HandleInptingNotify()
        {
            
        }

        public void HandleMediaCommunicate(ClientType sourceClientType, CommunicateMediaType mediaType, CommunicateType communicateType, string tag)
        {
            
        }

        public void HandleOfflineFileResultReceived(string fileName, bool accept)
        {
            string msg = string.Format("对方{0}了您发送的离线文件'{1}'", accept ? "已成功接收" : "拒绝", fileName);
            this.chatPanel.AppendSysMessage(msg);
        }

        public void HandleSnapchatMessage(SnapchatMessage snapchatMessage, DateTime? msgOccureTime)
        {
            
        }

        public void HandleSnapchatRead(string messageID)
        {
            
        }

        public void HandleVibration()
        {
            
        }

        public void MyInfoChanged(GGUser my)
        {
            
        }

        public void MyselfOffline()
        {
            
        }

        public void OnMediaCommunicateAnswerOnOtherDevice(ClientType answerType, CommunicateMediaType type, bool agree)
        {
            
        }

        public void OnRemovedFromFriend()
        {
            MessageBoxEx.Show("对方已经将您从好友列表中移除。");
            this.Close();
        }

        public void RefreshUI()
        {
            
        }

        public void UnitCommentNameChanged(IUnit unit)
        {
            if (unit.ID == this.currentFriend.ID)
            {
                this.FriendInfoChanged((GGUser)unit);
            }            
        }
        #endregion

        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.chatPanel.Window_Changed();
        }
    }
}
