using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PrintStudioModel;
using PrintStudioRule;
using ZXing.Common;
using ZXing;

namespace CommonPrintStudio
{
    public class Bar2DQRControl : ContentControlBase
    {
        public override void EndInit()
        {
            base.EndInit();
            //工具箱中控件 反射创建该控件时 不会触发EndInit
            if (EncodingOptionsHeight == 200 && EncodingOptionsWidth == 200)
            {
                img.Source = null;
                img.Source = ImageHelper.ChangeBitmapToImageSource(BarCodeHelper.CreateBarCode(_printkeyValue, BarcodeFormat.QR_CODE, new EncodingOptions() { Width = EncodingOptionsWidth, Height = EncodingOptionsHeight, PureBarcode = true, Margin = EncodingOptionsMargin }));
                MinHeight = MaxHeight = Height = MinWidth = MaxWidth = Width = (img.Source.Width);
            }
        }
        /// <summary>
        /// Margin
        /// </summary>
        public int EncodingOptionsMargin
        {
            get { return (int)GetValue(EncodingOptionsMarginProperty); }
            set { SetValue(EncodingOptionsMarginProperty, value); }
        }

        public static readonly System.Windows.DependencyProperty EncodingOptionsMarginProperty = System.Windows.DependencyProperty.Register("EncodingOptionsMargin", typeof(int), typeof(Bar2DQRControl), new System.Windows.PropertyMetadata(0, new System.Windows.PropertyChangedCallback((sender, e) => { })));

        /// <summary>
        /// Height
        /// </summary>
        public int EncodingOptionsHeight
        {
            get { return (int)GetValue(EncodingOptionsWidthProperty); }
            set { SetValue(EncodingOptionsWidthProperty, value); }
        }

        public static readonly System.Windows.DependencyProperty EncodingOptionsHeightProperty = System.Windows.DependencyProperty.Register("EncodingOptionsHeight", typeof(int), typeof(Bar2DQRControl), new System.Windows.PropertyMetadata(21, new System.Windows.PropertyChangedCallback((sender, e) => { })));

        /// <summary>
        /// Width
        /// </summary>
        public int EncodingOptionsWidth
        {
            get { return (int)GetValue(EncodingOptionsWidthProperty); }
            set { SetValue(EncodingOptionsWidthProperty, value); }
        }

        public static readonly System.Windows.DependencyProperty EncodingOptionsWidthProperty = System.Windows.DependencyProperty.Register("EncodingOptionsWidth", typeof(int), typeof(Bar2DQRControl), new System.Windows.PropertyMetadata(21, new System.Windows.PropertyChangedCallback((sender, e) => { })));

        private Image img = null;

        public Bar2DQRControl()
        {
            Width = 1;
            MaxWidth = 1;
            MinWidth = 1;
            Height = 1;
            MaxHeight = 1;
            MinHeight = 1;
            Canvas.SetTop(this, 150);
            Canvas.SetLeft(this, 150);
            img = new Image() { Stretch = Stretch.Fill, HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch, VerticalAlignment = System.Windows.VerticalAlignment.Stretch, Source = new BitmapImage(new Uri(string.Format("/CommonPrintStudio;component/Style/Bar2DQR.png"), UriKind.RelativeOrAbsolute)) };
            this.Content = img;
            this.Template = this.FindResource("DesignerItemTemplate") as ControlTemplate;
            InitPropertyList();
            this.RenderTransform = new RotateTransform() { Angle = 0, CenterX = 0.5, CenterY = 0.5 };
        }

        /// <summary>
        /// 动态创建QR二维码
        /// </summary>
        public override string PrintKeyValue
        {
            get { return _printkeyValue; }
            set
            {
                _printkeyValue = value;
                if (string.IsNullOrEmpty(_printkeyValue))
                {
                    _printkeyValue = " ";
                }
                if (_printkeyValue != null)
                {
                    img.Source = null;
                    img.Source = ImageHelper.ChangeBitmapToImageSource(BarCodeHelper.CreateBarCode(_printkeyValue, BarcodeFormat.QR_CODE, new EncodingOptions() { Width = EncodingOptionsWidth, Height = EncodingOptionsHeight, PureBarcode = true, Margin = EncodingOptionsMargin }));
                    MinHeight = MaxHeight = Height = MinWidth = MaxWidth = Width = CurrentEnlarge * (img.Source.Width);
                }
                OnPropertyChanged("PrintKeyValue");
            }
        }

        private int currentEnlarge = 3;
        /// <summary>
        /// 当前放大倍数
        /// </summary>
        public int CurrentEnlarge
        {
            get { return currentEnlarge; }
            set
            {
                currentEnlarge = value;
                MinHeight = MaxHeight = Height = MinWidth = MaxWidth = Width = CurrentEnlarge * (img.Source.Width);
            }
        }

        private void InitPropertyList()
        {
            Index = new Random().Next(100000000);
            PrintCaption = "QR二维码";
            PrintFunctionName = "PrintBar2DQR";
            PrintKeyValue = "Hello World";
            PropertyModel p = null;
            Propertys = new List<PrintStudioModel.PropertyModel>();
            p = new PropertyModel()
            {
                Name = "PrintKeyValue",
                Caption = "打印值",
                Value = "Hello World",
                PropertyType = PropertyType.String,
                ComboBoxData = null,
                ComboBoxValueType = 0,
                PropertyChanged = OnPrintKeyValueChanged,
                SupportChinese = true
            };
            PrintKeyValue = (string)p.Value;
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "pX",
                Caption = "X坐标",
                Value = 150,
                PropertyType = PropertyType.Int,
                ComboBoxData = null,
                ComboBoxValueType = 0
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "pY",
                Caption = "Y坐标",
                Value = 150,
                PropertyType = PropertyType.Int,
                ComboBoxData = null,
                ComboBoxValueType = 0
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "w",
                Caption = "最大列印宽度",
                Value = 1,
                PropertyType = PropertyType.Int,
                ComboBoxData = null,
                ComboBoxValueType = 1
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "v",
                Caption = "最大列印度高",
                Value = 1,
                PropertyType = PropertyType.Int,
                ComboBoxData = null,
                ComboBoxValueType = 0
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "o",
                Caption = "旋转方向",
                Value = 0,
                PropertyType = PropertyType.Combobox,
                ComboBoxData = new List<KeyCaption>() 
                { 
                    new KeyCaption(){ID=0,Caption="不旋转"},
                    new KeyCaption(){ID=1,Caption="旋转90度"},
                    new KeyCaption(){ID=2,Caption="旋转180度"},
                    new KeyCaption(){ID=3,Caption="旋转270度"},
                },
                ComboBoxValueType = 1,
                PropertyChanged = OnRotateChanged
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "type",
                Caption = "二维码类型",
                Value = 0,
                PropertyType = PropertyType.Combobox,
                ComboBoxData = new List<KeyCaption>() 
                { 
                    new KeyCaption(){ID=0,Caption="QRCode"},
                    new KeyCaption(){ID=1,Caption="DataMatrix"},
                },
                ComboBoxValueType = 1,
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "r",
                Caption = "放大倍数",
                Value = 3,
                PropertyType = PropertyType.Combobox,
                ComboBoxData = new List<KeyCaption>() 
                { 
                    new KeyCaption(){ID=1,Caption="1"},
                    new KeyCaption(){ID=2,Caption="2"},
                    new KeyCaption(){ID=3,Caption="3"},
                    new KeyCaption(){ID=4,Caption="4"},
                    new KeyCaption(){ID=5,Caption="5"},
                    new KeyCaption(){ID=6,Caption="6"},
                    new KeyCaption(){ID=7,Caption="7"},
                    new KeyCaption(){ID=8,Caption="8"},
                    new KeyCaption(){ID=9,Caption="9"},
                },
                ComboBoxValueType = 1,
                PropertyChanged = OnAmplificationChangd
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "m",
                Caption = "编码模式",
                Value = 4,
                PropertyType = PropertyType.Combobox,
                ComboBoxData = new List<KeyCaption>() 
                {
                    new KeyCaption(){ID=0,Caption="数字模式"},
                    new KeyCaption(){ID=1,Caption="数字字母模式"},
                    new KeyCaption(){ID=2,Caption="字节模式"},
                    new KeyCaption(){ID=3,Caption="中国汉字模式"},
                    new KeyCaption(){ID=4,Caption="混合模式"},
                }
                ,
                ComboBoxValueType = 1
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "g",
                Caption = "纠错等级",
                Value = 1,
                PropertyType = PropertyType.Combobox,
                ComboBoxData = new List<KeyCaption>() 
                {
                      new KeyCaption(){ID=0,Caption="L"},
                    new KeyCaption(){ID=1,Caption="M"},
                    new KeyCaption(){ID=2,Caption="Q1"},
                    new KeyCaption(){ID=3,Caption="H1"},
                },
                ComboBoxValueType = 1
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "s",
                Caption = "掩模图形",
                Value = 0,
                PropertyType = PropertyType.Combobox,
                ComboBoxData = new List<KeyCaption>() 
                {
                    new KeyCaption(){ID=0,Caption="000"},
                    new KeyCaption(){ID=1,Caption="001"},
                    new KeyCaption(){ID=2,Caption="010"},
                    new KeyCaption(){ID=3,Caption="011"},
                    new KeyCaption(){ID=4,Caption="100"},
                    new KeyCaption(){ID=5,Caption="101"},
                    new KeyCaption(){ID=6,Caption="110"},
                    new KeyCaption(){ID=7,Caption="111"},
                    new KeyCaption(){ID=8,Caption="自动选择"},
                },
                ComboBoxValueType = 1
            };
            Propertys.Add(p);
        }

        private void OnRotateChanged(PropertyChangedFromTextBoxEventArgs property)
        {
            PropertyModel p = property.Property;
            Bar2DQRControl c = (Bar2DQRControl)property.PrintControl;
            int flag = (int)p.Value;
            RotateTransform r = (RotateTransform)c.RenderTransform;
            if (r != null)
            {
                r.CenterX = c.Width / 2;
                r.CenterY = c.Height / 2;
                r.Angle = (flag) * 90;
            }
        }

        private void OnAmplificationChangd(PropertyChangedFromTextBoxEventArgs property)
        {
            PropertyModel p = property.Property;
            Bar2DQRControl c = (Bar2DQRControl)property.PrintControl;
            int e = (int)Convert.ChangeType(p.Value, typeof(int));
            c.MinHeight = c.MaxHeight = c.Height = c.MinWidth = c.MaxWidth = c.Width = e * (1 + c.PrintKeyValue.Length);
            c.CurrentEnlarge = e;
        }

        private void OnPrintKeyValueChanged(PropertyChangedFromTextBoxEventArgs property)
        {
            PropertyModel p = property.Property;
            Bar2DQRControl c = (Bar2DQRControl)property.PrintControl;
            c.PrintKeyValue = (string)p.Value;
        }
    }
}
