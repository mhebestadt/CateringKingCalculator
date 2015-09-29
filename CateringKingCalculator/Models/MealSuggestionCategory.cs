using SQLite;



namespace hebestadt.CateringKingCalculator.Models
{
    class MealSuggestionCategory
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
