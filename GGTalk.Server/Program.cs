using ESBasic.Loggers;
using ESFramework.Boost;
using ESFramework.Boost.DynamicGroup.Server;
using ESFramework.Boost.NetworkDisk.Server;
using ESFramework.Server.UserManagement;
using ESPlus.Advanced;
using ESPlus.Application.CustomizeInfo;
using ESPlus.Rapid;
using ESPlus.Serialization;
using OMCS.Server;
using SqlSugar;
using System;
using System.Configuration;
using System.Runtime.Remoting;
using System.Windows.Forms;
using TalkBase;
using TalkBase.Server;


/*
 * 本demo采用的是ESFramework和OMCS的免费版本，不需要再次授权、也没有使用期限限制。若想获取ESFramework和OMCS的其它版本，请联系 www.oraycn.com 或 QQ：372841921。
 * 
 */
namespace GGTalk.Server
{
    static class Program
    {       
        private static IMultimediaServer MultimediaServer;
        private static DignosticLogger DignosticLogger;
        private static IGlobalService GlobalService;

        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                IAgileLogger logger = new FileAgileLogger(AppDomain.CurrentDomain.BaseDirectory + "AppLog.txt");
                CompactPropertySerializer.SetDefault(new ExtenseCompactPropertySerializer());
                IDBPersisterExtend persister = null;
                if (bool.Parse(ConfigurationManager.AppSettings["UseVirtualDB"]))
                {
                    persister = new MemoryPersister();
                }
                else
                {
                    if (ConfigurationManager.AppSettings["DBType"] == "Oracle")
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

                //设置重登陆模式
                //rapidServerEngine.UserManager.RelogonMode = RelogonMode.ReplaceOld;


                //初始化网盘处理器
                NetworkDiskPathManager networkDiskPathManager = new NetworkDiskPathManager();
                NetworkDisk networkDisk = new NetworkDisk(networkDiskPathManager, rapidServerEngine.FileController);
                nDiskHandler.Initialize(rapidServerEngine.FileController, networkDisk, logger);

                dynamicGroupHandler.Initialize(rapidServerEngine.UserManager, rapidServerEngine.CustomizeController, dynamicGroupManager);
                #endregion

                #region 初始化OMCS服务器
                OMCS.GlobalUtil.SetAuthorizedUser("FreeUser", "");
                OMCS.GlobalUtil.SetMaxLengthOfUserID(20);

                //用于验证登录用户的帐密
                DefaultUserVerifier userVerifier = new DefaultUserVerifier();
                Program.MultimediaServer = MultimediaServerFactory.CreateMultimediaServer(int.Parse(ConfigurationManager.AppSettings["OmcsPort"]), userVerifier,  false);

                #endregion

                #region 发布用于注册的Remoting服务
                RemotingConfiguration.Configure("GGTalk.Server.exe.config", false);
                GGService ggService = new Server.GGService(resourceCenter);
                RemotingServices.Marshal(ggService, "GGService");
                RemotingServices.Marshal((MarshalByRefObject)resourceCenter.GlobalService, "GlobalService");
                Program.GlobalService = resourceCenter.GlobalService;
                #endregion

                resourceCenter.Initialize(Program.MultimediaServer, null);

                //如果不需要默认的UI显示，可以替换下面这句为自己的Form
                MainServerForm mainForm = new MainServerForm(rapidServerEngine, Program.MultimediaServer , true);
                mainForm.Text = ConfigurationManager.AppSettings["SoftwareName"] + " 服务器（支持多端登录）";
                //Program.DignosticLogger = new DignosticLogger(engine.Advanced.DiagnosticsViewer, "Diagnostics.txt" ,10);
                Application.Run(mainForm);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
    
    }
}
