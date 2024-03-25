using System;
using System.Collections.Generic;
using System.Text;
using ESPlus.Application.CustomizeInfo;
using ESFramework;

namespace GGTalk
{
    /// <summary>
    /// 扩展功能时，客户端自定义消息的处理器。
    /// </summary>
    class ClientBusinessHandler : IIntegratedCustomizeHandler
    {
        public bool CanHandle(int informationType)
        {
            return BusinessInfoTypes.Contains(informationType);
        }

        //对应 IRapidPassiveEngine.CustomizeOutter.Send 方法，而不是 IRapidPassiveEngine.SendMessage方法
        public void HandleInformation(string sourceUserID, ClientType clientType, int informationType, byte[] info)
        {

        }

        public byte[] HandleQuery(string sourceUserID, ClientType clientType, int informationType, byte[] info)
        {
            return null;
        }
    }
}
