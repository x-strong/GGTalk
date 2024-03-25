using System.Collections.Generic;

namespace TalkBase
{
    /// <summary>
    /// TalkBase需要类库使用者提供实现的基础接口。
    /// </summary>   
    public interface ITalkBaseHelper<TGroup> : IUnitTypeRecognizer
    {
        TGroup DoCreateGroup(string creatorID, string groupID, string groupName, string announce, List<string> members, bool isPrivate);        

        /// <summary>
        /// 用于消息3Des加密的密钥。如果返回null，表示不启用消息加密。
        /// </summary>
        string Key4MessageEncrypt { get; }
    }
}
