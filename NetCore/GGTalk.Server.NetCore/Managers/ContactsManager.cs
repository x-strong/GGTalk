using System;
using System.Collections.Generic;
using System.Text;
using TalkBase.Server;


namespace GGTalk.Server
{
    /// <summary>
    /// 组管理器。
    /// </summary>
    internal class ContactsManager : ESPlus.Application.Contacts.Server.IContactsManager
    {
        private ServerGlobalCache<GGUser, GGGroup> globalCache;
        public ContactsManager(ServerGlobalCache<GGUser, GGGroup> db)
        {
            this.globalCache = db;
        }

        public List<string> GetGroupMemberList(string groupID)
        {
            GGGroup group =  this.globalCache.GetGroup(groupID);
            if (group == null)
            {
                return new List<string>();
            }

            return group.MemberList;
        }       

        public List<string> GetContacts(string userID)
        {
            return this.globalCache.GetAllContactsNecessary(userID); //上下线仅通知关注的联系人。
        }

        public void OnUserOffline(string userID)
        {
            
        }
    }
}
