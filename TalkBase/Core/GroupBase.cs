using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using ESBasic;
using SqlSugar;

namespace TalkBase
{
    [Serializable]
    public class GroupBase : IGroup
    {
        #region Force Static Check       
        public const string _GroupID = "GroupID";
        public const string _Name = "Name";
        public const string _CreatorID = "CreatorID";
        public const string _Announce = "Announce";
        public const string _Members = "Members";
        public const string _CreateTime = "CreateTime";
        public const string _IsPrivate = "IsPrivate";
        public const string _Version = "Version";
        #endregion        

        #region GroupID
        private string groupID = "";
        public string GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }
        [SugarColumn(IsIgnore = true)]
        public string ID
        {
            get { return this.GroupID; }
        }
        #endregion

        #region GroupType
        private GroupType groupType = GroupType.CommonGroup;
        [SugarColumn(IsIgnore = true)]
        public GroupType GroupType
        {
            get { return groupType; }
            set { groupType = value; }
        } 
        #endregion

        #region Name
        private string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        #endregion

        #region Creator
        private string creatorID = "";
        /// <summary>
        /// 群创建者。
        /// </summary>
        public string CreatorID
        {
            get { return creatorID; }
            set { creatorID = value; }
        } 
        #endregion

        #region CreateTime
        private DateTime createTime = DateTime.Now;
        /// <summary>
        /// 创建时间。
        /// </summary>
        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        } 
        #endregion

        #region Announce
        private string announce = "";
        /// <summary>
        /// 公告。
        /// </summary>
        public string Announce
        {
            get { return announce; }
            set { announce = value; }
        } 
        #endregion

        #region Members
        private string members = "";
        /// <summary>
        /// 组成员，ID使用英文逗号隔开。
        /// </summary>
        public string Members
        {
            get { return members; }
            set 
            { 
                members = value;
                this.memberList = null;
            }
        }

        [SugarColumn(IsIgnore = true)]
        public int MemberCount => this.MemberList.Count;

        #region MemberList
        private List<string> memberList = null;

        [SugarColumn(IsIgnore = true)]
        [XmlIgnore]
        /// <summary>
        /// 非DB字段
        /// </summary>
        public List<string> MemberList
        {
            get
            {
                if (memberList == null)
                {
                    this.memberList = new List<string>(this.members.Split(','));
                    this.memberList.Remove("");
                }

                return memberList;
            }
        }
        #endregion

        public void AddMember(string userID)
        {
            if (this.MemberList.Contains(userID))
            {
                return;
            }
            this.MemberList.Add(userID);
            this.Members = ESBasic.Helpers.StringHelper.ContactString<string>(this.MemberList, ",");
        }

        public void RemoveMember(string userID)
        {
            this.MemberList.Remove(userID);
            this.Members = ESBasic.Helpers.StringHelper.ContactString<string>(this.MemberList, ",");
        }
        #endregion

        #region IsPrivate
        private bool isPrivate = false;
        public bool IsPrivate
        {
            get { return this.isPrivate; }
            set { this.isPrivate = value; }
        } 
        #endregion

        #region Version
        private int version = 0;
        public int Version
        {
            get { return version; }
            set { version = value; }
        }
        #endregion        

        #region CommentName
        private string commentName = null;

        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 用于客户端，为群组设定的备注名称。
        /// </summary>        
        public string CommentName
        {
            get { return commentName; }
            set { commentName = value; }
        }
        #endregion

        #region DisplayName
        [SugarColumn(IsIgnore = true)]
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.CommentName))
                {
                    return this.CommentName;
                }

                return this.Name;
            }
        } 
        #endregion

        #region LastWordsRecord
        private LastWordsRecord lastWordsRecord;


        [SugarColumn(IsIgnore = true)]
        public virtual LastWordsRecord LastWordsRecord
        {
            get { return lastWordsRecord; }
            set { lastWordsRecord = value; }
        }
        #endregion       

        public Parameter<string, string> GetIDName()
        {
            return new Parameter<string, string>(this.GroupID, this.Name);
        }

        public void ChangeMembers(List<string> members)
        {
            this.Members = ESBasic.Helpers.StringHelper.ContactString<string>(members, ",");
        }

        public virtual void ReplaceOldUnit(IUnit old)
        {
        }

        public virtual void UpdateBusinessInfo(Dictionary<string, byte[]> businessInfo)
        {
        }

        [SugarColumn(IsIgnore = true)]
        public UnitType UnitType
        {
            get { return TalkBase.UnitType.Group; }
        }
        [SugarColumn(IsIgnore = true)]
        public bool IsUser
        {
            get { return false; }
        }
    }
}
