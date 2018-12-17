using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace MefDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //代码添加资源
            this.Resources.MergedDictionaries.Clear();
            ResourceDictionary resource = (ResourceDictionary)Application.LoadComponent(new Uri("/MefDemo;component/Style/Style.xaml", UriKind.RelativeOrAbsolute));
            this.Resources.MergedDictionaries.Add(resource);
        }
    }
}
