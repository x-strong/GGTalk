using TalkBase.Client;

namespace TalkBase.NetCore.Client
{
    #region ILastWordsComputer
    /// <summary>
    /// 将Unit的LastWordsRecord属性转换成string，给RecentListBox使用。
    /// </summary>
    internal interface ILastWordsComputer
    {
        string GetLastWords(IUnit unit);
    }

    /// <summary>
    /// ILastWordsComputer的默认实现。
    /// 其假定LastWordsRecord.ChatContent字段是ChatBoxContent2紧凑序列化（使用CompactPropertySerializer）之后的值。
    /// </summary>    
    internal class DefaultLastWordsComputer<TUser, TGroup> : ILastWordsComputer
        where TUser : class, TalkBase.IUser, new()
        where TGroup : TalkBase.IGroup, new()
    {
        private ResourceCenter<TUser, TGroup> resourceCenter;
        public DefaultLastWordsComputer(ResourceCenter<TUser, TGroup> center)
        {
            this.resourceCenter = center;
        }

        public string GetLastWords(IUnit unit)
        {
            if (unit == null)
            {
                return null;
            }

            if (unit.LastWordsRecord == null || unit.LastWordsRecord.ChatContent == null)
            {
                return "";
            }

            GGTalk.Linux.Controls.ChatBoxContent2 chatBoxContent = ESPlus.Serialization.CompactPropertySerializer.Default.Deserialize<GGTalk.Linux.Controls.ChatBoxContent2>(unit.LastWordsRecord.ChatContent, 0);
            string content = chatBoxContent.GetTextWithPicPlaceholder("[图]");
            TalkBase.IUser speaker = this.resourceCenter.ClientGlobalCache.GetUser(unit.LastWordsRecord.SpeakerID);
            string lastWords = null;
            if (unit.IsUser)
            {
                lastWords = string.Format("{0}： {1}", speaker.ID == this.resourceCenter.CurrentUserID ? "我" : "TA", content);
            }
            else
            {
                lastWords = string.Format("{0}： {1}", speaker.Name, content);
            }
            return lastWords;
        }
    }
    #endregion
}
