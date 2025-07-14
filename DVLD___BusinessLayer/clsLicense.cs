using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___BusinessLayer
{
    public class clsLicense
    {
        public enum enMode { AddNew = 0, Update = 1};
        public enMode Mode;

        public enum enIssueReason { FirstTime = 1, Renew = 2, ReplacementForLost = 3, ReplacementForDamaged = 4 };

        public int LicenseID { get; set; }

        public int ApplicationID { get; set; }
        public clsApplication ApplicationInfo;

        public int DriverID { get; set; }
        public clsDriver DriverInfo;

        public int LicenseClassID { get; set; }
        public clsLicenseClass LicenseClassInfo;

        public DateTime IssueDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string Notes { get; set; }

        public float PaidFees { get; set; }

        public bool IsActive { get; set; }

        public enIssueReason IssueReason { get; set; }
        public string IssueReasonText
        {
            get
            {
                return GetIssueReasonText(IssueReason);
            }
        }

        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo;

        public clsDetainedLicense DetainInfo { get; set; }
        public bool IsDetained
        { get
            { return clsDetainedLicense.IsDetainedLicense(this.LicenseID); } 
        }

        public clsLicense()
        {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClassID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = "";
            this.PaidFees = 0;
            this.IsActive = true;
            this.IssueReason = enIssueReason.FirstTime;
            this.CreatedByUserID = -1;

            this.Mode = enMode.AddNew;
        }

        private clsLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate,
            DateTime ExpirationDate, string Notes, float PaidFees, bool IsActive, enIssueReason IssueReason, int CreatedByUserID)
        {
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.ApplicationInfo = clsApplication.Find(ApplicationID);
            this.DriverID = DriverID;
            this.DriverInfo = clsDriver.FindByDriverID(DriverID);
            this.LicenseClassID = LicenseClassID;
            this.LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.FindByUserID(CreatedByUserID);
            this.DetainInfo = clsDetainedLicense.FindByLicenseID(this.LicenseID);

            this.Mode = enMode.Update;
        }

        public static clsLicense Find(int LicenseID)
        {
            int ApplicationID = -1, DriverID = -1, CreatedByUserID = -1, LicenseClassID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = "";
            float PaidFees = 0;
            bool IsActive = false;
            byte IssueReason = (byte)enIssueReason.FirstTime;

            if(clsLicenseData.GetLicenseInfoByLicenseID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClassID, ref IssueDate, 
                ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate,
                    Notes, PaidFees, IsActive, (enIssueReason)IssueReason, CreatedByUserID);
            }

            return null;
        }

        private bool _AddNewLicense()
        {
            this.LicenseID = clsLicenseData.AddNewLicense(this.ApplicationID, this.DriverID, this.LicenseClassID, this.IssueDate,
                this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (byte)this.IssueReason, this.CreatedByUserID);

            return this.LicenseID != -1;
        }

        public bool _UpdateLicense()
        {
            return clsLicenseData.UpdateLicense(this.LicenseID, this.ApplicationID, this.DriverID, this.LicenseClassID, this.IssueDate,
                this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (byte)this.IssueReason, this.CreatedByUserID);
        }

        public bool Save()
        {
            switch(this.Mode)
            {
                case enMode.AddNew:
                    if(_AddNewLicense())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateLicense();
            }

            return false;
        }

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            return clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);
        }

        public static bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID)
        {
            return (GetActiveLicenseIDByPersonID(PersonID, LicenseClassID) != -1);
        }

        public string GetIssueReasonText(enIssueReason IssueReason)
        {
            switch(IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.ReplacementForDamaged:
                    return "Replacement For Damaged";
                case enIssueReason.ReplacementForLost:
                    return "Replacement For Lost";
                default:
                    return "First Time";
            }
        }

        public static DataTable GetLocalLicensesByDriverID(int DriverID)
        {
            return clsLicenseData.GetLocalLicensesByDriverID(DriverID);
        }

        public bool DeactivateCurrentLicense()
        {
            return clsLicenseData.DeactivateOldLicense(this.LicenseID);
        }

        public clsLicense RenewLicense(string Notes, int UserID)
        {
            // Add local license application then local license
            clsApplication NewApplication = new clsApplication();
            NewApplication.ApplicantPersonID = this.DriverInfo.PersonID;
            NewApplication.ApplicationDate = DateTime.Now;
            NewApplication.ApplicationTypeID = (int)clsApplication.enApplicationType.RenewLicense;
            NewApplication.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            NewApplication.LastStatusDate = DateTime.Now;
            NewApplication.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewLicense).Fees;
            NewApplication.CreatedByUserID = this.CreatedByUserID;           

            if(!NewApplication.Save())
                return null;

            clsLicense NewLicense = new clsLicense();
            NewLicense.ApplicationID = NewApplication.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClassID = this.LicenseClassID;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = enIssueReason.Renew;
            NewLicense.CreatedByUserID = UserID;

            if (!NewLicense.Save())
                return null;

            DeactivateCurrentLicense();

            return NewLicense;

        }

        public clsLicense ReplaceLicense(enIssueReason IssueReason, int UserID)
        {
            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = IssueReason == enIssueReason.ReplacementForDamaged ? 
                (int)clsApplication.enApplicationType.ReplaceDamagedLicense : 
                (int)clsApplication.enApplicationType.ReplaceLostLicense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find(Application.ApplicationTypeID).Fees;
            Application.CreatedByUserID = UserID;

            if (!Application.Save())
                return null;

            clsLicense ReplacedLicense = new clsLicense();

            ReplacedLicense.ApplicationID = Application.ApplicationID;
            ReplacedLicense.DriverID = this.DriverID;
            ReplacedLicense.LicenseClassID = this.LicenseClassID;
            ReplacedLicense.IssueDate = DateTime.Now;
            ReplacedLicense.ExpirationDate = this.ExpirationDate;
            ReplacedLicense.Notes = this.Notes;
            ReplacedLicense.PaidFees = 0; // There is no fees for the license in the case of replacement
            ReplacedLicense.IsActive = true;
            ReplacedLicense.IssueReason = IssueReason;
            ReplacedLicense.CreatedByUserID = UserID;

            if (!ReplacedLicense.Save())
                return null;

            DeactivateCurrentLicense();

            return ReplacedLicense;
        }

        public bool IsExpired()
        {
            return this.ExpirationDate < DateTime.Now;
        }


        public clsDetainedLicense Detain(float FineFees, int UserID)
        {
            clsDetainedLicense DetainedLicense = new clsDetainedLicense();

            DetainedLicense.LicenseID = this.LicenseID;
            DetainedLicense.DetainDate = DateTime.Now;
            DetainedLicense.FineFees = FineFees;
            DetainedLicense.CreatedByUserID = UserID;
            DetainedLicense.IsReleased = false;

            if (!DetainedLicense.Save())
                return null;

            return DetainedLicense;
        }

        public bool Release(int UserID, ref int ReleaseApplicationID)
        {
            clsApplication Application = new clsApplication();

            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.ReleaseDetainedLicense;
            Application.CreatedByUserID = UserID;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedLicense).Fees;

            if (!Application.Save())
                return false;

            ReleaseApplicationID = Application.ApplicationID;

            return this.DetainInfo.ReleaseDetainedLicense(UserID, Application.ApplicationID);

        }

    }
}
