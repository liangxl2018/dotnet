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

namespace CommonPrintStudio
{
    /// <summary>
    /// AddPrintItemControl.xaml 的交互逻辑
    /// </summary>
    public partial class AddPrintItemControl : Window
    {
        public PrintItemControl UserPrintItem { get; private set; }

        public AddPrintItemControl()
        {
            InitializeComponent();
        }

        private void btnSure_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show("请填写Name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(tbCaption.Text))
            {
                MessageBox.Show("请填写Caption.");
                return;
            }
            UserPrintItem = new PrintItemControl() { Name=tbName.Text,Caption=tbCaption.Text};
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
