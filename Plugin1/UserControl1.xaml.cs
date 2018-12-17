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
using System.IO;
using MyModel;
namespace Plugin1
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            tbInfo.Content = AppDomain.CurrentDomain.BaseDirectory;
            //获取文件时需要加plugin\\
            FileStream f = File.Open("plugin\\HM_SOCA_VE.zip", FileMode.Open);

            //可以将MyModel.dll放在AppDomain.CurrentDomain.BaseDirectory下，作为公共的dll。
            MyModel.MyModel g = new MyModel.MyModel();
            g.Name = "HHIUHIUHU";
            tbInfo.Content = g.Name;

        }
    }
}
