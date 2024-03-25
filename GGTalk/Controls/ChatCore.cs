using ESBasic;
using ESPlus.FileTransceiver;
using GGTalk.Core;
using GGTalk.Properties;
using OMCS.Passive.ShortMessages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GGTalk.Controls
{
    #region MessageBox
    //控件定义的接口
    internal interface IGGChatControl
    {
        void Draw(Graphics g);

        void OnMouseMove(Point p);
        void OnMouseDown(Point p);
        void OnMouseleave();

        void SetOffsetY(int offsetY);

        int Height { get; }

        Rectangle AllSize { get; }

        /// <summary>
        /// 消息ID
        /// </summary>
        string MsgID { get; }
    }

    //系统消息提示
    internal class GGSystemTipLabel : IGGChatControl
    {
        private SolidBrush SystemTipBrush = new SolidBrush(Color.Gray);
        private int offsetY;
        private string SystemTipLabels;
        private IChatControl parent;
        private RectangleF rectangleF;

        private string msgID = string.Empty;
        public string MsgID => this.msgID;

        private int height;
        public int Height
        {
            get
            {
                return this.height;
            }
        }
        public Rectangle AllSize { get; set; }

        public void SetOffsetY(int offsetY)
        {
            this.offsetY = offsetY;
        }

        public GGSystemTipLabel(String SystemTipLabel, IChatControl _parent)
        {
            this.SystemTipLabels = SystemTipLabel;
            this.parent = _parent;
            this.height = 12;
        }


        public void Draw(Graphics g)
        {
            string fontname = System.Drawing.SystemFonts.DefaultFont.Name;
            FontFamily fontFamily = new FontFamily(fontname);
            Font font = new Font(fontFamily, 12, FontStyle.Regular, GraphicsUnit.Pixel);
            Size size = TextRenderer.MeasureText(this.SystemTipLabels, font);
            
            rectangleF = new RectangleF(0, offsetY, 500, 200);
            rectangleF.X = (parent.Bounds.Width - size.Width) / 2;
            g.DrawString(SystemTipLabels, font, SystemTipBrush, rectangleF);
        }

        public void OnMouseleave()
        {
        }
        public void OnMouseDown(Point p)
        {
        }

        public void OnMouseMove(Point p)
        {
        }
    }

    //发送表情和文本
    internal class GGChatContentEmotionAndText : IGGChatControl
    {

        private List<IGGChatInputItem> AllTestAndEmotion;
        private ContextMenu contextMenu = new ContextMenu();
        private DrawTextAttribute drawTextAttribute = new DrawTextAttribute();
        public int offsetY;
        private IChatControl parent;
        private Rectangle rectangle;
        private Point headPos;
        private Point rectPos;
        public int textheight;
        private SolidBrush bs;
        private Pen pen = new Pen(Color.Transparent);
        float radius = 0.5F;
        private int Allheight;
        private bool Isme;
        private int ChatConstraint;
        private Image bitmap;
        public bool disNameIf;//是否显示名称
        public string sendName;
        public Point namePos;

        public int Height
        {
            get
            {
                return Allheight;
            }
        }
        public Rectangle AllSize { get; set; }

        private string msgID = string.Empty;
        public string MsgID => this.msgID;

        public GGChatContentEmotionAndText(List<IGGChatInputItem> all, IChatControl chatBox, SenderInfo senderInfo)
        {
            this.Init(all, senderInfo.IsMe, chatBox);
            this.offsetY = 100;
            this.bitmap = senderInfo.HeadImg;
            this.sendName = senderInfo.DisplayName;
            this.disNameIf = senderInfo.SenderShowType == SenderShowType.HeadAndName;
        }

        private void Init(List<IGGChatInputItem> all, bool isme, IChatControl chatBox)
        {
            this.AllTestAndEmotion = all;
            this.Isme = isme;
            this.parent = chatBox;
            int tx = 0;
            int ty = 0;

            for (int i = 0; i < all.Count; i++)
            {
                IGGChatInputItem aloneItem = AllTestAndEmotion[i];
                GGChatInputText aloneText = aloneItem as GGChatInputText;
                if (aloneText != null)
                {
                    this.drawTextAttribute.Text += aloneText.Content;
                    if (aloneText.Content == "\n")
                    {
                        tx = 0;
                        ty += RowHeight;
                        continue;
                    }
                }

                if (tx + aloneItem.Width + GGChatContentEmotionAndText.RightPadding >= 310)
                {
                    tx = 0;
                    ty += RowHeight;
                }

                tx += aloneItem.Width + ItemInterval;
            }
            this.Allheight = ty + AllTestAndEmotion[AllTestAndEmotion.Count - 1].Height;
        }


        public void SetOffsetY(int offsetY)
        {
            this.offsetY = offsetY;
        }


        private void Item_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(drawTextAttribute.Text);
        }


        private int aloneX;
        private int aloneY;

        private const int RowHeight = 24;
        private const int RightPadding = 1;
        private const int LeftPadding = 1;
        private const int ItemInterval = 1;
        private int widthx;

        public void Draw(Graphics g)
        {
            ChatConstraint = (int)this.parent.Bounds.Width - 130; ;
            int heighta = 24;
            int counta = 0;
            int widtha = 0;
            
            //获取宽度
            for (int i = 0; i < AllTestAndEmotion.Count; i++)
            {
                IGGChatInputItem aloneItem = AllTestAndEmotion[i];
                GGChatInputText aloneText = aloneItem as GGChatInputText;
                if (aloneText != null && aloneText.Content == "\n")
                {
                    counta++;
                    widtha = 0;
                    heighta += RowHeight;
                    continue;
                }
                if (widtha + aloneItem.Width + GGChatContentEmotionAndText.RightPadding + 4 >= ChatConstraint)
                {
                    counta++;
                    widtha = 0;
                    heighta += RowHeight;
                    widthx = ChatConstraint;
                }
                if (aloneText != null)
                {
                    widtha += aloneItem.Width - 7;
                }
                else
                {
                    widtha += aloneItem.Width + ItemInterval;
                }
                widthx = widthx > widtha ? widthx : widtha;
            }
            

            int listotal = AllTestAndEmotion.Count;
            if (Isme)
            {
                if (disNameIf)
                {
                    headPos = new Point(this.parent.Bounds.Width - 56, offsetY);
                    rectPos = new Point(this.parent.Bounds.Width - widthx - 76, offsetY + 28);
                    namePos = new Point(this.parent.Bounds.Width - 64 - TextRenderer.MeasureText(sendName, new Font("微软雅黑", 9)).Width, offsetY + 4);
                }
                else
                {
                    rectPos = new Point(this.parent.Bounds.Width - widthx - 76, offsetY + 4);
                    headPos = new Point(this.parent.Bounds.Width - 56, offsetY);
                }
                bs = new SolidBrush(Color.White);
            }
            else
            {
                if (disNameIf)
                {
                    headPos = new Point(18, offsetY);
                    rectPos = new Point(60, offsetY + 28);
                    namePos = new Point(60, offsetY + 4);
                }
                else
                {
                    headPos = new Point(18, offsetY);
                    rectPos = new Point(60, offsetY + 4);
                }
                bs = new SolidBrush(Color.White);
            }
            aloneX = (int)rectPos.X;
            aloneY = (int)rectPos.Y;

            rectangle = new Rectangle(rectPos, new Size(widthx + 12, heighta + 10));
            this.DrawRectangle(g, rectangle, radius);

            for (int i = 0; i < listotal; i++)
            {
                IGGChatInputItem aloneItem = AllTestAndEmotion[i];
                GGChatInputText aloneText = aloneItem as GGChatInputText;
                //GGChatInPutImage aloneImage= aloneItem as GGChatInPutImage;

                if (aloneText != null && aloneText.Content == "\n")
                {
                    aloneX = (int)rectPos.X;
                    aloneY += RowHeight;
                    continue;
                }
                if (aloneX + aloneItem.Width + GGChatContentEmotionAndText.RightPadding + 4 >= ChatConstraint + (int)rectPos.X)
                {
                    aloneX = (int)rectPos.X;
                    aloneY += RowHeight;
                }

                aloneItem.Draw(g, new Point(aloneX + 4, aloneY + 8));

                if (aloneText != null)
                {
                    aloneX += aloneItem.Width - 7;
                }
                else
                {
                    aloneX += aloneItem.Width + ItemInterval;
                }

            }
            this.Allheight = aloneY - offsetY + AllTestAndEmotion[listotal - 1].Height;
            if (disNameIf)
            {
                g.DrawString(sendName, new Font("微软雅黑", 9), new SolidBrush(Color.Gray), namePos);
            }
            this.DrawImage(g, bitmap, new Point(headPos.X, headPos.Y), new Size(32, 32));

        }
        //绘制矩形
        public void DrawRectangle(Graphics g, Rectangle rectangle, float f)
        {
            g.DrawRectangle(pen, rectangle);
            g.FillRectangle(bs, rectangle);
        }

        public void DrawImage(Graphics g, Image bitmap, Point point)
        {
            this.DrawImage(g, bitmap, point, bitmap.Size);
        }

        public void DrawImage(Graphics g, Image bitmap, Point point, Size dstSize)
        {
            Image image = bitmap;
            Rectangle rectangle = new Rectangle(point, new Size(32, 32));
            g.DrawImage(image, rectangle);
        }



        private bool selected = false;
        public void OnMouseleave()
        {
        }

        public void OnMouseDown(Point p)
        {
        }
        public void OnMouseMove(Point p)
        {
        }
    }


    //发送图片
    internal class GGChatContentImage : IGGChatControl
    {
        private int offsetY;
        private bool Isme;
        private IChatControl parent;
        private Point headPos;
        private Point rectPos;
        private Size maxBitmapSize = new Size(0, 0);
        public bool disNameIf;//是否显示名称
        public string sendName;
        public Point namePos;
        private Image head;
        private Image messageImage = null;
        public Image MessageImage => this.messageImage;
        public GGChatContentImage(Image _messageImage, IChatControl _parent, SenderInfo senderInfo)
        {
            this.Init(senderInfo.IsMe, _parent, _messageImage);
            this.head = senderInfo.HeadImg;
            this.sendName = senderInfo.DisplayName;
            this.disNameIf = senderInfo.SenderShowType == SenderShowType.HeadAndName;
            this.maxBitmapSize = CommonHelper.GetBitmapMaxSize(_messageImage, (int)(_parent.Bounds.Width * 0.6f));
            if (this.disNameIf)
            {
                this.height = (int)maxBitmapSize.Height + 24;
            }
            else
            {
                this.height = (int)maxBitmapSize.Height;
            }
        }


        private void Init(bool _Isme, IChatControl _parent, Image _messageImage)
        {
            this.parent = _parent;
            this.Isme = _Isme;
            this.messageImage = _messageImage;
        }
        private string msgID = string.Empty;
        public string MsgID => this.msgID;
        private int height;
        public int Height
        {
            get
            {
                return this.height;
            }
        }
        public Rectangle AllSize { get; set; }

        public void SetOffsetY(int offsetY)
        {
            this.offsetY = offsetY;
        }
        public void Draw(Graphics g)
        {

            if (Isme)
            {
                if (disNameIf)
                {
                    headPos = new Point(this.parent.Bounds.Width - 56, offsetY);
                    namePos = new Point(this.parent.Bounds.Width - 64 - TextRenderer.MeasureText(sendName, new Font("微软雅黑", 9)).Width, offsetY + 4);
                    rectPos = new Point(this.parent.Bounds.Width - 64 - maxBitmapSize.Width, offsetY + 24);
                }
                else
                {
                    headPos = new Point(this.parent.Bounds.Width - 56, offsetY);
                    rectPos = new Point(this.parent.Bounds.Width - 64 - maxBitmapSize.Width, offsetY);
                }

            }
            else
            {
                if (disNameIf)
                {
                    headPos = new Point(18, offsetY);
                    namePos = new Point(60, offsetY);
                    rectPos = new Point(60, offsetY + 24);
                }
                else
                {
                    headPos = new Point(18, offsetY);
                    rectPos = new Point(60, offsetY);
                }

            }
            if (disNameIf)
            {
                g.DrawString(sendName, new Font("微软雅黑", 9), new SolidBrush(Color.Gray), namePos);
            }
            this.DrawImage(g, head, headPos, new Size(32, 32));
            this.DrawImage(g, messageImage, rectPos);
            this.AllSize = new Rectangle(rectPos, this.maxBitmapSize);
        }


        private void DrawImage(Graphics g, Image bitmap, Point pos)
        {
            this.DrawImage(g, bitmap, pos, this.maxBitmapSize);
        }
        private void DrawImage(Graphics g, Image bitmap, Point pos, Size dstSize)
        {
            Rectangle rectangles = new Rectangle(new Point(pos.X, pos.Y), dstSize);
            g.DrawImage(bitmap, rectangles);
        }



        public void OnMouseleave()
        {
        }
        public void OnMouseDown(Point p)
        {

        }

        public void OnMouseMove(Point p)
        {
            if (this.AllSize.Contains(p))
            {
                this.parent.Cursor = ChatBoxCursors.Hand;
            }
            else
            {
                this.parent.Cursor = ChatBoxCursors.Arrow;
            }
        }
    }

    //发送文件
    internal class GGFileBox : IGGChatControl
    {
        private Rectangle rect;
        private int offsetY;
        public Point headPos;
        public bool Isme;
        public string filename;
        public string FileType;
        public string fileSize;
        public int transprogress;
        public string informationStr;
        private int scrollBarExternalWidth = 0;
        /// <summary>
        /// 进度 （0-100）
        /// </summary>
        public int ScrollBarExternalWidth { set { this.scrollBarExternalWidth = value; } }
        public Rectangle AllSize { get; set; }
        public IChatControl parent;
        private Point fileSizePoint;
        private Point fileSizeBackGroundPoint;
        public Bitmap fileTypeImage = null;
        private int offsetX = 58;
        private int splitHeight;
        private SolidBrush outMost = new SolidBrush(Color.FromArgb(237, 237, 237));
        private SolidBrush splitBrush = new SolidBrush(Color.FromArgb(78, 197, 32));
        private SolidBrush fileSizeBrush = new SolidBrush(Color.White);
        private SolidBrush externalBrush = new SolidBrush(Color.White);
        public bool disNameIf;//是否显示名称
        public string sendName;
        public Point namePos;
        public string skinLabel_Tip { get; set; }
        #region//取消\拒绝 按钮
        public string skinLabel_cancel;
        public Rectangle bt_CancelRrectangle;
        public SolidBrush btaColor;
        public int btaCount;
        public int btaLeaveCount;
        #endregion

        #region//转离线发送\接收\打开文件夹 按钮
        public string skinLabel_received;
        public Rectangle bt_ReceivedRrectangle;
        public SolidBrush btbColor;
        public int btbCount;
        public int btbLeaveCount;
        #endregion

        #region//打开按钮
        public string skinLabel_open;
        public Rectangle bt_OpenRrectangle;
        public SolidBrush btcColor;
        public int btcCount;
        public int btcLeaveCount;
        #endregion

        private SolidBrush aa = new SolidBrush(Color.SkyBlue);
        private SolidBrush bb = new SolidBrush(Color.SkyBlue);
        private SolidBrush cc = new SolidBrush(Color.SkyBlue);

        private TransferingProject transferingProject;
        public TransferingProject TransferingProject
        {
            get
            {
                return this.transferingProject;
            }
        }

        #region Event

        /// <summary>
        /// FileCanceled 当点击“取消”按钮时，将触发此事件。参数： GGFileBox, projectID,isSend
        /// </summary>
        public event System.Action<GGFileBox, string, bool> FileCanceled;
        /// <summary>
        /// 当 点击 “接收”按钮 时，触发  参数：GGFileBox,projectID,isSend,savePath
        /// </summary>
        public event System.Action<GGFileBox, string, bool, string> FileReceived;
        /// <summary>
        /// 当点击“拒绝”按钮时，触发  参数：projectID
        /// </summary>
        public event System.Action<string> FileRejected;

        /// <summary>
        /// 当点击“转离线发送”按钮时，触发。
        /// </summary>
        public event CbGeneric<TransferingProject, GGFileBox> ConvertToOfflineFile;

        #endregion

        //public event ESBasic.CbGeneric SaveButtonClicked;
        Image Head = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfoPara"></param>
        /// <param name="_userAgentClient">文件说明</param>
        /// <param name="_ScrollBarExternalWidth">进度（0-100）</param>
        /// <param name="_parent"></param>
        /// <param name="senderInfo"></param>
        public GGFileBox(IChatControl _parent)
        {
            this.parent = _parent;
            btaColor = new SolidBrush(Color.FromArgb(58, 144, 230));
            btcColor = new SolidBrush(Color.FromArgb(58, 144, 230));
            btbColor = new SolidBrush(Color.FromArgb(58, 144, 230));
        }

        #region Initialize
        public void Initialize(TransferingProject info, bool offlineFile, bool doneAgreed, SenderInfo senderInfo)
        {
            this.transferingProject = info;
            this.filename = info.ProjectName;
            this.fileSize = ESBasic.Helpers.PublicHelper.GetSizeString((ulong)this.transferingProject.TotalSize);
            this.fileTypeImage = CommonHelper.GetFileIconBitmap(info.ProjectName);


            this.Isme = senderInfo.IsMe;
            this.Head = senderInfo.HeadImg;
            this.sendName = senderInfo.DisplayName;
            this.disNameIf = senderInfo.SenderShowType == SenderShowType.HeadAndName;
            if (this.disNameIf)
            {
                this.height = 120;
            }
            else
            {
                this.height = 96;
            }
            if (info.IsSender)
            {
                this.skinLabel_cancel = "取消";
                this.skinLabel_received = "转离线发送";
            }
            else
            {
                this.skinLabel_cancel = "拒绝";
                this.skinLabel_received = "接收";
            }
            this.splitHeight = 1;
        }

        #endregion

        private string msgID = string.Empty;
        public string MsgID => this.msgID;
        private int height;
        public int Height
        {
            get
            {
                return this.height;
            }
        }

        public void SetOffsetY(int offsetY)
        {
            this.offsetY = offsetY;
        }

        private bool isTransfering = false;
        public bool IsTransfering
        {
            get { return isTransfering; }
            set { isTransfering = value; }
        }

        /// <summary>
        /// 设置进度值
        /// </summary>
        /// <param name="scrollValue">进度 （0-100）</param>
        private void SetScrollBarExternalWidth(int scrollValue)
        {
            this.scrollBarExternalWidth = scrollValue;
            this.parent.Invalidate(this.AllSize);
        }

        private DateTime lastSetTime = DateTime.Now;
        public void SetProgress(ulong total, ulong transmitted)
        {
            TimeSpan span = DateTime.Now - this.lastSetTime;
            if (span.TotalSeconds < 0.2)
            {
                return;
            }
            this.lastSetTime = DateTime.Now;
            int scrollValue = (int)((float)transmitted * 100 / total);
            if (scrollValue > 95)
            {

            }
            this.SetScrollBarExternalWidth(scrollValue);
        }
        private DateTime lastDisplaySpeedTime = DateTime.Now;
        private ulong lastTransmittedPreSecond = 0;
        private ulong lastSpeed = 0;
        private bool firstSecond = true; //解决续传时，初始速度非常大的bug
        private ulong totalSize = 1; //解决0速度的问题
        private ulong lastTransmitted = 0;
        private void SetProgress2(ulong total, ulong transmitted)
        {
            this.totalSize = total;
            this.lastTransmitted = transmitted;


            DateTime now = DateTime.Now;
            TimeSpan span = now - this.lastDisplaySpeedTime;

            if (span.TotalSeconds >= 1)
            {
                if (!this.firstSecond)
                {
                    if (lastSpeed == 0)
                    {
                        lastSpeed = (ulong)((transmitted - this.lastTransmittedPreSecond) / span.TotalSeconds); ;
                    }

                    ulong transferSpeed = (ulong)((transmitted - this.lastTransmittedPreSecond) / span.TotalSeconds);
                    this.lastSpeed = transferSpeed;
                    byte littleNum = 0;
                    if (transferSpeed > 1024 * 1024)
                    {
                        littleNum = 1;
                    }
                    //this.label_speed.Text = PublicHelper.GetSizeString((ulong)transferSpeed, littleNum) + "/s";
                    int leftSecs = transferSpeed == 0 ? 10000 : (int)((total - transmitted) / transferSpeed);
                    int hour = leftSecs / 3600;
                    int min = (leftSecs % 3600) / 60;
                    int sec = ((leftSecs % 3600) % 60) % 60;
                    this.lastDisplaySpeedTime = now;
                }

                this.lastTransmittedPreSecond = transmitted;

                if (this.firstSecond)
                {
                    this.firstSecond = false;
                }

            }
        }


        public void Draw(Graphics g)
        {
            string fontname = System.Drawing.SystemFonts.DefaultFont.Name;
            FontFamily fontFamily = new FontFamily(fontname);
            Size Constraint = new Size(200, 200);
            Size headSize = new Size(32, 32);

            //通过判断是谁发送，确认发送者头像和GGFileBox控件的位置
            if (Isme)
            {
                if (disNameIf)
                {
                    namePos = new Point(this.parent.Bounds.Width - 64 - TextRenderer.MeasureText(sendName, new Font("微软雅黑", 9)).Width, offsetY + 4);
                    this.rect = new Rectangle(new Point(this.parent.Bounds.Width - offsetX - 300, offsetY + 24), new Size(0, 0));
                    this.headPos = new Point(this.parent.Bounds.Width - 56, offsetY);
                }
                else
                {
                    this.rect = new Rectangle(new Point(this.parent.Bounds.Width - offsetX - 300, offsetY), new Size(0, 0));
                    this.headPos = new Point(this.parent.Bounds.Width - 56, offsetY);
                }
            }
            else
            {
                if (disNameIf)
                {
                    namePos = new Point(offsetX, offsetY);
                    this.rect = new Rectangle(new Point(offsetX, offsetY + 24), new Size(0, 0));
                    this.headPos = new Point(18, offsetY);
                }
                else
                {
                    this.rect = new Rectangle(new Point(offsetX, offsetY), new Size(0, 0));
                    this.headPos = new Point(18, offsetY);
                }
            }
            if (disNameIf)
            {
                g.DrawString(sendName, new Font("微软雅黑", 9), new SolidBrush(Color.Gray), namePos);
            }
            fileSizePoint = new Point(rect.X, rect.Y);
            fileSizeBackGroundPoint = new Point(rect.X, rect.Y);
            Font fontA = new Font("微软雅黑", 13, FontStyle.Regular, GraphicsUnit.Pixel);
            Font fontB = new Font("微软雅黑", 12, FontStyle.Regular, GraphicsUnit.Pixel);
            Font fontC = new Font("微软雅黑", 12, FontStyle.Regular, GraphicsUnit.Pixel);
            g.DrawImage(Head, new Rectangle(headPos, headSize));
            g.FillRectangle(outMost, rect.X, rect.Y, 296, 98);
            g.FillRectangle(externalBrush, rect.X + 1, rect.Y + 1, 294, 96);
            g.FillRectangle(new SolidBrush(Color.FromArgb(211, 211, 211)), rect.X + 1, rect.Y + 65, 294, splitHeight);


            g.DrawImage(fileTypeImage, rect.X + 13, rect.Y + 7, 42, 50);
            g.DrawString(Compute.LimitText(filename, fontA, 130), fontA, new SolidBrush(Color.Black), rect.X + 60, rect.Y + 12);
            //提示文件传输失败，被对方拒绝


            g.DrawString(Compute.LimitText(fileSize, fontB, 130), fontB, new SolidBrush(Color.Gray), rect.X + 60 + TextRenderer.MeasureText(filename, fontA).Width, rect.Y + 13);
            Size sizea = TextRenderer.MeasureText(skinLabel_cancel, fontC);
            Size sizeb = TextRenderer.MeasureText(skinLabel_received, fontC);
            Size sizec = TextRenderer.MeasureText(skinLabel_open, fontC);
            Point btaPos = new Point(rect.X - sizea.Width + 285, rect.Y + 74);
            Point btbPos = new Point(rect.X - sizea.Width - sizeb.Width + 275, rect.Y + 74);
            Point btcPos = new Point(rect.X - sizea.Width - sizeb.Width - sizec.Width + 265, rect.Y + 74);
            if (this.transferingProject.IsSender)
            {
                g.DrawString(skinLabel_cancel, fontC, btaColor, btaPos);
                g.DrawString(skinLabel_received, fontC, btbColor, btbPos);
                g.FillRectangle(splitBrush, rect.X + 1, rect.Y + 65, this.scrollBarExternalWidth * 2.94f, splitHeight);

                bt_CancelRrectangle = new Rectangle(btaPos.X, btaPos.Y, sizea.Width, sizea.Height);
                bt_ReceivedRrectangle = new Rectangle(btbPos.X, btbPos.Y, sizeb.Width, sizeb.Height);
            }
            else
            {
                g.DrawString(skinLabel_cancel, fontC, btaColor, btaPos);
                g.DrawString(skinLabel_received, fontC, btbColor, btbPos);
                g.DrawString(skinLabel_open, fontC, btcColor, btcPos);
                g.FillRectangle(splitBrush, rect.X + 1, rect.Y + 65, this.scrollBarExternalWidth * 2.94f, splitHeight);
                bt_CancelRrectangle = new Rectangle(btaPos.X, btaPos.Y, sizea.Width, sizea.Height);
                bt_ReceivedRrectangle = new Rectangle(btbPos.X, btbPos.Y, sizeb.Width, sizeb.Height);
                bt_OpenRrectangle = new Rectangle(btcPos.X, btcPos.Y, sizec.Width, sizec.Height);
            }
            g.DrawString(skinLabel_Tip, fontA, new SolidBrush(Color.Gray), rect.X + 14, rect.Y + 74);
            AllSize = new Rectangle(rect.X, rect.Y, 296, 98);
        }

        #region//文件传输事件处理


        /// <summary>
        /// 发送方转离线传输
        /// </summary>
        private void Transmit_Gooffline()
        {
            this.splitHeight = 2;
            this.skinLabel_received = null;
            this.parent.Invalidate(this.AllSize);
        }

        /// <summary>
        /// 传输进行时调用该方法
        /// </summary>
        public void Transmit_Running()
        {
            this.splitHeight = 2;
            this.skinLabel_cancel = "取消";
            this.skinLabel_received = null;
            this.skinLabel_open = null;
            btaColor = new SolidBrush(Color.FromArgb(58, 144, 230));
            this.parent.Invalidate(this.AllSize);
        }

        public void Transmit_Disruptted(string reason)
        {
            this.splitHeight = 2;
            this.ShowTips(reason);
        }

        /// <summary>
        /// 传输完成时调用该方法
        /// </summary>
        public void Transmit_Completed()
        {
            this.splitHeight = 1;
            this.scrollBarExternalWidth = 0;
            if (this.transferingProject.IsSender)
            {
                this.skinLabel_cancel = "文件已发送";
                this.skinLabel_received = null;
                this.skinLabel_open = null;
            }
            else
            {
                this.skinLabel_cancel = "转发";
                this.skinLabel_received = "打开文件夹";
                this.skinLabel_open = "打开";
            }
            btaColor = new SolidBrush(Color.FromArgb(58, 144, 230));
            btcColor = new SolidBrush(Color.FromArgb(58, 144, 230));
            btbColor = new SolidBrush(Color.FromArgb(58, 144, 230));
            this.parent.Invalidate(this.AllSize);
        }


        /// <summary>
        /// 取消传输事件
        /// </summary>
        public void Transmit_Canelled()
        {
            this.splitHeight = 1;
            this.scrollBarExternalWidth = 0;
            this.ShowTips("文件传输取消");
        }

        private void ShowTips(string msg)
        {
            this.skinLabel_cancel = null;
            this.skinLabel_received = null;
            this.skinLabel_open = null;
            this.skinLabel_Tip = msg;
            this.parent.Invalidate(this.AllSize);
        }
        #endregion

        private void DrawImage(Graphics g, Bitmap img, Point position)
        {
            this.DrawImage(g, img, position, img.Size);
        }
        private void DrawImage(Graphics g, Bitmap img, Point position, Size dstSize)
        {

            Rectangle rectangledi = new Rectangle(position, dstSize);
            g.DrawImage(img, rectangledi);
        }



        public void OnMouseleave()
        {
        }


        /// <summary>
        /// 文件操作按钮悬浮事件  bta对应右边的按钮 btb中间的按钮 btc左边的按钮
        /// </summary>
        /// <param name="p"></param>
        public void OnMouseMove(Point p)
        {
            if (bt_CancelRrectangle.Contains(p))
            {
                if (btaLeaveCount != 1)
                {
                    this.parent.Cursor = ChatBoxCursors.Hand;
                    btaColor = new SolidBrush(Color.Black);
                    this.parent.Invalidate(bt_CancelRrectangle);
                }
                btaCount = 1;
                btaLeaveCount = 1;

            }
            else if (bt_OpenRrectangle.Contains(p))
            {
                if (btcLeaveCount != 1)
                {
                    this.parent.Cursor = ChatBoxCursors.Hand;
                    btcColor = new SolidBrush(Color.Black);
                    this.parent.Invalidate(bt_OpenRrectangle);
                }
                btcCount = 1;
                btcLeaveCount = 1;
            }
            else if (bt_ReceivedRrectangle.Contains(p))
            {
                if (btbLeaveCount != 1)
                {
                    this.parent.Cursor = ChatBoxCursors.Hand;
                    btbColor = new SolidBrush(Color.Black);
                    this.parent.Invalidate(bt_ReceivedRrectangle);
                }
                btbCount = 1;
                btbLeaveCount = 1;
            }
            else
            {
                if (btcCount == 1)
                {
                    this.parent.Cursor = ChatBoxCursors.Arrow;
                    btcColor = new SolidBrush(Color.FromArgb(58, 144, 230));
                    this.parent.Invalidate(bt_OpenRrectangle);
                }
                if (btaCount == 1)
                {
                    this.parent.Cursor = ChatBoxCursors.Arrow;
                    btaColor = new SolidBrush(Color.FromArgb(58, 144, 230));
                    this.parent.Invalidate(bt_CancelRrectangle);
                }
                if (btbCount == 1)
                {
                    this.parent.Cursor = ChatBoxCursors.Arrow;
                    btbColor = new SolidBrush(Color.FromArgb(58, 144, 230));
                    this.parent.Invalidate(bt_ReceivedRrectangle);
                }
                btcCount = 0;
                btcLeaveCount = 0;
                btaCount = 0;
                btaLeaveCount = 0;
                btbCount = 0;
                btbLeaveCount = 0;
            }
        }

        /// <summary>
        /// 文件操作按钮点击事件  bta对应右边的按钮 btb中间的按钮 btc左边的按钮
        /// </summary>
        /// <param name="p"></param>
        public void OnMouseDown(Point p)
        {
            if (bt_CancelRrectangle.Contains(p))
            {
                this.skinLabel_Cancel_Click();
            }
            else if (bt_ReceivedRrectangle.Contains(p))
            {
                this.linkLabel_receive_LinkClicked();
            }
            else if (bt_OpenRrectangle.Contains(p))
            {
                this.skinLabel_Open_Click();
            }

        }

        //点击接收
        private void linkLabel_receive_LinkClicked()
        {
            try
            {
                if (this.skinLabel_received == "接收")
                {
                    string savePath = ESBasic.Helpers.FileHelper.GetPathToSave("保存", this.transferingProject.ProjectName, null);
                    if (!string.IsNullOrEmpty(savePath))
                    {
                        if (this.FileReceived != null)
                        {
                            this.skinLabel_received = null;
                            this.skinLabel_cancel = "取消";
                            this.FileReceived(this, this.transferingProject.ProjectID, this.transferingProject.IsSender, savePath);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else if (this.skinLabel_received == "转离线发送")
                {
                    if (this.ConvertToOfflineFile != null)
                    {
                        this.ConvertToOfflineFile(this.transferingProject, this);
                    }
                }
                else if (this.skinLabel_received == "打开文件夹")
                {
                    if (this.skinLabel_open == "打开")
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(ESBasic.Helpers.FileHelper.GetFileDirectory(this.transferingProject.LocalSavePath));
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show("打开文件夹失败：" + ee.Message);
                        }
                    }

                }

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "GGFileBox.linkLabel_receive_LinkClicked");
            }
        }

        //点击拒绝或取消
        private void skinLabel_Cancel_Click()
        {
            try
            {
                if (this.skinLabel_cancel == "拒绝")
                {
                    if (this.FileRejected != null)
                    {
                        this.FileRejected(this.transferingProject.ProjectID);
                    }
                }
                else if (this.skinLabel_cancel == "取消")
                {
                    if (this.FileCanceled != null)
                    {
                        this.FileCanceled(this, this.transferingProject.ProjectID, this.transferingProject.IsSender);
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "GGFileBox.skinLabel_Cancel_Click");
            }
        }

        private void skinLabel_Open_Click()
        {
            try
            {
                if (this.skinLabel_open == "打开")
                {
                    try
                    {
                        System.Diagnostics.Process.Start(this.transferingProject.LocalSavePath);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("打开文件失败：" + ee.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "GGFileBox.skinLabel_Open_Click");
            }
        }
    }

    internal class GGChatCard : IGGChatControl
    {
        #region 参数
        public Image headImage { get; }

        public Image cardImage { get; }
        public string nickName { get; }
        public string userID { get; }
        public bool isMe { get; set; }
        public IChatControl parent { get; set; }
        public int offsetY { get; set; }
        public Point headPos { get; set; }
        public Point msgBoxPos { get; set; }

        private string msgID = string.Empty;
        public string MsgID => this.msgID;
        /// <summary>
        /// 返回当前绘制名片的高度
        /// </summary>
        private int height;
        public int Height
        {
            get
            {
                return this.height;
            }
        }

        public SolidBrush outWardBs;

        public SolidBrush insideBs;

        public SolidBrush cutBs;

        public SolidBrush bottomBs;

        public FontFamily DefaultFontFamily;

        public bool disNameIf;//是否显示名称
        public string sendName;
        public Point namePos;
        /// <summary>
        /// 界面大小改变后重新返回高度
        /// </summary>
        /// <param name="offsetY"></param>
        public void SetOffsetY(int offsetY)
        {
            this.offsetY = offsetY;

        }

        #endregion

        public GGChatCard(PersonCardModel card, ref int offsetY, IChatControl parent, SenderInfo senderInfo)
        {
            this.headImage = senderInfo.HeadImg;
            this.cardImage = card.CardImage;
            this.nickName = card.NickName;
            this.userID = card.UserID;
            this.isMe = senderInfo.IsMe;
            this.sendName = senderInfo.DisplayName;
            this.offsetY = offsetY;
            this.parent = parent;
            outWardBs = new SolidBrush(Color.FromArgb(237, 237, 237));
            insideBs = new SolidBrush(Color.FromArgb(255, 255, 255));
            cutBs = new SolidBrush(Color.FromArgb(247, 247, 247));
            bottomBs = new SolidBrush(Color.FromArgb(255, 255, 255));
            DefaultFontFamily = new FontFamily("微软雅黑");
            this.disNameIf = senderInfo.SenderShowType == SenderShowType.HeadAndName;
            if (this.disNameIf)
            {
                this.height = 126;
            }
            else
            {
                this.height = 102;
            }
        }

        public void Draw(Graphics g)
        {
            //通过判断是谁发送，确认发送者头像和GGFileBox控件的位置
            if (isMe)
            {
                if (disNameIf)
                {
                    namePos = new Point(this.parent.Bounds.Width - 64 - TextRenderer.MeasureText(sendName, new Font("微软雅黑", 9)).Width, offsetY + 4);
                    this.msgBoxPos = new Point(this.parent.Bounds.Width - 288, offsetY + 24);
                    this.headPos = new Point(this.parent.Bounds.Width - 56, offsetY);
                }
                else
                {
                    this.headPos = new Point(this.parent.Bounds.Width - 56, offsetY);
                    this.msgBoxPos = new Point(this.parent.Bounds.Width - 288, offsetY);
                }
            }
            else
            {
                if (disNameIf)
                {
                    namePos = new Point(66, offsetY);
                    this.headPos = new Point(18, offsetY + 24);
                    this.msgBoxPos = new Point(66, offsetY);
                }
                else
                {
                    this.headPos = new Point(18, offsetY);
                    this.msgBoxPos = new Point(66, offsetY);
                }
            }
            if (disNameIf)
            {
                g.DrawString(sendName, new Font("微软雅黑", 9), new SolidBrush(Color.Gray), namePos);
            }
            Font nickNameFont = new Font(DefaultFontFamily, 14, FontStyle.Regular, GraphicsUnit.Pixel);
            Font userIDFont = new Font(DefaultFontFamily, 12, FontStyle.Regular, GraphicsUnit.Pixel);
            g.DrawImage(headImage, new Rectangle(headPos, new Size(36, 36)));
            g.FillRectangle(outWardBs, msgBoxPos.X, msgBoxPos.Y, 222, 102);
            g.FillRectangle(insideBs, msgBoxPos.X + 1, msgBoxPos.Y + 1, 220, 100);
            g.FillRectangle(cutBs, msgBoxPos.X + 1, msgBoxPos.Y + 78, 220, 1);
            g.FillRectangle(bottomBs, msgBoxPos.X + 1, msgBoxPos.Y + 79, 220, 23);
            g.DrawImage(cardImage, new Rectangle(new Point(msgBoxPos.X + 14, msgBoxPos.Y + 14), new Size(52, 52)));
            g.DrawString(Compute.LimitText(nickName, nickNameFont, 100), nickNameFont, new SolidBrush(Color.Black), msgBoxPos.X + 76, msgBoxPos.Y + 20);
            g.DrawString(Compute.LimitText(userID, userIDFont, 100), userIDFont, new SolidBrush(Color.FromArgb(183, 183, 183)), msgBoxPos.X + 76, msgBoxPos.Y + 46);
            g.DrawString("个人名片", new Font(DefaultFontFamily, 12, FontStyle.Regular, GraphicsUnit.Pixel), new SolidBrush(Color.FromArgb(178, 178, 178)), msgBoxPos.X + 14, msgBoxPos.Y + 82);
            AllSize = new Rectangle(msgBoxPos.X, msgBoxPos.Y, 222, 102);
        }

        public Rectangle AllSize { get; set; }

        public void OnMouseleave()
        {
            outWardBs = new SolidBrush(Color.FromArgb(237, 237, 237));
            insideBs = new SolidBrush(Color.FromArgb(255, 255, 255));
            cutBs = new SolidBrush(Color.FromArgb(247, 247, 247));
            bottomBs = new SolidBrush(Color.FromArgb(255, 255, 255));
            this.parent.Invalidate(new Rectangle(msgBoxPos.X, msgBoxPos.Y, 222, 102));
        }
        public void OnMouseMove(Point p)
        {
            outWardBs = new SolidBrush(Color.FromArgb(231, 231, 231));
            insideBs = new SolidBrush(Color.FromArgb(246, 246, 246));
            cutBs = new SolidBrush(Color.FromArgb(231, 231, 231));
            bottomBs = new SolidBrush(Color.FromArgb(242, 242, 242));
            this.parent.Invalidate(new Rectangle(msgBoxPos.X, msgBoxPos.Y, 222, 102));
        }

        public void OnMouseDown(Point p)
        {
        }
    }

    internal class GGAudioMessageBox : IGGChatControl
    {
        public IChatControl parent { get; set; }
        public int offsetY { get; set; }
        private int height;
        public bool disNameIf;
        public bool isMe { get; set; }
        public string sendName { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        public int burningTime { get; set; }

        public Point namePos { get; set; }
        public Point headPos { get; set; }
        public Point msgBoxPos { get; set; }

        public Image headImage;
        public Image recordImage;
        public Rectangle voiceRectangle;
        private System.Windows.Forms.Timer timer1 = new Timer();

        private Bitmap[] leftVoiceBitmaps = new Bitmap[] { Resources.voice_01, Resources.voice_02, Resources.voice_03 };
        private Bitmap[] rightVoiceBitmaps = new Bitmap[] { Resources.voice_right_01, Resources.voice_right_02, Resources.voice_right_03 };

        private string msgID = string.Empty;
        /// <summary>
        /// 消息ID
        /// </summary>
        public string MsgID => this.msgID;
        public int Height
        {
            get
            {
                return this.height;
            }
        }

        public Rectangle AllSize { get; set; }

        private string audioMessageID;
        public string AudioMessageID
        {
            get { return audioMessageID; }
        }

        private object audioMessage;
        public object AudioMessage
        {
            get { return audioMessage; }
        }

        public GGAudioMessageBox(AudioMessage msg, ref int _offsetY, IChatControl _parent, SenderInfo senderInfo)
        {
            this.isMe = senderInfo.IsMe;
            this.headImage = senderInfo.HeadImg;
            this.sendName = senderInfo.DisplayName;
            this.burningTime = msg.SpanInMSecs % 1000 > 0 ? msg.SpanInMSecs / 1000 + 1 : msg.SpanInMSecs / 1000;
            this.msgID = msg.GetUniqueID();
            this.offsetY = _offsetY;
            this.parent = _parent;
            this.audioMessage = msg;
            this.audioMessageID = msg.GetUniqueID();
            if (senderInfo.IsMe)
            {
                this.recordImage = Resources.voice_right_01;
            }
            else
            {
                this.recordImage = Resources.voice_01;
            }

            this.disNameIf = senderInfo.SenderShowType == SenderShowType.HeadAndName;
            if (disNameIf)
            {
                this.height = 50;
            }
            else
            {
                this.height = 26;
            }
            this.timer1.Tick += Timer1_Tick;
            this.timer1.Interval = 200;
        }

        private int imageIndex = 0;
        private void Timer1_Tick(object sender, EventArgs e)
        {
            ++imageIndex;
            Bitmap bitmap = this.isMe ? this.rightVoiceBitmaps[imageIndex % this.rightVoiceBitmaps.Length] : this.leftVoiceBitmaps[imageIndex % this.leftVoiceBitmaps.Length];
            this.ChangeImage(bitmap);
        }

        public void Draw(Graphics g)
        {

            if (isMe)
            {
                if (disNameIf)
                {
                    namePos = new Point(this.parent.Bounds.Width - 64 - TextRenderer.MeasureText(sendName, new Font("微软雅黑", 9)).Width, offsetY + 4);
                    this.msgBoxPos = new Point(this.parent.Bounds.Width - 116, offsetY + 24);
                    this.headPos = new Point(this.parent.Bounds.Width - 56, offsetY);
                }
                else
                {
                    this.headPos = new Point(this.parent.Bounds.Width - 56, offsetY);
                    this.msgBoxPos = new Point(this.parent.Bounds.Width - 116, offsetY + 7);
                }
                g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255)), msgBoxPos.X, msgBoxPos.Y, 50, 24);
                g.DrawImage(recordImage, new Rectangle(new Point(msgBoxPos.X + 27, msgBoxPos.Y), new Size(22, 22)));
                g.DrawString(burningTime.ToString() + "''", new Font("微软雅黑", 12, FontStyle.Regular, GraphicsUnit.Pixel), new SolidBrush(Color.Black), msgBoxPos.X + 6, msgBoxPos.Y + 2);

            }
            else
            {
                if (disNameIf)
                {
                    this.headPos = new Point(18, offsetY);
                    namePos = new Point(60, offsetY);
                    this.msgBoxPos = new Point(60, offsetY + 24);
                }
                else
                {
                    this.headPos = new Point(18, offsetY);
                    this.msgBoxPos = new Point(60, offsetY + 5);
                }
                Font burningTimeFont = new Font("微软雅黑", 12, FontStyle.Regular, GraphicsUnit.Pixel);
                g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255)), msgBoxPos.X, msgBoxPos.Y, 50, 24);
                g.DrawImage(recordImage, new Rectangle(new Point(msgBoxPos.X + 3, msgBoxPos.Y), new Size(22, 22)));
                g.DrawString(Compute.LimitText(burningTime.ToString() + "''", burningTimeFont, 40), burningTimeFont, new SolidBrush(Color.Black), msgBoxPos.X + 24, msgBoxPos.Y + 2);

            }
            //是否绘制名称
            if (disNameIf)
            {
                g.DrawString(sendName, new Font("微软雅黑", 9), new SolidBrush(Color.Gray), namePos);
            }
            g.DrawImage(headImage, new Rectangle(headPos, new Size(32, 32)));

            //获取矩形区域
            if (disNameIf)
            {
                AllSize = new Rectangle(msgBoxPos.X, msgBoxPos.Y, 50, 27);
                if (isMe)
                {
                    voiceRectangle = new Rectangle(msgBoxPos.X + 27, msgBoxPos.Y + 24, 22, 22);
                }
                else
                {
                    voiceRectangle = new Rectangle(msgBoxPos.X + 6, msgBoxPos.Y + 24, 22, 22);
                }
            }
            else
            {
                AllSize = new Rectangle(msgBoxPos.X, msgBoxPos.Y, 50, 27);
                if (isMe)
                {

                    voiceRectangle = new Rectangle(msgBoxPos.X + 27, msgBoxPos.Y + 7, 22, 22);
                }
                else
                {

                    voiceRectangle = new Rectangle(msgBoxPos.X + 3, msgBoxPos.Y + 7, 22, 22);

                }
            }
        }
        private bool isPlaying = false;
        public bool IsPlaying => this.isPlaying;
        public void OnMouseDown(Point p)
        {
            //if (this.isPlaying)
            //{
            //    this.StopPlay();
            //}
            //else
            //{
            //    this.StartPlay();
            //}
        }

        public void StartPlay()
        {
            this.timer1.Start();
            this.isPlaying = true;
        }

        public void StopPlay()
        {
            this.timer1.Stop();
            this.isPlaying = false;
            this.imageIndex = 0;
            if (isMe)
            {
                ChangeImage(Resources.voice_right_01);
            }
            else
            {
                ChangeImage(Resources.voice_01);
            }
        }



        public void OnMouseleave()
        {
            this.parent.Cursor = ChatBoxCursors.Arrow;
        }

        public void OnMouseMove(Point p)
        {
            if (this.AllSize.Contains(p))
            {
                this.parent.Cursor = ChatBoxCursors.Hand;
            }
            else
            {
                this.parent.Cursor = ChatBoxCursors.Arrow;
            }
        }

        public void ChangeImage(Bitmap bitmap)
        {
            recordImage = bitmap;
            this.parent.Invalidate(this.AllSize);
        }

        public void SetOffsetY(int offsetY)
        {
            this.offsetY = offsetY;
        }



    }
    #endregion


    #region InputMessageBox
    public interface IGGChatInputItem
    {
        void Draw(Graphics g, Point location);

        int Width { get; }

        string Content { get; }

        bool IsEnter { get; }

        int Height { get; }
    }

    internal class GGChatInputText : IGGChatInputItem
    {
        private DrawTextAttribute drawTextAttribute;
        private Brush brush;
        private Size size;

        public GGChatInputText(DrawTextAttribute _drawTextAttribute, Brush brush)
        {
            this.drawTextAttribute = _drawTextAttribute;
            this.brush = brush;
        }

        public string Content
        {
            get
            {
                return this.drawTextAttribute.Text;
            }
        }

        public bool IsEnter
        {
            get
            {
                return this.drawTextAttribute.Text == "\n";
            }
        }


        public int Width
        {
            get
            {
                if (this.IsEnter)
                {
                    return 0;
                }

                return TextRenderer.MeasureText(drawTextAttribute.Text, drawTextAttribute.font).Width;
            }
        }
        public int Height
        {
            get
            {
                return TextRenderer.MeasureText(drawTextAttribute.Text, drawTextAttribute.font).Height;
            }
        }
        public void Draw(Graphics g, Point pt)
        {
            size = new Size((int)drawTextAttribute.rectangleF.Width, (int)drawTextAttribute.rectangleF.Height);
            drawTextAttribute.rectangleF = new RectangleF(pt, size);
            g.DrawString(drawTextAttribute.Text, drawTextAttribute.font, drawTextAttribute.brush, pt);
        }


        public override string ToString()
        {
            return this.drawTextAttribute.Text.ToString();
        }
    }

    internal class GGChatInPutEmotion : IGGChatInputItem
    {
        private int emotionIndex;
        private Image image;
        private Rectangle rectangle;

        public int EmotionIndex
        {
            get { return this.emotionIndex; }
        }

        public bool IsEnter
        {
            get
            {
                return false;
            }
        }


        public GGChatInPutEmotion(int _emotionIndex, Image bitmap)
        {
            this.image = bitmap;
            this.emotionIndex = _emotionIndex;
        }
        public string Content
        {
            get
            {
                return null;
            }
        }
        public int Width
        {
            get
            {
                return 24;
            }
        }

        public int Height
        {
            get
            {
                return 24;
            }
        }


        public void Draw(Graphics g, Point pt)
        {
            Point pm = new Point(pt.X + 2, pt.Y - 5);
            rectangle = new Rectangle(pm, new Size(24, 24));
            g.DrawImage(image, rectangle);
        }

    }

    internal class GGChatInPutImage : IGGChatInputItem
    {
        private Image bitmap;
        private int emotionIndex;
        private Image image;
        private Rectangle rectangle;

        public Image Bitmap { get { return this.bitmap; } }

        private Size maxBitmapSize = new Size(0, 0);

        public bool IsEnter
        {
            get
            {
                return false;
            }
        }

        public GGChatInPutImage(Image bitmap)
        {
            this.bitmap = bitmap;
        }
        public string Content
        {
            get
            {
                return null;
            }
        }
        public int Width
        {
            get
            {
                return (int)this.maxBitmapSize.Width;
            }
        }

        public int Height
        {
            get
            {
                return (int)this.maxBitmapSize.Height;
            }
        }

        public void Draw(Graphics g, Point pt)
        {
            Point pm = new Point(pt.X, pt.Y - 5);
            rectangle = new Rectangle(pm, new Size(bitmap.Width, bitmap.Height));
            image = bitmap;
            g.DrawImage(image, rectangle);
        }
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
    #endregion
}
