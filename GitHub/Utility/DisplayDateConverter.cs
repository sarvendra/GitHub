using System;
using System.Globalization;
using System.Windows.Data;

namespace GitHub.Utility
{
    public class DisplayDateConverter : IValueConverter
    {
        private string _prevDate;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var date = value as string;
                if (date == null)
                {
                    return "Visible";
                }

                date = date.Split('T')[0];
                if (date != _prevDate)
                {
                    _prevDate = date;
                    return "Visible";
                }
                return "Collapsed";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
