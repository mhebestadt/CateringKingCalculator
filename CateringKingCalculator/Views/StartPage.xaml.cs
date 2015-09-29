using hebestadt.CateringKingCalculator;
using hebestadt.CateringKingCalculator.ViewModels;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CateringKingCalculator.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartPage : Page
    {
        MealsViewModel _mealsViewModel = null;
        ObservableCollection<MealViewModel> _meals = null;

        public StartPage()
        {
            this.InitializeComponent();

            _mealsViewModel = new MealsViewModel();
            _meals = _mealsViewModel.GetMeals();
            MealsStartView.ItemsSource = _meals;
        }

        private void MealsStartView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(MealItems), e.ClickedItem);
        }

        private void MealsAdd_AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewMealCustomerEventDataInput), new MealViewModel());
        }
    }
}
