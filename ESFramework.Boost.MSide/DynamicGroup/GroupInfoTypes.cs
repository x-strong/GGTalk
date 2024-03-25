using System;
using System.Collections.Generic;
using ESPlus.Core;
using ESFramework.Core ;
using ESFramework;
using ESPlus.Application.CustomizeInfo;
using ESBasic;

namespace ESFramework.Boost.DynamicGroup
{
	/// <summary>
    /// GroupMessageTypeRoom ���������ص�������Ϣ���Ͷ��塣
	/// </summary>
    public class GroupInfoTypes : BaseInformationTypes
	{
        #region Ctor      
        public GroupInfoTypes() : this(-200)
        {
        }
        public GroupInfoTypes(int _startKey)
        {
            base.StartKey = _startKey;
            base.Initialize();
        } 
        #endregion   
				
        #region DestroyGroup
        private int destroyGroup = 2;
        public int DestroyGroup
        {
            get { return destroyGroup; }
            set { destroyGroup = value; }
        }
        #endregion

		#region Join
		private int join = 3 ; 
        /// <summary>
        /// ��������顣��ӦЭ��Ϊstring,utf-8,groupID���ظ�Э��Ϊint,JoinGroupResult
        /// </summary>
		public int Join
		{
			get
			{
				return this.join ;
			}
			set
			{
				this.join = value ;
			}
		}
		#endregion        

        #region QuitGroup
        private int quitGroup = 4 ; 
        /// <summary>
        /// �����˳��顣��ӦЭ��Ϊstring,utf-8,groupID��û�лظ���
        /// </summary>
        public int QuitGroup
		{
			get
			{
				return this.quitGroup ;
			}
			set
			{
				this.quitGroup = value ;
			}
		}
		#endregion				

		#region SomeoneJoinGroupNotify
        private int someoneJoinGroupNotify = 5;
        /// <summary>
        /// ������֪ͨ�³�Ա�����顣Server=>Client����ӦЭ��ΪGroupNotifyContract
        /// </summary>
        public int SomeoneJoinGroupNotify
		{
			get
			{
                return this.someoneJoinGroupNotify;
			}
			set
			{
                this.someoneJoinGroupNotify = value;
			}
		}
		#endregion

        #region SomeoneQuitGroupNotify
        private int someoneQuitGroupNotify = 6;
        /// <summary>
        /// ������֪ͨĳ��Ա�˳��顣Server=>Client����ӦЭ��ΪGroupNotifyContract
        /// </summary>
        public int SomeoneQuitGroupNotify
		{
			get
			{
                return this.someoneQuitGroupNotify;
			}
			set
			{
                this.someoneQuitGroupNotify = value;
			}
		}
		#endregion               

        #region GroupmateOfflineNotify
        private int groupmateOfflineNotify = 8;
        /// <summary>
        /// ������֪ͨĳ��Ա���ߣ�S->C������ӦЭ��ΪUserContract��
        /// </summary>
        public int GroupmateOfflineNotify
        {
            get { return groupmateOfflineNotify; }
            set { groupmateOfflineNotify = value; }
        }
        #endregion	      

        #region GetGroupMembers
        private int getGroupMembers = 10;
        public int GetGroupMembers
        {
            get { return getGroupMembers; }
            set { getGroupMembers = value; }
        } 
        #endregion

        #region Broadcast
        private int broadcast = 11;
        public int Broadcast
        {
            get { return broadcast; }
            set { broadcast = value; }
        } 
        #endregion

        #region BroadcastByServer
        private int broadcastByServer = 12;
        public int BroadcastByServer
        {
            get { return broadcastByServer; }
            set { broadcastByServer = value; }
        }
        #endregion

        #region Recruit
        private int recruit = 13;
        public int Recruit
        {
            get { return recruit; }
            set { recruit = value; }
        }
        #endregion

        #region Fire
        private int fire = 14;
        public int Fire
        {
            get { return fire; }
            set { fire = value; }
        }
        #endregion

        #region BroadcastBlob
        private int broadcastBlob = 15;
        /// <summary>
        /// �㲥�����ݿ���Ϣ��Э��ΪBlobFragmentContract��
        /// </summary>
        public int BroadcastBlob
        {
            get { return broadcastBlob; }
            set { broadcastBlob = value; }
        } 
        #endregion

        #region BroadcastBlobByServer
        private int broadcastBlobByServer = 16;
        /// <summary>
        /// �㲥�����ݿ���Ϣ��Э��ΪBlobFragmentContract��
        /// </summary>
        public int BroadcastBlobByServer
        {
            get { return broadcastBlobByServer; }
            set { broadcastBlobByServer = value; }
        }
        #endregion     
  
        #region P2PChannelOpen
        private int p2PChannelOpen = 17;
        /// <summary>
        /// ���ͻ���P2Pͨ�������ɹ�ʱ��֪ͨ��������C->S����Ϣ��ΪP2PChannelReportContract��
        /// </summary>
        public int P2PChannelOpen
        {
            get { return p2PChannelOpen; }
            set { p2PChannelOpen = value; }
        }
        #endregion

        #region P2PChannelClose
        private int p2PChannelClose = 18;
        /// <summary>
        /// ���ͻ���P2Pͨ���ر�ʱ��֪ͨ��������C->S����Ϣ��ΪP2PChannelReportContract��
        /// </summary>
        public int P2PChannelClose
        {
            get { return p2PChannelClose; }
            set { p2PChannelClose = value; }
        }
        #endregion
	}   
}
