using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TalkBase;

namespace GGTalk.Linux.Views
{
    internal class UpdateGroupInfoWindow : BaseWindow
    {
        private TextBox txb_ID, txb_Name, txb_Announce;
        private Button btn_cancel, btn_ok;
        private GGGroup currentGroup;

        public UpdateGroupInfoWindow(GGGroup _group)
        {
            this.currentGroup = _group;
        }

        protected override void InitializeComponent()
        {
            this.Icon = GlobalResourceManager.Png64;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "修改讨论组信息";
            Width = 450;
            Height = 190;
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
                                MarginLeft = 25,
                                Text = "修改讨论组信息",
                            },
                            new Panel
                            {
                                MarginRight = 1,
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
                        Height = 160,
                        MarginBottom = 0,
                        Children =
                        {
                            new TextBlock
                            {
                                MarginBottom = 125,
                                MarginLeft = 38,
                                MarginTop = 19,
                                Text = "讨论组编号："
                            },
                            new Border
                            {
                                MarginLeft = 115,
                                MarginTop = 13,
                                Width = 178,
                                Height = 26,
                                BorderFill = "150,180,198",
                                BorderThickness = new Thickness(1,1,1,1),
                                BorderType = BorderType.BorderThickness
                            },
                            new TextBox
                            {
                                Classes = "commonTextBoxB",
                                Name=nameof(this.txb_ID),
                                PresenterFor=this,
                                MarginLeft = 116,
                                MarginTop = 14,
                                Width = 176,
                                Height=24,
                                IsReadOnly=true,
                                Background = "240,240,240",
                                BorderThickness = new Thickness(0,0,0,0),
                                BorderType = BorderType.BorderThickness,
                                //Commands={ { nameof(MouseDown),(s,e)=> { this.linkLabel_groupID_LinkClicked(); } } }
                            },
                            new TextBlock
                            {
                                MarginLeft = 38,
                                MarginTop = 56,
                                Text = "讨论组名称："
                            },
                            new TextBox
                            {
                                Name=nameof(this.txb_Name),
                                PresenterFor=this,
                                MarginBottom = 83,
                                MarginLeft = 115,
                                MarginTop = 52,
                                Classes = "TextBoxModeA",
                                Width = 296,
                            },
                            new TextBlock
                            {
                                MarginLeft = 38,
                                MarginTop = 92,
                                Text = "讨论组公告："
                            },
                            new TextBox
                            {
                                Name=nameof(this.txb_Announce),
                                PresenterFor=this,
                                MarginLeft = 115,
                                MarginTop = 88,
                                Classes = "TextBoxModeA",
                                Width = 296,
                                Height = 25
                            },
                            new Button
                            {
                                Name=nameof(this.btn_cancel),
                                PresenterFor=this,
                                Classes = "commonButton",
                                MarginRight = 122,
                                MarginTop = 124,
                                Width = 64,
                                Height = 26,
                                BorderFill = "207,207,207",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Content = "取消",
                                Background = "#fff",
                                Commands={ { nameof(Button.Click),(s,e)=> { this.skinButton1_Click(); } } }
                            },
                            new Button
                            {
                                Name=nameof(this.btn_ok),
                                PresenterFor=this,
                                Classes = "commonButton",
                                MarginRight = 35,
                                MarginTop = 124,
                                Width = 64,
                                Height = 26,
                                BorderFill = "207,207,207",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Content = "确定",
                                Background = "#fff",
                                Commands={ { nameof(Button.Click),(s,e)=> { this.btnClose_Click(); } } }
                            }
                        }
                    }
                }
            });
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));
            //加载样式文件，文件需要设置为内嵌资源

            if (!DesignMode)//设计模式下不执行，也可以用#if !DesignMode
            {
                this.txb_ID = this.FindPresenterByName<TextBox>(nameof(this.txb_ID));
                this.txb_Name = this.FindPresenterByName<TextBox>(nameof(this.txb_Name));
                this.txb_Announce = this.FindPresenterByName<TextBox>(nameof(this.txb_Announce));
                this.btn_cancel = this.FindPresenterByName<Button>(nameof(this.btn_cancel));
                this.btn_ok = this.FindPresenterByName<Button>(nameof(this.btn_ok));
                this.txb_ID.Text = this.currentGroup.GroupID.StartsWith(FunctionOptions.PrefixGroupID) ? this.currentGroup.GroupID.Remove(0, 1) : this.currentGroup.GroupID;
                this.txb_Name.Text = this.currentGroup?.Name;
                this.txb_Announce.Text = this.currentGroup?.Announce;
                if (this.currentGroup.CreatorID != Program.ResourceCenter.CurrentUserID)
                {
                    this.txb_Name.IsReadOnly = true;
                    this.txb_Announce.IsReadOnly = true;
                    this.btn_ok.Visibility = Visibility.Collapsed;
                    this.btn_cancel.Visibility = Visibility.Collapsed;
                }
            }
        }


        private void skinButton1_Click()
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnClose_Click()
        {
            if (this.txb_Name.Text.Trim().Length == 0)
            {
                MessageBoxEx.Show("讨论组名称不能为空！");
                return;
            }

            try
            {
                if (this.currentGroup.CreatorID != Program.ResourceCenter.CurrentUserID)
                {
                    MessageBoxEx.Show("只有创建者才能修改");
                    return;
                }
                Program.ResourceCenter.ClientOutter.ChangeGroupInfo(this.currentGroup.GroupID, this.txb_Name.Text.Trim(), this.txb_Announce.Text.Trim());

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show("创建讨论组失败！" + ee.Message);
            }
        }

        private void linkLabel_groupID_LinkClicked()
        {
            Clipboard.SetData((DataFormat.Text, this.txb_ID.Text));
            MessageBoxEx.Show("已复制组编号！");
        }
    }
}
