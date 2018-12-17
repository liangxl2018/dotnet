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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PrintStudioModel;

namespace CommonPrintStudio
{
    /// <summary>
    /// PrintItemPropety.xaml 的交互逻辑
    /// </summary>
    public partial class PrintItemPropety : UserControl
    {
        /// <summary>
        /// 当前打印控件
        /// </summary>
        public ContentControlBase CurrentPrintControl { get; set; }

        public PrintItemPropety()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if ((CurrentPrintControl is TextWorldControl) || (CurrentPrintControl is BarCodeControl) || (CurrentPrintControl is Bar2DQRControl))
            {
                tbPrintKeyValue.Visibility = Visibility.Visible;
                tbValueCaption.Visibility = Visibility.Visible;
                btnSet.Visibility = Visibility.Visible;
            }
            else
            {
                tbPrintKeyValue.Visibility = Visibility.Collapsed;
                tbValueCaption.Visibility = Visibility.Collapsed;
                btnSet.Visibility = Visibility.Collapsed;
            }
            this.DataContext = CurrentPrintControl;
        }

        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            PrintKeyValueControl p = new PrintKeyValueControl();
            p.CurrentPrintControl = CurrentPrintControl;
            p.Owner = App.Current.MainWindow;
            p.ShowDialog();
        }
    }
}
