using CateringKingCalculator.ViewModels;
using System;
using Windows.UI.Xaml.Data;

namespace hebestadt.CateringKingCalculator.Converters
{
    class ContactIdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            string result = "";

            if ((int)value != 0)
            {
                ContactViewModel contact = new ContactViewModel();
                contact = contact.GetContact((int)value);
                result = contact.NameAndAddress;
            }

            return result;
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            object result = null;

            return result;
        }
    }
}
