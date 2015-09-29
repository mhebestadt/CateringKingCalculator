using System;
using Windows.UI.Xaml.Data;

namespace hebestadt.CateringKingCalculator.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            string result = "";

            try
            {
                if (value is DateTimeOffset)
                {
                    DateTimeOffset theDate = (DateTimeOffset)value;
                    result = string.Format("{0:ddd dd.MM.yyyy}", theDate);
                }
                else
                {
                    DateTimeOffset theDate = DateTimeOffset.Now;
                    result = string.Format("{0:ddd dd.MM.yyyy}", theDate);
                }
            }
            catch (FormatException e)
            {
                string erroMessage = e.Message;
            }

            return result;
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            object result = null;

            try
            {
                result =  System.Convert.ToDateTime(value);
            }
            catch(FormatException e)
            {
                string erroMessage = e.Message;
            }

            return result;
        }
    }
}
