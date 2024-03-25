using System;
using System.Collections.Generic;
using System.Text;
using ESPlus.FileTransceiver;

namespace ESFramework.Boost
{
    /// <summary>
    /// 枚举描述器。用于将枚举值转换为对应的中文表达。
    /// </summary>
    public static class FileTransHelper
    {

        /// <summary>
        /// 获取文件传送中断时的提示消息。
        /// </summary>  
        public static string GetTipMessage4FileTransDisruptted(string projectName, bool isSender, FileTransDisrupttedType fileTransDisrupttedType, bool convertToOfflineFile)
        {
            string showText = "";
            switch (fileTransDisrupttedType)
            {
                case FileTransDisrupttedType.RejectAccepting:
                    {
                        if (isSender)
                        {
                            showText += string.Format("'{0}'传送失败！{1}", projectName, "对方拒绝接收文件。");
                        }
                        else
                        {
                            showText += string.Format("'{0}'传送失败！{1}", projectName, "您拒绝接收文件。");
                        }
                        break;
                    }
                case FileTransDisrupttedType.ActiveCancel:
                    {
                        if (isSender)
                        {
                            if (convertToOfflineFile)
                            {
                                showText += string.Format("您将'{0}'改为离线发送！", projectName);
                            }
                            else
                            {
                                showText += string.Format("'{0}'传送失败！{1}", projectName, "您取消发送文件。");
                            }
                        }
                        else
                        {
                            showText += string.Format("'{0}'传送失败！{1}", projectName, "您取消接收文件。");
                        }
                        break;
                    }
                case FileTransDisrupttedType.DestCancel:
                    {
                        if (isSender)
                        {
                            showText += string.Format("'{0}'传送失败！{1}", projectName, "对方取消接收文件。");
                        }
                        else
                        {
                            if (convertToOfflineFile)
                            {
                                showText += string.Format("对方取消在线传输，转为发送离线文件'{0}'。", projectName);
                            }
                            else
                            {
                                showText += string.Format("'{0}'传送失败！{1}", projectName, "对方取消发送文件。");
                            }
                        }

                        break;
                    }
                case FileTransDisrupttedType.DestOffline:
                    {
                        showText += string.Format("'{0}'传送失败！{1}", projectName, "对方掉线！");
                        break;
                    }
                case FileTransDisrupttedType.SelfOffline:
                    {
                        showText += string.Format("'{0}'传送失败！{1}", projectName, "您已经掉线！");
                        break;
                    }
                case FileTransDisrupttedType.DestInnerError:
                    {
                        showText += string.Format("'{0}'传送失败！{1}", projectName, "对方系统内部错误。");
                        break;
                    }
                case FileTransDisrupttedType.InnerError:
                    {
                        showText += string.Format("'{0}'传送失败！{1}", projectName, "本地系统内部错误。");
                        break;
                    }
                case FileTransDisrupttedType.ReliableP2PChannelClosed:
                    {
                        showText += string.Format("'{0}'传送失败！{1}", projectName, "与对方可靠的P2P通道已经关闭。");
                        break;
                    }
                case FileTransDisrupttedType.NetworkSpeedSlow:
                    {
                        showText += string.Format("'{0}'传送失败！{1}", projectName, "网速太慢或网络繁忙！");
                        break;
                    }
                case FileTransDisrupttedType.SendThreadNotStarted:
                    {
                        showText += string.Format("'{0}'传送失败！{1}", projectName, "SendThreadNotStarted！");
                        break;
                    }
                case FileTransDisrupttedType.Timeout4FirstPackage:
                    {
                        showText += string.Format("'{0}'传送失败！{1}", projectName, "Timeout4FirstPackage！");
                        break;
                    }
                case FileTransDisrupttedType.AnswerFileRequestOnOtherDevice:
                    {
                        showText += string.Format("已经在其它设备上回应了对方传送'{0}'文件的请求！", projectName);
                        break;
                    }

            }

            return showText;
        }


        /// <summary>
        /// 获取文件传送中断时的提示消息。(不带文件名称)
        /// </summary>  
        public static string GetTipMessage4FileTransDisruptted(bool isSender, FileTransDisrupttedType fileTransDisrupttedType, bool convertToOfflineFile)
        {
            string showText = "";
            switch (fileTransDisrupttedType)
            {
                case FileTransDisrupttedType.RejectAccepting:
                    {
                        if (isSender)
                        {
                            showText += "对方拒绝接收文件。";
                        }
                        else
                        {
                            showText += "您拒绝接收文件。";
                        }
                        break;
                    }
                case FileTransDisrupttedType.ActiveCancel:
                    {
                        if (isSender)
                        {
                            if (convertToOfflineFile)
                            {
                                showText += "您改为离线发送！";
                            }
                            else
                            {
                                showText += "您取消发送文件。";
                            }
                        }
                        else
                        {
                            showText += "您取消接收文件。";
                        }
                        break;
                    }
                case FileTransDisrupttedType.DestCancel:
                    {
                        if (isSender)
                        {
                            showText += "对方取消接收文件。";
                        }
                        else
                        {
                            if (convertToOfflineFile)
                            {
                                showText += "对方取消在线传输，转为发送离线文件。";
                            }
                            else
                            {
                                showText += "对方取消发送文件。";
                            }
                        }

                        break;
                    }
                case FileTransDisrupttedType.DestOffline:
                    {
                        showText += "对方掉线！";
                        break;
                    }
                case FileTransDisrupttedType.SelfOffline:
                    {
                        showText += "您已经掉线！";
                        break;
                    }
                case FileTransDisrupttedType.DestInnerError:
                    {
                        showText += "对方系统内部错误。";
                        break;
                    }
                case FileTransDisrupttedType.InnerError:
                    {
                        showText += "本地系统内部错误。";
                        break;
                    }
                case FileTransDisrupttedType.ReliableP2PChannelClosed:
                    {
                        showText += "与对方可靠的P2P通道已经关闭。";
                        break;
                    }
                case FileTransDisrupttedType.NetworkSpeedSlow:
                    {
                        showText += "网速太慢或网络繁忙！";
                        break;
                    }
                case FileTransDisrupttedType.SendThreadNotStarted:
                    {
                        showText += "SendThreadNotStarted！";
                        break;
                    }
                case FileTransDisrupttedType.Timeout4FirstPackage:
                    {
                        showText += "Timeout4FirstPackage！";
                        break;
                    }
                case FileTransDisrupttedType.AnswerFileRequestOnOtherDevice:
                    {
                        showText += "已经在其它设备上回应了对方的请求！";
                        break;
                    }
            }

            return showText;
        }

    }
}
