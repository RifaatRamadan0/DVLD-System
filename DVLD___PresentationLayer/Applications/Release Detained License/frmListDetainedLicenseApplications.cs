using DVLD___BusinessLayer;
using DVLDWinForms___Presentation_Layer.Licenses;
using DVLDWinForms___Presentation_Layer.Licenses.Detain_License;
using DVLDWinForms___Presentation_Layer.Licenses.Local_License;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDWinForms___Presentation_Layer.Applications.Release_Detained_License
{
    public partial class frmListDetainedLicenseApplications : Form
    {
        public frmListDetainedLicenseApplications()
        {
            InitializeComponent();
        }

        private DataTable _dtDetainedLicenses;

        private void frmListDetainedLicenseApplications_Load(object sender, EventArgs e)
        {
            _dtDetainedLicenses = clsDetainedLicense.GetAllDetainedLicenses();
            dgvDetainedLicenseApplications.DataSource = _dtDetainedLicenses;
            lblNumOfRecords.Text = dgvDetainedLicenseApplications.Rows.Count.ToString();

            cmbFilterBy.SelectedIndex = 0;
            if(dgvDetainedLicenseApplications.Rows.Count > 0)
            {
                dgvDetainedLicenseApplications.Columns[0].HeaderText = "D.ID";
                dgvDetainedLicenseApplications.Columns[0].Width = 80;

                dgvDetainedLicenseApplications.Columns[1].HeaderText = "L.ID";
                dgvDetainedLicenseApplications.Columns[1].Width = 80;

                dgvDetainedLicenseApplications.Columns[2].HeaderText = "D.Date";
                dgvDetainedLicenseApplications.Columns[2].Width = 140;

                dgvDetainedLicenseApplications.Columns[3].HeaderText = "Is Released";
                dgvDetainedLicenseApplications.Columns[3].Width = 80;

                dgvDetainedLicenseApplications.Columns[4].HeaderText = "Fine Fees";
                dgvDetainedLicenseApplications.Columns[4].Width = 100;

                dgvDetainedLicenseApplications.Columns[5].HeaderText = "Release Date";
                dgvDetainedLicenseApplications.Columns[5].Width = 140;

                dgvDetainedLicenseApplications.Columns[6].HeaderText = "N.No";
                dgvDetainedLicenseApplications.Columns[6].Width = 80;

                dgvDetainedLicenseApplications.Columns[7].HeaderText = "Full Name";
                dgvDetainedLicenseApplications.Columns[7].Width = 240;

                dgvDetainedLicenseApplications.Columns[8].HeaderText = "Release App.ID";
                dgvDetainedLicenseApplications.Columns[8].Width = 120;
            }

        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterBy.Clear();
            txtFilterBy.Visible = (cmbFilterBy.Text != "None" && cmbFilterBy.Text != "Is Released");

            if(txtFilterBy.Visible)
                txtFilterBy.Focus();

            cmbIsReleased.Visible = (cmbFilterBy.Text == "Is Released");

            if (cmbIsReleased.Visible)
                cmbIsReleased.SelectedIndex = 0;
        }

        private void cmbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool IsReleased = true;

            switch(cmbIsReleased.Text)
            {
                case "Yes":
                    IsReleased = true;
                    break;
                case "No":
                    IsReleased = false;
                    break;

            }

            if (cmbIsReleased.Text == "All")
                _dtDetainedLicenses.DefaultView.RowFilter = "";
            else
                _dtDetainedLicenses.DefaultView.RowFilter = $"IsReleased = {IsReleased}";

        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();
            frmListDetainedLicenseApplications_Load(null, null);

        }

        private void btnReleaseLicense_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicenseApplication frm = new frmReleaseDetainedLicenseApplication();
            frm.ShowDialog();
            frmListDetainedLicenseApplications_Load(null, null);
        }

        private void cmsDetainedLicense_Opening(object sender, CancelEventArgs e)
        {
            bool IsAlreadyReleased = (bool)dgvDetainedLicenseApplications.CurrentRow.Cells[3].Value;
            releaseDetainedLicenseToolStripMenuItem.Enabled = !IsAlreadyReleased;

        }

        private void ShowPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string NationalNo = (string)dgvDetainedLicenseApplications.CurrentRow.Cells[6].Value;
            frmShowPersonInfo frm = new frmShowPersonInfo(NationalNo);
            frm.ShowDialog();   
            frmListDetainedLicenseApplications_Load(null, null);
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvDetainedLicenseApplications.CurrentRow.Cells[1].Value;
            frmShowLocalLicenseInfo frm = new frmShowLocalLicenseInfo(LicenseID);
            frm.ShowDialog();
        }

        private void showPersonLienseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clsDriver.GetDriverIDByLienseID()
           
            int LicenseID = (int)dgvDetainedLicenseApplications.CurrentRow.Cells[1].Value;
            int PersonID = clsLicense.Find(LicenseID).DriverInfo.PersonID;
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(PersonID);
            frm.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvDetainedLicenseApplications.CurrentRow.Cells[1].Value;
            frmReleaseDetainedLicenseApplication frm = new frmReleaseDetainedLicenseApplication(LicenseID);
            frm.ShowDialog();
            frmListDetainedLicenseApplications_Load(null, null);
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            string SelectedColumn = "";

            switch(cmbFilterBy.Text)
            {
                case "Detain ID":
                    SelectedColumn = "DetainID";
                    break;
                case "National No.":
                    SelectedColumn = "NationalNo";
                    break;
                case "Full Name":
                    SelectedColumn = "FullName";
                    break;
                case "Release Application ID":
                    SelectedColumn = "ReleaseApplicationID";
                    break;

            }

            if(txtFilterBy.Text.Trim() == "")
            {
                _dtDetainedLicenses.DefaultView.RowFilter = "";
                lblNumOfRecords.Text = dgvDetainedLicenseApplications.Rows.Count.ToString();
                return;
            }

            if(SelectedColumn == "FullName" || SelectedColumn == "NationalNo")
                _dtDetainedLicenses.DefaultView.RowFilter = $"{SelectedColumn} LIKE '{txtFilterBy.Text.Trim()}%'";
            else
                _dtDetainedLicenses.DefaultView.RowFilter = $"{SelectedColumn} = '{txtFilterBy.Text.Trim()}'";

            lblNumOfRecords.Text = dgvDetainedLicenseApplications.Rows.Count.ToString();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cmbFilterBy.Text == "Detain ID" || cmbFilterBy.Text == "Release Application ID")
            {
                e.Handled = !char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }
    }
}
