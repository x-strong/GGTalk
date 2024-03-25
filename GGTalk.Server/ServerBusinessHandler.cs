using ESFramework;
using ESPlus.Application.CustomizeInfo;
using ESPlus.Serialization;
using System;
using System.Text;
using TalkBase;
using TalkBase.Server;

namespace GGTalk.Server
{
    /// <summary>
    /// 扩展傲瑞通功能时，服务端自定义消息的处理器。
    /// </summary>
    class ServerBusinessHandler : IIntegratedCustomizeHandler
    {
        private ResourceCenter<GGUser, GGGroup> resourceCenter;
        private IDBPersisterExtend dBPersister;
        private GGService ggService;
        public ServerBusinessHandler(ResourceCenter<GGUser, GGGroup> center, IDBPersisterExtend dBPersister)
        {
            this.resourceCenter = center;
            this.dBPersister = dBPersister;
            this.ggService = new GGService(center);
        }

        public bool CanHandle(int informationType)
        {
            return BusinessInfoTypes.Contains(informationType);
        }

        public void HandleInformation(string sourceUserID, ClientType clientType, int informationType, byte[] info)
        {

        }

        public byte[] HandleQuery(string sourceUserID, ClientType clientType, int informationType, byte[] info)
        {
            if (informationType == BusinessInfoTypes.ResetPassword)
            {
                try
                {
                    ResetPasswordContract contract = CompactPropertySerializer.Default.Deserialize<ResetPasswordContract>(info, 0);
                    GGUser ggUser = this.resourceCenter.ServerGlobalCache.GetUser4Phone(contract.Phone);
                    if (ggUser == null)
                    {
                        return BitConverter.GetBytes((int)ResetPasswordResult.UserNotExist);
                    }
                    this.resourceCenter.ServerGlobalCache.ChangePassword(ggUser.UserID, contract.NewPasswordMD5);
                    return BitConverter.GetBytes((int)ResetPasswordResult.Succeed);
                }
                catch (Exception ee)
                {
                    return BitConverter.GetBytes((int)ResetPasswordResult.Error);
                }
            }
            if (informationType == BusinessInfoTypes.Register4Admin)
            {
                RegisterUserContract contract = CompactPropertySerializer.Default.Deserialize<RegisterUserContract>(info, 0);
                GGUser ggUser = new GGUser(contract.UserID, contract.PasswordMd5, contract.Name, "", "#0", "", 1, "");
                ggUser.Phone = contract.Phone;
                RegisterResult registerResult= this.ggService.Register(ggUser, false);
                return BitConverter.GetBytes((int)registerResult);
            }
            if (informationType == BusinessInfoTypes.GetUserPhone)
            {
                string userId = Encoding.UTF8.GetString(info);
                string phone = this.dBPersister.GetPhone4UserID(userId);
                return Encoding.UTF8.GetBytes(phone);
            }
            if (informationType == BusinessInfoTypes.ChangeMyPhone)
            {
                string newPhone = Encoding.UTF8.GetString(info);
                ChangePhoneResult changeMyPhoneResult = this.ChangePhone(sourceUserID, newPhone);
                return BitConverter.GetBytes((int)changeMyPhoneResult);
            }
            return null;
        }

        /// <summary>
        /// 更换手机号码
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="newPhone"></param>
        /// <returns></returns>
        private ChangePhoneResult ChangePhone(string userID, string newPhone)
        {
            GGUser user = this.dBPersister.GetUser(userID);
            if (user == null)
            {
                return ChangePhoneResult.UserNotExist;
            }
            GGUser newUser = this.dBPersister.GetUser4Phone(newPhone);
            if (newUser != null)
            {
                return ChangePhoneResult.PhoneExisted;
            }
            try
            {
                int version = user.Version + 1;
                this.dBPersister.UpdateUserPhone(userID, newPhone, version);
                return ChangePhoneResult.Succeed;
            }
            catch (Exception e)
            {
                return ChangePhoneResult.Error;
            }

        }
    }
}
