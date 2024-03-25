using System;
using ESBasic;
using TalkBase;

namespace GGTalk
{
    //扩展傲瑞通功能时，自定义的消息协议。

    /// <summary>
    /// 组织内系统通知的协议类。
    /// </summary>
    public class OrgSystemNotifyContract
    {
        public OrgSystemNotifyContract()
        {
        }

        public OrgSystemNotifyContract(string title, string content ,string url)
        {
            this.Title = title;
            this.Content = content;
            this.LinkUrl = url;
        }

        public string Title { get; set; }
        public string Content { get; set; }
        public string LinkUrl { get; set; }  
    }

    [Serializable]
    public class ResetPasswordContract
    {
        public ResetPasswordContract()
        { }
		//金手指为UserID,傲瑞通为Phone
        public ResetPasswordContract(string phone, string smsCode, string newPasswordMD5)
        {
            this.Phone = phone;
            this.SmsCode = smsCode;
            this.NewPasswordMD5 = newPasswordMD5;
        }

        /// <summary>
        /// 唯一键 （傲瑞通为Phone，金手指为UserID，因为金手指一个Phone可能会对应多个UserID）
        /// </summary>
        public string Phone { get; set; }
        public string SmsCode { get; set; }
        public string NewPasswordMD5 { get; set; }
    }

    [Serializable]
    public class SendSmsCodeContract
    {
        public SendSmsCodeContract() { }

        public SendSmsCodeContract(SmsCodeType type, string phone)
        {
            this.SmsCodeType = (int)type;
            this.Phone = phone;
        }

        public int SmsCodeType { get; set; }
        public string Phone { get; set; }
    }

    [Serializable]
    public class RegisterUserContract
    {
        public RegisterUserContract() { }
        public RegisterUserContract(string userid,string name,string passwordMd5,string phone)
        {
            this.UserID = userid;
            this.Name = name;
            this.PasswordMd5 = passwordMd5;
            this.Phone = phone;
        }

        public string UserID { get; set; }
        public string Name { get; set; }
        public string PasswordMd5 { get; set; }
        public string Phone { get; set; }
    }

    [Serializable]
    public class CreateGroup4AdminContract
    {
        public CreateGroup4AdminContract() { }
        public CreateGroup4AdminContract(string groupID, string groupName, string createrID)
        {
            this.GroupID = groupID;
            this.GroupName = groupName;
            this.CreaterID = createrID;
        }

        public string GroupID { get; set; }

        public string GroupName { get; set; }

        public string CreaterID { get; set; }
    }

    public class ChangePhoneContract
    {
        public ChangePhoneContract() { }

        public ChangePhoneContract(string userID, string newPhone)
        {
            this.UserID = userID;
            this.NewPhone = newPhone;
        }

        public string UserID { get; set; }
        public string NewPhone { get; set; }
    }

    [Serializable]
    public class GetSensitiveRecordPageContract
    {
        public GetSensitiveRecordPageContract() { }

        public GetSensitiveRecordPageContract(string speakerID, string audienceID, int pageSize, int pageIndex, Date startDate, Date endDate,int evilType)
        {
            this.SpeakerID = speakerID;
            this.AudienceID = audienceID;
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.EvilType = evilType;
        }

        public string SpeakerID { get; set; }
        public string AudienceID { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public Date StartDate { get; set; }

        public Date EndDate { get; set; }

        public int EvilType { get; set; }
    }
}
