using DVLD___BusinessLayer;
using DVLDWinForms___Presentation_Layer.Global_Classes;
using DVLDWinForms___Presentation_Layer.Licenses;
using DVLDWinForms___Presentation_Layer.Licenses.Local_License;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDWinForms___Presentation_Layer.Applications.Renew_Local_License
{
    public partial class frmRenewLocalLicenseApplication : Form
    {
        public frmRenewLocalLicenseApplication()
        {
            InitializeComponent();
        }

        private int _NewLicenseID = -1;

        private void frmRenewLocalLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            lblIssueDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewLicense).Fees.ToString();
            lblCreatedByUsername.Text = clsGlobal.CurrentUser.UserName;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRenewLocalLicense_Click(object sender, EventArgs e)
        {
            clsLicense NewLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.RenewLicense(txtNotes.Text.Trim(), 
                clsGlobal.CurrentUser.UserID);

            if(NewLicense == null)
            {
                MessageBox.Show("License Has not renewed successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblRenewLicenseApplicationID.Text = NewLicense.ApplicationID.ToString();
            _NewLicenseID = NewLicense.LicenseID;
            lblRenewLocalLicenseID.Text = _NewLicenseID.ToString();

            MessageBox.Show("License Renewed Successfully with ID = [" + NewLicense.LicenseID + "]", "License Renewed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            linkShowLicenseInfo.Enabled = true;
            linkShowLicensesHistory.Enabled = true;
            btnRenewLocalLicense.Enabled = false;

        }

        private void linkShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void linkShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLocalLicenseInfo frm = new frmShowLocalLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int LocalLicenseID = obj;

            btnRenewLocalLicense.Enabled = false;
            linkShowLicensesHistory.Enabled = false;
            linkShowLicenseInfo.Enabled = false;

            if (LocalLicenseID == -1)
            {
                return;
            }

            lblOldLocalLicenseID.Text = LocalLicenseID.ToString();
            linkShowLicensesHistory.Enabled = true;
            txtNotes.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Notes;
            lblExpirationDate.Text = DateTime.Now.AddYears(clsLicenseClass.Find(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassID).DefaultValidityLength).ToString("dd-MMM-yyyy");
            lblLocalLicenseFees.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.PaidFees.ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblLocalLicenseFees.Text) + Convert.ToSingle(lblApplicationFees.Text)).ToString();

            // Check if this local license is expired or not
            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsExpired())
            {
                MessageBox.Show("The Selected License is not expired yet, it will expire on " + ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ExpirationDate, "Not Expired", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Ensure that the license is active
            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("The Selected License is not active, choose an active license", "Not Active", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnRenewLocalLicense.Enabled = true;
        }
    }
}
