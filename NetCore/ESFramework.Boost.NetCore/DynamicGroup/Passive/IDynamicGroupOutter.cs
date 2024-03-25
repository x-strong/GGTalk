using System;
using System.Collections.Generic;
using ESFramework;
using ESFramework.Core ;
using ESFramework.Passive ;
using ESBasic;

namespace ESFramework.Boost.DynamicGroup.Passive
{
	/// <summary>
	/// ���ڿͻ��˷������������ص���Ϣ�͹㲥��Ϣ��    
	/// </summary>
    public interface IDynamicGroupOutter
	{
        /// <summary>
        /// ���³�Ա������ʱ���������¼���������GroupID - MemberID
        /// </summary>
        event CbGeneric<string, string> SomeoneJoinGroup;

        /// <summary>
        /// ����Ա�˳���ʱ���������¼���������GroupID - MemberID
        /// </summary>
        event CbGeneric<string, string> SomeoneQuitGroup;

        /// <summary>
        /// ���û����ߣ������ж�̬�����˳�ʱ���������¼���������MemberID
        /// </summary>
        event CbGeneric<string> GroupmateOffline;

        /// <summary>
        /// �����յ�ĳ�����ڵĹ㲥��Ϣ�����������ݿ���Ϣ��ʱ���������¼���������broadcasterID - groupID - broadcastType - broadcastContent��
        /// ���broadcasterIDΪnull����ʾ�Ƿ���˷��͵Ĺ㲥��
        /// </summary>
        event CbGeneric<string, string, int, byte[]> BroadcastReceived;

        /// <summary>
        /// ���͹㲥��Ϣ��
        /// </summary>        
        void Broadcast(string groupID, int broadcastType, byte[] broadcastContent, ActionTypeOnChannelIsBusy action);
        
        /// <summary>
        /// ����������ʱ���Ƿ��Զ�����P2P���ӡ�Ĭ��ֵΪtrue��
        /// </summary>
        bool TryP2PWhenGroupmateConnected { get; set; }
      
        /// <summary>
        /// �����顣
        /// </summary>  
        void JoinGroup(string groupID);

        bool Recruit(string groupID, string memberID);

        void Fire(string groupID, string memberID);

        /// <summary>
        /// �˳��顣
        /// </summary>  
        void QuitGroup(string groupID);

        void DestroyGroup(string groupID);

        /// <summary>
        /// ��ȡ���Ա�б���������л��棬��ֱ�Ӵӱ��ػ�����ȡ��
        /// </summary>        
        List<string> GetGroupMembers(string groupID);

        /// <summary>
        /// �ӷ�������ȡ���Ա�б���ʹ�����л��棬Ҳ��ѯ��������
        /// </summary>
        List<string> GetGroupMembersFromServer(string groupID);

        /// <summary>
        /// �����ڹ㲥��Ϣ��
        /// </summary>
        /// <param name="groupID">���չ㲥��Ϣ����ID</param>
        /// <param name="broadcastType">�㲥��Ϣ������</param>
        /// <param name="broadcastContent">��Ϣ������</param>
        /// <param name="broadcastChannelMode">ѡ��ͨ��ģ�͡�</param>
        /// <param name="action">��ͨ����æʱ��ȡ�Ĳ�����</param>        
        void Broadcast(string groupID, int broadcastType, byte[] broadcastContent, ActionTypeOnChannelIsBusy action, BroadcastChannelMode broadcastChannelMode);

        /// <summary>
        /// �����ڹ㲥�����ݿ���Ϣ��ֱ�����ݷ�����ϣ��÷����Ż᷵�ء�������ĳ�ʱ�����������̣߳��ɿ����첽���ñ�������
        /// </summary>
        /// <param name="groupID">���չ㲥��Ϣ����ID</param>
        /// <param name="broadcastType">�㲥��Ϣ������</param>
        /// <param name="blobContent">�����ݿ������</param>
        /// <param name="broadcastChannelMode">ѡ��ͨ��ģ�͡�</param>
        /// <param name="fragmentSize">��Ƭ����ʱ��Ƭ�εĴ�С</param>  
        void BroadcastBlob(string groupID, int broadcastType, byte[] blobContent, int fragmentSize, BroadcastChannelMode broadcastChannelMode);
	}

    /// <summary>
    /// �㲥ͨ��ѡ��ģ�͡�
    /// </summary>
    public enum BroadcastChannelMode
    {
        /// <summary>
        /// �����ĳ���û�֮���P2Pͨ�����ڣ��򵽸��û��Ĺ㲥����P2Pͨ�����ͣ����򣬾�����������ת��
        /// </summary>
        P2PChannelFirst = 0,
        /// <summary>
        /// ȫ��������������ת����ʹ���κ�P2Pͨ����
        /// </summary>
        AllTransferByServer,
        /// <summary>
        /// ����ʹ��P2Pͨ���������ĳ���û�֮���P2Pͨ�������ڣ��򲻷��͹㲥�����û���
        /// </summary>
        AllByP2PChannel
    }
}
