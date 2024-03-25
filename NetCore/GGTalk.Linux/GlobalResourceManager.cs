using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using ESBasic.Loggers;
using System.IO;
using ESBasic;
using ESBasic.Helpers;

using TalkBase;
using GGTalk.Linux;

using GGTalk.Linux.Helpers;

using System.Diagnostics;
using CPF.Drawing;
using CPF.Platform;
using System.Runtime.InteropServices;

namespace GGTalk
{
    internal class GlobalResourceManager
    {
        public static void Initialize()
        {
            try
            {
                #region Log
                string logFilePath = SystemSettings.SystemSettingsDir + "AppLog.txt";
                GlobalResourceManager.logger = new FileAgileLogger(logFilePath);
                #endregion                               
                GlobalResourceManager.softwareName = ConfigurationManager.AppSettings["SoftwareName"];
                GlobalResourceManager.companyUrl = ConfigurationManager.AppSettings["CompanyUrl"];
                string resourceDir = AppDomain.CurrentDomain.BaseDirectory + "Resource/";
                GlobalResourceManager.noneIcon64 = Linux.Helpers.FileHelper.FindAssetsBitmap("64.ico");             
                GlobalResourceManager.groupIcon = Linux.Helpers.FileHelper.FindAssetsBitmap("normal_group_40.png");
                GlobalResourceManager.notifyIcon = Linux.Helpers.FileHelper.FindAssetsBitmap("notify.png");

                #region HeadImage
                List<string> list = ESBasic.Helpers.FileHelper.GetOffspringFiles(AppDomain.CurrentDomain.BaseDirectory + "Head/");
                GlobalResourceManager.headImages = new Image[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    GlobalResourceManager.headImages[i] = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Head/" + i.ToString() + ".png");
                }

                GlobalResourceManager.headImagesGrey = new Bitmap[list.Count];
                for (int i = 0; i < GlobalResourceManager.headImagesGrey.Length; i++)
                {
                    //GlobalResourceManager.headImagesGrey[i] = ESBasic.Helpers.ImageHelper.ConvertToGrey(GlobalResourceManager.headImages[i]);
                }
                #endregion

                #region Emotion
                List<string> tempList = ESBasic.Helpers.FileHelper.GetOffspringFiles(AppDomain.CurrentDomain.BaseDirectory + "Emotion/");
                List<string> emotionFileList = new List<string>();
                foreach (string file in tempList)
                {
                    string name = file.ToLower();
                    if (name.EndsWith(".bmp") || name.EndsWith(".jpg") || name.EndsWith(".jpeg") || name.EndsWith(".png") || name.EndsWith(".gif"))
                    {
                        emotionFileList.Add(name);
                    }
                }
                emotionFileList.Sort(new Comparison<string>(CompareEmotionName));
                List<Image> emotionList = new List<Image>();
                for (int i = 0; i < emotionFileList.Count; i++)
                {
                    emotionList.Add(Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Emotion/" + emotionFileList[i]));
                }
                #endregion

                #region SetStatusImage
                Dictionary<UserStatus, Bitmap> statusImageDictionary = new Dictionary<UserStatus, Bitmap>();
                statusImageDictionary.Add(UserStatus.Online, GGTalk.Linux.Helpers.FileHelper.FindAssetsBitmap("0.png"));
                statusImageDictionary.Add(UserStatus.Away, GGTalk.Linux.Helpers.FileHelper.FindAssetsBitmap("1.png"));
                statusImageDictionary.Add(UserStatus.Busy, GGTalk.Linux.Helpers.FileHelper.FindAssetsBitmap("2.png"));
                statusImageDictionary.Add(UserStatus.DontDisturb, GGTalk.Linux.Helpers.FileHelper.FindAssetsBitmap("3.png"));
                statusImageDictionary.Add(UserStatus.Hide, GGTalk.Linux.Helpers.FileHelper.FindAssetsBitmap("4.png"));
                statusImageDictionary.Add(UserStatus.OffLine, GGTalk.Linux.Helpers.FileHelper.FindAssetsBitmap("5.png"));
                SetStatusImage(statusImageDictionary);

                #endregion

                GlobalResourceManager.audioFilePath = resourceDir + "ring.wav";

                int registerPort = int.Parse(ConfigurationManager.AppSettings["RemotingPort"]);

                GlobalResourceManager.png64 = Image.FromFile(resourceDir + "64.png");
                GlobalResourceManager.icon64 =GlobalResourceManager.png64;// ImageHelper.ConvertToIcon(GlobalResourceManager.png64, 64);
                //GlobalResourceManager.icon64Grey = ImageHelper.ConvertToIcon(ImageHelper.ConvertToGrey(GlobalResourceManager.png64), 64);
                GlobalResourceManager.mainBackImage = Image.FromFile(resourceDir + "BackImage.png");
                GlobalResourceManager.emotionList = emotionList;
                GlobalResourceManager.loginBackImage = GlobalResourceManager.MainBackImage;
            }
            catch (Exception ee)
            {
                Logger.Log(ee, "GlobalResourceManager.Initialize()", ErrorLevel.High);
                MessageBoxEx.Show("加载系统资源时，出现错误。" + ee.Message);
            }
        }

        public static readonly bool IsWindowsOS = true;

        private static int CompareEmotionName(string a, string b)
        {
            if (a.Length != b.Length)
            {
                return a.Length - b.Length;
            }

            return a.CompareTo(b);
        }

        private static void SetStatusImage(Dictionary<UserStatus, Bitmap> _statusImageDictionary)
        {
            GlobalResourceManager.do_SetStatusImage(_statusImageDictionary);
        }

        #region CompanyUrl
        private static string companyUrl = "";
        public static string CompanyUrl
        {
            get { return GlobalResourceManager.companyUrl; }
        }
        #endregion                      

        #region Icon64
        private static Image icon64;
        public static Image Icon64
        {
            get { return icon64; }
        }
        #endregion

        #region Icon64Grey
        private static Image icon64Grey;
        public static Image Icon64Grey
        {
            get { return icon64Grey; }
        }
        #endregion

        #region MainBackImage
        private static Image mainBackImage;
        public static Image MainBackImage
        {
            get { return mainBackImage; }
        }
        #endregion

        #region EmotionList、EmotionDictionary
        private static List<Image> emotionList;
        public static List<Image> EmotionList
        {
            get { return emotionList; }
        }
        private static Dictionary<uint, Image> emotionDictionary;
        public static Dictionary<uint, Image> EmotionDictionary
        {
            get
            {
                if (emotionDictionary == null)
                {
                    emotionDictionary = new Dictionary<uint, Image>();
                    for (uint i = 0; i < emotionList.Count; i++)
                    {
                        emotionDictionary.Add(i, emotionList[(int)i]);
                    }
                }
                return emotionDictionary;
            }
        }
        #endregion

        #region Png64
        private static Image png64;
        public static Image Png64
        {
            get { return png64; }
        }
        #endregion

        #region PrimaryScreen
        private static Screen primaryScreen;
        public static Screen PrimaryScreen => primaryScreen;

        public static void SetPrimaryScreen(Screen screen)
        {
            primaryScreen = screen;
        }
        #endregion

        #region FileAssistantImage
        public static Bitmap FileAssistantImage
        {
            get
            {
                return null;
                //return global::GGTalk.Properties.Resources.FileAssistant;
            }
        } 
        #endregion

        #region GetStatusImage
        private static Dictionary<UserStatus, Image> statusIconDictionary = new Dictionary<UserStatus, Image> ();
        private static Dictionary<UserStatus, Bitmap> statusImageDictionary = new Dictionary<UserStatus, Bitmap>();
        private static void do_SetStatusImage(Dictionary<UserStatus, Bitmap> _statusImageDictionary)
        {
            GlobalResourceManager.statusImageDictionary = _statusImageDictionary;

            GlobalResourceManager.statusIconDictionary.Add(UserStatus.Online, GlobalResourceManager.Icon64);
            GlobalResourceManager.statusIconDictionary.Add(UserStatus.Busy, CombineStateImage(GlobalResourceManager.png64, statusImageDictionary[UserStatus.Busy]));
            GlobalResourceManager.statusIconDictionary.Add(UserStatus.Away, CombineStateImage(GlobalResourceManager.png64, statusImageDictionary[UserStatus.Away]));
            GlobalResourceManager.statusIconDictionary.Add(UserStatus.DontDisturb, CombineStateImage(GlobalResourceManager.png64, statusImageDictionary[UserStatus.DontDisturb]));
            GlobalResourceManager.statusIconDictionary.Add(UserStatus.Hide, CombineStateImage(GlobalResourceManager.png64, statusImageDictionary[UserStatus.Hide]));
            GlobalResourceManager.statusIconDictionary.Add(UserStatus.OffLine, GlobalResourceManager.Icon64Grey);
        }

        private static Bitmap CombineStateImage(Image img, Image stateImage)
        {
            return null;
            //Bitmap bm = new Bitmap(img);
            //using (Graphics g = Graphics.FromImage(bm))
            //{
            //    int len = (int)(img.Width * 0.45);
            //    g.DrawImage(stateImage, new Rectangle(len, len, img.Width - len, img.Height - len), new Rectangle(0, 0, stateImage.Width, stateImage.Height), GraphicsUnit.Pixel);
            //}

            //return ImageHelper.ConvertToIcon(bm, 64);
        }

        public static Bitmap GetStatusImage(UserStatus status)
        {
            return GlobalResourceManager.statusImageDictionary[status];
        }

        public static Image GetStatusIcon(UserStatus status)
        {
            return GlobalResourceManager.statusIconDictionary[status];
        }

        public static Dictionary<UserStatus, Bitmap> GetStatusImageDictionary()
        {
            return statusImageDictionary;
        }
        #endregion

        //#region ConvertUserStatus
        //public static ChatListSubItem.UserStatus ConvertUserStatus(UserStatus status)
        //{
        //    if (status == UserStatus.Hide)
        //    {
        //        return ChatListSubItem.UserStatus.OffLine;
        //    }

        //    return (ChatListSubItem.UserStatus)((int)status);
        //}
        //#endregion

        #region GetUserStatusName
        public static string GetUserStatusName(UserStatus status)
        {
            if (status == UserStatus.Online)
            {
                return "在线";
            }
            if (status == UserStatus.Away)
            {
                return "离开";
            }
            if (status == UserStatus.Busy)
            {
                return "忙碌";
            }
            if (status == UserStatus.DontDisturb)
            {
                return "请勿打扰";
            }
            return "隐身或离线";
        }
        #endregion


        #region RemotingService
        private static IGGService remotingService;
        public static IGGService RemotingService
        {
            get { return GlobalResourceManager.remotingService; }
        } 
        #endregion
       
        #region Logger
        private static IAgileLogger logger = null;
        public static IAgileLogger Logger
        {
            get { return GlobalResourceManager.logger; }
        }

        public static void WriteErrorLog(Exception ee)
        {
            StackTrace stackTrace = new StackTrace(true);
            StackFrame sf = stackTrace.GetFrame(1);   
            logger.LogSimple(ee, sf.GetFileName() + " : " + sf.GetMethod().Name, ErrorLevel.Standard);
        }
        #endregion

        //#region UiSafeInvoker
        //private static UiSafeInvoker uiSafeInvoker;
        //public static UiSafeInvoker UiSafeInvoker
        //{
        //    get { return GlobalResourceManager.uiSafeInvoker; }
        //}

        //public static void SetUiSafeInvoker(UiSafeInvoker invoker)
        //{
        //    GlobalResourceManager.uiSafeInvoker = invoker;
        //}
        //#endregion
        
        #region SoftwareName
        private static string softwareName = "傲瑞通";
        public static string SoftwareName
        {
            get { return GlobalResourceManager.softwareName; }            
        } 
        #endregion                      

        #region NoneIcon64
        private static Bitmap noneIcon64;
        public static Bitmap NoneIcon64
        {
            get { return noneIcon64; }
        }
        #endregion        

        #region GroupIcon
        private static Bitmap groupIcon;
        public static Bitmap GroupIcon
        {
            get { return GlobalResourceManager.groupIcon; }
        } 
        #endregion          


        #region NotifyIcon
        private static Bitmap notifyIcon;
        public static Bitmap NotifyIcon
        {
            get { return GlobalResourceManager.notifyIcon; }
        }
        #endregion  

        #region LoginBackImage
        private static Image loginBackImage;
        public static Image LoginBackImage
        {
            get { return loginBackImage; }
        }
        #endregion

        #region HeadImages
        private static Image[] headImages;
        public static Image[] HeadImages
        {
            get
            {
                return headImages;
            }
        } 
        #endregion        

        #region HeadImagesGrey
        private static Image[] headImagesGrey;
        public static Image[] HeadImagesGrey
        {
            get
            {
                return headImagesGrey;
            }
        }
        #endregion        

        #region 播放声音
        public static void PlayAudioAsyn()
        {
            if (!SystemSettings.Singleton.PlayAudio4Message)
            {
                return;
            }
            System.Threading.Tasks.Task.Factory.StartNew(()=> {
                PlayAudio(audioFilePath, 1);
            });
            //CbGeneric<string, int> cbPlayAudio = new CbGeneric<string, int>(PlayAudio);
            //cbPlayAudio.BeginInvoke(audioFilePath, 1, null, null);
            //UiSafeInvoker.ActionOnUI<string, int>(PlayAudio, audioFilePath, 1);
        }

        private static string audioFilePath = "";
        private static void PlayAudio(string audioPath, int playTimes)
        {
            try
            {
                NetCoreAudio.Player player = new NetCoreAudio.Player();
                player.Play(audioPath);
                if (playTimes > 1)
                {
                    System.Threading.Thread.Sleep(2000);
                    player.Play(audioPath);
                }
            }
            catch (Exception ee)
            {
                GlobalResourceManager.WriteErrorLog(ee);
            }

            //System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(audioPath);
            //sndPlayer.Play();
            //if (playTimes > 1)
            //{
            //    System.Threading.Thread.Sleep(2000);
            //    sndPlayer.Play();
            //}
        }
        #endregion        

        public static Image GetHeadImage(GGUser user)
        {
            return GlobalResourceManager.GetHeadImage(user, false);
        }

        public static Image GetHeadImageOnline(GGUser user)
        {
            if (user.HeadImageIndex >= 0)
            {
                if (user.HeadImageIndex < GlobalResourceManager.headImages.Length)
                {
                    return GlobalResourceManager.headImages[user.HeadImageIndex];
                }

                return GlobalResourceManager.headImages[0];
            }
            else
            {
                return BitmapHelper.Convert(user.HeadImageData);
            }
        }

        public static Image GetHeadImage(GGUser user, bool mine)
        {
            if (user.HeadImageIndex >= 0)
            {
                //Bitmap[] ary = (mine ? !user.OnlineOrHide : user.OfflineOrHide) ? GlobalResourceManager.headImagesGrey : GlobalResourceManager.headImages;
                Image[] ary = GlobalResourceManager.headImages;
                if (user.HeadImageIndex < GlobalResourceManager.headImages.Length)
                {
                    return ary[user.HeadImageIndex];
                }

                return ary[0];
            }
            else
            {
                return BitmapHelper.Convert(user.HeadImageData);
               // return (mine ? !user.OnlineOrHide : user.OfflineOrHide) ? user.HeadImageGrey : user.HeadImage;
            }
        }
    }  
}
