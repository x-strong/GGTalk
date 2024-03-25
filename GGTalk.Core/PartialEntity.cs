using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESBasic;

namespace OrayTalk
{
    public partial class SensitiveRecord
    {
        public String EvilTypeStr
        {
            get
            {
                return EnumDescription.GetFieldText(this.m_EvilType);
            }
        }

        public String ContentImageStr
        {
            get {
                if (this.ContentImage == null)
                {
                    return string.Empty;
                }
                return GlobalConsts.ViewPicture;
            }
        }

    }
}
