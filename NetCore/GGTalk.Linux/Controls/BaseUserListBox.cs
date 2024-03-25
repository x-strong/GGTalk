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
using TalkBase;

namespace GGTalk.Linux.Controls
{
    internal class BaseUserListBox : Control
    {
        private TreeView treeView;
        private FriendListViewModel viewModel = new FriendListViewModel() { FriendListModelList = new Collection<FriendListModel>() };
        private object locker = new object();
        /// <summary>
        /// 成员双击事件
        /// </summary>
        public event Action<GGUserPlus> User_DoubleTapped;
        protected override void InitializeComponent()
        {//模板定义
            Children.Add(new Panel
            {
                Background = "#fff",
                Size = SizeField.Fill,
                Children =
                {
                    new TreeView
                    {
                        Size=SizeField.Fill,
                        Name = nameof(this.treeView),
                        PresenterFor = this,
                        DisplayMemberPath = nameof(FriendListModel.CatalogName),
                        ItemsMemberPath = nameof(FriendListModel.UserList),
                        ItemTemplate=new CatalogItem
                        {
                            ItemTemplate=new BaseUserTreeViewItem()
                            {
                                Commands={ { nameof(DoubleClick),(s,e)=> { this.TreeView_DoubleTapped(s); } } }
                            }
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
            }
        }

        private void TreeView_DoubleTapped(CpfObject s)
        {
            if (this.User_DoubleTapped != null)
            {
                GGUserPlus ggUserPlus = s.DataContext as GGUserPlus;
                if (ggUserPlus != null)
                {
                    this.User_DoubleTapped(ggUserPlus);
                }
            }
        }


        public void Initialize(string _catalogName, List<GGUser> ggUsers)
        {
            this.viewModel.FriendListModelList.Clear();
            this.AddCatalogName(_catalogName);
            foreach (GGUser user in ggUsers)
            {
                this.AddUser(new GGUserPlus(user), _catalogName);
            }
            this.BindSource();
        }

        public void Initialize4Friend()
        {
            this.AddCatalogName(FunctionOptions.DefaultFriendCatalog);
            List<string> catalogList = new List<string>(Program.ResourceCenter.ClientGlobalCache.CurrentUser.GetFriendCatalogList());
            catalogList.Remove(FunctionOptions.BlackListCatalogName);
            foreach (string catalog in catalogList)
            {
                this.AddCatalogName(catalog);
            }
            //if (FunctionOptions.BlackList)
            //{
            //    this.AddCatalogName(FunctionOptions.BlackListCatalogName);
            //}
            this.AddUser(new GGUserPlus(Program.ResourceCenter.ClientGlobalCache.CurrentUser));

            foreach (string friendID in Program.ResourceCenter.ClientGlobalCache.CurrentUser.GetAllFriendList())
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


        private void AddCatalogName(string catalogName)
        {
            if (this.viewModel.FriendListModelList.Exists(x=>x.CatalogName==catalogName))
            {
                return;
            }
            lock (this.locker)
            {
                this.viewModel.FriendListModelList.Add(new FriendListModel() { CatalogName = catalogName, UserList = new Collection<GGUserPlus>() });
            }
        }

        private FriendListModel GetFriendListModel(string catalogName)
        {
            return this.viewModel.FriendListModelList.Find(x => x.CatalogName == catalogName);
        }

        public void AddUser(GGUserPlus user, string catalogName = null)
        {
            lock (this.locker)
            {
                if (catalogName == null)
                {
                    catalogName = Program.ResourceCenter.ClientGlobalCache.CurrentUser.GetFriendCatalog(user.ID) ?? FunctionOptions.DefaultFriendCatalog;
                }
                FriendListModel model = this.GetFriendListModel(catalogName);
                if (model == null) { return; }
                //若集合中未存在该用户则添加
                if (model.UserList.Find(x => x.ID == user.ID) == null)
                {
                    model.UserList.Add(user);
                }
            }
        }

        public void DeleteUser(string userID)
        {
            lock (this.locker)
            {
                Collection<FriendListModel> friendListModels = this.viewModel.FriendListModelList;
                foreach (FriendListModel model in friendListModels)
                {
                    foreach (GGUserPlus ggUserPlus in model.UserList)
                    {
                        if (ggUserPlus.ID == userID)
                        {
                            model.UserList.Remove(ggUserPlus);
                            return;
                        }
                    }
                }
            }
        }

        public void UpdateUser(GGUser ggUser)
        {
            lock (this.locker)
            {
                foreach (FriendListModel model in this.viewModel.FriendListModelList)
                {
                    for (int i = 0; i < model.UserList.Count; i++)
                    {
                        if (model.UserList[i].ID == ggUser.ID)
                        {
                            model.UserList[i] = new GGUserPlus(ggUser);
                            return;
                        }
                    }
                }
            }
        }

        private void BindSource()
        {
            DataContext = this.viewModel;
            this.treeView.ExpandFirstNode();
        }

    }
}
