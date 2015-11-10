using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using Windows.UI.Xaml.Data;

namespace CateringKingCalculatorTests.Converters
{
    public abstract class ConverterTestBase
    {
        object result;

        public void ConvertWithNullParameter_ShouldThrowArgumentNullException(IValueConverter converter)
        {

            try
            {
                result = converter.Convert(null, null, null, "");
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException exception)
            {
                StringAssert.Contains(exception.Message, "");
            }
            catch(Exception e)
            {
                Assert.Fail("Wrong exception  has been thrown");
            }
        }

        public void ConvertBackWithNullParameter_ShouldThrowArgumentNullException(IValueConverter converter)
        {
            try
            {
                result = converter.ConvertBack(null, null, null, "");
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException exception)
            {
                StringAssert.Contains(exception.Message, "");
            }
            catch (Exception e)
            {
                Assert.Fail("The wrong exception has been thrown");
            }
        }
    }
}
