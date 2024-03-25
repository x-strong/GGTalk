using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.ObjectManagement.Managers;
using ESFramework;

namespace TalkBase.Server.Application
{
    /// <summary>
    /// 用于在内存中存储离线消息和离线文件条目。
    /// </summary>
    public class OfflineMemoryCache
    {
        private ObjectManager<string, List<OfflineMessage>> offlineMessageTable = new ObjectManager<string, List<OfflineMessage>>();//key:用户ID 。 
        private ObjectManager<string, List<OfflineFileItem>> offlineFileTable = new ObjectManager<string, List<OfflineFileItem>>();//key:用户ID 。 
        private ObjectManager<string, List<OfflineFileItem>> offlineFile4AssistantTable = new ObjectManager<string, List<OfflineFileItem>>();//key:用户ID 。 

        #region OfflineMessage
        /// <summary>
        /// 存储离线消息。
        /// </summary>       
        /// <param name="msg">要存储的离线消息</param>
        public void StoreOfflineMessage(OfflineMessage msg)
        {
            if (!this.offlineMessageTable.Contains(msg.DestUserID))
            {
                this.offlineMessageTable.Add(msg.DestUserID, new List<OfflineMessage>());
            }

            this.offlineMessageTable.Get(msg.DestUserID).Add(msg);
        }

        /// <summary>
        /// 提取目标用户的所有离线消息。
        /// </summary>       
        /// <param name="destUserID">接收离线消息用户的ID</param>
        /// <returns>属于目标用户的离线消息列表，按时间升序排列</returns>
        public List<OfflineMessage> PickupOfflineMessage(string destUserID)
        {
            if (!this.offlineMessageTable.Contains(destUserID))
            {
                return new List<OfflineMessage>();
            }

            List<OfflineMessage> list = this.offlineMessageTable.Get(destUserID);
            this.offlineMessageTable.Remove(destUserID);
            return list;
        }
        #endregion

        #region OfflineFile
        /// <summary>
        /// 将一个离线文件条目保存到数据库中。
        /// </summary>     
        public void StoreOfflineFileItem(OfflineFileItem item)
        {
            if (item.SenderID == item.AccepterID) //多端助手
            {
                if (!this.offlineFile4AssistantTable.Contains(item.AccepterID))
                {
                    this.offlineFile4AssistantTable.Add(item.AccepterID, new List<OfflineFileItem>());
                }

                this.offlineFile4AssistantTable.Get(item.AccepterID).Add(item);
            }
            else
            {
                if (!this.offlineFileTable.Contains(item.AccepterID))
                {
                    this.offlineFileTable.Add(item.AccepterID, new List<OfflineFileItem>());
                }

                this.offlineFileTable.Get(item.AccepterID).Add(item);
            }
        }

        /// <summary>
        /// 从数据库中提取接收者为指定用户的所有离线文件条目。
        /// </summary>       
        public List<OfflineFileItem> PickupOfflineFileItem(string accepterID)
        {
            if (!this.offlineFileTable.Contains(accepterID))
            {
                return new List<OfflineFileItem>();
            }

            List<OfflineFileItem> list = this.offlineFileTable.Get(accepterID);
            this.offlineFileTable.Remove(accepterID);
            return list;
        }

        public List<OfflineFileItem> PickupOfflineFileItem4Assistant(string accepterID, ClientType type)
        {
            if (!this.offlineFile4AssistantTable.Contains(accepterID))
            {
                return new List<OfflineFileItem>();
            }

            List<OfflineFileItem> result = new List<OfflineFileItem>();
            List<OfflineFileItem> list = this.offlineFile4AssistantTable.Get(accepterID);
            foreach(OfflineFileItem item in list.ToArray())
            {
                if(item.AccepterType == type)
                {
                    result.Add(item);
                    list.Remove(item);
                }
            }

            if(list.Count == 0)
            {
                this.offlineFile4AssistantTable.Remove(accepterID);
            }
           
            return result;
        }
        #endregion
    }
}
