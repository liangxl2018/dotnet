using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using PrintStudioModel;
using System.Text.RegularExpressions;

namespace PrintStudioRule
{
    public class CommonRule
    {
        /// <summary>
        /// 是否包含中文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool HasChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }

        /// <summary>
        /// 动态创建类的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="templetModels"></param>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public static T LoadClassInstance<T>(string nameSpace, string className)
        {
            Assembly assembly = Assembly.Load(nameSpace);
            T t = (T)assembly.CreateInstance(string.Format("{0}.{1}", nameSpace, className), true);
            if (t == null)
            {
                throw new Exception(string.Format("加载实例类:{0}失败.", className));
            }
            return t;
        }

        public static Type LoadClassType(string nameSpace, string className)
        {
            Assembly assembly = Assembly.Load(nameSpace);
            object type = assembly.CreateInstance(string.Format("{0}.{1}", nameSpace, className), true);
            if (type != null)
            {
                return type.GetType();
            }
            else
            {
                throw new Exception(string.Format("加载实例类型:{0}失败.", className));
            }
        }

        /// <summary>
        /// 动态创建类的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="templetModels"></param>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public static T LoadClassInstanceByActivator<T>(string fullName, string assemblyName)
        {
            Type type = Type.GetType(string.Format("{0},{1}", fullName, assemblyName));
            return (T)Activator.CreateInstance(type, true);
        }

        /// <summary>
        /// 注意:程序集一定要你在可执行路径下.
        ///  LoadClassTypeByActivator("System.Windows.Controls.TextBox","PresentationFramework");
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static Type LoadClassTypeByActivator(string fullName, string assemblyName)
        {
            return Type.GetType(string.Format("{0},{1}", fullName, assemblyName));
        }

        /// <summary>
        /// 解析indexs
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static List<int> ParseIndexs(string info)
        {
            List<int> indexs = new List<int>();
            string[] temp = info.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (temp != null && temp.Length > 0)
            {
                foreach (var item in temp)
                {
                    indexs.Add(int.Parse(item));
                }
            }
            return indexs;
        }

        /// <summary>
        /// 从Xml获取泛型节点列表 可通用所有简单类型属性.
        /// 如果有复杂类型属性,请参考WriteTempletDal.GetWriteTempletList处理方式.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static List<T> GetXmlNodeList<T>(string path, string xpath) where T : new()
        {
            T temp = default(T);
            List<T> reValue = new List<T>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            foreach (XmlNode item in xmlDoc.SelectNodes(xpath))
            {
                temp = new T();
                foreach (XmlAttribute attribute in item.Attributes)
                {
                    PropertyInfo pi = temp.GetType().GetProperty(attribute.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (pi != null && pi.CanWrite)
                    {
                        Type baseType = Nullable.GetUnderlyingType(pi.PropertyType);
                        if (baseType != null)
                        {
                            pi.SetValue(temp, attribute.Value, null);
                        }
                        else
                        {
                            pi.SetValue(temp, Convert.ChangeType(attribute.Value, pi.PropertyType), null);
                        }
                    }
                }
                reValue.Add(temp);
            }
            return reValue;
        }

        /// <summary>
        /// 获取属性默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefaultPropertyValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// 根据属性名设置属性值
        /// </summary>
        /// <param name="t"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(object objectType, string propertyName, object value)
        {
            PropertyInfo pi = objectType.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (pi == null)
            {
                throw new Exception(string.Format("未在类型{0}中找到公共属性{1}.", objectType.GetType().Name, propertyName));
            }
            Type baseType = Nullable.GetUnderlyingType(pi.PropertyType);
            if (baseType != null)
            {
                pi.SetValue(objectType, value, null);
            }
            else
            {
                pi.SetValue(objectType, Convert.ChangeType(value, pi.PropertyType), null);
            }
        }

        /// <summary>
        /// 根据属性名设置属性值
        /// </summary>
        /// <param name="t"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static object GetPropertyValue(object obj, string propertyName)
        {
            PropertyInfo pi = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (pi == null)
            {
                throw new Exception(string.Format("未在{0}中找到公共属性{1}.", obj.GetType().Name, propertyName));
            }
            return pi.GetValue(obj, null);
        }

        /// <summary>
        /// 获取本机mac地址
        /// </summary>
        /// <returns></returns>
        public static string GetHostMacAddress()
        {
            try
            {
                string mac = "未知MAC";
                List<string> macs = new List<string>();
                NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface item in adapters)
                {
                    macs.Add(item.GetPhysicalAddress().ToString());
                }
                foreach (var item in macs)
                {
                    string temp = MacChange(item);
                    if (MacCheck(temp))
                    {
                        mac = temp;
                    }
                }
                return mac;
            }
            catch
            {
                return "未知MAC";
            }
        }

        /// <summary>
        ///获取主机IP地址
        /// </summary>
        public static string GetHostIpAddress()
        {
            try
            {
                string reValue = "未知IP";
                string hostName = Dns.GetHostName();
                IPHostEntry ips = Dns.GetHostEntry(hostName);
                foreach (IPAddress _ipaddress in ips.AddressList)
                {
                    if (_ipaddress.AddressFamily.ToString().ToLower() == "internetwork")
                    {
                        reValue = _ipaddress.ToString();
                        break;
                    }
                }
                return reValue;
            }
            catch
            {
                return "未知IP";
            }
        }

        /// <summary>
        ///获取主机IP地址
        /// </summary>
        public static string GetHostIpAddress(string subIp)
        {
            try
            {
                string reValue = "未知IP";
                string hostName = Dns.GetHostName();
                IPHostEntry ips = Dns.GetHostEntry(hostName);
                foreach (IPAddress _ipaddress in ips.AddressList)
                {
                    if (_ipaddress.AddressFamily.ToString().ToLower() == "internetwork")
                    {
                        reValue = _ipaddress.ToString();
                        if (reValue.IndexOf(subIp) > -1)
                        {
                            break;
                        }
                    }
                }
                return reValue;
            }
            catch
            {
                return "未知IP";
            }
        }

        /// <summary>
        ///获取主机IP地址
        /// </summary>
        public static List<string> GetHostIpAddressList()
        {
            try
            {
                List<string> reValue = new List<string>();
                string hostName = Dns.GetHostName();
                IPHostEntry ips = Dns.GetHostEntry(hostName);
                foreach (IPAddress _ipaddress in ips.AddressList)
                {
                    if (_ipaddress.AddressFamily.ToString().ToLower() == "internetwork")
                    {
                        reValue.Add(_ipaddress.ToString());
                    }
                }
                return reValue;
            }
            catch
            {
                return null;
            }
        }
        public static bool IpCheck(string ip)
        {
            try
            {
                if (string.IsNullOrEmpty(ip))
                {
                    return false;
                }
                else
                {
                    string num = "(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)";
                    return System.Text.RegularExpressions.Regex.IsMatch(ip, ("^" + num + "\\." + num + "\\." + num + "\\." + num + "$"));
                }
            }
            catch
            {
                return false;
            }
        }

        public static string MacChange(string mac)
        {
            try
            {
                string reValulue = string.Empty;
                if (mac.Length == 12)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (i == 5)
                        {
                            reValulue += mac.Substring(i * 2, 2);
                        }
                        else
                        {
                            reValulue += mac.Substring(i * 2, 2) + ":";
                        }
                    }
                }
                else
                {
                    return mac;
                }
                return reValulue;
            }
            catch
            {
                return mac;
            }
        }

        public static bool PingState(string ipaddress)
        {
            int ix = 0;
            bool IsOnLine = false;
            while (ix < 3 && IsOnLine == false)
            {
                Task<PingReply> ping_result = PingAsync(ipaddress);
                if (ping_result.Result != null)
                {
                    if (ping_result.Result.Status == IPStatus.Success)
                    {
                        IsOnLine = true;
                        break;
                    }
                }
                ix++;
            }
            return IsOnLine;
        }

        public static Task<PingReply> PingAsync(string address)
        {
            var tcs = new TaskCompletionSource<PingReply>();
            try
            {
                Ping ping = new Ping();
                ping.PingCompleted += (obj, sender) =>
                {
                    tcs.SetResult(sender.Reply);
                };
                ping.SendAsync(address, 3, new object());
            }
            catch
            {
                tcs.SetResult(null);
            }
            return tcs.Task;
        }

        public static bool MacCheck(string mac)
        {
            try
            {
                if (string.IsNullOrEmpty(mac))
                {
                    return false;
                }
                else
                {
                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^([0-9a-fA-F]{2})(([:-][0-9a-fA-F]{2}){5})$");
                    return regex.IsMatch(mac);
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///1. 预置一个 16 位的寄存器为全1（即十六进制FFFFH），称此寄存器为CRC 寄存器；
        ///2. 把第一个 8 位数据与CRC 寄存器的低8 位相异或，结果放回CRC 寄存器；
        ///3. 把 16 位CRC 寄存器右移一位，用0 添补最高位，检测移出位：
        ///4. 如果移出位为 0，则重复第3 步骤（再次移出）；如果移出位为1，则CRC 寄存器与多项式A001H 相异
        ///或，结果放回CRC 寄存器；
        ///5. 重复第 3、4 步骤，直至移出8 位；
        ///6. 将下一个 8 位数据与CRC 寄存器低8 位相异或，结果放回CRC 寄存器，重复第2、3、4、5 步骤；
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] CRC16(byte[] buf, int length)
        {
            try
            {
                UInt16 crc = 0xFFFF;
                for (int i = 0; i < length; i++)
                {
                    UInt16 c = (UInt16)(buf[i] & 0x00FF);
                    //特别注意:c的高字节为00,与crc高字节异或结果是crc高字节本身.
                    crc ^= c;
                    for (int j = 0; j < 8; j++)
                    {
                        //crc最低位为1,与0xA001异或.
                        if ((crc & 0x0001) == 1)
                        {
                            crc >>= 1;
                            crc ^= 0xA001;
                        }
                        else
                        {
                            crc >>= 1;
                        }
                    }
                }
                return new byte[] { (byte)(crc & 0x00ff), (byte)(crc >> 8) };
            }
            catch
            {
                return new byte[] { 0, 0 };
            }
        }

        /// <summary>
        /// CRC8
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte CRC8(byte[] buffer, int startIndex = 0, int length = 0)
        {
            if (length == 0) length = buffer.Length - startIndex;
            var endIndex = startIndex + length;
            if (startIndex < 0 || startIndex >= buffer.Length) throw new ArgumentException("startIndex小于0或者超过字节数组下标");
            if (endIndex > buffer.Length) throw new ArgumentException("startIndex + length 大于字节数组长度", "length");

            byte crc = 0;
            for (int i = startIndex; i < endIndex; i++)
            {
                crc ^= buffer[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x01) != 0)
                    {
                        crc >>= 1;
                        crc ^= 0x8c;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }
            return (byte)~crc;
        }
    }
}
