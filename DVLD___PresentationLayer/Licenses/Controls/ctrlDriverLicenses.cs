using DVLD___BusinessLayer;
using DVLDWinForms___Presentation_Layer.Licenses.International_License;
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

namespace DVLDWinForms___Presentation_Layer.Licenses.Controls
{
    public partial class ctrlDriverLicenses : UserControl
    {
        private int _DriverID = -1;
        private clsDriver _Driver;
        private DataTable _dtLocalLicenses;
        private DataTable _dtInternationalLicenses;

        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }


        private void _LoadLocalLicenses()
        {
            _dtLocalLicenses = clsDriver.GetLocalLicensesByDriverID(_DriverID);
            if(_dtLocalLicenses.Rows.Count > 0)
                _dtLocalLicenses = _dtLocalLicenses.DefaultView.ToTable(false, "LicenseID", "ApplicationID", "ClassName",
                "IssueDate", "ExpirationDate", "IsActive");
            dgvLocalLicenses.DataSource = _dtLocalLicenses;
            lblNumOfLocalLicenses.Text = _dtLocalLicenses.Rows.Count.ToString();

            if(dgvLocalLicenses.Rows.Count > 0)
            {
                dgvLocalLicenses.Columns[0].HeaderText = "License ID";
                dgvLocalLicenses.Columns[0].Width = 130;

                dgvLocalLicenses.Columns[1].HeaderText = "Application ID";
                dgvLocalLicenses.Columns[1].Width = 130;

                dgvLocalLicenses.Columns[2].HeaderText = "Class Name";
                dgvLocalLicenses.Columns[2].Width = 250;

                dgvLocalLicenses.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicenses.Columns[3].Width = 200;

                dgvLocalLicenses.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicenses.Columns[4].Width = 200;

                dgvLocalLicenses.Columns[5].HeaderText = "Is Active";
                dgvLocalLicenses.Columns[5].Width = 80;
            }

        }

        private void _LoadInternationalLicenses()
        {
            _dtInternationalLicenses = clsDriver.GetInternationalLicensesByDriverID(_DriverID);
            if(_dtInternationalLicenses.Rows.Count > 0)
                _dtInternationalLicenses = _dtInternationalLicenses.DefaultView.ToTable(false, "InternationalLicenseID", "ApplicationID",
                    "IssuedUsingLocalLicenseID", "IssueDate", "ExpirationDate", "IsActive");
            dgvInternationalLicenses.DataSource = _dtInternationalLicenses;
            lblNumOfInternationalLicenses.Text = _dtInternationalLicenses.Rows.Count.ToString();

            if(dgvInternationalLicenses.Rows.Count > 0)
            {
                dgvInternationalLicenses.Columns[0].HeaderText = "International License ID";
                dgvInternationalLicenses.Columns[0].Width = 130;

                dgvInternationalLicenses.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicenses.Columns[1].Width = 150;

                dgvInternationalLicenses.Columns[2].HeaderText = "Local License ID";
                dgvInternationalLicenses.Columns[2].Width = 150;

                dgvInternationalLicenses.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicenses.Columns[3].Width = 200;

                dgvInternationalLicenses.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalLicenses.Columns[4].Width = 200;

                dgvInternationalLicenses.Columns[5].HeaderText = "Is Active";
                dgvInternationalLicenses.Columns[5].Width = 100;
            }

        }

        public void LoadDriverLicenses(int PersonID)
        {
            _Driver = clsDriver.FindByPersonID(PersonID);

            if(_Driver == null)
            {
                lblNumOfLocalLicenses.Text = "0";
                lblNumOfInternationalLicenses.Text = "0";
                MessageBox.Show("There is no driver with person ID = [" + PersonID + "]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _DriverID = _Driver.DriverID;
            _LoadLocalLicenses();
            _LoadInternationalLicenses();
                                   
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow LocalLicenseRow = dgvLocalLicenses.CurrentRow;

            if (LocalLicenseRow != null)
            {
                int LocalLicenseID = (int)LocalLicenseRow.Cells[0].Value;
                frmShowLocalLicenseInfo frm = new frmShowLocalLicenseInfo(LocalLicenseID);
                frm.ShowDialog();
            }

        }

        public void Clear()
        {
            dgvLocalLicenses.DataSource = "";
            dgvInternationalLicenses.DataSource = "";
        }


        private void showLicenseInfoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataGridViewRow InternationalLicenseRow = dgvInternationalLicenses.CurrentRow;

            if(InternationalLicenseRow != null)
            {
                int InternationalLicenseID = (int)InternationalLicenseRow.Cells[0].Value;
                frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(InternationalLicenseID);
                frm.ShowDialog();
            }
        }
    }
}
