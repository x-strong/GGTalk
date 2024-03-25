using CPF;
using GGTalk;
using GGTalk.Linux.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TalkBase;

namespace GGTalk.Linux.ViewModels
{
    internal class RecentListViewModel : CPF.CpfObject
    {
        private Collection<RecentListModel> recentLists;
        public Collection<RecentListModel> RecentListModels
        {
            get { return (Collection<RecentListModel>)GetValue(); }
            set { SetValue(value); }
        }
    }

    internal class RecentListModel : CPF.CpfObject
    {
        public string CatalogName
        {
            get { return (string)GetValue(); }
            set { SetValue(value); }
        }

        public Collection<RecentContactModel> RecentList {
            get { return (Collection<RecentContactModel>)GetValue(); }
            set { SetValue(value); }
        }
    }

    internal class RecentContactModel : IUnit, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public RecentContactModel() { }
        public RecentContactModel(GGGroup ggGroup)
        {
            this.id = ggGroup.ID;
            this.name = ggGroup.Name;
            this.commentName = ggGroup.CommentName;
            this.unitType = UnitType.Group;
            this.headImg = GGTalk.Linux.CommonOptions.ResourcesCatalog+ "Group1.png";
            this.LastWordsRecord = ggGroup.LastWordsRecord;
        }

        public RecentContactModel(GGUser ggUser)
        {
            this.id = ggUser.ID;
            this.name = ggUser.Name;
            this.commentName = ggUser.CommentName;
            this.unitType = UnitType.User;
            this.headImg = GlobalResourceManager.GetHeadImage(ggUser);
            this.LastWordsRecord = ggUser.LastWordsRecord;
        }

        private string id = "";
        public string ID { get => this.id; set { this.id = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
                }
            } }
        private string name = "";

        public string Name
        {
            get => this.name;
            set { this.name = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }

        }

        private UnitType unitType = UnitType.User;
        public UnitType UnitType { get { return this.unitType; } set { this.unitType = value; } }

        public bool IsUser => this.unitType == UnitType.User;

        private string commentName = "";
        public string CommentName { get => this.commentName; 
            set { this.commentName = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(CommentName)));
                }
            } }

        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.commentName))
                {
                    return this.commentName;
                }
                return this.name;
            }
        }
        private string lastWords = string.Empty;
        public string LastWords
        {
            get
            {
                if (string.IsNullOrEmpty(this.lastWords) && this.LastWordsRecord != null && this.LastWordsRecord.ChatContent != null)
                {                    
                    ChatBoxContent2 chatBoxContent = ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<ChatBoxContent2>(this.LastWordsRecord.ChatContent, 0);
                    return chatBoxContent.GetTextWithPicPlaceholder("[图]");
                }
                return this.lastWords;
            }
            set
            {
                this.lastWords = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(LastWords)));
                }
            }
        }

        private string describe = string.Empty;
        public string Describe
        {
            get { return this.describe; }
            set
            {
                this.describe = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Describe)));
                }
            }
        }


        private object headImg = null;
        public object HeadImg
        {
            get
            {
                if (this.IsNotifyMsg)
                {
                    return GGTalk.Linux.Helpers.FileHelper.FindAssetsBitmap("broadcast.png");
                }
                return this.headImg;
            }
        }
        public int Version
        {
            get
            {
                return 0;
            }
            set
            {

            }
        }
        private bool isNewMsg = false;
        public bool IsNewMsg
        {
            get { return this.isNewMsg; }
            set
            {
                this.isNewMsg = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(IsNewMsg)));
                }
            }
        }

        private bool isNotifyMsg = false;
        public bool IsNotifyMsg
        {
            get { return this.isNotifyMsg; }
            set
            {
                this.isNotifyMsg = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotifyMsg)));
                }
            }
        }

        private LastWordsRecord lastWordsRecord;
        public LastWordsRecord LastWordsRecord
        {
            get { return this.lastWordsRecord; }
            set
            {
                this.lastWordsRecord = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(LastWordsRecord)));
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(LastWords)));
                }
            }
        }

        public void ReplaceOldUnit(IUnit old)
        {

        }

        public void UpdateBusinessInfo(Dictionary<string, byte[]> businessInfo)
        {

        }

        public ESBasic.Parameter<string, string> GetIDName()
        {
            return null;
        }
    }
}
