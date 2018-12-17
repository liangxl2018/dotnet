using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace PrintStudioModel
{
    public class ContentMenuEventArgs : EventArgs
    {
        /// <summary>
        /// 数据源
        /// </summary>
        public object Source { get; set; }

        /// <summary>
        /// 右键
        /// </summary>
        public MenuItem MenuItem { get; set; }
    }

    /// <summary>
    /// TextBox中值发生改变,通知更新打印控件事件参数
    /// </summary>
    public class PropertyChangedFromTextBoxEventArgs : EventArgs
    {
        /// <summary>
        /// 打印子控件
        /// </summary>
        public ContentControlBase PrintControl { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        public PropertyModel Property { get; set; }

        /// <summary>
        /// TextBox控件源
        /// </summary>
        public TextBox TextBox { get; set; }
    }
}
