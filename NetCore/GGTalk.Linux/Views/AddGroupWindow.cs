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
using TalkBase;

namespace GGTalk.Linux.Views
{
    internal class AddGroupWindow : BaseWindow
    {
        private GGGroup currentGroup;
        private TextBox tb_id, tb_name;
        private TextBox textbox_comment;

        public AddGroupWindow(GGGroup ggGroup)
        {
            this.currentGroup = ggGroup;
        }

        protected override void InitializeComponent()
        {
            this.Icon = GlobalResourceManager.Png64;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "加入群组";
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
                                MarginTop = 1,
                                MarginLeft = 24,
                                Text = "加入群组",
                            },
                            new Panel
                            {
                                MarginRight = 0,
                                MarginLeft = "Auto",
                                MarginTop = -6,
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
                        MarginLeft = 0,
                        Width = "100%",
                        Height = 164,
                        MarginBottom = 0,
                        Background = "#fff",
                        Children =
                        {
                            new TextBlock
                            {
                                MarginLeft = 21,
                                MarginTop = 19,
                                Text = "群账号："
                            },
                            new Border
                            {
                                MarginLeft = 85,
                                MarginTop = 14,
                                Width = 208,
                                Height = 27,
                                BorderFill = "150,180,198",
                                BorderThickness = new Thickness(1,1,1,1),
                                BorderType = BorderType.BorderThickness
                            },
                            new TextBox
                            {
                                Name=nameof(this.tb_id),
                                PresenterFor=this,
                                WordWarp = false,
                                HScrollBarVisibility = ScrollBarVisibility.Hidden,
                                VScrollBarVisibility = ScrollBarVisibility.Hidden,
                                Text = "",
                                IsReadOnly=true,
                                MarginLeft = 86,                                
                                MarginTop = 15,
                                Height = 25,
                                Width=206,
                                Background = "240,240,240",
                                BorderThickness = new Thickness(0,0,0,0),
                                BorderType = BorderType.BorderThickness
                            },
                            new TextBlock
                            {
                                MarginLeft = 21,
                                MarginTop = 57,
                                Text = "群名称："
                            },
                            new Border
                            {
                                MarginLeft = 84,
                                MarginTop = 52,
                                Width = 208,
                                Height = 27,
                                BorderFill = "150,180,198",
                                BorderThickness = new Thickness(1,1,1,1),
                                BorderType = BorderType.BorderThickness
                            },
                            new TextBox
                            {
                                Name=nameof(this.tb_name),
                                PresenterFor=this,
                                Text = "讨论组1",
                                IsReadOnly=true,
                                Width = 206,
                                MarginLeft = 85,
                                MarginTop = 53,
                                Height = 25,
                                Background = "240,240,240",
                                BorderThickness = new Thickness(0,0,0,0),
                                BorderType = BorderType.BorderThickness,
                                HScrollBarVisibility = ScrollBarVisibility.Hidden,
                                VScrollBarVisibility = ScrollBarVisibility.Hidden,
                                WordWarp = false,
                            },
                            new TextBlock
                            {
                                MarginLeft = 33,
                                MarginTop = 95,
                                Text = "备注："
                            },
                            new TextBox
                            {
                                Name=nameof(this.textbox_comment),
                                PresenterFor=this,
                                MarginTop = 90,
                                WordWarp=false,                                
                                MaxLength=20,
                                BorderFill = "150,180,198",
                                BorderThickness = new Thickness(1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Width = 208,
                                Height = 26,
                                Background = "#fff",
                                MarginLeft = 84,
                                Classes = "commonTextBoxB"
                            },
                            new Button
                            {
                                Classes = "commonButton",
                                MarginLeft = 149,
                                MarginTop = 127,
                                Width = 64,
                                Height = 26,
                                BorderFill = "207,207,207",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness,
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
                                MarginLeft = 228,
                                MarginTop = 127,
                                Width = 64,
                                Height = 26,
                                BorderFill = "207,207,207",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Content = "确定",
                                Background = "#fff",
                                Commands=
                                {
                                    {
                                        nameof(Button.Click),
                                        (s,e)=>
                                        {
                                            this.RequestAddGroup();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));
            if (!DesignMode)//设计模式下不执行，也可以用#if !DesignMode
            {
                this.tb_id = this.FindPresenterByName<TextBox>(nameof(this.tb_id));
                this.tb_id.Text = this.currentGroup.ID.Substring(1);
                this.tb_name = this.FindPresenterByName<TextBox>(nameof(this.tb_name));
                this.tb_name.Text = this.currentGroup.Name;
                this.textbox_comment = this.FindPresenterByName<TextBox>("textbox_comment");
            }
        }

        private void RequestAddGroup()
        {
            string groupID = this.currentGroup.ID;

            if (groupID.Length == 0)
            {
                MessageBoxEx.Show("组编号不能为空！");
                return;
            }
            try
            {
                IGroup group = Program.ResourceCenter.ClientOutter.SearchGroup(groupID);
                if (group == null)
                {
                    MessageBoxEx.Show("讨论组不存在！");
                    return;
                }
                if (group.MemberList.Contains(Program.ResourceCenter.CurrentUserID))
                {
                    MessageBoxEx.Show("您已存在讨论组中！");
                    return;
                }
                string comment = this.textbox_comment.Text.Trim();
                Program.ResourceCenter.ClientOutter.RequestAddGroup(groupID, comment);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show("申请加入群组失败！" + ee.Message);
            }
        }

        private void Btn_cancel_Click()
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
