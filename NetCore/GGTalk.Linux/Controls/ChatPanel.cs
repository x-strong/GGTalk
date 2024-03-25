using CPF;
using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using ESBasic;
using ESBasic.Loggers;
using ESBasic.ObjectManagement.Managers;
using ESFramework.Extensions.ChatRendering;
using ESPlus.Serialization;
using GGTalk.Controls.ChatRender4Dll;
using GGTalk.Linux.Controller;
using GGTalk.Linux.Controls.ChatRender4Dll;
using GGTalk.Linux.Helpers;
using GGTalk.Linux.Views;
using OVCS.Client.Linux.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TalkBase;
using TalkBase.NetCore.Client.Core;

namespace GGTalk.Linux.Controls
{
    internal class ChatPanel : Control, IUserNameGetter, OrayChatContextMenuHandler
    {
        private IChatRender chatRender => this.chatBox?.ChatRender;
        private IUnit destUnit;
        private bool IsInHisBlackList = false;
        private int unhanleMessageCount = 0;//未提取的消息数量
        public EmojiPanel emojiPanel;
        private ESFramework.Boost.NetCore.Controls.ChatBox.ChatBox chatBox;
        private ChatBox inputbox;
        private ObjectManager<string, string> atSelectedUserManager = new ObjectManager<string, string>();
        public event Action CloseChatPanel;

        

        public void Initialize(IUnit unit)
        {
            this.destUnit = unit;
            this.InitChatBox();
            //this.unhanleMessageCount = MainWindow.MessageCacheManager.GetUnhanleMessageCount(this.destUnit);
            if (unit.UnitType == TalkBase.UnitType.User)
            {
                if (Program.ResourceCenter.Connected)
                {
                    this.IsInHisBlackList = Program.ResourceCenter.ClientOutter.IsInHisBlackList(unit.ID);
                }
            }

            //List<UnReadMsgModel> datas = UnReadMsgBox.Singleton.Get(this.destUnit.ID);
            //if (datas != null)
            //{
            //    foreach (var data in datas)
            //    {
            //        this.SetHistory(data.unit, data.bytes);
            //    }
            //    CommonHelper.ClearUnReadMsg(this.destUnit.ID);
            //} 
            //else 
            
            if (this.destUnit.LastWordsRecord != null)
            {
                IUnit unit1 = Program.ResourceCenter.ClientGlobalCache.GetUnit(this.destUnit.LastWordsRecord.SpeakerID);
                this.SetHistory(unit1, this.destUnit.LastWordsRecord.ChatContent);
            }

            this.Window_Changed();


            #region 获取未读消息

            //TwinkleNotifyIcon twinkleNotifyIcon = MainWindow.MessageCacheManager as TwinkleNotifyIcon;
            //if (twinkleNotifyIcon != null)
            //{
            //    twinkleNotifyIcon.PickUnReadMSgForUnitID(this.destUnit.ID);
            //}

            #endregion

        }
         

        private void SetHistory(IUnit speaker, byte[] data)
        { 
            ChatBoxContent2 content = CompactPropertySerializer.Default.Deserialize<ChatBoxContent2>(data, 0);
            bool isMe = speaker.ID == Program.ResourceCenter.CurrentUserID;
            if (this.destUnit.IsUser)
            {
                this.AppendChatBoxContent(isMe, isMe ? Program.ResourceCenter.ClientGlobalCache.CurrentUser : (GGUser)this.destUnit, this.destUnit.LastWordsRecord.SpeakTime, content);
            }
            else
            {
                string talker = speaker.DisplayName + "（" + this.destUnit.LastWordsRecord.SpeakerID + ")";  //string.Format("{0}({1})", speaker.DisplayName, record.SpeakerID);
                this.AppendChatBoxContent(isMe, (GGUser)speaker, this.destUnit.LastWordsRecord.SpeakTime, content);
            }
        }
             
         

        private void SetHistoryRecord()
        {
            this.Cursor = Cursors.Wait;
            this.chatBox.Cursor = Cursors.Wait;
            Task task = Task.Run(() =>
            {
                try
                {
                    System.Threading.Thread.Sleep(10);
                    ChatRecordPage page = this.GetChatRecordPage(this.pageIndex);
                    if (page == null || page.Content.Count == 0)
                    {
                        return;
                    }
                    UiSafeInvoker.ActionOnUI<ChatRecordPage, bool>(this.SetHistoryRecord2, page, true);
                }
                catch (Exception ee)
                {
                    GlobalResourceManager.WriteErrorLog(ee);
                }
                finally {
                    this.Cursor = Cursors.Arrow;
                    this.chatBox.Cursor = Cursors.Ibeam;
                }
            });
        }
        private void SetHistoryRecord2(ChatRecordPage page, bool lastPage)
        {
            this.SetHistoryChatMessageRecord(page, true);
            this.AppendSysMessage("——— 以上是历史消息 ———");
            this.ScrollToEnd();
        }


        #region 历史消息
        private int pageSize = 50;
        private int pageIndex = 0;
        private int currentPageIndex = -1;

        private IChatRecordGetter CurrentChatRecordGetter
        {
            get
            {
                return Program.ResourceCenter.RemoteChatRecordGetter;
            }
        }

        private ChatRecordPage GetChatRecordPage(int pageIndex)
        {
            ChatRecordPage page = null;
            if (this.destUnit.UnitType == TalkBase.UnitType.Group)
            {
                page = this.CurrentChatRecordGetter.GetGroupChatRecordPage(ChatRecordTimeScope.All, this.destUnit.ID, this.pageSize, pageIndex);
            }
            else
            {
                page = this.CurrentChatRecordGetter.GetChatRecordPage(ChatRecordTimeScope.All, Program.ResourceCenter.CurrentUserID, this.destUnit.ID, this.pageSize, pageIndex);
            }
            return page;
        }

        private void SetHistoryChatMessageRecord(ChatRecordPage page, bool lastPage)
        {
            if (page == null || page.Content.Count == 0)
            {
                return;
            }

            int leftCount = page.Content.Count;
            if (lastPage)
            {
                leftCount = page.Content.Count - this.unhanleMessageCount;
                if (leftCount <= 0)
                {
                    return;
                }
            }

            for (int i = 0; i < leftCount; i++)
            {
                ChatMessageRecord record = page.Content[i];
                byte[] msg = record.Content;
                ChatBoxContent2 content = ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<ChatBoxContent2>(msg, 0);

                IUser speaker = Program.ResourceCenter.ClientGlobalCache.GetUser(record.SpeakerID);
                Parameter<string, string> my = Program.ResourceCenter.ClientGlobalCache.CurrentUser.GetIDName();
                //Parameter<string,string> dest=
                bool isMe = speaker.ID == Program.ResourceCenter.CurrentUserID;
                if (this.destUnit.IsUser)
                {

                    this.AppendChatBoxContent(isMe, isMe ? Program.ResourceCenter.ClientGlobalCache.CurrentUser : (GGUser)this.destUnit, record.OccureTime, content);
                }
                else
                {
                    string talker = speaker.DisplayName + "（" + record.SpeakerID + ")";  //string.Format("{0}({1})", speaker.DisplayName, record.SpeakerID);
                    this.AppendChatBoxContent(isMe, (GGUser)speaker, record.OccureTime, content);
                }

            }
        }
        #endregion
        private bool first =true;
        protected override void OnPropertyChanged(string propertyName, object oldValue, object newValue, PropertyMetadataAttribute propertyMetadata)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue, propertyMetadata);

            if (propertyName == "ActualSize" && this.chatBox != null )
            {
                if (first)
                {
                    first = false;
                    Task task = Task.Run(() =>
                    {
                        try
                        {
                            System.Threading.Thread.Sleep(600);
                            this.Window_Changed();
                        }
                        catch (Exception ee)
                        {
                            GlobalResourceManager.WriteErrorLog(ee);
                        }
                    });
                }
                
                this.Window_Changed();
            }
        }

        protected override void InitializeComponent()
        {//模板定义
            Children.Add(new Grid
            {
                Size = SizeField.Fill,
                Background = null,
                ClipToBounds = true,

                RowDefinitions =
                        {
                            new RowDefinition
                            {

                            },
                            new RowDefinition
                            {
                                Height = 170
                            },
                        },
                Children = {


                    new ESFramework.Boost.NetCore.Controls.ChatBox.ChatBox
                    {
                        Name = nameof(this.chatBox),
                        chatPanel = this,
                        PresenterFor = this,
                        Attacheds={
                            {
                                Grid.RowIndex,
                                0
                            },
                        }
                    },

                    new Grid
                    {

                        Size = SizeField.Fill,
                        Background = null,
                        Children =
                        {
                            new EmojiPanel
                            {
                                Visibility = Visibility.Hidden,
                                Name = nameof(this.emojiPanel),
                                PresenterFor = this,
                                MarginBottom = 170,
                                MarginLeft = 0,
                                Commands =
                                {
                                    {
                                        nameof(EmojiPanel.LostFocus),
                                        (s,e)=>this.Emoji_LostFocus()
                                    }
                                }
                            },
                            new Panel
                            {
                                Height = 40,
                                Width = "100%",
                                Background = "#adcdeb",
                                MarginBottom = 130,
                                Children =
                                {
                                    new Button
                                    {
                                        Width = 24,
                                        Height = 24,
                                        Classes = "ChatButton",
                                        ToolTip = "表情",
                                        MarginLeft = 10,
                                        Commands =
                                        {
                                            {
                                                nameof(Button.Click),
                                                (s,e)=>this.Click_EmojiPanel()
                                            }
                                        },
                                        Content =
                                                new Picture
                                                {
                                                    Stretch = Stretch.Fill,
                                                    Source = CommonOptions.ResourcesCatalog+"expression_icon.png",
                                                }
                                    },
                                    new Button
                                    {
                                        Classes = "ChatButton",
                                        ToolTip = "图片",
                                        MarginLeft = 46,
                                        Width = 24,
                                        Height = 24,
                                        Content =
                                                new Picture
                                                {
                                                    Stretch = Stretch.Fill,
                                                    Source = CommonOptions.ResourcesCatalog+"pic.png",
                                                },
                                        Commands={ { nameof(MouseDown),(s,e)=>this.SendScreenshot_Click() } }
                                    },
                                    new Button
                                    {
                                        Classes = "ChatButton",
                                        Cursor = Cursors.Hand,
                                        MarginRight = 18,
                                        Width = 80,
                                        Height = 24,
                                        BorderThickness = new Thickness(0, 0, 0, 0),
                                        BorderType = BorderType.BorderThickness,
                                        Content =
                                                new Panel
                                                {
                                                    Children =
                                                    {
                                                        new Picture
                                                        {
                                                            Width = 26,
                                                            Height = 26,
                                                            Stretch = Stretch.Fill,
                                                            MarginLeft = -26,
                                                            MarginBottom = 1,
                                                            Source = CommonOptions.ResourcesCatalog+"record_icon.png",
                                                        },
                                                        new TextBlock
                                                        {
                                                            Text = "消息记录",
                                                            Foreground = "#000000",
                                                            MarginRight = -28
                                                        }
                                                    }
                                                },
                                        Commands={ { nameof(MouseDown),(s,e)=>this.ChatRecord_Click() } }
                                    }
                                }
                            },
                            new ChatBox
                            {
                                Name = nameof(this.inputbox),
                                PresenterFor = this,
                                Height = 90,
                                Width = "100%",
                                MarginBottom=42,
                                WordWarp = true,
                                IsAllowPasteImage = false,
                            },
                            new Panel
                            {
                                Height = 42,
                                Width = "100%",
                                Background = "#adcdeb",
                                MarginBottom = 0,
                                Children =
                                {
                                    new Button
                                    {
                                        Classes = "CheckButton",
                                        Width = 60,
                                        Height = 27,
                                        Content = "关闭",
                                        Cursor=Cursors.Hand,
                                        MarginRight = 100,
                                        Foreground = "#000000",
                                        Commands={ { nameof(MouseDown),(s,e)=>this.CloseWindow() } }
                                    },
                                    new Button
                                    {
                                        Classes = "CheckButton",
                                        Width = 60,
                                        Height = 27,
                                        Content = "发送",
                                        Cursor=Cursors.Hand,
                                        ToolTip="按Enter键发送，按Ctrl+Enter换行",
                                        MarginRight = 20,
                                        Foreground = "#000000",
                                        Commands={ { nameof(MouseDown),(s,e)=>this.SendMessage() } }
                                    }
                                }
                            }
                        },
                        Attacheds={
                            {
                                Grid.RowIndex,
                                1
                            },
                        }

                    }
                }
            });
            if (!this.DesignMode)
            {
                this.chatBox = this.FindPresenterByName<ESFramework.Boost.NetCore.Controls.ChatBox.ChatBox>(nameof(this.chatBox));
                this.emojiPanel = FindPresenterByName<EmojiPanel>(nameof(this.emojiPanel));
                this.emojiPanel.EmotionClicked += EmojiPanel_EmotionClicked;
                this.inputbox = FindPresenterByName<ChatBox>(nameof(this.inputbox));
                this.inputbox.KeyDown += Inputbox_KeyDown;
                this.chatBox.ChatRender_ShowContextMenu += ChatBox_ChatRender_ShowContextMenu;
            }

        }


        private void Inputbox_KeyDown(object sender, CPF.Input.KeyEventArgs e)
        {
            if (e.Modifiers != CPF.Input.InputModifiers.Control && e.Key == CPF.Input.Keys.Enter)//按Enter键发送消息，Ctrl+ Enter 换行
            {
                this.SendMessage();
                e.Handled = true; 
            }
            else if (e.Modifiers == InputModifiers.Control && e.Key == Keys.V)
            {
                string copyContent = Clipboard.GetData(DataFormat.Text) as string;
                if(!string.IsNullOrEmpty(copyContent))
                { 
                    this.inputbox.Past4TextBox(copyContent);
                    e.Handled = true;
                }
            }
        }
         


        /// <summary>
        /// 向ChatBox中添加聊天内容
        /// </summary>
        /// <param name="isMe"></param>
        /// <param name="sender"></param>
        /// <param name="originTime"></param>
        /// <param name="content"></param>
        public void AppendChatBoxContent(bool isMe, GGUser sender, DateTime originTime, ChatBoxContent2 content)
        {
            string str = CommonHelper.ParseChatBoxContent(content);
            Dictionary<uint, NonTextItem> dictionary_Img = content.NonTextItemDictionary;
            if (string.IsNullOrEmpty(str) && dictionary_Img?.Count > 0)
            {
                List<uint> list = dictionary_Img.Keys.ToList();
                foreach (var obj in list)
                {
                    NonTextItem nonTextItem = content.NonTextItemDictionary[obj];
                    if (nonTextItem.ChatBoxElementType == ChatBoxElementType.ForeignImage)
                    {
                        this.AppendImage(sender.ID, nonTextItem.ForeignImage, originTime);
                        continue;
                    }
                }
                return;
            }
            this.AppendText(sender.ID, str, originTime);
        }

         

        /// <summary>
        /// 向ChatBox中添加系统消息
        /// </summary>
        /// <param name="msg"></param>
        public void AppendSysMessage(string msg)
        {
            this.AppendSystemMsg(msg);
        }

        /// <summary>
        /// 获取输入框中的聊天内容
        /// </summary>
        /// <returns></returns>
        private ChatBoxContent2 GetContent()
        {
            GGInputContent ggInputContent = this.inputbox.GetInputContent();
            ChatBoxContent2 chatBoxContent = new ChatBoxContent2(ggInputContent.Text, SystemSettings.DefaultSimpleFont, System.Drawing.Color.Black);
            foreach (KeyValuePair<int, int> item in ggInputContent.EmotionDictionary)
            {
                chatBoxContent.AddEmotion((uint)item.Key, (uint)item.Value);
            }
            foreach (KeyValuePair<int, Image> item in ggInputContent.ImageDictionary)
            {
                chatBoxContent.AddForeignImage((uint)item.Key, item.Value);
            }
            return chatBoxContent;
        }

        private ChatBoxContent2 CreateImageContent(Image bitmap)
        {
            ChatBoxContent2 chatBoxContent = new ChatBoxContent2(" ", SystemSettings.DefaultSimpleFont, System.Drawing.Color.Black);
            chatBoxContent.AddForeignImage(0, bitmap);
            return chatBoxContent;
        }

        private void SendMessage(ChatBoxContent2 content, bool isClearInputbox = true)
        {
            try
            {
                if (!Program.ResourceCenter.RapidPassiveEngine.Connected)
                {
                    MessageBoxEx.Show("已经掉线。");
                    //this.toolShow.Show("已经掉线。", this.skinButton_send, new Point(this.skinButton_send.Width / 2, -this.skinButton_send.Height), 3000);
                    return;
                }
                if (this.IsInHisBlackList)
                {
                    MessageBoxEx.Show("对方已将您加入黑名单，不能发送消息！");
                    return;
                }


                if (content.IsEmpty())
                {
                    return;
                }

                byte[] buff = CompactPropertySerializer.Default.Serialize(content);

                ChatMessageRecord record = null;
                if (this.destUnit.UnitType == TalkBase.UnitType.User)
                {
                    record = Program.ResourceCenter.ClientOutter.SendChatMessage(this.destUnit.ID, buff);
                }
                else if (this.destUnit.UnitType == TalkBase.UnitType.Group)
                {
                    string atMemberIDs = this.GetAtMemberIDs();
                    record = Program.ResourceCenter.ClientOutter.SendGroupChatMessage(this.destUnit.ID, buff, atMemberIDs);
                }
                bool followingWords = false;

                if (this.destUnit.LastWordsRecord != null && this.destUnit.LastWordsRecord.SpeakerID == Program.ResourceCenter.CurrentUserID)
                {
                    followingWords = (DateTime.Now - this.destUnit.LastWordsRecord.SpeakTime).TotalSeconds <= 30;
                }
                content.RecordID = (int)record.AutoID;
                this.AppendChatBoxContent(true, Program.ResourceCenter.ClientGlobalCache.CurrentUser, DateTime.Now, content);
                this.ScrollToEnd();

                if (isClearInputbox)
                {
                    //清空输入框
                    this.inputbox.Clear();
                }

            }
            catch (Exception ee)
            {
                GlobalResourceManager.Logger.Log(ee, "ChatPanel.btnSend_Click", ESBasic.Loggers.ErrorLevel.Standard);
                this.AppendSysMessage("发送消息失败！" + ee.Message);
            }
            finally
            {
                UIThreadPoster uIThreadPoster = new UIThreadPoster(delegate { this.inputbox.Focus(); });
                uIThreadPoster.Post();
            }

        }

        #region HandleChatMessage
        public void HandleChatMessage(string broadcasterID, ChatBoxContent2 content, DateTime? originTime, string tag)
        {
            if (originTime == null)
            {
                originTime = DateTime.Now;
            }
            if (this.destUnit.IsUser)
            {
                //bool followingWords = false;
                //if (this.destUnit.LastWordsRecord != null && this.destUnit.LastWordsRecord.SpeakerID != this.resourceCenter.CurrentUserID)
                //{
                //    followingWords = (DateTime.Now - this.destUnit.LastWordsRecord.SpeakTime).TotalSeconds <= 30;
                //}
                this.AppendChatBoxContent(false, (GGUser)this.destUnit, originTime.GetValueOrDefault(), content);
            }
            else
            {
                GGUser user = Program.ResourceCenter.ClientGlobalCache.GetUser(broadcasterID);
                string talker = string.Format("{0}({1})", broadcasterID, broadcasterID);
                if (user != null)
                {
                    talker = string.Format("{0}({1})", user.Name, user.UserID);
                }
                System.Drawing.Color color = broadcasterID == Program.ResourceCenter.CurrentUserID ? System.Drawing.Color.SeaGreen : System.Drawing.Color.Blue;
                this.AppendChatBoxContent(false, user, originTime.GetValueOrDefault(), content);
                //检查是否自己被@
                if (string.IsNullOrEmpty(tag))
                {
                    this.ScrollToEnd();
                    return;
                }
                List<string> memberIDs = new List<string>(tag.Split(','));

                //if (memberIDs.Contains(this.resourceCenter.CurrentUserID) || memberIDs.Contains("@all"))
                //{
                //    this.atMessagePanle1.Initialize(user.DisplayName, GlobalResourceManager.GetHeadImageOnline(user), content);
                //    this.atMessagePanle1.Visible = true;
                //}
            }
            this.ScrollToEnd();

        }

        public void HandleChatMessageOfMine(ChatBoxContent2 content)
        {
            this.AppendChatBoxContent(true, Program.ResourceCenter.ClientGlobalCache.CurrentUser, DateTime.Now, content);
            this.ScrollToEnd();
        }

        public void OnFileEvent(string msg, string filePath)
        {
            UiSafeInvoker.ActionOnUI<string>(this.AppendSysMessage, msg);
            //this.AppendMessage("系统", Color.Gray, msg);
            //this.chatBox_history.AppendText("\n");
            //if (filePath != null)
            //{
            //    this.chatBox_history.AppendFileLink("    ", filePath);
            //    this.chatBox_history.AppendText("\n");
            //}
        }

        #endregion

        #region 窗体按钮事件

        private void EmojiPanel_EmotionClicked(int emotionIndex, Image image)
        {
            this.emojiPanel.Visibility = Visibility.Hidden;
            this.inputbox.AppentEmoji(emotionIndex);
        }

        //点击表情框
        public void Click_EmojiPanel()
        {
            if (emojiPanel.Visibility == Visibility.Visible)
            {
                emojiPanel.Visibility = Visibility.Hidden;
            }
            else {
                emojiPanel.Visibility = Visibility.Visible;
                emojiPanel.Focus();
            }
        }
        //表情框失去焦点事件
        public void Emoji_LostFocus()
        {
            emojiPanel.Visibility = Visibility.Hidden;
        }

        public void CloseWindow()
        {
            if (this.CloseChatPanel != null)
            {
                this.CloseChatPanel();
            }
        }

        //点击发送按钮
        public void SendMessage()
        {
            ChatBoxContent2 content = this.GetContent();
            this.SendMessage(content);
        }

        public void ChatRecord_Click() {
            if (Program.ResourceCenter.ClientGlobalCache.CurrentUser.UserStatus == UserStatus.OffLine)
            {
                return;
            }

            ChatRecordWindow form = null;
            if (this.destUnit.IsUser)
            {
                form = new ChatRecordWindow((IUser)this.destUnit, this);
            }
            else
            {
                form = new ChatRecordWindow((IGroup)this.destUnit, this);
            }
            form.Show_Topmost();
        }

        public void SendScreenshot_Click() {
            //string title = "选择图片";
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.AllowMultiple = true;
            //FileDialogFilter filter = new FileDialogFilter() { Name = "*" };
            //filter.Extensions = "bmp,jpeg,jpg,png";// new List<string>() { "jpg", "png" };
            //openFileDialog.Filters = new List<FileDialogFilter>() { filter };
            //if (!string.IsNullOrEmpty(title))
            //{
            //    openFileDialog.Title = title;
            //}
            //Task<string[]> task = openFileDialog.ShowAsync((Window)this.Root);
            //await task.ConfigureAwait(false);
            //UiSafeInvoker.ActionOnUI<string[]>(this.SendImage4Path, task.Result);
            FileHelper.FileToOpen4Action((Window)this.Root, "选择图片", GlobalConsts.ImageExtensions, false, this.SendImage4Path);
        }
        private async void SendImage4Path(string[] paths)
        {
            try
            {
                if (paths.Length == 0) { return; }
                if (!string.IsNullOrEmpty(paths[0]))
                {
                    ImageSelectorWindow imageSelectorWindow = new ImageSelectorWindow(paths[0], "发送");
                    Task<object> task = imageSelectorWindow.ShowDialog();
                    await task.ConfigureAwait(false); 
                    if (task.Result == null)
                    {
                        return;
                    }
                    this.SendMessage(this.CreateImageContent(imageSelectorWindow.ContentBitmap), false);
                }
            }
            catch (Exception ee)
            {
                GlobalResourceManager.WriteErrorLog(ee);
            }
        }
        #endregion



        //获取at用户的ID 集合
        private string GetAtMemberIDs()
        {
            string memberIDs = "";
            string text = this.inputbox.GetInputContent().Text;
            Dictionary<string, string> dic = this.atSelectedUserManager.ToDictionary();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                if (text.Contains('@' + kvp.Value))
                {
                    memberIDs += kvp.Key + ",";
                }
            }
            if (memberIDs.Length > 0)
            {
                memberIDs = memberIDs.Substring(0, memberIDs.Length - 1);
            }
            this.atSelectedUserManager.Clear();
            return memberIDs;
        }

        #region IUserNameGetter
        public string GetUserName(string userID)
        {
            return Program.ResourceCenter.ClientGlobalCache.GetUserName(userID);
        }
        #endregion 

        #region InitChatBox_History

        /// <summary>
        /// 初始化ChatRender
        /// </summary>
        private void InitChatBox()
        {
            bool multiSpeaker = this.destUnit.UnitType == UnitType.Group;
            RenderAttr attr = new RenderAttr(Program.ResourceCenter.CurrentUserID, this.destUnit.ID, multiSpeaker, WinRenderType.Winform);
            this.chatBox.InitChatRender(attr);
        }

        #endregion

        #region RenderChatMsg

        private List<string> guidMangers;
        private object locker = new object();
        //private ObjectManager<string, SnapchatMessage> snaps;
        private DateTime lastMessageTime = DateTime.MinValue;

        /// <summary>
        /// 绘制系统时间
        /// </summary>
        /// <param name="time"></param>
        private void AppendSendTime(DateTime? sendTime = null)
        {
            lock (this.locker)
            {
                DateTime time = sendTime == null ? DateTime.Now : (DateTime)sendTime;
                if (time.Subtract(this.lastMessageTime) > TimeSpan.FromMinutes(5))
                {
                    this.lastMessageTime = time;
                    this.chatRender.AddChatItemTime(time);
                }
            }
        }

        /// <summary>
        /// 绘制系统消息
        /// </summary>
        /// <param name="strings"></param>
        public void AppendSystemMsg(string str)
        {
            if (string.IsNullOrEmpty(str)) return;
            this.AppendSendTime();
            this.chatRender.AddChatItemSystemMessage(str);
            this.Window_Changed();
        }

        private void AppendText(string speakerID, string text, DateTime? time)
        {
            if (string.IsNullOrEmpty(text)) return;
            string guid = Guid.NewGuid().ToString();
            this.AppendSendTime(time);
            this.chatRender.AddChatItemText(guid, speakerID, text);
            this.AddGuidToManager(guid);
            this.Window_Changed();
        }


        /// <summary>
        /// 在调用AppendImage时，需要生成一个唯一的GUID作为key Image是Value值存到字典中
        /// </summary>
        private ObjectManager<string, ImgModel> chatRenderImgList;
        private void SaveImageToManager(string guid, ImgModel model)
        {
            if (chatRenderImgList == null) chatRenderImgList = new ObjectManager<string, ImgModel>();
            chatRenderImgList.Add(guid, model);
        }

        private void AppendImage(string speakerID,Image sourceImg, DateTime? time)
        {
            if (sourceImg == null) return;
            Image img = CommonHelper.CropPicture(sourceImg);
            this.AppendSendTime(time);
            string guid = Guid.NewGuid().ToString();
            ImgModel model = new ImgModel(guid, sourceImg, img);
            Image image = model.GetShowImage(); 
            this.SaveImageToManager(guid, model);
            this.chatRender.AddChatItemImage(guid, speakerID, image, new System.Drawing.Size(image.Width, image.Height));
            this.AddGuidToManager(guid);
            this.Window_Changed();
        }

        //private void AppendAudio(string audioID, string speakerID, AudioMessage msg, DateTime? time)
        //{
        //    this.AppendSendTime(time);
        //    int secs = CommonHelper.GetAudioMessageSecs(msg);
        //    this.chatRender.AddChatItemAudio(audioID, speakerID, secs, msg);
        //    this.Window_Changed();
        //}

        //private void AppendSnap(SnapchatMessage snapchat, string speakerID, DateTime? time)
        //{
        //    this.AppendSendTime(time);
        //    string guid = speakerID + "-" + snapchat.ID;
        //    this.AddSnapToManager(guid, snapchat);
        //    this.chatRender.AddChatItemSnap(guid, speakerID);
        //    this.Window_Changed();
        //}
         
        private void AddGuidToManager(string guid)
        {
            if (this.guidMangers == null)
                this.guidMangers = new List<string>();
            this.guidMangers.Add(guid);
        }


        //private void AddSnapToManager(string guid, SnapchatMessage snapchat)
        //{
        //    if (snaps == null) snaps = new ObjectManager<string, SnapchatMessage>();
        //    snaps.Add(guid, snapchat);
        //}

        //internal SnapchatMessage GetSnapForId(string id)
        //{
        //    return snaps.Get(id);
        //}

        private void RemoveMsg(string guid)
        {
            this.chatRender.RemoveChatMessage(guid);
        }

        private void ScrollToEnd()
        {
            this.chatBox.ScrollToEnd();
        }

        internal void Window_Changed()
        {
            this.chatBox.RefreshUI();
            this.ScrollToEnd();
        } 

        #endregion

        #region ChatRender ContextMenu 

        private string selectGuid;

        private ChatMessageType selectType;

        private void ChatBox_ChatRender_ShowContextMenu(System.Drawing.Point point, ChatMessageType type, string guid)
        {
            if (guidMangers == null || guidMangers.Count <= 0) return;
            this.selectGuid = string.Empty;
            this.selectType = type;
            if (type == ChatMessageType.Image)
            {
                this.selectGuid = guid;
            }
            bool isShowMenuItem = !(string.IsNullOrEmpty(selectGuid));
            ChatContextMenu.SetHandler(this);
            ChatContextMenu.SetCopyVisible(isShowMenuItem || type == ChatMessageType.TextEmotion);
            ChatContextMenu.SetSaveVisible(isShowMenuItem);
            ChatContextMenu.SetShowImageVisible(isShowMenuItem);
            ChatContextMenu.Show();
        }


        public void OnCopy()
        {
            try
            { 
                if (selectType == ChatMessageType.TextEmotion)
                {
                    string str = this.chatRender.GetSelectedText();
                    if (!string.IsNullOrEmpty(str))
                    {
                        Clipboard.SetData((DataFormat.Text, str));
                    } 
                }
                else if(selectType == ChatMessageType.Image)
                {
                    Image image = this.GetImageForGuid(selectGuid);
                    if (image == null) return;
                    Clipboard.SetData((DataFormat.Image, image));
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
                GlobalResourceManager.Logger.Log(ex, "ChatPanel.OnCopy", ErrorLevel.Standard);
            }
        }

        public async void OnSave()
        {
            Image image = this.GetImageForGuid(selectGuid);
            if (image == null) return; 
            System.Threading.Tasks.Task<string> task = FileHelper.SaveFileDialog((Window)this.Root, "另存为", DateTime.Now.Ticks + ".png");
            await task.ConfigureAwait(false);
            if (string.IsNullOrEmpty(task.Result)) { return; }
            image.Save(task.Result);
        }

        public void OnShowImage()
        {
            Image image = this.GetImageForGuid(selectGuid);
            if (image == null) return;
            PictureWindow pictureWindow = new PictureWindow(image);
            pictureWindow.Show_Topmost();
        }

        internal Image GetImageForGuid(string guid)
        { 
            ImgModel imgModel = chatRenderImgList.Get(guid);
            Image image = imgModel.sourceImg != null ? imgModel.sourceImg : imgModel.thumbnailImg;
            return image;
        }

        public void OnClear()
        {
            if(this.guidMangers != null)
            { 
                foreach (var guid in this.guidMangers)
                {
                    this.RemoveMsg(guid);
                }
            }
            this.guidMangers?.Clear();
            this.chatRenderImgList?.Clear();
            this.lastMessageTime = DateTime.MinValue;
            this.Window_Changed();
        }

        #endregion

    }
}
