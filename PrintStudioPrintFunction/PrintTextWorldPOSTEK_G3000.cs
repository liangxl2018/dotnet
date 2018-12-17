using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;
using PrintStudioRule;

namespace PrintStudioPrintFunction
{
    /// <summary>
    /// 打印一行文字 有些参数必须传,有些如果没传就使用默认值
    /// 在用界面制作 配置文件时,最好每种打印项目均将参数配全.
    /// </summary>
    public class PrintTextWorldPOSTEK_G3000 : IPrintFunction
    {
        public void PrintParseFuntion(PrintItemModel printItem, object other = null)
        {
            try
            {
                string flag = Guid.NewGuid().ToString();
                flag = flag.Substring(flag.LastIndexOf("-") + 1);
                PrintRuleBase.PTK_DrawTextTrueTypeW
                      (
                          PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation,
                          PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation,
                          PrintRuleBase.GetPrintParameterByName<int>(printItem, "pHeight", this.GetType().Name),
                          PrintRuleBase.GetPrintParameterByName<int>(printItem, "fWidth", this.GetType().Name),
                          PrintRuleBase.GetPrintParameterByName<string>(printItem, "fType", this.GetType().Name),
                          PrintRuleBase.GetPrintParameterByName<int>(printItem, "fSpin", this.GetType().Name),
                          PrintRuleBase.GetPrintParameterByName<int>(printItem, "fWeight", this.GetType().Name),
                          PrintRuleBase.GetPrintParameterByName<int>(printItem, "fItalic", this.GetType().Name),
                          PrintRuleBase.GetPrintParameterByName<int>(printItem, "fUnline", this.GetType().Name),
                          PrintRuleBase.GetPrintParameterByName<int>(printItem, "fStrikeOut", this.GetType().Name),
                          flag,
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
