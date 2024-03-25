using System;
using System.Collections.Generic;
using System.Text;
using ESBasic;
using TalkBase;

namespace GGTalk
{
    /// <summary>
    /// 服务端用于提供Remoting服务的接口。功能有：
    /// （1）注册用户、查找用户。
    /// （2）获取最新版本号。
    /// </summary>
    public interface IGGService : IChatRecordGetter
    {
        int GetGGTalkVersion();
        RegisterResult Register(GGUser user,bool checkLuckNumber=false);
        List<GGUser> SearchUser(string idOrName);        
  
    }


}
