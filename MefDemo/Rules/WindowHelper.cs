using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MefDemo
{
    public class WindowHelper
    {
        /// <summary>
        /// 使用Window显示一用户控件
        /// </summary>
        /// <param name="element">要显示的用户控件</param>
        /// <param name="isShow">是否立即显示,默认值为true</param>
        /// <returns></returns>
        public static Window ShowWindow(object element, bool isShow = true)
        {
            Window window = WindowHelper.GetWindow(element);

            if (isShow)
            {
                window.ShowDialog();
            }
            return window;
        }

        /// <summary>
        /// 使用Window显示一用户控件
        /// </summary>
        /// <param name="element">要显示的用户控件</param>
        /// <param name="title">窗体显示时的标题</param>
        /// <param name="isShow">是否立即显示,默认值为true</param>
        public static Window ShowWindow(object element, string title, bool isShow = true)
        {
            Window window = WindowHelper.GetWindow(element);
            window.Title = title;

            if (isShow)
            {
                window.ShowDialog();
            }
            return window;
        }

        public static Window ShowWindow(object element, double width, double height, bool isShow = true)
        {
            Window window = WindowHelper.GetWindow(element);
            window.Width = width;
            window.Height = height;

            if (isShow)
            {
                window.ShowDialog();
            }
            return window;
        }

        public static Window ShowWindow(object element, EventHandler closeCallBack, bool isShow = true)
        {
            Window window = WindowHelper.GetWindow(element);

            if (closeCallBack != null)
            {
                window.Closed += closeCallBack;
            }
            if (isShow)
            {
                window.ShowDialog();
            }
            return window;
        }

        /// <summary>
        /// 使用Window显示一用户控件
        /// </summary>
        /// <param name="element">要显示的用户控件</param>
        /// <param name="title">窗体显示时的标题</param>
        /// <param name="isShow">是否立即显示,默认值为true</param>
        public static Window ShowWindow(object element, string title, double width, double height, bool isShow = true)
        {
            return WindowHelper.ShowWindow(element, title, width, height, null, isShow);
        }

        /// <summary>
        /// 使用Window显示一用户控件
        /// </summary>
        /// <param name="element">要显示的用户控件</param>
        /// <param name="title">窗体显示时的标题</param>
        /// <param name="isShow">是否立即显示,默认值为true</param>
        public static Window ShowWindow(object element, string title, double width, double height, EventHandler closeCallBack, bool isShow = true)
        {
            Window window = WindowHelper.GetWindow(element);
            window.Title = title;
            window.Width = width;
            window.Height = height;

            if (closeCallBack != null)
            {
                window.Closed += new EventHandler(closeCallBack);
            }
            if (isShow)
            {
                window.ShowDialog();
            }
            return window;
        }

        /// <summary>
        /// 获取一个 Window 窗口用于显示当前元素
        /// </summary>
        private static Window GetWindow(object element)
        {
            Window window = new Window()
            {
                Content = element,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            return window;
        }
    }
}
