using System;
using System.Collections.Generic;
using System.Text;
using ESPlus.Rapid;
using ESPlus.Serialization;
using ESBasic.Loggers;

namespace TalkBase.Client
{
    /// <summary>
    /// 客户端全局缓存，在内存中缓存用户、群组。
    /// </summary>  
    internal class ClientGlobalCache<TUser, TGroup> : BaseGlobalCache<TUser, TGroup>
        where TUser :class, TalkBase.IUser, new()
        where TGroup : TalkBase.IGroup, new()
       
    {
        #region protected
        private IRapidPassiveEngine rapidPassiveEngine;
        private ITalkBaseHelper<TGroup> talkBaseHelper;
        private TalkBaseInfoTypes talkBaseInfoTypes;

        public ClientGlobalCache(IRapidPassiveEngine engine, ITalkBaseHelper<TGroup> helper, TalkBaseInfoTypes infoTypes, string persistenceFilePath, IAgileLogger logger)
        {
            this.rapidPassiveEngine = engine;
            this.talkBaseHelper = helper;
            this.talkBaseInfoTypes = infoTypes;
            this.Initialize(this.rapidPassiveEngine.CurrentUserID, persistenceFilePath, helper, logger);
        }

        protected override TUser DoGetUser(string userID)
        {
            byte[] bUser = this.rapidPassiveEngine.CustomizeOutter.Query(this.talkBaseInfoTypes.GetUserInfo, Encoding.UTF8.GetBytes(userID));
            if (bUser == null)
            {
                return null;
            }
            return ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<TUser>(bUser, 0);
        }

        protected override TGroup DoGetGroup(string groupID)
        {
            byte[] bGroup = this.rapidPassiveEngine.CustomizeOutter.Query(this.talkBaseInfoTypes.GetGroup, System.Text.Encoding.UTF8.GetBytes(groupID));
            return CompactPropertySerializer.Default.Deserialize<TGroup>(bGroup, 0);
        }
        protected override List<TGroup> DoGetMyGroups()
        {
            byte[] bMyGroups = this.rapidPassiveEngine.CustomizeOutter.Query(this.talkBaseInfoTypes.GetMyGroups, null);
            return CompactPropertySerializer.Default.Deserialize<List<TGroup>>(bMyGroups, 0);
        }
        protected override List<TGroup> DoGetSomeGroups(List<string> groupIDList)
        {
            byte[] bMyGroups = this.rapidPassiveEngine.CustomizeOutter.Query(this.talkBaseInfoTypes.GetSomeGroups, CompactPropertySerializer.Default.Serialize(groupIDList));
            return CompactPropertySerializer.Default.Deserialize<List<TGroup>>(bMyGroups, 0);
        }
        protected override ContactRTDatas DoGetContactsRTDatas()
        {
            byte[] res = this.rapidPassiveEngine.CustomizeOutter.QueryBlob(this.talkBaseInfoTypes.GetContactsRTData, null);
            return ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<ContactsRTDataContract>(res, 0);
        }
        protected override List<TUser> DoGetSomeUsers(List<string> userIDList)
        {
            byte[] bFriends = this.rapidPassiveEngine.CustomizeOutter.QueryBlob(this.talkBaseInfoTypes.GetSomeUsers, CompactPropertySerializer.Default.Serialize(userIDList));
            return CompactPropertySerializer.Default.Deserialize<List<TUser>>(bFriends, 0);
        }

        protected override List<TUser> DoGetAllContacts() //好友，包括组友 
        {
            byte[] bFriends = this.rapidPassiveEngine.CustomizeOutter.QueryBlob(this.talkBaseInfoTypes.GetAllContacts, null);
            return CompactPropertySerializer.Default.Deserialize<List<TUser>>(bFriends, 0);
        }

        protected override List<string> DoGetAllContactIDs()
        {
            byte[] bFriends = this.rapidPassiveEngine.CustomizeOutter.Query(this.talkBaseInfoTypes.GetAllContactIDs, null);
            return CompactPropertySerializer.Default.Deserialize<List<string>>(bFriends, 0);
        } 
        #endregion        
    }    
}
