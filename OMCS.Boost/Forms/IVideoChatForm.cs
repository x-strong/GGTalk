using System;
using ESBasic;
using OMCS.Passive;

namespace OMCS.Boost.Forms
{
    /// <summary>
    /// 与好友视频聊天窗口的接口。
    /// </summary>
    public interface IVideoChatForm
    {
        event CbGeneric<HungUpCauseType> HungUpVideo;

        event CbGeneric ConnectorDisconnected;

        void Initialize(IMultimediaManager mgr, string destLoginID);

        void OnAgree(string destLoginID);

        /// <summary>
        /// 对方拒绝。
        /// </summary>        
        void OnReject();

        void OnHungUpVideo();
    }    
}
