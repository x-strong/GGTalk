using ESPlus.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TalkBase
{
    /// <summary>
    /// 功能选项、系统设定。
    /// </summary>
    public static class FunctionOptions
    {
        /// <summary>
        /// 好友默认分组的名称。
        /// </summary>
        public const string DefaultFriendCatalog = "我的好友";

        /// <summary>
        /// 黑名单分组的名称。
        /// </summary>
        public const string BlackListCatalogName = "黑名单";

        /// <summary>
        /// 群默认分组的名称。
        /// </summary>
        public const string DefaultGroupCatalog = "我的群";

        /// <summary>
        /// 文件默认分隔字符
        /// </summary>
        public static readonly char FilePathSeparatorChar = ESBasic.Helpers.FileHelper.GetFilePathSeparatorChar();

        /// <summary>
        /// 组ID的前缀。 
        /// </summary>
        public const string PrefixGroupID = "*";

        /// <summary>
        /// 注册账号前缀（帐号由服务端随机生成）
        /// </summary>
        public const string RegistActionToken = "#Reg:";

        /// <summary>
        /// 注册账号前缀（帐号由申请用户指定）
        /// </summary>
        public const string RegistActionToken2 = "#Reg2:";

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
        /// 是否启用快捷回复。
        /// </summary>
        public const bool ShortcutResponse = true;

        /// <summary>
        /// 是否允许为好友备注名称。
        /// </summary>
        public const bool FriendCommentName = true;

        /// <summary>
        /// 是否启用黑名单
        /// </summary>
        public const bool BlackList = true;

        /// <summary>
        /// 是否启用离线群消息。
        /// </summary>
        public const bool OfflineGroupOrgChatMessage = false;

        /// <summary>
        /// 客户端用户关注的联系人的类型。影响到：启动时更新用户资料（第一次启动时仍然加载所有用户），用户资料（基础资料、头像、业务数据）变更通知。
        /// 至于上下线通知是否要受此影响，可在具体应用的服务端实现IContactsManager的GetContacts方法时确定。
        /// </summary>
        public static ContactsCareType ContactsCareType = ContactsCareType.All;

        /// <summary>
        /// 用户头像的边长。
        /// </summary>
        public static int HeadImageLength = 100;
        public static Size GetHeadImageSize()
        {
            return new Size(FunctionOptions.HeadImageLength, FunctionOptions.HeadImageLength);
        }

    }

    /// <summary>
    /// 客户端用户关注的联系人的类型。
    /// 注意：初次启动时，仍然是加载整个组织结构的。 
    /// </summary>
    public enum ContactsCareType
    {
        /// <summary>
        /// 所有人。
        /// </summary>
        All = 0 ,

        /// <summary>
        /// 仅好友。好友之外的联系人的资料需手动更新，或第一次打开聊天框的时候更新。
        /// </summary>
        FriendsOnly,

        /// <summary>
        /// 都不关心。所有联系人的资料都需手动更新，或第一次打开聊天框的时候更新。
        /// </summary>
        None
    }
}
