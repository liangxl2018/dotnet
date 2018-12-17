using System;
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
    public class PrintBar2DQRZebraPrinter : IPrintFunction
    {
        public void PrintParseFuntion(PrintItemModel printItem, object other = null)
        {
            try
            {
                string d = "M";
                int type = PrintRuleBase.GetPrintParameterByName<int>(printItem, "type", this.GetType().Name);
                int flag = PrintRuleBase.GetPrintParameterByName<int>(printItem, "g", this.GetType().Name);
                if (flag == 0)
                {
                    d = "L";
                }
                else if (flag == 1)
                {
                    d = "M";
                }
                else if (flag == 2)
                {
                    d = "Q";
                }
                else if (flag == 3)
                {
                    d = "H";
                }
                if (type == 0)
                {
                    ZebraPrinterHelper.PrintQRCode
                        (
                            PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation,
                            PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation,
                            PrintRuleBase.GetPrintParameterByName<int>(printItem, "r", this.GetType().Name),
                            d,
                            printItem.PrintKeyValue
                        );
                }
                else
                {
                    ZebraPrinterHelper.PrintDataMatrix
                                          (
                                              PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation,
                                              PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation,
                                              PrintRuleBase.GetPrintParameterByName<int>(printItem, "r", this.GetType().Name),
                                              PrintRuleBase.GetPrintParameterByName<int>(printItem, "s", this.GetType().Name),
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
