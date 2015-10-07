using hebestadt.CateringKingCalculator.Models;
using hebestadt.CateringKingCalculator.Converters;
using hebestadtaCateringKingCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using CateringKingCalculator.Converters;
using System.Text;
using System.Collections.ObjectModel;
using CateringKingCalculator.ViewModels;

namespace hebestadt.CateringKingCalculator.ViewModels
{
    public class MealViewModel : ViewModelBase, IMealViewModel
    {

        #region Properties

        private static GenericListToByteArray _converter = new GenericListToByteArray();
        private static GenericDictionaryToByteArray _dictionaryConverter = new GenericDictionaryToByteArray();
        private static GenericDictionaryToByteArrayFloat _dictionaryConverterFloat = new GenericDictionaryToByteArrayFloat();
        private static DateTimeToStringConverter _dateTimeConverter = new DateTimeToStringConverter();
        private static TimeSpanToStringConverter _timeSpanConverter = new TimeSpanToStringConverter();

        private int id = 0;
        public int Id
        {
            get
            { return id; }

            set
            {
                if (id == value)
                { return; }

                id = value;
                RaisePropertyChanged("Id");
            }
        }

        private string deliverynoteid = string.Empty;
        public string DeliveryNoteId
        {
            get
            {
                return deliverynoteid;
            }

            set
            {
                if (deliverynoteid == value)
                { return; }

                deliverynoteid = value;
                isDirty = true;
                RaisePropertyChanged("DeliveryNoteId");
            }
        }

        private string name = string.Empty;
        public string Name
        {
            get
            { return name; }

            set
            {
                if (name == value)
                { return; }

                name = value;
                isDirty = true;
                RaisePropertyChanged("Name");
            }
        }
                      
        private bool silverware = false;
        public bool SilverWare
        {
            get                                   
            { return silverware; }

            set
            {
                if (silverware == value)
                { return; }

                silverware = value;
                isDirty = true;
                RaisePropertyChanged("SilverWare");
            }
        }
                                                                                                                 
        private int numberofguests = 0;
        public int NumberOfGuests
        {                                                                 
            get
            { return numberofguests; }

            set
            {
                if (numberofguests == value)
                { return; }

                numberofguests = value;
                isDirty = true;
                RaisePropertyChanged("NumberOfGuests");
            }
        }


        private int contactid = 0;
        public int ContactId
        {
            get
            {
                return contactid;
            }

            set
            {
                if (contactid == value)
                { return; }

                contactid = value;

                isDirty = true;
                RaisePropertyChanged("ContactId");
            }
        }

        private string contact = string.Empty;
        public string Contact
        {
            get
            { return contact; }

            set
            {
                if (contact == value)
                { return; }

                contact = value;
                isDirty = true;
                RaisePropertyChanged("Contact");
            }
        }

        private DateTimeOffset deliverydate = System.DateTime.Today.AddDays(7);
        public DateTimeOffset DeliveryDate
        {
            get
            { return deliverydate; }

            set
            {
                if (deliverydate == value)
                { return; }

                deliverydate = value;
                isDirty = true;
                RaisePropertyChanged("DeliveryDate");
            }
        }

        private DateTimeOffset pickupdate = System.DateTime.Today.AddDays(7);
        public DateTimeOffset PickupDate
        {
            get { return pickupdate; }

            set
            {
                if (pickupdate == value)
                { return; }
                pickupdate = value;
                isDirty = true;
                RaisePropertyChanged("PickupDate");
            }
        }

        private TimeSpan deliverytime = System.TimeSpan.Zero;
        public TimeSpan DeliveryTime
        {
            get
            { return deliverytime;  }

            set
            {
                if (deliverytime == value)
                { return;  }

                deliverytime = value;
                isDirty = true;
                RaisePropertyChanged("DeliveryTime");
            }
        }

        private string deliverylocation = string.Empty;
        public string DeliveryLocation
        {
            get
            { return deliverylocation; }

            set
            {
                if (deliverylocation == value)
                { return; }

                deliverylocation = value;
                isDirty = true;
                RaisePropertyChanged("DeliveryLocation");
            }
        }

        private Dictionary<float, float> mealitemidswithweight = new Dictionary<float, float>();
        public Dictionary<float, float> MealItemIDsWithWeight
        {
            get
            {
                return mealitemidswithweight;
            }

            set
            {
                mealitemidswithweight = value;
                isDirty = true;
                RaisePropertyChanged("mealItemIDsWithWeight");
            }
        }

        private List<int> mealitemids = new List<int> { };
        public List<int> MealItemIDs
        {
            get
            { return mealitemids; }

            set
            {
                mealitemids = value;
                isDirty = true;
                RaisePropertyChanged("MealItemIDs");
            }
        }

        private bool isDirty = false;
        public bool IsDirty
        {
            get
            {
                return isDirty;
            }

            set
            {
                isDirty = value;
                RaisePropertyChanged("IsDirty");
            }
        }

        int IMealViewModel.CategoryId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion "Properties"

        public IMealViewModel GetMeal(int mealId)
        {
            var meal = new MealViewModel();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var _meal = (db.Table<Meal>().Where(
                    c => c.Id == mealId)).Single();
                meal.Id = _meal.Id;
                meal.DeliveryNoteId = _meal.DeliveryNoteId;
                meal.Name = _meal.Name;
                meal.DeliveryDate = _meal.DeliveryDate;
                meal.DeliveryTime = _meal.DeliveryTime;
                meal.DeliveryLocation = _meal.DeliveryLocation;
                meal.PickupDate = _meal.PickupDate;
                meal.Contact = _meal.Contact;
                meal.ContactId = _meal.ContactId;
                meal.NumberOfGuests = _meal.NumberOfGuests;
                meal.SilverWare = _meal.SilverWare;
                meal.MealItemIDs = (List<int>)_converter.ConvertBack(_meal.MealItemIDs, null, null, "");
                meal.MealItemIDsWithWeight = (Dictionary<float, float>)_dictionaryConverterFloat.ConvertBack(_meal.MealItemIDsWithWeight, null, null, "");
            }

            return meal;
        }

        public string SaveMeal(IMealViewModel meal)
        {
            string result = string.Empty;
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                string change = string.Empty;
                try
                {
                    var existingMeal = (db.Table<Meal>().Where(
                        c => c.Id == meal.Id)).SingleOrDefault();

                    if (existingMeal != null)
                    {
                        existingMeal.DeliveryNoteId = meal.DeliveryNoteId;
                        existingMeal.Name = meal.Name;
                        existingMeal.DeliveryDate = meal.DeliveryDate;
                        existingMeal.DeliveryTime = meal.DeliveryTime;
                        existingMeal.DeliveryLocation = meal.DeliveryLocation;
                        existingMeal.PickupDate = meal.PickupDate;
                        existingMeal.Contact = meal.Contact;
                        existingMeal.ContactId = meal.ContactId;
                        existingMeal.NumberOfGuests = meal.NumberOfGuests;
                        existingMeal.SilverWare = meal.SilverWare;
                        existingMeal.MealItemIDs = (byte[])_converter.Convert(meal.MealItemIDs, null, null, "");
                        existingMeal.MealItemIDsWithWeight = 
                            (byte[])_dictionaryConverterFloat.Convert(meal.MealItemIDsWithWeight, null, null, "");

                        int success = db.Update(existingMeal);
                    }
                    else
                    {
                        int success = db.Insert(new Meal()
                        {
                            DeliveryNoteId = meal.DeliveryNoteId,
                            Name = meal.Name,
                            DeliveryDate = meal.DeliveryDate,
                            DeliveryTime = meal.DeliveryTime,
                            DeliveryLocation = meal.DeliveryLocation,
                            PickupDate = meal.PickupDate,
                            Contact = meal.Contact,
                            ContactId = meal.ContactId,
                            NumberOfGuests = meal.NumberOfGuests,
                            SilverWare = meal.SilverWare,
                            MealItemIDs = (byte[])_converter.Convert(meal.MealItemIDs, null, null, ""),
                            MealItemIDsWithWeight =(byte[])_dictionaryConverterFloat.Convert(meal.MealItemIDsWithWeight, null, null, "")
                    });
                    }
                    result = "Success";
                }
                catch
                {
                    result = "This meal was not saved.";
                }
            }

            return result;
        }

        public string DeleteMeal(int mealId)
        {
            string result = string.Empty;
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var existingMeal = (db.Table<Meal>().Where(
                    c => c.Id == mealId)).Single();

                if (db.Delete(existingMeal) > 0)
                {
                    result = "Success";
                }
                else
                {
                    result = "This meal was not removed";
                }
            }
            return result;
        }

        public string AddMealItem(IMealViewModel meal, int mealItemID, float mealItemWeight)
        {
            string result = string.Empty;
            float foundMealItemWeight = -1;

            if (!meal.MealItemIDsWithWeight.TryGetValue(mealItemID, out foundMealItemWeight))
            {
                meal.MealItemIDsWithWeight.Add(mealItemID, mealItemWeight);
                result = meal.SaveMeal(meal);
            }
            else
            {
                result = "Meal Item already exits";
            }

            return result;
        }

        public string RemoveMealItem(IMealViewModel meal, int mealItemID)
        {
            string result = string.Empty;

            meal.MealItemIDsWithWeight.Remove(mealItemID);
            result = meal.SaveMeal(meal);

            return result;
        }

        public string SetDefaultMealItemWeights(IMealViewModel meal)
        {
            string result = "";
            MealItemViewModel mealItem = new MealItemViewModel();

            foreach (var mealItemKeyValue in meal.MealItemIDsWithWeight.ToList())
            { 
                mealItem = mealItem.GetMealItemById((int)mealItemKeyValue.Key);
                float mealItemWeightDefault = mealItem.TotalAmount;
                float totalWeight = mealItemWeightDefault * meal.NumberOfGuests;
                meal.MealItemIDsWithWeight[mealItemKeyValue.Key] = totalWeight;
            }
            
            return result;
        }

        public string GetTextRepresentation(IMealViewModel meal)
        {
            StringBuilder result = new StringBuilder();
            MealItemsViewModel mealItemsView = new MealItemsViewModel();
            IngredientsViewModel ingredientsView = new IngredientsViewModel();
            ContactViewModel contact = new ContactViewModel();
            contact = contact.GetContact(meal.ContactId);
            string tmp = contact.NameAndAddress;
            tmp = tmp.Replace('\n', '|');
            tmp = tmp.Replace('\r', '|');
            string nameAndAddress = tmp.Replace("||", "\\line ");
            nameAndAddress = contact.Attention + "\\line " + tmp.Replace("||", "\\line ");

            result.Append(@"{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang1033{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset0 Calibri;}}");
            result.Append(@"{\*\generator Riched20 10.0.10240}\viewkind4\uc1\pard\tx720\cf1\f0\fs22\lang1031\line\line\line\f1\fs26");
            result.Append(" Lieferschein Nr. ").Append(meal.DeliveryNoteId).Append(@"\line \line "); ;
            result.Append(nameAndAddress).Append(@" \line ");
            result.Append("Tel. Nr. : ").Append(contact.PhoneNr).Append(@" \line ");
            result.Append("Handy Nr. : ").Append(contact.CellPhoneNr).Append(@" \line \line ");
            result.Append("Veranstaltungsort:").Append(@" \line ");
            result.Append(meal.DeliveryLocation).Append(@" \line \line ");
            result.Append("Ihr Ansprechpartner: ").Append(@" \line \line ");
            result.Append(_dateTimeConverter.Convert(meal.DeliveryDate, null, null, "")).Append("  ");
            result.Append(_timeSpanConverter.Convert(meal.DeliveryTime,null,null,"")).Append("  Uhr Buffetbeginn").Append(@"\line \line \line");
            result.Append(@" \highlight2 ");
            result.Append("Erwachsene: ").Append(meal.NumberOfGuests).Append(@"\line \line ");
            result.Append(@" \highlight0 ");

            ObservableCollection<MealItemViewModel> _mealItems = 
                mealItemsView.GetMealItems(meal.MealItemIDsWithWeight);

            foreach (var mealItem in _mealItems)
            {
                float mealItemWeight = meal.MealItemIDsWithWeight[mealItem.Id];
                result.Append(@"\b ");
                result.Append(mealItem.Name.ToString()).Append("   ");

                UnitOfMeasureViewModel unitOfMeasure = new UnitOfMeasureViewModel();
                string unitOfMeasureName = unitOfMeasure.GetUnitOfMeasure(mealItem.TotalAmountUnitOfMeasure).UnitName;
                string unitOfMeasureAbbreviation = unitOfMeasure.GetUnitOfMeasure(mealItem.TotalAmountUnitOfMeasure).Abbreviation;

                result.Append(mealItemWeight.ToString()).Append("").Append(unitOfMeasureAbbreviation);
                result.Append(@"\line\b0 ");
                result.Append(GetIngredientsAsText(ingredientsView, mealItem, meal.MealItemIDsWithWeight));
                result.Append(@" \line ");
            }

            result.Append(@" \line }");

            return result.ToString();
        }

        private string GetIngredientsAsText(IngredientsViewModel ingredientsView, 
            MealItemViewModel mealItem, Dictionary<float,float> mealItemIDsWithWeight)
        {
            StringBuilder sb = new StringBuilder();
            ObservableCollection<IngredientViewModel> ingredients =
                    ingredientsView.GetIngredients(mealItem.IngredientIDsWithTotalAmount);

            if (ingredients.Count > 0)
            {
                //sb.Append(@"\b ");
                int loopCount = 0;
                UnitOfMeasureViewModel unitOfMeasure = new UnitOfMeasureViewModel();
                MealItemViewModel defaultMealItem = new MealItemViewModel();
                defaultMealItem = defaultMealItem.GetMealItemById(mealItem.Id);

                foreach (var ingredient in ingredients)
                {
                    float ingredientDefaultWeight = mealItem.IngredientIDsWithTotalAmount[ingredient.Id];
                    if (defaultMealItem.TotalAmount == 0) defaultMealItem.TotalAmount = 1;
                    float ingredientWeight = (ingredientDefaultWeight / defaultMealItem.TotalAmount) * mealItemIDsWithWeight[mealItem.Id];
                    decimal roundedIngredientWeight = Math.Round((decimal)ingredientWeight, 0);
                    string uomName = unitOfMeasure.GetUnitOfMeasure(ingredient.UnitOfMeasure).Abbreviation;
                    sb.Append(roundedIngredientWeight.ToString());
                    sb.Append(uomName).Append(" ");
                    sb.Append(ingredient.Name);

                    if ((ingredients.Count >1) && (loopCount < (ingredients.Count-1)))
                    { sb.Append(", "); }

                    loopCount++;
                }

                //sb.Append(@"\line\b0");
                sb.Append(@"\line");
                return sb.ToString();
            }
            else
                return "";
        }
    }
}
