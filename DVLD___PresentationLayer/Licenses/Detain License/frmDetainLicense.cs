using DVLD___BusinessLayer;
using DVLDWinForms___Presentation_Layer.Global_Classes;
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

namespace DVLDWinForms___Presentation_Layer.Licenses.Detain_License
{
    public partial class frmDetainLicense : Form
    {
        public frmDetainLicense()
        {
            InitializeComponent();
        }

        private int _DetainedLicenseID = -1;

        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            lblCreatedByUsername.Text = clsGlobal.CurrentUser.UserName;

        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            if(txtFees.Text.Trim() == "")
            {
                errorProvider1.SetError(txtFees, "This Field is required!");
                return;
            }

            if(!clsValidation.IsNumber(txtFees.Text.Trim()))
            {
                errorProvider1.SetError(txtFees, "Invalid Number!");
                return;
            }

            e.Cancel = false;
            errorProvider1.SetError(txtFees, "");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields are not valid, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            float FineFees = float.Parse(txtFees.Text);
            clsDetainedLicense DetainedLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo
                .Detain(FineFees, clsGlobal.CurrentUser.UserID);

            if (DetainedLicense == null)
            {
                MessageBox.Show("The License is not detained successfully", "Not Detained", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _DetainedLicenseID = DetainedLicense.LicenseID;
            lblDetainID.Text = DetainedLicense.DetainID.ToString();
            lblLicenseID.Text = _DetainedLicenseID.ToString();
            MessageBox.Show("The License with ID = [" + _DetainedLicenseID + "] is detained", "License Detained", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnDetainLicense.Enabled = false;
            linkShowLicenseInfo.Enabled = true;
            txtFees.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;

            linkShowLicenseInfo.Enabled = false;
            linkShowLicensesHistory.Enabled = false;
            btnDetainLicense.Enabled = false;

            if(SelectedLicenseID == -1)
            {
                return;
            }

            lblLicenseID.Text = SelectedLicenseID.ToString();
            linkShowLicensesHistory.Enabled = true;

            if(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("The Selected License is already detained", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtFees.Focus();
            btnDetainLicense.Enabled = true;

        }

        private void linkShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void linkShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLocalLicenseInfo frm = new frmShowLocalLicenseInfo(_DetainedLicenseID);
            frm.ShowDialog();
        }
    }
}
