using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ESBasic;
using OMCS.Passive.ShortMessages;
using ESFramework;


namespace TalkBase.Client.Bridges
{
    /// <summary>
    /// 聊天窗口控制器基础接口。
    /// </summary>
    public interface IChatFormController
    {
        event CbGeneric<IChatForm, IUnit> FormCreated;
        ChatFormMode ChatFormMode { get; }
        Form GetChatRecordForm(string unitID);
        IChatForm GetForm(string unitID);
        IChatForm GetFileAssistantForm();
        Form GetNotifyForm();
        Form GetControlForm();
        Form GetAddFriendForm(string unitID);

        Form GetNewGroupVideoCallForm(string videoGroupID, string requestorID, List<string> memberIDList);
        Form GetGroupVideoCallForm(string videoGroupID);
        bool ExistGroupVideoCallForm();
        Form GetNewGroupVideoChatForm(string videoGroupID);
        Form GetGroupVideoChatForm(string videoGroupID);
        bool ExistGroupVideoChatForm();
        bool IsExistNotifyForm();
        void CloseForm(string unitID);

        /// <summary>
        /// 从缓存中移除
        /// </summary>
        /// <param name="unitID"></param>
        void RemoveForm4Cache(string unitID);
        void CloseAllForms();


        IChatForm GetExistedForm(string unitID);
        List<IChatForm> GetAllForms();
        void FocusOnForm(string unitID, bool createNew);
        void OnNewMessage(string unitID);
    }

    /// <summary>
    /// 聊天窗口显示模式
    /// </summary>
    public enum ChatFormMode
    {
        /// <summary>
        /// 独立、分散的
        /// </summary>
        Seperated = 0,
        /// <summary>
        /// 合并窗口
        /// </summary>
        Combined
    }

    /// <summary>
    /// 聊天窗口基础接口。
    /// </summary>   
    public interface IChatForm
    {
        string UnitID { get; }
        
        void MyselfOffline();
        void FriendStateChanged(string friendID, UserStatus newStatus);
        void FriendInfoChanged(IUser user);
        void UnitCommentNameChanged(IUnit unit);
        void MyInfoChanged(IUser my);
        void RefreshUI();    
        
        object Tag { get; set; }

    }

    public interface IChatFormShower
    {
        void ShowChatForm(IChatForm form);

        void ShowControl(Control control);
    }

    /// <summary>
    /// 好友聊天窗口接口。
    /// </summary>    
    public interface IFriendChatForm : IChatForm
    {
        void HandleVibration();
        void HandleInptingNotify();
        void OnRemovedFromFriend();
        void HandleFriendAddedNotify();

        /// <summary>
        /// 成功添加好友
        /// </summary>
        void AddFriendSucceed();
        void HandleChatMessage(byte[] info, DateTime? msgOccureTime);
        void HandleChatMessageOfMine(byte[] info);//自己在其它设备上发送的消息

        void HandleSnapchatMessage(SnapchatMessage snapchatMessage, DateTime? msgOccureTime);

        /// <summary>
        /// 处理收到自焚消息已阅读
        /// </summary>
        /// <param name="messageID">自焚消息ID</param>
        void HandleSnapchatRead(string messageID);

        void HandleAudioMessage(AudioMessage msg, DateTime? msgOccureTime);
        void HandleAudioMessageOfMine(AudioMessage msg);
        void HandleOfflineFileResultReceived(string fileName, bool accept);       
        void HandleMediaCommunicate(ClientType sourceClientType ,CommunicateMediaType mediaType, CommunicateType communicateType, string tag);
        void OnMediaCommunicateAnswerOnOtherDevice(ClientType answerType, CommunicateMediaType type, bool agree);
    }

    /// <summary>
    /// 文件传输助手窗体接口。
    /// </summary>
    public interface IFileAssistantForm
    {
        void HandleChatMessage(byte[] info, DateTime? msgOccureTime);
    }

    /// <summary>
    /// 群组聊天窗口接口。
    /// </summary>  
    public interface IGroupChatForm : IChatForm
    {
        void HandleBePulledIntoGroupNotify(string operatorUserID);        
        void OnGroupChanged(GroupChangedType type, string operatorID, string userID);
        void HandleGroupChatMessage(string broadcasterID, byte[] content,string tag);
        void HandleOfflineGroupChatMessage(string broadcasterID, byte[] content, DateTime msgOccureTime,string tag);
        void HandleGroupFileUploadedNotify(string sourceUserID, string groupID, string fileName);

        /// <summary>
        /// 处理已被禁言通知
        /// </summary>
        /// <param name="operatorID"></param>
        /// <param name="minutes"></param>
        void HandleGroupBanNotify(string operatorID, double minutes);

        /// <summary>
        /// 处理已被解除禁言通知
        /// </summary>
        void HandleRemoveGroupBanNotify();

        /// <summary>
        /// 处理开启全员禁言
        /// </summary>
        void HandleAllGroupBan();

        /// <summary>
        /// 处理关闭全员禁言
        /// </summary>
        void HandleRemoveAllGroupBan();
    }



    /// <summary>
    /// 好友通知窗口接口
    /// </summary>
    public interface INotifyForm
    {
        /// <summary>
        /// 切换显示页面
        /// </summary>
        /// <param name="notifyType"></param>
        void ShowTabPage(NotifyType notifyType);

        /// <summary>
        /// 收到申请添加好友请求
        /// </summary>
        /// <param name="requesterID"></param>
        /// <param name="comment"></param>
        void OnAddFriendRequestReceived(string requesterID, string comment);

        /// <summary>
        /// 收到处理的申请添加好友的结果
        /// </summary>
        /// <param name="friendID"></param>
        /// <param name="isRequester">是否为申请者</param>
        /// <param name="IsAgreed"></param>
        void OnHandleAddFriendRequestReceived(string friendID, bool isRequester, bool IsAgreed);

        /// <summary>
        /// 收到请求好友分页的结果
        /// </summary>
        /// <param name="page"></param>
        void OnAddFriendRequestPageReceived(AddFriendRequestPage page);

        /// <summary>
        /// 收到申请加入讨论组请求
        /// </summary>
        /// <param name="requesterID"></param>
        /// <param name="groupID"></param>
        /// <param name="comment"></param>
        void OnAddGroupRequestReceived(string requesterID, string groupID, string comment);

        /// <summary>
        /// 收到处理的加入讨论组申请的结果
        /// </summary>
        /// <param name="requesterID"></param>
        /// <param name="groupID"></param>
        /// <param name="IsAgreed"></param>
        void OnHandleAddGroupRequestReceived(string requesterID, string groupID, bool IsAgreed);

        /// <summary>
        /// 收到请求加入讨论组申请分页的结果
        /// </summary>
        /// <param name="page"></param>
        void OnAddGroupRequestPageReceived(AddGroupRequestPage page);
    }

    public interface IGroupVideoForm
    {
        /// <summary>
        /// 收到他人拒绝加入群聊 的处理方法
        /// </summary>
        /// <param name="rejecterID"></param>
        void OnRejectJoinReceived(string rejecterID);

        /// <summary>
        /// 收到自己在其他设备应答的消息 处理
        /// </summary>
        void OnGroupVideoAnswerOnOtherDevice();
    }


}
