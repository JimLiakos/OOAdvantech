using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace WPFUIElementObjectBind.Converters
{
    /// <MetaDataID>{451dc716-9888-4c90-9a97-db570029db1d}</MetaDataID>
    public class NullVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string defaultInvisibility = parameter as string;
            Visibility invisibility = (defaultInvisibility != null) ?
                (Visibility)Enum.Parse(typeof(Visibility), defaultInvisibility)
                : Visibility.Collapsed;
            return value == null ? invisibility : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
