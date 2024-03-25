using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using GGTalk.Linux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TalkBase;

namespace GGTalk.Linux.Views
{
    internal class AddFriendWindow : BaseWindow
    {
        private TextBox skinTextBox_id;
        private ComboBox skinComboBox;
        private TextBox skinTextBox_comment;
        private Button btn_cancel, btn_ok;

        #region FriendID
        private string friendID = "";
        public string FriendID
        {
            get
            {
                return this.friendID;
            }
        }
        #endregion

        #region CatalogName
        private string catalogName = "";
        public string CatalogName
        {
            get { return catalogName; }
        }
        #endregion

        public AddFriendWindow(string friendID)
        {   
            this.friendID = friendID;
        }

        protected override void InitializeComponent()
        {
            this.Icon = GlobalResourceManager.Png64;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "添加好友";
            Width = 321;
            Height = 194;
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
                                Text = "添加好友",
                            },
                            new Panel
                            {
                                MarginRight = 3,
                                MarginTop = -3,
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
                                            this.DialogResult=false;
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
                        Height = 164,
                        MarginBottom = 0,
                        Background = "#fff",
                        Children =
                        {
                            new TextBlock
                            {
                                MarginLeft = 22,
                                MarginTop = 24,
                                Text = "好友账号："
                            },
                            new Border
                            {
                                MarginLeft = 88,
                                MarginTop = 19,
                                Width = 208,
                                Height = 26,
                                BorderFill = "150,180,198",
                                BorderThickness = new Thickness(1,1,1,1),
                                BorderType = BorderType.BorderThickness
                            },
                            new TextBox
                            {
                                Name=nameof(this.skinTextBox_id),
                                PresenterFor=this,
                                IsReadOnly=true,
                                Text = "10001",
                                MarginLeft = 89,
                                MarginTop = 20,
                                Width = 206,
                                Height = 24,
                                WordWarp=false,
                                VScrollBarVisibility = ScrollBarVisibility.Hidden,
                                HScrollBarVisibility = ScrollBarVisibility.Hidden,
                                Background = "240,240,240",
                                BorderThickness = new Thickness(0,0,0,0),
                                BorderType = BorderType.BorderThickness
                            },
                            new TextBlock
                            {
                                MarginLeft = 22,
                                MarginTop = 60,
                                Text = "好友分组："
                            },
                            new ComboBox
                            {
                                Name=nameof(this.skinComboBox),
                                PresenterFor=this,
                                BorderFill = "150,180,198",
                                BorderThickness = new Thickness(1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                MarginLeft = 88,
                                MarginTop = 55,
                                Width = 208,
                                Height = 26,
                                Background = "#fff",
                            },
                            new TextBlock
                            {
                                MarginLeft = 46,
                                MarginTop = 95,
                                Text = "备注："
                            },
                            new TextBox
                            {
                                Name = nameof(this.skinTextBox_comment),
                                PresenterFor=this,
                                MarginTop = 91,
                                BorderFill = "150,180,198",
                                BorderThickness = new Thickness(1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Width = 208,
                                Height = 28,
                                MaxLength=20,
                                Background = "#fff",
                                MarginLeft = 88,
                                HScrollBarVisibility = ScrollBarVisibility.Hidden,
                                VScrollBarVisibility = ScrollBarVisibility.Hidden,
                                WordWarp = false,
                            },
                            new Button
                            {
                                Classes = "commonButton",
                                MarginLeft = 154,
                                MarginTop = 128,
                                Width = 64,
                                Height = 26,
                                Content = "取消",
                                Background = "#fff",
                                Commands=
                                {
                                    {
                                        nameof(Button.Click),
                                        (s,e)=>
                                        {
                                            this.Btn_cancel_Click();
                                        }
                                    }
                                }
                            },
                            new Button
                            {
                                Classes = "commonButton",
                                MarginLeft = 232,
                                MarginTop = 128,
                                Width = 64,
                                Height = 26,
                                Content = "确定",
                                Background = "#fff",
                                Commands=
                                {
                                    {
                                        nameof(Button.Click),
                                        (s,e)=>
                                        {
                                            this.RequestAddFriend();
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
                this.skinTextBox_id = this.FindPresenterByName<TextBox>(nameof(this.skinTextBox_id));
                this.skinTextBox_id.Text = this.friendID;
                this.skinTextBox_comment = this.FindPresenterByName<TextBox>(nameof(this.skinTextBox_comment));
                this.skinComboBox = this.FindPresenterByName<ComboBox>(nameof(this.skinComboBox));
                List<string> list = Program.ResourceCenter.ClientGlobalCache.CurrentUser.GetFriendCatalogList();
                list.Remove(FunctionOptions.BlackListCatalogName);
                this.skinComboBox.Items = list;
                this.skinComboBox.SelectedIndex = 0;
            }
        }

        private void Btn_cancel_Click()
        {
            this.DialogResult = false;
            this.Close();
        }

        private void RequestAddFriend()
        {
            if (this.friendID.Length == 0)
            {
                MessageBoxEx.Show("帐号不能为空！");
                return;
            }

            try
            {
                if (Program.ResourceCenter.ClientGlobalCache.CurrentUser.GetAllFriendList().Contains(this.friendID))
                {
                    MessageBoxEx.Show("该用户已经是好友！");
                    return;
                }

                this.catalogName = this.skinComboBox.SelectedValue.ToString();
                string comment = this.skinTextBox_comment.Text ?? "";
                Program.ResourceCenter.ClientOutter.RequestAddFriend(this.friendID, comment, this.CatalogName);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show("添加好友失败！" + ee.Message);
            }

        }
    }
}
