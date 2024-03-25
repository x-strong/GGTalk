using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.ObjectManagement.Managers;
using ESBasic.Collections;
using ESBasic;

namespace ESFramework.Boost.DynamicGroup.Server
{
    /// <summary>
    /// 动态组管理器。
    /// （1）所有的组员都是在线的。只要组员掉线，则将其从组中移除。
    /// （2）如果要加入的目标组不存在，则自动创建目标组，并加入。
    /// （3）当某个组中不存在任何成员时（比如都退出），则销毁组。
    /// </summary>
    public class DynamicGroupManager : IDynamicGroupManager
    {
        private object locker = new object() ;
        private ObjectManager<string, SortedArray<string>> manager = new ObjectManager<string, SortedArray<string>>();        

        public event CbGeneric<string, string> SomeoneJoinGroup; //参数：GroupID - MemberID
        public event CbGeneric<string, string> SomeoneQuitGroup;
        public event CbGeneric<string, List<string>> GroupmateOffline;
      
        public void JoinGroup(string groupID ,string userID)
        {
            lock (this.locker)
            {
                SortedArray<string> group = this.manager.Get(groupID);
                if (group == null)
                {
                    this.manager.Add(groupID, new SortedArray<string>());
                    group = this.manager.Get(groupID);
                }
                group.Add(userID);
            }

            if (this.SomeoneJoinGroup != null)
            {
                this.SomeoneJoinGroup(groupID, userID);
            }           
        }

        public void DestroyGroup(string destroierID, string groupID)
        {
            this.manager.Remove(groupID);     
        }

        public void QuitGroup(string groupID, string userID)
        {
            lock (this.locker)
            {
                SortedArray<string> group = this.manager.Get(groupID);
                if (group == null)
                {
                    return;
                }

                group.Remove(userID);
            }

            if (this.SomeoneQuitGroup != null)
            {
                this.SomeoneQuitGroup(groupID, userID);
            }
        }

        public void Offline(string userID)
        {
            SortedArray<string> array = new SortedArray<string>();
            lock (this.locker)
            {
                foreach (string groupID in this.manager.GetKeyList())
                {
                    SortedArray<string> group = this.manager.Get(groupID);
                    if (group.Contains(userID))
                    {
                        array.Add(group.GetAll());
                        this.QuitGroup(groupID, userID); //2015.09.28
                        //group.Remove(userID);
                    }
                }
            }

            if (this.GroupmateOffline != null)
            {
                this.GroupmateOffline(userID, array.GetAll());
            }
        }

        public List<string> GetGroupmates(string userID)
        {
            SortedArray<string> array = new SortedArray<string>();
            foreach (SortedArray<string> group in this.manager.GetAll())
            {
                if (group.Contains(userID))
                {
                    array.Add(group.GetAll());
                }
            }
            return array.GetAll();
        }

        /// <summary>
        /// 返回的集合不可修改。
        /// </summary>        
        public List<string> GetGroupMembers(string groupID)
        {
            SortedArray<string> group = this.manager.Get(groupID);
            if (group == null)
            {
                return null;
            }

            return group.GetAllReadonly(); 
        }
    }    
}
