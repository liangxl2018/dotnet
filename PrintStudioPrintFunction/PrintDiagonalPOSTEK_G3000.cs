using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;
using PrintStudioRule;

namespace PrintStudioPrintFunction
{
    /// <summary>
    /// 打印斜线
    /// </summary>
    public class PrintDiagonalPOSTEK_G3000 : IPrintFunction
    {
        public void PrintParseFuntion(PrintItemModel printItem, object other = null)
        {
            try
            {
                PrintRuleBase.PTK_DrawDiagonal
                    (
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation,
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation,
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "pHeight", this.GetType().Name),
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "pEx", this.GetType().Name),
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "pEy", this.GetType().Name)
                    );
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("打印{0}异常:{1}", this.GetType().Name, ex.Message));
            }
        }
    }
}
