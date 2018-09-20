using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Управление_заказами.ViewModels.Converters
{
    class StringToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string color = value as string;
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
