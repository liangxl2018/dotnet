using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using PrintStudioModel;

namespace CommonPrintStudio
{
    public class LineOrControl : ContentControlBase
    {
        public LineOrControl()
        {
            Width = 200;
            MinWidth = 1;
            Height = 1;
            MinHeight = 1;
            Canvas.SetTop(this, 150);
            Canvas.SetLeft(this, 150);
            this.Content = new Rectangle() { StrokeThickness = 1, Stroke = new SolidColorBrush(Colors.Black), Fill = new SolidColorBrush(Colors.Black) };
            this.Template = this.FindResource("DesignerItemTemplate") as ControlTemplate;
            InitPropertyList();
        }

        private void InitPropertyList()
        {
            Index = new Random().Next(100000000);
            PrintCaption = "直线";
            PrintFunctionName = "PrintLineOr";
            PrintKeyValue = string.Empty;
            PropertyModel p = null;
            Propertys = new List<PrintStudioModel.PropertyModel>();
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
                Name = "pWidth",
                Caption = "水平长度",
                Value = 200,
                PropertyType = PropertyType.Int,
                ComboBoxData = null,
                ComboBoxValueType = 0,
                PropertyChanged = OnWidthChanged
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "pHeight",
                Caption = "垂直高度",
                Value = 2,
                PropertyType = PropertyType.Int,
                ComboBoxData = null,
                ComboBoxValueType = 0,
                PropertyChanged = OnHeightChanged
            };
            Propertys.Add(p);
        }

        private void OnWidthChanged(PropertyChangedFromTextBoxEventArgs property)
        {
            TextBox textBox = property.TextBox;
            PropertyModel p = property.Property;
            LineOrControl c = (LineOrControl)property.PrintControl;
            double ex = (double)Convert.ChangeType(p.Value, typeof(double));
            if (ex < c.MinWidth)
            {
                ex = c.MinWidth;
            }
            c.Width = ex;
            p.Value = ex;
            if (textBox != null)
            {
                textBox.Text = ex.ToString();
            }
        }

        private void OnHeightChanged(PropertyChangedFromTextBoxEventArgs property)
        {
            LineOrControl c = (LineOrControl)property.PrintControl;
            PropertyModel p = property.Property;
            TextBox textBox = property.TextBox;
            double ex = (double)Convert.ChangeType(p.Value, typeof(double));
            if (ex < c.MinHeight)
            {
                ex = c.MinHeight;
            }
            c.Height = ex;
            p.Value = ex;
            if (textBox != null)
            {
                textBox.Text = ex.ToString();
            }
        }
    }
}
