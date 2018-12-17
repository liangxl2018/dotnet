using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;
using PrintStudioRule;

namespace PrintStudioPrintFunction
{
    /// <summary>
    /// 打印一维码
    /// </summary>
    public class PrintBarcode : IPrintFunction
    {
        public void PrintParseFuntion(PrintItemModel printItem,object other = null)
        {
            try
            {
                PrintRuleBase.PTK_DrawBarcode
                    (
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation,
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation,
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "pDirec", this.GetType().Name),
                        PrintRuleBase.GetPrintParameterByName<string>(printItem, "pCode", this.GetType().Name),
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "narrowWidth", this.GetType().Name),
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "pHorizontal", this.GetType().Name),
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "pHeight", this.GetType().Name),
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "pText", this.GetType().Name),
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
