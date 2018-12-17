using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using PluginInterface;
using System.Windows.Controls;

namespace Plugin2
{
    [Export(typeof(IPlugin))]
    public class MyPlugin2 : IPlugin
    {
        public string Text
        {
            get { return "插件2"; }
        }

        public UserControl Do()
        {
            return new UserControl1();
        }
    }
}
