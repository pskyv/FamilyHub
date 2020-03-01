using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FamilyAgenda.Converters
{
    public class InitialsBgColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var initials = (char)value;
            var userInitials = Preferences.Get("user", "");
            if (initials == userInitials[0])
            {
                return "Accent";
            }
            return "#CDDC39";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
