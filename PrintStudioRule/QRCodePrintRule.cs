using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace PrintStudioRule
{
    /// <summary>
    /// 斑马打印二维码封装类, 拷贝自MES
    /// </summary>
    public class QRCodePrintRule
    {
        //打印机
        private static IntPtr hPrinter = new IntPtr(0);
        private static bool isOpen = false;
        private static string lzpOrder = "";
        // Structure and API declarions:
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

        //检取一个标识特定打印机或打印服务器的句柄并打开 
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        //关闭给定的打印机对象
        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool ClosePrinter(IntPtr hPrinter);

        //通知假脱机打印程序将在假脱机上打印一个文档
        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        //终止给定打印机的一个打印作业
        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool EndDocPrinter(IntPtr hPrinter);

        //通知假脱机打印程序将在给定打印机上打印一页
        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool StartPagePrinter(IntPtr hPrinter);

        //指示一页的结束和下一页的开始
        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool EndPagePrinter(IntPtr hPrinter);

        //通知假脱机打印程序应向给定的打印机写指定的数据
        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = "My C#.NET RAW Document";
            di.pDataType = "RAW";

            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }


        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;
            // How many characters are in the string?
            dwCount = szString.Length;
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.
            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }

        public static bool OpenLZPPrinter(string szPrinterName)
        {
            hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            di.pDocName = "My C#.NET RAW Document";
            di.pDataType = "RAW";

            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        lzpOrder = "^XA\n";
                        isOpen = true;
                        return true;
                    }
                    else
                    {
                        isOpen = false;
                        return false;
                    }
                }
                else
                {
                    isOpen = false;
                    return false;
                }
            }
            isOpen = false;
            return false;
        }

        public static bool CloseLZPPrinter()
        {
            Int32 dwWritten = 0;
            bool bSuccess = false;
            if (hPrinter != null && isOpen == true)
            {
                IntPtr pBytes;
                Int32 dwCount;
                lzpOrder += "^XZ";
                dwCount = lzpOrder.Length;
                pBytes = Marshal.StringToCoTaskMemAnsi(lzpOrder);
                bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                Marshal.FreeCoTaskMem(pBytes);
                EndPagePrinter(hPrinter);
                EndDocPrinter(hPrinter);
                ClosePrinter(hPrinter);
            }
            return bSuccess;
        }
        /// <summary>
        /// 打印DataMatrix二维码
        /// </summary>
        /// <param name="x">X轴</param>
        /// <param name="y">Y轴</param>
        /// <param name="dmultiple">各个元素的空间高度</param>
        /// <param name="data">数据</param>
        public static void PrintDataMatrix(int x, int y, int dmultiple, string data)
        {
            lzpOrder += string.Format("^FO{0},{1}\n^BXN,{2},200\n^FD{3}^FS\n", x, y, dmultiple, data);
        }

        /// <summary>
        /// 打印字符串
        /// </summary>
        /// <param name="type">本方法支持ZLP打印内置打印类型</param>
        /// <param name="heigth">字符高度</param>
        /// <param name="width">字符长度</param>
        /// <param name="x">X轴</param>
        /// <param name="y">Y轴</param>
        /// <param name="data">数据</param>
        public static void PrintString(string type, int heigth, int width, int x, int y, string data)
        {
            lzpOrder += string.Format("^A{0}N,{1},{2}\n^FO{3},{4}\n^FD{5}^FS\n", type, heigth, width, x, y, data);
        }

        /// <summary>
        /// 打印测试
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="printerName"></param>
        /// <param name="data"></param>
        public static void TestPrint(int x, int y, string printerName, string data)
        {
            if (OpenLZPPrinter(printerName))
            {
                PrintDataMatrix(x, y, 6, data);
                PrintString("0", 30, 25, x, y + 100, data.Substring(data.Length - 8));
            }
            CloseLZPPrinter();
        }
    }
}
