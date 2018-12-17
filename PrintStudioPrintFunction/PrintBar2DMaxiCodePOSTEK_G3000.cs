using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;
using PrintStudioRule;

namespace PrintStudioPrintFunction
{
    /// <summary>
    /// MaxiCode二维码
    /// </summary>
    public class PrintBar2DMaxiCodePOSTEK_G3000 : IPrintFunction
    {
        public void PrintParseFuntion(PrintItemModel printItem, object other = null)
        {
            try
            {
                PrintRuleBase.PTK_DrawBar2D_MaxiCode
               (
                   PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation,
                   PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation,
                   PrintRuleBase.GetPrintParameterByName<int>(printItem, "m", this.GetType().Name),
                   PrintRuleBase.GetPrintParameterByName<int>(printItem, "u", this.GetType().Name),
                   printItem.PrintKeyValue
               );
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("打印{0}异常:{1}", this.GetType().Name, ex.Message));
            }
        }
    }
}
