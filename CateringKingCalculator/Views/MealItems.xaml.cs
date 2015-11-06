using hebestadt.CateringKingCalculator.Models;
using hebestadt.CateringKingCalculator.ViewModels;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using System.Globalization;
using CateringKingCalculator.ViewModels;
using System.Linq;
using Windows.UI.Popups;
using hebestadt.CateringKingCalculator.Dialogs;

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
        MealViewModel _meal = null;
        IngredientsViewModel ingredientsViewModel = null;
        ListView listViewIngredientsDetail = new ListView();
        ContactViewModel _contact = null;
        string _numberOfGuests = "";
        bool _numberOfGuestsChanged = false;

        public MealItems()
        {
            this.InitializeComponent();

            MealItemsGridView.ItemsSource = null;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                /* This section adds dynamic list view - it will be removed at some point */
                ListViewStackPanel.Children.Add(listViewIngredientsDetail);
                listViewIngredientsDetail.SelectionChanged += ListView_SelectionChanged;
                Thickness margin = listViewIngredientsDetail.Margin;
                margin.Bottom = 0;
                margin.Left = 380;
                margin.Right = 100;
                margin.Top = 100;
                listViewIngredientsDetail.Margin = margin;
                /*-----------------------------------------------------------------------*/

                _meal = (MealViewModel)e.Parameter;
                mealItemsViewModel = new MealItemsViewModel();
                _mealItems = mealItemsViewModel.GetMealItems(_meal.MealItemIDsWithWeight);

                MealItemsGridView.ItemsSource = _mealItems;

                _contact = new ContactViewModel();
                _contact = _contact.GetContact(_meal.ContactId);
                ContactNameTextBox.Text = _contact.NameAndAddress;

                this.DataContext = _meal;
                
                NumberOfGuestsTextBox.Text = _meal.NumberOfGuests.ToString();
                _numberOfGuests = _meal.NumberOfGuests.ToString();
                DeliveryDatePicker.Date = _meal.DeliveryDate;
                DeliveryTimePicker.Time = _meal.DeliveryTime;
                DeliveryNoteIdTextBox.Text = _meal.DeliveryNoteId;
            }
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MealItemsByCategory), _meal);
        }

        private void NavigateBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StartPage));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Dialogs.ShowYesNoDialog("Dieses Essen wirklich löschen??",
                new UICommandInvokedHandler(this.DeleteMealInvokedHandler));
        }

        private void DeleteMealInvokedHandler(IUICommand command)
        {
            if (command.Label == "Ja")
            {
                string result = _meal.DeleteMeal(_meal.Id);

                if (string.CompareOrdinal("Success", result) == 0)
                {
                    this.Frame.Navigate(typeof(StartPage));
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_numberOfGuestsChanged == true)
            {
                Dialogs.ShowYesNoDialog("Anzahl der Gäste wurde geändert. Die Änderungen jetzt speichern?", SaveNoGuestsInvokeHandler);
            }
            else
            {
                _meal.SaveMeal(_meal);
                this.Frame.Navigate(typeof(StartPage));
            }
        }

        private void MealItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IngredientsListView.ItemsSource = null;
            IngredientsListView.Items.Clear();

            MealItemViewModel mealItem = null;
            mealItem = (MealItemViewModel)MealItemsGridView.SelectedItem;

            if (mealItem != null)
            {
                ingredientsViewModel = new IngredientsViewModel();
                ObservableCollection<IngredientViewModel> ingredients = ingredientsViewModel.GetIngredients(mealItem.IngredientIDsWithTotalAmount);
                IngredientsListView.ItemsSource = ingredients;
            }
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
                    this._meal.MealItemIDsWithWeight[currentSelectedMealItem.Id] =
                        float.Parse(str, CultureInfo.InvariantCulture.NumberFormat);                 
                }
            }
        }

        private void NumberOfGuestsTextBox_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
           /*
            int oldNumberOfGuests = Int32.Parse(_numberOfGuests);
            int newNumberOfGuests = 0; 
            float mealItemTotalWeight = 0;

            if(NumberOfGuestsTextBox.Text != "")
                newNumberOfGuests = Int32.Parse(NumberOfGuestsTextBox.Text);

            if ((e.Key == Windows.System.VirtualKey.Enter) && (oldNumberOfGuests != newNumberOfGuests))
            {    
                if (currentSelectedMealItem != null)
                {    
                    mealItemTotalWeight = float.Parse(WeightTextBox.Text, CultureInfo.InvariantCulture.NumberFormat);
                    float mealItemNewWeight = (mealItemTotalWeight / oldNumberOfGuests) * newNumberOfGuests;
                    WeightTextBox.Text = mealItemNewWeight.ToString().Replace('.', ',');
                }

                foreach (var mealIDAndWeight in _meal.MealItemIDsWithWeight.ToList())
                {
                    mealItemTotalWeight = mealIDAndWeight.Value;
                    
                    decimal roundedWeight = Math.Round((decimal)(mealItemTotalWeight / oldNumberOfGuests) * newNumberOfGuests, 0);
                    _meal.MealItemIDsWithWeight[mealIDAndWeight.Key] = (float)roundedWeight;
                }

                MealItemsGridView.Focus(FocusState.Keyboard);
            }
            */
        }

        private void MenuButton3_Click(object sender, RoutedEventArgs e)
        {
            Dialogs.ShowYesNoDialog("Alle Gewichte zurücksetzen?", new UICommandInvokedHandler(this.ResetWeightsInvokedHandler));
        }

        private void ResetWeightsInvokedHandler(IUICommand command)
        {
            if (command.Label == "Ja")
            {
                float mealItemTotalWeight = 0;

                if (NumberOfGuestsTextBox.Text != "")
                {
                    int numberOfGuests = Int32.Parse(NumberOfGuestsTextBox.Text);
                    MealItemViewModel defaultMealItem = new MealItemViewModel();

                    foreach (var mealIDAndWeight in _meal.MealItemIDsWithWeight.ToList())
                    {
                        defaultMealItem = defaultMealItem.GetMealItemById((int)mealIDAndWeight.Key);
                        mealItemTotalWeight = defaultMealItem.TotalAmount;

                        decimal roundedWeight = Math.Round((decimal)(mealItemTotalWeight * numberOfGuests), 0);
                        _meal.MealItemIDsWithWeight[mealIDAndWeight.Key] = (float)roundedWeight;
                    }
                }
            }
        }

        private void NumberOfGuestsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _numberOfGuestsChanged = true;
        }

        private void MealItemsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            currentSelectedMealItem = (MealItemViewModel)e.ClickedItem;
            WeightTextBlock.Visibility = Visibility.Visible;
            WeightTextBox.Visibility = Visibility.Visible;

            UnitOfMeasureViewModel unitOfMeasure = new UnitOfMeasureViewModel();
            string unitOfMeasureName = unitOfMeasure.GetUnitOfMeasure(currentSelectedMealItem.TotalAmountUnitOfMeasure).UnitName;
            WeightTextBlock.Text = "Menge in " + unitOfMeasureName;

            string mealItemWeight = "";
            if (_meal.MealItemIDsWithWeight[currentSelectedMealItem.Id] > 0)
            {
                mealItemWeight = this._meal.MealItemIDsWithWeight[currentSelectedMealItem.Id].ToString();
            }
            else
            {
                float mealItemWeightDefault = currentSelectedMealItem.TotalAmount;
                int numberGuestes = Int32.Parse(NumberOfGuestsTextBox.Text);
                float totalWeight = mealItemWeightDefault * numberGuestes;
                mealItemWeight = totalWeight.ToString();
            }

            WeightTextBox.Text = mealItemWeight.Replace('.', ',');
        }

        private void DeliveryDatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            if (e.NewDate >= DateTimeOffset.Now)
            {
                _meal.DeliveryDate = e.NewDate;
            }
        }

        private void DeliveryTimePicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {

            if (TimeSpan.Compare(e.NewTime, e.OldTime) != 0)
                _meal.DeliveryTime = e.NewTime;
        }

        private void DeliveryNoteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_numberOfGuestsChanged == true)
            {
                Dialogs.ShowYesNoDialog("Anzahl der Gäste wurde geändert. Jetzt speichern?", SaveNoGuestsInvokeHandler);
            }
            else
            {
                this.Frame.Navigate(typeof(DeliveryNote), _meal);
            }
        }

        private void SaveNoGuestsInvokeHandler(IUICommand command)
        {
            if (command.Label == "Ja")
            {
                int oldNumberOfGuests = Int32.Parse(_numberOfGuests);
                int newNumberOfGuests = 0;
                float mealItemTotalWeight = 0;

                if (NumberOfGuestsTextBox.Text != "")
                {
                    newNumberOfGuests = Int32.Parse(NumberOfGuestsTextBox.Text);

                    foreach (var mealIDAndWeight in _meal.MealItemIDsWithWeight.ToList())
                    {
                        mealItemTotalWeight = mealIDAndWeight.Value;

                        decimal roundedWeight = Math.Round((decimal)(mealItemTotalWeight / oldNumberOfGuests) * newNumberOfGuests, 0);
                        _meal.MealItemIDsWithWeight[mealIDAndWeight.Key] = (float)roundedWeight;
                    }

                    _meal.SaveMeal(_meal);
                }
            }
        }

        private void MenuButton2_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MealItemsByCategory), _meal);
        }

        private void ContactNameTextBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            //We only want to get results when it was a user typing, 
            //otherwise we assume the value got filled in by TextMemberPath 
            //or the handler for SuggestionChosen
            if ((args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) && (sender.Text != ""))
            {
                ContactViewModel contactViewModel = new ContactViewModel();
                //ObservableCollection<ContactViewModel> matchingContacts = contactViewModel.GetMatchingContacts(sender.Text);
                ObservableCollection<string> matchingContacts = contactViewModel.GetMatchingContacts(sender.Text);
                sender.ItemsSource = matchingContacts;
            }
        }

        private void ContactNameTextBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                //User selected an item, take an action on it here
                var test = args.ChosenSuggestion;
            }
        }

        private void NumberOfGuestsTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _numberOfGuests = NumberOfGuestsTextBox.Text;
        }

        private void MealSuggestionButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MealSuggestions), _meal);
        }

        private void RemoveMealItemButton_Click(object sender, RoutedEventArgs e)
        {
            string result = "";
            MealItemViewModel mealItem = null;
            mealItem = (MealItemViewModel)MealItemsGridView.SelectedItem;
            if (mealItem != null) result = _meal.RemoveMealItem(_meal, mealItem.Id);
            if (string.CompareOrdinal("Success", result) == 0) _mealItems.Remove(mealItem);
        }

        private void EditMealItemButton_Click(object sender, RoutedEventArgs e)
        {
            MealItemViewModel mealItem = null;
            mealItem = (MealItemViewModel)MealItemsGridView.SelectedItem;
            if (mealItem != null) this.Frame.Navigate(typeof(NewMealItem), mealItem);
        }
    }
}
