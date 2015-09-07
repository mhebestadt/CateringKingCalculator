using SQLite;

namespace hebestadt.CateringKingCalculator.Models
{
    class Ingredient
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public float Weight { get; set; }
        public int UnitOfMeasure { get; set; }
        public string Category { get; set; }
        public int IngredientMealWeight { get; set; }
    }
}
