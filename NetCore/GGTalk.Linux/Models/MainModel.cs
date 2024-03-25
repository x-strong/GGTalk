using CPF;
using CPF.Controls;
using CPF.Drawing;
using CPF.Platform;
using System;
using System.Collections.Generic;
using TalkBase;
using System.Text;

namespace GGTalk.Linux.Models
{

    internal class GGUserPlus : GGUser, IComparable
    {
        public GGUserPlus() { }
        public GGUserPlus(GGUser user)
        {
            ESBasic.Helpers.ReflectionHelper.CopyProperty(user, this);
        }

        #region HeadImage
        [NonSerialized]
        private Image headImage = null;
        /// <summary>
        /// 自定义头像。非DB字段。
        /// </summary>
        public Image HeadImage
        {
            get
            {
                this.headImage = GlobalResourceManager.GetHeadImage(this);
                return headImage;
            }
        }
        public void SetHeadImage(Image img)
        {
            this.headImage = img;
        }
        #endregion

        #region HeadImageGrey
        [NonSerialized]
        private Image headImageGrey = null;
        /// <summary>
        /// 自定义头像。非DB字段。
        /// </summary>
        public Image HeadImageGrey
        {
            get
            {
                if (this.headImageGrey == null && base.HeadImageData != null)
                {
                    //this.headImageGrey = ESBasic.Helpers.ImageHelper.ConvertToGrey(this.HeadImage);
                }
                return this.headImageGrey;
            }
        }
        #endregion

        public int CompareTo(object obj)
        {
            GGUserPlus other = obj as GGUserPlus;
            if (this == other)
            {
                return 0;
            }

            if (this == null)
            {
                return -1;
            }

            if (other == null)
            {
                return 1;
            }

            if (this.ID == Program.PassiveEngine.CurrentUserID)
            {
                return -1;
            }

            if (other.ID == Program.PassiveEngine.CurrentUserID)
            {
                return 1;
            }

            if (this.UserStatus == TalkBase.UserStatus.OffLine && other.UserStatus == UserStatus.OffLine)
            {
                return this.DisplayName.CompareTo(other.DisplayName);
            }

            if (this.UserStatus != UserStatus.OffLine && other.UserStatus != UserStatus.OffLine)
            {
                return this.DisplayName.CompareTo(other.DisplayName);
            }

            if (this.UserStatus != UserStatus.OffLine)
            {
                return -1;
            }

            return 1;
        }
    }



    internal class MainModel : CpfObject
    {

        
        

        public Visibility visib = Visibility.Collapsed;

        public bool SearchIfClick = false;

        public MainModel()
        {
            
            //分组列表
            FriendGroupList = new Collection<(string, Collection<FriendGroup>)>();
            for (int i = 0; i < 1; i++)
            {
                var list = new Collection<FriendGroup>();

                for (int j = 0; j < 7; j++)
                {
                    var nn = new FriendGroup
                    {
                        ImagePath = CommonOptions.ResourcesCatalog+ "8.png",
                        StateImagePath = CommonOptions.ResourcesCatalog+ "0.png",
                        NickName = "名称" + j,
                        Signature = "个性签名" + j
                    };

                    list.Add(nn);
                }
                FriendGroupList.Add(("分组" + i, list));
            }

            //群列表
            GroupList = new Collection<(string, Collection<Group>)>();
            for (int i = 0; i < 1; i++)
            {
                var list = new Collection<Group>();

                for (int j = 0; j < 4; j++)
                {
                    var nn = new Group
                    {
                        GroupImagePath = CommonOptions.ResourcesCatalog+ "Group1.png",
                        GroupName = "讨论组" + j,
                        Number = j +"人" ,
                        CreationTime = "2020-09-0" + j
                    };

                    list.Add(nn);
                }
                GroupList.Add(("我的群 [" +i+3 +"/"+ i+3 +"]"  , list));
            }

            RecentContactList = new Collection<(string, Collection<RecentContact>)>();
            for (int i = 0; i < 1; i++)
            {
                var list = new Collection<RecentContact>();

                for (int j = 0; j < 7; j++)
                {
                    var nn = new RecentContact
                    {
                        HeadImagePath = CommonOptions.ResourcesCatalog+ "8.png",
                        NickName = "贝贝" + j,
                        Sender = "发送者：" + j,
                        Message = "发送的消息" + j
                    };

                    list.Add(nn);
                }
                RecentContactList.Add(("最近联系人 [" + i + 3 + "/" + i + 3 + "]", list));
            }

        }


        //分组列表
        public Collection<(string, Collection<FriendGroup>)> FriendGroupList
        {
            get { return GetValue<Collection<(string, Collection<FriendGroup>)>>(); }
            set { SetValue(value); }
        }

        //群列表
        public Collection<(string, Collection<Group>)> GroupList
        {
            get { return GetValue<Collection<(string, Collection<Group>)>>(); }
            set { SetValue(value); }
        }

        //最近联系人
        public Collection<(string, Collection<RecentContact>)> RecentContactList
        {
            get { return GetValue<Collection<(string, Collection<RecentContact>)>>(); }
            set { SetValue(value); }
        }


        //命令
        //public void SearchClick()
        //{
        //    SearchIfClick = !SearchIfClick;
        //    if(SearchIfClick) {
        //        visib = Visibility.Visible;
        //    }
        //    if (SearchIfClick)
        //    {
        //        visib = Visibility.Collapsed;
        //    }
        //}


        public void SearchClisks()
        {
            
        }


        public bool IsChecked
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }



    }


    /// <summary>
    /// 分组模型
    /// </summary>
    internal class FriendGroup
    {
        public String NickName { get; set; }

        public String ImagePath { get; set; }

        public String Signature { get; set; }

        public String StateImagePath { get; set; }


        public List<FriendGroup> FriendGroupList { get; set; } = new List<FriendGroup>();
    }


    /// <summary>
    /// 群组模型
    /// </summary>
    internal class Group
    {

        public String GroupImagePath { get; set; }

        public String GroupName { get; set; }
                
        public String Number { get; set; }

        public String CreationTime  { get; set; }

        public List<FriendGroup> GroupList { get; set; } = new List<FriendGroup>();
    }

    /// <summary>
    /// 最近联系人模型
    /// </summary>
    internal class RecentContact
    {

        public String HeadImagePath { get; set; }

        public String NickName { get; set; }

        public String Sender { get; set; }

        public String Message { get; set; }

        public List<RecentContact> RecentContactList { get; set; } = new List<RecentContact>();
    }

}
