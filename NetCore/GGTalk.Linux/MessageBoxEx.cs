using GGTalk.Linux.Helpers;
using GGTalk.Linux.Views;
using System;
using System.Threading.Tasks;

namespace GGTalk.Linux
{
    internal class MessageBoxEx
    {
        public static void Show(string message)
        {
            Show("提示", message);
        }

        public static void Show(string title, string message)
        {
            UiSafeInvoker.ActionOnUI<string, string>(doShow, title, message);
        }

        private static void doShow(string title, string message)
        {
            MessageBoxManager.GetMessageBoxWindow(title, message,ButtonEnum.Ok).Show();
        }

        public static Task<ButtonResult> ShowQuery(string title, string message)
        {
            //return null;
            return ShowQuery(title, message, ButtonEnum.YesNo);
        }

        public static Task<ButtonResult> ShowQuery(string message)
        {
            return ShowQuery("提示", message, ButtonEnum.YesNo);
        }

        public static Task<ButtonResult> ShowQuery(string title,string message, ButtonEnum buttonEnum)
        {
            return MessageBoxManager.GetMessageBoxWindow(title, message, buttonEnum).Show();
        }

        public static Task<ButtonResult> ShowDialogQuery(CPF.Controls.Window ownerWindow, string title, string message)
        {
            return ShowDialogQuery(ownerWindow,title, message, ButtonEnum.YesNo);
        }

        public static Task<ButtonResult> ShowDialogQuery(CPF.Controls.Window ownerWindow, string message)
        {
            return ShowDialogQuery(ownerWindow,"提示", message, ButtonEnum.YesNo);
        }

        public static Task<ButtonResult> ShowDialogQuery(CPF.Controls.Window ownerWindow,string title, string message, ButtonEnum buttonEnum)
        {
            return MessageBoxManager.GetMessageBoxWindow(title, message, buttonEnum).ShowDialog(ownerWindow);
        }


    }
}
