using System.Drawing.Printing;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace PrintStudioRule
{
    /// <summary>
    /// 通用打印帮助类
    /// </summary>
    public abstract class CommonPrintBase
    {
        /// <summary>
        /// 打印实例
        /// </summary>
        private PrintDocument printInstance = null;

        public CommonPrintBase()
        {
            printInstance = new PrintDocument();
            printInstance.PrintPage += new PrintPageEventHandler(printInstance_PrintPage);
            printInstance.QueryPageSettings += new QueryPageSettingsEventHandler(printInstance_QueryPageSettings);
        }

        /// <summary>
        /// 设置页面大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void printInstance_QueryPageSettings(object sender, QueryPageSettingsEventArgs e);

        /// <summary>
        /// 绘制打印内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void printInstance_PrintPage(object sender, PrintPageEventArgs e);

        /// <summary>
        /// 打印设置
        /// </summary>
        public void PrintSet()
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printInstance;
            printDialog.ShowDialog();
        }

        /// <summary>
        /// 页面设置
        /// </summary>
        public void PageSet()
        {
            PageSetupDialog pageSetupDialog = new PageSetupDialog();
            pageSetupDialog.Document = printInstance;
            pageSetupDialog.ShowDialog();
        }

        /// <summary>
        /// 预览
        /// </summary>
        public void PrintView()
        {
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printInstance;
            printPreviewDialog.ShowDialog();
        }

        /// <summary>
        /// 打印
        /// </summary>
        public void Print(string printName)
        {
            //弹窗打印
            //PrintDialog printDialog = new PrintDialog();
            //printDialog.Document = printInstance;
            //if (printDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    printInstance.Print();
            //}
            //直接调用 需要在调用打印之前已设置了打印机名
            printInstance.PrinterSettings.PrinterName = printName;
            printInstance.Print();
        }
    }
}
