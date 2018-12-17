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

namespace CommonPrintStudio
{
    /// <summary>
    /// PrintItemControl.xaml 的交互逻辑
    /// </summary>
    public partial class PrintItemControl : UserControl
    {
        public event RoutedEventHandler OnRemoveClick = null;

        public PrintItemControl()
        {
            InitializeComponent();
        }

        public string Caption
        {
            set { tbCaption.Text = value; }
            get { return tbCaption.Text; }
        }

        public string Value
        {
            get { return tbValue.Text; }
            set { tbValue.Text = value; }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (OnRemoveClick != null)
            {
                OnRemoveClick(this, e);
            }
        }
    }
}
