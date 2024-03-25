using ESPlus.Advanced;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ESBasic;
using ESBasic.Loggers;

namespace ESFramework.Boost
{
    public class DignosticLogger : IDisposable
    {
        private IDiagnosticsViewer diagnosticsViewer;
        private IAgileLogger agileLogger;
        private int spanInSecs = 30;
        private volatile bool stop = false;

        public DignosticLogger(IDiagnosticsViewer viewer ,string logFilePath, int logSpanInSecs)
        {            
            this.diagnosticsViewer = viewer;
            this.agileLogger = new FileAgileLogger(logFilePath);
            this.spanInSecs = logSpanInSecs;

            if (this.diagnosticsViewer != null)
            {
                CbGeneric cb = new CbGeneric(this.LogThread);
                cb.BeginInvoke(null, null);
            }
        }

        private void LogThread()
        {
            int span = this.spanInSecs * 1000;
            while (!this.stop)
            {
                System.Threading.Thread.Sleep(span);
                this.Log();
            }
        }


        private void Log()
        {
            try
            {
                List<InfoHandleRecordStatistics> stats = this.diagnosticsViewer.GetCustomizeInfoStatistics();
                ThreadPoolInfo tpi = this.diagnosticsViewer.GetThreadPoolInfo();
                List<InfoHandleRecord> uncommittedInfos = this.diagnosticsViewer.GetUncommittedCustomizeInfos();                
                this.agileLogger.LogWithTime(string.Format("线程情况：WorkThreadCount={0}/{1}, IocpThreadCount={2}/{3}", tpi.AvailableWorkThreadCount, tpi.MaxWorkThreadCount, tpi.AvailableIocpThreadCount, tpi.MaxIocpThreadCount));

                if (stats.Count > 0)
                {
                    this.agileLogger.LogWithTime("历史记录:");
                    foreach (InfoHandleRecordStatistics stat in stats)
                    {
                        this.agileLogger.LogWithTime(string.Format("InformationType={0}, InformationStyle={1}, CallCount={2}, ExceptionCount={3}", stat.InformationType, stat.InformationStyle, stat.CallCount, stat.ExceptionCount));
                        foreach (InfoHandleRecord rcd in stat.LastRecords)
                        {
                            this.agileLogger.LogWithTime(string.Format("    ID={0}, StartTime={1}, TimeSpent={2}", rcd.ID, rcd.StartTime, rcd.TimeSpent));
                        }
                    }
                }

                if (uncommittedInfos.Count > 0)
                {               
                    this.agileLogger.LogWithTime("正在处理中的消息:");
                    foreach (InfoHandleRecord record in uncommittedInfos)
                    {
                        this.agileLogger.LogWithTime(string.Format("ID={0}, InformationType={1}, InfoStyle={2}, StartTime={3}", record.ID, record.InformationType, record.InformationStyle, record.StartTime));
                    }
                }

                this.agileLogger.LogWithTime("");
                this.agileLogger.LogWithTime("------------------ ------------------ ------------------ ------------------ ");
                this.agileLogger.LogWithTime("");
            }
            catch (Exception ex)
            {
                this.agileLogger.Log(ex, "DignosticLogger.Log", ErrorLevel.Standard);
            }
            finally
            {
               
            }
        }

        public void Dispose()
        {
            this.stop = true;            
        }
    }
}
