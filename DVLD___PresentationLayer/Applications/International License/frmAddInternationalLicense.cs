using DVLD___BusinessLayer;
using DVLDWinForms___Presentation_Layer.Global_Classes;
using DVLDWinForms___Presentation_Layer.Licenses.Local_License.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDWinForms___Presentation_Layer.Licenses.International_License
{
    public partial class frmAddInternationalLicense : Form
    {
        public frmAddInternationalLicense()
        {
            InitializeComponent();
        }

        private int _InternationalLicenseID = -1;

        private void ValidateSelectedLicense(int SelectedLocalLicenseID)
        {

            lblLocalLicenseID.Text = SelectedLocalLicenseID.ToString();
            linkShowLicensesHistory.Enabled = (SelectedLocalLicenseID != -1);
            linkShowInternationalLicenseInfo.Enabled = false;
            btnIssueInternationalLicense.Enabled = false;

            // Verify whether the local license meets the requirements for issuing an international license.

            // Its message box already showed
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo == null)
            {
                lblLocalLicenseID.Text = "[???]";
                return;
            }

            // Verify that the license class is 3
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassID != 3)
            {

                MessageBox.Show("Selected License Should be Class 3, Select Another One", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if the International license is already issued
            _InternationalLicenseID = clsInternationalLicense.GetActiveInternationalLicenseIDByDriverID(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID);
            if (_InternationalLicenseID != -1)
            {
                linkShowInternationalLicenseInfo.Enabled = true;
                MessageBox.Show("There is already an issued internation license for this person with ID = " + _InternationalLicenseID, "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            btnIssueInternationalLicense.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmIssueInternationalLicense_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            lblIssueDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToString("dd-MMM-yyyy"); // It's better to make 1 dynamic in the system and not Hardcoded
            lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).Fees.ToString();
            lblCreatedByUsername.Text = clsGlobal.CurrentUser.UserName;

            // Subscribe manualy in the OnSelectedLicense event
            ctrlDriverLicenseInfoWithFilter1.OnLicenseSelected += ValidateSelectedLicense; 
        }

        private void linkShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void linkShowInternationalLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }

        private void btnIssueInternationalLicense_Click(object sender, EventArgs e)
        {
             clsInternationalLicense InternationalLicense = new clsInternationalLicense();

            // Fill the base application info first
            InternationalLicense.ApplicationStatus = clsApplication.enApplicationStatus.Completed; // Since there is no process to issue the license
            InternationalLicense.LastStatusDate = DateTime.Now;
            InternationalLicense.ApplicantPersonID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID;
            InternationalLicense.ApplicationDate = DateTime.Now;
            InternationalLicense.ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;
            InternationalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            InternationalLicense.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).Fees;


            InternationalLicense.DriverID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID;
            InternationalLicense.IssuedUsingLocalLicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID;
            InternationalLicense.IssueDate = DateTime.Now;
            InternationalLicense.ExpirationDate = DateTime.Now.AddYears(1);
            InternationalLicense.IsActive = true;


            if (!InternationalLicense.Save())
            {
                MessageBox.Show("The International License Is not Issued Successfully", "Not Saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _InternationalLicenseID = InternationalLicense.InternationalLicenseID;
            lblInternationalLicenseApplicationID.Text = InternationalLicense.ApplicationID.ToString();
            lblInternationalLicenseID.Text = _InternationalLicenseID.ToString();
            MessageBox.Show("The International License with ID = [" + InternationalLicense.InternationalLicenseID + "] Issued Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            linkShowInternationalLicenseInfo.Enabled = true;
            btnIssueInternationalLicense.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;

        }
    }
}
