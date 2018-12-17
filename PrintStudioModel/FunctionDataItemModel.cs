using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PrintStudioModel
{
    [Serializable]
    public class FunctionDataItemModel : INotifyPropertyChanged
    {

        private string _ValueContainer = "{0}";
        /// <summary>
        /// 获取值
        /// </summary>
        public string ValueContainer { get { return _ValueContainer; } set { _ValueContainer = value; OnPropertyChanged("ValueContainer"); } }


        private string _FunctionName = string.Empty;
        /// <summary>
        /// 解析方法名
        /// </summary>
        public string FunctionName { get { return _FunctionName; } set { _FunctionName = value; OnPropertyChanged("FunctionName"); } }

        private List<int> _FunctionIndexs = null;

        private string _functionIndexsCaption = string.Empty;
        public string FunctionIndexsCaption
        {
            get
            {
                return _functionIndexsCaption;
            }
            set
            {
                _functionIndexsCaption = value;
                OnPropertyChanged("FunctionIndexsCaption");
            }
        }

        /// <summary>
        /// 解析indexs
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private List<int> ParseIndexs(string info)
        {
            List<int> indexs = new List<int>();
            string[] temp = info.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (temp != null && temp.Length > 0)
            {
                foreach (var item in temp)
                {
                    indexs.Add(int.Parse(item));
                }
            }
            return indexs;
        }

        /// <summary>
        /// 参数Indexs
        /// </summary>
        public List<int> FunctionIndexs
        {
            get { return _FunctionIndexs; }
            set
            {
                _FunctionIndexs = value;
                OnPropertyChanged("FunctionIndexs");
            }
        }

        private int _RandomType = 0;
        /// <summary>
        ///随机串类型  0:数字 1:数字加小写字母 2:数字加大写字母 3:数字加大小写 4:大写字母 5:小写字母
        /// </summary>
        public int RandomType { get { return _RandomType; } set { _RandomType = value; OnPropertyChanged("RandomType"); } }

        private int _RandomLength = 0;
        /// <summary>
        /// 随机串长度
        /// </summary>
        public int RandomLength { get { return _RandomLength; } set { _RandomLength = value; OnPropertyChanged("RandomLength"); } }

        private bool _IsHeadMontage = false;
        /// <summary>
        /// 从同开始剪裁
        /// </summary>
        public bool IsHeadMontage { get { return _IsHeadMontage; } set { _IsHeadMontage = value; OnPropertyChanged("IsHeadMontage"); } }

        private int _MontageLength = 0;
        /// <summary>
        /// 剪裁长度
        /// </summary>
        public int MontageLength { get { return _MontageLength; } set { _MontageLength = value; OnPropertyChanged("MontageLength"); } }

        /// <summary>
        /// 解析方法
        /// </summary>
        public IDataFunction ParseFuntion { get; set; }

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
