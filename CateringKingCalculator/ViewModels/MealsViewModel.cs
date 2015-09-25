using hebestadt.CateringKingCalculator.Converters;
using hebestadtaCateringKingCalculator;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CateringKingCalculator.Converters;

namespace hebestadt.CateringKingCalculator.ViewModels
{
    public class MealsViewModel : ViewModelBase
    {
        private static GenericListToByteArray _converter = new GenericListToByteArray();
        private static GenericDictionaryToByteArray _dictionaryConverter = new GenericDictionaryToByteArray();
        private static GenericDictionaryToByteArrayFloat _dictionaryConverterFloat = new GenericDictionaryToByteArrayFloat();

        private ObservableCollection<MealViewModel> meals;
        public ObservableCollection<MealViewModel> Meals
        {
            get
            {
                return meals;
            }

            set
            {
                meals = value;
                RaisePropertyChanged("Meals");
            }
        }

        public ObservableCollection<MealViewModel> GetMeals()
        {
            meals = new ObservableCollection<MealViewModel>();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var query = db.Table<Meal>().OrderBy(c => c.Name);
                foreach (var _meal in query)
                {
                    var meal = new MealViewModel()
                    {
                        Id = _meal.Id,
                        DeliveryNoteId = _meal.DeliveryNoteId,
                        Name = _meal.Name,
                        DeliveryDate = _meal.DeliveryDate,
                        DeliveryTime = _meal.DeliveryTime,
                        DeliveryLocation = _meal.DeliveryLocation,
                        PickupDate = _meal.PickupDate,
                        Contact = _meal.Contact,
                        ContactId = _meal.ContactId,
                        NumberOfGuests = _meal.NumberOfGuests,
                        SilverWare = _meal.SilverWare,
                        MealItemIDs = (List<int>)_converter.ConvertBack(_meal.MealItemIDs, null, null, ""),
                        MealItemIDsWithWeight = (Dictionary<float,float>)_dictionaryConverterFloat.ConvertBack(_meal.MealItemIDsWithWeight, null, null, "")
                    };

                    meals.Add(meal);
                }
            }

            return meals;
        }
    }
}
