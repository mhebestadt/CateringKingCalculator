using CateringKingCalculator.ViewModels;
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
    public sealed partial class NewMealCustomerEventDataInput : Page
    {
        static readonly ObservableCollection<string> _attention = new ObservableCollection<string>() { "Frau", "Herr", "Firma" };
        static readonly ObservableCollection<string> _contactPerson =
            new ObservableCollection<string>() { "Karina Helmert", "Sigrid Kluge" };
        MealViewModel _meal = null;
        ContactViewModel _contact = null;

        public NewMealCustomerEventDataInput()
        {
            this.InitializeComponent();

            _contact = new ContactViewModel();

            AttentionTextBox.ItemsSource = _attention;
            ContactTextBox.ItemsSource = _contactPerson;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SaveButton.IsEnabled = false;

            if (e.Parameter != null)
            {
                _meal = (MealViewModel)e.Parameter;
            }
        }

        private bool HasEnoughData()
        {
            bool result = false;

            if (AttentionTextBox.SelectedItem != null)
            {
                if (AttentionTextBox.SelectedItem.ToString() != "" &&
                    CustomerDataTextBox.Text != "")
                {
                    result = true;
                }
            }

            return result;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_meal != null)
            {
                _contact.Attention = AttentionTextBox.SelectedItem.ToString();
                _contact.NameAndAddress = CustomerDataTextBox.Text;
                string result = _contact.SaveContact(_contact);

                if (result == "Success")
                {
                    _meal.ContactId = _contact.Id;
                    _meal.DeliveryDate = DeliveryDatePicker.Date;
                    _meal.DeliveryTime = DeliveryTimePicker.Time;
                    _meal.DeliveryLocation = EventLocationDataTextBox.Text;
                    _meal.PickupDate = PickupDatePicker.Date;
                    SaveButton.IsEnabled = false;
                    this.Frame.Navigate(typeof(MealItems), _meal);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AttentionTextBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HasEnoughData())
                SaveButton.IsEnabled = true;
        }

        private void CustomerDataTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (HasEnoughData())
                SaveButton.IsEnabled = true;
        }

        private void DeliveryTimePicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {

        }

        private void DeliveryDatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {

        }

        private void PickupDatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {

        }

        private void PickupTimePicker_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {

        }
    }
}
