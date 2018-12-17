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
    /// PrintTool.xaml 的交互逻辑
    /// </summary>
    public partial class PrintTool : UserControl
    {
        /// <summary>
        /// 鼠标拖动事件
        /// </summary>
        public event MouseEventHandler OnMouseMoveEvent = null;

        /// <summary>
        /// 鼠标左键Up事件
        /// </summary>
        public event MouseButtonEventHandler OnMouseLeftButtonUpEvent = null;

        /// <summary>
        /// 鼠标左键按在事件
        /// </summary>
        public event MouseButtonEventHandler OnMouseLeftButtonDownEvent = null;

        public PrintTool()
        {
            InitializeComponent();
        }

        private void PrintControl_OnMouseMoveEvent(object sender, MouseEventArgs e)
        {
            if (OnMouseMoveEvent != null)
            {
                OnMouseMoveEvent(sender, e);
            }
        }

        private void PrinControl_OnMouseLeftButtonUpEvent(object sender, MouseButtonEventArgs e)
        {
            if (OnMouseLeftButtonUpEvent != null)
            {
                OnMouseLeftButtonUpEvent(sender, e);
            }
        }

        private void PrintControl_OnMouseLeftButtonDownEvent(object sender, MouseButtonEventArgs e)
        {
            if (OnMouseLeftButtonDownEvent != null)
            {
                OnMouseLeftButtonDownEvent(sender, e);
            }
        }
    }
}
