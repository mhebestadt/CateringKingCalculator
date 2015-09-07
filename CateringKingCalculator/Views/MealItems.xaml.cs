using hebestadt.CateringKingCalculator.Converters;
using hebestadt.CateringKingCalculator.Models;
using hebestadt.CateringKingCalculator.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using System.Globalization;
using System.IO;
using Windows.UI.Xaml.Documents;
using CateringKingCalculator.ViewModels;
using System.Linq;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CateringKingCalculator.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MealItems : Page
    {
        MealItemsViewModel mealItemsViewModel = null;
        MealItemViewModel currentSelectedMealItem = null;
        ObservableCollection<MealItemViewModel> _mealItems = null;
        MealViewModel meal = null;
        IngredientsViewModel ingredientsViewModel = null;
        ListView listViewIngredientsDetail = new ListView();

        public MealItems()
        {
            this.InitializeComponent();

            MealItemsGridView.ItemsSource = null;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (e.Parameter != null)
            {
                meal = (MealViewModel)e.Parameter;
                Dictionary<float, float> mealItemIDsWithWeight = meal.mealItemIDsWithWeight;

                ListViewStackPanel.Children.Add(listViewIngredientsDetail);
                listViewIngredientsDetail.SelectionChanged += ListView_SelectionChanged;

                Thickness margin = listViewIngredientsDetail.Margin;
                margin.Bottom = 0;
                margin.Left = 380;
                margin.Right = 100;
                margin.Top = 100;
                listViewIngredientsDetail.Margin = margin;
                mealItemsViewModel = new MealItemsViewModel();
                _mealItems = mealItemsViewModel.GetMealItems(mealItemIDsWithWeight);

                MealItemsGridView.ItemsSource = _mealItems;               

                if (meal.Name == "")
                {
                    SaveButton.IsEnabled = false;
                }
                else { ContactNameTextBox.Text = meal.Name; }
                
                this.DataContext = meal;
                
                NumberOfGuestsTextBox.Text = meal.NumberOfGuests.ToString();
                DeliveryDatePicker.Date = meal.DeliveryDate;
                DeliveryTimePicker.Time = meal.DeliveryTime;
                DeliveryNoteIdTextBox.Text = meal.DeliveryNoteId;
            }
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MealItemsByCategory), meal);
        }

        private void NavigateBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StartPage));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string result = meal.DeleteMeal(meal.Id);

            if (string.CompareOrdinal("Success", result) == 0)
            {
                this.Frame.Navigate(typeof(StartPage));
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string result = meal.SaveMeal(meal);

            if (string.CompareOrdinal("Success", result) == 0)
            {
                this.Frame.Navigate(typeof(StartPage));
            }
        }

        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            MealItemViewModel mealItem = null;
            mealItem = (MealItemViewModel)MealItemsGridView.SelectedItem;
            string result = meal.RemoveMealItem(meal, mealItem.Id);

            if (string.CompareOrdinal("Success", result) == 0)
            {

            }
        }

        private void MealItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MealItemViewModel mealItem = null;
            mealItem = (MealItemViewModel)MealItemsGridView.SelectedItem;

            IngredientsListView.ItemsSource = null;
            IngredientsListView.Items.Clear();
            ingredientsViewModel = new IngredientsViewModel();
            ObservableCollection<IngredientViewModel> ingredients = ingredientsViewModel.GetIngredients(mealItem.IngredientIDsWithTotalAmount);
            IngredientsListView.ItemsSource = ingredients;

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MealItemsDetailListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void NameTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            if (ContactNameTextBox.Text != "")
            {
                SaveButton.IsEnabled = true;
            }
        }

        private void NewCategoryButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void WeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value;
            if (currentSelectedMealItem != null)
            {
                if (WeightTextBox.Text != "" || int.TryParse(WeightTextBox.Text, out value) || WeightTextBox.Text == ",")
                {
                    string str = WeightTextBox.Text.Replace(',', '.');
                    this.meal.mealItemIDsWithWeight[currentSelectedMealItem.Id] =
                        float.Parse(str, CultureInfo.InvariantCulture.NumberFormat);                 
                }
            }
        }

        private void MealItemsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            currentSelectedMealItem =  (MealItemViewModel)e.ClickedItem;
            WeightTextBlock.Visibility = Visibility.Visible;
            WeightTextBox.Visibility = Visibility.Visible;

            UnitOfMeasureViewModel unitOfMeasure = new UnitOfMeasureViewModel();
            string unitOfMeasureName = unitOfMeasure.GetUnitOfMeasure(currentSelectedMealItem.TotalAmountUnitOfMeasure).UnitName;
            WeightTextBlock.Text = "Menge in " + unitOfMeasureName;
            string mealItemWeight = this.meal.mealItemIDsWithWeight[currentSelectedMealItem.Id].ToString();
            WeightTextBox.Text = mealItemWeight.Replace('.', ',');
        }

        private void DeliveryDatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            if (e.NewDate >= DateTimeOffset.Now)
            {
                meal.DeliveryDate = e.NewDate;
            }
        }

        private void DeliveryTimePicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {

            if (TimeSpan.Compare(e.NewTime, e.OldTime) != 0)
                meal.DeliveryTime = e.NewTime;
        }

        private void DeliveryNoteButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DeliveryNote), meal);
        }

        private void MenuButton2_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MealItemsByCategory), meal);
        }

        private void ContactNameTextBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            //We only want to get results when it was a user typing, 
            //otherwise we assume the value got filled in by TextMemberPath 
            //or the handler for SuggestionChosen
            if ((args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) && (sender.Text != ""))
            {
                ContactViewModel contactViewModel = new ContactViewModel();
                ObservableCollection<ContactViewModel> matchingContacts = contactViewModel.GetMatchingContacts(sender.Text);
                sender.ItemsSource = matchingContacts;
                //var matchingContacts = contactViewModel.GetMatchingContacts(sender.Text);

                //sender.ItemsSource = matchingContacts.ToList();
            }
        }
    }
}
