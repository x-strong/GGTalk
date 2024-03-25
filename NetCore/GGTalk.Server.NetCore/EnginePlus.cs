using ESFramework;
using ESPlus.Rapid;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGTalk.Server.NetCore
{
    internal class EnginePlus
    {
        private IRapidServerEngine rapidServerEngine;
        public EnginePlus(IRapidServerEngine engine)
        {
            this.rapidServerEngine = engine;
            this.rapidServerEngine.MessageReceived += RapidServerEngine_MessageReceived;
            this.rapidServerEngine.UserManager.UserConnected += UserManager_UserConnected;
            this.rapidServerEngine.UserManager.UserDisconnected += UserManager_UserDisconnected;
            this.rapidServerEngine.ContactsController.BroadcastReceived += ContactsController_BroadcastReceived;
        }

        private void ContactsController_BroadcastReceived(string broadcasterID, string groupID, int broadcastType, byte[] broadcastContent, string tag)
        {

        }

        private void UserManager_UserDisconnected(string userID)
        {
            Console.WriteLine(userID + "断开连接！");
        }

        private void UserManager_UserConnected(string userID)
        {
            Console.WriteLine(userID + "已连接！");
        }

        private void RapidServerEngine_MessageReceived(string sourceUserID, ClientType sourceType, int informationType, byte[] info, string tag)
        {
            Console.WriteLine("收到MessageReceived消息号：" + informationType);
        }
    }
}
