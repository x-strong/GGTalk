using CPF;
using CPF.Controls;
using CPF.Drawing;
using ESFramework.Extensions.ChatRendering;
using GGTalk.Controls.ChatRender4Dll;
using GGTalk.Linux;
using GGTalk.Linux.Controls;
using GGTalk.Linux.Helpers;
using GGTalk.Linux.Views;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace ESFramework.Boost.NetCore.Controls.ChatBox
{
    public class ChatBox : Control
    {
        private RenderBox renderBox;
        internal ChatPanel chatPanel;

        internal IChatRender ChatRender => renderBox.chatRender;

        public ScrollViewer ScrollViewer;

        private InnerChatBox innerChatBox;
        public IChatControl ChatControl => innerChatBox;


        #region Event         

        /// <summary>
        /// 当某个语音消息被单击时，触发此事件。 参数：AudioMessageID - AudioMessage
        /// </summary>
        public event ESBasic.CbGeneric<string, object> AudioMessageClicked;

        /// <summary>
        /// 当某个阅后自焚消息被单击时，触发此事件。 参数：SnapchatMessageID - SnapchatMessage
        /// </summary>
        public event ESBasic.CbGeneric<string> SnapchatMessageClicked;


        internal event Action<System.Drawing.Point, ChatMessageType, string> ChatRender_ShowContextMenu;

        #endregion

        //模板定义
        protected override void InitializeComponent()
        {
            Size = SizeField.Fill;
            Children.Add(new ScrollViewer()
            {
                Name = nameof(this.ScrollViewer),
                PresenterFor = this,
                Size = SizeField.Fill,
                BorderThickness = new Thickness(1),
                BorderFill = Color.FromRgb(231, 231, 231),
                BorderType = BorderType.BorderThickness,
                Content = new InnerChatBox
                {
                    Name = nameof(innerChatBox),
                    Size = SizeField.Fill,
                    MarginTop = 0,
                    PresenterFor = this,
                    FontSize = 14,
                    IsAntiAlias = true,
                },
            });
            SetControlEvent();
        }

        private void SetControlEvent()
        {
            this.ScrollViewer = this.FindPresenterByName<ScrollViewer>(nameof(this.ScrollViewer));
            this.innerChatBox = this.FindPresenterByName<InnerChatBox>(nameof(this.innerChatBox));
            this.innerChatBox.Parent = this;
            this.ScrollViewer.PropertyChanged += ScrollViewer_PropertyChanged;
            this.ScrollViewer.MouseDown += ScrollViewer_MouseDown;
            this.MouseWheel += ChatBox_MouseWheel;
        }

        private void ScrollViewer_MouseDown(object sender, CPF.Input.MouseButtonEventArgs e)
        {
            this.innerChatBox.OnMouseDown2(e);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="chatRender">聊天消息渲染器</param>
        public void Initialize(IChatRender chatRender)
        {
            this.innerChatBox.Initialize(chatRender);
        }

        /// <summary>
        /// 更改渲染器尺寸
        /// </summary>
        public void ChangeRenderSurfaceSize()
        {
            this.innerChatBox.ChangeRenderSurfaceSize();
        }

        /// <summary>
        /// 内容滚动到最下方
        /// </summary>
        public void ScrollToEnd()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                UiSafeInvoker.ActionOnUI(() => { ScrollViewer.VerticalOffset = ScrollViewer.ExtentHeight - ScrollViewer.ViewportHeight; });
            });
        }

        private void ChatBox_MouseWheel(object sender, CPF.Input.MouseWheelEventArgs e)
        {
            this.innerChatBox.ChangeSurfaceScrollOffsetY((int)this.ScrollViewer.VerticalOffset);
            this.innerChatBox.InvalidateSurface();
        }

        private void ScrollViewer_PropertyChanged(object sender, CPFPropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActualSize")
            {
                this.innerChatBox.ChangeRenderSurfaceSize();
                this.innerChatBox.InvalidateSurface();
                return;
            }
            if (e.PropertyName == "VerticalOffset")
            {
                this.innerChatBox.ChangeSurfaceScrollOffsetY((int)this.ScrollViewer.VerticalOffset);
                this.innerChatBox.InvalidateSurface();
                return;
            }
        }



        #region InitRenderBox


        /// <summary>
        /// 初始化ChatRender
        /// </summary>
        internal void InitChatRender(RenderAttr attr)
        {
            this.renderBox = new RenderBox();
            //this.renderBox.AudioMessageClicked += RenderBox_AudioMessageClicked;
            this.renderBox.ImageMessageClicked += RenderBox_ImageMessageClicked;
            //this.renderBox.SnapchatMessageClicked += RenderBox_SnapchatMessageClicked;
            this.renderBox.ChatRender_ShowContextMenu += RenderBox_ChatRender_ShowContextMenu;
            attr.chatBox = this;
            this.renderBox.Init(attr);
            this.Initialize(this.renderBox.chatRender);
        }

        private void RenderBox_ChatRender_ShowContextMenu(System.Drawing.Point point, ChatMessageType type, string guid)
        {
            if (ChatRender_ShowContextMenu != null)
                this.ChatRender_ShowContextMenu(point, type, guid);
        }

        private void RenderBox_SnapchatMessageClicked(string id)
        {
            if (SnapchatMessageClicked != null)
                this.SnapchatMessageClicked(id);
        }

        private void RenderBox_ImageMessageClicked(string guid,Image img)
        {
            Image image = this.chatPanel.GetImageForGuid(guid);
            PictureWindow pictureWindow = new PictureWindow(image);
            pictureWindow.Show_Topmost();
        }

        private void RenderBox_AudioMessageClicked(string AudioMessageID, object audiMesage)
        {
            if (this.AudioMessageClicked != null)
                this.AudioMessageClicked(AudioMessageID, audiMesage);
        }

        internal void RefreshUI()
        {
            this.innerChatBox.RefreshUI();
        }

        #endregion
    }
}
