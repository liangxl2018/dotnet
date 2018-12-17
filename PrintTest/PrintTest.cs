using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginInterface;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows;

namespace PrintTest
{
    [Export(typeof(IPlugin))]
    public class PrintTest :IPlugin
    {
        public string Text
        {
            get { return "打印助手插件"; }
        }

        public UserControl Do()
        {
            //xml中引用的dll一定要放在AppDomain.CurrentDomain.BaseDirectory下。
            //可用svn指定每个文件下载位置实现dll一定要放在AppDomain.CurrentDomain.BaseDirectory。下载到临时文件，提示重启生效。在重启检查临时文件进行文件处理。
            //获取文件时需要加plugin\\
            //printTemplet.Clone()不行，因为主程序下无PrintFactoryModel的dll。
            return new CommonPrintStudio.TempletPrint();
        }
    }
}
