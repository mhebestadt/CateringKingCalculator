using SQLite;
using System;
using System.Collections.Generic;

namespace hebestadt.CateringKingCalculator
{
    class Meal
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string DeliveryNoteId { get; set; }
        public DateTimeOffset DeliveryDate { get; set; }
        public TimeSpan DeliveryTime { get; set; }
        public string DeliveryLocation { get; set; }
        public DateTimeOffset PickupDate { get; set; }
        public string Contact { get; set; }
        public int ContactId { get; set; }
        public int NumberOfGuests { get; set; }
        public bool SilverWare { get; set; }
        public byte[] MealItemIDs { get; set; }
        public byte[] MealItemIDsWithWeight { get; set; }
    }
}
