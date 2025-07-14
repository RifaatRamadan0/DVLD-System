using DVLD___BusinessLayer;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDWinForms___Presentation_Layer.Users
{
    public partial class frmAddUpdateUser : Form
    {
        public enum enMode { AddNew = 0, Update = 1};
        private enMode _Mode;
        private int _UserID = -1;
        private clsUser _User;

        public frmAddUpdateUser()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();

            _UserID = UserID;
            _Mode = enMode.Update;
        }


        private void _ResetDefaultValues()
        {
            if(_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New User";
                this.Text = "Add New User";
                _User = new clsUser();
                tbLoginInfo.Enabled = false;
            }
            else
            {
                lblTitle.Text = "Update User";
                this.Text = "Update User";
                tbPages.Enabled = true;
                btnSave.Enabled = true;
            }

            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            chkIsActive.Checked = true;

        }
        private void _FillUserInfo()
        {
            lblUserID.Text = _User.UserID.ToString();
            txtUserName.Text = _User.UserName;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = _User.Password;
            chkIsActive.Checked = _User.IsActive;
        }
        private void _LoadData()
        {
            _User = clsUser.FindByUserID(_UserID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            if (_User == null)
            {
                MessageBox.Show($"User With Id = {_UserID} Does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                this.Close();
                return;
            }

            ctrlPersonCardWithFilter1.LoadPerson(_User.PersonID);
            _FillUserInfo();
        }
        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if(_Mode == enMode.Update)
            {
                _LoadData();
            }

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(_Mode == enMode.Update)
            {
                txtUserName.Focus();
                tbLoginInfo.Enabled = true;
                tbPages.SelectedTab = tbLoginInfo;
                return;
            }

            if(ctrlPersonCardWithFilter1.SelectedPerson == null)
            {
                MessageBox.Show("Please Select a Person", "Select Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
                return;
            }

            if(clsUser.IsUserExistForPersonID(ctrlPersonCardWithFilter1.SelectedPerson.PersonID))
            {
                MessageBox.Show("This Person already has a user. Please select another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
                return;
            }

            txtUserName.Focus();
            btnSave.Enabled = true;
            tbLoginInfo.Enabled = true;
            tbPages.SelectedTab = tbLoginInfo;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            bool IsDuplicate = clsUser.IsUserExist(txtUserName.Text.Trim());

            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "Username can not be blank");
                return;
            }

            if(_Mode == enMode.AddNew)
            {
                if(IsDuplicate)
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "This Username is used by another user");
                    return;
                }
            }
            else
            {
                if(txtUserName.Text.Trim() != _User.UserName && IsDuplicate)
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "This Username is used by another user");
                    return;
                }
            }

            errorProvider1.SetError(txtUserName, null);
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Password can not be blank");
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text.Trim() != txtPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match Password");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }

        }

        private void _SaveUser()
        {
            _User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _User.Person = ctrlPersonCardWithFilter1.SelectedPerson;
            _User.UserName = txtUserName.Text.Trim();
            _User.Password = txtPassword.Text.Trim();
            _User.IsActive = chkIsActive.Checked;
            
            if(_User.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _Mode = enMode.Update;
                _UserID = _User.UserID;
                lblUserID.Text = _UserID.ToString();
                lblTitle.Text = "Update User";
                this.Text = "Update User";
            }
            else
            {
                MessageBox.Show("Data is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            // We Ensure that the user enters username, password
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields are not valid, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if Person is selected
            if(ctrlPersonCardWithFilter1.SelectedPerson == null)
            {
                MessageBox.Show("Please Select a Person", "Person Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _SaveUser();
        }

    }
}
