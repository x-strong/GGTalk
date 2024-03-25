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
using GGTalk.Linux.Controls;
using GGTalk.Linux.Controls.Templates;
using GGTalk.Linux.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGTalk.Linux.Views
{
    internal class UserSelectedWindow : BaseWindow
    {

        private GGGroup currentGroup;
        private UserSelectedViewModel userSelectedViewModel = new UserSelectedViewModel();
        private bool restrictDelete = false;//限制删除操作（若为true，只有管理员和创建者才有权）
        private bool isCreator;
        private ListBox userSelectedListBox;
        private BaseUserListBox baseUserListBox;
        private TextBlock tb_selectedCount;
        private TextBox txb_input;
        public List<string> UserIDSelected
        {
            get
            {
                List<string> currentMemberList = new List<string>();
                foreach (var item in userSelectedViewModel.UserSelectedModels)
                {
                    currentMemberList.Add(item.ID);
                }
                return currentMemberList;
            }
        }
        public Panel searchPanel;
        private bool showMyself = true;

        public void Initialize4Group(GGGroup ggGroup, bool _showMyself)
        {
            this.currentGroup = ggGroup;
            this.showMyself = _showMyself;
        }

        public void Initialize()
        {
            this.baseUserListBox.Initialize4Friend();
            if (!showMyself)
            {
                this.baseUserListBox.DeleteUser(Program.ResourceCenter.CurrentUserID);
            }
            this.isCreator = this.currentGroup.CreatorID == Program.ResourceCenter.CurrentUserID;
            
            this.userSelectedViewModel.UserSelectedModels = new Collection<UserSelectedModel>();
            foreach (string friendID in this.currentGroup.MemberList)
            {
                if (friendID == Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserID)
                {
                    continue;
                }
                GGUser friend = Program.ResourceCenter.ClientGlobalCache.GetUser(friendID);
                this.AddUserSelectedItem(friend);
            }
            this.DataContext = userSelectedViewModel;
            this.restrictDelete = true;
        }


        protected override void InitializeComponent()
        {
            this.Icon = GlobalResourceManager.Png64;
            Size = SizeField.Fill;
            CommandContext = this;
            ViewFill color = "#fff";
            ViewFill hoverColor = "255,255,255,40";
            Title = "编辑群成员";
            Width = 600;
            Height = 612;
            Background = null;
            Children.Add(new Panel
            {
                BorderFill = "#619fd7",
                BorderThickness = new Thickness(2, 2, 2, 2),
                BorderType = BorderType.BorderThickness,
                ClipToBounds = true,
                Background = "247,250,253",
                MarginRight = 0f,
                Width = "100%",
                Height = "100%",
                Children = //内容元素放这里
                {
                    new Panel
                    {
                        Height = 66,
                        Width = "100%",
                        MarginTop = 0,
                        Background = "#619fd7",
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
                                Text = "添加用户",
                            },
                            new Panel
                            {
                                PresenterFor = this,
                                Name = nameof(searchPanel),
                                MarginLeft = 7,
                                MarginBottom = 4,
                                Width = 272,
                                Height = 28,
                                Background = "#fff",
                                Children =
                                {
                                    new Border
                                    {
                                       Width = 272,
                                        Height = 28,
                                        BorderFill = "#619fd7",
                                        BorderThickness=new Thickness(2,2,2,2),
                                        BorderType= BorderType.BorderThickness,
                                    },
                                    new Picture
                                    {
                                        MarginLeft = 6f,
                                        MarginTop = 6f,
                                        Width = 16,
                                        Height = 16,
                                        Stretch = Stretch.Fill,
                                        Source =CommonOptions.ResourcesCatalog+ "search_icon.png",
                                    },
                                    new TextBox
                                    {
                                        Classes="commonTextBoxB",
                                        Name=nameof(this.txb_input),
                                        PresenterFor=this,
                                        AcceptsReturn=false,
                                        MarginLeft = 22f,
                                        MarginTop = 1.5f,
                                        Width = 230,
                                        Height = 24,
                                        WordWarp=false,
                                        BorderThickness = new Thickness (0,0,0,0),
                                        BorderType = BorderType.BorderThickness,
                                        HScrollBarVisibility = ScrollBarVisibility.Hidden,
                                    },
                                    new Button
                                    {
                                        Classes = "CloseSearch",
                                        MarginRight = 4,
                                        MarginTop = 3,
                                        Width = 22,
                                        Height = 22,
                                        Content =
                                        new Picture
                                        {                                            
                                            Width = 22,
                                            Height = 22,
                                            Stretch = Stretch.Fill,
                                            Source =CommonOptions.ResourcesCatalog+ "imagea.png",
                                        },
                                        Commands={ { nameof(Button.Click),(s,e)=> { this.CloseButton_Click(); } } }
                                    },

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
                            new TextBlock
                            {
                                Name=nameof(this.tb_selectedCount),
                                PresenterFor=this,
                                MarginLeft = 337,
                                MarginTop = 41,
                                Text = "已选择 ：（5人）",
                                FontSize = 15,
                                Bindings={ { nameof(TextBlock.Text), nameof(UserSelectedViewModel.Count), null, BindingMode.OneWay, x => "已选择 ：("+ x + "人)" } }
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
                    new Panel
                    {
                        Height = 502,
                        Width = "100%",
                        MarginTop = 66,
                        Children =
                        {
                            new BaseUserListBox(){
                                Name=nameof(this.baseUserListBox),
                                PresenterFor=this,
                                MarginLeft = 3,
                                Width = 290,
                                Height = 500,
                            },
                            new ScrollViewer
                            {
                                Background = "#fff",
                                MarginRight = 3,
                                Width = 290,
                                Height = 500,
                                Content =new WrapPanel
                                {
                                    Size = SizeField.Fill,
                                    Children =
                                    {
                                        new ListBox
                                        {
                                            Name=nameof(this.userSelectedListBox),
                                            PresenterFor=this,
                                            Size = SizeField.Fill,
                                            ItemTemplate = typeof(UserSelectedListBoxItemTemplate),
                                            Bindings =
                                            {
                                                {
                                                    nameof(ListBox.Items),
                                                    nameof(UserSelectedViewModel.UserSelectedModels)
                                                }
                                            },
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new Panel
                    {
                        Height = 44,
                        Width = "100%",
                        MarginBottom = 0,
                        Children =
                        {
                            new Button
                            {
                                Classes = "commonButton",
                                MarginLeft = 430,
                                MarginTop = 9,
                                Width = 64,
                                Height = 26,
                                BorderFill = "207,207,207",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Content = "取消",
                                Background = "#fff",
                                Commands={ { nameof(Button.Click),(s,e)=> { this.DialogResult = false;this.Close(); } } }
                            },
                            new Button
                            {
                                Classes = "commonButton",
                                MarginLeft = 508,
                                MarginTop = 9,
                                Width = 64,
                                Height = 26,
                                BorderFill = "207,207,207",
                                BorderThickness = new Thickness (1,1,1,1),
                                BorderType = BorderType.BorderThickness,
                                Content = "确定",
                                Background = "#fff",
                                Commands={ { nameof(Button.Click),(s,e)=> { this.DialogResult = true;this.Close(); } } }
                            }
                        }
                    },
                }
            }) ;
            //加载样式文件，文件需要设置为内嵌资源
            LoadStyleFile(string.Format("res://{0}/Main.css", CommonOptions.ProcessName));


            if (!DesignMode)//设计模式下不执行
            {
                this.baseUserListBox = this.FindPresenterByName<BaseUserListBox>(nameof(this.baseUserListBox));
                this.baseUserListBox.User_DoubleTapped += BaseUserListBox_User_DoubleTapped;
                this.userSelectedListBox = this.FindPresenterByName<ListBox>(nameof(this.userSelectedListBox));
                this.txb_input = this.FindPresenterByName<TextBox>(nameof(this.txb_input));
                this.txb_input.KeyDown += Txb_input_KeyDown;
                this.tb_selectedCount = this.FindPresenterByName<TextBlock>(nameof(this.tb_selectedCount));
                IEnumerable<Button> s= this.Find<Button>();
                //Button button = this.Find<Button>("btn_delete");
                this.Initialize();
            }
        }         

        public void UserSelectedListBoxItemTemplate_RemoveUserSelectedItemEvent(UserSelectedModel model)
        {
            if (model == null) { return; }
            if (this.restrictDelete)
            {
                if (!isCreator)
                {
                    MessageBoxEx.Show("您无权操作！");
                    return;
                }
                if (model.ID == this.currentGroup.CreatorID)
                {
                    MessageBoxEx.Show("不能删除管理员！");
                    return;
                }
            }
            this.userSelectedViewModel.UserSelectedModels.Remove(model);
            this.userSelectedViewModel.Count = this.userSelectedViewModel.UserSelectedModels.Count;
        }

        private void BaseUserListBox_User_DoubleTapped(GGUserPlus ggUserPlus)
        {
            this.AddUserSelectedItem(ggUserPlus);
        }

        private void Txb_input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Keys.Enter)
            {
                this.txb_input.FocusLastIndex();
                this.DoSearch();
            }
        }

        private void DoSearch()
        {
            string idOrName = this.txb_input.Text.Trim();
            List<GGUser> users = Program.ResourceCenter.ClientGlobalCache.SearchUser(idOrName);
            this.baseUserListBox.Initialize("查找结果", users);
        }

        //点击查询的关闭按钮
        private void CloseButton_Click()
        {
            this.txb_input.Text = string.Empty;
            this.baseUserListBox.Initialize4Friend();
        }

        private void RemoveUserSelectedItem_Click(CpfObject sender)
        {

        }

        private void AddUserSelectedItem(GGUser ggUser)
        {
            if (ggUser == null) { return; }
            if (this.userSelectedViewModel.UserSelectedModels.Exists(x => x.ID == ggUser.ID))
            {
                return;
            }
            UserSelectedModel selectedModel = new UserSelectedModel(ggUser.ID, ggUser.Name, GlobalResourceManager.GetHeadImage(ggUser));
            //selectedModel.Command = { { nameof(Button.Click),(s, e) => UserSelectedListBoxItemTemplate_RemoveUserSelectedItemEvent(selectedModel); } };
                this.userSelectedViewModel.UserSelectedModels.Add(selectedModel);
            this.userSelectedViewModel.Count = this.userSelectedViewModel.UserSelectedModels.Count;
        }

    }
}
