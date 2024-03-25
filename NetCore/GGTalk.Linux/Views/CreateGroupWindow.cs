using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using GGTalk;
using GGTalk.Linux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkBase;

namespace GGTalk.Linux.Views
{
    internal class CreateGroupWindow : BaseWindow
    {
        string groupID;
        private CheckBox checkBox;
        private TextBox txb_GroupName, txb_GroupAnnounce;

        #region Group
        private GGGroup group = null;
        public GGGroup Group
        {
            get
            {
                return this.group;
            }
        }
        #endregion

        public CreateGroupWindow()
        {
            this.groupID = FunctionOptions.PrefixGroupID + Program.ResourceCenter.CurrentUserID + "_" + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
        }
        protected override void InitializeComponent()
        {
            this.Icon = GlobalResourceManager.Png64;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "创建讨论组";
            Width = 316;
            Height = 184;
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
                                Text = "创建讨论组",
                            },
                            new Panel
                            {
                                MarginRight = 2,
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
                        Height = 154,
                        MarginBottom = 0,
                        Background = "#FFF",
                        Children =
                        {
                            new CheckBox
                            {
                                Name=nameof(this.checkBox),
                                PresenterFor=this,
                                MarginRight = 64,
                                Width = 22,
                                Height = 22,
                                MarginTop = 9.6f
                            },
                            new TextBlock
                            {
                                MarginLeft = 254,
                                Text = "密聊群",
                                MarginTop = 12
                            },
                            new TextBlock
                            {
                                MarginLeft = 18,
                                MarginTop = 46,
                                Text = "组名称："
                            },
                            new TextBox
                            {
                                Classes = "TextBoxModeA",
                                Name=nameof(this.txb_GroupName),
                                PresenterFor=this,
                                MarginBottom = 87,
                                MarginLeft = 82,
                                MarginTop = 41,
                                Width = 208,
                                Height = 26,
                                WordWarp=false,
                                Background = "#fff",                               
                            },
                            new TextBlock
                            {
                                MarginLeft = 18,
                                MarginTop = 83,
                                Text = "组公告："
                            },
                            new TextBox
                            {
                                Classes = "TextBoxModeA",
                                Name=nameof(this.txb_GroupAnnounce),
                                PresenterFor=this,
                                MarginTop = 78,
                                WordWarp=false,
                                Width = 208,
                                Height = 28,
                                Background = "#fff",
                                MarginLeft = 82,
                            },
                            new Button
                            {
                                Classes = "commonButton",
                                MarginLeft = 148,
                                MarginTop = 121,
                                Width = 64,
                                Height = 26,
                                Content = "取消",
                                Background = "#fff",
                                Commands={ { nameof(Button.Click),(s,e)=> { this.Btn_cancel_Click(); } } }
                            },
                            new Button
                            {
                                Classes = "commonButton",
                                MarginLeft = 226,
                                MarginTop = 121,
                                Width = 64,
                                Height = 26,
                                Content = "确定",
                                Background = "#fff",
                                Commands={ { nameof(Button.Click),(s,e)=> { this.Btn_ok_Click(); } } }
                            }
                        }
                    }
                }
            });
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));
            //加载样式文件，文件需要设置为内嵌资源

            if (!DesignMode)//设计模式下不执行，也可以用#if !DesignMode
            {
                this.checkBox = this.FindPresenterByName<CheckBox>(nameof(this.checkBox));
                this.txb_GroupAnnounce = this.FindPresenterByName<TextBox>(nameof(this.txb_GroupAnnounce));
                this.txb_GroupName = this.FindPresenterByName<TextBox>(nameof(this.txb_GroupName));
            }
        }

        private async void Btn_ok_Click()
        {
            if (this.groupID.Length == 0)
            {
                MessageBoxEx.Show("讨论组帐号不能为空！");
                return;
            }

            if (this.txb_GroupName.Text.Trim().Length == 0)
            {
                MessageBoxEx.Show("讨论组名称不能为空！");
                return;
            }

            try
            {
                List<string> newMembers = new List<string>();

                newMembers.Add(Program.ResourceCenter.CurrentUserID);
                UserSelectedWindow form = new UserSelectedWindow();
                GGGroup newGroup = new GGGroup();
                newGroup.CreatorID = Program.ResourceCenter.CurrentUserID;
                form.Initialize4Group(newGroup, false);
                Task<object> task = form.ShowDialog_Topmost(this);
                await task.ConfigureAwait(true);
                if (Convert.ToBoolean(task.Result))
                {
                    List<string> list = form.UserIDSelected;
                    newMembers.AddRange(list);
                }

                CreateGroupResult res = Program.ResourceCenter.ClientOutter.CreateGroup(groupID, this.txb_GroupName.Text.Trim(), this.txb_GroupAnnounce.Text, new List<string>(newMembers.Distinct()), this.checkBox.IsChecked.GetValueOrDefault());
                if (res == CreateGroupResult.GroupExisted)
                {
                    MessageBoxEx.Show("同ID的讨论组已经存在！");
                    return;
                }
                this.group = Program.ResourceCenter.ClientGlobalCache.GetGroup(groupID);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show("创建讨论组失败！" + ee.Message);
            }
        }

        private void Btn_cancel_Click()
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
