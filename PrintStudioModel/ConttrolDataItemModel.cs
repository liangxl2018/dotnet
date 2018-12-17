using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;
using System.ComponentModel;

namespace PrintStudioModel
{
    [Serializable]
    public class ConttrolDataItemModel : INotifyPropertyChanged
    {
        private string _ValueContainer = "{0}";
        /// <summary>
        /// 获取值
        /// </summary>
        public string ValueContainer { get { return _ValueContainer; } set { _ValueContainer = value; OnPropertyChanged("ValueContainer"); } }

        private string _ControlCaption = string.Empty;
        /// <summary>
        /// 值名称
        /// </summary>
        public string ControlCaption { get { return _ControlCaption; } set { _ControlCaption = value; OnPropertyChanged("ControlCaption"); } }

        private string _ControlName = string.Empty;
        /// <summary>
        /// 界面值控件名
        /// </summary>
        public string ControlName { get { return _ControlName; } set { _ControlName = value; OnPropertyChanged("ControlName"); } }

        private string _PropertyName = string.Empty;
        /// <summary>
        /// 控件属性名
        /// </summary>
        public string PropertyName { get { return _PropertyName; } set { _PropertyName = value; OnPropertyChanged("PropertyName"); } }

        private string _ChildPropertyName = string.Empty;
        /// <summary>
        /// 值得子值
        /// </summary>
        public string ChildPropertyName { get { return _ChildPropertyName; } set { _ChildPropertyName = value; OnPropertyChanged("ChildPropertyName"); } }


        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
