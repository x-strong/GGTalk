using DataRabbit;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalkBase
{
    #region AddFriendRequest
    [Serializable]
    public partial class AddFriendRequest : IEntity<System.Int32>
    {
        #region Force Static Check
        public const string TableName = "AddFriendRequest";
        public const string _AutoID = "AutoID";
        public const string _RequesterID = "RequesterID";
        public const string _AccepterID = "AccepterID";
        public const string _RequesterCatalogName = "RequesterCatalogName";
        public const string _AccepterCatalogName = "AccepterCatalogName";
        public const string _Comment2 = "Comment2";
        public const string _State = "State";
        public const string _Notified = "Notified";
        public const string _CreateTime = "CreateTime";
        #endregion

        #region Property

        #region AutoID
        private System.Int32 m_AutoID = 0;
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 AutoID
        {
            get
            {
                return this.m_AutoID;
            }
            set
            {
                this.m_AutoID = value;
            }
        }
        #endregion

        #region RequesterID
        private System.String m_RequesterID = "";
        public System.String RequesterID
        {
            get
            {
                return this.m_RequesterID;
            }
            set
            {
                this.m_RequesterID = value;
            }
        }
        #endregion

        #region AccepterID
        private System.String m_AccepterID = "";
        public System.String AccepterID
        {
            get
            {
                return this.m_AccepterID;
            }
            set
            {
                this.m_AccepterID = value;
            }
        }
        #endregion

        #region RequesterCatalogName
        private System.String m_RequesterCatalogName = "";
        public System.String RequesterCatalogName
        {
            get
            {
                return this.m_RequesterCatalogName;
            }
            set
            {
                this.m_RequesterCatalogName = value;
            }
        }
        #endregion

        #region AccepterCatalogName
        private System.String m_AccepterCatalogName = "";
        public System.String AccepterCatalogName
        {
            get
            {
                return this.m_AccepterCatalogName;
            }
            set
            {
                this.m_AccepterCatalogName = value;
            }
        }
        #endregion

        #region Comment
        private System.String m_Comment2 = "";
        public System.String Comment2
        {
            get
            {
                return this.m_Comment2;
            }
            set
            {
                this.m_Comment2 = value;
            }
        }
        #endregion

        #region State
        private int m_State = 0;
        /// <summary>
        /// State 0:请求中 1：同意  2：拒绝
        /// </summary>
        public int State
        {
            get
            {
                return this.m_State;
            }
            set
            {
                this.m_State = value;
            }
        }
        #endregion

        #region Notified
        private System.Boolean m_Notified = false;
        /// <summary>
        /// Notified 0:申请未通知对方  1：申请已通知对方
        /// </summary>
        public System.Boolean Notified
        {
            get
            {
                return this.m_Notified;
            }
            set
            {
                this.m_Notified = value;
            }
        }
        #endregion

        #region CreateTime
        private System.DateTime m_CreateTime = DateTime.Now;
        public System.DateTime CreateTime
        {
            get
            {
                return this.m_CreateTime;
            }
            set
            {
                this.m_CreateTime = value;
            }
        }
        #endregion
        #endregion

        #region IEntity Members
        public System.Int32 GetPKeyValue()
        {
            return this.AutoID;
        }
        #endregion
    }

    [Serializable]
    public class AddFriendRequestPage
    {
        public List<AddFriendRequest> AddFriendRequestList { get; set; }

        public int TotalEntityCount { get; set; }
    }
    #endregion

    #region AddGroupRequest
    [Serializable]
    public partial class AddGroupRequest : IEntity<System.Int32>
    {
        #region Force Static Check
        public const string TableName = "AddGroupRequest";
        public const string _AutoID = "AutoID";
        public const string _RequesterID = "RequesterID";
        public const string _GroupID = "GroupID";
        public const string _AccepterID = "AccepterID";
        public const string _Comment2 = "Comment2";
        public const string _State = "State";
        public const string _Notified = "Notified";
        public const string _CreateTime = "CreateTime";
        #endregion

        #region Property

        #region AutoID
        private System.Int32 m_AutoID = 0;
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 AutoID
        {
            get
            {
                return this.m_AutoID;
            }
            set
            {
                this.m_AutoID = value;
            }
        }
        #endregion

        #region RequesterID
        private System.String m_RequesterID = "";
        public System.String RequesterID
        {
            get
            {
                return this.m_RequesterID;
            }
            set
            {
                this.m_RequesterID = value;
            }
        }
        #endregion

        #region GroupID
        private System.String m_GroupID = "";
        public System.String GroupID
        {
            get
            {
                return this.m_GroupID;
            }
            set
            {
                this.m_GroupID = value;
            }
        }
        #endregion

        #region AccepterID
        private System.String m_AccepterID = "";
        public System.String AccepterID
        {
            get
            {
                return this.m_AccepterID;
            }
            set
            {
                this.m_AccepterID = value;
            }
        }
        #endregion

        #region Comment
        private System.String m_Comment2 = "";
        public System.String Comment2
        {
            get
            {
                return this.m_Comment2;
            }
            set
            {
                this.m_Comment2 = value;
            }
        }
        #endregion

        #region State
        private int m_State = 0;
        /// <summary>
        /// State 0:请求中 1：同意  2：拒绝
        /// </summary>
        public int State
        {
            get
            {
                return this.m_State;
            }
            set
            {
                this.m_State = value;
            }
        }
        #endregion

        #region Notified
        private System.Boolean m_Notified = false;
        /// <summary>
        /// Notified 0:申请未通知对方  1：申请已通知对方
        /// </summary>
        public System.Boolean Notified
        {
            get
            {
                return this.m_Notified;
            }
            set
            {
                this.m_Notified = value;
            }
        }
        #endregion

        #region CreateTime
        private System.DateTime m_CreateTime = DateTime.Now;
        public System.DateTime CreateTime
        {
            get
            {
                return this.m_CreateTime;
            }
            set
            {
                this.m_CreateTime = value;
            }
        }
        #endregion
        #endregion

        #region IEntity Members
        public System.Int32 GetPKeyValue()
        {
            return this.AutoID;
        }
        #endregion
    }


    [Serializable]
    public class AddGroupRequestPage
    {
        public List<AddGroupRequest> AddGroupRequestList { get; set; }

        public int TotalEntityCount { get; set; }
    }
    #endregion

    #region GroupBan
    [Serializable]
    public partial class GroupBan : IEntity<System.Int32>
    {
        #region Force Static Check
        public const string TableName = "GroupBan";
        public const string _AutoID = "AutoID";
        public const string _GroupID = "GroupID";
        public const string _OperatorID = "OperatorID";
        public const string _UserID = "UserID";
        public const string _Comment2 = "Comment2";
        public const string _EnableTime = "EnableTime";
        public const string _CreateTime = "CreateTime";
        #endregion

        #region Property

        #region AutoID
        private System.Int32 m_AutoID = 0;
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 AutoID
        {
            get
            {
                return this.m_AutoID;
            }
            set
            {
                this.m_AutoID = value;
            }
        }
        #endregion

        #region GroupID
        private System.String m_GroupID = "";
        /// <summary>
        /// GroupID 群组ID
        /// </summary>
        public System.String GroupID
        {
            get
            {
                return this.m_GroupID;
            }
            set
            {
                this.m_GroupID = value;
            }
        }
        #endregion

        #region OperatorID
        private System.String m_OperatorID = "";
        /// <summary>
        /// OperatorID 操作人ID
        /// </summary>
        public System.String OperatorID
        {
            get
            {
                return this.m_OperatorID;
            }
            set
            {
                this.m_OperatorID = value;
            }
        }
        #endregion

        #region UserID
        private System.String m_UserID = "";
        /// <summary>
        /// UserID 被禁用者ID
        /// </summary>
        public System.String UserID
        {
            get
            {
                return this.m_UserID;
            }
            set
            {
                this.m_UserID = value;
            }
        }
        #endregion

        #region Comment
        private System.String m_Comment2 = "";
        /// <summary>
        /// Comment 备注
        /// </summary>
        public System.String Comment2
        {
            get
            {
                return this.m_Comment2;
            }
            set
            {
                this.m_Comment2 = value;
            }
        }
        #endregion

        #region EnableTime
        private System.DateTime m_EnableTime = DateTime.Now;
        /// <summary>
        /// EnableTime 截至时间
        /// </summary>
        public System.DateTime EnableTime
        {
            get
            {
                return this.m_EnableTime;
            }
            set
            {
                this.m_EnableTime = value;
            }
        }
        #endregion

        #region CreateTime
        private System.DateTime m_CreateTime = DateTime.Now;
        /// <summary>
        /// CreateTime 创建时间
        /// </summary>
        public System.DateTime CreateTime
        {
            get
            {
                return this.m_CreateTime;
            }
            set
            {
                this.m_CreateTime = value;
            }
        }
        #endregion
        #endregion

        #region IEntity Members
        public System.Int32 GetPKeyValue()
        {
            return this.AutoID;
        }
        #endregion
    }
    #endregion
}
