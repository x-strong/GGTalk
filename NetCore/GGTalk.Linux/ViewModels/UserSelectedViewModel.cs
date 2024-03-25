using CPF;
using CPF.Drawing;
using GGTalk.Linux.Controls.Templates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GGTalk.Linux.ViewModels
{
    internal class UserSelectedViewModel : CPF.CpfObject
    {
        public UserSelectedViewModel()
        {
            UserSelectedModels = new Collection<UserSelectedModel>();
            UserSelectedModels.CollectionChanged += UserSelectedModels_CollectionChanged;
        }

        private void UserSelectedModels_CollectionChanged(object sender, CollectionChangedEventArgs<UserSelectedModel> e)
        {
            this.Count = this.UserSelectedModels.Count;
        }

        public Collection<UserSelectedModel> UserSelectedModels
        {
            get { return (Collection<UserSelectedModel>)GetValue(); }
            set { SetValue(value);  }
        }         

        [PropertyMetadata(0)]
        public int Count {
            
            get {               
                return (int)GetValue(); }
            set { SetValue(value); }
        }

    }


    internal class UserSelectedModel : CPF.CpfObject
    {
        public string ID
        {
            get { return (string)GetValue(); }
            set { SetValue(value); }
        }
        public string Name
        {
            get { return (string)GetValue(); }
            set { SetValue(value); }
        }
        public Image HeadImage
        {
            get { return (Image)GetValue(); }
            set { SetValue(value); }
        }

        public UserSelectedModel(string _id, string _name, Image _headImg)
        {
            this.ID = _id;
            this.Name = _name;
            this.HeadImage = _headImg;
        }

        public Commands Command { get; set; }

    }
}
