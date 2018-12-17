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
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using PluginInterface;
using System.Diagnostics;

namespace MefDemo
{
    /// <summary>
    /// 插件生成在单独的一个文件夹，主程序在该文件夹中加载。
    /// 一般来说，各个插件之间无公共的依赖dll。因此，可以考虑将插件包共享至svn，主程序列出所有插件包，按需下载。
    /// 已经下载的，需要在主程序有记录，可记录在Settings里，启动时，有更新再更新，可以每个插件包一个版本信息文件。管理更新工作。也可以利用svn，有svn版本更新就自动更新。
    /// 
    /// PES有太多的公用dll。每个插件一个文件夹，会造成dll重复和占用较大空间。基于此，可以将插件全部生成在一个文件夹，利用svn管理。一旦有新版本，主程序立即更新。
    /// 其实，公共的dll可以放在AppDomain.CurrentDomain.BaseDirectory，但需要人为分清哪些是公共的，哪些是应放在插件包里的，这样不易操作，直接放弃。
    /// 
    /// Plugin1与Plugin2均使用了MyModel程序集,基于此可以将MyModel生成在MefDemo下,Plugin1与Plugin2下可以不需要MyModel,
    /// 程序执行时会首先在MefDemo下查找,如果不存在，又会在自动在Plugin1与Plugin2下查找
    /// 
    /// 当然不一样的插件,如果引用较为复杂,仅仅将某个共有程序集移到MefDemo下,可能会出错，具体问题只能具体分析了
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 插件集合
        /// </summary>
        [ImportMany]
        private IEnumerable<IPlugin> myPlugins = null;

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MenuItem item = new MenuItem() { Header = "插件" };
            ms.Items.Add(item);
            foreach (IPlugin plugin in myPlugins)
            {
                MenuItem subItem = new MenuItem() { Header = plugin.Text, Tag = plugin };
                subItem.Click += (s, arg) =>
                {
                    IPlugin pluginTemp = (IPlugin)((MenuItem)s).Tag;
                    UserControl u = pluginTemp.Do();
                    foreach (TabItem app in workSpace.Items)
                    {
                        if ((string)app.Tag == pluginTemp.Text)
                        {
                            workSpace.SelectedItem = app;
                            return;
                        }
                    }
                    CloseableTabItem tabItem = new CloseableTabItem(pluginTemp.Text);
                    tabItem.Tag = pluginTemp.Text;
                    tabItem.Content = u;
                    tabItem.TabItemClosing += tabItem_OnClose;
                    workSpace.Items.Add(tabItem);
                    workSpace.SelectedIndex = workSpace.Items.Count - 1;
                };
                item.Items.Add(subItem);
            }
        }

        public void tabItem_OnClose(object sender, RoutedEventArgs e)
        {
            CloseableTabItem wt = sender as CloseableTabItem;
        }

        private void Init()
        {
            //设置目录，让引擎能自动去发现新的扩展
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog("plugin\\"));
            //创建一个容器，相当于是生产车间
            CompositionContainer _container = new CompositionContainer(catalog);
            //调用车间的ComposeParts把各个部件组合到一起
            //这里只需要传入当前应用程序实例就可以了，其它部分会自动发现并组装
            _container.ComposeParts(this);


            //根据表达式获取所需的部件。上面的当然可以直接实现过滤，这里仅仅演示自定义Catalog的构建方法。
            //可在自定义Catalog中实现其他功能，如定时刷新插件等功能。遇到有需求时在深入研究。
            //DirectoryCatalog catalog = new DirectoryCatalog("plugin\\");
            //FilteredCatalog filteredCatalog = new FilteredCatalog(catalog, o => true);
            //CompositionContainer filteredContainer = new CompositionContainer(filteredCatalog);
            //filteredContainer.ComposeParts(this);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (TabItem item in workSpace.Items)
            {

            }
            Process.GetCurrentProcess().Kill();
        }
    }
}
