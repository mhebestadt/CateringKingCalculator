using CateringKingCalculator.Converters;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;

namespace CateringKingCalculatorTests.Converters
{
    [TestClass]
    public class GenericDictionaryToByteArrayTests : ConverterTestBase
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

        [TestMethod]
        public void ConvertAndBackTestValues()
        {
            byte[] theArray = (byte[])converter.Convert(new Dictionary<int, int>() { { 1, 15 }, { 2, 20 }, { 3, 30 }, { 4, 40 } });
            Dictionary<int, int> theDict = new Dictionary<int, int>();
            theDict = (Dictionary<int, int>)converter.ConvertBack(theArray);
            Assert.IsTrue(theDict[1] == 15);
            Assert.IsTrue(theDict[2] == 20);
            Assert.IsTrue(theDict[3] == 30);
            Assert.IsTrue(theDict[4] == 40);
        }
    }
}
