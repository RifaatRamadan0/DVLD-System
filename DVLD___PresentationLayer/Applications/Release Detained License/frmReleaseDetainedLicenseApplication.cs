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

namespace DVLDWinForms___Presentation_Layer.Applications.Release_Detained_License
{
    public partial class frmReleaseDetainedLicenseApplication : Form
    {
        private int _ReleasedLicenseID = -1;
        public frmReleaseDetainedLicenseApplication()
        {
            InitializeComponent();
        }

        public frmReleaseDetainedLicenseApplication(int LicenseID)
        {
            InitializeComponent();

            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(LicenseID);
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
        }

        private void linkShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void linkShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLocalLicenseInfo frm = new frmShowLocalLicenseInfo(_ReleasedLicenseID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;

            linkShowLicenseInfo.Enabled = false;
            linkShowLicensesHistory.Enabled = false;
            btnReleaseLicense.Enabled = false;

            if (SelectedLicenseID == -1)
                return;

            linkShowLicensesHistory.Enabled = true;
            lblLicenseID.Text = SelectedLicenseID.ToString();

            if(!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("The Selected License is not detained", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
                        
            lblDetainID.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainInfo.DetainID.ToString();
            lblLicenseID.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID.ToString();
            lblDetainDate.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainInfo.DetainDate.ToString("dd-MMM-yyyy");
            lblCreatedByUsername.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainInfo.CreatedByUserInfo.UserName;
            lblFineFees.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainInfo.FineFees.ToString();
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedLicense).Fees.ToString();
            lblTotalFees.Text = (float.Parse(lblFineFees.Text) + float.Parse(lblApplicationFees.Text)).ToString();

            btnReleaseLicense.Enabled = true;
        }

        private void btnReleaseLicense_Click(object sender, EventArgs e)
        {

            int ReleaseApplicationID = -1;
            bool IsReleased = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Release(clsGlobal.CurrentUser.UserID, ref ReleaseApplicationID);

            if (!IsReleased)
            {
                MessageBox.Show("The Selected License is not released", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblApplicationID.Text = ReleaseApplicationID.ToString();
            _ReleasedLicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID;
            MessageBox.Show("Detained License released Successfully ", "Detained License Released", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnReleaseLicense.Enabled = false;
            linkShowLicenseInfo.Enabled = true;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;

        }
    }
}
