﻿using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Shapes;
using CPF.Styling;
using GGTalk;
using GGTalk.Linux;
using GGTalk.Linux.ViewModels;
using GGTalk.Linux.Views;
using GGTalk.Linux.Views;
using System;
using System.Collections.Generic;
using System.Text;
using TalkBase;

namespace GGTalk.Linux.Controls
{
    internal class NotifyItemPanel2 : ListBoxItem
    {
        private TextBlock tb_time, tb_name, tb_remark, tb_result;
        private Picture image_head;
        private Button btn_refuse, btn_agree;

        private NotifyType notifyType = NotifyType.User;
        private bool isRequester = false;
        private string friendID = string.Empty;
        private string groupID = string.Empty;
        public string ID { get; set; }
        /// <summary>
        /// 处理申请请求按钮点击时触发  string: ID  / false：拒绝  true：同意
        /// </summary>
        public event Action<string, bool> OnHandleRequestClicked;

        public NotifyItemPanel2()
        {
            this.InitializeComponent();
            this.Initialized += NotifyItemPanel2_Initialized;
        }

        private void NotifyItemPanel2_Initialized(object sender, EventArgs e)
        {
            this.InitializeView();
        }

        private bool initialized = false;
        protected override void InitializeComponent()
        {
            if (this.initialized) { return; }
            this.initialized = true;
            //模板定义
            Width = "100%";
            Height = 134;
            Children.Add(
                new Panel
                {
                    Height = 30,
                    Width = "100%",
                    MarginTop = 0,
                    Children =
                    {
                        new TextBlock
                        {
                            Name=nameof(this.tb_time),
                            PresenterFor=this,
                            MarginLeft = 14,
                        }
                    }
                }
            );
            Children.Add(
                new Panel
                {
                    Background = "240,240,240",
                    BorderFill = "193,208,214",
                    BorderThickness = new Thickness(1, 1, 1, 1),
                    BorderType = BorderType.BorderThickness,
                    Height = 94,
                    Width = 550,
                    MarginTop = 30,
                    Children =
                    {
                        new Picture
                        {
                            Name=nameof(this.image_head),
                            PresenterFor=this,
                            Cursor=Cursors.Hand,
                            IsAntiAlias = true,
                            Width = 62,
                            Height = 62,
                            MarginLeft = 10,
                            Stretch = Stretch.Fill,
                        },
                       new TextBlock
                        {
                            Name =nameof(this.tb_name),
                            PresenterFor=this,
                            MarginLeft = 78,
                            MarginTop = 22,
                            FontSize = 13,
                            FontStyle = FontStyles.Bold,
                        },
                        new TextBlock
                        {
                            Name=nameof(this.tb_remark),
                            PresenterFor=this,
                            Foreground = "179,179,179",
                            FontSize = 12,
                            MarginLeft = 78,
                            MarginBottom = 22,
                        },
                        new Button
                        {
                            Name=nameof(this.btn_agree),
                            PresenterFor=this,
                            Classes = "NoticeCheckButton",
                            Background = "240,240,240",
                            MarginRight = 20,
                            Width = 70 ,
                            Height = 26,
                            Content = "同意",
                            BorderFill = "159,159,159",
                            BorderThickness = new Thickness(1,1,1,1),
                            BorderType = BorderType.BorderThickness,
                        },
                        new Button
                        {
                            Name=nameof(this.btn_refuse),
                            PresenterFor=this,
                            Classes = "NoticeCheckButton",
                            Background = "240,240,240",
                            MarginRight = 100,
                            Width = 70 ,
                            Height = 26,
                            Content = "拒绝",
                            BorderFill = "159,159,159",
                            BorderThickness = new Thickness(1,1,1,1),
                            BorderType = BorderType.BorderThickness,
                        },
                        new TextBlock
                        {
                            Name=nameof(this.tb_result),
                            PresenterFor=this,
                            MarginRight = 20,
                            Visibility = Visibility.Hidden,
                            Text = "已同意"
                        }
                    }
                }
            );
  
            if (!this.DesignMode)
            {
                this.tb_time = this.FindPresenterByName<TextBlock>(nameof(this.tb_time));
                this.tb_name = this.FindPresenterByName<TextBlock>(nameof(this.tb_name));
                this.tb_remark = this.FindPresenterByName<TextBlock>(nameof(this.tb_remark));
                this.tb_result = this.FindPresenterByName<TextBlock>(nameof(this.tb_result));
                this.image_head = this.FindPresenterByName<Picture>(nameof(this.image_head));
                this.image_head.MouseDown += Image_head_MouseDown;
                this.btn_refuse = this.FindPresenterByName<Button>(nameof(this.btn_refuse));
                this.btn_agree = this.FindPresenterByName<Button>(nameof(this.btn_agree));
                this.btn_agree.Click += Agree_Click;
                this.btn_refuse.Click += Refuse_Click;
                this.OnHandleRequestClicked += delegate { };
            }
        }

        //点击头像 跳转到详情界面
        private void Image_head_MouseDown(object sender, CPF.Input.MouseButtonEventArgs e)
        {
            if (notifyType == NotifyType.User)
            {
                GGUser friend = Program.ResourceCenter.ClientGlobalCache.GetUser(this.friendID);
                if (friend == null) { return; }
                UserInfoWindow userInfoWindow = new UserInfoWindow(friend);
                userInfoWindow.Show_Topmost();
            }
            else if (notifyType == NotifyType.Group)
            {
                GGGroup ggGroup = Program.ResourceCenter.ClientGlobalCache.GetGroup(this.groupID);
                if (ggGroup == null) { return; }
                GroupInfoWindow groupInfoWindow = new GroupInfoWindow(ggGroup);
                groupInfoWindow.Show_Topmost();
            }
        }

        private void InitializeView()
        {
            AddFriendOrGroupRequestModel model = this.DataContext as AddFriendOrGroupRequestModel;
            if (model == null) return;
            this.notifyType = model.NotifyType;
            this.isRequester = model.RequesterID == Program.ResourceCenter.CurrentUserID;
            this.friendID = this.isRequester ? model.AccepterID : model.RequesterID;
            GGUser friend = Program.ResourceCenter.ClientGlobalCache.GetUser(this.friendID);
            this.image_head.Source = GlobalResourceManager.GetHeadImageOnline(friend);
            this.tb_time.Text = model.CreateTime.GetDateTimeFormats('f')[0];

            if (model.NotifyType == NotifyType.Group)
            {
                this.groupID = model.GroupID;
                GGGroup group = Program.ResourceCenter.ClientGlobalCache.GetGroup(model.GroupID);
                this.tb_name.Text = isRequester ? "已申请加入 " + group.Name : friend.DisplayName + "申请加入 " + group.Name;
                this.tb_remark.Text = isRequester ? "已发送验证请求" : model.Comment2;
            }
            else
            {
                this.tb_name.Text = friend.DisplayName;
                this.tb_remark.Text = isRequester ? "已发送验证请求" : model.Comment2;
            }
            this.InitResultShow(isRequester, (RequsetType)model.State);
        }

        private void InitResultShow(bool isRequester, RequsetType requsetType)
        {
            if (!isRequester && requsetType == RequsetType.Request)
            {
                this.tb_result.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.btn_refuse.Visibility = Visibility.Collapsed;
                this.btn_agree.Visibility = Visibility.Collapsed;
                this.tb_result.Visibility = Visibility.Visible;
                switch (requsetType)
                {
                    case RequsetType.Request:
                        this.tb_result.Text = "等待验证";
                        break;
                    case RequsetType.Agree:
                        this.tb_result.Text = "已同意";
                        break;
                    case RequsetType.Reject:
                        this.tb_result.Text = "已拒绝";
                        break;
                }
            }

            //if (requsetType == RequsetType.Agree)
            //{
            //    this.OnRequestStateChanged(true);
            //}
            //else if (requsetType == RequsetType.Reject)
            //{
            //    this.OnRequestStateChanged(false);
            //}
        }

        private void Refuse_Click(object sender, RoutedEventArgs e)
        {
            if (notifyType == NotifyType.User)
            {
                Program.ResourceCenter.ClientOutter.HandleAddFriendRequest(friendID, "", false);
            }
            else if (notifyType == NotifyType.Group)
            {
                Program.ResourceCenter.ClientOutter.HandleAddGroupRequest(friendID, groupID, false);
            }
            this.OnRequestStateChanged(false);
        }

        private void Agree_Click(object sender, RoutedEventArgs e)
        {
            if (notifyType == NotifyType.User)
            {
                Program.ResourceCenter.ClientOutter.HandleAddFriendRequest(friendID, FunctionOptions.DefaultFriendCatalog, true);
                //双方加为好友后，跳转到好友聊天框
                FriendChatWindow form = (FriendChatWindow)MainWindow.ChatFormController.GetForm(friendID);
                form.AddFriendSucceed();
                form.Show_Topmost();
            }
            else if (notifyType == NotifyType.Group)
            {
                Program.ResourceCenter.ClientOutter.HandleAddGroupRequest(friendID, groupID, true);
            }
            this.OnRequestStateChanged(true);
        }

        /// <summary>
        /// AddFriendRequest状态发生改变
        /// </summary>
        /// <param name="isAgree"></param>
        public void OnRequestStateChanged(bool isAgree)
        {
            AddFriendOrGroupRequestModel model = this.DataContext as AddFriendOrGroupRequestModel;
            if (isAgree)
            {
                model.State = (byte)RequsetType.Agree;
                this.tb_result.Text = "已同意";
            }
            else
            {
                model.State = (byte)RequsetType.Reject;
                this.tb_result.Text = "已拒绝";
            }
            this.btn_refuse.Visibility = Visibility.Collapsed;
            this.btn_agree.Visibility = Visibility.Collapsed;
            this.tb_result.Visibility = Visibility.Visible;
            this.OnHandleRequestClicked(this.ID, isAgree);
        }

    }
}
