using System;
using System.Collections.Generic;
using System.Text;
using ESFramework.Server.UserManagement;
using ESBasic.Collections;

namespace ESFramework.Boost.DynamicGroup.Server
{
    /// <summary>
    /// 记录当前AS上的用户已建立的P2P连接。
    /// </summary>
    internal class P2PChannelManager
    {
        //key 表示P2P通道的起始点用户ID，value 表示P2P通道的目的点用户列表。（单向，因为某些P2P通道就是单向的）
        private SortedArray<string, SortedArray<string>> channels = new SortedArray<string, SortedArray<string>>();

        public void Initialize(IUserManager userManager)
        {
            userManager.UserDisconnected += new ESBasic.CbGeneric<string>(userManager_SomeOneDisconnected);
        }

        void userManager_SomeOneDisconnected(string userID)
        {
            this.channels.RemoveByKey(userID);
        }

        public void Register(string startUserID, string destUserID)
        {
            if (!this.channels.ContainsKey(startUserID))
            {
                this.channels.Add(startUserID, new SortedArray<string>());
            }

            this.channels[startUserID].Add(destUserID);
        }

        public void Unregister(string startUserID, string destUserID)
        {
            if (this.channels.ContainsKey(startUserID))
            {
                this.channels[startUserID].Remove(destUserID);
            }
        }

        public bool IsP2PChannelExist(string startUserID, string destUserID)
        {
            if (!this.channels.ContainsKey(startUserID))
            {
                return false;
            }

            return this.channels[startUserID].Contains(destUserID);
        }

        public string GetState()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string userID in this.channels.GetAllKeyList())
            {
                sb.Append(userID);
                sb.Append(" --  【");
                foreach (string target in this.channels[userID].GetAll())
                {
                    sb.Append(target);
                    sb.Append(" , ");
                }               
                sb.Append("】\n");
            }

            return sb.ToString();
        }
    }
}
