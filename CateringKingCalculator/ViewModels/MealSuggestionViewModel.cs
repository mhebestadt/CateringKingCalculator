using CateringKingCalculator.Converters;
using hebestadt.CateringKingCalculator.Models;
using hebestadtaCateringKingCalculator;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.ObjectModel;
using SQLite;

namespace hebestadt.CateringKingCalculator.ViewModels
{
    public class MealSuggestionViewModel : ViewModelBase, IMealViewModel
    {
        #region Properties

        private static GenericDictionaryToByteArrayFloat _dictionaryConverterFloat = 
            new GenericDictionaryToByteArrayFloat();

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

        private int categoryid = 0;
        public int CategoryId
        {
            get
            { return categoryid; }

            set
            {
                if (categoryid == value)
                { return; }

                categoryid = value;
                RaisePropertyChanged("CategoryId");
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

        string IMealViewModel.Contact
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

        int IMealViewModel.ContactId
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

        DateTimeOffset IMealViewModel.DeliveryDate
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

        string IMealViewModel.DeliveryLocation
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

        string IMealViewModel.DeliveryNoteId
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

        TimeSpan IMealViewModel.DeliveryTime
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

        bool IMealViewModel.IsDirty
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

        List<int> IMealViewModel.MealItemIDs
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

        string IMealViewModel.Name
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

        int IMealViewModel.NumberOfGuests
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

        DateTimeOffset IMealViewModel.PickupDate
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

        bool IMealViewModel.SilverWare
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

        #endregion Properties

        public IMealViewModel GetMeal(int mealId)
        {
            var mealSuggestion = new MealSuggestionViewModel();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var _mealSuggestion = (db.Table<MealSuggestion>().Where(
                    c => c.Id == mealId)).Single();
                mealSuggestion.Id = _mealSuggestion.Id;
                mealSuggestion.CategoryId = _mealSuggestion.CategoryId;
                mealSuggestion.MealItemIDsWithWeight = 
                    (Dictionary<float, float>)_dictionaryConverterFloat.ConvertBack(_mealSuggestion.MealItemIDsWithWeight, null, null, "");
            }

            return mealSuggestion;
        }

        public string SaveMeal(IMealViewModel mealSuggestion)
        {
            string result = string.Empty;
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                string change = string.Empty;
                try
                {
                    var existingMealSuggestion = (db.Table<MealSuggestion>().Where(
                        c => c.Id == mealSuggestion.Id)).SingleOrDefault();

                    if (existingMealSuggestion != null)
                    {
                        existingMealSuggestion.CategoryId = mealSuggestion.CategoryId;
                        existingMealSuggestion.MealItemIDsWithWeight =
                            (byte[])_dictionaryConverterFloat.Convert(mealSuggestion.MealItemIDsWithWeight, null, null, "");

                        int success = db.Update(existingMealSuggestion);
                    }
                    else
                    {
                        int success = db.Insert(new MealSuggestion()
                        {
                            CategoryId = mealSuggestion.CategoryId,
                            MealItemIDsWithWeight = 
                                (byte[])_dictionaryConverterFloat.Convert(mealSuggestion.MealItemIDsWithWeight, null, null, "")
                        });

                        SQLiteCommand cmd = db.CreateCommand("SELECT last_insert_rowid()");
                        int rowId = cmd.ExecuteScalar<int>();
                        cmd.CommandText = "SELECT Id FROM MealSuggestion WHERE rowid = " + rowId.ToString();
                        mealSuggestion.Id = cmd.ExecuteScalar<int>();
                    }
                    result = "Success";
                }
                catch
                {
                    result = "This meal suggestion was not saved.";
                }
            }

            return result;
        }

        public string DeleteMealSuggestion(int mealSuggestionId)
        {
            string result = string.Empty;
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var existingMealSuggestion = (db.Table<MealSuggestion>().Where(
                    c => c.Id == mealSuggestionId)).Single();

                if (db.Delete(existingMealSuggestion) > 0)
                {
                    result = "Success";
                }
                else
                {
                    result = "This meal suggestion was not removed";
                }
            }

            return result;
        }

        public string AddMealItem(IMealViewModel mealSuggestion, int mealItemID, float mealItemWeight)
        {
            string result = string.Empty;
            float foundMealItemWeight = -1;

            if (!mealSuggestion.MealItemIDsWithWeight.TryGetValue(mealItemID, out foundMealItemWeight))
            {
                mealSuggestion.MealItemIDsWithWeight.Add(mealItemID, mealItemWeight);
                result = mealSuggestion.SaveMeal(mealSuggestion);
            }
            else
            {
                result = "Meal Item already exits";
            }

            return result;
        }

        public string RemoveMealItem(IMealViewModel mealSuggestion, int mealItemID)
        {
            string result = string.Empty;

            mealSuggestion.MealItemIDsWithWeight.Remove(mealItemID);
            result = mealSuggestion.SaveMeal(mealSuggestion);

            return result;
        }

        public ObservableCollection<IMealViewModel> GetMeals()
        {
            ObservableCollection<IMealViewModel> mealSuggestions = 
                new ObservableCollection<IMealViewModel>();

            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var query = db.Table<MealSuggestion>().OrderBy(c => c.Id);
                foreach (var _mealSuggestion in query)
                {
                    var mealSuggestion = new MealSuggestionViewModel()
                    {
                        Id = _mealSuggestion.Id,
                        CategoryId = _mealSuggestion.CategoryId,
                        MealItemIDsWithWeight = 
                            (Dictionary<float, float>)_dictionaryConverterFloat.ConvertBack(_mealSuggestion.MealItemIDsWithWeight, null, null, "")
                    };

                    mealSuggestions.Add(mealSuggestion);
                }
            }

            return mealSuggestions;
        }

        string IMealViewModel.DeleteMeal(int mealId)
        {
            throw new NotImplementedException();
        }

        string IMealViewModel.GetTextRepresentation(IMealViewModel meal)
        {
            throw new NotImplementedException();
        }
    }
}
