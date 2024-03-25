using System;
using System.Collections.Generic;
using System.Text;

namespace TalkBase.Core
{
    internal class Global
    {
        private static DateTime expiredTime = new DateTime(2021, 6, 6);
        public static bool IsExpired()
        {
           // return DateTime.Now > expiredTime;

            return false; //正式版用false
        }


        public static DateTime DateMaxValue
        {
            get {
                return new DateTime(3900, 1, 1);            
            }

        }

    }
}
