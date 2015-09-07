using hebestadt.CateringKingCalculator;
using hebestadt.CateringKingCalculator.Models;
using hebestadt.CateringKingCalculator.ViewModels;
using hebestadtaCateringKingCalculator;
using System.Linq;

namespace CateringKingCalculator.ViewModels
{
    public class FoodCategoryViewModel : ViewModelBase
    {
        #region Properties

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

        private int unitofmeasure = 0;
        public int UnitOfMeasure
        {
            get
            { return unitofmeasure; }

            set
            {
                if (unitofmeasure == value)
                { return; }

                unitofmeasure = value;
                isDirty = true;
                RaisePropertyChanged("UnitOfMeasure");
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

        public FoodCategoryViewModel GetFoodCategory(int foodCategoryId)
        { 
            var foodcategoryitem = new FoodCategoryViewModel();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var _foodCategory = (db.Table<FoodCategory>().Where(
                    c => c.Id == foodCategoryId)).Single();
                foodcategoryitem.Id = _foodCategory.Id;
                foodcategoryitem.Name = _foodCategory.Name;
                foodcategoryitem.UnitOfMeasure = _foodCategory.UnitOfMeasure;
            }

            return foodcategoryitem;
        }


        public string SaveFoodCategory(FoodCategoryViewModel foodCategory)
        {
            string result = string.Empty;
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                //string change = string.Empty;
                try
                {
                    var existingFoodCategory = (db.Table<FoodCategory>().Where(
                        c => c.Id == foodCategory.Id)).SingleOrDefault();

                    if (existingFoodCategory != null)
                    {
                        existingFoodCategory.Name = foodCategory.Name;
                        existingFoodCategory.UnitOfMeasure = foodCategory.UnitOfMeasure;

                        int success = db.Update(existingFoodCategory);
                    }
                    else
                    {
                        int success = db.Insert(new FoodCategory()
                        {
                            Name = foodCategory.Name,
                            UnitOfMeasure = foodCategory.UnitOfMeasure
                        });
                    }

                    result = "Success";
                }
                catch
                {
                    result = "This food category item was not saved.";
                }
            }

            return result;
        }
    }
}
