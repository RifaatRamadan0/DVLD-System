using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___BusinessLayer
{
    public class clsLocalLicenseApplication : clsApplication
    {
        public new enum enMode { AddNew = 0, Update = 1};
        public new enMode Mode;

        public int LocalLicenseApplicationID
        { get; set; }

        public int LicenseClassID
        { get; set; }

        public clsLicenseClass LicenseClassInfo;
        
        public string PersonFullName
        {
            get { return base.ApplicantInfo.FullName; }
        }
        
        private clsLocalLicenseApplication(int LocalLicenseApplicationID, int ApplicationID, int ApplicantPersonID, 
            DateTime ApplicationDate, int ApplicationTypeID, enApplicationStatus ApplicationStatus, DateTime LastStatusDate, 
            float PaidFees, int CreatedByUserID, int LicenseClassID) : 
            base(ApplicationID, ApplicantPersonID, ApplicationDate,
                ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID)
        {
            this.LocalLicenseApplicationID = LocalLicenseApplicationID;
            this.LicenseClassID = LicenseClassID;
            this.LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);
            this.Mode = enMode.Update;
        }

        public clsLocalLicenseApplication() : base()
        {
            this.LocalLicenseApplicationID = -1;
            this.LicenseClassID = -1;
            this.LicenseClassInfo = null;
            this.Mode = enMode.AddNew;
        }

        public static DataTable GetAllLocalLicenseApplications()
        {
            return clsLocalLicenseApplicationData.GetAllLocalLicenseApplications();
        }
        public static new clsLocalLicenseApplication Find(int LocalLicenseApplicationID)
        
        {
            int ApplicationID = -1, LicenseClassID = -1;


            if(clsLocalLicenseApplicationData.GetLocalLicenseApplicationInfoByLocalLicenseID(LocalLicenseApplicationID, 
                ref ApplicationID, ref LicenseClassID))
            {
                clsApplication Application = clsApplication.Find(ApplicationID);

                return new clsLocalLicenseApplication(LocalLicenseApplicationID, ApplicationID, Application.ApplicantPersonID,
                     Application.ApplicationDate, Application.ApplicationTypeID, Application.ApplicationStatus, 
                     Application.LastStatusDate, Application.PaidFees, Application.CreatedByUserID, LicenseClassID);

            }

            return null;

        }

        private bool _AddNewLocalLicenseApplication()
        {
            this.LocalLicenseApplicationID = clsLocalLicenseApplicationData.AddNewLocalLicenseApplication(this.ApplicationID, this.LicenseClassID);

            return (this.LocalLicenseApplicationID != -1);
        }

        private bool _UpdateLocalLicenseApplication()
        {
            return clsLocalLicenseApplicationData.UpdateLocalLicenseApplication(this.LocalLicenseApplicationID,
                this.ApplicationID, this.LicenseClassID);
        }

        public new bool Save()
        {
            base.Mode = (clsApplication.enMode)this.Mode;
            if (!base.Save())
                return false;

            switch (this.Mode)
            {
                case enMode.AddNew:
                    if(_AddNewLocalLicenseApplication())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    break;
                case enMode.Update:
                    return _UpdateLocalLicenseApplication();
                    
            }

            return false;

        }

        public static int GetActiveApplicationIDForLicenseClass(int ApplicantPersonID,clsApplication.enApplicationType ApplicationType, int LicenseClassID)
        {
            return clsLocalLicenseApplicationData.GetActiveApplicationIDForLicenseClass(ApplicantPersonID, (int)ApplicationType, LicenseClassID);
        }

        public bool IsLicenseIssued()
        {
            return clsLicense.GetActiveLicenseIDByPersonID(this.ApplicantPersonID, this.LicenseClassID) != - 1;
        }

        public bool DoesPassTestType(clsTestType.enTestType TestType)
        {
            return clsLocalLicenseApplicationData.DoesPassTestType(this.LocalLicenseApplicationID, (int)TestType);
        }

        public bool HasActiveTestAppointment(clsTestType.enTestType TestType)
        {
            return clsLocalLicenseApplicationData.HasActiveTestAppointment(this.LocalLicenseApplicationID, (int)TestType);
        }

        public clsTest GetLastTest(clsTestType.enTestType TestType)
        {
            return clsTest.FindLastTest(this.LocalLicenseApplicationID, (int)TestType);
        }

        public int GetNumberOfTrials(int TestTypeID)
        {
            return clsLocalLicenseApplicationData.GetNumberOfTrials(this.LocalLicenseApplicationID, TestTypeID);
        }

        public int GetPassedTestCount()
        {
            return clsTest.GetPassedTestsCount(this.LocalLicenseApplicationID);
        }

        public int GetActiveLicenseID()
        {
            return clsLicense.GetActiveLicenseIDByPersonID(this.ApplicantPersonID, this.LicenseClassID);
        }

        public bool HasPassedAllTests()
        {
            return clsTest.HasPassedAllTests(this.LocalLicenseApplicationID);
        }

        public int IssueLicenseForTheFirstTime(string Notes, int UserID)
        {
            int DriverID = -1;
            clsDriver Driver = clsDriver.FindByPersonID(this.ApplicantPersonID);
            if (Driver == null)
            {
                Driver = new clsDriver();
                Driver.PersonID = this.ApplicantPersonID;
                Driver.CreatedByUserID = this.CreatedByUserID;
                Driver.CreatedDate = DateTime.Now;
                if (Driver.Save())
                {
                   DriverID = Driver.DriverID;
                }
                else
                {
                    DriverID = -1;
                }
            }
            else
            {
                DriverID = Driver.DriverID;
            }

            int LicenseID = -1;
            clsLicense License = new clsLicense();
            License.ApplicationID = this.ApplicationID;
            License.DriverID = DriverID;
            License.LicenseClassID = this.LicenseClassID;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            License.Notes = Notes;
            License.PaidFees = this.LicenseClassInfo.ClassFees;
            License.IsActive = true;
            License.IssueReason = clsLicense.enIssueReason.FirstTime;
            License.CreatedByUserID = UserID;

            if( License.Save())
            {
                this.SetComplete();
                LicenseID = License.LicenseID;
            }

            return LicenseID;
        }

        public new bool Delete()
        {

            if (!clsLocalLicenseApplicationData.DeleteLocalLicenseApplication(this.LocalLicenseApplicationID))
                return false;

            return base.Delete();
            
        }

    }
}
