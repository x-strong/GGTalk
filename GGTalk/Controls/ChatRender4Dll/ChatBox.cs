using ESBasic.Helpers;
using ESFramework.Boost.Controls;
using ESFramework.Extensions.ChatRendering;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TalkBase;

namespace GGTalk.Controls.ChatRender4Dll
{


    public partial class ChatBox : ScrollableControl, IChatControl
    {
        #region ctor

        public ChatBox()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            this.Scroll += ChatBox_Scroll;
            this.SizeChanged += ChatBox_SizeChanged;
            this.MouseWheel += ChatBox_MouseWheel;
        }

        #endregion

        internal ChatPanel chatPanel;

        internal int offsetY => this.verticalOffset;

        private RenderBox renderBox;
         
        internal IChatRender ChatRender;

        public System.Drawing.Size RenderSize
        {
            get
            {
                return this.Size;
            }
        }
        public event Action<Point> WhenMouseMove;
        public event Action<Point> WhenMouseLeave;
        public event Action<bool, Point> WhenMouseDown;
        public event Action<bool, Point> WhenMouseUp;
        public event Action<Size> RenderSurfaceSizeChanged;
        public event Action<int> RenderSurfaceYScrollChanged;
        private int verticalOffset => this.VerticalScroll.Value;

        #region Event

        /// <summary>
        /// 当文件（夹）拖放到控件内时，触发此事件。参数：文件路径的集合。
        /// </summary>
        [Description("当文件（夹）拖放到控件内时，触发此事件。")]
        internal event ESBasic.CbGeneric<string[]> FileOrFolderDragDrop;
 
         
        /// <summary>
        /// 当某个语音消息被单击时，触发此事件。 参数：AudioMessageID - AudioMessage
        /// </summary>
        public event ESBasic.CbGeneric<string, object> AudioMessageClicked;
         
        /// <summary>
        /// 当某个阅后自焚消息被单击时，触发此事件。 参数：SnapchatMessageID - SnapchatMessage
        /// </summary>
        public event ESBasic.CbGeneric<string> SnapchatMessageClicked;


        internal event Action<Point, ChatMessageType, string> ChatRender_ShowContextMenu;
        public event Action<bool> VisiableChanged;

        #endregion


        private void ChatBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (this.RenderSurfaceYScrollChanged != null)
            {
                this.RenderSurfaceYScrollChanged(-this.AutoScrollPosition.Y);
                this.Invalidate();
            }
        }

        private void ChatBox_SizeChanged(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(30);
                GlobalResourceManager.UiSafeInvoker.ActionOnUI(() =>
                {
                    if (this.RenderSurfaceSizeChanged != null)
                    {
                        this.RenderSurfaceSizeChanged(this.Size);
                    }
                });
            });
        }

        private void ChatBox_Scroll(object sender, ScrollEventArgs e)
        {
            if (this.RenderSurfaceYScrollChanged != null)
            {
                this.RenderSurfaceYScrollChanged(-this.AutoScrollPosition.Y);   
                this.Invalidate();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="chatRender">聊天消息渲染器。</param>
        public void Initialize(IChatRender chatRender)
        {
            this.ChatRender = chatRender;
            Graphics graphics = this.CreateGraphics();
            this.ChatRender.SetComputeGraphics(graphics);
            this.Paint += ChatBox_Paint;
        }

        private void ChatBox_Paint(object sender, PaintEventArgs e)
        {
            Matrix transform = new Matrix();
            transform.Translate(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);
            e.Graphics.Transform = transform;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.ChatRender.Render(e.Graphics);            
        }

        #region IChatControl

        public void SetRenderTotalHeight(int totalHeight)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<int>(this.SetRenderTotalHeight), totalHeight);
            }
            else
            {
                this.AutoScrollMinSize = new Size(this.ClientSize.Width, totalHeight);
                this.Invalidate();
            }
        }            

        public void InvalidateSurface()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(InvalidateSurface));
            }
            else
            {
                this.Invalidate();
            }
        }
        #endregion

        public void ScrollToBottom()
        {
            this.AutoScrollPosition = new Point(this.AutoScrollPosition.X, this.AutoScrollMinSize.Height - this.Height);
            if (this.RenderSurfaceYScrollChanged != null)
            {
                this.RenderSurfaceYScrollChanged(-this.AutoScrollPosition.Y);
                this.Invalidate();
            }
        }

        public bool IsScrollAtBottom()
        {
            int maxY = this.AutoScrollMinSize.Height - this.Height;
            return maxY - (-this.AutoScrollPosition.Y) <= 5;            
        }

        #region 控件鼠标事件


        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                if (this.WhenMouseDown != null)
                {
                    Point pt = e.Location;
                    this.WhenMouseDown(e.Button == MouseButtons.Left, new Point(pt.X, pt.Y + verticalOffset));
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (this.WhenMouseUp != null)
            {
                Point pt = e.Location;
                if (pt.X < 0 || pt.Y < 0)
                {
                    return;
                }
                this.WhenMouseUp(e.Button == MouseButtons.Left, new Point(pt.X, pt.Y + verticalOffset));
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.WhenMouseMove != null)
            {
                Point pt = e.Location;
                this.WhenMouseMove(new Point(pt.X, pt.Y + verticalOffset));
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (this.WhenMouseLeave != null)
            {
                this.WhenMouseLeave(Point.Empty);
            }
        }

        #endregion


        #region OnDragDrop


        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileOrDirs = (string[])drgevent.Data.GetData(DataFormats.FileDrop);
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

        #region InitRenderBox


        /// <summary>
        /// 初始化ChatRender
        /// </summary>
        internal void InitChatRender(RenderAttr attr)
        { 
            this.renderBox = new RenderBox();
            this.renderBox.AudioMessageClicked += RenderBox_AudioMessageClicked;
            this.renderBox.ImageMessageClicked += RenderBox_ImageMessageClicked;
            this.renderBox.SnapchatMessageClicked += RenderBox_SnapchatMessageClicked;
            this.renderBox.ChatRender_ShowContextMenu += RenderBox_ChatRender_ShowContextMenu;
            attr.chatBox = this;
            this.renderBox.Init(attr);
            this.Initialize(this.renderBox.chatRender);
            this.HorizontalScroll.Visible = false;
            this.HorizontalScroll.Enabled = false;
        }

        private void RenderBox_ChatRender_ShowContextMenu(Point point, ChatMessageType type, string guid)
        {
            if (ChatRender_ShowContextMenu != null)
                this.ChatRender_ShowContextMenu(point,type,guid);
        }

        private void RenderBox_SnapchatMessageClicked(string id)
        {
            if (SnapchatMessageClicked != null)
                this.SnapchatMessageClicked(id);
        }

        private void RenderBox_ImageMessageClicked(string guid,Image img)
        {
            Image image = this.chatPanel.GetImageForGuid(guid);
            ImageForm form = new ImageForm(image);
            form.Show();
        }

        private void RenderBox_AudioMessageClicked(string AudioMessageID, object audiMesage)
        {
            if(this.AudioMessageClicked != null)
                this.AudioMessageClicked(AudioMessageID, audiMesage);
        }


        #endregion


    }
}
