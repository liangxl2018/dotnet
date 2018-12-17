using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using PluginInterface;
using System.Windows.Controls;

namespace Plugin1
{
    [Export(typeof(IPlugin))]
    public class MyPlugin : IPlugin
    {
        public string Text
        {
            get { return "插件1"; }
        }


        public UserControl Do()
        {
            return new UserControl1();
        }
    }
}
