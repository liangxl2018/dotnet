using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PrintStudioModel;
using System.IO;
using PrintStudioRule;
using System.ComponentModel;

namespace CommonPrintStudio
{
    public class PictureControl : ContentControlBase
    {
        /// <summary>
        /// 图像
        /// </summary>
        private Image img = null;

        public PictureControl()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                Canvas.SetTop(this, 150);
                Canvas.SetLeft(this, 150);
                img = new Image() { };
                this.Content = img;
                this.Template = this.FindResource("DesignerItemTemplate") as ControlTemplate;
                InitPropertyList();
            }
        }

        public override string PrintKeyValue
        {
            get { return _printkeyValue; }
            set
            {
                _printkeyValue = value;
                if (_printkeyValue.Equals("e8fc74c3-9a6d-4bb2-bbed-a5d7f92b8dd1.pcx"))
                {
                    _printkeyValue = string.Format("plugin\\{0}", _printkeyValue);
                }
                if (File.Exists(_printkeyValue))
                {
                    img.Source = ImageHelper.ChangeBitmapToImageSource(ImageHelper.LoadImageFormFreeImage(_printkeyValue));
                }
                else
                {
                    throw new Exception(string.Format("未找到图片资源【{0}】.", _printkeyValue));
                }
                OnPropertyChanged("PrintKeyValue");
            }
        }

        private void InitPropertyList()
        {
            Index = new Random().Next(100000000);
            PrintCaption = "图像";
            PrintFunctionName = "PrintPCX";
            PrintKeyValue = "e8fc74c3-9a6d-4bb2-bbed-a5d7f92b8dd1.pcx";
            PropertyModel p = null;
            Propertys = new List<PrintStudioModel.PropertyModel>();
            p = new PropertyModel()
            {
                Name = "PrintKeyValue",
                Caption = "图像资源",
                Value = "Girl.pcx",
                PropertyType = PropertyType.Picture,
                ComboBoxData = null,
                ComboBoxValueType = 0
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
        }

        /// <summary>
        /// 根据图片大小,更新容器大小.
        /// </summary>
        public void UpdateContainerSize()
        {
            BitmapImage f = (this.Content as Image).Source as BitmapImage;
            double pixelWidth = f.Width;
            double pixelHeight = f.Height;
            Width = pixelWidth;
            MinWidth = pixelWidth;
            MaxWidth = pixelWidth;
            Height = pixelHeight;
            MinHeight = pixelHeight;
            MaxHeight = pixelHeight;
        }
    }
}
