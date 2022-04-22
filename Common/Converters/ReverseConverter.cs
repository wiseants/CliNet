using System;
using System.Globalization;
using System.Windows.Data;

namespace Common.Converters
{
    /// <summary>
    /// bool ==> bool == false 컨버터.
    /// </summary>
    public class ReverseConverter : IValueConverter
    {
        #region IValueConverter members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Parameter of converter is null.");
            }

            return (bool)value == false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Parameter of converter is null.");
            }

            return (bool)value == false;
        }

        #endregion
    }
}
