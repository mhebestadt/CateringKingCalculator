using System;
using System.Globalization;
using System.Linq;
using Windows.UI.Xaml.Data;


namespace hebestadt.CateringKingCalculator.Converters
{
    public class DateTimeToWeekOfYearISO8601 : IValueConverter
    {
        private static DateTime ConvertFromDateTimeOffset(DateTimeOffset dateTime)
        {
            if (dateTime.Offset.Equals(TimeSpan.Zero))
                return dateTime.UtcDateTime;
            else if (dateTime.Offset.Equals(TimeZoneInfo.Local.GetUtcOffset(dateTime.DateTime)))
                return DateTime.SpecifyKind(dateTime.DateTime, DateTimeKind.Local);
            else
                return dateTime.DateTime;
        }


        public object Convert(object value, Type type, object parameter, string language)
        {
            int result = 0;

            if (value == null)
                throw new ArgumentNullException();

            try
            {
                if (value is DateTimeOffset)
                {
                    DateTime dateTime = ConvertFromDateTimeOffset((DateTimeOffset)value);
                    DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(dateTime);

                    if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                    {
                        dateTime = dateTime.AddDays(3);
                    }

                    result = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            catch (FormatException e)
            {
                string erroMessage = e.Message;
            }

            return result;
        }

        private DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            DateTime[] result = null;

            if (value == null)
                throw new ArgumentNullException();

            if (value is int)
            {
                if (((int)value > 0) && ((int)value < 54))
                {
                    DateTime startDate = FirstDateOfWeekISO8601(DateTime.Now.Year, (int)value);
                    result = Enumerable.Range(0, 7).Select(num => startDate.AddDays(num)).ToArray();
                }
                else
                {
                    string errorMessage = "Invalid week number: " + ((int)value).ToString();
                    throw new ArgumentOutOfRangeException("value", errorMessage);
                }
            }

            return result;
            
        }
    }
}
