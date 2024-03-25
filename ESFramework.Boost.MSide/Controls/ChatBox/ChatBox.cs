using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ESFramework.Boost.Controls.Internals;
using System.IO;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing.Imaging;
using ESBasic.Helpers;
using ESBasic;

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
        SnapchatMessage=500000
    }


    /// <summary>
    /// 支持图片和动画的RichTextBox。
    /// </summary>
    public class ChatBox : RichTextBox
    {
        private int itemIndex = 0;
        private ESBasic.Collections.SortedArray<int, object> itemArray = new ESBasic.Collections.SortedArray<int, object>();
        private Dictionary<uint, Image> defaultEmotionDictionary = new Dictionary<uint, Image>();
        private ContextMenuStrip contextMenuStrip1;
        private IContainer components;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ContextMenuStrip contextMenuStrip2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem5; //表情图片在内置列表中的index - emotion
        private Image imageOnRightClick = null;
        private ToolStripMenuItem toolStripMenuItem6;
        private ContextMenuStrip contextMenuStrip3;
        private ToolStripMenuItem toolStripMenuItem7;

        /// <summary>
        /// 当文件（夹）拖放到控件内时，触发此事件。参数：文件路径的集合。
        /// </summary>
        [Description("当文件（夹）拖放到控件内时，触发此事件。")]
        public event ESBasic.CbGeneric<string[]> FileOrFolderDragDrop;

        #region ContextMenuMode
        private ChatBoxContextMenuMode contextMenuMode = ChatBoxContextMenuMode.None;
        private ContextMenuStrip contextMenuStrip4;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem toolStripMenuItem9;
        private PictureBox pictureBox1;
        /// <summary>
        /// 快捷菜单的模式。
        /// </summary>
        [Description("快捷菜单的模式。")]
        public ChatBoxContextMenuMode ContextMenuMode
        {
            get { return contextMenuMode; }
            set
            {
                contextMenuMode = value;
                //if (this.contextMenuMode == ChatBoxContextMenuMode.ForOutput)
                //{
                //    this.Cursor = Cursors.Arrow;                    
                //}

                //if (this.contextMenuMode == ChatBoxContextMenuMode.ForInput)
                //{
                //    this.Cursor = Cursors.IBeam;
                //}
            }
        }
        #endregion

        #region PopoutImageWhenDoubleClick
        private bool popoutImageWhenDoubleClick = false;
        /// <summary>
        /// 双击图片时，是否弹出图片。
        /// </summary>
        public bool PopoutImageWhenDoubleClick
        {
            get { return popoutImageWhenDoubleClick; }
            set { popoutImageWhenDoubleClick = value; }
        }
        #endregion

        #region UseRtf
        private bool useRtf = false;
        /// <summary>
        /// 是否使用Rtf？（2018.05.31 为了支持“@某人”增加该属性，如果使用Rtf，ChatBoxContent中就不需要Font和Color了，但是ChatBoxContent的size就会变得大很多）
        /// </summary>
        public bool UseRtf
        {
            get { return useRtf; }
            set { useRtf = value; }
        }
        #endregion


        private const int WM_SETFOCUS = 0x7;       
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;  

        protected override void WndProc(ref Message m)
        {
            //if (this.ReadOnly)
            //{
            //    if (m.Msg == WM_SETFOCUS || m.Msg == WM_KEYDOWN || m.Msg == WM_KEYUP)
            //    {
            //        return;
            //    }
            //}
            base.WndProc(ref m);
        }    


        public ChatBox()
        {
            this.InitializeComponent();
            this.Cursor = Cursors.IBeam;
            this.ShowSelectionMargin = false;

            this.AllowDrop = false;

            this.DragDrop += new DragEventHandler(textBoxSend_DragDrop);
            this.DragEnter += new DragEventHandler(textBoxSend_DragEnter);
            this.KeyDown += new KeyEventHandler(ChatBox_KeyDown);
            this.MouseDown += new MouseEventHandler(ChatBox_MouseDown);

            this.SizeChanged += new EventHandler(ChatBox_SizeChanged);
            this.DoubleClick += new EventHandler(ChatBox_DoubleClick);
            this.LinkClicked += new LinkClickedEventHandler(ChatBox_LinkClicked);
            this.VScroll += new EventHandler(ChatBox_VScroll);           
        }

        void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            
        }

        //protected override void WndProc(ref Message m)
        //{
        //    if (this.ContextMenuMode == ChatBoxContextMenuMode.ForOutput)
        //    {
        //        if (m.Msg == 0x0302)            //0x0302是粘贴消息
        //        {
        //            m.Result = IntPtr.Zero;    
        //            return;
        //        }
        //    }
        //    base.WndProc(ref m);           
        //}

        void ChatBox_VScroll(object sender, EventArgs e)
        {

        }

        private GifBox HitTest4GifBox(Point pt, bool selectTarget)
        {
            int index = this.GetCharIndexFromPosition(pt);
            Point origin = this.GetPositionFromCharIndex(index);
            GifBox box = null;
            bool backOne = false;
            List<REOBJECT> list = this.RichEditOle.GetAllREOBJECT();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].posistion == index || list[i].posistion + 1 == index)
                {
                    if (list[i].dwUser == (int)ChatBoxElementType.ForeignImage)
                    {
                        box = (GifBox)Marshal.GetObjectForIUnknown(list[i].poleobj);
                        if (list[i].posistion + 1 == index)
                        {
                            origin = new Point(origin.X - box.Width, origin.Y);
                            backOne = true;
                        }
                        break;
                    }
                }
            }

            if (box == null)
            {
                return null;
            }

            Rectangle rect = new Rectangle(origin.X, origin.Y, box.Width, box.Height);
            if (!rect.Contains(pt))
            {
                return null;
            }

            if (selectTarget)
            {
                this.Select(backOne ? index - 1 : index, 1);
            }

            return box;
        }

        private LinkLabel HitTest4LinkLable(Point pt, bool selectTarget)
        {
            int index = this.GetCharIndexFromPosition(pt);
            Point origin = this.GetPositionFromCharIndex(index);
            LinkLabel linkLabel = null;
            bool backOne = false;
            List<REOBJECT> list = this.RichEditOle.GetAllREOBJECT();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].posistion == index || list[i].posistion + 1 == index)
                {
                    if (list[i].dwUser == (int)ChatBoxElementType.FileLink || list[i].dwUser == (int)ChatBoxElementType.LinkText)
                    {
                        linkLabel = (LinkLabel)Marshal.GetObjectForIUnknown(list[i].poleobj);
                        if (list[i].posistion + 1 == index)
                        {
                            origin = new Point(origin.X - linkLabel.Width, origin.Y);
                            backOne = true;
                        }
                        break;
                    }
                }
            }

            if (linkLabel == null)
            {
                return null;
            }

            Rectangle rect = new Rectangle(origin.X, origin.Y, linkLabel.Width, linkLabel.Height);
            if (!rect.Contains(pt))
            {
                return null;
            }

            if (selectTarget)
            {
                this.Select(backOne ? index - 1 : index, 1);
            }

            return linkLabel;
        }

        private AudioMessageBox HitTest4AudioMessageToken(Point pt)
        {
            int index = this.GetCharIndexFromPosition(pt);
            Point origin = this.GetPositionFromCharIndex(index);
            AudioMessageBox box = null;
            List<REOBJECT> list = this.RichEditOle.GetAllREOBJECT();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].posistion == index || list[i].posistion + 1 == index)
                {
                    if (list[i].dwUser == (int)ChatBoxElementType.AudioMessage)
                    {
                        box = (AudioMessageBox)Marshal.GetObjectForIUnknown(list[i].poleobj);
                        if (list[i].posistion + 1 == index)
                        {
                            origin = new Point(origin.X - box.Width, origin.Y);
                        }
                        break;
                    }
                }
            }

            if (box == null)
            {
                return null;
            }

            Rectangle rect = new Rectangle(origin.X, origin.Y, box.Width, box.Height);
            if (!rect.Contains(pt))
            {
                return null;
            }

            return box;
        }

        private SnapchatMessageBox HitTest4SnapchatMessage(Point pt)
        {
            int index = this.GetCharIndexFromPosition(pt);
            Point origin = this.GetPositionFromCharIndex(index);
            SnapchatMessageBox box = null;
            List<REOBJECT> list = this.RichEditOle.GetAllREOBJECT();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].posistion == index || list[i].posistion + 1 == index)
                {
                    if (list[i].dwUser == (int)ChatBoxElementType.SnapchatMessage)
                    {
                        box = (SnapchatMessageBox)Marshal.GetObjectForIUnknown(list[i].poleobj);
                        if (list[i].posistion + 1 == index)
                        {
                            origin = new Point(origin.X - box.Width, origin.Y);
                        }
                        break;
                    }
                }
            }

            if (box == null)
            {
                return null;
            }

            Rectangle rect = new Rectangle(origin.X, origin.Y, box.Width, box.Height);
            if (!rect.Contains(pt))
            {
                return null;
            }

            return box;
        }

        void ChatBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                AudioMessageBox audioMessageBox = this.HitTest4AudioMessageToken(e.Location);
                if (audioMessageBox != null)
                {
                    if (this.AudioMessageClicked != null)
                    {
                        this.AudioMessageClicked(audioMessageBox.AudioMessageID, audioMessageBox.AudioMessage);
                    }
                    return;
                }

                SnapchatMessageBox snapchatMessageBox = this.HitTest4SnapchatMessage(e.Location);
                if (snapchatMessageBox != null)
                {
                    if (this.SnapchatMessageClicked != null)
                    {
                        this.SnapchatMessageClicked(snapchatMessageBox);
                    }
                    return;
                }

                LinkLabel linkLabel = this.HitTest4LinkLable(e.Location, false);
                if (linkLabel == null)
                {
                    return;
                }

                LinkText linkText = linkLabel.Tag as LinkText;
                if (linkText != null)
                {
                    System.Diagnostics.Process.Start(linkText.Url);
                }
                else
                {
                    try
                    {
                        System.Diagnostics.Process.Start((string)linkLabel.Tag);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("无法打开文件：" + ee.Message);
                    }
                }
            }

            if (e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                return;
            }

            if (this.contextMenuMode == ChatBoxContextMenuMode.None)
            {
                this.ContextMenuStrip = null;
                return;
            }

            if (this.contextMenuMode == ChatBoxContextMenuMode.ForInput)
            {
                this.toolStripMenuItem9.Visible = !string.IsNullOrEmpty(this.SelectedText);
                this.ContextMenuStrip = this.contextMenuStrip1;
                return;
            }

            GifBox box = this.HitTest4GifBox(e.Location, true);
            if (box == null)
            {
                if (!string.IsNullOrEmpty(this.SelectedText))
                {
                    this.imageOnRightClick = null;
                    this.ContextMenuStrip = this.contextMenuStrip4;
                    return;
                }
                this.imageOnRightClick = null;
                this.ContextMenuStrip = this.contextMenuStrip3;
                return;
            }
            this.imageOnRightClick = box.Image;
            this.ContextMenuStrip = this.contextMenuStrip2;
        }

        void ChatBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        #region ChatBox_KeyDown
        void ChatBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                if (this.ContextMenuMode == ChatBoxContextMenuMode.ForOutput)
                {
                    return;
                }

                if (Clipboard.ContainsImage())
                {
                    Image img = Clipboard.GetImage();
                    if (img != null)
                    {
                        this.InsertImage(img);
                    }
                    e.Handled = true;
                    return;
                }
            }
        }
        #endregion

        #region ChatBox_SizeChanged
        void ChatBox_SizeChanged(object sender, EventArgs e)
        {
            if (this.RichEditOle == null)
            {
                return;
            }

            List<REOBJECT> list = this.RichEditOle.GetAllREOBJECT();
            for (int i = 0; i < list.Count; i++)
            {
                GifBox box = Marshal.GetObjectForIUnknown(list[i].poleobj) as GifBox;
                if (box != null)
                {
                    box.Size = this.ComputeGifBoxSize(box.Image.Size);
                }
            }
        }
        #endregion

        #region ChatBox_DoubleClick
        void ChatBox_DoubleClick(object sender, EventArgs arg)
        {
            try
            {
                if (!this.popoutImageWhenDoubleClick)
                {
                    return;
                }

                MouseEventArgs e = arg as MouseEventArgs;
                if (e == null)
                {
                    return;
                }

                GifBox box = this.HitTest4GifBox(e.Location, true);
                if (box == null)
                {
                    return;
                }

                ImageForm form = new ImageForm(box.Image);
                form.Show();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private string GetPathToSave(string title, string defaultName, string iniDir)
        {
            string extendName = Path.GetExtension(defaultName);
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = string.Format("The Files (*{0})|*{0}", extendName);
            saveDlg.FileName = defaultName;
            saveDlg.InitialDirectory = iniDir;
            saveDlg.OverwritePrompt = false;
            if (title != null)
            {
                saveDlg.Title = title;
            }

            DialogResult res = saveDlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                return saveDlg.FileName;
            }

            return null;
        }
        #endregion

        #region textBoxSend_DragEnter
        void textBoxSend_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        #endregion

        #region textBoxSend_DragDrop
        void textBoxSend_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileOrDirs = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (fileOrDirs == null || fileOrDirs.Length == 0)
                {
                    return;
                }

                if (this.FileOrFolderDragDrop != null)
                {
                    this.FileOrFolderDragDrop(fileOrDirs);
                }
            }
        }
        #endregion

        #region RichEditOle
        private RichEditOle richEditOle;
        private RichEditOle RichEditOle
        {
            get
            {
                if (richEditOle == null)
                {
                    if (base.IsHandleCreated)
                    {
                        richEditOle = new RichEditOle(this);
                    }
                }

                return richEditOle;
            }
        }
        #endregion

        #region Initialize
        public void Initialize(Dictionary<uint, Image> defaultEmotions)
        {
            this.defaultEmotionDictionary = defaultEmotions ?? new Dictionary<uint, Image>();
        }
        #endregion

        #region InsertImage 、InsertDefaultEmotion
        public void InsertDefaultEmotion(uint emotionID)
        {
            this.InsertDefaultEmotion(emotionID, this.TextLength);
        }

        /// <summary>
        /// 在position位置处，插入系统内置表情。
        /// </summary>      
        /// <param name="position">插入的位置</param>
        /// <param name="emotionID">表情图片在内置列表中的index</param>
        public void InsertDefaultEmotion(uint emotionID, int position)
        {
            Image image = this.pictureBox1.ErrorImage;
            if (this.defaultEmotionDictionary.ContainsKey(emotionID))
            {
                image = this.defaultEmotionDictionary[emotionID];
            }
            GifBox gif = new GifBox();
            gif.Cursor = Cursors.IBeam;
            gif.BackColor = base.BackColor;
            gif.Size = this.ComputeGifBoxSize(image.Size);
            gif.Image = image;
            gif.Tag = emotionID;
            this.RichEditOle.InsertControl(gif, position, (int)ChatBoxElementType.InnerEmotion);
        }

        public void InsertImage(Image image)
        {
            this.InsertImage(image, this.TextLength);
        }

        /// <summary>
        /// 在position位置处，插入图片。
        /// </summary>   
        /// <param name="image">要插入的图片</param>
        /// <param name="position">插入的位置</param>       
        public void InsertImage(Image image, int position)
        {
            GifBox gif = new GifBox();
            gif.Cursor = Cursors.IBeam;
            gif.BackColor = base.BackColor;
            gif.Size = this.ComputeGifBoxSize(image.Size);
            gif.Image = image;
            this.RichEditOle.InsertControl(gif, position, (int)ChatBoxElementType.ForeignImage);
        }

        private int maxLength = 300;//图片最大长度 

        private Size ComputeGifBoxSize(Size imgSize)
        {
            if (maxLength <= 0)
            {
                return new Size(0, 0);
            }
            int width = imgSize.Width, height = imgSize.Height;
            if (imgSize.Width > maxLength && imgSize.Height > maxLength)
            {
                if (imgSize.Width > imgSize.Height)
                {
                    width = maxLength;
                    height = (int)(imgSize.Height * width / imgSize.Width);
                }
                else
                {
                    height = maxLength;
                    width = (int)(imgSize.Width * height / imgSize.Height);
                }
            }
            else if (imgSize.Width > maxLength)
            {
                width = maxLength;
                height = (int)(imgSize.Height * width / imgSize.Width);
            }
            else if (imgSize.Height > maxLength)
            {
                height = maxLength;
                width = (int)(imgSize.Width * height / imgSize.Height);
            }
            return new Size(width, height);


            //int maxWidth = 400;
            //if (this.Width-20 < maxWidth)
            //{
            //    maxWidth = this.Width - 20;
            //}

            //if (imgSize.Width <= maxWidth)
            //{
            //    return imgSize;
            //}

            //int newImgHeight = maxWidth * imgSize.Height / imgSize.Width; ;
            //return new Size(maxWidth, newImgHeight);
        }
        #endregion

        //2019.10.09
        public void RemoveItem(int itemIndex)
        {
            if(!this.itemArray.ContainsKey(itemIndex))
            {
                return;
            }

            this.itemArray.RemoveByKey(itemIndex);
            this.Clear();
            for (int i = 0; i < this.itemArray.Count; i++)
            {
                KeyValuePair<int, object> pair = this.itemArray.GetAt(i);
                if (pair.Value.GetType() == typeof(ChatBoxContent))
                {
                    ChatBoxContent obj = (ChatBoxContent)pair.Value;
                    this.DoAppendChatBoxContent(obj);
                }
                if (pair.Value.GetType() == typeof(RichTextBag))
                {
                    RichTextBag obj = (RichTextBag)pair.Value;
                    this.DoAppendRichText(obj);
                }

                if (pair.Value.GetType() == typeof(AudioMessageBox))
                {
                    AudioMessageBox obj = (AudioMessageBox)pair.Value;
                    this.DoInsertAudioMessageBox(obj);
                }

                if (pair.Value.GetType() == typeof(SnapchatMessageBox))
                {
                    SnapchatMessageBox obj = (SnapchatMessageBox)pair.Value;
                    this.DoInsertSnapchatMessageBox(obj);
                }

                
            }

            this.Select(this.Text.Length, 0);
            this.ScrollToCaret();
        }

        #region AppendRtf
        private void AppendRtf(string _rtf)
        {
            base.Select(this.TextLength, 0);
            base.SelectedRtf = _rtf;
            base.Update();
            base.Select(this.Rtf.Length, 0);
        }
        #endregion

        #region AppendRichText
        /// <summary>
        /// 在现有内容后面追加富文本。
        /// </summary>              
        public int AppendRichText(string textContent, Font font, Color color)
        {            
            RichTextBag bag = new RichTextBag(textContent , font , color);    
            this.DoAppendRichText(bag);

            ++this.itemIndex;
            this.itemArray.Add(this.itemIndex, bag);
            return this.itemIndex;
        }

        private void DoAppendRichText(RichTextBag bag)
        {
            int count = this.Text.Length;
            this.AppendText(bag.TextContent);

            this.Select(count, bag.TextContent.Length);
            if (bag.Color != null)
            {
                this.SelectionColor = bag.Color;
            }
            if (bag.Font != null)
            {
                this.SelectionFont = bag.Font;
            }
            this.AppendText("\n");
        }
        #endregion

        #region GetContent
        /// <summary>
        /// 获取Box中的所有内容。
        /// </summary>        
        /// <param name="containsForeignObject">内容中是否包含不是由IImagePathGetter管理的图片对象</param>
        /// <returns>key为位置，val为图片的ID</returns>
        public ChatBoxContent GetContent()
        {
            string text = this.useRtf ? base.Rtf : base.Text;
            ChatBoxContent content = new ChatBoxContent(text, this.Font, this.ForeColor);
            List<REOBJECT> list = this.RichEditOle.GetAllREOBJECT();
            for (int i = 0; i < list.Count; i++)
            {
                uint pos = (uint)list[i].posistion;
                if (list[i].dwUser == (int)ChatBoxElementType.ForeignImage)
                {
                    GifBox box = (GifBox)Marshal.GetObjectForIUnknown(list[i].poleobj);
                    content.AddForeignImage(pos, box.Image);

                }
                else if (list[i].dwUser == (int)ChatBoxElementType.FileLink)
                {
                    LinkLabel linkLabel = (LinkLabel)Marshal.GetObjectForIUnknown(list[i].poleobj);
                    content.AddLinkFile(pos, (string)linkLabel.Tag);
                }
                else if (list[i].dwUser == (int)ChatBoxElementType.LinkText)
                {
                    LinkLabel linkLabel = (LinkLabel)Marshal.GetObjectForIUnknown(list[i].poleobj);
                    content.AddLinkText(pos, (LinkText)linkLabel.Tag);

                }
                else if (list[i].dwUser == (int)ChatBoxElementType.InnerEmotion)
                {
                    GifBox box = Marshal.GetObjectForIUnknown(list[i].poleobj) as GifBox;
                    if (box != null)
                    {
                        content.AddEmotion(pos, (uint)box.Tag);
                    }
                    else
                    {
                        
                        string[] files = Clipboard.GetData(DataFormats.FileDrop) as String[];
                        if (files != null && files.Length > 0)
                        {
                            this.Clear();
                            if (this.FileOrFolderDragDrop != null)
                            {
                                this.FileOrFolderDragDrop(files);
                            }
                        }
                    }
                }
                else
                {
                }
            }

            return content;
        }

        #endregion

        #region AppendChatBoxContent
        public int AppendChatBoxContent(ChatBoxContent content)
        {
            this.DoAppendChatBoxContent(content);

            ++this.itemIndex;
            this.itemArray.Add(this.itemIndex, content);
            return this.itemIndex;
        }

        private void DoAppendChatBoxContent(ChatBoxContent content)
        {
            if (content == null || content.Text == null)
            {
                return ;
            }

            int count = this.Text.Length;
            if (content.NonTextItemDictionary != null && content.NonTextItemDictionary.Count > 0)
            {
                string pureText = content.Text;
                if (!this.useRtf)
                {
                    //去掉表情和图片的占位符   
                    for (int i = content.NonTextItemPositions.Count - 1; i >= 0; i--)
                    {
                        pureText = pureText.Remove((int)content.NonTextItemPositions[i], 1);
                    }
                    this.AppendText(pureText);
                }
                else
                {
                    this.AppendRtf(pureText);
                }

                //插入NonTextItem
                for (int i = 0; i < content.NonTextItemPositions.Count; i++)
                {
                    uint position = content.NonTextItemPositions[i];
                    NonTextItem item = content.NonTextItemDictionary[position];
                    if (item.ChatBoxElementType == ChatBoxElementType.InnerEmotion)
                    {
                        this.InsertDefaultEmotion(item.InnerEmotionIndex, (int)(count + position));
                    }
                    else if (item.ChatBoxElementType == ChatBoxElementType.ForeignImage)
                    {
                        this.InsertImage(item.ForeignImage, (int)(count + position));
                    }
                    else if (item.ChatBoxElementType == ChatBoxElementType.FileLink)
                    {
                        this.InsertFileLink(item.FilePath, (int)(count + position));
                    }
                    else //LinkText
                    {
                        this.InsertLinkText(item.LinkText, (int)(count + position));
                    }
                }
            }
            else
            {
                if (this.useRtf)
                {
                    this.AppendRtf(content.Text);
                }
                else
                {
                    this.AppendText(content.Text);
                }
            }

            if (!this.useRtf)
            {
                this.Select(count, content.Text.Length);
                if (content.Color != null)
                {
                    this.SelectionColor = content.Color;
                }
                if (content.Font != null)
                {
                    this.SelectionFont = content.Font;
                }
            }
            this.AppendText("\n");
        }
        #endregion

        #region AudioMessage
        /// <summary>
        /// 当某个语音消息被单击时，触发此事件。 参数：AudioMessageID - AudioMessage
        /// </summary>
        public event CbGeneric<string, object> AudioMessageClicked;

        private Dictionary<string, AudioMessageBox> audioMessageBoxDic = new Dictionary<string, AudioMessageBox>();       
        public int InsertAudioMessageBox(string msgID, int spanInSecs, object audioMsg)
        {
            AudioMessageBox audioMessageBox = new AudioMessageBox();
            audioMessageBox.Cursor = Cursors.IBeam;
            audioMessageBox.Initialize(msgID, audioMsg, spanInSecs);
            audioMessageBox.Tag = audioMsg;
            this.audioMessageBoxDic.Add(msgID, audioMessageBox);
            this.DoInsertAudioMessageBox(audioMessageBox);

            ++this.itemIndex;
            this.itemArray.Add(this.itemIndex, audioMessageBox);
            return this.itemIndex;
        }

        public void DoInsertAudioMessageBox(AudioMessageBox audioMessageBox)
        {
            this.RichEditOle.InsertControl(audioMessageBox, this.TextLength, (int)ChatBoxElementType.AudioMessage);
            this.AppendText("\n");
        }

        /// <summary>
        /// 开始语音消息播放的动画。
        /// </summary>       
        public void StartPlayAudioMessageAnimation(string msgID)
        {
            if (!this.audioMessageBoxDic.ContainsKey(msgID))
            {
                return;
            }

            this.audioMessageBoxDic[msgID].Start();
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

            this.audioMessageBoxDic[msgID].Stop();
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

        #region SnapchatMessage
        /// <summary>
        /// 当某个阅后自焚消息被单击时，触发此事件。 参数：SnapchatMessageID - SnapchatMessage
        /// </summary>
        public event CbGeneric<SnapchatMessageBox> SnapchatMessageClicked;

        private Dictionary<string, SnapchatMessageBox> snapchatMessageBoxDic = new Dictionary<string, SnapchatMessageBox>();

        public int InsertSnapchatMessageBox(SnapchatMessageBox snapchatMessageBox)
        {
            this.snapchatMessageBoxDic.Add(snapchatMessageBox.MessageID, snapchatMessageBox);
            this.DoInsertSnapchatMessageBox(snapchatMessageBox);

            ++this.itemIndex;
            this.itemArray.Add(this.itemIndex, snapchatMessageBox);
            return this.itemIndex;
        }

        public void DoInsertSnapchatMessageBox(SnapchatMessageBox snapchatMessageBox)
        {           
            this.RichEditOle.InsertControl(snapchatMessageBox, this.TextLength, (int)ChatBoxElementType.SnapchatMessage);
            this.AppendText("\n");
        }

        public void RemoveSnapchatMessageBox(string messageID)
        {
            if (!this.snapchatMessageBoxDic.ContainsKey(messageID))
            {
                return;
            }
            SnapchatMessageBox box = this.snapchatMessageBoxDic[messageID];

            for (int i = box.StartIndex; i <= box.EndIndex; i++)
            {
                this.RemoveItem(i);
            }           
            this.snapchatMessageBoxDic.Remove(messageID);
        }


        #endregion

        public void InsertLinkText(LinkText linkText)
        {
            this.InsertLinkText(linkText, this.TextLength);
        }

        public void InsertLinkText(LinkText linkText, int pos)
        {
            LinkLabel linkLabel = new LinkLabel();
            linkLabel.TextAlign = ContentAlignment.MiddleLeft;
            linkLabel.Cursor = Cursors.IBeam;
            linkLabel.BackColor = base.BackColor;
            linkLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            linkLabel.Text = linkText.Text;
            linkLabel.Tag = linkText;
            //linkLabel.TextAlign = ContentAlignment.MiddleLeft;
            //linkLabel.Size = new System.Drawing.Size(linkText.Text.Length*10, 30);
            //linkLabel.AutoSize = true;
            //linkLabel.Image = image;

            //linkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel_LinkClicked);
            //linkLabel.MouseDown += new MouseEventHandler(linkLabel_MouseDown);            
            this.RichEditOle.InsertControl(linkLabel, pos, (int)ChatBoxElementType.LinkText);
        }

        void linkLabel_MouseDown(object sender, MouseEventArgs e)
        {

        }

        void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        public void AppendFileLink(string prefixMessage, string filePath)
        {
            this.AppendText(prefixMessage);
            this.InsertFileLink(filePath, this.Text.Length);
        }

        private void InsertFileLink(string filePath, int pos)
        {
            LinkLabel linkLabel = new LinkLabel();
            linkLabel.TextAlign = ContentAlignment.MiddleLeft;
            linkLabel.Cursor = Cursors.IBeam;
            linkLabel.BackColor = base.BackColor;
            linkLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            linkLabel.Text = "打开";
            //linkLabel.Image = image;
            linkLabel.Tag = filePath;
            this.RichEditOle.InsertControl(linkLabel, pos, (int)ChatBoxElementType.FileLink);
        }

        #region Clear
        public new void Clear()
        {
            this.audioMessageBoxDic.Clear();
            List<REOBJECT> list = this.RichEditOle.GetAllREOBJECT();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].dwUser == (int)ChatBoxElementType.ForeignImage)
                {
                    GifBox box = (GifBox)Marshal.GetObjectForIUnknown(list[i].poleobj);
                    box.Dispose();
                }

                if (list[i].dwUser == (int)ChatBoxElementType.FileLink)
                {
                    LinkLabel box = (LinkLabel)Marshal.GetObjectForIUnknown(list[i].poleobj);
                    box.Dispose();
                }

                if (list[i].dwUser == (int)ChatBoxElementType.InnerEmotion)
                {
                    GifBox box = Marshal.GetObjectForIUnknown(list[i].poleobj) as GifBox;
                    if (box != null)
                    {
                        box.Dispose();
                    }
                }

                if (list[i].dwUser == (int)ChatBoxElementType.LinkText)
                {
                    LinkLabel box = (LinkLabel)Marshal.GetObjectForIUnknown(list[i].poleobj);
                    box.Dispose();
                }
            }
            base.Clear();
            //this.richEditOle.Clear();         
        }
        #endregion

        #region InitializeComponent
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip4 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip3.SuspendLayout();
            this.contextMenuStrip4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem9,
            this.toolStripMenuItem2,
            this.toolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(146, 70);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem9.Text = "复制";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.toolStripMenuItem9_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.toolStripMenuItem2.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem2.Text = "粘贴";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem1.Text = "插入图片";
            this.toolStripMenuItem1.Visible = false;
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(137, 92);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuItem3.Text = "复制";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuItem4.Text = "另存为";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuItem5.Text = "新窗口显示";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuItem6.Text = "清屏";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem7});
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.Size = new System.Drawing.Size(101, 26);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem7.Text = "清屏";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
            // 
            // contextMenuStrip4
            // 
            this.contextMenuStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem8});
            this.contextMenuStrip4.Name = "contextMenuStrip4";
            this.contextMenuStrip4.Size = new System.Drawing.Size(101, 26);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem8.Text = "复制";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.toolStripMenuItem8_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip3.ResumeLayout(false);
            this.contextMenuStrip4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.SelectedText);
        }

        void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.SelectedText);
        }

        void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            this.Clear();
        }

        void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            this.Clear();
        }

        void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            try
            {
                ImageForm form = new ImageForm(this.imageOnRightClick);
                form.Show();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        //保存图片
        void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            try
            {

                bool gif = ImageHelper.IsGif(this.imageOnRightClick);
                string postfix = gif ? "gif" : "jpg";

                string path = this.GetPathToSave("请选择保存路径", "image." + postfix, null);
                if (path == null)
                {
                    return;
                }
                ImageFormat format = gif ? ImageFormat.Gif : ImageFormat.Jpeg;

                ImageHelper.Save(this.imageOnRightClick, path, format);
                MessageBox.Show("成功保存图片。");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        //复制
        void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetImage(this.imageOnRightClick);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsImage())
                {
                    Image img = Clipboard.GetImage();
                    if (img != null)
                    {
                        this.InsertImage(img);
                    }
                    return;
                }

                //System.Windows.Forms.IDataObject obj = Clipboard.GetDataObject();

                this.Paste();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                string file = this.GetFileToOpen2("请选择图片", null, ".jpg", ".bmp", ".png", ".gif");
                if (file == null)
                {
                    return;
                }

                ulong size = ESBasic.Helpers.FileHelper.GetFileSize(file);
                if (size > 1024 * 1024)
                {
                    MessageBox.Show("插入的图片不能超过1M。");
                    return;
                }

                Image img = Image.FromFile(file);
                this.InsertImage(img);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private string GetFileToOpen2(string title, string iniDir, params string[] extendNames)
        {
            StringBuilder filterBuilder = new StringBuilder("(");
            for (int i = 0; i < extendNames.Length; i++)
            {
                filterBuilder.Append("*");
                filterBuilder.Append(extendNames[i]);
                if (i < extendNames.Length - 1)
                {
                    filterBuilder.Append(";");
                }
                else
                {
                    filterBuilder.Append(")");
                }
            }
            filterBuilder.Append("|");
            for (int i = 0; i < extendNames.Length; i++)
            {
                filterBuilder.Append("*");
                filterBuilder.Append(extendNames[i]);
                if (i < extendNames.Length - 1)
                {
                    filterBuilder.Append(";");
                }
            }

            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = filterBuilder.ToString();
            openDlg.FileName = "";
            openDlg.InitialDirectory = iniDir;
            if (title != null)
            {
                openDlg.Title = title;
            }

            openDlg.CheckFileExists = true;
            openDlg.CheckPathExists = true;

            DialogResult res = openDlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                return openDlg.FileName;
            }

            return null;
        }
        #endregion
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
        public ChatBoxContent(string _text, Font _font, Color c)
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
        private Font font;
        public Font Font
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
        public RichTextBag(string content, Font f,Color c)
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
