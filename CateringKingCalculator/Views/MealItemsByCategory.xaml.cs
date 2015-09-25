using CateringKingCalculator.ViewModels;
using hebestadt.CateringKingCalculator.Models;
using hebestadt.CateringKingCalculator.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CateringKingCalculator.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MealItemsByCategory : Page
    {
        ObservableCollection<FoodCategoryViewModel> _foodCategories = null;
        ObservableCollection<MealItemViewModel> _selectedMealItems = null;
        MealItemsViewModel mealItemsViewModel = null;
        IMealViewModel _meal = null;
        bool _foodCategoriesSelectionChanged = false;

        public MealItemsByCategory()
        {
            this.InitializeComponent();

            FoodCategoriesViewModel FoodCategoriesViewModel = new FoodCategoriesViewModel();
            _foodCategories = FoodCategoriesViewModel.GetFoodCategories();
            FoodCategoriesListView.ItemsSource = _foodCategories;
            _selectedMealItems = new ObservableCollection<MealItemViewModel>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (e.Parameter != null)
            {
                _meal = (IMealViewModel)e.Parameter;
            }
        }

        private void FoodCategoriesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MealItemsListView.SelectedItems.Count > 0)
                _foodCategoriesSelectionChanged = true;

            FoodCategoryViewModel foodCategory = null;
            foodCategory = (FoodCategoryViewModel)FoodCategoriesListView.SelectedItem;
            MealItemsListView.ItemsSource = null;
            MealItemsListView.Items.Clear();
            mealItemsViewModel = new MealItemsViewModel();
            ObservableCollection<MealItemViewModel> mealItems = mealItemsViewModel.GetMealItemsByCategory(foodCategory.Name);
            MealItemsListView.ItemsSource = mealItems;

            int loopCount = 0;
            foreach (MealItemViewModel mealItem in MealItemsListView.Items)
            {
                if (_selectedMealItems.Any<MealItemViewModel>(p => p.Id == mealItem.Id))
                {
                    MealItemsListView.SelectRange(new ItemIndexRange(loopCount, 1));
                }

                loopCount++;
            }
        }

        private void NavigateBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MealItems), _meal);
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string result = String.Empty;

            foreach (var _mealItem in _selectedMealItems)
            {
                result = _meal.AddMealItem(_meal, ((MealItemViewModel)_mealItem).Id, 0);
            }

            /* Leave this here for now must think about it
            if (string.CompareOrdinal("Success", result) == 0)
            {
                this.Frame.Navigate(typeof(MealItems), meal);
            }
            else
            {
                var dialog = new MessageDialog(result);
                await dialog.ShowAsync();
            }
            */
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var mealItem = MealItemsListView.SelectedItems[0];
            this.Frame.Navigate(typeof(NewMealItem), mealItem);
        }

        private void AddMealItemButton_Click(object sender, RoutedEventArgs e)
        {
            FoodCategoryViewModel category = (FoodCategoryViewModel)FoodCategoriesListView.SelectedItem;
            if (category != null)
            {
                this.Frame.Navigate(typeof(NewMealItem), category);
            }
        }

        private void MealItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_foodCategoriesSelectionChanged)
            {
                foreach (MealItemViewModel mealItem in e.RemovedItems)
                {
                    _selectedMealItems.Remove(mealItem);
                }

                foreach (MealItemViewModel mealItem in e.AddedItems)
                {
                    _selectedMealItems.Add(mealItem);
                }
            }
            else
            {
                _foodCategoriesSelectionChanged = false;
            }

            int numberSelectedItems = MealItemsListView.SelectedItems.Count;

            if (numberSelectedItems > 1)
                EditButton.IsEnabled = false;
            else
                EditButton.IsEnabled = true;
        }
    }
}