using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using ESBasic.ObjectManagement;
using ESBasic.ObjectManagement.Cache;
using GGTalk.Server.Helpers;
using TalkBase;

namespace GGTalk.Server.Managers
{
    /// <summary>
    /// 短信验证码管理器。
    /// </summary>
    public class SmsCodeManager
    {
        private HotCache<string, string> codeManager = new HotCache<string, string>();
        private object locker = new object();
        private object locker_get = new object();

        private static SmsCodeManager instance;
        public static SmsCodeManager Instance
        {
            get {
                if (instance == null)
                {
                    instance = new SmsCodeManager();
                }
                return instance;
            }
        }


        private SmsCodeManager()
        {
            //最后一次访问后N分钟失效
            this.codeManager.MaxMuteSpanInMinutes = 5;
            //每隔N秒检查一次
            this.codeManager.DetectSpanInSecs = 10;          
            this.codeManager.Initialize();
        }

        /// <summary>
        /// 创建验证码
        /// </summary>
        /// <param name="numberFlag">是否是纯数字 （true：纯数字；false：数字加字母）</param>
        /// <param name="length">验证码长度</param>
        /// <returns></returns>
        private string CreateSmsCode(bool numberFlag, int length)
        {
            string verificationCode = "";
            string strTable = numberFlag ? "1234567890" : "1234567890abcdefghijkmnpqrstuvwxyz";
            Random randomNum = new Random(unchecked((int)(DateTime.Now.Ticks)));
            for (int i = 0; i < length; i++)
            {
                int intR = randomNum.Next(strTable.Length);
                verificationCode += strTable[intR];
            }
            return verificationCode;
        }

        /// <summary>
        /// 添加验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        private void AddSmsCode(string phone, string code)
        {
            lock (this.locker)
            {
                this.codeManager.Add(phone, code);
            }
        }

        /// <summary>
        /// 发送6位纯数字验证码
        /// </summary>
        /// <param name="phone">电话号码</param>
        /// <returns></returns>
        public bool SendSmsCode(SmsCodeType smsCodeType, string phone)
        {
            if (!bool.Parse(ConfigurationManager.AppSettings["SmsCodeEnabled"]))
            {
                return true;
            }

            string code = GlobalFunction.CreateRandomStr(true, 6);
            SendSmsResponse response = SmsHelper.SendSms(smsCodeType,phone, code);
            if (response.Code != null && response.Code == "OK")
            {
                this.AddSmsCode(phone, code);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取用户验证码
        /// </summary>
        /// <param name="phone">电话号码</param>
        /// <returns>若不存在该用户验证码则返回null</returns>
        public string GetSmsCode(string phone)
        {
            if (!bool.Parse(ConfigurationManager.AppSettings["SmsCodeEnabled"]))
            {
                return "0000";
            }

            lock (this.locker_get)
            {
                return this.codeManager.Get(phone);
            }
        }
    }
}
