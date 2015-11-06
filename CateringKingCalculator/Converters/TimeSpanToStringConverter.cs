using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace hebestadt.CateringKingCalculator.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value)
        {
            return this.Convert(value, null, null, "");
        }

        public object Convert(object value, Type type, object parameter, string language)
        {
            string result = "";

            try
            {
                if (value is TimeSpan)
                {
                    TimeSpan theTime = (TimeSpan)value;
                    //result = theTime.ToString();

                    //D2 = pad with 00
                    result = String.Format("{0:D2}:{1:D2}", theTime.Hours, theTime.Minutes);
                }
                else
                {
                    TimeSpan theTime = new TimeSpan();
                    result = String.Format("{0:D2}:{1:D2}", theTime.Hours, theTime.Minutes);
                }
            }
            catch (FormatException e)
            {
                string erroMessage = e.Message;
            }

            return result;
        }

        public object ConvertBack(object value)
        {
            return this.ConvertBack(value, null, null, "");
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            TimeSpan result;

            try
            {
                result = TimeSpan.ParseExact((string)value, "c", null);
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            return result;
        }
    }
}
