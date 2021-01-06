using System;
using System.Collections.Generic;
using System.Text;

namespace HelperTools
{
    /*
    RandomChars.NumChar(4,RandomChars.UppLowType.random);    //返回 1eAb , 6bCD,...

    RandomChars.OnlyNum(4);     //返回 7413 , 6715,...

    RandomChars.OnlyChar(4,RandomChars.UppLowType.upper);   //返回 SFEC, AEVL,...
    */
    /// <summary>
    /// 生成字符串
    /// </summary>
    public class RandomChars
    {
        public static int StartDate = 2020;//开始日期
        public static int Step = 0;//起始索引
        public static string ChangeTimeArea(string DateStr,int Flg=0)
        {
            string[] s = DateStr.Split('-');
            int YearIndex = Convert.ToInt16(s[0]) - StartDate;
            int MonthIndex = Convert.ToInt16(s[1]) - 1;
            int DayIndex = Convert.ToInt16(s[2]) - 1;
            string Code = "";
            string[] Year = new string[]{
                "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N",
                "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
            };
            string[] Month = new string[] {
                "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M"
            };
            string[] Day = new string[] {
                "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N",
                "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1",
                "2", "3", "4", "5", "6"
            };
            switch (Flg)
            {
                case 0:
                    Code = Year[YearIndex] + Month[MonthIndex] + Day[DayIndex];
                    break;
                case 1:
                    Code = Year[YearIndex] + Month[MonthIndex] ;
                    break;
                case 2:
                    Code = Year[YearIndex] ;
                    break;
            }
            
            return Code;
        }
        /// <summary>
        /// 字符类型
        /// </summary>
        private enum CharType
        {
            /// <summary>
            /// 纯数字
            /// </summary>
            Num = 0,
            /// <summary>
            /// 纯字母
            /// </summary>
            Char = 1,
            /// <summary>
            /// 数字和字母结合
            /// </summary>
            NumChar = 2
        }
        /// <summary>
        /// 字母大小写类型
        /// </summary>
        public enum UppLowType
        {
            /// <summary>
            /// 大写
            /// </summary>
            upper = 0,
            /// <summary>
            /// 小写
            /// </summary>
            lower = 1,
            /// <summary>
            /// 随机
            /// </summary>
            random = 2
        }
        #region 生成纯随机数字
        /// <summary>
        /// 生成纯随机数字
        /// </summary>
        /// <param name="length">字符长度</param>
        /// <returns>纯随机数字</returns>
        public static string OnlyNum(int Length)
        {
            return Chars(Length, false, CharType.Num);
        }
        #endregion
        #region 生成随机字母与数字结合
        /// <summary>
        /// 生成随机字母与数字结合
        /// </summary>
        /// <param name="Length">字符长度</param>
        /// <param name="type">字母大小写类型</param>
        /// <returns>字母与数字结合的字符串</returns>
        public static string NumChar(int Length, UppLowType type)
        {
            string str = Chars(Length, false, CharType.NumChar);
            //return CaseConvert(str, type);
            return str;
        }
        #endregion

        #region 生成纯随机字母
        /// <summary>
        /// 生成纯随机字母
        /// </summary>
        /// <param name="Length">字符长度</param>
        /// <param name="type">字母大小写类型</param>
        /// <returns>纯随机字母</returns>
        public static string OnlyChar(int Length, UppLowType type)
        {
            string str = Chars(Length, true, CharType.Char);
            return CaseConvert(str, type);
            //return str;
        }
        #endregion

        #region 生成随机字符
        /// <summary>
        /// 生成随机字符
        /// </summary>
        /// <param name="Length">字符长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <param name="charType">字符类型</param>
        /// <returns>随机字符组成的字符串</returns>
        private static string Chars(int Length, bool Sleep, CharType charType)
        {
            //if (Sleep)
            //System.Threading.Thread.Sleep(1);
            int minValue = 0;
            int maxValue = 0;
            char[] chars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N',
                'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };
            switch (charType)
            {
                case CharType.Num:
                    minValue = 0;
                    maxValue = 10;
                    break;
                case CharType.Char:
                    minValue = 11;
                    maxValue = 34;
                    break;
                case CharType.NumChar:
                    minValue = 0;
                    maxValue = 34;
                    break;
                default:
                    break;
            }
            string result = "";
            Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(minValue, maxValue);
                result += chars[rnd];
            }
            return result;
        }
        #endregion

        #region 字母大小写转换
        /// <summary>
        /// 字母大小写转换
        /// </summary>
        /// <param name="chars">需要转换的字符串</param>
        /// <param name="type">字母大小写类型</param>
        /// <returns>转换后的字符串</returns>
        public  static string CaseConvert(string chars, UppLowType type)
        {
            string result = string.Empty;
            switch (type)
            {
                case UppLowType.upper:
                    result = chars.ToUpper();
                    break;
                case UppLowType.lower:
                    result = chars.ToLower();
                    break;
                case UppLowType.random:
                    string tempStr = string.Empty;
                    Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
                    for (int i = 0; i < chars.Length; i++)
                    {
                        string str = chars.Substring(i, 1);
                        tempStr += random.Next(1) == 1 ? str.ToUpper() : str.ToLower();
                    }
                    result = tempStr;
                    break;
                default:
                    break;
            }
            return result;
        }
        #endregion
    }
}
