using System;
using System.Globalization;
using System.Windows.Data;

namespace GitHub.Utility
{
    public class FormatDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string date = value as string;
                string formattedDate = date.Split('T')[0];
                formattedDate += " " + date.Split('T')[1].Split('Z')[0];
                return formattedDate;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
