using CCWin;
using ESFramework.Boost.DynamicGroup.Passive;
using ESFramework.Boost.NetworkDisk.Passive;
using ESFramework.Extensions.ChatRendering;
using ESPlus.Application.CustomizeInfo;
using ESPlus.Rapid;
using ESPlus.Serialization;
using System;
using System.Configuration;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TalkBase;
using TalkBase.Client;
using TalkBase.Client.Bridges;

namespace GGTalk
{
    static class Program
    {
        internal static IChatFormShower ChatFormShower;

        internal static ResourceCenter<GGUser, GGGroup> ResourceCenter;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                GlobalResourceManager.Initialize();
                TalkClientInitializer.Initialize(GlobalResourceManager.Icon64, GlobalResourceManager.MainBackImage, GlobalResourceManager.EmotionDictionary);
                OMCS.GlobalUtil.SetMaxLengthOfUserID(20);
                ESPlus.GlobalUtil.SetMaxLengthOfUserID(20);
                ESPlus.GlobalUtil.SetMaxLengthOfMessage(1024 * 1024 * 10);
                CompactPropertySerializer.SetDefault(new ExtenseCompactPropertySerializer());
                IRapidPassiveEngine passiveEngine = RapidEngineFactory.CreatePassiveEngine();
                passiveEngine.Logger = GlobalResourceManager.Logger;
                ClientBusinessOutter.Instance.Initialize(passiveEngine);
                ResourceCenter = new ResourceCenter<GGUser, GGGroup>();

                /*
                * 注意：GGTalk的讨论组或群，是通过ESPlus.Application.Contacts命名空间来实现的。
                * 
                * 而视频会议则是通过ESPlus.Application.Group命名空间来实现的。
                */

                //视频会议、即动态组
                DynamicGroupOutter dynamicGroupOutter = new DynamicGroupOutter();
                dynamicGroupOutter.TryP2PWhenGroupmateConnected = false;
                dynamicGroupOutter.RapidPassiveEngine = passiveEngine;
                DynamicGroupPassiveHandler groupPassiveHandler = new DynamicGroupPassiveHandler();
                groupPassiveHandler.Initialize(dynamicGroupOutter);

                NDiskPassiveHandler nDiskPassiveHandler = new NDiskPassiveHandler();
                ClientBusinessHandler businessHandler = new ClientBusinessHandler();
                ComplexCustomizeHandler complexHandler = new ComplexCustomizeHandler(ResourceCenter.TalkBaseHandler, businessHandler, nDiskPassiveHandler, groupPassiveHandler);
                LoginForm loginForm = new LoginForm(passiveEngine, complexHandler);

                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                bool p2pEnabled = bool.Parse(ConfigurationManager.AppSettings["P2PEnabled"]);
                //passiveEngine.P2PController.P2PChannelMode = p2pEnabled ? P2PChannelMode.TcpAndUdp : P2PChannelMode.None;
                string sqliteFilePath = SystemSettings.SystemSettingsDir + passiveEngine.CurrentUserID + ".db";
                string globalCacheFilePath = SystemSettings.SystemSettingsDir + passiveEngine.CurrentUserID + ".dat";
                nDiskPassiveHandler.Initialize(passiveEngine.FileOutter, null);
                dynamicGroupOutter.Initialize(passiveEngine.CurrentUserID);


                RenderSettings.TextBackColor = Color.LightGray;
                MainForm mainForm = new MainForm();
                ChatFormShower = mainForm;
                TalkBase.Client.Bridges.IChatFormController chatFormController = new SeperateController(ResourceCenter, mainForm);
                ResourceCenter.Initialize(passiveEngine, new TalkBaseHelper(), new TalkBaseInfoTypes(1000), globalCacheFilePath, sqliteFilePath, GlobalResourceManager.Logger, mainForm.TwinkleNotifyIcon, mainForm, chatFormController, GlobalResourceManager.RemotingService);
                mainForm.Initialize(ResourceCenter, (UserStatus)((int)loginForm.LoginStatus), dynamicGroupOutter);

                Application.Run(mainForm);


            }
            catch (Exception ee)
            {
                MessageBoxEx.Show(ee.Message);
            }
        }        
    }

}
