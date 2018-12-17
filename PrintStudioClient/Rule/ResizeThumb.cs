using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using PrintStudioModel;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Media;

namespace CommonPrintStudio
{
    /// <summary>
    /// 缩放
    /// </summary>
    public class ResizeThumb : Thumb
    {
        public ResizeThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.ResizeThumb_DragDelta);
        }

        /// <summary>
        /// 缩放事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e) 
        {
            Control designerItem = this.DataContext as Control;
            if (designerItem.RenderTransform != null && designerItem.RenderTransform is RotateTransform)
            {
                //有旋转时禁用移动和缩放
                RotateTransform r = (RotateTransform)designerItem.RenderTransform;
                if (r != null)
                {
                    if (r.Angle != 0)
                    {
                        return;
                    }
                }
            }
            if (designerItem != null)
            {
                double deltaVertical, deltaHorizontal;
                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange, designerItem.ActualHeight - designerItem.MinHeight);
                        designerItem.Height -= deltaVertical;
                        if (designerItem.Height > designerItem.MaxHeight)
                        {
                            designerItem.Height = designerItem.MaxHeight;
                        }
                        break;
                    case VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange, designerItem.ActualHeight - designerItem.MinHeight);
                        Canvas.SetTop(designerItem, Canvas.GetTop(designerItem) + deltaVertical);
                        designerItem.Height -= deltaVertical;
                        if (designerItem.Height > designerItem.MaxHeight)
                        {
                            designerItem.Height = designerItem.MaxHeight;
                        }
                        break;
                    default:
                        break;
                }
                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(e.HorizontalChange, designerItem.ActualWidth - designerItem.MinWidth);
                        Canvas.SetLeft(designerItem, Canvas.GetLeft(designerItem) + deltaHorizontal);
                        designerItem.Width -= deltaHorizontal;
                        if (designerItem.Width > designerItem.MaxWidth)
                        {
                            designerItem.Width = designerItem.MaxWidth;
                        }
                        break;
                    case HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange, designerItem.ActualWidth - designerItem.MinWidth);
                        designerItem.Width -= deltaHorizontal;
                        if (designerItem.Width > designerItem.MaxWidth)
                        {
                            designerItem.Width = designerItem.MaxWidth;
                        }
                        break;
                    default:
                        break;
                }
                if (designerItem is ContentControlBase)
                {
                    ContentControlBase temp = designerItem as ContentControlBase;
                    PropertyModel pX = GetPropertyItemByName(temp.Propertys, "pHeight");
                    if (pX != null)
                    {
                        pX.Value = Math.Floor(designerItem.Height);
                    }
                    PropertyModel pY = GetPropertyItemByName(temp.Propertys, "pWidth");
                    if (pY != null)
                    {
                        pY.Value = Math.Floor(designerItem.Width);
                    }

                    double y = Math.Floor(Canvas.GetTop(temp));
                    double x = Math.Floor(Canvas.GetLeft(temp));
                    PropertyModel px = GetPropertyItemByName(temp.Propertys, "pX");
                    if (px != null)
                    {
                        px.Value = x;
                    }
                    PropertyModel py = GetPropertyItemByName(temp.Propertys, "pY");
                    if (py != null)
                    {
                        py.Value = y;
                    }
                }
                if (designerItem is ContentControlBase)
                {
                    DealWithSpecialPrintControl((ContentControlBase)designerItem);
                }
            }
            e.Handled = true;
        }

        /// <summary>
        /// 特殊控件处理
        /// </summary>
        private void DealWithSpecialPrintControl(ContentControlBase c)
        {
            Type t = c.GetType();
            if (t == typeof(RectangleControl))
            {
                DealWithRectangleControl(c);
            }
            else
            {
                //TODO
            }
        }

        /// <summary>
        /// 矩形控件处理
        /// </summary>
        /// <param name="c"></param>
        private void DealWithRectangleControl(ContentControlBase c)
        {
            double y = Math.Floor(Canvas.GetTop(c));
            double x = Math.Floor(Canvas.GetLeft(c));
            PropertyModel pEX = GetPropertyItemByName(c.Propertys, "pEX");
            if (pEX != null)
            {
                pEX.Value = Math.Floor(x+c.Width);
            }
            PropertyModel pEY = GetPropertyItemByName(c.Propertys, "pEY");
            if (pEY != null)
            {
                pEY.Value = Math.Floor(y+c.Height);
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
    }
}
