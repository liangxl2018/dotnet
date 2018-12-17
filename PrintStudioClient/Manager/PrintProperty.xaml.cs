
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
using PrintStudioModel;
using PrintStudioRule;

namespace CommonPrintStudio
{
    /// <summary>
    /// PrintAttribute.xaml 的交互逻辑
    /// </summary>
    public partial class PrintProperty : UserControl
    {
        private ContentControlBase CurrentPrintControl = null;

        public PrintProperty()
        {
            //注意 this.Resources!=Properties.Resources

            //this.Resources是FrameworkElement的资源
            //而Properties.Resources是运用程序的全局资源
            //ConfigurationManager.AppSettings是配置文件资源
            InitializeComponent();
        }

        /// <summary>
        /// 属性改变事件 暂不使用,尝试在此类中直接改变控件属性
        /// </summary>
        public event EventHandler OnPrintCcontrolPropertyChanged = null;

        /// <summary>
        /// 显示控件属性
        /// </summary>
        /// <param name="c"></param>
        public void DisplayPrintCcontrolProperty(ContentControlBase c)
        {
            gdAttribute.Children.Clear();
            gdAttribute.RowDefinitions.Clear();
            gdAttribute.ColumnDefinitions.Clear();
            CurrentPrintControl = c;
            if (c != null)
            {
                CreatePropertyWind(c);
            }
        }

        private void CreatePropertyWind(ContentControlBase c)
        {
            gdAttribute.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            gdAttribute.ColumnDefinitions.Add(new ColumnDefinition() { });
            int index = 0;
            TextBox tbTextBox = null;
            CheckBox cbCheckBox = null;
            TextBlock tbTextBlock = null;
            ComboBox cbComboBox = null;
            DirOrFileSelect drOrFileSelect = null;
            gdAttribute.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            PrintItemPropety p = new PrintItemPropety() { Margin = new Thickness(5, 5, 5, 5) };
            p.CurrentPrintControl = c;
            gdAttribute.Children.Add(p);
            Grid.SetRow(p, index);
            Grid.SetColumn(p, 0);
            Grid.SetColumnSpan(p, 2);
            index++;
            gdAttribute.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            Rectangle line = new Rectangle() { Margin = new Thickness(5, 5, 5, 5), Fill = new SolidColorBrush(Colors.RosyBrown), Height = 2 };
            gdAttribute.Children.Add(line);
            Grid.SetRow(line, index);
            Grid.SetColumn(line, 0);
            Grid.SetColumnSpan(line, 2);
            index++;
            foreach (PropertyModel item in c.Propertys)
            {
                if (!item.IsDisplayInPropertyControl)
                {
                    continue;
                }
                gdAttribute.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                tbTextBlock = new TextBlock() { VerticalAlignment = VerticalAlignment.Center, Text = item.Caption, Margin = new Thickness(5), FontWeight = FontWeights.Bold };
                switch (item.PropertyType)
                {
                    case PropertyType.String:
                        {
                            tbTextBox = new TextBox() { DataContext = c, VerticalAlignment = VerticalAlignment.Center, Text = item.Value != null ? item.Value.ToString() : string.Empty, Tag = item, Margin = new Thickness(5) };
                            InputMethod.SetIsInputMethodEnabled(tbTextBox, item.SupportChinese);
                            tbTextBox.LostFocus += new RoutedEventHandler(tbTextBox_LostFocus);
                            tbTextBox.KeyDown += new KeyEventHandler(tbTextBox_KeyDown);
                            gdAttribute.Children.Add(tbTextBox);
                            Grid.SetRow(tbTextBox, index);
                            Grid.SetColumn(tbTextBox, 1);
                            break;
                        }
                    case PropertyType.Int:
                        {
                            tbTextBox = new TextBox() { DataContext = c, VerticalAlignment = VerticalAlignment.Center, Text = item.Value != null ? item.Value.ToString() : string.Empty, Tag = item, Margin = new Thickness(5) };
                            InputMethod.SetIsInputMethodEnabled(tbTextBox, item.SupportChinese);
                            tbTextBox.LostFocus += new RoutedEventHandler(tbTextBox_LostFocus);
                            tbTextBox.KeyDown += new KeyEventHandler(tbTextBox_KeyDown);
                            gdAttribute.Children.Add(tbTextBox);
                            Grid.SetRow(tbTextBox, index);
                            Grid.SetColumn(tbTextBox, 1);
                            break;
                        }
                    case PropertyType.Double:
                        {
                            tbTextBox = new TextBox() { DataContext = c, VerticalAlignment = VerticalAlignment.Center, Text = item.Value != null ? item.Value.ToString() : string.Empty, Tag = item, Margin = new Thickness(5) };
                            InputMethod.SetIsInputMethodEnabled(tbTextBox, item.SupportChinese);
                            tbTextBox.LostFocus += new RoutedEventHandler(tbTextBox_LostFocus);
                            tbTextBox.KeyDown += new KeyEventHandler(tbTextBox_KeyDown);
                            gdAttribute.Children.Add(tbTextBox);
                            Grid.SetRow(tbTextBox, index);
                            Grid.SetColumn(tbTextBox, 1);
                            break;
                        }
                    case PropertyType.CheckBox:
                        {
                            cbCheckBox = new CheckBox() { DataContext = c, VerticalAlignment = VerticalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, IsChecked = item.Value != null ? ((int)Convert.ChangeType(item.Value, typeof(int)) == 0 ? false : true) : false, Tag = item, Margin = new Thickness(5) };
                            cbCheckBox.Checked += new RoutedEventHandler(ckCheckBox_Checked);
                            cbCheckBox.Unchecked += new RoutedEventHandler(ckCheckBox_Unchecked);
                            gdAttribute.Children.Add(cbCheckBox);
                            Grid.SetRow(cbCheckBox, index);
                            Grid.SetColumn(cbCheckBox, 1);
                            break;
                        }
                    case PropertyType.Combobox:
                        {
                            cbComboBox = new ComboBox() { DataContext = c, VerticalAlignment = VerticalAlignment.Center, DisplayMemberPath = "Caption", Tag = item, Margin = new Thickness(5) };
                            cbComboBox.ItemsSource = item.ComboBoxData;
                            if (item.ComboBoxData != null && item.Value != null)
                            {
                                foreach (KeyCaption k in item.ComboBoxData)
                                {
                                    if (item.ComboBoxValueType == 0)
                                    {
                                        if (k.Caption == (string)Convert.ChangeType(item.Value, typeof(string)))
                                        {
                                            cbComboBox.SelectedItem = k;
                                            break;
                                        }
                                    }
                                    else if (item.ComboBoxValueType == 1)
                                    {
                                        try
                                        {
                                            if (k.ID == (int)Convert.ChangeType(item.Value, typeof(int)))
                                            {
                                                cbComboBox.SelectedItem = k;
                                                break;
                                            }
                                        }
                                        catch
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            cbComboBox.SelectionChanged += new SelectionChangedEventHandler(cbComboBox_SelectionChanged);
                            gdAttribute.Children.Add(cbComboBox);
                            Grid.SetRow(cbComboBox, index);
                            Grid.SetColumn(cbComboBox, 1);
                            break;
                        }
                    case PropertyType.Picture:
                        {
                            drOrFileSelect = new DirOrFileSelect() { Tag = item, DialogValue = item.Value != null ? item.Value.ToString() : string.Empty, VerticalAlignment = VerticalAlignment.Center, DialogType = 1, Filter = "PCX(*.pcx)|*.pcx|BMP(*.bmp)|*.bmp|JPG(*.jpg)|*.jpg|PNG(*.png)|*.png" };
                            drOrFileSelect.OnSelectedChanged += new EventHandler<PictureSelectedEventArgs>(drOrFileSelect_OnSelectedChanged);
                            gdAttribute.Children.Add(drOrFileSelect);
                            Grid.SetRow(drOrFileSelect, index);
                            Grid.SetColumn(drOrFileSelect, 1);
                            break;
                        }
                }
                gdAttribute.Children.Add(tbTextBlock);
                Grid.SetRow(tbTextBlock, index);
                Grid.SetColumn(tbTextBlock, 0);
                index++;
            }
        }

        void tbTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox tb = sender as TextBox;
                if (tb != null)
                {
                    //代码触发事件
                    MouseButtonEventArgs args = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
                    args.RoutedEvent = TextBox.LostFocusEvent;
                    tb.RaiseEvent(args);
                }
            }
        }

        /// <summary>
        /// 针对所有TextBox输入值得改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tbTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdatePropertyFromTextBox(sender);
        }

        private void UpdatePropertyFromTextBox(object sender)
        {
            TextBox tb = sender as TextBox;
            if (tb == null)
            {
                return;
            }
            PropertyModel p = tb.Tag as PropertyModel;
            if (p == null)
            {
                return;
            }
            string propertyValue = string.Empty;
            switch (p.PropertyType)
            {
                case PropertyType.String:
                    {
                        if (string.IsNullOrWhiteSpace(tb.Text))
                        {
                            tb.Text = string.Empty;
                        }
                        if (p.Value != null)
                        {
                            propertyValue = (string)Convert.ChangeType(p.Value, typeof(string));
                            p.Value = tb.Text;
                        }
                        else
                        {
                            p.Value = string.Empty;
                        }
                        break;
                    }
                case PropertyType.Int:
                    {
                        if (string.IsNullOrWhiteSpace(tb.Text))
                        {
                            tb.Text = "0";
                        }
                        try
                        {
                            int.Parse(tb.Text);
                        }
                        catch
                        {
                            tb.Text = "0";
                        }
                        try
                        {
                            if (p.Value != null)
                            {
                                propertyValue = Convert.ChangeType(p.Value, typeof(int)).ToString();
                            }
                            else
                            {
                                propertyValue = "0";
                            }
                        }
                        catch
                        {
                            propertyValue = "0";
                        }
                        p.Value = int.Parse(tb.Text);
                        break;
                    }
                case PropertyType.Double:
                    {
                        if (string.IsNullOrWhiteSpace(tb.Text))
                        {
                            tb.Text = "0";
                        }
                        try
                        {
                            tb.Text = Math.Floor(double.Parse(tb.Text)).ToString();
                        }
                        catch
                        {
                            tb.Text = "0";
                        }
                        try
                        {
                            if (p.Value != null)
                            {
                                propertyValue = Convert.ChangeType(p.Value, typeof(double)).ToString();
                            }
                            else
                            {
                                propertyValue = "0";
                            }
                        }
                        catch
                        {
                            propertyValue = "0";
                        }
                        p.Value = double.Parse(tb.Text);
                        break;
                    }
            }
            if (tb.Text != propertyValue)
            {
                DealWithSpecialPrintControl((ContentControlBase)tb.DataContext, p, tb);
            }
        }

        /// <summary>
        /// Picture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void drOrFileSelect_OnSelectedChanged(object sender, PictureSelectedEventArgs e)
        {
            PropertyModel p = e.Source as PropertyModel;
            if (p == null)
            {
                return;
            }
            p.Value = e.PictureResource;
            //这里是完整路径 提取时可灵活处理.或可统一取名字,打印时路径为可执行文件目录.
            CurrentPrintControl.PrintKeyValue = e.PictureResource;
        }

        /// <summary>
        /// CheckBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ckCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ComboBox tb = sender as ComboBox;
            if (tb == null)
            {
                return;
            }
            if (tb.SelectedItem == null)
            {
                return;
            }
            PropertyModel p = tb.Tag as PropertyModel;
            if (p == null)
            {
                return;
            }
            KeyCaption k = tb.SelectedItem as KeyCaption;
            if (p.ComboBoxValueType == 0)
            {
                p.Value = k.Caption;
            }
            else
            {
                p.Value = k.ID;
            }
        }

        /// <summary>
        /// CheckBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ckCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox tb = sender as CheckBox;
            if (tb == null)
            {
                return;
            }
            PropertyModel p = tb.Tag as PropertyModel;
            if (p == null)
            {
                return;
            }
            if (tb.IsChecked == true)
            {
                p.Value = 1;
            }
            else
            {
                p.Value = 0;
            }
        }


        /// <summary>
        /// ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cbComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePropertyFromComboBox(sender);
        }

        private void UpdatePropertyFromComboBox(object sender)
        {
            ComboBox tb = sender as ComboBox;
            if (tb == null)
            {
                return;
            }
            if (tb.SelectedItem == null)
            {
                return;
            }
            PropertyModel p = tb.Tag as PropertyModel;
            if (p == null)
            {
                return;
            }
            KeyCaption k = tb.SelectedItem as KeyCaption;
            if (p.ComboBoxValueType == 0)
            {
                p.Value = k.Caption;
            }
            else
            {
                p.Value = k.ID;
            }
            DealWithSpecialPrintControl((ContentControlBase)CurrentPrintControl, p);
        }


        private void DealWithSpecialPrintControl(ContentControlBase c, PropertyModel p, TextBox textBox = null)
        {
            if (p.Name == "pX")
            {
                Canvas.SetLeft(c, (double)Convert.ChangeType(p.Value, typeof(double)));
                return;
            }
            if (p.Name == "pY")
            {
                Canvas.SetTop(c, (double)Convert.ChangeType(p.Value, typeof(double)));
                return;
            }
            if (p.PropertyChanged != null)
            {
                PropertyChangedFromTextBoxEventArgs proerty = new PropertyChangedFromTextBoxEventArgs()
                {
                    PrintControl = c,
                    TextBox = textBox,
                    Property = p
                };
                p.PropertyChanged(proerty);
            }
        }
    }
}
