using hebestadt.CateringKingCalculator.Models;
using hebestadt.CateringKingCalculator.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace CateringKingCalculator.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MealItemDetail : Page
    {
        MealItemsViewModel mealItemsViewModel = null;
        ObservableCollection<MealItemViewModel> _mealItems = null;

        MealViewModel meal = null;

        public MealItemDetail()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            meal = (MealViewModel)e.Parameter;
            mealItemsViewModel = new MealItemsViewModel();
            _mealItems = mealItemsViewModel.GetAllMealItems();
            //MealItemDetailListView.ItemsSource = _mealItems;
        }

        private void NavigateBackButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MealItems), meal);
        }

        private void SaveButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MealItems), meal);
        }
    }
}
