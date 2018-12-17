using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using PrintStudioModel;
using System.ComponentModel;

namespace PrintStudioModel
{
    public class ContentControlBase : ContentControl, INotifyPropertyChanged
    {
        #region XML数据
        private string _printCaption = string.Empty;
        /// <summary>
        /// 条码Caption
        /// </summary>
        public string PrintCaption
        {
            get { return _printCaption; }
            set { _printCaption = value; }
        }

        private string _printFunctionName = string.Empty;
        /// <summary>
        /// 打印函数名称
        /// </summary>
        public string PrintFunctionName { get { return _printFunctionName; } set { _printFunctionName = value; } }

        protected string _printkeyValue = string.Empty;
        /// <summary>
        /// 打印关键值
        /// </summary>
        public virtual string PrintKeyValue { get { return _printkeyValue; } set { _printkeyValue = value; OnPropertyChanged("PrintKeyValue"); } }

        /// <summary>
        /// 关键值数据源
        /// </summary>
        public int DataSourceType { get; set; }

        /// <summary>
        /// 索引 注意由代码创建 保证唯一性
        /// </summary>
        public int Index { get; set; }

        private bool _isValid = true;
        /// <summary>
        /// 对否有效
        /// </summary>
        public bool IsValid { get { return _isValid; } set { _isValid = value; OnPropertyChanged("IsValid"); } }

        private ConttrolDataItemModel _ConttrolData = new ConttrolDataItemModel();
        /// <summary>
        /// 界面数据
        /// </summary>
        public ConttrolDataItemModel ConttrolData { get { return _ConttrolData; } set { _ConttrolData = value; } }

        private FunctionDataItemModel _FunctionData = new FunctionDataItemModel();
        /// <summary>
        /// 方法数据
        /// </summary>
        public FunctionDataItemModel FunctionData { get { return _FunctionData; } set { _FunctionData = value; } }

        /// <summary>
        /// 控件属性集合
        /// </summary>
        public List<PropertyModel> Propertys { get; set; }
        #endregion

        #region 其他
        /// <summary>
        /// 右键菜单事件
        /// </summary>
        public event EventHandler<ContentMenuEventArgs> OnContentMenuEvent = null;

        /// <summary>
        /// 鼠标拖动事件
        /// </summary>
        public event MouseEventHandler OnMouseMoveEvent = null;

        /// <summary>
        /// 右键菜单
        /// </summary>
        public ContentMenuShow ContentMenuShow = new ContentMenuShow();

        /// <summary>
        /// 鼠标左键Up事件
        /// </summary>
        public event MouseButtonEventHandler OnMouseLeftButtonUpEvent = null;

        /// <summary>
        /// 鼠标左键按在事件
        /// </summary>
        public event MouseButtonEventHandler OnMouseLeftButtonDownEvent = null;

        /// <summary>
        /// 是否显示右键菜单
        /// </summary>
        public bool IsShowContentMenu
        {
            get { return (bool)GetValue(IsShowContentMenuProperty); }
            set { SetValue(IsShowContentMenuProperty, value); }
        }

        public static readonly DependencyProperty IsShowContentMenuProperty = DependencyProperty.Register("IsShowContentMenu", typeof(bool), typeof(ContentControlBase), new UIPropertyMetadata(true));

        public ContentControlBase()
        {
            this.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(Element_MouseLeftButtonDown), true);
            this.AddHandler(UIElement.MouseMoveEvent, new MouseEventHandler(Element_MouseMove), true);
            this.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Element_MouseLeftButtonUp), true);
            this.AddHandler(UIElement.MouseRightButtonDownEvent, new MouseButtonEventHandler(Element_MouseRightButtonDown), true);
            this.AddHandler(UIElement.MouseRightButtonUpEvent, new MouseButtonEventHandler(Element_MouseRightButtonUp), true);
            ContentMenuShow.MenuItemClicked += new RoutedEventHandler(ContentMenuShow_MenuItemClicked);
            //代码添加资源
            ResourceDictionary resource = (ResourceDictionary)Application.LoadComponent(new Uri("/CommonPrintStudio;component/Style/Style.xaml", UriKind.RelativeOrAbsolute));
            this.Resources.MergedDictionaries.Add(resource);
        }

        public override void EndInit()
        {
            base.EndInit();
        }

        private void ContentMenuShow_MenuItemClicked(object sender, RoutedEventArgs e)
        {
            if (OnContentMenuEvent != null)
            {
                OnContentMenuEvent(sender, new ContentMenuEventArgs() { Source = this, MenuItem = (MenuItem)sender });
            }
        }

        void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (OnMouseMoveEvent != null)
            {
                OnMouseMoveEvent(sender, e);
            }
        }

        void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (OnMouseLeftButtonDownEvent != null)
            {
                OnMouseLeftButtonDownEvent(sender, e);
            }
        }

        void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (OnMouseLeftButtonUpEvent != null)
            {
                OnMouseLeftButtonUpEvent(sender, e);
            }
        }

        void Element_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        void Element_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsShowContentMenu)
            {
                this.ContextMenu = ContentMenuShow.GetPrintItemContextMenu();
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
