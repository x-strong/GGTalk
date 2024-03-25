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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGTalk.Linux.Views
{
    internal class UserInfoWindow : BaseWindow
    {
        private TextBlock btn_remark, btn_remark2;
        private Button btn_edit,btn_send;
        private GGUserPlus ggUserPlus;
        private bool isFriend = false;
        public UserInfoWindow(GGUser ggUser)
        {
            this.ggUserPlus = new GGUserPlus(ggUser);
            this.isFriend = Program.ResourceCenter.ClientGlobalCache.CurrentUser.IsFriend(ggUser.ID);
            this.DataContext = this.ggUserPlus;
            this.InitializeComponent();
            this.SetRemarkBtn(this.ggUserPlus.CommentName);
        }


        private bool initialized = false;
        protected override void InitializeComponent()
        {
            if (this.initialized) { return; }
            this.Icon = GlobalResourceManager.Png64;
            this.initialized = true;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "用户资料";
            Width = 410;
            Height = 250;
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
                                Source=CommonOptions.ResourcesCatalog+"32.ico" ,
                            },
                            new TextBlock
                            {
                                MarginTop = 4,
                                MarginLeft = 27,
                                Text = "用户资料",
                            },
                            new Panel
                            {
                                MarginRight = 4f,
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
                    new Panel
                    {
                        Width = "100%",
                        Height = 220,
                        MarginBottom = 0,
                        Background = "#fff",
                        Children =
                        {
                            new Picture
                            {
                                MarginTop = 13,
                                MarginLeft = 28,
                                Width = 100,
                                Height = 100,
                                Bindings={ { nameof(Picture.Source),nameof(ggUserPlus.HeadImage)} }
                            },
                            new Button
                            {
                                Name=nameof(this.btn_send),
                                PresenterFor=this,
                                Classes = "commonButton",
                                MarginLeft = 50,
                                MarginTop = 124,
                                Width = 64,
                                Height = 26,
                                BorderFill = "207,207,207",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Content = "发消息",
                                Background = "#fff",
                            },
                            new TextBlock
                            {
                                MarginLeft = 145,
                                MarginTop = 13,
                                Text = "账号：",
                                FontSize = 14, 
                            },
                            new TextBlock
                            {
                                MarginBottom = 198,
                                MarginLeft = 195,
                                MarginTop = 13,
                                Text = "123456",
                                FontSize = 14,
                                Bindings={ { nameof(TextBlock.Text),nameof(ggUserPlus.ID)} }
                            },
                            new TextBlock
                            {
                                MarginLeft = 145,
                                MarginTop = 40,
                                Text = "名称：",
                                FontSize = 14,
                            },
                            new TextBlock
                            {
                                MarginLeft = 200,
                                MarginTop = 40,
                                Text = "哦哦哦",
                                FontSize = 14,
                                Bindings={ { nameof(TextBlock.Text),nameof(ggUserPlus.Name)} }
                            },
                            new TextBlock
                            {
                                MarginBottom = 131,
                                MarginLeft = 147,
                                MarginTop = 70,
                                Text = "注册：",
                                FontSize = 14,
                            },
                            new TextBlock
                            {
                                MarginLeft = 200,
                                MarginTop = 70,
                                Text = "2021-01-09",
                                FontSize = 14,
                                Bindings={ { nameof(TextBlock.Text),nameof(ggUserPlus.CreateTime)} }
                            },
                            new TextBlock
                            {
                                MarginBottom = 101,
                                MarginLeft = 145,
                                MarginTop = 100,
                                Text = "备注：",
                                FontSize = 14,
                            },
                            new WrapPanel
                            {
                                MarginLeft = 200,
                                MarginTop = 100,
                                Width=180,
                                Height=24,
                                Children =
                                {
                                    new TextBlock
                                    {
                                        Name=nameof(this.btn_remark),
                                        PresenterFor=this,
                                        Text = "添加备注姓名",                                        
                                        MaxWidth=140,
                                        Height=24,
                                        FontSize = 14,
                                        Cursor=Cursors.Hand,
                                        Foreground = "139,57,255",
                                        TextTrimming=TextTrimming.CharacterEllipsis,
                                        Commands={ { nameof(MouseDown),(s,e)=>this.Btn_remark_Click() } },    
                                    },
                                    new TextBlock
                                    {
                                        Name=nameof(this.btn_remark2),
                                        PresenterFor=this,
                                        Text = "添加备注姓名添加备注姓名添加备注姓名",
                                        MaxWidth=140,
                                        Height=24,
                                        FontSize = 14,
                                        Foreground = Color.Black,
                                        TextTrimming=TextTrimming.CharacterEllipsis,
                                      },
                                    new Button{
                                        Name=nameof(this.btn_edit),
                                        PresenterFor=this,
                                        BorderThickness = new Thickness(0,0,0,0),
                                        BorderType = BorderType.BorderThickness,
                                        Background = Color.Transparent,
                                        MarginLeft = 10 ,                                                        
                                        ToolTip = "编辑",
                                        Content =new Picture
                                        {
                                            Width = 18,
                                            Height = 18,
                                            Stretch = Stretch .Fill,
                                            Source =CommonOptions.ResourcesCatalog+ "edit_icon.png",
                                        },
                                        Commands={ { nameof(Button.Click),(s,e)=> { this.Btn_remark_Click(); } } }

                                    }
                                }
                            },
                            
                            new TextBlock
                            {
                                MarginLeft = 145,
                                MarginTop = 129,
                                Text = "签名：",
                                FontSize = 14,
                            },
                            new TextBox
                            {
                                Width = 180,
                                Height = 80,
                                IsReadOnly=true,
                                IsHitTestVisible=false,
                                WordWarp=true,
                                MarginLeft = 200,
                                MarginTop = 126,
                                BorderFill = "150,180,198",
                                BorderThickness = new Thickness(1, 1, 1, 1),
                                BorderType = BorderType.BorderThickness,
                                Bindings={ { nameof(TextBox.Text),nameof(ggUserPlus.Signature)} }
                            }
                        }
                    }
                }
            }) ;
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));
            //加载样式文件，文件需要设置为内嵌资源

            if (!DesignMode)//设计模式下不执行，也可以用#if !DesignMode
            {
                this.btn_remark = this.FindPresenterByName<TextBlock>(nameof(this.btn_remark));
                this.btn_remark2 = this.FindPresenterByName<TextBlock>(nameof(this.btn_remark2));
                this.btn_edit = this.FindPresenterByName<Button>(nameof(this.btn_edit));
                this.btn_send = this.FindPresenterByName<Button>(nameof(this.btn_send));
                this.btn_send.Content =this.isFriend  ? "发消息" : "加好友";
                this.btn_send.Click += Btn_send_Click;
            }
        }

        private void Btn_send_Click(object sender, RoutedEventArgs e)
        {
            if (this.isFriend)
            {
                CommonHelper.MoveToChat(this.ggUserPlus);
            }
            else {
                CommonBusinessMethod.AddFriend(this, Program.ResourceCenter, this.ggUserPlus.ID);
            }
        }

        private void SetRemarkBtn(string comment)
        {
            if (string.IsNullOrEmpty(comment))
            {
                this.btn_remark.Visibility=Visibility.Visible;
                this.btn_remark2.Visibility = Visibility.Collapsed;
                this.btn_edit.Visibility = Visibility.Collapsed;
            }
            else {
                this.btn_remark2.Text = comment;
                this.btn_remark.Visibility = Visibility.Collapsed;
                this.btn_remark2.Visibility = Visibility.Visible;
                this.btn_edit.Visibility = Visibility.Visible;
            }
        }

        private async void Btn_remark_Click()
        {
            string oldName = this.btn_remark2.Visibility == Visibility.Visible ? this.btn_remark2.Text : this.ggUserPlus.CommentName;
            EditCommentNameWindow form = new EditCommentNameWindow(oldName);
            System.Threading.Tasks.Task<object> task = form.ShowDialog_Topmost(this);
            await task.ConfigureAwait(true);

            if (Convert.ToBoolean(task.Result))
            {
                Program.ResourceCenter.ClientOutter.ChangeUnitCommentName(this.ggUserPlus.ID, form.NewName);
                this.SetRemarkBtn(form.NewName);
            }
        }
    }
}
