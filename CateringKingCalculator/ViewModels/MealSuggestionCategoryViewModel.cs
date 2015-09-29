using hebestadt.CateringKingCalculator.Models;
using hebestadtaCateringKingCalculator;
using SQLite;
using System.Collections.ObjectModel;
using System.Linq;

namespace hebestadt.CateringKingCalculator.ViewModels
{
    class MealSuggestionCategoryViewModel : ViewModelBase
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
                isDirty = true;
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

        public string Save(MealSuggestionCategoryViewModel mealSuggestionCategory)
        {
            string result = string.Empty;
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                string change = string.Empty;
                try
                {
                    var existingMealSuggestion = (db.Table<MealSuggestionCategory>().Where(
                        c => c.Id == mealSuggestionCategory.Id)).SingleOrDefault();

                    if (existingMealSuggestion != null)
                    {
                        existingMealSuggestion.Name = mealSuggestionCategory.Name;

                        int success = db.Update(existingMealSuggestion);
                    }
                    else
                    {
                        int success = db.Insert(new MealSuggestionCategoryViewModel()
                        {
                            Name = mealSuggestionCategory.Name
                        });

                        SQLiteCommand cmd = db.CreateCommand("SELECT last_insert_rowid()");
                        int rowId = cmd.ExecuteScalar<int>();
                        cmd.CommandText = "SELECT Id FROM MealSuggestionCategory WHERE rowid = " + rowId.ToString();
                        mealSuggestionCategory.Id = cmd.ExecuteScalar<int>();
                    }
                    result = "Success";
                }
                catch
                {
                    result = "This meal suggestion category was not saved.";
                }
            }

            return result;
        }

        public MealSuggestionCategoryViewModel GetMealSuggestionCategory(int mealSuggestionCategoryId)
        {
            var mealSuggestionCategory = new MealSuggestionCategoryViewModel();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var _mealSuggestionCategory = (db.Table<MealSuggestionCategory>().Where(
                    c => c.Id == mealSuggestionCategoryId)).Single();
                mealSuggestionCategory.Id = _mealSuggestionCategory.Id;
                mealSuggestionCategory.Name = _mealSuggestionCategory.Name;
            }

            return mealSuggestionCategory;
        }

        public ObservableCollection<MealSuggestionCategoryViewModel> GetAllCategories()
        {
            ObservableCollection<MealSuggestionCategoryViewModel> mealSuggestionCategories =
                new ObservableCollection<MealSuggestionCategoryViewModel>();

            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var query = db.Table<MealSuggestionCategory>().OrderBy(c => c.Id);
                foreach (var _mealSuggestionCategory in query)
                {
                    var mealSuggestionCategory = new MealSuggestionCategoryViewModel()
                    {
                        Id = _mealSuggestionCategory.Id,
                        Name = _mealSuggestionCategory.Name
                    };

                    mealSuggestionCategories.Add(mealSuggestionCategory);
                }
            }

            return mealSuggestionCategories;
        }

    }
}
