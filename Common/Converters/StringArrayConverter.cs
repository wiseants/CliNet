using System;
using System.ComponentModel;
using System.Globalization;

namespace Common.Converters
{
    /// <summary>
    /// 설정 타입 컨버터 구현 클래스.
    /// </summary>
    // http://5.9.10.113/24291249/dialogpage-string-array-not-persisted
    public class StringArrayConverter : TypeConverter
    {
        #region Fields

        private const string delimiter = ";";

        #endregion

        #region TypeConverter implementations

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string[]) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string v = value as string;

            return v == null ? base.ConvertFrom(context, culture, value) : v.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            string[] v = value as string[];
            if (destinationType != typeof(string) || v == null)
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
            return string.Join(delimiter, v);
        }

        #endregion
    }
}
