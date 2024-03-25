using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Shapes;
using CPF.Styling;
using ESBasic.ObjectManagement.Managers;
using GGTalk;
using GGTalk.Linux;
using GGTalk.Linux.Controls;
using GGTalk.Linux.Models;
using GGTalk.Linux.Views;
using GGTalk.Linux.ViewModels;
using GGTalk.Linux.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TalkBase;

namespace GGTalk.Linux.Controls
{

    internal class FriendListBox : Control
    {
        TreeView treeView;
        private object locker = new object();
        private Comparison<FriendListModel> comparison4ListItem;
        private Comparison<GGUserPlus> comparison4User;

        public FriendListBox()
        {
            this.InitializeComponent();
        }

        public void Initialize()
        {
            this.comparison4ListItem = new Comparison<FriendListModel>(this.CompareChatListItem);
            this.comparison4User = new Comparison<GGUserPlus>(this.Comparison4UserItem);
            List<string> catalogList = Program.ResourceCenter.ClientGlobalCache.CurrentUser.GetFriendCatalogList();
            this.AddCatalog(FunctionOptions.DefaultFriendCatalog, false);
            foreach (string catalog in catalogList)
            {
                this.AddCatalog(catalog, false);
            }
            if (FunctionOptions.BlackList)
            {
                this.AddCatalog(FunctionOptions.BlackListCatalogName, false);
            }

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
            models.Sort(this.comparison4ListItem);
            this.BindSource();
            
        }

        Collection<FriendListModel> models=new Collection<FriendListModel>();
        FriendListViewModel viewModel;
        private void BindSource()
        {
            models.Sort(this.comparison4ListItem);
            viewModel = new FriendListViewModel()
            {
                FriendListModelList = models
            };
            DataContext = viewModel;
            //models = viewModel.FriendListModelList;
            this.treeView.ExpandAllNode();
        }

        private FriendListModel GetFriendListModel(string catalogName)
        {
            return this.models.Find(x => x.CatalogName == catalogName);
        }

        private bool ExistsFriendListModel(string catalogName)
        {
            return this.models.Exists(x => x.CatalogName == catalogName);
        }

        private List<string> GetModelKeyList()
        {
            List<string> list = new List<string>();
            foreach (FriendListModel item in this.models)
            {
                list.Add(item.CatalogName);
            }
            return list;
        }


        #region 好友相关
        public void AddUser(GGUserPlus user,bool needRefreshSource = false)
        {
            lock (this.locker)
            {
                Collection<GGUserPlus> userList = this.GetCatalogUserList4User(user);
                if (userList == null) { return; }
                foreach (GGUserPlus item in userList)
                {
                    if (item.ID == user.ID)
                    {
                        return;
                    }
                }
                userList.Add(user);
                userList.Sort(this.comparison4User);
                if (needRefreshSource) { this.BindSource(); }
            }
        }

        public void AddUser(string userID)
        {
            GGUser ggUser = Program.ResourceCenter.ClientGlobalCache.GetUser(userID);
            this.AddUser(ggUser);
        }

        private void AddUser(GGUser ggUser)
        {
            lock (this.locker)
            {
                if (ggUser == null) { return; }
                this.AddUser(new GGUserPlus(ggUser),true);
            }
        }

        public void DeleteUser(string userID)
        {
            lock (this.locker)
            {
                GlobalResourceManager.Logger.LogWithTime(string.Format("进入删除用户{0}", userID));
                Collection<FriendListModel> friendListModels = models;
                foreach (FriendListModel model in friendListModels)
                {
                    foreach (GGUserPlus ggUserPlus in model.UserList)
                    {
                        if (ggUserPlus.ID == userID)
                        {
                            model.UserList.Remove(ggUserPlus);
                            GlobalResourceManager.Logger.LogWithTime(string.Format("删除用户{0},集合剩余数量:{1}", userID, model.UserList.Count));
                            this.BindSource();
                            return;
                        }
                    }
                }
            }
        }

        public void UserStatusChanged(IUnit unit)
        {
            this.UnitInfoChanged(unit);
        }

        public void UnitInfoChanged(IUnit unit)
        {
            lock (this.locker)
            {
                Collection<GGUserPlus> userInList = null;
                GGUser ggUser = (GGUser)unit;
                foreach (FriendListModel model in models)
                {
                    for (int i = 0; i < model.UserList.Count; i++)
                    {
                        if (model.UserList[i].ID == unit.ID)
                        {
                            //ESBasic.Helpers.ReflectionHelper.CopyProperty(new GGUserPlus(ggUser), model.UserList[i]);

                            model.UserList[i] = new GGUserPlus(ggUser);
                            userInList = model.UserList;
                            break;
                        }
                    }
                    if (userInList != null) { break; }
                }                
                this.BindSource();
            }
        }

        public void SetAllUnitOffline()
        {
            try
            {
                lock (this.locker)
                {
                    foreach (FriendListModel model in models)
                    {
                        for (int i = 0; i < model.UserList.Count; i++)
                        {
                            model.UserList[i].UserStatus = UserStatus.OffLine;
                        }
                        model.UserList.Sort(this.comparison4User);
                    }
                }
            }
            catch (Exception ex) { }
        }


        public void UnitCommentNameChanged(IUnit unit)
        {
            this.UnitInfoChanged(unit);
        }

        private Collection<GGUserPlus> GetCatalogUserList4User(IUser user)
        {
            string catalogName = Program.ResourceCenter.ClientGlobalCache.CurrentUser.GetFriendCatalog(user.ID) ?? FunctionOptions.DefaultFriendCatalog;
            if (string.IsNullOrEmpty(catalogName) || !this.ExistsFriendListModel(catalogName)) { return null; }
            return this.GetFriendListModel(catalogName).UserList;
        }
        #endregion

        #region 分组相关
        /// <summary>
        /// 添加分组
        /// </summary>
        /// <param name="catelogName">添加分组</param>
        /// <param name="needNotice"></param>
        public void AddCatalog(string catelogName, bool needNotice)
        {
            if (Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            foreach (string item in this.GetModelKeyList())
            {
                if (item == catelogName)
                {
                    if (needNotice)
                    {
                        MessageBoxEx.Show("已经存在同名的分组！");
                    }                    
                    return;
                }
            }
            this.models.Add(new FriendListModel() { CatalogName = catelogName, UserList = new Collection<GGUserPlus>() });
            models.Sort(this.comparison4ListItem);
            if (needNotice)
            {
                Program.ResourceCenter.ClientOutter.AddFriendCatalog(catelogName);
            }
        }


      
        #endregion

        private ContextMenu contextMenu_Catalog;
        private ContextMenu contextMenu_User;


        private bool initialized = false;
        protected override void InitializeComponent()
        {//模板定义
            if (this.initialized) { return; }
            this.initialized = true;
            DataContext = null;
            Size = SizeField.Fill;
            Children.Add(
                    new TreeView
                    { 
                        Name = nameof(this.treeView),
                        PresenterFor = this,
                        Size = SizeField.Fill,
                        DisplayMemberPath = nameof(FriendListModel.CatalogName),
                        ItemsMemberPath = nameof(FriendListModel.UserList),

                        ItemTemplate = new GGTalk.Linux.Controls.CatalogItem
                        {
                            ItemTemplate = new UserItem()
                            {
                                Commands ={
                                {
                                    nameof(MouseDown),(s,e)=>{
                                        CPF.Input.MouseButtonEventArgs args = e as CPF.Input.MouseButtonEventArgs;
                                        if (args.MouseButton == CPF.Input.MouseButton.Right)
                                        {
                                            args.Handled = true;
                                            GGUserPlus ggUser = s.DataContext as GGUserPlus;
                                            if(ggUser==null){ return; }
                                            if(ggUser.ID==Program.ResourceCenter.CurrentUserID){ return; }
                                            this.contextMenu_User.PlacementTarget = (UIElement)s;
                                            this.contextMenu_User.IsOpen = true;
                                            
                                        }
                                } },
                                {nameof(DoubleClick),(s,e)=>this.Friend_DoubleClick((UIElement)s) }
                                }
                            },
                            Commands ={
                                    {
                                        nameof(MouseDown),(s,e)=>{
                                            CPF.Input.MouseButtonEventArgs args=e as CPF.Input.MouseButtonEventArgs;
                                            if(args.MouseButton==CPF.Input.MouseButton.Right)
                                            {
                                                this.contextMenu_Catalog.PlacementTarget = (UIElement)s;
                                                this.contextMenu_Catalog.IsOpen = true;
                                            }

                                        }
                                    }
                                }
                        },


                        Bindings =
                        {
                            {
                                nameof(TreeView.Items),
                                nameof(FriendListViewModel.FriendListModelList)
                            }
                        }

                    });

            if (!this.DesignMode)
            {
                this.treeView = this.FindPresenterByName<TreeView>(nameof(this.treeView));
                this.contextMenu_Catalog = new ContextMenu
                {
                    Items = new UIElement[]
                                {
                                    new MenuItem
                                    {
                                        Header = "修改分组",
                                        Commands = {
                                            {
                                                nameof(MouseDown),
                                                (s, e) => UpdateCatalogName_Click(s)
                                            }
                                        }
                                    },
                                    new MenuItem
                                    {
                                        Header = "添加分组",
                                        Commands = {
                                            {
                                                nameof(MouseDown),
                                                (s, e) => AddCatalogName_Click()
                                            }
                                        }
                                    },
                                    new MenuItem
                                    {
                                        Header = "删除分组",
                                        Commands = {
                                            {
                                                nameof(MouseDown),
                                                (s, e) => DeleteCatalogName_Click(s)
                                            }
                                        }
                                    },
                                }
                };
                this.contextMenu_User = new ContextMenu()
                {
                    Items = new UIElement[] {
                                 new MenuItem
                                 {
                                    Header = "发送消息",
                                    Commands = {
                                        {
                                            nameof(MouseDown),
                                            (s, e) => SendMessage_Click(s)
                                        }
                                    }
                                 },
                                 new MenuItem
                                 {
                                    Header = "消息记录",
                                    Commands = {
                                        {
                                            nameof(MouseDown),
                                            (s, e) => MessageRecord_Click(s)
                                        }
                                    }
                                 },
                                 new MenuItem
                                 {
                                    Header = "查看资料",
                                    Commands = {
                                        {
                                            nameof(MouseDown),
                                            (s, e) => UserInfo_Click(s)
                                        }
                                    }
                                 },
                                 new MenuItem
                                 {
                                    Header = "修改备注名称",
                                    Commands = {
                                        {
                                            nameof(MouseDown),
                                            (s, e) => UpdateFirendRemark_Click(s)
                                        }
                                    }
                                 },
                                 new MenuItem
                                 {
                                    Header = "移动联系人至",
                                    Commands = {
                                        {
                                            nameof(MouseDown),
                                            (s, e) => MoveToCatalog_Click(s)
                                        }
                                    }
                                 },
                                 new MenuItem
                                 {
                                    Header = "删除好友",
                                    Commands = {
                                        {
                                            nameof(MouseDown),
                                            (s, e) => DeleteFirend_Click(s)
                                        }
                                    }
                                 },
                                }

                };


            }
        }

        private void Friend_DoubleClick(UIElement s)
        {
            GGUserPlus ggUserPlus = ((UIElement)s).DataContext as GGUserPlus;
            CommonHelper.MoveToChat(ggUserPlus);
        }

        #region CompareChatListItem
        private int CompareChatListItem(FriendListModel a, FriendListModel b)
        {
            if (a == b)
            {
                return 0;
            }

            if (a.CatalogName == FunctionOptions.DefaultFriendCatalog)
            {
                return -1;
            }

            if (b.CatalogName == FunctionOptions.DefaultFriendCatalog)
            {
                return 1;
            }

            if (a.CatalogName == FunctionOptions.BlackListCatalogName)
            {
                return 1;
            }

            if (b.CatalogName == FunctionOptions.BlackListCatalogName)
            {
                return -1;
            }

            return a.CatalogName.CompareTo(b.CatalogName);
        }
        #endregion        

        #region CompareSubItem
        public int Comparison4UserItem(GGUserPlus x, GGUserPlus y)
        {
            if (x == y)
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            if (x.ID == Program.ResourceCenter.CurrentUserID)
            {
                return -1;
            }

            if (y.ID == Program.ResourceCenter.CurrentUserID)
            {
                return 1;
            }

            if (x.UserStatus == UserStatus.OffLine && y.UserStatus == UserStatus.OffLine)
            {
                return x.DisplayName.CompareTo(y.DisplayName);
            }

            if (x.UserStatus != UserStatus.OffLine && y.UserStatus != UserStatus.OffLine)
            {
                return x.DisplayName.CompareTo(y.DisplayName);
            }

            if (x.UserStatus != UserStatus.OffLine)
            {
                return -1;
            }

            return 1;
        }
        #endregion

        #region 分组右键菜单

        //删除分组
        private async void DeleteCatalogName_Click(CpfObject s)
        {
            if (Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }
            FriendListModel friendListModel = this.treeView.SelectedValue as FriendListModel;
            if (friendListModel == null) { return; }
            string name = friendListModel.CatalogName;
            if (FunctionOptions.DefaultFriendCatalog == name)
            {
                MessageBoxEx.Show(string.Format("分组 [{0}] 是默认分组，不能删除！", name));
                return;
            }

            if (name == FunctionOptions.BlackListCatalogName)
            {
                MessageBoxEx.Show(string.Format("分组 [{0}] 是默认分组，不能删除！", FunctionOptions.BlackListCatalogName));
                return;
            }

            if (friendListModel.UserList.Count > 0)
            {
                MessageBoxEx.Show(string.Format("分组 [{0}] 不为空，不能删除！", name));
                return;
            }

            Task<ButtonResult> task = MessageBoxEx.ShowQuery(string.Format("您确定要删除分组 {0} 吗？", friendListModel.CatalogName));
            await task.ConfigureAwait(true);
            if (task.Result != ButtonResult.Yes)
            {
                return;
            }
            this.models.Remove(friendListModel);
            Program.ResourceCenter.ClientOutter.RemoveFriendCatalog(friendListModel.CatalogName);
        }

        //添加分组
        private async void AddCatalogName_Click()
        {
            if (Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }
            EditCatelogNameWindow form = new EditCatelogNameWindow();
            System.Threading.Tasks.Task<object> task = form.ShowDialog_Topmost((Window)this.Root);
            await task.ConfigureAwait(true);
            if (Convert.ToBoolean(task.Result))
            {
                this.AddCatalog(form.NewName,true);
            }
        }

        //修改分组名
        private async void UpdateCatalogName_Click(CpfObject s)
        {
            if (Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }
            FriendListModel friendListModel = this.treeView.SelectedValue as FriendListModel;
            if (friendListModel == null) { return; }
            string oldName = friendListModel.CatalogName;
            if (oldName == FunctionOptions.BlackListCatalogName || oldName == FunctionOptions.DefaultFriendCatalog)
            {
                MessageBoxEx.Show("当前分组名称不能被修改。");
                return;
            }

            EditCatelogNameWindow form = new EditCatelogNameWindow(oldName);
            System.Threading.Tasks.Task<object> task = form.ShowDialog_Topmost((Window)this.Root);
            await task.ConfigureAwait(true);
            if (Convert.ToBoolean(task.Result))
            {
                this.ChangeCatelogName(oldName, form.NewName);
            }
        }

        //#region ChangeCatelogName/AddCatalog
        public void ChangeCatelogName(string oldName, string newName)
        {
            if (Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            if (oldName == FunctionOptions.BlackListCatalogName)
            {
                MessageBoxEx.Show("当前分组名称不能被修改。");
                return;
            }

            FriendListModel oldFriendListModel = this.GetFriendListModel(oldName);
            
            if (this.ExistsFriendListModel(newName))
            {
                this.models.Remove(oldFriendListModel);
                Collection<GGUserPlus> userList = this.GetFriendListModel(newName).UserList;
                userList.AddRange(oldFriendListModel.UserList);
                userList.Sort(this.comparison4User);
            }
            else
            {
                oldFriendListModel.CatalogName = newName;
            }
            this.models.Sort(this.comparison4ListItem);
            Program.ResourceCenter.ClientOutter.ChangeFriendCatalogName(oldName, newName, false);
        }

        public void Catalog4Moved(string friendID, string oldCatalogName, string newCatalogName)
        {
            lock (this.locker)
            {
                FriendListModel friendListModel_old = this.GetFriendListModel(oldCatalogName);
                GGUserPlus frined = friendListModel_old.UserList.Find(o => o.ID == friendID);
                FriendListModel friendListModel_new = this.GetFriendListModel(newCatalogName);

                if (friendListModel_old != null && friendListModel_new != null && frined != null)
                {

                    friendListModel_old.UserList.Remove(frined);
                    friendListModel_new.UserList.Add(frined);
                    friendListModel_new.UserList.Sort(this.comparison4User);

                    this.BindSource();
                }
            }
        }

        //#endregion

        #endregion

        #region 好友右键菜单
        //删除好友
        private async void DeleteFirend_Click(CpfObject s)
        {
            GGUserPlus ggUser = this.treeView.SelectedValue as GGUserPlus;
            if (ggUser == null) { return; }
            try
            {
                if (Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
                {
                    return;
                }
                if (ggUser.ID == Program.ResourceCenter.RapidPassiveEngine.CurrentUserID)
                {
                    return;
                }
                Task<ButtonResult> task = MessageBoxEx.ShowQuery(string.Format("您确定要删除好友 {0}({1}) 吗？", ggUser.Name, ggUser.ID));
                await task.ConfigureAwait(true);
                GlobalResourceManager.Logger.LogWithTime(task.Result.ToString());
                if (task.Result != ButtonResult.Yes)
                {
                    return;
                }
                
                //SendCertainly 发送请求，并等待Ack回复
                Program.ResourceCenter.ClientOutter.RemoveFriend(ggUser.ID);
                GlobalResourceManager.Logger.LogWithTime("发送删除好友消息执行完成");
                this.DeleteUser(ggUser.ID);
                MainWindow.ChatFormController.CloseForm(ggUser.ID);
            }
            catch (Exception ee)
            {
                MessageBoxEx.Show("请求超时！" + ee.Message);
                GlobalResourceManager.Logger.LogWithTime("删除失败"+ee.Message);
            }
        }

        //移动到 分组
        private async void MoveToCatalog_Click(CpfObject s)
        {
            GGUserPlus ggUser = this.treeView.SelectedValue as GGUserPlus;

            if (ggUser == null || ggUser.ID == Program.ResourceCenter.CurrentUserID) { return; }
            string catelogName = Program.ResourceCenter.ClientGlobalCache.CurrentUser.GetFriendCatalog(ggUser.ID);
        
            FriendCatelogSelectorWindow friendCatelogSelectorWindow = new FriendCatelogSelectorWindow(catelogName, Program.ResourceCenter.ClientGlobalCache.CurrentUser.GetFriendCatalogList());

            System.Threading.Tasks.Task<object> task = friendCatelogSelectorWindow.ShowDialog_Topmost((Window)this.Root);
            await task.ConfigureAwait(true);
            if (Convert.ToBoolean(task.Result))
            {
                Program.ResourceCenter.ClientOutter.MoveFriendCatalog(ggUser.ID, catelogName, friendCatelogSelectorWindow.NewName);
                this.Catalog4Moved(ggUser.ID, catelogName, friendCatelogSelectorWindow.NewName);
                if (friendCatelogSelectorWindow.NewName == FunctionOptions.BlackListCatalogName)
                {

                }
            }

        }

        //修改好友备注
        private async void UpdateFirendRemark_Click(CpfObject s)
        {
            GGUserPlus ggUser = this.treeView.SelectedValue as GGUserPlus;
            if (ggUser == null) { return; }
            if (ggUser.ID == Program.ResourceCenter.CurrentUserID) { return; }
            EditCommentNameWindow form = new EditCommentNameWindow(ggUser.CommentName);
            System.Threading.Tasks.Task<object> task = form.ShowDialog_Topmost((Window)this.Root);
            await task.ConfigureAwait(true);

            if (Convert.ToBoolean(task.Result))
            {
                Program.ResourceCenter.ClientOutter.ChangeUnitCommentName(ggUser.ID, form.NewName);
                ggUser.CommentName = form.NewName;
            }
        }

        //查看用户信息
        private void UserInfo_Click(CpfObject s)
        {
            GGUserPlus ggUser = this.treeView.SelectedValue as GGUserPlus;
            if (ggUser == null) { return; }
            UserInfoWindow userInfoWindow = new UserInfoWindow(ggUser);
            userInfoWindow.Show_Topmost();
        }

        //查看消息记录
        private void MessageRecord_Click(CpfObject s)
        {
            GGUserPlus ggUser = this.treeView.SelectedValue as GGUserPlus;
            if (ggUser == null) { return; }
            Window form = MainWindow.ChatFormController.GetChatRecordForm(ggUser.ID);
            if (form != null)
            {
                form.Show_Topmost();
            }
        }

        //发送消息
        private void SendMessage_Click(CpfObject s)
        {
            GGUserPlus ggUser = this.treeView.SelectedValue as GGUserPlus;
            if (ggUser == null) { return; }
            CommonHelper.MoveToChat(ggUser);
        }
        #endregion
    }
}
