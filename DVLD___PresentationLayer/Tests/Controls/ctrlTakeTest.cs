using DVLD___BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDWinForms___Presentation_Layer.Tests.Controls
{
    public partial class ctrlTakeTest : UserControl
    {
        private clsTestAppointment _TestAppointment;
        private int _TestAppointmentID = -1;
        private clsLocalLicenseApplication _LocalLicenseApplication;
        private int _LocalLicenseApplicationID = -1;
        private clsTestType.enTestType _TestTypeID;
        private int _TestID = -1;

        public ctrlTakeTest()
        {
            InitializeComponent();
        }

        private void _HandleTestTypes()
        {
            switch (_TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    gbTestInfo.Text = "Vision Test";
                    pbTestType.ImageLocation = @"C:\Users\rifaa\Desktop\Programming Princples Courses\19th Principle Course\DVLD System\DVLDWinForms - Presentation Layer\Icons\Vision 512.png";
                    break;
                case clsTestType.enTestType.WrittenTest:
                    gbTestInfo.Text = "Written Test";
                    pbTestType.ImageLocation = @"C:\Users\rifaa\Desktop\Programming Princples Courses\19th Principle Course\DVLD System\DVLDWinForms - Presentation Layer\Icons\Written Test 512.png";
                    break;
                case clsTestType.enTestType.StreetTest:
                    gbTestInfo.Text = "Street Test";
                    pbTestType.ImageLocation = @"C:\Users\rifaa\Desktop\Programming Princples Courses\19th Principle Course\DVLD System\DVLDWinForms - Presentation Layer\Icons\driving-test 512.png";
                    break;
            }

        }

        public clsTestType.enTestType TestTypeID
        {
            get { return _TestTypeID; }
            set
            {
                _TestTypeID = value;
                _HandleTestTypes();
            }
        }

        public int TestID
        {
            get { return _TestID; }
        }

        public int TestAppointmentID
        {
            get { return _TestAppointmentID; }
        }

        public void LoadInfo(int AppointmentID)
        {
            _TestAppointmentID = AppointmentID;
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);

            if (_TestAppointment == null)
            {
                MessageBox.Show("There is no test appointment with ID = " + AppointmentID, "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _TestAppointmentID = -1;
                return;
            }

            _LocalLicenseApplicationID = _TestAppointment.LocalLicenseApplicationID;
            _LocalLicenseApplication = clsLocalLicenseApplication.Find(_LocalLicenseApplicationID);

            if(_LocalLicenseApplication == null)
            {
                MessageBox.Show("There is no Local License Application with ID = " + _LocalLicenseApplicationID, "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            TestTypeID = (clsTestType.enTestType)_TestAppointment.TestTypeID;

            lblLocalLicenseApplicationID.Text = _LocalLicenseApplication.LocalLicenseApplicationID.ToString();
            lblLicenseClass.Text = _LocalLicenseApplication.LicenseClassInfo.ClassName;
            lblApplicantName.Text = _LocalLicenseApplication.PersonFullName;
            lblNumOfTrials.Text = _LocalLicenseApplication.GetNumberOfTrials((int)TestTypeID).ToString();

            lblAppointmentDate.Text = _TestAppointment.AppointmentDate.ToString("dd-MMM-yyyy");
            lblTestApplicationFees.Text = _TestAppointment.PaidFees.ToString();

            _TestID = _TestAppointment.TestID;
            if (_TestID != -1)
                lblTestID.Text = _TestID.ToString();
            else
                lblTestID.Text = "Not Taken Yet";



        }

    }
}
