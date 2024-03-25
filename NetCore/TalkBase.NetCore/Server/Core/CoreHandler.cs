using System;
using System.Collections.Generic;
using System.Text;
using ESPlus.Serialization;
using ESPlus.Rapid;
using ESFramework;

namespace TalkBase.Server
{
    /// <summary>
    /// 用于处理来自ServerHanler与GlobalService（第三方系统）的公共调用部分。
    /// </summary>   
    internal class CoreHandler<TUser, TGroup>
        where TUser : TalkBase.IUser
        where TGroup : TalkBase.IGroup
    {
        private ServerGlobalCache<TUser, TGroup> serverGlobalCache;
        private IRapidServerEngine rapidServerEngine;
        private TalkBaseInfoTypes talkBaseInfoTypes;
        public CoreHandler(ServerGlobalCache<TUser, TGroup> cache, IRapidServerEngine engine, TalkBaseInfoTypes types)
        {
            this.serverGlobalCache = cache;
            this.rapidServerEngine = engine;
            this.talkBaseInfoTypes = types;
        }

        #region SendSystemMessage

        public void DoSendSystemMessage(string sourceUserID, ClientType sourceType, SystemMessageContract contract, byte[] contractData)
        {
            if (contract.ReceiverUnitID == null) //接收者是所有人
            {
                List<string> userList = this.serverGlobalCache.GetAllUserID();
                foreach (string userID in userList)
                {
                    if (this.rapidServerEngine.UserManager.IsUserOnLine(userID))
                    {
                        this.rapidServerEngine.SendMessage(userID, this.talkBaseInfoTypes.SystemMessage, contractData, null, 2048);
                    }
                    else
                    {
                        if (contract.UseOfflineMessageMode)
                        {
                            OfflineMessage msg = new OfflineMessage(sourceUserID, sourceType, userID, null, this.talkBaseInfoTypes.SystemMessage, contractData );
                            this.serverGlobalCache.StoreOfflineMessage(msg);
                        }
                    }
                }
                return;
            }

            TalkBase.IUser user = this.serverGlobalCache.GetUser(contract.ReceiverUnitID); //接收者是单个人
            if (user != null)
            {
                if (this.rapidServerEngine.UserManager.IsUserOnLine(user.ID))
                {
                    this.rapidServerEngine.SendMessage(user.ID, this.talkBaseInfoTypes.SystemMessage, contractData, null, 2048);
                }
                else
                {
                    if (contract.UseOfflineMessageMode)
                    {
                        OfflineMessage msg = new OfflineMessage(sourceUserID, sourceType, user.ID, null, this.talkBaseInfoTypes.SystemMessage, contractData);
                        this.serverGlobalCache.StoreOfflineMessage(msg);
                    }
                }
                return;
            }

            TalkBase.IGroup group = this.serverGlobalCache.GetGroup(contract.ReceiverUnitID); //接收者为群组
            if (group != null)
            {
                foreach (string userID in group.MemberList)
                {
                    if (this.rapidServerEngine.UserManager.IsUserOnLine(userID))
                    {
                        this.rapidServerEngine.SendMessage(userID, this.talkBaseInfoTypes.SystemMessage, contractData, null, 2048);
                    }
                    else
                    {
                        if (contract.UseOfflineMessageMode)
                        {
                            OfflineMessage msg = new OfflineMessage(sourceUserID, sourceType, userID, null, this.talkBaseInfoTypes.SystemMessage, contractData);
                            this.serverGlobalCache.StoreOfflineMessage(msg);
                        }
                    }
                }
                return;
            }
        }

        public void SendSystemMessage(string sourceUserID, string receiverUnitID, int sysMsgType, byte[] msgContent, string tag , bool useOfflineMessageMode)
        {
            SystemMessageContract contract = new SystemMessageContract(sourceUserID, receiverUnitID, sysMsgType, msgContent, tag , useOfflineMessageMode);
            byte[] data = CompactPropertySerializer.Default.Serialize(contract);
            this.DoSendSystemMessage(sourceUserID, ClientType.ServerSide, contract, data);
        } 
        #endregion

    }
}
