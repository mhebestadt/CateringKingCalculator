using hebestadt.CateringKingCalculator.Models;
using hebestadt.CateringKingCalculator.ViewModels;
using hebestadtaCateringKingCalculator;
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

        public ObservableCollection<ContactViewModel> GetMatchingContacts(string query)
        {
            var contacts = new ObservableCollection<ContactViewModel>();
            ContactViewModel cont1 = new ContactViewModel();
            cont1.LastName = "Last Name 1";
            contacts.Add(cont1);

            ContactViewModel cont2 = new ContactViewModel();
            cont2.LastName = "Last Name 1";
            contacts.Add(cont2);

            /*using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var conts = db.Table<Contact>()
                    .Where(c => c.FirstName.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1 ||
                                c.LastName.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1 ||
                                c.Company.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1);

                foreach(var cont in conts )
                {
                    int c = 11;
                    string lastname = cont.LastName;
                }

                db.Table<Contact>()
                    .Where(c => c.FirstName.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1 ||
                                c.LastName.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1 ||
                                c.Company.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1)
                                .OrderByDescending(c => c.FirstName.StartsWith(query, StringComparison.CurrentCultureIgnoreCase))
                                .ThenByDescending(c => c.LastName.StartsWith(query, StringComparison.CurrentCultureIgnoreCase))
                                .ThenByDescending(c => c.Company.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));
            }*/

            return contacts;
        }

        public ContactViewModel GetContact(string contactInfo)
        {
            var contact = new ContactViewModel();
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                var _contact = (db.Table<Contact>().Where(c => c.LastName.Contains(contactInfo))).Single();
                contact.FirstName = _contact.FirstName;
                contact.LastName = _contact.LastName;
                contact.Company = _contact.Company;
                contact.Street = _contact.Street;
                contact.Zip = _contact.Zip;
                contact.City = _contact.Zip;
                contact.PhoneNr = _contact.PhoneNr;
            }

            return contact;
        }
    }
}
