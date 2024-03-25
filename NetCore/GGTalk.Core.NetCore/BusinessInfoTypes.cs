using System;
using System.Collections.Generic;
using System.Text;

namespace GGTalk
{
    /// <summary>
    /// 扩展功能时，自定义的消息类型。
    /// 注意：发送自定义消息时，使用 IRapidPassiveEngine.CustomizeOutter.Send 方法，而不是 IRapidPassiveEngine.SendMessage方法
    /// </summary>
    public static class BusinessInfoTypes
    {

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

        /// <summary>
        /// 新创建的组ID (创建组前调用) （c->s）
        /// </summary>
        public const int CreateGroupID = 24;

        /// <summary>
        /// 管理员开群 （c->s）
        /// </summary>
        public const int CreateGroup4Admin = 25;

        /// <summary8
        /// 管理员开群成功,通知指定的群组刷新群组列表 （s->c）
        /// </summary>
        public const int CreateGroupSuccess4Admin = 2026;

        /// <summary>
        /// 管理员修改指定用户的手机号 （c->s）
        /// </summary>
        public const int ChangePhone4Admin = 2027;

        /// <summary>
        /// 获取违规记录分页 （c->s）
        /// </summary>
        public const int GetSensitiveRecordPage = 30;





        public static bool Contains(int infoType)
        {
            return (infoType >= 0 && infoType <= 200) || infoType >= 2000;
        }
    }

}
