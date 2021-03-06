﻿using System;
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
    public class PrintRectangle : IPrintFunction
    {
        public void PrintParseFuntion(PrintItemModel printItem,object other = null)
        {
            try
            {
                PrintRuleBase.PTK_DrawRectangle
                    (
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation,
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation,
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "thickness", this.GetType().Name),
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "pEX", this.GetType().Name) + printItem.XDeviation,
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "pEY", this.GetType().Name) + printItem.YDeviation
                    );
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("打印{0}异常:{1}", this.GetType().Name, ex.Message));
            }
        }
    }
}
