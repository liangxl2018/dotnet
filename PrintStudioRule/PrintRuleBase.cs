using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using PrintStudioModel;

namespace PrintStudioRule
{
    /// <summary>
    /// G2000打印机1mm=8dot  G3000打印机1mm=11.7dot
    /// </summary>
    public class PrintRuleBase
    {
        [DllImport("WINPSK.dll")]
        public static extern int PTK_BinGraphicsDel(string pid);

        /// <summary>
        /// direct：方向，取值为B或T，缺省值为T。
        ///B：将从标签右下角开始打印      
        /// </summary>
        /// <param name="direct"></param>
        /// <returns></returns>
        [DllImport("WINPSK.dll")]
        public static extern int PTK_SetDirection(char direct);

        [DllImport("WINPSK.dll")]
        public static extern int ClosePort();

        /// <summary>
        /// 打开通讯端口
        /// </summary>
        /// <param name="name">当前所使用的打印机在WINDOWS下的名称</param>
        /// <returns></returns>
        [DllImport("WINPSK.dll")]
        public static extern int OpenPort(string name);

        /// <summary>
        /// 清除打印机缓冲内存的内容
        /// </summary>
        /// <returns></returns>
        [DllImport("WINPSK.dll")]
        public static extern int PTK_ClearBuffer();

        /// <summary>
        /// 打印一行 TrueType Font 文字，并且文字宽度和高度可以微调
        /// </summary>
        /// <param name="px">设置X 坐标，以点(dots)为单位</param>
        /// <param name="py">设置X 坐标，以点(dots)为单位</param>
        /// <param name="hight">字型高度，以点(dots)为单位</param>
        /// <param name="width">字型宽度，以点(dots)为单位,如果想打印正常比例的字体，需将 FWidth 设置为 0</param>
        /// <param name="font">字型名称</param>
        /// <param name="fspin">字体旋转角度及文字相对坐标对齐方式</param>
        /// <param name="fweight">居左 0 度, 2 -> 居左 90 度, 3 ->居左 180 度, 4 ->居左 270 度，5 ->居中 0 度, 6 ->居中 90 度, 7 ->居中270 度, 8 ->居中 270 度</param>
        /// <param name="Fitalic">字体粗细。 0 and 400 -> 400 标准、 100 -> 非常细、200 -> 极细、 300 -> 细、500 -> 中等、 600 -> 半粗、700 -> 粗  、 POSTEK PPLE API  函数手册  8BWINPSK.dll 错误返回值解析 16 800 -> 特粗  、900 -> 黑体。</param>
        /// <param name="funline">文字加底线，0 -> FALSE、1 -> TRUE；</param>
        /// <param name="fstrikeOut">文字加删除线，0 -> FALSE、1 -> TRUE；</param>
        /// <param name="id_name">识别名称，因为一行 TrueType 文字将被转换成 PCX格式数据以 id_name 作为PCX 格式图形的名称存放到打印机内，在关机前都可以多次通过 PTK_DrawPcxGraphics( )调用 id_name打印这行文字； (当 data 参数或其他参数不同时，请务必设定不同的 id_name 值) </param>
        /// <param name="data">字符串内容</param>
        /// <returns></returns>
        [DllImport("WINPSK.dll")]
        public static extern int PTK_DrawTextTrueTypeW(int pX, int pY, int fHight, int fWidth, string fType, int fSpin, int fWeight, int fItalic, int fUnline, int fStrikeOut, string idName, string printData);

        /// <summary>
        /// 打印一个条码
        /// </summary>
        /// <param name="px">置X坐标,以点(dots)为单位.</param>
        /// <param name="py">设置Y 坐标,以点(dots)为单位</param>
        /// <param name="pdirc">选择条码的打印方向. 0—不旋转;1—旋转 90°; 2—旋转 180°; 3—旋转 270°.</param>
        /// <param name="pCode">选择要打印的条码类型</param>
        /// <param name="narrowWidth">设置条码中窄单元的宽度,以点(dots)为单位</param>
        /// <param name=" pHorizontal">设置条码中宽单元的宽度,以点(dots)为单位</param>
        /// <param name="pVertical">设置条码高度,以点(dots)为单位</param>
        /// <param name="ptext">选’N’则不打印条码下面的人可识别文字,  选’B’则打印条码下面的人可识别文字. </param>
        /// <param name="data">一个长度为 1-100 的字符串</param>
        /// <returns></returns>
        [DllImport("WINPSK.dll")]
        public static extern int PTK_DrawBarcode(int pX, int pY, int pDirec, string pCode, int narrowWidth, int pHorizontal, int pVertical, int pText, string printData);

        /// <summary>
        /// 命令打印机执行打印工作
        /// </summary>
        /// <param name="x">印标签的数量，取值范围：1—65535；</param>
        /// <param name="y">张标签的复制份数，取值范围：1—65535； 如果 cpnumber没有设置，那么默认为 1。</param>
        /// <returns></returns>
        [DllImport("WINPSK.dll")]
        public static extern int PTK_PrintLabel(int x, int y);

        /// <summary>
        /// 打印头发热温度
        /// </summary>
        /// <param name="x">取值范围：0—20，缺省为 10</param>
        /// <returns></returns>
        [DllImport("WINPSK.dll")]
        public static extern int PTK_SetDarkness(int x);

        /// <summary>
        /// 设置打印速度
        /// </summary>
        /// <param name="x">值范围为0 - 6，或者10 – 80。</param>
        /// <returns></returns>
        [DllImport("WINPSK.dll")]
        public static extern int PTK_SetPrintSpeed(int x);

        /// <summary>
        /// 标签的宽度
        /// </summary>
        /// <param name="x">标签的宽度，以点(dots)为单位</param>
        /// <returns></returns>
        [DllImport("WINPSK.dll")]
        public static extern int PTK_SetLabelWidth(int x);

        /// <summary>
        /// 打印一行文本文字
        /// </summary>
        /// <param name="px">置 X 坐标,以点(dots)为单位.</param>
        /// <param name="py">设置Y 坐标,以点(dots)为单位</param>
        /// <param name="pdirec">选择文字的打印方向. 0—不旋转;1—旋转90°; 2—旋转 180°; 3—旋转 270°.</param>
        /// <param name="pFont">选择内置字体或软字体. 1—5: 为打印机内置字体; ‘A’—‘Z’: 为下载的软字体</param>
        /// <param name="pHorizontal">设置点阵水平放大系数. 可选择:1—24.</param>
        /// <param name="pVertical">设置点阵垂直放大系数. 可选择:1—24.</param>
        /// <param name="ptext">选’N’则打印正常文本(如黑字白底文本),  选’R’则打印文本反色文本(如白字黑底文本).</param>
        /// <param name="data">一个长度为 1-100的字符串</param>
        /// <returns></returns>
        [DllImport("WINPSK.dll")]
        public static extern int PTK_DrawText(int px, int py, int pdirec, int pFont, int pHorizontal, int pVertical, int ptext, string data);

        /// <summary>
        /// 标签的高度和定位间隙\黑线\穿孔的高度
        /// </summary>
        /// <param name="lheight">标签的高度，以点(dots)为单位，取值范围：0-65535；</param>
        /// <param name="gapH">标签间的定位间隙/黑线/穿孔的高度，以点(dots)为单位，</param>
        /// <returns></returns>
        [DllImport("WINPSK.dll")]
        public static extern int PTK_SetLabelHeight(int lheight, int gapH);

        /// <summary>
        /// 打印二维码
        /// </summary>
        [DllImport("WINPSK.dll")]
        public static extern int PTK_DrawBar2D_QR(int x, int y, int w, int v, int o, int r, int m, int g, int s, string pstr);

        /// <summary>
        /// 打印直线
        /// </summary>
        [DllImport("WINPSK.dll")]
        public static extern int PTK_DrawLineOr(int px, int py, int pLength, int pH);

        /// <summary>
        /// 打印矩形
        /// </summary>
        [DllImport("WINPSK.dll")]
        public static extern int PTK_DrawRectangle(int pX, int pY, int thickness, int pEx, int pEy);

        /// <summary>
        /// 打印斜线
        /// </summary>
        [DllImport("WINPSK.dll")]
        public static extern int PTK_DrawDiagonal(int px, int py, int thickness, int pEx, int pEy);

        /// <summary>
        /// 打印一个DataMatrix二维条码
        /// </summary>
        [DllImport("WINPSK.dll")]
        public static extern int PTK_DrawBar2D_DATAMATRIX(int x, int y, int w, int v, int o, int m, string pstr);

        /// <summary>
        /// 打印一个MaxiCode二维条码
        /// </summary>
        [DllImport("WINPSK.dll")]
        public static extern int PTK_DrawBar2D_MaxiCode(int x, int y, int m, int u, string pstr);

        /// <summary>
        /// 打印一个PDF417二维条码
        /// </summary>
        [DllImport("WINPSK.dll")]
        public static extern int PTK_DrawBar2D_Pdf417(int x, int y, int w, int v, int s, int c, int px, int py, int r, int l, int t, int o, string pstr);

        /// <summary>
        /// 打印一个汉信码二维条码
        /// </summary>
        [DllImport("WINPSK.dll")]
        public static extern int PTK_DrawBar2D_HANXIN(int x, int y, int w, int v, int o, int r, int m, int g, int s, string pstr);

        [DllImport("WINPSK.dll")]
        public static extern int PTK_PrintPCX(int px, int py, string filename);

        [DllImport("WINPSK.dll")]
        public static extern int PTK_PcxGraphicsDownload(string pcxname, string pcxpath);

        [DllImport("WINPSK.dll")]
        public static extern int PTK_DrawPcxGraphics(int px, int py, string gname);

        /// <summary>
        /// 获取指定参数名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p"></param>
        /// <param name="name"></param>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public static T GetPrintParameterByName<T>(PrintItemModel p, string name, string functionName)
        {
            try
            {
                if (p.Parameters == null)
                {
                    throw new Exception(string.Format("参数列表为空."));
                }
                if (!p.Parameters.ContainsKey(name))
                {
                    throw new Exception("未查询到有效值.");
                }
                string temp = p.Parameters.FirstOrDefault(s =>
                 {
                     return s.Key.ToUpper().Trim() == name.ToUpper().Trim();
                 }).Value;
                return (T)Convert.ChangeType(temp, typeof(T));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("获取<{0}>的打印参数<{1}>值异常:{2}", functionName, name, ex.Message));
            }
        }
    }
}
