using System;
using System.Collections.Generic;
using System.Text;

namespace GGTalk.Linux
{
    internal class CommonOptions
    {

        /// <summary>
        /// 当前项目的进程名
        /// </summary>
        public const string ProcessName = "GGTalk.Linux";//项目名称

        /// <summary>
        /// 资源路径目录
        /// </summary>
        public const string ResourcesCatalog = "res://GGTalk.Linux/Resources/";

        public static readonly string ResourceDir = AppDomain.CurrentDomain.BaseDirectory + "Resource/";
    }
}
