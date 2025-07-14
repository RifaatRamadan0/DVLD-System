using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___BusinessLayer
{
    public class clsDetainedLicense
    {
        public enum enMode { AddNew = 0, Update = 1};
        public enMode Mode;

        public int DetainID { get; set; }

        public int LicenseID { get; set; }

        public DateTime DetainDate { get; set; }
        public float FineFees { get; set; }

        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo;

        public bool IsReleased { get; set; }
        public DateTime ReleaseDate { get; set; }

        public int ReleasedByUserID { get; set; }
        public clsUser ReleasedUserInfo;

        public int ReleaseApplicationID { get; set; }
        public clsApplication ReleaseApplicationInfo;

        public clsDetainedLicense()
        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.Now;
            this.FineFees = 0;
            this.CreatedByUserID = -1;
            this.IsReleased = false;
            this.ReleaseDate = default;
            this.ReleasedByUserID = -1;
            this.ReleaseApplicationID = -1;

            this.Mode = enMode.AddNew;
        }

        private clsDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate, float FineFees, int CreatedByUserID, 
            bool IsReleased, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.DetainDate = DetainDate;
            this.FineFees = FineFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.FindByUserID(CreatedByUserID);
            this.IsReleased = IsReleased;
            this.ReleaseDate = ReleaseDate;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleasedUserInfo = clsUser.FindByUserID(ReleasedByUserID);
            this.ReleaseApplicationID = ReleaseApplicationID;
            this.ReleaseApplicationInfo = clsApplication.Find(ReleaseApplicationID);

            this.Mode = enMode.Update;
        }

        public static clsDetainedLicense FindByDetainID(int DetainID)
        {
            int LicenseID = -1;
            DateTime DetainDate = DateTime.Now, ReleaseDate = default;
            float FineFees = 0;
            bool IsReleased = false;
            int ReleasedByUserID = -1, ReleaseApplicationID = -1, CreatedByUserID = -1;

            if (clsDetainedLicenseData.GetDetainedLicenseInfoByDetainID(DetainID, ref LicenseID, ref DetainDate, ref FineFees,
                ref CreatedByUserID, ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID))
            {
                return new clsDetainedLicense(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased,
                    ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
            }

            return null;
        }

        public static clsDetainedLicense FindByLicenseID(int LicenseID)
        {
            int DetainID = -1;
            DateTime DetainDate = DateTime.Now, ReleaseDate = default;
            float FineFees = 0;
            bool IsReleased = false;
            int ReleasedByUserID = -1, ReleaseApplicationID = -1, CreatedByUserID = -1;

            if(clsDetainedLicenseData.GetDetainedLicenseInfoByLicenseID(LicenseID, ref DetainID, ref DetainDate, ref FineFees,
                ref CreatedByUserID, ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID))
            {
                return new clsDetainedLicense(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased,
                    ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
            }

            return null;
        }

        public static bool IsDetainedLicense(int LicenseID)
        {
            return clsDetainedLicenseData.IsDetainedLicense(LicenseID);
        }

        private bool _AddNewDetainedLicense()
        {
            this.DetainID = clsDetainedLicenseData.AddNewDetainedLicense(this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID);

            return this.DetainID != -1;
        }

        private bool _UpdateDetainedLicense()
        {
            return clsDetainedLicenseData.UpdateDetainedLicense(this.DetainID, this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID);
        }

        public bool Save()
        {
            switch(this.Mode)
            {
                case enMode.AddNew:
                    if(_AddNewDetainedLicense())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    break;
                case enMode.Update:
                    return _UpdateDetainedLicense();
            }

            return false;
        }

        public bool ReleaseDetainedLicense(int ReleasedByUserID, int ReleaseApplicationID)
        {
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleaseApplicationID = ReleaseApplicationID;
            this.IsReleased = true;
            this.ReleaseDate = DateTime.Now;

            return clsDetainedLicenseData.ReleaseDetainedLicense(this.DetainID, this.ReleasedByUserID, this.ReleaseApplicationID);
        }

        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainedLicenseData.GetAllDetainedLicenses();
        }

    }
}
