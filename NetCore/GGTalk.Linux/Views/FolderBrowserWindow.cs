using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using ESBasic.Helpers;
using GGTalk.Linux.Controls;
using GGTalk.Linux.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGTalk.Linux.Views
{
    internal class FolderBrowserWindow : Window
    {
        private NetWorkListBoxTemplate listBox;
        private TextBlock toolStripLabel_path;
        private TextBox txb_Path, txb_fileName;
        private WrapPanel wrapPanel;
        private ScrollViewer scrollViewer;
        private Button toolStripButton_parent, toolStripButton_root, toolStripButton_refresh, toolStripButton_newFileFolder;
        private Button btn_ok, btn_cancel;
        private Panel panel_fileName;
        /// <summary>
        /// 是否是选择文件夹 （默认选择文件）
        /// </summary>
        public bool IsChooseFolder = false;
        /// <summary>
        /// 过滤后缀名 文件  null 不过滤
        /// </summary>
        public ICollection<string> Extensions = null;

        private FileChooserAction fileChooserAction = FileChooserAction.Open;

        private string initialFileName = string.Empty;

        private SelectionMode selectionMode = SelectionMode.Single;//选择文件模式

        public FolderBrowserWindow(string _title = "选择文件")
        {
            this.Title = _title;
            this.Closed += delegate
            {
                tcs.TrySetResult(selectedPaths);
                if (selectedPaths == null || selectedPaths.Length == 0) { tcs2.TrySetResult(null); }
                else
                {
                    tcs2.TrySetResult(this.selectedPaths[0]);
                }
            };
            GlobalResourceManager.Logger.LogWithTime("FolderBrowserWindow new完成");
        }

        protected override void InitializeComponent()
        {
            this.Icon = GlobalResourceManager.Png64;
            CanResize = true;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
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
                Children =
                {
                    new Grid
                    {
                        Size = SizeField.Fill,
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
                                Height = 30,
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
                                        Text = this.Title,
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
                            new Panel()
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
                                                    },//new WrapPanel
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
                                                    },//new Button
                                                    //{
                                                    //    Name=nameof(this.toolStripButton_newFileFolder),
                                                    //    PresenterFor=this,
                                                    //    Classes = "NetWorkButton",
                                                    //    MarginLeft = 10,
                                                    //    MarginTop = 5,
                                                    //    Cursor = Cursors.Hand,
                                                    //    ToolTip = "新建文件夹",
                                                    //    Width = 20,
                                                    //    Height = 20,
                                                    //    Content =
                                                    //    new Picture
                                                    //    {
                                                    //        Stretch = Stretch.Fill,
                                                    //        Size = SizeField.Fill,
                                                    //        Source = CommonOptions.ResourcesCatalog+ "btn_addfile.png",
                                                    //    }
                                                    //},
                                                //}
                                            //}
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
                                                CommandContext=this,
                                                Content = new  WrapPanel
                                                {
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
                                                            ItemTemplate = typeof(NetWorkListBoxItem),
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
                                                    },
                                                    //{
                                                    //    Grid.RowSpan,2
                                                    //}
                                                }
                                            },
                                            new Panel
                                            {
                                                Name=nameof(this.panel_fileName),
                                                PresenterFor=this,
                                                Children =
                                                {
                                                    new TextBlock
                                                    {
                                                        MarginLeft = 10,
                                                        MarginTop = 7,
                                                        PresenterFor=this,
                                                        Text = "文件名",
                                                    },
                                                    new TextBox
                                                    {
                                                        Name=nameof(this.txb_fileName),
                                                        PresenterFor=this,
                                                        MarginLeft = 56,
                                                        Width = 728,
                                                        Height = 25,
                                                        WordWarp=false,
                                                    }
                                                },
                                                Size = SizeField.Fill,
                                                Attacheds=
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
                            new Panel()
                            {
                                Background = "#eee",
                                Size = SizeField.Fill,
                                Children =
                                {
                                    new Button
                                    {
                                        Name=nameof(this.btn_ok),
                                        PresenterFor=this,
                                        MarginTop = 2,
                                        MarginRight = 90,
                                        Classes = "commonButton",
                                        Width = 64,
                                        Height = 26,
                                        BorderFill = "207,207,207",
                                        BorderThickness = new Thickness (1,1,1,1),
                                        BorderType = BorderType.BorderThickness,
                                        Content = "选择",
                                        Background = "#fff"
                                    },
                                    new Button
                                    {
                                        Name=nameof(this.btn_cancel),
                                        PresenterFor=this,
                                        MarginTop = 2,
                                        MarginRight = 12,
                                        Classes = "commonButton",
                                        Width = 64,
                                        Height = 26,
                                        BorderFill = "207,207,207",
                                        BorderThickness = new Thickness (1,1,1,1),
                                        BorderType = BorderType.BorderThickness,
                                        Content = "取消",
                                        Background = "#fff"
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
                                        2
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
                this.txb_fileName = this.FindPresenterByName<TextBox>(nameof(this.txb_fileName));
                this.txb_fileName.Text = this.initialFileName;
                this.scrollViewer = this.FindPresenterByName<ScrollViewer>(nameof(this.scrollViewer));
                this.wrapPanel = this.FindPresenterByName<WrapPanel>(nameof(this.wrapPanel));
                this.listBox = this.FindPresenterByName<NetWorkListBoxTemplate>(nameof(this.listBox));
                this.listBox.SelectionChanged += ListBox_SelectionChanged;
                this.listBox.SelectionMode = this.selectionMode;
                this.toolStripLabel_path = this.FindPresenterByName<TextBlock>(nameof(this.toolStripLabel_path));
                this.txb_Path = this.FindPresenterByName<TextBox>("txb_Path");
                this.toolStripButton_parent = this.FindPresenterByName<Button>("toolStripButton_parent");
                this.toolStripButton_parent.Click += ToolStripButton_parent_Click;
                this.toolStripButton_root = this.FindPresenterByName<Button>("toolStripButton_root");
                this.toolStripButton_root.Click += ToolStripButton_root_Click;
                this.toolStripButton_refresh = this.FindPresenterByName<Button>("toolStripButton_refresh");
                this.toolStripButton_refresh.Click += ToolStripButton_refresh_Click;
                //this.toolStripButton_newFileFolder = this.FindPresenterByName<Button>("toolStripButton_newFileFolder");
                //this.toolStripButton_newFileFolder.Click += ToolStripButton_newFileFolder_Click;

                this.btn_ok = this.FindPresenterByName<Button>(nameof(this.btn_ok));
                this.btn_ok.Click += Btn_ok_Click;
                this.btn_cancel = this.FindPresenterByName<Button>(nameof(this.btn_cancel));
                this.btn_cancel.Click += Btn_cancel_Click;
                this.panel_fileName = this.FindPresenterByName<Panel>(nameof(this.panel_fileName));
                this.SetPanle();
                this.GotoRoot();
            }
        }

        private void ListBox_SelectionChanged(object sender, EventArgs e)
        {
            FileOrDirectoryTag fileOrDirectoryTag = this.listBox.SelectedValue as FileOrDirectoryTag;
            if (fileOrDirectoryTag == null || !fileOrDirectoryTag.IsFile) return;
            this.txb_fileName.Text = fileOrDirectoryTag.Name;
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> tagFullNames = new List<string>();
                if (this.fileChooserAction == FileChooserAction.Open)
                {


                    foreach (object item in this.listBox.SelectedItems)
                    {
                        FileOrDirectoryTag fileOrDirectoryTag = item as FileOrDirectoryTag;
                        if (fileOrDirectoryTag == null)
                        {
                            continue; //若当前选择的item为空，或者是文件类型 直接过滤掉
                        }
                        if (this.listBox.SelectedItems.Count() == 1 && !fileOrDirectoryTag.IsFile)
                        {
                            //若当前选择的是文件夹 直接进入到子目录
                            this.FileBoxItem_DoubleClicked(fileOrDirectoryTag);
                            return;
                        }
                        fileOrDirectoryTag.FullName = this.currentDirPath + fileOrDirectoryTag.Name;
                        tagFullNames.Add(fileOrDirectoryTag.FullName);
                    }
                }
                else if (this.fileChooserAction == FileChooserAction.SelectFolder)
                {
                    FileOrDirectoryTag fileOrDirectoryTag = this.listBox.SelectedValue as FileOrDirectoryTag;
                    if (fileOrDirectoryTag == null || fileOrDirectoryTag.IsFile) { tagFullNames.Add(this.currentDirPath); }//若当前选择的item为空，或者不是文件类型 直接返回 当前目录路径
                    else { tagFullNames.Add(this.currentDirPath + fileOrDirectoryTag.Name); }
                }
                else if (this.fileChooserAction == FileChooserAction.Save)
                {
                    FileOrDirectoryTag fileOrDirectoryTag = this.listBox.SelectedValue as FileOrDirectoryTag;
                    if (string.IsNullOrEmpty(this.txb_fileName.Text)) { return; }
                    string fullName = this.currentDirPath + this.txb_fileName.Text.Trim();
                    //if (File.Exists(fullName))
                    //{

                    //}
                    tagFullNames.Add(fullName);
                }

                this.SelectFileOrDirectory(tagFullNames);
            }
            catch (Exception ee)
            {
                GlobalResourceManager.WriteErrorLog(ee);
            }
           
        }

        private void SetPanle()
        {
            if (this.fileChooserAction == FileChooserAction.Open)
            {
                this.panel_fileName.Visibility = Visibility.Collapsed;
                Grid.RowSpan(this.scrollViewer, 2);
            }
            else if (this.fileChooserAction == FileChooserAction.Save)
            {
                this.btn_ok.Content = "保存";
                this.listBox.SelectionMode = SelectionMode.Single;
            }
            else if (this.fileChooserAction == FileChooserAction.SelectFolder)
            {
                this.btn_ok.Content = "选择";
                this.listBox.SelectionMode = SelectionMode.Single;
                this.panel_fileName.Visibility = Visibility.Collapsed;
                Grid.RowSpan(this.scrollViewer, 2);
            }
        }

        private void ToolStripButton_newFileFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToolStripButton_refresh_Click(object sender, RoutedEventArgs e)
        {
            this.LoadDirectory(this.currentDirPath);
        }

        private void ToolStripButton_root_Click(object sender, RoutedEventArgs e)
        {
            this.GotoRoot();
        }

        private void ToolStripButton_parent_Click(object sender, RoutedEventArgs e)
        {
            this.GotoParent();
        }

        public void FileBoxItem_DoubleClicked(FileOrDirectoryTag tag)
        {
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
                this.LoadDirectory(tempPath);
                return;
            }
            tag.FullName = this.currentDirPath + tag.Name;
            this.SelectFileOrDirectory(new List<string>() { tag.FullName });
        }

        private void SelectFileOrDirectory(List<string> tags)
        {
            if (tags == null||tags.Count==0)
            {
                return;
            }
            this.selectedPaths = tags.ToArray(); 
            BeginInvoke(this.Close);
        }

        private string[] selectedPaths = null;

        TaskCompletionSource<string[]> tcs = new TaskCompletionSource<string[]>();
        TaskCompletionSource<string> tcs2 = new TaskCompletionSource<string>();
        /// <summary>
        /// 打开选择文件
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="allowMultiple">允许多选</param>
        /// <returns></returns>
        public Task<string[]> ShowDialog_Open(Window parent, bool allowMultiple = false)
        {
            this.fileChooserAction = FileChooserAction.Open;
            this.selectionMode = allowMultiple ? SelectionMode.Extended : SelectionMode.Single;
            GlobalResourceManager.Logger.LogWithTime("FolderBrowserWindow 开始showDialog");
            this.ShowDialog(parent);
            GlobalResourceManager.Logger.LogWithTime("FolderBrowserWindow 完成showDialog");
            return tcs.Task;
        }

        /// <summary>
        /// 打开保存对话框
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public Task<string> ShowDialog_Save(Window parent, string _initialFileName)
        {
            this.fileChooserAction = FileChooserAction.Save;
            this.initialFileName = _initialFileName;
            this.ShowDialog(parent);
            return tcs2.Task;
        }

        /// <summary>
        /// 打开选择文件夹对话框
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public Task<string> ShowDialog_SelectFolder(Window parent)
        {
            this.fileChooserAction = FileChooserAction.SelectFolder;
            this.ShowDialog(parent);
            return tcs2.Task;
        }

        private string currentDirPath;
        private FileOrDirectoryViewModel viewModel = new FileOrDirectoryViewModel() { TagList = new Collection<FileOrDirectoryTag>() };
        private void GotoRoot()
        {
            this.currentDirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            this.LoadDirectory(this.currentDirPath);
        }

        private void GotoParent()
        {
            try
            {
                if (string.IsNullOrEmpty(this.currentDirPath))
                {
                    return;
                }

                DirectoryInfo directoryInfo = new DirectoryInfo(this.currentDirPath);
                if (directoryInfo.Parent == null)
                {
                    //this.GotoRoot();
                    return;
                }

                this.LoadDirectory(directoryInfo.Parent.FullName);
            }
            catch (Exception ee)
            {

            }
        }

        private void LoadDirectory(string path)
        {
            try
            {
                this.viewModel.TagList.Clear();
                DirectoryInfo sharedDirectory = Directory.CreateDirectory(path);
                if (sharedDirectory == null)
                {
                    MessageBoxEx.Show("未找到该目录" + path);
                }
                else
                {
                    #region Action
                    //this.wrapPanel.Children.Clear();
                    List<DirectoryInfo> directoryInfos = sharedDirectory.GetDirectories().ToList();
                    directoryInfos.Sort((x,y)=> x.Name.CompareTo(y.Name));
                    foreach (DirectoryInfo dirDetail in directoryInfos)
                    {
                        FileOrDirectoryTag tag = new FileOrDirectoryTag(dirDetail.Name, 0, dirDetail.CreationTime, false);
                        //FileBoxItem item = new FileBoxItem(tag);
                        this.AddFileBoxItem(tag);
                    }

                    if (this.fileChooserAction == FileChooserAction.Open)
                    {
                        List<FileInfo> fileInfos = sharedDirectory.GetFiles().ToList();
                        fileInfos.Sort((x, y) => x.Name.CompareTo(y.Name));
                        foreach (FileInfo file in fileInfos)
                        {
                            FileOrDirectoryTag tag = new FileOrDirectoryTag(file.Name, file.Length, file.CreationTime, true);
                            if (this.ContainsSuffixFile(tag.Name))
                            {
                                tag.ToolTip = string.Format("大    小：{0}\n创建日期：{1}", PublicHelper.GetSizeString((uint)file.Length), file.CreationTime);
                                this.AddFileBoxItem(tag);
                            }
                        }
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

                this.txb_Path.Text = this.currentDirPath;

                #endregion

                this.DataContext = this.viewModel;
                //this.listBox.Items = this.viewModel.TagList;
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(ee.Message);
            }
        }

        private void AddFileBoxItem(FileOrDirectoryTag tag)
        {
            this.viewModel.TagList.Add(tag);
        }

        /// <summary>
        /// 是否包含后缀名 Extensions为null 表示都包含
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool ContainsSuffixFile(string fileName)
        {
            if (this.Extensions == null) { return true; }

            string[] strs = fileName.Split('.');
            if (strs == null || strs.Length == 0)
            {
                return false;
            }
            return this.Extensions.Contains(strs[strs.Length - 1]);
        }
    }
}
