using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using PrintStudioModel;
using System.Windows;
using PrintStudioRule;
using ZXing.Common;
using ZXing;

namespace CommonPrintStudio
{
    public class BarCodeControl : ContentControlBase
    {
        private Image img = null;

        public BarCodeControl()
        {
            Width = 156;
            MinWidth = 1;
            Height = 40;
            MinHeight = 1;
            Canvas.SetTop(this, 150);
            Canvas.SetLeft(this, 150);
            img = new Image() { Stretch = Stretch.Fill, HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch, VerticalAlignment = System.Windows.VerticalAlignment.Stretch, Source = new BitmapImage(new Uri(string.Format("/CommonPrintStudio;component/Style/BarCode.png"), UriKind.RelativeOrAbsolute)) };
            Grid g = new Grid();
            g.RowDefinitions.Add(new RowDefinition() { });
            g.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            g.Children.Add(img);
            Grid.SetRow(img, 0);
            //注意字体随条码高度变化,这里不明确变化规则,如有必要,调试或咨询厂商后再处理.
            //这里的高度是条码加文字.如果显示文字,那么条码高度需要按比例(不明确)提取.
            //果显示文字,当放大或属性设置高度后,均需要设置文本字体大小.由高度转化.
            //基于此,显示/不显示,文本属性,暂不可设.因此在解析xml或生成xml时,均暂不考虑.
            TextBlock t = new TextBlock() { Visibility = Visibility.Collapsed, Name = "pText", Text = "Hello World" };
            g.Children.Add(t);
            Grid.SetRow(t, 1);
            this.Content = g;
            this.Template = this.FindResource("DesignerItemTemplate") as ControlTemplate;
            InitPropertyList();
            this.RenderTransform = new RotateTransform() { Angle = 0, CenterX = 100, CenterY = 25 };
        }

        public override string PrintKeyValue
        {
            get
            {
                return base.PrintKeyValue;
            }
            set
            {
                base.PrintKeyValue = value;
                if(string.IsNullOrEmpty(_printkeyValue))
                {
                    _printkeyValue=" ";
                }
                if (_printkeyValue != null)
                {
                    img.Source = null;
                    byte[] t = Encoding.GetEncoding("gb2312").GetBytes(_printkeyValue);
                    for (int i = 0; i < t.Length; i++)
                    {
                        t[i] = (byte)(t[i] & 0x7f);
                    }
                    string temp = Encoding.GetEncoding("gb2312").GetString(t);
                    img.Source = ImageHelper.ChangeBitmapToImageSource(BarCodeHelper.CreateBarCode(temp, BarcodeFormat.CODE_128, new EncodingOptions() { Width = 46, Height = 1, PureBarcode = true, Margin = 0 }));
                    Width = (img.Source.Width)+10;
                }
                OnPropertyChanged("PrintKeyValue");
            }
        }

        private void InitPropertyList()
        {
            Index = new Random().Next(100000000);
            PrintCaption = "一维码";
            PrintFunctionName = "PrintBarcode";
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
                PropertyChanged = OnPrintKeyValueChanged
            };
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
                Name = "pDirec",
                Caption = "打印方向",
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
                PropertyChanged = OnDirecChanged
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "pCode",
                Caption = "条码类型",
                Value = "1",
                PropertyType = PropertyType.Combobox,
                ComboBoxData = new List<KeyCaption>() 
                { 
                    new KeyCaption(){ID=0,Caption="0"},
                    new KeyCaption(){ID=1,Caption="1"},
                    new KeyCaption(){ID=2,Caption="1A"},
                    new KeyCaption(){ID=3,Caption="1B"},
                    new KeyCaption(){ID=4,Caption="1C"},
                    new KeyCaption(){ID=5,Caption="1E"},
                    new KeyCaption(){ID=6,Caption="2"},
                    new KeyCaption(){ID=7,Caption="2C"},
                    new KeyCaption(){ID=8,Caption="2D"},
                    new KeyCaption(){ID=9,Caption="2G"},
                    new KeyCaption(){ID=10,Caption="2M"},
                    new KeyCaption(){ID=11,Caption="2U"},
                    new KeyCaption(){ID=12,Caption="3"},
                    new KeyCaption(){ID=13,Caption="3C"},
                    new KeyCaption(){ID=14,Caption="3E"},
                    new KeyCaption(){ID=15,Caption="3F"},
                    new KeyCaption(){ID=16,Caption="9"},
                    new KeyCaption(){ID=17,Caption="E30"},
                    new KeyCaption(){ID=18,Caption="E32"},
                    new KeyCaption(){ID=19,Caption="E35"},
                    new KeyCaption(){ID=20,Caption="E80"},
                    new KeyCaption(){ID=21,Caption="E82"},
                    new KeyCaption(){ID=22,Caption="E-85"},
                    new KeyCaption(){ID=23,Caption="K"},
                    new KeyCaption(){ID=24,Caption="P"},
                    new KeyCaption(){ID=25,Caption="UA0"},
                    new KeyCaption(){ID=26,Caption="UA2"},
                    new KeyCaption(){ID=27,Caption="UA5"},
                    new KeyCaption(){ID=28,Caption="UE0"},
                    new KeyCaption(){ID=29,Caption="UE2"},
                    new KeyCaption(){ID=30,Caption="UE5"},
                },
                ComboBoxValueType = 0,
                PropertyChanged = OnCodeChanged
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "narrowWidth",
                Caption = "窄单元的宽度",
                Value = 2,
                PropertyType = PropertyType.Int,
                ComboBoxData = null,
                ComboBoxValueType = 0
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "pHorizontal",
                Caption = "宽单元的宽度",
                Value = 2,
                PropertyType = PropertyType.Int,
                ComboBoxData = null,
                ComboBoxValueType = 0
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "pHeight",
                Caption = "条码高度",
                Value = 40,
                PropertyType = PropertyType.Int,
                ComboBoxData = null,
                ComboBoxValueType = 1,
                PropertyChanged = OnHeightChanged
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "pText",
                Caption = "打印条码文字",
                Value = 78,
                PropertyType = PropertyType.Combobox,
                ComboBoxData = new List<KeyCaption>() 
                { 
                    new KeyCaption(){ID=66,Caption="显示"},
                    new KeyCaption(){ID=78,Caption="不显示"},
                },
                ComboBoxValueType = 1,
                IsDisplayInPropertyControl = false,
                PropertyChanged = OnTextChanged
            };
            Propertys.Add(p);
        }

        private void OnDirecChanged(PropertyChangedFromTextBoxEventArgs property)
        {
            PropertyModel p = property.Property;
            BarCodeControl c = (BarCodeControl)property.PrintControl;
            RotateTransform r = (RotateTransform)c.RenderTransform;
            if (r != null)
            {
                r.CenterX = c.Width / 2;
                r.CenterY = c.Height / 2;
                r.Angle = ((int)p.Value) * 90;
            }
        }

        private void OnCodeChanged(PropertyChangedFromTextBoxEventArgs property)
        {
            //由于不清楚具体类型条码的样式变化,因此这里不作精准性处理
        }

        private void OnHeightChanged(PropertyChangedFromTextBoxEventArgs property)
        {
            PropertyModel p = property.Property;
            BarCodeControl c = (BarCodeControl)property.PrintControl;
            c.Height = (double)Convert.ChangeType(p.Value, typeof(double));
        }

        private void OnTextChanged(PropertyChangedFromTextBoxEventArgs property)
        {
            PropertyModel p = property.Property;
            BarCodeControl c = (BarCodeControl)property.PrintControl;
            DependencyObject d = DependencyHelper.FindVisualChildByName(c, p.Name);
            if (d != null)
            {
                Type t = d.GetType();
                if (t == typeof(TextBlock))
                {
                    TextBlock r = d as TextBlock;
                    int i = (int)Convert.ChangeType(p.Value, typeof(int));
                    if (i == 66)
                    {
                        r.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        r.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void OnPrintKeyValueChanged(PropertyChangedFromTextBoxEventArgs property)
        {
            BarCodeControl c = (BarCodeControl)property.PrintControl;
            PropertyModel p = property.Property;
            TextBox textBox = property.TextBox;
            DependencyObject d = DependencyHelper.FindVisualChildByName(c, "pText");
            if (d != null)
            {
                Type t = d.GetType();
                if (t == typeof(TextBlock))
                {
                    TextBlock r = d as TextBlock;
                    string value = (string)Convert.ChangeType(p.Value, typeof(string));
                    if (CommonRule.HasChinese(p.Value.ToString()))
                    {
                        p.Value = c.PrintKeyValue;
                        textBox.Text = c.PrintKeyValue;
                        return;
                    }
                    r.Text = value;
                    c.PrintKeyValue = r.Text;
                }
            }
        }
    }
}
