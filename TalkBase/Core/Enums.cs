using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TalkBase
{
    public enum UnitType
    {
        User = 0,
        Group
    }
   
    public enum UserStatus
    {
        Online = 2,
        Away = 3,
        Busy = 4,
        DontDisturb = 5,
        OffLine = 6,
        Hide = 7
    }

    /// <summary>
    /// 用户状态 （正常，冻结，禁言，停用）
    /// </summary>
    public enum UserState
    {
        /// <summary>
        /// 正常
        /// </summary>
        [ESBasic.EnumDescription("正常")]
        Normal,

        /// <summary>
        /// 冻结
        /// </summary>
        [ESBasic.EnumDescription("冻结")]
        Frozen,

        /// <summary>
        /// 禁言
        /// </summary>
        [ESBasic.EnumDescription("禁言")]
        NoSpeaking,

        /// <summary>
        /// 停用
        /// </summary>
        [ESBasic.EnumDescription("停用")]
        StopUsing
    }

    public enum NotifyType
    {
        User = 0,
        Group = 1
    }

    public enum GroupChangedType
    {
        /// <summary>
        /// 成员的资料发生变化
        /// </summary>
        MemberInfoChanged = 0,
        /// <summary>
        /// 组的资料（如组名称、公告等）发生变化
        /// </summary>
        GroupInfoChanged,
        SomeoneJoin,
        SomeoneQuit,
        /// <summary>
        /// 某人被彻底删除。
        /// </summary>
        SomeoneDeleted,
        GroupDeleted,
        /// <summary>
        /// 自己被移除目标组
        /// </summary>
        MyselfBeRemovedFromGroup,
        /// <summary>
        /// 自己被拖进目标组
        /// </summary>
        MyselfBePulledIntoGroup,
        /// <summary>
        /// 其他人被移除目标组
        /// </summary>
        OtherBeRemovedFromGroup,
        /// <summary>
        /// 其他人被拖进目标组
        /// </summary>
        OtherBePulledIntoGroup
    }

    public enum JoinGroupResult
    {
        Succeed = 0,
        GroupNotExist,
    }

    public enum CreateGroupResult
    {
        Succeed = 0,
        GroupExisted,
    }

    public enum ChangePasswordResult
    {
        Succeed = 0,
        OldPasswordWrong,
        UserNotExist
    }

    public enum AddFriendResult
    {
        Succeed = 0,
        FriendNotExist,
    }

    /// <summary>
    /// 交互的类型。比如 请求视频会话，同意视频会话，拒绝视频会话，终止视频会话
    /// </summary>
    public enum CommunicateType
    {
        Request = 0,
        Agree,
        Reject,
        Terminate,
        Busy
    }

    /// <summary>
    /// 交互媒体的类型。
    /// </summary>
    public enum CommunicateMediaType
    {
        Video = 0,
        Audio,
        RemoteHelp,
        RemoteControl,
        RemoteDisk,
        GroupVideo=7
    }

    /// <summary>
    /// 添加好友的类型
    /// </summary>
    public enum RequsetType
    {
        Request = 0,
        Agree = 1,
        Reject = 2,
    }

    public enum RegisterResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 0,

        /// <summary>
        /// 帐号已经存在
        /// </summary>
        Existed,

        /// <summary>
        /// 过程中出现错误
        /// </summary>
        Error,

        /// <summary>
        /// 账号为吉祥号
        /// </summary>
        IdIsLuckNumber,

        /// <summary>
        /// 验证码错误
        /// </summary>
        SmsCodeError,

        /// <summary>
        /// 验证码不存在或已过期
        /// </summary>
        SmsCodeExpired,

        /// <summary>
        /// 手机号码已存在
        /// </summary>
        PhoneExisted,

        /// <summary>
        /// 手机号绑定超过最大数量
        /// </summary>
        PhoneBindExceedMaxNumber
    }

    public enum SendSmsCodeResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 0,

        /// <summary>
        /// 过程中出现错误
        /// </summary>
        Error
    }

    public enum CheckSmsCodeResult
    {
        /// <summary>
        /// 验证码错误
        /// </summary>
        SmsCodeError,

        /// <summary>
        /// 验证码不存在或已过期
        /// </summary>
        SmsCodeExpired
    }


    public enum ResetPasswordResult
    {
        /// <summary>
        /// 过程中出现错误
        /// </summary>
        Error = 0,

        /// <summary>
        /// 成功
        /// </summary>
        Succeed,

        /// <summary>
        /// 用户不存在
        /// </summary>
        UserNotExist,

        /// <summary>
        /// 验证码错误
        /// </summary>
        SmsCodeError,

        /// <summary>
        /// 验证码不存在或已过期
        /// </summary>
        SmsCodeExpired

    }



    public enum SmsCodeType
    {
        Register,
        ResetPassword,
        ChangePhone,
        DismissGroup,
        FindID
    }

    public enum ChangePhoneResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 0,

        /// <summary>
        /// 手机号码已存在
        /// </summary>
        PhoneExisted,


        /// <summary>
        /// 用户不存在
        /// </summary>
        UserNotExist,

        /// <summary>
        /// 过程中出现错误
        /// </summary>
        Error,

        /// <summary>
        /// 手机号绑定超过最大数量
        /// </summary>
        PhoneBindExceedMaxNumber
    }

    public enum ChangeMyPhoneResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 0,

        /// <summary>
        /// 手机号码已存在
        /// </summary>
        PhoneExisted,


        /// <summary>
        /// 用户不存在
        /// </summary>
        UserNotExist,

        /// <summary>
        /// 过程中出现错误
        /// </summary>
        Error
    }


    /// <summary>
    /// 检测内容类型
    /// </summary>
    public enum ContentType
    {
        Text = 0,
        Image = 1
    }

    /// <summary>
    /// 违规类型
    /// </summary>
    public enum EvilType
    {
        /// <summary>
        /// 正常
        /// </summary>
        [ESBasic.EnumDescription("正常")]
        [Description("正常")]
        Normal = 100,

        /// <summary>
        /// 政治
        /// </summary>
        [ESBasic.EnumDescription("政治")]
        [Description("政治")]
        Polity = 20001,

        /// <summary>
        /// 情色
        /// </summary>
        [ESBasic.EnumDescription("情色")]
        [Description("情色")]
        Smut = 20002,

        /// <summary>
        /// 涉毒
        /// </summary>
        [ESBasic.EnumDescription("涉毒")]
        [Description("涉毒")]
        Drug = 20006,

        /// <summary>
        /// 谩骂
        /// </summary>
        [ESBasic.EnumDescription("谩骂")]
        [Description("谩骂")]
        Abuse = 20007,

        /// <summary>
        /// 性感
        /// </summary>
        [ESBasic.EnumDescription("性感")]
        [Description("性感")]
        Hot = 20103,

        /// <summary>
        /// 广告引流
        /// </summary>
        [ESBasic.EnumDescription("广告引流")]
        [Description("广告引流")]
        AD = 20105,

        /// <summary>
        /// 暴恐
        /// </summary>
        [ESBasic.EnumDescription("暴恐")]
        [Description("暴恐")]
        Terror = 24001,

        /// <summary>
        /// 综合
        /// </summary>
        [ESBasic.EnumDescription("综合")]
        [Description("综合")]
        Synthetical = 21000
    }

}
