using SQLite;

namespace hebestadt.CateringKingCalculator.Models
{
    class MealSuggestion
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public byte[] MealItemIDsWithWeight { get; set; }
    }
}
