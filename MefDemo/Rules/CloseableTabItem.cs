using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace MefDemo
{
    /// <summary>
    /// 是否自动排序
    /// </summary>
    public enum SortWay
    {
        IsAutoSort, NotSort
    }

    public class CloseableTabItem : TabItem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CloseButtonDelegate(object sender, RoutedEventArgs e);

        /// <summary>
        /// 选项卡TabItem关闭时事件
        /// </summary>
        public event RoutedEventHandler TabItemClosing;

        /// <summary>
        /// 实例化标签项
        /// </summary>
        public CloseableTabItem()
            : this("New Tab")
        { }

        /// <summary>
        /// 实例化标签项(给定标题)
        /// </summary>
        /// <param name="title">新标签项的标题字符串</param>
        public CloseableTabItem(string title)
            : this(title, SortWay.NotSort)
        {
        }

        /// <summary>
        /// 实例化标签项(给定标题,给定是否自动排序)
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="isAutoSort">是否自动排序</param>
        public CloseableTabItem(string title, SortWay sortWay)
        {
            //设定样式
            this.Style = (Style)Application.Current.Resources["TabItemStyle"];
            //this.Style = (Style)Application.Current.Resources["TabItemStyleFlyMainTestZeng"];

            //生产一个可关闭的Header
            CloseableTabItemHeader ctih = createCloseableTabItem();
            //自动排序
            switch (sortWay)
            {
                case SortWay.IsAutoSort:
                    break;
                case SortWay.NotSort:
                    break;
                default:
                    break;
            }
            //设定标题
            ctih.Title = title;
            //设定Header
            this.Header = ctih;
        }

        /// <summary>
        /// 创建一个标签页头
        /// </summary>
        /// <returns>标签页头(自定义控件)</returns>
        private CloseableTabItemHeader createCloseableTabItem()
        {
            //实例化一个Header
            CloseableTabItemHeader ctih = new CloseableTabItemHeader();
            //添加关闭按钮点击事件
            ctih.btnClose.Click += new RoutedEventHandler(btnClose_Click);
            //返回Header
            return ctih;
        }

        /// <summary>
        /// 关闭按钮的点击事件处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // 触发标签项关闭事件
            if (TabItemClosing != null)
            {
                TabItemClosing.Invoke(this, e);
            }
            //关闭当前TabItem
            ((TabControl)GetParentObject<TabControl>(this)).Items.Remove(this);
        }

        /// 获得指定元素的父元素  
        /// </summary>  
        /// <typeparam name="T">父级元素类型</typeparam>  
        /// <param name="obj">指定查找元素</param>  
        /// <returns></returns>  
        public T GetParentObject<T>(DependencyObject obj) where T : FrameworkElement
        {
            //返回可视对象的父对象
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            //按层、类型提取父级
            while (parent != null)
            {
                if (parent is T)
                    return (T)parent;
                parent = VisualTreeHelper.GetParent(parent);
            }
            //返回
            return null;
        }
    }
}
