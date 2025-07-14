using DVLD___DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLD___BusinessLayer
{
    public class clsTest
    {
        public enum enMode { AddNew = 0, Update = 1};
        public enMode Mode = enMode.AddNew;

        public int TestID { get; set; }

        public int TestAppointmentID { get; set; }
        public clsTestAppointment TestAppointmentInfo;

        public bool TestResult { get; set; }

        public string Notes { get; set; }

        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo;

        public clsTest()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestAppointmentInfo = null;
            this.TestResult = false;
            this.Notes = "";
            this.CreatedByUserID = -1;
            this.CreatedByUserInfo = null;

            this.Mode = enMode.AddNew;
        }

        private clsTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.TestAppointmentInfo = clsTestAppointment.Find(TestAppointmentID);
            this.TestResult = TestResult;
            this.Notes = Notes;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.FindByUserID(this.CreatedByUserID);

            this.Mode = enMode.Update;
        }


        public static clsTest Find(int TestID)
        {
            int TestAppointmentID = -1, CreatedByUserID = -1;
            bool TestResult = false;
            string Notes = "";

            if(clsTestData.GetTestInfoByTestID(TestID, ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUserID))
            {
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            }

            return null;
        }
        public static clsTest FindLastTest(int LocalLicenseApplicationID, int TestTypeID)
        {
            int TestID = -1, TestAppointmentID = -1;
            bool TestResult = false;
            string Notes = "";
            int CreatedByUserID = -1;

            if(clsTestData.GetLastTestInfoBy(LocalLicenseApplicationID, TestTypeID, ref TestID, ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUserID))
            {
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            }

            return null;

        }

        public static int GetPassedTestsCount(int LocalLicenseApplicationID)
        {
            return clsTestData.GetPassedTestsCount(LocalLicenseApplicationID);
        }

        public static bool HasPassedAllTests(int LocalLicenseApplicationID)
        {
            return GetPassedTestsCount(LocalLicenseApplicationID) == 3;
        }

        public bool _AddNewTest()
        {
            this.TestID = clsTestData.AddNewTest(this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);

            return this.TestID != -1;
        }

        public bool _UpdateTest()
        {
            return clsTestData.UpdateTest(this.TestID, this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
        }

        public bool Save()
        {
            switch(this.Mode)
            {
                case enMode.AddNew:
                    if(_AddNewTest())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    break;
                case enMode.Update:
                    return _UpdateTest();
            }

            return false;
        }

    }
}
