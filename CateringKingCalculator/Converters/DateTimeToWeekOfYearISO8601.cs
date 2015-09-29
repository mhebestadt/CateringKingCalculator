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

        private static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);

            if (firstWeek <= 1 || firstWeek > 50)
            {
                weekOfYear -= 1;
            }

            return firstWeekDay.AddDays(weekOfYear * 7);
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            DateTime[] result = null;

            if (value is int)
            {
                if (((int)value > 0) && ((int)value < 54))
                {
                    DateTime startDate = FirstDateOfWeekISO8601(DateTime.Now.Year, (int)value);
                    DateTime endDate = startDate.AddDays(6);
                    result = Enumerable.Range(1, 7).Select(num => startDate.AddDays(num)).ToArray();
                    /*DateTime start = new DateTime(DateTime.Now.Year, 1, 4);
                    start = start.AddDays(-((int)start.DayOfWeek));
                    start = start.AddDays(7 * ((int)value - 1));
                    result = Enumerable.Range(1, 7).Select(num => start.AddDays(num)).ToArray();*/
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
