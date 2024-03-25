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
using GGTalk.Linux.Views;
using GGTalk.Linux.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using TalkBase;
using TalkBase.NetCore.Client;

namespace GGTalk.Linux.Controls
{

    internal class RecentListBox : Control
    {
        TreeView treeView;
        private ObjectManager<string, RecentContactModel> objectManager = new ObjectManager<string, RecentContactModel>();
        private RecentListViewModel viewModel;
        private RecentListModel recentListModel;
        private Collection<RecentContactModel> recentContacts = new Collection<RecentContactModel>();
        public event Action<bool> HasNewMsgEvent;
        private object locker = new object();
        private RecentContactModel notifyModel;

        public RecentListBox()
        {
            InitializeComponent();
            this.notifyModel = new RecentContactModel()
            {
                ID = "#notifyID",
                Name = "验证消息",
                IsNotifyMsg = true,                
                LastWordsRecord = new LastWordsRecord()
            };
        }

        public void Initialize()
        {
            //加载最近联系人
            DefaultLastWordsComputer<GGUser, GGGroup> computer = new DefaultLastWordsComputer<GGUser, GGGroup>(Program.ResourceCenter);
            int insertIndex = 0;
            foreach (string unitID in Program.ResourceCenter.ClientGlobalCache.GetRecentList())
            {
                IUnit unit = Program.ResourceCenter.ClientGlobalCache.GetUnit(unitID);
                if (unit == null)
                {
                    continue;
                }

                string lastWords = computer.GetLastWords(unit);
                RecentContactModel model = this.ConvertToRecentContactModel(unit);
                this.AddRecentModel(model, insertIndex);
                ++insertIndex;
            }

            recentListModel = new RecentListModel();
            recentListModel.CatalogName = "最近联系人";
            recentListModel.RecentList = new Collection<RecentContactModel>();
            recentListModel.RecentList =this.recentContacts;
            this.viewModel = new RecentListViewModel()
            {
                RecentListModels = new Collection<RecentListModel>() {this.recentListModel }
            };
            //this.DataContext = this.viewModel;
            //this.treeView.ExpandFirstNode();
            this.BindSource();
        }

        private void BindSource()
        {
            //this.recentListModel.RecentList.Clear();
            //this.recentListModel.RecentList.AddRange(this.recentContacts);
            //this.viewModel.RecentListModels.Clear();
            //this.viewModel.RecentListModels.Add(this.recentListModel);
            if (this.viewModel == null) { return; }
            this.treeView.Items = this.viewModel.RecentListModels;
            this.treeView.ExpandFirstNode();
        }

        private RecentContactModel ConvertToRecentContactModel(IUnit unit)
        {
            if (unit == null) { return null; }
            if (unit.IsUser)
            {
                return new RecentContactModel((GGUser)unit);
            }
            else if (unit.UnitType == TalkBase.UnitType.Group)
            {
                return new RecentContactModel((GGGroup)unit);
            }
            return null;
        }

        private void AddRecentModel(RecentContactModel model, int insertIndex)
        {
            lock (this.locker)
            {
                if (this.objectManager.Contains(model.ID))
                {
                    RecentContactModel temp_exist =  this.recentContacts.Find(x => x.ID == model.ID);
                    if (temp_exist != null) { this.recentContacts.Remove(temp_exist); }                    
                    this.objectManager.Remove(model.ID);
                }
                this.objectManager.Add(model.ID, model);
                this.recentContacts.Insert(insertIndex, model);
                this.CheckHasNewMessage();
                this.BindSource();
            }
        }

        public List<string> GetRecentUserList(int maxCount)
        {
            List<string> recentList = new List<string>();
            for (int i = 0; i < recentContacts.Count; i++)
            {
                if (i >= maxCount)
                {
                    break;
                }
                recentList.Add(recentContacts[i].ID);
            }
            return recentList;
        }

        private bool hasNewMsg = false;

        private void CheckHasNewMessage()
        {
            bool hasNewMessage = this.HasNewMessage();
            if (this.hasNewMsg != hasNewMessage && HasNewMsgEvent != null)
            {
                HasNewMsgEvent(hasNewMessage);
            }
            this.hasNewMsg = hasNewMessage;
        }

        private bool HasNewMessage()
        {
            foreach (RecentContactModel item in this.recentContacts)
            {
                if (item.IsNewMsg)
                {
                    return true;
                }
            }
            return false;
        }

        private bool initialized = false;
        protected override void InitializeComponent()
        {//模板定义
            if (this.initialized) { return; }
            this.initialized = true;
            DataContext = null;
            Size = SizeField.Fill;
            var contextMenu = new ContextMenu()
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
                    Header = "查看消息记录",
                    Commands = {
                        {
                            nameof(MouseDown),
                            (s, e) => MessageRecord_Click(s)
                        }
                    }
                    },
                    new MenuItem
                    {
                    Header = "从列表中移除",
                    Commands = {
                        {
                            nameof(MouseDown),
                            (s, e) => DeleteItemFormList_Click(s)
                        }
                    }
                    }
                }
            };
            var contextMenu2 = new ContextMenu()
            {
                Items = new UIElement[] {
                     new MenuItem
                    {
                    Header = "从列表中移除",
                    Commands = {
                        {
                            nameof(MouseDown),
                            (s, e) => DeleteItemFormList_Click(s)
                        }
                    }
                    }
                }
            };

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
                        DisplayMemberPath=nameof(RecentListModel.CatalogName), //"CatalogName",
                        ItemsMemberPath=  nameof(RecentListModel.RecentList),//"RecentList",
                        ItemTemplate=new CatalogItem
                        {
                            ItemTemplate=new RecentItem{

                                Commands={ 
                                    { nameof(DoubleClick),(s,e)=> { this.TreeView_DoubleTapped(s); } } ,
                                    { nameof(MouseDown),(s,e)=>{
                                            CPF.Input.MouseButtonEventArgs args=e as CPF.Input.MouseButtonEventArgs;
                                            if(args.MouseButton==CPF.Input.MouseButton.Right)
                                            {
                                                RecentContactModel model = s.DataContext as RecentContactModel;
                                                if(model==null){ return; }
                                                if(model.IsNotifyMsg)//通知消息右键只有移除功能
                                                {
                                                    contextMenu2.PlacementTarget = (UIElement)s;
                                                    contextMenu2.IsOpen = true;
                                                }else{
                                                    contextMenu.PlacementTarget = (UIElement)s;
                                                    contextMenu.IsOpen = true;
                                                }
                                            }
                                        }                                    
                                    } }
                            }
                        },
                        //Bindings =
                        //{
                        //    {
                        //        nameof(TreeView.Items),
                        //        nameof(RecentListViewModel.RecentListModels)
                        //    }
                        //}
                    }
                }
            });
            if (!this.DesignMode)
            {
                this.treeView = this.FindPresenterByName<TreeView>(nameof(this.treeView));
            }
        }
        private void TreeView_DoubleTapped(CpfObject sender)
        {
            object sourceData = sender.DataContext;
            if (sourceData is RecentContactModel)
            {
                RecentContactModel model = (RecentContactModel)sourceData;

                if (model.IsNotifyMsg)
                {
                    NotifyType type = model.IsUser ? NotifyType.User : NotifyType.Group;
                    CommonHelper.MoveToNotifyWindow(type);
                    ///TO DO 跳转到通知消息界面
                }
                else
                {
                    ///跳转到聊天界面
                    CommonHelper.MoveToChat(model);
                }
            }
            this.CheckHasNewMessage();
        }


        public void UnitInfoChanged(IUser user)
        {
            lock (this.locker)
            {
                for (int i = 0; i < this.recentContacts.Count; i++)
                {
                    if (this.recentContacts[i].ID == user.ID)
                    {
                        this.recentContacts[i] = this.ConvertToRecentContactModel(user);
                        this.BindSource();
                        return;
                    }
                }
            }
        }
        public void RemoveUnit(string unitID)
        {
            lock (this.locker)
            {
                for (int i = 0; i < this.recentContacts.Count; i++)
                {
                    if (this.recentContacts[i].ID == unitID)
                    {
                        this.recentContacts.RemoveAt(i);
                        this.CheckHasNewMessage();
                        this.BindSource();
                        return;
                    }
                }
            }
        }

        public void UnitCommentNameChanged(IUnit unit)
        {
            RecentContactModel model = this.FindModel4Cache(unit.ID);
            if (model != null)
            {
                model.CommentName = unit.CommentName;
            }
            this.BindSource();
        }

        private RecentContactModel FindModel4Cache(string id)
        {
            foreach (RecentContactModel item in this.recentContacts)
            {
                if (item.ID == id)
                {
                    return item;
                }
            }
            return null;
        }

        public void LastWordChanged(IUnit unit, string lastWords,bool newMsg = false)
        {
            this.AddRecentRecord(unit, newMsg);
            
        }

        public void SetNewMessageIco(IUnit unit, bool isNewMsg)
        {
            //若列表中不存在，且为不是新消息，直接跳过
            if (!this.objectManager.Contains(unit.ID) && !isNewMsg)
            {
                return;
            }
            this.AddRecentRecord(unit, isNewMsg);
        }

        private void AddRecentRecord(IUnit unit, bool isNewMsg)
        {
            RecentContactModel model = this.ConvertToRecentContactModel(unit);
            model.IsNewMsg = isNewMsg;
            this.AddRecentModel(model, 0);
        }

        public void SetNewNotifyMessage(bool isNewMsg, string msgDescribe,TalkBase.UnitType? unitType )
        {
            this.notifyModel.IsNewMsg = isNewMsg;
            if (!string.IsNullOrEmpty(msgDescribe))
            {
                this.notifyModel.LastWords = msgDescribe;
                if (unitType.HasValue) { this.notifyModel.UnitType = unitType.Value; }
            }
            this.AddRecentModel(this.notifyModel, 0);
        }

        #region 右键菜单
        //发送消息
        private void SendMessage_Click(CpfObject sender)
        {
            RecentContactModel recentModel = this.treeView.SelectedValue as RecentContactModel;
            if (recentModel == null) { return; }
            CommonHelper.MoveToChat(recentModel);
        }

        //查看消息记录
        private void MessageRecord_Click(CpfObject sender)
        {
            RecentContactModel recentModel = this.treeView.SelectedValue as RecentContactModel;
            if (recentModel == null) { return; }
            Window form = MainWindow.ChatFormController.GetChatRecordForm(recentModel.ID);
            if (form != null)
            {
                form.Show_Topmost();
            }
        }

        //从列表中移除
        private void DeleteItemFormList_Click(CpfObject sender)
        {
            RecentContactModel recentModel = this.treeView.SelectedValue as RecentContactModel;
            if (recentModel == null) { return; }
            this.RemoveUnit(recentModel.ID);
        }

        #endregion
    }
}
