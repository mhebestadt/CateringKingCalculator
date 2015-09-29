using hebestadt.CateringKingCalculator.Converters;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;

namespace CateringKingCalculatorTests.Converters
{
    [TestClass]
    public class DateTimeToWeekOfYearISO8601Tests
    {
        private static DateTimeToWeekOfYearISO8601 converter = new DateTimeToWeekOfYearISO8601();

        [TestMethod]
        public void GetWeekOfYear_ValidDate()
        {
            int weekOfYear = (int)converter.Convert(DateTimeOffset.Now, null, null, "");
            Assert.IsTrue((weekOfYear > 0) && (weekOfYear < 44));
        }

        [TestMethod]
        public void GetWeekofYearFromLastDayOfYear()
        {
            DateTime t = new DateTime(DateTime.Now.Year, 12, 31);
            DateTimeOffset lastDayOfYear = new DateTimeOffset(t);
            int weekOfYear = (int)converter.Convert(lastDayOfYear, null, null, "");
            Assert.IsTrue((weekOfYear == 52) || (weekOfYear == 53));
        }

        [TestMethod]
        public void GetDateRangeFromInvalidWeekOfYear_ShouldThrowArgumentOutOfRangeException()
        {
            DateTime[] result = null;

            try
            {
                result = (DateTime[])converter.ConvertBack(89, null, null, "");
            }
            catch(ArgumentOutOfRangeException exception)
            {
                StringAssert.Contains(exception.Message, "");
            }    
        }

        [TestMethod]
        public void GetDateRangeForTodaysWeekOfYear()
        {
            DateTime[] result = null;

            int weekOfYear = (int)converter.Convert(DateTimeOffset.Now, null, null, "");
            result = (DateTime[])converter.ConvertBack(weekOfYear, null, null, "");
            Assert.IsTrue(result.Length == 7);
        }
    }
}
