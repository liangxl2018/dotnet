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
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using System.IO;
using PrintStudioModel;

namespace CommonPrintStudio
{
    /// <summary>
    /// TempletPrint.xaml 的交互逻辑
    /// </summary>
    public partial class TempletPrint : UserControl
    {
        public TempletPrint()
        {
            InitializeComponent();
            printTool.OnMouseMoveEvent += new MouseEventHandler(printTool_OnMouseMoveEvent);
            printTool.OnMouseLeftButtonUpEvent += new MouseButtonEventHandler(printTool_OnMouseLeftButtonUpEvent);
            printTool.OnMouseLeftButtonDownEvent += new MouseButtonEventHandler(printTool_OnMouseLeftButtonDownEvent);
            printCanvas.OnPrintControlPropertyEvent += new EventHandler<ContentMenuEventArgs>(printCanvas_OnPrintControlPropertyEvent);
            printCanvas.OnCanvasContentMenuEvent += new EventHandler<ContentMenuEventArgs>(printCanvas_OnCanvasContentMenuEvent);
            printAttribute.OnPrintCcontrolPropertyChanged += new EventHandler(printAttribute_OnPrintCcontrolPropertyChanged);

        }

        public void SaveConfig()
        {
            printCanvas.SaveInterface();
            printClient.SaveInterface();
        }

        void printCanvas_OnCanvasContentMenuEvent(object sender, ContentMenuEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            int flag = (int)mi.Tag;
            if (flag == 1000)
            {
                DisplayToolWindow(true);
            }
            else if (flag == 1001)
            {
                DisplayAttributeWindow(true);
            }
        }

        /// <summary>
        /// 控件属性改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printAttribute_OnPrintCcontrolPropertyChanged(object sender, EventArgs e)
        {
            printCanvas.UpdatePrintControlFromProperty((ContentControlBase)sender);
        }

        /// <summary>
        /// 显示控件属性事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printCanvas_OnPrintControlPropertyEvent(object sender, ContentMenuEventArgs e)
        {
            printAttribute.DisplayPrintCcontrolProperty((ContentControlBase)sender);
            if (e != null)
            {
                if ((int)e.MenuItem.Tag == 1000)
                {
                    DisplayAttributeWindow(true);
                }
            }
        }

        /// <summary>
        /// 工具箱开关
        /// </summary>
        /// <param name="visible"></param>
        public void DisplayAttributeWindow(bool visible)
        {
            attributeWindow.IsVisible = visible;
        }

        /// <summary>
        /// 属性开关
        /// </summary>
        /// <param name="visible"></param>
        public void DisplayToolWindow(bool visible)
        {
            toolWindow.IsVisible = visible;
        }

        /// <summary>
        /// 打印工具栏 控件左键Down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printTool_OnMouseLeftButtonDownEvent(object sender, MouseButtonEventArgs e)
        {
            ContentControlBase p = sender as ContentControlBase;
            printCanvas.UpdatePrintControlFromTool(p);
        }

        /// <summary>
        /// 打印工具栏 控件左键Up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printTool_OnMouseLeftButtonUpEvent(object sender, MouseButtonEventArgs e)
        {
            printCanvas.AddPrinControlByDrag();
        }

        /// <summary>
        /// 打印工具栏 控件MouseMove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printTool_OnMouseMoveEvent(object sender, MouseEventArgs e)
        {
            //TODO 暂不使用
        }
    }
}
