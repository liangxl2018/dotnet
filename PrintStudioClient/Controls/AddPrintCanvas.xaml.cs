using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PrintStudioModel;

namespace CommonPrintStudio
{
    /// <summary>
    /// AddPrintItemControl.xaml 的交互逻辑
    /// </summary>
    public partial class AddPrintCanvas : Window
    {
        public PrintItemControl UserPrintItem { get; private set; }

        public AddPrintCanvas(PrintLableModel printLable)
        {
            InitializeComponent();
            if (printLable != null)
            {
                tbWidth.Text = ((printLable.Width * 127 / 1500.0)).ToString();
                tbHeight.Text = ((printLable.Height * 127 / 1500.0)).ToString();
            }
        }

        public PrintLableModel PrintLable { get; set; }

        private void btnSure_Click(object sender, RoutedEventArgs e)
        {
            double result = 0;
            PrintLable = new PrintLableModel();
            if (string.IsNullOrWhiteSpace(tbWidth.Text))
            {
                MessageBox.Show("请填写Caption.");
                return;
            }
            else
            {
                if (double.TryParse(tbWidth.Text, out result))
                {
                    //300打印机 1dot=25.4/300mm
                    //203打印机 1dot=25.4/203mm
                    //将实际标签尺寸转成以打印机dot为单位的尺寸,即y=x*300/25.4
                    //这里生成的是y dot,但PrintLable.Width是WPF单位,实际界面呈现时会等比例放大或缩小z.
                    //幸运的是,并不影响使用,因为条码控件本身就是WPF类型控件,其界面呈现时与打印机单位相比也会等比例放大或缩小z.
                    //如果是200点的打印机,那么在这里更改缩放比例即可.即result * 203 / 25.4.
                    //就是说通过调整界面呈现大小,来适应不同打印设备.
                    PrintLable.Width = ((result * 1500 / 127.0));
                }
                else
                {
                    MessageBox.Show("请正确填写标签Width.");
                    return;
                }
            }
            if (string.IsNullOrWhiteSpace(tbHeight.Text))
            {
                MessageBox.Show("请填写Caption.");
                return;
            }
            else
            {
                if (double.TryParse(tbHeight.Text, out result))
                {
                    PrintLable.Height = ((result * 1500 / 127.0));
                }
                else
                {
                    MessageBox.Show("请正确填写标签Height.");
                    return;
                }
            }
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
