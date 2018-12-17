using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace PrintStudioModel
{
    public class PictureSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// 数据源
        /// </summary>
        public object Source { get; set; }

        /// <summary>
        /// 右键
        /// </summary>
        public string PictureResource { get; set; }
    }
}
