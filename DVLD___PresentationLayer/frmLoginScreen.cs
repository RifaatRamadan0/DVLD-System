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
using DVLDWinForms___Presentation_Layer.Global_Classes;

namespace DVLDWinForms___Presentation_Layer
{
    public partial class frmLoginScreen : Form
    {

        public frmLoginScreen()
        {
            InitializeComponent();
        }
     
        private void frmLoginScreen_Load(object sender, EventArgs e)
        {
            string Username = "", Password = "";

            if (clsGlobal.GetStoredCredentials(ref Username, ref Password))
            {
                txtUsername.Text = Username;
                txtPassword.Text = Password;
                chkRememberMe.Checked = true;
            }
            else
            {
                chkRememberMe.Checked = false;
            }
        }
      
        private void _Login(clsUser User)
        {
            clsGlobal.CurrentUser = User;
            this.Hide();
            frmMainDVLD frm = new frmMainDVLD(this);
            frm.Show();
        }
        private void _HandleRememberMe()
        {
            if(chkRememberMe.Checked)
            {
                clsGlobal.SaveCredentials(txtUsername.Text, txtPassword.Text);
            }
            else
            {
                clsGlobal.ClearCredentials();
            }

        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            clsUser User = clsUser.FindUsernameAndPassword(txtUsername.Text.Trim(), txtPassword.Text.Trim());

            if (User == null)
            {
                MessageBox.Show("Invalid Username/Password.", "Wrong Credentials.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _HandleRememberMe();

            if (!User.IsActive)
            {
                MessageBox.Show("User Is Not Active, Contact Your Admin", "Active User", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                return;
            }

            // If Login Successfully:
            _Login(User);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
