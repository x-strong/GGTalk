using System;
using System.Collections.Generic;
using System.Text;
using ESBasic;
using System.Runtime.InteropServices;

namespace ESFramework.Boost.Basic
{
    /// <summary>
    /// 网络状态监控器。当本地网络连接状态发生变化时，将触发NetworkStateChanged事件来通知外部。
    /// ???? 在某些电脑上与视频组件有冲突。
    /// </summary>
    public class NetworkMonitor
    {
        /// <summary>
        /// 当网络状态发生变化时，触发该事件。事件参数：当前网络是否连接上？
        /// </summary>
        public event CbGeneric<bool> NetworkStateChanged;

        private bool? connected = null;       
        private bool useEventNotify = false;
        private int detectSpanInSecs = 5;

        /// <summary>
        /// 初始化检测器。
        /// </summary>
        /// <param name="detectSpan">每隔几秒检测一次网络状态。</param>
        public void Initialize(int detectSpan, bool _useEventNotify)
        {
            this.useEventNotify = _useEventNotify;
            this.detectSpanInSecs = detectSpan;

            CbGeneric cb = new CbGeneric(this.DetectThread);
            cb.BeginInvoke(null, null);
        }

        private void DetectThread()
        {
            while (true)
            {
                try
                {
                    System.Threading.Thread.Sleep(this.detectSpanInSecs * 1000);

                    bool online = false;
                    if (this.useEventNotify)
                    {
                        online = this.IsNetworkConnected();
                    }
                    else
                    {
                        online = this.IsNetworkConnected2();
                    }

                    if (this.connected == null)
                    {
                        this.connected = online;
                        continue;
                    }

                    if (this.connected.Value != online)
                    {
                        this.connected = online;
                        if (this.NetworkStateChanged != null)
                        {
                            this.NetworkStateChanged(online);
                        }
                    }
                }
                catch (Exception ee)
                {
                }
            }
        }

        #region IsNetworkConnected
        /// <summary>
        /// 可以及时反应网络连通情况，但是需要服务System Event Notification支持（系统默认自动启动该服务）
        /// </summary>       
        public bool IsNetworkConnected()
        {
            int flags;//上网方式 
            bool online = IsNetworkAlive(out flags);
            return online;

            #region details
            int NETWORK_ALIVE_LAN = 0;
            int NETWORK_ALIVE_WAN = 2;
            int NETWORK_ALIVE_AOL = 4;
            string outPut = null;
            if (online)//在线   
            {
                if ((flags & NETWORK_ALIVE_LAN) == NETWORK_ALIVE_LAN)
                {
                    outPut = "在线：NETWORK_ALIVE_LAN\n";
                }
                if ((flags & NETWORK_ALIVE_WAN) == NETWORK_ALIVE_WAN)
                {
                    outPut = "在线：NETWORK_ALIVE_WAN\n";
                }
                if ((flags & NETWORK_ALIVE_AOL) == NETWORK_ALIVE_AOL)
                {
                    outPut = "在线：NETWORK_ALIVE_AOL\n";
                }
            }
            else
            {
                outPut = "不在线\n";
            }
            #endregion
        }

        /// <summary>
        /// 对网络状况不能及时反应
        /// </summary>        
        public bool IsNetworkConnected2()
        {
            int flags;//上网方式 
            bool online = InternetGetConnectedState(out flags, 0);
            return online;

            #region Details
            int INTERNET_CONNECTION_MODEM = 1;
            int INTERNET_CONNECTION_LAN = 2;
            int INTERNET_CONNECTION_PROXY = 4;
            int INTERNET_CONNECTION_MODEM_BUSY = 8;
            string outPut = null;
            if (online)//在线   
            {
                if ((flags & INTERNET_CONNECTION_MODEM) == INTERNET_CONNECTION_MODEM)
                {
                    outPut = "在线：拨号上网\n";
                }
                if ((flags & INTERNET_CONNECTION_LAN) == INTERNET_CONNECTION_LAN)
                {
                    outPut = "在线：通过局域网\n";
                }
                if ((flags & INTERNET_CONNECTION_PROXY) == INTERNET_CONNECTION_PROXY)
                {
                    outPut = "在线：代理\n";
                }
                if ((flags & INTERNET_CONNECTION_MODEM_BUSY) == INTERNET_CONNECTION_MODEM_BUSY)
                {
                    outPut = "MODEM被其他非INTERNET连接占用\n";
                }
            }
            else
            {
                outPut = "不在线\n";
            }
            #endregion
        }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);
        [DllImport("sensapi.dll")]
        private extern static bool IsNetworkAlive(out int connectionDescription);
        #endregion     
    }
}
