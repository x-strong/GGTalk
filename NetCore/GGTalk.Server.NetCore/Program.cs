using DataRabbit.DBAccessing;
using ESBasic.Loggers;
using ESFramework.Boost;
using ESFramework.Boost.DynamicGroup.Server;
using ESFramework.Boost.NetworkDisk.Server;
using ESPlus.Advanced;
using ESPlus.Application.CustomizeInfo;
using ESPlus.Rapid;
using OMCS.Server;
using SqlSugar;
using System;
using System.Configuration;
using TalkBase;
using TalkBase.Server;

namespace GGTalk.Server.NetCore
{
    class Program
    {

        private static IMultimediaServer MultimediaServer;
        private static DignosticLogger DignosticLogger;
        private static IGlobalService GlobalService;
        static void Main(string[] args)
        {
            IAgileLogger logger = new FileAgileLogger(AppDomain.CurrentDomain.BaseDirectory + "AppLog.txt");
            try
            {

                IDBPersisterExtend persister = null;
                if (bool.Parse(ConfigurationManager.AppSettings["UseVirtualDB"]))
                {
                    persister = new MemoryPersister();
                }
                else
                {
                    DataBaseType dataBaseType = (DataBaseType)Enum.Parse(typeof(DataBaseType), ConfigurationManager.AppSettings["DBType"]);
                    if (dataBaseType == DataBaseType.Dm || dataBaseType == DataBaseType.Kdbndp)
                    {
                        persister = new DBPersister_SqlSugar(dataBaseType);
                    }
                    else if (dataBaseType == DataBaseType.Oracle)
                    {
                        persister = new DBPersister_SqlSugar(DbType.Oracle);
                    }
                    else
                    {
                        persister = new DBPersister();
                    }
                }
                
                IRapidServerEngine rapidServerEngine = RapidEngineFactory.CreateServerEngine();
                rapidServerEngine.HeartbeatTimeoutInSecs = int.Parse(ConfigurationManager.AppSettings["HeartbeatTimeoutInSecs"]);
                rapidServerEngine.Advanced.DiagnosticsEnabled = true;
                rapidServerEngine.Advanced.CustomizeInfoHandleMode = CustomizeInfoHandleMode.IocpDirectly;
                TalkBaseInfoTypes infoTypes = new TalkBaseInfoTypes(1000);
                ResourceCenter<GGUser, GGGroup> resourceCenter = new ResourceCenter<GGUser, GGGroup>(rapidServerEngine, persister, new TalkBaseHelper(), infoTypes, logger);
                if (bool.Parse(ConfigurationManager.AppSettings["PreLoadData"]))
                {
                    resourceCenter.ServerGlobalCache.PreLoadUsers();
                    resourceCenter.ServerGlobalCache.PreLoadGroups();
                }
                #region 初始化ESFramework服务端引擎             
                ESPlus.GlobalUtil.SetAuthorizedUser("FreeUser", "");
                ESPlus.GlobalUtil.SetMaxLengthOfUserID(20);
                ESPlus.GlobalUtil.SetMaxLengthOfMessage(1024 * 1024 * 10);

                /*
                 * 注意：GGTalk的讨论组或群，是通过ESPlus.Application.Contacts命名空间来实现的。
                 * 
                 * 而视频会议则是通过ESPlus.Application.Group命名空间来实现的。
                 */

                //自定义的联系人管理器
                ContactsManager contactsManager = new ContactsManager(resourceCenter.ServerGlobalCache);
                rapidServerEngine.ContactsManager = contactsManager;

                #region 视频会议动态组
                DynamicGroupManager dynamicGroupManager = new DynamicGroupManager();//视频会议房间管理、即动态组管理 
                DynamicGroupHandler dynamicGroupHandler = new DynamicGroupHandler();
                #endregion

                ServerBusinessHandler businessHandler = new ServerBusinessHandler(resourceCenter, persister);
                NDiskHandler nDiskHandler = new NDiskHandler();

                ComplexCustomizeHandler complexHandler = new ComplexCustomizeHandler(resourceCenter.CustomizeHandler, businessHandler, nDiskHandler, dynamicGroupHandler);

                //初始化服务端引擎
                //rapidServerEngine.GroupManager = dynamicGroupManager;
                rapidServerEngine.Logger = resourceCenter.Logger;
                rapidServerEngine.SecurityLogEnabled = false;
                //rapidServerEngine.Advanced.DiagnosticsEnabled = true;
                rapidServerEngine.Initialize(int.Parse(ConfigurationManager.AppSettings["Port"]), complexHandler, new BasicHandler(persister, resourceCenter));
                rapidServerEngine.ContactsController.ContactsConnectedNotifyEnabled = false; //上线通知 使用自定义信息
                rapidServerEngine.ContactsController.ContactsDisconnectedNotifyEnabled = true;
                rapidServerEngine.ContactsController.UseContactsNotifyThread = false; //当系统人数很多时，需要开启。
                rapidServerEngine.ContactsController.BroadcastBlobListened = true; //用于群聊天记录存储                
 
                //初始化网盘处理器
                NetworkDiskPathManager networkDiskPathManager = new NetworkDiskPathManager();
                NetworkDisk networkDisk = new NetworkDisk(networkDiskPathManager, rapidServerEngine.FileController);
                nDiskHandler.Initialize(rapidServerEngine.FileController, networkDisk,logger);

                dynamicGroupHandler.Initialize(rapidServerEngine.UserManager, rapidServerEngine.CustomizeController, dynamicGroupManager);
                #endregion

                #region 初始化OMCS服务器
                OMCS.GlobalUtil.SetAuthorizedUser("FreeUser", "");
                OMCS.GlobalUtil.SetMaxLengthOfUserID(20);

                //用于验证登录用户的帐密
                DefaultUserVerifier userVerifier = new DefaultUserVerifier();
                Program.MultimediaServer = MultimediaServerFactory.CreateMultimediaServer(int.Parse(ConfigurationManager.AppSettings["OmcsPort"]), userVerifier,  false);
                #endregion
                resourceCenter.Initialize(Program.MultimediaServer, null);
                new EnginePlus(rapidServerEngine);
                Console.WriteLine("GGTalk Service Started! listening on port :" + ConfigurationManager.AppSettings["Port"]);
            }
            catch (Exception ee)
            {
                logger.Log(ee, "Main", ErrorLevel.High);
                Console.WriteLine("启动GGTalk Server 失败：" + ee.Message);
                Console.ReadLine();
            }


        }


    }
}
