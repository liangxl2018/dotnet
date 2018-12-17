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
using System.Reflection;
using PrintStudioModel;
using System.Drawing.Printing;

namespace CommonPrintStudio
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 命令参数 文件路径
        /// </summary>
        public static string FileName { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == (ModifierKeys.Control) && e.Key == Key.P)
            {
                templePrint.DisplayAttributeWindow(true);
            }
            else if (Keyboard.Modifiers == (ModifierKeys.Control) && e.Key == Key.T)
            {
                templePrint.DisplayToolWindow(true);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                //templePrint.printCanvas.SaveInterface();
                //templePrint.printClient.SaveInterface();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(string.Format("保存异常:{0}", ex.Message));
                e.Cancel = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(FileName))
            {
                //templePrint.printCanvas.LoadInterfaceByCommonFileName(FileName);
            }
        }
    }
}
