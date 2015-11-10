using CateringKingCalculator.Converters;
using CateringKingCalculator.ViewModels;
using hebestadt.CateringKingCalculator.Converters;
using hebestadt.CateringKingCalculator.Models;
using hebestadt.CateringKingCalculator.ViewModels;
using hebestadtaCateringKingCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hebestadt.CateringKingCalculator.ViewModels
{
    public class MealItemViewModel : ViewModelBase
    {
        #region Properties

        private static GenericListToByteArray _converter = new GenericListToByteArray();
        private static GenericDictionaryToByteArrayFloat _dictionaryConverterFloat = new GenericDictionaryToByteArrayFloat();

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

        private string category = string.Empty;
        public string Category
        {
            get
            { return category; }

            set
            {
                if (category == value)
                { return; }

                category = value;
                isDirty = true;
                RaisePropertyChanged("Category");
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
                isDirty = true;
                RaisePropertyChanged("CategoryId");
            }
        }

        private float totalamount = 0;
        public float TotalAmount
        {
            get
            {
                return totalamount;
            }

            set
            {
                if (totalamount == value)
                { return; }

                totalamount = value;
                isDirty = true;
                RaisePropertyChanged("TotalAmount");
            }
        }

        private int totalamountunitofmeasure = 0;
        public int TotalAmountUnitOfMeasure
        {
            get
            {
                return totalamountunitofmeasure;
            }

            set
            {
                if (totalamountunitofmeasure == value)
                { return; }

                totalamountunitofmeasure = value;
                isDirty = true;
                RaisePropertyChanged("TotalAmountUnitOfMeasure");
            }
        }

        private Dictionary<float, float> ingredientidswithtotalamount = new Dictionary<float,float>();
        public Dictionary<float, float> IngredientIDsWithTotalAmount
        {
            get
            {
                return ingredientidswithtotalamount;
            }

            set
            {
                ingredientidswithtotalamount = value;
                isDirty = true;
                RaisePropertyChanged("IngredientIDsWithTotalAmount");
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

        #endregion Properties

        public MealItemViewModel GetMealItemById(int meaItemId)
        {
            var mealitem = new MealItemViewModel();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                try
                {
                    var _mealitem = (db.Table<MealItem>().Where(
                        c => c.Id == meaItemId)).Single();
                    mealitem.Id = _mealitem.Id;
                    mealitem.Name = _mealitem.Name;
                    mealitem.Category = _mealitem.Category;
                    mealitem.TotalAmount = _mealitem.TotalAmount;
                    mealitem.TotalAmountUnitOfMeasure = _mealitem.TotalAmountUnitOfMeasure;
                    mealitem.IngredientIDsWithTotalAmount = (Dictionary<float, float>)_dictionaryConverterFloat.ConvertBack(_mealitem.IngredientIDsWithTotalAmount, null, null, "");
                }
                catch(Exception e)
                { }
            }

            return mealitem;
        }

        public string SaveMealItem(MealItemViewModel mealitem)
        {
            string result = string.Empty;
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                string change = string.Empty;
                try
                {
                    var existingMealItem = (db.Table<MealItem>().Where(
                        c => c.Id == mealitem.Id)).SingleOrDefault();

                    if (existingMealItem != null)
                    {
                        existingMealItem.Name = mealitem.Name;
                        existingMealItem.Category = mealitem.Category;
                        existingMealItem.TotalAmount = mealitem.TotalAmount;
                        existingMealItem.TotalAmountUnitOfMeasure = mealitem.TotalAmountUnitOfMeasure;
                        existingMealItem.CategoryId = ((db.Table<FoodCategory>().Where(c => c.Name == mealitem.Category)).Single()).Id;
                        existingMealItem.IngredientIDsWithTotalAmount = (byte[])_dictionaryConverterFloat.Convert(mealitem.IngredientIDsWithTotalAmount, null, null, "");
                        
                        int success = db.Update(existingMealItem);
                    }
                    else
                    {
                        var foodCategory = (db.Table<FoodCategory>().Where(c => c.Name == mealitem.Category)).Single();

                        int success = db.Insert(new MealItem()
                        {
                            Name = mealitem.Name,
                            Category = mealitem.Category,
                            CategoryId = foodCategory.Id,
                            TotalAmount = mealitem.TotalAmount,
                            TotalAmountUnitOfMeasure = mealitem.TotalAmountUnitOfMeasure,
                            IngredientIDsWithTotalAmount = (byte[])_dictionaryConverterFloat.Convert(mealitem.IngredientIDsWithTotalAmount, null, null, "")
                        });
                    }

                    result = "Success";
                }
                catch
                {
                    result = "This meal item was not saved.";
                }
            }

            return result;
        }

        public bool MealItemExists(string mealItemName)
        {
            bool result = true;  //initialized true because nothing happens in that case

            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var mealItem = db.Table<MealItem>()
                    .Where(c => c.Name.Contains(mealItemName));
                if (mealItem.Count() > 0) result = true;
                else result = false;
            }

            return result;
        }

        public string DeleteMealItem(int mealitemId)
        {
            string result = string.Empty;
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var existingMealItem = (db.Table<MealItem>().Where(
                    c => c.Id == mealitemId)).Single();

                if (db.Delete(existingMealItem) > 0)
                {
                    result = "Success";
                }
                else
                {
                    result = "This meal item was not removed";
                }
            }
            return result;
        }
    }
}
