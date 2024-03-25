using System;
using System.Collections.Generic;
using System.Text;
using ESPlus.Rapid;
using ESBasic.Loggers;
using ESPlus.Application.CustomizeInfo;
using OMCS.Server;
using TalkBase.Server.Core;

namespace TalkBase.Server
{
    /// <summary>
    /// 服务端全局资源中心。
    /// </summary>  
    public class ResourceCenter<TUser, TGroup> 
        where TUser : TalkBase.IUser, new()
        where TGroup : TalkBase.IGroup
    {
        public ResourceCenter(IRapidServerEngine engine, IDBPersister<TUser, TGroup> persister, ITalkBaseHelper<TGroup> helper, TalkBaseInfoTypes types, IAgileLogger agileLogger)
        {
            this.logger = agileLogger ?? new EmptyAgileLogger();           
            this.rapidServerEngine = engine;            
            this.serverGlobalCache = new ServerGlobalCache<TUser, TGroup>(persister, helper);

            CoreHandler<TUser, TGroup> sender = new CoreHandler<TUser, TGroup>(this.serverGlobalCache, this.rapidServerEngine, types);
            this.serverHandler = new ServerHandler<TUser, TGroup>(this.serverGlobalCache, this.rapidServerEngine, sender, types, helper);       
            this.globalService = new GlobalService<TUser, TGroup>(this.serverGlobalCache, engine, sender,this.serverHandler ,  types);                
        }

        /// <summary>
        /// 初始化。在IRapidServerEngine初始化成功后调用。
        /// </summary>
        /// <param name="server">如果没有使用到OMCS框架，则可传入null。</param>
        /// <param name="messagePusher">如果没有推送消息到移动端需求，则可传入null。</param>
        public void Initialize(IMultimediaServer server, IMessagePusher messagePusher = null)
        {
            this.multimediaServer = server;
            this.serverHandler.MessagePusher = messagePusher;
            this.serverHandler.Initialize();
        }


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

        #region MultimediaServer
        private IMultimediaServer multimediaServer;
        /// <summary>
        /// OMCS服务端引擎。如果没有使用到OMCS框架，则返回null。
        /// </summary>
        public IMultimediaServer MultimediaServer
        {
            get { return multimediaServer; }
        } 
        #endregion

        #region RapidServerEngine
        private IRapidServerEngine rapidServerEngine;
        /// <summary>
        /// 网络通信引擎。
        /// </summary>
        public IRapidServerEngine RapidServerEngine
        {
            get { return rapidServerEngine; }
        } 
        #endregion

        #region ServerGlobalCache
        private ServerGlobalCache<TUser, TGroup> serverGlobalCache;
        /// <summary>
        /// 服务端的全局缓存。（包括用户资料、群组资料等）
        /// </summary>
        public ServerGlobalCache<TUser, TGroup> ServerGlobalCache
        {
            get { return serverGlobalCache; }
        } 
        #endregion

        #region GlobalService
        private IGlobalService globalService;
        /// <summary>
        /// 提供给外部系统使用的接口。
        /// </summary>
        public IGlobalService GlobalService
        {
            get { return globalService; }
        } 
        #endregion

        #region CustomizeHandler、TalkBaseHandler
        private ServerHandler<TUser, TGroup> serverHandler;
        /// <summary>
        /// TalkBase类库提供的服务端消息处理器。（用于通过集成后注入RapidEngine）
        /// </summary>
        public IIntegratedCustomizeHandler CustomizeHandler
        {
            get { return serverHandler; }
        }
       
        /// <summary>
        /// TalkBase服务端消息处理器。
        /// </summary>
        public ITalkBaseHandler TalkBaseHandler
        {
            get { return serverHandler; }
        } 
        #endregion        
    }
}
