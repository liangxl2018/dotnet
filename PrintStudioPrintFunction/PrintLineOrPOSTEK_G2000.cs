using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;
using PrintStudioRule;

namespace PrintStudioPrintFunction
{
    /// <summary>
    /// 打印直线
    /// </summary>
    public class PrintLineOrPOSTEK_G2000 : IPrintFunction
    {
        public void PrintParseFuntion(PrintItemModel printItem,object other = null)
        {
            try
            {
                PrintRuleBase.PTK_DrawLineOr
                             (
                                 (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation) * 2 / 3,
                                 (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation) * 2 / 3,
                                 (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pWidth", this.GetType().Name)) * 2 / 3,
                                 (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pHeight", this.GetType().Name)) * 2 / 3
                             );
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("打印{0}异常:{1}", this.GetType().Name, ex.Message));
            }
        }
    }
}
