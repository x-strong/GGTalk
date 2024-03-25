using ESFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalkBase
{
    /// <summary>
    /// 在离线文件的环境中解析BeginSendFile方法的comment参数。
    /// </summary>
    public static class Comment4OfflineFile
    {
        private const string Prefix = "OfflineFile:";
        public static string ParseUserID(string comment)
        {
            ClientType senderType = ClientType.Others;
            return ParseUserID(comment, out senderType);
        }

        /// <summary>
        /// 在离线文件场景中，解析发送者或接收者UserID。
        /// 在发送离线文件给服务器的情景中，comment中包含的是接收者的UserID。
        /// 在服务器转发文件给接收者的情景中，comment中包含的是发送者的UserID。
        /// </summary>        
        public static string ParseUserID(string comment ,out ClientType senderType)
        {
            if (comment == null || !comment.StartsWith(Comment4OfflineFile.Prefix))
            {
                senderType = ClientType.Others;
                return null;
            }

            string tmp = comment.Substring(Comment4OfflineFile.Prefix.Length);
            string[] ary = tmp.Split('-');
            senderType = (ClientType)int.Parse(ary[0]);
            return ary[1];
        }

        /// <summary>
        /// 在离线文件场景中，构造comment。
        /// 在发送离线文件给服务器的情景中，userID传的是接收者ID。
        /// 在服务器转发文件给接收者的情景中，userID传的是发送者ID。
        /// </summary>       
        public static string BuildComment(string userID ,ClientType senderType)
        {
            return Comment4OfflineFile.Prefix + ((int)senderType).ToString() + "-" + userID;
        }
    }
}
