using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Управление_заказами.ViewModels.Converters
{
    class GCColorIdToSolidColorBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string colorId = value as string;
            string colorHash = AppSettings.GoogleCalendarColors[colorId];
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorHash));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
