using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MyToDo.Common.Converters
{
    public class IntToBoolConveter : IValueConverter
    {   

        /// <summary>
        /// 后端转前端 类型转换 int to bool
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && int.TryParse(value.ToString(), out int result))
            {
                if (result == 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 前端转后端 类型转换 bool to int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value !=null && bool.TryParse(value.ToString(),out bool result))
            {
                if (result)
                {
                    return 1;
                }
            }
            return 0;
        }
    }
}
