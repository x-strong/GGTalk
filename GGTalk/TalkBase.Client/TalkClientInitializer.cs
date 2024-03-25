using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TalkBase.Client
{
    /// <summary>
    /// TalkBase客户端初始化器。
    /// </summary>
    public static class TalkClientInitializer
    {
        public static void Initialize(Icon icon64 ,Image back, Dictionary<uint, Image> emotion)
        {
            TalkClientInitializer.Icon64 = icon64;
            TalkClientInitializer.MainBackImage = back;
            TalkClientInitializer.EmotionDictionary = emotion;
        }

        public static Icon Icon64;
        public static Image MainBackImage;
        public static Dictionary<uint, Image> EmotionDictionary;
    }
}
