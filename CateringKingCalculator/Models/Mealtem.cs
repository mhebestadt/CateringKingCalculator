using SQLite;

namespace hebestadt.CateringKingCalculator.Models
{
    class MealItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public float TotalAmount { get; set; }
        public int TotalAmountUnitOfMeasure { get; set; }
        public byte[] IngredientIDsWithTotalAmount { get; set; }
    }
}
