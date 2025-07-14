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

namespace DVLDWinForms___Presentation_Layer.Users
{
    public partial class frmChangePassword : Form
    {

        private int _UserID;
        private clsUser _User;

        public frmChangePassword(int UserID)
        {
            InitializeComponent();

            _UserID = UserID;
        }


        private void _ResetDefaultValues()
        {
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtCurrentPassword.Focus();
        }
        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            _User = clsUser.FindByUserID(_UserID);

            if (_User == null)
            {
                MessageBox.Show("User does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrlUserCard1.LoadUserInfo(_UserID);
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtCurrentPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Password can not be blank");
                return;
            }


            if(_User.Password.Trim() != txtCurrentPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current Password is wrong");
                return;
            }

            errorProvider1.SetError(txtCurrentPassword, null);

        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "Password can not be blank");
            }
            else
            {
                errorProvider1.SetError(txtNewPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if(txtConfirmPassword.Text.Trim() != txtNewPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match new password");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
        }

        private void _ChangePassword()
        {
            _User.Password = txtNewPassword.Text.Trim();

            if(_User.Save())
            {
                MessageBox.Show("Password Changed Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
               _ResetDefaultValues();
            }
            else
            {
                MessageBox.Show("An Error occured, Password did not change.");
            }

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields are not valid, put the mouse under the red icon the see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _ChangePassword();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
