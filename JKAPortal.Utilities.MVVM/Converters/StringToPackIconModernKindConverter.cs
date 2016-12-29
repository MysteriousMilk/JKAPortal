using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace JKAPortal.Utilities.MVVM.Converters
{
    public class StringToPackIconModernKindConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PackIconModernKind iconKind = PackIconModernKind.App;

            if (value is string)
            {

                try
                {
                    iconKind = (PackIconModernKind)Enum.Parse(typeof(PackIconModernKind), value as string);
                }
                catch (Exception)
                {
                    iconKind = PackIconModernKind.App;
                }
            }

            return iconKind;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PackIconModernKind)
                return value.ToString();
            return null;
        }
    }
}
