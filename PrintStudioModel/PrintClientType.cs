using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintStudioModel
{
    /// <summary>
    /// 打印类型
    /// </summary>
    public enum PrintClientType
    {
        /// <summary>
        /// 通用打印机
        /// </summary>
        CommonPrinter = 0,

        /// <summary>
        /// 200点
        /// </summary>
        POSTEK_G2000 = 1,

        /// <summary>
        /// 300点
        /// </summary>
        POSTEK_G3000 = 2,

        /// <summary>
        /// 600点
        /// </summary>
        POSTEK_G6000 = 3,

        /// <summary>
        /// 斑马打印 300点
        /// </summary>
        ZebraPrinter = 4,

        /// <summary>
        /// 斑马打印 600点
        /// </summary>
        ZebraPrinter600=5
    }
}
