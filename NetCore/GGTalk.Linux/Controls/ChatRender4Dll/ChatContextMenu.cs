using CPF.Controls;
using CPF.Drawing;
using CPF;
using System;
using System.Collections.Generic;
using System.Text;
using GGTalk.Linux.Helpers;

namespace OVCS.Client.Linux.Tools
{
    //聊天消息右键菜单
    class ChatContextMenu
    {
        private static OrayChatContextMenuHandler Handler;
        private static ContextMenu Menu;
        private static MenuItem CopyItem;
        private static MenuItem SaveItem;
        private static MenuItem ShowImageItem;
        private static MenuItem ClearItem;

        static ChatContextMenu()
        {
            ChatContextMenu.Menu = new ContextMenu()
            {
                FontFamily = ("微软雅黑"),
                FontStyle = FontStyles.Regular,
            };
            ChatContextMenu.CopyItem = new MenuItem();
            ChatContextMenu.CopyItem.Header = "复制";
            ChatContextMenu.CopyItem.MouseDown += CopyItem_Click;
            ChatContextMenu.SaveItem = new MenuItem();
            ChatContextMenu.SaveItem.Header = "另存为";
            ChatContextMenu.SaveItem.MouseDown += Save_Click;
            ChatContextMenu.ShowImageItem = new MenuItem();
            ChatContextMenu.ShowImageItem.Header = "新窗口显示";
            ChatContextMenu.ShowImageItem.MouseDown += ShowImageItem_Click;
            ChatContextMenu.ClearItem = new MenuItem();
            ChatContextMenu.ClearItem.Header = "清屏";
            ChatContextMenu.ClearItem.MouseDown += ClearItem_Click;
            ChatContextMenu.Menu.Items.Add(ChatContextMenu.CopyItem);
            ChatContextMenu.Menu.Items.Add(ChatContextMenu.SaveItem);
            ChatContextMenu.Menu.Items.Add(ChatContextMenu.ShowImageItem);
            ChatContextMenu.Menu.Items.Add(ChatContextMenu.ClearItem);

        }
        private static void CopyItem_Click(object sender, RoutedEventArgs e)
        {
            ChatContextMenu.Handler.OnCopy();
        }
        private static void Save_Click(object sender, RoutedEventArgs e)
        {
            ChatContextMenu.Handler.OnSave();
        }
        private static void ShowImageItem_Click(object sender, RoutedEventArgs e)
        {
            ChatContextMenu.Handler.OnShowImage();
        }
        private static void ClearItem_Click(object sender, RoutedEventArgs e)
        {
            ChatContextMenu.Handler.OnClear();
        }

        public static void SetCopyVisible(bool isShow)
        {
            ChatContextMenu.CopyItem.Visibility = isShow ? Visibility.Visible : Visibility.Collapsed;
        }
        public static void SetSaveVisible(bool isShow)
        {
            ChatContextMenu.SaveItem.Visibility = isShow ? Visibility.Visible : Visibility.Collapsed;
        }
        public static void SetShowImageVisible(bool isShow)
        {
            ChatContextMenu.ShowImageItem.Visibility = isShow ? Visibility.Visible : Visibility.Collapsed;
        }

        public static void SetHandler(OrayChatContextMenuHandler handler)
        {
            ChatContextMenu.Handler = handler;
        }

        public static void Show()
        {
            UiSafeInvoker.ActionOnUI(() =>
            { 
                Control control = Handler as Control;
                Menu.PlacementTarget = control;
                Menu.IsOpen = true;
                Menu.Invalidate();
            }); 
        }
    }

    interface OrayChatContextMenuHandler
    {

        void OnCopy();

        void OnSave();

        void OnShowImage();

        void OnClear();

    }
}
