using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;

namespace PrintStudioModel
{
    [Serializable]
    public class PrintItemModel
    {
        /// <summary>
        /// 长度 界面呈现用
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 高度 界面呈现用
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string  PrintCaption { get; set; }
        
        /// <summary>
        /// 打印方法名
        /// </summary>
        public string PrintFunctionName { get; set; }

        /// <summary>
        /// 打印方法
        /// </summary>
        public IPrintFunction ParseFuntion { get; set; }

        /// <summary>
        /// 有效值
        /// </summary>
        public string PrintKeyValue { get; set; }

        /// <summary>
        /// 组装值来源
        /// </summary>
        public int DataSourceType { get; set; }

        /// <summary>
        /// 条目索引
        /// </summary>
        public int Index { get; set; }

        private bool _isValid = true;
        /// <summary>
        /// 是否有效 
        /// </summary>
        public bool IsValid
        {
            get { return _isValid; }
            set { _isValid = value; }
        }

        /// <summary>
        /// 界面数据
        /// </summary>
        public ConttrolDataItemModel ConttrolData { get; set; }

        /// <summary>
        /// 方法数据
        /// </summary>
        public FunctionDataItemModel FunctionData { get; set; }

        /// <summary>
        /// 打印除Value外的参数集合
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// 默认参数集合
        /// </summary>
        public Dictionary<string, string> DefaultParameters { get; set; }

        /// <summary>
        /// X偏移
        /// </summary>
        public int XDeviation { get; set; }

        /// <summary>
        /// Y偏移
        /// </summary>
        public int YDeviation { get; set; }
    }
}
