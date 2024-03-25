using ESPlus.Rapid;
using ESPlus.Serialization;
using System;
using System.Text;
using TalkBase;

namespace GGTalk
{
    internal class ClientBusinessOutter
    {
        private IRapidPassiveEngine engine;

        private static ClientBusinessOutter clientBusinessOutter;
        public static ClientBusinessOutter Instance
        {
            get {
                if (clientBusinessOutter == null)
                {
                    clientBusinessOutter = new ClientBusinessOutter();
                }
                return clientBusinessOutter;
            }            
        }

        public void Initialize(IRapidPassiveEngine rapidPassiveEngine)
        {
            this.engine = rapidPassiveEngine;
        }


        public string GetUserPhone(string userID)
        {
            byte[] bUser = this.engine.CustomizeOutter.Query(BusinessInfoTypes.GetUserPhone, Encoding.UTF8.GetBytes(userID));
            if (bUser == null)
            {
                return string.Empty;
            }
            return Encoding.UTF8.GetString(bUser);
        }

        /// <summary>
        /// 修改自己的电话号码 （个人信息的版本号一同更改）
        /// </summary>
        /// <param name="newPhone"></param>
        public ChangePhoneResult ChangeMyPhone(string newPhone)
        {
            byte[] data = this.engine.CustomizeOutter.Query(BusinessInfoTypes.ChangeMyPhone, Encoding.UTF8.GetBytes(newPhone));
            return (ChangePhoneResult)BitConverter.ToInt32(data, 0);
        }

    }
}
