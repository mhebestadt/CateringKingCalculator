using hebestadt.CateringKingCalculator.Models;
using hebestadtaCateringKingCalculator;
using System.Linq;

namespace hebestadt.CateringKingCalculator.ViewModels
{
    public class IngredientViewModel : ViewModelBase
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

        private string manufacturer = string.Empty;
        public string Manufacturer
        {
            get
            { return manufacturer; }

            set
            {
                if (manufacturer == value)
                { return; }

                manufacturer = value;
                isDirty = true;
                RaisePropertyChanged("Manufacturer");
            }
        }

        private float weight = 0;
        public float Weight
        {
            get
            { return weight; }

            set
            {
                if (weight == value)
                { return; }

                weight = value;
                isDirty = true;
                RaisePropertyChanged("Weight");
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

        private float ingredientMealWeight = 0;
        public float IngredientMealWeight
        {
            get
            { return ingredientMealWeight;  }

            set
            {
                if (ingredientMealWeight == value)
                { return; }

                ingredientMealWeight = value;
                isDirty = true;
                RaisePropertyChanged("IngredientMealWeight");
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

        public IngredientViewModel GetIngredient(float ingredientId, float ingredientMealWeight)
        {
            var ingredient = new IngredientViewModel();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var _ingredient = (db.Table<Ingredient>().Where(
                    c => c.Id == ingredientId)).Single();
                ingredient.Id = _ingredient.Id;
                ingredient.Name = _ingredient.Name;
                ingredient.Manufacturer = _ingredient.Manufacturer;
                ingredient.Weight = _ingredient.Weight;
                ingredient.UnitOfMeasure = _ingredient.UnitOfMeasure;
                ingredient.Category = _ingredient.Category;
                ingredient.IngredientMealWeight = ingredientMealWeight;
            }

            return ingredient;
        }

        public string SaveIngredient(IngredientViewModel ingredient)
        {
            string result = string.Empty;
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                string change = string.Empty;
                try
                {
                    var existingIngredient = (db.Table<Ingredient>().Where(
                        c => c.Id == ingredient.Id)).SingleOrDefault();

                    if (existingIngredient != null)
                    {
                        existingIngredient.Name = ingredient.Name;
                        existingIngredient.Weight = ingredient.Weight;
                        existingIngredient.UnitOfMeasure = ingredient.UnitOfMeasure;
                        
                        int success = db.Update(existingIngredient);
                    }
                    else
                    {
                        int success = db.Insert(new Ingredient()
                        {
                            Name = ingredient.Name,
                            Weight = ingredient.Weight,
                            UnitOfMeasure = ingredient.UnitOfMeasure
                        });
                    }
                    result = "Success";
                }
                catch
                {
                    result = "This ingredient was not saved.";
                }
            }

            return result;
        }

        public string DeleteIngredient(int ingredientId)
        {
            string result = string.Empty;
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var existingIngredient = (db.Table<Ingredient>().Where(
                    c => c.Id == ingredientId)).Single();

                if (db.Delete(existingIngredient) > 0)
                {
                    result = "Success";
                }
                else
                {
                    result = "This ingredient was not removed";
                }
            }
            return result;
        }
    }
}
