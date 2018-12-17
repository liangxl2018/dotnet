using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintStudioRule
{
    /// <summary>
    /// 随机字符串产生类
    /// </summary>
    public class RandomStringHelper
    {
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="type">0:数字 1:数字加小写字母 2:数字加大写字母 3:数字加大小写 4:大写字母 5:小写字母</param>
        /// <param name="count">字符个数</param>
        /// <returns>结果</returns>
        public static string GetRandomString(int type, int count)
        {
            int number;
            string reValue = String.Empty;
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                number = random.Next();
                if (type == 0)
                {
                    number = number % 10;
                    number += 48;
                }
                else if (type == 1)
                {
                    number = number % 36;
                    if (number < 10)
                    {
                        number += 48;    //数字0-9编码在48-57 
                    }
                    else
                    {
                        number += 87;    //字母a-z编码在97-122
                    }
                }
                else if (type == 2)
                {
                    number = number % 36;
                    if (number < 10)
                    {
                        number += 48;    //数字0-9编码在48-57 
                    }
                    else
                    {
                        number += 55;    //字母A-Z编码在65-90 
                    }
                }
                else if (type == 3)
                {
                    number = number % 62;
                    if (number < 10)
                    {
                        number += 48;    //数字0-9编码在48-57 
                    }
                    else if (number < 36)
                    {
                        number += 55;    //字母A-Z编码在65-90 
                    }
                    else
                    {
                        number += 61;    //字母a-z编码在97-122
                    }
                }
                else if (type == 4)
                {
                    number = number % 26;
                    number += 65;         //字母A-Z编码在65-90 
                }
                else if (type == 4)
                {
                    number = number % 26;
                    number += 97;       //字母a-z编码在97-122
                }
                else
                {
                    number = number % 10;
                    number += 48;
                }
                reValue += ((char)number);
            }
            return reValue;
        }
    }
}
