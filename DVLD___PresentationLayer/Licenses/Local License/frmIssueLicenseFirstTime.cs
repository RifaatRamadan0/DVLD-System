using DVLD___BusinessLayer;
using DVLDWinForms___Presentation_Layer.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDWinForms___Presentation_Layer.Applications.Local_Driving_License
{
    public partial class frmIssueLicenseFirstTime : Form
    {
        private int _LocalLicenseApplicationID = -1;
        private clsLocalLicenseApplication _LocalLicenseApplication;

        public frmIssueLicenseFirstTime(int LocalLicenseApplicationID)
        {
            InitializeComponent();

            _LocalLicenseApplicationID = LocalLicenseApplicationID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmIssueLicenseFirstTime_Load(object sender, EventArgs e)
        {
            _LocalLicenseApplication = clsLocalLicenseApplication.Find(_LocalLicenseApplicationID);

            if (_LocalLicenseApplication == null)
            {
                MessageBox.Show("There is no local license application with ID = " + _LocalLicenseApplicationID, "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }


            // Ensure that the person is eligible to issue a new license for more security
            if (!_LocalLicenseApplication.HasPassedAllTests())
            {
                MessageBox.Show("The Person has not passed all the tests required", "Not Eligible", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if(_LocalLicenseApplication.IsLicenseIssued())
            {
                MessageBox.Show("There is already issued license for this person", "License already issued", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrlDrivingLicenseApplicationInfo1.LoadDrivingLicenseApplicationInfo(_LocalLicenseApplication.LocalLicenseApplicationID);

        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {

            int LicenseID = _LocalLicenseApplication.IssueLicenseForTheFirstTime(txtNotes.Text.Trim(), clsGlobal.CurrentUser.UserID);

            if(LicenseID != -1)
            {
                MessageBox.Show("License with ID = " + LicenseID + " Issued Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("License is not issued Successfully.", "Not Saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
        }

    }
}
