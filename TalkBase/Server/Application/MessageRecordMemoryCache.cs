using System;
using System.Collections.Generic;
using System.Text;
using ESBasic;
using ESBasic.ObjectManagement.Managers;


namespace TalkBase.Server.Application
{
    /// <summary>
    /// 用于在内存中存储聊天记录。
    /// </summary>
    public class MessageRecordMemoryCache
    {
        private ObjectManager<string, ObjectManager<string, List<ChatMessageRecord>>> chatRecordTable = new ObjectManager<string, ObjectManager<string, List<ChatMessageRecord>>>();//ownerID - guestID - msgRtf。 
        private ObjectManager<string, List<ChatMessageRecord>> groupChatRecordTable = new ObjectManager<string, List<ChatMessageRecord>>();//groupID - guestID - msgRtf。 
     
        private int autoID = 0;
        private object locker=new object();
        #region InsertChatMessageRecord
        public int InsertChatMessageRecord(ChatMessageRecord chatMessage)
        {
            lock (locker)
            {
                autoID++;
                chatMessage.AutoID = autoID;
                if (!chatMessage.IsGroupChat)
                {
                    //owner 为Sender
                    if (!this.chatRecordTable.Contains(chatMessage.SpeakerID))
                    {
                        this.chatRecordTable.Add(chatMessage.SpeakerID, new ObjectManager<string, List<ChatMessageRecord>>());
                    }
                    ObjectManager<string, List<ChatMessageRecord>> guests = this.chatRecordTable.Get(chatMessage.SpeakerID);
                    if (!guests.Contains(chatMessage.AudienceID))
                    {
                        guests.Add(chatMessage.AudienceID, new List<ChatMessageRecord>());
                    }
                    List<ChatMessageRecord> records = guests.Get(chatMessage.AudienceID);
                    records.Add(chatMessage);

                    //owner 为chatMessage.AudienceID
                    if (!this.chatRecordTable.Contains(chatMessage.AudienceID))
                    {
                        this.chatRecordTable.Add(chatMessage.AudienceID, new ObjectManager<string, List<ChatMessageRecord>>());
                    }
                    ObjectManager<string, List<ChatMessageRecord>> guests2 = this.chatRecordTable.Get(chatMessage.AudienceID);
                    if (!guests2.Contains(chatMessage.SpeakerID))
                    {
                        guests2.Add(chatMessage.SpeakerID, new List<ChatMessageRecord>());
                    }
                    List<ChatMessageRecord> records2 = guests2.Get(chatMessage.SpeakerID);
                    records2.Add(chatMessage);
                }
                else
                {
                    if (!this.groupChatRecordTable.Contains(chatMessage.AudienceID))
                    {
                        this.groupChatRecordTable.Add(chatMessage.AudienceID, new List<ChatMessageRecord>());
                    }
                    List<ChatMessageRecord> records = this.groupChatRecordTable.Get(chatMessage.AudienceID);
                    records.Add(chatMessage);
                }
                return autoID;
            }
        }
        #endregion        

        public ChatMessageRecord GetChatMessageRecord(int id)
        {
            foreach (ObjectManager<string, List<ChatMessageRecord>> dic in this.chatRecordTable.GetAll())
            {
                foreach (List<ChatMessageRecord> list in dic.GetAll())
                {
                    foreach (ChatMessageRecord item in list)
                    {
                        if (item.AutoID == id)
                        {
                            return item;
                        }
                    }

                }
            }
            foreach (List<ChatMessageRecord> list in this.groupChatRecordTable.GetAll())
            {
                foreach (ChatMessageRecord item in list)
                {
                    if (item.AutoID == id)
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        #region GetGroupChatRecordPage
        public ChatRecordPage GetGroupChatRecordPage(ChatRecordTimeScope timeScope, string groupID, int pageSize, int pageIndex)
        {
            int totalCount = 0;
            if (pageSize <= 0 || pageIndex < 0)
            {
                return new ChatRecordPage(totalCount, pageIndex, new List<ChatMessageRecord>());
            }

            if (!this.groupChatRecordTable.Contains(groupID))
            {
                return new ChatRecordPage(totalCount, pageIndex, new List<ChatMessageRecord>());
            }

            List<ChatMessageRecord> records = this.groupChatRecordTable.Get(groupID);
            totalCount = records.Count;
            int pageCount = records.Count / pageSize;
            if (records.Count % pageSize > 0)
            {
                ++pageCount;
            }
            if (pageIndex == int.MaxValue)
            {
                pageIndex = pageCount - 1;
            }
            if (pageIndex >= pageCount)
            {
                return new ChatRecordPage(totalCount, pageIndex, new List<ChatMessageRecord>());
            }

            List<ChatMessageRecord> page = new List<ChatMessageRecord>();
            for (int i = pageIndex * pageSize; i < records.Count && page.Count <= pageSize; i++)
            {
                page.Add(records[i]);
            }

            return new ChatRecordPage(totalCount, pageIndex, page); ;
        }

        /// <summary>
        /// 获取指定时间后的群聊天记录
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public List<ChatMessageRecord> GetChatRecordList(string groupID, DateTime startTime)
        {
            if (startTime > DateTime.Now)
            {
                return new List<ChatMessageRecord>();
            }
            if (!this.groupChatRecordTable.Contains(groupID))
            {
                return new List<ChatMessageRecord>();
            }            
            List<ChatMessageRecord> messageRecords = new List<ChatMessageRecord>();
            List<ChatMessageRecord> records = this.groupChatRecordTable.Get(groupID);
            foreach (ChatMessageRecord item in records)
            {
                if (item.OccureTime > startTime)
                {
                    messageRecords.Add(item);
                }
            }
            messageRecords.Sort((x, y) => { return x.AutoID.CompareTo(y.AutoID); });
            return messageRecords;
        }
        #endregion

        #region GetChatRecordPage
        public ChatRecordPage GetChatRecordPage(ChatRecordTimeScope timeScope, string senderID, string accepterID, int pageSize, int pageIndex)
        {
            int totalCount = 0;
            if (pageSize <= 0 || pageIndex < 0)
            {
                return new ChatRecordPage(totalCount, pageIndex, new List<ChatMessageRecord>());
            }

            if (!this.chatRecordTable.Contains(senderID))
            {
                return new ChatRecordPage(totalCount, pageIndex, new List<ChatMessageRecord>());
            }

            ObjectManager<string, List<ChatMessageRecord>> friends = this.chatRecordTable.Get(senderID);
            if (!friends.Contains(accepterID))
            {
                return new ChatRecordPage(totalCount, pageIndex, new List<ChatMessageRecord>());
            }

            List<ChatMessageRecord> records = friends.Get(accepterID);
            totalCount = records.Count;
            int pageCount = records.Count / pageSize;
            if (records.Count % pageSize > 0)
            {
                ++pageCount;
            }
            if (pageIndex == int.MaxValue)
            {
                pageIndex = pageCount - 1;
            }
            if (pageIndex >= pageCount)
            {
                return new ChatRecordPage(totalCount, pageIndex, new List<ChatMessageRecord>());
            }

            List<ChatMessageRecord> page = new List<ChatMessageRecord>();
            for (int i = pageIndex * pageSize; i < records.Count && page.Count <= pageSize; i++)
            {
                page.Add(records[i]);
            }

            return new ChatRecordPage(totalCount, pageIndex, page);
        }
        #endregion
        

        public void Clear()
        {
            this.chatRecordTable.Clear();
            this.groupChatRecordTable.Clear();
        }
    }
}
