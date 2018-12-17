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
using System.Drawing.Printing;
using PrintStudioModel;
using PrintStudioRule;
namespace CommonPrintStudio
{
    /// <summary>
    /// PrintClient.xaml 的交互逻辑
    /// </summary>
    public partial class PrintClient : UserControl
    {
        /// <summary>
        /// 打印模板
        /// </summary>
        private PrintFactoryModel printTemplet = null;

        /// <summary>
        /// 右键菜单
        /// </summary>
        private ContentMenuShow contentMenuShow = new ContentMenuShow();

        public PrintClient()
        {
            InitializeComponent();
            configModel.DialogType = 1;
            configModel.Filter = "PrintConfig(*.xml)|*.xml";
            cbPrintName.ItemsSource = PrinterSettings.InstalledPrinters;
            contentMenuShow.MenuItemClicked += new RoutedEventHandler(contentMenuShow_MenuItemClicked);
            cbPrintType.ItemsSource = Enum.GetValues(typeof(PrintClientType));
        }

        private void txValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            try
            {
                if (tb != null)
                {
                    int.Parse(tb.Text);
                }
            }
            catch
            {
                if (tb != null)
                {
                    tb.Text = "1";
                }
            }
        }

        private void btnLoadPrintConfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string errorInfo = string.Empty;
                if (string.IsNullOrWhiteSpace(configModel.DialogValue))
                {
                    MessageBox.Show("请选择配置模板.");
                    return;
                }
                printTemplet = XmlHelper.GetPrintTemplet(configModel.DialogValue,out errorInfo);
                if (!string.IsNullOrWhiteSpace(errorInfo))
                {
                    MessageBox.Show(errorInfo);
                    return;
                }
                //PES测试
                //Dictionary<string,string> Values=new Dictionary<string,string>();
                //Values.Add("Hello","41545454");
                //Values.Add("World","World");
                //PrintHelper.StartPrintForPES(Values, printTemplet.Clone(),"PrintStudioPrintFunction", "PrintStudioDataFunction", cbPrintName.SelectedValue.ToString(), int.Parse(txtQCNumber.Text), int.Parse(txtPrintX.Text), int.Parse(txtPrintY.Text));
                //printTemplet.Clone()不行，因为主程序下无PrintFactoryModel的dll。因为Deserialize时，会在主程序下查找对应Model。
                PrintFactoryModel p = printTemplet;
                p.PrintItems.ForEach(item => { item.PrintFunctionName = string.Format("{0}{1}", item.PrintFunctionName, (PrintClientType)(cbPrintType.SelectedItem)); });
                if ((PrintClientType)(cbPrintType.SelectedItem) == PrintClientType.CommonPrinter)
                {
                    PrintHelper.CommonStartPrint(this, p, "PrintStudioPrintFunction", "PrintStudioDataFunction", cbPrintName.SelectedValue.ToString(), int.Parse(txtQCNumber.Text), int.Parse(txtPrintX.Text), int.Parse(txtPrintY.Text));
                }
                else if ((PrintClientType)(cbPrintType.SelectedItem) == PrintClientType.ZebraPrinter)
                {
                    PrintHelper.ZebraStartPrint(this, p, "PrintStudioPrintFunction", "PrintStudioDataFunction", cbPrintName.SelectedValue.ToString(), int.Parse(txtQCNumber.Text), int.Parse(txtPrintX.Text), int.Parse(txtPrintY.Text));
                }
                else if ((PrintClientType)(cbPrintType.SelectedItem) == PrintClientType.ZebraPrinter600)
                {
                    PrintHelper.ZebraStartPrint(this, p, "PrintStudioPrintFunction", "PrintStudioDataFunction", cbPrintName.SelectedValue.ToString(), int.Parse(txtQCNumber.Text), int.Parse(txtPrintX.Text), int.Parse(txtPrintY.Text));
                }
                else
                {
                    PrintHelper.StartPrint(this, p, "PrintStudioPrintFunction", "PrintStudioDataFunction", cbPrintName.SelectedValue.ToString(), int.Parse(txtQCNumber.Text), int.Parse(txtPrintX.Text), int.Parse(txtPrintY.Text));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void contentMenuShow_MenuItemClicked(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            int flag = (int)mi.Tag;
            if (flag == 1000)
            {
                AddPrintItemControl a = new AddPrintItemControl();
                a.Owner = App.Current.MainWindow;
                a.ShowDialog();
                if (a.DialogResult == true)
                {
                    PrintItemControl p = a.UserPrintItem;
                    p.OnRemoveClick += new RoutedEventHandler(p_OnRemoveClick);
                    foreach (UIElement item in gridStudio.Children)
                    {
                        if (item is PrintItemControl)
                        {
                            if ((item as PrintItemControl).Name == p.Name)
                            {
                                MessageBox.Show(string.Format("已存在名称为:{0}的输入控件.", p.Name));
                                return;
                            }
                        }
                    }
                    gridStudio.Children.Add(p);
                }
            }
        }

        void p_OnRemoveClick(object sender, RoutedEventArgs e)
        {
            gridStudio.Children.Remove((UIElement)sender);
        }

        private void gridStudio_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void gridStudio_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            gridStudio.ContextMenu = contentMenuShow.GetPrintContextMenu();
        }

        /// <summary>
        /// 客户端配置界面保存
        /// </summary>
        public void SaveInterface()
        {
            PrintClientConfig printClientConfig = new PrintClientConfig();
            List<PrintItemControlModel> printItemControls = new List<PrintItemControlModel>();
            foreach (UIElement item in gridStudio.Children)
            {
                if (item is PrintItemControl)
                {
                    PrintItemControl temp = item as PrintItemControl;
                    printItemControls.Add(new PrintItemControlModel()
                    {
                        Caption = temp.Caption,
                        Name = temp.Name,
                        Value = temp.Value
                    });
                }
            }
            printClientConfig.X = int.Parse(txtPrintX.Text);
            printClientConfig.Y = int.Parse(txtPrintY.Text);
            printClientConfig.PrintTypeIndex = cbPrintType.SelectedIndex;
            printClientConfig.PrintNameIndex = cbPrintName.SelectedIndex;
            printClientConfig.PrintItemControls = printItemControls;
            XmlHelper.SaveClientConfig(printClientConfig, AppDomain.CurrentDomain.BaseDirectory + "ClientConfig.xml");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (gridStudio.Children.Count < 1)
            {
                PrintClientConfig printClientConfig = XmlHelper.GetClientConfig(AppDomain.CurrentDomain.BaseDirectory + "ClientConfig.xml");
                if (printClientConfig != null)
                {
                    PrintItemControl p = null;
                    List<PrintItemControlModel> printItemControls = printClientConfig.PrintItemControls;
                    if (printItemControls != null)
                    {
                        foreach (PrintItemControlModel item in printItemControls)
                        {
                            p = new PrintItemControl() { Name = item.Name, Caption = item.Caption, Value = item.Value };
                            p.OnRemoveClick += new RoutedEventHandler(p_OnRemoveClick);
                            gridStudio.Children.Add(p);
                        }
                    }
                    txtPrintX.Text = printClientConfig.X.ToString();
                    txtPrintY.Text = printClientConfig.Y.ToString();
                    cbPrintType.SelectedIndex = printClientConfig.PrintTypeIndex;
                    cbPrintName.SelectedIndex = printClientConfig.PrintNameIndex;
                }
            }
        }
    }
}
