using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using Windows.UI.Xaml.Data;

namespace CateringKingCalculatorTests.Converters
{
    public abstract class ConverterTestBase
    {
        public void ConvertWithNullParameter_ShouldThrowArgumentNullException(IValueConverter converter)
        {
            int weekOfYear = 0;

            try
            {
                weekOfYear = (int)converter.Convert(null, null, null, "");
            }
            catch (ArgumentNullException exception)
            {
                StringAssert.Contains(exception.Message, "");
            }
        }

        public void ConvertBackWithNullParameter_ShouldThrowArgumentNullException(IValueConverter converter)
        {
            DateTime[] result = null;

            try
            {
                result = (DateTime[])converter.ConvertBack(null, null, null, "");
            }
            catch (ArgumentNullException exception)
            {
                StringAssert.Contains(exception.Message, "");
            }
        }
    }
}
