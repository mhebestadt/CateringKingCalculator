using hebestadt.CateringKingCalculator.Models;
using hebestadt.CateringKingCalculator.ViewModels;
using hebestadtaCateringKingCalculator;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringKingCalculator.ViewModels
{
    public class ContactViewModel : ViewModelBase
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
                RaisePropertyChanged("Id");
            }
        }

        private string attention = string.Empty;
        public string Attention
        {
            get
            {
                return attention;
            }

            set
            {
                if (attention == value)
                { return; }

                attention = value;
                isDirty = true;
                RaisePropertyChanged("Attention");
            }
        }

        private string firstname = string.Empty;
        public string FirstName
        {
            get
            {
                return firstname;
            }

            set
            {
                if (firstname == value)
                { return; }

                firstname = value;
                isDirty = true;
                RaisePropertyChanged("FirstName");
            }
        }

        private string lastname = string.Empty;
        public string LastName
        {
            get
            {
                return lastname;
            }

            set
            {
                if (lastname == value)
                { return; }

                lastname = value;
                isDirty = true;
                RaisePropertyChanged("LastName");
            }
        }

        private string nameandaddress = string.Empty;
        public string NameAndAddress
        {
            get
            {
                return nameandaddress;
            }

            set
            {
                if (nameandaddress == value)
                { return; }

                nameandaddress = value;
                isDirty = true;
                RaisePropertyChanged("NameAndAddress");
            }
        }

        private string company = string.Empty;
        public string Company
        {
            get
            {
                return company;
            }
            
            set
            {
                if (company == value)
                { return; }

                company = value;
                isDirty = true;
                RaisePropertyChanged("Company");
            }
        }

        private string street = string.Empty;
        public string Street
        {
            get
            {
                return street;
            }

            set
            {
                if (street == value)
                { return; }

                street = value;
                isDirty = true;
                RaisePropertyChanged("Street");
            }
        }

        private string zip = string.Empty;
        public string Zip
        {
            get
            {
                return zip;
            }

            set
            {
                if (zip == value)
                { return; }

                zip = value;
                isDirty = true;
                RaisePropertyChanged("Zip");
            }
        }

        private string city = string.Empty;
        public string City
        {
            get
            {
                return city;
            }

            set
            {
                if (city == value)
                { return; }

                city = value;
                isDirty = true;
                RaisePropertyChanged("City");
            }
        }

        private string phonenr = string.Empty;
        public string PhoneNr
        {
            get
            {
                return phonenr;
            }

            set
            {
                if (phonenr == value)
                { return; }

                phonenr = value;
                isDirty = true;
                RaisePropertyChanged("PhoneNr");
            }
        }

        private string cellphonenr = string.Empty;
        public string CellPhoneNr
        {
            get
            {
                return cellphonenr;
            }

            set
            {
                if (cellphonenr == value)
                { return; }
                cellphonenr = value;
                isDirty = true;
                RaisePropertyChanged("CellPhoneNr");
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
                RaisePropertyChanged("IsDirty");
            }
        }

        #endregion "Properties"

        //public ObservableCollection<ContactViewModel> GetMatchingContacts(string query)
        public ObservableCollection<string> GetMatchingContacts(string query)
        {
            //var contacts = new ObservableCollection<ContactViewModel>();
            var contacts = new ObservableCollection<string>();

            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var conts = db.Table<Contact>()
                    .Where(c => c.FirstName.Contains(query) ||
                                c.LastName.Contains(query) ||
                                c.Company.Contains(query));

                if (conts.Count() > 0)
                {
                    foreach (var cont in conts)
                    {
                        contacts.Add(cont.LastName);
                    }
                }

                /*db.Table<Contact>()
                    .Where(c => c.FirstName.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1 ||
                                c.LastName.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1 ||
                                c.Company.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1)
                                .OrderByDescending(c => c.FirstName.StartsWith(query, StringComparison.CurrentCultureIgnoreCase))
                                .ThenByDescending(c => c.LastName.StartsWith(query, StringComparison.CurrentCultureIgnoreCase))
                                .ThenByDescending(c => c.Company.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));*/
            }

            return contacts;
        }

        public ContactViewModel GetContact(int contactID)
        {
            var contact = new ContactViewModel();

            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var _contact = (db.Table<Contact>().Where( c => c.Id == contactID)).Single();
                contact.Attention = _contact.Attention;
                contact.FirstName = _contact.FirstName;
                contact.LastName = _contact.LastName;
                contact.NameAndAddress = _contact.NameAndAddress;
                contact.Company = _contact.Company;
                contact.Street = _contact.Street;
                contact.Zip = _contact.Zip;
                contact.City = _contact.Zip;
                contact.PhoneNr = _contact.PhoneNr;
                contact.CellPhoneNr = _contact.CellPhoneNr;
            }

            return contact;
        }

        public ContactViewModel GetContact(string contactInfo)
        {
            var contact = new ContactViewModel();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var _contact = (db.Table<Contact>().Where(c => c.LastName.Contains(contactInfo))).Single();
                contact.Attention = _contact.Attention;
                contact.FirstName = _contact.FirstName;
                contact.LastName = _contact.LastName;
                contact.NameAndAddress = _contact.NameAndAddress;
                contact.Company = _contact.Company;
                contact.Street = _contact.Street;
                contact.Zip = _contact.Zip;
                contact.City = _contact.Zip;
                contact.PhoneNr = _contact.PhoneNr;
                contact.CellPhoneNr = _contact.CellPhoneNr;
            }

            return contact;
        }

        public string SaveContact(ContactViewModel contact)
        {
            string result = string.Empty;
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                string change = string.Empty;
                try
                {
                    var existingContact = (db.Table<Contact>().Where(
                        c => c.Id == contact.Id)).SingleOrDefault();

                    if (existingContact != null)
                    {
                        existingContact.Attention = contact.Attention;
                        existingContact.FirstName = contact.FirstName;
                        existingContact.LastName = contact.LastName;
                        existingContact.NameAndAddress = contact.NameAndAddress;
                        existingContact.Company = contact.Company;
                        existingContact.Street = contact.Street;
                        existingContact.Zip = contact.Zip;
                        existingContact.City = contact.City;
                        existingContact.PhoneNr = contact.PhoneNr;
                        existingContact.CellPhoneNr = contact.CellPhoneNr;

                        int success = db.Update(existingContact);
                    }
                    else
                    {
                        int success = db.Insert(new Contact()
                        {
                            Attention = contact.Attention,
                            FirstName = contact.FirstName,
                            LastName = contact.LastName,
                            NameAndAddress = contact.NameAndAddress,
                            Company = contact.Company,
                            Street = contact.Street,
                            Zip = contact.Zip,
                            City = contact.City,
                            PhoneNr = contact.PhoneNr,
                            CellPhoneNr = contact.CellPhoneNr
                        });
                        
                        SQLiteCommand cmd = db.CreateCommand("SELECT last_insert_rowid()");                       
                        int rowId = cmd.ExecuteScalar<int>();
                        cmd.CommandText = "SELECT Id FROM Contact WHERE rowid = " + rowId.ToString();
                        contact.Id = cmd.ExecuteScalar<int>();
                    }
                    result = "Success";
                }
                catch
                {
                    result = "This contact was not saved.";
                }
            }

            return result;
        }
    }
}
