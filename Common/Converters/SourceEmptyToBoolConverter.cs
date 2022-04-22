using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace Common.Converters
{
    /// <summary>
    /// 데이터 소스의 비었음을 확인하는 컨버터.
    /// </summary>
    public class SourceEmptyToBoolConverter : IValueConverter
    {
        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public SourceEmptyToBoolConverter()
        {
            IfEmpty = true;
            ElseEmpty = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 소스가 빈 경우의 값.
        /// </summary>
        public bool IfEmpty
        {
            get;
            set;
        }

        /// <summary>
        /// 소스가 비지 않은 경우의 값.
        /// </summary>
        public bool ElseEmpty
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
                return IfEmpty;
            }

            ICollection source = value as ICollection;
            if (source == null)
            {
                return IfEmpty;
            }

            return source.Count > 0 ? ElseEmpty : IfEmpty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new ArgumentNullException("Could not convert back.");
        }

        #endregion
    }
}
