using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Shapes;
using CPF.Styling;
using GGTalk;
using GGTalk.Linux;
using GGTalk.Linux.Controls;
using GGTalk.Linux.Models;
using GGTalk.Linux.Controls.Templates;
using GGTalk.Linux.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGTalk.Linux.Controls
{
    [CPF.Design.DesignerLoadStyle("res://GGTalk.Linux.Controls/Stylesheet1.css")]//用于设计的时候加载样式
    internal class UserInGroupBox : Control
    {
        private TreeView treeView;
        private ContextMenu contextMenu_User;
        private Window parentWindow;
        private GGGroup currentGroup;
        private string catalogName = "群成员";
        private object locker = new object();
        private Collection<FriendListModel> friendListModelList = new Collection<FriendListModel>();
        private FriendListModel friendListModel;

        public UserInGroupBox()
        {
            this.friendListModel = new FriendListModel() { CatalogName = this.catalogName, UserList = new Collection<GGUserPlus>() };
        }


        private bool initialized = false;
        protected override void InitializeComponent()
        {//模板定义
            if (this.initialized) { return; }
            this.initialized = true;
            DataContext = null;
            Size = SizeField.Fill;
            Children.Add(new Panel
            {
                Size = SizeField.Fill,
                Children =
                {
                    new TreeView
                    {
                        Name=nameof(this.treeView),
                        PresenterFor=this,
                        Size=SizeField.Fill,
                        DisplayMemberPath=nameof(FriendListModel.CatalogName) ,
                        ItemsMemberPath=nameof(FriendListModel.UserList),

                        ItemTemplate=new CatalogItem
                        {
                            ItemTemplate= new UserInGroupItem{
                                    
                                    Commands ={
                                    {
                                        nameof(MouseDown),(s,e)=>{
                                            CPF.Input.MouseButtonEventArgs args = e as CPF.Input.MouseButtonEventArgs;
                                            if (args.MouseButton == CPF.Input.MouseButton.Right)
                                            {
                                                this.contextMenu_User.PlacementTarget = (UIElement)s;
                                                this.contextMenu_User.IsOpen = true;
                                                args.Handled = true;
                                            }
                                    } },
                                    {nameof(DoubleClick),(s,e)=>this.Friend_DoubleClick((UIElement)s) }
                                }
                            },
                        },
                        Bindings =
                        {
                            {
                                nameof(TreeView.Items),
                                nameof(FriendListViewModel.FriendListModelList)
                            }
                        }
                    }
                }
            });

            if (!this.DesignMode)
            {
                this.treeView = this.FindPresenterByName<TreeView>(nameof(this.treeView));

                this.contextMenu_User = new ContextMenu()
                {
                    //Items = new UIElement[] {
                    //             new MenuItem
                    //             {
                    //                Header = "发送消息",
                    //                Commands = {
                    //                    {
                    //                        nameof(MouseDown),
                    //                        (s, e) => SendMessage_Click(s)
                    //                    }
                    //                }
                    //             },
                    //             new MenuItem
                    //             {
                    //                Header = "消息记录",
                    //                Commands = {
                    //                    {
                    //                        nameof(MouseDown),
                    //                        (s, e) => MessageRecord_Click(s)
                    //                    }
                    //                }
                    //             },
                    //             new MenuItem
                    //             {
                    //                Header = "查看资料",
                    //                Commands = {
                    //                    {
                    //                        nameof(MouseDown),
                    //                        (s, e) => UserInfo_Click(s)
                    //                    }
                    //                }
                    //             },
                    //             new MenuItem
                    //             {
                    //                Header = "修改备注名称",
                    //                Commands = {
                    //                    {
                    //                        nameof(MouseDown),
                    //                        (s, e) => UpdateFirendRemark_Click(s)
                    //                    }
                    //                }
                    //             },
                    //             new MenuItem
                    //             {
                    //                Header = "移动联系人至",
                    //                Commands = {
                    //                    {
                    //                        nameof(MouseDown),
                    //                        (s, e) => MoveToCatalog_Click(s)
                    //                    }
                    //                }
                    //             },
                    //             new MenuItem
                    //             {
                    //                Header = "删除好友",
                    //                Commands = {
                    //                    {
                    //                        nameof(MouseDown),
                    //                        (s, e) => DeleteFirend_Click(s)
                    //                    }
                    //                }
                    //             },
                    //            }

                };


            }
        }

        public void Initialize(GGGroup ggGroup, Window _parentWindow)
        {
            this.parentWindow = _parentWindow;
            this.currentGroup = ggGroup;
            this.AddUser(new GGUserPlus(Program.ResourceCenter.ClientGlobalCache.CurrentUser));
            foreach (string friendID in this.currentGroup.MemberList)
            {
                if (friendID == Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserID)
                {
                    continue;
                }
                GGUser friend = Program.ResourceCenter.ClientGlobalCache.GetUser(friendID);
                if (friend != null)
                {
                    this.AddUser(new GGUserPlus(friend));
                }
            }
            this.BindSource();
        }

        private void BindSource()
        {
            this.friendListModelList.Add(this.friendListModel);
            FriendListViewModel viewModel = new FriendListViewModel()
            {
                FriendListModelList = this.friendListModelList
            };
            DataContext = viewModel;
            this.treeView.ExpandFirstNode();
        }

        //private void AddCatalogName(string _catalogName)
        //{
        //    bool existCatalogName = this.friendListModelList.Exists(x => x.CatalogName == _catalogName);
        //    if (existCatalogName) { return; }
        //    this.friendListModelList.Add(new FriendListModel() { CatalogName = _catalogName, UserList = new Collection<GGUserPlus>() });
        //}

        public void AddUser(GGUserPlus user)
        {
            lock (this.locker)
            {
                Collection<GGUserPlus> list = this.friendListModel.UserList;
                //若集合中未存在该用户则添加
                if (list.Find(x => x.ID == user.ID) == null)
                {
                    list.Add(user);
                }
            }

        }

        public void DeleteUser(string userID)
        {
            lock (this.locker)
            {
                for (int i = 0; i < this.friendListModel.UserList.Count; i++)
                {
                    if (this.friendListModel.UserList[i].ID == userID)
                    {
                        this.friendListModel.UserList.RemoveAt(i);
                        return;
                    }
                }
            }
        }

        public void UpdateUser(GGUser ggUser)
        {
            lock (this.locker)
            {
                for (int i = 0; i < this.friendListModel.UserList.Count; i++)
                {
                    if (this.friendListModel.UserList[i].ID == ggUser.ID)
                    {
                        this.friendListModel.UserList[i] = new GGUserPlus(ggUser);
                        return;
                    }
                }
            }
        }


        private void Friend_DoubleClick(UIElement s)
        {
            GGUserPlus ggUserPlus = ((UIElement)s).DataContext as GGUserPlus;
            CommonHelper.MoveToChat(ggUserPlus);
        }


        #region 右键菜单
        //点击添加好友
        private void AddFriend_Click(UIElement s)
        {
            GGUserPlus ggUserPlus = ((UIElement)s).DataContext as GGUserPlus;
            if (ggUserPlus == null) { return; }
            CommonBusinessMethod.AddFriend(this.parentWindow, Program.ResourceCenter, ggUserPlus.ID);
        }
        #endregion
    }
}
