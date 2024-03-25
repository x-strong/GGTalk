using System;
using System.Collections.Generic;
using System.Text;

namespace ESFramework.Boost.Server
{
    /// <summary>
    /// 将多个组管理器组合成一个。（返回第一个非空集合。）
    /// </summary>
    public class ComplexGroupManager : IGroupManager
    {
        private List<IGroupManager> managerList = new List<IGroupManager>();
        public ComplexGroupManager(params IGroupManager[] managers)
        {
            if (managers != null && managers.Length > 0)
            {
                this.managerList = new List<IGroupManager>(managers);
            }
        }

        public List<string> GetGroupMembers(string groupID)
        {
            foreach (IGroupManager mgr in this.managerList)
            {
                List<string> members = mgr.GetGroupMembers(groupID);
                if (members != null && members.Count > 0)
                {
                    return members;
                }
            }

            return new List<string>();
        }

        public List<string> GetGroupmates(string userID)
        {           
            foreach (IGroupManager mgr in this.managerList)
            {
                List<string> members = mgr.GetGroupmates(userID);
                if (members != null && members.Count > 0)
                {
                    return members;
                }
            }

            return new List<string>();
        }
    }
}
