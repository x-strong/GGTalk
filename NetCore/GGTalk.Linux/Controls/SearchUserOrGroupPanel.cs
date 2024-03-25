using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using CPF.Styling;
using GGTalk;
using GGTalk.Linux;
using GGTalk.Linux.Controls;
using GGTalk.Linux.ViewModels;
using GGTalk.Linux.Views;
using System;
using System.Collections.Generic;
using System.Text;
using TalkBase;

namespace GGTalk.Linux.Controls
{
    [CPF.Design.DesignerLoadStyle("res://GGTalk.Linux.Controls/Stylesheet1.css")]//用于设计的时候加载样式
    internal class SearchUserOrGroupPanel : Control
    {       

        private List<RecentContactModel> recentContacts = new List<RecentContactModel>();
        private ElTextBox textBox;
        private TextBlock tb_noResult;
        private TreeView treeView;
        private Window parentWindow;

        public void Initialize(Window _parentWindow)
        {
            this.parentWindow = _parentWindow;
        }


        private bool initialized = false;
        protected override void InitializeComponent()
        {//模板定义
            if (this.initialized) { return; }
            this.initialized = true;
            this.DataContext = null;
            Size = SizeField.Fill;
            BorderFill = "#619fd7";
            BorderThickness = new Thickness(1, 1, 1, 1);
            BorderType = BorderType.BorderThickness;
            Children.Add(new Grid
            {
                PresenterFor = this,
                MarginLeft = 0f,
                Size = SizeField.Fill,
                Background = "#fff",
                ColumnDefinitions =
                {
                    new ColumnDefinition
                    {

                    },
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
                },
                Children =
                {
                    new Panel
                    {
                        Width = "100%",
                        Children =
                        {
                            new Border
                            {
                                MarginLeft = 0,
                                MarginTop = 0,
                                Width = "100%",
                                Height = 29,
                                BorderFill = "#619fd7",
                                BorderThickness = new Thickness(2, 2, 2, 2),
                                BorderType = BorderType.BorderThickness,
                            },
                            new Picture
                            {
                                MarginLeft = 5f,
                                MarginTop = 6f,
                                Width = 16,
                                Height = 16,
                                Stretch = Stretch.Fill,
                                Source = CommonOptions.ResourcesCatalog + "search_icon.png",
                            },
                            new ElTextBox
                            {
                                Name=nameof(this.textBox),
                                PresenterFor=this,
                                MarginLeft = 22f,
                                MarginTop = 1.5f,
                                AcceptsReturn=false,
                                WordWarp=false,
                                Width = 228,
                                Height = 27,
                                BorderThickness = new Thickness(0, 0, 0, 0),
                                BorderType = BorderType.BorderThickness,
                                VScrollBarVisibility=ScrollBarVisibility.Hidden,
                                HScrollBarVisibility=ScrollBarVisibility.Hidden,  
                                Placeholder="请输入账号，按回车查询"
                            },
                            new Button
                            {
                                Classes = "commonButton",
                                MarginRight=5,
                                MarginTop = 2,
                                Width = 24,
                                Height = 24,
                                BorderThickness =new Thickness(0,0,0,0),
                                BorderType=BorderType.BorderThickness,
                                //CornerRadius="6",
                                Content =
                                new Picture
                                {                                    
                                    Width = 24,
                                    Height = 24,
                                    Stretch = Stretch.Fill,
                                    Source = CommonOptions.ResourcesCatalog + "imagea.png",
                                },
                                Commands =
                                {
                                    {
                                        nameof(Button.Click),
                                        (s, e) => { this.Visibility = Visibility.Hidden; }
                                    }
                                }
                            }

                        },
                        Attacheds = { { Grid.ColumnIndex, 0 }, { Grid.RowIndex, 0 } }
                    },
                    new Panel
                    {
                        Size = SizeField.Fill,                        
                        Children =
                        {
                            new TextBlock{
                                Name=nameof(this.tb_noResult),
                                PresenterFor=this,
                                MarginTop=20,    
                                
                            },
                            new TreeView
                            {
                                Name = nameof(this.treeView),
                                PresenterFor = this,
                                Size = SizeField.Fill,                                
                                DisplayMemberPath = nameof(RecentListModel.CatalogName),
                                ItemsMemberPath = nameof(RecentListModel.RecentList),

                                ItemTemplate = new GGTalk.Linux.Controls.CatalogItem
                                {
                                    ItemTemplate = new RecentItem()
                                    {
                                        Commands={{nameof(DoubleClick),(s,e)=>this.TreeView_DoubleTapped(s) } }
                                    },
                                    //Commands ={
                                    //        {
                                    //            nameof(MouseDown),(s,e)=>{
                                    //                CPF.Input.MouseButtonEventArgs args=e as CPF.Input.MouseButtonEventArgs;
                                    //                if(args.MouseButton==CPF.Input.MouseButton.Right)
                                    //                {
                                    //                    this.contextMenu_Catalog.PlacementTarget = (UIElement)s;
                                    //                    this.contextMenu_Catalog.IsOpen = true;
                                    //                }

                                    //            }
                                    //        }
                                    //    }
                                },

                                Bindings =
                                {
                                    {
                                        nameof(TreeView.Items),
                                        nameof(RecentListViewModel.RecentListModels)
                                    }
                                }

                            }
                        },
                        Attacheds = { { Grid.ColumnIndex, 0 }, { Grid.RowIndex, 1 } }
                    }
                }
            });

            if (!this.DesignMode)
            {
                this.treeView = (TreeView)this.FindPresenterByName<TreeView>(nameof(this.treeView));

                this.tb_noResult = this.FindPresenterByName<TextBlock>(nameof(this.tb_noResult));
                this.textBox = this.FindPresenterByName<ElTextBox>(nameof(this.textBox));
                this.textBox.KeyUp += TextBox_KeyUp;
            }
        }

        #region 双击
        private void TreeView_DoubleTapped(CpfObject sender)
        {
            object sourceData = sender.DataContext;
            if (sourceData is RecentContactModel)
            {
                RecentContactModel model = (RecentContactModel)sourceData;
                if (Program.ResourceCenter.CurrentUserID == model.ID)
                {
                    return;
                }

                try
                {
                    // CommonHelper.MoveToChat(model);
                    if (model.IsUser)
                    {
                        GGUser friend = Program.ResourceCenter.ClientGlobalCache.GetUser(model.ID);
                        this.StartUserChat(friend);
                    }
                    else
                    {
                        GGGroup ggGroup = Program.ResourceCenter.ClientGlobalCache.GetGroup(model.ID);
                        this.StartGroupChat(ggGroup);
                    }
                }
                catch (Exception ee)
                {
                    GlobalResourceManager.Logger.Log(ee, "SearchUserOrGroupPanel.TreeView_DoubleTapped", ESBasic.Loggers.ErrorLevel.Standard);
                }
                return;
            }
        }

        private void StartUserChat(GGUser friend)
        {
            CommonBusinessMethod.AddFriend(this.parentWindow, Program.ResourceCenter, friend.ID);
        }

        private async void StartGroupChat(GGGroup group)
        {
            if (group == null)
            {
                return;
            }

            bool isMember = group.MemberList.Contains(Program.ResourceCenter.CurrentUserID);
            if (isMember)
            {
                ///跳转到聊天界面
                CommonHelper.MoveToChat(group);
            }
            else
            {
                AddGroupWindow addGroupForm = new AddGroupWindow(group);
                System.Threading.Tasks.Task<object> task = addGroupForm.ShowDialog_Topmost(this.parentWindow);
                await task.ConfigureAwait(true);
                if (Convert.ToBoolean(task.Result))
                {
                    MessageBoxEx.Show("提示", "发送成功");
                }
            }
        }

        #endregion

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!Program.ResourceCenter.Connected)
            {
                MessageBoxEx.Show("您已掉线！");
            }
            string idOrName = this.textBox.Text.Trim();
            this.tb_noResult.Visibility = Visibility.Hidden;
            this.recentContacts.Clear();
            if (string.IsNullOrEmpty(idOrName))
            {
                return;
            }
            if (e.Key == Keys.Enter)
            {
                List<GGUser> users = new List<GGUser>();
                List<IUser> iusers = Program.ResourceCenter.ClientOutter.SearchUserList(idOrName);
                if (iusers != null && iusers.Count > 0) {
                    foreach (IUser item in iusers)
                    {
                        users.Add((GGUser)item);
                    }                
                }
                List<IGroup> groups = Program.ResourceCenter.ClientOutter.SearchGroupList(idOrName);
                this.SetSearchResult(users, groups, true);
                return;
            }

            List<GGUser> ggUsers = Program.ResourceCenter.ClientGlobalCache.SearchUser_Fuzzy(idOrName);
            List<GGGroup> ggGroups = Program.ResourceCenter.ClientGlobalCache.SearchGroup_Fuzzy(idOrName);
            this.tb_noResult.Text = "按回车获取结果";
            List<IGroup> iGroups = new List<IGroup>();
            if (ggGroups != null)
            {
                iGroups = ESBasic.Collections.CollectionConverter.ConvertListUpper<IGroup, GGGroup>(ggGroups);
            }
            this.SetSearchResult(ggUsers, iGroups);
        }

        private void SetSearchResult(List<GGUser> users, List<IGroup> groups, bool isFromServer = false)
        {
            bool hasNoResult = users.Count == 0 && groups.Count == 0;
            if (isFromServer)
            {
                this.tb_noResult.Text = "没有找到符合搜索条件的结果";
            }
            this.tb_noResult.Visibility =CommonHelper.GetVisibility(hasNoResult) ;
            this.treeView.Visibility = CommonHelper.GetVisibility(!hasNoResult);
            if (!hasNoResult)
            {
                foreach (IUser unit in users)
                {
                    if (unit.ID == Program.ResourceCenter.CurrentUserID)
                    {
                        continue;
                    }
                    UserStatus status = (!unit.IsUser) ? UserStatus.Online : unit.UserStatus;
                    string catalog = Program.ResourceCenter.ClientGlobalCache.CurrentUser.GetFriendCatalog(unit.ID);
                    string describe = unit.DisplayName;
                    if (catalog != null)
                    {
                        describe += string.Format("  [ {0} ]", catalog);
                    }
                    else
                    {
                        describe += "  [ 陌生人，双击添加 ]";
                    }
                    RecentContactModel model = new RecentContactModel((GGUser)unit);
                    model.CommentName = describe;
                    model.Describe = unit.Signature;
                    this.recentContacts.Add(model);
                }
                foreach (IGroup group in groups)
                {
                    string describe = group.Name;
                    if (group.MemberList.Contains(Program.ResourceCenter.CurrentUserID))
                    {
                        describe += "  [ 群 ]";
                    }
                    else
                    {
                        describe += "  [ 双击加入 ]";
                    }
                    RecentContactModel model = new RecentContactModel((GGGroup)group);
                    model.CommentName = describe;
                    model.Describe = group.Announce;
                    this.recentContacts.Add(model);
                }
            }
            this.BindSource();
        }

        private void BindSource()
        {
            RecentListModel recentListModel = new RecentListModel();
            recentListModel.CatalogName = "查询结果";
            recentListModel.RecentList = new Collection<RecentContactModel>();
            recentListModel.RecentList.AddRange(this.recentContacts);
            RecentListViewModel viewModel = new RecentListViewModel()
            {
                RecentListModels = new Collection<RecentListModel>() { recentListModel }
            };
            DataContext = viewModel;
            this.treeView.ExpandAllNode();
        }

        /// <summary>
        /// 设置输入框为焦点
        /// </summary>
        public void SetFocus()
        {
            this.textBox.FocusLastIndex();
        }
    }
}
