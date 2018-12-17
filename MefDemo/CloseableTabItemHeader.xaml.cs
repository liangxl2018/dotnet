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

namespace MefDemo
{
    /// <summary>
    /// CloseableTabItemHeader.xaml 的交互逻辑
    /// </summary>
    public partial class CloseableTabItemHeader : UserControl
    {
        /// <summary>
        ///  实例化可关闭标签项
        /// </summary>
        public CloseableTabItemHeader()
        {
            InitializeComponent();
            Title = lblTitle.Content as string;
        }

        /// <summary>
        /// 实例化可关闭标签项(给定标题文本)
        /// </summary>
        /// <param name="title">Title</param>
        public CloseableTabItemHeader(string title)
        {
            InitializeComponent();
            Title = title;
        }

        // 可关闭标签项文本
        private string _title;

        /// <summary>
        /// 设置标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                lblTitle.Content = _title;
            }
        }
    }
}
