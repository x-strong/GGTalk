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
using GGTalk.Linux.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using TalkBase;

namespace GGTalk.Linux.Controls
{
    internal class GroupListBox : Control
    {
        private ObjectManager<string, GroupListModel> groupManager = new ObjectManager<string, GroupListModel>();
        private object locker = new object();
        public GroupListBox()
        {
            this.InitializeComponent();
        }

        public new void Initialize()
        {
            foreach (GGGroup group in Program.ResourceCenter.ClientGlobalCache.GetAllGroups())
            {
                this.AddGroup(group);
            }
            this.BindSource();
        }

        private void BindSource()
        {
            Collection<GroupListModel> list = new Collection<GroupListModel>();
            list.AddRange(this.groupManager.GetAll());
            GroupListViewModel viewModel = new GroupListViewModel()
            {
                GroupListModelList = list
            };
            treeView.Items = list;
            //DataContext = viewModel;
            this.treeView.ExpandFirstNode();
        }

        public void AddGroup(IUnit unit)
        {
            GGGroup ggGroup = unit as GGGroup;
            if (ggGroup == null) { return; }
            lock (this.locker)
            {
                string catalogName = this.GetCatalogName(ggGroup);
                if (!this.groupManager.Contains(catalogName)) { this.AssureCatalog(catalogName); }
                this.groupManager.Get(catalogName).GroupList.Add(ggGroup);
            }
        }

        private void AssureCatalog(string catalogName)
        {
            if (this.groupManager.Contains(catalogName))
            {
                return;
            }
            this.groupManager.Add(catalogName, new GroupListModel() { CatalogName = catalogName, GroupList = new Collection<GGGroup>() });
        }

        public void RemoveGroup(string groupID)
        {
            lock (this.locker)
            {
                foreach (GroupListModel model in this.groupManager.GetAll())
                {
                    for (int i = 0; i < model.GroupList.Count; i++)
                    {
                        if (model.GroupList[i].ID == groupID)
                        {
                            model.GroupList.RemoveAt(i);
                            return;
                        }
                    }
                }
            }
        }


        public void OnGroupInfoChanged(IUnit unit)
        {
            lock (this.locker)
            {
                GGGroup ggGroup = unit as GGGroup;
                GGGroup temp = this.FindGGGroup4Cache(unit.ID);
                if (ggGroup == null || temp == null) { return; }
                temp = ggGroup;
                //this.BindSource();
                //ESBasic.Helpers.ReflectionHelper.CopyProperty(ggGroup, temp);
            }
        }

        private string GetCatalogName(GGGroup group)
        {
            if (group.GroupType == GroupType.CommonGroup)
            {
                return "我的群";
            }
            if (group.GroupType == GroupType.DiscussGroup)
            {
                return "讨论组";
            }
            return "";
        }

        public void GroupCommentNameChanged(IUnit unit)
        {
            lock (this.locker)
            {
                GGGroup group = this.FindGGGroup4Cache(unit.ID);
                if (group == null) { return; }
                group.CommentName = unit.CommentName;
            }
        }

        private GGGroup FindGGGroup4Cache(string groupID)
        {
            foreach (GroupListModel model in this.groupManager.GetAll())
            {
                foreach (GGGroup group in model.GroupList)
                {
                    if (group.ID == groupID)
                    {
                        return group;
                    }
                }
            }
            return null;
        }


        TreeView treeView;
        private volatile bool initialized = false;
        protected override void InitializeComponent()
        {//模板定义
            if (this.initialized) { return; }
            this.initialized = true;
            DataContext = null;
            Size = SizeField.Fill;
            Children.Add(new Panel
            {
                Size=SizeField.Fill,
                Children =
                {
                    new TreeView
                    {
                        Name=nameof(this.treeView),
                        PresenterFor=this,
                        Size=SizeField.Fill,
                        DisplayMemberPath=nameof(GroupListModel.CatalogName) ,
                        ItemsMemberPath=nameof(GroupListModel.GroupList),
                        ItemTemplate=new CatalogItem
                        {
                            ItemTemplate=new GroupItem(){
                            Commands={ { nameof(DoubleClick), (s, e) => this.Group_DoubleClick((UIElement)s) } }
                            }
                        },
                        Bindings =
                        {
                            {
                                nameof(TreeView.Items),
                                nameof(GroupListViewModel.GroupListModelList)
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

        private void Group_DoubleClick(UIElement s)
        {
            GGGroup ggGroup = ((UIElement)s).DataContext as GGGroup;
            CommonHelper.MoveToChat(ggGroup);
        }
    }
}
