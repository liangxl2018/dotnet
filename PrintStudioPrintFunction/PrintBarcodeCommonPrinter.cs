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
    /// 打印一维码
    /// </summary>
    public class PrintBarcodeCommonPrinter : IPrintFunction
    {
        public void PrintParseFuntion(PrintItemModel printItem, object other = null)
        {
            try
            {
                Graphics g = (Graphics)other;
                //这里无法精细化绘制一维码,是抛弃该打印方式的的主要原因.
                //如果说有更好的、高还原的一维码生成解决方案便可采纳.
                //但依然存在着如进纸的精准性控制、效果细微差别、条码长度不可控等问题.
                //CODE_128不支持中文字符,这里的值如果是中文,将会抛出异常.
                Bitmap img = BarCodeHelper.CreateBarCode(printItem.PrintKeyValue, BarcodeFormat.CODE_128, new EncodingOptions() { Height = (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pHeight", this.GetType().Name)) / 3, PureBarcode = true, Margin = 0 });
                g.DrawImageUnscaled
                     (
                         img,
                         (int)(PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation) / 3,
                         (int)(PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation) / 3
                     );
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("打印{0}异常:{1}", this.GetType().Name, ex.Message));
            }
        }
    }
}
