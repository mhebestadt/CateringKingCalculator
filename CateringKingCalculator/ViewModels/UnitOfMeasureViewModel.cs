using hebestadt.CateringKingCalculator;
using hebestadt.CateringKingCalculator.Models;
using hebestadtaCateringKingCalculator;
using System;
using System.Linq;

namespace CateringKingCalculator.ViewModels
{
    public class UnitOfMeasureViewModel
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
            }
        }

        private string abbreviation = string.Empty;
        public string Abbreviation
        {
            get
            { return abbreviation; }

            set
            {
                if (abbreviation == value)
                { return; }

                abbreviation = value;
                isDirty = true;
            }
        }

        private string unitname = string.Empty;
        public string UnitName
        {
            get
            { return unitname; }

            set
            {
                if (unitname == value)
                { return; }

                unitname = value;
                isDirty = true;
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
            }
        }

        #endregion Properties

        public UnitOfMeasureViewModel GetUnitOfMeasure(int uomId)
        {
            var unitOfMeasure = new UnitOfMeasureViewModel();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                try
                {
                    var _uom = (db.Table<UnitOfMeasure>().Where(c => c.Id == uomId)).Single();
                    unitOfMeasure.Id = _uom.Id;
                    unitOfMeasure.UnitName = _uom.UnitName;
                    unitOfMeasure.Abbreviation = _uom.Abbreviation;
                }
                catch (Exception e)
                {
                    string errMessage = e.Message;
                }
                
            }
            return unitOfMeasure;
        }
    }
}
