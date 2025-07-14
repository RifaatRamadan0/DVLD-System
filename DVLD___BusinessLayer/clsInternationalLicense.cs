using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD___BusinessLayer
{
    public class clsInternationalLicense : clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1};
        public enMode Mode;

        public int InternationalLicenseID
        { get; set; }

        public int DriverID
        { get; set; }
        public clsDriver DriverInfo;

        public int IssuedUsingLocalLicenseID
        { get; set; }
        public clsLicense IssuedUsingLocalLicenseInfo;

        public DateTime IssueDate
        { get; set; }
        
        public DateTime ExpirationDate
        { get; set; }

        public bool IsActive
        { get; set; }

        public clsInternationalLicense() : base()
        {
            this.InternationalLicenseID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.IsActive = true;

            this.Mode = enMode.AddNew;
        }

        private clsInternationalLicense(int InternationalLicenseID, int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, 
            DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
            enApplicationStatus ApplicationStatus, DateTime LastStatusDate, float PaidFees, int CreatedByUserID) :
            base (ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID)
        {
            this.InternationalLicenseID = InternationalLicenseID;
            this.DriverID = DriverID;
            this.DriverInfo = clsDriver.FindByDriverID(DriverID);
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.IssuedUsingLocalLicenseInfo = clsLicense.Find(IssuedUsingLocalLicenseID);
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;

            this.Mode = enMode.Update;
        }

        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            int ApplicationID = -1, DriverID = -1, IssuedUsingLocalLicenseID = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            bool IsActive = true;

            if(clsInternationalLicenseData.GetInternationalLicenseInfoByID(InternationalLicenseID, ref ApplicationID, ref DriverID, 
                ref IssuedUsingLocalLicenseID, ref CreatedByUserID, ref IssueDate, ref ExpirationDate, ref IsActive))
            {
                clsApplication Application = clsApplication.Find(ApplicationID);

                return new clsInternationalLicense(InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID,
                    IssueDate, ExpirationDate, IsActive, Application.ApplicantPersonID, Application.ApplicationDate, Application.ApplicationTypeID, 
                    Application.ApplicationStatus, Application.LastStatusDate, Application.PaidFees, CreatedByUserID);
            }

            return null;

        }

        public static DataTable GetInternationalLicensesByDriverID(int DriverID)
        {
            return clsInternationalLicenseData.GetInternationalLicensesByDriverID(DriverID);
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            return clsInternationalLicenseData.GetActiveInternationalLicenseIDByDriverID(DriverID);
        }

        private bool _AddNewInternationalLicense()
        {
            this.InternationalLicenseID = clsInternationalLicenseData.AddNewInternationalLicense(this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID, 
                this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);

            return this.InternationalLicenseID != -1;
        }

        private bool _UpdateInternationalLicense()
        {
            return true;
        }

        public new bool Save()
        {
            if (!base.Save())
            {
                return false;
            }

            switch (this.Mode)
            {
                case enMode.AddNew:
                    if(_AddNewInternationalLicense())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateInternationalLicense();
            }

            return false;

        }

        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses();
        }

    }
}
