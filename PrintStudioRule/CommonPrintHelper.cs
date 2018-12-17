using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;
using System.Drawing.Printing;
using System.Drawing;
using ZXing;
using ZXing.Common;

namespace PrintStudioRule
{
    public class CommonPrintHelper : CommonPrintBase
    {
        /// <summary>
        /// 打印数量
        /// </summary>
        public int PrintCount { get; set; }

        /// <summary>
        /// 打印机名
        /// </summary>
        public string PrintName { get; set; }

        /// <summary>
        /// 打印资源
        /// </summary>
        public PrintFactoryModel PrintFactory { get; set; }

        /// <summary>
        /// 绘制打印图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void printInstance_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            PrintDocument printDocument = (PrintDocument)sender;
            PrintFactory.PrintItems.ForEach(p =>
            {
                p.ParseFuntion.PrintParseFuntion(p, g);
            });
            PrintCount--;
            if (PrintCount < 1)
            {
                e.HasMorePages = false;
                printDocument.Dispose();
            }
            else
            {
                e.HasMorePages = true;
            }
        }

        /// <summary>
        /// 开始打印
        /// </summary>
        /// <param name="PrintItems"></param>
        /// <param name="printName"></param>
        /// <param name="printCount"></param>
        public void StartPrint(PrintFactoryModel printFactoryModel, string printName, int printCount)
        {
            if (printFactoryModel == null || printFactoryModel.PrintItems == null || printFactoryModel.PrintItems.Count < 1)
            {
                throw new Exception("打印条目集合不能为空.");
            }
            if (string.IsNullOrWhiteSpace(printName))
            {
                throw new Exception("打印机名不能为空.");
            }
            if (printCount < 1)
            {
                throw new Exception("打印数量必须大于0.");
            }
            this.PrintFactory = printFactoryModel;
            this.PrintName = printName;
            this.PrintCount = printCount;
            this.Print(PrintName);
        }

        /// <summary>
        /// 设置页面大小 如果设置的PaperSize.Height小于打印机当前单个条码的高度，打印机至少前进一个单个条码的高度。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void printInstance_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        {
            PageSettings p = e.PageSettings;
            PrintDocument printDocument = (PrintDocument)sender;
            printDocument.OriginAtMargins = false;
            p.PaperSize = new PaperSize()
            {
                //这里的PrintFactory.Width就是目前300点打印机的dot数
                //1dot=25.4/300 mm
                //1print=25.4/100 mm
                //先将dot转mm 再将mm转print
                //高度应略小于PrintFactory.Width/3,否则可能进纸2片
                Width = (int)(PrintFactory.Width / 3 - 0.1),
                Height = (int)(PrintFactory.Height / 3 - 0.1)
            };
            p.Margins = new Margins(0, 0, 0, 0);
        }
    }
}
