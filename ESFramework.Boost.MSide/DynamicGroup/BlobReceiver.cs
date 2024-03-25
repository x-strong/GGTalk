using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.ObjectManagement.Managers;

namespace ESFramework.Boost.DynamicGroup
{
    /// <summary>
    /// 用于接收Blob的片段，并将其拼接为一个完整的Blob。
    /// </summary>
    internal class BlobReceiver
    {
        private ObjectManager<string, Blob> blobManager = new ObjectManager<string ,Blob>(); //SourceUserID - blobID - Blob

        private string ConstructID(string sourceID, int blobID)
        {
            return string.Format("{0}#{1}" ,sourceID ,blobID);
        }
        public Information Receive(string sourceID, string destID ,BlobFragmentContract fragment)
        {
            if (fragment.BlobID == 1 && fragment.FragmentIndex == 0 ) //可能是掉线重新登录过
            {
                this.OnUserOffline(sourceID);
            }

            if (fragment.FragmentIndex == 0 && fragment.IsLast)
            {
                return new Information(sourceID, destID, fragment.InformationType, fragment.Fragment);
            }

            string id = this.ConstructID(sourceID, fragment.BlobID);
            Blob blob = this.blobManager.Get(id);                        
            if (blob == null)
            {
                blob = new Blob(fragment.BlobID, sourceID, destID, fragment.InformationType);
                this.blobManager.Add(id, blob);
            }

            Information info = blob.AddFragment(fragment);
            if (info != null)
            {
                this.blobManager.Remove(id);
            }
            return info;
        }

        public void OnUserOffline(string userID)
        {
            this.blobManager.RemoveByPredication(delegate(Blob blob) { return blob.SourceID == userID; });
        }
    }

    internal class Blob
    {
        private ObjectManager<int, BlobFragmentContract> fragmentDictionary = new ObjectManager<int, BlobFragmentContract>(); //fragment Index - BlobFragmentContract
        private int lastFragmentIndex = -1 ;

        public Blob(int _blobID, string _sourceID, string _destID, int _informationType)
        {
            this.blobID = _blobID;
            this.sourceID = _sourceID;
            this.destID = _destID;
            this.informationType = _informationType;
        }

        #region BlobID
        private int blobID = 0;
        /// <summary>
        /// 大数据块唯一编号。从1开始逐个递增。
        /// </summary>
        public int BlobID
        {
            get { return blobID; }
            set { blobID = value; }
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
        public int InformationType
        {
            get { return informationType; }
            set { informationType = value; }
        } 
        #endregion

        private object locker = new object();

        public Information AddFragment(BlobFragmentContract fragment)
        {
            lock (this.locker)
            {
                if (fragment.IsLast)
                {
                    this.lastFragmentIndex = fragment.FragmentIndex;
                }

                this.fragmentDictionary.Add(fragment.FragmentIndex, fragment);
                return this.ConstructBlob();
            }
        }

        private Information ConstructBlob()
        {
            if (this.lastFragmentIndex < 0)
            {
                return null;
            }

            int totalSize = 0;
            for (int i = 0; i <= this.lastFragmentIndex; i++)
            {
                BlobFragmentContract fragment = this.fragmentDictionary.Get(i) ;
                if (fragment == null)
                {
                    return null;
                }

                totalSize += fragment.Fragment.Length;
            }

            byte[] blob = new byte[totalSize];
            int offset = 0;
            for (int i = 0; i <= this.lastFragmentIndex; i++)
            {
                BlobFragmentContract fragment = this.fragmentDictionary.Get(i);
                Buffer.BlockCopy(fragment.Fragment, 0, blob, offset, fragment.Fragment.Length);
                offset += fragment.Fragment.Length;
            }

            return new Information(this.sourceID, this.destID, this.informationType, blob);
        }
    }
}
