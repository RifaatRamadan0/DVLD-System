using DVLD___BusinessLayer;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDWinForms___Presentation_Layer
{
    public partial class frmListPeople : Form
    {

        public frmListPeople()
        {
            InitializeComponent();
        }

        private DataTable _dtAllPeople;
        private DataTable _dtPeople;

        private void _RefreshData()
        {
            _dtAllPeople = clsPerson.GetAllPeople();
            _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
               "FirstName", "SecondName", "ThirdName", "LastName", "Gendor", "DateOfBirth",
                "Nationality", "Phone", "Email");
            dgvManagePeople.DataSource = _dtPeople;
            lblNumOfRecords.Text = dgvManagePeople.Rows.Count.ToString();
        }
        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            _RefreshData();

            cmbFilterBy.SelectedIndex = 0;
            if(dgvManagePeople.Rows.Count > 0)
            {
                dgvManagePeople.Columns[0].HeaderText = "Person ID";
                dgvManagePeople.Columns[0].Width = 110;

                dgvManagePeople.Columns[1].HeaderText = "National No.";
                dgvManagePeople.Columns[1].Width = 110;

                dgvManagePeople.Columns[2].HeaderText = "First Name";
                dgvManagePeople.Columns[2].Width = 110;

                dgvManagePeople.Columns[3].HeaderText = "Second Name";
                dgvManagePeople.Columns[3].Width = 110;

                dgvManagePeople.Columns[4].HeaderText = "Third Name";
                dgvManagePeople.Columns[4].Width = 110;

                dgvManagePeople.Columns[5].HeaderText = "Last Name";
                dgvManagePeople.Columns[5].Width = 110;

                dgvManagePeople.Columns[6].HeaderText = "Gendor";
                dgvManagePeople.Columns[6].Width = 110;

                dgvManagePeople.Columns[7].HeaderText = "Date Of Birth";
                dgvManagePeople.Columns[7].Width = 110;

                dgvManagePeople.Columns[8].HeaderText = "Nationality";
                dgvManagePeople.Columns[8].Width = 110;

                dgvManagePeople.Columns[9].HeaderText = "Phone";
                dgvManagePeople.Columns[9].Width = 110;

                dgvManagePeople.Columns[10].HeaderText = "Email";
                dgvManagePeople.Columns[10].Width = 110;
            }

        }

        private void _AddPerson()
        {
            frmAddUpdatePerson AddEditPerson = new frmAddUpdatePerson();
            AddEditPerson.ShowDialog();
            _RefreshData();
        }
        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            _AddPerson();
        }

        private void cmShowDetails_Click(object sender, EventArgs e)
        {
            frmShowPersonInfo personCard = new frmShowPersonInfo((int)dgvManagePeople.CurrentRow.Cells[0].Value);
            personCard.ShowDialog();
            _RefreshData();
        }

        private void _UpdatePerson()
        {
            frmAddUpdatePerson AddEditPerson = new frmAddUpdatePerson((int)dgvManagePeople.CurrentRow.Cells[0].Value);
            AddEditPerson.ShowDialog();
            _RefreshData();
        }
        private void cmEdit_Click(object sender, EventArgs e)
        {
            _UpdatePerson();
        }

        private void cmAddNewPerson_Click(object sender, EventArgs e)
        {
            _AddPerson();
        }

        private void cmbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterBy.Visible = (cmbFilterBy.Text != "None");

            if (txtFilterBy.Visible)
            {
                txtFilterBy.Text = "";
                txtFilterBy.Focus();
            }         
    
        }
        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            string SelectedColumn = "";

            switch (cmbFilterBy.Text)
            {
                case "Person ID":
                    SelectedColumn = "PersonID";
                    break;
                case "National No.":
                    SelectedColumn = "NationalNo";
                    break;
                case "First Name":
                    SelectedColumn = "FirstName";
                    break;
                case "Second Name":
                    SelectedColumn = "SecondName";
                    break;
                case "Third Name":
                    SelectedColumn = "ThirdName";
                    break;
                case "Last Name":
                    SelectedColumn = "LastName";
                    break;
                case "Gendor":
                    SelectedColumn = "Gendor";
                    break;
                case "Nationality":
                    SelectedColumn = "Nationality";
                    break;
                case "Phone":
                    SelectedColumn = "Phone";
                    break;
                case "Email":
                    SelectedColumn = "Email";
                    break;

                default:
                    SelectedColumn = "None";
                    break;
            }

            _RefreshData(); // This is to keep data updated if other devices made changes, but this is dangerous on the performance

            if (txtFilterBy.Text.Trim() == "")
            {
                _dtPeople.DefaultView.RowFilter = "";
                lblNumOfRecords.Text = dgvManagePeople.Rows.Count.ToString();
                return;
            }

            if (SelectedColumn == "PersonID")
                _dtPeople.DefaultView.RowFilter = $"PersonID = {txtFilterBy.Text.Trim()}";
            else
            {
                _dtPeople.DefaultView.RowFilter = $"{SelectedColumn} LIKE '{txtFilterBy.Text.Trim()}%'";
            }
            

            lblNumOfRecords.Text = dgvManagePeople.Rows.Count.ToString();         
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmDelete_Click(object sender, EventArgs e)
        {
            int SelectedPersonID = (int)dgvManagePeople.CurrentRow.Cells["PersonID"].Value;
            if (MessageBox.Show($"Are You Sure You Want To Delete Person {SelectedPersonID}? ", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                == DialogResult.OK)
            {   
                if (clsPerson.DeletePerson(SelectedPersonID))
                {
                    MessageBox.Show("Person Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshData();
                }
                else
                {
                    MessageBox.Show("Person Has Not Deleted Successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilterBy.Text == "Person ID")
            {
                if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }

            }
        }

        private void cmSendEmail_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature is not Implemented Yet", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void cmPhoneCall_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature is not Implemented Yet", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void dgvManagePeople_DoubleClick(object sender, EventArgs e)
        {
            frmShowPersonInfo PersonCard = new frmShowPersonInfo((int)dgvManagePeople.CurrentRow.Cells["PersonID"].Value);
            PersonCard.ShowDialog();
            _RefreshData();
        }
    }
}
