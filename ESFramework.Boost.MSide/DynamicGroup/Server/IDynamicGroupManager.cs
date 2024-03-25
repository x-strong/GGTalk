using System;
using System.Collections.Generic;
using ESBasic;

namespace ESFramework.Boost.DynamicGroup.Server
{
    public interface IDynamicGroupManager 
    {
        /// <summary>
        /// 当新成员加入组时，触发该事件。参数：GroupID - MemberID
        /// </summary>
        event CbGeneric<string, string> SomeoneJoinGroup;

        /// <summary>
        /// 当成员退出组时，触发该事件。参数：GroupID - MemberID
        /// </summary>
        event CbGeneric<string, string> SomeoneQuitGroup;

        /// <summary>
        /// 当用户掉线，从所有组中退出时，触发该事件。参数：userID - groupmate。将不再触发SomeoneQuitGroup事件。
        /// </summary>
        event CbGeneric<string, List<string>> GroupmateOffline;

        ///// <summary>
        ///// 获取用户的所有组友。
        ///// </summary>       
        List<string> GetGroupmates(string userID);

        /// <summary>
        /// 获取目标组的所有成员。
        /// </summary>        
        List<string> GetGroupMembers(string groupID);
        
        /// <summary>
        /// 将用户加入目标组。
        /// </summary>        
        void JoinGroup(string groupID, string userID);

        /// <summary>
        /// 将用户从目标组中移除。
        /// </summary>        
        void QuitGroup(string groupID, string userID);

        void DestroyGroup(string destroierID, string groupID);

        /// <summary>
        /// 将用户从所有组中移除。
        /// </summary>        
        void Offline(string userID);
    }
}
