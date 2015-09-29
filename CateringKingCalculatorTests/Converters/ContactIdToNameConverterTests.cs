using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using hebestadt.CateringKingCalculator.Converters;
using CateringKingCalculatorTests.Converters;

namespace hebestadt.CateringKingCalculatorTests.Converters
{
    [TestClass]
    public class ContactIdToNameConverterTests : ConverterTestBase
    {
        private static ContactIdToNameConverter converter = new ContactIdToNameConverter();

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
