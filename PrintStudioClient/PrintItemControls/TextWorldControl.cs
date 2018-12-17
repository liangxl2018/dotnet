using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using PrintStudioModel;

namespace CommonPrintStudio
{
    public class TextWorldControl : ContentControlBase
    {
        /// <summary>
        /// 文本标签
        /// </summary>
        private Label lable = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TextWorldControl()
        {
            Width = 175;
            MinWidth = 1;
            Height = 40;
            MinHeight = 1;
            Canvas.SetTop(this, 150);
            Canvas.SetLeft(this, 150);
            lable = new Label() { Padding = new Thickness(0), Style = this.FindResource("LabelStyle1") as Style, FontSize = 32, FontWeight = FontWeights.Normal, Content = "Hello World" };
            Viewbox v = new Viewbox() { Child = lable, HorizontalAlignment = System.Windows.HorizontalAlignment.Left, VerticalAlignment = System.Windows.VerticalAlignment.Stretch };
            this.Content = v;
            this.Template = this.FindResource("DesignerItemTemplate") as ControlTemplate;
            this.RenderTransform = new RotateTransform() { Angle = 0, CenterX = 75, CenterY = 20 };
            InitPropertyList();
        }

        /// <summary>
        /// 打印值 可更具文字多少调整控件长度 暂未实现
        /// </summary>
        public override string PrintKeyValue
        {
            get
            {
                return base._printkeyValue;
            }
            set
            {
                base._printkeyValue = value;
                lable.Content = _printkeyValue;
                OnPropertyChanged("PrintKeyValue");
            }
        }
        private void InitPropertyList()
        {
            Index = new Random().Next(100000000);
            PrintCaption = "文字";
            PrintFunctionName = "PrintTextWorld";
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
                Name = "pHeight",
                Caption = "字型高度",
                Value = 40,
                PropertyType = PropertyType.Int,
                ComboBoxData = null,
                ComboBoxValueType = 0,
                PropertyChanged = OnHeightChanged
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "fWidth",
                Caption = "字型宽度",
                Value = 0,
                PropertyType = PropertyType.Int,
                ComboBoxData = null,
                ComboBoxValueType = 0
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "fType",
                Caption = "字型名称",
                Value = "Arial",
                PropertyType = PropertyType.Combobox,
                ComboBoxData = new List<KeyCaption>() 
                { 
                    new KeyCaption(){ID=1,Caption="Arial"},
                    new KeyCaption(){ID=2,Caption="宋体"},
                    new KeyCaption(){ID=3,Caption="ZEBRA0"},
                },
                ComboBoxValueType = 0
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "fSpin",
                Caption = "旋转角度",
                Value = 1,
                PropertyType = PropertyType.Combobox,
                ComboBoxData = new List<KeyCaption>() 
                { 
                    new KeyCaption(){ID=1,Caption="居左0度"},
                    new KeyCaption(){ID=2,Caption="居左90度"},
                    new KeyCaption(){ID=3,Caption="居左180度"},
                    new KeyCaption(){ID=4,Caption="居左270度"},
                    new KeyCaption(){ID=5,Caption="居中0度"},
                    new KeyCaption(){ID=6,Caption="居中90度"},
                    new KeyCaption(){ID=7,Caption="居中180度"},
                    new KeyCaption(){ID=8,Caption="居中270度"},
                },
                ComboBoxValueType = 1,
                PropertyChanged = OnSpinChanged
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "fWeight",
                Caption = "字体粗细",
                Value = 400,
                PropertyType = PropertyType.Combobox,
                ComboBoxData = new List<KeyCaption>() 
                { 
                    new KeyCaption(){ID=400,Caption="标准"},
                    new KeyCaption(){ID=100,Caption="非常细"},
                    new KeyCaption(){ID=200,Caption="极细"},
                    new KeyCaption(){ID=300,Caption="细"},
                    new KeyCaption(){ID=500,Caption="中等"},
                    new KeyCaption(){ID=600,Caption="半粗"},
                    new KeyCaption(){ID=700,Caption="粗"},
                    new KeyCaption(){ID=800,Caption="特粗"},
                },
                ComboBoxValueType = 1
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "fItalic",
                Caption = "斜体",
                Value = 0,
                PropertyType = PropertyType.CheckBox,
                ComboBoxData = null,
                ComboBoxValueType = 0
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "fUnline",
                Caption = "文字底线",
                Value = 0,
                PropertyType = PropertyType.CheckBox,
                ComboBoxData = null,
                ComboBoxValueType = 0
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "fStrikeOut",
                Caption = "删除线",
                Value = 0,
                PropertyType = PropertyType.CheckBox,
                ComboBoxData = null,
                ComboBoxValueType = 0
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "idName",
                Caption = "识别名称",
                Value = "Text",
                PropertyType = PropertyType.String,
                ComboBoxData = null,
                ComboBoxValueType = 0
            };
            Propertys.Add(p);
        }

        /// <summary>
        /// 打印值改变
        /// </summary>
        /// <param name="property"></param>
        private void OnPrintKeyValueChanged(PropertyChangedFromTextBoxEventArgs property)
        {
            PropertyModel p = property.Property;
            PrintKeyValue = (string)Convert.ChangeType(p.Value, typeof(string));
        }

        /// <summary>
        /// 高度值改变
        /// </summary>
        /// <param name="property"></param>
        private void OnHeightChanged(PropertyChangedFromTextBoxEventArgs property)
        {
            PropertyModel p = property.Property;
            TextBox textBox = property.TextBox;
            TextWorldControl c = (TextWorldControl)property.PrintControl;
            double ex = (double)Convert.ChangeType(p.Value, typeof(double));
            if (ex < c.MinHeight)
            {
                ex = c.MinHeight;
            }
            double h = c.Height;
            c.Height = ex;
            c.Width = c.Width * (c.Height / h);
            p.Value = ex;
            if (textBox != null)
            {
                textBox.Text = ex.ToString();
            }
        }

        private void OnSpinChanged(PropertyChangedFromTextBoxEventArgs property)
        {
            PropertyModel p = property.Property;
            TextWorldControl c = (TextWorldControl)property.PrintControl;
            int flag = (int)p.Value;
            RotateTransform r = (RotateTransform)c.RenderTransform;
            if (r != null)
            {
                if (flag < 5)
                {
                    r.CenterX = 0;
                    r.CenterY = c.Height / 2;
                    r.Angle = (flag - 1) * 90;
                }
                else
                {
                    r.CenterX = c.Width / 2;
                    r.CenterY = c.Height / 2;
                    r.Angle = (flag - 5) * 90;
                }
            }
        }
    }
}
