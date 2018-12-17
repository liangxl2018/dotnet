using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace PrintStudioModel
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// 无参构造
        /// </summary>
        public NotifyPropertyChangedBase() { }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~NotifyPropertyChangedBase()
        {
            DoDispose();
        }

        /// <summary>
        /// 值缓存
        /// </summary>
        private Dictionary<object, object> _ValueDictionary = null;

        #region 根据属性名获取属性值
        public T GetPropertyValue<T>(string propertyName)
        {
            if (_ValueDictionary == null)
            {
                _ValueDictionary = new Dictionary<object, object>();
            }
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentException("invalid " + propertyName);
            object _propertyValue;
            if (!_ValueDictionary.TryGetValue(propertyName, out _propertyValue))
            {
                _propertyValue = default(T);
                _ValueDictionary.Add(propertyName, _propertyValue);
            }
            return (T)_propertyValue;
        }
        #endregion

        #region 根据属性名设置属性值
        public void SetPropertyValue<T>(string propertyName, T value)
        {
            if (_ValueDictionary == null)
            {
                _ValueDictionary = new Dictionary<object, object>();
            }
            if (!_ValueDictionary.ContainsKey(propertyName) || _ValueDictionary[propertyName] != (object)value)
            {
                _ValueDictionary[propertyName] = value;
                OnPropertyChanged(propertyName);
            }
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            DoDispose();
        }

        private void DoDispose()
        {
            if (_ValueDictionary != null)
            {
                _ValueDictionary.Clear();
            }
        }
        #endregion

        #region INotifyPropertyChanged

        public void RaisePropertyChanged()
        {
            var stack = new StackTrace();
            var lastFrame = stack.GetFrame(1);
            var methodName = lastFrame.GetMethod().Name;
            var propertyName = methodName.Substring(methodName.IndexOf("_") + 1);
            this.OnPropertyChanged(propertyName);
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }

    public static class NotifyPropertyChangedBaseExtend
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="t"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static U GetValue<T, U>(this T t, Expression<Func<T, U>> exp) where T : NotifyPropertyChangedBase
        {
            string _pN = GetPropertyName(exp);
            return t.GetPropertyValue<U>(_pN);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="t"></param>
        /// <param name="exp"></param>
        /// <param name="value"></param>
        public static void SetValue<T, U>(this T t, Expression<Func<T, U>> exp, U value) where T : NotifyPropertyChangedBase
        {
            string _pN = GetPropertyName(exp);
            t.SetPropertyValue<U>(_pN, value);
        }

        /// <summary>
        /// 获取属性名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        internal static string GetPropertyName<T, U>(Expression<Func<T, U>> exp)
        {
            string _pName = string.Empty;
            if (exp.Body is MemberExpression)
            {
                _pName = (exp.Body as MemberExpression).Member.Name;
            }
            else if (exp.Body is UnaryExpression)
            {
                _pName = ((exp.Body as UnaryExpression).Operand as MemberExpression).Member.Name;
            }
            return _pName;
        }
    }
}
