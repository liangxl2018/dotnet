using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace PrintStudioRule
{
    /// <summary>
    /// 斑马打印机帮助类
    /// </summary>
    public class ZebraPrinterHelper
    {
        /// <summary>
        /// 打印机句柄
        /// </summary>
        private static IntPtr hPrinter = new IntPtr(0);

        /// <summary>
        /// 命令字符串
        /// </summary>
        private static StringBuilder lzpOrder = new StringBuilder();

        /// <summary>
        /// 文档结构体定义
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }

        /// <summary>
        /// 检取一个标识特定打印机或打印服务器的句柄并打开 
        /// </summary>
        /// <param name="szPrinter"></param>
        /// <param name="hPrinter"></param>
        /// <param name="pd"></param>
        /// <returns></returns>
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        /// <summary>
        /// 关闭给定的打印机对象
        /// </summary>
        /// <param name="hPrinter"></param>
        /// <returns></returns>
        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool ClosePrinter(IntPtr hPrinter);

        /// <summary>
        /// 通知打印机打印程序将在打印机上打印一个文档
        /// </summary>
        /// <param name="hPrinter"></param>
        /// <param name="level"></param>
        /// <param name="di"></param>
        /// <returns></returns>
        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        /// <summary>
        /// 终止给定打印机的一个打印作业
        /// </summary>
        /// <param name="hPrinter"></param>
        /// <returns></returns>
        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool EndDocPrinter(IntPtr hPrinter);

        /// <summary>
        /// 通知打印机打印程序将在给定打印机上打印一页
        /// </summary>
        /// <param name="hPrinter"></param>
        /// <returns></returns>
        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool StartPagePrinter(IntPtr hPrinter);

        /// <summary>
        /// 指示一页的结束和下一页的开始
        /// </summary>
        /// <param name="hPrinter"></param>
        /// <returns></returns>
        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool EndPagePrinter(IntPtr hPrinter);

        //通知打印机打印程序应向给定的打印机写指定的数据
        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        #region 官方打印示例
        /// <summary>
        /// SendBytesToPrinter()
        /// When the function is given a printer name and an unmanaged array</summary>
        /// of bytes, the function sends those bytes to the print queue.<param name="szPrinterName"></param>
        /// <param name="pBytes"></param>
        /// <param name="dwCount"></param>
        /// <returns></returns>
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false;
            di.pDocName = "My C#.NET RAW Document";
            di.pDataType = "RAW";
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    if (StartPagePrinter(hPrinter))
                    {
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }

        /// <summary>
        /// 打印指令字符串
        /// </summary>
        /// <param name="szPrinterName"></param>
        /// <param name="szString"></param>
        /// <returns></returns>
        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            byte[] datas = Encoding.GetEncoding("gb2312").GetBytes(szString);
            IntPtr pBytes = Marshal.AllocHGlobal(datas.Length);
            Marshal.Copy(datas, 0, pBytes, datas.Length);
            SendBytesToPrinter(szPrinterName, pBytes, datas.Length);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }
        #endregion

        /// <summary>
        /// 打开打印机
        /// </summary>
        public static void Open(string printerName)
        {
            lzpOrder = new StringBuilder();
            hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            di.pDocName = "My C#.NET RAW Document";
            di.pDataType = "RAW";
            //打开打印机
            if (!OpenPrinter(printerName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                throw new Exception(string.Format("打印机{0}打开失败.", printerName));
            }
            //开启一个文档
            if (!StartDocPrinter(hPrinter, 1, di))
            {
                throw new Exception(string.Format("打印机{0}打开失败.", printerName));
            }
            //开启一页
            if (!StartPagePrinter(hPrinter))
            {
                throw new Exception(string.Format("打印机{0}打开失败.", printerName));
            }
            else
            {
                lzpOrder.Append("^XA");
            }
        }

        /// <summary>
        /// 打印、关闭打印机 
        /// </summary>
        /// <returns></returns>
        public static void PTK_PrintLabel(int count)
        {
            Int32 dwWritten = 0;
            if (hPrinter != null)
            {
                lzpOrder.Append("^XZ");
                byte[] datas = Encoding.GetEncoding("gb2312").GetBytes(lzpOrder.ToString());
                IntPtr pBytes = Marshal.AllocHGlobal(datas.Length);
                Marshal.Copy(datas, 0, pBytes, datas.Length);
                while (count > 0)
                {
                    WritePrinter(hPrinter, pBytes, datas.Length, out dwWritten);
                    count--;
                }
                Marshal.FreeCoTaskMem(pBytes);
                EndPagePrinter(hPrinter);
                EndDocPrinter(hPrinter);
                ClosePrinter(hPrinter);
            }
        }

        /// <summary>
        /// 打印字符串
        /// 将字体Z替换成MSUNG，设置之后字体A就代表MSUNG,而不是先前A代表的字体.后续中便可以使用A来代表MSUNG字体.
        /// </summary>
        /// <param name="type">本方法支持ZLP打印内置打印类型 Z:中英混合</param>
        /// <param name="heigth">字符高度(宽度)</param>
        /// <param name="width">字符长度</param>
        /// <param name="x">X轴</param>
        /// <param name="y">Y轴</param>
        /// <param name="data">数据</param>
        public static void PrintString(string type, int heigth, int width, int x, int y, string data)
        {
            if (data.Contains("~"))
            {
                lzpOrder.Append("^CT︴");
            }
            if (lzpOrder.ToString().Contains("^SEE:GB.DAT^CWZ,E:SIMSUN.TTF^FS^CWY,E:ARIAL.TTF^CI26"))
            {
                lzpOrder.Append(string.Format("^CI26^A{0},N,{1},{2}^FO{3},{4}^FD{5}^FS", type, heigth, width, x, y, data));
            }
            else
            {
                lzpOrder.Append(string.Format("^SEE:GB.DAT^CWZ,E:SIMSUN.TTF^FS^CWY,E:ARIAL.TTF^CI26^A{0},N,{1},{2}^FO{3},{4}^FD{5}^FS", type, heigth, width, x, y, data));
            }
            if (data.Contains("~"))
            {
                lzpOrder.Append("^CT~");
            }
        }

        /// <summary>
        /// Code128一维码
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="byw">宽窄比</param>
        /// <param name="h">高度(宽度)</param>
        /// <param name="data">数据</param>
        public static void PrintCode128(int x, int y, int byw, int h, string data)
        {
            lzpOrder.Append(string.Format("^FO{0},{1}^BY{2}^BCN,{3},N,N,N^FD{4}^FS", x, y, byw, h, data));
        }

        /// <summary>
        /// QR二维码
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="c">放大倍数</param>
        /// <param name="d">H = 极高可靠性级别 Q = 高可靠性级别 M = 标准级别 L = 高密度级别
        /// <param name="data">数据</param>
        public static void PrintQRCode(int x, int y, int c, string d, string data)
        {
            lzpOrder.Append(string.Format("^CI26^FO{0},{1}^BQN,2,{2},{3},7^FD{3}A,{4}^FS", x, y, c, d, data));
        }

        /// <summary>
        /// DataMatrix二维码
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="h">单个元素高度</param>
        /// <param name="a">1:正方形 2:矩形
        /// <param name="data">数据</param>
        public static void PrintDataMatrix(int x, int y, int h, int a, string data)
        {
            lzpOrder.Append(string.Format("^CI26^FO{0},{1}^BXN,{2},200,,,,,{3}^FD{4}^FS", x, y, h, a, data));
        }

        /// <summary>
        ///  矩形框打印
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="w">长</param>
        /// <param name="h">高度(宽度)</param>
        /// <param name="t">粗细</param>
        public static void PrintRectangle(int x, int y, int w, int h, int t)
        {
            lzpOrder.Append(string.Format("^FO{0},{1}^GB{2},{3},{4}^FS", x, y, w, h, t));
        }

        /// <summary>
        /// 打印水平直线
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="w">长度</param>
        /// <param name="t">粗细</param>
        public static void PrintLine(int x, int y, int w, int t)
        {
            lzpOrder.Append(string.Format("^FO{0},{1}^GB{2},0,{3}^FS", x, y, w, t));
        }

        /// <summary>
        /// Code128 C一维码
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="byw">宽窄比</param>
        /// <param name="h">高度(宽度)</param>
        /// <param name="data">数据</param>
        public static void PrintCode128C(int x, int y, int byw, int h, string data)
        {
            bool hasCodeC = false;
            bool hasCodeB = false;
            char first, second;
            string temp = string.Empty;
            string lastCode = string.Empty;
            if (data[0] >= 48 & data[0] <= 57)
            {
                temp += ">;";
                hasCodeC = true;
                lastCode = "CodeC";
            }
            else
            {
                temp += ">:";
                hasCodeB = true;
                lastCode = "CodeB";
            }
            for (int i = 0; i < data.Length; i++)
            {
                first = data[i++];
                if (i == data.Length)
                {
                    if (data.Length == 1)
                    {
                        temp = string.Format(">:{0}", first);
                        continue;
                    }
                    if (lastCode.Equals("CodeB") || data.Length == 1)
                    {
                        temp += first;
                    }
                    else
                    {
                        temp += string.Format(">6{0}", first);
                    }
                }
                else
                {
                    second = data[i];
                    if ((first >= 48 & first <= 57) & (second >= 48 & second <= 57))
                    {
                        hasCodeB = false;
                        if (hasCodeC)
                        {
                            temp += string.Format("{0}{1}", first, second);
                        }
                        else
                        {
                            temp += string.Format(">5{0}{1}", first, second);
                            hasCodeC = true;
                        }
                        lastCode = "CodeC";
                    }
                    else
                    {
                        hasCodeC = false;
                        lastCode = "CodeB";
                        if ((first < 48 | first > 57) & (second < 48 | second > 57))
                        {
                            if (hasCodeB)
                            {
                                temp += string.Format("{0}{1}", first, second);
                            }
                            else
                            {
                                temp += string.Format(">6{0}{1}", first, second);
                                hasCodeB = true;
                            }
                        }
                        else
                        {
                            if (hasCodeB)
                            {
                                temp += string.Format("{0}", first);
                            }
                            else
                            {
                                temp += string.Format(">6{0}", first);
                                hasCodeB = true;
                            }
                            i--;
                        }
                    }
                }
            }
            lzpOrder.Append(string.Format("^FO{0},{1}^BY{2}^BCN,{3},N,N,N^FD{4}^FS", x, y, byw, h, temp));
        }

        /// <summary>
        /// Code93一维码
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="byw">宽窄比</param>
        /// <param name="h">高度(宽度)</param>
        /// <param name="data">数据</param>
        public static void PrintCode93(int x, int y, int byw, int h, string data)
        {
            //对小写字母支持
            Regex cn = new Regex("[a-z]");
            string value = string.Empty;
            foreach (char item in data)
            {
                if (cn.IsMatch(item.ToString()))
                {
                    value += string.Format("){0}", item);
                }
                else
                {
                    value += item;
                }
            }
            lzpOrder.Append(string.Format("^FO{0},{1}^BY{2}^BAN,{3},N,N,N^FD{4}^FS", x, y, byw, h, value));
        }

        ///<summary>
        /// 对纯黑白图像支持较好,反之应找更为准确的转换方法.
        /// </summary>
        /// <param name="s_FilePath">图片路径</param>
        /// <returns>二值黑白字符串</returns>
        private static string ConvertImageToString(string s_FilePath)
        {
            int b = 0;
            long n = 0;
            long clr;
            StringBuilder sb = new StringBuilder();
            Bitmap bm = ImageHelper.LoadImageFormFreeImage(s_FilePath);
            int w = ((bm.Size.Width / 8 + ((bm.Size.Width % 8 == 0) ? 0 : 1)) * bm.Size.Height);
            int h = (bm.Size.Width / 8 + ((bm.Size.Width % 8 == 0) ? 0 : 1));

            sb.Append(w.ToString().PadLeft(5, '0') + "," + h.ToString().PadLeft(3, '0') + ",\n");
            for (int y = 0; y < bm.Size.Height; y++)
            {
                for (int x = 0; x < bm.Size.Width; x++)
                {
                    //每次左移1位,将位置调整正确.
                    //比如第一个像素就是1,其余7个均为0.
                    //那么最终b=10000000,为最终显示成这样,需要将1从最低位依次推送到高位.
                    b = b * 2;
                    clr = bm.GetPixel(x, y).ToArgb();
                    string s = clr.ToString("X");

                    //FFFFFF表示白色,000000表示黑色,这里取BBBBBB作为分界点.
                    //大于BBBBBB的转换成白色,反之转换成黑色.
                    //需要注意的是,斑马打印机用0表示白色,用1表示黑色.
                    if (s.Substring(s.Length - 6, 6).CompareTo("BBBBBB") < 0)
                    {
                        b++;
                    }
                    n++;
                    if (x == (bm.Size.Width - 1))
                    {
                        if (n < 8)
                        {
                            //不足八个像素点时,将位置调整正确.
                            //比如还剩下5个像素点，第一个像素就是1，其余4个均为0。
                            //当n=5时,b=00010000
                            //与实际值,1000000有差别,因此需要修正正确,即需要进行下面的位移操作.
                            b = b * ((int)Math.Pow(2, 8 - n));
                            sb.Append(b.ToString("X").PadLeft(2, '0'));
                            b = 0;
                            n = 0;
                        }
                    }
                    if (n >= 8)
                    {
                        sb.Append(b.ToString("X").PadLeft(2, '0'));
                        b = 0;
                        n = 0;
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 打印图片
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        public static void PrintImage(string imagePath, int x, int y)
        {
            lzpOrder.Append(string.Format("^FO{0},{1}~DGR:{2}.GRF,{3}^XGR:{2}.GRF^FS", x, y, Path.GetFileNameWithoutExtension(imagePath), ConvertImageToString(imagePath)));
        }

        /// <summary>
        /// 删除缓存中图片
        /// ^FS指令表示字段定义已结束,有时可省略.但与^FD必须成对出现.
        /// 比如打印一个一维码,从字体、坐标、数据定义整个过程算是一个字段,如果中间插入^FS,会使插入前面的属性失效.
        /// 某些地方不能随便插入^FS,否则打印无效果.应依照官方文档示例编写.
        /// </summary>
        /// <param name="name">推荐使用:*.GRF</param>
        public static void DeleteImage(string name)
        {
            lzpOrder.Append(string.Format("^IDR:{0}^FS", name));
        }
    }
}
