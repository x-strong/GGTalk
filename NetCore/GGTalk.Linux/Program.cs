using CPF.Linux;//如果需要支持Linux才需要
//using CPF.Mac;//如果需要支持Mac才需要
using CPF.Platform;
using CPF.Skia;
using CPF.Windows;
using ESFramework.Boost.DynamicGroup.Passive;
using ESFramework.Boost.NetworkDisk.Passive;
using ESFramework.Extensions.ChatRendering;
using ESPlus.Application.CustomizeInfo;
using ESPlus.Rapid;
using ESPlus.Serialization;
using GGTalk.Linux.Views;
using System;
using System.Drawing;
using TalkBase.Client;

namespace GGTalk.Linux
{
    internal class Program
    {
        private static IRapidPassiveEngine passiveEngine;
        public static IRapidPassiveEngine PassiveEngine => passiveEngine;

        private static ResourceCenter<GGUser, GGGroup> resourceCenter;
        internal static ResourceCenter<GGUser, GGGroup> ResourceCenter => resourceCenter;

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application.Initialize(
                    (OperatingSystemType.Windows, new WindowsPlatform(), new SkiaDrawingFactory())
                    //, (OperatingSystemType.OSX, new MacPlatform(), new SkiaDrawingFactory())//如果需要支持Mac才需要
                    , (OperatingSystemType.Linux, new LinuxPlatform(), new SkiaDrawingFactory())//如果需要支持Linux才需要
                );

                //OMCS.GlobalUtil.SetMaxLengthOfUserID(20);
                ESPlus.GlobalUtil.SetMaxLengthOfUserID(20);
                ESPlus.GlobalUtil.SetMaxLengthOfMessage(1024 * 1024 * 10);
                
                CompactPropertySerializer.SetDefault(new ExtenseCompactPropertySerializer());
                SystemSettings.Singleton.CreateSystemDirectory();
                GlobalResourceManager.Initialize();
                passiveEngine = RapidEngineFactory.CreatePassiveEngine();
                PassiveEngine.Logger = GlobalResourceManager.Logger;
                passiveEngine.WaitResponseTimeoutInSecs = 60;
                ClientBusinessOutter.Instance.Initialize(PassiveEngine);
                resourceCenter = new ResourceCenter<GGUser, GGGroup>();


                //视频会议、即动态组
                DynamicGroupOutter dynamicGroupOutter = new DynamicGroupOutter();
                dynamicGroupOutter.TryP2PWhenGroupmateConnected = false;
                dynamicGroupOutter.RapidPassiveEngine = PassiveEngine;
                DynamicGroupPassiveHandler groupPassiveHandler = new DynamicGroupPassiveHandler();
                groupPassiveHandler.Initialize(dynamicGroupOutter);

                NDiskPassiveHandler nDiskPassiveHandler = new NDiskPassiveHandler();
                ClientBusinessHandler businessHandler = new ClientBusinessHandler();
                ComplexCustomizeHandler complexHandler = new ComplexCustomizeHandler(resourceCenter.TalkBaseHandler, businessHandler, nDiskPassiveHandler, groupPassiveHandler);
                LoginWindow loginWindow = new LoginWindow(complexHandler, nDiskPassiveHandler, dynamicGroupOutter);

                string ttfDirectoryPath = $"{AppDomain.CurrentDomain.BaseDirectory}FontFamily{ESBasic.Helpers.FileHelper.GetFilePathSeparatorChar()}";
                Application.LoadFont($"{ttfDirectoryPath}微软雅黑.ttf", "微软雅黑");
                RenderSettings.TtfResourceDirPath = ttfDirectoryPath;
                RenderSettings.TextBackColor = Color.LightGray; 
                RenderSettings.TextBackColor = System.Drawing.Color.FromArgb(245, 245, 245);
                RenderSettings.TextFontSize = 16;
                //MainWindow mainWindow = new MainWindow(complexHandler, nDiskPassiveHandler, dynamicGroupOutter,TalkBase.UserStatus.Online);
                Application.Run(loginWindow);

                
            }
            catch (Exception ee)
            {
                GlobalResourceManager.WriteErrorLog(ee);
                MessageBoxEx.Show("系统出现异常：" + ee.Message);
            }

        }

    }
}
