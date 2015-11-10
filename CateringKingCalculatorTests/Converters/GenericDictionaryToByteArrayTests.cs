using CateringKingCalculator.Converters;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CateringKingCalculatorTests.Converters
{
    [TestClass]
    class GenericDictionaryToByteArrayTests : ConverterTestBase
    {
        private static GenericDictionaryToByteArray converter = new GenericDictionaryToByteArray();

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
