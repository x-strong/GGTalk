using System;
using System.Collections.Generic;
using System.Text;
using ESPlus.Rapid;
using ESBasic.Loggers;
using TalkBase.Client.Bridges;
using ESBasic.Security;
using ESBasic.ObjectManagement.Managers;
using ESFramework;
using TalkBase.Client.Core;

namespace TalkBase.Client
{
    /// <summary>
    /// 客户端全局资源中心。
    /// </summary>    
    public class ResourceCenter<TUser, TGroup> : IResourceCenter  
        where TUser : class, TalkBase.IUser, new()
        where TGroup : TalkBase.IGroup, new()
    {
        public ResourceCenter()
        {
            this.clientHandler = new ClientHandler<TUser, TGroup>();
        }

        public void Initialize(IRapidPassiveEngine engine, ITalkBaseHelper<TGroup> helper, TalkBaseInfoTypes types, string globalCacheFilePath, string sqliteFilePath, IAgileLogger agileLogger, TwinkleNotifyIcon icon, IUnitInfoProvider provider, IChatFormController controller, IChatRecordGetter remote)
        {
            this.logger =agileLogger ?? new EmptyAgileLogger();          
            this.rapidPassiveEngine = engine;
            if (string.IsNullOrEmpty(sqliteFilePath))
            {
                this.localChatRecordPersister = new EmptyChatRecordPersister();
            }
            else
            {
                this.localChatRecordPersister = new SqliteChatRecordPersister(sqliteFilePath);
            }

            if (!string.IsNullOrEmpty(helper.Key4MessageEncrypt))
            {
                this.desEncryption = new DesEncryption(DesStrategy.Des3, helper.Key4MessageEncrypt);
            }

            this.clientGlobalCache = new ClientGlobalCache<TUser, TGroup>(engine, helper, types, globalCacheFilePath, logger);
            this.clientOutter = new ClientOutter<TUser, TGroup>(engine, this.clientGlobalCache, this.localChatRecordPersister, types, helper ,this.desEncryption);           
            this.talkBaseInfoTypes = types;
            this.unitInfoProvider = provider;
            this.remoteChatRecordGetter = new ChatRecordGetter(engine,types);
            this.chatFormController = controller; 
            this.clientHandler.Initialize(this, icon);

            this.chatFormController.FormCreated += new ESBasic.CbGeneric<IChatForm, IUnit>(chatFormController_FormCreated);
        }

        void chatFormController_FormCreated(IChatForm form, IUnit unit)
        {
            if (unit.UnitType == UnitType.User)
            {
                //在某些模式下，需要在打开聊天窗口时，同步对方的资料。
                if (FunctionOptions.ContactsCareType == ContactsCareType.None ||
                    (FunctionOptions.ContactsCareType == ContactsCareType.FriendsOnly && !this.clientGlobalCache.CurrentUser.IsFriend(unit.ID))
                   )
                {
                    this.clientOutter.SyncUserBaseInfoInBackground(unit.ID);
                }
            }
        }

        #region DesEncryption
        private DesEncryption desEncryption = null;
        /// <summary>
        /// 3DES加密。如果消息不需要加密，则返回null。
        /// </summary>
        public DesEncryption DesEncryption
        {
            get { return desEncryption; }
        } 
        #endregion
     
        #region Logger
        private IAgileLogger logger = new EmptyAgileLogger();
        /// <summary>
        /// 日志记录器。
        /// </summary>
        public IAgileLogger Logger
        {
            get { return logger; }
        } 
        #endregion

        #region CurrentUserID
        /// <summary>
        /// 当前登录用户的ID
        /// </summary>
        public string CurrentUserID
        {
            get
            {
                return this.rapidPassiveEngine.CurrentUserID;
            }
        }
        #endregion

        #region CurrentClientType
        /// <summary>
        /// 当前登录设备类型
        /// </summary>
        public ClientType CurrentClientType
        {
            get
            {
                return this.rapidPassiveEngine.CurrentClientType;
            }
        } 
        #endregion

        #region Connected
        /// <summary>
        /// 与服务器的连接是否正常？
        /// </summary>
        public bool Connected
        {
            get
            {
                return this.rapidPassiveEngine.Connected;
            }
        } 
        #endregion
        
        #region LocalChatRecordPersister
        private IChatRecordPersister localChatRecordPersister;
        /// <summary>
        /// 聊天记录本地持久化器。
        /// </summary>
        public IChatRecordPersister LocalChatRecordPersister
        {
            get { return localChatRecordPersister; }
        } 
        #endregion

        #region RemoteChatRecordGetter
        private IChatRecordGetter remoteChatRecordGetter;
        /// <summary>
        /// 从服务端获取聊天记录的接口。
        /// </summary>
        public IChatRecordGetter RemoteChatRecordGetter
        {
            get { return remoteChatRecordGetter; }
        }
        #endregion

        #region RapidPassiveEngine
        private IRapidPassiveEngine rapidPassiveEngine;
        /// <summary>
        /// 网络通信引擎。
        /// </summary>
        public IRapidPassiveEngine RapidPassiveEngine
        {
            get { return rapidPassiveEngine; }
        } 
        #endregion

        #region ClientOutter
        private IClientOutter clientOutter;
        /// <summary>
        /// 与服务器/其它在线用户进行交互的通道。
        /// </summary>
        public IClientOutter ClientOutter
        {
            get { return clientOutter; }
        } 
        #endregion

        #region ClientGlobalCache
        private BaseGlobalCache<TUser, TGroup> clientGlobalCache;
        /// <summary>
        /// 客户端的全局缓存。（包括用户资料、群组资料等）
        /// </summary>
        public BaseGlobalCache<TUser, TGroup> ClientGlobalCache
        {
            get { return clientGlobalCache; }            
        } 
        #endregion

        #region TalkBaseInfoTypes
        private TalkBaseInfoTypes talkBaseInfoTypes;
        /// <summary>
        /// TalkBase框架使用的消息类型定义。
        /// </summary>
        public TalkBaseInfoTypes TalkBaseInfoTypes
        {
            get { return talkBaseInfoTypes; }
        } 
        #endregion

        #region TalkBaseHandler
        private ClientHandler<TUser, TGroup> clientHandler;
        /// <summary>
        /// TalkBase类库提供的客户端消息处理器。（用于通过集成后注入RapidEngine）
        /// </summary>
        public ClientHandler<TUser, TGroup> TalkBaseHandler
        {
            get { return clientHandler; }
        } 
        #endregion

        #region UnitInfoProvider
        private IUnitInfoProvider unitInfoProvider;
        /// <summary>
        /// Unit基础信息提供者。
        /// </summary>
        public IUnitInfoProvider UnitInfoProvider
        {
            get { return unitInfoProvider; }
        } 
        #endregion

        #region ChatFormController
        private IChatFormController chatFormController;
        /// <summary>
        /// 聊天窗口控制器。
        /// </summary>
        public IChatFormController ChatFormController
        {
            get { return chatFormController; }
        } 
        #endregion       

        #region ExtendCache
        private ObjectManager<string, object> extendCache = new ObjectManager<string, object>();
        /// <summary>
        /// 设置扩展缓存。
        /// </summary>
        /// <param name="name">缓存名称</param>
        /// <param name="content">缓存内容</param>
        public void SetExtendCache(string name, object content)
        {
            this.extendCache.Add(name, content);
        }

        /// <summary>
        /// 获取扩展缓存。
        /// </summary>
        /// <param name="name">缓存名称</param>
        /// <returns>缓存内容</returns>
        public object GetExtendCache(string name)
        {
            return this.extendCache.Get(name);
        } 
        #endregion
    }
}
