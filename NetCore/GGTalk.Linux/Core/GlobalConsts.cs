using System;
using System.Collections.Generic;
using System.Text;
using ESBasic;

namespace GGTalk
{
    /// <summary>
    /// 全局常量。
    /// </summary>
    internal static class GlobalConsts
    {
        /// <summary>
        /// 图片扩展名
        /// </summary>
        public static string[] ImageExtensions = new string[] { "bmp", "jpeg", "png", "jpg" };

        /// <summary>
        /// Configuration表中用于存储版本号的那条记录的key的值。
        /// </summary>
        public const string OrgVersionFieldName = "OrgVersion";

        /// <summary>
        /// 枚举全选值
        /// </summary>
        public const int AllSelected = -1;

        public readonly static int GGTalkVersion = 40;

        /// <summary>
        /// 普通视频采集分辨率。（长宽之和）
        /// </summary>
        public readonly static int CommonQualityVideo = 700;

        /// <summary>
        /// 高清视频采集分辨率。（长宽之和）
        /// </summary>
        public readonly static int HighQualityVideo = 1000;

        public const string ViewPicture = "查看图片";

    }
}
