using System;
using System.Collections.Generic;
using System.Text;
using TalkBase;

namespace GGTalk
{
    /// <summary>
    /// TalkBase 帮助类。
    /// </summary>
    internal class TalkBaseHelper : ITalkBaseHelper<GGGroup>
    {
        public GGGroup DoCreateGroup(string creatorID, string groupID, string groupName, string announce, List<string> members,bool isPrivate)
        {            
            string memStr = ESBasic.Helpers.StringHelper.ContactString<string>(members, ",");
            return new GGGroup(groupID, groupName, creatorID, announce, memStr,isPrivate);
        }


        public UnitType Recognize(string unitID)
        {
            if (unitID.StartsWith(FunctionOptions.PrefixGroupID))
            {
                return UnitType.Group;
            }
            return UnitType.User;
        }


        public string Key4MessageEncrypt
        {
            get { return null; }
        }


    }
}
