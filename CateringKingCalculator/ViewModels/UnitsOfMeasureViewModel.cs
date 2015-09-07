using hebestadt.CateringKingCalculator.Models;
using hebestadt.CateringKingCalculator.ViewModels;
using hebestadtaCateringKingCalculator;
using System.Collections.ObjectModel;

namespace CateringKingCalculator.ViewModels
{
    class UnitsOfMeasureViewModel : ViewModelBase
    {
        private ObservableCollection<UnitOfMeasureViewModel> _unitsOfMeasure;
        public ObservableCollection<UnitOfMeasureViewModel> UnitsOfMeasure
        {
            get
            {
                return _unitsOfMeasure;
            }

            set
            {
                _unitsOfMeasure = value;
                RaisePropertyChanged("UnitsOfMeasure");
            }
        }

        public ObservableCollection<UnitOfMeasureViewModel> GetUnitsOfMeasure()
        {
            _unitsOfMeasure = new ObservableCollection<UnitOfMeasureViewModel>();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var query = db.Table<UnitOfMeasure>().OrderBy(c => c.Id);
                foreach (var _unitOfMeasure in query)
                {
                    var unitOfMeasure = new UnitOfMeasureViewModel()
                    {
                        Id = _unitOfMeasure.Id,
                        Abbreviation = _unitOfMeasure.Abbreviation,
                        UnitName = _unitOfMeasure.UnitName
                    };

                    _unitsOfMeasure.Add(unitOfMeasure);
                }
            }

            return _unitsOfMeasure;
        }
    }
}
