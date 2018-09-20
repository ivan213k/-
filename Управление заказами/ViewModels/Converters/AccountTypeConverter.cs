using System;
using System.Globalization;
using System.Windows.Data;

namespace Управление_заказами.ViewModels.Converters
{
    class AccountTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0: return "Администратор";
                case 1: return "Менеджер";
                case 2: return "Пользователь";
                default: return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
