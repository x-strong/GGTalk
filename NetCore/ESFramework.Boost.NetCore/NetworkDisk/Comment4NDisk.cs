using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Serialization;

namespace ESFramework.Boost.NetworkDisk
{
    /// <summary>
    /// 在网盘的环境中解析BeginSendFile方法的comment参数。
    /// </summary>
    public static class Comment4NDisk
    {
        private const string Prefix = "NDisk:";
        public static NDiskParameters Parse(string comment)
        {
            if (comment == null || !comment.StartsWith(Comment4NDisk.Prefix))
            {
                return null;
            }
            if (!comment.Contains(",ESFramework.Boost.MSide.NetCore"))
            {
                comment = comment.Replace(",ESFramework.Boost.MSide", ",ESFramework.Boost.MSide.NetCore");                
            }
            return (NDiskParameters)SpringFox.ObjectXml(comment.Substring(Comment4NDisk.Prefix.Length));
        }

        public static string BuildComment(string directoryPath ,string netDiskID)
        {
            NDiskParameters para = new NDiskParameters(directoryPath, netDiskID);
            string xml = SpringFox.XmlObject(para);
            return Comment4NDisk.Prefix + xml;
        }

        /// <summary>
        /// 获取真正的路径
        /// </summary>
        /// <param name="rootPath"></param>
        /// <returns></returns>
        public static string GetRealPath(string rootPath)
        {
            if (string.IsNullOrEmpty(rootPath))
            {
                return string.Empty;
            }
            rootPath = rootPath.Replace("//", "/");
            return rootPath.Replace('\\', '/');
        }
    }

    [Serializable]
    public class NDiskParameters
    {
        public NDiskParameters() { }
        public NDiskParameters(string path, string id)
        {
            this.DirectoryPath = path;
            this.NetDiskID = id;
        }

        #region DirectoryPath
        private string directoryPath;
        public string DirectoryPath
        {
            get { return directoryPath; }
            set { directoryPath = value; }
        } 
        #endregion

        #region NetDiskID
        private string netDiskID = "";
        public string NetDiskID
        {
            get { return netDiskID; }
            set { netDiskID = value ?? ""; }
        } 
        #endregion
    }
}
