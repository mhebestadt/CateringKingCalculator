using hebestadt.CateringKingCalculator.Dialogs;
using hebestadt.CateringKingCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
        MealSuggestionViewModel _mealSuggestionViewModel = null;
        ObservableCollection<IMealViewModel> _mealSuggestions = null;
        MealSuggestionViewModel _mealSuggestion = null;
        MealViewModel _meal = null;

        public MealSuggestions()
        {
            this.InitializeComponent();

            _mealSuggestionViewModel = new MealSuggestionViewModel();
            _mealSuggestions = _mealSuggestionViewModel.GetMeals();
            MealSuggestionsGridView.ItemsSource = _mealSuggestions;
            AcceptMealSuggestions_AppBarButton.IsEnabled = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                if (e.Parameter is MealViewModel)
                {
                    _meal = e.Parameter as MealViewModel;
                }
                else if (e.Parameter is MealSuggestionViewModel)
                {
                    _mealSuggestion = null;
                }
            }
        }

        private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (MealSuggestionsGridView.SelectedItems.Count > 0)
            {
                AcceptMealSuggestions_AppBarButton.IsEnabled = true;
                _mealSuggestion = (MealSuggestionViewModel)MealSuggestionsGridView.SelectedItem;
            }
        }

        private void AddMealSuggestions_AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            _mealSuggestion = new MealSuggestionViewModel();
            this.Frame.Navigate(typeof(MealItemsByCategory), _mealSuggestion);
        }

        private void AcceptMealSuggestions_AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (_meal.MealItemIDsWithWeight.Count == 0)
            {
                _meal.MealItemIDsWithWeight = _mealSuggestion.MealItemIDsWithWeight;
                _meal.SetDefaultMealItemWeights(_meal);
                this.Frame.Navigate(typeof(MealItems), _meal);
            }
            else
            {
                Dialogs.ShowYesNoDialog("Das Essen enthält bereits Menüteile? Diese überschreiben?",
                    MessageDialogInvokedHandler);
            }
        }

        private void MessageDialogInvokedHandler(IUICommand command)
        {
            if (command.Label == "Ja")
            {
                _meal.MealItemIDsWithWeight = _mealSuggestion.MealItemIDsWithWeight;
                this.Frame.Navigate(typeof(MealItems), _meal);
            }
        }

        private void MealSuggestionsGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MealSuggestionsGridView.SelectedItems.Count > 0)
            {
                AcceptMealSuggestions_AppBarButton.IsEnabled = true;
                _mealSuggestion = (MealSuggestionViewModel)MealSuggestionsGridView.SelectedItem;
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;

            if (DeliveryNoteButton.Visibility == Visibility.Visible)
                DeliveryNoteButton.Visibility = Visibility.Collapsed;
            else if (DeliveryNoteButton.Visibility == Visibility.Collapsed)
                DeliveryNoteButton.Visibility = Visibility.Visible;
        }

        private void DeliveryNoteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuButton2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuButton3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MealSuggestionButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
