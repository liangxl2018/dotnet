using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace CommonPrintStudio
{
    public class FunctionIndexsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                {
                    return string.Empty;
                }
                double reValue = (double)value;
                return reValue - 30;
            }
            catch
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }

    public class ValueContainerVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int reValue = (int)value;
                string flag = (string)parameter;
                if (flag.Equals("ControlData"))
                {
                    if (reValue == 0)
                    {
                        return Visibility.Hidden;
                    }
                    else if (reValue == 1)
                    {
                        return Visibility.Visible;
                    }
                    else
                    {
                        return Visibility.Hidden;
                    }
                }
                else
                {
                    if (reValue == 0)
                    {
                        return Visibility.Hidden;
                    }
                    else if (reValue == 1)
                    {
                        return Visibility.Hidden;
                    }
                    else
                    {
                        return Visibility.Visible;
                    }
                }
            }
            catch
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DataSourceTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int reValue = (int)value;
                if (reValue == 0)
                {
                    return "固定值";
                }
                else if (reValue == 1)
                {
                    return "界面值";
                }
                else
                {
                    return "方法值";
                }
            }
            catch
            {
                return "未知";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
