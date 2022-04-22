using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Common.Converters
{
    /// <summary>
    /// bool ==> Visibility 컨버터.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public BoolToVisibilityConverter()
        {
            IfTrueVisibility = Visibility.Visible;
            ElseVisibility = Visibility.Collapsed;
        }

        #endregion

        #region Properties

        /// <summary>
        /// True인 경우의 비지빌리티 타입.
        /// </summary>
        public Visibility IfTrueVisibility
        {
            get;
            set;
        }

        /// <summary>
        /// True가 아닌 경우의 비지빌리티 타입.
        /// </summary>
        public Visibility ElseVisibility
        {
            get;
            set;
        }

        #endregion

        #region IValueConverter members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null
                ? throw new ArgumentNullException("Parameter of converter is null.")
                : (bool)value ? IfTrueVisibility : ElseVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Parameter of converter is null.");
            }

            return (Visibility)value == IfTrueVisibility ? true : false;
        }

        #endregion
    }
}
