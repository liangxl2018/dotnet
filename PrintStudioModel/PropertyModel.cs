using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintStudioModel
{
    /// <summary>
    /// 值类型
    /// </summary>
    public enum PropertyType
    {
        //字符串 Textbox中获取string型值
        String = 0,
        //整型 Textbox中获取int值 注意限制只能输入有符号整形
        Int = 1,
        //浮点型 Textbox中获取Double值 注意限制只能输入有符号浮点数
        Double = 2,
        //布尔型 CheckBox中获取bool值
        CheckBox = 3,
        //Object Combobox中获取选择的条目值
        Combobox = 4,
        /// <summary>
        /// 路径
        /// </summary>
        Picture = 5
    }

    /// <summary>
    /// 打印控件属性Model
    /// </summary>
    public class PropertyModel
    {
        /// <summary>
        /// TextBox值改变事件
        /// </summary>
        public  Action<PropertyChangedFromTextBoxEventArgs> PropertyChanged = null;

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 值类型
        /// </summary>
        public PropertyType PropertyType { get; set; }

        /// <summary>
        /// ComboBox数据源
        /// </summary>
        public List<KeyCaption> ComboBoxData { get; set; }

        /// <summary>
        /// Combobox值类型 0:string 1:int
        /// int 取ID string 取Caption
        /// </summary>
        public int ComboBoxValueType { get; set; }
        
        /// <summary>
        ///是否是可设置属性
        /// </summary>
        private bool isDisplayInPropertyControl=true;
        public bool IsDisplayInPropertyControl
        {
            get { return isDisplayInPropertyControl; }
            set { isDisplayInPropertyControl = value; }
        }

        /// <summary>
        /// 是否可输入中文
        /// </summary>
        public bool SupportChinese { get; set; }
    }
}
