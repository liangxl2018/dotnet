using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintStudioModel
{
    public interface IPrintFunction
    {
        /// <summary>
        /// 解析函数
        /// </summary>
        /// <param name="templetModels"></param>
        /// <param name="indexs"></param>
        /// <returns></returns>
        void PrintParseFuntion(PrintItemModel printItem,object other = null);
    }
}
