using CPF;
using System;
using System.Collections.Generic;
using System.Text;
using TalkBase;

namespace GGTalk.Linux.ViewModels
{
    internal class NotifyListViewModel:CpfObject
    {
        public Collection<AddFriendOrGroupRequestModel> Models
        {
            get { return GetValue<Collection<AddFriendOrGroupRequestModel>>(); }
            set { SetValue(value); }
        }
    }

    internal class AddFriendOrGroupRequestModel:CPF.CpfObject
    {
        public AddFriendOrGroupRequestModel(AddFriendRequest addFriendRequest)
        {
            this.NotifyType = NotifyType.User;
            this.AutoID = addFriendRequest.AutoID;
            this.RequesterID = addFriendRequest.RequesterID;
            this.AccepterID = addFriendRequest.AccepterID;
            this.Comment2 = addFriendRequest.Comment2;
            this.State = addFriendRequest.State;
            this.Notified = addFriendRequest.Notified;
            this.CreateTime = addFriendRequest.CreateTime;
        }

        public AddFriendOrGroupRequestModel(AddGroupRequest addGroupRequest)
        {
            this.NotifyType = NotifyType.Group;
            this.AutoID = addGroupRequest.AutoID;
            this.RequesterID = addGroupRequest.RequesterID;
            this.AccepterID = addGroupRequest.AccepterID;
            this.GroupID = addGroupRequest.GroupID;
            this.Comment2 = addGroupRequest.Comment2;
            this.State = addGroupRequest.State;
            this.Notified = addGroupRequest.Notified;
            this.CreateTime = addGroupRequest.CreateTime;
        }

        public NotifyType NotifyType
        {
            get { return GetValue<NotifyType>(); }
            set { SetValue(value); }
        }

        public System.Int32 AutoID
        {
            get { return GetValue<Int32>(); }
            set { SetValue(value); }
        }

        public System.String RequesterID
        {
            get { return GetValue<String>(); }
            set { SetValue(value); }
        }

        public System.String GroupID
        {
            get { return GetValue<String>(); }
            set { SetValue(value); }
        }

        public System.String AccepterID
        {
            get { return GetValue<String>(); }
            set { SetValue(value); }
        }


        public System.String Comment2
        {
            get { return GetValue<String>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// State 0:请求中 1：同意  2：拒绝
        /// </summary>
        public int State
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Notified 0:申请未通知对方  1：申请已通知对方
        /// </summary>
        public System.Boolean Notified
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public System.DateTime CreateTime
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }
    }
}
