using System;
using System.Collections.Generic;
using System.Text;
using ESFramework;

namespace ESFramework.Boost.DynamicGroup
{
    public class BroadcastContract
    {
        #region Ctor
        public BroadcastContract() { }
        public BroadcastContract(string _broadcasterID, string _groupID, int infoType ,byte[] info ,ActionTypeOnChannelIsBusy action)
        {
            this.broadcasterID = _broadcasterID;
            this.groupID = _groupID;
            this.content = info;
            this.informationType = infoType;
            this.actionTypeOnChannelIsBusy = action;
        }        
        #endregion

        #region BroadcasterID
        private string broadcasterID = null;
        public string BroadcasterID
        {
            get { return broadcasterID; }
            set { broadcasterID = value; }
        } 
        #endregion

        #region GroupID
        private string groupID = "";
        public string GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        } 
        #endregion

        #region InformationType
        private int informationType = 0;
        /// <summary>
        /// 当MessageType为CustomizeInfoMessageTypeRoom.Response时，忽略该属性。
        /// </summary>
        public int InformationType
        {
            get { return informationType; }
            set { informationType = value; }
        } 
        #endregion

        #region Content
        private byte[] content;
        public byte[] Content
        {
            get { return content; }
            set { content = value; }
        }
        #endregion

        #region ActionTypeOnChannelIsBusy
        private ActionTypeOnChannelIsBusy actionTypeOnChannelIsBusy = ActionTypeOnChannelIsBusy.Continue;
        public ActionTypeOnChannelIsBusy ActionTypeOnChannelIsBusy
        {
            get { return actionTypeOnChannelIsBusy; }
            set { actionTypeOnChannelIsBusy = value; }
        } 
        #endregion
    }

    public class GroupContract
    {
        public GroupContract() { }
        public GroupContract(string _groupID)
        {
            this.groupID = _groupID;
        }

        #region GroupID
        private string groupID;
        public string GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }
        #endregion
    }

    /// <summary>
    /// 服务器给客户端的通知。
    /// </summary>	
    public class GroupNotifyContract
    {
        public GroupNotifyContract() { }

        public GroupNotifyContract(string grID)
        {
        }

        public GroupNotifyContract(string grID, string userID)
        {
            this.groupID = grID;
            this.memberID = userID;
        }

        #region GroupID
        private string groupID = "";
        public string GroupID
        {
            get
            {
                return this.groupID;
            }
            set
            {
                this.groupID = value;
            }
        }
        #endregion

        #region MemberID
        private string memberID = "";
        public string MemberID
        {
            get
            {
                return this.memberID;
            }
            set
            {
                this.memberID = value;
            }
        }
        #endregion
    }

    public class GroupResultContract
    {
        public GroupResultContract() { }
        public GroupResultContract(int _result)
        {
            this.result = _result;
        }

        #region Result
        private int result = 0;
        public int Result
        {
            get { return result; }
            set { result = value; }
        }
        #endregion
    }

    public class P2PChannelReportContract
    {
        public P2PChannelReportContract() { }
        public P2PChannelReportContract(string dest)
        {
            this.destUserID = dest;
        }

        #region DestUserID
        private string destUserID;
        public string DestUserID
        {
            get { return destUserID; }
            set { destUserID = value; }
        }
        #endregion
    }

    public class RecruitOrFireContract
    {
        public RecruitOrFireContract() { }
        public RecruitOrFireContract(string _groupID, string _memberID)
        {
            this.groupID = _groupID;
            this.memberID = _memberID;
        }

        #region GroupID
        private string groupID;
        public string GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }
        #endregion

        #region MemberID
        private string memberID;
        public string MemberID
        {
            get { return memberID; }
            set { memberID = value; }
        }
        #endregion
    }

    public class UserContract
    {
        public UserContract() { }
        public UserContract(string _userID)
        {
            this.userID = _userID;
        }

        #region UserID
        private string userID;
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        #endregion
    }
}
