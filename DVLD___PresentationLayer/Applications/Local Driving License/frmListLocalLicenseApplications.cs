using DVLD___BusinessLayer;
using DVLDWinForms___Presentation_Layer.Global_Classes;
using DVLDWinForms___Presentation_Layer.Licenses;
using DVLDWinForms___Presentation_Layer.Licenses.Local_License;
using DVLDWinForms___Presentation_Layer.Tests;
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
    public partial class frmListLocalLicenseApplications : Form
    {
        public frmListLocalLicenseApplications()
        {
            InitializeComponent();
        }

        private DataTable _dt;

        private void _RefreshData()
        {
            _dt = clsLocalLicenseApplication.GetAllLocalLicenseApplications();
            dgvLocalLicenseApplications.DataSource = _dt;
            lblNumOfRecords.Text = dgvLocalLicenseApplications.Rows.Count.ToString();
        }
        private void frmListLocalLicenseApplications_Load(object sender, EventArgs e)
        {
            _RefreshData();

            cmbFilterBy.SelectedIndex = 0;
            if (dgvLocalLicenseApplications.Rows.Count > 0)
            {
                dgvLocalLicenseApplications.Columns[0].Width = 140;
                dgvLocalLicenseApplications.Columns[0].HeaderText = "L.D.L.AppID";

                dgvLocalLicenseApplications.Columns[1].Width = 300;
                dgvLocalLicenseApplications.Columns[1].HeaderText = "Driving Class";

                dgvLocalLicenseApplications.Columns[2].Width = 140;
                dgvLocalLicenseApplications.Columns[2].HeaderText = "National No.";

                dgvLocalLicenseApplications.Columns[3].Width = 250;
                dgvLocalLicenseApplications.Columns[3].HeaderText = "Full Name";

                dgvLocalLicenseApplications.Columns[4].Width = 170;
                dgvLocalLicenseApplications.Columns[4].HeaderText = "Application Date";

                dgvLocalLicenseApplications.Columns[5].Width = 110;
                dgvLocalLicenseApplications.Columns[5].HeaderText = "Passed Tests";

                dgvLocalLicenseApplications.Columns[6].Width = 110;
                dgvLocalLicenseApplications.Columns[6].HeaderText = "Status";

            }

        }

        private void cmbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterBy.Visible = (cmbFilterBy.Text != "None");
            txtFilterBy.Clear();
            txtFilterBy.Focus();
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            if(txtFilterBy.Text.Trim() == "")
            {
                _dt.DefaultView.RowFilter = "";
                lblNumOfRecords.Text = dgvLocalLicenseApplications.Rows.Count.ToString();
                return;
            }

            string SelectedColumn = "";
            switch(cmbFilterBy.Text)
            {
                case "L.D.L.AppID":
                    SelectedColumn = "LocalDrivingLicenseApplicationID";
                    break;
                case "National No.":
                    SelectedColumn = "NationalNo";
                    break;
                case "Full Name":
                    SelectedColumn = "FullName";
                    break;
                case "Status":
                    SelectedColumn = "Status";
                    break;

            }

            if(SelectedColumn == "LocalDrivingLicenseApplicationID")
                _dt.DefaultView.RowFilter = $"{SelectedColumn} = {txtFilterBy.Text.Trim()}";
            else
                _dt.DefaultView.RowFilter = $"{SelectedColumn} LIKE '{txtFilterBy.Text.Trim()}%'";

            lblNumOfRecords.Text = dgvLocalLicenseApplications.Rows.Count.ToString();
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalLicenseApplication frm = new frmAddUpdateLocalLicenseApplication();
            frm.ShowDialog();

            _RefreshData();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cmbFilterBy.Text == "L.D.L.AppID")
            {
                e.Handled = (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

            int LocalLicenseApplicationID = (int)dgvLocalLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalLicenseApplication LocalLicenseApplication = clsLocalLicenseApplication.Find(LocalLicenseApplicationID);

            int PassedTests = (int)dgvLocalLicenseApplications.CurrentRow.Cells[5].Value;
            bool IsLicenseIssued = LocalLicenseApplication.IsLicenseIssued();

            editApplicationToolStripMenuItem.Enabled = (!IsLicenseIssued && LocalLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New); 
            deleteApplicationToolStripMenuItem.Enabled = (LocalLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);
            cancelApplicationToolStripMenuItem.Enabled = (LocalLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (PassedTests == 3 && !IsLicenseIssued);
            showLicenseToolStripMenuItem.Enabled = IsLicenseIssued;


            bool HasPassedVisionTest = LocalLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest);
            bool HasPassedWrittenTest = LocalLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest);
            bool HasPassedStreetTest = LocalLicenseApplication.DoesPassTestType(clsTestType.enTestType.StreetTest);

            scheduleTestsToolStripMenuItem.Enabled = (!HasPassedVisionTest || !HasPassedWrittenTest || !HasPassedStreetTest) 
                && (LocalLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);

            if(scheduleTestsToolStripMenuItem.Enabled)
            {
                scheduleVisionTestToolStripMenuItem.Enabled = !HasPassedVisionTest;
                scheduleWrittenTestToolStripMenuItem.Enabled = HasPassedVisionTest && !HasPassedWrittenTest;
                scheduleStreetTestToolStripMenuItem.Enabled = HasPassedVisionTest && HasPassedWrittenTest && !HasPassedStreetTest;
            }

        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalLicenseApplicationID = (int)dgvLocalLicenseApplications.CurrentRow.Cells[0].Value;
            frmDrivingLicenseApplicationInfo frm = new frmDrivingLicenseApplicationInfo(LocalLicenseApplicationID);
            frm.ShowDialog();
            _RefreshData();
        }

        private void _ScheduleTest(clsTestType.enTestType TestType)
        {
            frmListTestAppointments frm = new frmListTestAppointments((int)dgvLocalLicenseApplications.CurrentRow.Cells[0].Value, TestType);
            frm.ShowDialog();
            _RefreshData();
        }
        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.VisionTest);
        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.WrittenTest);
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.StreetTest);
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalLicenseApplicationID = (int)dgvLocalLicenseApplications.CurrentRow.Cells[0].Value;
            frmAddUpdateLocalLicenseApplication frm = new frmAddUpdateLocalLicenseApplication(LocalLicenseApplicationID);
            frm.ShowDialog();
            _RefreshData();
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to cancel this application?", "Confirm", MessageBoxButtons.YesNo, 
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                // Update the application status to Cancel
                int LocalLicenseApplicationID = (int)dgvLocalLicenseApplications.CurrentRow.Cells[0].Value;
                clsLocalLicenseApplication LocalLicenseApplication = clsLocalLicenseApplication.Find(LocalLicenseApplicationID);

                if(LocalLicenseApplication == null)
                {
                    MessageBox.Show("There is no Local Application with ID = " + LocalLicenseApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


                if(LocalLicenseApplication.Cancel())
                {
                    MessageBox.Show("Application Cancelled Successfully.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshData();
                }
                else
                {
                    MessageBox.Show("Application is not Cancelled Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalLicenseApplicationID = (int)dgvLocalLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalLicenseApplication LocalLicenseApplication = clsLocalLicenseApplication.Find(LocalLicenseApplicationID);

            if(LocalLicenseApplication == null)
            {
                MessageBox.Show("There is no Local Application with ID = " + LocalLicenseApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int LicenseID = LocalLicenseApplication.GetActiveLicenseID();

            if(LicenseID == -1)
            {
                MessageBox.Show("There is no License for this application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            frmShowLocalLicenseInfo frm = new frmShowLocalLicenseInfo(LicenseID);
            frm.ShowDialog();
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalLicenseApplicationID = (int)dgvLocalLicenseApplications.CurrentRow.Cells[0].Value;
            frmIssueLicenseFirstTime frm = new frmIssueLicenseFirstTime(LocalLicenseApplicationID);
            frm.ShowDialog();
            _RefreshData();
        }

        private void showPersonLienseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalLicenseApplicationID = (int)dgvLocalLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalLicenseApplication LocalLicenseApplication = clsLocalLicenseApplication.Find(LocalLicenseApplicationID);
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(LocalLicenseApplication.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete this application?", "Confirm", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int LocalLicenseApplicationID = (int)dgvLocalLicenseApplications.CurrentRow.Cells[0].Value;
                clsLocalLicenseApplication LocalLicenseApplication = clsLocalLicenseApplication.Find(LocalLicenseApplicationID);

                if(LocalLicenseApplication == null)
                {
                    MessageBox.Show("There is no local application with ID = [" + LocalLicenseApplicationID + "]", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if(!(LocalLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New))
                {
                    MessageBox.Show("You can not delete this application", "Not Deleted", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if(LocalLicenseApplication.Delete())
                {
                    MessageBox.Show("Application deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshData();
                }
                else
                {
                    MessageBox.Show("You can not delete this application, because it has data linked to it", "Not Deleted", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
    }
}
