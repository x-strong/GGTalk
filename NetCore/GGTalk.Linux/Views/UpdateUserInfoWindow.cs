using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using GGTalk;
using GGTalk.Linux;
using GGTalk.Linux.Helpers;
using GGTalk.Linux.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGTalk.Linux.Views
{
    internal class UpdateUserInfoWindow : BaseWindow
    {
        private GGUser currentUser = new GGUser();

        private TextBox txb_Signature, txb_Name;
        private Ellipse headImg;
        private bool selfPhoto = false;
        private int headImageIndex = 0;

        private Image imageSource;

        public UpdateUserInfoWindow()
        {
            this.currentUser = Program.ResourceCenter.ClientGlobalCache.CurrentUser;
        }

        protected override void InitializeComponent()
        {
            this.Icon = GlobalResourceManager.Png64;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "修改个人信息";
            Width = 420;
            Height = 424;
            Background = null;
            Children.Add(new Panel()
            {
                BorderFill = "#619fd7",
                BorderThickness = new Thickness(1, 1, 1, 1),
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
                                MarginTop = 7,
                                MarginLeft = 25,
                                Text = "修改个人信息",
                            },
                            new Panel
                            {
                                MarginRight = 0,
                                MarginLeft = "Auto",
                                MarginTop = 0f,
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
                                            this.DialogResult = false;
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
                    new Panel
                    {
                        MarginTop = 30,
                        Width = "100%",
                        Height = 182,
                        Children =
                        {
                            new Picture
                            {
                                MarginLeft = 0,
                                MarginTop = 0,
                                Width = "100%",
                                Height = "100%",
                                Stretch = Stretch.Fill,
                                Source =CommonOptions.ResourcesCatalog+  "userInfoBackground.png"
                            },
                            new Ellipse
                            {
                                Name=nameof(this.headImg),
                                PresenterFor=this,
                                MarginLeft = 160,
                                MarginTop = 16,
                                Width = 100,
                                Height = 100,
                                StrokeFill = null,
                                Fill = new TextureFill() {Image=CommonOptions.ResourcesCatalog+ "8.png",  Stretch = Stretch.Fill   },
                                //Stretch = Stretch.Fill,
                                //Source =CommonOptions.ResourcesCatalog+  "8.png"
                            },
                            new Button
                            {
                                BorderThickness = new Thickness(0,0,0,0),
                                BorderType = BorderType.BorderThickness,
                                CornerRadius = "4",
                                Background = Color.Transparent,
                                MarginBottom = 36,
                                MarginRight = 242,
                                Width = 20,
                                Height = 20,
                                ToolTip="随机头像",
                                Content =new Picture
                                {
                                    Stretch = Stretch .Fill,
                                    Source =CommonOptions.ResourcesCatalog+  "replace_btn_icon.png",
                                },
                                Commands={ { nameof(Button.Click),(s,e)=> { this.UpdateHeadImageIndex(); } } }
                            },
                            new Button
                            {
                                BorderThickness = new Thickness(0,0,0,0),
                                BorderType = BorderType.BorderThickness,
                                CornerRadius = "4",
                                Background = Color.Transparent,
                                MarginBottom = 36,
                                MarginRight = 200,
                                Width = 20,
                                Height = 20,
                                ToolTip="上传头像",
                                Content =new Picture
                                {
                                    Stretch = Stretch .Fill,
                                    Source =CommonOptions.ResourcesCatalog+  "upload_btn_icon.png",
                                },
                                Commands={ { nameof(Button.Click),(s,e)=> { this.ClickUploadImage(); } } }
                            },
                        }
                    },
                    new Panel
                    {
                        MarginBottom = 0,
                        Width = "100%",
                        Height = 212,
                        Children =
                        {
                            new TextBlock
                            {
                                MarginLeft = 37,
                                MarginTop = 18,
                                Text = "账号："
                            },
                            new TextBlock
                            {
                                MarginLeft = 95,
                                MarginTop = 18,
                                Text = this.currentUser.ID
                            },
                            new TextBlock
                            {
                                Height = 19,
                                MarginLeft = 37,
                                MarginTop = 56,
                                Text = "姓名："
                            },
                            new TextBox
                            {
                                Name=nameof(this.txb_Name),
                                PresenterFor=this,
                                MarginLeft = 95,
                                MarginTop = 52,
                                Width = 167,
                                Height = 28,
                                MaxLength=15,
                                VScrollBarVisibility=ScrollBarVisibility.Hidden,
                                HScrollBarVisibility=ScrollBarVisibility.Hidden,
                                BorderFill = "150,180,199",
                                WordWarp=false,
                                BorderThickness = new Thickness(1,1,1,1),
                                BorderType =BorderType .BorderThickness
                            },
                            new TextBlock
                            {
                                Height = 19,
                                MarginLeft = 37,
                                MarginTop = 106,
                                Text = "签名："
                            },
                            new TextBox
                            {
                                Name=nameof(this.txb_Signature),
                                PresenterFor=this,
                               MarginLeft = 95,
                                MarginTop = 101,
                                Width = 290,
                                Height = 28,
                                WordWarp=false,
                                MaxLength=30,
                                VScrollBarVisibility=ScrollBarVisibility.Hidden,
                                HScrollBarVisibility=ScrollBarVisibility.Hidden,
                                BorderFill = "150,180,199",
                                BorderThickness = new Thickness(1,1,1,1),
                                BorderType =BorderType.BorderThickness
                            },
                            new Button
                            {
                                Classes = "commonButton",
                                MarginTop = 170,
                                MarginLeft = 233,
                                Width = 64,
                                Height = 26,
                                BorderFill = "207,207,207",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Content = "取消",
                                Background = "#fff",
                                Commands={ { nameof(Button.Click),(s,e)=> { this.Cancel_Click(); } } }
                            },
                            new Button
                            {
                                Classes = "commonButton",
                                MarginLeft = 321,
                                MarginTop = 170,
                                Width = 64,
                                Height = 26,
                                BorderFill = "207,207,207",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Content = "确定",
                                Background = "#fff",
                                Commands={ { nameof(Button.Click),(s,e)=> { this.Confirm_Click(); } } }
                            }
                        }
                    }
                }
            });
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));
            //加载样式文件，文件需要设置为内嵌资源

            if (!DesignMode)//设计模式下不执行，也可以用#if !DesignMode
            {
                this.txb_Name = this.FindPresenterByName<TextBox>("txb_Name");
                this.txb_Name.Text = this.currentUser.Name;
                this.txb_Signature = this.FindPresenterByName<TextBox>("txb_Signature");
                this.txb_Signature.Text = this.currentUser.Signature;
                this.headImg = this.FindPresenterByName<Ellipse>("headImg");
                if (Program.ResourceCenter.ClientGlobalCache.CurrentUser.HeadImageIndex >= 0)
                {
                    this.headImageIndex = Program.ResourceCenter.ClientGlobalCache.CurrentUser.HeadImageIndex;
                }
                else
                {
                    this.selfPhoto = true;
                }
                this.SetImageSource(GlobalResourceManager.GetHeadImage(Program.ResourceCenter.ClientGlobalCache.CurrentUser));
            }
        }

        private void Cancel_Click()
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Confirm_Click()
        {
            try
            {
                //0923
                if (!Program.ResourceCenter.Connected)
                {
                    MessageBoxEx.Show("离线状态，无法修改资料。");
                    return;
                }
                GGUser mine = Program.ResourceCenter.ClientGlobalCache.CurrentUser;
                string newSignature = this.txb_Signature.Text;
                string newName = this.txb_Name.Text;
                if (newSignature != mine.Signature || newName != mine.Name)
                {
                    Program.ResourceCenter.ClientOutter.ChangeMyBaseInfo(newName, newSignature, "");
                }

                if (this.selfPhoto)
                {
                    this.headImageIndex = -1;
                }

                bool headChanged = false;
                byte[] newHead = null;

                if (this.headImageIndex != mine.HeadImageIndex)
                {
                    headChanged = true;
                    if (this.headImageIndex < 0)
                    {
                        newHead = BitmapHelper.Bitmap2Byte(this.imageSource);
                    }
                }
                else
                {
                    if (this.headImageIndex < 0)
                    {
                        newHead = BitmapHelper.Bitmap2Byte(this.imageSource);
                        headChanged = newHead.Length != mine.HeadImageData.Length;
                        if (!headChanged)
                        {
                            for (int i = 0; i < newHead.Length; i++)
                            {
                                if (newHead[i] != mine.HeadImageData[i])
                                {
                                    headChanged = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (headChanged)
                {
                    Program.ResourceCenter.ClientOutter.ChangeMyHeadImage(this.headImageIndex, newHead);
                }
                //MessageBoxEx.Show("修改成功！");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show("修改失败！" + ee.Message);
            }
        }

        //更换系统自带头像索引
        private void UpdateHeadImageIndex()
        {
            this.headImageIndex = (++this.headImageIndex) % GlobalResourceManager.HeadImages.Length;
            this.SetImageSource(GlobalResourceManager.HeadImages[this.headImageIndex]);
            this.selfPhoto = false;
        }
        //自拍头像
        private void ClickSelfPhoto()
        {

        }
        //上传头像
        private void ClickUploadImage()
        {
            //string title = "选择图片";
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.AllowMultiple = true;
            //FileDialogFilter filter = new FileDialogFilter() { Name = "*" };
            //filter.Extensions = "bmp,jpeg,jpg,png";// new List<string>() { "jpg", "png" };
            //openFileDialog.Filters = new List<FileDialogFilter>() { filter };
            //if (!string.IsNullOrEmpty(title))
            //{
            //    openFileDialog.Title = title;
            //}
            //System.Threading.Tasks.Task<string[]> task = openFileDialog.ShowAsync((Window)this.Root);
            //await task.ConfigureAwait(false);
            //UiSafeInvoker.ActionOnUI<string[]>(this.SendImage4Path, task.Result);
            FileHelper.FileToOpen4Action(this, "选择图片", GlobalConsts.ImageExtensions, false, this.SendImage4Path);
        }

        private async void SendImage4Path(string[] paths)
        {
            if (paths.Length == 0) { return; }
            if (!string.IsNullOrEmpty(paths[0]))
            {
                ImageSelectorWindow imageSelectorWindow = new ImageSelectorWindow(paths[0]);
                System.Threading.Tasks.Task<object> task = imageSelectorWindow.ShowDialog_Topmost(this);
                await task.ConfigureAwait(true);
                if (Convert.ToBoolean(task.Result))
                {
                    this.selfPhoto = true;
                    this.SetImageSource(imageSelectorWindow.ContentBitmap);
                }
            }

        }

        /// <summary>
        /// 设置图像源
        /// </summary>
        /// <param name="_imageSource"></param>
        private void SetImageSource(Image _imageSource)
        {
            this.imageSource = _imageSource;
            this.headImg.Fill = new TextureFill() { Image = this.imageSource, Stretch = Stretch.Fill };  //根据ID获取头像   
        }
    }
}
