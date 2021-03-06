﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;
using PrintStudioRule;

namespace PrintStudioPrintFunction
{
    /// <summary>
    /// DATAMATRIX二维码
    /// </summary>
    public class PrintBar2DDATAMATRIX : IPrintFunction
    {
        public void PrintParseFuntion(PrintItemModel printItem,object other = null)
        {
            try
            {
                PrintRuleBase.PTK_DrawBar2D_DATAMATRIX
                    (
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation,
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation,
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "w", this.GetType().Name),
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "v", this.GetType().Name),
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "o", this.GetType().Name),
                        PrintRuleBase.GetPrintParameterByName<int>(printItem, "m", this.GetType().Name),
                        printItem.PrintKeyValue
                    );
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("打印{0}异常:{1}", this.GetType().Name, ex.Message)); ;
            }
        }
    }
}
