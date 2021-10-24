using System;
using System.Globalization;
using Xamarin.Forms;

namespace PointZ.Converters
{
    public class StringLengthToBoolConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = (string)value;
            return text.Length > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}