using CPF.Controls;
using CPF.Drawing;
using CPF.Styling;
using GGTalk.Linux.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GGTalk.Linux.Helpers
{
    internal class FileHelper
    {
        /// <summary>
        /// 文件夹分隔符 linux：/  windows：\
        /// </summary>
        public static readonly char FolderSeparatorChar = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows) ? '\\' : '/';

        //static FileHelper()
        //{
        //    Environment.GetFolderPath(Environment.SpecialFolder.Desktop).Contains('/');
        //}

        #region 项目资源文件

        public static Bitmap GetBitmap(string fullPath)
        {
            try
            {
                Image image = null;
                ResourceManager.GetImage(fullPath, i => { image = i; });
                return new Bitmap(image);
            }
            catch (Exception ee)
            {
                GlobalResourceManager.WriteErrorLog(ee);
                return null;
            }
        }


        /// <summary>
        /// 获取Resources资源目录下的图片
        /// </summary>
        /// <param name="filePath">Resources下的文件路径（不需填写Resources之前的 如 32.ioc 或者 Head/0.png）</param>
        /// <returns></returns>
        public static Bitmap FindAssetsBitmap(string filePath)
        {
            try
            {
                string fullPath = string.Format("res://{0}/Resources/{1}", CommonOptions.ProcessName, filePath);
                Image image = null;
                ResourceManager.GetImage(fullPath, i => { image = i; });
                if (image == null) { return null; }
                return new Bitmap(image);                
            }
            catch (Exception ee)
            {
                GlobalResourceManager.WriteErrorLog(ee);
                return null;
            }
        }


        /// <summary>
        /// 获取Assets文件夹下指定的文件流
        /// </summary>
        /// <param name="filePath">Resources下的文件路径（不需填写Resources之前的 如 32.ioc 或者 Head/0.png）</param>
        /// <returns></returns>
        public static Stream FindAssetsStream(string filePath)
        {
            try
            {
                string fullPath = string.Format("res://{0}/Resources/{1}", CommonOptions.ProcessName, filePath);
                Stream stream = null;
                FileHelper.FindAssetsStream(fullPath,s=> { stream = s; });
                return stream;

            }
            catch (Exception ee)
            {
                GlobalResourceManager.WriteErrorLog(ee);
            }
            return null;
        }

        public static void FindAssetsStream(string filePath ,Action<Stream> action)
        {
            try
            {
                string fullPath = string.Format("res://{0}/Resources/{1}", CommonOptions.ProcessName, filePath);
                Task<Stream> task = ResourceManager.GetStream(fullPath);
                task.ConfigureAwait(true);
                if (action != null) {
                    action.Invoke(task.Result);
                }
            }
            catch (Exception ee)
            {
                GlobalResourceManager.WriteErrorLog(ee);
            }
        }

        #endregion


        /// <summary>
        /// 打开选择文件对话框并执行对应的委托
        /// </summary>
        /// <param name="parent">父窗体</param>
        /// <param name="title"></param>
        /// <param name="action">对话框标题</param>
        public async static void FileToOpen4Action(Window parent,string title, bool allowMultiple, Action<string[]> action)
        {
            try
            {
                Task<string[]> task = OpenFileDialog(parent, title, allowMultiple);
                await task.ConfigureAwait(false);
                if (task.Result == null) { return; }
                if (action != null) { UiSafeInvoker.ActionOnUI<string[]>(action, task.Result); }
            }
            catch (Exception ee)
            {
                GlobalResourceManager.WriteErrorLog(ee);
            }

        }

        /// <summary>
        /// 打开(指定扩展名)选择文件对话框并执行对应的委托
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="title"></param>
        /// <param name="extensions"></param>
        /// <param name="allowMultiple"></param>
        /// <param name="action"></param>
        public async static void FileToOpen4Action(Window parent, string title, string[] extensions, bool allowMultiple, Action<string[]> action)
        {
            try
            {
                GlobalResourceManager.Logger.LogWithTime("准备打开文件对话框");
                Task<string[]> task = OpenFileDialog(parent, title, extensions, allowMultiple);
                await task.ConfigureAwait(false);
                if (task.Result == null) { return; }
                GlobalResourceManager.Logger.LogWithTime("获得文件结果："+ task.Result);
                if (action != null) { UiSafeInvoker.ActionOnUI<string[]>(action, task.Result);}
            }
            catch (Exception ee)
            {
                GlobalResourceManager.WriteErrorLog(ee);
            }

        }

        /// <summary>
        /// 打开选择文件对话框并执行对应的委托
        /// </summary>
        /// <param name="parent">父窗体</param>
        /// <param name="title">对话框标题</param>
        /// <param name="func"></param>
        public async static void GetFileToOpen4Action(Window parent, string title, Func<string[],string[]> func)
        {
            try
            {
                Task<string[]> task = OpenFileDialog(parent, title, true);
                await task.ConfigureAwait(false);
                if (task.Result == null) { return; }
                if (func != null) { func.Invoke(task.Result); }
            }
            catch (Exception ee)
            {
                GlobalResourceManager.WriteErrorLog(ee);
            }
        }

        /// <summary>
        /// 打开选择文件对话框 (指定扩展名)
        /// </summary>
        /// <param name="parent">父窗体</param>
        /// <param name="title">对话框标题</param>
        /// <param name="extensions">后缀名</param>
        /// <param name="allowMultiple">是否允许多选</param>
        /// <returns></returns>
        public static Task<string[]> OpenFileDialog(Window parent, string title, string[] extensions, bool allowMultiple = false)
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.AllowMultiple = allowMultiple;
                    if (extensions != null)
                    {
                        FileDialogFilter filter = new FileDialogFilter() { Name = "*" };
                        string extensionStr = ESBasic.Helpers.StringHelper.ContactString<string>(",", extensions);
                        filter.Extensions = extensionStr; // new List<string>(extensions);
                        openFileDialog.Filters = new List<FileDialogFilter>() { filter };
                    }
                    if (!string.IsNullOrEmpty(title))
                    {
                        openFileDialog.Title = title;
                    }
                    return openFileDialog.ShowAsync(parent);
                }
                else {
                    FolderBrowserWindow folderBrowserWindow = new FolderBrowserWindow(title);
                    folderBrowserWindow.Extensions = extensions;
                    return folderBrowserWindow.ShowDialog_Open(parent);
                }
            }
            catch (Exception ee)
            {
                GlobalResourceManager.WriteErrorLog(ee);
            }
            return null;
        }

        /// <summary>
        /// 打开选择文件对话框
        /// </summary>
        /// <param name="parent">父窗体</param>
        /// <param name="title">对话框标题</param>
        /// <param name="allowMultiple">是否允许多选</param>
        /// <returns></returns>
        public static Task<string[]> OpenFileDialog(Window parent, string title,bool allowMultiple=false)
        {
             return OpenFileDialog(parent, title, null, allowMultiple);
        }

        public static Task<string> OpenFolderDialog(Window parent, string title)
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    OpenFolderDialog openFolderDialog = new OpenFolderDialog();
                    if (!string.IsNullOrEmpty(title))
                    {
                        openFolderDialog.Title = title;
                    }
                    return openFolderDialog.ShowAsync(parent);
                }
                FolderBrowserWindow folderBrowserWindow = new FolderBrowserWindow(title);
                return folderBrowserWindow.ShowDialog_SelectFolder(parent);

            }
            catch (Exception ee)
            {
                GlobalResourceManager.WriteErrorLog(ee);
            }
            return null;
        }

        public static Task<string> SaveFileDialog(Window parent, string title, string fileDirName = null)
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    if (!string.IsNullOrEmpty(title))
                    {
                        saveFileDialog.Title = title;
                    }
                    if (!string.IsNullOrEmpty(fileDirName))
                    {
                        fileDirName = ESBasic.Helpers.FileHelper.GetFileNameNoPath(fileDirName);
                        saveFileDialog.InitialFileName = fileDirName;
                    }
                    return saveFileDialog.ShowAsync(parent);
                }
                FolderBrowserWindow folderBrowserWindow = new FolderBrowserWindow(title);
                return folderBrowserWindow.ShowDialog_Save(parent, fileDirName);
            }
            catch (Exception ee)
            {
                GlobalResourceManager.WriteErrorLog(ee);
            }
            return null;

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
                string iconDirectory = "fileIcon/";
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
                return FindAssetsBitmap(iconDirectory + iconName) ;
            }
            catch (Exception ee)
            {
                return null;
            }
           
        }
    }

    internal enum FileChooserAction
    {
        /// <summary>
        /// 选择文件
        /// </summary>
        Open,

        /// <summary>
        /// 保存文件
        /// </summary>
        Save,

        /// <summary>
        /// 选择文件夹
        /// </summary>
        SelectFolder,
    }

    internal enum FileType
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

    #region FileOrDirectoryTag
    internal class FileOrDirectoryTag: INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public FileOrDirectoryTag() { }
        public FileOrDirectoryTag(string _name, long _size, DateTime time, bool _isFile)
        {
            this.name = _name;
            this.size = _size;
            this.creatTime = time;
            this.isFile = _isFile;
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(FullName)));
                }
            }
        }

        private string fullName;
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(FullName)));
                }
            }
        }

        private long size;
        public long Size
        {
            get { return size; }
            set { size = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Size)));
                }
            }
        }

        private DateTime creatTime;
        public DateTime CreatTime
        {
            get { return creatTime; }
            set { creatTime = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(CreatTime)));
                }
            }
        }

        private bool isFile = true;
        public bool IsFile
        {
            get { return isFile; }
            set { isFile = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(IsFile)));
                }
            }
        }

        private string toolTip = null;
        public string ToolTip
        {
            get { return toolTip; }
            set
            {
                toolTip = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ToolTip)));
                }
            }
        }

        public Bitmap FileIco {

            get { return this.isFile ? FileHelper.GetFileIconBitmap(this.Name) : GGTalk.Linux.Helpers.FileHelper.FindAssetsBitmap("fileIcon/file_type_folder.png"); }
        }
    }
    #endregion
}
