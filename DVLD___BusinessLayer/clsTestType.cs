using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___BusinessLayer
{
    public class clsTestType
    {
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3};


        public enTestType ID
        { get; set; }

        public string Title
        { get; set; }

        public string Description
        { get; set;}

        public float Fees
        { get; set; }

        public clsTestType()
        {
            this.ID = enTestType.VisionTest;
            this.Title = "";
            this.Description = "";
            this.Fees = 0;
        }

        private clsTestType(enTestType ID,string Title, string Description, float Fees)
        {
            this.ID = ID;
            this.Title = Title;
            this.Description = Description;
            this.Fees = Fees;
        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypeData.GetAllTestTypes();
        }

        public static clsTestType Find(enTestType ID)
        {
            string Title = "", Description = "";
            float Fees = 0;

            if(clsTestTypeData.GetTestTypeInfoByID((int)ID, ref Title, ref Description, ref Fees))
            {
                return new clsTestType((enTestType)ID, Title, Description, Fees);
            }

            return null;
        }

        private bool _UpdateTestType()
        {
            return clsTestTypeData.UpdateTestType((int)this.ID, this.Title, this.Description, this.Fees);
        }

        public bool Save()
        {
            return _UpdateTestType();
        }

    }
}
