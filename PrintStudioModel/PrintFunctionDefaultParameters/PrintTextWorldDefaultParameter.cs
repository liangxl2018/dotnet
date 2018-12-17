using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace PrintStudioModel
{
    /// <summary>
    /// 文字默认参数
    /// </summary>
    public class PrintTextWorldDefaultParameter : Dictionary<string, string>
    {
        public PrintTextWorldDefaultParameter()
        {
            Add("X", "10");
            Add("Y", "20");
        }
    }
}
