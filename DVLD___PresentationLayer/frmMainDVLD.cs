using DVLDWinForms___Presentation_Layer.Applications.International_License;
using DVLDWinForms___Presentation_Layer.Applications.Local_Driving_License;
using DVLDWinForms___Presentation_Layer.Applications.Release_Detained_License;
using DVLDWinForms___Presentation_Layer.Applications.Renew_Local_License;
using DVLDWinForms___Presentation_Layer.Applications.ReplaceLostOrDamagedLicense;
using DVLDWinForms___Presentation_Layer.ApplicationTypes;
using DVLDWinForms___Presentation_Layer.Drivers;
using DVLDWinForms___Presentation_Layer.Global_Classes;
using DVLDWinForms___Presentation_Layer.Licenses;
using DVLDWinForms___Presentation_Layer.Licenses.Detain_License;
using DVLDWinForms___Presentation_Layer.Licenses.International_License;
using DVLDWinForms___Presentation_Layer.Test_Types;
using DVLDWinForms___Presentation_Layer.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDWinForms___Presentation_Layer
{
    public partial class frmMainDVLD : Form
    {
        private frmLoginScreen _LoginScreen;

        public frmMainDVLD()
        {
            InitializeComponent();
        }

        public frmMainDVLD(frmLoginScreen LoginScreen)
        {
            InitializeComponent();
            _LoginScreen = LoginScreen;
        }

        private void frmMainDVLD_Load(object sender, EventArgs e)
        {
            lblUserLoggedIn.Text = clsGlobal.CurrentUser.UserName;
        }

        private void MenuPeople_Click(object sender, EventArgs e)
        {
            frmListPeople frm = new frmListPeople();
            frm.ShowDialog();
        }

        private void MenuUsers_Click(object sender, EventArgs e)
        {
            frmListUsers frm = new frmListUsers();
            frm.ShowDialog();
        }

        private void MenuSignOut_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null;
            this.Close();
            _LoginScreen.Show();
        }

        private void MenuChangePassword_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();
        }

        private void MenuUserInfo_Click(object sender, EventArgs e)
        {
            frmUserCard frm = new frmUserCard(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListApplicationTypes frm = new frmListApplicationTypes();
            frm.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListTestTypes frm = new frmListTestTypes();
            frm.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalLicenseApplication frm = new frmAddUpdateLocalLicenseApplication();
            frm.ShowDialog();
        }

        private void internationalDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListInternationalLicenseApplications frm = new frmListInternationalLicenseApplications();
            frm.ShowDialog();
        }

        private void localLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListLocalLicenseApplications frm = new frmListLocalLicenseApplications();
            frm.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddInternationalLicense frm = new frmAddInternationalLicense();
            frm.ShowDialog();

        }

        private void MenuRenewLicense_Click(object sender, EventArgs e)
        {
            frmRenewLocalLicenseApplication frm = new frmRenewLocalLicenseApplication();
            frm.ShowDialog();
        }

        private void MenuReplaceLicense_Click(object sender, EventArgs e)
        {
            frmReplaceLostOrDamagedLicenseApplication frm = new frmReplaceLostOrDamagedLicenseApplication();
            frm.ShowDialog();
        }

        private void MenuDetainLicense_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();
        }

        private void MenuRelease_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicenseApplication frm = new frmReleaseDetainedLicenseApplication();
            frm.ShowDialog();
        }

        private void MenuReleaseLicense_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicenseApplication frm = new frmReleaseDetainedLicenseApplication();
            frm.ShowDialog();
        }

        private void MenuManageDetainedLicenses_Click(object sender, EventArgs e)
        {
            frmListDetainedLicenseApplications frm = new frmListDetainedLicenseApplications();
            frm.ShowDialog();
        }

        private void MenuDrivers_Click(object sender, EventArgs e)
        {
            frmListDrivers frm = new frmListDrivers();
            frm.ShowDialog();
            
        }
    }
}
