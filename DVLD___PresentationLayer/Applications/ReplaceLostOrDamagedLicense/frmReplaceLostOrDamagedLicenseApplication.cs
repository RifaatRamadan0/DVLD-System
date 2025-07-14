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

namespace DVLDWinForms___Presentation_Layer.Applications.ReplaceLostOrDamagedLicense
{
    public partial class frmReplaceLostOrDamagedLicenseApplication : Form
    {
        private int _ReplacedLicenseID = -1;
        public clsLicense.enIssueReason IssueReason;

        public frmReplaceLostOrDamagedLicenseApplication()
        {
            InitializeComponent();
        }

        private void frmReplaceLostOrDamagedLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            lblCreatedByUsername.Text = clsGlobal.CurrentUser.UserName;
            rbDamagedLicense.Checked = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void linkShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLocalLicenseInfo frm = new frmShowLocalLicenseInfo(_ReplacedLicenseID);
            frm.ShowDialog();
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            IssueReason = clsLicense.enIssueReason.ReplacementForLost;
            lblTitle.Text = "Replacement For Lost License";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = clsApplicationType.Find((int)IssueReason).Fees.ToString();
        }
        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            IssueReason = clsLicense.enIssueReason.ReplacementForDamaged;
            lblTitle.Text = "Replacement For Damaged License";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = clsApplicationType.Find((int)IssueReason).Fees.ToString();
        }

        private void btnIssueReplacedLicense_Click(object sender, EventArgs e)
        {
            clsLicense ReplacedLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ReplaceLicense(IssueReason, clsGlobal.CurrentUser.UserID);

            if(ReplacedLicense == null)
            {
                MessageBox.Show("License is not replaced successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblReplaceLicenseApplicationID.Text = ReplacedLicense.ApplicationID.ToString();
            _ReplacedLicenseID = ReplacedLicense.LicenseID;
            lblReplacedLocalLicenseID.Text = _ReplacedLicenseID.ToString();
            MessageBox.Show("License Replaced Successfully with ID = [" + _ReplacedLicenseID + "]", "License Replaced", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnIssueReplacedLicense.Enabled = false;
            linkShowLicenseInfo.Enabled = true;
            gbReplacementFor.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int LocalLicenseID = obj;

            linkShowLicenseInfo.Enabled = false;
            linkShowLicensesHistory.Enabled = false;
            btnIssueReplacedLicense.Enabled = false;

            if (LocalLicenseID == -1)
                return;

            lblOldLocalLicenseID.Text = LocalLicenseID.ToString();
            linkShowLicensesHistory.Enabled = true;

            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsExpired())
            {
                MessageBox.Show("The Selected License is expired, Choose another one", "Not Active", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("The Selected License is not Active, Choose an active license", "Not Active", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnIssueReplacedLicense.Enabled = true;
        }
    }
}
