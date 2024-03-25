using System;
using System.Collections.Generic;
using System.Text;

namespace ESFramework.Boost.DynamicGroup
{
    /// <summary>
    /// 一个完整的自定义信息。
    /// </summary>
    [Serializable]
    public class Information
    {
        #region Ctor
        public Information() { }
        public Information(string _sourceID, string _destID, int _infoType, byte[] _content)
        {
            this.sourceID = _sourceID;
            this.destID = _destID;
            this.informationType = _infoType;
            this.content = _content;            
        } 
        #endregion

        #region SourceID
        private string sourceID = "";
        /// <summary>
        /// 信息的发送者。可以为UserID或者NetServer.SystemUserID。
        /// </summary>
        public string SourceID
        {
            get { return sourceID; }
            set { sourceID = value; }
        } 
        #endregion

        #region DestID
        private string destID = "";
        /// <summary>
        /// 信息的接收者。可以为UserID或者NetServer.SystemUserID或GroupID（广播消息）。
        /// </summary>
        public string DestID
        {
            get { return destID; }
            set { destID = value; }
        } 
        #endregion

        #region InformationType
        private int informationType = 0;
        /// <summary>
        /// 自定义信息类型
        /// </summary>
        public int InformationType
        {
            get { return informationType; }
            set { informationType = value; }
        } 
        #endregion

        #region Content
        private byte[] content = null;
        /// <summary>
        /// 信息的内容
        /// </summary>
        public byte[] Content
        {
            get { return content; }
            set { content = value; }
        } 
        #endregion        
    }
}
