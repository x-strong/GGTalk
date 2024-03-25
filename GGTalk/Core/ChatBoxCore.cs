using ESBasic;
using ESBasic.ObjectManagement.Managers;
using ESBasic.Threading.Engines;
using ESFramework;
using ESFramework.Boost;
using ESFramework.Boost.Controls;
using ESPlus.Application.FileTransfering.Passive;
using ESPlus.FileTransceiver;
using OMCS.Passive.ShortMessages;
using GGTalk.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using TalkBase;
using TalkBase.Client;
using CCWin.Win32.Const;

namespace GGTalk.Core
{
    internal class ChatBoxCore: IFileTransferingViewer
    {
        public SenderShowType SenderShowType { get; set; }
        private DateTime lastMessageTime = DateTime.MinValue;
        public int offsetY = 8;
        private List<IGGChatControl> controlList = new List<IGGChatControl>();
        private IUIAction uiAction;
        private IChatControl parent;

        /// <summary>
        /// 点击图片消息
        /// </summary>
        public event Action<Image> ImageMessageClicked;

        /// <summary>
        /// 内容滚动到最后
        /// </summary>
        public event Action ContentScrollToEnd;

        public int Width { get; set; }
        public int Height { get; set; }

        private int scrollOffsetY = 0;
        /// <summary>
        /// VerticalScroll Y偏移量
        /// </summary>
        public int ScrollOffsetY { private get => this.scrollOffsetY; set => this.scrollOffsetY = value; }

        public ChatBoxCore()
        {
            this.AudioMessageClicked += delegate { };
            this.ImageMessageClicked += delegate { };

            this.FileResumedTransStarted += delegate { };
            this.FileTransCompleted += delegate { };
            this.FileTransDisruptted += delegate { };
            this.FileTransStarted += delegate { };
            this.AllTaskFinished += delegate { };
            this.FileTransCompleted2 += delegate { };

            this.ContentScrollToEnd += delegate { };
        }

        public void Initialize(IUIAction _uIAction, IChatControl _parent)
        {
            this.uiAction = _uIAction;
            this.parent = _parent;
        }

        private object locker = new object();
        /// <summary>
        /// 追加发送时间
        /// </summary>
        /// <param name="time"></param>
        private void AppendSendTime(DateTime time)
        {
            lock (this.locker)
            {
                if (time.Subtract(this.lastMessageTime) > TimeSpan.FromMinutes(5))
                {
                    this.lastMessageTime = time;
                    String datea = time.ToString("yyyy/MM/dd HH:mm");
                    GGSystemTipLabel o = new GGSystemTipLabel(datea, this.parent);
                    this.controlList.Add(o);
                }
            }
        }

        /// <summary>
        /// 将最后聊天记录时间重置 为初始值
        /// </summary>
        public void InitLastMessageTime()
        {
            this.lastMessageTime = DateTime.MinValue;
        }


        #region 追加消息

        /// <summary>
        /// 追加系统消息
        /// </summary>
        /// <param name="msg"></param>
        public void AppendSysMessage(string msg)
        {
            GGSystemTipLabel addSystemremind = new GGSystemTipLabel(msg, this.parent);
            this.controlList.Add(addSystemremind);
            this.AutoSizeChanged();
        }


        /// <summary>
        /// 追加表情和文本
        /// </summary>
        /// <param name="isme"></param>
        /// <param name="content"></param>
        /// <param name="time"></param>
        /// <param name="headImg"></param>
        public void AppendTextAndEmoji(bool isme, ESFramework.Boost.Controls.ChatBoxContent content, DateTime time, Image headImg, string displayName)
        {
            List<IGGChatInputItem> items = this.GetIGGChatInputItems4Text(content);
            List<GGChatInPutImage> images = this.GetGGChatInPutImages(content);
            if (items == null) { return; }
            SenderInfo senderInfo = new SenderInfo() { IsMe = isme, DisplayName = displayName, HeadImg = headImg };

            if (items.Count > 0)
            {
                this.AppendTextAndEmoji(items, senderInfo, time);
            }
            foreach (GGChatInPutImage item in images)
            {
                this.AppendImage(item.Bitmap, senderInfo, time);
            }
        }

        #region 从ChatBoxContent2 提取文本、表情对象
        private List<IGGChatInputItem> GetIGGChatInputItems4Text(ESFramework.Boost.Controls.ChatBoxContent content)
        {
            if (content == null || content.Text == null)
            {
                return null;
            }
            List<IGGChatInputItem> inputItemListRebuild = new List<IGGChatInputItem>();
            int count = content.Text.Length;
            if (content.NonTextItemDictionary != null && content.NonTextItemDictionary.Count > 0)
            {
                string pureText = content.Text;

                //去掉表情和图片的占位符   
                for (int i = content.NonTextItemPositions.Count - 1; i >= 0; i--)
                {
                    pureText = pureText.Remove((int)content.NonTextItemPositions[i], 1);

                }
                foreach (char item in pureText)
                {
                    inputItemListRebuild.Add(this.GetGGChatInputText(item.ToString()));
                }

                //this.AppendText(pureText);

                //插入NonTextItem
                for (int i = 0; i < content.NonTextItemPositions.Count; i++)
                {
                    uint position = content.NonTextItemPositions[i];
                    NonTextItem item = content.NonTextItemDictionary[position];
                    if (item.ChatBoxElementType == ChatBoxElementType.InnerEmotion)
                    {
                        inputItemListRebuild.Insert((int)position, new GGChatInPutEmotion((int)item.InnerEmotionIndex, GlobalResourceManager.EmotionDictionary[item.InnerEmotionIndex]));
                    }
                    else if (item.ChatBoxElementType == ChatBoxElementType.ForeignImage)
                    {
                        //inputItemListRebuild.Insert((int)position, new GGChatInPutImage(item.ForeignImage));

                        //this.InsertImage(item.ForeignImage, (int)(count + position));
                    }
                    else if (item.ChatBoxElementType == ChatBoxElementType.FileLink)
                    {
                        //this.InsertFileLink(item.FilePath, (int)(count + position));
                    }
                    else //LinkText
                    {
                        //this.InsertLinkText(item.LinkText, (int)(count + position));
                    }
                }
            }
            else
            {
                foreach (char item in content.Text)
                {
                    inputItemListRebuild.Add(this.GetGGChatInputText(item.ToString()));
                }
            }
            return inputItemListRebuild;

        }
        private SolidBrush inputTextBrush = new SolidBrush(Color.Black);
        private GGChatInputText GetGGChatInputText(string chars)
        {
            DrawTextAttribute inputText = this.CreateFormattedText(chars);
            return new GGChatInputText(inputText, inputTextBrush);
        }

        private DrawTextAttribute CreateFormattedText(string chars)
        {
            string fontname = System.Drawing.SystemFonts.DefaultFont.Name;
            FontFamily fontFamily = new FontFamily(fontname);
            DrawTextAttribute drawTextAttribute = new DrawTextAttribute();
            drawTextAttribute.rectangleF = new RectangleF(new Point(0, 0), new Size(200, 200));
            drawTextAttribute.font = new Font(fontname, 14, FontStyle.Regular, GraphicsUnit.Pixel);
            drawTextAttribute.Text = chars;
            drawTextAttribute.brush = new SolidBrush(Color.Black);
            return drawTextAttribute;
        }

        private List<GGChatInPutImage> GetGGChatInPutImages(ESFramework.Boost.Controls.ChatBoxContent content)
        {
            if (content == null)
            {
                return null;
            }
            List<GGChatInPutImage> ggChatInPutImages = new List<GGChatInPutImage>();
            if (content.NonTextItemDictionary != null && content.NonTextItemDictionary.Count > 0)
            {
                //插入NonTextItem
                for (int i = 0; i < content.NonTextItemPositions.Count; i++)
                {
                    uint position = content.NonTextItemPositions[i];
                    NonTextItem item = content.NonTextItemDictionary[position];
                    if (item.ChatBoxElementType == ChatBoxElementType.ForeignImage)
                    {
                        ggChatInPutImages.Add(new GGChatInPutImage(item.ForeignImage));
                    }

                }
            }
            return ggChatInPutImages;
        }
        #endregion

        /// <summary>
        /// 追加表情和文本
        /// </summary>
        /// <param name="all"></param>
        /// <param name="offsetY"></param>
        /// <param name="isme"></param>
        /// <param name="_headImg"></param>
        private void AppendTextAndEmoji(List<IGGChatInputItem> all, SenderInfo senderInfo, DateTime time)
        {
            this.AppendSendTime(time);
            senderInfo.SenderShowType = this.SenderShowType;
            controlList.Add(new GGChatContentEmotionAndText(all, this.parent, senderInfo));
            this.AutoSizeChanged();
        }

        /// <summary>
        /// 追加图片
        /// </summary>
        /// <param name="_Isme"></param>
        /// <param name="offsetY"></param>
        /// <param name="_messageImage"></param>
        /// <param name="_headImg"></param>
        private void AppendImage(Image _messageImage, SenderInfo senderInfo, DateTime time)
        {
            this.AppendSendTime(time);
            senderInfo.SenderShowType = this.SenderShowType;
            controlList.Add(new GGChatContentImage(_messageImage, this.parent, senderInfo));
            this.AutoSizeChanged();
        }


        /// <summary>
        /// 追加个人名片消息
        /// </summary>
        /// <param name="cardModel"></param>
        /// <param name="isme"></param>
        /// <param name="headImg"></param>
        /// <param name="displayName"></param>
        /// <param name="time"></param>
        public void AppendCard(PersonCardModel cardModel, bool isme, Image headImg, string displayName, DateTime time)
        {
            SenderInfo senderInfo = new SenderInfo() { IsMe = isme, DisplayName = displayName, HeadImg = headImg };
            this.AppendCard(cardModel, senderInfo, time);
        }

        /// <summary>
        /// 追加个人名片
        /// </summary>
        /// <param name="cardModel"></param>
        private void AppendCard(PersonCardModel cardModel, SenderInfo senderInfo, DateTime time)
        {
            this.AppendSendTime(time);
            senderInfo.SenderShowType = this.SenderShowType;
            controlList.Add(new GGChatCard(cardModel, ref offsetY, this.parent, senderInfo));
            this.AutoSizeChanged();
        }

        /// <summary>
        /// 追加语音消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="isme"></param>
        /// <param name="headImg"></param>
        /// <param name="displayName"></param>
        /// <param name="time"></param>
        public void AppendAudioMessageBox(AudioMessage msg, bool isme, Image headImg, string displayName, DateTime time)
        {
            SenderInfo senderInfo = new SenderInfo() { IsMe = isme, DisplayName = displayName, HeadImg = headImg };
            this.AppendAudioMessageBox(senderInfo, msg, time);
        }

        private void AppendAudioMessageBox(SenderInfo senderInfo, AudioMessage msg, DateTime time)
        {
            if (msg == null) { return; }
            this.AppendSendTime(time);
            senderInfo.SenderShowType = this.SenderShowType;
            GGAudioMessageBox ggAudioMessageBox = new GGAudioMessageBox(msg, ref offsetY, this.parent, senderInfo);
            controlList.Add(ggAudioMessageBox);
            this.audioMessageBoxDic.Add(msg.GetUniqueID(), ggAudioMessageBox);
            this.AutoSizeChanged();
        }
        #endregion


        private object locker2 = new object();
        private int totalHeight;

        /// <summary>
        /// 所有内容的总高度
        /// </summary>
        public int ContentTotalHeight { get { lock (this.locker2) { return this.totalHeight; } } }

        public void AutoSizeChanged()
        {
            lock (this.locker2)
            {
                int startOffset = 8 - this.scrollOffsetY;
                this.totalHeight = 0;
                string str = "";
                for (int i = 0; i < this.controlList.Count; i++)
                {
                    offsetY = startOffset + this.totalHeight;
                    this.controlList[i].SetOffsetY(offsetY);
                    this.totalHeight += this.controlList[i].Height + 20;
                    if (this.controlList[i] is GGChatContentEmotionAndText)
                    {
                        this.totalHeight += 10;
                    }
                    str += this.controlList[i].Height + ",";
                }
                if (controlList.Count > 0)
                {
                    offsetY += controlList[controlList.Count - 1].Height + 40;
                    this.totalHeight += 30;
                }
            }
            this.parent.Invalidate();
        }


        public void OnDraw(Graphics g)
        {
            g.Clear(Color.FromArgb(245, 245, 245));
            for (int i = 0; i < this.controlList.Count; i++)
            {
                this.controlList[i].Draw(g);
            }
        }

        private IGGChatControl lastOnEnterControl = null;
        public void OnMouseMove(Point location)
        {
            for (int i = 0; i < this.controlList.Count; i++)
            {
                if (controlList[i].AllSize.Contains(location))
                {
                    if (this.lastOnEnterControl == controlList[i])//若在同一控件上移动,不再次触发OnMouseMove事件 （GGFileBox 除外）
                    {
                        GGFileBox ggFileBox = controlList[i] as GGFileBox;
                        if (ggFileBox != null)
                        {
                            controlList[i].OnMouseMove(location);
                        }

                        GGAudioMessageBox ggAudioMessageBox = controlList[i] as GGAudioMessageBox;
                        if (ggAudioMessageBox != null)
                        {
                            controlList[i].OnMouseMove(location);
                        }
                        return;
                    }
                    controlList[i].OnMouseMove(location);
                    this.lastOnEnterControl = controlList[i];
                    return;
                }
            }
            if (this.lastOnEnterControl != null)
            {
                this.lastOnEnterControl.OnMouseleave();
                this.lastOnEnterControl = null;
            }
        }

        public void OnMouseDown(Point location)
        {
            for (int i = 0; i < this.controlList.Count; i++)
            {
                GGFileBox ggFileBox = controlList[i] as GGFileBox;
                if (ggFileBox != null)
                {
                    if (ggFileBox.AllSize.Contains(location))
                    {
                        controlList[i].OnMouseDown(location);
                        return;
                    }
                    continue;
                }
                GGAudioMessageBox ggAudioMessageBox = controlList[i] as GGAudioMessageBox;
                if (ggAudioMessageBox != null)
                {
                    if (ggAudioMessageBox.AllSize.Contains(location))
                    {
                        controlList[i].OnMouseDown(location);
                        this.AudioMessageClicked(ggAudioMessageBox.AudioMessageID, ggAudioMessageBox.AudioMessage);
                        return;
                    }
                    continue;
                }

                GGChatContentImage ggChatContentImage = controlList[i] as GGChatContentImage;
                if (ggChatContentImage != null)
                {
                    if (ggChatContentImage.AllSize.Contains(location))
                    {
                        controlList[i].OnMouseDown(location);
                        this.ImageMessageClicked(ggChatContentImage.MessageImage);
                        return;
                    }
                    continue;
                }
            }
        }


        #region 语音消息

        /// <summary>
        /// 当某个语音消息被单击时，触发此事件。 参数：AudioMessageID - AudioMessage
        /// </summary>
        public event System.Action<string, object> AudioMessageClicked;

        private Dictionary<string, GGAudioMessageBox> audioMessageBoxDic = new Dictionary<string, GGAudioMessageBox>();

        /// <summary>
        /// 开始语音消息播放的动画。
        /// </summary>       
        public void StartPlayAudioMessageAnimation(string msgID)
        {
            if (!this.audioMessageBoxDic.ContainsKey(msgID))
            {
                return;
            }

            this.audioMessageBoxDic[msgID].StartPlay();
        }

        /// <summary>
        /// 停止语音消息播放的动画。
        /// </summary>
        public void StopPlayAudioMessageAnimation(string msgID)
        {
            if (!this.audioMessageBoxDic.ContainsKey(msgID))
            {
                return;
            }

            this.audioMessageBoxDic[msgID].StopPlay();
        }

        /// <summary>
        /// 目标语音动画是否正在播放？
        /// </summary>        
        public bool IsPlayingAudioMessageAnimation(string msgID)
        {
            if (!this.audioMessageBoxDic.ContainsKey(msgID))
            {
                return false;
            }

            return this.audioMessageBoxDic[msgID].IsPlaying;
        }
        #endregion

        #region IFileTransferingViewer
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        private ESBasic.Func<TransferingProject, bool> projectFilter;
        private IFileOutter fileOutter;
        private AgileCycleEngine agileCycleEngine;
        private ObjectManager<string, GGFileBox> fileBoxManager = new ObjectManager<string, GGFileBox>();

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

        #region Initialize
        public void Initialize(ResourceCenter<GGUser, GGGroup> _resourceCenter, IFileOutter _fileOutter)
        {
            this.Initialize(_resourceCenter, _fileOutter, (ESBasic.Func<TransferingProject, bool>)null);
        }

        public void Initialize(ResourceCenter<GGUser, GGGroup> _resourceCenter, IFileOutter _fileOutter, string friendUserID)
        {
            if (friendUserID == null)
            {
                friendUserID = ESFramework.NetServer.SystemUserID;
            }
            this.Initialize(_resourceCenter, _fileOutter, delegate (TransferingProject pro) { return pro.DestUserID == friendUserID; });
        }


        /// <summary>
        /// 初始化控件。
        /// </summary>         
        /// <param name="filter">当前的Viewer要显示哪些传送项目的状态信息</param>        
        public void Initialize(ResourceCenter<GGUser, GGGroup> _resourceCenter, IFileOutter _fileOutter, ESBasic.Func<TransferingProject, bool> filter)
        {
            this.resourceCenter = _resourceCenter;
            this.fileOutter = _fileOutter;
            this.projectFilter = filter;

            //this.fileOutter.FileRequestReceived += new ESPlus.Application.FileTransfering.CbFileRequestReceived(fileOutter_FileRequestReceived);
            this.fileOutter.FileSendingEvents.FileTransStarted += new CbGeneric<ITransferingProject>(fileTransStarted);
            this.fileOutter.FileSendingEvents.FileTransCompleted += new CbGeneric<ITransferingProject>(fileTransCompleted);
            this.fileOutter.FileSendingEvents.FileTransDisruptted += new CbGeneric<ITransferingProject, FileTransDisrupttedType, string>(fileTransDisruptted);
            this.fileOutter.FileSendingEvents.FileTransProgress += new CbFileSendedProgress(fileTransProgress);
            this.fileOutter.FileSendingEvents.FileResumedTransStarted += new CbGeneric<ITransferingProject>(fileSenderManager_FileResumedTransStarted);

            this.fileOutter.FileReceivingEvents.FileTransStarted += new CbGeneric<ITransferingProject>(fileTransStarted);
            this.fileOutter.FileReceivingEvents.FileResumedTransStarted += new CbGeneric<ITransferingProject>(fileReceiverManager_FileResumedTransStarted);
            this.fileOutter.FileReceivingEvents.FileTransCompleted += new CbGeneric<ITransferingProject>(fileTransCompleted);
            this.fileOutter.FileReceivingEvents.FileTransDisruptted += new CbGeneric<ITransferingProject, FileTransDisrupttedType, string>(fileTransDisruptted);
            this.fileOutter.FileReceivingEvents.FileTransProgress += new CbFileSendedProgress(fileTransProgress);
        }

        public List<string> GetTransferingProjectIDsInCurrentViewer()
        {
            List<string> list = new List<string>();
            List<GGFileBox> boxes = this.fileBoxManager.GetAllReadonly();
            foreach (GGFileBox item in boxes)
            {
                if (item.IsTransfering)
                {
                    list.Add(item.MsgID);
                }
            }
            return list;
        }

        /// <summary>
        /// 当收到发送文件的请求时,接收方调用此方法显示fileTransferItem
        /// </summary>        
        public void NewFileTransferItem(string projectID, bool offlineFile, bool doneAgreed)
        {
            TransferingProject pro = this.fileOutter.GetTransferingProject(projectID);
            if (pro == null)
            {
                return;
            }

            if (this.projectFilter != null)
            {
                if (!this.projectFilter(pro))
                {
                    return;
                }
            }

            this.AddFileTransItem(pro, offlineFile, doneAgreed);
        }

        /// <summary>
        ///  一定要在控件销毁的时候，取消预订的事件。否则，已被释放的控件的处理函数仍然会被调用，而引发"对象已经被释放"的异常。
        /// </summary>        
        public void BeforeDispose()
        {
            this.agileCycleEngine.StopAsyn();

            this.fileOutter.FileSendingEvents.FileTransStarted -= new CbGeneric<ITransferingProject>(fileTransStarted);
            this.fileOutter.FileSendingEvents.FileTransCompleted -= new CbGeneric<ITransferingProject>(fileTransCompleted);
            this.fileOutter.FileSendingEvents.FileTransDisruptted -= new CbGeneric<ITransferingProject, FileTransDisrupttedType, string>(fileTransDisruptted);
            this.fileOutter.FileSendingEvents.FileTransProgress -= new CbFileSendedProgress(fileTransProgress);
            this.fileOutter.FileSendingEvents.FileResumedTransStarted -= new CbGeneric<ITransferingProject>(fileSenderManager_FileResumedTransStarted);

            this.fileOutter.FileReceivingEvents.FileTransStarted -= new CbGeneric<ITransferingProject>(fileTransStarted);
            this.fileOutter.FileReceivingEvents.FileResumedTransStarted -= new CbGeneric<ITransferingProject>(fileReceiverManager_FileResumedTransStarted);
            this.fileOutter.FileReceivingEvents.FileTransCompleted -= new CbGeneric<ITransferingProject>(fileTransCompleted);
            this.fileOutter.FileReceivingEvents.FileTransDisruptted -= new CbGeneric<ITransferingProject, FileTransDisrupttedType, string>(fileTransDisruptted);
            this.fileOutter.FileReceivingEvents.FileTransProgress -= new CbFileSendedProgress(fileTransProgress);
        }

        void fileReceiverManager_FileResumedTransStarted(ITransferingProject pro)
        {
            TransferingProject info = (TransferingProject)pro;
            this.uiAction.ActionOnUI(new Action<TransferingProject>(this.dofileReceiverManager_FileResumedTransStarted), info);
        }

        private void dofileReceiverManager_FileResumedTransStarted(TransferingProject info)
        {
            if (this.projectFilter != null)
            {
                if (!this.projectFilter(info))
                {
                    return;
                }
            }
            GGFileBox item = this.GetExistedItem(info.ProjectID);

            if (item != null)
            {
                item.Transmit_Running();
                this.FileResumedTransStarted(info.ProjectName, false);
            }
        }

        void fileSenderManager_FileResumedTransStarted(ITransferingProject pro)
        {
            TransferingProject info = (TransferingProject)pro;
            this.uiAction.ActionOnUI(new Action<TransferingProject>(this.dofileSenderManager_FileResumedTransStarted), info);
        }

        private void dofileSenderManager_FileResumedTransStarted(TransferingProject info)
        {

            if (this.projectFilter != null)
            {
                if (!this.projectFilter(info))
                {
                    return;
                }
            }

            this.AddFileTransItem(info, false, true);
            GGFileBox item = this.GetExistedItem(info.ProjectID);
            if (item != null)
            {
                item.Transmit_Running();
                this.FileResumedTransStarted(info.ProjectName, true);
            }
        }

        /// <summary>
        /// 当前是否有文件正在传送中。
        /// </summary>   
        public bool IsFileTransfering
        {
            get
            {
                return (this.GetTransferingProjectIDsInCurrentViewer()?.Count > 0);
            }
        }
        #endregion

        #region fileTransStarted
        void fileTransStarted(ITransferingProject pro)
        {
            TransferingProject info = pro as TransferingProject;
            //if (!this.IsHandleCreated)
            //{
            //    return;
            //}
            this.uiAction.ActionOnUI<TransferingProject>(new Action<TransferingProject>(this.dofileTransStarted), info);
        }


        void dofileTransStarted(ITransferingProject pro)
        {
            TransferingProject info = (pro as TransferingProject);
            if (this.projectFilter != null)
            {
                if (!this.projectFilter(info))
                {
                    return;
                }
            }
            this.AddFileTransItem(info, false, true);
            GGFileBox item = this.GetExistedItem(info.ProjectID);
            if (item != null)
            {
                item.IsTransfering = true;
                item.Transmit_Running();
            }
        }
        #endregion

        #region fileTransProgress
        void fileTransProgress(string projectID, ulong total, ulong sended)
        {
            //if (!this.IsHandleCreated)
            //{
            //    return;
            //}
            this.uiAction.ActionOnUI<string, ulong, ulong>(new System.Action<string, ulong, ulong>(this.dofileTransProgress), projectID, total, sended);
        }

        void dofileTransProgress(string projectID, ulong total, ulong sended)
        {
            GGFileBox item = this.GetExistedItem(projectID);
            if (item != null)
            {
                item.SetProgress(total, sended);
            }
        }
        #endregion

        #region fileTransDisruptted
        void fileTransDisruptted(ITransferingProject pro, FileTransDisrupttedType disrupttedType, string cause)
        {
            TransferingProject info = (TransferingProject)pro;
            this.uiAction.ActionOnUI(new System.Action<TransferingProject, FileTransDisrupttedType, string>(this.dofileTransDisruptted), info, disrupttedType, cause);
        }

        void dofileTransDisruptted(TransferingProject info, FileTransDisrupttedType disrupttedType, string cause)
        {
            GGFileBox item = this.GetExistedItem(info.ProjectID);
            if (item != null)
            {
                cause = cause ?? FileTransHelper.GetTipMessage4FileTransDisruptted(info.IsSender, disrupttedType, cause == FriendChatForm.Token4ConvertToOfflineFile);
                item.Transmit_Disruptted(cause);
                if (cause == "转为离线发送")
                {
                    this.controlList.Remove(item);
                    this.AutoSizeChanged();
                }
                //this.flowLayoutPanel1.Controls.Remove(item);
                this.FileTransDisruptted(info.ProjectName, info.IsSender, disrupttedType, cause);
            }
        }
        #endregion

        #region fileTransCompleted
        void fileTransCompleted(ITransferingProject pro)
        {
            TransferingProject info = pro as TransferingProject;
            //if (!this.IsHandleCreated)
            //{
            //    return;
            //}
            this.uiAction.ActionOnUI(new Action<TransferingProject>(this.dofileTransCompleted), info);
        }

        void dofileTransCompleted(TransferingProject info)
        {
            GGFileBox item = this.GetExistedItem(info.ProjectID);
            if (item != null)
            {
                this.FileTransCompleted2(info);
                item.Transmit_Completed();
                //this.flowLayoutPanel1.Controls.Remove(item);
                this.FileTransCompleted(info.ProjectName, info.IsSender, info.Comment, info.IsFolder);
            }
        }
        #endregion


        #region AddFileTransItem ,GetFileTransItem        
        private void AddFileTransItem(TransferingProject project, bool offlineFile, bool doneAgreed)
        {
            if (this.projectFilter != null)
            {
                if (!this.projectFilter(project))
                {
                    return;
                }
            }
            GGFileBox ggFileBox = this.GetExistedItem(project.ProjectID);
            if (ggFileBox != null)
            {
                return;
            }
            ggFileBox = new GGFileBox(this.parent);
            ggFileBox.FileCanceled += fileTransItem_FileCanceled;
            ggFileBox.FileReceived += fileTransItem_FileReceived;
            ggFileBox.FileRejected += fileTransItem_FileRejected;
            ggFileBox.ConvertToOfflineFile += new CbGeneric<TransferingProject, GGFileBox>(fileTransItem_ConvertToOfflineFile);
            SenderInfo senderInfo = new SenderInfo();
            senderInfo.IsMe = project.IsSender;
            string senderID = project.SenderID == NetServer.SystemUserID ? Comment4OfflineFile.ParseUserID(project.Comment) : project.SenderID;
            GGUser sender = this.resourceCenter.ClientGlobalCache.GetUser(CommonHelper.GetUserID4OMCS(senderID));
            senderInfo.HeadImg = GlobalResourceManager.GetHeadImage(sender);
            senderInfo.DisplayName = sender.DisplayName;
            senderInfo.SenderShowType = this.SenderShowType;
            ggFileBox.Initialize(project, offlineFile, doneAgreed, senderInfo);
            this.fileBoxManager.Add(ggFileBox.TransferingProject.ProjectID, ggFileBox);
            this.controlList.Add(ggFileBox);
            this.FileTransStarted(project.ProjectName, project.IsSender);
            this.AutoSizeChanged();
            this.ContentScrollToEnd();
        }

        void fileTransItem_ConvertToOfflineFile(TransferingProject project, GGFileBox item)
        {
            if (this.FileNeedOfflineSend != null)
            {
                this.FileNeedOfflineSend(project);
            }
        }

        private GGFileBox GetExistedItem(string projectID)
        {
            foreach (string item in this.fileBoxManager.GetKeyList())
            {
                if (item == projectID)
                {
                    return this.fileBoxManager.Get(item);
                }
            }
            return null;
        }

        void fileTransItem_FileRejected(string projectID)
        {
            this.fileOutter.RejectFile(projectID);
        }

        void fileTransItem_FileReceived(GGFileBox item, string projectID, bool isSend, string savePath)
        {
            this.fileOutter.BeginReceiveFile(projectID, savePath);
        }

        void fileTransItem_FileCanceled(GGFileBox item, string projectID, bool isSend)
        {
            this.fileOutter.CancelTransfering(projectID);
        }
        #endregion      



        #region EngineAction

        private void CheckZeroSpeed()
        {
            //if (!this.IsHandleCreated)
            //{
            //    return;
            //}

            this.uiAction.ActionOnUI(new Action(this.doCheckZeroSpeed));
        }

        void doCheckZeroSpeed()
        {
            foreach (object obj in this.fileBoxManager.GetAll())
            {
                FileTransferItem item = obj as FileTransferItem;
                if (item != null && item.IsTransfering)
                {
                    item.CheckZeroSpeed();
                }
            }
        }
        #endregion


        #endregion
    }

    internal interface IUIAction
    {
        void ActionOnUI(Action action);

        void ActionOnUI<T>(Action<T> action,T t);

        void ActionOnUI<T1, T2>(System.Action<T1, T2> action, T1 t1, T2 t2);

        void ActionOnUI<T1,T2,T3>(System.Action<T1, T2, T3> action, T1 t1,T2 t2 ,T3 t3);
    }

    internal interface IChatControl
    {
        //
        // 摘要:
        //     使控件的指定区域无效（将其添加到控件的更新区域，下次绘制操作时将重新绘制更新区域），并向控件发送绘制消息。
        //
        // 参数:
        //   region:
        //     要使之无效的 System.Drawing.Region。
        void Invalidate(Region region);
        //
        // 摘要:
        //     使控件的指定区域无效（将其添加到控件的更新区域，下次绘制操作时将重新绘制更新区域），并向控件发送绘制消息。
        //
        // 参数:
        //   rc:
        //     一个 System.Drawing.Rectangle，表示要使之无效的区域。
        void Invalidate(Rectangle rc);
        //
        // 摘要:
        //     使控件的整个图面无效并导致重绘控件。
        void Invalidate();

        Rectangle Bounds { get; set; }

        /// <summary>
        /// 获取或设置当鼠标指针位于控件上时显示的光标。
        /// </summary>
        ChatBoxCursors Cursor { get; set; }
    }

    internal enum ChatBoxCursors
    {
        Arrow=0,
        Hand,
        Wait
    }



}
