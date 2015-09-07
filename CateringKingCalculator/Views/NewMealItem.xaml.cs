using CateringKingCalculator.Extensions;
using CateringKingCalculator.ViewModels;
using hebestadt.CateringKingCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CateringKingCalculator.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewMealItem : Page
    {
        IngredientViewModel ingredientViewModel = new IngredientViewModel();
        IngredientsViewModel ingredientsViewModel = new IngredientsViewModel();
        ObservableCollection<IngredientViewModel> _allIngredients = null;
        ObservableCollection<IngredientViewModel> _selectedIngredientsList = null;
        ObservableCollection<UnitOfMeasureViewModel> _unitOfMeasureList = null;
        MealItemViewModel _existingMealItem = null;
        int currentSelectedIngredientIndex = -1;
        

        public NewMealItem()
        {
            this.InitializeComponent();

            IngredientsViewModel ingredientsViewModel = new IngredientsViewModel();
            _allIngredients = ingredientsViewModel.GetAllIngredients();
            _selectedIngredientsList = new ObservableCollection<IngredientViewModel>();
            UnitsOfMeasureViewModel unitsOfMeasureViewModel = new UnitsOfMeasureViewModel();
            _unitOfMeasureList = unitsOfMeasureViewModel.GetUnitsOfMeasure();
            this.DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Type type = e.Parameter.GetType();
            TotalAmountUOMTextBox.ItemsSource = _unitOfMeasureList;

            if (type == typeof(FoodCategoryViewModel))
            {
                FoodCategoryViewModel category = (FoodCategoryViewModel)e.Parameter;
                CategoryTextBox.Text = category.Name;
                CategoryTextBox.IsReadOnly = true;
                _existingMealItem = new MealItemViewModel();
                _existingMealItem.Category = category.Name;
            }
            else if (type == typeof(MealItemViewModel))
            {
                _existingMealItem = (MealItemViewModel)e.Parameter;
                CategoryTextBox.Text = _existingMealItem.Category;
                CategoryTextBox.IsReadOnly = true;
                NameTextBox.Text = _existingMealItem.Name;
                NameTextBox.IsReadOnly = true;
                ingredientsViewModel = new IngredientsViewModel();
                _selectedIngredientsList = ingredientsViewModel.GetIngredients(_existingMealItem.IngredientIDsWithTotalAmount);
                TotalAmountTextBox.Text = _existingMealItem.TotalAmount.ToString();
                TotalAmountUOMTextBox.SelectedIndex = _existingMealItem.TotalAmountUnitOfMeasure-1;

                var copy = new ObservableCollection<IngredientViewModel>(_selectedIngredientsList);
                foreach (var ingredient in copy)
                {
                    FindAndRemoveIngredient(_allIngredients, ingredient);
                }
            }

            AllIngredientsList.ItemsSource = _allIngredients;
            SelectedIngredientsList.ItemsSource = _selectedIngredientsList;           
        }

        private void FindAndRemoveIngredient(ObservableCollection<IngredientViewModel> source, IngredientViewModel item)
        {
            foreach (var ingredient in source)
            {
                if (ingredient.Id == item.Id)
                {
                    bool removed = source.Remove(ingredient);
                    break;
                }
                
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_existingMealItem == null)
            {
                _existingMealItem = new MealItemViewModel();
                _existingMealItem.Name = NameTextBox.Text.ToString();
                _existingMealItem.Category = CategoryTextBox.Text.ToString();
            }

            if (NameTextBox.Text == "")
            {
                var dialog = new MessageDialog("Bitte einen Namen angeben.");
                await dialog.ShowAsync();
                return;
            }

            if (_existingMealItem.Name == "")
                _existingMealItem.Name = NameTextBox.Text.ToString();

            _existingMealItem.TotalAmount = float.Parse(TotalAmountTextBox.Text, CultureInfo.InvariantCulture.NumberFormat);
            UnitOfMeasureViewModel unitOfMeasure = (UnitOfMeasureViewModel)TotalAmountUOMTextBox.SelectedItem;
            _existingMealItem.TotalAmountUnitOfMeasure = unitOfMeasure.Id;

            string result = _existingMealItem.SaveMealItem(_existingMealItem);
            
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddMealButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NavigateBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MealItemsByCategory));
        }

        private void IngredientsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            IngredientViewModel ingredient = (IngredientViewModel)e.ClickedItem;

            if (_selectedIngredientsList.Contains(ingredient))
            {
                _selectedIngredientsList.Remove(ingredient);
                currentSelectedIngredientIndex = -1;
                IngredientAmount.Visibility = Visibility.Collapsed;
                IngredientAmountTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                UnitOfMeasureViewModel uom = new UnitOfMeasureViewModel();
                switch (ingredient.UnitOfMeasure)
                {
                    case 1:
                        {
                            IngredientAmountTextBlock.Text = uom.GetUnitOfMeasure(1).UnitName;
                            
                            break;
                        }
                    case 2:
                        {
                            IngredientAmountTextBlock.Text = uom.GetUnitOfMeasure(2).UnitName;
                            break;
                        }
                    case 3:
                        {
                            IngredientAmountTextBlock.Text = uom.GetUnitOfMeasure(3).UnitName;
                            break;
                        }
                }
                IngredientAmount.Text = ingredient.Weight.ToString();
                IngredientAmount.Visibility = Visibility.Visible;
                IngredientAmountTextBlock.Visibility = Visibility.Visible;
                _selectedIngredientsList.Add(ingredient);
                currentSelectedIngredientIndex = _selectedIngredientsList.IndexOf(ingredient);
            }
        }

        private void Amount_TextChanged(object sender, TextChangedEventArgs e)
        {
            IngredientViewModel ingredient = (IngredientViewModel)SelectedIngredientsList.SelectedItem;

            if (IngredientAmount.Text != "")
            {
                if (_existingMealItem.IngredientIDsWithTotalAmount.ContainsKey(ingredient.Id))
                    _existingMealItem.IngredientIDsWithTotalAmount[ingredient.Id] = 
                        float.Parse(IngredientAmount.Text, CultureInfo.InvariantCulture.NumberFormat);
                else
                    _existingMealItem.IngredientIDsWithTotalAmount.Add(ingredient.Id, 
                        float.Parse(IngredientAmount.Text, CultureInfo.InvariantCulture.NumberFormat));
            }
        }

        private void NavLinksList_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        private void AllIngredientsList_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            IngredientViewModel ingredient = (IngredientViewModel)AllIngredientsList.SelectedItem;

            if (!_selectedIngredientsList.Contains(ingredient))
            {
                _existingMealItem.IngredientIDsWithTotalAmount.Add(ingredient.Id, 0);
                _selectedIngredientsList.Add(ingredient);                
                _allIngredients.Remove(ingredient);
            }
            
        }

        private void SelectedIngredientsList_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            IngredientViewModel ingredient = (IngredientViewModel)SelectedIngredientsList.SelectedItem;

            if (!_allIngredients.Contains(ingredient))
            {
                _existingMealItem.IngredientIDsWithTotalAmount.Remove(ingredient.Id);
                _selectedIngredientsList.Remove(ingredient);
                _allIngredients.Insert(ingredient.Id, ingredient);
                IngredientAmount.Visibility = Visibility.Collapsed;
                IngredientAmountTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void SelectedIngredientsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            IngredientViewModel ingredient = (IngredientViewModel)e.ClickedItem;
            
            switch (ingredient.UnitOfMeasure)
            {
                case 1:
                    {
                        IngredientAmountTextBlock.Text = "Menge in Gramm";

                        break;
                    }
                case 2:
                    {
                        IngredientAmountTextBlock.Text = "Menge in Milliliter";
                        break;
                    }
                case 3:
                    {
                        IngredientAmountTextBlock.Text = "Menge in Stück";
                        break;
                    }
            }

            Dictionary<float, float> dict = new Dictionary<float, float>();
            dict = _existingMealItem.IngredientIDsWithTotalAmount;

            if (dict.ContainsKey(ingredient.Id))
                IngredientAmount.Text = dict[ingredient.Id].ToString();
            else
                IngredientAmount.Text = "";

            IngredientAmount.Visibility = Visibility.Visible;
            IngredientAmountTextBlock.Visibility = Visibility.Visible;
        }

        private void TotalAmountUOMTextBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
