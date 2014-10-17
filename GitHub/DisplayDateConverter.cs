using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GitHub
{
    public class DisplayDateConverter : IValueConverter
    {
        private string prevDate = null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string date = value as string;
                date = date.Split('T')[0];
                if (date != prevDate)
                {
                    prevDate = date;
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
