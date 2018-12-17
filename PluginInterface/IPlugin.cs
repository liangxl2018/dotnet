using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace PluginInterface
{
    public interface IPlugin
    {
        string Text { get; }
        UserControl Do();
    }
}
