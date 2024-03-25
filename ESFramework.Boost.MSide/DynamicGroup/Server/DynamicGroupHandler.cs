using System;
using System.Collections.Generic;
using System.Text;
using ESFramework.Server.UserManagement;
using ESFramework.Core;
using ESPlus.Core;
using ESFramework.Server;
using ESFramework;
using ESBasic;
using ESBasic.ObjectManagement;
using ESBasic.Threading.Engines;
using ESPlus.Application.CustomizeInfo;
using ESPlus.Serialization;
using ESPlus.Application.CustomizeInfo.Server;

namespace ESFramework.Boost.DynamicGroup.Server
{
    /// <summary>
    /// 与ESPlatform结合：
    /// (1)由事件（比如加入组）发生的宿主服务器（即导致事件发生的用户所在的服务器）负责通知群内成员或群友。
    /// (2)如果事件是由平台或服务端触发，同理，则由目标用户所在的服务器负责通知。
    /// </summary>
    public class DynamicGroupHandler : IIntegratedCustomizeHandler, IDynamicGroupController, IEngineActor
    {       
        private BlobReceiver blobReceiver = new BlobReceiver();
        private bool isInPlatform = false;
        private AgileCycleEngine notifyEngine;
        public event CbGeneric<string, string, int, byte[]> BroadcastReceived;
        private IUserManager userManager = null;        
        private ICustomizeController customizeController;
        private IDynamicGroupManager dynamicGroupManager;

        #region Property  
        #region GroupInfoTypes
        private GroupInfoTypes groupInfoTypes = new GroupInfoTypes();
        public GroupInfoTypes GroupInfoTypes
        {
            get { return groupInfoTypes; }
            set { groupInfoTypes = value; }
        } 
        #endregion

        #region GroupNotifyEnabled
        private bool groupNotifyEnabled = true;        
        public bool GroupNotifyEnabled
        {
            get { return groupNotifyEnabled; }
            set
            {
                groupNotifyEnabled = value;
            }
        }
        #endregion             

        #region PlatformUserManager
        private IPlatformUserManager platformUserManager;
        public IPlatformUserManager PlatformUserManager
        {
            set
            {
                platformUserManager = value;
                //this.isInPlatform = !(platformUserManager is VirtualPlatformUserManager);
            }
        }
        #endregion

        #region UseGroupNotifyThread
        private bool useGroupNotifyThread = false;
        /// <summary>
        /// 组友上下线通知是否使用单独的线程。默认值为false。
        /// 如果要开启组友通知线程，必须先开启组友通知（即必须先将GroupNotifyEnabled设置为true）。
        /// </summary>
        public bool UseGroupNotifyThread
        {
            get { return useGroupNotifyThread; }
            set
            {
                if (this.useGroupNotifyThread == value)
                {
                    return;
                }

                if (value && !this.groupNotifyEnabled)
                {
                    throw new InvalidOperationException("Can't Use Group Notify Thread because GroupNotifyEnabled is false.");
                }

                useGroupNotifyThread = value;
                if (this.useGroupNotifyThread)
                {
                    this.notifyEngine.Start();
                }
                else
                {
                    this.notifyEngine.Stop();
                }
            }
        }
        #endregion          

        #endregion

        #region Ctor
        public DynamicGroupHandler()
        {
            this.notifyEngine = new AgileCycleEngine(this);
            this.notifyEngine.DetectSpanInSecs = 0;           
        } 
        #endregion

        #region Initialize
        public void Initialize(IUserManager mgr ,ICustomizeController controller , IDynamicGroupManager groupMgr)
        {        
            this.userManager = mgr;
            this.customizeController = controller;
            this.dynamicGroupManager = groupMgr;
           
            if (this.dynamicGroupManager != null)
            {                
                this.userManager.UserDisconnected += new ESBasic.CbGeneric<string>(userManager_SomeOneDisconnected);

                this.dynamicGroupManager.SomeoneJoinGroup += new ESBasic.CbGeneric<string, string>(groupManager_SomeoneJoinGroup);
                this.dynamicGroupManager.SomeoneQuitGroup += new ESBasic.CbGeneric<string, string>(groupManager_SomeoneQuitGroup);
                this.dynamicGroupManager.GroupmateOffline += new CbGeneric<string, List<string>>(dynamicGroupManager_GroupmateOffline);
            }
        }

        void userManager_SomeOneDisconnected(string userID)
        {
            this.blobReceiver.OnUserOffline(userID);
            this.dynamicGroupManager.Offline(userID);
        } 

        void dynamicGroupManager_GroupmateOffline(string userID, List<string> groupmates)
        {
            if (!this.groupNotifyEnabled)
            {
                return;
            }

            if (this.useGroupNotifyThread)
            {
                this.notifyDisconnectedQueue.Enqueue(userID);
                return;                    
            }

            byte[] info = CompactPropertySerializer.Default.Serialize(new UserContract(userID));
            foreach (string memberID in groupmates)
            {
                if (memberID != userID)
                {
                    this.customizeController.Send(memberID, this.groupInfoTypes.GroupmateOfflineNotify, info, true, ActionTypeOnChannelIsBusy.Continue);
                }
            } 
        }

        void groupManager_SomeoneQuitGroup(string groupID, string userID)
        {
            if (!this.groupNotifyEnabled)
            {
                return;
            }

            List<string> members = this.dynamicGroupManager.GetGroupMembers(groupID);
            byte[] info = CompactPropertySerializer.Default.Serialize( new GroupNotifyContract(groupID, userID));
            foreach (string memberID in members)
            {
                if (memberID != userID)
                {
                    this.customizeController.Send(memberID,this.groupInfoTypes.SomeoneQuitGroupNotify,info,true, ActionTypeOnChannelIsBusy.Continue) ;                    
                }
            }
        }

        void groupManager_SomeoneJoinGroup(string groupID, string userID)
        {
            if (!this.groupNotifyEnabled)
            {
                return;
            }

            List<string> members = this.dynamicGroupManager.GetGroupMembers(groupID);
            byte[] info = CompactPropertySerializer.Default.Serialize(new GroupNotifyContract(groupID, userID));
            foreach (string memberID in members)
            {
                if (memberID != userID)
                {
                    this.customizeController.Send(memberID, this.groupInfoTypes.SomeoneJoinGroupNotify, info, true, ActionTypeOnChannelIsBusy.Continue);             
                }
            }
        }                     
        #endregion

        #region NotifyThread       
        private CircleQueue<string> notifyDisconnectedQueue = new CircleQueue<string>(10000);
        public bool EngineAction()
        {
            try
            {
               if (this.notifyDisconnectedQueue.Count > 0)
                {
                    string userID = this.notifyDisconnectedQueue.Dequeue();
                    this.NotifyGroupmateOffline(false, userID);
                }
                else
                {
                    System.Threading.Thread.Sleep(10);
                }
            }
            catch
            {
            }
            return true;
        }

        private void NotifyGroupmateOffline(bool post, string userID)
        {
            bool online = this.userManager.IsUserOnLine(userID);
            if (online) //与最新状态进行比较
            {
                return;
            }

            List<string> groupmates = this.dynamicGroupManager.GetGroupmates(userID);
            List<string> onlineGroupmatesOnPlatform = groupmates;
            if (this.isInPlatform)
            {
                onlineGroupmatesOnPlatform = this.platformUserManager.SelectOnlineUserFrom(groupmates);
            }           
            byte[] info = CompactPropertySerializer.Default.Serialize(new UserContract(userID));
            foreach (string memberID in groupmates)
            {
                if (memberID != userID)
                {
                    this.customizeController.Send(memberID,  this.groupInfoTypes.GroupmateOfflineNotify, info, post, ActionTypeOnChannelIsBusy.Continue);
                }
            }   
        }
        #endregion

        public bool CanProcess(int messageType)
        {
            return this.groupInfoTypes.Contains(messageType);
        }

        public void HandleInformation(string sourceUserID, ClientType clientType , int informationType, byte[] information)
        {
            if (informationType == this.groupInfoTypes.Join)
            {
                GroupContract contract = CompactPropertySerializer.Default.Deserialize<GroupContract>(information, 0);
                this.dynamicGroupManager.JoinGroup(contract.GroupID, sourceUserID);
                return;
            }

            if (informationType == this.groupInfoTypes.DestroyGroup)
            {
                GroupContract contract = CompactPropertySerializer.Default.Deserialize<GroupContract>(information, 0);
                this.dynamicGroupManager.DestroyGroup(sourceUserID, contract.GroupID);
                return;
            }

            if (informationType == this.groupInfoTypes.QuitGroup)
            {
                GroupContract contract = CompactPropertySerializer.Default.Deserialize<GroupContract>(information, 0);  
                this.dynamicGroupManager.QuitGroup(contract.GroupID, sourceUserID);                
                return ;
            }              
            
            if (informationType == this.groupInfoTypes.Fire)
            {
                RecruitOrFireContract contract = CompactPropertySerializer.Default.Deserialize<RecruitOrFireContract>(information, 0);  
                this.dynamicGroupManager.QuitGroup(contract.GroupID, contract.MemberID);
                return ;
            }
            
            if (informationType == this.groupInfoTypes.Broadcast || informationType == this.groupInfoTypes.BroadcastByServer)
            {
                bool transfer = informationType == this.groupInfoTypes.BroadcastByServer;                
                BroadcastContract contract = CompactPropertySerializer.Default.Deserialize<BroadcastContract>(information, 0);
                string groupID = contract.GroupID;
                if (this.BroadcastReceived != null)
                {                   
                    this.BroadcastReceived(sourceUserID, groupID, contract.InformationType, contract.Content);
                }

                List<string> members = this.dynamicGroupManager.GetGroupMembers(groupID);
                if (members != null)
                {
                    foreach (string memberID in members)
                    {                       
                        if (memberID != sourceUserID)
                        {                            
                            this.customizeController.Send(memberID, informationType, information, true, contract.ActionTypeOnChannelIsBusy);  
                        }
                    }
                }
                return;
            }
            if (informationType == this.groupInfoTypes.BroadcastBlob || informationType == this.groupInfoTypes.BroadcastBlobByServer)
            {
                BlobFragmentContract contract = CompactPropertySerializer.Default.Deserialize<BlobFragmentContract>(information, 0);
                if (this.BroadcastReceived != null)
                {                   
                    Information info = this.blobReceiver.Receive(sourceUserID, contract.DestUserID, contract);
                    if (info != null)
                    {
                        this.BroadcastReceived(sourceUserID, contract.DestUserID, info.InformationType, info.Content);
                    }
                }

                bool transfer = informationType == this.groupInfoTypes.BroadcastBlobByServer;
                List<string> members = this.dynamicGroupManager.GetGroupMembers(contract.DestUserID);
                if (members != null)
                {
                    foreach (string memberID in members)
                    {                      
                        if (memberID != sourceUserID)
                        {                            
                            this.customizeController.Send(memberID, informationType, information, true, ActionTypeOnChannelIsBusy.Continue);  
                        }
                    }
                }
                return ;
            }            
           
        }

        public void Broadcast(string groupID, int broadcastType, byte[] broadcastContent , ActionTypeOnChannelIsBusy action)
        {
            List<string> members = this.dynamicGroupManager.GetGroupMembers(groupID);
            if (members != null)
            {
                byte[] info = CompactPropertySerializer.Default.Serialize(new BroadcastContract(NetServer.SystemUserID , groupID, broadcastType, broadcastContent ,action));               
                foreach (string memberID in members)
                {
                    this.customizeController.Send(memberID, this.groupInfoTypes.BroadcastByServer, info, true, action);
                }
            }
        }

        public bool CanHandle(int informationType)
        {
            return this.groupInfoTypes.Contains(informationType);
        }

        public byte[] HandleQuery(string sourceUserID, ClientType clientType, int informationType, byte[] information)
        {   
            if (informationType == this.groupInfoTypes.Recruit)
            {
                RecruitOrFireContract contract = CompactPropertySerializer.Default.Deserialize<RecruitOrFireContract>(information, 0);
                if (!this.userManager.IsUserOnLine(contract.MemberID))
                {
                    return BitConverter.GetBytes(false);
                }

                this.dynamicGroupManager.JoinGroup(contract.GroupID, contract.MemberID);
                return BitConverter.GetBytes(true);
            }
            if (informationType == this.groupInfoTypes.GetGroupMembers)
            {
                GroupContract contract = CompactPropertySerializer.Default.Deserialize<GroupContract>(information, 0);
                List<string> members = this.dynamicGroupManager.GetGroupMembers(contract.GroupID);
                return CompactPropertySerializer.Default.Serialize<List<string>>(members);
            }

            return null;
        }
    }
}
