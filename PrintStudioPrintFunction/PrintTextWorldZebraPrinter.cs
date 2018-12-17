using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;
using PrintStudioRule;
using System.Text.RegularExpressions;

namespace PrintStudioPrintFunction
{
    /// <summary>
    /// 斑马打印 Text
    /// </summary>
    public class PrintTextWorldZebraPrinter : IPrintFunction
    {
        public void PrintParseFuntion(PrintItemModel printItem, object other = null)
        {
            try
            {
                Regex cn = new Regex("[\u4e00-\u9fa5]+");//正则表达式 表示汉字范围
                string type = PrintRuleBase.GetPrintParameterByName<string>(printItem, "fType", this.GetType().Name);
                if (type == "Arial")
                {
                    type = "Y";
                }
                else if (type == "ZEBRA0")
                {
                    type = "0";
                }
                else
                {
                    type = "Z";
                }
                if (cn.IsMatch(printItem.PrintKeyValue))
                {
                    type = "Z";
                }
                ZebraPrinterHelper.PrintString
                    (
                       type,
                       PrintRuleBase.GetPrintParameterByName<int>(printItem, "pHeight", this.GetType().Name),
                       PrintRuleBase.GetPrintParameterByName<int>(printItem, "fWidth", this.GetType().Name),
                       PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation,
                       PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation,
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
