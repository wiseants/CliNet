using System;
using System.Globalization;
using System.Windows.Data;

namespace Common.Converters
{
    /// <summary>
    /// 전달된 텍스트값이 null이거나 빈 경우 false, 아닌 경우 true.
    /// </summary>
    public class TextToEnableConverter : IValueConverter
    {
        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public TextToEnableConverter()
        {
        }

        #endregion

        #region IValueConverter members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            string valueString = value as string;
            if (string.IsNullOrEmpty(valueString) == true)
            {
                return false;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new ArgumentNullException("Could not convert back.");
        }

        #endregion
    }
}
