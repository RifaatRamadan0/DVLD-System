using DVLD___BusinessLayer;
using DVLDWinForms___Presentation_Layer.Licenses;
using DVLDWinForms___Presentation_Layer.Licenses.International_License;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDWinForms___Presentation_Layer.Drivers
{
    public partial class frmListDrivers : Form
    {
        private DataTable _dtDrivers;
        public frmListDrivers()
        {
            InitializeComponent();
        }

        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            _dtDrivers = clsDriver.GetAllDrivers();
            dgvDrivers.DataSource = _dtDrivers;
            lblNumOfRecords.Text = dgvDrivers.Rows.Count.ToString();

            cmbFilterBy.SelectedIndex = 0;
            if (dgvDrivers.Rows.Count > 0)
            {
                dgvDrivers.Columns[0].HeaderText = "Driver ID";
                dgvDrivers.Columns[0].Width = 100;

                dgvDrivers.Columns[1].HeaderText = "Person ID";
                dgvDrivers.Columns[1].Width = 100;

                dgvDrivers.Columns[2].HeaderText = "National No.";
                dgvDrivers.Columns[2].Width = 120;

                dgvDrivers.Columns[3].HeaderText = "Full Name";
                dgvDrivers.Columns[3].Width = 260;

                dgvDrivers.Columns[4].HeaderText = "Date";
                dgvDrivers.Columns[4].Width = 180;

                dgvDrivers.Columns[5].HeaderText = "Active Licenses";
                dgvDrivers.Columns[5].Width = 120;

            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterBy.Clear();
            txtFilterBy.Visible = cmbFilterBy.Text != "None";
            if (txtFilterBy.Visible)
                txtFilterBy.Focus();

        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            string SelectedColumn = "";

            switch (cmbFilterBy.Text)
            {
                case "Driver ID":
                    SelectedColumn = "DriverID";
                    break;
                case "PersonID":
                    SelectedColumn = "PersonID";
                    break;
                case "National No.":
                    SelectedColumn = "NationalNo";
                    break;
                case "Full Name":
                    SelectedColumn = "FullName";
                    break;

            }

            if (txtFilterBy.Text.Trim() == "")
            {
                _dtDrivers.DefaultView.RowFilter = "";
                lblNumOfRecords.Text = dgvDrivers.Rows.Count.ToString();
                return;
            }

            if (SelectedColumn == "FullName" || SelectedColumn == "NationalNo")
                _dtDrivers.DefaultView.RowFilter = $"{SelectedColumn} LIKE '{txtFilterBy.Text.Trim()}%'";
            else
                _dtDrivers.DefaultView.RowFilter = $"{SelectedColumn} = '{txtFilterBy.Text.Trim()}'";

            lblNumOfRecords.Text = dgvDrivers.Rows.Count.ToString();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilterBy.Text == "Person ID" || cmbFilterBy.Text == "Driver ID")
                e.Handled = !char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void ShowPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvDrivers.CurrentRow.Cells[1].Value;
            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
            frmListDrivers_Load(null, null);

        }

        private void showPersonLienseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvDrivers.CurrentRow.Cells[1].Value;
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(PersonID);
            frm.ShowDialog();
            frmListDrivers_Load(null, null);
        }

        private void IssueInternationalLicensetoolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddInternationalLicense frm = new frmAddInternationalLicense();
            frm.ShowDialog();
            frmListDrivers_Load(null, null);
        }
    }
}
