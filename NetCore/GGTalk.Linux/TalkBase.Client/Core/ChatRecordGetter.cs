using ESPlus.Rapid;
using ESPlus.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TalkBase.Client.Core
{
    class ChatRecordGetter: IChatRecordGetter
    {
        private IRapidPassiveEngine rapidPassiveEngine;
        private TalkBaseInfoTypes talkBaseInfoTypes;
        public ChatRecordGetter(IRapidPassiveEngine engine, TalkBaseInfoTypes infoTypes)
        {
            this.rapidPassiveEngine = engine;
            this.talkBaseInfoTypes = infoTypes;
        }

        public ChatMessageRecord GetChatMessageRecord(int id)
        {
            byte[] data= this.rapidPassiveEngine.CustomizeOutter.QueryBlob(this.talkBaseInfoTypes.GetChatMessageRecord, BitConverter.GetBytes(id));
            return CompactPropertySerializer.Default.Deserialize<ChatMessageRecord>(data, 0);
        }

        public ChatRecordPage GetChatRecordPage(ChatRecordTimeScope timeScope, string myID, string friendID, int pageSize, int pageIndex)
        {
            GetChatRecordPageContract contract = new GetChatRecordPageContract(timeScope, myID, friendID, pageSize, pageIndex);
            byte[] dd = CompactPropertySerializer.Default.Serialize(contract);
            byte[] data = this.rapidPassiveEngine.CustomizeOutter.QueryBlob(this.talkBaseInfoTypes.GetChatRecordPage, dd);
            return CompactPropertySerializer.Default.Deserialize<ChatRecordPage>(data, 0);
        }

        public ChatRecordPage GetGroupChatRecordPage(ChatRecordTimeScope timeScope, string groupID, int pageSize, int pageIndex)
        {
            GetGroupChatRecordPageContract contract = new GetGroupChatRecordPageContract(timeScope, groupID, pageSize, pageIndex);
            byte[] data = this.rapidPassiveEngine.CustomizeOutter.QueryBlob(this.talkBaseInfoTypes.GetGroupChatRecordPage, CompactPropertySerializer.Default.Serialize(contract));
            return CompactPropertySerializer.Default.Deserialize<ChatRecordPage>(data, 0);
        }
    }
}
