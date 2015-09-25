using System;
using System.Collections.Generic;

namespace hebestadt.CateringKingCalculator.ViewModels
{
    public interface IMealViewModel
    {
        string Contact { get; set; }
        int ContactId { get; set; }
        DateTimeOffset DeliveryDate { get; set; }
        string DeliveryLocation { get; set; }
        string DeliveryNoteId { get; set; }
        TimeSpan DeliveryTime { get; set; }
        int Id { get; set; }
        bool IsDirty { get; set; }
        List<int> MealItemIDs { get; set; }
        Dictionary<float, float> MealItemIDsWithWeight { get; set; }
        string Name { get; set; }
        int NumberOfGuests { get; set; }
        DateTimeOffset PickupDate { get; set; }
        bool SilverWare { get; set; }

        string AddMealItem(IMealViewModel meal, int mealItemID, float mealItemWeight);
        string DeleteMeal(int mealId);
        IMealViewModel GetMeal(int mealId);
        string GetTextRepresentation(IMealViewModel meal);
        string RemoveMealItem(IMealViewModel meal, int mealItemID);
        string SaveMeal(IMealViewModel meal);
    }
}