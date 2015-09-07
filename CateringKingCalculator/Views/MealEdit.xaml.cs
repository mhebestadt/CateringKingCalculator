using hebestadt.CateringKingCalculator.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CateringKingCalculator.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MealEdit : Page
    {
        MealViewModel meal = null;

        public MealEdit()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);
            meal = new MealViewModel();
            meal.DeliveryDate = DateTime.Now;
            this.DataContext = meal;

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StartPage));
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            string result = meal.SaveMeal(meal);

            if (string.CompareOrdinal("Success", result) == 0)
            {
                this.Frame.Navigate(typeof(StartPage));
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            meal.DeleteMeal(meal.Id);
        }
    }
}
