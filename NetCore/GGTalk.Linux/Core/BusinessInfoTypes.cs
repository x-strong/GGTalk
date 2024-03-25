using System;
using System.Collections.Generic;
using System.Text;

namespace GGTalk
{
    /// <summary>
    /// 扩展傲瑞通功能时，自定义的消息类型。
    /// 注意：发送自定义消息时，使用 IRapidPassiveEngine.CustomizeOutter.Send 方法，而不是 IRapidPassiveEngine.SendMessage方法
    /// </summary>
    public static class BusinessInfoTypes
    {


        #region SmsCode

        /// <summary>
        /// 发送注册短信验证码
        /// </summary>
        public const int SendSmsCode = 10;

        /// <summary>
        /// 发送重置密码短信验证码
        /// </summary>
        //public const int SendSmsCode4ResetPassword = 11;

        /// <summary>
        /// 获取短信验证码
        /// </summary>
        public const int GetSmsCode = 19;
        #endregion


        /// <summary>
        /// 重置密码
        /// </summary>
        public const int ResetPassword = 20;

        /// <summary>
        /// 管理员开户
        /// </summary>
        public const int Register4Admin = 21;

        /// <summary>
        /// 获取指定用户手机号码
        /// </summary>
        public const int GetUserPhone = 22;

        /// <summary>
        /// 更改自己已绑定的手机号
        /// </summary>
        public const int ChangeMyPhone = 23;




        public static bool Contains(int infoType)
        {
            return infoType >= 0 && infoType <= 200;
        }
    }

}
