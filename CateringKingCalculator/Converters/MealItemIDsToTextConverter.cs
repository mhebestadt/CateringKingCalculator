using hebestadt.CateringKingCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;



namespace hebestadt.CateringKingCalculator.Converters
{
    class MealItemIDsToTextConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            string result = "";

            if (value != null)
            {
                StringBuilder gridText = new StringBuilder();
                Dictionary<float, float> mealItemIDs = (Dictionary<float, float>)value;
                MealItemViewModel mealItem = new MealItemViewModel();
                int loopCount = 0;

                foreach (var mealItemID in mealItemIDs)
                {
                    mealItem = mealItem.GetMealItemById((int)mealItemID.Key);

                    if (mealItem != null)                        
                    {
                        if (loopCount < mealItemIDs.Count - 1)
                            gridText.Append(mealItem.Name).Append(", ");
                        else
                            gridText.Append(mealItem.Name);
                    }

                    loopCount++;
                }

                result = gridText.ToString();
            }

            return result;
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            object result = null;

            return result;
        }
    }
}
