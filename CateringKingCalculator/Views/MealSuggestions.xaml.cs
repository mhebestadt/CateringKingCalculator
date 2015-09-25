using hebestadt.CateringKingCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CateringKingCalculator.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MealSuggestions : Page
    {
        MealSuggestionViewModel _mealsViewModel = null;
        ObservableCollection<IMealViewModel> _mealsSuggestion = null;

        public MealSuggestions()
        {
            this.InitializeComponent();

            _mealsViewModel = new MealSuggestionViewModel();
            _mealsSuggestion = _mealsViewModel.GetMeals();
            MealSuggestionsGridView.ItemsSource = _mealsSuggestion;
        }

        private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void AddMealSuggestions_AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            MealSuggestionViewModel mealSuggestion = new MealSuggestionViewModel();
            this.Frame.Navigate(typeof(MealItemsByCategory), mealSuggestion);
        }
    }
}
