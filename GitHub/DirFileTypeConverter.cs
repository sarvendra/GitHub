using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GitHub
{
    public class DirFileTypeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string imagePath = null;
                string stringValue = value as string;
                if (stringValue == "dir")
                {
                    imagePath = "/Assets/directory.png";
                }
                else if (stringValue == "file")
                {
                    imagePath = "/Assets/file.png";
                }
                return imagePath;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
