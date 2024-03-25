using System;
using System.Collections.Generic;
using System.Text;

namespace ESFramework.Boost.DynamicGroup
{
    /// <summary>
    /// 大数据块的某个片段。
    /// </summary>
    public class BlobFragmentContract
    {
        public BlobFragmentContract()
        {
        }

        public BlobFragmentContract(string _sourceUserID, int _blobID, int _informationType, int _fragmentIndex, byte[] _fragment, bool _isLast, string destUser)
        {
            this.sourceUserID = _sourceUserID;
            this.blobID = _blobID;
            this.informationType = _informationType;
            this.fragmentIndex = _fragmentIndex;
            this.fragment = _fragment;
            this.isLast = _isLast;
            this.destUserID = destUser;
        }

        #region BlobID
        private int blobID = 0;
        /// <summary>
        /// 大数据块唯一编号。从1开始计数，逐个递增。对于那些只有一个Fragment的Blob，可以将BlobID设置为-1，以减少编号的使用。
        /// </summary>
        public int BlobID
        {
            get { return blobID; }
            set { blobID = value; }
        } 
        #endregion

        #region SourceUserID
        private string sourceUserID = null;       
        public string SourceUserID
        {
            get { return sourceUserID; }
            set { sourceUserID = value; }
        } 
        #endregion

        #region FragmentIndex
        private int fragmentIndex = 0;
        /// <summary>
        /// 片段的索引。从0开始计数。
        /// </summary>
        public int FragmentIndex
        {
            get { return fragmentIndex; }
            set { fragmentIndex = value; }
        } 
        #endregion

        #region Fragment
        private byte[] fragment = null;
        /// <summary>
        /// 片段内容。
        /// </summary>
        public byte[] Fragment
        {
            get { return fragment; }
            set { fragment = value; }
        } 
        #endregion

        #region InformationType
        private int informationType = 0;       
        public int InformationType
        {
            get { return informationType; }
            set { informationType = value; }
        }
        #endregion

        #region IsLast
        private bool isLast = false;
        /// <summary>
        /// 是否为最后一个片段。
        /// </summary>
        public bool IsLast
        {
            get { return isLast; }
            set { isLast = value; }
        } 
        #endregion

        #region DestUserID
        private string destUserID = "";
        public string DestUserID
        {
            get { return destUserID; }
            set { destUserID = value; }
        } 
        #endregion
    }
}
