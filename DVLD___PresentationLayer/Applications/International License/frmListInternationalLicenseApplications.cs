using DVLD___BusinessLayer;
using DVLDWinForms___Presentation_Layer.Licenses;
using DVLDWinForms___Presentation_Layer.Licenses.International_License;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDWinForms___Presentation_Layer.Applications.International_License
{
    public partial class frmListInternationalLicenseApplications : Form
    {
        public frmListInternationalLicenseApplications()
        {
            InitializeComponent();
        }

        private DataTable _dtInternationalLicenses;

        private void frmListInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            _dtInternationalLicenses = clsInternationalLicense.GetAllInternationalLicenses();
            dgvInternationalLicenseApplications.DataSource = _dtInternationalLicenses;
            lblNumOfRecords.Text = dgvInternationalLicenseApplications.Rows.Count.ToString();

            cmbFilterBy.SelectedIndex = 0;
            if(dgvInternationalLicenseApplications.Rows.Count > 0)
            {
                dgvInternationalLicenseApplications.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicenseApplications.Columns[0].Width = 100;

                dgvInternationalLicenseApplications.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicenseApplications.Columns[1].Width = 100;

                dgvInternationalLicenseApplications.Columns[2].HeaderText = "Driver ID";
                dgvInternationalLicenseApplications.Columns[2].Width = 100;

                dgvInternationalLicenseApplications.Columns[3].HeaderText = "L.License ID";
                dgvInternationalLicenseApplications.Columns[3].Width = 100;

                dgvInternationalLicenseApplications.Columns[4].HeaderText = "Issue Date";
                dgvInternationalLicenseApplications.Columns[4].Width = 180;

                dgvInternationalLicenseApplications.Columns[5].HeaderText = "Expiration Date";
                dgvInternationalLicenseApplications.Columns[5].Width = 180;

                dgvInternationalLicenseApplications.Columns[6].HeaderText = "Is Active";
                dgvInternationalLicenseApplications.Columns[6].Width = 80;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar));
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            string SelectedColumn = "";

            switch (cmbFilterBy.Text)
            {
                case "International License ID":
                    SelectedColumn = "InternationalLicenseID";
                    break;
                case "Application ID":
                    SelectedColumn = "ApplicationID";
                    break;
                case "Driver ID":
                    SelectedColumn = "DriverID";
                    break;
                case "Local License ID":
                    SelectedColumn = "IssuedUsingLocalLicenseID";
                    break;
                default:
                    SelectedColumn = "None";
                    break;
            }

            if(txtFilterBy.Text.Trim() == "" || SelectedColumn == "None")
                _dtInternationalLicenses.DefaultView.RowFilter = "";   
            else
                _dtInternationalLicenses.DefaultView.RowFilter = $"{SelectedColumn} = '{txtFilterBy.Text.Trim()}'";

            lblNumOfRecords.Text = dgvInternationalLicenseApplications.Rows.Count.ToString();

        }

        private void cmbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterBy.Clear();
            txtFilterBy.Visible = (cmbFilterBy.Text != "None" && cmbFilterBy.Text != "Is Active");
            cmbIsActive.Visible = (cmbFilterBy.Text == "Is Active");

            if (cmbIsActive.Visible)
            { 
                cmbIsActive.SelectedIndex = 0;
                cmbIsActive.Focus();
            }
            else
                txtFilterBy.Focus();

        }

        private void cmbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbIsActive.Text)
            {
                case "All":
                    _dtInternationalLicenses.DefaultView.RowFilter = "";
                    break;
                case "Yes":
                    _dtInternationalLicenses.DefaultView.RowFilter = "IsActive = 1";
                    break;
                case "No":
                    _dtInternationalLicenses.DefaultView.RowFilter = "IsActive = 0";
                    break;
            }

            lblNumOfRecords.Text = dgvInternationalLicenseApplications.Rows.Count.ToString();

        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddInternationalLicense frm = new frmAddInternationalLicense();
            frm.ShowDialog();

            frmListInternationalLicenseApplications_Load(null, null);
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvInternationalLicenseApplications.CurrentRow.Cells[2].Value;
            clsDriver Driver = clsDriver.FindByDriverID(DriverID);

            frmShowPersonInfo frm = new frmShowPersonInfo(Driver.PersonID);
            frm.ShowDialog();

            frmListInternationalLicenseApplications_Load(null, null);
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int InternationalLicenseID = (int)dgvInternationalLicenseApplications.CurrentRow.Cells[0].Value;
            
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(InternationalLicenseID);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvInternationalLicenseApplications.CurrentRow.Cells[2].Value;
            clsDriver Driver = clsDriver.FindByDriverID(DriverID);

            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(Driver.PersonID);
            frm.ShowDialog();
        }
    }
}
