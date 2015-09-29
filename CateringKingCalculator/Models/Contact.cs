using SQLite;

namespace hebestadt.CateringKingCalculator.Models
{
    class Contact
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Attention { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NameAndAddress { get; set; }
        public string Company { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string PhoneNr { get; set; }
        public string CellPhoneNr { get; set; }
    }
}
