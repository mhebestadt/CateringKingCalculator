using hebestadt.CateringKingCalculator.Models;
using hebestadt.CateringKingCalculator.ViewModels;
using hebestadtaCateringKingCalculator;
using System.Collections.ObjectModel;

namespace CateringKingCalculator.ViewModels
{
    public class FoodCategoriesViewModel : ViewModelBase
    {
        private ObservableCollection<FoodCategoryViewModel> _foodCategories;
        public ObservableCollection<FoodCategoryViewModel> FoodCategories
        {
            get
            {
                return _foodCategories;
            }

            set
            {
                _foodCategories = value;
                RaisePropertyChanged("FoodCategories");
            }
        }

        public ObservableCollection<FoodCategoryViewModel> GetFoodCategories()
        {
            _foodCategories = new ObservableCollection<FoodCategoryViewModel>();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var query = db.Table<FoodCategory>().OrderBy(c => c.Id);
                foreach (var _foodCategory in query)
                {
                    var foodcategory = new FoodCategoryViewModel()
                    {
                        Id = _foodCategory.Id,
                        Name = _foodCategory.Name
                    };

                    _foodCategories.Add(foodcategory);
                }
            }

            return _foodCategories;
        }
    }
}
