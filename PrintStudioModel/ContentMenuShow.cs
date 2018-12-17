using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PrintStudioModel
{
    /// <summary>
    /// 菜单类
    /// </summary>
    public class ContentMenuShow
    {
        /// <summary>
        /// 条目选择事件
        /// </summary>
        public event RoutedEventHandler MenuItemClicked = null;

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MenuItemClicked != null)
                {
                    MenuItemClicked(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private ContextMenu _printContextmenu = null;
        public ContextMenu GetPrintContextMenu()
        {
            try
            {
                _printContextmenu = new ContextMenu();
                _printContextmenu.Tag = this;
                MenuItem mi = null;
                mi = new MenuItem();
                mi.Header = "新建TextBox输入项";
                mi.Tag = 1000;
                mi.Click += new RoutedEventHandler(MenuItem_Click);
                _printContextmenu.Items.Add(mi);
                return _printContextmenu;
            }
            catch
            {
                return null;
            }
        }

        private ContextMenu _printItemContextmenu = null;
        public ContextMenu GetPrintItemContextMenu()
        {
            try
            {
                _printItemContextmenu = new ContextMenu();
                _printItemContextmenu.Tag = this;
                MenuItem mi = null;
                mi = new MenuItem();
                mi.Header = "属性";
                mi.Tag = 1000;
                mi.Click += new RoutedEventHandler(MenuItem_Click);
                _printItemContextmenu.Items.Add(mi);

                mi = new MenuItem();
                mi.Header = "删除";
                mi.Tag = 1001;
                mi.Click += new RoutedEventHandler(MenuItem_Click);
                _printItemContextmenu.Items.Add(mi);
                return _printItemContextmenu;
            }
            catch
            {
                return null;
            }
        }

        private ContextMenu _printCanvasContextmenu = null;
        public ContextMenu GetPrintCanvasContextMenu()
        {
            try
            {
                _printCanvasContextmenu = new ContextMenu();
                _printCanvasContextmenu.Tag = this;
                MenuItem mi = null;
                mi = new MenuItem();
                mi.Header = "工具箱";
                mi.Tag = 1000;
                mi.Click += new RoutedEventHandler(MenuItem_Click);
                _printCanvasContextmenu.Items.Add(mi);

                mi = new MenuItem();
                mi.Header = "属性栏";
                mi.Tag = 1001;
                mi.Click += new RoutedEventHandler(MenuItem_Click);
                _printCanvasContextmenu.Items.Add(mi);

                mi = new MenuItem();
                mi.Header = "清空";
                mi.Tag = 1002;
                mi.Click += new RoutedEventHandler(MenuItem_Click);
                _printCanvasContextmenu.Items.Add(mi);

                mi = new MenuItem();
                mi.Header = "标签调整";
                mi.Tag = 1003;
                mi.Click += new RoutedEventHandler(MenuItem_Click);
                _printCanvasContextmenu.Items.Add(mi);
                return _printCanvasContextmenu;
            }
            catch
            {
                return null;
            }
        }
    }
}

