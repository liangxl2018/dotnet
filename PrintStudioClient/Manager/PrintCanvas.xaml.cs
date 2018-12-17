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
using PrintStudioRule;
using PrintStudioModel;
using My = System.Windows.Forms;

//300点 
//1dot=0.32WPF
//1WPF=3.125dot

//203点 暂未给出
namespace CommonPrintStudio
{
    /// <summary>
    /// PrintCanvas.xaml 的交互逻辑
    /// </summary>
    public partial class PrintCanvas : UserControl
    {
        private string xmlPath = string.Empty;

        /// <summary>
        /// 当前选中控件
        /// </summary>
        private ContentControlBase CurrentControlBase = null;

        /// <summary>
        /// 右键菜单
        /// </summary>
        public ContentMenuShow ContentMenuShow = new ContentMenuShow();

        /// <summary>
        /// Mouse是否存在该面板内
        /// </summary>
        private static bool IsMouseEnter = false;

        /// <summary>
        /// 放大倍数Index 默认1
        /// </summary>
        private static int ZoomableCanvasScale = 4;

        /// <summary>
        /// 当前标签基本信息
        /// </summary>
        private PrintLableModel PrintLable = new PrintLableModel()
        {
            Width = 519,
            Height = 318
        };

        /// <summary>
        /// 工作台缩放列表
        /// </summary>
        private static List<double> ScaleList = new List<double>() 
        { 
            0.2,0.32,0.5,0.8,1,1.2,1.5,1.8,2,2.2,2.5,2.8,3,3.2,3.5,3.8,4,4.2,4.5,4.8,5
        };

        /// <summary>
        /// 放大、缩小工作台
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="orignalWidth"></param>
        /// <param name="orignalHeight"></param>
        private void UpdateScale(double scale, double orignalWidth, double orignalHeight)
        {
            borderWork.Width = orignalWidth * scale;
            borderWork.Height = orignalHeight * scale;
            canvasSpace.Scale = scale;
            tbScale.Text = string.Format("{0:f1}*", scale);
        }


        /// <summary>
        /// 显示控件属性事件
        /// </summary>
        public event EventHandler<ContentMenuEventArgs> OnPrintControlPropertyEvent = null;

        /// <summary>
        /// 画板右键菜单
        /// </summary>
        public event EventHandler<ContentMenuEventArgs> OnCanvasContentMenuEvent = null;

        /// <summary>
        /// 工具栏选择的打印控件
        /// </summary>
        private static volatile ContentControlBase PrintControlFromTool = null;

        /// <summary>
        /// 工具栏选择的打印控件放置坐标
        /// </summary>
        private static Point PrintControlFromToolOfPosition = default(Point);

        public PrintCanvas()
        {
            InitializeComponent();
            ContentMenuShow.MenuItemClicked += new RoutedEventHandler(ContentMenuShow_MenuItemClicked);
            this.AddHandler(UIElement.KeyUpEvent, new KeyEventHandler(UserControl_KeyUp), true);
        }

        void ContentMenuShow_MenuItemClicked(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            int flag = (int)mi.Tag;
            if (flag == 1002)
            {
                canvasSpace.Children.Clear();
                OnPrintControlPropertyEvent(null, null);
                return;
            }
            else if (flag == 1003)
            {
                AddPrintCanvas a = new AddPrintCanvas(PrintLable);
                a.Owner = App.Current.MainWindow;
                a.ShowDialog();
                if (a.DialogResult == true)
                {
                    PrintLable = a.PrintLable;
                    AddOrUpdatePrintLable(PrintLable, false);
                }
            }
            if (OnCanvasContentMenuEvent != null)
            {
                OnCanvasContentMenuEvent(sender, null);
            }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="c"></param>
        private void AddPrinControlEventHandle(ContentControlBase c)
        {
            c.OnContentMenuEvent += new EventHandler<ContentMenuEventArgs>(c_OnContentMenuEvent);
            c.OnMouseLeftButtonDownEvent += new MouseButtonEventHandler(c_OnMouseLeftButtonDownEvent);
            c.OnMouseLeftButtonUpEvent += new MouseButtonEventHandler(c_OnMouseLeftButtonUpEvent);
            c.OnMouseMoveEvent += new MouseEventHandler(c_OnMouseMoveEvent);
        }

        void c_OnMouseMoveEvent(object sender, MouseEventArgs e)
        {
            //TODO 暂不使用
        }

        void c_OnMouseLeftButtonUpEvent(object sender, MouseButtonEventArgs e)
        {
            CurrentControlBase = sender as ContentControlBase;
            if (OnPrintControlPropertyEvent != null)
            {
                OnPrintControlPropertyEvent(sender, null);
            }
        }

        /// <summary>
        /// 鼠标左键Down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void c_OnMouseLeftButtonDownEvent(object sender, MouseButtonEventArgs e)
        {
            if (OnPrintControlPropertyEvent != null)
            {
                OnPrintControlPropertyEvent(sender, null);
            }
        }

        /// <summary>
        /// 控件右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void c_OnContentMenuEvent(object sender, ContentMenuEventArgs e)
        {
            MenuItem m = e.MenuItem;
            ContentControlBase Control = (ContentControlBase)e.Source;
            int flag = (int)m.Tag;
            if (flag == 1000)
            {
                if (OnPrintControlPropertyEvent != null)
                {
                    OnPrintControlPropertyEvent(Control, e);
                }
            }
            else if (flag == 1001)
            {
                canvasSpace.Children.Remove(Control);
                OnPrintControlPropertyEvent(null, e);
            }

            else if (flag == 1002)
            {
                canvasSpace.Children.Clear();
                OnPrintControlPropertyEvent(null, e);
            }
        }

        /// <summary>
        /// 拖动添加打印控件
        /// </summary>
        public void AddPrinControlByDrag()
        {
            if (IsMouseEnter && PrintControlFromTool != null)
            {
                ContentControlBase control = CommonRule.LoadClassInstance<ContentControlBase>(this.GetType().Namespace, PrintControlFromTool.GetType().Name);
                Canvas.SetTop(control, PrintControlFromToolOfPosition.Y);
                Canvas.SetLeft(control, PrintControlFromToolOfPosition.X);
                AddPrinControlEventHandle(control);
                canvasSpace.Children.Add(control);
                //更新放置坐标到控件相应属性
                PropertyModel pX = GetPropertyItemByName(control.Propertys, "pX");
                if (pX != null)
                {
                    pX.Value = Math.Floor(PrintControlFromToolOfPosition.X);
                }
                PropertyModel pY = GetPropertyItemByName(control.Propertys, "pY");
                if (pY != null)
                {
                    pY.Value = Math.Floor(PrintControlFromToolOfPosition.Y);
                }
                CurrentControlBase = control;
                PrintControlFromTool = null;
                this.Cursor = null;

            }
        }

        /// <summary>
        /// 工具栏选择了打印控件
        /// </summary>
        /// <param name="printControlFromTool"></param>
        public void UpdatePrintControlFromTool(ContentControlBase printControlFromTool)
        {
            PrintControlFromTool = printControlFromTool;
        }

        /// <summary>
        /// 属性更改
        /// </summary>
        /// <param name="printControlFromTool"></param>
        public void UpdatePrintControlFromProperty(ContentControlBase printControlFromTool)
        {
            //TODO
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (PrintControlFromTool != null)
            {
                PrintControlFromToolOfPosition = e.GetPosition(canvasSpace);
            }
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (PrintControlFromTool != null)
            {
                ContentControlBase control = CommonRule.LoadClassInstance<ContentControlBase>(this.GetType().Namespace, PrintControlFromTool.GetType().Name);
                Canvas.SetTop(control, PrintControlFromToolOfPosition.Y);
                Canvas.SetLeft(control, PrintControlFromToolOfPosition.X);
                AddPrinControlEventHandle(control);
                canvasSpace.Children.Add(control);
                //更新放置坐标到控件相应属性
                PropertyModel pX = GetPropertyItemByName(control.Propertys, "pX");
                if (pX != null)
                {
                    pX.Value = Math.Floor(PrintControlFromToolOfPosition.X);
                }
                PropertyModel pY = GetPropertyItemByName(control.Propertys, "pY");
                if (pY != null)
                {
                    pY.Value = Math.Floor(PrintControlFromToolOfPosition.Y);
                }
                CurrentControlBase = control;
                PrintControlFromTool = null;
                this.Cursor = null;
            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            IsMouseEnter = true;
            if (PrintControlFromTool != null)
            {
                this.Cursor = System.Windows.Input.Cursors.Cross;
                PrintControlFromToolOfPosition = e.GetPosition(canvasSpace);
            }
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            IsMouseEnter = false;
            PrintControlFromTool = null;
            this.Cursor = null;
        }

        private void canvasSpace_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void canvasSpace_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            borderBackgroup.ContextMenu = ContentMenuShow.GetPrintCanvasContextMenu();
        }

        private void btnEnlarge_Click(object sender, RoutedEventArgs e)
        {
            if (ZoomableCanvasScale < ScaleList.Count - 1)
            {
                ZoomableCanvasScale++;
                UpdateScale(ScaleList[ZoomableCanvasScale], PrintLable.Width, PrintLable.Height);
            }
        }

        private void btnNarrow_Click(object sender, RoutedEventArgs e)
        {
            if (ZoomableCanvasScale > 0)
            {
                ZoomableCanvasScale--;
                UpdateScale(ScaleList[ZoomableCanvasScale], PrintLable.Width, PrintLable.Height);
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (canvasSpace.Children.Count > 0 || xmlPath != string.Empty)
                {
                    if (System.Windows.MessageBox.Show(string.Format("是否保存当前打印信息?"), "警告", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        if (string.IsNullOrWhiteSpace(xmlPath))
                        {
                            My.SaveFileDialog saveFileDialog = new My.SaveFileDialog();
                            saveFileDialog.Filter = "All(*.xml)|*.xml";
                            if (saveFileDialog.ShowDialog() == My.DialogResult.OK)
                            {
                                xmlPath = saveFileDialog.FileName;
                            }
                            else
                            {
                                return;
                            }
                        }
                        SaveInterfaceToXML(xmlPath);
                    }
                }
                xmlPath = string.Empty;
                AddPrintCanvas a = new AddPrintCanvas(null);
                a.Owner = App.Current.MainWindow;
                a.ShowDialog();
                if (a.DialogResult == true)
                {
                    PrintLable = a.PrintLable;
                    AddOrUpdatePrintLable(PrintLable, true);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(string.Format("新建异常:{0}", ex.Message));
            }
        }

        private void AddOrUpdatePrintLable(PrintLableModel printLable, bool isAdd)
        {
            if (printLable != null)
            {
                if (isAdd)
                {
                    canvasSpace.Children.Clear();
                    borderWork.Width = printLable.Width;
                    borderWork.Height = printLable.Height;
                }
                else
                {
                    borderWork.Width = printLable.Width;
                    borderWork.Height = printLable.Height;
                }
                ZoomableCanvasScale = 4;
                tbScale.Text = string.Format("{0}*", ScaleList[ZoomableCanvasScale]);
            }
        }

        /// <summary>
        /// 配置文件换界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (canvasSpace.Children.Count > 0 || xmlPath != string.Empty)
                {
                    if (System.Windows.MessageBox.Show(string.Format("是否保存当前打印信息?"), "警告", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        if (string.IsNullOrWhiteSpace(xmlPath))
                        {
                            My.SaveFileDialog saveFileDialog = new My.SaveFileDialog();
                            saveFileDialog.Filter = "All(*.xml)|*.xml";
                            if (saveFileDialog.ShowDialog() == My.DialogResult.OK)
                            {
                                xmlPath = saveFileDialog.FileName;
                            }
                            else
                            {
                                return;
                            }
                        }
                        SaveInterfaceToXML(xmlPath);
                    }
                }
                My.OpenFileDialog myDialog = new My.OpenFileDialog();
                myDialog.Filter = "All(*.xml)|*.xml";
                myDialog.CheckFileExists = true;
                myDialog.Multiselect = false;
                if (myDialog.ShowDialog() == My.DialogResult.OK)
                {
                    xmlPath = myDialog.FileName;
                    string errorInfo = string.Empty;
                    PrintFactoryModel p = XmlHelper.GetPrintTemplet(xmlPath, out errorInfo);
                    if (errorInfo != string.Empty)
                    {
                        System.Windows.MessageBox.Show(string.Format("获取配置文件失败:{0}", errorInfo));
                        xmlPath = string.Empty;
                        return;
                    }
                    if (p.PrintItems == null)
                    {
                        System.Windows.MessageBox.Show(string.Format("未获取到有效打印条目."));
                        xmlPath = string.Empty;
                        return;
                    }
                    CreateInterfaceByXML(p);
                }
            }
            catch (Exception ex)
            {
                canvasSpace.Children.Clear();
                System.Windows.MessageBox.Show(string.Format("打开异常:{0}", ex.Message));
            }
        }

        /// <summary>
        /// 根据属性名称获取属性Model
        /// </summary>
        /// <param name="Propertys"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private PropertyModel GetPropertyItemByName(List<PropertyModel> Propertys, string name)
        {
            PropertyModel reValue = null;
            if (Propertys != null)
            {
                reValue = Propertys.FirstOrDefault
                    (
                        p =>
                        {
                            return p.Name == name;
                        }
                    );
            }
            return reValue;
        }


        private void UpdatePrintControlData(PrintItemModel p, ContentControlBase c, bool changeSize)
        {
            List<PropertyModel> Propertys = new List<PropertyModel>();
            if (p.Parameters != null)
            {
                foreach (KeyValuePair<string, string> item in p.Parameters)
                {
                    PropertyModel temp = c.Propertys.FirstOrDefault(x => { return x.Name == item.Key; });
                    if (temp != null)
                    {
                        temp.Value = item.Value;
                    }
                    if (temp.Name == "pX")
                    {
                        Canvas.SetLeft(c, (double)Convert.ChangeType(temp.Value, typeof(double)));
                    }
                    else if (temp.Name == "pY")
                    {
                        Canvas.SetTop(c, (double)Convert.ChangeType(temp.Value, typeof(double)));
                    }
                }
            }
            c.FunctionData = p.FunctionData;
            c.ConttrolData = p.ConttrolData;
            c.PrintKeyValue = p.PrintKeyValue;
            c.DataSourceType = p.DataSourceType;
            c.Index = p.Index;
            c.IsValid = p.IsValid;
            if (changeSize)
            {
                c.Width = p.Width;
                c.Height = p.Height;
            }
            AddPrinControlEventHandle(c);
        }

        private void UpdatePrintControlData(PrintItemModel p, ContentControlBase c)
        {
            List<PropertyModel> Propertys = new List<PropertyModel>();
            if (p.Parameters != null)
            {
                foreach (KeyValuePair<string, string> item in p.Parameters)
                {
                    PropertyModel temp = c.Propertys.FirstOrDefault(x => { return x.Name == item.Key; });
                    if (temp != null)
                    {
                        temp.Value = item.Value;
                    }
                    if (temp.Name == "pX")
                    {
                        Canvas.SetLeft(c, (double)Convert.ChangeType(temp.Value, typeof(double)));
                    }
                    else if (temp.Name == "pY")
                    {
                        Canvas.SetTop(c, (double)Convert.ChangeType(temp.Value, typeof(double)));
                    }
                }
            }
            c.FunctionData = p.FunctionData;
            c.ConttrolData = p.ConttrolData;
            c.PrintKeyValue = p.PrintKeyValue;
            c.DataSourceType = p.DataSourceType;
            c.Index = p.Index;
            c.IsValid = p.IsValid;
            c.Width = p.Width;
            c.Height = p.Height;
            AddPrinControlEventHandle(c);
        }

        /// <summary>
        /// XML创建界面
        /// </summary>
        /// <param name="p"></param>
        private void CreateInterfaceByXML(PrintFactoryModel p)
        {
            //清空界面
            canvasSpace.Children.Clear();
            ZoomableCanvasScale = 4;
            PrintLable.Width = p.Width;
            PrintLable.Height = p.Height;
            UpdateScale(ScaleList[ZoomableCanvasScale], PrintLable.Width, PrintLable.Height);
            tbScale.Text = string.Format("{0}*", ScaleList[ZoomableCanvasScale]);
            foreach (PrintItemModel item in p.PrintItems)
            {
                switch (item.PrintFunctionName)
                {
                    case "PrintBarcode":
                        {
                            BarCodeControl b = new BarCodeControl();
                            UpdatePrintControlData(item, b);
                            PropertyModel o = GetPropertyItemByName(b.Propertys, "pDirec");
                            RotateTransform r = (RotateTransform)b.RenderTransform;
                            if (r != null)
                            {
                                r.CenterX = b.Width / 2;
                                r.CenterY = b.Height / 2;
                                r.Angle = (int)Convert.ChangeType(o.Value, typeof(int)) * 90;
                            }
                            canvasSpace.Children.Add(b);
                            break;
                        }
                    case "PrintTextWorld":
                        {
                            TextWorldControl b = new TextWorldControl();
                            UpdatePrintControlData(item, b);
                            PropertyModel o = GetPropertyItemByName(b.Propertys, "fSpin");
                            RotateTransform r = (RotateTransform)b.RenderTransform;
                            if (r != null)
                            {
                                int flag = (int)Convert.ChangeType(o.Value, typeof(int));
                                if (flag < 5)
                                {
                                    r.CenterX = 0;
                                    r.CenterY = b.Height / 2;
                                    r.Angle = (flag - 1) * 90;
                                }
                                else
                                {
                                    r.CenterX = b.Width / 2;
                                    r.CenterY = b.Height / 2;
                                    r.Angle = (flag - 5) * 90;
                                }
                            }
                            canvasSpace.Children.Add(b);
                            break;
                        }
                    case "PrintPCX":
                        {
                            PictureControl b = new PictureControl();
                            UpdatePrintControlData(item, b, false);
                            canvasSpace.Children.Add(b);
                            break;
                        }
                    case "PrintRectangle":
                        {
                            RectangleControl b = new RectangleControl();
                            UpdatePrintControlData(item, b);
                            PropertyModel thickness = GetPropertyItemByName(b.Propertys, "thickness");
                            if (thickness != null)
                            {
                                Rectangle d = b.Content as Rectangle;
                                if (d != null)
                                {
                                    d.StrokeThickness = (double)Convert.ChangeType(thickness.Value, typeof(double));
                                }
                            }
                            canvasSpace.Children.Add(b);
                            break;
                        }
                    case "PrintLineOr":
                        {
                            LineOrControl b = new LineOrControl();
                            UpdatePrintControlData(item, b);
                            canvasSpace.Children.Add(b);
                            break;
                        }
                    case "PrintBar2DQR":
                        {
                            Bar2DQRControl b = new Bar2DQRControl();
                            UpdatePrintControlData(item, b);
                            b.MaxWidth = b.Width = b.MaxHeight = b.Height = item.Width;
                            PropertyModel temp = b.Propertys.FirstOrDefault(x => { return x.Name == "r"; });
                            if (temp != null)
                            {
                                b.CurrentEnlarge = (int)Convert.ChangeType(temp.Value, typeof(int));
                            }
                            PropertyModel o = GetPropertyItemByName(b.Propertys, "o");
                            RotateTransform r = (RotateTransform)b.RenderTransform;
                            if (r != null)
                            {
                                r.CenterX = b.Width / 2;
                                r.CenterY = b.Height / 2;
                                r.Angle = (int)Convert.ChangeType(o.Value, typeof(int)) * 90;
                            }
                            canvasSpace.Children.Add(b);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// 保存为配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(xmlPath))
                {
                    if (string.IsNullOrWhiteSpace(xmlPath))
                    {
                        My.SaveFileDialog saveFileDialog = new My.SaveFileDialog();
                        saveFileDialog.Filter = "All(*.xml)|*.xml";
                        if (saveFileDialog.ShowDialog() == My.DialogResult.OK)
                        {
                            xmlPath = saveFileDialog.FileName;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                SaveInterfaceToXML(xmlPath);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(string.Format("保存异常:{0}", ex.Message));
            }
        }

        /// <summary>
        /// 命令行加载
        /// </summary>
        /// <param name="fileName"></param>
        public void LoadInterfaceByCommonFileName(string fileName)
        {
            try
            {
                xmlPath = fileName;
                string errorInfo = string.Empty;
                PrintFactoryModel p = XmlHelper.GetPrintTemplet(fileName, out errorInfo);
                if (errorInfo != string.Empty)
                {
                    System.Windows.MessageBox.Show(string.Format("获取配置文件失败:{0}", errorInfo));
                    xmlPath = string.Empty;
                    return;
                }
                if (p.PrintItems == null)
                {
                    System.Windows.MessageBox.Show(string.Format("未获取到有效打印条目."));
                    xmlPath = string.Empty;
                    return;
                }
                CreateInterfaceByXML(p);
            }
            catch (Exception ex)
            {
                xmlPath = string.Empty;
                canvasSpace.Children.Clear();
                System.Windows.MessageBox.Show(string.Format("打开异常:{0}", ex.Message));
            }
        }

        public void SaveInterface()
        {
            if (System.Windows.MessageBox.Show(string.Format("是否保存当前打印信息?"), "警告", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (string.IsNullOrWhiteSpace(xmlPath))
                {
                    if (string.IsNullOrWhiteSpace(xmlPath))
                    {
                        My.SaveFileDialog saveFileDialog = new My.SaveFileDialog();
                        saveFileDialog.Filter = "All(*.xml)|*.xml";
                        if (saveFileDialog.ShowDialog() == My.DialogResult.OK)
                        {
                            xmlPath = saveFileDialog.FileName;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                SaveInterfaceToXML(xmlPath);
            }
        }

        /// <summary>
        /// 保存至XML
        /// </summary>
        private void SaveInterfaceToXML(string path)
        {
            try
            {
                PrintFactoryModel p = GetPrintFactoryInfo();
                string reValue = XmlHelper.SavePrintTemplet(p, path);
                if (reValue != string.Empty)
                {
                    System.Windows.MessageBox.Show(string.Format("保存失败:{0}", reValue));
                }
                else
                {
                    System.Windows.MessageBox.Show(string.Format("保存成功"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("保存打印信息异常,{0}", ex.Message));
            }
        }

        /// <summary>
        /// 获取界面打印信息
        /// </summary>
        /// <returns></returns>
        public PrintFactoryModel GetPrintFactoryInfo()
        {
            string picturResource = string.Empty;
            //打印模板
            PrintFactoryModel p = new PrintFactoryModel();
            //打印条目
            PrintItemModel printItemModel = null;
            //参数集合
            Dictionary<string, string> parameters = null;
            //打印条目集合
            List<PrintItemModel> printItems = new List<PrintItemModel>();
            //工作台Width
            p.Width = Math.Floor(PrintLable.Width);
            //工作台heighht
            p.Height = Math.Floor(PrintLable.Height);
            //获取打印条目
            foreach (UIElement item in canvasSpace.Children)
            {
                if (item is ContentControlBase)
                {
                    ContentControlBase c = item as ContentControlBase;
                    printItemModel = new PrintItemModel()
                    {
                        PrintCaption = c.PrintCaption,
                        PrintFunctionName = c.PrintFunctionName,
                        PrintKeyValue = c.PrintKeyValue,
                        DataSourceType = c.DataSourceType,
                        Index = c.Index,
                        IsValid = c.IsValid,
                        ConttrolData = c.ConttrolData,
                        FunctionData = c.FunctionData,
                        Width = c.Width.IsNaN() ? c.ActualWidth : c.Width,
                        Height = c.Height.IsNaN() ? c.ActualHeight : c.Height
                    };
                    if (c.Propertys != null && c.Propertys.Count > 0)
                    {
                        parameters = new Dictionary<string, string>();
                        foreach (PropertyModel pro in c.Propertys)
                        {
                            parameters.Add(pro.Name, (string)Convert.ChangeType(pro.Value, typeof(string)));
                        }
                    }
                    printItemModel.Parameters = parameters;
                    printItems.Add(printItemModel);
                    if (item is PictureControl)
                    {
                        string fileName = System.IO.Path.GetFileName(c.PrintKeyValue);
                        if (!picturResource.Contains(fileName))
                        {
                            picturResource += string.Format("{0};", System.IO.Path.GetFileName(c.PrintKeyValue));
                        }
                    }
                }
            }
            p.PicturResource = picturResource;
            p.PrintItems = printItems;
            return p;
        }

        /// <summary>
        /// 工作区获取了焦点才会接收键盘事件 
        /// 本案中打印子控件不具有接收焦点的能力,因此增加tbFocus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (CurrentControlBase != null)
                {
                    canvasSpace.Children.Remove(CurrentControlBase);
                    OnPrintControlPropertyEvent(null, null);
                    CurrentControlBase = null;
                }
            }
        }

        #region 矩形选择框
        private bool IsMouseDown = false;

        /// <summary>
        /// 开始点
        /// </summary>
        private Point StartPoint = new Point();

        /// <summary>
        /// 结束点
        /// </summary>
        private Point EndPoint = new Point();

        /// <summary>
        /// 矩形框
        /// </summary>
        private Rectangle SelectRectangle = new Rectangle()
        {
            StrokeDashArray = new DoubleCollection() { 3 },
            Stroke = new SolidColorBrush(Colors.Brown),
        };

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown)
            {
                EndPoint = e.GetPosition(canvasSpace);
                SelectRectangle.Width = Math.Abs(EndPoint.X - StartPoint.X);
                SelectRectangle.Height = Math.Abs(EndPoint.Y - StartPoint.Y);
                Canvas.SetTop(SelectRectangle, Math.Min(EndPoint.Y, StartPoint.Y));
                Canvas.SetLeft(SelectRectangle, Math.Min(EndPoint.X, StartPoint.X));
                if (!canvasSpace.Children.Contains(SelectRectangle))
                {
                    canvasSpace.Children.Add(SelectRectangle);
                }
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //初始化 所有控件处于不选中状态
            IsMouseDown = true;
            StartPoint = e.GetPosition(canvasSpace);
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsMouseDown = false;
            EndPoint = e.GetPosition(canvasSpace);
            //进行选中矩形框中元素等操作
            //注意根据StartPoint和EndPoint提取最小点和最大点
            //最小点:(Min(StartPoint.X,EndPoint.X),Min(StartPoint.Y,EndPoint.Y))
            //最大点:(Max(StartPoint.X,EndPoint.X),Max(StartPoint.Y,EndPoint.Y))
            //TODO
            if (canvasSpace.Children.Contains(SelectRectangle))
            {
                canvasSpace.Children.Remove(SelectRectangle);
            }
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;
            EndPoint = e.GetPosition(canvasSpace);
            //进行选中矩形框中元素等操作
            //TODO
            if (canvasSpace.Children.Contains(SelectRectangle))
            {
                canvasSpace.Children.Remove(SelectRectangle);
            }
        }
        #endregion

        private void BaseUserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            tbFocus.Focus();
        }
    }
}
