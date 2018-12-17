using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;
using PrintStudioRule;
using System.Drawing;

namespace PrintStudioPrintFunction
{
    /// <summary>
    /// 打印一行文字 有些参数必须传,有些如果没传就使用默认值
    /// 在用界面制作 配置文件时,最好每种打印项目均将参数配全.
    /// </summary>
    public class PrintTextWorldCommonPrinter : IPrintFunction
    {
        public void PrintParseFuntion(PrintItemModel printItem, object other = null)
        {
            try
            {
                Graphics g = (Graphics)other;
                g.DrawString
                            (
                                printItem.PrintKeyValue,
                                new Font(PrintRuleBase.GetPrintParameterByName<string>(printItem, "fType", this.GetType().Name), WordHeightToFontSize(PrintRuleBase.GetPrintParameterByName<double>(printItem, "pHeight", this.GetType().Name))),
                                new SolidBrush(Color.Black),
                                //细微修正-2 保证打印效果与界面呈现一致
                                (int)((PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation) / 3 - 2),
                                (int)((PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation) / 3 - 2),
                                new StringFormat()
                            );
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("打印{0}异常:{1}", this.GetType().Name, ex.Message));
            }
        }

        /// <summary>
        ///300点打印尺度转Graphics尺度 文本高度转FontSize
        /// </summary>
        /// <returns></returns>
        private float WordHeightToFontSize(double height)
        {
            //本来是71/300 考虑到控件本身会占用一点高度,所以设置成64.5
            return (float)(height * 64.5 / 300);
        }
    }
}
