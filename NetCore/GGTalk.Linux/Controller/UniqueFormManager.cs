using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkBase.Client;

namespace GGTalk
{
    /// <summary>
    /// 唯一Form管理
    /// </summary>
    internal class UniqueFormManager
    {
        private static UniqueFormManager instance;
        private UniqueFormManager() { }
        public static UniqueFormManager GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UniqueFormManager();
                }
                return instance;
            }
        }
    }
}
