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
using System.Windows.Shapes;
using PrintStudioModel;
using PrintStudioRule;

namespace CommonPrintStudio
{
    /// <summary>
    /// PrintKeyValueControl.xaml 的交互逻辑
    /// </summary>
    public partial class PrintKeyValueControl : Window
    {
        /// <summary>
        /// 当前打印控件
        /// </summary>
        public ContentControlBase CurrentPrintControl { get; set; }

        public PrintKeyValueControl()
        {
            InitializeComponent();
            cbSource.ItemsSource = new List<KeyCaption>() 
            { 
                new KeyCaption(){ID=0,Caption="固定值"},
                new KeyCaption(){ID=1,Caption="界面值"},
                new KeyCaption(){ID=2,Caption="方法值"},
            };
        }

        private void LoadHelpDocument()
        {
            try
            {
                List<ParameterModel> p = XmlHelper.GetHelpDocument(AppDomain.CurrentDomain.BaseDirectory + "HelpConfig.xml");
                int index = 0;
                TextBox tbKey = null;
                TextBlock tbValue = null;
                gdAttribute.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                gdAttribute.ColumnDefinitions.Add(new ColumnDefinition() { });
                foreach (ParameterModel item in p)
                {
                    gdAttribute.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    tbKey = new TextBox()
                    {
                        Margin = new Thickness(5),
                        Text = item.Key,
                        IsReadOnly = true,
                        FontWeight = FontWeights.Bold
                    };
                    gdAttribute.Children.Add(tbKey);
                    Grid.SetRow(tbKey, index);
                    Grid.SetColumn(tbKey, 0);
                    tbValue = new TextBlock()
                    {
                        Margin = new Thickness(5),
                        Text = item.Value
                    };
                    gdAttribute.Children.Add(tbValue);
                    Grid.SetRow(tbValue, index);
                    Grid.SetColumn(tbValue, 1);
                    index++;
                }
            }
            catch { }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = CurrentPrintControl;
            cbSource.SelectedIndex = CurrentPrintControl.DataSourceType;
            tbIndexs.Text = CurrentPrintControl.FunctionData.FunctionIndexsCaption;
            LoadHelpDocument();
        }

        private void cbSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            KeyCaption k = cbSource.SelectedItem as KeyCaption;
            if (k != null)
            {
                CurrentPrintControl.DataSourceType = k.ID;
            }
        }

        /// <summary>
        /// 索引处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbIndexs_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbIndexs.Text))
            {
                CurrentPrintControl.FunctionData.FunctionIndexs = null;
                CurrentPrintControl.FunctionData.FunctionIndexsCaption = string.Empty;
            }
            else
            {
                try
                {
                    CurrentPrintControl.FunctionData.FunctionIndexs = CommonRule.ParseIndexs(tbIndexs.Text);
                    CurrentPrintControl.FunctionData.FunctionIndexsCaption = tbIndexs.Text;
                }
                catch
                {
                    CurrentPrintControl.FunctionData.FunctionIndexs = null;
                    CurrentPrintControl.FunctionData.FunctionIndexsCaption = string.Empty;
                    tbIndexs.Text = string.Empty;
                }
            }
        }
    }
}
