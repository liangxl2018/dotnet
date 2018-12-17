using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;
using PrintStudioRule;

namespace PrintStudioPrintFunction
{
    /// <summary>
    /// 打印矩形
    /// </summary>
    public class PrintRectanglePOSTEK_G6000 : IPrintFunction
    {
        public void PrintParseFuntion(PrintItemModel printItem, object other = null)
        {
            try
            {
                PrintRuleBase.PTK_DrawRectangle
                    (
                        (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation)*2,
                        (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation)*2,
                        (PrintRuleBase.GetPrintParameterByName<int>(printItem, "thickness", this.GetType().Name))*2,
                        (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pEX", this.GetType().Name) + printItem.XDeviation)*2,
                        (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pEY", this.GetType().Name) + printItem.YDeviation)*2
                    );
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("打印{0}异常:{1}", this.GetType().Name, ex.Message));
            }
        }
    }
}
