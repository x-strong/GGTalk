using CPF.Controls;
using CPF.Drawing;
using CPF.Input;
using ESFramework.Extensions.ChatRendering;
using GGTalk.Linux.Helpers;
using System;

namespace ESFramework.Boost.NetCore.Controls.ChatBox
{
    [CPF.Design.DesignerLoadStyle("res://$safeprojectname$/Stylesheet1.css")]//用于设计的时候加载样式
    public class InnerChatBox : Control, IChatControl
    {

        /// <summary>
        /// 聊天列表渲染器  （添加记录到列表显示、以及列表触发的事件 等）
        /// </summary>
        private IChatRender chatRender;

        internal IChatRender ChatRender
        {
            get
            {
                return chatRender;
            }
        }

        internal ChatBox Parent;


        #region IChatControl

        System.Drawing.Size IChatControl.RenderSize
        {
            get
            {
                return new System.Drawing.Size((int)Parent?.ActualSize.Width, (int)Parent?.ActualSize.Height);
            }
        }

        public event Action<System.Drawing.Point> WhenMouseMove;
        public event Action<System.Drawing.Point> WhenMouseLeave;
        public event Action<bool, System.Drawing.Point> WhenMouseDown;
        public event Action<bool, System.Drawing.Point> WhenMouseUp;
        public event Action<System.Drawing.Size> RenderSurfaceSizeChanged;
        public event Action<int> RenderSurfaceYScrollChanged;
        public event Action<bool> VisiableChanged;

        public void InvalidateSurface()
        {
            UiSafeInvoker.ActionOnUI(this.Invalidate);
        }

        public void SetRenderTotalHeight(int totalHeight)
        {
            UiSafeInvoker.ActionOnUI(() => {
                this.Height = totalHeight;
            });
        }

        #endregion         

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="chatRender">聊天消息渲染器</param>
        public void Initialize(IChatRender chatRender)
        {
            this.chatRender = chatRender;
        }

        public void ChangeRenderSurfaceSize()
        {
            if (this.RenderSurfaceSizeChanged != null)
            {
                this.RenderSurfaceSizeChanged(new System.Drawing.Size((int)Parent?.ActualSize.Width, (int)Parent?.ActualSize.Height));
            }
        }

        public void ChangeSurfaceScrollOffsetY(int offsetY)
        {
            if (this.RenderSurfaceYScrollChanged != null)
            {
                this.RenderSurfaceYScrollChanged(offsetY);
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            this.chatRender.Render(drawingContext);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.WhenMouseMove != null)
            {
                Point pt = e.Location;
                this.WhenMouseMove(new System.Drawing.Point((int)pt.X, (int)pt.Y));
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (this.WhenMouseLeave != null)
            {
                Point pt = e.Location;
                this.WhenMouseLeave(new System.Drawing.Point((int)pt.X, (int)pt.Y));
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            this.OnMouseDown2(e);
        }

        internal void OnMouseDown2(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
            {
                this.Focus();
                Point pt = e.Location;
                if (pt.X < 0 || pt.Y < 0)
                {
                    return;
                }
                if (this.WhenMouseDown != null)
                {
                    this.WhenMouseDown(e.LeftButton == MouseButtonState.Pressed, new System.Drawing.Point((int)pt.X, (int)pt.Y));
                }
            }
            e.Handled = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (this.WhenMouseUp != null)
            {
                Point pt = e.Location;
                if (pt.X < 0 || pt.Y < 0)
                {
                    return;
                }
                this.WhenMouseUp(e.MouseButton == MouseButton.Left, new System.Drawing.Point((int)pt.X, (int)pt.Y));
            }
        }


        /// <summary>
        /// 刷新界面
        /// </summary>
        public void RefreshUI()
        {
            this.ChangeRenderSurfaceSize();
            this.ChangeSurfaceScrollOffsetY((int)Parent.ScrollViewer.VerticalOffset);
            this.InvalidateSurface();
        }
    }
}
