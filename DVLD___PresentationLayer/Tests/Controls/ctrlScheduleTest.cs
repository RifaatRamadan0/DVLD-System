using DVLD___BusinessLayer;
using DVLDWinForms___Presentation_Layer.Global_Classes;
using DVLDWinForms___Presentation_Layer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD___BusinessLayer.clsTestType;
using static System.Net.Mime.MediaTypeNames;

namespace DVLDWinForms___Presentation_Layer.Tests
{
    public partial class ctrlScheduleTest : UserControl
    {
        public enum enMode { AddNew = 0, Update };
        public enMode _Mode;
        public enum enCreationMode { LicenseFirstTimeSchedule = 0, RetakeTestSchedule = 1 };
        public enCreationMode CreationMode = enCreationMode.LicenseFirstTimeSchedule;

        public event EventHandler OnSave;
        private clsLocalLicenseApplication _LocalLicenseApplication;
        private int _LocalLicenseApplicationID = -1;
        private clsTestAppointment _TestAppointment;
        private int _TestAppointmentID = -1;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;

        public clsTestType.enTestType TestTypeID
        {
            get
            {
                return _TestTypeID;
            }
            set
            {
                _TestTypeID = value;

                switch (_TestTypeID)
                {

                    case clsTestType.enTestType.VisionTest:
                        pbTestType.ImageLocation = @"C:\Users\rifaa\Desktop\Programming Princples Courses\19th Principle Course\DVLD System\DVLDWinForms - Presentation Layer\Icons\Vision 512.png";
                        gbTestInfo.Text = "Vision Test";
                        break;
                    case clsTestType.enTestType.WrittenTest:
                        pbTestType.ImageLocation = @"C:\Users\rifaa\Desktop\Programming Princples Courses\19th Principle Course\DVLD System\DVLDWinForms - Presentation Layer\Icons\Written Test 512.png";
                        gbTestInfo.Text = "Written Test";
                        break;
                    case clsTestType.enTestType.StreetTest:
                        pbTestType.ImageLocation = @"C:\Users\rifaa\Desktop\Programming Princples Courses\19th Principle Course\DVLD System\DVLDWinForms - Presentation Layer\Icons\driving-test 512.png";
                        gbTestInfo.Text = "Street Test";
                        break;
                }

            }
        }

        public ctrlScheduleTest()
        {
            InitializeComponent();
        }

        private void _ResetDefaultValues()
        {
            lblLocalLicenseApplicationID.Text = "??";
            lblLicenseClass.Text = "??";
            lblApplicantName.Text = "??";
            lblNumOfTrials.Text = "??";
            DateOfAppointment.Value = DateTime.Now;
            lblTestApplicationFees.Text = "??";

            lblRetakeTestApplicationFees.Text = "??";
            lblRetakeTestApplicationID.Text = "N/A";
            lblTotalTestApplicationFees.Text = "??";

        }
        private bool _HandleAppointmentLockedConstraint()
        {
            if (_TestAppointment.IsLocked)
            {
                lblUserMessage.Visible = true;
                btnSave.Enabled = false;
                DateOfAppointment.Enabled = false;
                return false;
            }

            return true;
        }
        private bool _HandlePassedPreviousTestConstraint()
        {
            switch (_TestTypeID)
            {
                case enTestType.VisionTest:
                    // No previous test to handle
                    return true;
                case enTestType.WrittenTest:

                    if (!_LocalLicenseApplication.DoesPassTestType(enTestType.VisionTest))
                    {
                        lblUserMessage.Text = "Person should pass vision test before this test";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        DateOfAppointment.Enabled = false;
                        return false;
                    }

                    break;
                case enTestType.StreetTest:

                    if (!_LocalLicenseApplication.DoesPassTestType(enTestType.WrittenTest))
                    {
                        lblUserMessage.Text = "Person should pass Written test before this test";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        DateOfAppointment.Enabled = false;
                        return false;
                    }

                    break;
            }

            return true;
        }
        private bool _HandleActiveTestAppointmentConstraint()
        {
            if (_Mode == enMode.AddNew && _LocalLicenseApplication.HasActiveTestAppointment(TestTypeID))
            {
                MessageBox.Show("Person already has an active test appointment, you can not add new appointment", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                DateOfAppointment.Enabled = false;
                return false;
            }

            return true;
        }
        private bool _LoadTestAppointment()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);

            if (_TestAppointment == null)
            {
                MessageBox.Show("Test Appointment Application with ID = [" + _TestAppointmentID + "] Does not exist", "Not Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }

            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate) < 0)
                DateOfAppointment.MinDate = DateTime.Now;
            else
                DateOfAppointment.MinDate = _TestAppointment.AppointmentDate;

            DateOfAppointment.Value = _TestAppointment.AppointmentDate;
            lblTestApplicationFees.Text = _TestAppointment.PaidFees.ToString();


            if (_TestAppointment.RetakeTestApplicationID == -1)
            {
                lblRetakeTestApplicationID.Text = "N/A";
                lblRetakeTestApplicationFees.Text = "0";
            }
            else
            {
                lblRetakeTestApplicationID.Text = _TestAppointment.RetakeTestApplicationID.ToString();
                lblRetakeTestApplicationFees.Text = _TestAppointment.RetakeTestApplicationInfo.PaidFees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
            }

            return true;
        }
        public void LoadScheduleTestInfo(int LocalLicenseApplicationID, int TestAppointmentID = -1)
        {
            _ResetDefaultValues();

            if (TestAppointmentID == -1)
            {
                _Mode = enMode.AddNew;
            }
            else
            {
                _Mode = enMode.Update;
            }

            _LocalLicenseApplicationID = LocalLicenseApplicationID;
            _LocalLicenseApplication = clsLocalLicenseApplication.Find(LocalLicenseApplicationID);
            _TestAppointmentID = TestAppointmentID;

            if (_LocalLicenseApplication == null)
            {
                MessageBox.Show("Local License Application with ID = [" + LocalLicenseApplicationID + "] Does not exist", "Not Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            // This is not the best approach, we can use other optimized query and function
            clsTest LastTest = _LocalLicenseApplication.GetLastTest(TestTypeID);

            if (LastTest == null)
            {
                CreationMode = enCreationMode.LicenseFirstTimeSchedule;
            }
            else
            {
                CreationMode = enCreationMode.RetakeTestSchedule;
            }


            if (CreationMode == enCreationMode.RetakeTestSchedule)
            {
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                float RetakeTestApplicationFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).Fees;
                lblRetakeTestApplicationFees.Text = RetakeTestApplicationFees.ToString();
            }
            else
            {
                gbRetakeTestInfo.Enabled = false;
                lblTitle.Text = "Schedule Test";
                lblRetakeTestApplicationFees.Text = "0";
                lblRetakeTestApplicationID.Text = "N/A";
            }

            lblLocalLicenseApplicationID.Text = _LocalLicenseApplication.LocalLicenseApplicationID.ToString();
            lblLicenseClass.Text = _LocalLicenseApplication.LicenseClassInfo.ClassName;
            lblApplicantName.Text = _LocalLicenseApplication.PersonFullName;
            lblNumOfTrials.Text = _LocalLicenseApplication.GetNumberOfTrials((int)_TestTypeID).ToString();

            switch (_Mode)
            {
                case enMode.AddNew:
                    float TestApplicationFees = clsTestType.Find(_TestTypeID).Fees;
                    lblTestApplicationFees.Text = TestApplicationFees.ToString();
                    DateOfAppointment.MinDate = DateTime.Now;
                    lblRetakeTestApplicationID.Text = "N/A";
                    _TestAppointment = new clsTestAppointment();
                    break;
                case enMode.Update:

                    if (!_LoadTestAppointment())
                    {
                        return;
                    }

                    break;
            }

            lblTotalTestApplicationFees.Text = (Convert.ToSingle(lblTestApplicationFees.Text) +
                Convert.ToSingle(lblRetakeTestApplicationFees.Text)).ToString();

            if (!_HandleActiveTestAppointmentConstraint())
                return;

            if (!_HandleAppointmentLockedConstraint())
                return;

            if (!_HandlePassedPreviousTestConstraint())
                return;

        }

        private bool _HandleRetakeTestApplication()
        {
            // In case the person had a failed test and want to create a new one, the database should create a new application record and link it with test appointment
            if (_Mode == enMode.AddNew && CreationMode == enCreationMode.RetakeTestSchedule)
            {
                clsApplication Application = new clsApplication();
                Application.ApplicantPersonID = _LocalLicenseApplication.ApplicantPersonID;
                Application.ApplicationDate = DateTime.Now;
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).Fees;
                Application.CreatedByUserID = clsGlobal.CurrentUser.UserID;

                if (!Application.Save())
                {
                    _TestAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("Failed to Create an application", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                _TestAppointment.RetakeTestApplicationID = Application.ApplicationID;
            }

            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_HandleRetakeTestApplication())
                return;

            _TestAppointment.TestTypeID = (int)TestTypeID;
            _TestAppointment.LocalLicenseApplicationID = _LocalLicenseApplicationID;
            _TestAppointment.AppointmentDate = DateOfAppointment.Value;
            _TestAppointment.PaidFees = Convert.ToSingle(lblTestApplicationFees.Text);
            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            // in Save you have 2 mode: when you add new appointment or you are updating an appointment
            if (_TestAppointment.Save())
            {
                _Mode = enMode.Update;
                MessageBox.Show("Test Appointment Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Test Appointment is not saved", "Not Saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // To close the form 
            OnSave?.Invoke(null, null);

        }
    }
}
