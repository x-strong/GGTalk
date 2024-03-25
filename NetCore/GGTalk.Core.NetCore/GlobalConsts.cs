using System;
using System.Collections.Generic;
using System.Text;
using ESBasic;

namespace GGTalk
{
    /// <summary>
    /// 全局常量。
    /// </summary>
    public static class GlobalConsts
    {
        /// <summary>
        /// GGConfiguration表中用于存储组织结构版本号的那条记录的key的值。
        /// </summary>
        public const string OrgVersionFieldName = "OrgVersion";

        /// <summary>
        /// 枚举全选值
        /// </summary>
        public const int AllSelected = -1;

        public readonly static int TalkVersion = 40;

        /// <summary>
        /// 普通视频采集分辨率。（长宽之和）
        /// </summary>
        public readonly static int CommonQualityVideo = 700;

        /// <summary>
        /// 高清视频采集分辨率。（长宽之和）
        /// </summary>
        public readonly static int HighQualityVideo = 1000;

        public const string ViewPicture = "查看图片";

        /// <summary>
        /// 获取注册验证码前缀
        /// </summary>
        public const string GetRegistCodeToken = "#GetRegCode:";

        /// <summary>
        /// 获取重置密码 验证码前缀
        /// </summary>
        public const string GetResetPasswordCodeToken = "#GetResetPasswordCode:";

        /// <summary>
        /// 重置密码 前缀
        /// </summary>
        public const string ResetPasswordActionToken = "#ResetPassword:";

        /// <summary>
        /// 根据用户ID获取用户电话号码， 该用户不存在返回 "" 空字符串
        /// </summary>
        public const string GetUserPhoneToken = "#GetUserPhone:";

        /// <summary>
        /// 获取找回帐号验证码 前缀
        /// </summary>
        public const string GetFindIDCodeToken = "#GetFindIDCode:";

        /// <summary>
        /// 找回帐号 前缀
        /// </summary>
        public const string FindIDActionToken = "#FindID:";

    }
}
