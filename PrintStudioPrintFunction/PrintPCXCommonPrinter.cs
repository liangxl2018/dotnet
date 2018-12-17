using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;
using PrintStudioRule;
using System.IO;
using System.Drawing;

namespace PrintStudioPrintFunction
{
    /// <summary>
    /// 打印图片
    /// </summary>
    public class PrintPCXCommonPrinter : IPrintFunction
    {
        public void PrintParseFuntion(PrintItemModel printItem, object other = null)
        {
            try
            {
                Graphics g = (Graphics)other;
                if (!File.Exists(printItem.PrintKeyValue))
                {
                    throw new Exception(string.Format("未发现图片资源{0}.", printItem.PrintKeyValue));
                }
                Bitmap img = ImageHelper.LoadImageFormFreeImage(PrintRuleBase.GetPrintParameterByName<string>(printItem, "PrintKeyValue", this.GetType().Name));
                //如果是绘制原始图DrawImageUnscaled,不同图片即使Width、Height同,Graphics呈现效果也可能不一样.这与图片数据结构及Graphics呈现有关.
                //这里使用绝对长、宽.是界面呈现大小的1/3.
                //这里的PrintFactory.Width就是目前300点打印机的dot数
                //1dot=25.4/300 mm
                //1print=25.4/100 mm
                //先将dot转mm 再将mm转print
                g.DrawImage
                    (
                        img,
                        (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation) / 3,
                        (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation) / 3,
                        (float)printItem.Width / 3,
                        (float)printItem.Height / 3
                        );
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("打印{0}异常:{1}", this.GetType().Name, ex.Message));
            }
        }
    }
}
