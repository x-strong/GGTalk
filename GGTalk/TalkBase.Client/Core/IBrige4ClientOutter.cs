using System;
using OMCS.Passive.ShortMessages;
using ESFramework;

namespace TalkBase.Client
{
    /// <summary>
    /// ClientHandler 通过 IBrige4ClientOutter 将接收到的消息传递给IClientOutter，已触发对应的事件。
    /// </summary>
    public interface IBrige4ClientOutter
    {
        void OnVibrationReceived(string sourceUserID);
        void OnInptingNotifyReceived(string sourceUserID);
        void OnChatMessageReceived(string sourceUserID, ClientType sourceType, byte[] content, DateTime? offlineMsgOccurTime);
        void OnChatMessageEchoReceived(ClientType clientType, string destUserID, byte[] content);
        void OnSnapchatMessageReceived(string sourceUserID, ClientType sourceType, byte[] content, DateTime? offlineMsgOccurTime);

        void OnSnapchatReadReceived(string sourceUserID, SnapchatMessageRead snapchatMessage);
        void OnAudioMessageReceived(AudioMessage msg, DateTime? offlineMsgOccurTime);
        void OnAudioMessageEchoReceived(ClientType clientType, AudioMessage msg, string destUserID);
        void OnFriendAdded(string sourceUserID);
        void OnGroupChatMessageReceived(string broadcasterID, string groupID, byte[] content,string tag, DateTime? offlineMsgOccurTime);
        void OnGroupFileUploadedNotifyReceived(string sourceUserID, string groupID, string fileName);
        void OnMediaCommunicateReceived(string sourceUserID, ClientType sourceClientType, CommunicateMediaType mediaType, CommunicateType communicateType, string tag);
        void OnMediaCommunicateAnswerOnOtherDevice(string friendID ,ClientType answerType, CommunicateMediaType type,bool agree);
        void OnOfflineFileResultReceived(string sourceUserID, string fileName, bool accept);
        void OnSystemMessageReceived(SystemMessageContract msg, DateTime? offlineMsgOccurTime);

        void OnAddFriendRequestReceived(string requesterID, string comment);

        void OnHandleAddFriendRequestReceived(string friendID, bool isRequester, bool isAgreed);

        void OnAddFriendRequestPageReceived(AddFriendRequestPage page);

        void OnAddGroupRequestReceived(string requesterID,string groupID, string comment);

        void OnHandleAddGroupRequestReceived(string requesterID, string groupID, bool isAgreed);

        void OnAddGroupRequestPageReceived(AddGroupRequestPage page);

        void OnGroupBan4UserReceived(string operatorID,string groupID, double minutes);

        void OnRemoveGroupBan4UserReceived(string groupID);

        void OnGroupBan4GroupReceived(string operatorID, string groupID);

        void OnRemoveGroupBan4GroupReceived(string groupID);

    }
}
