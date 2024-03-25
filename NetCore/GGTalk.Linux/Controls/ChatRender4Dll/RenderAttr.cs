using ESFramework.Boost.NetCore.Controls.ChatBox;
using ESFramework.Extensions.ChatRendering;

namespace GGTalk.Controls.ChatRender4Dll
{

    /// <summary>
    /// ChatRender属性
    /// </summary>
    internal class RenderAttr
    {

        #region ctor

        internal RenderAttr(string mySelfTag, string otherTag, bool multiSpeaker, WinRenderType renderType)
        {   
            this.mySelfTag = mySelfTag;
            this.otherTag = otherTag;
            this.multiSpeaker = multiSpeaker;
            this.renderType = renderType;
        }

        #endregion

        #region IRenderDataProvider

        internal static IRenderDataProvider DataProvider = new RenderDataProvider();

        #endregion

        #region ChatBox

        internal ChatBox chatBox;

        #endregion

        #region MySelfTag

        internal string mySelfTag;

        #endregion

        #region OtherTag

        internal string otherTag;

        #endregion

        #region MultiSpeaker

        internal bool multiSpeaker;

        #endregion

        #region WinRenderType

        internal WinRenderType renderType;

        #endregion 

    }
}