using SQLite;

namespace hebestadt.CateringKingCalculator.Models
{
    class UnitOfMeasure
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Abbreviation { get; set; }
        public string UnitName { get; set; }
    }
}
