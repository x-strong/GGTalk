using System;
using ESBasic;
using DataRabbit.DBAccessing.Application;
using DataRabbit.DBAccessing.ORM;
using DataRabbit;
using System.Collections.Generic;
using System.IO;
using DataRabbit.DBAccessing;
using DataRabbit.DBAccessing.Relation;
using SqlSugar;

namespace TalkBase
{
    #region ChatMessageRecord
    /// <summary>
    /// 聊天记录Entity。
    /// </summary>
    [Serializable]
    public class ChatMessageRecord 
    {
        #region Force Static Check
        public const string TableName = "ChatMessageRecord";
        public const string _AutoID = "AutoID";
        public const string _SpeakerID = "SpeakerID";
        public const string _AudienceID = "AudienceID";
        public const string _IsGroupChat = "IsGroupChat";
        public const string _Content = "Content";
        public const string _OccureTime = "OccureTime";
        #endregion

        #region IEntity Members
        public System.Int64 GetPKeyValue()
        {
            return this.AutoID;
        }
        #endregion

        public ChatMessageRecord() { }
        public ChatMessageRecord(string speaker, string audience, byte[] _content, bool groupChat)
        {
            this.speakerID = speaker;
            this.audienceID = audience;
            this.Content = _content;
            this.isGroupChat = groupChat;
        }

        #region AutoID
        private long autoID = 0;
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        /// <summary>
        /// 自增ID，编号。
        /// </summary>
        public long AutoID
        {
            get { return autoID; }
            set { autoID = value; }
        }
        #endregion

        #region SpeakerID
        private string speakerID = "";
        /// <summary>
        /// 发言人的ID。
        /// </summary>
        public string SpeakerID
        {
            get { return speakerID; }
            set { speakerID = value; }
        }
        #endregion

        #region AudienceID
        private string audienceID = "";
        /// <summary>
        /// 听众ID，可以为GroupID。
        /// </summary>
        public string AudienceID
        {
            get { return audienceID; }
            set { audienceID = value; }
        }
        #endregion

        #region OccureTime
        private DateTime occureTime = DateTime.Now;
        /// <summary>
        /// 聊天记录发生的时间。
        /// </summary>
        public DateTime OccureTime
        {
            get { return occureTime; }
            set { occureTime = value; }
        }
        #endregion

        #region Content
        private byte[] content;
        /// <summary>
        /// 聊天的内容。
        /// </summary>
        public byte[] Content
        {
            get { return content; }
            set { content = value; }
        }
        #endregion

        #region IsGroupChat
        private bool isGroupChat = false;
        /// <summary>
        /// 是否为群聊记录。
        /// </summary>
        public bool IsGroupChat
        {
            get { return isGroupChat; }
            set { isGroupChat = value; }
        }
        #endregion

    }
    #endregion

    #region ChatRecordPage
    /// <summary>
    /// 聊天记录分页。
    /// </summary>
    [Serializable]
    public class ChatRecordPage
    {
        public ChatRecordPage() { }
        public ChatRecordPage(int total, int index, List<ChatMessageRecord> _page)
        {
            this.totalCount = total;
            this.pageIndex = index;
            this.content = _page;
        }

        #region TotalCount
        private int totalCount = 0;
        public int TotalCount
        {
            get { return totalCount; }
            set { totalCount = value; }
        }
        #endregion

        #region PageIndex
        private int pageIndex = 0;
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }
        #endregion

        #region Content
        private List<ChatMessageRecord> content = new List<ChatMessageRecord>();
        public List<ChatMessageRecord> Content
        {
            get { return content; }
            set { content = value; }
        }
        #endregion
    }
    #endregion

    #region ChatRecordTimeScope
    public enum ChatRecordTimeScope
    {
        RecentWeek = 0,
        RecentMonth,
        Recent3Month,
        All
    }
    #endregion



    #region IChatRecordPersister
    public interface IChatRecordPersister : IChatRecordGetter
    {
        /// <summary>
        /// 插入一条聊天记录（包括群聊天记录）。
        /// </summary>  
        int InsertChatMessageRecord(ChatMessageRecord record);

        /// <summary>
        /// 删除跟好友的聊天记录。
        /// </summary>        
        void DeleteChatRecord(string myID, string friendID);

        /// <summary>
        /// 删除群聊天记录。
        /// </summary>   
        void DeleteGroupChatRecord(string groupID);

        /// <summary>
        /// 删除所有聊天记录。
        /// </summary>   
        void ClearAllChatRecord();

    }

    public interface IChatRecordGetter
    {

        ///// <summary>
        ///// 获取与目标好友的最后一条聊天记录。
        ///// </summary>        
        //ChatMessageRecord GetLastFriendRecord(string myID, string friendID);

        ///// <summary>
        ///// 获取目标组的最后一条聊天记录。
        ///// </summary>  
        //ChatMessageRecord GetLastGroupRecord(string groupID);

         /// <summary>
         /// 获取聊天记录
         /// </summary>
         /// <param name="id"></param>
         /// <returns></returns>
        ChatMessageRecord GetChatMessageRecord(int id);

        /// <summary>
        /// 获取一页与好友的聊天记录。
        /// </summary>
        /// <param name="timeScope">日期范围</param>
        /// <param name="myID">自己的UserID</param>
        /// <param name="friendID">好友的ID</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>      
        /// <returns>聊天记录页</returns>
        ChatRecordPage GetChatRecordPage(ChatRecordTimeScope timeScope, string myID, string friendID, int pageSize, int pageIndex);

        /// <summary>
        /// 获取一页群聊天记录。
        /// </summary>
        /// <param name="timeScope">日期范围</param>
        /// <param name="groupID">群ID</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>     
        /// <returns>聊天记录页</returns>
        ChatRecordPage GetGroupChatRecordPage(ChatRecordTimeScope timeScope, string groupID, int pageSize, int pageIndex);

        /// <summary>
        /// 获取一页与好友的聊天记录。
        /// </summary>
        /// <param name="timeScope">日期范围</param>
        /// <param name="myID">自己的UserID</param>
        /// <param name="friendID">好友的ID</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>      
        /// <returns>聊天记录页</returns>
        ChatRecordPage GetChatRecordPage(DateTimeScope timeScope, string myID, string friendID, int pageSize, int pageIndex);

        /// <summary>
        /// 获取一页群聊天记录。
        /// </summary>
        /// <param name="timeScope">日期范围</param>
        /// <param name="groupID">群ID</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>     
        /// <returns>聊天记录页</returns>
        ChatRecordPage GetGroupChatRecordPage(DateTimeScope timeScope, string groupID, int pageSize, int pageIndex);


    } 
    #endregion

    #region EmptyChatRecordPersister
    public class EmptyChatRecordPersister : IChatRecordPersister
    {
        public int InsertChatMessageRecord(ChatMessageRecord record)
        {
            return 0;
        }

        public ChatRecordPage GetChatRecordPage(ChatRecordTimeScope timeScope, string myID, string friendID, int pageSize, int pageIndex)
        {
            return null;
        }

        public ChatRecordPage GetGroupChatRecordPage(ChatRecordTimeScope timeScope, string groupID, int pageSize, int pageIndex)
        {
            return null;
        }


        public void DeleteChatRecord(string myID, string friendID)
        {
            
        }

        public void DeleteGroupChatRecord(string groupID)
        {
            
        }

        public void ClearAllChatRecord()
        {
           
        }

        public ChatMessageRecord GetChatMessageRecord(int id)
        {
            return null;
        }

        public ChatRecordPage GetChatRecordPage(DateTimeScope timeScope, string myID, string friendID, int pageSize, int pageIndex)
        {
            return null;
        }

        public ChatRecordPage GetGroupChatRecordPage(DateTimeScope timeScope, string groupID, int pageSize, int pageIndex)
        {
            return null;
        }
    } 
    #endregion

    #region DefaultChatMessageRecordPersister
    /// <summary>
    /// 聊天记录持久化器。
    /// </summary>
    public class DefaultChatRecordPersister : IChatRecordPersister
    {
        private TransactionScopeFactory transactionScopeFactory;

        public void Initialize(TransactionScopeFactory fac)
        {
            this.transactionScopeFactory = fac;
        }

        /// <summary>
        /// 插入一条聊天记录。
        /// </summary>      
        public int InsertChatMessageRecord(ChatMessageRecord record)
        {
            if (this.transactionScopeFactory == null)
            {
                return 0;
            }
            object identity = 0;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
                accesser.Insert(record);
                scope.Commit();
            }
            return (int)record.AutoID;
        }

        public ChatMessageRecord GetChatMessageRecord(int id)
        {
            ChatMessageRecord chatMessageRecord = null;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
                chatMessageRecord = accesser.GetOne(id);
                scope.Commit();
            }
            return chatMessageRecord;
        }

        /// <summary>
        /// 获取一页群聊天记录。
        /// </summary>
        /// <param name="groupID">群ID</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>     
        /// <returns>聊天记录页</returns>
        public ChatRecordPage GetGroupChatRecordPage(ChatRecordTimeScope chatRecordTimeScope, string groupID, int pageSize, int pageIndex)
        {
            if (this.transactionScopeFactory == null)
            {
                return new ChatRecordPage(0, 0, new List<ChatMessageRecord>());
            }

            DateTimeScope timeScope = null;
            DateTime now = DateTime.Now;
            if (chatRecordTimeScope == ChatRecordTimeScope.RecentWeek) //一周
            {
                timeScope = new DateTimeScope(now.AddDays(-7), now);
            }
            else if (chatRecordTimeScope == ChatRecordTimeScope.RecentMonth)//一月
            {
                timeScope = new DateTimeScope(now.AddDays(-31), now);
            }
            else if (chatRecordTimeScope == ChatRecordTimeScope.Recent3Month)//三月
            {
                timeScope = new DateTimeScope(now.AddDays(-91), now);
            }
            else //全部
            {
            }
            return this.GetGroupChatRecordPage(timeScope, groupID, pageSize, pageIndex);
 
        }

        public ChatRecordPage GetGroupChatRecordPage(DateTimeScope timeScope, string groupID, int pageSize, int pageIndex)
        {
            List<Filter> filterList = new List<Filter>();
            filterList.Add(new Filter(ChatMessageRecord._AudienceID, groupID));
            filterList.Add(new Filter(ChatMessageRecord._IsGroupChat, true));
            if (timeScope != null)
            {
                filterList.Add(new Filter(ChatMessageRecord._OccureTime, new DateTime[] { timeScope.StartDate, timeScope.EndDate }, ComparisonOperators.BetweenAnd));
            }
            SimpleFilterTree tree = new SimpleFilterTree(filterList);

            //最后一页
            if (pageIndex == int.MaxValue)
            {
                int total = 0;
                using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
                {
                    IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
                    total = (int)accesser.GetCount(tree);
                    scope.Commit();
                }
                int pageCount = total / pageSize;
                if (total % pageSize > 0)
                {
                    pageCount += 1;
                }
                pageIndex = pageCount - 1;
            }
            if (pageIndex == -1) //表示没有记录
            {
                return new ChatRecordPage(0, 0, new List<ChatMessageRecord>());
            }
            int totalCount = 0;
            ChatMessageRecord[] page = null;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
                page = accesser.GetPage(tree, pageSize, pageIndex, ChatMessageRecord._AutoID, false, out totalCount);
                scope.Commit();
            }
            List<ChatMessageRecord> list = new List<ChatMessageRecord>(page);
            if (page != null && page.Length > 0)
            {
                list.Sort((x, y) =>
                {
                    return x.AutoID.CompareTo(y.AutoID);
                });
            }
            return new ChatRecordPage(totalCount, pageIndex, list);
        }

        /// <summary>
        /// 获取一页与好友的聊天记录。
        /// </summary>
        /// <param name="myID">自己的UserID</param>
        /// <param name="friendID">好友的ID</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>      
        /// <returns>聊天记录页</returns>
        public ChatRecordPage GetChatRecordPage(ChatRecordTimeScope chatRecordTimeScope, string myID, string friendID, int pageSize, int pageIndex)
        {
            if (this.transactionScopeFactory == null)
            {
                return new ChatRecordPage(0, 0, new List<ChatMessageRecord>());
            }

            DateTimeScope timeScope = null;
            DateTime now = DateTime.Now;
            if (chatRecordTimeScope == ChatRecordTimeScope.RecentWeek) //一周
            {
                timeScope = new DateTimeScope(now.AddDays(-7), now);
            }
            else if (chatRecordTimeScope == ChatRecordTimeScope.RecentMonth)//一月
            {
                timeScope = new DateTimeScope(now.AddDays(-31), now);
            }
            else if (chatRecordTimeScope == ChatRecordTimeScope.Recent3Month)//三月
            {
                timeScope = new DateTimeScope(now.AddDays(-91), now);
            }
            else //全部
            {
            }
            return GetChatRecordPage(timeScope, myID, friendID, pageSize, pageIndex);
        }


        public ChatRecordPage GetChatRecordPage(DateTimeScope timeScope, string myID, string friendID, int pageSize, int pageIndex)
        {

            IFilterTree tree1 = new SimpleFilterTree(new Filter(ChatMessageRecord._SpeakerID, myID), new Filter(ChatMessageRecord._AudienceID, friendID));
            IFilterTree tree2 = new SimpleFilterTree(new Filter(ChatMessageRecord._SpeakerID, friendID), new Filter(ChatMessageRecord._AudienceID, myID));
            IFilterTree tmp = new ComplexFilterTree(LogicType.Or, tree1, tree2);
            ComplexFilterTree tree = new ComplexFilterTree(LogicType.And, tmp, new Filter(ChatMessageRecord._IsGroupChat, false));
            if (timeScope != null)
            {
                tree = new ComplexFilterTree(LogicType.And, tree, new Filter(ChatMessageRecord._OccureTime, new DateTime[] { timeScope.StartDate, timeScope.EndDate }, ComparisonOperators.BetweenAnd));
            }
            //最后一页
            if (pageIndex == int.MaxValue)
            {
                int total = 0;
                using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
                {
                    IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
                    total = (int)accesser.GetCount(tree);
                    scope.Commit();
                }
                int pageCount = total / pageSize;
                if (total % pageSize > 0)
                {
                    pageCount += 1;
                }
                pageIndex = pageCount - 1;
            }

            if (pageIndex == -1) //表示没有记录
            {
                return new ChatRecordPage(0, 0, new List<ChatMessageRecord>());
            }

            int totalCount = 0;
            ChatMessageRecord[] page = null;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
                page = accesser.GetPage(tree, pageSize, pageIndex, ChatMessageRecord._AutoID, false, out totalCount);
                scope.Commit();
            }
            List<ChatMessageRecord> recordList = new List<ChatMessageRecord>(page);
            if (page != null && page.Length > 0)
            {
                recordList.Sort((x, y) => { return x.AutoID.CompareTo(y.AutoID); });
            }
            return new ChatRecordPage(totalCount, pageIndex, recordList);
        }





        public ChatMessageRecord GetLastFriendRecord(string myID, string friendID)
        {
            if (this.transactionScopeFactory == null)
            {
                return null;
            }

            ChatMessageRecord record = null;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
                SimpleFilterTree tree1 = new SimpleFilterTree(new Filter(ChatMessageRecord._SpeakerID ,myID) ,new Filter(ChatMessageRecord._AudienceID ,friendID)) ;
                SimpleFilterTree tree2 = new SimpleFilterTree(new Filter(ChatMessageRecord._SpeakerID ,friendID) ,new Filter(ChatMessageRecord._AudienceID ,myID)) ;
                ComplexFilterTree tmp = new ComplexFilterTree(LogicType.Or , tree1 ,tree2) ;
                ComplexFilterTree tree = new ComplexFilterTree(LogicType.And, tmp, new Filter(ChatMessageRecord._IsGroupChat,false));
                object id = accesser.GetAggregateValue<object>(AggregateQueryType.Max, ChatMessageRecord._AutoID, tree);
                if (id != null)
                {
                    record = accesser.GetOne(id);
                }
                scope.Commit();
            }
            return record;
        }

        public ChatMessageRecord GetLastGroupRecord(string groupID)
        {
            if (this.transactionScopeFactory == null)
            {
                return null;
            }

            ChatMessageRecord record = null;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
                object id = accesser.GetAggregateValue<object>(AggregateQueryType.Max, ChatMessageRecord._AutoID, new Filter(ChatMessageRecord._AudienceID, groupID), new Filter(ChatMessageRecord._IsGroupChat, true));
                if (id != null)
                {
                    record = accesser.GetOne(id);
                }
                scope.Commit();
            }
            return record;
        }


        public void DeleteChatRecord(string myID, string friendID)
        {
            if (this.transactionScopeFactory == null)
            {
                return ;
            }
            
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {               
                IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
                SimpleFilterTree tree1 = new SimpleFilterTree(new Filter(ChatMessageRecord._SpeakerID, myID), new Filter(ChatMessageRecord._AudienceID, friendID));
                SimpleFilterTree tree2 = new SimpleFilterTree(new Filter(ChatMessageRecord._SpeakerID, friendID), new Filter(ChatMessageRecord._AudienceID, myID));
                ComplexFilterTree tmp = new ComplexFilterTree(LogicType.Or, tree1, tree2);
                ComplexFilterTree tree = new ComplexFilterTree(LogicType.And, tmp, new Filter(ChatMessageRecord._IsGroupChat, false));
                accesser.Delete(tree);
                scope.Commit();
            }
        }

        public void DeleteGroupChatRecord(string groupID)
        {
            if (this.transactionScopeFactory == null)
            {
                return;
            }

            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();
                SimpleFilterTree tree = new SimpleFilterTree(new Filter(ChatMessageRecord._AudienceID, groupID), new Filter(ChatMessageRecord._IsGroupChat, true));                
                accesser.Delete(tree);
                scope.Commit();
            }
        }


        public void ClearAllChatRecord()
        {
            if (this.transactionScopeFactory == null)
            {
                return;
            }

            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {                
                IOrmAccesser<ChatMessageRecord> accesser = scope.NewOrmAccesser<ChatMessageRecord>();               
                accesser.Delete((IFilterTree)null);
                scope.Commit();
            }
        }

  
    } 
    #endregion

    #region SqliteChatRecordPersister
    /// <summary>
    /// 聊天记录本地持久化器（Sqlite数据库）。
    /// </summary>
    public class SqliteChatRecordPersister : DefaultChatRecordPersister
    {
        public SqliteChatRecordPersister(string sqlitePath)
        {
            try
            {
                bool isNew = !File.Exists(sqlitePath);

                //初始化Sqlite数据库
                DataConfiguration config = new SqliteDataConfiguration(sqlitePath);
                TransactionScopeFactory transactionScopeFactory = new TransactionScopeFactory(config);
                transactionScopeFactory.Initialize();

                if (isNew)
                {
                    string sql = "CREATE TABLE ChatMessageRecord (AutoID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, SpeakerID VARCHAR( 0, 20 ) NOT NULL, AudienceID VARCHAR( 0, 20 ) NOT NULL, IsGroupChat BOOLEAN NOT NULL, Content BLOB NOT NULL, OccureTime DATETIME NOT NULL ); "
                               + "CREATE INDEX idx_ChatMessageRecord ON ChatMessageRecord ( SpeakerID, AudienceID, IsGroupChat, OccureTime DESC );"
                               + "CREATE INDEX idx2_ChatMessageRecord ON ChatMessageRecord ( AudienceID, IsGroupChat, OccureTime DESC);";
                    using (TransactionScope scope = transactionScopeFactory.NewTransactionScope())
                    {
                        IRelationAccesser accesser = scope.NewRelationAccesser();
                        accesser.DoCommand(sql);
                        scope.Commit();
                    }
                }

                base.Initialize(transactionScopeFactory);
            }
            catch { }

        }
    } 
    #endregion
}
