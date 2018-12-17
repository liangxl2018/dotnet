using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using PrintStudioModel;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Windows.Media;

namespace CommonPrintStudio
{
    /// <summary>
    /// 移动
    /// </summary>
    public class MoveThumb : Thumb
    {
        public MoveThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }

        /// <summary>
        /// 移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Control designerItem = this.DataContext as Control;
            if (designerItem.RenderTransform != null && designerItem.RenderTransform is RotateTransform)
            {
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
                double left =Math.Floor( Canvas.GetLeft(designerItem) + e.HorizontalChange);
                double top = Math.Floor(Canvas.GetTop(designerItem) + e.VerticalChange);
                Canvas.SetLeft(designerItem, left);
                Canvas.SetTop(designerItem, top);
                if (designerItem is ContentControlBase)
                {
                    ContentControlBase temp = designerItem as ContentControlBase;
                    PropertyModel pX = GetPropertyItemByName(temp.Propertys, "pX");
                    if (pX != null)
                    {
                        pX.Value = left;
                    }
                    PropertyModel pY = GetPropertyItemByName(temp.Propertys, "pY");
                    if (pY != null)
                    {
                        pY.Value = top;
                    }
                    if (designerItem is ContentControlBase)
                    {
                        DealWithSpecialPrintControl((ContentControlBase)designerItem);
                    }
                }
            }
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
                pEX.Value = Math.Floor(x + c.Width);
            }
            PropertyModel pEY = GetPropertyItemByName(c.Propertys, "pEY");
            if (pEY != null)
            {
                pEY.Value = Math.Floor(y + c.Height);
            }
        }

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
