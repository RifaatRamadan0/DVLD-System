using DVLD___BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDWinForms___Presentation_Layer.Tests
{
    public partial class frmListTestAppointments : Form
    {
        private int _LocalLicenseApplicationID = -1;
        private clsTestType.enTestType _TestType = clsTestType.enTestType.VisionTest;
        private DataTable TestAppointments;

        public frmListTestAppointments(int LocalLicenseApplicationID, clsTestType.enTestType TestType)
        {
            InitializeComponent();

            _LocalLicenseApplicationID = LocalLicenseApplicationID;
            _TestType = TestType;
        }

        private void _LoadAppointments()
        {
            TestAppointments = clsTestAppointment.GetAllTestAppointmentsOf(_LocalLicenseApplicationID, _TestType);
            dgvTestAppointments.DataSource = TestAppointments;

            if(dgvTestAppointments.Rows.Count > 0)
            {
                dgvTestAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvTestAppointments.Columns[0].Width = 140;

                dgvTestAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvTestAppointments.Columns[1].Width = 220;

                dgvTestAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvTestAppointments.Columns[2].Width = 140;

                dgvTestAppointments.Columns[3].HeaderText = "Is Locked";
                dgvTestAppointments.Columns[3].Width = 140;
            }

            lblNumOfRecords.Text = dgvTestAppointments.Rows.Count.ToString();

        }
        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            switch (_TestType)
            {
                case clsTestType.enTestType.VisionTest:
                    this.Text = "Vision Test Appointments";
                    pbTestTypeImage.ImageLocation = @"C:\Users\rifaa\Desktop\Programming Princples Courses\19th Principle Course\DVLD System\DVLDWinForms - Presentation Layer\Icons\Vision 512.png";
                    lblTestTypeTitle.Text = "Vision Test Appointments";
                    break;
                case clsTestType.enTestType.WrittenTest:
                    this.Text = "Written Test Appointments";
                    pbTestTypeImage.ImageLocation = @"C:\Users\rifaa\Desktop\Programming Princples Courses\19th Principle Course\DVLD System\DVLDWinForms - Presentation Layer\Icons\Written Test 512.png";
                    lblTestTypeTitle.Text = "Written Test Appointments";
                    break;
                case clsTestType.enTestType.StreetTest:
                    this.Text = "Street Test Appointments";
                    pbTestTypeImage.ImageLocation = @"C:\Users\rifaa\Desktop\Programming Princples Courses\19th Principle Course\DVLD System\DVLDWinForms - Presentation Layer\Icons\driving-test 512.png";
                    lblTestTypeTitle.Text = "Street Test Appointments";
                    break;
                default:
                    MessageBox.Show("Unknown Test Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
            }

            ctrlDivingLicenseApplicationInfo1.LoadDrivingLicenseApplicationInfo(_LocalLicenseApplicationID);
            _LoadAppointments();
        }

        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            clsLocalLicenseApplication LocalLicenseApplication = clsLocalLicenseApplication.Find(_LocalLicenseApplicationID);
            
            // check if this application has an active test appointment
            if(LocalLicenseApplication.HasActiveTestAppointment(_TestType))
            {
                MessageBox.Show("Person already has an active test appointment, you can not add new appointment", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsTest LastTest = LocalLicenseApplication.GetLastTest(_TestType);

            if(LastTest != null && LastTest.TestResult == true)
            {
                MessageBox.Show("Person has already passed this test before, you can not add new appointment", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmScheduleTest frm = new frmScheduleTest(_LocalLicenseApplicationID, _TestType);
            frm.ShowDialog();
            _LoadAppointments();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvTestAppointments.CurrentRow.Cells[0].Value;
            frmScheduleTest frm = new frmScheduleTest(_LocalLicenseApplicationID, _TestType, TestAppointmentID);
            frm.ShowDialog();
            _LoadAppointments();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvTestAppointments.CurrentRow.Cells[0].Value;
            frmTakeTest frm = new frmTakeTest(TestAppointmentID, _TestType);
            frm.ShowDialog();
            _LoadAppointments();
        }
    }
}
