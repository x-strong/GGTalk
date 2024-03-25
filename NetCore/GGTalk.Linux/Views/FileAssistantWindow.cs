using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using ESBasic;
using ESFramework.Boost;
using ESFramework.Boost.NetworkDisk;
using ESPlus.FileTransceiver;
using ESPlus.Serialization;
using GGTalk;
using GGTalk.Linux;
using GGTalk.Linux.Controller;
using GGTalk.Linux.Controls;
using GGTalk.Linux.Helpers;
using GGTalk.Linux.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TalkBase;

namespace GGTalk.Linux.Views
{
    internal class FileAssistantWindow : Window, IFileAssistantForm
    {
        private ChatPanel chatPanel;
        private TabControl skinTabControl;
        private FileTransferingViewer fileTransferingViewer;

        public FileAssistantWindow()
        {
            this.Closing += FileAssistantWindow_Closing;
        }

        private void FileAssistantWindow_Closing(object sender, ClosingEventArgs e)
        {
            this.fileTransferingViewer.BeforeDispose();
        }


        protected override void InitializeComponent()
        {
            this.Icon = GlobalResourceManager.Png64;
            CanResize = true;
            Size = SizeField.Fill;
            CommandContext = this;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "文件传输助手";
            Width = 675;
            Height = 510;
            Background = null;
            Children.Add(new Panel
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
                    new Grid
                    {
                        Size = SizeField.Fill,
                        RowDefinitions =
                        {
                            new RowDefinition
                            {
                                Height = 60
                            },
                            new RowDefinition
                            {

                            },
                        },
                        ColumnDefinitions =
                        {
                            new ColumnDefinition
                            {

                            },
                            new ColumnDefinition
                            {
                                Width = 216
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
                                    new Picture
                                    {
                                        Stretch = Stretch.Fill,
                                        MarginLeft = 5,
                                        MarginTop = 6,
                                        Width = 20,
                                        Height = 20,
                                        Source = CommonOptions.ResourcesCatalog+ "FileAssistant64.png",
                                    },
                                    new TextBlock
                                    {
                                        Text = "文件传输助手",
                                        MarginLeft = 30,
                                        MarginTop = 8,
                                    },
                                    new Panel
                                    {
                                        MarginRight = 74,
                                        MarginTop = 0,
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
                                    },
                                    new Button
                                    {
                                        Classes = "BlueButton",
                                        MarginLeft = 10,
                                        MarginBottom = 3,
                                        Width = 29,
                                        Height = 26,
                                        Background = Color.Transparent,
                                        BorderThickness = new Thickness(0,0,0,0),
                                        BorderType = BorderType.BorderThickness,
                                        Content = new Picture
                                        {
                                            Stretch = Stretch.Fill,
                                            Source =CommonOptions.ResourcesCatalog+ "up_btn_icon.png"
                                        },
                                        Commands={ { nameof(Button.Click),(s,e)=> { this.ClickSendFile(); } } }
                                    }
                                },
                                Attacheds=
                                {
                                    {
                                        Grid.RowIndex,
                                        0
                                    },
                                    {
                                        Grid.ColumnSpan,
                                        2
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
                            new ChatPanel
                            {
                                PresenterFor = this,
                                Name = nameof(this.chatPanel),
                                Size = SizeField.Fill,
                                BorderThickness = new Thickness(0,0,2,0),
                                BorderType  = BorderType.BorderThickness,
                                BorderFill = "#eee",
                                Attacheds =
                                {
                                    {
                                        Grid.RowIndex,
                                        1
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
                                Visibility =Visibility.Hidden,
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
                        }
                    }
                }
            });
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));
            if (!DesignMode)//设计模式下不执行，也可以用#if !DesignMode
            {
                this.chatPanel = this.FindPresenterByName<ChatPanel>(nameof(this.chatPanel));
                this.skinTabControl = this.FindPresenterByName<TabControl>(nameof(this.skinTabControl));
                this.fileTransferingViewer = this.FindPresenterByName<FileTransferingViewer>(nameof(this.fileTransferingViewer));

                this.chatPanel.Initialize(Program.ResourceCenter.ClientGlobalCache.CurrentUser);
                this.chatPanel.CloseChatPanel += ChatPanel_CloseChatPanel;

                //文件传送
                this.fileTransferingViewer.Initialize(Program.ResourceCenter.RapidPassiveEngine.FileOutter, new ESBasic.Func<TransferingProject, bool>(this.FilterTransferingProject), this);
                this.fileTransferingViewer.FileTransDisruptted += new CbGeneric<string, bool, FileTransDisrupttedType, string>(fileTransferingViewer1_FileTransDisruptted);
                //this.fileTransferingViewer.FileTransCompleted += new CbGeneric<string, bool ,string,bool>(fileTransferingViewer1_FileTransCompleted);
                this.fileTransferingViewer.FileTransCompleted2 += new CbGeneric<TransferingProject>(fileTransferingViewer_FileTransCompleted2);
                this.fileTransferingViewer.FileResumedTransStarted += new CbGeneric<string, bool>(fileTransferingViewer1_FileResumedTransStarted);
                this.fileTransferingViewer.AllTaskFinished += new CbSimple(fileTransferingViewer1_AllTaskFinished);
                this.fileTransferingViewer.FileNeedOfflineSend += new CbGeneric<TransferingProject>(fileTransferingViewer_FileNeedOfflineSend);
            }
        }


        private void ChatPanel_CloseChatPanel()
        {
            this.Close();
        }

        #region 发送 传输文件的请求

        private void ClickSendFile()
        {
            this.SendFileOrFolder(false);
        }

        private async void SendFileOrFolder(bool isFolder)
        {
            if (!Program.ResourceCenter.Connected)
            {
                return;
            }

            try
            {
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
                    return;
                }
                string projectID;
                SendingFileParas sendingFileParas = new SendingFileParas(20480, 5);//文件数据包大小，可以根据网络状况设定，局网内可以设为204800，传输速度可以达到30M/s以上；公网建议设定为2048或4096或8192

                // BeginSendFile方法
                //（1）accepterID传入null，表示文件的接收者就是服务端
                //（2）巧用comment参数，参见Comment4OfflineFile类
                Program.ResourceCenter.RapidPassiveEngine.FileOutter.BeginSendFile(null, fileOrFolderPath,
                    Comment4OfflineFile.BuildComment(Program.ResourceCenter.CurrentUserID, Program.ResourceCenter.CurrentClientType), sendingFileParas, out projectID);
                this.FileRequestReceived(projectID);
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(GlobalResourceManager.SoftwareName, ee.Message);
            }
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
                Program.ResourceCenter.RapidPassiveEngine.FileOutter.BeginSendFile(null, project.OriginPath, Comment4OfflineFile.BuildComment(Program.ResourceCenter.CurrentUserID, Program.ResourceCenter.CurrentClientType), sendingFileParas, out projectID);
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
                Grid.ColumnSpan(this.chatPanel, 2);

            }
            else
            {
                this.skinTabControl.Visibility = Visibility.Visible;
                Grid.ColumnSpan(this.chatPanel, 1);
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
                return offlineFileSenderID == Program.ResourceCenter.CurrentUserID;
            }

            return pro.DestUserID == Program.ResourceCenter.CurrentUserID;
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
            TransferingProject pro = Program.ResourceCenter.RapidPassiveEngine.FileOutter.GetTransferingProject(projectID);
            if (offlineFile)
            {
                string strFile = pro.IsFolder ? "离线文件夹" : "离线文件";
                this.chatPanel.AppendSysMessage(string.Format("对方给您发送了{0}'{1}'，大小：{2}", strFile, pro.ProjectName, ESBasic.Helpers.PublicHelper.GetSizeString(pro.TotalSize)));
            }

            this.fileTransferingViewer.NewFileTransferItem(projectID, offlineFile, false);
        }

        internal void FileRequestReceived(string projectID)
        {
            this.FileRequestReceived(projectID, false);
        }


        #endregion

        #region IFileAssistantForm
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
        #endregion

    }
}
