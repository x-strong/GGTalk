using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGTalk.Server
{
    public class GlobalOptions
    {
        /// <summary>
        /// 默认UserID，  新注册用户可以默认添加该账号为好友
        /// </summary>
        public const string DefaultID = "10000";

        /// <summary>
        /// 注册成功是否添加默认好友
        /// </summary>
        public const bool IsAddDefaultFirend4Register = false;
    }
}
