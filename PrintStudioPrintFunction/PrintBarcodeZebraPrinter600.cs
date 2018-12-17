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
    public class PrintBarcodeZebraPrinter600 : IPrintFunction
    {
        public void PrintParseFuntion(PrintItemModel printItem, object other = null)
        {
            try
            {
                string code = PrintRuleBase.GetPrintParameterByName<string>(printItem, "pCode", this.GetType().Name);
                if (code == "1E")
                {
                    ZebraPrinterHelper.PrintCode93
                         (
                             (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation)*2,
                             (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation)*2,
                             (PrintRuleBase.GetPrintParameterByName<int>(printItem, "narrowWidth", this.GetType().Name))*2,
                             (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pHeight", this.GetType().Name))*2,
                             printItem.PrintKeyValue
                         );
                }
                else if (code == "1")
                {
                    ZebraPrinterHelper.PrintCode128C
                        (
                            (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation)*2,
                            (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation)*2,
                            (PrintRuleBase.GetPrintParameterByName<int>(printItem, "narrowWidth", this.GetType().Name))*2,
                            (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pHeight", this.GetType().Name))*2,
                            printItem.PrintKeyValue
                        );
                }
                else
                {
                    ZebraPrinterHelper.PrintCode128
                                         (
                                             PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation,
                                             PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation,
                                             PrintRuleBase.GetPrintParameterByName<int>(printItem, "narrowWidth", this.GetType().Name),
                                             PrintRuleBase.GetPrintParameterByName<int>(printItem, "pHeight", this.GetType().Name),
                                             printItem.PrintKeyValue
                                         );
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("打印{0}异常:{1}", this.GetType().Name, ex.Message));
            }
        }
    }
}
