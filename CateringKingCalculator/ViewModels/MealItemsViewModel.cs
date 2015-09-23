using CateringKingCalculator.Converters;
using hebestadt.CateringKingCalculator.Comparerers;
using hebestadt.CateringKingCalculator.Converters;
using hebestadt.CateringKingCalculator.Models;
using hebestadt.CateringKingCalculator.ViewModels;
using hebestadtaCateringKingCalculator;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace hebestadt.CateringKingCalculator.Models
{
    public class MealItemsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private static GenericListToByteArray _converter = new GenericListToByteArray();
        private static GenericDictionaryToByteArrayFloat _dictionaryConverterFloat = 
            new GenericDictionaryToByteArrayFloat();

        private ObservableCollection<MealItemViewModel> mealitems;
        public ObservableCollection<MealItemViewModel> MealItems
        {
            get
            {
                return mealitems;
            }

            set
            {
                mealitems = value;
                RaisePropertyChanged("MealItems");
            }
        }

        protected void AddSorted<T>(IList<T> list, T item, IComparer<T> comparer = null)
        {
            if (comparer == null)
                comparer = Comparer<T>.Default;

            int i = 0;
            while (i < list.Count && comparer.Compare(list[i], item) < 0)
                i++;

            list.Insert(i, item);
        }

        public ObservableCollection<MealItemViewModel> GetAllMealItems()
        {
            mealitems = new ObservableCollection<MealItemViewModel>();

            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var query = db.Table<MealItem>().OrderBy(c => c.CategoryId);
                foreach (var _mealitem in query)
                {
                    var mealitem = new MealItemViewModel()
                    {
                        Id = _mealitem.Id,
                        Name = _mealitem.Name,
                        Category = _mealitem.Category,
                        IngredientIDsWithTotalAmount = 
                        (Dictionary<float, float>)_dictionaryConverterFloat.ConvertBack(_mealitem.IngredientIDsWithTotalAmount, null, null, "")
                    };

                    AddSorted(mealitems, mealitem, new CategoryCompare());
                }
            }

            return mealitems;
        }

        public ObservableCollection<MealItemViewModel> GetMealItems(Dictionary<float, float> ingredientIdsWithWeight)
        {
            mealitems = new ObservableCollection<MealItemViewModel>();

            if (ingredientIdsWithWeight == null) { return MealItems; }

            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                foreach (var _itemIDWeight in ingredientIdsWithWeight)
                {
                    var query = db.Table<MealItem>().Where(
                        p1 => p1.Id == _itemIDWeight.Key).OrderBy(c => c.CategoryId);
                    foreach (var _mealitem in query)
                    {
                        var mealitem = new MealItemViewModel()
                        {
                            Id = _mealitem.Id,
                            Name = _mealitem.Name,
                            Category = _mealitem.Category,
                            CategoryId = _mealitem.CategoryId,
                            TotalAmount = _mealitem.TotalAmount,
                            TotalAmountUnitOfMeasure = _mealitem.TotalAmountUnitOfMeasure,
                            IngredientIDsWithTotalAmount = 
                                (Dictionary<float, float>)_dictionaryConverterFloat.ConvertBack(_mealitem.IngredientIDsWithTotalAmount, null, null, "")
                        };

                        AddSorted(mealitems, mealitem, new CategoryCompare());
                    }
                }
            }

            return mealitems;
        }

        public ObservableCollection<MealItemViewModel> GetMealItemsByCategory(string categoryName)
        {
            mealitems = new ObservableCollection<MealItemViewModel>();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var query = db.Table<MealItem>().Where(
                        p1 => p1.Category == categoryName);

                foreach (var _mealitem in query)
                {
                    var mealitem = new MealItemViewModel()
                    {
                        Id = _mealitem.Id,
                        Name = _mealitem.Name,
                        Category = _mealitem.Category,
                        TotalAmount = _mealitem.TotalAmount,
                        TotalAmountUnitOfMeasure = _mealitem.TotalAmountUnitOfMeasure,
                        IngredientIDsWithTotalAmount = 
                            (Dictionary<float, float>)_dictionaryConverterFloat.ConvertBack(_mealitem.IngredientIDsWithTotalAmount, null, null, "")
                    };

                    mealitems.Add(mealitem);
                }
            }

            return mealitems;
        }
    }
}
