using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace PrintStudioRule
{
    public static class DependencyHelper
    {
        public static DependencyObject FindVisualChildByName(Visual parent, string name)
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i) as Visual;
                    string controlName = child.GetValue(System.Windows.Controls.Control.NameProperty) as string;
                    if (controlName == name)
                    {
                        return child;
                    }
                    else
                    {
                        DependencyObject result = FindVisualChildByName(child, name);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }
            return null;
        }

        ///this.FindName("Name")仅可查询非动态创建的控件
        /// <summary>
        /// 静动控件均可以查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T FindVisualChildByName<T>(Visual parent, string name) where T : Visual
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i) as Visual;
                    string controlName = child.GetValue(System.Windows.Controls.Control.NameProperty) as string;
                    if (controlName == name)
                    {
                        return child as T;
                    }
                    else
                    {
                        T result = FindVisualChildByName<T>(child, name);
                        if (result != null)
                            return result;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 寻找符合指定类型的父对象
        /// 这里没有在父对象的子对象中查找,想要的结果可能在父类的子对象中.可调用VisualDownwardSearch实现.暂不实现.
        /// 当T实例不唯一时,可能并不是想要的结果.可以增加名称判断.暂不实现.
        /// </summary>
        public static DependencyObject VisualUpwardSearch<T>(this DependencyObject source)
        {
            DependencyObject temp = VisualTreeHelper.GetParent(source);
            while (temp != null && temp.GetType() != typeof(T))
            {
                temp = VisualTreeHelper.GetParent(temp);
            }
            return temp;
        }

        /// <summary>
        /// 寻找符合指定类型的子对象
        /// 当T实例不唯一时,可能并不是想要的结果.可以增加名称判断，暂不实现.
        /// </summary>
        public static DependencyObject VisualDownwardSearch<T>(this DependencyObject source)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(source);
            if (childCount.Equals(0))
            {
                return null;
            }
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject temp = VisualDownwardChildSearch<T>(VisualTreeHelper.GetChild(source, i));
                if (temp != null && temp.GetType() == typeof(T))
                {
                    return temp;
                }
            }
            return source;
        }

        /// <summary>
        /// 寻找符合指定类型的子对象
        /// </summary>
        public static DependencyObject VisualDownwardChildSearch<T>(this DependencyObject source)
        {
            try
            {
                int childCount = VisualTreeHelper.GetChildrenCount(source);
                if (childCount.Equals(0))
                {
                    return null;
                }
                for (int i = 0; i < childCount; i++)
                {
                    DependencyObject temp = VisualTreeHelper.GetChild(source, i);
                    if (temp != null && temp.GetType() == typeof(T))
                    {
                        return temp;
                    }
                    else
                    {
                        temp = VisualDownwardChildSearch<T>(temp);
                        if (temp != null && temp.GetType() == typeof(T))
                        {
                            return temp;
                        }
                    }
                }
                return source;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取子对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static List<T> GetVisualChildCollection<T>(object parent) where T : UIElement
        {
            List<T> visualCollection = new List<T>();
            GetVisualChildCollection(parent as DependencyObject, visualCollection);
            return visualCollection;
        }

        /// <summary>
        /// 获取子对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static void GetVisualChildCollection<T>(DependencyObject parent, List<T> visualCollection) where T : UIElement
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
                {
                    visualCollection.Add(child as T);
                }
                else if (child != null)
                {
                    GetVisualChildCollection(child, visualCollection);
                }
            }
        }
    }
}
