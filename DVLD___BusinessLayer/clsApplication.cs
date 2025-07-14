using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DVLD___BusinessLayer
{
    public class clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1};
        public enMode Mode;
        public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3};
        public enum enApplicationType { NewLocalLicense = 1, RenewLicense = 2, ReplaceLostLicense = 3, 
            ReplaceDamagedLicense = 4, ReleaseDetainedLicense = 5, NewInternationalLicense = 6, RetakeTest = 7 };

        public int ApplicationID
        { get; set; }

        public int ApplicantPersonID
        { get; set; }

        public clsPerson ApplicantInfo;

        public DateTime ApplicationDate
        { get; set; }

        public int ApplicationTypeID
        { get; set; }

        public clsApplicationType ApplicationTypeInfo;

        public enApplicationStatus ApplicationStatus
        { get; set; }

        public string StatusText
        {
            get 
            {
                switch(ApplicationStatus)
                {
                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";
                }
               
            }
        }

        public DateTime LastStatusDate
        { get; set; }

        public float PaidFees
        { get; set; }

        public int CreatedByUserID
        { get; set; }

        public clsUser CreatedByUserInfo;


        public clsApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
            enApplicationStatus ApplicationStatus, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicantInfo = clsPerson.Find(ApplicantPersonID);
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeInfo = clsApplicationType.Find(ApplicationTypeID);
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.FindByUserID(CreatedByUserID);
            this.Mode = enMode.Update;
        }

        public clsApplication()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicantInfo = null;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationTypeInfo = null;
            this.ApplicationStatus = enApplicationStatus.New;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            this.CreatedByUserInfo = null;
            this.Mode = enMode.AddNew;
        }

        public static clsApplication Find(int ApplicationID)
        {
            int ApplicantPersonID = -1, ApplicationTypeID = -1, CreatedByUserID = -1;
            DateTime ApplicationDate = DateTime.Now, LastStatusDate = DateTime.Now;
            byte ApplicationStatus = (byte)enApplicationStatus.New;
            float PaidFees = 0;

            if(clsApplicationData.GetApplicationInfoByApplicationID(ApplicationID, ref ApplicantPersonID, ref ApplicationDate,
                ref ApplicationTypeID, ref ApplicationStatus, ref LastStatusDate, ref PaidFees, ref CreatedByUserID))        
            {
                return new clsApplication(ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationTypeID,
                    (enApplicationStatus)ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID);
            }

            return null;
        }

        public bool Cancel()
        {
            return clsApplicationData.UpdateStatus(this.ApplicationID, (byte)clsApplication.enApplicationStatus.Cancelled);
        }
        public bool SetComplete()
        {
            return clsApplicationData.UpdateStatus(this.ApplicationID, (byte)clsApplication.enApplicationStatus.Completed);
        }

        private bool _AddNewApplication()
        {
            this.ApplicationID = clsApplicationData.AddNewApplication(this.ApplicantPersonID, this.ApplicationDate,
                this.ApplicationTypeID, (byte)this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);

            return (this.ApplicationID != -1);
        }

        private bool _UpdateApplication()
        {
            return clsApplicationData.UpdateApplication(this.ApplicationID, this.ApplicantPersonID, this.ApplicationDate,
                this.ApplicationTypeID, (byte)this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
        }

        public bool Save()
        {
            switch(this.Mode)
            {
                case enMode.AddNew:
                    if(_AddNewApplication())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    break;
                case enMode.Update:
                    return _UpdateApplication();
            }

            return false;
        }

        public bool Delete()
        {
            return clsApplicationData.DeleteAppliation(this.ApplicationID);
        }

    }
}
