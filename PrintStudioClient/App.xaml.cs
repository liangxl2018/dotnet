using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace CommonPrintStudio
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (e.Args != null && e.Args.Count() > 0)
            {
                string fileName = e.Args[0];
                CommonPrintStudio.MainWindow.FileName = fileName;
            }
            Application app = Application.Current;
            app.StartupUri = new Uri("MainWindow.xaml", UriKind.RelativeOrAbsolute);
        }
    }
}
