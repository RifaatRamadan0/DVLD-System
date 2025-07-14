using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___BusinessLayer
{
    public class clsApplicationType
    {
        public enum enMode { AddNew = 0, Update};
        public enMode Mode = enMode.AddNew;

        public int ID
        { get; set; }

        public string Title
        { get; set; }

        public float Fees
        { get; set; }

        public clsApplicationType()
        {
            this.ID = -1;
            this.Title = "";
            this.Fees = 0;
            this.Mode = enMode.AddNew;
        }

        private clsApplicationType(int ID, string Title, float Fees)
        {
            this.ID = ID;
            this.Title = Title;
            this.Fees = Fees;
            this.Mode = enMode.Update;
        }

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypeData.GetAllApplicationTypes();
        }

        public static clsApplicationType Find(int ID)
        {
            string Title = "";
            float Fees = 0;

            if(clsApplicationTypeData.GetApplicationTypeInfoByID(ID, ref Title, ref Fees))
            {
                return new clsApplicationType(ID, Title, Fees);
            }

            return null;
        }

        private bool _AddApplicationType()
        {
            this.ID = clsApplicationTypeData.AddNewApplicationType(this.Title, this.Fees);

            return (this.ID != -1);
        }

        private bool _UpdateApplicationType()
        {
            return clsApplicationTypeData.UpdateApplicationTypeInfo(this.ID, this.Title, this.Fees);
        }

        public bool Save()
        {
            switch(this.Mode)
            {
                case enMode.AddNew:
                    if(_AddApplicationType())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    break;
                case enMode.Update:
                    return _UpdateApplicationType();
            }

            return false;
        }

    }
}
