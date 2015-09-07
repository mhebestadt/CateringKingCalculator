using CateringKingCalculator.ViewModels;
using hebestadt.CateringKingCalculator.Models;
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
    public sealed partial class MealItemsByCategory : Page
    {
        ObservableCollection<FoodCategoryViewModel> _foodCategories = null;
        MealItemsViewModel mealItemsViewModel = null;
        MealViewModel meal = null;

        public MealItemsByCategory()
        {
            this.InitializeComponent();

            FoodCategoriesViewModel FoodCategoriesViewModel = new FoodCategoriesViewModel();
            _foodCategories = FoodCategoriesViewModel.GetFoodCategories();
            FoodCategoriesListView.ItemsSource = _foodCategories;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (e.Parameter != null)
            {
                meal = (MealViewModel)e.Parameter;
            }
        }

        private void FoodCategoriesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FoodCategoryViewModel foodCategory = null;
            foodCategory = (FoodCategoryViewModel)FoodCategoriesListView.SelectedItem;
            MealItemsListView.ItemsSource = null;
            MealItemsListView.Items.Clear();
            mealItemsViewModel = new MealItemsViewModel();
            ObservableCollection<MealItemViewModel> mealItems = mealItemsViewModel.GetMealItemsByCategory(foodCategory.Name);
            MealItemsListView.ItemsSource = mealItems;
        }

        private void NavigateBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MealItems), meal);
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string result = String.Empty;
            var mealItems = MealItemsListView.SelectedItems;

            foreach (var _mealItem in mealItems)
            {
                result = meal.AddMealItem(meal, ((MealItemViewModel)_mealItem).Id, 0);
            }            

            if (string.CompareOrdinal("Success", result) == 0)
            {
                this.Frame.Navigate(typeof(MealItems), meal);
            }
            else
            {
                var dialog = new MessageDialog(result);
                await dialog.ShowAsync();
            }
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
            int numberSelectedItems = MealItemsListView.SelectedItems.Count;

            if (numberSelectedItems > 1)
                EditButton.IsEnabled = false;
            else
                EditButton.IsEnabled = true;
        }
    }
}