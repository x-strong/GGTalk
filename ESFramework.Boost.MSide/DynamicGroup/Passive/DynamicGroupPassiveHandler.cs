using System;
using System.Collections.Generic;
using System.Text;
using ESPlus.Application.CustomizeInfo.Passive;
using ESFramework.Core;
using ESPlus.Core;
using ESFramework;
using ESPlus.Application;
using ESPlus.Application.CustomizeInfo;
using ESPlus.Serialization;

namespace ESFramework.Boost.DynamicGroup.Passive
{
    public class DynamicGroupPassiveHandler : IIntegratedCustomizeHandler
    {
        private BlobReceiver blobReceiver = new BlobReceiver();
        private DynamicGroupOutter groupOutter;

        #region GroupInfoTypes
        private GroupInfoTypes groupInfoTypes = new GroupInfoTypes();
        public GroupInfoTypes GroupInfoTypes
        {
            get { return groupInfoTypes; }
            set { groupInfoTypes = value; }
        } 
        #endregion        

        public void Initialize(DynamicGroupOutter outter)
        {          
            this.groupOutter = outter;
        }
        
        public bool CanHandle(int informationType)
        {
            return this.groupInfoTypes.Contains(informationType);
        }

        public void HandleInformation(string sourceUserID, ClientType clientType, int informationType, byte[] information)
        {            
            if (informationType == this.groupInfoTypes.SomeoneJoinGroupNotify)
            {
                GroupNotifyContract contract = CompactPropertySerializer.Default.Deserialize<GroupNotifyContract>(information, 0);  
                this.groupOutter.SomeoneJoinGroupNotify(contract.GroupID, contract.MemberID);
                return ;
            }
            if (informationType == this.groupInfoTypes.SomeoneQuitGroupNotify)
            {
                GroupNotifyContract contract = CompactPropertySerializer.Default.Deserialize<GroupNotifyContract>(information, 0);  
                this.groupOutter.SomeoneQuitGroupNotify(contract.GroupID, contract.MemberID);
                return ;
            }            
            if (informationType == this.groupInfoTypes.GroupmateOfflineNotify)
            {
                UserContract contract = CompactPropertySerializer.Default.Deserialize<UserContract>(information, 0);  
                this.groupOutter.GroupmateOfflineNotify(contract.UserID);
                return ;
            }
            if (informationType == this.groupInfoTypes.Broadcast || informationType == this.groupInfoTypes.BroadcastByServer)
            {
                BroadcastContract contract = CompactPropertySerializer.Default.Deserialize<BroadcastContract>(information, 0);
                string broadcasterID = contract.BroadcasterID == NetServer.SystemUserID ? null : contract.BroadcasterID;
                this.groupOutter.OnBroadcast(broadcasterID, contract.GroupID, contract.InformationType, contract.Content);
                return ;
            }

            if (informationType == this.groupInfoTypes.BroadcastBlob || informationType == this.groupInfoTypes.BroadcastBlobByServer)
            {
                BlobFragmentContract contract = CompactPropertySerializer.Default.Deserialize<BlobFragmentContract>(information, 0);
                Information info = this.blobReceiver.Receive(contract.SourceUserID, contract.DestUserID, contract);
                if (info != null)
                {
                    string broadcasterID = contract.SourceUserID == NetServer.SystemUserID ? null : contract.SourceUserID;
                    this.groupOutter.OnBroadcastBlob(broadcasterID, info.DestID, info.InformationType, info.Content);
                }
                
            }          
        }

        public byte[] HandleQuery(string sourceUserID, ClientType clientType, int informationType, byte[] info)
        {
            return null;
        }
    }
}
