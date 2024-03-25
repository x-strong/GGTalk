using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GGTalk.Controls
{
    public partial class ChatInPutBox : UserControl
    {
        private Brush brush1 = new SolidBrush(Color.Transparent);        
        private Brush cursorBrush = new SolidBrush(Color.Black);
        private int cursorIndex = 0;
        private List<IGGChatInputItem> inputItemListRebuild = new List<IGGChatInputItem>();

        private System.Threading.Timer timer;
        public bool drawCursor = true;
        private int maxWidth = 150;
        private int maxHeight = 0;
        private const int RightPadding = 4;
        private const int LeftPadding = 10;
        private const int RowHeight = 24;
        private const int ItemInterval = 1;
        private const int OffSetX = 10;
        private const int OffSetY = 10;
        private Point cursorPoint ;
        private bool deletetext;
        public ChatInPutBox()
        {
            this.InitializeComponent();
            this.timer = new System.Threading.Timer(this.CursorControl, null, 1000, 500);
        }

        private void Item_paste_Click1(object sender, EventArgs e)
        {
            this.WriteClipboardText();
        }

        protected override void OnPaint(PaintEventArgs e) 
        {
            base.OnPaint(e);

            int offsetX = ChatInPutBox.OffSetX;
            int offsetY = ChatInPutBox.OffSetY;           

            e.Graphics.FillRectangle(brush1, new RectangleF(new Point(0, 0), new Size(this.maxWidth, this.maxHeight)));  

            if (this.cursorIndex == 0)
            {
                //绘制光标
                if (this.drawCursor)
                {
                    cursorPoint = new Point(offsetX - 1, offsetY);
                    e.Graphics.FillRectangle(this.cursorBrush, new RectangleF(new Point(offsetX - 1, offsetY), new Size(1, 16)));
                }
            }

            //int autoEnterCount = 0;

            for (int i = 0; i < this.inputItemListRebuild.Count; i++)
            {

                IGGChatInputItem item = this.inputItemListRebuild[i];
                GGChatInputText textItem = item as GGChatInputText;
                bool isEnter = textItem != null && textItem.IsEnter;
                if (isEnter)
                {
                    if (this.cursorIndex == i && this.drawCursor)
                    {
                        cursorPoint = new Point(offsetX, offsetY);
                        e.Graphics.FillRectangle(this.cursorBrush, new RectangleF(new Point(offsetX, offsetY), new Size(1, 16)));
                    }
                    //未能理解？？？

                    offsetY += ChatInPutBox.RowHeight/2;
                    offsetX = ChatInPutBox.LeftPadding;
                }
                else
                {
                    //处理自动换行
                    if (offsetX + item.Width + ChatInPutBox.RightPadding >= this.maxWidth)
                    {
                        offsetY += ChatInPutBox.RowHeight;
                        offsetX = ChatInPutBox.LeftPadding;
                        //++autoEnterCount;
                    }
                    item.Draw(e.Graphics, new Point(offsetX, offsetY));
                    offsetX += item.Width + ItemInterval;
                }

                if (this.cursorIndex == i && !isEnter && this.cursorIndex != 0)
                {
                    //绘制光标
                    if (this.drawCursor)
                    {
                        cursorPoint = new Point(offsetX - item.Width - 1, offsetY);
                        e.Graphics.FillRectangle(this.cursorBrush, new RectangleF(new Point(offsetX - item.Width - 1, offsetY), new Size(1, 16)));
                    }
                }
            }

            if (this.drawCursor && this.cursorIndex == this.inputItemListRebuild.Count)
            {
                cursorPoint = new Point(offsetX - 1, offsetY);
                e.Graphics.FillRectangle(this.cursorBrush, new RectangleF(new Point(offsetX - 1, offsetY), new Size(1, 16)));
            }
        }

        public void MoveCursorUpDown(bool isUp)
        {
            if (isUp && this.cursorIndex == 0)
            {
                return;
            }

            int rowNumber = 0;
            int columnNumber = 0;
            int currentRowWidth = ChatInPutBox.LeftPadding;

            int cursorRowNumber = 0;
            int cursorColumnNumber = -1;


            //解析的时候，要将换行符Item放在当前Row的最后一个。
            Dictionary<int, List<int>> rowDictionary = new Dictionary<int, List<int>>(); // RowNumber - List<itemIndex>
            List<int> currentRowItemList = new List<int>();
            rowDictionary.Add(rowNumber, currentRowItemList);
            //this.inputItemList = this.inputItemList;

            for (int i = 0; i < this.inputItemListRebuild.Count; i++)
            {
                if (this.inputItemListRebuild[i].IsEnter)
                {
                    currentRowItemList.Add(i);

                    if (this.cursorIndex == i)
                    {
                        cursorRowNumber = rowNumber;
                        cursorColumnNumber = columnNumber;
                    }

                    ++rowNumber;
                    currentRowItemList = new List<int>();
                    rowDictionary.Add(rowNumber, currentRowItemList);
                    currentRowWidth = ChatInPutBox.LeftPadding;
                    columnNumber = -1;
                }
                else
                {
                    if (currentRowWidth + this.inputItemListRebuild[i].Width + ChatInPutBox.RightPadding >= this.maxWidth)
                    {
                        ++rowNumber;
                        currentRowItemList = new List<int>();
                        rowDictionary.Add(rowNumber, currentRowItemList);
                        currentRowItemList.Add(i);
                        currentRowWidth = ChatInPutBox.LeftPadding + this.inputItemListRebuild[i].Width;
                        columnNumber = 0;
                    }
                    else
                    {
                        currentRowWidth += this.inputItemListRebuild[i].Width + ItemInterval;
                        currentRowItemList.Add(i);
                    }
                }

                if (this.cursorIndex == i && !this.inputItemListRebuild[i].IsEnter)
                {
                    cursorRowNumber = rowNumber;
                    cursorColumnNumber = columnNumber;
                }

                columnNumber += 1;
            }

            if (this.cursorIndex == this.inputItemListRebuild.Count)
            {
                cursorRowNumber = rowNumber;
                cursorColumnNumber = columnNumber;
            }

            if (rowDictionary[rowDictionary.Count - 1].Count == 0)
            {
                if (cursorRowNumber == rowDictionary.Count - 1)
                {
                    cursorRowNumber -= 1;
                }
                rowDictionary.Remove(rowDictionary.Count - 1);
            }

            if (isUp && cursorRowNumber == 0)
            {
                return;
            }

            if ((!isUp) && cursorRowNumber == rowDictionary.Count - 1)
            {
                return;
            }

            if (isUp)
            {
                List<int> aboveRow = rowDictionary[cursorRowNumber - 1];   //上一行             
                if (cursorColumnNumber >= aboveRow.Count)
                {
                    this.cursorIndex = aboveRow[aboveRow.Count - 1] + 1;
                }
                else
                {
                    if (cursorColumnNumber == 0)
                    {
                        if (cursorRowNumber - 1 == 0)
                        {
                            this.cursorIndex = 0;
                        }
                        else
                        {
                            this.cursorIndex = aboveRow[0];
                        }
                    }
                    else
                    {
                        this.cursorIndex = aboveRow[cursorColumnNumber];
                    }
                }
            }
            else
            {
                List<int> underRow = rowDictionary[cursorRowNumber + 1];
                if (cursorColumnNumber > underRow.Count)
                {
                    this.cursorIndex = underRow[underRow.Count - 1] + 1;
                }
                else
                {
                    if (cursorColumnNumber == 0)
                    {
                        this.cursorIndex = underRow[0];
                    }
                    else
                    {
                        if (cursorColumnNumber >= underRow.Count)
                        {
                            this.cursorIndex = underRow[underRow.Count - 1];
                        }
                        else
                        {
                            this.cursorIndex = underRow[cursorColumnNumber];
                        }
                    }
                }
            }

            this.drawCursor = true;
            this.Invalidate();
        }


        public void Clear()
        {
            this.cursorIndex = 0;
            this.inputItemListRebuild.Clear();
            this.Invalidate();
        }

        private void AppendText(String _key_str)
        {
            string fontname = System.Drawing.SystemFonts.DefaultFont.Name;
            FontFamily fontFamily = new FontFamily(fontname);
            DrawTextAttribute drawTextAttribute = new DrawTextAttribute();
            drawTextAttribute.rectangleF = new RectangleF(new Point(0,0),new Size(200, 200));
            drawTextAttribute.font = new Font(fontname, 14, FontStyle.Regular, GraphicsUnit.Pixel);
            drawTextAttribute.Text = _key_str;
            drawTextAttribute.brush = new SolidBrush(Color.Black);
            GGChatInputText item = new GGChatInputText(drawTextAttribute, new SolidBrush(Color.Black));
            this.InsertInputItem(item);
        }

        public void AppendEmotion(int _emotionIndex, Bitmap bitmap)
        {
            this.InsertInputItem(new GGChatInPutEmotion(_emotionIndex, bitmap));
            this.Focus();
        }


        public void MoveCursorLeft()
        {
            this.cursorIndex -= 1;
            if (this.cursorIndex < 0)
            {
                this.cursorIndex = 0;
            }

            this.Invalidate();
        }

        public void MoveCursorRight()
        {
            this.cursorIndex += 1;
            if (this.cursorIndex > this.inputItemListRebuild.Count)
            {

                this.cursorIndex = this.cursorIndex = this.inputItemListRebuild.Count;
            }

            this.drawCursor = true;
            this.Invalidate();
        }



        public void DeleteItem(bool preItem)
        {
            if (preItem)
            {
                if (this.cursorIndex > 0)
                {
                    this.inputItemListRebuild.RemoveAt(this.cursorIndex - 1);
                    this.cursorIndex -= 1;
                    if (this.cursorIndex < 0)
                    {
                        this.cursorIndex = 0;
                    }
                }
            }
            else
            {
                if (this.cursorIndex <= this.inputItemListRebuild.Count - 1)
                {
                    this.inputItemListRebuild.RemoveAt(this.cursorIndex);
                }
            }
            deletetext = true;
            this.Invalidate();
        }

        private void InsertInputItem(IGGChatInputItem item)
        {
            if (this.cursorIndex != this.inputItemListRebuild.Count)
            {
                this.inputItemListRebuild.Insert(this.cursorIndex, item);
                this.cursorIndex += 1;
            }
            else
            {
                inputItemListRebuild.Add(item);
                this.cursorIndex = this.inputItemListRebuild.Count;
            }

            this.Invalidate();
        }

        

        private void CursorControl(object arg)
        {
            this.drawCursor = !this.drawCursor;
            Rectangle r = new Rectangle(cursorPoint, new Size(1, 16));
            this.Invalidate(r);
        }

        internal GGInputContent GetInputContent()
        {
            Dictionary<int, int> emotionDictionary = new Dictionary<int, int>();
            Dictionary<int, Image> imageDictionary = new Dictionary<int, Image>();
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < this.inputItemListRebuild.Count; i++)
            {
                IGGChatInputItem item = this.inputItemListRebuild[i];
                GGChatInputText text = item as GGChatInputText;
                GGChatInPutEmotion emotion = item as GGChatInPutEmotion;
                GGChatInPutImage image = item as GGChatInPutImage;
                if (text != null)
                {
                    sb.Append(text.Content);
                }
                if (emotion != null)
                {
                    emotionDictionary.Add(sb.Length, emotion.EmotionIndex);
                    sb.Append(" ");
                }
                if (image != null)
                {
                    imageDictionary.Add(sb.Length, image.Bitmap);
                    sb.Append(" ");
                }
            }
            return new GGInputContent() { EmotionDictionary = emotionDictionary, ImageDictionary = imageDictionary, Text = sb.ToString() };
        }

        public void OnPointerCursor(Point p)
        {
            int offsetX = ChatInPutBox.OffSetX;
            int offsetY = ChatInPutBox.OffSetY;

            for (int i = 0; i < this.inputItemListRebuild.Count; i++)
            {
                IGGChatInputItem pointerListItem = inputItemListRebuild[i];
                if (i == 0 && offsetX + pointerListItem.Width / 2 > p.X && offsetY + pointerListItem.Height > p.Y)
                {
                    this.cursorIndex = 0;
                    this.Invalidate();
                    return;
                }

                IGGChatInputItem item = this.inputItemListRebuild[i];
                //if (offsetX + item.Width +4 >=this.Width-40)
                if (offsetX + item.Width + ChatInPutBox.RightPadding >= this.maxWidth)
                {
                    offsetY += ChatInPutBox.RowHeight;
                    offsetX = ChatInPutBox.LeftPadding;
                }

                if (item.IsEnter)
                {
                    offsetY += ChatInPutBox.RowHeight;
                    offsetX = ChatInPutBox.LeftPadding;
                }
                else
                {
                    offsetX += item.Width + ChatInPutBox.ItemInterval;
                }

                if (offsetX + pointerListItem.Width / 2 >= p.X && offsetY + pointerListItem.Height >= p.Y)
                {
                    this.cursorIndex = i + 1;
                    this.Invalidate();
                    return;
                }

            }
        }         

        //将剪贴板的内容复制到文本中
        private async void WriteClipboardText()
        {
            Task<string> task = this.GetClipboardText();
            await task.ConfigureAwait(true);
            foreach (char item in task.Result)
            {
                this.AppendText(item.ToString());
            }
        }


        //获得剪辑版的文本
        private Task<string> GetClipboardText()
        {
            IDataObject iData = Clipboard.GetDataObject();
            string s = (string)iData.GetData(DataFormats.Text);
            return Task.Run(() =>
            {
                return s;
            });
        }



        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.maxWidth = this.Width;
            this.maxHeight = this.Height;           
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!deletetext)
            {
                string text = this.textBox1.Text;
                string v = text.Substring(text.Length - 1, 1);
                this.AppendText(v);
            }
            else
            {
                deletetext = false;
            }
            
        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
            this.textBox1.SelectionStart = this.textBox1.Text.Length;
            this.textBox1.ScrollToCaret();
            base.OnPreviewKeyDown(e);
            Keys keyCode = e.KeyCode;
            switch (keyCode)
            {
                case Keys.Left:
                    this.MoveCursorLeft();
                    break;
                case Keys.Right:
                    this.MoveCursorRight();
                    break;
                case Keys.Delete:
                    this.DeleteItem(false);
                    break;
                case Keys.Back:
                    this.DeleteItem(true);
                    break;
                case Keys.Enter:
                    this.AppendText("\n");
                    break;
                case Keys.Up:
                    this.MoveCursorUpDown(true);
                    break;
                case Keys.Down:
                    this.MoveCursorUpDown(false);
                    break;
                case Keys.V:
                    if (e.Modifiers == Keys.Control)
                    {
                        this.WriteClipboardText();
                    }
                    break;
            }
        }
    }





}
