using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using PrintStudioModel;
using PrintStudioRule;
using System.Windows;

namespace CommonPrintStudio
{
    /// <summary>
    /// 矩形可控控件
    /// </summary>
    public class RectangleControl : ContentControlBase
    {
        public RectangleControl()
        {
            Width = 200;
            MinWidth = 1;
            Height = 100;
            MinHeight = 1;
            Canvas.SetTop(this, 150);
            Canvas.SetLeft(this, 150);
            this.Content = new Rectangle() { Name = "thickness", Stroke = new SolidColorBrush(Colors.Black), StrokeThickness = 1, Fill = new SolidColorBrush(Colors.Transparent) };
            this.Template = this.FindResource("DesignerItemTemplate") as ControlTemplate;
            InitPropertyList();
        }

        private void InitPropertyList()
        {
            Index = new Random().Next(100000000);
            PrintCaption = "矩形";
            PrintFunctionName = "PrintRectangle";
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
                Name = "thickness",
                Caption = "边框粗细",
                Value = 1,
                PropertyType = PropertyType.Int,
                ComboBoxData = null,
                ComboBoxValueType = 0,
                PropertyChanged = OnThicknessChanged
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "pEX",
                Caption = "终点X坐标",
                Value = 800,
                PropertyType = PropertyType.Int,
                ComboBoxData = null,
                ComboBoxValueType = 0,
                PropertyChanged = OnpEXChanged
            };
            Propertys.Add(p);

            p = new PropertyModel()
            {
                Name = "pEY",
                Caption = "终点Y坐标",
                Value = 300,
                PropertyType = PropertyType.Int,
                ComboBoxData = null,
                ComboBoxValueType = 0,
                PropertyChanged = OnpEYChanged
            };
            Propertys.Add(p);
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

        private void OnThicknessChanged(PropertyChangedFromTextBoxEventArgs property)
        {
            PropertyModel p = property.Property;
            RectangleControl c = (RectangleControl)property.PrintControl;
            PropertyModel thickness = GetPropertyItemByName(c.Propertys, p.Name);
            if (thickness != null)
            {
                DependencyObject d = DependencyHelper.FindVisualChildByName(c, thickness.Name);
                if (d != null)
                {
                    Type t = d.GetType();
                    if (t == typeof(Rectangle))
                    {
                        Rectangle r = d as Rectangle;
                        r.StrokeThickness = (double)Convert.ChangeType(thickness.Value, typeof(double));
                    }
                }
            }
        }

        private void OnpEXChanged(PropertyChangedFromTextBoxEventArgs property)
        {
            TextBox textBox = property.TextBox;
            PropertyModel p = property.Property;
            RectangleControl c = (RectangleControl)property.PrintControl;
            double ex = (double)Convert.ChangeType(p.Value, typeof(double));
            if (ex < c.MinWidth + Canvas.GetLeft(c))
            {
                ex = c.MinWidth + Canvas.GetLeft(c);
            }
            c.Width = ex - Canvas.GetLeft(c);
            p.Value = ex;
            if (textBox != null)
            {
                textBox.Text = ex.ToString();
            }
        }

        private void OnpEYChanged(PropertyChangedFromTextBoxEventArgs property)
        {
            TextBox textBox = property.TextBox;
            PropertyModel p = property.Property;
            RectangleControl c = (RectangleControl)property.PrintControl;
            double ex = (double)Convert.ChangeType(p.Value, typeof(double));
            if (ex < c.MinHeight + Canvas.GetTop(c))
            {
                ex = c.MinHeight + Canvas.GetTop(c);
            }
            c.Height = ex - Canvas.GetTop(c);
            p.Value = ex;
            if (textBox != null)
            {
                textBox.Text = ex.ToString();
            }
        }
    }
}
