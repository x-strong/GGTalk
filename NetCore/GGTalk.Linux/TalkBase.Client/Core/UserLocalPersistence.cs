using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TalkBase.Client
{
    /// <summary>
    /// 用户、群组本地缓存到文件。
    /// </summary>    
    [Serializable]
    public class UserLocalPersistence<TUser, TGroup>
    {
        #region Load、Save
        public static UserLocalPersistence<TUser, TGroup> Load(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return null;
                }
                string data = ESBasic.Helpers.FileHelper.GetFileContent(filePath);
                return ESBasic.Helpers.SerializeHelper.ObjectXml<UserLocalPersistence<TUser, TGroup>>(data);
            }
            catch
            {
                return null;
            }
        }

        public void Save(string filePath)
        {
            string data = ESBasic.Helpers.SerializeHelper.XmlObject(this);
            ESBasic.Helpers.FileHelper.GenerateFile(filePath, data);
        } 
        #endregion

        public UserLocalPersistence() { }
        public UserLocalPersistence(List<TUser> friends, List<TGroup> groups, List<string> recents, List<string> quickAnswers)
        {
            this.friendList = friends;
            this.groupList = groups ?? new List<TGroup>();
            this.recentList = recents ?? new List<string>();
            this.quickAnswerList = quickAnswers ?? new List<string>();
        }             

        #region FriendList
        private List<TUser> friendList = new List<TUser>();
        /// <summary>
        /// 好友缓存。（包括组友）
        /// </summary>
        public List<TUser> FriendList
        {
            get { return friendList ?? new List<TUser>(); }
            set { friendList = value; }
        }
        #endregion

        #region GroupList
        private List<TGroup> groupList = new List<TGroup>();
        public List<TGroup> GroupList
        {
            get { return groupList ?? new List<TGroup>(); }
            set { groupList = value; }
        } 
        #endregion

        #region RecentList
        private List<string> recentList = new List<string>();
        /// <summary>
        /// 最近联系人/群。
        /// </summary>
        public List<string> RecentList
        {
            get { return recentList ?? new List<string>(); }
            set { recentList = value; }
        }
        #endregion

        #region QuickAnswerList
        private List<string> quickAnswerList = new List<string>();
        /// <summary>
        /// 快捷回复列表。
        /// </summary>
        public List<string> QuickAnswerList
        {
            get { return quickAnswerList ?? new List<string>(); }
            set { quickAnswerList = value; }
        } 
        #endregion
    }    
}
