using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ESFramework.Boost.Controls.Internals;
using System.IO;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing.Imaging;
using ESBasic.Helpers;
using ESBasic;
using ESPlus.Serialization;

namespace ESFramework.Boost.Controls
{
    public enum ChatBoxContextMenuMode
    {
        None = 0,
        ForInput,
        ForOutput
    }

    public enum ChatBoxElementType
    {
        InnerEmotion = 0,

        /// <summary>
        /// 插入的图片。（如果小于100000，则为系统内置的表情图片，其值为表情图片的index）
        /// </summary>
        ForeignImage = 100000,

        /// <summary>
        /// 插入的为一个指向文件夹的链接
        /// </summary>
        FileLink = 200000,

        /// <summary>
        /// 文本链接
        /// </summary>
        LinkText = 300000,

        /// <summary>
        /// 语音消息
        /// </summary>
        AudioMessage = 400000,

        /// <summary>
        /// 阅后自焚消息
        /// </summary>
        SnapchatMessage = 500000
    }


    #region ChatBoxContent
    /// <summary>
    /// 封装完整的一个聊天消息，消息中可以包含表情和图片。    
    /// 注意：Text属性的值表示纯文本信息，而图片都变成了文本中的对应位置的占位符（空格）。
    /// </summary>
    [Serializable]
    public class ChatBoxContent
    {
        public ChatBoxContent() { }
        public ChatBoxContent(string _text, SimpleFont _font, Color c)
        {
            this.text = _text;
            this.font = _font;
            this.color = c;
        }

        #region TransferTime
        //private DateTime transferTime = DateTime.Now;
        ///// <summary>
        ///// 该聊天消息经服务器转发的时间。2019.07.08
        ///// </summary>
        //public DateTime TransferTime
        //{
        //    get { return transferTime; }
        //    set { transferTime = value; }
        //} 
        #endregion

        #region Text
        private string text = "";
        /// <summary>
        /// 纯文本信息，图片都变成了一个占位符（空格）。
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        #endregion

        #region Font
        private SimpleFont font;
        public SimpleFont Font
        {
            get { return font; }
            set { font = value; }
        }
        #endregion

        #region Color
        private Color color;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        #endregion

        #region NonTextItemDictionary
        private Dictionary<uint, NonTextItem> nonTextItemDictionary = new Dictionary<uint, NonTextItem>();
        /// <summary>
        /// 非内置的表情图片。key - 在ChatBox中的位置。
        /// </summary>
        public Dictionary<uint, NonTextItem> NonTextItemDictionary
        {
            get { return nonTextItemDictionary; }
            set { nonTextItemDictionary = value; }
        }

        #region NonTextItemPositions
        private List<uint> _nonTextItemPositions = null;
        /// <summary>
        /// 所有图片的位置。从小到大排列。
        /// </summary>
        public List<uint> NonTextItemPositions
        {
            get
            {
                if (this._nonTextItemPositions == null)
                {
                    this._nonTextItemPositions = new List<uint>();

                    if (this.nonTextItemDictionary != null)
                    {
                        foreach (uint key in this.nonTextItemDictionary.Keys)
                        {
                            this._nonTextItemPositions.Add(key);
                        }
                    }
                    this._nonTextItemPositions.Sort();
                }

                return _nonTextItemPositions;
            }
        }
        #endregion
        #endregion

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(this.text.Trim()) && (this.nonTextItemDictionary == null || this.nonTextItemDictionary.Count == 0);
        }

        public bool ContainsForeignImage()
        {
            return this.nonTextItemDictionary != null && this.nonTextItemDictionary.Count > 0;
        }

        public void AddForeignImage(uint pos, Image img)
        {
            this.nonTextItemDictionary.Add(pos, new NonTextItem(img));
        }

        public void AddEmotion(uint pos, uint emotionIndex)
        {
            this.nonTextItemDictionary.Add(pos, new NonTextItem(emotionIndex));
        }

        public void AddLinkFile(uint pos, string path)
        {
            this.nonTextItemDictionary.Add(pos, new NonTextItem(path));
        }

        public void AddLinkText(uint pos, LinkText linkText)
        {
            this.nonTextItemDictionary.Add(pos, new NonTextItem(linkText));
        }

        public string GetTextWithPicPlaceholder(string placeholder)
        {
            if (this.NonTextItemPositions == null || this.NonTextItemPositions.Count == 0)
            {
                return this.Text;
            }

            string tmp = this.Text;
            for (int i = this.NonTextItemPositions.Count - 1; i >= 0; i--)
            {
                tmp = tmp.Insert((int)this.NonTextItemPositions[i], placeholder);
            }
            return tmp;
        }

        /// <summary>
        /// 存在DB中聊天记录AutoID
        /// </summary>
        [ESPlus.Serialization.NotSerializedCompactly]
        public int RecordID { get; set; }
    }

    public class SimpleFont
    {
        public SimpleFont() { }

        public string FontFamily { get; set; }
        public float Size { get; set; }
        public int FontStyle { get; set; }
    }

    public class LinkText
    {
        #region Ctor
        public LinkText() { }
        public LinkText(string _text, string _url)
        {
            this.text = _text;
            this.url = _url;
        }
        #endregion

        #region Text
        private string text = "";
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        #endregion

        #region Url
        private string url = "";
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        #endregion
    }
    #endregion

    public class NonTextItem
    {
        #region Ctor
        public NonTextItem() { }
        public NonTextItem(LinkText text)
        {
            this.nonTextItemType = Controls.ChatBoxElementType.LinkText;
            this.linkText = text;
        }

        public NonTextItem(uint index)
        {
            this.nonTextItemType = Controls.ChatBoxElementType.InnerEmotion;
            this.innerEmotionIndex = index;
        }

        public NonTextItem(Image img)
        {
            this.nonTextItemType = Controls.ChatBoxElementType.ForeignImage;
            this.foreignImage = img;
        }

        public NonTextItem(string path)
        {
            this.nonTextItemType = Controls.ChatBoxElementType.FileLink;
            this.filePath = path;
        }
        #endregion

        #region ChatBoxElementType
        private ChatBoxElementType nonTextItemType = ChatBoxElementType.LinkText;
        public ChatBoxElementType ChatBoxElementType
        {
            get { return nonTextItemType; }
            set { nonTextItemType = value; }
        }
        #endregion

        #region LinkText
        private LinkText linkText = null;
        /// <summary>
        /// 当ChatBoxElementType为LinkText时使用。
        /// </summary>
        public LinkText LinkText
        {
            get { return linkText; }
            set { linkText = value; }
        }
        #endregion

        #region InnerEmotionIndex
        private uint innerEmotionIndex = 0;
        /// <summary>
        /// 当ChatBoxElementType为InnerEmotion时使用。
        /// </summary>
        public uint InnerEmotionIndex
        {
            get { return innerEmotionIndex; }
            set { innerEmotionIndex = value; }
        }
        #endregion

        #region ForeignImage
        private Image foreignImage = null;
        /// <summary>
        /// 当ChatBoxElementType为ForeignImage时使用。
        /// </summary>
        public Image ForeignImage
        {
            get { return foreignImage; }
            set { foreignImage = value; }
        }
        #endregion

        #region FilePath
        private string filePath = null;
        /// <summary>
        /// 当ChatBoxElementType为FileLink时使用。
        /// </summary>
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        #endregion

    }

    public class RichTextBag
    {
        public RichTextBag() { }
        public RichTextBag(string content, Font f, Color c)
        {
            this.TextContent = content;
            this.Font = f;
            this.Color = c;
        }

        public string TextContent;
        public Font Font;
        public Color Color;
    }
}
