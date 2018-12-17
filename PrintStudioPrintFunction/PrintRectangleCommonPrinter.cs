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
    /// 打印矩形
    /// </summary>
    public class PrintRectangleCommonPrinter : IPrintFunction
    {
        public void PrintParseFuntion(PrintItemModel printItem, object other = null)
        {
            try
            {
                Graphics g = (Graphics)other;
                Pen p = new Pen(Color.Black, (PrintRuleBase.GetPrintParameterByName<int>(printItem, "thickness", this.GetType().Name)) / 3);
                g.DrawRectangle(p, new Rectangle()
                {
                    X = (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pX", this.GetType().Name) + printItem.XDeviation) / 3,
                    Y = (PrintRuleBase.GetPrintParameterByName<int>(printItem, "pY", this.GetType().Name) + printItem.YDeviation) / 3,
                    Width = (int)printItem.Width / 3,
                    Height = (int)printItem.Height / 3,
                });
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("打印{0}异常:{1}", this.GetType().Name, ex.Message));
            }
        }
    }
}
