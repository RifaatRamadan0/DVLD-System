using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DVLD___DataAccessLayer;

namespace DVLD___BusinessLayer
{
    public class clsPerson
    { 
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int PersonID { get; set; }
        public string NationalID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get {  return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; } 
        }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public short Gender { get; set; }
        public string Email { get; set; }
        public int CountryID { get; set; }
        public string ImagePath { get; set; }
        public clsCountry CountryInfo { get; set; }

        public clsPerson()
        {
            this.PersonID = -1;
            this.NationalID = "";
            this.FirstName = ""; this.SecondName = ""; this.ThirdName = ""; this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.CountryID = -1;
            this.CountryInfo = new clsCountry();
            this.ImagePath = "";

            this.Mode = enMode.AddNew;
        }

        private clsPerson(int PersonID, string NationalID, string FirstName, string SecondName, string ThirdName, string LastName,
            DateTime DateOfBirth, short Gender, string Address, string Phone, string Email, int CountryID, string ImagePath)
        {
            this.PersonID = PersonID;
            this.NationalID = NationalID;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.CountryID = CountryID;
            this.CountryInfo = clsCountry.Find(this.CountryID);
            this.ImagePath = ImagePath;

            Mode = enMode.Update;
        }

        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeopleData();
        }

        public static clsPerson Find(int PersonID)
        {
            string NationalNo = "";
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "";
            DateTime DateOfBirth = DateTime.Now;
            short Gender = 0;
            string Address = "";
            string Phone = "";
            string Email = "";
            int CountryID = -1;
            string ImagePath = "";

            bool IsFound = clsPersonData.GetPersonByID(PersonID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName,
                ref LastName, ref DateOfBirth, ref Gender, ref Address, ref Phone, ref Email,
                ref CountryID, ref ImagePath);

            if (IsFound)
            {
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName,
                 LastName, DateOfBirth, Gender, Address, Phone, Email, CountryID, ImagePath);
            }

            return null;
        }

        public static clsPerson Find(string NationalNo)
        {
            int PersonID = -1;
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "";
            DateTime DateOfBirth = DateTime.Now;
            short Gender = 0;
            string Address = "";
            string Phone = "";
            string Email = "";
            int CountryID = -1;
            string ImagePath = "";

            bool IsFound = clsPersonData.GetPersonByNationalNo(NationalNo, ref PersonID, ref FirstName, ref SecondName, ref ThirdName,
                ref LastName, ref DateOfBirth, ref Gender, ref Address, ref Phone, ref Email,
                ref CountryID, ref ImagePath);

            if (IsFound)
            {
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName,
                 LastName, DateOfBirth, Gender, Address, Phone, Email, CountryID, ImagePath);
            }

            return null;
        }

        public static bool IsPersonExist(string NationalNo)
        {
            return clsPersonData.IsPersonExist(NationalNo);
        }

        public static bool IsPersonExist(int PersonID)
        {
            return clsPersonData.IsPersonExist(PersonID);
        }

        private bool _SaveNewPerson()
        {
            this.PersonID = clsPersonData.AddNewPerson(this.NationalID, this.FirstName, this.SecondName, 
                this.ThirdName, this.LastName, this.DateOfBirth, this.Gender, this.Address, 
                this.Phone, this.Email, this.CountryID, this.ImagePath);

            return (this.PersonID != -1);
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(this.PersonID, this.NationalID, this.FirstName,
                this.SecondName, this.ThirdName, this.LastName, this.DateOfBirth, this.Gender,
                this.Address, this.Phone, this.Email, this.CountryID, this.ImagePath);
        }

        public bool Save()
        {
            switch(this.Mode)
            {
                case enMode.AddNew:
                    if(_SaveNewPerson())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }

                    return false;

                case enMode.Update:
                    return _UpdatePerson();
            }

            return false;
        }

        public static bool DeletePerson(int PersonID)
        {
            return clsPersonData.DeletePerson(PersonID);
        }


    }
}
