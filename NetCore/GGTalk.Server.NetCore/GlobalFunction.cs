using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGTalk.Server
{
    internal static class GlobalFunction
    {
        /// <summary>
        /// 检查该账号是否为吉祥号
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static bool IsLuckNumber_UserID(string numbers)
        {
            if (string.IsNullOrEmpty(numbers) || numbers.Length < 5)
            {
                return true;
            }
            switch (numbers.Length)
            {
                case 5:
                    return CheckLuckNumber_5(numbers);
                case 6:
                    return CheckLuckNumber_6(numbers);
                case 7:
                    return CheckLuckNumber_7(numbers);
                default:
                    break;
            }
            return false;
        }

        private static bool CheckLuckNumber_5(string numbers)
        {
            int existSameNumberCount_0 = GetCount_StrExistChar(numbers, numbers[0]);//第0位字符的重复次数
            switch (existSameNumberCount_0)
            {
                case 5:
                    //5A
                    return true;
                case 4:
                    //4A1B
                    if (numbers[0] != numbers[numbers.Length - 1])
                    {
                        return true;
                    }
                    break;
                case 3:
                    //3A2B
                    if (numbers[numbers.Length - 1] == numbers[numbers.Length - 2] && numbers[numbers.Length - 1] != numbers[0])
                    {
                        return true;
                    }
                    break;
                case 2:
                    //2B3A
                    if (numbers[0] == numbers[1] && GetCount_StrExistChar(numbers, numbers[2]) == 3)
                    {
                        return true;
                    }
                    break;
                case 1:
                    //1A4B
                    if (GetCount_StrExistChar(numbers, numbers[1]) == 4)
                    {
                        return true;
                    }
                    //ABCDE
                    return CheckConsecutiveNumbers(numbers);
                default:
                    return false;
            }
            return false;
        }


        private static bool CheckLuckNumber_6(string numbers)
        {
            int existSameNumberCount_0 = GetCount_StrExistChar(numbers, numbers[0]);//第0位字符的重复次数
            switch (existSameNumberCount_0)
            {
                case 6:
                    //6A
                    return true;
                case 5:
                    //5A1B
                    if (numbers[0] != numbers[numbers.Length - 1])
                    {
                        return true;
                    }
                    break;
                case 4:
                    //4A2B
                    if (numbers[0] != numbers[numbers.Length - 1] && numbers[numbers.Length - 1] == numbers[numbers.Length - 2])
                    {
                        return true;
                    }
                    break;
                case 3:
                    //3A3B
                    if (numbers[0] == numbers[1] && numbers[0] == numbers[2] && GetCount_StrExistChar(numbers, numbers[numbers.Length - 1]) == 3)
                    {
                        return true;
                    }
                    break;
                case 2:
                    //2A4B
                    if (numbers[0] == numbers[1] && GetCount_StrExistChar(numbers, numbers[numbers.Length - 1]) == 4)
                    {
                        return true;
                    }
                    break;
                case 1:
                    //1A5B
                    if (GetCount_StrExistChar(numbers, numbers[numbers.Length - 1]) == 5)
                    {
                        return true;
                    }
                    //ABCDEF
                    return CheckConsecutiveNumbers(numbers);
            }
            return false;
        }


        private static bool CheckLuckNumber_7(string numbers)
        {
            int existSameNumberCount_0 = GetCount_StrExistChar(numbers, numbers[0]);//第0位字符的重复次数
            switch (existSameNumberCount_0)
            {
                case 7:
                    //7A
                    return true;
                case 6:
                    //6A1B
                    if (numbers[0] != numbers[numbers.Length - 1])
                    {
                        return true;
                    }
                    break;
                case 5:
                    //5A2B
                    if (numbers[0] != numbers[numbers.Length - 1] && numbers[numbers.Length - 1] == numbers[numbers.Length - 2])
                    {
                        return true;
                    }
                    break;
                case 4:
                    //4A3B
                    if (numbers[0] != numbers[numbers.Length - 1] && numbers[numbers.Length - 1] == numbers[numbers.Length - 2] && numbers[numbers.Length - 1] == numbers[numbers.Length - 3])
                    {
                        return true;
                    }
                    break;
                case 3:
                    //3A4B
                    if (numbers[0] == numbers[1] && numbers[0] == numbers[2] && GetCount_StrExistChar(numbers, numbers[numbers.Length - 1]) == 4)
                    {
                        return true;
                    }
                    break;
                case 2:
                    //2A5B
                    if (numbers[0] == numbers[1] && GetCount_StrExistChar(numbers, numbers[numbers.Length - 1]) == 5)
                    {
                        return true;
                    }
                    break;
                case 1:
                    //1A6B
                    if (GetCount_StrExistChar(numbers, numbers[numbers.Length - 1]) == 6)
                    {
                        return true;
                    }
                    //ABCDEFG
                    return CheckConsecutiveNumbers(numbers);
            }
            return false;
        }

        /// <summary>
        /// 检查字符是否为连续的
        /// </summary>
        /// <returns></returns>
        public static bool CheckConsecutiveNumbers(string numbers)
        {
            byte tempB = (byte)numbers[0];
            //ABCDE
            for (int i = 1; i < numbers.Length; i++)
            {
                byte b = (byte)numbers[i];
                if (tempB + 1 != b)
                {
                    return false;
                }
                tempB = b;
            }
            return true;
        }

        /// <summary>
        /// 获取字符串中存在指定字符的个数
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="c">指定字符</param>
        /// <returns></returns>
        public static int GetCount_StrExistChar(string str, char c)
        {
            int existCount = 0;
            Char[] chars = str.ToCharArray();
            foreach (Char cc in chars)
            {
                if (cc == c)
                {
                    existCount++;
                }
            }
            return existCount;
        }

        /// <summary>
        /// 创建随机字符串
        /// </summary>
        /// <param name="numberFlag">是否是纯数字 （true：纯数字；false：数字加字母）</param>
        /// <param name="length">字符串长度</param>
        /// <returns></returns>
        public static string CreateRandomStr(bool numberFlag, int length)
        {
            string code = "";
            string strTable = numberFlag ? "1234567890" : "1234567890abcdefghijkmnpqrstuvwxyz";
            Random randomNum = new Random(unchecked((int)(DateTime.Now.Ticks)));
            for (int i = 0; i < length; i++)
            {
                int intR = randomNum.Next(strTable.Length);
                code += strTable[intR];
            }
            return code;
        }
    }
}
