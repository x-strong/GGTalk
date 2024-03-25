using ESBasic;
using ESBasic.Helpers;
using ESBasic.ObjectManagement.Managers;
using ESBasic.Threading.Engines;
using ESFramework;
using ESFramework.Boost;
using ESFramework.Boost.Controls;
using ESPlus.Application.FileTransfering.Passive;
using ESPlus.FileTransceiver;
using OMCS.Passive.ShortMessages;
using GGTalk.Core;
using GGTalk.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TalkBase;
using TalkBase.Client;

namespace GGTalk.Controls
{
    public partial class ChatContentBox : UserControl, IUIAction, IChatControl
    {       
        private SenderShowType senderShowType = SenderShowType.Head;
        public SenderShowType SenderShowType { get { return this.senderShowType; } set { this.senderShowType = value; this.chatBoxCore.SenderShowType = value; } }

        private ChatBoxCore chatBoxCore = new ChatBoxCore();
        internal ChatBoxCore ChatBoxCore { get => this.chatBoxCore; }

        public ChatContentBox()
        {            
            InitializeComponent();
            this.chatBoxCore.Initialize(this, this);
            this.chatBoxCore.Width = this.Width;
            this.chatBoxCore.Height = this.Height;
            this.chatBoxCore.ContentScrollToEnd += ScrollToEnd;
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.UserPaint, true);//自行绘制            
            this.UpdateStyles();

            this.AutoSizeChanged += ChatContentBox_AutoSizeChanged;
            this.Load += ChatContentBox_Load;
            this.Scroll += ChatContentBox_Scroll;
            this.MouseWheel += ChatContentBox_MouseWheel; 
        }



        private void ChatContentBox_MouseWheel(object sender, MouseEventArgs e)
        {
            this.chatBoxCore.ScrollOffsetY = this.VerticalScroll.Value;
            this.ChatContentBox_AutoSizeChanged(null, null);
        }

        private void ChatContentBox_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll && e.OldValue != e.NewValue)
            {
                this.chatBoxCore.ScrollOffsetY = e.NewValue;
                this.ChatContentBox_AutoSizeChanged(null, null);
            }
        }

        private volatile bool firstToEnd = true;
        public void ScrollToEnd()
        {
            if (firstToEnd)
            {
                this.firstToEnd = false;

                System.Threading.Thread.Sleep(100);
                this.BeginInvoke(new Action(() => { this.ChatContentBox_AutoSizeChanged(null, null); }));
                System.Threading.Thread.Sleep(200);

                this.firstToEnd = false;
                this.BeginInvoke(new Action(this.ScrollToEnd));
                return;
            }

            this.AutoScrollMinSize = new Size(this.Width - 150, this.chatBoxCore.ContentTotalHeight);
            int max = this.AutoScrollMinSize.Height - 1 - this.Height;
            if (max <= 0) return;

            this.AutoScrollPosition = new Point(0, max);
            this.ChatContentBox_Scroll(null, new ScrollEventArgs(ScrollEventType.EndScroll, max, ScrollOrientation.VerticalScroll));
        }

        private void ChatContentBox_Load(object sender, EventArgs e)
        {            
            this.AutoScrollPosition = new Point(0, 0);
            this.HorizontalScroll.Visible = false;
        }       


        private void ChatContentBox_AutoSizeChanged(object sender, EventArgs e)
        {
            this.chatBoxCore.AutoSizeChanged();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.chatBoxCore.OnDraw(e.Graphics);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            this.chatBoxCore.OnMouseMove(e.Location);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.chatBoxCore.OnMouseDown(e.Location);
        }      

        #region IUIAction
        public void ActionOnUI(Action action)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(action);
            }
            else
            {
                action.Invoke();
            }
        }

        public void ActionOnUI<T>(Action<T> action, T t)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(action,t);
            }
            else
            {
                action.Invoke(t);
            }
        }


        public void ActionOnUI<T1, T2>(System.Action<T1, T2> action, T1 t1, T2 t2) {
            if (this.InvokeRequired)
            {
                this.Invoke(action, t1,t2);
            }
            else
            {
                action.Invoke(t1,t2);
            }
        }

        public void ActionOnUI<T1, T2, T3>(System.Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3) {
            if (this.InvokeRequired)
            {
                this.Invoke(action, t1,t2,t3);
            }
            else
            {
                action.Invoke(t1, t2, t3);
            }
        }
        #endregion

        #region IChatControl

        private ChatBoxCursors chatBoxCursors = ChatBoxCursors.Arrow;
        ChatBoxCursors IChatControl.Cursor 
        { 
            get => this.chatBoxCursors;
            set {
                this.chatBoxCursors = value;
                switch (this.chatBoxCursors)
                {
                    case ChatBoxCursors.Arrow:
                        this.Cursor = Cursors.Arrow;
                        break;
                    case ChatBoxCursors.Hand:
                        this.Cursor = Cursors.Hand;
                        break;
                    case ChatBoxCursors.Wait:
                        this.Cursor = Cursors.WaitCursor;
                        break;
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// 发送者信息
    /// </summary>
    internal class SenderInfo
    {

        /// <summary>
        /// 显示的名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 显示的头像
        /// </summary>
        public Image HeadImg { get; set; }

        /// <summary>
        /// 是否为自己
        /// </summary>
        public bool IsMe { get; set; }

        /// <summary>
        /// 发送者信息显示的类型
        /// </summary>
        public SenderShowType SenderShowType { get; set; }

    }

    /// <summary>
    /// 发送者信息显示的类型
    /// </summary>
    public enum SenderShowType
    {
        /// <summary>
        /// 只显示头像
        /// </summary>
        Head = 0,

        /// <summary>
        /// 只显示名称
        /// </summary>
        Name,

        /// <summary>
        /// 头像和名称都显示
        /// </summary>
        HeadAndName
    }
    public class Compute
    {
        /// <summary>
        /// 限制字符的最大长度 
        /// </summary>
        /// <param name="s">原字符串</param>
        /// <param name="font"></param>
        /// <param name="AllWidth">能绘制的最大长度 （单位:像素）</param>
        /// <returns></returns>
        public static string LimitText(string s, Font font, int AllWidth)
        {
            Size size = TextRenderer.MeasureText(s, font);
            string news = string.Empty;
            if (size.Width >= AllWidth)
            {
                char[] vs = s.ToCharArray();
                for (int i = 0; i < vs.Length - 1; i++)
                {
                    news += vs[i];
                    if (TextRenderer.MeasureText(news, font).Width >= AllWidth)
                    {
                        news = news.Substring(0, news.Length - 2);
                        news += "...";
                    }
                }
                return news;
            }
            return s;
        }
    }
}
