using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGTalk
{
    internal static class CommonOptions
    {
        /// <summary>
        /// 短信验证码总倒计时（秒）
        /// </summary>
        public const int SmsCodeCountdown = 90;

        public const string TwinkleNotifyIcon = "TwinkleNotifyIcon";

        public const string FileAssistantTitle = "文件传输助手";


        /// <summary>
        /// 正在视频或语音通话的对方ID （视频、语音：为对方friendID ；群视频：为动态组VideoGroupID）
        /// </summary>
        public static string CallingID4VideoOrVoice = string.Empty;



    }
}
