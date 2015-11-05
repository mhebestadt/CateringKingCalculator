using CateringKingCalculatorTests.Converters;
using hebestadt.CateringKingCalculator.Converters;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace hebestadt.CateringKingCalculatorTests.Converters
{
    class DateTimeToStringConverterTests : ConverterTestBase
    {
        DateTimeToStringConverter converter = new DateTimeToStringConverter();

        [TestMethod]
        public void ConvertWithNullParameter_ShouldThrowArgumentNullException()
        {
            this.ConvertWithNullParameter_ShouldThrowArgumentNullException(converter);
        }

        [TestMethod]
        public void ConvertBackWithNullParameter_ShouldThrowArgumentNullException()
        {
            this.ConvertBackWithNullParameter_ShouldThrowArgumentNullException(converter);
        }
    }
}
