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

        public NewMealCustomerEventDataInput()
        {
            this.InitializeComponent();

            AttentionTextBox.ItemsSource = _attention;
            ContactTextBox.ItemsSource = _contactPerson;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

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
