using hebestadt.CateringKingCalculator.Models;
using hebestadt.CateringKingCalculator.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CateringKingCalculator.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MealDetail : Page
    {
        MealItemsViewModel mealItemsViewModel = null;
        ObservableCollection<MealItemViewModel> mealitems = null;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var meal = (MealViewModel)e.Parameter;
            
            mealItemsViewModel = new MealItemsViewModel();
            mealitems = mealItemsViewModel.GetMealItems(meal.mealItemIDsWithWeight);
            int y = mealitems.Count;
            MealItemsListView.ItemsSource = mealitems;
        }
    }
}
