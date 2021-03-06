﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;
using PrintStudioRule;

namespace PrintStudioPrintFunction
{
    /// <summary>
    /// QR二维码
    /// </summary>
    public class PrintBar2DQRPOSTEK_G6000 : IPrintFunction
    {
        public void PrintParseFuntion(PrintItemModel printItem, object other = null)
        {
            try
            {
                int r = PrintRuleBase.GetPrintParameterByName<int>(printItem, "r", this.GetType().Name) * 2;
                r = r > 9 ? 9 : r;
                PrintRuleBase.PTK_DrawBar2D_QR
                    (
                        (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation) * 2,
                        (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation) * 2,
                        (PrintRuleBase.GetPrintParameterByName<int>(printItem, "w", this.GetType().Name)),
                        (PrintRuleBase.GetPrintParameterByName<int>(printItem, "v", this.GetType().Name)),
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "o", this.GetType().Name),
                        r,
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "m", this.GetType().Name),
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "g", this.GetType().Name),
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "s", this.GetType().Name),
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
