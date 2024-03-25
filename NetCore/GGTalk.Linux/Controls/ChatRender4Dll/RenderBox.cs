using ESFramework.Extensions.ChatRendering;
using System;
using System.Drawing;

namespace GGTalk.Controls.ChatRender4Dll
{
    internal class RenderBox
    {
         
        #region Init 

        internal void Init(RenderAttr attr)
        {  
            this.chatRender = ChatRenderFactory.CreateChatRender(RenderAttr.DataProvider, attr.chatBox.ChatControl, attr.mySelfTag, attr.otherTag, attr.multiSpeaker);
            this.chatRender.AudioMessageClicked += ChatRender_AudioMessageClicked;
            this.chatRender.ImageClicked += ChatRender_ImageClicked;
            this.chatRender.SnapClicked += ChatRender_SnapClicked;
            this.chatRender.ContextMenuCalled += ChatRender_ContextMenuCalled;
        }

        #endregion

        #region ChatRender

        internal IChatRender chatRender;

        #endregion

        #region Event

        internal event Action<string,CPF.Drawing.Image> ImageMessageClicked;
        internal event Action<string, object> AudioMessageClicked;
        internal event Action<string> SnapchatMessageClicked;
        internal event Action<Point,ChatMessageType,string> ChatRender_ShowContextMenu;

        #endregion

        #region Click

        private void ChatRender_ContextMenuCalled(Point point, ChatMessageType type, string guid)
        { 
            if (this.ChatRender_ShowContextMenu != null)
                this.ChatRender_ShowContextMenu(point,type,guid);
        }

        private void ChatRender_ImageClicked(string guid, object noSourceImg)
        {
            if (this.ImageMessageClicked != null)
                this.ImageMessageClicked(guid,(CPF.Drawing.Image)noSourceImg);
        }

        private void ChatRender_AudioMessageClicked(string guid, object audioMessage)
        {
            if (this.AudioMessageClicked != null)
                this.AudioMessageClicked(guid, audioMessage);
        }

        private void ChatRender_SnapClicked(string msgID)
        {
            if (this.SnapchatMessageClicked != null)
                this.SnapchatMessageClicked(msgID);
        }

        #endregion



    }



}
