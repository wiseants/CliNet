using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Common.Converters
{
    /// <summary>
    /// 데이터 소스의 비었음을 비지빌리티로 주는 컨버터.
    /// </summary>
    public class SourceEmptyToVisibliltyConverter : IValueConverter
    {
        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public SourceEmptyToVisibliltyConverter()
        {
            IfEmptyVisibility = Visibility.Visible;
            ElseVisibility = Visibility.Collapsed;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 참인 경우의 비지빌리티 타입.
        /// </summary>
        public Visibility IfEmptyVisibility
        {
            get;
            set;
        }

        /// <summary>
        /// 참이 아닌 경우의 비지빌리티 타입.
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
            if (value == null)
            {
                return IfEmptyVisibility;
            }

            ICollection source = value as ICollection;
            if (source == null)
            {
                return IfEmptyVisibility;
            }

            return source.Count > 0 ? ElseVisibility : IfEmptyVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Parameter of converter is null.");
            }

            return (Visibility)value == IfEmptyVisibility ? true : false;
        }

        #endregion
    }
}
