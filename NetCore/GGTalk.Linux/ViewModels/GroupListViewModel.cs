using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Shapes;
using CPF.Styling;
using GGTalk;
using GGTalk.Linux.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGTalk.Linux.ViewModels
{

    internal class GroupListViewModel : CPF.CpfObject
    {
        public Collection<GroupListModel> GroupListModelList
        {
            get { return (Collection<GroupListModel>)GetValue(); }
            set { SetValue(value); }
        }
    }

    internal class GroupListModel : CPF.CpfObject
    {
        public string CatalogName
        {
            get { return (string)GetValue(); }
            set { SetValue(value); }
        }
        public Collection<GGGroup> GroupList
        {
            get { return (Collection<GGGroup>)GetValue(); }
            set { SetValue(value); }
        }
    }

    internal class GroupDetailUserListModel : CPF.CpfObject
    {
        public Collection<GGUserPlus> UserList
        {
            get { return (Collection<GGUserPlus>)GetValue(); }
            set { SetValue(value); }
        }
    }
}
