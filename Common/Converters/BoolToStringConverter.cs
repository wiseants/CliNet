using System;
using System.Globalization;
using System.Windows.Data;

namespace Common.Converters
{
    /// <summary>
    /// bool ==> 문자열 컨버터.
    /// </summary>
    public class BoolToStringConverter : IValueConverter
    {
        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public BoolToStringConverter()
        {
            IfTrueString = "true";
            ElseString = "false";
        }

        #endregion

        #region Properties

        /// <summary>
        /// True인 경우의 문자열.
        /// </summary>
        public string IfTrueString
        {
            get;
            set;
        }

        /// <summary>
        /// True가 아닌 경우의 문자열.
        /// </summary>
        public string ElseString
        {
            get;
            set;
        }

        #endregion

        #region IValueConverter members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Parameter of converter is null.");
            }

            return (bool)value == true ? IfTrueString : ElseString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Parameter of converter is null.");
            }

            return ((string)value).Equals(IfTrueString, StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}
