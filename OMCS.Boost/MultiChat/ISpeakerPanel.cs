using System;
using System.Collections.Generic;
using System.Text;

namespace OMCS.Boost.MultiChat
{
    /// <summary>
    /// 多人会话场景中的发言人接口。
    /// </summary>
    public interface ISpeakerPanel
    {
        string MemberID { get; }
    }
}
