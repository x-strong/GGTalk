using ESFramework.Boost.Controls;
using OMCS.Passive.ShortMessages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GGTalk
{
    internal static class CommonHelper
    {
        /// <summary>
        /// 跳转到官网
        /// </summary>
        public static void JumpToWebsite()
        {
            if (!string.IsNullOrEmpty(GlobalResourceManager.CompanyUrl))
            {
                System.Diagnostics.Process.Start(GlobalResourceManager.CompanyUrl);
            }
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
            return boolStr.ToLower() == "true";
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
        /// 获取文件类型
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public static FileType GetFileType(string fileName)
        {
            try
            {
                String suffix = fileName.Substring(fileName.LastIndexOf('.') + 1);
                switch (suffix.ToLower())
                {
                    case "txt":
                        return FileType.Txt;
                    case "xls":
                    case "xlsx":
                        return FileType.Excel;
                    case "doc":
                    case "docx":
                        return FileType.Word;
                    case "ppt":
                    case "pptx":
                        return FileType.PPT;
                    case "pdf":
                        return FileType.PDF;
                    case "zip":
                    case "rar":
                        return FileType.Rar;
                    case "exe":
                        return FileType.Exe;
                    case "gif":
                    case "jpg":
                    case "jpeg":
                    case "png":
                    case "bmp":
                    case "ico":
                    case "svg":
                    case "tiff":
                        return FileType.Image;
                    case "mp3":
                    case "wma":
                    case "wav":
                    case "aif":
                    case "aiff":
                    case "au":
                    case "ram":
                    case "mid":
                    case "rmi":
                        return FileType.Music;
                    case "mp4":
                    case "avi":
                    case "mov":
                    case "rmvb":
                    case "rm":
                    case "flv":
                    case "3gp":
                    case "mpeg":
                    case "mpg":
                    case "dat":
                    case "mkv":
                        return FileType.Video;
                    default:
                        return FileType.Other;
                }

            }
            catch (Exception e)
            {
                return FileType.None;
            }
        }


        /// <summary>
        /// 获取文件默认图标
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Bitmap GetFileIconBitmap(string fileName)
        {
            try
            {
                FileType fileType = GetFileType(fileName);
                string iconDirectory = "FileIcon/";
                string iconName = "file_type_unknown.png";
                switch (fileType)
                {
                    case FileType.Video:
                        iconName = "file_type_video.png";
                        break;
                    case FileType.Music:
                        iconName = "file_type_music.png";
                        break;
                    case FileType.Rar:
                        iconName = "file_type_rar.png";
                        break;
                    case FileType.PPT:
                        iconName = "file_type_ppt.png";
                        break;
                    case FileType.Word:
                        iconName = "file_type_word.png";
                        break;
                    case FileType.Excel:
                        iconName = "file_type_excel.png";
                        break;
                    case FileType.Txt:
                        iconName = "file_type_txt.png";
                        break;
                    case FileType.PDF:
                        iconName = "file_type_pdf.png";
                        break;
                    case FileType.Image:
                        iconName = "file_type_image.png";
                        break;
                    case FileType.Other:
                        iconName = "file_type_unknown.png";
                        break;
                    case FileType.None:
                        iconName = "file_type_folder.png";
                        break;
                }
                return new Bitmap(iconDirectory + iconName);
            }
            catch (Exception ee)
            {
                return null;
            }

        }

        /// <summary>
        /// 指定最大长度，获取图片对尺寸 （超过最大长度，以最长边的长度 为最大长度值）
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        public static Size GetBitmapMaxSize(Image bitmap, int maxSize)
        {
            if (bitmap == null) { return new Size(0, 0); }
            if (bitmap.Size.Width <= maxSize && bitmap.Size.Height <= maxSize)
            {
                return bitmap.Size;
            }
            double width = 0d, height = 0d;
            if (bitmap.Size.Width > bitmap.Size.Height)
            {
                width = maxSize;
                height = maxSize * bitmap.Size.Height / bitmap.Size.Width;
            }
            else
            {
                height = maxSize;
                width = maxSize * bitmap.Size.Width / bitmap.Size.Height;
            }
            return new Size((int)width, (int)height);
        }

        /// <summary>
        /// 获取语音消息秒数
        /// </summary>
        /// <param name="audioMessage"></param>
        /// <returns></returns>
        public static int GetAudioMessageSecs(this AudioMessage audioMessage)
        {
            int audioMessageSecs = audioMessage.SpanInMSecs / 1000 + 1;
            return audioMessageSecs;
        }

        /// <summary>
        /// 解析消息记录中的文字和表情
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        internal static string ParseChatBoxContent(ChatBoxContent chatBoxContent)
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

        internal static string GetPathToSave(string title, string defaultName, string iniDir)
        {
            string extendName = Path.GetExtension(defaultName);
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = string.Format("The Files (*{0})|*{0}", extendName);
            saveDlg.FileName = defaultName;
            saveDlg.InitialDirectory = iniDir;
            saveDlg.OverwritePrompt = false;
            if (title != null)
            {
                saveDlg.Title = title;
            }

            DialogResult res = saveDlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                return saveDlg.FileName;
            }

            return null;
        }


        //internal static void UnReadHandle(string destUnitID, string speakerID, byte[] content)
        //{
        //    return;
        //    UnReadMsgBox.Singleton.Add(destUnitID, speakerID, content);
        //}

        //internal static void ClearUnReadMsg(string unitID)
        //{
        //    UnReadMsgBox.Singleton.Remove(unitID);
        //}


    }


    public enum FileType
    {
        None,
        Other,
        Txt,
        Excel,
        Word,
        PPT,
        PDF,
        Rar,
        Exe,
        Image,
        Music,
        Video,
    }
}
