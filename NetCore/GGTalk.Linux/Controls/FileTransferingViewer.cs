using CPF;
using CPF.Animation;
using CPF.Controls;
using CPF.Drawing;
using CPF.Shapes;
using CPF.Styling;
using ESBasic;
using ESBasic.Threading.Engines;
using ESPlus.Application.FileTransfering.Passive;
using ESPlus.FileTransceiver;
using GGTalk.Linux.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GGTalk.Linux.Controls
{
    [CPF.Design.DesignerLoadStyle("res://GGTalk.Linux.Controls/Stylesheet1.css")]//用于设计的时候加载样式
    internal class FileTransferingViewer : Control, IEngineActor
    {
        protected override void InitializeComponent()
        {//模板定义
            Children.Add(new WrapPanel
            {
                Width = 210,
                Height = "100%",
                Name = "FileTransfer_WrapPanel",
                PresenterFor = this,
            });
            fileTransfer_WrapPanel = FindPresenterByName<WrapPanel>("FileTransfer_WrapPanel");
            fileTransfer_WrapPanel.UIElementRemoved += FileTransfer_WrapPanel_UIElementRemoved; 
        }

        private void FileTransfer_WrapPanel_UIElementRemoved(object sender, UIElementRemovedEventArgs e)
        {
            if (this.fileTransfer_WrapPanel.Children.Count == 0)
            {
                this.AllTaskFinished();
            }
        }

        private WrapPanel fileTransfer_WrapPanel;
        private ESBasic.Func<TransferingProject, bool> projectFilter;
        private IFileOutter fileOutter;
        private AgileCycleEngine agileCycleEngine;
        private Window parentWindow;

        #region event FileTransferingViewer的所有事件都是在UI线程中触发的。

        /// <summary>
        /// 当某个文件开始续传时，触发该事件。参数为FileName - isSending
        /// </summary>
        public event CbGeneric<string, bool> FileResumedTransStarted;

        /// <summary>
        /// 当某个文件传送完毕时，触发该事件。参数为FileName - isSending - comment - isFolder
        /// </summary>
        public event CbGeneric<string, bool, string, bool> FileTransCompleted;

        /// <summary>
        /// 当某个文件传送完毕时，触发该事件。
        /// </summary>
        public event CbGeneric<TransferingProject> FileTransCompleted2;

        /// <summary>
        /// 当某个文件传送中断时，触发该事件。参数为FileName - isSending - FileTransDisrupttedType - cause
        /// </summary>
        public event CbGeneric<string, bool, FileTransDisrupttedType, string> FileTransDisruptted;

        /// <summary>
        /// 当某个文件传送开始时，触发该事件。参数为FileName - isSending
        /// </summary>
        public event CbGeneric<string, bool> FileTransStarted;

        /// <summary>
        /// 当所有文件都传送完成时，触发该事件。
        /// </summary>
        public event CbSimple AllTaskFinished;

        /// <summary>
        /// 当某个文件需要离线发送时，触发该事件。参数为TransferingProject
        /// </summary>
        public event CbGeneric<TransferingProject> FileNeedOfflineSend;

        #endregion

        #region Ctor
        public FileTransferingViewer()
        {

            this.agileCycleEngine = new AgileCycleEngine(this);
            this.agileCycleEngine.DetectSpanInSecs = 1;
            this.agileCycleEngine.Start();

            this.FileResumedTransStarted += delegate { };
            this.FileTransCompleted += delegate { };
            this.FileTransDisruptted += delegate { };
            this.FileTransStarted += delegate { };
            this.AllTaskFinished += delegate { };
            this.FileTransCompleted2 += delegate { };
        }

        #endregion


        #region Initialize
        public void Initialize(IFileOutter _fileOutter, Window _parentWindow)
        {
            this.Initialize(_fileOutter, (ESBasic.Func<TransferingProject, bool>)null, _parentWindow);
        }

        public void Initialize(IFileOutter _fileOutter, string friendUserID, Window _parentWindow)
        {
            if (friendUserID == null)
            {
                friendUserID = ESFramework.NetServer.SystemUserID;
            }
            this.Initialize(_fileOutter, delegate (TransferingProject pro) { return pro.DestUserID == friendUserID; }, _parentWindow);
        }


        /// <summary>
        /// 初始化控件。
        /// </summary>         
        /// <param name="filter">当前的Viewer要显示哪些传送项目的状态信息</param>        
        public void Initialize(IFileOutter _fileOutter, ESBasic.Func<TransferingProject, bool> filter, Window _parentWindow)
        {
            this.fileOutter = _fileOutter;
            this.projectFilter = filter;
            this.parentWindow = _parentWindow;
            //this.fileOutter.FileRequestReceived += new ESPlus.Application.FileTransfering.CbFileRequestReceived(fileOutter_FileRequestReceived);
            this.fileOutter.FileTransferingEvents.FileTransStarted += new CbGeneric<ITransferingProject>(fileTransStarted);
            this.fileOutter.FileTransferingEvents.FileTransCompleted += new CbGeneric<ITransferingProject>(fileTransCompleted);
            this.fileOutter.FileTransferingEvents.FileTransDisruptted += new CbGeneric<ITransferingProject, FileTransDisrupttedType, string>(fileTransDisruptted);
            this.fileOutter.FileTransferingEvents.FileTransProgress += new CbFileSendedProgress(fileTransProgress);
            this.fileOutter.FileTransferingEvents.FileResumedTransStarted += new CbGeneric<ITransferingProject>(fileSenderManager_FileResumedTransStarted);

            //this.fileOutter.FileReceivingEvents.FileTransStarted += new CbGeneric<ITransferingProject>(fileTransStarted);
            //this.fileOutter.FileReceivingEvents.FileResumedTransStarted += new CbGeneric<ITransferingProject>(fileReceiverManager_FileResumedTransStarted);
            //this.fileOutter.FileReceivingEvents.FileTransCompleted += new CbGeneric<ITransferingProject>(fileTransCompleted);
            //this.fileOutter.FileReceivingEvents.FileTransDisruptted += new CbGeneric<ITransferingProject, FileTransDisrupttedType, string>(fileTransDisruptted);
            //this.fileOutter.FileReceivingEvents.FileTransProgress += new CbFileSendedProgress(fileTransProgress);
        }

        public List<string> GetTransferingProjectIDsInCurrentViewer()
        {
            List<string> list = new List<string>();
            foreach (FileTransferItem item in this.fileTransfer_WrapPanel.Children)
            {
                list.Add(item.TransmittingProject.ProjectID);
            }

            return list;
        }

        /// <summary>
        /// 当收到发送文件的请求时,接收方调用此方法显示fileTransferItem
        /// </summary>        
        public void NewFileTransferItem(string projectID, bool offlineFile, bool doneAgreed)
        {
            UIThreadPoster<string, bool, bool> poster = new UIThreadPoster<string, bool, bool>(this.doNewFileTransferItem, projectID, offlineFile, doneAgreed);
            poster.Post();

        }
        private void doNewFileTransferItem(string projectID, bool offlineFile, bool doneAgreed)
        {

            CommonHelper.Log_Tmp($"projectID-{projectID}：doNewFileTransferItem 1");
            TransferingProject pro = this.fileOutter.GetTransferingProject(projectID);
            CommonHelper.Log_Tmp($"projectID-{projectID}：doNewFileTransferItem 2");
            if (pro == null)
            {
                return;
            }

            if (this.projectFilter != null)
            {
                if (!this.projectFilter(pro))
                {
                    CommonHelper.Log_Tmp($"projectID-{projectID}：doNewFileTransferItem 3");
                    return;
                }
            }

            CommonHelper.Log_Tmp($"projectID-{projectID}：doNewFileTransferItem 4");
            this.AddFileTransItem(pro, offlineFile, doneAgreed);
        }

        /// <summary>
        ///  一定要在控件销毁的时候，取消预订的事件。否则，已被释放的控件的处理函数仍然会被调用，而引发"对象已经被释放"的异常。
        /// </summary>        
        public void BeforeDispose()
        {
            this.agileCycleEngine.StopAsyn();

            this.fileOutter.FileTransferingEvents.FileTransStarted -= new CbGeneric<ITransferingProject>(fileTransStarted);
            this.fileOutter.FileTransferingEvents.FileTransCompleted -= new CbGeneric<ITransferingProject>(fileTransCompleted);
            this.fileOutter.FileTransferingEvents.FileTransDisruptted -= new CbGeneric<ITransferingProject, FileTransDisrupttedType, string>(fileTransDisruptted);
            this.fileOutter.FileTransferingEvents.FileTransProgress -= new CbFileSendedProgress(fileTransProgress);
            this.fileOutter.FileTransferingEvents.FileResumedTransStarted -= new CbGeneric<ITransferingProject>(fileSenderManager_FileResumedTransStarted);

            //this.fileOutter.FileReceivingEvents.FileTransStarted -= new CbGeneric<ITransferingProject>(fileTransStarted);
            //this.fileOutter.FileReceivingEvents.FileResumedTransStarted -= new CbGeneric<ITransferingProject>(fileReceiverManager_FileResumedTransStarted);
            //this.fileOutter.FileReceivingEvents.FileTransCompleted -= new CbGeneric<ITransferingProject>(fileTransCompleted);
            //this.fileOutter.FileReceivingEvents.FileTransDisruptted -= new CbGeneric<ITransferingProject, FileTransDisrupttedType, string>(fileTransDisruptted);
            //this.fileOutter.FileReceivingEvents.FileTransProgress -= new CbFileSendedProgress(fileTransProgress);
        }

        void doFileReceiverManager_FileResumedTransStarted(TransferingProject info)
        {
            if (this.projectFilter != null)
            {
                if (!this.projectFilter(info))
                {
                    return;
                }
            }
            FileTransferItem item = this.GetExistedItem(info.ProjectID);
            if (item != null)
            {
                this.FileResumedTransStarted(info.ProjectName, false);
            }
        }

        void fileReceiverManager_FileResumedTransStarted(ITransferingProject pro)
        {
            TransferingProject info = pro as TransferingProject;
            UIThreadPoster<TransferingProject> poster = new UIThreadPoster<TransferingProject>(this.doFileReceiverManager_FileResumedTransStarted, info);
            poster.Post();
        }

        void doFileSenderManager_FileResumedTransStarted(TransferingProject info)
        {
            if (this.projectFilter != null)
            {
                if (!this.projectFilter(info))
                {
                    return;
                }
            }
            this.AddFileTransItem(info, false, true);
            FileTransferItem item = this.GetExistedItem(info.ProjectID);
            if (item != null)
            {
                this.FileResumedTransStarted(info.ProjectName, true);
            }
        }


        void fileSenderManager_FileResumedTransStarted(ITransferingProject pro)
        {
            TransferingProject info = pro as TransferingProject;
            UIThreadPoster<TransferingProject> poster = new UIThreadPoster<TransferingProject>(this.doFileSenderManager_FileResumedTransStarted, info);
            poster.Post();
        }

        /// <summary>
        /// 当前是否有文件正在传送中。
        /// </summary>   
        public bool IsFileTransfering
        {
            get
            {
                return (this.fileTransfer_WrapPanel.Children.Count > 0);
            }
        }
        #endregion

        #region fileTransStarted      
        void fileTransStarted(ITransferingProject pro)
        {
            TransferingProject info =pro as TransferingProject;
            if (this.projectFilter != null)
            {
                if (!this.projectFilter(info))
                {
                    return;
                }
            }

            UIThreadPoster<TransferingProject> poster = new UIThreadPoster<TransferingProject>(this.doFileTransStarted, info);
            poster.Post();
        }

        private void doFileTransStarted(TransferingProject project)
        {
            this.AddFileTransItem(project, false, true);
            FileTransferItem item = this.GetExistedItem(project.ProjectID);
            if (item != null)
            {
                item.IsTransfering = true;
            }
        }

        #endregion

        #region fileTransProgress
        void fileTransProgress(string projectID, ulong total, ulong sended)
        {
            FileTransferItem item = this.GetExistedItem(projectID);
            if (item != null)
            {
                item.SetProgress(total, sended);
            }
        }
        #endregion

        #region fileTransDisruptted

        void doFileTransDisruptted(TransferingProject info, FileTransDisrupttedType disrupttedType, string cause)
        {
            FileTransferItem item = this.GetExistedItem(info.ProjectID);
            if (item != null)
            {
                this.fileTransfer_WrapPanel.Children.Remove(item);
                this.FileTransDisruptted(info.ProjectName, info.IsSender, disrupttedType, cause);
            }
        }

        void fileTransDisruptted(ITransferingProject pro, FileTransDisrupttedType disrupttedType, string cause)
        {
            TransferingProject info = pro as TransferingProject;
            UIThreadPoster<TransferingProject, FileTransDisrupttedType, string> poster = new UIThreadPoster<TransferingProject, FileTransDisrupttedType, string>(this.doFileTransDisruptted, info, disrupttedType, cause);
            poster.Post();
        }
        #endregion

        #region fileTransCompleted


        void doFileTransCompleted(TransferingProject info)
        {
            FileTransferItem item = this.GetExistedItem(info.ProjectID);
            if (item != null)
            {
                this.FileTransCompleted2(info);
                this.fileTransfer_WrapPanel.Children.Remove(item);
                this.FileTransCompleted(info.ProjectName, info.IsSender, info.Comment, info.IsFolder);
            }
        }
        void fileTransCompleted(ITransferingProject pro)
        {
            TransferingProject info = pro as TransferingProject;
            UIThreadPoster<TransferingProject> poster = new UIThreadPoster<TransferingProject>(this.doFileTransCompleted, info);
            poster.Post();
        }
        #endregion


        #region AddFileTransItem ,GetFileTransItem        
        private void AddFileTransItem(TransferingProject project, bool offlineFile, bool doneAgreed)
        {
            CommonHelper.Log_Tmp($"projectID-{project.ProjectID}：AddFileTransItem 1");
            if (this.projectFilter != null)
            {
                if (!this.projectFilter(project))
                {
                    CommonHelper.Log_Tmp($"projectID-{project.ProjectID}：AddFileTransItem 2");
                    return;
                }
            }

            CommonHelper.Log_Tmp($"projectID-{project.ProjectID}：AddFileTransItem 3");
            FileTransferItem fileTransItem = this.GetExistedItem(project.ProjectID);
            if (fileTransItem != null)
            {
                CommonHelper.Log_Tmp($"projectID-{project.ProjectID}：AddFileTransItem 4");
                return;
            }
            fileTransItem = new FileTransferItem();
            fileTransItem.FileCanceled += new CbFileCanceled(fileTransItem_FileCanceled);
            fileTransItem.FileReceived += new CbFileReceived(fileTransItem_FileReceived);
            fileTransItem.FileRejected += new CbFileRejected(fileTransItem_FileRejected);
            fileTransItem.ConvertToOfflineFile += new CbGeneric<TransferingProject, FileTransferItem>(fileTransItem_ConvertToOfflineFile);
            fileTransItem.Initialize(project, offlineFile, doneAgreed, this.parentWindow);
            CommonHelper.Log_Tmp($"projectID-{project.ProjectID}：AddFileTransItem 5");
            this.fileTransfer_WrapPanel.Children.Add(fileTransItem);
            this.FileTransStarted(project.ProjectName, project.IsSender);
        }



        void fileTransItem_ConvertToOfflineFile(TransferingProject project, FileTransferItem item)
        {

            if (this.FileNeedOfflineSend != null)
            {
                this.FileNeedOfflineSend(project);
            }

        }

        private FileTransferItem GetExistedItem(string projectID)
        {
            foreach (FileTransferItem item in this.fileTransfer_WrapPanel.Children)
            {
                if (item.TransmittingProject.ProjectID == projectID)
                {
                    return item;
                }
            }

            return null;
        }

        void fileTransItem_FileRejected(string projectID)
        {
            this.fileOutter.RejectFile(projectID);
        }

        void fileTransItem_FileReceived(FileTransferItem item, string projectID, bool isSend, string savePath)
        {
            this.fileOutter.BeginReceiveFile(projectID, savePath);
        }

        void fileTransItem_FileCanceled(FileTransferItem item, string projectID, bool isSend)
        {
            this.fileOutter.CancelTransfering(projectID);
        }
        #endregion       

        #region EngineAction
        public bool EngineAction()
        {
            try
            {
                this.CheckZeroSpeed();
            }
            catch { }
            return true;
        }

        private void doCheckZeroSpeed()
        {

            foreach (object obj in this.fileTransfer_WrapPanel.Children)
            {
                FileTransferItem item = obj as FileTransferItem;
                if (item != null && item.IsTransfering)
                {
                    item.CheckZeroSpeed();
                }
            }
        }

        private void CheckZeroSpeed()
        {
            UIThreadPoster poster = new UIThreadPoster(this.doCheckZeroSpeed);
            poster.Post();
        }
        #endregion

        //private void flowLayoutPanel1_Paint(object sender, PointerEventArgs e)
        //{

        //}
    }

    internal delegate FileTransferItem CbGetFileTransItem(string projectID, bool isSended);
    internal delegate FileTransferItem CbGetFileTransItem2(string projectID);
    public delegate void CbFileReceiveCanceled(string friend, string projectID);
    public delegate void CbCancelFile(string projectID, bool isSsend);

}
