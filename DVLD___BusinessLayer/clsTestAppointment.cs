using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___BusinessLayer
{
    public class clsTestAppointment
    {
        public enum enMode { AddNew = 0, Update = 1};
        public enMode Mode;

        public int TestAppointmentID { get; set; }

        public int TestTypeID { get; set; }
        public clsTestType TestTypeInfo { get; set; }

        public int LocalLicenseApplicationID { get; set; }
        public clsLocalLicenseApplication LocalLicenseApplicationInfo;

        public DateTime AppointmentDate { get; set; }

        public float PaidFees { get; set; }

        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo;

        public bool IsLocked { get; set; }

        public int RetakeTestApplicationID { get; set; }
        public clsApplication RetakeTestApplicationInfo;

        public int TestID
        {
            get
            {
                return _GetTestID();
            }
        }
            


        public clsTestAppointment()
        {
            this.TestAppointmentID = -1;
            this.TestTypeID = -1;
            this.TestTypeInfo = null;
            this.LocalLicenseApplicationID = -1;
            this.LocalLicenseApplicationInfo = null;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            this.CreatedByUserInfo = null;
            this.IsLocked = false;
            this.RetakeTestApplicationID = -1;
            this.RetakeTestApplicationInfo = null;
            this.Mode = enMode.AddNew;
        }

        private clsTestAppointment(int TestAppointmentID, int TestTypeID, int LocalLicenseApplicationID, DateTime AppointmentDate,
            float PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID )
        {
            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.TestTypeInfo = clsTestType.Find((clsTestType.enTestType)this.TestTypeID);
            this.LocalLicenseApplicationID = LocalLicenseApplicationID;
            this.LocalLicenseApplicationInfo = clsLocalLicenseApplication.Find(LocalLicenseApplicationID);
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.FindByUserID(CreatedByUserID);
            this.IsLocked = IsLocked;
            this.RetakeTestApplicationID = RetakeTestApplicationID;
            this.RetakeTestApplicationInfo = clsApplication.Find(RetakeTestApplicationID);
            this.Mode = enMode.Update;
        }

        public static clsTestAppointment Find(int TestAppointmentID)
        {
            int TestTypeID = -1, LocalLicenseApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now;
            float PaidFees = 0;
            int CreatedByUserID = -1;
            bool IsLocked = false;
            int RetakeTestApplicationID = -1;

            if (clsTestAppointmentData.GetTestAppointmentInfoByID(TestAppointmentID, ref TestTypeID, ref LocalLicenseApplicationID, 
                ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID)) 
            {
                return new clsTestAppointment(TestAppointmentID, TestTypeID, LocalLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
            }

            return null;
        }

        public static DataTable GetAllTestAppointmentsOf(int LocalLicenseApplicationID, clsTestType.enTestType TestType)
        {
            return clsTestAppointmentData.GetAllTestAppointmentsOf(LocalLicenseApplicationID, (int)TestType);
        }

        private bool _AddNewTestAppointment()
        {          
            this.TestAppointmentID = (clsTestAppointmentData.AddNewTestAppointment(this.TestTypeID, this.LocalLicenseApplicationID, 
                this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked, this.RetakeTestApplicationID));

            return this.TestAppointmentID != -1;

        }

        private bool _UpdateTestAppointment()
        {
            return clsTestAppointmentData.UpdateTestAppointment(this.TestAppointmentID, this.TestTypeID, this.LocalLicenseApplicationID,
                this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked, this.RetakeTestApplicationID);
        }

        public bool Save()
        {

            switch(this.Mode)
            {
                case enMode.AddNew:
                    if(_AddNewTestAppointment())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateTestAppointment();
            }

            return false;
        }

        private int _GetTestID()
        {
            return clsTestAppointmentData.GetTestID(this.TestAppointmentID);
        }

    }
}
