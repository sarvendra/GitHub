using System;
using System.Globalization;
using System.Windows.Data;

namespace GitHub.Utility
{
    public class ShowHideControlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var str = value as string;
                if (!string.IsNullOrEmpty(str))
                {
                    return "Visible";
                }
            }
            return "Collapsed";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
