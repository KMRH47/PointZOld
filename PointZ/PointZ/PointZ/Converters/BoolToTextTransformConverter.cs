using System;
using System.Globalization;
using Xamarin.Forms;

namespace PointZ.Converters
{
    public class BoolToTextTransformConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? TextTransform.Default : TextTransform.None; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}