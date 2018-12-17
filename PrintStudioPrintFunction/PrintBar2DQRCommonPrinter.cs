using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;
using PrintStudioRule;
using System.Drawing;
using ZXing;
using ZXing.Common;

namespace PrintStudioPrintFunction
{
    /// <summary>
    /// QR二维码
    /// </summary>
    public class PrintBar2DQRCommonPrinter : IPrintFunction
    {
        public void PrintParseFuntion(PrintItemModel printItem, object other = null)
        {
            try
            {
                Graphics g = (Graphics)other;
                //1像素的大小是不确定的.图像大小400*500,表示长400个像素单位,宽500个像素单位.不同分辨率的设备像素单位大小不一样,因此图像显示得有大有小.
                //创建出的图像最小都是 21*21个像素点
                //300点打印机和Graphics像素单位长度之比为1:3(1像素单位长度固定).
                //Bitmap的Width为像素点个数,因此在绘制二维码时,用printItem.Width / 3弥补素单位长度之比造成的图像放大.
                //即printItem.Width为300点打印机像素点个数,printItem.Width / 3为转化后对应在的Graphics中像素点个数.
                Bitmap img = BarCodeHelper.CreateBarCode(printItem.PrintKeyValue, BarcodeFormat.QR_CODE, new EncodingOptions() { Width = (int)printItem.Width / 3, Height = (int)printItem.Height / 3, PureBarcode = true, Margin = 0 });
                g.DrawImageUnscaled
                     (
                         img,
                         (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation) / 3,
                         (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation) / 3
                     );
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("打印{0}异常:{1}", this.GetType().Name, ex.Message));
            }
        }
    }
}
