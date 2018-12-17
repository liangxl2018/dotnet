using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;
using PrintStudioRule;
using System.IO;

namespace PrintStudioPrintFunction
{
    /// <summary>
    /// 打印图片
    /// </summary>
    public class PrintPCXZebraPrinter : IPrintFunction
    {
        public void PrintParseFuntion(PrintItemModel printItem, object other = null)
        {
            try
            {
                if (!File.Exists(printItem.PrintKeyValue))
                {
                    throw new Exception(string.Format("未发现图片资源{0}.", printItem.PrintKeyValue));
                }

                ZebraPrinterHelper.PrintImage
                   (  
                   printItem.PrintKeyValue,
                       PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation,
                       PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation
                   );
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("打印{0}异常:{1}", this.GetType().Name, ex.Message));
            }
        }
    }
}
