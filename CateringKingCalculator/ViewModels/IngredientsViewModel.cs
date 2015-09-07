using hebestadtaCateringKingCalculator;
using hebestadt.CateringKingCalculator.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using CateringKingCalculator.Extensions;

namespace hebestadt.CateringKingCalculator.ViewModels
{
    public class IngredientsViewModel : ViewModelBase
    {
        IngredientViewModel ingredientViewModel = new IngredientViewModel();

        private ObservableCollection<IngredientViewModel> ingredients;
        public ObservableCollection<IngredientViewModel> Ingredients
        {
            get
            {
                return ingredients;
            }

            set
            {
                ingredients = value;
                RaisePropertyChanged("Ingredients");
            }
        }

        public ObservableCollection<IngredientViewModel> GetIngredients(Dictionary<float, float> ingredientIDs)
        {
            ingredients = new ObservableCollection<IngredientViewModel>();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                foreach (var item in ingredientIDs)
                {
                    ingredients.Add(ingredientViewModel.GetIngredient(item.Key, item.Value));
                }
            }

            return ingredients;
        }

        public ObservableCollection<IngredientViewModel> GetAllIngredients()
        {
            ingredients = new ObservableCollection<IngredientViewModel>();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var query = db.Table<Ingredient>().OrderBy(c => c.Name);
                foreach (var _ingredient in query)
                {
                    var ingredient = new IngredientViewModel()
                    {
                        Id = _ingredient.Id,
                        Name = _ingredient.Name,
                        Weight = _ingredient.Weight,
                        UnitOfMeasure = _ingredient.UnitOfMeasure
                    };

                    ingredients.Add(ingredient);
                }
            }

            return ingredients;
        }
    }
}
