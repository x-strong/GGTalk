
using System;
using System.Collections.Generic;
using System.Diagnostics; 
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CPF;
using CPF.Controls;
using CPF.Drawing;
using GGTalk.Linux.Controller;
using GGTalk.Linux.Controls;
using GGTalk.Linux.Helpers;
using GGTalk.Linux.Views;
using TalkBase;


namespace GGTalk.Linux
{
    internal static class CommonHelper
    {

        public static string SeparAtor = ESBasic.Helpers.FileHelper.GetFilePathSeparatorChar().ToString();
        /// <summary>
        /// 跳转到聊天页面
        /// </summary>
        /// <param name="unit"></param>
        public static void MoveToChat(IUnit unit,bool show = true)
        {
            if (unit == null) { return; }
            Window window = Program.PassiveEngine.CurrentUserID == unit.ID ? MainWindow.ChatFormController.GetFileAssistantForm() : MainWindow.ChatFormController.GetForm(unit.ID);
            if (window != null && show)
            {
                window.Show_Topmost();
            }
        } 

        /// <summary>
        /// 弹出通知页面
        /// </summary>
        public static void MoveToNotifyWindow(NotifyType notifyType= NotifyType.User)
        {
            NotifyWindow window = (NotifyWindow)MainWindow.ChatFormController.GetNotifyForm();
            if (window != null)
            {
                window.Show_Topmost();
                window.ShowTabPage(notifyType);
            }
        }

        /// <summary>
        /// 获取指定用户状态的资源图片
        /// </summary>
        /// <param name="userStatus"></param>
        /// <returns></returns>
        public static string GetUserStatusIco(UserStatus userStatus)
        {
            string icoName = "0.png";
            switch (userStatus)
            {
                case UserStatus.Online:
                    icoName = "0.png";
                    break;
                case UserStatus.Away:
                    icoName = "1.png";
                    break;
                case UserStatus.Busy:
                    icoName = "2.png";
                    break;
                case UserStatus.DontDisturb:
                    icoName = "3.png";
                    break;
                case UserStatus.Hide:
                    icoName = "4.png";
                    break;
                case UserStatus.OffLine:
                    return null;
            }
            return CommonOptions.ResourcesCatalog + icoName;
        }

        /// <summary>
        /// 是否为电话号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return false;
            }
            //[0-9]{11,11}
            Regex regex = new Regex("^1\\d{10}$");
            return regex.IsMatch(phone);
        }

        /// <summary>
        /// 字符串转化为bool值
        /// </summary>
        /// <param name="boolStr"></param>
        /// <returns></returns>
        public static bool StringIntoBool(string boolStr)
        {
            return boolStr.ToLower() =="true";
        }

        //获取OMCS登录真实账号
        public static string GetUserID4OMCS(string id)
        {
            string[] strs = id.Split('#');
            if (strs == null || strs.Length == 0) { return ""; }
            else if (strs.Length == 1) { return strs[0]; }
            return strs[1];
        }

        /// <summary>
        /// 获取显示状态
        /// </summary>
        /// <param name="show"></param>
        /// <returns></returns>
        public static Visibility GetVisibility(bool show)
        {
            return show ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// 浏览器打开Url
        /// </summary>
        /// <param name="url"></param>
        public static void OpenUrl4Browser(string url)
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                System.Diagnostics.Process.Start("explorer", url);
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
            {
                System.Diagnostics.Process.Start("xdg-open", url);
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
            {
                System.Diagnostics.Process.Start("open", url);
            }
        }
 

        /// <summary>
        /// 解析消息记录中的文字和表情
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        internal static string ParseChatBoxContent(ChatBoxContent2 chatBoxContent)
        {
            string contentString = chatBoxContent.Text;
            Dictionary<uint, NonTextItem> keyValues = chatBoxContent.NonTextItemDictionary;
            List<uint> list = keyValues.Keys.ToList();
            list.Reverse();

            foreach (uint index in list)
            {
                contentString = contentString.Remove((int)index, 1);
                NonTextItem nonTextItem = keyValues[index];
                if (nonTextItem.ChatBoxElementType == ChatBoxElementType.InnerEmotion)
                {
                    uint innerEmotionIndex = nonTextItem.InnerEmotionIndex;
                    string emotionIndex = "[" + innerEmotionIndex.ToString().PadLeft(3, '0') + "]";
                    contentString = contentString.Insert((int)index, emotionIndex);
                }
            }
            return contentString;
        }


        public static ValueTuple<int, int> HaveSelectContent(this TextBox textBox)
        {
            try
            {
                StackTrace st = new StackTrace();
                StackFrame[] sf = st.GetFrames();
                foreach (StackFrame cur_sf in sf)
                {
                    string fun_name = cur_sf.GetMethod().Name;
                    Console.WriteLine(fun_name + "\n");
                }
                uint uint1 = 0;
                uint uint2 = 0;
                if (textBox.SelectionEnd.Count > 0)
                {
                    uint1 = textBox.SelectionEnd[0];
                }
                uint2 = textBox.CaretIndex[0];
                int startIndex = (int)(uint1 < uint2 ? uint1 : uint2);
                int length = Math.Abs((int)(uint1 - uint2));
                if (textBox.SelectionEnd.Count == 0)
                {
                    startIndex = (int)textBox.CaretIndex[0];
                    length = 0;
                }
                ValueTuple<int, int> value = new ValueTuple<int, int>(startIndex, length);
                return value;
            }
            catch (Exception ex)
            {
                GlobalResourceManager.Logger.Log(ex, "ExtenseMethod.HaveSelectContent", ESBasic.Loggers.ErrorLevel.Standard);
                return new ValueTuple<int, int>(0, 0);
            }
        }



        internal static void LastWordsChanged(IUnit unit,bool newMsg = false)
        {
            UiSafeInvoker.ActionOnUI(() => {
                MainWindow main = LoginWindow.MainWindow;
                RecentListBox recentListBox = main.recentListBox;
                if (recentListBox != null)
                {
                    ILastWordsComputer lastWordsComputer = main.Event2ChatListBridge.LastWordsComputer;
                    if (lastWordsComputer != null)
                    {
                        string lastWords = lastWordsComputer.GetLastWords(unit);
                        recentListBox.LastWordChanged(unit, lastWords, newMsg);
                    }
                }
            });
        } 

       //internal static void UnReadHandle(string destUnitID, string speakerID, byte[] content)
       // {
       //     UnReadMsgBox.Singleton.Add(destUnitID, speakerID, content);
       // }

       // internal static void ClearUnReadMsg(string unitID)
       // {
       //     UnReadMsgBox.Singleton.Remove(unitID);
       // }

        /// <summary>
        /// YZY
        /// </summary>
        internal static void Log_Tmp(string str)
        { 
            GlobalResourceManager.Logger.LogWithTime(str);
        }



        internal static CPF.Drawing.Image CropPicture(Image image, int maxLength = 300)
        {  
            if (image.Width < maxLength && image.Height < maxLength)
            {
                return null;
            }
            bool widthMax = image.Width > image.Height;
            int cropWidth, cropHeight;
            if (widthMax)
            {
                cropWidth = maxLength;
                cropHeight = (int)((float)image.Height / image.Width * maxLength);
            }
            else
            {
                cropHeight = maxLength;
                cropWidth = (int)((float)image.Width / image.Height * maxLength);
            }
            Bitmap clone = new Bitmap(cropWidth, cropHeight);  
            using(DrawingContext graphics = DrawingContext.FromBitmap(clone))
            {
                Rect srcRect = new Rect(0, 0, image.Width, image.Height);
                Rect dstRect = new Rect(0, 0, cropWidth, cropHeight);
                graphics.DrawImage(image, dstRect, srcRect);
            } 
            return clone; 
        }
    }
}
