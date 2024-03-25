using CPF;
using CPF.Controls;
using CPF.Documents;
using CPF.Drawing;
using CPF.Input;
using CPF.Shapes;
using GGTalk;
using GGTalk.Linux;
using GGTalk.Linux.Controls;
using GGTalk.Linux.Helpers;
using GGTalk.Linux.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GGTalk.Linux.Controls
{
    internal class ChatBox : TextBox
    {
        public string emojiPath = $"{AppDomain.CurrentDomain.BaseDirectory}Emotion{CommonHelper.SeparAtor}";
        private DateTime lastMessageTime = DateTime.MinValue;
        private Thickness thicknessIsMe;
        private Thickness ContentPadding;

        public  SudokuImageFill BackgroundEnd = null;
        public  Thickness rightness = new Thickness(0, 0, 30, 10);
        public  Thickness leftness = new Thickness(30, 0, 0, 10);
        private bool myselfOnTheRight = true;//我自己在右边
        private bool isChatRecord = false;//是否为聊天记录
        

        private object locker = new object();
        public bool IsChatRecord {
            get { return this.isChatRecord; }
            set { this.isChatRecord = value; this.myselfOnTheRight = !value; this.ShowMyHeadImg = false;this.ShowOtherHeadImg = false;this.ShowBackground = false; }
        }

        /// <summary>
        /// 显示自己的头像
        /// </summary>
        public bool ShowMyHeadImg = true;

        /// <summary>
        /// 显示其他人的头像
        /// </summary>
        public bool ShowOtherHeadImg = true;

        /// <summary>
        /// 显示自己的名称
        /// </summary>
        public bool ShowMyName = true;

        /// <summary>
        /// 显示其他人的名称
        /// </summary>
        public bool ShowOtherName = true;

        /// <summary>
        /// 单条记录是否显示背景
        /// </summary>
        public bool ShowBackground = true;

        private ContextMenu imgContextMenu;
        private Image currentSelectedImage;
        public ChatBox()
        {
            this.imgContextMenu = new ContextMenu
            {
                Items = new UIElement[]
                                {
                                    new MenuItem
                                    {
                                        Header = "另存为",
                                        Commands = {
                                            {
                                                nameof(MouseDown),
                                                (s, e) => SaveImage(this.currentSelectedImage)
                                            }
                                        }
                                    },   
                                    new MenuItem
                                    {
                                        Header = "新窗口弹出",
                                        Commands = {
                                            {
                                                nameof(MouseUp),
                                                (s, e) => ShowImage(this.currentSelectedImage)
                                            }
                                        }
                                    },
                                }
            };
        }

        /// <summary>
        /// 将最后聊天记录时间重置 为初始值
        /// </summary>
        public void InitLastMessageTime()
        {
            this.lastMessageTime = DateTime.MinValue;
        }

        public void Clear()
        {
            this.Document.Children.Clear();
            this.Text = string.Empty;
            this.ScrollToEnd();//将光标指针移动到最初的状态
        }

        public void AppendSysMessage(string msg)
        {            
            Block systemPrompt = new Block(msg) { TextAlignment = CPF.Drawing.TextAlignment.Center, Width = "100%",Top=4 };
            this.Document.Add(systemPrompt);
        }

        public void AppendChatMessage(bool isme, ChatBoxContent2 content, DateTime time, Image headImg, string _senderName)
        {
            lock (this.locker)
            {
                if (time.Subtract(this.lastMessageTime) > TimeSpan.FromMinutes(5))
                {
                    this.AppendSysMessage(time.ToString("G"));
                    this.lastMessageTime = time;
                }                    
                SudokuImageFill sudokuImageFill = new SudokuImageFill("");
                TextAlignment tEnd = isme && this.myselfOnTheRight ? TextAlignment.Right : TextAlignment.Left;
                Block block = new Block { TextAlignment = tEnd, Width = "100%" };

                if (tEnd == TextAlignment.Right)
                {
                    if (this.ShowMyName)
                    {
                        block.Add(new InlineUIContainer
                        {
                            UIElement = new TextBlock
                            {
                                Text = _senderName,
                                Foreground = "0,128,111",
                                MarginRight = 10
                            }
                        });
                    }
                    if (this.ShowMyHeadImg)
                    {
                        block.Add(new InlineUIContainer { UIElement = new Picture { Width = 26, Height = 26, IsAntiAlias = true, MarginBottom = -6, MarginRight = 10, Source = headImg, Stretch = Stretch.Fill } });
                    }
                    sudokuImageFill = new SudokuImageFill(CommonOptions.ResourcesCatalog + "mesendmessagepsb.png");
                    this.thicknessIsMe = this.rightness;
                }
                else
                {
                    if (this.ShowOtherHeadImg)
                    {
                        block.Add(new InlineUIContainer { UIElement = new Picture { Width = 26, Height = 26, IsAntiAlias = true, MarginBottom = -6, MarginRight = 10, Source = headImg, Stretch = Stretch.Fill } });
                    }
                    if (this.ShowOtherName) {
                        block.Add(new InlineUIContainer
                        {
                            UIElement = new TextBlock
                            {
                                Text = _senderName,
                                Foreground = "0,0,255"
                            }
                        });
                    }
                    this.thicknessIsMe = this.leftness;
                    sudokuImageFill = new SudokuImageFill(CommonOptions.ResourcesCatalog + "mesageboxpsb.png");

                }
                if (!isChatRecord && myselfOnTheRight && isme)
                {
                    ContentPadding = new Thickness(10, 12, 16, 12);
                }
                else
                {
                    ContentPadding = new Thickness(16, 12, 10, 12);
                }
                Block text_EmojiBlock = new Block()
                {
                    WordWarp = true,
                    MaxWidth = this.myselfOnTheRight ? "80%" : "100%",
                    Margin = this.thicknessIsMe,
                    Padding = ContentPadding,                    
                };
                if (this.ShowBackground)
                {
                    text_EmojiBlock.Background = sudokuImageFill; //去掉聊天背景
                }
                this.SetTextEmojiBlock(text_EmojiBlock, this.GetTextEmojiList(content));
                block.Add(text_EmojiBlock);
                this.Document.Add(block);
            }
        }

        /// <summary>
        /// 获取聊天消息中的文字与表情的集合
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private List<IInputModel> GetTextEmojiList(ChatBoxContent2 content)
        {
            if (content == null || content.Text == null)
            {
                return null;
            }
            List<IInputModel> list = new List<IInputModel>();

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
                    list.Add(new TextModel(item.ToString()));
                }

                //this.AppendText(pureText);                

                //插入NonTextItem
                for (int i = 0; i < content.NonTextItemPositions.Count; i++)
                {
                    uint position = content.NonTextItemPositions[i];
                    NonTextItem item = content.NonTextItemDictionary[position];
                    if (item.ChatBoxElementType == ChatBoxElementType.InnerEmotion)
                    {
                        list.Insert((int)position, new EmojiModel((int)item.InnerEmotionIndex, GlobalResourceManager.EmotionDictionary[item.InnerEmotionIndex]));
                        //this.InsertDefaultEmotion(item.InnerEmotionIndex, (int)(count + position));
                    }
                    else if (item.ChatBoxElementType == ChatBoxElementType.ForeignImage)
                    {
                        Image image = CommonHelper.CropPicture(item.ForeignImage);
                        Image img = image != null ? image : item.ForeignImage;
                        list.Insert((int)position, new ImageModel(img));
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
                    list.Add(new TextModel(item.ToString()));
                }
            }
            return list;

        }

        /// <summary>
        /// 将文字与表情设置到Block 中去
        /// </summary>
        /// <param name="textEmojiBlock"></param>
        /// <param name="textEmojis"></param>
        private void SetTextEmojiBlock(Block textEmojiBlock, List<IInputModel> textEmojis)
        {
            if (textEmojiBlock == null || textEmojis == null) { return; }
            foreach (IInputModel item in textEmojis)
            {
                TextModel textModel = item as TextModel;
                if (textModel != null)
                {
                    textEmojiBlock.Add(
                        new DocumentChar(textModel.TextContent[0])
                        {
                            Top = 4,                            
                        }
                    );
                    continue;
                }
                EmojiModel emojiModel = item as EmojiModel;
                if (emojiModel != null)
                {
                    textEmojiBlock.Add(
                        new InlineUIContainer
                        {
                            UIElement = new Picture
                            {
                                MarginTop = 4,
                                MarginRight = 2,
                                MaxWidth = 24,
                                MaxHeight = 24,
                                IsAntiAlias = true,
                                Stretch = Stretch.UniformToFill,
                                StretchDirection = StretchDirection.DownOnly,
                                MarginBottom = 0,
                                MarginLeft = 4,
                                Source = emojiModel.Source,
                                Tag = emojiModel.EmojiIndex
                            }
                        });
                    continue;
                }
                ImageModel imageModel = item as ImageModel;
                if (imageModel != null)
                {
                    textEmojiBlock.Add(
                        new InlineUIContainer
                        {
                            UIElement = new Picture
                            {
                                MarginTop = 4,
                                MarginRight = 2,
                                MaxWidth = 400,
                                MaxHeight = 400,
                                IsAntiAlias = true,
                                Stretch = Stretch.Uniform,
                                StretchDirection = StretchDirection.DownOnly,
                                MarginBottom = 0,
                                MarginLeft = 4,
                                Source = imageModel.Source,
                                Commands = {  
                                     {
                                    nameof(MouseUp),
                                    (s, e) => {

                                        CPF.Input.MouseButtonEventArgs args = e as CPF.Input.MouseButtonEventArgs;
                                        if(args.MouseButton==CPF.Input.MouseButton.Right)
                                            {
                                                this.currentSelectedImage=imageModel.Source;
                                                this.imgContextMenu.PlacementTarget = (UIElement)s;
                                                this.imgContextMenu.IsOpen = true;
                                            }
                                    }
                                }
                            }
                            }
                        }); 
                    continue;
                }

            }
        } 

        internal GGInputContent GetInputContent()
        {
            Dictionary<int, int> emotionDictionary = new Dictionary<int, int>();
            Dictionary<int, Image> imageDictionary = new Dictionary<int, Image>();
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < this.Document.Children.Count; i++)
            {
                IDocumentElement item = this.Document.Children[i];
                CPF.Documents.DocumentChar text = item as CPF.Documents.DocumentChar;
                CPF.Documents.InlineUIContainer image = item as CPF.Documents.InlineUIContainer;
                
                if (text != null)
                {
                    sb.Append(text.Char);
                }
                if (image != null)
                {
                    sb.Append(" ");
                    Picture picture = image.UIElement as Picture;
                    if (picture.Tag == null)
                    {
                        imageDictionary.Add(i,FileHelper.GetBitmap( picture.Source.ToString()));
                    }
                    else {
                        emotionDictionary.Add(i, (int)picture.Tag);
                    }                     
                }

            }
            return new GGInputContent() { EmotionDictionary = emotionDictionary, ImageDictionary = imageDictionary, Text = sb.ToString() };
        }

        private async void SaveImage(Image image)
        {
            if (image == null) { return; }

            System.Threading.Tasks.Task<string> task= FileHelper.SaveFileDialog((Window)this.Root, "另存为", DateTime.Now.Ticks + ".png");
            await task.ConfigureAwait(false);
            if (string.IsNullOrEmpty(task.Result)) { return; }
            image.Save(task.Result);
        }


        private async void ShowImage(Image image)
        {
            if (image == null) { return; }  
            PictureWindow pictureWindow = new PictureWindow(image);
            pictureWindow.Show_Topmost();
        }


        public void Past4TextBox(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str))
                {
                    return;
                }
                (int, int) value = this.HaveSelectContent();
                int startIndex = value.Item1;
                int length = value.Item2;
                if (length != 0)
                {
                    this.Document.Children.RemoveRange(startIndex, length);
                }
                Dictionary<int, int> emotionDictionary = new Dictionary<int, int>();
                if (str.Contains('[') && str.Contains(']'))
                {
                    for (int i = 0; i < str.Length; i++)
                    {
                        if (str[i] == '[' && i + 4 < str.Length)
                        {
                            if (str[i + 4] == ']')
                            {
                                int emotionIndex = -1;
                                int.TryParse(str.Substring(i + 1, 3), out emotionIndex);
                                if (emotionIndex > -1)//中间是表情
                                {
                                    emotionDictionary.Add(i, emotionIndex);
                                    str = str.Remove(i, 5);
                                    str = str.Insert(i, " ");
                                }
                            }
                        }
                    }
                }
                for (int i = str.Length - 1; i >= 0; i--)
                {
                    if (str[i] == ' ' && emotionDictionary.ContainsKey(i))
                    {
                        this.AppentEmoji(emotionDictionary[i]);
                        continue;
                    }
                    this.AppentChar(str[i]);
                }
                this.ScrollToEnd();
            }
            catch (Exception ex)
            {
                GlobalResourceManager.Logger.Log(ex, "ChatBox.Past4TextBox", ESBasic.Loggers.ErrorLevel.Standard);
            }
        }
        public InlineUIContainer CreateEmotionInlineUIContainer(int innerEmotionIndex)
        {
            EmojiModel emojiModel = new EmojiModel(innerEmotionIndex, GlobalResourceManager.EmotionDictionary[(uint)innerEmotionIndex]);
            string index = (innerEmotionIndex + 1).ToString();
            if (innerEmotionIndex < 9)
            {
                index = "0" + index;
            }
            string imgPath = $"{emojiPath}{index}.png";
            Picture picture = new Picture
            {
                MarginTop = 4,
                MarginRight = 2,
                MaxWidth = 24,
                MaxHeight = 24,
                IsAntiAlias = true,
                Stretch = Stretch.Uniform,
                StretchDirection = StretchDirection.DownOnly,
                MarginBottom = 0,
                MarginLeft = 4,
                Source = imgPath,
                Tag = emojiModel.EmojiIndex
            };
            return new InlineUIContainer
            {
                UIElement = picture
            };
        }

        public void AppentEmoji(int innerEmotionIndex)
        {
            (int, int) value = this.HaveSelectContent();
            int startIndex = value.Item1;
            this.Document.Children.Insert(startIndex, this.CreateEmotionInlineUIContainer(innerEmotionIndex));
        }

        public void AppentChar(char c)
        {
            (int, int) value = this.HaveSelectContent();
            int startIndex = value.Item1;
            this.Document.Children.Insert(startIndex, new DocumentChar(c));
        }

    }


    internal interface IInputModel
    {
        bool IsText { get; }
    }

    internal class TextModel : IInputModel
    {
        public bool IsText { get { return true; } }

        public TextModel(string s)
        {
            this.TextContent = s;
        }
        public string TextContent { get; set; }
    }

    internal class EmojiModel : IInputModel
    {
        public EmojiModel(int index, Image image)
        {
            this.EmojiIndex = index;
            this.Source = image;
        }
        public Image Source { get; set; }
        public int EmojiIndex { get; set; }
        public bool IsText { get { return false; } }
    }

    internal class ImageModel : IInputModel
    {
        public ImageModel(Image image)
        {
            this.Source = image;
        }
        public Image Source { get; set; }
        public bool IsText { get { return false; } }
    }

    internal class GGInputContent
    {
        /// <summary>
        /// 发送的表情集合  key: postion  value:emotionID
        /// </summary>
        public Dictionary<int, int> EmotionDictionary = new Dictionary<int, int>();
        public Dictionary<int, Image> ImageDictionary = new Dictionary<int, Image>();
        public string Text = "";
    }
}
