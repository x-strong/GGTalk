using System;
using System.Collections.Generic;
using System.Text;
using CCWin.Win32;
using System.Windows.Forms;
using CCWin.SkinControl;
using ESBasic;
using ESPlus.Serialization;
using System.Runtime.InteropServices;
using OMCS.Passive;
using JustLib;
using JustLib;
using JustLib.Records;
using JustLib.Controls;

namespace OrayTalk
{
    public partial class MainForm : ESPlus.Application.CustomizeInfo.IIntegratedCustomizeHandler
    {
        #region HandleInformation
        public void HandleInformation(string sourceUserID, int informationType, byte[] info)
        {
            if (!this.initialized)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbGeneric<string, int, byte[]>(this.HandleInformation), sourceUserID, informationType, info);
            }
            else
            {
                try
                {
                    if (informationType == InformationTypes.RebateNotify)
                    {
                        RebateNotifyContract notifyContract = CompactPropertySerializer.Default.Deserialize<RebateNotifyContract>(info, 0);
                        FriendChatForm form = (FriendChatForm)this.chatFormController.GetForm(notifyContract.SourceUserID);
                        form.ShowRebateInfoFromNotify(notifyContract.AutoID);
                        this.PlayAudioAsyn();
                        return;
                    }

                    if (informationType == InformationTypes.RebateCancel)
                    {
                        RebateNotifyContract notifyContract = CompactPropertySerializer.Default.Deserialize<RebateNotifyContract>(info, 0);
                        FriendChatForm form = (FriendChatForm)this.chatFormController.GetExistedForm(notifyContract.SourceUserID);
                        if (form != null)
                        {
                            form.OnRebateCancel(notifyContract.AutoID);
                            this.PlayAudioAsyn();
                        }
                        return;
                    }

                    if (informationType == InformationTypes.RebateExpired)
                    {
                        RebateExpiredNotifyContract notifyContract = CompactPropertySerializer.Default.Deserialize<RebateExpiredNotifyContract>(info, 0);
                        string destUserID = notifyContract.OutterID == this.resourceCenter.CurrentUserID ? notifyContract.InnerID : notifyContract.OutterID;

                        FriendChatForm form = (FriendChatForm)this.chatFormController.GetExistedForm(destUserID);
                        if (form != null)
                        {
                            form.OnRebateExpired(notifyContract.AutoID);
                            this.PlayAudioAsyn();
                        }
                        return;
                    }

                    if (informationType == InformationTypes.RebateCountChangedNotify)
                    {
                        RebateCountChangedContract contract = CompactPropertySerializer.Default.Deserialize<RebateCountChangedContract>(info, 0);
                        OrayUser user = this.resourceCenter.ClientGlobalCache.GetUser(contract.UserID);
                        user.RebateInCount = contract.InCount;
                        user.RebateOutCount = contract.OutCount;
                        user.Version = contract.UserVersion;
                        this.resourceCenter.ClientGlobalCache.SpringUserInfoChangedEvent(contract.UserID);
                        return;
                    }
                }
                catch (Exception ee)
                {
                    GlobalResourceManager.Logger.Log(ee, "MainForm.HandleInformation", ESBasic.Loggers.ErrorLevel.Standard);                    
                    MessageBox.Show(ee.Message);
                }
            }
        }

        #endregion

        public byte[] HandleQuery(string sourceUserID, int informationType, byte[] info)
        {
            return null;
        }

        public bool CanHandle(int informationType)
        {
            return InformationTypes.ContainsInformationType(informationType);
        }        
    }
}
