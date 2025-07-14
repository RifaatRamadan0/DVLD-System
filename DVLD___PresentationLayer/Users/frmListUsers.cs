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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DVLDWinForms___Presentation_Layer.Users
{
    public partial class frmListUsers : Form
    {
        public frmListUsers()
        {
            InitializeComponent();
        }

        private DataTable _dtAllUsers;

        private void _RefreshData()
        {
            _dtAllUsers = clsUser.GetAllUsers();
            dgvManageUsers.DataSource = _dtAllUsers;
            lblNumOfRecords.Text = dgvManageUsers.Rows.Count.ToString();

        }
        private void frmListUsers_Load(object sender, EventArgs e)
        {
            _RefreshData();

            cmbFilterBy.SelectedIndex = 0;
            if(dgvManageUsers.Rows.Count > 0)
            {
                dgvManageUsers.Columns[0].HeaderText = "User ID";
                dgvManageUsers.Columns[0].Width = 110;

                dgvManageUsers.Columns[1].HeaderText = "Person ID";
                dgvManageUsers.Columns[1].Width = 120;

                dgvManageUsers.Columns[2].HeaderText = "Full Name";
                dgvManageUsers.Columns[2].Width = 300;

                dgvManageUsers.Columns[3].HeaderText = "Username";
                dgvManageUsers.Columns[3].Width = 120;

                dgvManageUsers.Columns[4].HeaderText = "Is Active";
                dgvManageUsers.Columns[4].Width = 120;
            }

        }

        private void cmbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterBy.Visible = (cmbFilterBy.Text != "None" && cmbFilterBy.Text != "Is Active");
            cmbIsActive.Visible = (cmbFilterBy.Text == "Is Active");

            if(txtFilterBy.Visible)
            {
                txtFilterBy.Clear();
                txtFilterBy.Focus();
            }

            if(cmbIsActive.Visible)
            {
                cmbIsActive.SelectedIndex = 0;
            }
        }
        
        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {

            string SelectedColumn = "";
            switch(cmbFilterBy.Text)
            {
                case "Person ID":
                    SelectedColumn = "PersonID";
                    break;
                case "User ID":
                    SelectedColumn = "UserID";
                    break;
                case "Full Name":
                    SelectedColumn = "FullName";
                    break;
                case "UserName":
                    SelectedColumn = "UserName";
                    break;
                case "Is Active":
                    SelectedColumn = "IsActive";
                    break;
            }

            _RefreshData(); // This is to keep data updated if other devices made changes, but this is dangerous on the performance

            if (txtFilterBy.Text.Trim() == "")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                return;
            }

            if (SelectedColumn == "PersonID" || SelectedColumn == "UserID")
            {
                _dtAllUsers.DefaultView.RowFilter = $"{SelectedColumn} = {txtFilterBy.Text.Trim()}";
            }
            else
            {
                _dtAllUsers.DefaultView.RowFilter = $"{SelectedColumn} LIKE '{txtFilterBy.Text.Trim()}%'";
            }

            lblNumOfRecords.Text = (dgvManageUsers.Rows.Count.ToString());
        }

        private void cmbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {

            if(cmbIsActive.Text == "All")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
            }
            else
            {
                bool IsActive = (cmbIsActive.Text == "Yes");
                _dtAllUsers.DefaultView.RowFilter = $"IsActive = {IsActive}";
            }

            lblNumOfRecords.Text = dgvManageUsers.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cmbFilterBy.Text == "Person ID" || cmbFilterBy.Text == "User ID")
            {
                // Enforce the user to enter only numbers
                e.Handled = (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();
            _RefreshData();
        }

        private void cmShowDetails_Click(object sender, EventArgs e)
        {
            frmUserCard frm = new frmUserCard((int)dgvManageUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void cmAddNewUser_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();
            _RefreshData();
        }

        private void cmEdit_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser((int)dgvManageUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshData();
        }

        private void dgvManageUsers_DoubleClick(object sender, EventArgs e)
        {
            frmUserCard frm = new frmUserCard((int)dgvManageUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void cmDelete_Click(object sender, EventArgs e)
        {
            int SelectedUserID = (int)dgvManageUsers.CurrentRow.Cells[0].Value;
            if (MessageBox.Show("Are You Sure You Want to delete User with ID = [" + SelectedUserID + "]", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if(clsUser.DeleteUser(SelectedUserID))
                {
                    MessageBox.Show("User Deleted Successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshData();
                }
                else
                {
                    MessageBox.Show("User is not deleted because other data linked to it", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmChangePassword_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword((int)dgvManageUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
    }
}
