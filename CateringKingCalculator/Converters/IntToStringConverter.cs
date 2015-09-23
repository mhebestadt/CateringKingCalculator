using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace hebestadt.CateringKingCalculator.Converters
{
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            string result = "";

            result = value.ToString();

            return result;
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            return Int32.Parse((string)value);

        }
    }
}
