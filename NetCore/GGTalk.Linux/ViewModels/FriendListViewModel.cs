using CPF;
using GGTalk.Linux.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGTalk.Linux.ViewModels
{
    internal class FriendListViewModel : CPF.CpfObject
    {
        public Collection<FriendListModel> FriendListModelList
        {
            get { return (Collection<FriendListModel>)GetValue(); }
            set { SetValue(value); }
        }
    }

    internal class FriendListModel : CpfObject
    {
        public string CatalogName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public Collection<GGUserPlus> UserList
        {
            get { return (Collection<GGUserPlus>)GetValue(); }
            set { SetValue(value); }
        }
    }
}
