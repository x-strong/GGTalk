using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using ESBasic;
using ESBasic.Helpers;
using ESFramework;
using ESFramework.Boost.NetworkDisk;
using ESFramework.Boost.NetworkDisk.Passive;
using ESPlus.Application.FileTransfering.Passive;
using ESPlus.FileTransceiver;
using GGTalk;
using GGTalk.Linux;
using GGTalk.Linux.Helpers;
using GGTalk.Linux.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GGTalk.Linux.Controls
{
    internal class NDiskBrowser : Control, INDiskBrowser
    {

        private FileOrDirectoryViewModel viewModel = new FileOrDirectoryViewModel() { TagList = new Collection<FileOrDirectoryTag>() };
        private NetWorkListBoxTemplate listBox;
        private FileTransferingViewer fileTransferingViewer;
        private Picture toolStripButton_state;
        private TextBlock toolStripLabel_msg, toolStripLabel_path;
        private TextBox txb_Path;
        private List<Bitmap> imageList2 = new List<Bitmap>();
        private Panel listViewGrid, transferingViewerGrid;
        private WrapPanel wrapPanel;
        private ScrollViewer scrollViewer;
        private Button toolStripButton_parent, toolStripButton_root, toolStripButton_refresh, toolStripButton_newFileFolder, toolStripButton_uploadFile, toolStripButton_download, toolStripButton_delete;

        private CutOrCopyAction cutOrCopyAction = null;
        private INDiskOutter fileDirectoryOutter;
        private IFileOutter fileOutter;
        private string ownerID; //若取值为NetServer.SystemUserID，则表示请求对象为网络硬盘。
        private string currentUserID;
        public event Action<string> UploadCompleted;

        private bool ownerSharedAllDisk = false;
        ContextMenu networkpop;
        public NDiskBrowser()
        {
            this.imageList2.Add(GGTalk.Linux.Helpers.FileHelper.FindAssetsBitmap("tick.png"));
            this.imageList2.Add(GGTalk.Linux.Helpers.FileHelper.FindAssetsBitmap("error.png"));

            this.InitializeComponent();
        }

        private bool loaded = false;
        protected override void InitializeComponent()
        {
            if (this.loaded) { return; }
            this.loaded = true;
            DataContext = null;
            Size = SizeField.Fill;

            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Background = null;
            BorderFill = "#eee";
            BorderThickness = new Thickness(2, 2, 2, 2);
            BorderType = BorderType.BorderThickness;
            Children.Add(new Panel()
            {
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
                                MarginBottom = 0,
                                Background = "#fff",
                                ColumnDefinitions =
                                {
                                    new ColumnDefinition
                                    {

                                    },
                                    new ColumnDefinition
                                    {
                                         Width  = 210
                                    }
                                },
                                RowDefinitions =
                                {
                                    new RowDefinition
                                    {
                                        Height = 30
                                    },
                                    new RowDefinition
                                    {

                                    },
                                    new RowDefinition
                                    {
                                        Height = 30
                                    }
                                },
                                Children =
                                {
                                    new WrapPanel
                                    {
                                        Width="100%",
                                        Children =
                                        {
                                            new TextBlock
                                            {
                                                Name=nameof(this.toolStripLabel_path),
                                                PresenterFor=this,
                                                MarginTop = 7,
                                                Text = "路径",
                                                MarginLeft = 10,
                                            },
                                            new TextBox
                                            {
                                                Name=nameof(this.txb_Path),
                                                PresenterFor=this,
                                                MarginLeft = 2,
                                                MarginTop = 2,
                                                IsReadOnly=true,
                                                Classes = "NetWorkTextBox",
                                                Width = 400,
                                                Height = 23,
                                                VScrollBarVisibility = ScrollBarVisibility.Hidden,
                                            },
                                            //new WrapPanel
                                            //{
                                                //MarginLeft = "56%",
                                                //Children =
                                                //{
                                                    new Button
                                                    {
                                                        Name=nameof(this.toolStripButton_parent),
                                                        PresenterFor=this,
                                                        Classes = "NetWorkButton",
                                                        MarginLeft = 10,
                                                        MarginTop = 5,
                                                        Cursor = Cursors.Hand,
                                                        ToolTip = "向上一级",
                                                        Width = 20,
                                                        Height = 20,
                                                        Content =
                                                        new Picture
                                                        {
                                                            Stretch = Stretch.Fill,
                                                            Size = SizeField.Fill,
                                                            Source = CommonOptions.ResourcesCatalog+ "btn_back.png",
                                                        }
                                                    },
                                                    new Button
                                                    {
                                                        Name=nameof(this.toolStripButton_root),
                                                        PresenterFor=this,
                                                        Classes = "NetWorkButton",
                                                        MarginLeft = 10,
                                                        MarginTop = 5,
                                                        Cursor = Cursors.Hand,
                                                        ToolTip = "根目录",
                                                        Width = 20,
                                                        Height = 20,
                                                        Content =
                                                        new Picture
                                                        {
                                                            Size = SizeField.Fill,
                                                            Source = CommonOptions.ResourcesCatalog+ "btn_root.png",
                                                        }
                                                    },
                                                    new Button
                                                    {
                                                        Name=nameof(this.toolStripButton_refresh),
                                                        PresenterFor=this,
                                                        Classes = "NetWorkButton",
                                                        MarginLeft = 10,
                                                        MarginTop = 5,
                                                        Cursor = Cursors.Hand,
                                                        ToolTip = "刷新目录",
                                                        Width = 20,
                                                        Height = 20,
                                                        Content =
                                                        new Picture
                                                        {
                                                            Stretch = Stretch.Fill,
                                                            Size = SizeField.Fill,
                                                            Source = CommonOptions.ResourcesCatalog+ "refresh.png",
                                                        }
                                                    },
                                                    new Button
                                                    {
                                                        Name=nameof(this.toolStripButton_newFileFolder),
                                                        PresenterFor=this,
                                                        Classes = "NetWorkButton",
                                                        MarginLeft = 10,
                                                        MarginTop = 5,
                                                        Cursor = Cursors.Hand,
                                                        ToolTip = "新建文件夹",
                                                        Width = 20,
                                                        Height = 20,
                                                        Content =
                                                        new Picture
                                                        {
                                                            Stretch = Stretch.Fill,
                                                            Size = SizeField.Fill,
                                                            Source = CommonOptions.ResourcesCatalog+ "btn_addfile.png",
                                                        }
                                                    },
                                                    new Button
                                                    {
                                                        Name=nameof(this.toolStripButton_uploadFile),
                                                        PresenterFor=this,
                                                        Classes = "NetWorkButton",
                                                        MarginLeft = 10,
                                                        MarginTop = 5,
                                                        Cursor = Cursors.Hand,
                                                        ToolTip = "上传",
                                                        Width = 20,
                                                        Height = 20,
                                                        Content =
                                                        new Picture
                                                        {
                                                            Stretch = Stretch.Fill,
                                                            Size = SizeField.Fill,
                                                            Source = CommonOptions.ResourcesCatalog+ "btn_upload.png",
                                                        }
                                                    },
                                                    new Button
                                                    {
                                                        Name=nameof(this.toolStripButton_download),
                                                        PresenterFor=this,
                                                        Classes = "NetWorkButton",
                                                        MarginLeft = 10,
                                                        MarginTop = 5,
                                                        Cursor = Cursors.Hand,
                                                        ToolTip = "下载",
                                                        Width = 20,
                                                        Height = 20,
                                                        Content =
                                                        new Picture
                                                        {
                                                            Stretch = Stretch.Fill,
                                                            Size = SizeField.Fill,
                                                            Source = CommonOptions.ResourcesCatalog+ "btn_download.png",
                                                        }
                                                    },
                                                    new Button
                                                    {
                                                        Name=nameof(this.toolStripButton_delete),
                                                        PresenterFor=this,
                                                        Classes = "NetWorkButton",
                                                        MarginLeft = 10,
                                                        MarginTop = 5,
                                                        Cursor = Cursors.Hand,
                                                        ToolTip = "删除",
                                                        Width = 20,
                                                        Height = 20,
                                                        Content =
                                                        new Picture
                                                        {
                                                            Stretch = Stretch.Fill,
                                                            Size = SizeField.Fill,
                                                            Source = CommonOptions.ResourcesCatalog+ "btn_delete.png",
                                                        }
                                                    },
                                                //}
                                            //}
                                        },
                                        Size = SizeField.Fill,
                                        Attacheds =
                                        {
                                            {
                                                Grid.ColumnSpan,
                                                2
                                            },
                                            {
                                                Grid.RowIndex,
                                                0
                                            }
                                        }
                                    },
                                    new ScrollViewer
                                    {
                                        Name=nameof(this.scrollViewer),
                                        PresenterFor=this,
                                        BorderFill = "130,135,144",
                                        BorderThickness = new Thickness(1,1,1,1),
                                        BorderType = BorderType.BorderThickness,
                                        Width = "97%",
                                        Height = "97%",
                                        Content = new  WrapPanel{
                                            Name=nameof(this.wrapPanel),
                                            PresenterFor=this,
                                            Width ="100%",
                                            MarginTop = 0,
                                            Children =
                                            {
                                                new NetWorkListBoxTemplate
                                                {
                                                    Name=nameof(this.listBox),
                                                    PresenterFor=this,
                                                    Size = SizeField.Fill,
                                                    ItemTemplate = new NetWorkListBoxItem(){
                                                        Commands={ { nameof(DoubleClick),(s,e)=>  { ((MouseButtonEventArgs)e).Handled = true; this.FileBoxItem_DoubleClicked((FileOrDirectoryTag)s.DataContext);  } },
                                                        },
                                                        ContextMenu=new ContextMenu
                                                        {
                                                            Items = new UIElement[]
                                                            {
                                                                new MenuItem
                                                                {
                                                                    Header = "重命名",
                                                                    Commands = {
                                                                                    {
                                                                                        nameof(MouseDown),
                                                                                        (s, e) => FileBoxItem_RenameClicked((FileOrDirectoryTag)this.listBox.SelectedValue)
                                                                                    }
                                                                                }

                                                                },
                                                                new MenuItem
                                                                {
                                                                    Header = "下载",
                                                                    Commands = {
                                                                                    {
                                                                                        nameof(MouseDown),
                                                                                        (s, e) => FileBoxItem_DownLoadClicked((FileOrDirectoryTag)this.listBox.SelectedValue)
                                                                                    }
                                                                                }

                                                                },
                                                                new MenuItem
                                                                {

                                                                    Header = "删除",
                                                                    Commands = {
                                                                                    {
                                                                                        nameof(MouseDown),
                                                                                        (s, e) => FileBoxItem_DeleteClicked((FileOrDirectoryTag)this.listBox.SelectedValue)
                                                                                    }
                                                                                }
                                                                }
                                                            }
                                                        }
                                                    },
                                                    Bindings =
                                                    {
                                                        {
                                                            nameof(ListBox.Items),
                                                            nameof(FileOrDirectoryViewModel.TagList)
                                                        }
                                                    },
                                                }
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
                                                1
                                            }
                                        }
                                    },
                                    new Panel
                                    {
                                        Name=nameof(this.transferingViewerGrid),
                                        PresenterFor=this,
                                        Size = SizeField.Fill,
                                        Background = "#fff",
                                        Visibility =Visibility.Visible,
                                        Children =
                                        {
                                            new FileTransferingViewer
                                            {
                                                Name = "fileTransferingViewer",
                                                PresenterFor = this,
                                                Size = SizeField.Fill,
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
                                        Children =
                                        {
                                            new Picture
                                            {
                                                Name=nameof(this.toolStripButton_state),
                                                PresenterFor=this,
                                                Width = 20,
                                                Height = 20,
                                                MarginLeft = 10,
                                                Stretch = Stretch.Fill,
                                                Source = CommonOptions.ResourcesCatalog + "tick.png",
                                            },
                                            new TextBlock
                                            {
                                                Name=nameof(this.toolStripLabel_msg),
                                                PresenterFor=this,
                                                MarginLeft = 40,
                                                MarginTop = 7,
                                                Text = "就绪"
                                            }
                                        },
                                        Size = SizeField.Fill,
                                        Attacheds =
                                        {
                                            {
                                                Grid.ColumnIndex,
                                                0
                                            },
                                            {
                                                Grid.RowIndex,
                                                2
                                            }
                                        }
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
                                        1
                                    }
                                }
                            }
                        }

            });

            //加载样式文件，文件需要设置为内嵌资源

            if (!DesignMode)//设计模式下不执行，也可以用#if !DesignMode
            {
                this.scrollViewer = this.FindPresenterByName<ScrollViewer>(nameof(this.scrollViewer));
                this.wrapPanel = this.FindPresenterByName<WrapPanel>(nameof(this.wrapPanel));
                this.listBox = this.FindPresenterByName<NetWorkListBoxTemplate>(nameof(this.listBox));
                this.toolStripLabel_path = this.FindPresenterByName<TextBlock>(nameof(this.toolStripLabel_path));
                this.txb_Path = this.FindPresenterByName<TextBox>("txb_Path");
                this.toolStripButton_state = this.FindPresenterByName<Picture>("toolStripButton_state");
                this.toolStripLabel_msg = this.FindPresenterByName<TextBlock>("toolStripLabel_msg");
                this.listViewGrid = this.FindPresenterByName<Grid>("listViewGrid");
                this.transferingViewerGrid = this.FindPresenterByName<Panel>("transferingViewerGrid");


                this.toolStripButton_parent = this.FindPresenterByName<Button>("toolStripButton_parent");
                this.toolStripButton_parent.Click += ToolStripButton_parent_Click;
                this.toolStripButton_root = this.FindPresenterByName<Button>("toolStripButton_root");
                this.toolStripButton_root.Click += ToolStripButton_root_Click;
                this.toolStripButton_refresh = this.FindPresenterByName<Button>("toolStripButton_refresh");
                this.toolStripButton_refresh.Click += ToolStripButton_refresh_Click;
                this.toolStripButton_newFileFolder = this.FindPresenterByName<Button>("toolStripButton_newFileFolder");
                this.toolStripButton_newFileFolder.Click += ToolStripButton_newFileFolder_Click;
                this.toolStripButton_uploadFile = this.FindPresenterByName<Button>("toolStripButton_uploadFile");
                this.toolStripButton_uploadFile.Click += ToolStripButton_uploadFile_Click;
                this.toolStripButton_download = this.FindPresenterByName<Button>("toolStripButton_download");
                this.toolStripButton_download.Click += ToolStripButton_download_Click;
                this.toolStripButton_delete = this.FindPresenterByName<Button>("toolStripButton_delete");
                this.toolStripButton_delete.Click += ToolStripButton_delete_Click;
                this.ResetTransferingViewerVisible(false);

                this.fileTransferingViewer = this.FindPresenterByName<FileTransferingViewer>("fileTransferingViewer");
                this.fileTransferingViewer.FileTransStarted += new CbGeneric<string, bool>(fileTransferingViewer1_FileTransStarted);
                this.fileTransferingViewer.AllTaskFinished += new CbSimple(fileTransferingViewer1_AllTaskFinished);
                this.fileTransferingViewer.FileResumedTransStarted += new CbGeneric<string, bool>(fileTransferingViewer1_FileResumedTransStarted);
                this.fileTransferingViewer.FileTransCompleted += new CbGeneric<string, bool, string, bool>(fileTransferingViewer1_FileTransCompleted);
                this.fileTransferingViewer.FileTransDisruptted += new ESBasic.CbGeneric<string, bool, FileTransDisrupttedType, string>(fileTransferingViewer1_FileTransDisruptted);

            }
            this.Componentloaded = true;
        }

        
        public bool Componentloaded = false;


        private void ResetTransferingViewerVisible(bool showViewer)
        {
            if (!showViewer)
            {
                this.transferingViewerGrid.Visibility = Visibility.Hidden;
                Grid.ColumnSpan(this.scrollViewer, 2);
            }
            else
            {
                this.transferingViewerGrid.Visibility = Visibility.Visible;
                Grid.ColumnSpan(this.scrollViewer, 1);
            }
        }
        #region 上方按钮事件
        private void ToolStripButton_delete_Click(object sender, RoutedEventArgs e)
        {
            this.DeleteFileOrDir();
        }

        private void ToolStripButton_download_Click(object sender, RoutedEventArgs e)
        {
            if (((this.ownerSharedAllDisk || this.IsNetworkDisk) && this.currentDirPath == null) || this.listBox.SelectedValue == null)
            {
                return;
            }
            FileOrDirectoryTag tag = this.listBox.SelectedValue as FileOrDirectoryTag;
            this.Download(tag.Name, tag.IsFile);
        }

        private async void ToolStripButton_uploadFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Task<string[]> task = GGTalk.Linux.Helpers.FileHelper.OpenFileDialog(this.parentWindow, "请选择要上传的文件", false);
                await task.ConfigureAwait(true);
                if (task.Result == null || task.Result.Length == 0)
                {
                    return;
                }
                string filePath = task.Result[0];
                string fileName = ESBasic.Helpers.FileHelper.GetFileNameNoPath(filePath);
                foreach (FileOrDirectoryTag target in this.viewModel.TagList)
                {

                    if (target == null) { continue; }
                    if (target.IsFile && target.Name.ToLower() == fileName.ToLower())
                    {
                        Task<ButtonResult> task1 = MessageBoxEx.ShowQuery(string.Format("{0}已经存在，确定要覆盖它吗？", fileName));
                        await task1.ConfigureAwait(true);
                        if (task1.Result != ButtonResult.Yes)
                        {
                            return;
                        }
                    }
                }

                if (this.IsNetworkDisk)
                {
                    ulong fileSize = ESBasic.Helpers.FileHelper.GetFileSize(filePath);
                    NetworkDiskState state = this.fileDirectoryOutter.GetNetworkDiskState(this.netDiskID);
                    ulong available = state.TotalSize - state.SizeUsed;
                    if (available < fileSize)
                    {
                        MessageBoxEx.Show(string.Format("网络硬盘剩余空间为{0}，无法上传大小为{1}的文件！", PublicHelper.GetSizeString(available), PublicHelper.GetSizeString(fileSize)));
                        return;
                    }
                }
                this.fileDirectoryOutter.Upload(this.ownerID, this.netDiskID, filePath, this.currentDirPath + fileName);
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(ee.Message);
            }
        }

        private void ToolStripButton_newFileFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = "新建文件夹";
                bool found = true;
                int i = 1;
                while (found)
                {
                    found = false;
                    foreach (FileOrDirectoryTag target in this.viewModel.TagList)
                    {

                        if (target == null) { continue; }
                        if (!target.IsFile && name == target.Name)
                        {
                            found = true;
                        }
                    }

                    if (found)
                    {
                        name = "新建文件夹" + i.ToString();
                        i++;
                    }
                }

                Cursor = new Cursor(Cursors.Wait);
                this.fileDirectoryOutter.CreateDirectory(this.ownerID, this.netDiskID, this.currentDirPath, name);
                FileOrDirectoryTag tag = new FileOrDirectoryTag(name, 0, DateTime.Now, false);
                this.AddFileBoxItem(tag);
                //item.BeginEdit();
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(ee.Message);
            }
            finally
            {
                //this.isLableEditing = false;
                Cursor = Cursors.Arrow;
            }
        }

        private void ToolStripButton_refresh_Click(object sender, RoutedEventArgs e)
        {
            this.LoadDirectory(this.currentDirPath, true);
        }

        private void ToolStripButton_root_Click(object sender, RoutedEventArgs e)
        {
            this.GotoRoot();
        }

        private void ToolStripButton_parent_Click(object sender, RoutedEventArgs e)
        {
            this.GotoParent();
        }

        private void GotoParent()
        {
            try
            {
                if ((this.ownerSharedAllDisk || this.IsNetworkDisk) && this.currentDirPath == null)
                {
                    return;
                }

                DirectoryInfo directoryInfo = new DirectoryInfo(this.currentDirPath);
                string temp = this.currentDirPath.Substring(0, this.currentDirPath.Length - 1);
                if (directoryInfo.Parent == null || !temp.Contains(GGTalk.Linux.Helpers.FileHelper.FolderSeparatorChar))
                {
                    this.GotoRoot();
                    return;
                }
                int pos = temp.LastIndexOf(GGTalk.Linux.Helpers.FileHelper.FolderSeparatorChar);
                string relativeDir = temp.Substring(0, pos) + GGTalk.Linux.Helpers.FileHelper.FolderSeparatorChar;
                this.LoadDirectory(relativeDir, true);
            }
            catch (Exception ee)
            {
                GlobalResourceManager.WriteErrorLog(ee);
            }
        }

        private async void Download(string fileOrDirName, bool isFile)
        {
            string tip = "下载文件！请选择保存路径";
            if (!isFile)
            {
                tip = "下载文件夹！请选择保存路径";
            }

            Task<string> task = GGTalk.Linux.Helpers.FileHelper.SaveFileDialog(this.parentWindow, tip, fileOrDirName);
            await task.ConfigureAwait(true);
            if (string.IsNullOrEmpty(task.Result))
            {
                return;
            }
            string savePath = task.Result;
            GlobalResourceManager.Logger.LogWithTime("保存路径savePath：" + savePath);
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (File.Exists(savePath) || Directory.Exists(savePath))
                {
                    Task<ButtonResult> task1 = MessageBoxEx.ShowQuery(string.Format("{0}已经存在，确定要覆盖它吗？", ESBasic.Helpers.FileHelper.GetFileNameNoPath(savePath)));
                    await task1.ConfigureAwait(true);
                    if (task1.Result != ButtonResult.Yes)
                    {
                        return;
                    }
                }
            }
            OperationResult operationResult = this.fileDirectoryOutter.Download(this.ownerID, this.netDiskID, this.currentDirPath + fileOrDirName, savePath, isFile);
            if (!operationResult.Succeed)
            {
                MessageBoxEx.Show(operationResult.ErrorMessage);
                this.LoadDirectory(this.currentDirPath, true);
            }
        }

        private async void DeleteFileOrDir()
        {
            if (((this.ownerSharedAllDisk || this.IsNetworkDisk) && this.currentDirPath == null) || this.listBox.SelectedValue == null)
            {
                return;
            }
            GlobalResourceManager.Logger.LogWithTime("进入删除：当前线程是否为后台线程：" + System.Threading.Thread.CurrentThread.IsBackground);

            FileOrDirectoryTag tag = this.listBox.SelectedValue as FileOrDirectoryTag;
            Task<ButtonResult> task1 = MessageBoxEx.ShowQuery(string.Format("您确定要删除{0} {1} 吗？", tag.IsFile ? "文件" : "文件夹", tag.Name));
            await task1.ConfigureAwait(true);
            if (task1.Result != ButtonResult.Yes)
            {
                return;
            }

            if (this.cutOrCopyAction != null && this.currentDirPath == this.cutOrCopyAction.ParentPathOfCuttedOrCopyed && tag.Name == this.cutOrCopyAction.ItemNameOfCuttedOrCopyed)
            {
                this.cutOrCopyAction = null;
            }

            List<string> files = new List<string>();
            List<string> dirs = new List<string>();
            if (tag.IsFile)
            {
                files.Add(tag.Name);
            }
            else
            {
                dirs.Add(tag.Name);
            }

            OperationResult result = this.fileDirectoryOutter.Delete(this.ownerID, this.netDiskID, this.currentDirPath, files, dirs);
            if (!result.Succeed)
            {
                MessageBoxEx.Show(result.ErrorMessage);
            }
            this.LoadDirectory(this.currentDirPath, true);
        }

        #endregion
        #region fileTransferingViewer Event
        private void fileTransferingViewer1_FileTransDisruptted(string fileName, bool isSending, FileTransDisrupttedType disrupttedType, string cause)
        {
            if (this == null)
            {
                return;
            }
            string info = string.Format("{0} {1}中断！", fileName, isSending ? "上传" : "下载");
            if (disrupttedType == FileTransDisrupttedType.ActiveCancel)
            {
                info += "原因：您取消了文件传送。";
            }
            else if (disrupttedType == FileTransDisrupttedType.DestCancel)
            {
                info += "原因：对方取消了文件传送。";
            }
            else if (disrupttedType == FileTransDisrupttedType.DestOffline)
            {
                info += "原因：对方已经掉线。";
            }
            else if (disrupttedType == FileTransDisrupttedType.SelfOffline)
            {
                info += "原因：您已经掉线。";
            }
            else if (disrupttedType == FileTransDisrupttedType.DestInnerError)
            {
                info += "原因：对方系统错误。";
            }
            else if (disrupttedType == FileTransDisrupttedType.InnerError)
            {
                info += "原因：系统错误。";
            }
            else if (disrupttedType == FileTransDisrupttedType.ReliableP2PChannelClosed)
            {
                info += "原因：可靠的P2P通道关闭。";
            }
            else
            {

            }

            this.toolStripButton_state.Source = this.imageList2[1];
            this.toolStripLabel_msg.Text = string.Format("{0}： {1}", DateTime.Now.ToString(), info);
        }

        private void fileTransferingViewer1_FileTransCompleted(string fileName, bool isSending, string comment, bool isFolder)
        {
            if (this == null)
            {
                return;
            }
            this.toolStripButton_state.Source = this.imageList2[0];
            string info = string.Format("{0} {1}完成！", fileName, isSending ? "上传" : "下载");
            this.toolStripLabel_msg.Text = string.Format("{0}： {1}", DateTime.Now.ToString(), info);

            if (isSending)
            {
                System.Threading.Thread.Sleep(500);
                this.LoadDirectory(this.currentDirPath, false);

                if (this.UploadCompleted != null)
                {
                    this.UploadCompleted(fileName);
                }
            }
        }

        private void fileTransferingViewer1_FileResumedTransStarted(string fileName, bool isSending)
        {
            if (this == null)
            {
                return;
            }
            this.toolStripButton_state.Source = this.imageList2[0];
            string info = string.Format("{0}： {1} {2}，启动断点续传！", DateTime.Now.ToString(), fileName, isSending ? "上传" : "下载");
            this.toolStripLabel_msg.Text = info;
        }

        private void fileTransferingViewer1_AllTaskFinished()
        {
            if (this == null)
            {
                return;
            }
            this.ResetTransferingViewerVisible(false);
        }

        private void fileTransferingViewer1_FileTransStarted(string fileName, bool isSending)
        {
            if (this == null)
            {
                return;
            }
            this.ResetTransferingViewerVisible(true);

            this.toolStripButton_state.Source = this.imageList2[0];
            string info = string.Format("{0} {1}开始！", fileName, isSending ? "上传" : "下载");
            this.toolStripLabel_msg.Text = string.Format("{0}： {1}", DateTime.Now.ToString(), info);
        }


        #endregion

        #region LoadDirectory
        private void LoadDirectory(string path, bool tipOnException)
        {
            if (this.ownerID == null)
            {
                return;
            }

            try
            {
                this.viewModel.TagList.Clear();
                SharedDirectory sharedDirectory = this.fileDirectoryOutter.GetSharedDirectory(this.ownerID, this.netDiskID, path);
                if (sharedDirectory == null)
                {
                    MessageBoxEx.Show("网络硬盘未开放！");
                }
                else if (!sharedDirectory.Valid)
                {
                    MessageBoxEx.Show(sharedDirectory.Exception);
                }
                else
                {
                    if (path == null)
                    {
                        this.ownerSharedAllDisk = sharedDirectory.DirectoryPath == null;
                    }

                    #region Action
                    //this.wrapPanel.Children.Clear();
                    if (sharedDirectory.DirectoryPath == null)
                    {
                        sharedDirectory.DriveList.Sort();
                        foreach (DiskDrive drive in sharedDirectory.DriveList)
                        {
                            //int imageIndex = 2;
                            //if (drive.DriveType == DriveType.CDRom)
                            //{
                            //    imageIndex = 3;
                            //}
                            //if (drive.DriveType == DriveType.Removable)
                            //{
                            //    imageIndex = 4;
                            //}
                            FileOrDirectoryTag tag = new FileOrDirectoryTag(drive.Name, 0, DateTime.Now, false);
                            //FileBoxItem fileBoxItem = new FileBoxItem(tag);

                            string name = drive.VolumeLabel;
                            if (name == null || name.Length == 0)
                            {
                                name = drive.Name;
                            }
                            tag.ToolTip = string.Format("{0}\n可用空间：{1}\n总 大 小：{2}", name, PublicHelper.GetSizeString(drive.AvailableFreeSpace), PublicHelper.GetSizeString(drive.TotalSize));
                            this.AddFileBoxItem(tag);
                        }
                    }
                    else
                    {
                        foreach (DirectoryDetail dirDetail in sharedDirectory.SubDirectorys)
                        {
                            FileOrDirectoryTag tag = new FileOrDirectoryTag(dirDetail.Name, 0, dirDetail.CreateTime, false);
                            //FileBoxItem item = new FileBoxItem(tag);
                            this.AddFileBoxItem(tag);
                        }

                        foreach (FileDetail file in sharedDirectory.FileList)
                        {
                            FileOrDirectoryTag tag = new FileOrDirectoryTag(file.Name, file.Size, file.CreateTime, true);
                            tag.ToolTip = string.Format("大    小：{0}\n创建日期：{1}", PublicHelper.GetSizeString((uint)file.Size), file.CreateTime);
                            this.AddFileBoxItem(tag);
                        }

                        //this.columnIndexToSort = 0;
                        //this.asendingOrder = true;
                        //this.wrapPanel.Children.Sort<FileBoxItem>(this.Compare);
                    }

                    this.currentDirPath = path;
                    if (this.currentDirPath != null && !this.currentDirPath.EndsWith(GGTalk.Linux.Helpers.FileHelper.FolderSeparatorChar))
                    {
                        this.currentDirPath += GGTalk.Linux.Helpers.FileHelper.FolderSeparatorChar;
                    }

                    string displayPath = this.IsNetworkDisk ? "网络硬盘" : "共享磁盘";
                    if (this.currentDirPath != null && this.currentDirPath != sharedDirectory.DirectoryPath)
                    {
                        displayPath += GGTalk.Linux.Helpers.FileHelper.FolderSeparatorChar + this.currentDirPath;
                    }
                    this.txb_Path.Text = displayPath;

                    #endregion
                }
                this.DataContext = this.viewModel;
                //this.listBox.Items = this.viewModel.TagList;
            }
            catch (Exception ee)
            {
                if (tipOnException)
                {
                    MessageBoxEx.Show(ee.Message);
                }
            }
        }

        private void AddFileBoxItem(FileOrDirectoryTag tag)
        {
            this.viewModel.TagList.Add(tag);

            //if (fileBoxItem == null) { return; }
            //fileBoxItem.DeleteClicked += FileBoxItem_DeleteClicked;
            //fileBoxItem.DownLoadClicked += FileBoxItem_DownLoadClicked;
            //fileBoxItem.DoubleClicked += FileBoxItem_DoubleClicked;
            //this.wrapPanel.Children.Add(fileBoxItem);
            //this.curFileBoxItems.Add(fileBoxItem);
            //this.curFileBoxItems.Sort(this.Compare);
            //this.wrapPanel.Children.Clear();
            //this.wrapPanel.Children.AddRange(this.curFileBoxItems);
        }

        private void FileBoxItem_DoubleClicked(FileOrDirectoryTag tag)
        {
            if (!this.ownerIsOnline)
            {
                return;
            }
            if (tag == null)
            {
                return;
            }

            if (!tag.IsFile) //目录
            {
                string tempPath = this.currentDirPath;
                if (tempPath == null)
                {
                    tempPath = tag.Name;
                }
                else
                {
                    tempPath += tag.Name;
                }
                this.LoadDirectory(tempPath, true);
                return;
            }
            this.Download(tag.Name, true);
        }    

        private async void FileBoxItem_RenameClicked(FileOrDirectoryTag tag)
        {
            try
            {
                NewNameOrRenameWindow newNameOrRenameWindow = new NewNameOrRenameWindow("名称",tag.Name);
                Task<object> task = newNameOrRenameWindow.ShowDialog_Topmost((Window)this.Root);
                await task.ConfigureAwait(true);
                if (Convert.ToBoolean(task.Result))
                {
                    string newName = newNameOrRenameWindow.NewName;

                    foreach (FileOrDirectoryTag target in this.viewModel.TagList)
                    {
                        if (newName == target.Name)
                        {
                            MessageBoxEx.Show(String.Format("{0} 已存在,请更换名称！", newName));
                            return;
                        }
                    }

                    if (this.cutOrCopyAction != null && this.currentDirPath == this.cutOrCopyAction.ParentPathOfCuttedOrCopyed && tag.Name == this.cutOrCopyAction.ItemNameOfCuttedOrCopyed)
                    {
                        this.cutOrCopyAction = null;
                    }

                    OperationResult result = this.fileDirectoryOutter.Rename(this.ownerID, this.netDiskID, this.currentDirPath, tag.IsFile, tag.Name, newName);
                    if (!result.Succeed)
                    {
                        MessageBoxEx.Show(result.ErrorMessage);
                    }
                    else {
                        tag.Name = newName;
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(ee.Message);
            }

        }

        private void FileBoxItem_DownLoadClicked(FileOrDirectoryTag tag)
        {
            this.Download(tag.Name, tag.IsFile);
        }

        private void FileBoxItem_DeleteClicked(FileOrDirectoryTag tag)
        {
            this.DeleteFileOrDir();
        }

        private Dictionary<string, int> iconIndexDic = new Dictionary<string, int>();// ".txt" - 3

        #endregion

        #region INDiskBrowser

        #region NetDiskID
        private string netDiskID = "";
        /// <summary>
        /// 网盘的标志。（对于远程磁盘而言，即OwnerID为某个用户的ID时，该属性无效）。
        /// 如果是群组共享的文件夹，则可以将其设置为对应的群组的ID。
        /// </summary>
        public string NetDiskID
        {
            get { return netDiskID; }
            set { netDiskID = value ?? ""; }
        }
        #endregion

        #region AllowUploadFolder
        private bool allowUploadFolder = false;
        /// <summary>
        /// 是否允许上传文件夹
        /// </summary>
        public bool AllowUploadFolder
        {
            get { return allowUploadFolder; }
            set { allowUploadFolder = value; }
        }
        #endregion
        #region LockRootDirectory
        private bool lockRootDirectory = false;
        /// <summary>
        /// 锁定为只访问根目录（不允许操作子文件夹）。（在初始化之前设置）
        /// </summary>
        public bool LockRootDirectory
        {
            get { return lockRootDirectory; }
            set { lockRootDirectory = value; }
        }
        #endregion

        #region CurrentDirectoryPath
        private string currentDirPath;
        public string CurrentDirectoryPath
        {
            get
            {
                return this.currentDirPath;
            }
        }
        #endregion

        #region ownerIsOnline
        private bool ownerIsOnline = true;
        /// <summary>
        /// 当前用户与Onwer是否处于连接状态。
        /// </summary>
        public bool Connected
        {
            get { return ownerIsOnline; }
            set
            {
                ownerIsOnline = value;
                this.toolStripButton_parent.IsEnabled = value;
                this.toolStripButton_root.IsEnabled = value;
                this.toolStripButton_refresh.IsEnabled = value;
            }
        }
        #endregion

        #region OwnerID
        public string OwnerID
        {
            get
            {
                if (this.ownerID == NetServer.SystemUserID)
                {
                    return null;
                }
                return this.ownerID;
            }
        }
        #endregion

        #region IsNetworkDisk
        /// <summary>
        /// IsNetworkDisk 是否正在访问网络硬盘。
        /// </summary>
        private bool IsNetworkDisk
        {
            get
            {
                return this.ownerID == NetServer.SystemUserID;
            }
        }

        #region IsFileTransfering
        public bool IsFileTransfering
        {
            get
            {
                return this.fileTransferingViewer.IsFileTransfering;
            }
        }
        #endregion

        #region Initialized
        private bool initialized = false;
        /// <summary>
        /// 是否完成了初始化？
        /// </summary>
        public bool Initialized
        {
            get { return initialized; }
        }
        #endregion

        #region MyRegion
        Window parentWindow = null;
        /// <summary>
        /// 需再Initialize前设置才有效
        /// </summary>
        public Window ParentWindow { get => parentWindow; set => parentWindow = value; }
        #endregion
        public void Initialize(string _ownerID, IFileOutter _fileOutter, INDiskOutter outter, string curUserID)
        {
            this.ownerID = _ownerID ?? NetServer.SystemUserID;
            this.currentUserID = curUserID;
            this.currentDirPath = null;
            this.cutOrCopyAction = null;
            this.fileOutter = _fileOutter;
            this.fileDirectoryOutter = outter;
            this.fileTransferingViewer.Initialize(this.fileOutter, this.FilterTransferingProject, this.parentWindow);

            Visibility visibility= !this.lockRootDirectory ? Visibility.Visible : Visibility.Collapsed;
            //this.toolStripMenuItem_copy.Visible = !this.lockRootDirectory;
            //this.toolStripMenuItem_cut.Visible = !this.lockRootDirectory;
            //this.toolStripMenuItem_rename.Visible = !this.lockRootDirectory;
            //this.toolStripMenuItem_paste.Visible = !this.lockRootDirectory;
            //this.ToolStripMenuItem_newFolder.Visible = !this.lockRootDirectory;
            this.toolStripLabel_path.Visibility = visibility;
            this.txb_Path.Visibility = visibility;
            this.toolStripButton_parent.Visibility = visibility;
            this.toolStripButton_root.Visibility = visibility;
            this.toolStripButton_newFileFolder.Visibility = visibility;

            if (this.lockRootDirectory)
            {
                //this.详细信息ToolStripMenuItem_Click(this.详细信息ToolStripMenuItem, new EventArgs());
                this.LoadDirectory(this.netDiskID, true);
            }
            else
            {
                this.GotoRoot();
            }

            this.initialized = true;
        }

        public void CancelAllTransfering()
        {
            this.fileOutter.CancelTransferingAbout(this.ownerID);
        }

        /// <summary>
        /// 刷新当前目录。
        /// </summary>
        public void RefreshDirectory()
        {
            this.LoadDirectory(this.currentDirPath, false);
        }

        private bool FilterTransferingProject(TransferingProject pro)
        {
            NDiskParameters para = Comment4NDisk.Parse(pro.Comment);
            if (para == null)
            {
                return false;
            }

            return this.ownerID == pro.DestUserID && para.NetDiskID == this.netDiskID;
        }


        private void GotoRoot()
        {
            this.currentDirPath = this.ownerID == NetServer.SystemUserID ? this.currentUserID : null;
            if (!string.IsNullOrEmpty(this.netDiskID))
            {
                this.currentDirPath = null;
            }
            this.LoadDirectory(this.currentDirPath, true);
        }
        #endregion

        #endregion


    }

    #region CutOrCopyAction
    internal class CutOrCopyAction
    {
        public CutOrCopyAction() { }
        public CutOrCopyAction(string parentPath, string itemName, bool _isFile, bool _isCutted)
        {
            this.parentPathOfCuttedOrCopyed = parentPath;
            this.itemNameOfCuttedOrCopyed = itemName;
            this.isCutted = _isCutted;
            this.isFile = _isFile;
        }

        #region ParentPathOfCuttedOrCopyed
        private string parentPathOfCuttedOrCopyed = null;
        /// <summary>
        /// 被剪切或复制物品的父目录的的路径
        /// </summary>
        public string ParentPathOfCuttedOrCopyed
        {
            get { return parentPathOfCuttedOrCopyed; }
            set { parentPathOfCuttedOrCopyed = value; }
        }
        #endregion

        #region ItemNameOfCuttedOrCopyed
        private string itemNameOfCuttedOrCopyed = null;
        /// <summary>
        /// 被复制或剪切的文件夹或文件的名称
        /// </summary>
        public string ItemNameOfCuttedOrCopyed
        {
            get { return itemNameOfCuttedOrCopyed; }
            set { itemNameOfCuttedOrCopyed = value; }
        }
        #endregion

        #region IsFile
        private bool isFile = true;
        public bool IsFile
        {
            get { return isFile; }
            set { isFile = value; }
        }
        #endregion

        #region IsCutted
        private bool isCutted = true;
        public bool IsCutted
        {
            get { return isCutted; }
            set { isCutted = value; }
        }
        #endregion
    }
    #endregion

    internal class FileOrDirectoryViewModel : CPF.CpfObject
    {
        public Collection<FileOrDirectoryTag> TagList
        {
            get { return (Collection<FileOrDirectoryTag>)GetValue(); }
            set { SetValue(value); }
        }
    }
}
